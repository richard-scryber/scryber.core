using System;
using Scryber.Expressive;
using Scryber.Expressive.Tokenisation;

namespace Scryber.Binding
{
    /// <summary>
    /// Standard tokeniser for the BindingCalcExpression based on Tokeniser2, rather than the base tokeniser
    /// </summary>
    public class BindingCalcTokeniser : Tokeniser
    {
        public BindingCalcTokeniser(Context context)
            : base(context)
        {
        }
    }
}
