using System.Collections.Generic;
using UnityEngine;
using Yd.Collection;
using Yd.Extension;

namespace Yd.Gameplay.AbilitySystem
{
    public class GameplayAttributeSet : MonoBehaviour
    {
        [SerializeField] private GameplayAttributeData attributeData;

        private readonly SDictionary<GameplayAttributeType, GameplayAttribute> attributes = new();

        public GameplayAttribute this[GameplayAttributeType gameplayAttributeType] => attributes[gameplayAttributeType];

        public IReadOnlyDictionary<GameplayAttributeType, float> BaseValues
        {
            get
            {
                var baseValues = new Dictionary<GameplayAttributeType, float>();

                foreach (var (type, attribute) in attributes)
                {
                    baseValues.Add(type, attribute.BaseValue);
                }

                return baseValues;
            }
        }

        public IReadOnlyDictionary<GameplayAttributeType, float> CurrentValues
        {
            get
            {
                var currentValues = new Dictionary<GameplayAttributeType, float>();

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
                attributes.Add(type, new GameplayAttribute(value));
            }
        }

        private void Update()
        {
            LogCurrentValues(this);
        }

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
    }
}