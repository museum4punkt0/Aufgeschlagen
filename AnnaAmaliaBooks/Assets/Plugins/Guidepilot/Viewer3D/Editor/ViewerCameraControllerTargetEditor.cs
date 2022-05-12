#if GUIDEPILOT_CORE_VIEWER3D

using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace com.guidepilot.guidepilotcore
{
    [CustomEditor(typeof(ViewerCameraControllerTarget))]
    public class ViewerCameraControllerTargetEditor : OdinEditor
    {
        private float ArcSize = 1.0f;

        private void OnSceneGUI()
        {
            ViewerCameraControllerTarget cameraTarget = (ViewerCameraControllerTarget)target;

            float[] boundsSizes = new float[] { cameraTarget.rendererBounds.size.x, cameraTarget.rendererBounds.size.y, cameraTarget.rendererBounds.size.z };
            ArcSize = Mathf.Max(boundsSizes);

            if (!Application.isPlaying)
            {
                Vector3 forwardDirection = (cameraTarget.cameraAngleRestrictions.inverse) ? -cameraTarget.transform.forward : cameraTarget.transform.forward;

                Handles.color = new Color(1.0f, 0.5f, 0.5f, 0.1f);
                if (cameraTarget.cameraAngleRestrictions.constrainHorizontalAngles)
                {
                    Handles.DrawSolidArc(cameraTarget.transform.position, cameraTarget.transform.up, forwardDirection, -cameraTarget.cameraAngleRestrictions.minHorizontalAngle, ArcSize);
                    Handles.DrawSolidArc(cameraTarget.transform.position, cameraTarget.transform.up, forwardDirection, cameraTarget.cameraAngleRestrictions.maxHorizontalAngle, ArcSize);
                }
                else
                {
                    Handles.DrawSolidArc(cameraTarget.transform.position, cameraTarget.transform.up, forwardDirection, 360f, ArcSize);
                }

                Handles.color = new Color(0.5f, 1.0f, 0.5f, 0.1f);
                if (cameraTarget.cameraAngleRestrictions.constrainVerticalAngles)
                {
                    Handles.DrawSolidArc(cameraTarget.transform.position, cameraTarget.transform.right, forwardDirection, -cameraTarget.cameraAngleRestrictions.minVerticalAngle, ArcSize);
                    Handles.DrawSolidArc(cameraTarget.transform.position, cameraTarget.transform.right, forwardDirection, cameraTarget.cameraAngleRestrictions.maxVerticalAngle, ArcSize);
                }
                else
                {
                    Handles.DrawSolidArc(cameraTarget.transform.position, cameraTarget.transform.right, forwardDirection, 360f, ArcSize);
                }
            }
        }
    }
}

#endif


