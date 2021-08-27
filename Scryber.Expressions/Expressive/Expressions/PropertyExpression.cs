using Scryber.Expressive.Expressions.Binary;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Scryber.Expressive.Expressions
{
    public class PropertyExpression : BinaryExpressionBase
    {
        public PropertyExpression(IExpression left, IExpression right, Context context)
            : base(EnsureLeftExpression(left, context), right, context)
        {
        }

        private static IExpression EnsureLeftExpression(IExpression left, Context context)
        {
            if(null == left)
            {
                left = new CurrentDataExpression(context);
            }
            return left;
        }

        

        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
        {
            if (null != rightHandSide)
            {
                string rhs;
                if (rightHandSide is VariableExpression varExpr)
                {
                    rhs = varExpr.variableName;
                }
                else
                {
                    rhs = rightHandSide.Evaluate(variables).ToString();
                }

                if (string.IsNullOrEmpty(rhs))
                {
                    return null;
                }
                else
                {
                    return DoGetMyValue(lhsResult, rhs, this.Context);
                }
            }
            else
            {
                return null;
            }
        }


        private object DoGetMyValue(object parent, string name, Context context)
        {
            

            if(null == parent)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            var type = parent.GetType();

            PropertyInfo pi = null;
            FieldInfo fi = null;

            if (this.TryGetProperty(type, name, context.IsCaseInsensitiveParsingEnabled, out pi))
            {
                return pi.GetValue(parent, null);
            }

            else if (this.TryGetField(type, name, context.IsCaseInsensitiveParsingEnabled, out fi))
            {
                return fi.GetValue(parent);
            }
            else if (parent is System.Dynamic.ExpandoObject)
            {
                object found;
                IDictionary<string, object> expando = parent as System.Dynamic.ExpandoObject;
                if (expando.TryGetValue(name, out found))
                {
                    return found;
                }
                else
                {
                    return null; //As we are dynamic, let's be generous and not throw an error.
                }
            }
            else if (parent is ICustomTypeDescriptor)
            {
                var properties = (parent as ICustomTypeDescriptor).GetProperties();
                var prop = properties.Find(name, Context.IsCaseInsensitiveParsingEnabled);

                if (null != prop)
                {
                    return prop.GetValue(parent);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

       

        private bool TryGetProperty(Type fortype, string name, bool ignoreCase, out PropertyInfo found)
        {
            found = fortype.GetProperty(name);

            if (null == found)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool TryGetField(Type fortype, string name, bool ignoreCase, out FieldInfo found)
        {
            found = fortype.GetField(name);
            if (null == found)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
