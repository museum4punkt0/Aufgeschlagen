//
//  MenuSectionDocument.cs
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
	public class MenuSectionDocument: CoreObject, DocumentRelation {
	[Preserve]
	public enum Relation {
		Media,
		MainIconImage,
		IconImage,
		MainImage,
		TargetIconImage
	}

	[Preserve]
	public class RelationConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			MenuSectionDocument.Relation castedValue = (MenuSectionDocument.Relation)value;
			switch (castedValue) {
			case MenuSectionDocument.Relation.Media:
			writer.WriteValue("MEDIA");break;
			case MenuSectionDocument.Relation.MainIconImage:
			writer.WriteValue("MAIN_ICON_IMAGE");break;
			case MenuSectionDocument.Relation.IconImage:
			writer.WriteValue("ICON_IMAGE");break;
			case MenuSectionDocument.Relation.MainImage:
			writer.WriteValue("MAIN_IMAGE");break;
			case MenuSectionDocument.Relation.TargetIconImage:
			writer.WriteValue("TARGET_ICON_IMAGE");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "MEDIA":
			return MenuSectionDocument.Relation.Media;
			case "MAIN_ICON_IMAGE":
			return MenuSectionDocument.Relation.MainIconImage;
			case "ICON_IMAGE":
			return MenuSectionDocument.Relation.IconImage;
			case "MAIN_IMAGE":
			return MenuSectionDocument.Relation.MainImage;
			case "TARGET_ICON_IMAGE":
			return MenuSectionDocument.Relation.TargetIconImage;
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