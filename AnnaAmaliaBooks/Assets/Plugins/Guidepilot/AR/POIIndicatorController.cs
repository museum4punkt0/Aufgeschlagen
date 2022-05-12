#if GUIDEPILOT_CORE_AR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace com.guidepilot.guidepilotcore
{
	public class POIIndicatorController : MonoBehaviour
	{
		public enum Status { Default, Highlight}

		[EnumToggleButtons]
		[OnValueChanged("OnStatusUpdate")]
		[ShowInInspector]
		public Status status { get; private set; }

		[SerializeField]
		private SpriteRenderer background = null;

		[SerializeField]
		private Color defaultColor = Color.black;

		[SerializeField]
		private Color highlightColor = new Color(255, 104, 5);

		public void SetStatus(bool status)
		{
			SetStatus(status ? Status.Highlight : Status.Default);
		}

		public void SetStatus(Status status)
		{
			if (background == null) return;

            switch (status)
            {
                case Status.Default:
					background.color = defaultColor;
                    break;
                case Status.Highlight:
					background.color = highlightColor;
                    break;
                default:
					background.color = defaultColor;
                    break;
            }

			this.status = status;
        }

		private void OnStatusUpdate()
		{
			SetStatus(status);
		}
	}
}

#endif
