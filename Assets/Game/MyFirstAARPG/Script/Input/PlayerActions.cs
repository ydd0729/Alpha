using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFirstAARPG
{
    public class PlayerActions : MonoBehaviour, InputActions.IPlayerActions
    {
        [SerializeField] private bool showCursor;
        [SerializeField, Range(0, 1)] private float horizontalRotationSensitivity;
        [SerializeField, Range(0, 1)] private float verticalRotationSensitivity;

        public event Action<Vector2> Move;
        public event Action<Vector2> LookAround;
        public event Action Jump; 
        public event Action ToggleWalkRun;

        public void Initialize()
        {
#if !UNITY_EDITOR
            Cursor.visible = showCursor;
#endif

            if (inputActions != null)
            {
                inputActions.Disable();
                // inputActions.Dispose();
            }
            inputActions = new();
            inputActions.Enable();
            inputActions.Player.SetCallbacks(this);
        }

        private void OnDestroy()
        {
            inputActions?.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());

            // Debug.Log("Move");
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
            Jump?.Invoke();
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
            var mouseDelta = context.ReadValue<Vector2>();

            LookAround?.Invoke(new Vector2(mouseDelta.x * horizontalRotationSensitivity, mouseDelta.y * verticalRotationSensitivity));
        }

        public void OnToggleWalkRun(InputAction.CallbackContext context)
        {
            ToggleWalkRun?.Invoke();
        }

        private InputActions inputActions;
    }
}