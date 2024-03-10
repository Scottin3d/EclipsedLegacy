using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

namespace in3d.EL.Agent.Controllers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class WeaponAnimationController : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        [SerializeField] private MultiAimConstraint weaponLookAtConstraint;

        // Update is called once per frame
        void Update()
        {
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
    }
}
