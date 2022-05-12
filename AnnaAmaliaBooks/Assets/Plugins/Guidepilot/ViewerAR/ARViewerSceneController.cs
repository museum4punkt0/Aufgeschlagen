#if GUIDEPILOT_CORE_AR

using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

namespace com.guidepilot.guidepilotcore
{
    public class ARViewerSceneController : ViewerSceneControllerBase<ARViewerSceneController, ARViewerConfiguration>
    {
        //[BoxGroup("Settings")]
        //[SerializeField]
        //private bool initializeOnStart = true;

        //[BoxGroup("Settings")]
        //[InlineEditor]
        //[SerializeField]
        //private ARViewerConfiguration configuration;

        public ARSession Session => session;
        public ARSessionOrigin SessionOrigin => sessionOrigin;

        [BoxGroup("References")]
        [InlineEditor]
        [SerializeField]
        private GameObject ARPlanePrefab;

        public static UnityAction<ARTrackable> OnTrackingAddedEvent;
        public static UnityAction<ARTrackable> OnTrackingRemovedEvent;
        public static UnityAction<ARTrackable> OnTrackingFoundEvent;
        public static UnityAction<ARTrackable> OnTrackingLostEvent;
        public static UnityAction<ARTrackedImage> OnTrackedImageTrackingEvent;
        public static UnityAction<ARPlanesChangedEventArgs> OnARPlanesChangedEvent;
        public static UnityAction<ARPlanesChangedEventArgs> OnARPlanesFoundEvent;
        //public static UnityAction<UnityContent> OnContentUpdatedEvent;
        //public static UnityAction<ARViewerSceneController> OnAwakeEvent;
        //public static UnityAction<ARViewerSceneController> OnDestroyEvent;
        //public static UnityAction<ARViewerSceneController> OnInitializationCompleteEvent;

        private Dictionary<ARTrackedImage, LineRenderer> lineRendererDictionary = new Dictionary<ARTrackedImage, LineRenderer>();
        private Dictionary<ARTrackable, TrackingState> trackingStateDictionary = new Dictionary<ARTrackable, TrackingState>();

        [BoxGroup("Debug")]
        [SerializeField]
        private bool debug = false;

        [BoxGroup("Debug")]
        [ShowIf("debug")]
        [SerializeField]
        private GameObject debugObject = null;

        [BoxGroup("Debug")]
        [ShowIf("debug")]
        [SerializeField]
        private Material debugLineMaterial = null;

        [BoxGroup("Debug")]
        [ShowIf("debug")]
        [SerializeField]
        private float debugLineWidth = 0.01f;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private ARSession session;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private ARSessionOrigin sessionOrigin;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private Camera arCamera;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private ARTrackedImageManager trackedImageManager;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private ARPlaneManager planeManager;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private ARRaycastManager raycastManager;

        private GameObject instantiatedGameObject;
        private Transform originTransform;

        //private void Awake()
        //{
        //    ViewerScene.OnLoadEvent += LoadViewerScene;
        //    ViewerScene.OnUnloadEvent += UnloadViewerScene;
        //
        //    OnAwakeEvent?.Invoke(this);
        //}
        //
        protected override void OnDestroy()
        {
            if (trackedImageManager != null)
                trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

            base.OnDestroy();
        }

        //private void Start()
        //{
        //    if (initializeOnStart)
        //        Initialize(configuration);
        //}

        public override void Initialize(ARViewerConfiguration configuration)
        {
            if (configuration == null)
            {
                Debug.LogWarning($"{typeof(ARViewerSceneController).Name}: Can't initialize because configuration is null");
                return;
            }

            this.configuration = configuration;

            if (!SetupIsValid()) return;

            switch (configuration.trackingMode)
            {
                case ARViewerConfiguration.TrackingMode.SurfaceTracking:
                    InitializeSurfaceTracking();
                    break;
                case ARViewerConfiguration.TrackingMode.ImageTracking:
                    InitializeImageTracking();
                    break;
                default:
                    break;
            }

            base.Initialize(configuration);
        }

