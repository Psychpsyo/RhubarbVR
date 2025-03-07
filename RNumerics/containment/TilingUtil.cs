﻿using System;
using System.Collections.Generic;

namespace RNumerics
{
	public static class TilingUtil
	{


		/// <summary>
		/// Regular-grid tiling of element inside bounds, with spacing between elements
		/// Returns list of translations to element.
		/// Always allows at least one row and column, even if element overflows bounds in that dimension.
		/// </summary>
		public static List<Vector2d> BoundedRegularTiling2(AxisAlignedBox2d element, AxisAlignedBox2d bounds,
														   double spacing)
		{
            var oshift = -element.Min;
            var w = element.Width;
            var h = element.Height;

            var nx = Math.Max(1, (int)(bounds.Width / w));
            var spacew = (nx - 1) * spacing;
			while (nx > 1 && bounds.Width - ((w * nx) + spacew) < 0)
            {
                nx--;
            }

            var ny = Math.Max(1, (int)(bounds.Height / h));
            var spaceh = (ny - 1) * spacing;
			while (ny > 1 && bounds.Height - ((h * ny) + spaceh) < 0)
            {
                ny--;
            }

            var translations = new List<Vector2d>();
			for (var yi = 0; yi < ny; ++yi)
			{
                var dy = (yi * h) + (yi * spacing);
				for (var xi = 0; xi < nx; ++xi)
				{
                    var dx = (xi * w) + (xi * spacing);
					translations.Add(new Vector2d(dx, dy) + oshift + bounds.Min);
				}
			}

			return translations;
		}





		/// <summary>
		/// hex-grid tiling of circles inside bounds, with spacing between elements
		/// Returns list of translations to element.
		/// Always allows at least one row and column, even if element overflows bounds in that dimension.
		/// </summary>
		public static List<Vector2d> BoundedCircleTiling2(AxisAlignedBox2d element, AxisAlignedBox2d bounds,
															 double spacing)
		{
			var oshift = -element.Min;
			var w = element.Width;
			var h = element.Height;
			if (MathUtil.EpsilonEqual(w, h, MathUtil.EPSILONF) == false)
            {
                throw new Exception("BoundedHexTiling2: input box is not square");
            }

            // note: this is a circle tiling, not a hex tiling, so even though we are
            // starting in top-left with a "tip" hex, we don't have to offset down so that tip is inside
            // the box (circle cuts off)

            var r = w / 2;
            var hex = new Hexagon2d(element.Center, r, Hexagon2d.TopModes.Tip)
            {
                InnerRadius = r
            };

            var stepx = hex.HorzSpacing;
			var stepy = hex.VertSpacing;

			var spacingy = spacing;
			var spacingx = spacing;

			// half-rows on top and bottom add up to full row-height
			var ny = Math.Max(1, (int)(bounds.Height / stepy));
			// reduce count so that we fit w/ spacing
			var spaceh = (ny - 1) * spacingy;
            while (ny > 1 && bounds.Height - ((stepy * ny) + spaceh) < 0)
            {
                ny--;
            }

            // even rows start at x=0
            var nx_even = Math.Max(1, (int)(bounds.Width / stepx));
			// reduce count if we spill over w/ spacing
			var spacew = (nx_even - 1) * spacingx;
            while (nx_even > 1 && bounds.Width - ((stepx * nx_even) + spacew) < 0)
            {
                nx_even--;
            }

            // odd rows have an extra half-step added to left side,
            // so we may need to reduce count
            var nx_odd = nx_even;
			spacew = (nx_odd - 1) * spacingx;
            if (ny > 0 && ((stepx * nx_odd) + spacew + (stepx * 0.5)) > bounds.Width)
			{
				nx_odd--;
				//spacew = (nx_odd - 1) * spacingx;
			}


			var translations = new List<Vector2d>();
			for (var yi = 0; yi < ny; ++yi)
			{
                var dy = (yi * stepy) + (yi * spacingy);

				// x shift and count are different on odd rows
				var shiftx = stepx * 0.5;
				var nx = nx_odd;
				if (yi % 2 == 0)
				{
					shiftx = 0;
					nx = nx_even;
				}

				for (var xi = 0; xi < nx; ++xi)
				{
                    var dx = shiftx + (xi * stepx) + (xi * spacingx);
					translations.Add(new Vector2d(dx, dy) + oshift + bounds.Min);
				}
			}

			return translations;
		}

	}
}
