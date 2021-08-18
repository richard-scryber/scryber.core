using System;
using System.Collections.Generic;
using Scryber.Binding;
using Scryber.Expressive;

namespace Scryber.Styles
{
    public class StyleValueExpression<T> : StyleValue<T>
    {

        private Scryber.Expressive.Expression _expression;
        private Expressive.IVariableProvider _variableProvider;
        private StyleValueConvertor<T> _convertor;
        protected Style _style;

        private string _expressionString;

        

        public StyleValueExpression(PDFStyleKey<T> key, string expressionString, StyleValueConvertor<T> convertor)
            : base(key, default)
        {
            this._expressionString = expressionString ?? throw new ArgumentNullException(nameof(expressionString));
            this._convertor = convertor;
            this._variableProvider = null;
            this._expression = null;
        }

        public override T Value(StyleBase forStyle)
        {
            return EvaluateExpression(forStyle);
        }

        public void BindValue(object sender, PDFDataBindEventArgs args)
        {
            Style style;

            if (sender is Style)
                style = (sender as Style);
            else if (sender is IPDFStyledComponent)
                style = (sender as IPDFStyledComponent).Style;
            else
                throw new InvalidCastException("Style values can only be bound on styles or the StyledComponents that own them");

            var context = args.Context;
            if (null == _expression)
            {
                _expression = CreateExpression(context);
                _variableProvider = context.Items.ValueProvider();
            }
        }

        protected virtual Expressive.Expression CreateExpression(PDFDataContext context)
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            var factory = config.ParsingOptions.GetBindingFactoryForPrefix("calc") as BindingCalcExpressionFactory;
            var expr = factory.CreateExpression(this._expressionString);

            if (null != expr)
            {
                expr.ExpressionContext.CurrentDataContext = context.DataStack.HasData ? context.DataStack.Current : null;
                expr.ExpressionContext.CurrentDataIndex = context.CurrentIndex;
            }
            return expr;
        }

        protected virtual T EvaluateExpression(StyleBase forStyle)
        {
            IVariableProvider provider = this._variableProvider;

            if (forStyle is Style style && style.HasVariables)
                provider = new ChainedStyleVariableProvider(style.Variables, provider);

            object value = _expression.Evaluate(provider);
            T result;

            if (null != _convertor)
                _convertor(forStyle, value, null, out result);
            else if (value is T)
                result = (T)value;
            else
                throw new InvalidCastException("Could not convert the returned value to type " + typeof(T) + ", please provide a convertor");

            return result;
        }


        private class ChainedStyleVariableProvider : IVariableProvider
        {

            private IVariableProvider _previous;
            private StyleVariableSet _variables;

            public ChainedStyleVariableProvider(StyleVariableSet vars, IVariableProvider prev)
            {
                this._variables = vars;
                this._previous = prev;
            }

            public bool TryGetValue(string variableName, out object value)
            {
                StyleVariable styleVar;
                if(null != this._variables && this._variables.TryGetValue(variableName, out styleVar))
                {
                    value = styleVar.Value;
                    return true;
                }
                else if(null != this._previous)
                {
                    return this._previous.TryGetValue(variableName, out value);
                }
                else
                {
                    value = null;
                    return false;
                }
            }
        }
    }
}
