using Scryber.Drawing;

namespace Scryber.Html
{
    /// <summary>
    /// The CSSTransformOperationSet implements the base TransformOperationSet, but parses the explicit
    /// content for the html element transform attribute values
    /// </summary>
    [PDFParsableValue]
    public class CSSTransformOperationSet : Scryber.Drawing.TransformOperationSet
    {
        
        public static CSSTransformOperationSet Parse(string value)
        {
            CSSTransformOperationSet parsed = new CSSTransformOperationSet();
            
            if (TransformOperationSet.ParseIntoSet(parsed, value))
            {
                return parsed;
            }
            else
                return null;
        }
        
    }
}