
using UnityEngine;
using UnityEditor;
using in3d.Utilities.GameLogic.Detection;

namespace in3d.Utilities.GameLogic.EditorScripts
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fov = (FieldOfView)target;
            if (fov.ShowGizmos)
            {

                Handles.color = Color.white;
                // outer radius
                Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.DetectionRadius);
                if(fov.gizmoType == GizmoDisplayType.d3){
                    Handles.DrawWireDisc(fov.transform.position, Vector3.right, fov.DetectionRadius);
                    Handles.DrawWireDisc(fov.transform.position, Vector3.forward, fov.DetectionRadius);
                }

                // inner radius
                Handles.color = Color.yellow;
                Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.InnerDetectionRadius);
                if(fov.gizmoType == GizmoDisplayType.d3){
                    Handles.DrawWireDisc(fov.transform.position, Vector3.right, fov.InnerDetectionRadius);
                    Handles.DrawWireDisc(fov.transform.position, Vector3.forward, fov.InnerDetectionRadius);
                }

                Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.Angle / 2);
                Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.Angle / 2);

                Handles.color = Color.yellow;
                Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.DetectionRadius);
                Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.DetectionRadius);

                if (fov.CanSeeTarget && fov.BestTarget != null)
                {
                    Handles.DrawLine(fov.transform.position, fov.BestTarget.position);
                }
            }
        }

        private Vector3 DirectionFromAngle(float eulerV, float angleInDegrees)
        {
            angleInDegrees += eulerV;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}