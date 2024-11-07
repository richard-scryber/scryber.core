using Scryber.Expressive.Expressions;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Relational
{
    public class CountFunction : FunctionBase
    {
        #region FunctionBase Members

        /// <inheritdoc />
        public override string Name => "Count";

        /// <inheritdoc />
        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            var count = 0;

            foreach (var value in parameters)
            {
                int increment;

                if (null != value)
                {
                    var evaluatedValue = value.Evaluate(variables);
                    if (null != evaluatedValue)
                    {
                        increment = 1;


                        IEnumerable ienum;

                        if (Helpers.Collections.TryIsCollection(evaluatedValue, out ienum))
                        {
                            increment = 0;
                            
                            foreach (var inner in ienum)
                            {
                                var innerVal = Comparison.ExtractAnyJsonValue(inner);
                                if (null != innerVal)
                                {
                                    increment++;
                                }
                            }

                        }

                        count += increment;
                    }
                }
            }

            return count;
        }

        #endregion
    }
}
