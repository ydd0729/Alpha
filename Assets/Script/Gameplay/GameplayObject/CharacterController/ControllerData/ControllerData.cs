using UnityEngine;

namespace Yd.Gameplay.Object
{
    [CreateAssetMenu(fileName = "ControllerData", menuName = "Scriptable Objects/Gameplay/ControllerData")]
    public class ControllerData : ScriptableObject
    {
        // Follow Offset
        public Vector3 controllerFollowOffset = Vector3.up;

        // Rotation
        public float timeToRotate = 0.1f;

        // Jump
        public float jumpForwardSpeed = 4;
        public float jumpUpSpeed = 4;

        // Acceleration
        public Vector3 gravitationalAcceleration = new(0, -9.8f, 0);
        public Vector3 airDrag = new(0.01f, 0.001f, 0.01f);

        // Movement
        public bool useRootMotionMovement;

        public bool useStrafeSet;

        public float groundTolerance = 0.2f;
    }
}