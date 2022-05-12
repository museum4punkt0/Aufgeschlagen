#if GUIDEPILOT_CORE_VIEWER3D

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace com.guidepilot.guidepilotcore
{
    [ExecuteInEditMode]
    public class ViewerCameraControllerTarget : MonoBehaviour
    {
        [System.Serializable]
        public class CameraAngleRestrictions
        {
            public bool inverse = false;
            public bool constrainVerticalAngles = false;

            [ShowIf("constrainVerticalAngles")]
            [Range(0, 90f)]
            public float minVerticalAngle = 45f;

            [ShowIf("constrainVerticalAngles")]
            [Range(0, 90f)]
            public float maxVerticalAngle = 45f;

            public bool constrainHorizontalAngles = false;

            [ShowIf("constrainHorizontalAngles")]
            [Range(0f, 180f)]
            public float minHorizontalAngle = 90f;

            [ShowIf("constrainHorizontalAngles")]
            [Range(0f, 180f)]
            public float maxHorizontalAngle = 90f;
        }

        [System.Serializable]
        public class CameraTransitionSettings
        {
            public float moveDuration = 1.0f;
            public Ease moveEase = Ease.OutSine;
            public float rotationDuration = 1.0f;
            public Ease rotationEase = Ease.OutSine;
        }

        public enum BoundsCalculation { Renderer, Collider };

        public static UnityAction<ViewerCameraControllerTarget> OnTargetActivated;

        [BoxGroup("Settings")]
        public bool activateOnStart = false;

        

        [BoxGroup("Settings")]
        public CameraAngleRestrictions cameraAngleRestrictions;

        [BoxGroup("Settings")]
        public CameraTransitionSettings cameraTransitionSettings;

        [BoxGroup("Settings")]
        //[OnValueChanged("CalculateInitialCameraPosition")]
        public Vector2 initialCameraAngle;

        [BoxGroup("Settings")]
        [Range(1, 2)]
        public float distanceToScreenBorder = 1.5f;

        [BoxGroup("Settings")]
        [EnumToggleButtons]
        public BoundsCalculation boundsCalculation;

        [BoxGroup("Settings")]
        [ShowIf("boundsCalculation", BoundsCalculation.Collider)]
        public new Collider collider;

        [BoxGroup("Settings")]
        public bool usePhysics;

        [BoxGroup("Settings")]
        [ShowIf("usePhysics")]
        public ForceMode forceMode;

        [BoxGroup("Debug")]
        [ShowInInspector]
        [ReadOnly]
        public Bounds rendererBounds { get => GetBounds(); }

        [BoxGroup("Debug")]
        [ShowInInspector]
        [ReadOnly]
        public Vector3 initialCameraPosition
        {
            get
            {
                SphericalToCartesian(1.0f, Mathf.Deg2Rad * initialCameraAngle.x, Mathf.Deg2Rad * initialCameraAngle.y, out Vector3 sphericalPosition);
                return sphericalPosition + transform.position;
            }
        }

        //private BoxCollider boxCollider = null;

        [BoxGroup("Debug")]
        [ShowIf("usePhysics")]
        [ShowInInspector]
        [ReadOnly]
        public new Rigidbody rigidbody { get; private set; }

        private void Awake()
        {
            if (usePhysics)
            {
                this.rigidbody = (TryGetComponent(out Rigidbody rigidbody)) ? rigidbody : gameObject.AddComponent<Rigidbody>();
                this.rigidbody.useGravity = false;
                this.rigidbody.isKinematic = false;
            }
        }

        private void Start()
        {
            if (activateOnStart)
                ActivateTarget();
        }

        [Button]
        public void ActivateTarget()
        {
            OnTargetActivated?.Invoke(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(initialCameraPosition, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(rendererBounds.center, rendererBounds.size);
        }

        private void SphericalToCartesian(float radius, float polar, float elevation, out Vector3 outCart)
        {
            float a = radius * Mathf.Cos(elevation);
            outCart.x = a * Mathf.Cos(polar);
            outCart.y = radius * Mathf.Sin(elevation);
            outCart.z = a * Mathf.Sin(polar);
        }

        private Bounds GetBounds()
        {
            Bounds bounds = new Bounds(transform.position, Vector3.zero);

            switch (boundsCalculation)
            {
                case BoundsCalculation.Renderer:

                    foreach (Renderer r in GetComponentsInChildren<Renderer>())
                    {
                        bounds.Encapsulate(r.bounds);
                    }

                    break;
                case BoundsCalculation.Collider:

                    if (collider != null)
                        bounds = collider.bounds;

                    break;
                default:
                    break;
            }

            return bounds;
        }
    }
}

#endif
