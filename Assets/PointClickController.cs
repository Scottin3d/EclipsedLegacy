
using in3d.EL.GameLogic.StateMachine.Player;
using in3d.Utilities.GameLogic.StateMachine;
using in3d.Utilities.StateMachine.interfaces;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL.Player.Controllers
{
    public class PointClickController : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Animator animator;
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        PlayerInputs playerInputs;

        [SerializeField] private ParticleSystem clickEffect;
        [SerializeField] private LayerMask clickLayerMask;

        private StateMachine stateMachine;

        void Awake(){
            playerInputs = new PlayerInputs();
            AssignInputActions();

        }
        private void Start()
        {
            stateMachine = new StateMachine();
            var idleState = new PlayerIdleState(animator);
            var walkState = new PlayerWalkState(animator, navMeshAgent);

            Any(idleState, new FuncPredicate(() => !navMeshAgent.hasPath));
            At(idleState, walkState, new FuncPredicate(() => navMeshAgent.hasPath));

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

        private void AssignInputActions()
        {
            playerInputs.PointClick.Move.performed += _ => OnClick();
        }

        private void OnClick()
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, clickLayerMask))
            {
                navMeshAgent.SetDestination(hit.point);
                if(clickEffect != null)
                {
                    Instantiate(clickEffect, hit.point += new Vector3(0f, 0.1f, 0f), Quaternion.identity);
                }
            }
        }

        void OnEnable()
        {
            playerInputs.Enable();
        }

        void OnDisable(){
            playerInputs.Disable();
        }

         void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    }
}


