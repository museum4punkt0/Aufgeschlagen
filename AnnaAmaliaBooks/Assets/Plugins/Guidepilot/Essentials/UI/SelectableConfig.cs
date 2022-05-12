using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace com.guidepilot.guidepilotcore
{
	public class SelectableConfig
	{
		public SelectableConfig(string title, UnityAction action)
		{
			this.title = title;
			this.action = action;
		}

		public SelectableConfig(LocalizedString titleReference, UnityAction action)
		{
			this.titleReference = titleReference;
			this.action = action;
		}

		public SelectableConfig(string title, LocalizedString titleReference, UnityAction action)
		{
			this.title = title;
			this.titleReference = titleReference;
			this.action = action;
		}

		public string title = "";
		public UnityAction action = null;
		public LocalizedString titleReference = null;
	}
}
