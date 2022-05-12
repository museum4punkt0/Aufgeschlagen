//
//  AppLifecycleEvent.cs
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
	public class AppLifecycleEvent: ContextEvent {
	//  

	[Preserve]
	public enum Verb {
		Launched,
		EnteredForeground,
		EnteredBackground,
		Terminated
	}

	[Preserve]
	public class VerbConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			AppLifecycleEvent.Verb castedValue = (AppLifecycleEvent.Verb)value;
			switch (castedValue) {
			case AppLifecycleEvent.Verb.Launched:
			writer.WriteValue("LAUNCHED");break;
			case AppLifecycleEvent.Verb.EnteredForeground:
			writer.WriteValue("ENTERED_FOREGROUND");break;
			case AppLifecycleEvent.Verb.EnteredBackground:
			writer.WriteValue("ENTERED_BACKGROUND");break;
			case AppLifecycleEvent.Verb.Terminated:
			writer.WriteValue("TERMINATED");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "LAUNCHED":
			return AppLifecycleEvent.Verb.Launched;
			case "ENTERED_FOREGROUND":
			return AppLifecycleEvent.Verb.EnteredForeground;
			case "ENTERED_BACKGROUND":
			return AppLifecycleEvent.Verb.EnteredBackground;
			case "TERMINATED":
			return AppLifecycleEvent.Verb.Terminated;
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