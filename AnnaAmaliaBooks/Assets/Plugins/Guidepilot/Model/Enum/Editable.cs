//
//  Editable.cs
//  GuidePilot
//
//  Weather user can change state of UI element.
//
//  Generated by GuidePilot - EnumGenerator
//  Copyright © 2021 MicroMovie Media GmbH. All rights reserved.


using System;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace com.guidepilot.guidepilotcore {
	[Preserve]
	public enum Editable {
		Always,
		Never
	}

	[Preserve]
	public class EditableConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			Editable castedValue = (Editable)value;
			switch (castedValue) {
			case Editable.Always:
			writer.WriteValue("ALWAYS");break;
			case Editable.Never:
			writer.WriteValue("NEVER");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "ALWAYS":
			return Editable.Always;
			case "NEVER":
			return Editable.Never;
			}
			return null;
		}

		public override bool CanConvert(System.Type objectType) {
			return objectType == typeof(string);
		}
	}}
