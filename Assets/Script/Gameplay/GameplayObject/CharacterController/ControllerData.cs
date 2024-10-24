using UnityEngine;
using UnityEngine.Serialization;
using Yd.Collection;

namespace Yd.Gameplay.Object
{
    [CreateAssetMenu(fileName = "ControllerData", menuName = "Scriptable Objects/Gameplay/ControllerData")]
    public class ControllerData : ScriptableObject
    {
        public Vector3 controllerFollowOffset = Vector3.up;

        public GameObject followCameraPrefab;
        public float cameraVerticalArmLength = 0.4f;
        public float defaultCameraDistance = 5;
        public SRange<float> cameraDistanceRange;
        public float cameraZoomUnit = 0.05f;

        public float timeToRotate = 0.1f;
        public SRange<float> lookAroundAngleLimit = new(-70, 80);

        public bool showCursor;

        public float jumpForwardSpeed = 4;
        public float jumpUpSpeed = 4;

        public Vector3 gravitationalAcceleration = new(0, -9.8f, 0);
        public Vector3 airDrag = new(0.01f, 0.001f, 0.01f);

        public float groundTolerance = 0.2f;
        [FormerlySerializedAs("lookAroundSensitivity")] public SRangeFloat lookAroundUnit = new(0.1f, 0.05f);

        private void OnValidate()
        {
            lookAroundAngleLimit.ClampRange(-90, 0, 0, 90);
            lookAroundUnit.ClampRange(0, 1, 0, 1);
            cameraDistanceRange.ClampRange(0, Mathf.Infinity);
            cameraDistanceRange.Clamp(defaultCameraDistance);
        }
    }
}