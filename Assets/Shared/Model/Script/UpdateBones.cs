using System;
using UnityEngine;

namespace Shared
{
    public class UpdateBones : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer originalSkinnedMeshRenderer;
        [SerializeField] private SkinnedMeshRenderer[] currentSkinnedMeshRenderers;

        private void OnValidate()
        {
            if (originalSkinnedMeshRenderer  != null && currentSkinnedMeshRenderers != null)
            {
                foreach (var currentSkinnedMeshRenderer in currentSkinnedMeshRenderers)
                {
                    currentSkinnedMeshRenderer.bones = originalSkinnedMeshRenderer.bones;
                    currentSkinnedMeshRenderer.rootBone = originalSkinnedMeshRenderer.rootBone;
                }
            }
        }
    }
}
