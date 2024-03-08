
using System;
using in3d.EL.GameLogic.StateMachine.Player;
using in3d.Utilities.GameLogic.StateMachine;
using in3d.Utilities.StateMachine.interfaces;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace in3d.EL.Player.Controllers
{

    public class PointClickController : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Animator animator;
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        PlayerInputController playerInputs;

        [SerializeField] private ParticleSystem clickEffect;
        [SerializeField] private LayerMask clickLayerMask;
        float lastClickTime = 0f;

        private StateMachine stateMachine;

        void Awake()
        {
            playerInputs = PlayerInputController.Instance;

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

            HandleClick();
        }
        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        private void HandleClick()
        {
            if (playerInputs.Click) OnClick();
        }


        private void OnClick()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, clickLayerMask))
            {
                if (Time.time - lastClickTime < 0.2f)
                {
                    navMeshAgent.speed = 7f;
                }
                navMeshAgent.SetDestination(hit.point);
                if (clickEffect != null)
                {
                    Instantiate(clickEffect, hit.point += new Vector3(0f, 0.1f, 0f), Quaternion.identity);
                }
            }
            lastClickTime = Time.time;
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    }
}


