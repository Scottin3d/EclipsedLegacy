using UnityEngine;

namespace in3d.EL.GameLogic.StateMachine.Agent
{
    public class AgentBuildState: AgentBaseState
    {
        public AgentBuildState(Animator animator) : base(animator) { 
            layer = AgentAnimationLayers.Build;
        }

        public override void OnEnter()
        {
            animator.SetFloat("Speed", 0f);
            animator.SetLayerWeight((int)layer, 1);
            animator.SetTrigger("Build");
            animator.SetBool("IsBuilding", true);
            Debug.Log("Entering Build State");
        }

        public override void OnExit()
        {
            animator.SetLayerWeight((int)layer, 0);
            animator.SetBool("IsBuilding", false);
        }

    }

}
