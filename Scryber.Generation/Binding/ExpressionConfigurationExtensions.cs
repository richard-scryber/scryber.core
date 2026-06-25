using System;
using Scryber.Expressive.Functions;
using Scryber.Expressive.Operators;
using Scryber.Options;

namespace Scryber.Binding;

public static class ExpressionConfigurationExtensions
{

    public static void AddConfiguredFunctions(this FunctionSet set)
    {
        var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
        if (service != null && service.ExpressionOptions != null && service.ExpressionOptions.Register != null)
        {
            foreach (var one in service.ExpressionOptions.Register)
            {
                if (one.Type == CustomFunctionType.Function && one.Enabled)
                {
                    var func = one.GetInstance();
                    if (func is IFunction f)
                        set.RegisterFunction(f, one.Override);
                    else
                        throw new InvalidCastException("The configured function '" + one.Name + "' does not implement the " + typeof(IFunction).FullName + " interface");
                }
            }
        }
    }

    public static void AddConfiguredOperators(this OperatorSet set)
    {
        var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
        if (service != null && service.ExpressionOptions != null && service.ExpressionOptions.Register != null)
        {
            foreach (var one in service.ExpressionOptions.Register)
            {
                if (one.Type == CustomFunctionType.Operator && one.Enabled)
                {
                    var func = one.GetInstance();
                    if (func is IOperator o)
                        set.RegisterOperator(o, one.Override);
                    else
                        throw new InvalidCastException("The configured operator '" + one.Name + "' does not implement the " + typeof(IOperator).FullName + " interface");
                }
            }
        }
    }
}