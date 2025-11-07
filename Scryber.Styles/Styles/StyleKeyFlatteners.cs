using System;
using System.ComponentModel;
using Scryber.Drawing;

namespace Scryber.Styles
{
	/// <summary>
	/// Delegate methods that can flatten (make absolute) style values.
	/// </summary>
	public static class StyleKeyFlatteners
	{

        public static void FlattenRelativeNotAllowedValue(Style style, StyleKey<Unit> key, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            StyleValue<Unit> value;
            if (style.TryGetValue(key, out value))
            {
                Unit dim = value.Value(style);
                if (dim.IsRelative)
                {
                    throw new InvalidOperationException("Relative units are not allowed on the style property '" + key.ToString() + "'. The value '" + dim + "' cannot be used");
                }
                else
                {
                    value.SetValue(dim);
                }
            }
        }

        /// <summary>
        /// Flattens a style key with preference given to the horizontal container on percent
        /// </summary>
        /// <param name="style"></param>
        /// <param name="key"></param>
        /// <param name="pageSize"></param>
        /// <param name="containerSize"></param>
        /// <param name="fontSize"></param>
        /// <param name="rootFontSize"></param>
        public static void FlattenHorizontalPositionValue(Style style, StyleKey<Unit> key, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            StyleValue<Unit> value;
            if (style.TryGetValue(key, out value))
            {
                Unit dim = value.Value(style);
                Unit result;
                if (dim.IsRelative)
                {
                    result = FlattenHorizontalUnit(dim, pageSize, containerSize, fontSize, rootFontSize);
                    value.SetValue(result);
                }
                else
                {
                    value.SetValue(dim);
                }
            }
        }

