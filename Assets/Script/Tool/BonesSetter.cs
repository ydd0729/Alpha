using UnityEngine;

namespace Yd.Tool
{
    public class BonesSetter : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer reference;

        public void SetBones()
        {
            if (reference == null)
            {
                return;
            }

            foreach (var skinnedMeshRenderer in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinnedMeshRenderer.bones = reference.bones;
                skinnedMeshRenderer.rootBone = reference.rootBone;
            }
        }
    }
}