using System;
using Scryber.Binding;
using Scryber.Expressive;
using Scryber.Options;

namespace Scryber.Styles
{
    public class StyleVariableExpression : StyleVariable
    {

        private readonly string _expressionString;
        private Scryber.Expressive.Expression _expression;
        private Expressive.IVariableProvider _variableProvider;
        
        /// <summary>
        /// Gets the expression to be evaluated and set to the variable
        /// </summary>
        public string ExpressionString
        {
            get => _expressionString;
        }

        public StyleVariableExpression(string name, string expression) : base(name, null)
        {
            this._expressionString = expression;
        }

        public void BindValue(object sender, DataBindEventArgs args)
        {
            Style style;
            
            if(sender is Style)
                style =  (Style)sender;
            else if(sender is IStyledComponent)
                style = ((IStyledComponent)sender).Style;
            else
                throw new InvalidCastException("Style variable expressions can only be set on styles, or Styled Components");
            
            var context = args.Context;
            this._expression = CreateExpression();
            this._variableProvider = context.Items.ValueProvider(context.CurrentIndex, context.DataStack.HasData ? context.DataStack.Current : null, context.DataStack);
            this._expression.BindExpression(this._variableProvider);
            
            var value = this._expression.Evaluate(this._variableProvider);
            
            if (null == value)
                this.Value = null;
            else
                this.Value = value.ToString();

        }
        
        #region protected virtual Expression CreateExpression(PDFDataContext context)

        /// <summary>
        /// Creates the Expressive.Expression with the datacontext
        /// </summary>
        /// <param name="context">The datacontext for the current dataitem and index</param>
        /// <returns>A compiled expression or null</returns>
        protected virtual Expression CreateExpression()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();

            var prefix = ParsingOptions.CalcBindingPrefix;
            var factory = config.ParsingOptions.GetBindingFactoryForPrefix(prefix) as IExpressionFactory;

            if (null == factory)
                throw new InvalidOperationException("Cannot use expressions without a valid BindingExpressionFactory that supports the IExpressionFactory interface");

            var expr = factory.CreateExpression(this._expressionString);

            return expr;
        }

        #endregion
    }
}