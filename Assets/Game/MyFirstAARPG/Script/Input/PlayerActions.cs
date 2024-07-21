using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFirstAARPG
{
    public class PlayerActions : MonoBehaviour, InputActions.IPlayerActions
    {
        [SerializeField] private bool showCursor;
        [SerializeField] private bool lockCursor;
        [SerializeField, Range(0, 1)] private float horizontalRotationSensitivity;
        [SerializeField, Range(0, 1)] private float verticalRotationSensitivity;

        public event EventHandler<Vector2> Move;
        public event EventHandler<Vector2> LookAround;

        public event EventHandler ToggleWalkRun;

        private void Awake()
        {
            inputActions = new();

#if !UNITY_EDITOR
            Cursor.visible = showCursor;
#endif
        }

        private void Start()
        {
            inputActions.Enable();
            inputActions.Player.SetCallbacks(this);
            
            initialCursorPosition =
                new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
        }

        private void OnDestroy()
        {
            inputActions?.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(this, context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnLookAround(InputAction.CallbackContext context)
        {
            // if (context.performed)
            // {
                var mouseDelta = context.ReadValue<Vector2>();

#if !UNITY_EDITOR
                if (lockCursor)
                {
                    Mouse.current.WarpCursorPosition(initialCursorPosition);
                }
#endif

                LookAround?.Invoke(this,
                    new Vector2(mouseDelta.x * horizontalRotationSensitivity,
                        mouseDelta.y * verticalRotationSensitivity));
            // }
        }

        public void OnToggleWalkRun(InputAction.CallbackContext context)
        {
            ToggleWalkRun?.Invoke(this, EventArgs.Empty);
        }

        private InputActions inputActions;
        private Vector2 initialCursorPosition;
    }

    public class PlayerActionEventArgs : EventArgs
    {
        private InputAction.CallbackContext context;
    }
}