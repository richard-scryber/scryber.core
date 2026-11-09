using System;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing
{
    /// <summary>
    /// Static class - will support the standard css values of Initial, Unset, Inherit etc. in the future - at the moment, not supported.
    /// </summary>
    [Obsolete("CSS Global values of initial, inherited, unset, etc. will be supported - but for the moment these are ignored.", true)]
    public static class CSSStyleValueGlobals
    {

        //public static Unit CSSInitial = Unit.Pt(double.MinValue);
        //public static Unit CSSInherited = Unit.Enumeration(double.MinValue + 1);

    }
}