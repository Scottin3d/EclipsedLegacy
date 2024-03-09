
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
        [SerializeField, Self] private AnimationController animationController;
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        PlayerInputController playerInputs;

        [SerializeField] private ParticleSystem clickEffect;
        [SerializeField] private LayerMask clickLayerMask;
        float lastClickTime = 0f;
        void Awake()
        {
            playerInputs = PlayerInputController.Instance;

        }
        private void Start()
        {
        }

        void Update()
        {

            HandleClick();
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
                navMeshAgent.speed = 3.5f;
                navMeshAgent.SetDestination(hit.point);
                if (clickEffect != null)
                {
                    Instantiate(clickEffect, hit.point += new Vector3(0f, 0.1f, 0f), Quaternion.identity);
                }
            }
            lastClickTime = Time.time;
        }
    }
}


