using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL.GameLogic.StateMachine.Agent
{
    public class AgentWeaponLocomotionState: AgentBaseLocomotionState{
        public AgentWeaponLocomotionState(Animator animator, NavMeshAgent navMeshAgent) : base(animator, navMeshAgent) {
            layer = AgentAnimationLayers.Weapon;
         }
        public override void OnEnter()
        {
            animator.SetLayerWeight((int)layer, 1);
            animator.SetBool("HasWeapon", true);
            Debug.Log("Entering Weapon Locomotion State");
        }

        public override void OnExit()
        {
            animator.SetLayerWeight((int)layer, 0);
            animator.SetBool("HasWeapon", false);
        }
    }

}
