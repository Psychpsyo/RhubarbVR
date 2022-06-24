﻿using System;
using System.Collections.Generic;
using System.Text;

using RhuEngine.Linker;
using RhuEngine.Components;
using RhuEngine.WorldObjects.ECS;
using System.Linq;
using StereoKit;
using RNumerics;
using RhuEngine;
using RhuEngine.WorldObjects;

namespace RStereoKit
{
	public class UIRender : RenderLinkBase<UICanvas>
	{
		public override void Init() {
		}

		public override void Remove() {
		}

		public override void Render() {
			RenderingComponent?.RenderUI();
		}

		public override void Started() {
		}

		public override void Stopped() {
		}
	}

	public class SKTextRender : RenderLinkBase<WorldText>
	{
		public override void Init() {
			//Not needed for StereoKit
		}

		public override void Remove() {
			//Not needed for StereoKit
		}

		public override void Render() {
			RenderingComponent.textRender.Render(RNumerics.Matrix.Identity, RenderingComponent.Entity.GlobalTrans);
		}

		public override void Started() {
			//Not needed for StereoKit
		}

		public override void Stopped() {
			//Not needed for StereoKit
		}
	}

	public class SKMeshRender : RenderLinkBase<MeshRender>
	{
		public override void Init() {
			//Not needed for StereoKit
		}

		public override void Remove() {
			//Not needed for StereoKit
		}

		public override void Render() {
			if (RenderingComponent.mesh.Asset != null) {
				SKRender(RenderingComponent.mesh.Asset, RenderingComponent.materials,RenderingComponent.Entity.GlobalTrans, RenderingComponent.colorLinear.Value, RenderingComponent.renderLayer.Value);
			}
		}

		private void SKRender(RMesh rMesh,IEnumerable<ISyncObject> mits, RNumerics.Matrix globalTrans,Colorf color,RhuEngine.Linker.RenderLayer layer) {
			foreach (AssetRef<RMaterial> item in mits) {
				if (item.Asset != null) {
					rMesh?.Draw("NUllNotNeeded",item.Asset,globalTrans,color, RenderingComponent.OrderOffset.Value);
				}
			}
		}

		public override void Started() {
			//Not needed for StereoKit
		}

		public override void Stopped() {
			//Not needed for StereoKit
		}
	}
}
