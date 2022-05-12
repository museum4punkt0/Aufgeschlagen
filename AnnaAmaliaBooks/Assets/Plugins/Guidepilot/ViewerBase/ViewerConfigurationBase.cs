#if GUIDEPILOT_CORE_VIEWER3D || GUIDEPILOT_CORE_AR

using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace com.guidepilot.guidepilotcore
{
    public abstract class ViewerConfigurationBase : ScriptableObject
    {
        [BoxGroup("Controls")]
        [ToggleGroup("Controls/enableRotation", "Rotation")]
        [ToggleLeft]
        public bool enableRotation = true;

        [ToggleGroup("Controls/enableRotation")]
        public float rotationSpeed = 100f;

        [ToggleGroup("Controls/enablePan", "Pan")]
        [ToggleLeft]
        public bool enablePan = true;

        [ToggleGroup("Controls/enablePan")]
        public float panSpeed = 100f;

        [ToggleGroup("Controls/enableZoomScale", "Zoom / Scale")]
        [ToggleLeft]
        public bool enableZoomScale = true;

        [ToggleGroup("Controls/enableZoomScale")]
        public float zoomScaleSpeed = 100f;

        [BoxGroup("Scenes")]
        [TableList(AlwaysExpanded = true, ShowIndexLabels = true)]
        public List<ViewerScene> viewerScenes = new List<ViewerScene>();
    }
}

#endif