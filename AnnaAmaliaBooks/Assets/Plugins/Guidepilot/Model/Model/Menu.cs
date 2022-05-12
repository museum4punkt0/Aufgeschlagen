//
//  Menu.cs
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
	public class Menu: CoreLinkable, CoreObject, Customized, Subtitled, Tagged, Titled {
	[JsonProperty("suuid")]
	private string internSuuid;
	[JsonIgnore]
	public string suuid { get => internSuuid; set => internSuuid = value; }
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
[JsonConverter(typeof(DictionaryConverter))]
	[JsonProperty("custom_data")]
	private Dictionary<string, object> internCustomData;
	[JsonIgnore]
	public Dictionary<string, object> customData { get => internCustomData; set => internCustomData = value; }
	[JsonProperty("subtitle")]
	private Dictionary<string, object> internSubtitle;
	[JsonIgnore]
	public Dictionary<string, object> subtitle { get => internSubtitle; set => internSubtitle = value; }
	[JsonProperty("subtitle_short")]
	private Dictionary<string, object> internSubtitleShort;
	[JsonIgnore]
	public Dictionary<string, object> subtitleShort { get => internSubtitleShort; set => internSubtitleShort = value; }
	[JsonProperty("tags")]
	private string[] internTags;
	[JsonIgnore]
	public string[] tags { get => internTags; set => internTags = value; }
	[JsonProperty("title")]
	private Dictionary<string, object> internTitle;
	[JsonIgnore]
	public Dictionary<string, object> title { get => internTitle; set => internTitle = value; }
	[JsonProperty("title_short")]
	private Dictionary<string, object> internTitleShort;
	[JsonIgnore]
	public Dictionary<string, object> titleShort { get => internTitleShort; set => internTitleShort = value; }
	[JsonProperty("menu_sections")]
	public MenuSection[] menuSections;
	[JsonProperty("menu_documents")]
	public MenuDocument[] menuDocuments;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"menuSections:" + menuSections + Environment.NewLine + 	"menuDocuments:" + menuDocuments + Environment.NewLine);
	}
	}
}