using System;
namespace Scryber.Svg.Components
{
    public enum AspectRatioAlign : byte
    {
        None,
        xMinYMin,
        xMidYMin,
        xMaxYMin,
        xMinYMid,
        xMidYMid,
        xMaxYMid,
        xMinYMax,
        xMidYMax,
        xMaxYMax
    }

    public enum AspectRatioMeet : byte
    {
        None,
        Meet,
        Slice
    }
}
