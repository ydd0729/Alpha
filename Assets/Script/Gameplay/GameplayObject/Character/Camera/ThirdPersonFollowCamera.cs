using Unity.Cinemachine;
using UnityEngine;

namespace Yd.Gameplay.Object
{
    public class ThirdPersonFollowCamera : MonoBehaviour
    {
        private CinemachineThirdPersonFollow cinemachineThirdPersonFollow;
        private PlayerCharacterController controller;

        public void Initialize(PlayerCharacterController controller)
        {
            this.controller = controller;

            var followCamera = GetComponent<CinemachineCamera>();
            followCamera.Target = new CameraTarget
            {
                TrackingTarget = controller.transform,
                CustomLookAtTarget = false
            };

            cinemachineThirdPersonFollow = GetComponent<CinemachineThirdPersonFollow>();
            cinemachineThirdPersonFollow.VerticalArmLength = controller.Data.cameraVerticalArmLength;
            cinemachineThirdPersonFollow.CameraDistance = controller.Data.defaultCameraDistance;

            controller.PlayerActions.Zoom += OnZoom;
        }

        private void OnZoom(float zoom)
        {
            cinemachineThirdPersonFollow.CameraDistance += zoom;
            cinemachineThirdPersonFollow.CameraDistance = controller.Data.cameraDistanceRange.Clamp
                (cinemachineThirdPersonFollow.CameraDistance);

            // DebugE.LogValue(nameof(zoom), zoom);
            // DebugE.LogValue(nameof(cinemachineThirdPersonFollow.CameraDistance), cinemachineThirdPersonFollow.CameraDistance);
        }
    }
}