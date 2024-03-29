//
//  CoordinatePoint.cs
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
	public class CoordinatePoint: CoreObject {
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("h")]
	public double? h;
	[JsonProperty("v")]
	public double? v;
	[JsonProperty("z")]
	public double? z;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"h:" + h + Environment.NewLine + 	"v:" + v + Environment.NewLine + 	"z:" + z + Environment.NewLine);
	}
	}
}