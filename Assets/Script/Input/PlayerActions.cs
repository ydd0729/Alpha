using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Yd.Gameplay;
using Yd.Gameplay.Object;

namespace Yd.Input
{
    public class PlayerActions : MonoBehaviour, InputActions.IPlayerActions
    {
        // 玩家角色控制器
        private PlayerCharacterController controller;
        private InputActions inputActions;

        private void Awake()
        {
            inputActions?.Disable();
            inputActions = new InputActions();
        }

        private void OnDestroy()
        {
            inputActions?.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Jump?.Invoke();
            }
        }

        public void OnToggleWalkRun(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ToggleWalkRun?.Invoke();
            }
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<float>();
            if (input != 0)
            {
                Zoom?.Invoke(-input * controller.Character.PlayerCharacterData.cameraZoomUnit);
            }
        }

        public void OnSwitchWeaponForward(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameplayEvent?.Invoke(new GameplayEventArgs { EventType = GameplayEventType.SwitchWeaponForward });
            }
        }

        public void OnSwitchWeaponBackward(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameplayEvent?.Invoke(new GameplayEventArgs { EventType = GameplayEventType.SwitchWeaponBackward });
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();

            // 控制旋转视角的灵敏度
            LookAround?.Invoke(delta * controller.Character.PlayerCharacterData.lookAroundUnit);
        }

        public void OnNormalAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameplayEvent?.Invoke
                (
                    new GameplayAttackEventArgs
                    {
                        EventType = GameplayEventType.Attack,
                        AttackId = controller.Character.Weapon switch
                        {
                            Weapon.None => 0,
                            Weapon.Sword => 1,
                            _ => throw new ArgumentOutOfRangeException()
                        }
                    }
                );
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameplayEvent?.Invoke(new GameplayEventArgs { EventType = GameplayEventType.Interact });
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
        }

        public void OnNext(InputAction.CallbackContext context)
        {
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
        }

        public void EnableInput()
        {
            inputActions.Player.Enable();
            inputActions.Player.SetCallbacks(this);
        }

        public void DisableInput()
        {
            inputActions.Player.Disable();
        }

        // 移动
        public event Action<Vector2> Move;

        // 旋转视角
        public event Action<Vector2> LookAround;

        // 跳跃
        public event Action Jump;

        // 在走和跑之间切换
        public event Action ToggleWalkRun;

        public event Action<float> Zoom;

        public event Action<GameplayEventArgs> GameplayEvent;

        // 初始化
        public void Initialize(PlayerCharacterController controller)
        {
            this.controller = controller;
        }
    }
}