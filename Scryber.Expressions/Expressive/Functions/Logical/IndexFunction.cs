using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Relational
{
    public class IndexFunction : FunctionBase
    {

        public const string CurrentIndexVariableName = "[CurrentIndex]";

        public override string Name { get { return "Index"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            this.ValidateParameterCount(parameters, 0, 0);

            object value;
            if (variables.TryGetValue(CurrentIndexVariableName, out value))
                return value;
            else
                return -1;
        }
    }
    
    public class AtIndexFunction : FunctionBase
    {

        public const string CurrentIndexVariableName = "[CurrentIndex]";

        public override string Name { get { return "@Index"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            this.ValidateParameterCount(parameters, 0, 0);

            object value;
            if (variables.TryGetValue(CurrentIndexVariableName, out value))
                return value;
            else
                return -1;
        }
    }

    /// <summary>
    /// True for the first item when binding over a collection (@first)
    /// </summary>
    public class AtFirstFunction : FunctionBase
    {
        public override string Name { get { return "@First"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            this.ValidateParameterCount(parameters, 0, 0);

            object value;
            if (variables.TryGetValue(IndexFunction.CurrentIndexVariableName, out value) && value is int index)
                return index == 0;
            else
                return false;
        }
    }

    /// <summary>
    /// True for the last item when binding over a collection (@last)
    /// </summary>
    public class AtLastFunction : FunctionBase
    {
        public const string CurrentLastVariableName = "[CurrentLast]";

        public override string Name { get { return "@Last"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            this.ValidateParameterCount(parameters, 0, 0);

            object value;
            if (variables.TryGetValue(CurrentLastVariableName, out value) && value is bool last)
                return last;
            else
                return false;
        }
    }
}
