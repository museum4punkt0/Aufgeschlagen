//
//  ActorRole.cs
//  GuidePilot
//
//  actor role
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
	public class ActorRole: CoreObject, Prioritized {
	//  Describes the relation of the actor to the inventory.

	[Preserve]
	public enum Relation {
		Artist,
		Owner,
		Lender
	}

	[Preserve]
	public class RelationConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			ActorRole.Relation castedValue = (ActorRole.Relation)value;
			switch (castedValue) {
			case ActorRole.Relation.Artist:
			writer.WriteValue("ARTIST");break;
			case ActorRole.Relation.Owner:
			writer.WriteValue("OWNER");break;
			case ActorRole.Relation.Lender:
			writer.WriteValue("LENDER");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "ARTIST":
			return ActorRole.Relation.Artist;
			case "OWNER":
			return ActorRole.Relation.Owner;
			case "LENDER":
			return ActorRole.Relation.Lender;
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
	[JsonProperty("priority")]
	private int internPriority;
	[JsonIgnore]
	public int priority { get => internPriority; set => internPriority = value; }
	[JsonProperty("actor_id")]
	public int actorId;
[JsonConverter(typeof(RelationConverter))]
	[JsonProperty("relation")]
	public Relation relation;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"actorId:" + actorId + Environment.NewLine + 	"relation:" + relation + Environment.NewLine);
	}
	}
}