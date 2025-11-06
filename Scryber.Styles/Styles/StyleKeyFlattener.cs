using System;
using Scryber.Drawing;

namespace Scryber.Styles
{
    
    
    
    /// <summary>
    /// Base abstract implementation of the IFlattenStyleKeyValue&lt;T&gt; interface
    /// </summary>
    /// <typeparam name="T">The type of the key and value this flattener supports</typeparam>
    public abstract class StyleKeyFlattenValue<T> : IStyleKeyFlattenValue <T>
    {
        
            
        /// <summary>
        /// Flag that defines if any relative units converted should use the container or page width in preference to the height of the container or page. Does not override the actual relative dimensions directive.
        /// E.g. 'width: 20%;' would use the width of the container as a preference to the height. Whereas 'width: 0.2vh' would still honour the height of the page.
        /// </summary>
        public PreferredFlattenDimension PreferredDimension { get; protected set; }

        
        public StyleKeyFlattenValue(PreferredFlattenDimension preferDimension)
        {
            this.PreferredDimension = preferDimension;
        }
 
        public abstract void SetFlattenedValue(Style onStyle, StyleKey<T> key, Size page, Size container, Size font, Unit rootFont);

        public abstract T FlattenValue(T relative, Size page, Size container, Size font, Unit rootFont);
            
            
    }
    
    #region public class StyleKeyFlattenUnitValue : StyleKeyFlattenValue<Unit>
    
    /// <summary>
    /// Implements the flattening of individual Unit dimension values
    /// </summary>
    public class StyleKeyFlattenUnitValue : StyleKeyFlattenValue<Unit>
    {
        public StyleKeyFlattenUnitValue(PreferredFlattenDimension preferDimension) :
            base(preferDimension)
        {
        }

        public override Unit FlattenValue(Unit dim, Size page, Size container, Size font, Unit rootFont)
        {
            Unit flat;
            switch (this.PreferredDimension)
            {
                case(PreferredFlattenDimension.Width):
                    flat = FlattenHorizontalValue(dim, page, container, font, rootFont);
                    break;
                case(PreferredFlattenDimension.Height):
                    flat = FlattenVerticalValue(dim, page, container, font, rootFont);
                    break;
                case(PreferredFlattenDimension.Font):
                    flat = FlattenFontValue(dim, page, container, font, rootFont);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(this.PreferredDimension));
            }
            
            return flat;
        }

        public static Unit FlattenHorizontalValue(Unit dim, Size page, Size container, Size font, Unit rootFont)
        {
            return Unit.FlattenHorizontalValue(dim, page, container, font, rootFont);
        }
        
        public static Unit FlattenVerticalValue(Unit dim, Size page, Size container, Size font, Unit rootFont)
        {
            return Unit.FlattenVerticalValue(dim, page, container, font, rootFont);
        }

        public static Unit FlattenFontValue(Unit dim, Size page, Size container, Size font, Unit rootFont)
        {
            Unit result;
            switch (dim.Units)
            {
                case PageUnits.Percent:
                    result = dim.ToAbsolute(font.Height);
                    break;
                case PageUnits.EMHeight:
                    result = dim.ToAbsolute(font.Height);
                    break;
                case PageUnits.EXHeight:
                    //HACK: Assuming the width of a zero is the same size as an 'x' character high. Not usually a bad assumption
                    result = dim.ToAbsolute(font.Width);
                    break;
                case PageUnits.ZeroWidth:
                    result = dim.ToAbsolute(font.Width);
                    break;
                case PageUnits.RootEMHeight:
                    result = dim.ToAbsolute(rootFont);
                    break;
                case PageUnits.ViewPortWidth:
                    result = dim.ToAbsolute(page.Width);
                    break;
                case PageUnits.ViewPortHeight:
                    result = dim.ToAbsolute(page.Height);
                    break;
                case PageUnits.ViewPortMin:
                    Unit min = Unit.Min(page.Height, page.Width);
                    result = dim.ToAbsolute(min);
                    break;
                case PageUnits.ViewPortMax:
                    Unit max = Unit.Max(page.Height, page.Width);
                    result = dim.ToAbsolute(max);
                    break;
                default:
                    result = dim;
                    break;
            }

            return result;
        }
        
