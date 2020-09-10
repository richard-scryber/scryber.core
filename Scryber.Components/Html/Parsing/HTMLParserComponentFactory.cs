using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;

namespace Scryber.Html.Parsing
{
    internal class HTMLParserComponentFactory : IParserComponentFactory
    {

        /// <summary>
        /// The set of default known tags and their factories. Changes made to this collection will affect all default constructed component factories.
        /// </summary>
        public static readonly Dictionary<string, IParserComponentFactory> DefaultTags; //Initialized in static constructor

        #region ivars

        //A quick cache for the last one
        private IParserComponentFactory _last;
        private string _lastName;
        private IDictionary<string, IParserComponentFactory> _knowntags;

        #endregion

        #region public IDictionary<string, IParserComponentFactory> TagFactories

        /// <summary>
        /// Gets the collection of tags that are known and the factories associated with each one.
        /// </summary>
        public IDictionary<string, IParserComponentFactory> TagFactories
        {
            get { return _knowntags; }
        }

        #endregion

        //
        // .ctor
        //

        #region public HTMLParserComponentFactory()

        /// <summary>
        /// Initialises a new factory that contains the default set of individual component factories.
        /// </summary>
        public HTMLParserComponentFactory()
            : this(new ReadOnlyDictionary<string, IParserComponentFactory>(DefaultTags))
        {
        }

        #endregion

        #region public HTMLParserComponentFactory(IDictionary<string,IParserComponentFactory> knowntags)

        /// <summary>
        /// Instaniates a new component factory that contains the provided specific set of component factories
        /// </summary>
        /// <param name="knowntags"></param>
        public HTMLParserComponentFactory(IDictionary<string,IParserComponentFactory> knowntags)
        {
            _knowntags = knowntags;
        }

        #endregion

        //
        // methods
        //

        #region public IPDFComponent GetComponent(IContentParser parser, string name)

        /// <summary>
        /// Returns a new component for the parser based on the specified tag name.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="name"></param>
        /// <returns>The instaniated component or null if the name is not recognised</returns>
        public IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            IPDFComponent proxy = null;
            IParserComponentFactory innerfact;
            if (null != _last && _lastName == name)
            {
                proxy = _last.GetComponent(parser, name, out type);
            }
            else if (_knowntags.TryGetValue(name, out innerfact))
            {
                _last = innerfact;
                _lastName = name;
                proxy = innerfact.GetComponent(parser, name, out type);
            }
            else
            {
                _last = null;
                _lastName = null;
                type = HtmlComponentType.Unknown;
                proxy = GetUnknownComponent(parser, name);
            }

            if (proxy is Component)
                ((Component)proxy).Tag = name;
            return proxy;
        }

        #endregion

        #region protected virtual IPDFComponent GetUnknownComponent(IContentParser parser, string name)

        /// <summary>
        /// Returns the default value for an unknown tag name - in this case null
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual IPDFComponent GetUnknownComponent(IHtmlContentParser parser, string name)
        {
            return null;
        }

        #endregion

        #region public bool IsContainerComponent(IHtmlContentParser parser, IPDFComponent component, string name)

        /// <summary>
        /// Returns true if the component is a container of other components or text
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="component"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsContainerComponent(IHtmlContentParser parser, IPDFComponent component, string name)
        {
            bool container = false;
            IParserComponentFactory innerfact;
            if (null != _last && _lastName == name)
            {
                container = _last.IsContainerComponent(parser, component, name);
            }
            else if (_knowntags.TryGetValue(name, out innerfact))
            {
                _last = innerfact;
                _lastName = name;
                container = _last.IsContainerComponent(parser, component, name);
            }
            else
            {
                _last = null;
                _lastName = null;
                container = false;
            }
            return container;
        }

        #endregion

        #region public virtual IPDFComponent GetTextComponent(IContentParser parser, string text)

