using UnityEngine;
using UnityEngine.Serialization;

namespace Yd.Gameplay.Object
{
    [CreateAssetMenu(fileName = "ControllerData", menuName = "Scriptable Objects/Gameplay/ControllerData")]
    public class CharacterData : ScriptableObject
    {
        // Follow Offset
        public Vector3 controllerFollowOffset = Vector3.up;

        // Rotation
        public float timeToRotate = 0.1f;

        // Jump
        public float jumpForwardSpeed = 1f;
        public float jumpUpSpeed = 4;

        // Movement
        public bool useRootMotionMovement;
        public bool useStrafeSet;

        [FormerlySerializedAs("groundTolerance")] public float GroundToleranceWhenGrounded = 0.01f;
        public float GroundToleranceWhenFalling = 0.01f;
    }
}