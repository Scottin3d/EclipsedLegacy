using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

namespace in3d.EL.GameLogic.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentTargetController : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        [SerializeField, Self] private Animator animator;
        [SerializeField] private float lookRotationSpeed = 8f;

        [SerializeField] private Rig spineRig;
        [Range(0, 1)]
        [SerializeField] private float spineWeight = 0.7f;
        [SerializeField] private Rig headRig;
        [Range(0, 1)]
        [SerializeField] private float headWeight = 1f;

        public bool HasTarget => navMeshAgent.destination != transform.position;

        private Queue<Vector3> targetQueue = new Queue<Vector3>();
        [SerializeField] private Transform lookAtRetarget;
        [SerializeField] private Transform lookAtTarget = null;
        [SerializeField] private float lookAtThreshold = 135f;
        private Vector3 lookDirection;

        void Update()
        {
            if (lookAtTarget != null)
            {
                lookAtRetarget.position = Vector3.Slerp(lookAtRetarget.position, lookAtTarget.position, 10f * Time.deltaTime);
            }
            else
            {
                lookAtRetarget.position = Vector3.Slerp(lookAtRetarget.position, transform.position + transform.forward * 10f, 10f * Time.deltaTime);
            }

            float lookAtAngle = Vector3.Angle(transform.forward, lookAtRetarget.position - transform.position);


            // TODO at priorty override to change target
            if (lookAtAngle > lookAtThreshold)
            {
                spineRig.weight = Mathf.Lerp(spineRig.weight, 0f, 10f * Time.deltaTime);
                headRig.weight = Mathf.Lerp(headRig.weight, 0f, 10f * Time.deltaTime);
            }
            else
            {
                spineRig.weight = Mathf.Lerp(spineRig.weight, spineWeight, 10f * Time.deltaTime);
                headRig.weight = Mathf.Lerp(headRig.weight, headWeight, 10f * Time.deltaTime);
            }

        }
        void LateUpdate()
        {
            if (HasTarget)
            {
                RotateToDestination();

                animator.SetFloat("LookDirectionX", lookDirection.x);
                animator.SetFloat("LookDirectionY", lookDirection.z);
            }
            else
            {
                RotateForward();
                animator.SetFloat("LookDirectionX", 0f);
                animator.SetFloat("LookDirectionY", 1f);
            }
        }

        public void SetLookAtTarget(Transform target = null)
        {
            lookAtTarget = target;
        }

        public void SetTarget(Vector3 target)
        {
            navMeshAgent.SetDestination(target);
        }

        public void AddTarget(Vector3 target)
        {
            targetQueue.Enqueue(target);
            if (!HasTarget)
            {
                SetTarget(targetQueue.Dequeue());
            }
        }

        public void ClearTarget()
        {
            navMeshAgent.ResetPath();
        }

        private void RotateToTarget()
        {
            lookDirection = (lookAtTarget.position - navMeshAgent.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }

        private void RotateToDestination()
        {
            lookDirection = (navMeshAgent.destination - navMeshAgent.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }

        private void RotateForward()
        {
            lookDirection = transform.forward;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * lookRotationSpeed);
        }
    }
}