        /// <summary>
        /// Retuns a new textual component that represents the text string provided
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual IPDFComponent GetTextComponent(IHtmlContentParser parser, string text)
        {
            _last = null;
            _lastName = null;
            return new TextLiteral(text);
        }

        #endregion

        #region public void SetAttribute(IContentParser parser, IComponent parsed, string componentName, string attrName, string attrValue)

        /// <summary>
        /// Sets the parsed value provided on an attribute with the specified name to the parsed component of the provided name
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="parsed"></param>
        /// <param name="componentName"></param>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        public void SetAttribute(IHtmlContentParser parser, IPDFComponent parsed, string componentName, string attrName, string attrValue)
        {
            IParserComponentFactory innerfact;

            //Quick check on the last one used - as this generally follows the component creation
            if (null != _last && _lastName == componentName)
            {
                _last.SetAttribute(parser, parsed, componentName, attrName, attrValue);
            }
            else if (_knowntags.TryGetValue(componentName, out innerfact))
            {
                _last = innerfact;
                _lastName = componentName;
                innerfact.SetAttribute(parser, parsed, componentName, attrName, attrValue);
            }
            else
            {
                //Skip the attribute
            }
            
        }

        #endregion

        //
        // ..ctor

        #region static HTMLParserComponentFactory()

        /// <summary>
        /// Static constructor that initializes the known factories
        /// </summary>
        static HTMLParserComponentFactory()
        {
            Dictionary<string, IParserComponentFactory> known = InitKnownFactories();
            DefaultTags = known;
        }

        #endregion

        #region private static Dictionary<string, IParserComponentFactory> InitKnownFactories()

        /// <summary>
        /// Creates and returns a new Dictionary of known Tags
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, IParserComponentFactory> InitKnownFactories()
        {
            Dictionary<string, IParserComponentFactory> known = new Dictionary<string, IParserComponentFactory>(StringComparer.OrdinalIgnoreCase);
            known.Add("body", new HTMLBodyFactory());
            known.Add("span", new HTMLSpanFactory());
            known.Add("div", new HTMLDivCompFactory());
            known.Add("table", new HTMLTableFactory());
            known.Add("tr", new HTMLTableRowFactory());
            known.Add("td", new HTMLTableCellFactory());
            known.Add("th", new HTMLTableHeaderCellFactory());
            known.Add("ul", new HTMLUnorderedListFactory());
            known.Add("ol", new HTMLOrderedListFactory());
            known.Add("li", new HTMLListItemFactory());
            known.Add("h1", new HTMLHeadingFactory());
            known.Add("h2", new HTMLHeadingFactory());
            known.Add("h3", new HTMLHeadingFactory());
            known.Add("h4", new HTMLHeadingFactory());
            known.Add("h5", new HTMLHeadingFactory());
            known.Add("h6", new HTMLHeadingFactory());
            known.Add("blockquote", new HTMLDivCompFactory());
            known.Add("b", new HTMLSpanFactory());
            known.Add("i", new HTMLSpanFactory());
            known.Add("u", new HTMLSpanFactory());
            known.Add("em", new HTMLSpanFactory());
            known.Add("strong", new HTMLSpanFactory());
            known.Add("p", new HTMLParagraphFactory());
            known.Add("code", new HTMLCodeFactory());
            known.Add("pre", new HTMLCodeFactory());
            known.Add("img", new HTMLImageFactory());
            known.Add("br", new HTMLLineBreakFactory());
            known.Add("dl", new HTMLDefinitionListFactory());
            known.Add("dt", new HTMLDefinitionListItemFactory());
            known.Add("fieldset", new HTMLDivCompFactory());
            known.Add("legend", new HTMLDivCompFactory());
            known.Add("font", new HTMLSpanFactory());
            known.Add("a", new HTMLLinkFactory());
            known.Add("hr", new HTMLHorizontalRuleFactory());
            return known;
        }


        #endregion

    }
}
