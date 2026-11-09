using System;

namespace Scryber.Styles.Parsing
{
    /// <summary>
    /// Thrown when a var() or calc() expression in css cannot be compiled when the css is parsed.
    /// The css parsers otherwise log and skip any style that fails to parse, but an invalid expression
    /// is raised (in strict and lax mode) in the same way as an invalid expression in a template or attribute.
    /// </summary>
    public class CSSExpressionParseException : PDFException
    {
        public CSSExpressionParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
