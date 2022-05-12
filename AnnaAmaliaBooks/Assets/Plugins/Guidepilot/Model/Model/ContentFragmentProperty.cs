//
//  ContentFragmentProperty.cs
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
	public class ContentFragmentProperty: CoreObject, Sorted {
	//  Describes the meaning of the content fragment property.

	[Preserve]
	public enum Type {
		Title
	}

	[Preserve]
	public class TypeConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			ContentFragmentProperty.Type castedValue = (ContentFragmentProperty.Type)value;
			switch (castedValue) {
			case ContentFragmentProperty.Type.Title:
			writer.WriteValue("TITLE");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "TITLE":
			return ContentFragmentProperty.Type.Title;
			}
			return null;
		}

		public override bool CanConvert(System.Type objectType) {
			return objectType == typeof(string);
		}
	}

	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("sort_index")]
	private int internSortIndex;
	[JsonIgnore]
	public int sortIndex { get => internSortIndex; set => internSortIndex = value; }
[JsonConverter(typeof(TypeConverter))]
	[JsonProperty("type")]
	public Type type;
[JsonConverter(typeof(DataTypeConverter))]
	[JsonProperty("data_type")]
	public DataType dataType;
	[JsonProperty("value")]
	public Dictionary<string, object> value;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"type:" + type + Environment.NewLine + 	"dataType:" + dataType + Environment.NewLine + 	"value:" + value + Environment.NewLine);
	}
	}
}