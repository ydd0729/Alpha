using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFirstAARPG
{
    public class PlayerController : ActorController
    {
        // [SerializeField] private PlayerActions playerActions;
        [SerializeField] private Vector2 rotationEulerAngleXLimit;

        public PlayerActions PlayerActions { private get; set; }

        public void Initialize(Character inCharacter, PlayerActions playerActions, CinemachineCamera followCamera)
        {
            base.Initialize(inCharacter);

            PlayerActions = playerActions;
            
            PlayerActions.Move += PlayerActionsOnMove;
            PlayerActions.LookAround += PlayerActionsOnLookAround;
            
            followCamera.Target = new CameraTarget() { TrackingTarget = transform, CustomLookAtTarget = false };
        }

        private void PlayerActionsOnMove(object sender, Vector2 moveVector)
        {
            LocalMoveDirection = new Vector3(moveVector.x, 0, moveVector.y);
        }

        private void PlayerActionsOnLookAround(object sender, Vector2 delta)
        {
            var rotation = transform.rotation.eulerAngles;
            
            rotation.x += -delta.y;
            rotation.x = rotation.x <= 180 ? Math.Min(rotation.x, rotationEulerAngleXLimit.y) : Math.Max(rotation.x, 360 + rotationEulerAngleXLimit.x);
            
            rotation.y += delta.x;
            
            transform.rotation = Quaternion.Euler(rotation);
        }

        private void OnValidate()
        {
            rotationEulerAngleXLimit.x = Math.Clamp(rotationEulerAngleXLimit.x, -90, 0);
            rotationEulerAngleXLimit.y = Math.Clamp(rotationEulerAngleXLimit.y, 0, 90);
        }
    }
}