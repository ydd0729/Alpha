using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yd
{
    [CreateAssetMenu(fileName = "Root Motion Toggler Data", menuName = "Scriptable Objects/Root Motion Toggler Data")]
    public class RootMotionTogglerData : ScriptableObject
    {
        [SerializeField] private List<KeyValuePairStateMachineBehaviourEventsBool> toggleRootMotionList;

        private Dictionary<StateMachineBehaviourEvent, bool> toggleRootMotion;
        public Dictionary<StateMachineBehaviourEvent, bool> ToggleRootMotion => toggleRootMotion ??= new Dictionary<StateMachineBehaviourEvent, bool>();

        private void OnValidate()
        {
            ToggleRootMotion.Clear();

            if (toggleRootMotionList == null) return;

            foreach (var kv in toggleRootMotionList.Where(kv => !ToggleRootMotion.TryAdd(kv.key, kv.value))) Debug.LogWarning($"the key {kv.key} already exists.");
        }
    }

    [Serializable]
    public struct KeyValuePairStateMachineBehaviourEventsBool
    {
        [FormerlySerializedAs("Key")] public StateMachineBehaviourEvent key;
        [FormerlySerializedAs("Value")] public bool value;
    }
}