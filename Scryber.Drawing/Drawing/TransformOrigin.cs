using System;

namespace Scryber.Drawing
{
    
    
    [PDFParsableValue]
    public class TransformOrigin
    {
        private static readonly Unit Left = Unit.Percent(0);
        private static readonly Unit Right = Unit.Percent(100);
        private static readonly Unit Top = Unit.Percent(0);
        private static readonly Unit Bottom = Unit.Percent(100);
        private static readonly Unit Center = Unit.Percent(50);
        
        
        public Unit HorizontalOrigin { get; set; }
        
        public Unit VerticalOrigin { get; set; }

        /// <summary>
        /// Gets the origin as it was before it was flattened, or null if this has not been flattened.
        /// Percentages of a component's own box are only known when it is laid out.
        /// </summary>
        public TransformOrigin Unflattened { get; private set; }


        public TransformOrigin(Unit hOrigin, Unit vOrigin)
        {
            this.HorizontalOrigin = hOrigin;
            this.VerticalOrigin = vOrigin;
        }

        public TransformOrigin CloneAndFlatten(Size pageSize, Size containerSize, Size font, Unit rootFont)
        {
            var clone = (TransformOrigin)this.MemberwiseClone();
            clone.Unflattened = this.Unflattened ?? this;
            if (clone.HorizontalOrigin.IsRelative)
            {
                clone.HorizontalOrigin = Unit.FlattenHorizontalValue(clone.HorizontalOrigin, pageSize, containerSize, font, rootFont);
            }

            if (clone.VerticalOrigin.IsRelative)
            {
                clone.VerticalOrigin = Unit.FlattenVerticalValue(clone.VerticalOrigin, pageSize, containerSize, font, rootFont);
            }
            return clone;
        }

        /// <summary>
        /// Returns a new origin as a point on the page within the box (e.g. the border box of a component),
        /// where percentages are of the box size. If there is no origin, then the centre of the box is returned.
        /// </summary>
        /// <param name="origin">The (optional) origin to place in the box</param>
        /// <param name="box">The box on the page</param>
        public static TransformOrigin GetOriginWithin(TransformOrigin origin, Rect box)
        {
            Unit h, v;
            if (null == origin)
            {
                h = Center;
                v = Center;
            }
            else
            {
                h = GetOriginValueWithin(origin.HorizontalOrigin, origin.Unflattened?.HorizontalOrigin);
                v = GetOriginValueWithin(origin.VerticalOrigin, origin.Unflattened?.VerticalOrigin);
            }

            var x = box.X + (h.Units == PageUnits.Percent ? box.Width * (h.Value / 100.0) : h);
            var y = box.Y + (v.Units == PageUnits.Percent ? box.Height * (v.Value / 100.0) : v);
            return new TransformOrigin(x, y);
        }

        private static Unit GetOriginValueWithin(Unit value, Unit? unflattened)
        {
            //a percentage was flattened against the container, but it is of the component's own box
            if (unflattened.HasValue && unflattened.Value.Units == PageUnits.Percent)
                return unflattened.Value;
            else
                return value;
        }

        public Unit GetHorizontalOffset(Size container, DrawingOrigin origin)
        {
            if (origin != DrawingOrigin.BottomLeft)
                throw new InvalidOperationException("Origin for the drawing must be BottomLeft at the moment.");
            if (this.HorizontalOrigin.IsRelative)
                throw new InvalidOperationException("The origin must be cloned and flattened before using");
            
            return HorizontalOrigin;
        }
        
        public Unit GetVerticalOffset(Size container, DrawingOrigin origin)
        {
            if (origin != DrawingOrigin.BottomLeft)
                throw new InvalidOperationException("Origin for the drawing must be BottomLeft at the moment.");
            
            if (this.VerticalOrigin.IsRelative)
                throw new InvalidOperationException("The origin must be cloned and flattened before using. Make sure the databinding has been invoked");
            
            return container.Height - VerticalOrigin.PointsValue;
        }

        public override string ToString()
        {
            return "(" + this.HorizontalOrigin + ", " + this.VerticalOrigin + ")";
        }

