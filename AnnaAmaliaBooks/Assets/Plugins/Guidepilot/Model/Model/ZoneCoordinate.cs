//
//  ZoneCoordinate.cs
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
	public class ZoneCoordinate: CoreObject, Sorted {
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("sort_index")]
	private int internSortIndex;
	[JsonIgnore]
	public int sortIndex { get => internSortIndex; set => internSortIndex = value; }
	[JsonProperty("coordinate_id")]
	public int coordinateId;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"coordinateId:" + coordinateId + Environment.NewLine);
	}
	}
}