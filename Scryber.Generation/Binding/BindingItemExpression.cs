/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Scryber.Binding
{
    /// <summary>
    /// Instance of an {item:xxx} binding expression that is evaluated on the databinding event (using the called BindComponent delegate instance).
    /// It will evaluate it's known expression and set the result to it's known property on the sender of the event (instance that raised the databind event)
    /// </summary>
    /// <remarks>Use the static Create method to instantiate an instance of this class</remarks>
    public class BindingItemExpression : BindingExpressionBase
    {
        private const string CurrentDataContextName = "{current}";

        private string _expr;

        private System.Reflection.PropertyInfo _property;

        private ParserItemExpression _path;
        
        

        public string Expression
        {
            get { return _expr; }
        }

        protected System.Reflection.PropertyInfo Property
        {
            get { return _property; }
        }


        private BindingItemExpression()
        {
        }


        public void BindComponent(object sender, DataBindEventArgs args)
        {
            if(null != this._path)
            {
                try
                {
                    object value = this._path.GetValue(args.Context.Items, args.Context);

                    this.SetPropertyValue(sender, value, this.Expression, this.Property, args.Context);
                    
                }
                catch (PDFDataException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    string id;
                    if (sender is IComponent component)
                        id = ((IComponent)sender).ID;
                    else
                        id = "UNKNOWN";

                    string message = "Cannot bind the values for '" + sender.ToString() + "' with id " + id;

                    if (args.Context.Conformance == ParserConformanceMode.Lax)
                        args.Context.TraceLog.Add(TraceLevel.Error, "Data Binding", message, ex);
                    else
                        throw new Scryber.PDFBindException(message, ex);
                }
            }
        }

        private ParserItemExpression BuildExpressionPath(string expr)
        {
            ParserItemExpression top = null;

            char[] tokens = new char[] {'.','['};
            //First one must be a top level item name
            int index = expr.IndexOfAny(tokens);

            string subexpr;
            if (index > 0)
            {
                top = new ParserItemTopExpression(expr.Substring(0, index));
                subexpr = expr.Substring(index); //leave the dot or indexor in place
            }
            else if(index == 0)
            {
                top = new ParserItemTopExpression(CurrentDataContextName);
                subexpr = expr;
            }
            else
            {
                top = new ParserItemTopExpression(expr);
                subexpr = null;
            }
            
            if (!string.IsNullOrEmpty(subexpr))
                this.BuildInnerExpressionPath(top, subexpr, expr);

            return top;
        }

        private void BuildInnerExpressionPath(ParserItemExpression item, string expr, string fullExpression)
        {
            if (string.IsNullOrEmpty(expr))
                return;

            if (expr.StartsWith("."))
            {
                expr = expr.Substring(1);//remove the first .

                int end = expr.IndexOfAny(new char[] { '.', '[' });
                string prop;
                if (end > 0)
                {
                    prop = expr.Substring(0, end);
                    expr = expr.Substring(end);
                }
                else
                {
                    prop = expr;
                    expr = null;
                }
                item.AppendExpression(new ParserItemPropertyExpression(prop));

                //continue on
                BuildInnerExpressionPath(item, expr, fullExpression);
            }
            else if (expr.StartsWith("["))
            {
                int end = expr.IndexOf(']');
                if (end < 0)
                    throw new ArgumentOutOfRangeException("]", "No closing brace found for expression '" + fullExpression + "'");
                string content = expr.Substring(1, end - 1);
                expr = expr.Substring(end + 1);
                if (content.StartsWith("\"") || content.StartsWith("'"))
                {
                    content = content.Substring(1, content.Length - 2);
                    ParserItemKeyedExpression keyed = new ParserItemKeyedExpression(content);
                    item.AppendExpression(keyed);
                    BuildInnerExpressionPath(item, expr, fullExpression);
                }
                else
                {
                    int index;
                    if (!int.TryParse(content, out index))
                        throw new ArgumentException("index", "Could not convert the expression '" + content + " into an integer for the binding path '" + fullExpression + "'");
                    ParserItemIndexorExpression idexor = new ParserItemIndexorExpression(index);
                    item.AppendExpression(idexor);
                    BuildInnerExpressionPath(item, expr, fullExpression);
                }
            }
        }


        public static BindingItemExpression Create(string expression, System.Reflection.PropertyInfo destination)
        {
            BindingItemExpression binding = new BindingItemExpression();
            binding._property = destination;
            binding._expr = expression;
            try
            {
                binding._path = binding.BuildExpressionPath(expression);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cound not convert the expression '" + expression + "' to a valid route", "expression", ex);
            }

            return binding;
        }

        
    }
}
