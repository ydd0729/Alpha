using System;
using System.Collections.Generic;
using Shared.Collections;
using UnityEngine;

namespace Shared.Gameplay.AbilitySystem
{
    public class GameplayAttributeSet : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<GameplayAttributeType, float> attributeInitialValues;

        private void Awake()
        {
            attributes.Clear();
            
            foreach (var kv in attributeInitialValues)
            {
                attributes.Add(kv.Key, new GameplayAttributeData(kv.Value));
            }
        }
        
        public IReadOnlyDictionary<GameplayAttributeType, GameplayAttributeData> Attributes => attributes;
        
        /**
         *	Called just before any modification happens to an attribute. This is lower level than PreAttributeModify/PostAttribute modify.
         *	There is no additional context provided here since anything can trigger this. Executed effects, duration based effects, effects being removed, immunity being applied, stacking rules changing, etc.
         *	This function is meant to enforce things like "Health = Clamp(Health, 0, MaxHealth)" and NOT things like "trigger this extra thing if damage is applied, etc".
         *
         *	NewValue is a mutable reference so you are able to clamp the newly applied value as well.
         */
        public virtual void PreAttributeChange(GameplayAttribute Attribute, ref float NewValue)
        {
        }

        /** Called just after any modification happens to an attribute. */
        public virtual void PostAttributeChange(GameplayAttribute Attribute, float OldValue, float NewValue)
        {
        }

        /**
         *	This is called just before any modification happens to an attribute's base value when an attribute aggregator exists.
         *	This function should enforce clamping (presuming you wish to clamp the base value along with the final value in PreAttributeChange)
         *	This function should NOT invoke gameplay related events or callbacks. Do those in PreAttributeChange() which will be called prior to the
         *	final value of the attribute actually changing.
         */
        public virtual void PreAttributeBaseChange(GameplayAttribute Attribute, float NewValue)
        {
        }

        /** Called just after any modification happens to an attribute's base value when an attribute aggregator exists. */
        public virtual void PostAttributeBaseChange(GameplayAttribute Attribute, float OldValue, float NewValue)
        {
        }
        
        private readonly SerializableDictionary<GameplayAttributeType, GameplayAttributeData> attributes = new();
    }
}