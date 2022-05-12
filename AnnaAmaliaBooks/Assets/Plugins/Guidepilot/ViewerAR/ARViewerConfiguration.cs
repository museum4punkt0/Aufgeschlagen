#if GUIDEPILOT_CORE_AR

using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace com.guidepilot.guidepilotcore
{
    [CreateAssetMenu(fileName = "ARViewerConfiguration", menuName = "Guidepilot/ARViewer/ARViewerConfiguration")]
    public class ARViewerConfiguration : ViewerConfigurationBase
    {
        public enum TrackingMode { None, SurfaceTracking, ImageTracking };

        [BoxGroup("Settings", Order = -10)]
        public TrackingMode trackingMode;

        [BoxGroup("Settings")]
        [ShowIf("trackingMode", TrackingMode.ImageTracking)]
        public XRReferenceImageLibrary referenceLibrary;

        [BoxGroup("Settings")]
        public float defaultObjectSize = 1f;

        [ToggleGroup("Controls/enableZoomScale")]
        public float minScaleFactor = 0.5f;

        [ToggleGroup("Controls/enableZoomScale")]
        public float maxScaleFactor = 2.0f;

        [BoxGroup("References")]
        [ShowIf("trackingMode", TrackingMode.SurfaceTracking)]
        [InlineEditor]
        public GameObject shadowReceiverPrefab;
    }
}

#endif