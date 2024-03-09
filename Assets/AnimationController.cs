using System.Collections;
using System.Collections.Generic;
using in3d.EL.GameLogic.StateMachine.Player;
using in3d.Utilities.GameLogic.StateMachine;
using in3d.Utilities.StateMachine.interfaces;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL
{
    public class AnimationController : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Animator animator;
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        private StateMachine stateMachine;
        public bool hasJob = false;
        public bool hasWeapon = true;

        void Start(){
            stateMachine = new StateMachine();
            var baseLocomotionState = new PlayerLocomotionState(animator, navMeshAgent);
            var AgentWeaponLocomotionState = new AgentWeaponLocomotionState(animator, navMeshAgent);
            var buildState = new PlayerBuildState(animator);

            Any(baseLocomotionState, new FuncPredicate(() => hasWeapon));
            Any(baseLocomotionState, new FuncPredicate(ReturnToBaseLocomotion));
            At(baseLocomotionState, buildState, new FuncPredicate(EnterBuildState));

            stateMachine.SetState(baseLocomotionState);
        }

        void Update()
        {
            stateMachine.Update();
            animator.SetBool("HasWeapon", hasWeapon);
            
        }
        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        private bool EnterBuildState(){
            return !navMeshAgent.hasPath && hasJob && !hasWeapon;
        }

        private bool ReturnToBaseLocomotion(){
            return !navMeshAgent.hasPath && !hasJob;
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    }
}
