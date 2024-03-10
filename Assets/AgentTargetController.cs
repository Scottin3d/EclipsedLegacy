using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL.GameLogic.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentTargetController : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        [SerializeField, Self] private Animator animator;
        [SerializeField] private float lookRotationSpeed = 8f;

        public bool HasTarget => navMeshAgent.destination != transform.position;

        private Queue<Vector3> targetQueue = new Queue<Vector3>();
         [SerializeField] private Transform lookAtTarget = null;
        [SerializeField] private Vector3 lookDirection;
        [SerializeField] private Vector3 destination;

        void LateUpdate()
        {
            destination = navMeshAgent.destination;
            if (HasTarget)
            {
                if (lookAtTarget != null)
                {
                    RotateToTarget();
                }
                else
                {
                    RotateToDestination();
                }

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
