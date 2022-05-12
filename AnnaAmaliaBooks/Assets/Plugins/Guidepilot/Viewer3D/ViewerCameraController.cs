#if GUIDEPILOT_CORE_VIEWER3D

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace com.guidepilot.guidepilotcore
{
    public class ViewerCameraController : MonoBehaviour
    {
        [BoxGroup("Settings")]
        public bool initializeOnStart = true;

        //[BoxGroup("Settings")]
        //[InlineEditor]
        //public ViewerCameraConfiguration configuration;

        [ToggleGroup("Settings/enableRotation", "Rotation")]
        [ToggleLeft]
        public bool enableRotation = true;

        [ToggleGroup("Settings/enableRotation")]
        public float rotationSpeed = 1.0f;

        [ToggleGroup("Settings/enablePan", "Pan")]
        [ToggleLeft]
        public bool enablePan = true;

        [ToggleGroup("Settings/enableZoom", "Zoom")]
        [ToggleLeft]
        public bool enableZoom = true;

        [ToggleGroup("Settings/enableZoom")]
        public float zoomSpeed = 0.1f;

        [ToggleGroup("Settings/enableFokusMode", "Fokus Mode")]
        //[BoxGroup("Settings/Input")]
        [ToggleLeft]
        public bool enableFokusMode = true;

        [ToggleGroup("Settings/enableFokusMode")]
        //[ShowIf("enableFokusMode")]
        [Range(0, 1)]
        public float fokusModeCameraDistance = 0.5f;

        //[BoxGroup("Settings")]
        //public bool enableFokusMode = true;
        //
        //[BoxGroup("Settings")]
        //[ShowIf("enableFokusMode")]
        //[Range(0, 1)]
        //public float fokusModeCameraDistance = 0.5f;

        [BoxGroup("Settings")]
        public ViewerCameraControllerTarget.CameraTransitionSettings cameraTransitionSettings;

        [BoxGroup("References")]
        [ShowInInspector]
        [ReadOnly]
        public Camera cam { get; private set; }

        [BoxGroup("References")]
        [SerializeField]
        private Transform rotationCenter;

        [BoxGroup("References")]
        [SerializeField]
        private Transform target;

        [BoxGroup("References")]
        [SerializeField]
        private ViewerCameraControllerTarget currentTarget;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private float minCamDistance;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private float maxCamDistance;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private Vector2 orbitAngles = Vector2.zero;

        [BoxGroup("Debug")]
        [ShowInInspector]
        private Vector3 rotation { get => (rotationCenter != null) ? rotationCenter.transform.rotation.eulerAngles : Vector3.zero; }

        private Vector3 cameraTargetScreenPosition;
        private Vector3 dragScreenOffset;

        private ViewerCameraControllerTarget.CameraAngleRestrictions cameraAngleRestrictions;
        //private ViewerCameraControllerTarget.CameraTransitionSettings cameraTransitionSettings;

        private void OnEnable()
        {
            BasicInputController.ev_drag_performed += Rotate;
            BasicInputController.ev_twofingerdrag_started += PanStart;
            BasicInputController.ev_twofingerdrag_performed += Pan;
            BasicInputController.ev_pinch_performed += Zoom;
            BasicInputController.ev_doubletap_performed += SetFokusPoint;

            ViewerCameraControllerTarget.OnTargetActivated += SetCameraTarget;

            if (!cam) cam = Camera.main;
        }

        private void OnDisable()
        {
            BasicInputController.ev_drag_performed -= Rotate;
            BasicInputController.ev_twofingerdrag_started -= PanStart;
            BasicInputController.ev_twofingerdrag_performed -= Pan;
            BasicInputController.ev_pinch_performed -= Zoom;
            BasicInputController.ev_doubletap_performed -= SetFokusPoint;

            ViewerCameraControllerTarget.OnTargetActivated -= SetCameraTarget;
        }

        private void Start()
        {
            if (initializeOnStart)
                Initialize();
        }

        public void Initialize()
        {


            if (!rotationCenter)
            {
                rotationCenter = new GameObject("RotationCenter").transform;
                rotationCenter.parent = this.transform;

                cam.transform.parent = rotationCenter.transform;
                cam.transform.localPosition = Vector3.zero;
                cam.transform.rotation = Quaternion.Euler(Vector3.zero);
            }


            //UpdateConfiguration(configuration);

            Debug.Log($"{typeof(ViewerCameraController).Name}: Initialization complete.");
        }

        //public void UpdateConfiguration(ViewerCameraConfiguration configuration)
        //{
        //    Debug.Log($"{typeof(ViewerCameraController).Name}: Updated Configuration.");
        //}

        #region PAN_CONTROL
        void PanStart(BasicInputController.InputEventData ev)
        {
            cameraTargetScreenPosition = cam.WorldToScreenPoint(currentTarget.transform.position);

            Vector3 currentScreenPoint = new Vector3(ev.pointerPosition.x, ev.pointerPosition.y, cameraTargetScreenPosition.z);

            dragScreenOffset = cameraTargetScreenPosition - currentScreenPoint;
        }
        
        public void Pan(BasicInputController.InputEventData ev)
        {
            if (!enablePan) return;

            Vector3 currentScreenPoint = new Vector3(ev.pointerPosition.x, ev.pointerPosition.y, cameraTargetScreenPosition.z);
            Vector3 currentWorldPoint = cam.ScreenToWorldPoint(currentScreenPoint + dragScreenOffset);

            Vector3 movementVector = -currentWorldPoint;

            cam.transform.position += movementVector;

            //IsTargetOutOfView();
        }
        #endregion

        #region ROTATION_CONTROL
        public void Rotate(BasicInputController.InputEventData ev)
        {
            if (!enableRotation) return;

            Vector2 rotationVector = new Vector2(ev.deltaPosition.y, -ev.deltaPosition.x);

            orbitAngles += rotationSpeed * rotationVector * Time.unscaledDeltaTime;

            ConstrainAngles();

            Quaternion lookRotation = Quaternion.Euler(orbitAngles);

            rotationCenter.transform.rotation = lookRotation;

            //rotationCenter.transform.Rotate(Vector3.right, direction.y * rotationSpeed, Space.Self);
            //rotationCenter.transform.Rotate(Vector3.up, -direction.x * rotationSpeed, Space.World);
        }

        private void ConstrainAngles()
        {
            if (cameraAngleRestrictions == null) return;

            // apply contraints
            if (cameraAngleRestrictions.constrainVerticalAngles)
            {
                // work with values [-180 , 180]
                if (orbitAngles.x < -180 || orbitAngles.x > 180)
                    orbitAngles.x = 360 - orbitAngles.x;

                // clamp
                float clampedVerticalAngle = Mathf.Clamp(orbitAngles.x, -cameraAngleRestrictions.minVerticalAngle, cameraAngleRestrictions.maxVerticalAngle);
                orbitAngles.x = clampedVerticalAngle;
            }

            if (cameraAngleRestrictions.constrainHorizontalAngles)
            {
                float minHorizontalAngle = (cameraAngleRestrictions.inverse) ? cameraAngleRestrictions.minHorizontalAngle : cameraAngleRestrictions.minHorizontalAngle - 180f;
                float maxHorizontalAngle = (cameraAngleRestrictions.inverse) ? cameraAngleRestrictions.maxHorizontalAngle : cameraAngleRestrictions.maxHorizontalAngle + 180f;

                float clampedHorizontalAngle = Mathf.Clamp(orbitAngles.y, -minHorizontalAngle, maxHorizontalAngle);
                orbitAngles.y = clampedHorizontalAngle;
            }
        }
        #endregion

        #region ZOOM_CONTROL
        public void Zoom(BasicInputController.InputEventData ev)
        {
            if (!enableZoom) return;
            //print(deltaMagnitudeDiff * zoomSpeed * Time.unscaledDeltaTime);
            // calculate new cam position
            Vector3 zValue = cam.transform.position + cam.transform.forward * ev.deltaDistance * zoomSpeed * Time.unscaledDeltaTime;

            // update position only if in between min-max-camdistance
            float dist = Vector3.Distance(zValue, rotationCenter.transform.position);
            if (dist >= minCamDistance && dist <= maxCamDistance)
            {
                cam.transform.position = zValue;
            }
        }
        #endregion

        public void SetPositionAndRotation(Vector3 centerPosition, Quaternion centerRotation, Vector3 camPosition, ViewerCameraControllerTarget.CameraTransitionSettings cameraTransitionSettings)
        {
            if (DOTween.IsTweening(cam.transform))
                DOTween.Kill(cam.transform);

            cam.transform.DOLocalMove(camPosition, cameraTransitionSettings.moveDuration).SetEase(cameraTransitionSettings.moveEase);

            if (DOTween.IsTweening(rotationCenter.transform))
                DOTween.Kill(rotationCenter.transform);

            rotationCenter.transform.DOMove(centerPosition, cameraTransitionSettings.moveDuration).SetEase(cameraTransitionSettings.moveEase);
            rotationCenter.transform.DORotate(centerRotation.eulerAngles, cameraTransitionSettings.rotationDuration).SetEase(cameraTransitionSettings.rotationEase).OnUpdate(() => orbitAngles = rotationCenter.transform.rotation.eulerAngles);

            Debug.Log($"{typeof(ViewerCameraController).Name}: Set position and rotation to: CENTER POSITION: {centerPosition} | CENTER ROTATION: {centerRotation.eulerAngles} | CAM POSITION: {camPosition}");
        }

        [Button(ButtonSizes.Large, ButtonStyle.CompactBox)]
        public void ResetPositionAndRotation(ViewerCameraControllerTarget.CameraTransitionSettings cameraTransitionSettings = null)
        {
            if (currentTarget == null) return;

            if (cameraTransitionSettings == null)
                cameraTransitionSettings = this.cameraTransitionSettings;

            CalculateInitialPositionAndRotation(currentTarget, out Vector3 centerPosition, out Quaternion centerRotation, out Vector3 camPosition);
            SetPositionAndRotation(centerPosition, centerRotation, camPosition, cameraTransitionSettings);
        }

        public void SetCameraTarget(ViewerCameraControllerTarget target)
        {
            if (currentTarget == target || target == null) return;

            currentTarget = target;
            cameraAngleRestrictions = target.cameraAngleRestrictions;
            //cameraTransitionSettings = target.cameraTransitionSettings;

            ResetPositionAndRotation(target.cameraTransitionSettings);

            Debug.Log($"{typeof(ViewerCameraController).Name}: Set camera target to: {target.name}");
        }

        public void SetFokusPoint(BasicInputController.InputEventData ev)
        {
            Ray ray = cam.ScreenPointToRay(ev.pointerPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 direction = (hit.point - cam.transform.position).normalized;

                // point camera at hit.point, avg. of min-max-distance
                PointAt(hit.point, direction, Mathf.Lerp(minCamDistance, maxCamDistance, fokusModeCameraDistance));
            }
            else
            {
                ResetPositionAndRotation(cameraTransitionSettings);
            }
        }

        public void PointAt(Vector3 point, Vector3 direction, float distance)
        {
            Vector3 rotationCenterPosition = point;
            Quaternion rotationCenterRotation = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 camPosition = Vector3.back * distance;

            // clamp angles
            orbitAngles = rotationCenterRotation.eulerAngles;
            ConstrainAngles();
            rotationCenterRotation = Quaternion.Euler(orbitAngles);

            SetPositionAndRotation(rotationCenterPosition, rotationCenterRotation, camPosition, cameraTransitionSettings);
        }

        private void CalculateInitialPositionAndRotation(ViewerCameraControllerTarget target, out Vector3 centerPosition, out Quaternion centerRotation, out Vector3 camPosition)
        {
            Bounds bounds = target.rendererBounds;

            //Debug.Log("Center: " + bounds.center + "\n Extents: " + bounds.extents + "\n Size: " + bounds.size + "Max: " + bounds.max + "\n Min: " + bounds.min);

            float maxExtent = bounds.extents.magnitude;
            maxCamDistance = (maxExtent * target.distanceToScreenBorder) / Mathf.Sin(Mathf.Deg2Rad * cam.fieldOfView / 2.0f);

            minCamDistance = maxExtent * 0.5f;
            //maxCamDistance = maxExtent * 1.5f;

            camPosition = Vector3.back * maxCamDistance;
            centerPosition = bounds.center;
            centerRotation = Quaternion.LookRotation(target.transform.position - target.initialCameraPosition);

            //Debug.DrawRay(target.initialCameraPosition, target.transform.position - target.initialCameraPosition, Color.red, 2.0f);
            //cam.transform.LookAt(rotationCenter.position);
        }

        private bool IsTargetOutOfView()
        {
            Vector3 pos = cam.WorldToViewportPoint(target.transform.position);
            bool value = (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1);
            return value;
        }
    }

}


#endif
