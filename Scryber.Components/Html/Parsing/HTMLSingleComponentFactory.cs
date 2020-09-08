using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Html.Components;

namespace Scryber.Html.Parsing
{
    /// <summary>
    /// Abstract class that support the individual factories in creating specific components and applying attribute values
    /// </summary>
    internal abstract class HTMLSingleComponentFactory : IParserComponentFactory
    {

        public const string ClassAttrName = "class";
        public const string IdAttrName = "id";

        public static readonly string[] StyleAttrNames = new string[] { "border", "border-color", "border-width","border-style", "color", 
                                                                            "background", "background-image", "background-color", "background-repeat", "background-position", "background-position-x", "background-position-y",
                                                                            "font","font-family","font-size","font-style","font-weight","font-variant",
                                                                            "top", "left", "width","height",
                                                                            "margin","margin-left","margin-right","margin-top","margin-bottom",
                                                                            "padding","padding-left","padding-right","padding-top","padding-bottom",
                                                                            "white-space","overflow","visibility"};



        internal HTMLSingleComponentFactory()
        {
        }

        #region public abstract IComponent GetComponent(IContentParser parser, string name, out HtmlComponentType type, PDFContextBase context);

        /// <summary>
        /// Abstract method that inheritors implement to create and return specific types of components.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type);

        #endregion

        public virtual bool IsContainerComponent(IHtmlContentParser parser, IPDFComponent component, string name)
        {
            return true;
        }

        #region IComponent GetTextComponent(...)

        /// <summary>
        /// Support methods for proxies
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual IPDFComponent GetTextComponent(IHtmlContentParser parser, string text)
        {
            return new PDFTextLiteral(text);
        }

        
        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="parsed"></param>
        /// <param name="componentName"></param>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        public virtual void SetAttribute(IHtmlContentParser parser, IPDFComponent parsed, string componentName, string attrName, string attrValue)
        {
            if (attrName == ClassAttrName && parsed is IPDFStyledComponent)
                ((IPDFStyledComponent)parsed).StyleClass = attrValue;
            else if (attrName == IdAttrName)
                parsed.ID = attrValue;
            else if (StyleAttrNames.Contains(attrName) && parsed is IPDFStyledComponent)
                this.SetStyleAttribute(parsed as IPDFStyledComponent, attrName, attrValue);

        }

        protected virtual void SetStyleAttribute(IPDFStyledComponent comp, string name, string value)
        {
            //TODO: Implement style parsing.
        }
    }

