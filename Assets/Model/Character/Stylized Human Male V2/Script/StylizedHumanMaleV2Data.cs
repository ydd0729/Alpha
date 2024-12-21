using System;
using UnityEngine;
using UnityEngine.Serialization;
using Yd.Collection;

namespace Yd.StylizedHumanMaleV2
{
    [CreateAssetMenu(fileName = "Stylized Human Male V2 Data", menuName = "Scriptable Objects/Stylized Human Male V2 Data")]
    public class StylizedHumanMaleV2Data : ScriptableObject
    {
        [KeyValueName(keyName: "Skin Material Type", valueName: "Skin Material Lists")]
        public SDictionary<SkinMaterial, MaterialList[]> skinMaterials;

        public SDictionary<EyeMaterial, MaterialList> eyeMaterials;

        public SDictionary<HairMesh, SKeyValuePair<Mesh[], SDictionary<HairMaterial, MaterialList[]>>> hairMeshesMaterials;

        public SDictionary<TorsoMesh, SKeyValuePair<Mesh[], SDictionary<TorsoMaterial, MaterialList[]>>> torsoMeshesMaterials;

        [FormerlySerializedAs("thighsMaterials")] [FormerlySerializedAs("underwearMaterials")]
        public SDictionary<PantsMaterial, MaterialList> pantsMaterials;

        public SDictionary<BootsMesh, SKeyValuePair<Mesh[], SDictionary<BootsMaterial, MaterialList[]>>> bootsMeshesMaterials;

        public SDictionary<BrowsMesh, Mesh> browsMesh;

        [FormerlySerializedAs("BrowsMaterials")] [FormerlySerializedAs("BrowsMaterialList")]
        public SDictionary<BrowsMaterial, Material> browsMaterials;

        public SDictionary<HelmetMesh, Mesh[]> helmetMesh;

        [FormerlySerializedAs("BrowsMaterialList")]
        public SDictionary<HelmetMaterial, MaterialList[]> helmetMaterials;

        public SDictionary<BeardMesh, Mesh> beardMesh;

        public SDictionary<BeardMaterial, Material> beardMaterials;

        public SDictionary<BeltMaterial, Material> beltMaterials;

        public SDictionary<CapeMesh, Mesh> capeMesh;

        public SDictionary<CapeMaterial, Material[]> capeMaterials;

        public SDictionary<GloveMaterial, Material[]> gloveMaterials;
    }

    [Serializable]
    public enum StylizedHumanMaleV2PartType
    {
        Skin,
        Eye,
        Hair,
        Torso,
        Pants,
        Boots,
        Brows,
        Helmet,
        Beard,
        Belt,
        Cape,
        Gloves
    }

    [Serializable]
    public struct MaterialList
    {
        [FormerlySerializedAs("materials")] public Material[] list;
    }

    [Serializable]
    public enum EyeMaterial
    {
        Blue,
        Brown,
        Green,
        Purple
    }

    [Serializable]
    public enum HairMesh
    {
        None,
        Hair01,
        Hair02,
        Hair03,
        Hair04,
        Hair09
    }

    [Serializable]
    public enum HairMaterial
    {
        Gold,
        Black,
        Brown,
        Grey
    }

    [Serializable]
    public enum SkinMaterial
    {
        Skin01,
        Skin02,
        Skin03,
        Skin04,
        Skin05
    }

    [Serializable]
    public enum TorsoMesh
    {
        Naked,
        Peasant01,
        Peasant02,
        Peasant03
    }

    [Serializable]
    public enum TorsoMaterial
    {
        Blue,
        Brown,
        Red
    }

    [Serializable]
    public enum PantsMaterial
    {
        Underwear_Yellow,
        Underwear_Blue,
        Underwear_Red,
        Underwear_Brown,
        Underwear_Green,
        Pants_Peasant_Red,
        Pants_Peasant_Brown,
        Pants_Peasant_Green
    }

    [Serializable]
    public enum BootsMesh
    {
        Naked,
        Peasant
    }

    [Serializable]
    public enum BootsMaterial
    {
        Blue,
        Brown,
        Red
    }

    [Serializable]
    public enum BrowsMesh
    {
        Brows01,
        Brows02,
        Brows03,
        Brows04
    }

    [Serializable]
    public enum BrowsMaterial
    {
        Gold,
        Black,
        Brown,
        Grey
    }

    [Serializable]
    public enum HelmetMesh
    {
        None,
        Peasant
    }

    [Serializable]
    public enum HelmetMaterial
    {
        Blue,
        Brown,
        Red
    }

    [Serializable]
    public enum BeardMesh
    {
        None,
        Beard_01,
        Beard_02,
        Beard_03,
        Beard_05,
        Beard_06,
        Beard_07
    }

    [Serializable]
    public enum BeardMaterial
    {
        Gold,
        Black,
        Brown,
        Grey
    }

    [Serializable]
    public enum BeltMaterial
    {
        Blue,
        Brown,
        Red
    }

    [Serializable]
    public enum CapeMesh
    {
        None,
        Peasant
    }

    [Serializable]
    public enum CapeMaterial
    {
        Blue,
        Brown,
        Red
    }

    [Serializable]
    public enum GloveMaterial
    {
        None,
        Blue,
        Brown,
        Red
    }
}