        //public void UpdateContent(UnityContent content)
        //{
        //    if (content == null) return;
        //
        //    currentContent = content;
        //
        //    OnContentUpdatedEvent?.Invoke(content);
        //}

        private bool SetupIsValid()
        {
            if (session == null)
                session = FindObjectOfType<ARSession>();

            if (sessionOrigin == null)
                sessionOrigin = FindObjectOfType<ARSessionOrigin>();

            originTransform = sessionOrigin.transform;

            if (arCamera == null)
            {
                ARCameraManager arCameraManager = FindObjectOfType<ARCameraManager>(); ;
                arCamera = arCameraManager.GetComponent<Camera>();
            }

            if (session == null || sessionOrigin == null)
            {
                Debug.Log($"{typeof(ARViewerSceneController).Name}: Can't initialize because ar session or ar session origin is null.");
                return false;
            }

            return true;
        }

        #region SURFACE TRACKING

        private void InitializeSurfaceTracking()
        {
            if (planeManager == null)
                planeManager = sessionOrigin.gameObject.AddComponent<ARPlaneManager>();

            if (planeManager == null)
            {
                Debug.LogWarning($"{typeof(ARViewerSceneController).Name}: Can't initialize {configuration.trackingMode} because {typeof(ARPlaneManager)} is null.");
                return;
            }

            planeManager.planePrefab = ARPlanePrefab;

            planeManager.planesChanged += OnPlanesChanged;

            if (raycastManager == null)
                raycastManager = sessionOrigin.gameObject.AddComponent<ARRaycastManager>();

            OnInitializationCompleteEvent?.Invoke(this);
        }

        public Transform getOriginTransform()
        {
            return originTransform;
        }

        public Camera GetARcamera()
        {
            return arCamera;
        }

        public bool RaycastARPlane(Vector2 screenPosition, out Pose hitPose)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            bool hasHit = false;

            hitPose = new Pose();

            if (raycastManager != null)
            {
                hasHit = raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon);
                hitPose = hits[0].pose;
            }