        private static Unit FlattenHorizontalUnit(Unit dim, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            Unit result;
            switch (dim.Units)
            {
                case PageUnits.Percent:
                    result = dim.ToAbsolute(containerSize.Width);
                    break;
                case PageUnits.EMHeight:
                    result = dim.ToAbsolute(fontSize.Height);
                    break;
                case PageUnits.EXHeight:
                    //HACK: Assuming the width of a zero is the same size as an 'x' character high. Not usually a bad assumption
                    result = dim.ToAbsolute(fontSize.Width);
                    break;
                case PageUnits.ZeroWidth:
                    result = dim.ToAbsolute(fontSize.Width);
                    break;
                case PageUnits.RootEMHeight:
                    result = dim.ToAbsolute(rootFontSize);
                    break;
                case PageUnits.ViewPortWidth:
                    result = dim.ToAbsolute(pageSize.Width);
                    break;
                case PageUnits.ViewPortHeight:
                    result = dim.ToAbsolute(pageSize.Height);
                    break;
                case PageUnits.ViewPortMin:
                    Unit min = Unit.Min(pageSize.Height, pageSize.Width);
                    result = dim.ToAbsolute(min);
                    break;
                case PageUnits.ViewPortMax:
                    Unit max = Unit.Max(pageSize.Height, pageSize.Width);
                    result = dim.ToAbsolute(max);
                    break;
                default:
                    result = dim;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Flattens a style key with preference given to the horizontal container on percent
        /// </summary>
        /// <param name="style"></param>
        /// <param name="key"></param>
        /// <param name="pageSize"></param>
        /// <param name="containerSize"></param>
        /// <param name="fontSize"></param>
        /// <param name="rootFontSize"></param>
        public static void FlattenVerticalPositionValue(Style style, StyleKey<Unit> key, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            StyleValue<Unit> value;
            if (style.TryGetValue(key, out value))
            {
                Unit dim = value.Value(style);
                Unit result;
                if (dim.IsRelative)
                {
                    result = FlattenVerticalUnit(dim, pageSize, containerSize, fontSize, rootFontSize);
                    value.SetValue(result);
                }
                else
                {
                    value.SetValue(dim);
                }
            }
        }

        private static Unit FlattenVerticalUnit(Unit dim, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            Unit result;
            switch (dim.Units)
            {
                case PageUnits.Percent:
                    result = dim.ToAbsolute(containerSize.Height);
                    break;
                case PageUnits.EMHeight:
                    result = dim.ToAbsolute(fontSize.Height);
                    break;
                case PageUnits.EXHeight:
                    //HACK: Assuming the width of a zero is the same size as an 'x' character high. Not usually a bad assumption
                    result = dim.ToAbsolute(fontSize.Width);
                    break;
                case PageUnits.ZeroWidth:
                    result = dim.ToAbsolute(fontSize.Width);
                    break;
                case PageUnits.RootEMHeight:
                    result = dim.ToAbsolute(rootFontSize);
                    break;
                case PageUnits.ViewPortWidth:
                    result = dim.ToAbsolute(pageSize.Width);
                    break;
                case PageUnits.ViewPortHeight:
                    result = dim.ToAbsolute(pageSize.Height);
                    break;
                case PageUnits.ViewPortMin:
                    Unit min = Unit.Min(pageSize.Height, pageSize.Width);
                    result = dim.ToAbsolute(min);
                    break;
                case PageUnits.ViewPortMax:
                    Unit max = Unit.Max(pageSize.Height, pageSize.Width);
                    result = dim.ToAbsolute(max);
                    break;
                default:
                    result = dim;
                    break;
            }

            return result;
        }
        



        public static void FlattenPaddingAllThicknessPositionValue(Style style, StyleKey<Unit> key, Size pageSize, Size containerSize, Size fontSize, Unit rootFontHeight)
        {
            //As it is padding if we are using a percent then the top and bottom use the vertical, and left and right
            //use the horizontal.
            if (key != StyleKeys.PaddingAllKey)
                throw new InvalidOperationException("The expected key is Padding all, for this method");
            StyleValue<Unit> all;
            if (style.TryGetValue(key, out all))
            {
                Unit dim = all.Value(style);
                if (dim.IsRelative)
                {
                    Unit vert = FlattenVerticalUnit(dim, pageSize, containerSize, fontSize, rootFontHeight);
                    all.SetValue(vert);
                }
                else
                {
                    all.SetValue(dim);
                }
            }
            
        }

        public static void FlattenMarginAllThicknessPositionValue(Style style, StyleKey<Unit> key, Size pageSize, Size containerSize, Size fontSize, Unit rootFontHeight)
        {
            if (key != StyleKeys.MarginsAllKey)
                throw new InvalidOperationException("The expected key is Padding all, for this method");

            StyleValue<Unit> all;
            if (style.TryGetValue(key, out all))
            {
                Unit dim = all.Value(style);
                if (dim.IsRelative)
                {
                    Unit vert = FlattenVerticalUnit(dim, pageSize, containerSize, fontSize, rootFontHeight);
                    all.SetValue(vert);

                }
                else
                {
                    all.SetValue(dim);
                }
            }
        }

        /// <summary>
        /// Flattens a style key with preference given to the horizontal container on percent
        /// </summary>
        /// <param name="style"></param>
        /// <param name="key"></param>
        /// <param name="pageSize"></param>
        /// <param name="containerSize"></param>
        /// <param name="fontSize"></param>
        /// <param name="rootFontSize"></param>
        public static void FlattenFontSizeValue(Style style, StyleKey<Unit> key, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            StyleValue<Unit> value;
            if (style.TryGetValue(key, out value))
            {
                Unit dim = value.Value(style);
                Unit result;
                if (dim.IsRelative)
                {
                    result = FlattenFontUnit(dim, pageSize, fontSize, rootFontSize);
                    value.SetValue(result);
                }
                else
                {
                    value.SetValue(dim);
                }
            }
        }

        private static Unit FlattenFontUnit(Unit dim, Size pageSize, Size fontSize, Unit rootFontSize)
        {
            Unit result;
            switch (dim.Units)
            {
                case PageUnits.Percent:
                    result = dim.ToAbsolute(fontSize.Height);
                    break;
                case PageUnits.EMHeight:
                    result = dim.ToAbsolute(fontSize.Height);
                    break;
                case PageUnits.EXHeight:
                    //HACK: Assuming the width of a zero is the same size as an 'x' character high. Not usually a bad assumption
                    result = dim.ToAbsolute(fontSize.Width);
                    break;
                case PageUnits.ZeroWidth:
                    result = dim.ToAbsolute(fontSize.Width);
                    break;
                case PageUnits.RootEMHeight:
                    result = dim.ToAbsolute(rootFontSize);
                    break;
                case PageUnits.ViewPortWidth:
                    result = dim.ToAbsolute(pageSize.Width);
                    break;
                case PageUnits.ViewPortHeight:
                    result = dim.ToAbsolute(pageSize.Height);
                    break;
                case PageUnits.ViewPortMin:
                    Unit min = Unit.Min(pageSize.Height, pageSize.Width);
                    result = dim.ToAbsolute(min);
                    break;
                case PageUnits.ViewPortMax:
                    Unit max = Unit.Max(pageSize.Height, pageSize.Width);
                    result = dim.ToAbsolute(max);
                    break;
                default:
                    result = dim;
                    break;
            }

            return result;
        }


        /// <summary>
        /// Flattens the viewport rect
        /// </summary>
        /// <param name="style"></param>
        /// <param name="key"></param>
        /// <param name="pageSize"></param>
        /// <param name="containerSize"></param>
        /// <param name="fontSize"></param>
        /// <param name="rootFontSize"></param>
        public static void FlattenRectValue(Style style, StyleKey<Rect> key, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            StyleValue<Rect> value;
            if (style.TryGetValue(key, out value))
            {
                Rect dim = value.Value(style);

                if (dim.IsRelative)
                {
                    Rect flat = new Rect();
                    flat.X = FlattenHorizontalUnit(dim.X, pageSize, containerSize, fontSize, rootFontSize);
                    flat.Y = FlattenVerticalUnit(dim.Y, pageSize, containerSize, fontSize, rootFontSize);
                    flat.Width = FlattenHorizontalUnit(dim.Width, pageSize, containerSize, fontSize, rootFontSize);
                    flat.Height = FlattenVerticalUnit(dim.Height, pageSize, containerSize, fontSize, rootFontSize);

                    value.SetValue(flat);
                }
                else
                {
                    value.SetValue(dim);
                }
            }

            
        }

    }
}

