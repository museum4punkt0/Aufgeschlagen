//
//  DocumentRelation.cs
//  GuidePilot
//
//  Object, that links an document.
//
//  Generated by GuidePilot - EnumGenerator
//  Copyright © 2021 MicroMovie Media GmbH. All rights reserved.


using System;
using System.Collections.Generic;

namespace com.guidepilot.guidepilotcore {

	public interface DocumentRelation: Prioritized {
	 int documentId { get; set; }
	}
}