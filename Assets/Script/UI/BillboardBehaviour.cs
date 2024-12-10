using UnityEngine;

namespace Script.UI
{
    public class BillboardBehaviour : MonoBehaviour
    {
        public bool faceCamera;

        protected virtual void LateUpdate()
        {
            if (faceCamera)
            {
                var camera = Camera.main;
                if (camera)
                {
                    transform.LookAt(transform.position + camera.transform.forward);
                }
            }
        }
    }
}