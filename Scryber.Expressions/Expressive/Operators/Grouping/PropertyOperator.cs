using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Operators.Grouping
{
    public class PropertyOperator : OperatorBase
    {

        public override IEnumerable<string> Tags => new[] { "." };

        public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
        {
            if(null == expressions)
            {
                throw new ArgumentNullException(nameof(expressions));
            }
            else if(expressions.Length < 2)
            {
                //TODO: Support the current context with a single dot
                throw new ArgumentOutOfRangeException(nameof(expressions), "expressions for a property (.) operator should be 2");
            }
            
            return new PropertyExpression(expressions[0], expressions[1], context);
        }

        public override OperatorPrecedence GetPrecedence(Token previousToken)
        {
            return OperatorPrecedence.PropertyOperator;
        }

        public PropertyOperator()
        {
        }
    }
}
