using System;

namespace Scryber.Expressive
{
    public interface IExpressionFactory
    {
        Expression CreateExpression(string inputExpression);
    }
}
