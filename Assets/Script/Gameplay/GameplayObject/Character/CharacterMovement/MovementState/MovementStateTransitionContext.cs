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
            (!CurrentState.ShouldGrounded && GroundDistance <= UnityCharacterController.skinWidth + GroundTolerance) ||
            (CurrentState.ShouldGrounded && GroundDistance <= UnityCharacterController.skinWidth +
                UnityCharacterController.stepOffset + GroundTolerance);

        public readonly CharacterMovement CharacterMovement => Character.Movement;
        public readonly CharacterControllerBase CharacterController => Character.Controller;

        public float GroundTolerance => Character.Controller.Data.groundTolerance;
    }
}