using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    internal class EndsWithFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "EndsWith";
            }
        }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var text = (string)parameters[0].Evaluate(Variables);
            var value = (string)parameters[1].Evaluate(Variables);

            if (value is null)
            {
                return false;
            }
            
            return text?.EndsWith(value, context.EqualityStringComparison) == true;
        }

        #endregion
    }
}
