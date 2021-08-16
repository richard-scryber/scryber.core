using System;
using Scryber.Binding;
using Scryber.Expressive;
using Scryber.Expressive.Expressions;

namespace Scryber.Styles
{

    public delegate bool StyleValueConvertor<T>(object value, PDFDataContext context, out T result);

    public class StyleBindingExpression<T>
    {
        public PDFStyleKey<T> StyleKey { get; }

        public StyleValueConvertor<T> Convertor { get; }

        public IVariableProvider ItemValueProvider { get; private set; }

        public Expression Expression { get; private set; }

        public StyleBindingExpression(PDFStyleKey<T> key, Expression expression, StyleValueConvertor<T> convertor)
        {
            this.StyleKey = key;
            this.Convertor = convertor;
            this.Expression = expression;
        }

        public void BindValue(object sender, PDFDataBindEventArgs args)
        {
            if (null == this.ItemValueProvider)
                this.ItemValueProvider = args.Context.Items.ValueProvider();

            Style style = (Style)sender;

            object value = this.Expression.Evaluate(this.ItemValueProvider);

            T result;
            if(this.Convertor(value, args.Context, out result))
            {
                style.SetValue(this.StyleKey, result);
            }

        }
    }
}
