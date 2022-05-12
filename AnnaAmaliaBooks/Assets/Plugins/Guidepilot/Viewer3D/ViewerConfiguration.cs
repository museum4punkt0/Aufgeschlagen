#if GUIDEPILOT_CORE_VIEWER3D

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace com.guidepilot.guidepilotcore
{
    [CreateAssetMenu(fileName = "ViewerConfiguration", menuName = "Guidepilot/Viewer/ViewerConfiguration")]
    public class ViewerConfiguration : ViewerConfigurationBase
    {
        [BoxGroup("Settings", Order = -10)]
        public Color backgroundColor = Color.black;

        [ToggleGroup("Controls/enableFokusMode", "Fokus Mode")]
        public bool enableFokusMode = true;

        [ToggleGroup("Controls/enableFokusMode")]
        [Range(0, 1)]
        public float fokusModeCameraDistance = 0.5f;

        [BoxGroup("Tutorial")]
        [TableList(AlwaysExpanded = true, ShowIndexLabels = true)]
        public List<ViewerTutorialController.TutorialPart> tutorialParts = new List<ViewerTutorialController.TutorialPart>();
    }
}

#endif


