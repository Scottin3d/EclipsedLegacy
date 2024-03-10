using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL.GameLogic.StateMachine.Agent
{
    public class AgentBaseLocomotionState : AgentBaseState
    {
        private readonly NavMeshAgent navMeshAgent;

        public AgentBaseLocomotionState(Animator animator, NavMeshAgent navMeshAgent) : base(animator)
        {
            this.navMeshAgent = navMeshAgent;
        }

        public override void OnEnter()
        {
            animator.CrossFade(baseLocomotionHash, 0.01f);
            Debug.Log("Entering Locomotion State");
        }

        public override void FixedUpdate()
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.ResetPath();
            }
        }
    }

}
