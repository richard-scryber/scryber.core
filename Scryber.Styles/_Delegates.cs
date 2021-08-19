using System;
using Scryber.Styles;

namespace Scryber
{
    /// <summary>
    /// Attempts to convert an object value to the correct value setting the result and returning true if converted
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="onStyle">The style where the value will be applied</param>
    /// <param name="value">The value to be converted</param>
    /// <param name="result">The result that will be set</param>
    /// <returns>True if the conversion was successfull</returns>
    public delegate bool StyleValueConvertor<T>(StyleBase onStyle, object value, out T result);

}
