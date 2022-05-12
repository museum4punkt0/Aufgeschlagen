//
//  CameraPermissionEvent.cs
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
	public class CameraPermissionEvent: PermissionEvent {
	//  

	[Preserve]
	public enum Verb {
		Granted,
		Denied
	}

	[Preserve]
	public class VerbConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			CameraPermissionEvent.Verb castedValue = (CameraPermissionEvent.Verb)value;
			switch (castedValue) {
			case CameraPermissionEvent.Verb.Granted:
			writer.WriteValue("GRANTED");break;
			case CameraPermissionEvent.Verb.Denied:
			writer.WriteValue("DENIED");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "GRANTED":
			return CameraPermissionEvent.Verb.Granted;
			case "DENIED":
			return CameraPermissionEvent.Verb.Denied;
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
[JsonConverter(typeof(VerbConverter))]
	[JsonProperty("verb")]
	public Verb verb;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"verb:" + verb + Environment.NewLine);
	}
	}
}