//
//  Room.cs
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
	public class Room: CoreLinkable, CoreObject, Titled {
	[JsonProperty("suuid")]
	private string internSuuid;
	[JsonIgnore]
	public string suuid { get => internSuuid; set => internSuuid = value; }
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("title")]
	private Dictionary<string, object> internTitle;
	[JsonIgnore]
	public Dictionary<string, object> title { get => internTitle; set => internTitle = value; }
	[JsonProperty("title_short")]
	private Dictionary<string, object> internTitleShort;
	[JsonIgnore]
	public Dictionary<string, object> titleShort { get => internTitleShort; set => internTitleShort = value; }
	[JsonProperty("wing_id")]
	public int wingId;
	[JsonProperty("floor_id")]
	public int floorId;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"wingId:" + wingId + Environment.NewLine + 	"floorId:" + floorId + Environment.NewLine);
	}
	}
}