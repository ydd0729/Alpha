using UnityEngine;

namespace Yd.Gameplay.Object
{
    public struct MovementStateTransitionContext
    {
        public Character Character;
        public MovementState CurrentState;
        public MovementState LastState;

        public float GroundDistance;

        public CharacterController UnityCharacterController => Character.UnityCharacterController;

        public bool IsGrounded =>
            GroundDistance <= UnityCharacterController.skinWidth + GroundTolerance +
            (CurrentState.ShouldGrounded ? UnityCharacterController.stepOffset : 0);

        public readonly CharacterMovement CharacterMovement => Character.Movement;
        public readonly CharacterControllerBase CharacterController => Character.Controller;

        public float GroundTolerance => Character.Controller.ControllerData.groundTolerance;
    }
}