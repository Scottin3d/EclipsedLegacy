using in3d.EL.GameLogic.StateMachine.Agent;
using in3d.Utilities.GameLogic.StateMachine;
using in3d.Utilities.StateMachine.interfaces;
using KBCore.Refs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

namespace in3d.EL
{
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(PropHold))]
    public class AnimationController : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Animator animator;
        [SerializeField, Self] private PropHold propHold;
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        [SerializeField] private MultiAimConstraint weaponLookAtConstraint;
        [SerializeField] private Rig weaponLookAtRig;
        private StateMachine stateMachine;
        public bool hasJob = false;
        public bool hasWeapon = false;
        public bool isCarrying = false;

        void Start(){
            stateMachine = new StateMachine();
            var baseLocomotionState = new AgentBaseLocomotionState(animator, navMeshAgent);
            var AgentWeaponLocomotionState = new AgentWeaponLocomotionState(animator, navMeshAgent);
            var buildState = new AgentBuildState(animator);
            var carryState = new AgentCarryState(animator, navMeshAgent, propHold);


            Any(AgentWeaponLocomotionState, new FuncPredicate(ReturnToWeaponLocomotion));
            Any(baseLocomotionState, new FuncPredicate(ReturnToBaseLocomotion));
            Any(carryState, new FuncPredicate(() => isCarrying));
            At(baseLocomotionState, buildState, new FuncPredicate(EnterBuildState));
            

            stateMachine.SetState(baseLocomotionState);
        }

        void Update()
        {
            stateMachine.Update();
            animator.SetBool("HasWeapon", hasWeapon);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            Debug.Log("Path:"+ navMeshAgent.pathStatus);

            // set the rig weight of weaponLookAtRig if the agent velcoity is greater than 0
            // float egressWeight = math.lerp(1f, 0f, 5f * Time.deltaTime);
            // float ingressWeight = math.lerp(0f, 1f, 5f * Time.deltaTime);
            if (navMeshAgent.velocity.magnitude > 0)
            {
                // weaponLookAtRig.weight = 1f;
                var data = weaponLookAtConstraint.data.sourceObjects;
                data.SetWeight(0, 1f);
                data.SetWeight(1, 0f);
                weaponLookAtConstraint.data.sourceObjects = data;
                
            }
            else
            {
                // weaponLookAtRig.weight = 0f;
                var data = weaponLookAtConstraint.data.sourceObjects;
                data.SetWeight(0, 0f);
                data.SetWeight(1, 1f);
                weaponLookAtConstraint.data.sourceObjects = data;
            }
        }
        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        private bool EnterBuildState(){
            return hasJob;
        }

        private bool ReturnToBaseLocomotion(){
            return !hasJob && !hasWeapon && !isCarrying;
        }

        private bool ReturnToWeaponLocomotion(){
            return hasWeapon;
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    }
}
