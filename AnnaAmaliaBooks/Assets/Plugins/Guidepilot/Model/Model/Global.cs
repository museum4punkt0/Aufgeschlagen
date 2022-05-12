//
//  Global.cs
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
	public class Global: CoreObject {
	[JsonProperty("id")]
	private int internId;
	[JsonIgnore]
	public int id { get => internId; set => internId = value; }
	[JsonProperty("locales")]
	public string[] locales;
	[JsonProperty("partial_locales")]
	public string[] partialLocales;
	[JsonProperty("default_timezone")]
	public string defaultTimezone;
	[JsonProperty("default_currency")]
	public string defaultCurrency;
	[JsonProperty("analytics_configurations")]
	public AnalyticsConfiguration[] analyticsConfigurations;
	public void print() {
		Debug.Log("object:" + Environment.NewLine + 	"locales:" + locales + Environment.NewLine + 	"partialLocales:" + partialLocales + Environment.NewLine + 	"defaultTimezone:" + defaultTimezone + Environment.NewLine + 	"defaultCurrency:" + defaultCurrency + Environment.NewLine + 	"analyticsConfigurations:" + analyticsConfigurations + Environment.NewLine);
	}
	}
}