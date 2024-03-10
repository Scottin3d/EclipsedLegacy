using in3d.Utilities.GameLogic.Detection;
using UnityEngine;

namespace in3d.EL.Agent.Controllers
{
    public class FirearmController : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform currentTarget = null;
        // public bool HasTarget => currentTarget != null;
        public bool HasTarget = true;

        public float fireRate = 60f; // per minute
        private float timeSinceLastShot = 0f;
        void OnEnable(){
            FieldOfView.TargetDetected += SetTarget;
        }

        void OnDisable(){
            FieldOfView.TargetDetected -= SetTarget;
        }

        private void SetTarget(Transform source, Transform target)
        {
            if(source != transform) return;

            // check tag for list of targets
            currentTarget = target;
        }

        void Update(){
            if(currentTarget != null){
                timeSinceLastShot += Time.deltaTime;
                if(timeSinceLastShot >= 60f / fireRate){
                    FireWeapon();
                    timeSinceLastShot = 0f;
                }
            }
        }

        private void FireWeapon()
        {
            // raycast from firepoint to target
            Vector3 direction = firePoint.position - currentTarget.position;
            Ray ray = new Ray(firePoint.position, direction);
            RaycastHit hit;
            Debug.DrawLine(firePoint.position, currentTarget.position, Color.red, 2f);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // check if hit is target
                if (hit.transform == currentTarget)
                {
                    // damage target
                }
            }
        }
    }
}
