using System;
using UnityEngine;
using Yd.Extension;
using Yd.Input;

namespace Yd.Gameplay.Object
{
    public class PlayerCharacterController : GameplayCharacterController
    {
        public PlayerActions PlayerActions
        {
            get;
            private set;
        }

        public override void Initialize(Character character)
        {
            base.Initialize(character);

            NavMeshAgent.updatePosition = false;

            PlayerActions = gameObject.GetOrAddComponent<PlayerActions>();
            PlayerActions.Initialize(this);

            PlayerActions.Move += OnMove;
            PlayerActions.LookAround += OnLookAround;
            PlayerActions.ToggleWalkRun += OnToggleWalkRun;
            PlayerActions.Jump += Character.Movement.RequestJump;
            PlayerActions.GameplayEvent += OnGameplayEvent;

            var thirdPersonFollowCamera = Instantiate
                (Character.PlayerCharacterData.followCameraPrefab, transform.parent).GetComponent<ThirdPersonFollowCamera>();
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
                ? Math.Min(rotation.x, Character.PlayerCharacterData.lookAroundAngleLimit.MaxInclusive)
                : Math.Max(rotation.x, 360 + Character.PlayerCharacterData.lookAroundAngleLimit.MinInclusive);

            rotation.y += delta.x;

            transform.rotation = Quaternion.Euler(rotation);
        }

        private void OnMove(Vector2 moveVector)
        {
            LocalMoveDirection = new Vector3(moveVector.x, 0, moveVector.y);
        }
    }
}