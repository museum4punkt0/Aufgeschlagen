//
//  ContentUsage.cs
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
	public class ContentUsage: CoreLinkable, CoreObject, Prioritized {
	[JsonProperty("suuid")]
	private string internSuuid;
	[JsonIgnore]
	public string suuid { get => internSuuid; set => internSuuid = value; }
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("priority")]
	private int internPriority;
	[JsonIgnore]
	public int priority { get => internPriority; set => internPriority = value; }
	[JsonProperty("parent_content_usage_id")]
	public int parentContentUsageId;
	[JsonProperty("content_compilation_id")]
	public int contentCompilationId;
	[JsonProperty("content_id")]
	public int contentId;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"parentContentUsageId:" + parentContentUsageId + Environment.NewLine + 	"contentCompilationId:" + contentCompilationId + Environment.NewLine + 	"contentId:" + contentId + Environment.NewLine);
	}
	}
}