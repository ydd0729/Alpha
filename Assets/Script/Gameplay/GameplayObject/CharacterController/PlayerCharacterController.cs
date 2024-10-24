using System;
using UnityEngine;
using Yd.Extension;
using Yd.Input;

namespace Yd.Gameplay.Object
{
    public class PlayerCharacterController : CharacterControllerBase
    {
        public PlayerActions PlayerActions
        {
            get;
            private set;
        }

        public override void Initialize(Character character)
        {
            base.Initialize(character);

            PlayerActions = gameObject.GetOrAddComponent<PlayerActions>();
            PlayerActions.Initialize(this);

            PlayerActions.Move += OnMove;
            PlayerActions.LookAround += OnLookAround;
            PlayerActions.ToggleWalkRun += OnToggleWalkRun;
            PlayerActions.Jump += OnJump;
            PlayerActions.GameplayEvent += OnGameplayEvent;

            var thirdPersonFollowCamera = Instantiate
                (Data.followCameraPrefab, transform.parent).GetComponent<ThirdPersonFollowCamera>();
            thirdPersonFollowCamera.Initialize(this);
        }

        private void OnToggleWalkRun()
        {
            WalkRunToggle = WalkRunToggle switch
            {
                EWalkRunToggle.Walk => EWalkRunToggle.Run,
                EWalkRunToggle.Run => EWalkRunToggle.Walk,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void OnLookAround(Vector2 delta)
        {
            var rotation = transform.rotation.eulerAngles;

            rotation.x += -delta.y;
            rotation.x = rotation.x <= 180
                ? Math.Min(rotation.x, Data.lookAroundAngleLimit.Max)
                : Math.Max(rotation.x, 360 + Data.lookAroundAngleLimit.Min);

            rotation.y += delta.x;

            transform.rotation = Quaternion.Euler(rotation);
        }

        private void OnMove(Vector2 moveVector)
        {
            LocalMoveDirection = new Vector3(moveVector.x, 0, moveVector.y);
        }

        private void OnJump()
        {
            if (Character.Movement.TryTransitTo(MovementState.Jump))
            {
                Jump();
            }
        }
    }
}