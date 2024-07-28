using System;
using Shared.Extension;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace MyFirstAARPG
{
    public class PlayerCharacterController : CharacterControllerBase
    {
        [FormerlySerializedAs("rotationEulerAngleXLimit")] [SerializeField] private Vector2 lookAroundAngleLimit;
        
        private void OnValidate()
        {
            lookAroundAngleLimit.x = Math.Clamp(lookAroundAngleLimit.x, -90, 0);
            lookAroundAngleLimit.y = Math.Clamp(lookAroundAngleLimit.y, 0, 90);
        }

        public override void Initialize(Character inCharacter)
        {
            base.Initialize(inCharacter);

            var playerActions = inCharacter.PlayerActions;
            playerActions.Move += PlayerActionsOnMove;
            playerActions.LookAround += PlayerActionsOnLookAround;
            playerActions.ToggleWalkRun += PlayerActionsOnToggleWalkRun;
            playerActions.Jump += PlayerActionsOnJump;
        }

        private void PlayerActionsOnJump()
        {
            AnimationState = AnimationState.Jump;
        }

        private void PlayerActionsOnToggleWalkRun()
        {
            WalkRunToggle = WalkRunToggle switch
            {
                EWalkRunToggle.Walk => EWalkRunToggle.Run,
                EWalkRunToggle.Run => EWalkRunToggle.Walk,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void PlayerActionsOnLookAround(Vector2 delta)
        {
            var rotation = transform.rotation.eulerAngles;

            rotation.x += -delta.y;
            rotation.x = rotation.x <= 180 ? Math.Min(rotation.x, lookAroundAngleLimit.y) : Math.Max(rotation.x, 360 + lookAroundAngleLimit.x);

            rotation.y += delta.x;

            transform.rotation = Quaternion.Euler(rotation);
        }

        private void PlayerActionsOnMove(Vector2 moveVector)
        {
            LocalMoveDirection = new Vector3(moveVector.x, 0, moveVector.y);
            
            // DebugExtension.LogValue(nameof(LocalMoveDirection), LocalMoveDirection);
        }
    }
}