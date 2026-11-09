using System;
using Scryber.Styles;
using Scryber.Drawing;


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



    


    /// <summary>
    /// Delegate instances should return the size of a parent of the passed component, based on the requested size option.
    /// </summary>
    /// <param name="forComponent">The component to get the size of the parent for</param>
    /// <param name="withStyle">The style of the component</param>
    /// <param name="withPosition">The position mode of the component.</param>
    /// <returns></returns>
    public delegate Size ParentComponentSizer(IComponent forComponent, Style withStyle, PositionMode withPosition);
}
