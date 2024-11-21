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
        

        public TransformOrigin(Unit hOrigin, Unit vOrigin)
        {
            this.HorizontalOrigin = hOrigin;
            this.VerticalOrigin = vOrigin;
        }

        public TransformOrigin CloneAndFlatten(Size pageSize, Size containerSize, Size font, Unit rootFont)
        {
            var clone = (TransformOrigin)this.MemberwiseClone();
            if (clone.HorizontalOrigin.IsRelative)
            {
                clone.HorizontalOrigin = Unit.FlattenHorizontalValue(clone.HorizontalOrigin, pageSize, containerSize, font, rootFont);
            }

            if (clone.VerticalOrigin.IsRelative)
            {
                clone.VerticalOrigin = Unit.FlattenVerticalValue(clone.HorizontalOrigin, pageSize, containerSize, font, rootFont);
            }
            return clone;
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
                if (ParseHorizontalValue(one, out h))
                {
                    parsed = new TransformOrigin(h, Center);
                }
                else if (ParseVerticalValue(one, out v))
                {
                    parsed = new TransformOrigin(Center, v);
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

                if (ParseHorizontalValue(one, out h)) //This will catch a numeric value and assume it is horizontal - expected as there are 2 values
                {
                    ParseVerticalValue(two, out v);
                }
                else if (ParseVerticalValue(one, out v))
                {
                    ParseHorizontalValue(two, out h);
                }

                parsed = new TransformOrigin(h, v);
            }

            return parsed;
        }

        private static bool ParseHorizontalValue(string value, out Unit parsed)
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
                case "top":
                case "bottom":
                    parsed = Unit.Zero;
                    result = false;
                    break;
                default:
                    result = Unit.TryParse(value, out parsed);
                    break;
            }

            return result;
        }
        
        private static bool ParseVerticalValue(string value, out Unit parsed)
        {
            bool result;
            value = value.Trim();
            switch (value)
            {
                case "left":
                    parsed = Unit.Zero;
                    result = false;
                    break;
                case "right":
                    parsed = Unit.Zero;
                    result = false;
                    break;
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
                    result = Unit.TryParse(value, out parsed);
                    break;
            }

            return result;
        }
    }
}