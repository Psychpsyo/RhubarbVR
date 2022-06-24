﻿using RhuEngine.WorldObjects;
using RhuEngine.WorldObjects.ECS;

using RhuEngine.Components.ScriptNodes;
using System;
using RhuEngine.DataStructure;
using SharedModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RNumerics;

namespace RhuEngine.Components
{
	[Exposed]
	public static class RhuScriptStatics
	{
		public static string ToString(object value) {
			return value.ToString();
		}
		public static int Add(int value1,int value2) {
			return value1 + value2;
		}
	}
}
