//
//  ZoneConnectionDocument.cs
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
	public class ZoneConnectionDocument: CoreObject, DocumentRelation {
	//  Describes the relation of the a document to the zone connection.

	[Preserve]
	public enum Relation {
		Navigation
	}

	[Preserve]
	public class RelationConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			ZoneConnectionDocument.Relation castedValue = (ZoneConnectionDocument.Relation)value;
			switch (castedValue) {
			case ZoneConnectionDocument.Relation.Navigation:
			writer.WriteValue("NAVIGATION");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "NAVIGATION":
			return ZoneConnectionDocument.Relation.Navigation;
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