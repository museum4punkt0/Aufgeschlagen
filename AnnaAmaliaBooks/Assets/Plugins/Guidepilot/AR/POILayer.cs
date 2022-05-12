#if GUIDEPILOT_CORE_AR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace com.guidepilot.guidepilotcore
{
	[CreateAssetMenu(fileName = "New POILayer", menuName = "Guidepilot/AR/POILayer", order = 0)]
	public class POILayer : UnityContent
	{
		//[BoxGroup("Settings")]
		//public Sprite backgroundImage = null;

		[BoxGroup("Content")]
		[SerializeField]
		public List<POI> pointsOfInterest = new List<POI>();
	}
}

#endif
