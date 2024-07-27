using System;
using System.Collections.Generic;
using Shared.Collections;
using UnityEngine;

namespace Shared.Gameplay.AbilitySystem
{
    [Serializable]
    public class GameplayAttributeData
    {
        [SerializeField, HideInInspector] private float baseValue;
        [SerializeField, HideInInspector] private float currentValue;

        public GameplayAttributeData(float baseValue)
        {
            this.baseValue = baseValue;
        }

        public virtual float BaseValue
        {
            get => baseValue;
            set => baseValue = value;
        }

        public virtual float CurrentValue
        {
            get => currentValue;
            set => currentValue = value;
        }

        public virtual void PreAttributeCurrentValueChange(ref float value)
        {
        }

        public virtual void PostAttributeCurrentValueChange(float oldValue, float newValue)
        {
        }

        public virtual void PreAttributeBaseValueChange(ref float value)
        {
        }

        public virtual void PostAttributeBaseValueChange(float oldValue, float newValue)
        {
        }
    }
}