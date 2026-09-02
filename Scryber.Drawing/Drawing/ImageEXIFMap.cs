using System;
using System.Collections.Generic;

namespace Scryber.Drawing;

public class ImageEXIFMap
{
    private Dictionary<string, ImageEXIFValue> _values;
    
    public ImageEXIFMap()
        {
        _values = new();
        }

    public void Set(string key, ImageEXIFValue value)
    {
        _values[key] = value;
    }

    public ImageEXIFValue Get(string key)
    {
        if(_values.TryGetValue(key, out var value))
            return value;
        return null;
    }

    /// <summary>
    /// The keys of every EXIF field present for this image - used by the meta(path) single-arg
    /// expression function to list what's available before asking for a specific one.
    /// </summary>
    public IEnumerable<string> Keys => _values.Keys;

    public int Count => _values.Count;
}

public enum ImageEXIFValueType
{
    String,
    Int,
    Angle,
    Real,
    Coord
}

public abstract class ImageEXIFValue : IComparable<ImageEXIFValue>, IComparable
{
    public ImageEXIFValueType Type { get; set; }
    
    public ImageEXIFValue(ImageEXIFValueType type)
    {
        this.Type = type;
    }

    public abstract int CompareValuesTo(ImageEXIFValue other);

    public abstract override string ToString();

    public abstract override int GetHashCode();
    
    
    public int CompareTo(ImageEXIFValue other)
    {
        if (this.Type != other.Type)
            return this.Type.CompareTo(other.Type);
        else
        {
            return CompareValuesTo(other);
        }
            
    }
    
    public int CompareTo(object obj)
    {
        if(obj is ImageEXIFValue other)
            return CompareTo(other);
        else
            return 1;
    }

    public override bool Equals(object obj)
    {
        if(null == obj)
            return false;
        else
        {
            return this.CompareTo(obj) == 0;
        }
    }
}


public class ImageEXIFValueString : ImageEXIFValue
{

    public string Value { get; set; }

    public ImageEXIFValueString(string value)
        : base(ImageEXIFValueType.String)
    {
        this.Value = value;
    }

    public override int CompareValuesTo(ImageEXIFValue other)
    {
        var otherString = (ImageEXIFValueString)other;
        return String.CompareOrdinal(Value, otherString.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Type, this.Value);
    }


    public override string ToString()
    {
        return Value;
    }
}

/// <summary>
/// Covers Int, Real, Angle and Coord - all of these are a single double under the hood, differing
/// only in semantic meaning and display formatting (selected by the inherited Type), not in storage
/// or comparison behaviour, so one class covers all four rather than a separate class per type.
/// </summary>
public class ImageEXIFValueNumber : ImageEXIFValue
{

    public double Value { get; set; }

    public ImageEXIFValueNumber(ImageEXIFValueType type, double value)
        : base(type)
    {
        this.Value = value;
    }

    public override int CompareValuesTo(ImageEXIFValue other)
    {
        var otherNumber = (ImageEXIFValueNumber)other;
        return Value.CompareTo(otherNumber.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Type, this.Value);
    }

    public override string ToString()
    {
        switch (this.Type)
        {
            case ImageEXIFValueType.Int:
                return ((long)Value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            case ImageEXIFValueType.Coord:
                //Signed decimal degrees - 6dp is sub-metre precision, matching common GPS usage.
                return Value.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);
            case ImageEXIFValueType.Angle:
            case ImageEXIFValueType.Real:
            default:
                return Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}