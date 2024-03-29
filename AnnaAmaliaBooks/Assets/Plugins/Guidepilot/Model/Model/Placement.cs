//
//  Placement.cs
//  GuidePilot
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
	public class Placement: CoreObject, Prioritized {
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("priority")]
	private int internPriority;
	[JsonIgnore]
	public int priority { get => internPriority; set => internPriority = value; }
	[JsonProperty("content_usage_id")]
	public int contentUsageId;
	[JsonProperty("gp_zone_id")]
	public int gpZoneId;
	[JsonProperty("map_layer_id")]
	public int mapLayerId;
	[JsonProperty("coordinate_points")]
	public CoordinatePoint[] coordinatePoints;
	[JsonProperty("annotation_id")]
	public int annotationId;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"contentUsageId:" + contentUsageId + Environment.NewLine + 	"gpZoneId:" + gpZoneId + Environment.NewLine + 	"mapLayerId:" + mapLayerId + Environment.NewLine + 	"coordinatePoints:" + coordinatePoints + Environment.NewLine + 	"annotationId:" + annotationId + Environment.NewLine);
	}
	}
}