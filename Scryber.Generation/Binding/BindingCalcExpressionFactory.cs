using System;
using System.Reflection;

namespace Scryber.Binding
{
    public class BindingCalcExpressionFactory : IPDFBindingExpressionFactory
    {

        public string BindingKey { get { return "{"; } }

        
        public Expressive.ExpressiveOptions Options { get; set; }

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
        }

        public virtual BindingCalcExpression CreateExpression(string value, PropertyInfo forProperty)
        {
            return new BindingCalcExpression(value, forProperty, this.Options);
        }
    }
}
