using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Scryber.Drawing;
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

        public void BindComponent(object sender, DataBindEventArgs args)
        {
            if (null == this.ItemValueProvider)
                this.ItemValueProvider = args.Context.Items.ValueProvider(
                    args.Context.CurrentIndex,
                    args.Context.DataStack.HasData ? args.Context.DataStack.Current : null, args.Context.DataStack);

            object value;

            if (this.TryEvaluate(this.ItemValueProvider, sender, args.Context, out value))
                this.SetPropertyValue(sender, value, Expression.ToString(), BoundTo, args.Context);
            else
                args.Context.TraceLog.Add(TraceLevel.Warning, "Expression Binding", "The expression '" + this.OriginalExpression + "' for '" + sender.ToString() + "' on property '" + this.BoundTo.Name + "' could not be evaluated ");

        }

        protected bool TryEvaluate(IVariableProvider provider, object owner, DataContext context, out object value)
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
                if (owner is IComponent component)
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

        public static ItemVariableProvider ValueProvider(this ItemCollection items, int index, object currentdata, DataStack stack)
        {
            return new ItemVariableProvider(items, index, currentdata, stack);
        }
    }

    public class ItemVariableProvider : IVariableProvider
    {

        private ItemCollection _items;
        private object _currentData;
        private int _currentIndex;
        private RelativeDimensions _relatives;
        private DataStack _stack;
        
        private RelativeToAbsoluteDimensionCallback _relativeCallback;


        protected ItemCollection Items { get { return _items; } }

        protected object CurrentData { get { return _currentData; } }
        protected int CurrentIndex { get { return _currentIndex; } }

        protected RelativeDimensions Relatives { get { return _relatives; } }
        
        protected DataStack DataStack
        {
            get { return _stack; }
        }
        
        public RelativeToAbsoluteDimensionCallback RelativeCallback
        {
            get { return this._relativeCallback; }
        }

        public ItemVariableProvider(ItemCollection items, int index, object currentData, DataStack stack)
        {
            if (null == items)
                throw new ArgumentNullException(nameof(items));
            _items = items;
            _currentIndex = index;
            _currentData = currentData;
            _stack = stack;
        }

        public void AddRelativeDimensions(Size page, Size container, Size font, Unit rootFont, bool useWidth)
        {
            this._relatives = new RelativeDimensions(page, container, font, rootFont, useWidth);
        }

        public void AddRelativeCallback(RelativeToAbsoluteDimensionCallback callback)
        {
            this._relativeCallback = callback;
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
            else if (variableName == "../") //parent referernce
            {
                throw new NotSupportedException(
                    "The expression '../' is not supported in the library, please try and access the content from the root, or store the required value in a variable");
            }
            else if (variableName == UnitRelativeVars.RelativeCallbackVar)
            {
                value = this._relativeCallback;
                return null != value;
            }
            else if(null != this.Relatives && this.Relatives.TryGetValue(variableName, out value))
            {
                return null != value;
            }
            else
                return Items.TryGetValue(variableName, out value);
        }

        protected class RelativeDimensions
        {
            public Size PageSize { get; set; }

            public Size ContainerSize { get; set; }

            public Size FontSize { get; set; }

            public Unit RootFontSize { get; set; }

            public bool UseWidthAsPriority { get; set; }

            public RelativeDimensions(Size page, Size container, Size font, Unit root, bool useWidth)
            {
                this.PageSize = page;
                this.ContainerSize = container;
                this.FontSize = font;
                this.RootFontSize = root;
                this.UseWidthAsPriority = useWidth;
            }

            public bool TryGetValue(string name, out object value)
            {
                if(string.IsNullOrEmpty(name) || !name.StartsWith(UnitRelativeVars.RelativeVarPrefix))
                {
                    value = null;
                    return false;
                }

                bool result;
                
                switch (name)
                {
                    case UnitRelativeVars.PageWidth:
                        value = PageSize.Width;
                        result = true;
                        break;
                    case UnitRelativeVars.PageHeight:
                        value = PageSize.Height;
                        result = true;
                        break;
                    case UnitRelativeVars.ContainerWidth:
                        value = ContainerSize.Height;
                        result = true;
                        break;
                    case UnitRelativeVars.ContainerHeight:
                        value = ContainerSize.Height;
                        result = true;
                        break;
                    case UnitRelativeVars.FontUpperHeight:
                        value = FontSize.Height;
                        result = true;
                        break;
                    case UnitRelativeVars.FontLowercaseHeight:
                        value = FontSize.Width;
                        result = true;
                        break;
                    case UnitRelativeVars.FontStandardWidth:
                        value = FontSize.Width;
                        result = true;
                        break;
                    case UnitRelativeVars.FontRootUpperHeight:
                        value = RootFontSize;
                        result = true;
                        break;
                    case UnitRelativeVars.WidthIsPriority:
                        value = UseWidthAsPriority;
                        result = true;
                        break;
                    default:
                        value = null;
                        result = false;
                        break;
                }

                return result;
            }
        }
    }
}
