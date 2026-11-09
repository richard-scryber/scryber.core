using Scryber.Expressive;
using Scryber.Expressive.Expressions;

namespace Scryber.UnitConsoleTest.Functions;

public class CustomFunction : Scryber.Expressive.Functions.FunctionBase
{

    public override string Name { get; } = "Custom";

    public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
    {
        this.ValidateParameterCount(parameters, 1, 1);
        
        var param = parameters[0];
        var value = param.Evaluate(variables);

        if (null == value)
            return "Value was nothing";
        else
        {
            return "Value is " + value.ToString();
        }
    }
}