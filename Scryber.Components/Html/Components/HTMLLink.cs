using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("link")]
    public class HTMLLink : Scryber.Components.Component, IPDFTemplate
    {
        internal enum HTMLLinkType
        {
            Html,
            CSS,
            Other
        }

        //
        // Inner Classes
        //

        #region internal abstract class LinkContentBase

        /// <summary>
        /// Base Content for the parsed contents of a link
        /// </summary>
        internal abstract class LinkContentBase
        {
            public string LoadedSource { get; set; }

            public HTMLLinkType LinkType { get; private set; }

            public LinkContentBase(HTMLLinkType type, string source)
            {
                this.LinkType = type;
                this.LoadedSource = source;
            }

            public virtual void ClearContent(Component component)
            {

            }

            public virtual void AddContent(Component component, PDFContextBase context)
            {

            }

            public virtual IEnumerable<IPDFComponent> GetContent(IPDFComponent owner, PDFContextBase context)
            {
                return null;
            }

            public virtual void DataBind(PDFDataContext context)
            {

            }
        }

        #endregion

        #region internal class LinkContentHtml : LinkContentBase

        /// <summary>
        /// A Link content that is HTML
        /// </summary>
        internal class LinkContentHtml : LinkContentBase
        {
            
            private IPDFTemplate _gen = null;
            private int _index;
            
            public LinkContentHtml(IPDFTemplate gen, string path) : base(HTMLLinkType.Html, path)
            {
                this._gen = gen;
                this._index = 0;
            }

            public override IEnumerable<IPDFComponent> GetContent(IPDFComponent owner, PDFContextBase context)
            {
                if (null != this._gen)
                    return this._gen.Instantiate(_index, owner);
                else
                    return null;
            }

            public override void DataBind(PDFDataContext context)
            {
                
            }
        }

        #endregion

        #region internal class LinkContentCSS : LinkContentBase

        internal class LinkContentCSS : LinkContentBase
        {
            private StyleGroup _parsedGroup = null;
            private StyleCollection _parsed;

            public LinkContentCSS(StyleCollection styles, string path) : base(HTMLLinkType.CSS, path)
            {
                _parsed = styles;
            }

            public override void ClearContent(Component component)
            {
                var doc = component.Document;

                if (this._parsedGroup != null)
                    doc.Styles.Remove(this._parsedGroup);
                this._parsedGroup = null;
                this.LoadedSource = string.Empty;
            }

            public override void AddContent(Component component, PDFContextBase context)
            {
                var doc = component.Document;

                if(null == _parsedGroup)
                {
                    _parsedGroup = new StyleGroup();
                    foreach (var style in this._parsed)
                    {
                        _parsedGroup.Styles.Add(style);
                    }

                    doc.Styles.Add(_parsedGroup);
                }
            }

            public override void DataBind(PDFDataContext context)
            {
                this._parsed.DataBind(context);
            }
        }

        #endregion

        private string _href;
        private string _relationship;
        private LinkContentBase _content;
        private PDFRemoteFileRequest _request;

        internal LinkContentBase InnerContent
        {
            get { return _content; }
        }

        [PDFAttribute("href")]
        public string Href
        {
            get { return _href; }
            set
            {
                this._href = value;
                this.ClearInnerContent();
                //this.DoLoadReference();
            }
        }


        [PDFAttribute("rel")]
        public string Relationship
        {
            get { return this._relationship; }
            set
            {
                this._relationship = value;
                this.ClearInnerContent();
            }
        }

        

        [PDFAttribute("media")]
        public Scryber.Styles.Selectors.MediaMatcher Media
        {
            get;
            set;
        }

        /// <summary>
        /// Global Html hidden attribute used with xhtml as hidden='hidden'
        /// </summary>
        [PDFAttribute("hidden")]
        public string Hidden
        {
            get
            {
                if (this.Visible)
                    return string.Empty;
                else
                    return "hidden";
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value != "hidden")
                    this.Visible = true;
                else
                    this.Visible = false;
            }
        }

        public bool IsContentLoaded
        {
            get {
                if (null == this._content)
                    return false;
                else
                    return true;
            }
        }

        public bool IsContentLoading
        {
            get
            {
                if (null == this._request)
                    return false;
                else
                    return true;
            }
        }

        public HTMLLink()
            : base((PDFObjectType)"htmL")
        {
            
        }

        protected override void OnLoaded(PDFLoadContext context)
        {
            base.OnLoaded(context);

            DoLoadReference(context);
        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);

            DoLoadReference(context);

            if (this.IsContentLoaded )
                this.InnerContent.DataBind(context);
        }

        protected override void OnDataBound(PDFDataContext context)
        {
            base.OnDataBound(context);
            
        }

        protected override void OnPreLayout(PDFLayoutContext context)
        {
            base.OnPreLayout(context);

            DoLoadReference(context);

            if (this.IsContentLoaded)
                this.InnerContent.AddContent(this.Document, context);
        }

        protected void ClearInnerContent()
        {
            if (null != this._content)
            {
                this._content.ClearContent(this.Document);
                this._content = null;
            }
        }

        #region protected virtual void DoLoadReference(PDFContextBase context)
        
        protected virtual void DoLoadReference(PDFContextBase context)
        {
            if (String.IsNullOrEmpty(this.Href))
            {
                if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                    context.TraceLog.Add(TraceLevel.Verbose, "HTML", "No href value on the html link tag " + this.UniqueID);
                return;
            }

            if (null == this.Document)
                return;

            if (this.IsContentLoaded)
                return;

            if (this.IsContentLoading)
                return;

            HTMLLinkType type;

            if (this.ShouldAddContent(context.OutputFormat, context, out type) == false)
            {
                if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                    context.TraceLog.Add(TraceLevel.Verbose, "HTML", "Link " + this.UniqueID + " is not a stylesheet or include, or print reference (@rel), so ignoring");
                return;
            }

            bool isFile;
            string content = string.Empty;

            var path = this.MapPath(this.Href, out isFile);

            if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                context.TraceLog.Add(TraceLevel.Verbose, "HTML", "href for link " + this.UniqueID + " mapped to path '" + path + "'");

            //Using the new remote reference loader

            this._request = this.Document.RegisterRemoteFileRequest(path, (caller, args, stream) =>
            {
                if (args.Owner != this)
                    return false;
                else
                {
                    using (var measure = context.PerformanceMonitor.Record(PerformanceMonitorType.Parse_Files, args.FilePath))
                    {
                        if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                            context.TraceLog.Add(TraceLevel.Message, "HTML", "Initiating the load of remote href file " + path + " for link " + this.UniqueID);

                        var str = DoLoadReferenceResult(stream, args.FilePath, context);
                        this.ParseLoadedContent(type, str, args.FilePath, context);

                        if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                            context.TraceLog.Add(TraceLevel.Message, "HTML", "Completed the load of remote href file " + path + " for link " + this.UniqueID);

                        else if (context.TraceLog.ShouldLog(TraceLevel.Message))
                            context.TraceLog.Add(TraceLevel.Message, "HTML", "Loaded remote href file " + path + " for link " + this.UniqueID);

                        this._request = null;
                    }

                    return this.IsContentLoaded;
                }
            }, this);

            return;

            if (!isFile && Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {

                using (var measure = context.PerformanceMonitor.Record(PerformanceMonitorType.Parse_Files, path))
                {
                    if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                        context.TraceLog.Add(TraceLevel.Message, "HTML", "Initiating the load of remote href file " + path + " for link " + this.UniqueID);

                    content = DoLoadRemoteReference(path, context);

                    if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                        context.TraceLog.Add(TraceLevel.Message, "HTML", "Completed the load of remote href file " + path + " for link " + this.UniqueID);

                    else if (context.TraceLog.ShouldLog(TraceLevel.Message))
                        context.TraceLog.Add(TraceLevel.Message, "HTML", "Loaded remote href file " + path + " for link " + this.UniqueID);
                }
            }
            else if (isFile && System.IO.File.Exists(path))
            {
                using (var measure = context.PerformanceMonitor.Record(PerformanceMonitorType.Parse_Files, path))
                {
                    if (context.TraceLog.ShouldLog(TraceLevel.Message))
                        context.TraceLog.Add(TraceLevel.Message, "HTML", "Initiating the load of local href file " + path + " for link " + this.UniqueID);


                    content = System.IO.File.ReadAllText(path);

                    if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                        context.TraceLog.Add(TraceLevel.Message, "HTML", "Completed the load of local file " + path + " for link " + this.UniqueID);

                    else if (context.TraceLog.ShouldLog(TraceLevel.Message))
                        context.TraceLog.Add(TraceLevel.Message, "HTML", "Loaded local file " + path + " for link " + this.UniqueID);
                }
            }
            else if (context.Conformance == ParserConformanceMode.Strict)
                throw new System.IO.FileLoadException("The link with href " + this.Href + " could not be loaded from path '" + path + "'");
            else
            {
                context.TraceLog.Add(TraceLevel.Error, "HTML", "The link with href " + this.Href + " could not be loaded from path '" + path + "'");
            }

            ParseLoadedContent(type, content, path, context);
        }

        private void ParseLoadedContent(HTMLLinkType type, string content, string path, PDFContextBase context)
        {
            switch (type)
            {
                case (HTMLLinkType.CSS):
                    StyleCollection col = this.CreateInnerStyles(content, context);
                    this._content = new LinkContentCSS(col, path);
                    break;
                case (HTMLLinkType.Html):

                    Dictionary<string, string> ns = new Dictionary<string, string>();
                    foreach (var map in this.Document.NamespaceDeclarations)
                    {
                        ns.Add(map.Prefix, map.NamespaceURI);
                    }
                    Scryber.Data.ParsableTemplateGenerator gen = new Data.ParsableTemplateGenerator(content, ns);
                    this._content = new LinkContentHtml(gen, path);
                    break;

                default:
                    if (context.Conformance == ParserConformanceMode.Strict)
                        throw new System.IO.FileLoadException("The link with href " + this.Href + " could not be loaded from path '" + path + "' as it does not have a known rel type - stylesheet or include");
                    else
                        context.TraceLog.Add(TraceLevel.Error, "HTML", "The link with href " + this.Href + " could not be loaded from path '" + path + "'  as it does not have a known rel type - stylesheet or include");

                    break;
            }
        }

        protected virtual string DoLoadRemoteReference(string path, PDFContextBase context)
        {
            //TODO: Use the document for any client web requests.
            //context.Document.LoadRemoteResource(path, context, new RemoteResourceRequest(DoLoadReferenceResult));
            string content;
            HttpClient client = null;
            bool dispose = false;

            try
            {
                this.LoadedSource = path;
                client = this.GetServiceClient();
                if (null == client)
                {
                    client = new HttpClient();
                    dispose = true;
                }
                lock (context.Document)
                {
                    var task = client.GetStreamAsync(path);
                    var awaiter = task.GetAwaiter();


                    using (var response = task.Result)
                        content = this.DoLoadReferenceResult(response, path, context);
                }

            }
            catch (Exception ex)
            {
                content = string.Empty;
                if (context.Conformance == ParserConformanceMode.Lax)
                {
                    context.TraceLog.Add(TraceLevel.Error, "HTML", "Could not load link href the response from '" + path + "'", ex);
                }
                else
                    throw;
            }
            finally
            {
                if (null != client && dispose)
                    client.Dispose();
            }
            return content;
            
        }

        private HttpClient GetServiceClient()
        {
            var client = Scryber.ServiceProvider.GetService<HttpClient>();
            return client;

        }

        /// <summary>
        /// Forces the completion and loading of the remote result.
        /// </summary>
        private string DoLoadReferenceResult(System.IO.Stream stream, string path, PDFContextBase context)
        {

            string content = string.Empty;

            try
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    content = reader.ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                content = string.Empty;
                if (context.Conformance == ParserConformanceMode.Lax)
                {
                    context.TraceLog.Add(TraceLevel.Error, "HTML", "Could not load link href the response from '" + path + "'", ex);
                }
                else
                    throw;
            }
            return content;
        }

        #endregion


        protected StyleCollection CreateInnerStyles(string content, PDFContextBase context)
        {
            var collection = new StyleCollection();

            if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                context.TraceLog.Add(TraceLevel.Verbose, "HTML", "Parsing the css selectors from string for link " + this.UniqueID);

            this.ParseCssStyles(collection, content, context);
            
            return collection;
        }

        
        private bool ShouldAddContent(OutputFormat format, PDFContextBase context, out HTMLLinkType type)
        {
            type = HTMLLinkType.Other;

            //If we have a media value and it's not for this format, then we don't add them
            if (null != Media && this.Media.IsMatchedTo(format) == false)
                return false;

            if (this.Visible == false)
                return false;
            if(string.IsNullOrEmpty(this.Relationship))
            {
                if (context.Conformance == ParserConformanceMode.Lax)
                {
                    context.TraceLog.Add(TraceLevel.Error, "HTML", "The 'rel'ationship attribute is required on a html 'link' tag.");
                    return false;
                }
                else
                    throw new PDFParserException("The 'rel'ationship attribute is required on a html 'link' tag.");
            }    

            if (this.Relationship.Equals("stylesheet", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(this.Relationship))
            {
                type = HTMLLinkType.CSS;
                return true;
            }
            else if (this.Relationship.Equals("import"))
            {
                type = HTMLLinkType.Html;
                return true;
            }
            else
                return false;
        }

        protected virtual void ParseCssStyles(StyleCollection collection, string content, PDFContextBase context)
        {
            bool parseCss = true;

            if(!string.IsNullOrEmpty(this.Relationship))
            {
                if (this.Relationship.Equals("stylesheet", StringComparison.OrdinalIgnoreCase) == false)
                    parseCss = false;
            }

            

            if (parseCss)
            {
                var parser = new Scryber.Styles.Parsing.CSSStyleParser(content, context);
                foreach (var style in parser)
                {
                    if (null != style)
                        collection.Add(style);
                }
            }
        }

        public IEnumerable<IPDFComponent> Instantiate(int index, IPDFComponent owner)
        {
            if (this.IsContentLoaded)
            {
                var comp = this.InnerContent.GetContent(owner as Component, null);
                return comp;
            }
            else
                return null;
        }
    }
}