            return hasHit;
        }

        public void RemoveARPlanesUpdateEvent()
        {
            planeManager.planesChanged -= OnPlanesChanged;
        }

        private void OnPlanesChanged(ARPlanesChangedEventArgs planeEvent)
        {
            OnARPlanesChangedEvent?.Invoke(planeEvent);

            if (planeEvent.added.Count > 0 || planeEvent.updated.Count > 0)
            {
                OnARPlanesFoundEvent?.Invoke(planeEvent);
            }
        }

        public void TogglePlaneDetection()
        {
            if (planeManager != null)
            {
                planeManager.enabled = !planeManager.enabled;

                //SetAllPlanesActive(planeManager.enabled);
            }
        }

        private void SetAllPlanesActive(bool value)
        {
            if (planeManager != null)
            {
                foreach (var plane in planeManager.trackables)
                    plane.gameObject.SetActive(value);
            }
        }

        private void SetAllPlanesAlpha(float alpha)
        {
            if (planeManager != null)
            {
                foreach (ARPlane plane in planeManager.trackables)
                {
                    MeshRenderer r = plane.gameObject.GetComponent<MeshRenderer>();
                    if (r != null)
                    {
                        Material mat = r.material;
                        if (mat != null)
                        {
                            Color c = mat.color;
                            print(c);
                            if (DOTween.IsTweening(c))
                                DOTween.Kill(c);

                            mat.DOFade(alpha, 1f).SetEase(Ease.OutSine);
                        }
                    }
                }
            }
        }

        public void SetPlanesVisibility(bool flag)
        {
            float targetAlpha = flag ? 0.25f : 0;

            SetAllPlanesAlpha(targetAlpha);
        }

        #endregion

        #region IMAGE TRACKING

        private void InitializeImageTracking()
        {
            if (trackedImageManager == null)
                trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

            if (trackedImageManager == null)
            {
                Debug.LogWarning($"{typeof(ARViewerSceneController).Name}: Can't initialize {configuration.trackingMode} because {typeof(ARTrackedImageManager)} is null.");
                return;
            }

            //trackedImageManager.referenceLibrary = configuration.referenceLibrary;

            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

            OnInitializationCompleteEvent?.Invoke(this);
        }



        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (ARTrackedImage trackedImage in eventArgs.added)
            {
                OnTrackedImageAdded(trackedImage);
            }

            foreach (ARTrackedImage trackedImage in eventArgs.updated)
            {
                OnTrackedImageUpdated(trackedImage);
            }

            foreach (ARTrackedImage trackedImage in eventArgs.removed)
            {
                OnTrackedImageRemoved(trackedImage);
            }
        }

        private void OnTrackedImageUpdated(ARTrackedImage trackedImage)
        {
            TrackingState previousTrackingState = (trackingStateDictionary.ContainsKey(trackedImage)) ? trackingStateDictionary[trackedImage] : trackedImage.trackingState;

            switch (trackedImage.trackingState)
            {
                case TrackingState.None:
                    break;
                case TrackingState.Limited:

                    if (previousTrackingState != TrackingState.Limited)
                    {
                        if (debug && lineRendererDictionary.ContainsKey(trackedImage))
                            lineRendererDictionary[trackedImage].enabled = false;

                        OnTrackingLost(trackedImage);
                    }

                    break;
                case TrackingState.Tracking:

                    if (previousTrackingState != TrackingState.Tracking)
                    {
                        if (debug && lineRendererDictionary.ContainsKey(trackedImage))
                            lineRendererDictionary[trackedImage].enabled = true;

                        OnTrackingFound(trackedImage);
                    }

                    if (debug)
                        UpdateDebugObjects(trackedImage);

                OnTrackedImageTrackingEvent?.Invoke(trackedImage);

                    break;
                default:
                    break;
            }

            trackingStateDictionary[trackedImage] = trackedImage.trackingState;
        }

        private void OnTrackedImageAdded(ARTrackedImage trackedImage)
        {
            Debug.Log($"{typeof(ARViewerSceneController).Name}: Tracking added for {trackedImage.referenceImage.name}.");

            OnTrackingAddedEvent?.Invoke(trackedImage);
        }

        private void OnTrackedImageRemoved(ARTrackedImage trackedImage)
        {
            Debug.Log($"{typeof(ARViewerSceneController).Name}: Tracking removed for {trackedImage.referenceImage.name}.");

            OnTrackingRemovedEvent?.Invoke(trackedImage);
        }

        private void InitializeDebugObjects(ARTrackedImage trackedImage)
        {
            LineRenderer lineRenderer = trackedImage.gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = debugLineMaterial;
            lineRenderer.loop = true;
            lineRenderer.widthMultiplier = debugLineWidth;
            lineRenderer.positionCount = 4;

            lineRendererDictionary[trackedImage] = lineRenderer;
        }

        private void UpdateDebugObjects(ARTrackedImage trackedImage)
        {
            if (!lineRendererDictionary.ContainsKey(trackedImage))
                InitializeDebugObjects(trackedImage);

            Vector3 upperLeft = trackedImage.transform.position + new Vector3(-trackedImage.extents.x, trackedImage.extents.y, 0);
            Vector3 upperRight = trackedImage.transform.position + new Vector3(trackedImage.extents.x, trackedImage.extents.y, 0);
            Vector3 lowerLeft = trackedImage.transform.position + new Vector3(-trackedImage.extents.x, -trackedImage.extents.y, 0);
            Vector3 lowerRight = trackedImage.transform.position + new Vector3(trackedImage.extents.x, -trackedImage.extents.y, 0);

            Vector3[] positions = { upperLeft, upperRight, lowerRight, lowerLeft };
            lineRendererDictionary[trackedImage].SetPositions(positions);
        }

        #endregion

        private void OnTrackingFound(ARTrackable trackable)
        {
            Debug.Log($"{typeof(ARViewerSceneController).Name}: Tracking found for {trackable.name}.");

            OnTrackingFoundEvent?.Invoke(trackable);
        }

        private void OnTrackingLost(ARTrackable trackable)
        {
            Debug.Log($"{typeof(ARViewerSceneController).Name}: Tracking lost for {trackable.name}.");

            OnTrackingLostEvent?.Invoke(trackable);
        }


    }
}

#endif


