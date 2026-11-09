
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scryber.Drawing
{
	
	public delegate Unit RelativeToAbsoluteDimensionCallback(Unit relative);
	
	public static class UnitRelativeVars
	{
		public const string RelativeVarPrefix = "__";
		public const string PageWidth = RelativeVarPrefix + "PageWidth";
		public const string PageHeight = RelativeVarPrefix + "PageHeight";
		public const string ContainerWidth = RelativeVarPrefix + "ContainerWidth";
		public const string ContainerHeight = RelativeVarPrefix + "ContainerHeight";
		public const string FontUpperHeight = RelativeVarPrefix + "FontUpperHeight";
		public const string FontLowercaseHeight = RelativeVarPrefix + "FontLowercaseHeight";
		public const string FontRootUpperHeight = RelativeVarPrefix + "FontRootUpperHeight";
		public const string FontStandardWidth = RelativeVarPrefix + "FontWidth";
		public const string WidthIsPriority = RelativeVarPrefix + "WidthPriority";

		public const string RelativeCallbackVar = RelativeVarPrefix + "RelativeCallback";

		public static bool FillCallbackVar(Dictionary<string, object> vars, RelativeToAbsoluteDimensionCallback callback)
		{
			vars[RelativeCallbackVar] = callback;
			return true;
		}

		public static void ClearCallbackVar(Dictionary<string, object> vars)
		{
			vars.Remove(RelativeCallbackVar);
		}

		public static bool FillCSSVars(Dictionary<string, object> vars, Size page, Size container, Unit fontUpperHeight, Unit fontLowerHeight, Unit fontLowerWidth, Unit rootFontUpperHeight, bool isWidthPriority)
		{
			vars[PageWidth] = page.Width;
			vars[PageHeight] = page.Height;
			vars[ContainerWidth] = container.Width;
			vars[ContainerHeight] = container.Height;
			vars[FontUpperHeight] = fontUpperHeight;
			vars[FontLowercaseHeight] = fontLowerHeight;
			vars[FontStandardWidth] = fontLowerWidth;
			vars[FontRootUpperHeight] = rootFontUpperHeight;
			vars[WidthIsPriority] = isWidthPriority;

			return true;
		}

	}
}

