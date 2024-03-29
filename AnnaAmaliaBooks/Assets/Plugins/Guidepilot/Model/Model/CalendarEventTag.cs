//
//  CalendarEventTag.cs
//  GuidePilot
//
//  
//
//  Generated by GuidePilot - EnumGenerator
//  Copyright © 2021 MicroMovie Media GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;


namespace com.guidepilot.guidepilotcore {
	[Preserve]
	public class CalendarEventTag: CoreObject {
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("calendar_tag_id")]
	public int calendarTagId;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"calendarTagId:" + calendarTagId + Environment.NewLine);
	}
	}
}