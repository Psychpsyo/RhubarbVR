﻿using RhuEngine.WorldObjects;
using RhuEngine.WorldObjects.ECS;

namespace RhuEngine.Components
{
	[Category(new string[] { "CoreData" })]
	public class ValueField<T> : Component
	{
		public Sync<T> Value;
	}
}
