
using System;
using System.Collections;
using System.Collections.Generic;
using in3d.EL.Agent.Controllers;
using KBCore.Refs;
using UnityEngine;

namespace in3d.Utilities.GameLogic.Detection
{
    public enum GizmoDisplayType { d3, d2 }
    public class FieldOfView : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private AgentTargetController agentTargetController;
        public GizmoDisplayType gizmoType = GizmoDisplayType.d2;
        CountdownTimer detectionTimer;
        public bool ShowGizmos = true;
        [SerializeField] private Vector3 fovOffset = Vector3.zero;
        [SerializeField] private float detectionRadius = 10f;
        public float DetectionRadius => detectionRadius;
        [SerializeField] private float innerDetectionRadius = 5f;
        public float InnerDetectionRadius => innerDetectionRadius;
        [SerializeField] private float fovAngle = 60f;
        public float FOVAngle => fovAngle;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstructionMask;
        [SerializeField] private float detectionCoolDown = 1f;
        [SerializeField] public bool CanSeeTarget => targets.Count > 0;

        private Transform bestTarget = null;
        [SerializeField] List<Transform> targets;
        public Transform BestTarget => bestTarget;

        public static Action<Transform, Transform> TargetDetected;

        private void Start()
        {
            StartCoroutine(FOVRoutine());
            targets = new List<Transform>();
        }

        public void StartFOV()
        {
            StartCoroutine(FOVRoutine());
        }

        public void StopFOV()
        {
            StopCoroutine(FOVRoutine());
        }

        private IEnumerator FOVRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(detectionCoolDown);
                FieldOfViewCheck();
            }
        }

        /// <summary>
        /// Checks the field of view to determine if the player is within sight.
        /// </summary>
        private void FieldOfViewCheck()
        {
            bestTarget = null;
            targets.Clear();

            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, DetectionRadius, targetMask);

            if (rangeChecks.Length != 0)
            {
                foreach (Collider potentialTarget in rangeChecks)
                {
                    Transform target = potentialTarget.transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    float angle = Vector3.Angle(transform.forward, directionToTarget);
                    if (angle < fovAngle / 2 || distanceToTarget < InnerDetectionRadius)
                    // if (angle < FOVAngle / 2)
                    {
                        RaycastHit hit;
                        if (!Physics.Raycast(transform.position + fovOffset, directionToTarget, out hit, distanceToTarget, obstructionMask))
                        {
                            targets.Add(target);
                            // TargetDetected?.Invoke(transform, null);
                            // agentTargetController.SetLookAtTarget();
                        }
                    }
                }
            }

            // sort targets by distance ascending
            targets.Sort((a, b) => Vector3.Distance(transform.position, a.position).CompareTo(Vector3.Distance(transform.position, b.position)));
            
            // best target is the first in the list else null
            bestTarget = targets.Count > 0 ? targets[0] : null;
            TargetDetected?.Invoke(transform, bestTarget);
        }


        private void OnDrawGizmos()
        {
            if (ShowGizmos)
            {

                Gizmos.color = Color.white;
                // outer radius
                DrawGizmoCircle(transform.position, Vector3.forward, DetectionRadius);
                if(gizmoType == GizmoDisplayType.d3){
                    Gizmos.DrawWireSphere(transform.position, DetectionRadius);
                }

                // inner radius
                Gizmos.color = Color.yellow;
                DrawGizmoCircle(transform.position, Vector3.forward, InnerDetectionRadius);
                if(gizmoType == GizmoDisplayType.d3){
                    Gizmos.DrawWireSphere(transform.position, InnerDetectionRadius);
                }

                Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -fovAngle / 2);
                Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, fovAngle / 2);

                // view angle
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * DetectionRadius);
                Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * DetectionRadius);

                if (CanSeeTarget && BestTarget != null)
                {
                    Gizmos.DrawLine(transform.position, BestTarget.position);
                }
            }
        }
        
        private void DrawGizmoCircle(Vector3 position, Vector3 direction, float radius)
        {
            Vector3[] points = new Vector3[36];
            Quaternion rot = Quaternion.LookRotation(direction);
            for (int i = 0; i < 36; i++)
            {
                points[i] = position + rot * Vector3.forward * radius;
                rot *= Quaternion.Euler(0, 10, 0);
            }
            for (int i = 0; i < 36; i++)
            {
                Gizmos.DrawLine(points[i], points[(i + 1) % 36]);
            }
        }        
        private Vector3 DirectionFromAngle(float eulerV, float angleInDegrees)
        {
            angleInDegrees += eulerV;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

    }
}

