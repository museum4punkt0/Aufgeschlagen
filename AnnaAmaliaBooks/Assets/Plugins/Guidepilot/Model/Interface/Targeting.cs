//
//  Targeting.cs
//  GuidePilot
//
//  Entities, that are able to target a different object or list of objects
//
//  Generated by GuidePilot - EnumGenerator
//  Copyright © 2021 MicroMovie Media GmbH. All rights reserved.


using System;
using System.Collections.Generic;

namespace com.guidepilot.guidepilotcore {

	public interface Targeting {
	 TargetType targetType { get; set; }
	 string targetUuid { get; set; }
	 Dictionary<string, object> targetData { get; set; }
Language[] targetLocales { get; set; }
	 string targetScopeUuid { get; set; }
	}
}