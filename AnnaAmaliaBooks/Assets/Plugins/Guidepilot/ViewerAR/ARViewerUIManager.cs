#if GUIDEPILOT_CORE_AR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.XR.ARFoundation;
using System;
using UnityEngine.Events;

namespace com.guidepilot.guidepilotcore
{
    public class ARViewerUIManager : MonoBehaviour
    {
        [BoxGroup("References")]
        public UIController imageTargetScanUIController;

        [BoxGroup("References")]
        public UIController alertBox;

        public static UnityAction<ARViewerUIManager> OnAwakeEvent;
        public static UnityAction<ARViewerUIManager> OnDestroyEvent;

        private ARViewerSceneController sceneControllerViewerAR;

        private void Awake()
        {
            sceneControllerViewerAR = FindObjectOfType<ARViewerSceneController>();

            if (sceneControllerViewerAR == null)
            {
                this.enabled = false;
                Debug.LogWarning($"{typeof(ARViewerUIManager).Name}: Could not find a Viewer AR Scene Controller.");
                return;
            }

            OnAwakeEvent?.Invoke(this);
        }

        private void OnEnable()
        {
            ARViewerSceneController.OnContentUpdatedEvent += OnContentUpdated;
            //ARViewerSceneController.OnTrackingAddedEvent += OnTrackingAdded;
            //ARViewerSceneController.OnTrackingFoundEvent += OnTrackingFound;
        }

        private void OnDisable()
        {
            ARViewerSceneController.OnContentUpdatedEvent -= OnContentUpdated;
            //ARViewerSceneController.OnTrackingAddedEvent -= OnTrackingAdded;
            //ARViewerSceneController.OnTrackingFoundEvent -= OnTrackingFound;
        }

        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke(this);
        }

        public void SetScanUIActive(bool status, bool animate = true)
        {
            if (imageTargetScanUIController != null)
            {
                imageTargetScanUIController.SetActive(status, animate);
            }
        }

        public void SetAlertBoxActive(bool status, bool animate = true)
        {
            if (alertBox != null)
            {
                alertBox.SetActive(status, animate);
            }
        }

        private void OnContentUpdated(UnityContent content)
        {
            SetScanUIActive(true);

            if (imageTargetScanUIController != null)
            {
                imageTargetScanUIController.UpdateGraphic("ARTargetImage", content.sprite);
            }
        }

        //private void OnTrackingAdded(ARTrackable trackable) => OnTrackingFound(trackable);

        //private void OnTrackingFound(ARTrackable trackable)
        //{
        //    if (trackable is ARTrackedImage || trackable.name == "TestARTrackedImage")
        //    {
        //        ARTrackedImage trackedImage = (ARTrackedImage)trackable;
        //
        //        if (trackedImage.referenceImage.name != sceneControllerViewerAR.currentContent.contentUsageSUUID) return;
        //
        //        SetScanUIActive(false);
        //        SetAlertBoxActive(true);
        //    }            
        //}
    }
}

#endif

