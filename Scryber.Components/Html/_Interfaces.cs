using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Html.Parsing;

namespace Scryber.Html
{


    public interface IHtmlContentParser : IEnumerable<HTMLParserResult>
    {
        IParserStyleFactory StyleFactory { get; }
        IParserComponentFactory ComponentFactory { get; }

        bool IsLogging { get; }

        void Log(string message);
    }



    public interface IParserComponentFactory
    {
        IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type);

        IPDFComponent GetTextComponent(IHtmlContentParser parser, string text);

        /// <summary>
        /// Retuns true if the component can contain other components or text
        /// </summary>
        /// <param name="component"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsContainerComponent(IHtmlContentParser parser, IPDFComponent component, string name);

        void SetAttribute(IHtmlContentParser parser, IPDFComponent parsed, string componentName, string attrName, string attrValue);
    }

    public interface IParserStyleFactory
    {
        bool SetStyleValue(IHtmlContentParser parser, IPDFStyledComponent component, CSSStyleItemReader stylereader);
    }

    
}
