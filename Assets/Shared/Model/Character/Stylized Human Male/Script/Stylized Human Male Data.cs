using System;
using System.Collections.Generic;
using Shared.Collections;
using UnityEngine;

namespace Shared.StylizedHumanMale
{
    [CreateAssetMenu(fileName = "Stylized Human Male Data", menuName = "Scriptable Objects/Stylized Human Male Data")]
    public class StylizedHumanMaleData : ScriptableObject
    {
        [SerializeField]
        public SerializableDictionary<StylizedHumanMalePartType, StylizedHumanMalePart> StylizedHumanMaleParts;
    }

    [Serializable]
    public struct StylizedHumanMalePart
    {
        [SerializeField] public Mesh[] Meshes;
        [SerializeField] public SerializableDictionary<int, List<Material>> Materials;
    }

    [Serializable]
    public enum StylizedHumanMalePartType
    {
        Hair,
        Brows,
        Moustache,
        Beard,
        Body,
        Shirt,
        Belt,
        Pants,
        Boots
    }
}
