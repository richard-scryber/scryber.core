using System;
using System.Collections.Generic;
using System.Reflection;
using Scryber.Expressive;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Functions.Relational;
using Scryber.Expressive.Operators;

namespace Scryber.Binding
{
    public class BindingCalcExpression : BindingExpressionBase
    {

        public PropertyInfo BoundTo { get; set; }

        public Expression Expression { get; set; }

        public string OriginalExpression { get; set; }

        public IVariableProvider ItemValueProvider { get; set; }


        public BindingCalcExpression(Expression expression, string origExpr, PropertyInfo property)
        {
            this.Expression = expression;
            this.OriginalExpression = origExpr;
            this.BoundTo = property;
        }

        public void BindComponent(object sender, PDFDataBindEventArgs args)
        {
            if (null == this.ItemValueProvider)
                this.ItemValueProvider = args.Context.Items.ValueProvider(
                    args.Context.CurrentIndex,
                    args.Context.DataStack.HasData ? args.Context.DataStack.Current : null);

            object value;

            if (this.TryEvaluate(this.ItemValueProvider, sender, args.Context, out value))
                this.SetPropertyValue(sender, value, Expression.ToString(), BoundTo, args.Context);
            else
                args.Context.TraceLog.Add(TraceLevel.Warning, "Expression Binding", "The expression '" + this.OriginalExpression + "' for '" + sender.ToString() + "' on property '" + this.BoundTo.Name + "' could not be evaluated ");

        }

        protected bool TryEvaluate(IVariableProvider provider, object owner, PDFDataContext context, out object value)
        {
            value = null;
            bool result;

            try
            {
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
            

            return result;
        }
    }



    public static class PDFItemCollectionExtensions
    {

        public static IVariableProvider ValueProvider(this PDFItemCollection items, int index, object currentdata)
        {
            return new PDFItemVariableProvider(items, index, currentdata);
        }
    }

    public class PDFItemVariableProvider : IVariableProvider
    {

        private PDFItemCollection _items;
        private object _currentData;
        private int _currentIndex;


        protected PDFItemCollection Items { get { return _items; } }

        protected object CurrentData { get { return _currentData; } }
        protected int CurrentIndex { get { return _currentIndex; } }

        public PDFItemVariableProvider(PDFItemCollection items, int index, object currentData)
        {
            if (null == items)
                throw new ArgumentNullException(nameof(items));
            _items = items;
            _currentIndex = index;
            _currentData = currentData;
        }


        public bool TryGetValue(string variableName, out object value)
        {
            if (variableName == IndexFunction.CurrentIndexVariableName)
            {
                value = this.CurrentIndex;
                return null != value;
            }
            else if(variableName == CurrentDataExpression.CurrentDataVariableName)
            {
                value = this.CurrentData;

                return null != value;
            }
            else
                return Items.TryGetValue(variableName, out value);
        }
    }
}
