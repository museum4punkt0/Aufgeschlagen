//
//  ZoneRoute.cs
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
	public class ZoneRoute: CoreObject {
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("zone_route_items")]
	public ZoneRouteItem[] zoneRouteItems;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"zoneRouteItems:" + zoneRouteItems + Environment.NewLine);
	}
	}
}