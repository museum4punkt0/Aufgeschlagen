//
//  Inventory.cs
//  GuidePilot
//
//  Kunstobjekte
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
	public class Inventory: CoreLinkable, CoreObject, Named {
	//  Describes the kind of the inventory object (e.g. a painting or sculpture)

	[Preserve]
	public enum Type {
		Painting,
		Drawing,
		Sculpture,
		Photo,
		Video,
		Engraving,
		Etching,
		Lithograph,
		Other
	}

	[Preserve]
	public class TypeConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			Inventory.Type castedValue = (Inventory.Type)value;
			switch (castedValue) {
			case Inventory.Type.Painting:
			writer.WriteValue("PAINTING");break;
			case Inventory.Type.Drawing:
			writer.WriteValue("DRAWING");break;
			case Inventory.Type.Sculpture:
			writer.WriteValue("SCULPTURE");break;
			case Inventory.Type.Photo:
			writer.WriteValue("PHOTO");break;
			case Inventory.Type.Video:
			writer.WriteValue("VIDEO");break;
			case Inventory.Type.Engraving:
			writer.WriteValue("ENGRAVING");break;
			case Inventory.Type.Etching:
			writer.WriteValue("ETCHING");break;
			case Inventory.Type.Lithograph:
			writer.WriteValue("LITHOGRAPH");break;
			case Inventory.Type.Other:
			writer.WriteValue("OTHER");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "PAINTING":
			return Inventory.Type.Painting;
			case "DRAWING":
			return Inventory.Type.Drawing;
			case "SCULPTURE":
			return Inventory.Type.Sculpture;
			case "PHOTO":
			return Inventory.Type.Photo;
			case "VIDEO":
			return Inventory.Type.Video;
			case "ENGRAVING":
			return Inventory.Type.Engraving;
			case "ETCHING":
			return Inventory.Type.Etching;
			case "LITHOGRAPH":
			return Inventory.Type.Lithograph;
			case "OTHER":
			return Inventory.Type.Other;
			}
			return null;
		}

		public override bool CanConvert(System.Type objectType) {
			return objectType == typeof(string);
		}
	}

	[JsonProperty("suuid")]
	private string internSuuid;
	[JsonIgnore]
	public string suuid { get => internSuuid; set => internSuuid = value; }
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("name")]
	private Dictionary<string, object> internName;
	[JsonIgnore]
	public Dictionary<string, object> name { get => internName; set => internName = value; }
[JsonConverter(typeof(TypeConverter))]
	[JsonProperty("type")]
	public Type type;
	[JsonProperty("actor_roles")]
	public ActorRole[] actorRoles;
	[JsonProperty("inventory_properties")]
	public InventoryProperty[] inventoryProperties;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"type:" + type + Environment.NewLine + 	"actorRoles:" + actorRoles + Environment.NewLine + 	"inventoryProperties:" + inventoryProperties + Environment.NewLine);
	}
	}
}