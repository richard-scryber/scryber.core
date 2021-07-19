using System;
using Scryber.Expressive;

namespace Scryber.Binding
{
    /// <summary>
    /// Uses the BindingCalcTokenniser rather than the default which calls a lot of substrings
    /// </summary>
    public class BindingCalcParser : ExpressionParser
    {
        public BindingCalcParser(Context context)
            : base(context, new BindingCalcTokeniser(context))
        {
        }
    }
}
