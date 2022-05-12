//
//  CalendarFilter.cs
//  GuidePilot
//
//  Special filters to configure the calendar view
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
	public class CalendarFilter: CoreLinkable, CoreObject, Sorted, Titled {
	//  Describes the meaning of the function of a filter.

	[Preserve]
	public enum Type {
		MaxCount,
		TimeRange,
		FullText
	}

	[Preserve]
	public class TypeConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			CalendarFilter.Type castedValue = (CalendarFilter.Type)value;
			switch (castedValue) {
			case CalendarFilter.Type.MaxCount:
			writer.WriteValue("MAX_COUNT");break;
			case CalendarFilter.Type.TimeRange:
			writer.WriteValue("TIME_RANGE");break;
			case CalendarFilter.Type.FullText:
			writer.WriteValue("FULL_TEXT");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "MAX_COUNT":
			return CalendarFilter.Type.MaxCount;
			case "TIME_RANGE":
			return CalendarFilter.Type.TimeRange;
			case "FULL_TEXT":
			return CalendarFilter.Type.FullText;
			}
			return null;
		}

		public override bool CanConvert(System.Type objectType) {
			return objectType == typeof(string);
		}
	}

	[JsonProperty("suuid")]
	private string internSuuid;
	[JsonIgnore]
	public string suuid { get => internSuuid; set => internSuuid = value; }
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("sort_index")]
	private int internSortIndex;
	[JsonIgnore]
	public int sortIndex { get => internSortIndex; set => internSortIndex = value; }
	[JsonProperty("title")]
	private Dictionary<string, object> internTitle;
	[JsonIgnore]
	public Dictionary<string, object> title { get => internTitle; set => internTitle = value; }
	[JsonProperty("title_short")]
	private Dictionary<string, object> internTitleShort;
	[JsonIgnore]
	public Dictionary<string, object> titleShort { get => internTitleShort; set => internTitleShort = value; }
[JsonConverter(typeof(TypeConverter))]
	[JsonProperty("type")]
	public Type type;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"type:" + type + Environment.NewLine);
	}
	}
}