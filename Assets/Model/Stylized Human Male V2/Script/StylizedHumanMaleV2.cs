using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Yd.Collection;

namespace Yd.StylizedHumanMaleV2
{
    [RequireComponent(typeof(StylizedHumanMaleV2ConfigureButton))]
    public class StylizedHumanMaleV2 : MonoBehaviour
    {
        [SerializeField] private SDictionary<StylizedHumanMaleV2PartType, SkinnedMeshRenderer[]> renderers;

        [SerializeField] private StylizedHumanMaleV2Data data;
        [SerializeField] private StylizedHumanMaleV2Config config;

        private void Start()
        {
            Configure();
        }

        public void Configure(StylizedHumanMaleV2Config config = null)
        {
            if (config == null)
            {
                config = this.config;
            }
            else
            {
                this.config = config;
            }

            ConfigurePartMaterials
                (renderers[StylizedHumanMaleV2PartType.Eye], data.eyeMaterials.GetValueOrDefault(config.eyeMaterial));

            ConfigurePartMeshes(renderers[StylizedHumanMaleV2PartType.Hair], data.hairMeshesMaterials[config.hairMesh].Key);
            ConfigurePartMaterials
            (
                renderers[StylizedHumanMaleV2PartType.Hair],
                data.hairMeshesMaterials[config.hairMesh].Value.GetValueOrDefault(config.hairMaterial)
            );

            ConfigurePartMaterials
                (renderers[StylizedHumanMaleV2PartType.Skin], data.skinMaterials.GetValueOrDefault(config.skinMaterial));

            ConfigurePartMeshes(renderers[StylizedHumanMaleV2PartType.Torso], data.torsoMeshesMaterials[config.torsoMesh].Key);
            ConfigurePartMaterials
            (
                renderers[StylizedHumanMaleV2PartType.Torso],
                data.torsoMeshesMaterials[config.torsoMesh].Value.GetValueOrDefault(config.torsoMaterial)
            );

            ConfigurePartMaterials
                (renderers[StylizedHumanMaleV2PartType.Pants], data.pantsMaterials.GetValueOrDefault(config.pantsMaterial));

            ConfigurePartMeshes(renderers[StylizedHumanMaleV2PartType.Boots], data.bootsMeshesMaterials[config.bootsMesh].Key);
            ConfigurePartMaterials
            (
                renderers[StylizedHumanMaleV2PartType.Boots],
                data.bootsMeshesMaterials[config.bootsMesh].Value.GetValueOrDefault(config.bootsMaterial)
            );

            ConfigurePartMeshes(renderers[StylizedHumanMaleV2PartType.Brows], data.browsMesh[config.browsMesh]);
            ConfigurePartMaterials
                (renderers[StylizedHumanMaleV2PartType.Brows], data.browsMaterials.GetValueOrDefault(config.browsMaterial));

            ConfigurePartMeshes(renderers[StylizedHumanMaleV2PartType.Helmet], data.helmetMesh[config.helmetMesh]);
            ConfigurePartMaterials
                (renderers[StylizedHumanMaleV2PartType.Helmet], data.helmetMaterials.GetValueOrDefault(config.helmetMaterial));

            ConfigurePartMeshes(renderers[StylizedHumanMaleV2PartType.Beard], data.beardMesh[config.beardMesh]);
            ConfigurePartMaterials
                (renderers[StylizedHumanMaleV2PartType.Beard], data.beardMaterials.GetValueOrDefault(config.beardMaterial));

            ConfigurePartMaterials
                (renderers[StylizedHumanMaleV2PartType.Belt], data.beltMaterials.GetValueOrDefault(config.beltMaterial));

            ConfigurePartMeshes(renderers[StylizedHumanMaleV2PartType.Cape], data.capeMesh[config.capeMesh]);
            ConfigurePartMaterials
                (renderers[StylizedHumanMaleV2PartType.Cape], data.capeMaterials.GetValueOrDefault(config.capeMaterial));

            ConfigurePartMaterials
                (renderers[StylizedHumanMaleV2PartType.Gloves], data.gloveMaterials.GetValueOrDefault(config.gloveMaterial));
        }

        private static void ConfigurePartMaterials(
            IReadOnlyList<SkinnedMeshRenderer> partRenderers, [CanBeNull] IReadOnlyList<MaterialList> materialLists
        )
        {
            if (materialLists != null)
            {
                var count = Math.Min(partRenderers.Count, materialLists.Count);
                for (var i = 0; i < count; ++i)
                {
                    ConfigureRendererMaterial(partRenderers[i], materialLists[i].list);
                }
            }
        }

        private static void ConfigurePartMaterials(IReadOnlyList<SkinnedMeshRenderer> partRenderers, MaterialList materialList)
        {
            var materialLists = new MaterialList[partRenderers.Count];

            for (var i = 0; i < materialLists.Length; ++i)
            {
                materialLists[i] = materialList;
            }

            ConfigurePartMaterials(partRenderers, materialLists);
        }

        private static void ConfigurePartMaterials(IReadOnlyList<SkinnedMeshRenderer> partRenderers, Material[] materials)
        {
            ConfigurePartMaterials(partRenderers, new MaterialList { list = materials });
        }

        private static void ConfigurePartMaterials(IReadOnlyList<SkinnedMeshRenderer> partRenderers, Material material)
        {
            ConfigurePartMaterials(partRenderers, new[] { material });
        }

        //

        private static void ConfigurePartMeshes(
            IReadOnlyList<SkinnedMeshRenderer> partRenderers, [CanBeNull] IReadOnlyList<Mesh> meshes
        )
        {
            if (meshes != null)
            {
                var count = Math.Min(partRenderers.Count, meshes.Count);
                for (var i = 0; i < count; ++i)
                {
                    ConfigureRendererMesh(partRenderers[i], meshes[i]);
                }
            }
        }

        private static void ConfigurePartMeshes(IReadOnlyList<SkinnedMeshRenderer> partRenderers, [CanBeNull] Mesh mesh)
        {
            var meshes = new List<Mesh>(partRenderers.Count);
            for (var i = 0; i < partRenderers.Count; ++i)
            {
                meshes.Add(mesh);
            }

            ConfigurePartMeshes(partRenderers, meshes);
        }

        //

        private static void ConfigureRendererMesh([CanBeNull] SkinnedMeshRenderer partRenderer, [CanBeNull] Mesh mesh)
        {
            if (partRenderer == null)
            {
                return;
            }

            partRenderer.sharedMesh = mesh;
            partRenderer.enabled = mesh != null;
        }

        // 跳过 null 的材质

        private static void ConfigureRendererMaterial(
            [CanBeNull] Renderer renderer, [CanBeNull] IReadOnlyList<Material> materials
        )
        {
            if (renderer == null || materials == null)
            {
                return;
            }

            var sharedMaterials = new Material[Math.Max(renderer.sharedMaterials.Length, materials.Count)];
            renderer.sharedMaterials.CopyTo(sharedMaterials.AsSpan());

            for (var j = 0; j < materials.Count; ++j)
            {
                if (materials[j] != null)
                {
                    sharedMaterials[j] = materials[j];
                }
            }

            renderer.sharedMaterials = sharedMaterials;
        }
    }
}