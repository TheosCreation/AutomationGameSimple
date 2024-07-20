using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class PlayerMouse : MonoBehaviour
    {
        public Vector2 MousePosition { get; private set; }
        public Vector2 MouseInWorldPosition => Camera.main.ScreenToWorldPoint(MousePosition);

        private void OnEnable()
        {
            InputManager.Instance.playerInputActions.Game.MousePosition.performed += OnMousePositionPerformed;
        }

        private void OnDisable()
        {
            InputManager.Instance.playerInputActions.Game.MousePosition.performed -= OnMousePositionPerformed;
        }

        private void OnMousePositionPerformed(InputAction.CallbackContext ctx)
        {
            MousePosition = ctx.ReadValue<Vector2>();
        }

    }
}