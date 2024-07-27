using System;
using Shared.Extension;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFirstAARPG
{
    public class PlayerCharacterController : CharacterControllerBase
    {
        [SerializeField] private Vector2 rotationEulerAngleXLimit;
        public PlayerActions PlayerActions { private get; set; }
        
        private void OnValidate()
        {
            rotationEulerAngleXLimit.x = Math.Clamp(rotationEulerAngleXLimit.x, -90, 0);
            rotationEulerAngleXLimit.y = Math.Clamp(rotationEulerAngleXLimit.y, 0, 90);
        }

        public void Initialize(Character inCharacter, PlayerActions playerActions)
        {
            base.Initialize(inCharacter);

            PlayerActions = playerActions;

            PlayerActions.Move += PlayerActionsOnMove;
            PlayerActions.LookAround += PlayerActionsOnLookAround;
            PlayerActions.ToggleWalkRun += PlayerActionsOnToggleWalkRun;
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
            rotation.x = rotation.x <= 180 ? Math.Min(rotation.x, rotationEulerAngleXLimit.y) : Math.Max(rotation.x, 360 + rotationEulerAngleXLimit.x);

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