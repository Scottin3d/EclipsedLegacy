using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL.GameLogic.StateMachine.Agent
{
    public class AgentBaseLocomotionState : AgentBaseState
    {
        private readonly NavMeshAgent navMeshAgent;
        private Transform agent;
        private float lookRotationSpeed = 8f;

        public AgentBaseLocomotionState(Animator animator, NavMeshAgent navMeshAgent) : base(animator)
        {
            this.navMeshAgent = navMeshAgent;
            agent = navMeshAgent.transform;
        }

        public override void OnEnter()
        {
            animator.CrossFade(baseLocomotionHash, 0.01f);
            Debug.Log("Entering Locomotion State");
        }

        public override void Update()
        {
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }

        public override void FixedUpdate()
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.ResetPath();
            }

            RotateToDestination();
        }

        private void RotateToDestination()
        {
            Vector3 direction = (navMeshAgent.destination - agent.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            agent.rotation = Quaternion.Slerp(agent.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
    }

}
