
using System;
using System.Threading.Tasks;
using in3d.Utilities.StateMachine.interfaces;
using UnityEngine;

namespace in3d.EL.GameLogic.StateMachine.Agent
{
    public enum AgentAnimationLayers
    {
        Locomotion = 0,
        Carry = 1,
        Weapon = 2,
        Build = 3
    }
    public abstract class AgentBaseState : IState
    {
        protected readonly Animator animator;
        protected AgentAnimationLayers layer = AgentAnimationLayers.Locomotion;
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

}
