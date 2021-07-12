using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Scryber.Expressive;

namespace Scryber.Binding
{
    public class BindingCalcExpressionFactory : IPDFBindingExpressionFactory
    {

        public string BindingKey { get { return "{"; } }

        private Dictionary<string, string> _replacements = new Dictionary<string, string>()
        {
            {"&apos;", "'" },
            {"&amp;", "&" },
            {"&lt;", "<" },
            {"&gt;", ">" }
        };


        private object _lock = new object();

        public Dictionary<string,Expressive.Expression> ExpressionCache { get; set; }

        private ExpressiveOptions _options;
        public ExpressiveOptions Options
        {
            get { return _options; }
            set
            {
                _options = value;
                //We reset the cache based on the options
                this.ExpressionCache = GetDictionary(value);
            }
        }

        public DocumentGenerationStage BindingStage
        {
            get { return DocumentGenerationStage.Bound; }
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
            BindingCalcExpression expr = this.CreateExpression(expressionvalue, forProperty);
            return new PDFDataBindEventHandler(expr.BindComponent);
        }

        public BindingCalcExpressionFactory()
        {
            this.Options = Expressive.ExpressiveOptions.IgnoreCaseForParsing;
            //this.ExpressionCache = GetDictionary(this.Options); Doesn't need setting as the setting of options property will initialize the dictionary
        }

        private Dictionary<string, Expression> GetDictionary(ExpressiveOptions options)
        {
            if ((options & ExpressiveOptions.IgnoreCaseForParsing) == 0)
                return new Dictionary<string, Expression>(StringComparer.CurrentCulture);
            else
                return new Dictionary<string, Expression>(StringComparer.CurrentCultureIgnoreCase);
        }

        public virtual BindingCalcExpression CreateExpression(string value, PropertyInfo forProperty)
        {
            if (!string.IsNullOrEmpty(value) && value.IndexOf('&') > -1)
                value = CleanXmlString(value);

            Expression expr;
            lock (_lock)
            {
                if (!this.ExpressionCache.TryGetValue(value, out expr))
                {
                    expr = new Expression(value, this.Options);

                    //Check for caching flag before compiling and adding
                    if ((this.Options & ExpressiveOptions.NoCache) == 0)
                    {
                        expr.CompileExpression();
                        this.ExpressionCache.Add(value, expr);
                    }
                }
            }
            return new BindingCalcExpression(expr, forProperty);
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