    internal class HTMLBodyFactory : HTMLSingleComponentFactory
    {
        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.Body;
            HTMLBody body = new HTMLBody();
            body.ID = "html-body";
            return body;
        }
    }

    internal class HTMLDivCompFactory : HTMLSingleComponentFactory
    {
        public HTMLDivCompFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.Panel;
            if (String.Equals("blockquote", name, StringComparison.OrdinalIgnoreCase))
                return new HTMLBlockQuote();
            else if (String.Equals("fieldset", name, StringComparison.OrdinalIgnoreCase))
                return new HTMLFieldSet();
            else if (String.Equals("legend", name, StringComparison.OrdinalIgnoreCase))
                return new HTMLLegend();
            else
                return new HTMLDiv();
        }
    }

    internal class HTMLSpanFactory : HTMLSingleComponentFactory
    {
        public HTMLSpanFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            PDFSpanBase span;
            type = HtmlComponentType.Span;
            switch (name.ToLower())
            {
                case "b":
                case "strong":
                    span = new HTMLBoldSpan();
                    break;
                case "i":
                case "em":
                    span = new HTMLItalicSpan();
                    break;
                case "u":
                    span = new HTMLUnderlinedSpan();
                    break;
                case("font"):
                    span = new HTMLFontSpan();
                    break;
                default:
                    span = new HTMLSpan();
                    break;
            }
            return span;
        }
    }

    internal class HTMLTableFactory : HTMLSingleComponentFactory
    {
        public HTMLTableFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.Table;
            return new HTMLTableGrid();
        }
    }

    internal class HTMLTableRowFactory : HTMLSingleComponentFactory
    {
        public HTMLTableRowFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.TableRow;
            return new HTMLTableRow();
        }
    }

    internal class HTMLTableCellFactory : HTMLSingleComponentFactory
    {
        public HTMLTableCellFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.TableCell;
            return new HTMLTableCell();
        }

        public override void SetAttribute(IHtmlContentParser parser, IPDFComponent parsed, string componentName, string attrName, string attrValue)
        {
            if (attrName == "colspan")
            {
                int colspan;
                if(int.TryParse(attrValue, out colspan))
                    ((PDFTableCell)parsed).Style.Table.CellColumnSpan = colspan;
            }
            else
                base.SetAttribute(parser, parsed, componentName, attrName, attrValue);
        }
    }

    internal class HTMLUnorderedListFactory : HTMLSingleComponentFactory
    {
        public HTMLUnorderedListFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.List;
            return new HTMLListUnordered();
        }
    }

    internal class HTMLOrderedListFactory : HTMLSingleComponentFactory
    {
        public HTMLOrderedListFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.List;
            return new HTMLListOrdered();
        }
    }

    internal class HTMLListItemFactory : HTMLSingleComponentFactory
    {
        public HTMLListItemFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.ListItem;
            return new HTMLListItem();
        }
    }

    internal class HTMLDefinitionListFactory : HTMLSingleComponentFactory
    {
        public HTMLDefinitionListFactory()
            : base()
        {
        }
        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.List;
            return new HTMLListDefinition();
        }
    }

    internal class HTMLDefinitionListItemFactory : HTMLSingleComponentFactory
    {
        public HTMLDefinitionListItemFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.ListItem;
            return new HTMLListDefinitionItem();
        }
    }

    internal class HTMLHeadingFactory : HTMLSingleComponentFactory
    {
        public HTMLHeadingFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.Heading;
            PDFHeadingBase head;
            switch (name.ToLower())
            {
                case "h1":
                    head = new HTMLHead1();
                    break;
                case "h2":
                    head = new HTMLHead2();
                    break;
                case "h3":
                    head = new HTMLHead3();
                    break;
                case "h4":
                    head = new HTMLHead4();
                    break;
                case "h5":
                    head = new HTMLHead5();
                    break;
                case "h6":
                    head = new HTMLHead6();
                    break;
                default:
                    head = null;
                    break;
            }
            return head;
        }
    }

    internal class HTMLParagraphFactory : HTMLSingleComponentFactory
    {
        public HTMLParagraphFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.Panel;
            return new HTMLParagraph();
        }
    }

    internal class HTMLTableHeaderCellFactory : HTMLSingleComponentFactory
    {
        public HTMLTableHeaderCellFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.TableCell;
            return new HTMLTableHeaderCell();
        }
    }

    

    internal class HTMLCodeFactory : HTMLSingleComponentFactory
    {
        public HTMLCodeFactory()
            : base()
        {
        }
        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.Preformatted;
            return new HTMLPreformatted();
        }
    }

    internal class HTMLLineBreakFactory : HTMLSingleComponentFactory
    {
        public HTMLLineBreakFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.LineBreak;
            return new HTMLLineBreak();
        }

        public override bool IsContainerComponent(IHtmlContentParser parser, IPDFComponent component, string name)
        {
            return false;
        }
    }

    internal class HTMLImageFactory : HTMLSingleComponentFactory
    {
        public HTMLImageFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.Image;
            return new HTMLImage();
        }

        public override bool IsContainerComponent(IHtmlContentParser parser, IPDFComponent component, string name)
        {
            return false;
        }

        public override void SetAttribute(IHtmlContentParser parser, IPDFComponent parsed, string componentName, string attrName, string attrValue)
        {
            if (attrName == "src")
               ((PDFImage) parsed).Source = attrValue;
            else
                base.SetAttribute(parser, parsed, componentName, attrName, attrValue);
        }
    }

    internal class HTMLHorizontalRuleFactory : HTMLSingleComponentFactory
    {
        public HTMLHorizontalRuleFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.HorizontalLine;
            return new HTMLHorizontalRule();
        }

        public override bool IsContainerComponent(IHtmlContentParser parser, IPDFComponent component, string name)
        {
            return false;
        }
    }

    internal class HTMLLinkFactory : HTMLSingleComponentFactory
    {
        public HTMLLinkFactory()
            : base()
        {
        }

        public override IPDFComponent GetComponent(IHtmlContentParser parser, string name, out HtmlComponentType type)
        {
            type = HtmlComponentType.Link;
            return new HTMLAnchor();
        }

        public override void SetAttribute(IHtmlContentParser parser, IPDFComponent parsed, string componentName, string attrName, string attrValue)
        {
            if (attrName == "href")
            {
                HTMLAnchor link = (HTMLAnchor)parsed;
                if (string.IsNullOrEmpty(attrValue))
                { }
                else if (attrValue.StartsWith("#"))
                    link.Destination = attrValue;
                else
                    link.File = attrValue;
            }
            else
                base.SetAttribute(parser, parsed, componentName, attrName, attrValue);
        }
    }
}
