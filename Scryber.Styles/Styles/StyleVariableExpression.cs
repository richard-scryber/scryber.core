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

        /// <summary>
        /// Compiles the expression (without evaluating it) so an invalid expression is raised when the css is parsed.
        /// </summary>
        public void ValidateExpression()
        {
            try
            {
                this.CreateExpression();
            }
            catch (Exception ex)
            {
                throw new Scryber.Styles.Parsing.CSSExpressionParseException("The css expression '" + this._expressionString + "' is not valid: " + ex.Message, ex);
            }
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
            
            object value;
            try
            {
                value = this._expression.Evaluate(this._variableProvider);
            }
            catch (Exception ex) when (Scryber.Expressive.Exceptions.ExpressionErrors.IsSyntaxError(ex))
            {
                //The expression itself is invalid - raised in strict AND lax mode (PDFDataException is not consumed by Component.DataBind).
                throw new PDFDataException("The style variable expression '" + this._expressionString + "' is not valid: " + ex.Message, ex);
            }
            catch (Exception ex) when (Scryber.Expressive.Exceptions.NullRootVariableException.IsCausedBy(ex))
            {
                //The root variable has not been set or is null - always logged as an error (strict or lax)
                context.TraceLog.Add(Scryber.TraceLevel.Error, "Style Binding", "The style variable expression '" + this._expressionString + "' could not be evaluated: " + ex.Message);
                value = null;
            }

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