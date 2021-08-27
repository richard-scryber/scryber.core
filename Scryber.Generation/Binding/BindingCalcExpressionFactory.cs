using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Printing;
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
                    _stdFunctions = InitFunctions();
                    _stdOperators = InitOperators();
                    _cache = InitExpressionCache();
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
            this._useCache = true;
            this.Options = Expressive.ExpressiveOptions.IgnoreCaseForParsing;
        }

        public PDFInitializedEventHandler GetInitBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            throw new NotSupportedException("Expression Binding is not supported on any other document lifecycle stage than the databinding");
        }

        public PDFLoadedEventHandler GetLoadBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            throw new NotSupportedException("Expression Binding is not supported on any other document lifecycle stage than the databinding");
        }


        public PDFDataBindEventHandler GetDataBindingExpression(string expressionvalue, Type classType, PropertyInfo forProperty)
        {
            BindingCalcExpression expr = this.CreateBindingExpression(expressionvalue, forProperty);
            return new PDFDataBindEventHandler(expr.BindComponent);
        }

        protected virtual FunctionSet InitFunctions()
        {
            if ((this.Options & ExpressiveOptions.IgnoreCaseForParsing) > 0)
                return FunctionSet.CreateDefaultSet(StringComparer.OrdinalIgnoreCase);
            else
                return FunctionSet.CreateDefaultSet(StringComparer.Ordinal);
        }

        protected virtual OperatorSet InitOperators()
        {
            if ((this.Options & ExpressiveOptions.IgnoreCaseForParsing) > 0)
                return OperatorSet.CreateDefault(StringComparer.OrdinalIgnoreCase);
            else
                return OperatorSet.CreateDefault(StringComparer.Ordinal);
        }

        protected virtual IDictionary<string, Expression> InitExpressionCache()
        {
            //We are always case sensitive on the cache.
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
                expr.CompileExpression();

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

        

    }
}
