using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class SubstringFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "Substring";
            }
        }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 2);

            object value = parameters[0].Evaluate(Variables);

            if (value is null)
            {
                return null;
            }

            // Safely handle the text input, if not text then call ToString.
            string text = null;
            if (value is string)
            {
                text = (string)value;
            }
            else
            {
                text = value.ToString();
            }

            int startIndex = (int)parameters[1].Evaluate(Variables);

            if (parameters.Length > 2)
            {
                int length = (int)parameters[2].Evaluate(Variables);

                return text?.Substring(startIndex, length);
            }
            else
            {
                return text?.Substring(startIndex);
            }
        }

        #endregion
    }
}
