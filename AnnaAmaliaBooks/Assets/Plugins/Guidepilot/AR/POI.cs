#if GUIDEPILOT_CORE_AR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace com.guidepilot.guidepilotcore
{
	[CreateAssetMenu(fileName = "New POI", menuName = "Guidepilot/AR/POI", order = 0)]
	public class POI : UnityContent
	{
		[BoxGroup("Settings")]
		[MinValue(0)]
		[MaxValue(1)]
		public Vector2 position;

		[BoxGroup("Settings")]
		[AssetsOnly]
		public GameObject indicator = null;

		[BoxGroup("Settings")]
		[ShowIf("indicator")]
		public bool highlightPOIIndicator = false;

		[BoxGroup("Settings")]
		[AssetsOnly]
		public GameObject spriteDisplay = null;

		[BoxGroup("References")]
		[ReadOnly]
		public POIController controller = null;

        private void OnValidate()
        {
            if(controller != null)
				controller.transform.localPosition = (controller.layerOrigin - controller.layerDimensions) + new Vector3(controller.layerDimensions.x * position.x * 2, controller.layerDimensions.y * position.y * 2);
		}
    }
}

#endif
