using System;

namespace Scryber.Svg
{
    [PDFParsableValue]
    public struct AdornmentOrientationValue
    {
        public bool IsReverseAtStart { get; set; }

        public bool HasAngle { get; set; }

        public double AngleRadians { get; set; }

        public AdornmentOrientationValue(double angle)
        {
            this.IsReverseAtStart = false;
            this.HasAngle = true;
            this.AngleRadians = angle;
        }


        public static AdornmentOrientationValue Default
        {
            get { return new AdornmentOrientationValue(); }
        }

        public static AdornmentOrientationValue Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new AdornmentOrientationValue();
            else if (value == "auto")
                return new AdornmentOrientationValue() { IsReverseAtStart = false, HasAngle = false, AngleRadians = 0.0 };
            else if (value == "auto-start-reverse")
                return new AdornmentOrientationValue() { IsReverseAtStart = true, HasAngle = false, AngleRadians = 0.0 };
            else if (double.TryParse(value, out var angleDeg))
                return new AdornmentOrientationValue()
                    { IsReverseAtStart = false, HasAngle = true, AngleRadians = (angleDeg * (Math.PI / 180.0)) };
            else
                throw new ArgumentException("The value '" + value +
                                            "' could not be converted to a valid orientation. Expected an value in degrees, or 'auto' or 'auto-start-reverse'");
        }
    }
}