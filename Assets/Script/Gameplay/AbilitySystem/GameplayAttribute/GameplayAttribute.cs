using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yd.Gameplay.AbilitySystem
{
    [Serializable]
    public class GameplayAttribute
    {
        [SerializeField] [HideInInspector] private float baseValue;
        [SerializeField] [HideInInspector] private float currentValue;
        [FormerlySerializedAs("type")] [SerializeField] [HideInInspector] private GameplayAttributeTypeSO typeSo;

        public GameplayAttribute(GameplayAttributeTypeSO typeSo, float value)
        {
            this.typeSo = typeSo;
            baseValue = value;
            currentValue = value;
        }

        public virtual float BaseValue
        {
            get => baseValue;
            set
            {
                if (Math.Abs(value - baseValue) < float.Epsilon)
                {
                    return;
                }

                OnPreAttributeBaseValueChange(baseValue, ref value);

                var oldValue = baseValue;
                baseValue = value;

                OnPostAttributeBaseValueChange(oldValue, value);
            }
        }

        public virtual float CurrentValue
        {
            get => currentValue;
            set
            {
                if (Math.Abs(value - currentValue) < float.Epsilon)
                {
                    return;
                }

                OnPreAttributeCurrentValueChange(currentValue, ref value);

                var oldValue = currentValue;
                currentValue = value;

                OnPostAttributeCurrentValueChange(oldValue, value);
            }
        }

        public event Func<GameplayAttributeTypeSO, float, float, float> PreAttributeBaseValueChange;
        public event Func<GameplayAttributeTypeSO, float, float, float> PreAttributeCurrentValueChange;
        public event Action<GameplayAttributeTypeSO, float, float> PostAttributeBaseValueChange;
        public event Action<GameplayAttributeTypeSO, float, float> PostAttributeCurrentValueChange;

        public virtual void OnPreAttributeBaseValueChange(float oldValue, ref float value)
        {
            if (PreAttributeBaseValueChange != null)
            {
                value = PreAttributeBaseValueChange(typeSo, oldValue, value);
            }
        }

        public virtual void OnPostAttributeBaseValueChange(float oldValue, float value)
        {
            PostAttributeBaseValueChange?.Invoke(typeSo, oldValue, value);
        }

        public virtual void OnPreAttributeCurrentValueChange(float oldValue, ref float value)
        {
            if (PreAttributeCurrentValueChange != null)
            {
                value = PreAttributeCurrentValueChange(typeSo, oldValue, value);
            }
        }

        public virtual void OnPostAttributeCurrentValueChange(float oldValue, float value)
        {
            PostAttributeCurrentValueChange?.Invoke(typeSo, oldValue, value);
        }
    }
}