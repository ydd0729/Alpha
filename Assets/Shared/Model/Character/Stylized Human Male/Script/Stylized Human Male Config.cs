using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Collections;
using UnityEngine;

namespace Shared.StylizedHumanMale
{
    public class StylizedHumanMaleConfig : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer originalSkinnedMeshRenderer;
        [SerializeField] private SerializableDictionary<StylizedHumanMalePartType, SkinnedMeshRenderer> renderers;
        [SerializeField] private StylizedHumanMaleData data;
        [SerializeField] private StylizedHumanMaleConfigData config;
        
        private void OnValidate()
        {
            if (originalSkinnedMeshRenderer != null && data!= null && config != null)
            {
                foreach (var kv in config.stylizedHumanMaleConfig)
                {
                    StylizedHumanMalePartType type = kv.Key;
                    StylizedHumanMalePartConfig partConfig = kv.Value;

                    SkinnedMeshRenderer skinnedMeshRenderer = renderers[type];

                    if (skinnedMeshRenderer!= null)
                    {
                        skinnedMeshRenderer.bones = originalSkinnedMeshRenderer.bones;
                        skinnedMeshRenderer.rootBone = originalSkinnedMeshRenderer.rootBone;

                        if (partConfig.MeshIndex == -1)
                        {
                            skinnedMeshRenderer.sharedMesh = null;
                        }
                        else if (partConfig.MeshIndex < data.StylizedHumanMaleParts[type].Meshes.Length)
                        {
                            skinnedMeshRenderer.sharedMesh = data.StylizedHumanMaleParts[type].Meshes[partConfig.MeshIndex];
                        }

                        Material[] materials = new Material[skinnedMeshRenderer.sharedMaterials.Length];
                        foreach (var kv2 in partConfig.MaterialIndex)
                        {
                            if (kv2.Key < data.StylizedHumanMaleParts[type].Materials.Count)
                            {
                                materials[kv2.Key] = skinnedMeshRenderer.sharedMaterials[kv2.Key]; // assign original first
                                
                                if (kv2.Value < data.StylizedHumanMaleParts[type].Materials[kv2.Key].Count)
                                {
                                    materials[kv2.Key] = data.StylizedHumanMaleParts[type].Materials[kv2.Key][kv2.Value];
                                }
                            }
                        }

                        skinnedMeshRenderer.sharedMaterials = materials;
                    }
                }
            }
        }
    }
}
