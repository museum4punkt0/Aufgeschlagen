//
//  MenuDocument.cs
//  GuidePilot
//
//  menu documents
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
	public class MenuDocument: CoreObject, DocumentRelation {
	[Preserve]
	public enum Relation {
		Media,
		IconImage,
		MainImage
	}

	[Preserve]
	public class RelationConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			MenuDocument.Relation castedValue = (MenuDocument.Relation)value;
			switch (castedValue) {
			case MenuDocument.Relation.Media:
			writer.WriteValue("MEDIA");break;
			case MenuDocument.Relation.IconImage:
			writer.WriteValue("ICON_IMAGE");break;
			case MenuDocument.Relation.MainImage:
			writer.WriteValue("MAIN_IMAGE");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "MEDIA":
			return MenuDocument.Relation.Media;
			case "ICON_IMAGE":
			return MenuDocument.Relation.IconImage;
			case "MAIN_IMAGE":
			return MenuDocument.Relation.MainImage;
			}
			return null;
		}

		public override bool CanConvert(System.Type objectType) {
			return objectType == typeof(string);
		}
	}

	[JsonProperty("priority")]
	private int internPriority;
	[JsonIgnore]
	public int priority { get => internPriority; set => internPriority = value; }
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("document_id")]
	private int internDocumentId;
	[JsonIgnore]
	public int documentId { get => internDocumentId; set => internDocumentId = value; }
[JsonConverter(typeof(RelationConverter))]
	[JsonProperty("relation")]
	public Relation relation;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"relation:" + relation + Environment.NewLine);
	}
	}
}