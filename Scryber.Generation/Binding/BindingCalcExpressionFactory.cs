using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Scryber.Expressive;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Functions;
using Scryber.Expressive.Operators;



namespace Scryber.Binding
{
    public class BindingCalcExpressionFactory : IPDFBindingExpressionFactory, IExpressionFactory
    {

        public string BindingKey { get { return "{"; } }

        private Dictionary<string, string> _replacements = new Dictionary<string, string>()
        {
            {"&apos;", "'" },
            {"&amp;", "&" },
            {"&lt;", "<" },
            {"&gt;", ">" }
        };


        private ExpressiveOptions _options;
        private FunctionSet _stdFunctions;
        private OperatorSet _stdOperators;
        private IDictionary<string, Expression> _cache;
        private bool _useCache;

        public ExpressiveOptions Options
        {
            get { return _options; }
            set
            {
                if (value != _options)
                {
                    _options = value;
                    ResetBindingOptions();
                }
            }
        }

        public bool UseCache
        {
            get { return _useCache; }
            set { _useCache = value; }
        }

        public FunctionSet FactoryFunctions
        {
            get { return _stdFunctions; }
        }

        public OperatorSet FactoryOperators
        {
            get { return _stdOperators; }
        }

        public IDictionary<string, Expression> ExpressionCache
        {
            get { return _cache; }
        }

        
        public DocumentGenerationStage BindingStage
        {
            get { return DocumentGenerationStage.Bound; }
        }

        public BindingCalcExpressionFactory()
        {
#if DEBUG
            this._useCache = false;
#else
            this._useCache = true;
#endif

            this._options = Expressive.ExpressiveOptions.IgnoreCaseForParsing;
            this.ResetBindingOptions();
        }

        /// <summary>
        /// We don't do anything on Init
        /// </summary>
        public InitializedEventHandler GetInitBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            throw new NotSupportedException("Expression Binding is not supported on any other document lifecycle stage than the databinding");
        }

        /// <summary>
        /// We don't do anything on Load
        /// </summary>
        public LoadedEventHandler GetLoadBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            throw new NotSupportedException("Expression Binding is not supported on any other document lifecycle stage than the databinding");
        }

        /// <summary>
        /// Creates a new BindingCalcExpression handler to be added the component
        /// </summary>
        /// <param name="expressionvalue"></param>
        /// <param name="classType"></param>
        /// <param name="forProperty"></param>
        /// <returns></returns>
        public DataBindEventHandler GetDataBindingExpression(string expressionvalue, Type classType, PropertyInfo forProperty)
        {
            BindingCalcExpression expr = null;
            try
            {
                expr = this.CreateBindingExpression(expressionvalue, forProperty);
            }
            catch(Exception ex)
            {
                throw new PDFParserException("Could not parse the expression " + expressionvalue + ". " + ex.Message, ex);
            }

            return new DataBindEventHandler(expr.BindComponent);
        }



        protected virtual void ResetBindingOptions()
        {
            _stdFunctions = InitFunctions();
            _stdOperators = InitOperators();
            _cache = InitExpressionCache();
        }

        /// <summary>
        /// Initializes a new set of functions with the options for this class.
        /// These are added from the extension method for the FunctionSet
        /// </summary>
        /// <returns></returns>
        protected virtual FunctionSet InitFunctions()
        {
            var set = new FunctionSet(this.Options);
            set.AddDefaultFunctions();
            return set;
        }

        protected virtual OperatorSet InitOperators()
        {
            var set = new OperatorSet(this.Options);
            set.AddDefaultOperators();
            return set;
            
        }

        protected virtual IDictionary<string, Expression> InitExpressionCache()
        {
            return new ConcurrentDictionary<string, Expression>(StringComparer.Ordinal);
        }

        

        public Expressive.Context GetContext(ExpressiveOptions options)
        {
            return new Context(options, this._stdFunctions, this._stdOperators);
        }

        public virtual Expression CreateExpression(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.IndexOf('&') > -1)
                value = CleanXmlString(value);

            Expression expr;

            if (!this.UseCache || !this.ExpressionCache.TryGetValue(value, out expr))
            {
                var context = GetContext(this.Options);
                var parser = new BindingCalcParser(context);
                expr = new Expression(value, parser, context);

                try
                { 
                    expr.CompileExpression();
                }
                catch (Exception ex)
                {
                    throw new PDFParserException("Could not compile the expression '" + value + "'. " + ex.Message, ex);
                }

                if (UseCache)
                        this.ExpressionCache[value] = expr;
                
            }

            return expr;
        }

        public virtual BindingCalcExpression CreateBindingExpression(string value, PropertyInfo forProperty)
        {
            try
            {
                var expr = CreateExpression(value);
                return new BindingCalcExpression(expr, value, forProperty);
            }
            catch(Exception ex)
            {
                throw new PDFParserException("Could not bind the '" + value + "' expression. " + ex.Message, ex);
            }
        }

        private string CleanXmlString(string value)
        {
            StringBuilder buffer = new StringBuilder(value);
            foreach (var kvp in _replacements)
            {
                buffer.Replace(kvp.Key, kvp.Value);
            }

            return buffer.ToString();
        }


        public static void RegisterFunction(IFunction function)
        {
            BindingCalcFunctionSetExtensions.RegisterFunction(function);

            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            if (null != service)
            {
                foreach (var binding in service.ParsingOptions.Bindings)
                {
                    var factory = binding.GetFactory();
                    if (factory is BindingCalcExpressionFactory expressionFactory)
                        expressionFactory.ResetBindingOptions();
                }
            }
        }

        public static void RegisterFunction(params IFunction[] functions)
        {
            BindingCalcFunctionSetExtensions.RegisterFunction(functions);

            //Now reset the binding factories
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            if (null != service)
            {
                foreach (var binding in service.ParsingOptions.Bindings)
                {
                    var factory = binding.GetFactory();
                    if (factory is BindingCalcExpressionFactory expressionFactory)
                        expressionFactory.ResetBindingOptions();
                }
            }
        }

        public static void RegisterOperator(IOperator operation)
        {
            BindingCalcOperatorSetExtensions.RegisterOperator(operation);

            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            if (null != service)
            {
                foreach (var binding in service.ParsingOptions.Bindings)
                {
                    var factory = binding.GetFactory();
                    if (factory is BindingCalcExpressionFactory expressionFactory)
                        expressionFactory.ResetBindingOptions();
                }
            }
        }

        public static void RegisterOperator(params IOperator[] operations)
        {
            BindingCalcOperatorSetExtensions.RegisterOperator(operations);

            //Now reset the binding factories
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            if (null != service)
            {
                foreach (var binding in service.ParsingOptions.Bindings)
                {
                    var factory = binding.GetFactory();
                    if (factory is BindingCalcExpressionFactory expressionFactory)
                        expressionFactory.ResetBindingOptions();
                }
            }
        }


    }
}
