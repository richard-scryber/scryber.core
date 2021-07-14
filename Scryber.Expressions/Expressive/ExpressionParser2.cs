using System;
using Scryber.Expressive.Tokenisation;

namespace Scryber.Expressive
{
    public class ExpressionParser2 : ExpressionParser
    {
        public ExpressionParser2(Context context)
            : base(context, new Tokeniser2(context))
        {
        }
    }
}
