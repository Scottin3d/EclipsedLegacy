
using System;
using System.Threading.Tasks;
using in3d.Utilities.StateMachine.interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL.GameLogic.StateMachine.Agent
{
    public abstract class AgentBaseState : IState
    {
        protected readonly Animator animator;
        protected int baseLocomotionHash;
        protected const float crossFadeDuration = 0.1f;

        protected AgentBaseState(Animator animator)
        {
            this.animator = animator;

            AssignAnimationIDs();
        }

        private void AssignAnimationIDs()
        {
            baseLocomotionHash = Animator.StringToHash("locomotion");
        }

        public virtual void FixedUpdate() { }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void Update(){}
    }

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

    public class AgentWeaponLocomotionState: AgentBaseLocomotionState{
        public AgentWeaponLocomotionState(Animator animator, NavMeshAgent navMeshAgent) : base(animator, navMeshAgent) { }
        public override void OnEnter()
        {
            animator.SetLayerWeight(2, 1);
            animator.SetBool("HasWeapon", true);
            Debug.Log("Entering Weapon Locomotion State");
        }

        public override void OnExit()
        {
            animator.SetLayerWeight(2, 0);
            animator.SetBool("HasWeapon", false);
        }
    }

    public class AgentBuildState: AgentBaseState
    {
        public AgentBuildState(Animator animator) : base(animator) { }

        public override void OnEnter()
        {
            animator.SetFloat("Speed", 0f);
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("Build");
            animator.SetBool("IsBuilding", true);
            Debug.Log("Entering Build State");
        }

        public override void OnExit()
        {
            animator.SetLayerWeight(1, 0);
            animator.SetBool("IsBuilding", false);
        }

    }

}
