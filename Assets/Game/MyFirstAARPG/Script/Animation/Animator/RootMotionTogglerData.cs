using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyFirstAARPG
{
    [CreateAssetMenu(fileName = "Root Motion Toggler Data", menuName = "Scriptable Objects/Root Motion Toggler Data")]
    public class RootMotionTogglerData : ScriptableObject
    {
        [SerializeField]
        private List<KeyValuePair_StateMachineBehaviourEvents_bool> toggleRootMotionList;

        private void OnValidate()
        {
            ToggleRootMotion.Clear();
            
            if (toggleRootMotionList == null)
            {
                return;
            }

            foreach (var KV in toggleRootMotionList.Where(KV => !ToggleRootMotion.TryAdd(KV.Key, KV.Value)))
            {
                Debug.LogWarning($"the key {KV.Key} already exists.");
            }
        }
        
        private Dictionary<StateMachineBehaviourEvent, bool> toggleRootMotion;
        public Dictionary<StateMachineBehaviourEvent, bool> ToggleRootMotion => toggleRootMotion ??= new();
    }
    
    [Serializable]
    public struct KeyValuePair_StateMachineBehaviourEvents_bool
    {
        public StateMachineBehaviourEvent Key;
        public bool Value;
    }
}