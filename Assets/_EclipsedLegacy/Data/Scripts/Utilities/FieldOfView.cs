
using System.Collections;
using UnityEngine;

namespace in3d.Utilities.GameLogic.Detection
{
    public enum GizmoDisplayType { d3, d2 }
    public class FieldOfView : MonoBehaviour
    {
        public GizmoDisplayType gizmoType = GizmoDisplayType.d2;
        CountdownTimer detectionTimer;
        public bool ShowGizmos = true;
        [SerializeField] private float detectionRadius = 10f;
        public float DetectionRadius => detectionRadius;
        [SerializeField] private float innerDetectionRadius = 5f;
        public float InnerDetectionRadius => innerDetectionRadius;
        public float Angle { get; private set; } = 60f;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstructionMask;
        [SerializeField] private float detectionCoolDown = 1f;
        public bool CanSeeTarget { get; private set; }

        public Transform BestTarget { get; private set; } = null;

        private void OnDrawGizmos()
        {
            // if (ShowGizmos)
            // {
            //     Gizmos.color = Color.white;
            //     Gizmos.DrawWireSphere(transform.position, DetectionRadius);

            //     Gizmos.color = Color.yellow;
            //     Gizmos.DrawWireSphere(transform.position, InnerDetectionRadius);
            // }
        }

        private void Start()
        {
            StartCoroutine(FOVRoutine());
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
            BestTarget = null;

            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, DetectionRadius, targetMask);

            if (rangeChecks.Length != 0)
            {
                foreach (Collider potentialTarget in rangeChecks)
                {
                    Transform target = potentialTarget.transform;
                    Debug.Log(target.transform.name);
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2 || distanceToTarget < InnerDetectionRadius)
                    {

                        Debug.DrawLine(transform.position, directionToTarget * distanceToTarget, Color.red, 2f);
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, directionToTarget, out hit, distanceToTarget, obstructionMask))
                        {
                            // Log the name of the obstruction
                            Debug.Log("Obstruction: " + hit.collider.gameObject.name);
                            CanSeeTarget = false;
                        }
                        else
                        {
                            CanSeeTarget = true;
                            BestTarget = target;
                            Debug.Log("I see the player");
                        }

                    }
                    else { CanSeeTarget = false; }
                }
            }
            else if (CanSeeTarget)
            {
                CanSeeTarget = false;
            }
        }

    }
}

