
using UnityEngine;
using UnityEngine.InputSystem;

namespace in3d.EL.Systems.Camera
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float rotationSpeed = 50f;
        [SerializeField] private float dragPanSpeed = 2f;
        [SerializeField] private bool edgeScrollingEnabled = true;

        private bool dragPanMoveActive = false;
        private Vector2 lastMousePosition = Vector2.zero;
        void Update()
        {
            HandleMove();
            HandleRotation();

            if(Mouse.current.middleButton.wasPressedThisFrame){
                dragPanMoveActive = true;
                lastMousePosition = Mouse.current.position.ReadValue();
            }

            if(Mouse.current.middleButton.wasReleasedThisFrame){
                dragPanMoveActive = false;
            }

            HandleDragPanMove();
        }

        private void HandleDragPanMove()
        {
            if(!dragPanMoveActive) return;
            Vector2 mouseMovementDelta = Mouse.current.position.ReadValue() - lastMousePosition;
            
            Vector3 inputDirection = Vector3.zero;
            inputDirection.x = -mouseMovementDelta.x;
            inputDirection.z = -mouseMovementDelta.y;
            Vector3 moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;
            transform.position += moveDirection * dragPanSpeed * Time.deltaTime;
            lastMousePosition = Mouse.current.position.ReadValue();
        }
        private void HandleRotation()
        {
            float rotationDirection = 0f;
            if (Keyboard.current.qKey.isPressed) rotationDirection = 1f;
            if (Keyboard.current.eKey.isPressed) rotationDirection = -1f;

            Vector3 rotation = new Vector3(0f, rotationDirection, 0f) * rotationSpeed * Time.deltaTime;
            transform.Rotate(rotation);
        }

        private void HandleMove()
        {
            Vector3 inputDirection = Vector3.zero;

            if (Keyboard.current.wKey.isPressed) inputDirection.z = 1f;
            if (Keyboard.current.sKey.isPressed) inputDirection.z = -1f;
            if (Keyboard.current.aKey.isPressed) inputDirection.x = -1f;
            if (Keyboard.current.dKey.isPressed) inputDirection.x = 1f;

            // TODO:
            // increase threshold and increase speed as mouse gets closer to edge, clamp at movement speed
            MouseEdgeDetection(ref inputDirection);

            Vector3 moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        private void MouseEdgeDetection(ref Vector3 inputDirection)
        {
            if(!edgeScrollingEnabled) return;

            Vector3 mousePosition = Mouse.current.position.ReadValue();
            int edgeScrollThreshold = 20;
            if (mousePosition.y >= Screen.height - edgeScrollThreshold)
            {
                inputDirection.z = 1f;
            }
            if (mousePosition.y <= edgeScrollThreshold)
            {
                inputDirection.z = -1f;
            }
            if (mousePosition.x >= Screen.width - edgeScrollThreshold)
            {
                inputDirection.x = 1f;
            }
            if (mousePosition.x <= edgeScrollThreshold)
            {
                inputDirection.x = -1f;
            }
        }
    }
}
