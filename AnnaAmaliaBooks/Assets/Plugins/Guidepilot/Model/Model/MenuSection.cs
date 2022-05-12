//
//  MenuSection.cs
//  GuidePilot
//
//  Sections of a menu that contains items
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
	public class MenuSection: Colored, CoreLinkable, CoreObject, Customized, LocaleVisibility, Sorted, Subtitled, Tagged, Targeting, Timed, Titled {
	[Preserve]
	public enum HeaderVisibility {
		Always,
		Never
	}

	[Preserve]
	public class HeaderVisibilityConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			MenuSection.HeaderVisibility castedValue = (MenuSection.HeaderVisibility)value;
			switch (castedValue) {
			case MenuSection.HeaderVisibility.Always:
			writer.WriteValue("ALWAYS");break;
			case MenuSection.HeaderVisibility.Never:
			writer.WriteValue("NEVER");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "ALWAYS":
			return MenuSection.HeaderVisibility.Always;
			case "NEVER":
			return MenuSection.HeaderVisibility.Never;
			}
			return null;
		}

		public override bool CanConvert(System.Type objectType) {
			return objectType == typeof(string);
		}
	}

	[JsonProperty("color")]
	private string internColor;
	[JsonIgnore]
	public string color { get => internColor; set => internColor = value; }
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
	[JsonProperty("visible_in_locales")]
	private Language[] internVisibleInLocales;
	[JsonIgnore]
	public Language[] visibleInLocales { get => internVisibleInLocales; set => internVisibleInLocales = value; }
	[JsonProperty("sort_index")]
	private int internSortIndex;
	[JsonIgnore]
	public int sortIndex { get => internSortIndex; set => internSortIndex = value; }
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
	[JsonProperty("start")]
	private DateTime internStart;
	[JsonIgnore]
	public DateTime start { get => internStart; set => internStart = value; }
	[JsonProperty("end")]
	private DateTime internEnd;
	[JsonIgnore]
	public DateTime end { get => internEnd; set => internEnd = value; }
	[JsonProperty("title")]
	private Dictionary<string, object> internTitle;
	[JsonIgnore]
	public Dictionary<string, object> title { get => internTitle; set => internTitle = value; }
	[JsonProperty("title_short")]
	private Dictionary<string, object> internTitleShort;
	[JsonIgnore]
	public Dictionary<string, object> titleShort { get => internTitleShort; set => internTitleShort = value; }
	[JsonProperty("menu_section_style_id")]
	public int menuSectionStyleId;
[JsonConverter(typeof(HeaderVisibilityConverter))]
	[JsonProperty("header_visibility")]
	public HeaderVisibility headerVisibility;
	[JsonProperty("menu_section_items")]
	public MenuSectionItem[] menuSectionItems;
	[JsonProperty("menu_section_documents")]
	public MenuSectionDocument[] menuSectionDocuments;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"menuSectionStyleId:" + menuSectionStyleId + Environment.NewLine + 	"headerVisibility:" + headerVisibility + Environment.NewLine + 	"menuSectionItems:" + menuSectionItems + Environment.NewLine + 	"menuSectionDocuments:" + menuSectionDocuments + Environment.NewLine);
	}
	}
}