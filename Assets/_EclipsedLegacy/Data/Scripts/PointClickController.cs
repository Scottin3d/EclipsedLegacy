using in3d.EL.Agent.Controllers;
using in3d.EL.GameLogic.AI;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

namespace in3d.EL.Player.Controllers
{

    public class PointClickController : ValidatedMonoBehaviour
    {
        
        [SerializeField, Self] private AnimationController animationController;
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        [SerializeField, Self] private AgentTargetController agentTargetController;
        PlayerInputController playerInputs;

        [SerializeField] private ParticleSystem clickEffect;
        [SerializeField] private LayerMask clickLayerMask;
        float lastClickTime = 0f;
        void Awake()
        {
            playerInputs = PlayerInputController.Instance;

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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000f, clickLayerMask))
            {
                if (Time.time - lastClickTime < 0.2f)
                {
                    navMeshAgent.speed = 7f;
                }
                else
                {
                    navMeshAgent.speed = 3.5f;
                }
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


