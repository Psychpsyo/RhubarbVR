﻿using System;
using System.Collections.Generic;
using System.Text;

using RhuEngine.WorldObjects.ECS;

namespace RhuEngine.Components.PrivateSpace
{
	[PrivateSpaceOnly]
	public class RawAssetProvider<A>:AssetProvider<A> where A : class
	{
		public void LoadAsset(A asset) {
			Load(asset);
		}
	}
}
