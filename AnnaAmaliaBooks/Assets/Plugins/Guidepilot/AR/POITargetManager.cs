#if GUIDEPILOT_CORE_AR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Sirenix.OdinInspector;
using System;

namespace com.guidepilot.guidepilotcore
{
    public class POITargetManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject targetPrefab = null;

        [SerializeField]
        private ARTrackedImageManager trackedImageManager = null;

        //[SerializeField]
        //private XRReferenceImageLibrary xRReferenceImageLibrary = null;

        //[ShowInInspector]
        //[ReadOnly]
        //public List<POITarget> Targets
        //{
        //    get 
        //    {
        //        return targets;
        //    }
        //    set
        //    {
        //        if (targets == value) return;
        //        targets = value;
        //
        //        OnTargetsUpdate?.Invoke(value);
        //    }
        //}

        public bool instantiateTargetsOnStart = false;

        public List<POITarget> targets = new List<POITarget>();

        //private POITarget activeTarget = null;



        //[ReadOnly]
        [ShowInInspector]
        public POITargetController ActiveTargetController
        {
            get
            {
                return activeTargetController;
            }
            set
            {
                if (activeTargetController == value) return;
                activeTargetController = value;
                OnActiveTargetUpdate?.Invoke(value);
            }
        }

        [ShowInInspector]
        [OnValueChanged("OnCurrentTrackedTargetControllerUpdate")]
        public POITargetController currentTrackedTargetController { get; private set; }

        private POITargetController activeTargetController = null;
        private List<POITargetController> instantiatedTargetController = new List<POITargetController>();

        public UnityAction<List<POITarget>> OnTargetsUpdate;
        public POITargetController.POITargetControllerEvent OnActiveTargetUpdate = new POITargetController.POITargetControllerEvent();

        public UnityEvent OnTrackingFoundEvent = new UnityEvent();
        public UnityEvent OnTrackingLostEvent = new UnityEvent();

        private Dictionary<ARTrackedImage, TrackingState> trackingStateDictionary = new Dictionary<ARTrackedImage, TrackingState>();
        private static Dictionary<ARTrackedImage, POITargetController> targetControllerDictionary = new Dictionary<ARTrackedImage, POITargetController>();

        private ARTrackedImage currentTrackedImage = null;

        private void Awake()
        {
            

        }

        private void Start()
        {
            if (instantiateTargetsOnStart)
            {
                foreach (POITarget target in targets)
                {
                    CreatePOITarget(target, Vector2.one);
                }
            }
        }

        private void OnDestroy()
        {
            

            Debug.Log($"{this.name}: OnDestroy");
        }

        private void OnEnable()
        {
            if (trackedImageManager != null)
            {
                trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

                //if (xRReferenceImageLibrary != null)
                //    trackedImageManager.referenceLibrary = xRReferenceImageLibrary;
            }
        }

