using System;
using System.Globalization;
using Scryber.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace Scryber.Imaging
{
    /// <summary>
    /// Converts a SixLabors ExifProfile (present on any loaded Image, regardless of source format)
    /// into Scryber's own ImageEXIFMap. Two passes: a generic sweep of every present tag (skipping
    /// only truly opaque binary blobs like MakerNote/ExifVersion, and array values other than the
    /// GPS DMS triples handled below), then a GPS-specific pass that combines the raw
    /// GPSLatitude/GPSLongitude DMS triples with their hemisphere Ref into a single signed decimal-
    /// degree value - the raw triple alone isn't usable without that combination, which is the
    /// entire point of the GPS requirement this exists for.
    /// </summary>
    public static class ImageEXIFExtractor
    {
        public static ImageEXIFMap Extract(Image image)
        {
            return Extract(image?.Metadata?.ExifProfile);
        }

        /// <summary>
        /// Overload for the JPEG fast (native byte-header) load path, which never decodes a full
        /// Image via SixLabors - it only has the raw APP1 Exif segment bytes, from which
        /// ExifProfile can be constructed directly without a full image decode.
        /// </summary>
        public static ImageEXIFMap Extract(ExifProfile profile)
        {
            if (null == profile || profile.Values.Count == 0)
                return null;

            var map = new ImageEXIFMap();

            foreach (var exifValue in profile.Values)
            {
                var converted = ConvertValue(exifValue);
                if (null != converted)
                    map.Set(exifValue.Tag.ToString(), converted);
            }

            ExtractGpsCoordinate(profile, ExifTag.GPSLatitude, ExifTag.GPSLatitudeRef, "S", map, "GPSLatitude");
            ExtractGpsCoordinate(profile, ExifTag.GPSLongitude, ExifTag.GPSLongitudeRef, "W", map, "GPSLongitude");

            return map;
        }

        /// <summary>
        /// Maps one EXIF value to a Scryber ImageEXIFValue based on its EXIF data type - not its
        /// .NET runtime type, which varies (SixLabors uses its own Number/Rational/SignedRational
        /// structs for some numeric types rather than plain ushort/uint/double).
        /// </summary>
        private static ImageEXIFValue ConvertValue(IExifValue exifValue)
        {
            if (exifValue.IsArray)
                //Array values (ISOSpeedRatings, SubjectArea, LensSpecification, the GPS DMS
                //triples, etc.) aren't single scalar values - the GPS ones are handled explicitly
                //via ExtractGpsCoordinate below; the rest aren't useful as a single map entry.
                return null;

            object raw = exifValue.GetValue();
            if (null == raw)
                return null;

            switch (exifValue.DataType)
            {
                case ExifDataType.Ascii:
                    return new ImageEXIFValueString(Convert.ToString(raw, CultureInfo.InvariantCulture));

                case ExifDataType.Byte:
                case ExifDataType.SignedByte:
                case ExifDataType.Short:
                case ExifDataType.SignedShort:
                case ExifDataType.Long:
                case ExifDataType.SignedLong:
                case ExifDataType.Long8:
                case ExifDataType.SignedLong8:
                    return new ImageEXIFValueNumber(ImageEXIFValueType.Int, ToDouble(raw));

                case ExifDataType.Rational:
                case ExifDataType.SignedRational:
                case ExifDataType.SingleFloat:
                case ExifDataType.DoubleFloat:
                    return new ImageEXIFValueNumber(ImageEXIFValueType.Real, ToDouble(raw));

                default:
                    //Undefined (raw byte blobs - ExifVersion, MakerNote, ComponentsConfiguration),
                    //Ifd/Ifd8 (offsets to other directories), Unknown - none are useful as a
                    //single displayable value.
                    return null;
            }
        }

        /// <summary>
        /// SixLabors represents some numeric EXIF values with its own struct types (Number,
        /// Rational, SignedRational) rather than plain .NET numerics, so a blind
        /// Convert.ToDouble(object) doesn't reliably work across all of them.
        /// </summary>
        private static double ToDouble(object raw)
        {
            switch (raw)
            {
                case Rational rational:
                    return rational.ToDouble();
                case SignedRational signedRational:
                    return signedRational.ToDouble();
                case Number number:
                    return (uint)number;
                default:
                    return Convert.ToDouble(raw, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Combines a GPSLatitude/GPSLongitude DMS (degrees, minutes, seconds) triple with its
        /// hemisphere Ref ("N"/"S"/"E"/"W") into one signed decimal-degree value, overwriting the
        /// raw entry the generic sweep would otherwise have skipped (it's an array, so
        /// ConvertValue never produces one for these keys) - this combined value is what actually
        /// makes GPS data usable from a template, rather than requiring DMS maths on the caller's
        /// side.
        /// </summary>
        private static void ExtractGpsCoordinate(
            ExifProfile profile,
            ExifTag<Rational[]> valueTag,
            ExifTag<string> refTag,
            string negativeRefValue,
            ImageEXIFMap map,
            string key)
        {
            if (!profile.TryGetValue(valueTag, out var dmsValue) || dmsValue.Value == null || dmsValue.Value.Length != 3)
                return;

            double decimalDegrees = dmsValue.Value[0].ToDouble()
                                     + (dmsValue.Value[1].ToDouble() / 60.0)
                                     + (dmsValue.Value[2].ToDouble() / 3600.0);

            if (profile.TryGetValue(refTag, out var refValue)
                && string.Equals(refValue.Value, negativeRefValue, StringComparison.OrdinalIgnoreCase))
            {
                decimalDegrees = -decimalDegrees;
            }

            map.Set(key, new ImageEXIFValueNumber(ImageEXIFValueType.Coord, decimalDegrees));
        }
    }
}
