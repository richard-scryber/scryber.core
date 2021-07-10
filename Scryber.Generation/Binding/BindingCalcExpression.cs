using System;
using System.Collections.Generic;
using System.Reflection;
using Expressive;
using Expressive.Expressions;
using Expressive.Operators;

namespace Scryber.Binding
{
    public class BindingCalcExpression : BindingExpressionBase
    {
        

        public PropertyInfo BoundTo { get; set; }

        public Expression Expression { get; set; }

        public IVariableProvider ItemValueProvider { get; set; }


        public BindingCalcExpression(string expression, PropertyInfo property, ExpressiveOptions options)
        {
            this.Expression = new Expression(expression, options);
            this.BoundTo = property;
        }

        

        public void BindComponent(object sender, PDFDataBindEventArgs args)
        {
            if (null == this.ItemValueProvider)
                this.ItemValueProvider = args.Context.Items.ValueProvider();

            object value;
            if (this.TryEvaluate(this.ItemValueProvider, sender, args.Context, out value))
                this.SetPropertyValue(sender, value, Expression.ToString(), BoundTo, args.Context);

        }

        protected bool TryEvaluate(IVariableProvider provider, object owner, PDFDataContext context, out object value)
        {
            value = null;
            bool result = false;

            object prevData = null;
            int prevIndex = -1;
            bool hasData = false;

            try
            {
                // If we have a current data context, then we store the last value and
                // Update to the current value

                if (context.DataStack.HasData)
                {
                    prevData = this.Expression.ExpressionContext.CurrentDataContext;
                    prevIndex = this.Expression.ExpressionContext.CurrentDataIndex;
                    hasData = true;

                    this.Expression.ExpressionContext.CurrentDataContext = context.DataStack.Current;
                    this.Expression.ExpressionContext.CurrentDataIndex = context.CurrentIndex;
                }

                value = this.Expression.Evaluate(provider);
                result = true;
            }
            catch(Exception ex)
            {
                string id;
                if (owner is IPDFComponent component)
                    id = component.ID;
                else
                    id = "";

                string message = "Cannot bind the values for '" + owner.ToString() + "' with id " + id;
                result = false;

                if (context.Conformance == ParserConformanceMode.Lax)
                    context.TraceLog.Add(TraceLevel.Error, "Data Binding", message, ex);
                else
                    throw new Scryber.PDFBindException(message, ex);
            }
            finally
            {
                //try and restore the previous values.
                if(hasData && null != this.Expression && null != this.Expression.ExpressionContext)
                {
                    this.Expression.ExpressionContext.CurrentDataIndex = prevIndex;
                    this.Expression.ExpressionContext.CurrentDataContext = prevData;
                }
            }

            return result;
        }
    }



    internal static class PDFItemCollectionExtensions
    {

        public static IVariableProvider ValueProvider(this PDFItemCollection items)
        {
            return new PDFItemVariableProvider(items);
        }
    }

    internal class PDFItemVariableProvider : IVariableProvider
    {

        private PDFItemCollection _items;

        protected PDFItemCollection Items { get { return _items; } }

        public PDFItemVariableProvider(PDFItemCollection items)
        {
            if (null == items)
                throw new ArgumentNullException(nameof(items));
            _items = items;
        }


        public bool TryGetValue(string variableName, out object value)
        {
            return Items.TryGetValue(variableName, out value);
        }
    }
}
