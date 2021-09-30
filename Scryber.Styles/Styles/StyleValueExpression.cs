using System;
using System.Collections.Generic;
using Scryber.Binding;
using Scryber.Expressive;
using Scryber.Options;

namespace Scryber.Styles
{
    /// <summary>
    /// A style value that is an expression, rather than a constant value.
    /// </summary>
    /// <typeparam name="T">The output type of the expression</typeparam>
    /// <remarks>
    /// The style value expression overrides the base class to dymanically exaluate an expression, rather than a static value.
    /// It uses the databinding on the style itself to set up the expression, then
    /// overrides the Value(forStyle) method to evaluate the expression each time.
    /// </remarks>
    public class StyleValueExpression<T> : StyleValue<T>
    {
        //
        // ivars
        //

        private Scryber.Expressive.Expression _expression;
        private Expressive.IVariableProvider _variableProvider;
        private StyleValueConvertor<T> _convertor;

        //
        // properties
        //

        #region public string ExpressionString {get;}

        private string _expressionString;

        /// <summary>
        /// Gets the expression that the value will be evaluated from
        /// </summary>
        public string ExpressionString
        {
            get { return _expressionString; }
        }

        #endregion

        #region protected bool CanEvaluate {get;}

        /// <summary>
        /// Returns true if this expression value can execute an evaluation of the expression
        /// </summary>
        protected bool CanEvaluate
        {
            get
            {
                return this._expression != null;
            }
        }

        #endregion

        //
        // .ctor
        //

        #region public StyleValueExpression(PDFStyleKey<T> key, string expressionString, StyleValueConvertor<T> convertor)

        /// <summary>
        /// Creates a new style value expression for the style key with the expression and an optional convertor
        /// </summary>
        /// <param name="key">The style key this expression is assigned for</param>
        /// <param name="expressionString">The exppresion to be evaluated at runtime</param>
        /// <param name="convertor">An otpional convertor to make the result the required type</param>
        public StyleValueExpression(StyleKey<T> key, string expressionString, StyleValueConvertor<T> convertor)
            : this(key, expressionString, convertor, default)
        {
        }

        #endregion


        #region public StyleValueExpression(PDFStyleKey<T> key, string expressionString, StyleValueConvertor<T> convertor, T baseValue)

        /// <summary>
        /// Creates a new style value expression for the style key with the expression and an optional convertor and base value
        /// </summary>
        /// <param name="key">The style key this expression is assigned for</param>
        /// <param name="expressionString">The exppresion to be evaluated at runtime</param>
        /// <param name="convertor">An otpional convertor to make the result the required type</param>
        /// <param name="baseValue">An optional base value that will be returned before the expression is bound</param>
        public StyleValueExpression(StyleKey<T> key, string expressionString, StyleValueConvertor<T> convertor, T baseValue)
            : base(key, baseValue)
        {
            this._expressionString = expressionString ?? throw new ArgumentNullException(nameof(expressionString));
            this._convertor = convertor;
            this._variableProvider = null;
            this._expression = null;
        }

        #endregion

        //
        // base class overrides
        //

        #region public override T Value(StyleBase forStyle)

        /// <summary>
        /// Overrides the base implementation to return the result of the evaluation if possible.
        /// </summary>
        /// <param name="forStyle"></param>
        /// <returns></returns>
        public override T Value(StyleBase forStyle)
        {
            if (this.CanEvaluate == false)
            {
                this.EnsureExpression(forStyle);
            }

            return EvaluateExpression(forStyle);
            
        }

        #endregion

        //
        // Implementation
        //

        #region public void BindValue(object sender, PDFDataBindEventArgs args)

        /// <summary>
        /// Provides a method that can be added to the databind or databound event handlers,
        /// and will be called when that component or style is databound.
        /// </summary>
        /// <param name="sender">The sender of the event (must be a Style or an IPDFStyledComponent</param>
        /// <param name="args">The databind args with the context for creating the expression</param>
        /// <remarks>
        /// This method must be called before the Expression will be used, so the Expression can be built.
        /// If it is not called then the instance will fallback to the default base implementation
        /// </remarks>
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

            this._expression = CreateExpression();
            this._variableProvider = context.Items.ValueProvider(context.CurrentIndex,
                                            context.DataStack.HasData ? context.DataStack.Current : null);
            

            //Execute once to make sure we are all set up - although css variables may not be there.
            base.SetValue(this.EvaluateExpression(style));
        }

        #endregion

        /// <summary>
        /// Creates an expression without the varabile provider
        /// </summary>
        /// <param name="forStyle"></param>
        /// <returns></returns>
        protected virtual bool EnsureExpression(StyleBase forStyle)
        {
            if(null == _expression)
            {
                _expression = CreateExpression();
                _variableProvider = null;
            }
            return null != _expression;
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

        #region protected virtual T EvaluateExpression(StyleBase forStyle)

        /// <summary>
        /// Preforms the actual execution and optionally conversion of an expression into a valid style value
        /// </summary>
        /// <param name="forStyle"></param>
        /// <returns></returns>
        protected virtual T EvaluateExpression(StyleBase forStyle)
        {
            IVariableProvider provider = this._variableProvider;

            if (forStyle is Style style && style.HasVariables)
                provider = new StyleChainedVariableProvider(style.Variables, provider);

            object value = _expression.Evaluate(provider);
            T result;

            if (null != _convertor)
                _convertor(forStyle, value, out result);
            else if (value is T)
                result = (T)value;
            else
                throw new InvalidCastException("Could not convert the returned value to type " + typeof(T) + ", please provide a convertor");

            return result;
        }

        #endregion

    }
}
