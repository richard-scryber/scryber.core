using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class EvalFunction : FunctionBase
    {
        private Expression _last;
        private string _lastString;


        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "Eval";
            }
        }

        public EvalFunction() : base()
        { }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            //We can accept 1 parameter that is an array
            this.ValidateParameterCount(parameters, 1, 1);

            object value = parameters[0].Evaluate(variables);

            if(value is null) { return null; }

            var exprString = value.ToString();

            if (null == _last || _lastString != exprString)
            {
                var tokeniser = new Tokenisation.Tokeniser(context);
                var parser = new ExpressionParser(context, tokeniser);
                var expr = new Expression(exprString, context);

                try
                {
                    expr.CompileExpression();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("The expression '" + exprString + "' could not be compiled into a valid expression");
                }

                _last = expr;
                _lastString = exprString;
            }


            return _last.Evaluate(variables);
            
            
        }

        protected virtual void Evaluate(StringBuilder sb, IEnumerable parameters, IDictionary<string, object> variables, Context context)
        {
            foreach (var p in parameters)
            {
                object value;

                if (p is IExpression expression)
                {
                    value = expression.Evaluate(variables);
                }
                else
                {
                    value = p;
                }

                IEnumerable collection;

                if(Helpers.Collections.TryIsCollection(value, out collection))
                {
                    foreach(var item in collection)
                    {
                        var s = item.ToString();
                        sb.Append(s);
                    }
                }
                else if (null == value)
                {
                    continue;
                }
                else
                {
                    string s = value.ToString();
                    sb.Append(s);
                }

            }
        }

        #endregion
    }
}
