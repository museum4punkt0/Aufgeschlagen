//
//  ZoneEvent.cs
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
	public class ZoneEvent: EntityEvent {
	//  

	[Preserve]
	public enum Verb {
		Entered,
		Left
	}

	[Preserve]
	public class VerbConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			ZoneEvent.Verb castedValue = (ZoneEvent.Verb)value;
			switch (castedValue) {
			case ZoneEvent.Verb.Entered:
			writer.WriteValue("ENTERED");break;
			case ZoneEvent.Verb.Left:
			writer.WriteValue("LEFT");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "ENTERED":
			return ZoneEvent.Verb.Entered;
			case "LEFT":
			return ZoneEvent.Verb.Left;
			}
			return null;
		}

		public override bool CanConvert(System.Type objectType) {
			return objectType == typeof(string);
		}
	}

[JsonConverter(typeof(DictionaryConverter))]
	[JsonProperty("custom_data")]
	private Dictionary<string, object> internCustomData;
	[JsonIgnore]
	public Dictionary<string, object> customData { get => internCustomData; set => internCustomData = value; }
	[JsonProperty("timestamp")]
	private DateTime internTimestamp;
	[JsonIgnore]
	public DateTime timestamp { get => internTimestamp; set => internTimestamp = value; }
	[JsonProperty("collision_hash")]
	private string internCollisionHash;
	[JsonIgnore]
	public string collisionHash { get => internCollisionHash; set => internCollisionHash = value; }
	[JsonProperty("session_uuid")]
	private string internSessionUuid;
	[JsonIgnore]
	public string sessionUuid { get => internSessionUuid; set => internSessionUuid = value; }
	[JsonProperty("installation_uuid")]
	private string internInstallationUuid;
	[JsonIgnore]
	public string installationUuid { get => internInstallationUuid; set => internInstallationUuid = value; }
	[JsonProperty("entity_suuid")]
	private string internEntitySuuid;
	[JsonIgnore]
	public string entitySuuid { get => internEntitySuuid; set => internEntitySuuid = value; }
	[JsonProperty("main_package_revision")]
	private string internMainPackageRevision;
	[JsonIgnore]
	public string mainPackageRevision { get => internMainPackageRevision; set => internMainPackageRevision = value; }
[JsonConverter(typeof(VerbConverter))]
	[JsonProperty("verb")]
	public Verb verb;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"verb:" + verb + Environment.NewLine);
	}
	}
}