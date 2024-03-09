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

        void Start(){
            stateMachine = new StateMachine();
            var idleState = new PlayerIdleState(animator);
            var walkState = new PlayerLocomotionState(animator, navMeshAgent);
            var buildState = new PlayerBuildState(animator);

            Any(idleState, new FuncPredicate(ReturnToIdleState));
            At(idleState, walkState, new FuncPredicate(() => navMeshAgent.hasPath));
            At(walkState, buildState, new FuncPredicate(EnterBuildState));

            stateMachine.SetState(idleState);
        }

        void Update()
        {
            stateMachine.Update();
        }
        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        private bool EnterBuildState(){
            return !navMeshAgent.hasPath && hasJob;
        }

        private bool ReturnToIdleState(){
            return !navMeshAgent.hasPath && !hasJob;
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    }
}
