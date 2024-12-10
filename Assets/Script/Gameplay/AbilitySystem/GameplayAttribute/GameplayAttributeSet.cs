using System;
using System.Collections.Generic;
using UnityEngine;
using Yd.Collection;
using Yd.DebugExtension;

namespace Yd.Gameplay.AbilitySystem
{
    public class GameplayAttributeSet : MonoBehaviour
    {
        [SerializeField] private GameplayAttributeData attributeData;

        private readonly SDictionary<GameplayAttributeTypeSO, GameplayAttribute> attributes = new();

        public GameplayAttribute this[GameplayAttributeTypeSO gameplayAttributeTypeSo] => attributes[gameplayAttributeTypeSo];

        public IReadOnlyDictionary<GameplayAttributeTypeSO, float> BaseValues
        {
            get
            {
                var baseValues = new Dictionary<GameplayAttributeTypeSO, float>();

                foreach (var (type, attribute) in attributes)
                {
                    baseValues.Add(type, attribute.BaseValue);
                }

                return baseValues;
            }
        }

        public IReadOnlyDictionary<GameplayAttributeTypeSO, float> CurrentValues
        {
            get
            {
                var currentValues = new Dictionary<GameplayAttributeTypeSO, float>();

                foreach (var (type, attribute) in attributes)
                {
                    currentValues.Add(type, attribute.CurrentValue);
                }

                return currentValues;
            }
        }

        private void Awake()
        {
            attributes.Clear();

            if (attributeData == null)
            {
                return;
            }

            foreach (var (type, value) in attributeData.Data)
            {
                var attribute = new GameplayAttribute(type, value);
                attribute.PreAttributeCurrentValueChange += OnPreAttributeCurrentValueChanged;
                attribute.PostAttributeCurrentValueChange += OnPostAttributeCurrentValueChanged;

                attributes.Add(type, attribute);
            }
        }

        private void Start()
        {
            foreach (var (type, attribute) in attributes)
            {
                OnPostAttributeCurrentValueChanged(type, attribute.CurrentValue, attribute.CurrentValue);
            }
        }

        public event Action<GameplayAttributeTypeSO, float, float> AttributeCurrentValueChanged;

        // private void Update()
        // {
        //     LogCurrentValues(this);
        // }

        public void ResetCurrentValues()
        {
            foreach (var (_, attribute) in attributes)
            {
                attribute.CurrentValue = attribute.BaseValue;
            }
        }

        public static void LogCurrentValues(GameplayAttributeSet set)
        {
            foreach (var (type, value) in set.CurrentValues)
            {
                DebugE.LogValue(type.name, value);
            }
        }

        private float OnPreAttributeCurrentValueChanged(GameplayAttributeTypeSO typeSo, float oldValue, float value)
        {
            if (typeSo.MaxAttribute && value > attributes[typeSo.MaxAttribute].CurrentValue)
            {
                value = attributes[typeSo.MaxAttribute].CurrentValue;
            }
            if (typeSo.ValueAttribute && value > oldValue)
            {
                attributes[typeSo.ValueAttribute].BaseValue += value - oldValue;
                attributes[typeSo.ValueAttribute].CurrentValue += value - oldValue;
            }

            return value;
        }

        private void OnPostAttributeCurrentValueChanged(GameplayAttributeTypeSO typeSo, float oldValue, float value)
        {
            AttributeCurrentValueChanged?.Invoke(typeSo, oldValue, value);
        }
    }
}