        public static TransformOrigin Parse(string value)
        {
            TransformOrigin parsed;
            Unit h, v;
            
            if (string.IsNullOrEmpty(value))
            {
                return new TransformOrigin(Center, Center); //Default is Centre
            }

            var all = value.Split(' ');
            string one = all[0];
            string two;
                
            if (all.Length == 1)
            {
                if (ParseNamedHorizontalValue(one, out h))
                {
                    parsed = new TransformOrigin(h, Center);
                }
                else if (ParseNamedVerticalValue(one, out v))
                {
                    parsed = new TransformOrigin(Center, v);
                }
                else if (Unit.TryParse(one, out h))
                {
                    parsed = new TransformOrigin(h, Center);
                }
                else
                {
                    throw new ArgumentException("The value " + value +
                                                " could not be converted to a transformation origin");
                }
                
            }
            else
            {
                two = all[1];
                if (one == "center") //could be either
                {
                    if (ParseNamedHorizontalValue(two, out h))
                    {
                        //second is horizontal so the first was intended to be vertical.
                        v = Center;
                    }
                    else if (ParseNamedVerticalValue(two, out v))
                    {
                        //second was vertical - all good for horizontal center
                        h = Center;
                    }
                    else if (Unit.TryParse(two, out v))
                    {
                        //All good with defaulting to v as a unit after a center horizontal
                    }
                    else
                    {
                        throw new ArgumentException("The value " + value +
                                                    " could not be converted to a transformation origin");
                    }
                }
                else if (ParseNamedHorizontalValue(one, out h)) //This will catch left or right
                {
                    if (ParseNamedVerticalValue(two, out v))
                    {
                        //All good with H and V
                    }
                    else if (Unit.TryParse(two, out v))
                    {
                        //All good with unit following left or right as a vertical
                    }
                    else
                    {
                        throw new ArgumentException("The value " + value +
                                                    " could not be converted to a transformation origin");
                    }
                }
                else if (ParseNamedVerticalValue(one, out v)) //we know it is not center - so either top bottom
                {
                    if (ParseNamedHorizontalValue(two, out h))
                    {
                        //All good with vertical then horizontal
                    }
                    else if (Unit.TryParse(two, out h))
                    {
                        //All ok with numeric h following top or bottom
                    }
                    else
                    {
                        throw new ArgumentException("The value " + value +
                                                    " could not be converted to a transformation origin");
                    }
                }
                else if (Unit.TryParse(one, out h))
                {
                    if (ParseNamedVerticalValue(two, out v))
                    {
                        //number and vertical
                    }
                    else if (ParseNamedHorizontalValue(two, out v))
                    {
                        (h, v) = (v, h); //number and horizontal so swap
                    }
                    else if (Unit.TryParse(two, out v))
                    {
                        //just 2 numbers - h, v
                    }
                    else
                    {
                        throw new ArgumentException("The value " + value +
                                                    " could not be converted to a transformation origin");
                    }
                }
                else
                {
                    throw new ArgumentException("The value " + value +
                                                " could not be converted to a transformation origin");
                }
                

                parsed = new TransformOrigin(h, v);
            }

            return parsed;
        }

        private static bool ParseNamedHorizontalValue(string value, out Unit parsed)
        {
            bool result;
            value = value.Trim();
            switch (value)
            {
                case "left":
                    parsed = Left;
                    result = true;
                    break;
                case "right":
                    parsed = Right;
                    result = true;
                    break;
                case "center":
                    parsed = Center;
                    result = true;
                    break;
                default:
                    parsed = Unit.Zero;
                    result = false;
                    break;
            }

            return result;
        }
        
        private static bool ParseNamedVerticalValue(string value, out Unit parsed)
        {
            bool result;
            value = value.Trim();
            switch (value)
            {
                case "center":
                    parsed = Center;
                    result = true;
                    break;
                case "top":
                    parsed = Top;
                    result = true;
                    break;
                case "bottom":
                    parsed = Bottom;
                    result = true;
                    break;
                default:
                    parsed = Unit.Zero;
                    result = false;
                    break;
            }

            return result;
        }
    }
}