        public override void SetFlattenedValue(Style onStyle, StyleKey<Unit> key, Size page, Size container, Size font, Unit rootFont)
        {
            StyleValue<Unit> curr;
            if (onStyle.TryGetValue(key, out curr))
            {
                Unit dim = curr.Value(onStyle);
                if (dim.IsRelative)
                {
                    Unit flat = this.FlattenValue(dim, page, container, font, rootFont);

                    curr.SetValue(flat);
                    
                }
            }
        }
    }
    
    #endregion

    public class StyleKeyFlattenThicknessValue : StyleKeyFlattenValue<Unit>
    {
        public StyleKey<Unit> Left { get; private set; }
        public StyleKey<Unit> Top { get; private set; }
        public StyleKey<Unit> Right { get; private set; }
        public StyleKey<Unit> Bottom { get; private set; }
        
        public StyleKeyFlattenThicknessValue(StyleKey<Unit> left, StyleKey<Unit> top,
            StyleKey<Unit> right, StyleKey<Unit> bottom)
            : base(PreferredFlattenDimension.Width)
        {
            this.Left = left ?? throw new ArgumentNullException(nameof(left));
            this.Top = top ?? throw new ArgumentNullException(nameof(top));
            this.Right = right ?? throw new ArgumentNullException(nameof(right));
            this.Bottom = bottom ?? throw new ArgumentNullException(nameof(bottom));
        }

        public override Unit FlattenValue(Unit relative, Size page, Size container, Size font, Unit rootFont)
        {
            return StyleKeyFlattenUnitValue.FlattenHorizontalValue(relative, page, container, font, rootFont);
        }

        public override void SetFlattenedValue(Style onStyle, StyleKey<Unit> allKey, Size page, Size container, Size font, Unit rootFont)
        {
            StyleValue<Unit> all;
            if (onStyle.TryGetValue(allKey, out all))
            {
                Unit dim = all.Value(onStyle);
                if (dim.IsRelative)
                {
                    Unit vert = StyleKeyFlattenUnitValue.FlattenVerticalValue(dim, page, container, font, rootFont);
                    Unit horiz = StyleKeyFlattenUnitValue.FlattenHorizontalValue(dim, page, container, font, rootFont);
                    all.SetValue(vert);
                    
                    if (onStyle.TryGetValue(this.Left, out all) == false)
                        onStyle.SetValue(this.Left, horiz);
                    
                    if (onStyle.TryGetValue(this.Top, out all) == false)
                        onStyle.SetValue(this.Top, vert);
                    
                    if (onStyle.TryGetValue(this.Right, out all) == false)
                        onStyle.SetValue(this.Right, horiz);
                    
                    if (onStyle.TryGetValue(this.Bottom, out all) == false)
                        onStyle.SetValue(this.Bottom, vert);
                }
                else
                {
                    all.SetValue(dim);
                }
            }
        }
    }

    public class StyleKeyFlattenHorizontalValue : StyleKeyFlattenValue<Unit>
    {

        public StyleKeyFlattenHorizontalValue() : base(PreferredFlattenDimension.Width)
        {
            
        }

        public override Unit FlattenValue(Unit relative, Size page, Size container, Size font, Unit rootFont)
        {
            return StyleKeyFlattenUnitValue.FlattenHorizontalValue(relative, page, container, font, rootFont);
        }

        public override void SetFlattenedValue(Style onStyle, StyleKey<Unit> key, Size page, Size container, Size font, Unit rootFont)
        {
            StyleValue<Unit> all;
            if (onStyle.TryGetValue(key, out all))
            {
                Unit dim = all.Value(onStyle);
                if (dim.IsRelative)
                {
                    Unit horiz = StyleKeyFlattenUnitValue.FlattenHorizontalValue(dim, page, container, font, rootFont);
                    all.SetValue(horiz);
                }
                else
                {
                    all.SetValue(dim);
                }
            }
        }
    }
    
    public class StyleKeyFlattenVerticalValue : StyleKeyFlattenValue<Unit>
    {

        public StyleKeyFlattenVerticalValue() : base(PreferredFlattenDimension.Height)
        {
            
        }

        public override Unit FlattenValue(Unit relative, Size page, Size container, Size font, Unit rootFont)
        {
            return StyleKeyFlattenUnitValue.FlattenVerticalValue(relative, page, container, font, rootFont);
        }

        public override void SetFlattenedValue(Style onStyle, StyleKey<Unit> key, Size page, Size container, Size font, Unit rootFont)
        {
            StyleValue<Unit> all;
            if (onStyle.TryGetValue(key, out all))
            {
                Unit dim = all.Value(onStyle);
                if (dim.IsRelative)
                {
                    Unit vert = StyleKeyFlattenUnitValue.FlattenVerticalValue(dim, page, container, font, rootFont);
                    all.SetValue(vert);
                    
                }
                else
                {
                    all.SetValue(dim);
                }
            }
        }
    }
    
    public class StyleKeyFlattenFontValue : StyleKeyFlattenValue<Unit>
    {

        public StyleKeyFlattenFontValue() : base(PreferredFlattenDimension.Font)
        {
            
        }

        public override Unit FlattenValue(Unit relative, Size page, Size container, Size font, Unit rootFont)
        {
            return StyleKeyFlattenUnitValue.FlattenFontValue(relative, page, container, font, rootFont);
        }

        public override void SetFlattenedValue(Style onStyle, StyleKey<Unit> key, Size page, Size container, Size font, Unit rootFont)
        {
            StyleValue<Unit> all;
            if (onStyle.TryGetValue(key, out all))
            {
                Unit dim = all.Value(onStyle);
                if (dim.IsRelative)
                {
                    Unit fsize = StyleKeyFlattenUnitValue.FlattenFontValue(dim, page, container, font, rootFont);
                    all.SetValue(fsize);
                    
                }
                else
                {
                    all.SetValue(dim);
                }
            }
        }
    }

    public class StyleKeyTransformOperationFlattener : IStyleKeyFlattenValue<TransformOperationSet>
    {
        public TransformOperationSet FlattenValue(TransformOperationSet known, Size pageSize, Size containerSize, Size font, Unit rootFont)
        {
            return known.CloneAndFlatten(pageSize, containerSize, font, rootFont);
            //throw new NotImplementedException();
        }

        public void SetFlattenedValue(Style onStyle, StyleKey<TransformOperationSet> key, Size pageSize, Size containerSize, Size font, Unit rootFont)
        {
            TransformOperationSet set = onStyle.GetValue(key, null);
            if (set != null)
            {
                set = set.CloneAndFlatten(pageSize, containerSize, font, rootFont);
                onStyle.SetValue(key, set);
            }
            //throw new NotImplementedException();
        }
    }

    public class StyleKeyTransformOriginFlattener : IStyleKeyFlattenValue<TransformOrigin>
    {
        public TransformOrigin FlattenValue(TransformOrigin known, Size pageSize, Size containerSize, Size font, Unit rootFont)
        {
            return known.CloneAndFlatten(pageSize, containerSize, font, rootFont);
            //throw new NotImplementedException();
        }

        public void SetFlattenedValue(Style onStyle, StyleKey<TransformOrigin> key, Size pageSize, Size containerSize, Size font, Unit rootFont)
        {
            TransformOrigin origin = onStyle.GetValue(key, null);
            if (origin != null)
            {
                origin = origin.CloneAndFlatten(pageSize, containerSize, font, rootFont);
                onStyle.SetValue(key, origin);
            }
            //throw new NotImplementedException();
        }
    }
    
}