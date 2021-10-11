using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Html;
using Scryber.Html.Components;
using Scryber.Html.Parsing;

namespace Scryber.Components
{
    [PDFParsableComponent("HtmlFragment")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_htmlFragment")]
    public class HtmlFragment : Scryber.Components.VisualComponent
    {

        private List<IComponent> _added = null;
        private bool _parsed = false;
        private bool _unencode = false;

        private System.Xml.XmlNode _contents;
        private string _contentsAsString;
        private string _source;
        private string _rawContent;

        [PDFElement("")]
        [PDFAttribute("contents")]
        public System.Xml.XmlNode XHTMLContents
        {
            get { return _contents; }
            set
            {
                _contents = value;
                _parsed = false;
            }
        }
        [PDFAttribute("raw")]
        [PDFDesignable("Html Content", Category = "General", Priority = 6, Type = "String")]
        public string RawContents
        {
            get { return _rawContent; }
            set { _rawContent = value; _parsed = false; }
        }

        [PDFAttribute("unencode")]
        [PDFDesignable("Unencode Content",Category = "Data", Priority = 3, Type = "Boolean")]
        public bool UnEncode
        {
            get { return _unencode; }
            set { _unencode = value; }
        }

        public string ContentsAsString
        {
            get { return this._contentsAsString; } 
            set { this._contentsAsString = value; }
        }

        [PDFAttribute("source")]
        [PDFDesignable("Html Source", Category = "General", Priority = 6, Type = "Url")]
        public string Source
        {
            get { return _source; }
            set
            {
                _source = value;
                _parsed = false;
            }
        }

        [PDFAttribute("format")]
        [PDFDesignable("Html Format", Category = "General", Priority = 6, Type = "Select")]
        public HtmlFormatType Format { get; set; }


        public HtmlFragment()
            : this((ObjectType)"htmF")
        {
        }

        protected HtmlFragment(ObjectType type)
            : base(type)
        {
        }

        

        protected override void OnLoaded(LoadContext context)
        {
            bool performload = false;
            this.EnsureContentsParsed(context, performload);
            base.OnLoaded(context);
        }

        protected override void OnPreLayout(LayoutContext context)
        {
            bool performload = true;
            this.EnsureContentsParsed(context, performload);
            base.OnPreLayout(context);
        }

        

        protected virtual void EnsureContentsParsed(ContextBase context, bool performload)
        {
            if (_parsed)
                return;

            IContainerComponent container = this.GetContainerParent();
            int index = container.Content.IndexOf(this);
            try
            {
                if (_added != null && _added.Count > 0)
                {
                    foreach (IComponent prev in _added)
                    {
                        container.Content.Remove(prev as Component);
                    }

                    _added.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new PDFParserException("The previousl parsed components could not be removed from the parent container. See the inner exception for more details.", ex);
            }


            string fullpath = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(this.Source))
                {
                    //TODO:Put this at the document level

                    fullpath = this.MapPath(this.Source);
                    if (Uri.IsWellFormedUriString(fullpath, UriKind.Absolute))
                    {
                        using (System.Net.Http.HttpClient wc = new System.Net.Http.HttpClient())
                        {
                            this._contentsAsString = wc.GetStringAsync(fullpath).Result;
                        }
                    }
                    else
                    {
                        this._contentsAsString = System.IO.File.ReadAllText(fullpath);
                        if(this.UnEncode)
                        {
                            this._contentsAsString = System.Web.HttpUtility.HtmlDecode(this._contentsAsString);
                        }
                    }
                }
                else if (null != this.XHTMLContents)
                {
                    fullpath = "HTMLFragment.Contents";
                    _contentsAsString = this.XHTMLContents.OuterXml;
                }
                else if(!string.IsNullOrEmpty(this.RawContents))
                {
                    _contentsAsString = this.RawContents;

                    if (this.UnEncode)
                    {
                        this._contentsAsString = System.Web.HttpUtility.HtmlDecode(this._contentsAsString);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new PDFParserException("Could not download the required html contents from the specified source: " + fullpath, ex);
            }



            if (!string.IsNullOrEmpty(this._contentsAsString))
            {
                if (null == this._added)
                    _added = new List<IComponent>();

                try
                {
                    this.ParseHtmlContents(fullpath, this._contentsAsString, container, index, context);
                }
                catch (Exception ex)
                {
                    throw new PDFParserException("The Html reader could not parse the Html contents from the specified source: " + fullpath, ex);
                }

                if (_added.Count > 0)
                {
                    //Need to do the initialization for each of the items.

                    InitContext initContext = new InitContext(context.Items, context.TraceLog, context.PerformanceMonitor, this.Document, context.Format);
                    for (int i = 0; i < _added.Count; i++)
                    {
                        _added[i].Init(initContext);
                    }

                    //If the load even has already happened then we need to execute the load event
                    //for each of the relevant items that were added.

                    if (performload)
                    {
                        LoadContext loadContext = new LoadContext(context.Items, context.TraceLog, context.PerformanceMonitor, this.Document, context.Format);
                        for (int i = 0; i < _added.Count; i++)
                        {
                            IComponent comp = _added[i];
                            if (comp is VisualComponent)
                                (comp as VisualComponent).Load(loadContext);
                        }
                    }
                }
            }


            _parsed = true;
        }

        protected IContainerComponent GetContainerParent()
        {
            Component ele = this;
            while (null != ele)
            {
                Component par = ele.Parent;
                if (par == null)
                    throw new ArgumentNullException("This HTML Fragment does not have a parent component that is a container to add the conetents to");

                else if ((par is IContainerComponent) == false)
                    ele = par;
                else
                    return par as IContainerComponent;
            }

            //If we get this far then we haven't got a viable container to add our items to.
            throw new ArgumentNullException("This HTML Fragment does not have a parent component that is a container to add the conetents to");
        }

        protected virtual void ParseHtmlContents(string source, string html, IContainerComponent container, int insertIndex, ContextBase context)
        {
            HTMLParserSettings settings = GetParserSettings(context);

            if(this.Format == HtmlFormatType.Markdown)
            {
                Markdown md = new Markdown();
                html = md.Transform(html);
                
            }
            HTMLParser parser = new HTMLParser(html, settings);

            Stack<IComponent> route = new Stack<IComponent>();

            IComponentList contents = container.Content;
            //int codeDepth = 0;
            foreach (Scryber.Html.Parsing.HTMLParserResult result in parser)
            {
                if (result.Valid && null != result.Parsed)
                {
                    IComponent parsed = result.Parsed;

                    if (result.IsEnd)
                        route.Pop();
                    else
                    {
                        if (route.Count == 0)
                        {
                            _added.Add(parsed);
                            contents.Insert(insertIndex, parsed);
                            insertIndex++;
                            if (parsed is ILoadableComponent)
                            {
                                ((ILoadableComponent)parsed).LoadedSource = source;
                            }
                        }
                        else
                        {
                            IContainerComponent parent = (IContainerComponent)route.Peek();
                            ((IComponentList)parent.Content).Add(parsed);
                        }
                        route.Push(result.Parsed);
                    }
                }
            }
        }

        internal virtual HTMLParserSettings GetParserSettings(ContextBase context)
        {
            HTMLParserSettings settings = new HTMLParserSettings(context);
            return settings;
        }

    }
}
