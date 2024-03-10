using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace in3d.EL.Systems.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Target;
        public float SmoothSpeed = 0.125f;
        public Vector3 Offset;

        void LateUpdate()
        {
            if (Target == null) return;
            Vector3 desiredPosition = Target.position + Offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(Target);
        }
    }
}
