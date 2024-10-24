using System;
using UnityEngine;
using Yd.Collection;

namespace Yd.StylizedHumanMale
{
    [CreateAssetMenu(fileName = "Stylized Human Male Data", menuName = "Scriptable Objects/Stylized Human Male Data")]
    public class StylizedHumanMaleData : ScriptableObject
    {
        public SDictionary<HairType, Mesh> hairMeshes;
        public SDictionary<HairMaterial, Material> hairMaterials;

        public SDictionary<BrowsType, Mesh> browsMeshes;
        public SDictionary<BrowsMaterial, Material> browsMaterials;

        public SDictionary<MoustacheType, Mesh> moustacheMeshes;
        public SDictionary<MoustacheMaterial, Material> moustacheMaterials;

        public SDictionary<BeardType, Mesh> beardMeshes;
        public SDictionary<BeardMaterial, Material> beardMaterials;

        public SDictionary<BodyType, Mesh> bodyMeshes;
        public SDictionary<EyeMaterial, Material> eyeMaterials;
        public SDictionary<MouthMaterial, Material> mouthMaterials;
        public SDictionary<SkinMaterial, Material> skinMaterials;

        public SDictionary<ShirtType, Mesh> shirtMeshes;
        public SDictionary<ShirtMaterial, Material> shirtMaterials;

        public SDictionary<BeltType, Mesh> beltMeshes;
        public SDictionary<BeltMaterial, Material> beltMaterials;

        public SDictionary<PantsType, Mesh> pantsMeshes;
        public SDictionary<PantsMaterial, Material> pantsMaterials;

        public SDictionary<BootsType, Mesh> bootsMeshes;
        public SDictionary<BootsMaterial, Material> bootsMaterials;
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

    [Serializable]
    public enum HairType
    {
        None,
        Hair01,
        Hair02,
        Hair03,
        Hair04
    }

    [Serializable]
    public enum HairMaterial
    {
        Black,
        Brown
    }

    [Serializable]
    public enum BrowsType
    {
        None,
        Brows01,
        Brows02,
        Brows03
    }

    [Serializable]
    public enum BrowsMaterial
    {
        Black,
        Brown,
        Grey
    }

    [Serializable]
    public enum MoustacheType
    {
        None,
        Moustache01
    }

    [Serializable]
    public enum MoustacheMaterial
    {
        Black,
        Brown,
        Grey
    }

    [Serializable]
    public enum BeardType
    {
        None,
        Beard01,
        Beard02,
        Beard03,
        Beard04
    }

    [Serializable]
    public enum BeardMaterial
    {
        Black,
        Brown,
        Grey
    }

    [Serializable]
    public enum BodyType
    {
        Body01
    }

    [Serializable]
    public enum EyeMaterial
    {
        Brown,
        Green,
        Purple
    }

    [Serializable]
    public enum MouthMaterial
    {
        Default
    }

    [Serializable]
    public enum SkinMaterial
    {
        Skin01,
        Skin02,
        Skin03
    }

    [Serializable]
    public enum ShirtType
    {
        None,
        Default
    }

    [Serializable]
    public enum ShirtMaterial
    {
        Blue,
        Brown,
        Red
    }

    [Serializable]
    public enum BeltType
    {
        None,
        Default
    }

    [Serializable]
    public enum BeltMaterial
    {
        Blue,
        Brown,
        Red
    }

    [Serializable]
    public enum PantsType
    {
        None,
        Default
    }

    [Serializable]
    public enum PantsMaterial
    {
        Blue,
        Brown,
        Red
    }

    [Serializable]
    public enum BootsType
    {
        None,
        Default
    }

    [Serializable]
    public enum BootsMaterial
    {
        Blue,
        Brown,
        Red
    }
}