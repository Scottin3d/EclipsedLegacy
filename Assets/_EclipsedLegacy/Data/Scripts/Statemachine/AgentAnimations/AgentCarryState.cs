using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL.GameLogic.StateMachine.Agent
{
    public class AgentCarryState : AgentBaseLocomotionState {
       
        private PropHold propHold;
        public AgentCarryState(Animator animator, NavMeshAgent navMeshAgent, PropHold propHold) : base(animator, navMeshAgent) { 
            layer = AgentAnimationLayers.Carry;
            this.propHold = propHold;
        }

        public override void OnEnter()
        {
            animator.SetLayerWeight((int)layer, 1);
            propHold.PickUp();
        }

        public override void OnExit()
        {
            animator.SetLayerWeight((int)layer, 0);
            propHold.Drop();
        }
    }

}
