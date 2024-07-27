using System;
using Shared.Collections;
using Shared.StylizedHumanMale;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shared
{
    [CreateAssetMenu(fileName = "Stylized Human Male Config Data", menuName = "Scriptable Objects/Stylized Human Male Config Data")]
    public class StylizedHumanMaleConfigData : ScriptableObject
    {
        [SerializeField]
        public SerializableDictionary<StylizedHumanMalePartType, StylizedHumanMalePartConfig> stylizedHumanMaleConfig;
    }
    
    [Serializable]
    public struct StylizedHumanMalePartConfig
    {
        [SerializeField] public int MeshIndex;
        [SerializeField] public SerializableDictionary<int, int> MaterialIndex;
    }
}
