#if GUIDEPILOT_CORE_AR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace com.guidepilot.guidepilotcore
{
	[CreateAssetMenu(fileName = "New POITarget", menuName = "Guidepilot/AR/POITarget", order = 0)]
	public class POITarget : UnityContent
	{

		//[BoxGroup("General")]
		//public Vector2 realWorldScale = Vector2.one;

		[BoxGroup("Content")]
		[SerializeField]
		public List<POILayer> layers = new List<POILayer>();

		[ShowInInspector]
		[ReadOnly]
		[BoxGroup("Custom")]
		public Vector3 dimensions { get => (sprite != null) ? sprite.bounds.extents : Vector3.zero; }
    }
}

#endif
