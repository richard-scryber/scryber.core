using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class LengthFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "Length";
            }
        }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            object value = parameters[0].Evaluate(Variables);

            if (value is null) { return null; }

            string text = value as string;
            if (text != null)
            {
                return text.Length;
            }
            else
            {
                return value.ToString().Length;
            }
        }

        #endregion
    }
}
