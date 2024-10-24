using System;
using UnityEngine;

namespace Yd.Gameplay.AbilitySystem
{
    [Serializable]
    public class GameplayAttribute
    {
        [SerializeField] [HideInInspector] private float baseValue;
        [SerializeField] [HideInInspector] private float currentValue;

        public GameplayAttribute(float value)
        {
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

                PreAttributeBaseValueChange(ref value);

                var oldValue = baseValue;
                baseValue = value;

                PostAttributeBaseValueChange(oldValue);
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

                PreAttributeCurrentValueChange(ref value);

                var oldValue = currentValue;
                currentValue = value;

                PostAttributeCurrentValueChange(oldValue);
            }
        }

        public virtual void PreAttributeBaseValueChange(ref float value)
        {
        }

        public virtual void PostAttributeBaseValueChange(float oldValue)
        {
        }

        public virtual void PreAttributeCurrentValueChange(ref float value)
        {
        }

        public virtual void PostAttributeCurrentValueChange(float oldValue)
        {
        }
    }
}