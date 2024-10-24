using UnityEngine;
using Yd.Collection;

namespace Yd.StylizedHumanMale
{
    public class StylizedHumanMaleConfig : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer originalSkinnedMeshRenderer;
        [SerializeField] private SDictionary<StylizedHumanMalePartType, SkinnedMeshRenderer> renderers;
        [SerializeField] private StylizedHumanMaleData data;

        [SerializeField] private HairType hairType;
        [SerializeField] private HairMaterial hairMaterial;

        [SerializeField] private BrowsType browsType;
        [SerializeField] private BrowsMaterial browsMaterial;

        [SerializeField] private MoustacheType moustacheType;
        [SerializeField] private MoustacheMaterial moustacheMaterial;

        [SerializeField] private BeardType beardType;
        [SerializeField] private BeardMaterial beardMaterial;

        [SerializeField] private BodyType bodyType;
        [SerializeField] private EyeMaterial eyeMaterial;
        [SerializeField] private MouthMaterial mouthMaterial;
        [SerializeField] private SkinMaterial skinMaterial;

        [SerializeField] private ShirtType shirtType;
        [SerializeField] private ShirtMaterial shirtMaterial;

        [SerializeField] private BeltType beltType;
        [SerializeField] private BeltMaterial beltMaterial;

        [SerializeField] private PantsType pantsType;
        [SerializeField] private PantsMaterial pantsMaterial;

        [SerializeField] private BootsType bootsType;
        [SerializeField] private BootsMaterial bootsMaterial;

        private void OnValidate()
        {
            var hair = GetInitedMeshRenderer(StylizedHumanMalePartType.Hair);
            hair.sharedMesh = data.hairMeshes[hairType];
            hair.sharedMaterial = data.hairMaterials[hairMaterial];

            var brows = GetInitedMeshRenderer(StylizedHumanMalePartType.Brows);
            brows.sharedMesh = data.browsMeshes[browsType];
            brows.sharedMaterial = data.browsMaterials[browsMaterial];

            var moustache = GetInitedMeshRenderer(StylizedHumanMalePartType.Moustache);
            moustache.sharedMesh = data.moustacheMeshes[moustacheType];
            moustache.sharedMaterial = data.moustacheMaterials[moustacheMaterial];

            var beard = GetInitedMeshRenderer(StylizedHumanMalePartType.Beard);
            beard.sharedMesh = data.beardMeshes[beardType];
            beard.sharedMaterial = data.beardMaterials[beardMaterial];

            var body = GetInitedMeshRenderer(StylizedHumanMalePartType.Body);
            body.sharedMesh = data.bodyMeshes[bodyType];
            body.sharedMaterials = new[] { data.eyeMaterials[eyeMaterial], data.mouthMaterials[mouthMaterial], data.skinMaterials[skinMaterial] };

            var shirt = GetInitedMeshRenderer(StylizedHumanMalePartType.Shirt);
            shirt.sharedMesh = data.shirtMeshes[shirtType];
            shirt.sharedMaterial = data.shirtMaterials[shirtMaterial];

            var belt = GetInitedMeshRenderer(StylizedHumanMalePartType.Belt);
            belt.sharedMesh = data.beltMeshes[beltType];
            belt.sharedMaterial = data.beltMaterials[beltMaterial];

            var pants = GetInitedMeshRenderer(StylizedHumanMalePartType.Pants);
            pants.sharedMesh = data.pantsMeshes[pantsType];
            pants.sharedMaterial = data.pantsMaterials[pantsMaterial];

            var boots = GetInitedMeshRenderer(StylizedHumanMalePartType.Boots);
            boots.sharedMesh = data.bootsMeshes[bootsType];
            boots.sharedMaterial = data.bootsMaterials[bootsMaterial];
        }

        private SkinnedMeshRenderer GetInitedMeshRenderer(StylizedHumanMalePartType type)
        {
            var skinnedMeshRenderer = renderers[type];

            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.bones = originalSkinnedMeshRenderer.bones;
                skinnedMeshRenderer.rootBone = originalSkinnedMeshRenderer.rootBone;
            }

            return skinnedMeshRenderer;
        }
    }
}