        private void OnDisable()
        {
            if (trackedImageManager != null)
                trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

        public void SetTarget(POITarget target)
        {
            if (!targets.Contains(target))
            {
                Debug.Log($"{this.name}: Can't set target {target.name} because it is not included in this Target Manager's target list.");
                return;
            }

            ActiveTargetController = instantiatedTargetController.Find((controller) => controller.target == target);
            
            if (ActiveTargetController == null)
                ActiveTargetController = CreatePOITarget(target, Vector2.one);

            if (currentTrackedTargetController == null && currentTrackedImage != null)
                OnTrackingFound(currentTrackedImage);
            else
            {
                SetAllTargetsActive(false);
                OnTrackingLostEvent?.Invoke();
            }

            //if (currentTrackedTargetController != null && currentTrackedTargetController == ActiveTargetController)
            //{
            //    currentTrackedTargetController.SetActive(true);
            //    OnTrackingFoundEvent?.Invoke();
            //}
            //else
            //{
            //    SetAllTargetsActive(false);
            //    OnTrackingLostEvent?.Invoke();
            //}
        }

        public void SetAllTargetsActive(bool status)
        {
            foreach (POITargetController controller in instantiatedTargetController)
            {
                controller.SetActive(status);
            }
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

        private void OnTrackedImageAdded(ARTrackedImage trackedImage)
        {
            InitializeTargetController(trackedImage);

            OnTrackingFound(trackedImage);

            Debug.Log($"{this.name}: Tracked Image added: " + trackedImage.referenceImage.name);
        }

        private void OnTrackedImageUpdated(ARTrackedImage trackedImage)
        {
            if (!trackingStateDictionary.ContainsKey(trackedImage))
            {
                Debug.Log($"{this.name}: OnTrackedImageUpdated but tracked image missing in dictionary");

                foreach (KeyValuePair<ARTrackedImage, TrackingState> entry in trackingStateDictionary)
                {
                    Debug.Log($"{this.name}: trackingStateDictionary Key = {entry.Key}, Value = {entry.Value}");
                }

                Debug.Log($"{this.name}: trackedImage: " + trackedImage);

                return;
            }

            TrackingState previousTrackingState = trackingStateDictionary[trackedImage];

            switch (trackedImage.trackingState)
            {
                case TrackingState.None:
                    if (previousTrackingState != TrackingState.None)
                    {
                        Debug.Log($"{this.name}: OnTrackedImageUpdated tracking state NONE");
                    }

                    break;
                case TrackingState.Limited:

                    if (previousTrackingState != TrackingState.Limited)
                    {
                        Debug.Log($"{this.name}: OnTrackedImageUpdated tracking state LIMIT");
                        OnTrackingLost(trackedImage);
                    }

                    break;
                case TrackingState.Tracking:

                    if (previousTrackingState != TrackingState.Tracking)
                    {
                        Debug.Log($"{this.name}: OnTrackedImageUpdated tracking state TRACKING");
                        OnTrackingFound(trackedImage);
                    }

                    break;
                default:

                    Debug.Log($"{this.name}: OnTrackedImageUpdated tracking state {trackedImage.trackingState}");

                    break;
            }


            trackingStateDictionary[trackedImage] = trackedImage.trackingState;
        }


        private void OnTrackedImageRemoved(ARTrackedImage trackedImage)
        {

        }

        private void InitializeTargetController(ARTrackedImage trackedImage)
        {
            trackingStateDictionary[trackedImage] = trackedImage.trackingState;

            POITargetController controller = instantiatedTargetController.Find((controller) => controller.target.contentUsageSUUID == trackedImage.referenceImage.name);

            if (controller == null)
            {
                Debug.Log($"{this.name}: Could not initialize target controller for {trackedImage.referenceImage.name} because it is not instantiated yet.");
                return;
            }

            controller.transform.parent = trackedImage.transform;
            controller.transform.position = trackedImage.transform.position;
            controller.transform.localRotation = Quaternion.Euler(Vector3.right * 90);
            controller.transform.localScale *= trackedImage.size;
            targetControllerDictionary[trackedImage] = controller;

            Debug.Log($"{this.name}: Tracked Image initialized: " + trackedImage.referenceImage.name);
        }


        [Button]
        private void OnTrackingFound(ARTrackedImage trackedImage)
        {
            bool isTrackedImageInitialized = targetControllerDictionary.ContainsKey(trackedImage);

            if (!isTrackedImageInitialized)
                InitializeTargetController(trackedImage);

            if (targetControllerDictionary.ContainsKey(trackedImage) && ActiveTargetController == targetControllerDictionary[trackedImage])
            {
                POITargetController controller = targetControllerDictionary[trackedImage];
                controller.SetActive(true);
                currentTrackedTargetController = controller;

                Debug.Log($"{this.name}: CurrentTrackedTargetController set to: " + controller.name);

                OnTrackingFoundEvent?.Invoke();
            }

            currentTrackedImage = trackedImage;
            Debug.Log($"{this.name}: Tracking found: " + trackedImage.referenceImage.name);
        }

        [Button]
        private void OnTrackingLost(ARTrackedImage trackedImage)
        {
            if (targetControllerDictionary.ContainsKey(trackedImage) && ActiveTargetController == targetControllerDictionary[trackedImage])
            {
                POITargetController controller = targetControllerDictionary[trackedImage];
                controller.SetActive(false);
                currentTrackedTargetController = null;

                Debug.Log($"{this.name}: CurrentTrackedTargetController set to: null");

                OnTrackingLostEvent?.Invoke();
            }

            currentTrackedImage = null;
            Debug.Log($"{this.name}: Tracking lost: " + trackedImage.referenceImage.name);
        }
        private POITargetController CreatePOITarget(POITarget target, Vector2 targetSize, Transform parent = null, bool setActive = false)
        {
            if (target == null || instantiatedTargetController.Find((controller) => controller.target == target) != null)
            {
                Debug.Log($"{this.name}: Couldn't create {target.name} because target is null or already instantiated.");
                return null;
            }

            POITargetController controller = Instantiate(targetPrefab).AddComponent<POITargetController>();
            controller.Initialize(target, targetSize, parent);
            controller.gameObject.SetActive(setActive);
            //ActiveTargetController = controller;
            instantiatedTargetController.Add(controller);

            Debug.Log($"{this.name}: Created Target: " + target.title);

            return controller;
        }

        [Button]
        public void OnNewActiveTarget()
        {
            OnActiveTargetUpdate?.Invoke(activeTargetController);
        }

        [Button]
        private void CreatePOITargetInCameraView(POITarget target)
        {
            if (target == null || instantiatedTargetController.Find((controller) => controller.target == target) != null)
            {
                Debug.Log($"{this.name}: Couldn't create {target.name} because target is null or already instantiated.");
                return;
            }

            POITargetController controller = CreatePOITarget(target, Vector2.one, null, true);
            ActiveTargetController = controller;
            currentTrackedTargetController = controller;
            Camera mainCamera = Camera.main;
            controller.transform.position = mainCamera.transform.position + Vector3.forward * 2;
            controller.transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);

            OnTrackingFoundEvent?.Invoke();
        }

        private void OnCurrentTrackedTargetControllerUpdate()
        {
            bool status = currentTrackedTargetController != null && currentTrackedTargetController == ActiveTargetController;
            UnityEvent eventToCall = status ? OnTrackingFoundEvent : OnTrackingLostEvent;

            if (status)
            {
                currentTrackedTargetController.SetActive(true);
            }
            else
            {
                SetAllTargetsActive(false);
            }

            eventToCall?.Invoke();
        }
    }
}

#endif
