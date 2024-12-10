using UnityEngine;

namespace Yd.Gameplay.Object
{
    public struct MovementStateContext
    {
        public Character Character;
        public MovementState CurrentState;
        public MovementState LastOrNextState;
        public float EnterTime;
        public float GroundDistance;
        public bool JumpRequested;

        public CharacterController UnityController => Character.UnityController;

        public readonly CharacterMovement CharacterMovement => Character.Movement;
        public readonly GameplayCharacterController CharacterController => Character.Controller;

        public float Timer => Time.time - EnterTime;
    }
}