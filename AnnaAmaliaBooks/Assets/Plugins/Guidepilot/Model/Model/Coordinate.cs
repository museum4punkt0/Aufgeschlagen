//
//  Coordinate.cs
//  GuidePilot
//
//  bla bla bla
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
	public class Coordinate: CoreObject {
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("x")]
	public int x;
	[JsonProperty("y")]
	public int y;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"x:" + x + Environment.NewLine + 	"y:" + y + Environment.NewLine);
	}
	}
}