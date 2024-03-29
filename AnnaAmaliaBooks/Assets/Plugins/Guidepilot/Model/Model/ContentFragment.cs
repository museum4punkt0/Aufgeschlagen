//
//  ContentFragment.cs
//  GuidePilot
//
//  represents a smaller part (fragment) of a content
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
	public class ContentFragment: CoreObject, Customized, Sorted, Tagged, Targeting {
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
[JsonConverter(typeof(DictionaryConverter))]
	[JsonProperty("custom_data")]
	private Dictionary<string, object> internCustomData;
	[JsonIgnore]
	public Dictionary<string, object> customData { get => internCustomData; set => internCustomData = value; }
	[JsonProperty("sort_index")]
	private int internSortIndex;
	[JsonIgnore]
	public int sortIndex { get => internSortIndex; set => internSortIndex = value; }
	[JsonProperty("tags")]
	private string[] internTags;
	[JsonIgnore]
	public string[] tags { get => internTags; set => internTags = value; }
[JsonConverter(typeof(TargetTypeConverter))]
	[JsonProperty("target_type")]
	private TargetType internTargetType;
	[JsonIgnore]
	public TargetType targetType { get => internTargetType; set => internTargetType = value; }
	[JsonProperty("target_uuid")]
	private string internTargetUuid;
	[JsonIgnore]
	public string targetUuid { get => internTargetUuid; set => internTargetUuid = value; }
[JsonConverter(typeof(DictionaryConverter))]
	[JsonProperty("target_data")]
	private Dictionary<string, object> internTargetData;
	[JsonIgnore]
	public Dictionary<string, object> targetData { get => internTargetData; set => internTargetData = value; }
	[JsonProperty("target_locales")]
	private Language[] internTargetLocales;
	[JsonIgnore]
	public Language[] targetLocales { get => internTargetLocales; set => internTargetLocales = value; }
	[JsonProperty("target_scope_uuid")]
	private string internTargetScopeUuid;
	[JsonIgnore]
	public string targetScopeUuid { get => internTargetScopeUuid; set => internTargetScopeUuid = value; }
	[JsonProperty("content_fragment_documents")]
	public ContentFragmentDocument[] contentFragmentDocuments;
	[JsonProperty("content_fragment_properties")]
	public ContentFragmentProperty[] contentFragmentProperties;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"contentFragmentDocuments:" + contentFragmentDocuments + Environment.NewLine + 	"contentFragmentProperties:" + contentFragmentProperties + Environment.NewLine);
	}
	}
}