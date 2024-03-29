//
//  AnnotationDocument.cs
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
	public class AnnotationDocument: Anchored, CoreObject, DocumentRelation, Sized {
	//  Describes the relation of the a document to the annotation.

	[Preserve]
	public enum Relation {
		Media,
		Icon,
		SelectedIcon
	}

	[Preserve]
	public class RelationConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			AnnotationDocument.Relation castedValue = (AnnotationDocument.Relation)value;
			switch (castedValue) {
			case AnnotationDocument.Relation.Media:
			writer.WriteValue("MEDIA");break;
			case AnnotationDocument.Relation.Icon:
			writer.WriteValue("ICON");break;
			case AnnotationDocument.Relation.SelectedIcon:
			writer.WriteValue("SELECTED_ICON");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "MEDIA":
			return AnnotationDocument.Relation.Media;
			case "ICON":
			return AnnotationDocument.Relation.Icon;
			case "SELECTED_ICON":
			return AnnotationDocument.Relation.SelectedIcon;
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
	[JsonProperty("anchor_h")]
	private double? internAnchorH;
	[JsonIgnore]
	public double? anchorH { get => internAnchorH; set => internAnchorH = value; }
	[JsonProperty("anchor_v")]
	private double? internAnchorV;
	[JsonIgnore]
	public double? anchorV { get => internAnchorV; set => internAnchorV = value; }
	[JsonProperty("anchor_z")]
	private double? internAnchorZ;
	[JsonIgnore]
	public double? anchorZ { get => internAnchorZ; set => internAnchorZ = value; }
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("document_id")]
	private int internDocumentId;
	[JsonIgnore]
	public int documentId { get => internDocumentId; set => internDocumentId = value; }
	[JsonProperty("coordinate_width")]
	private double? internCoordinateWidth;
	[JsonIgnore]
	public double? coordinateWidth { get => internCoordinateWidth; set => internCoordinateWidth = value; }
	[JsonProperty("coordinate_height")]
	private double? internCoordinateHeight;
	[JsonIgnore]
	public double? coordinateHeight { get => internCoordinateHeight; set => internCoordinateHeight = value; }
[JsonConverter(typeof(RelationConverter))]
	[JsonProperty("relation")]
	public Relation relation;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"relation:" + relation + Environment.NewLine);
	}
	}
}