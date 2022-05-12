//
//  ControlEvent.cs
//  GuidePilot
//
//  Event about a view or screen
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
	public class ControlEvent: UiEvent {
	//  

	[Preserve]
	public enum Verb {
		Selected,
		Appeared,
		Disappeared
	}

	[Preserve]
	public class VerbConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			ControlEvent.Verb castedValue = (ControlEvent.Verb)value;
			switch (castedValue) {
			case ControlEvent.Verb.Selected:
			writer.WriteValue("SELECTED");break;
			case ControlEvent.Verb.Appeared:
			writer.WriteValue("APPEARED");break;
			case ControlEvent.Verb.Disappeared:
			writer.WriteValue("DISAPPEARED");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "SELECTED":
			return ControlEvent.Verb.Selected;
			case "APPEARED":
			return ControlEvent.Verb.Appeared;
			case "DISAPPEARED":
			return ControlEvent.Verb.Disappeared;
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
	[JsonProperty("locale")]
	private string internLocale;
	[JsonIgnore]
	public string locale { get => internLocale; set => internLocale = value; }
	[JsonProperty("name")]
	private string internName;
	[JsonIgnore]
	public string name { get => internName; set => internName = value; }
[JsonConverter(typeof(VerbConverter))]
	[JsonProperty("verb")]
	public Verb verb;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"verb:" + verb + Environment.NewLine);
	}
	}
}