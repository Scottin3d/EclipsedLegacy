
using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace in3d.EL.Systems.Camera
{
    public class CameraSystem : MonoBehaviour
    {
        // move
        [Header("Move settings")]
        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float dragPanSpeed = 2f;
        [SerializeField] private bool edgeScrollingEnabled = true;
        [SerializeField] private bool dragPanMoveEnabled = true;
        private bool dragPanMoveActive = false;
        private Vector2 lastMousePosition = Vector2.zero;

        // rotate
        [Header("Rotation settings")]
        [SerializeField] private float rotationSpeed = 50f;
        
        // zoom
        [Header("Zoom settings")]
        [SerializeField] private bool fovZoomEnabled = false;
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float maxFieldOfView = 50f;
        [SerializeField] private float minFieldOfView = 10f;
        private float targetFieldOfView = 50f;
        [SerializeField] private float maxFollowOffset = 50f;
        [SerializeField] private float minFollowOffset = 5f;
        private Vector3 targetFollowOffset;

        void Awake()
        {
            targetFollowOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        }
        void Update()
        {
            HandleMove();
            HandleRotation();
            HandleEdgeScrolling();
            HandleDragPanMove();
            HandleFOVZoom();
            HandleMoveZoom();
        }

        private void HandleMoveZoom()
        {
            if(fovZoomEnabled) return;
            
            Vector3 zoomDirection = targetFollowOffset.normalized;
            float yScrollDelta = Mouse.current.scroll.ReadValue().y;

            if(yScrollDelta == 0) return;

            if(yScrollDelta > 0){
                targetFollowOffset += zoomDirection;

            }
            if(yScrollDelta < 0){
               targetFollowOffset -= zoomDirection;
            }

            if(targetFollowOffset.magnitude > maxFollowOffset){
                targetFollowOffset =  zoomDirection * maxFollowOffset;
            }
            if(targetFollowOffset.magnitude < minFollowOffset){
                targetFollowOffset =  zoomDirection * minFollowOffset;
            }
            // modify the cinemachine  offset
        
            virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);
        }

        private void HandleFOVZoom()
        {
            if(!fovZoomEnabled) return;
            // TODO:
            // handle invert scroll
            float yScrollDelta = Mouse.current.scroll.ReadValue().y;
            if(yScrollDelta > 0){
                targetFieldOfView -= 5f;
            }
            if(yScrollDelta < 0){
                targetFieldOfView += 5f;
            }
            targetFieldOfView = Mathf.Clamp(targetFieldOfView, minFieldOfView, maxFieldOfView);
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFieldOfView, zoomSpeed * Time.deltaTime);
        }

        private void HandleDragPanMove()
        {
            if(!dragPanMoveEnabled) return;
            if(Mouse.current.middleButton.wasPressedThisFrame){
                dragPanMoveActive = true;
                lastMousePosition = Mouse.current.position.ReadValue();
            }

            if(Mouse.current.middleButton.wasReleasedThisFrame){
                dragPanMoveActive = false;
            }
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

            Vector3 moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        private void HandleEdgeScrolling()
        {
            if(!edgeScrollingEnabled) return;
            // TODO:
            // increase threshold and increase speed as mouse gets closer to edge, clamp at movement speed
            Vector3 inputDirection = Vector3.zero;

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

            Vector3 moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}
