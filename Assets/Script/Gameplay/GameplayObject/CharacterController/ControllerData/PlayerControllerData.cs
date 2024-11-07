using UnityEngine;
using Yd.Collection;

namespace Yd.Gameplay.Object
{
    [CreateAssetMenu(fileName = "Player Controller Data", menuName = "Scriptable Objects/Gameplay/Player Controller Data")]
    public class PlayerControllerData : ControllerData
    {
        // Camera
        public GameObject followCameraPrefab;
        public float cameraVerticalArmLength = 0.4f;
        public float defaultCameraDistance = 4;
        public SRange<float> cameraDistanceRange;
        public float cameraZoomUnit = 0.05f;

        // Look Around
        public SRange<float> lookAroundAngleLimit = new(-70, 80);
        public SRangeFloat lookAroundUnit = new(0.1f, 0.05f);

        private void OnValidate()
        {
            lookAroundAngleLimit.ClampRange(-90, 0, 0, 90);
            lookAroundUnit.ClampRange(0, 1, 0, 1);

            cameraDistanceRange.ClampRange(0, Mathf.Infinity);
            cameraDistanceRange.Clamp(defaultCameraDistance);
        }
    }
}