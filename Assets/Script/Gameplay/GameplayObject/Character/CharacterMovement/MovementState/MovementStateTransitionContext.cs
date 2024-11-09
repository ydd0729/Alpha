using UnityEngine;

namespace Yd.Gameplay.Object
{
    public struct MovementStateTransitionContext
    {
        public Character Character;
        public MovementState CurrentState;
        public MovementState LastOrNextState;
        public float EnterTime;

        public float GroundDistance;

        public CharacterController UnityController => Character.UnityController;

        public bool IsGrounded => GroundDistance <=
                                  (CurrentState.IsGroundState ? GroundToleranceWhenGrounded : GroundToleranceWhenFalling);

        public readonly CharacterMovement CharacterMovement => Character.Movement;
        public readonly GameplayCharacterController CharacterController => Character.Controller;

        public float GroundToleranceWhenFalling => Character.Data.GroundToleranceWhenFalling;
        public float GroundToleranceWhenGrounded => Character.Data.GroundToleranceWhenGrounded;

        public float Timer => Time.time - EnterTime;
    }
}