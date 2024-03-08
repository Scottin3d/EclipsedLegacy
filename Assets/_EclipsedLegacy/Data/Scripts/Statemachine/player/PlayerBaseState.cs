
using System;
using System.Threading.Tasks;
using in3d.Utilities.StateMachine.interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL.GameLogic.StateMachine.Player
{
    public abstract class PlayerBaseState : IState
    {
        protected readonly Animator animator;
        protected int idlAnimationHash;
        protected int walkAnimationHash;
        protected int buildAnimationHash;
        protected const float crossFadeDuration = 0.1f;

        protected PlayerBaseState(Animator animator)
        {
            this.animator = animator;

            AssignAnimationIDs();
        }

        private void AssignAnimationIDs()
        {
            idlAnimationHash = Animator.StringToHash("idle");
            walkAnimationHash = Animator.StringToHash("locomotion");
            buildAnimationHash = Animator.StringToHash("build");
        }

        public virtual void FixedUpdate() { }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void Update(){}
    }

    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(Animator animator) : base(animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(idlAnimationHash, crossFadeDuration);
            animator.SetFloat("Speed", 0f);
        }
    }

    public class PlayerLocomotionState : PlayerBaseState
    {
        private readonly NavMeshAgent navMeshAgent;
        private Transform agent;
        private float lookRotationSpeed = 8f;

        public PlayerLocomotionState(Animator animator, NavMeshAgent navMeshAgent) : base(animator)
        {
            this.navMeshAgent = navMeshAgent;
            agent = navMeshAgent.transform;
        }

        public override void OnEnter()
        {
            animator.CrossFade(walkAnimationHash, 0.01f);
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
                navMeshAgent.speed = 3.5f;
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

    public class PlayerBuildState: PlayerBaseState
    {
        public PlayerBuildState(Animator animator) : base(animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(buildAnimationHash, crossFadeDuration);
            animator.SetFloat("Speed", 0f);
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("Build");
            animator.SetBool("IsBuilding", true);
        }

        public override void OnExit()
        {
            animator.SetLayerWeight(1, 0);
            animator.SetBool("IsBuilding", false);
        }

    }

}
