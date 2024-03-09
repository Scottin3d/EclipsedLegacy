using in3d.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace in3d.EL.Player.Controllers
{
    [DisallowMultipleComponent]
    public class PlayerInputController : Singleton<PlayerInputController>,  @PlayerInputs.IPointClickActions
    {
        PlayerInputs playerInputs;
        public bool Click = false;
        public bool Ctrl = false;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = false;
        public bool cursorInputForLook = false;

        public void OnCtrl(InputAction.CallbackContext context) => Ctrl = context.ReadValueAsButton();

        public void OnMove(InputAction.CallbackContext context) => Click = context.ReadValueAsButton();

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        public void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        void Awake(){
            playerInputs = new PlayerInputs();
            playerInputs.PointClick.AddCallbacks(this);
            playerInputs.PointClick.Enable();
        }

        void LateUpdate(){
            Click = false;
        }
    }
}


