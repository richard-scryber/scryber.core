using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("link")]
    public class HTMLLink : Scryber.Components.Component
    {
        private string _href;
        private string _relationship;
        private StyleGroup _parsedGroup = null;

        [PDFAttribute("href")]
        public string Href
        {
            get { return _href; }
            set
            {
                this._href = value;
                this.ClearInnerStyles();
                //this.DoLoadReference();
            }
        }

        #region protected PDFStyleCollection InnerItems

        private StyleCollection _innerItems;

        protected StyleCollection InnerItems
        {
            get
            {
                return this._innerItems;
            }
            set
            {
                this._innerItems = value;
            }
        }

        #endregion

        #region public PDFStyleCollection Styles

        /// <summary>
        /// Gets all the styles in this group
        /// </summary>
        
        public StyleCollection Styles
        {
            get { return this.InnerItems; }
        }

        #endregion


        [PDFAttribute("rel")]
        public string Relationship
        {
            get { return this._relationship; }
            set
            {
                this._relationship = value;
                this.ClearInnerStyles();
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

        public bool IsSourceLoaded
        {
            get { 
                if (string.IsNullOrEmpty(this.LoadedSource))
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
            if (this.IsSourceLoaded && null != this.InnerItems)
                this.InnerItems.DataBind(context);
        }

        protected override void OnDataBound(PDFDataContext context)
        {
            base.OnDataBound(context);
            
        }

        protected override void OnPreLayout(PDFLayoutContext context)
        {
            base.OnPreLayout(context);

            DoLoadReference(context);
            if (this.IsSourceLoaded)
                this.AddStylesToDocument(context);
        }

        protected void ClearInnerStyles()
        {
            this.InnerItems = null;
            if (this._parsedGroup != null)
                this.Document.Styles.Remove(this._parsedGroup);
            this._parsedGroup = null;
            this.LoadedSource = string.Empty;
        }

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

            if (this.IsSourceLoaded)
                return;

            if (this.ShouldAddStyles(context.OutputFormat) == false)
            {
                if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                    context.TraceLog.Add(TraceLevel.Verbose, "HTML", "Link " + this.UniqueID + " is not a stylesheet print reference (@rel), so ignoring");
                return;
            }

            bool isFile;

            var path = this.MapPath(this.Href, out isFile);

            if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                context.TraceLog.Add(TraceLevel.Verbose, "HTML", "href for link " + this.UniqueID + " mapped to path '" + path + "'");

            if (!isFile && Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {

                if (context.PerformanceMonitor.RecordMeasurements)
                    context.PerformanceMonitor.Begin(PerformanceMonitorType.Font_Load);

                if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                    context.TraceLog.Add(TraceLevel.Message, "HTML", "Initiating the load of remote href file " + path + " for link " + this.UniqueID);

                DoLoadRemoteReference(path, context);

                if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                    context.TraceLog.End(TraceLevel.Message, "HTML", "Completed the load of remote href file " + path + " for link " + this.UniqueID);

                else if (context.TraceLog.ShouldLog(TraceLevel.Message))
                    context.TraceLog.Add(TraceLevel.Message, "HTML", "Loaded remote href file " + path + " for link " + this.UniqueID);
            }
            else if (isFile && System.IO.File.Exists(path))
            {
                if (context.TraceLog.ShouldLog(TraceLevel.Message))
                    context.TraceLog.Begin(TraceLevel.Message, "HTML", "Initiating the load of local href file " + path + " for link " + this.UniqueID);

                this.LoadedSource = path;
                var css = System.IO.File.ReadAllText(path);

                this.InnerItems = this.CreateInnerStyles(css, context);

                if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                    context.TraceLog.End(TraceLevel.Message, "HTML", "Completed the load of local file " + path + " for link " + this.UniqueID);

                else if (context.TraceLog.ShouldLog(TraceLevel.Message))
                    context.TraceLog.Add(TraceLevel.Message, "HTML", "Loaded local file " + path + " for link " + this.UniqueID);
            }
        }

        protected virtual void DoLoadRemoteReference(string path, PDFContextBase context)
        {
            //TODO: Use the document for any client web requests.
            //context.Document.LoadRemoteResource(path, context, new RemoteResourceRequest(DoLoadReferenceResult));
            this.LoadedSource = path;
            var request = System.Net.HttpWebRequest.Create(path);

            var result = request.GetResponse();
            this.DoLoadReferenceResult(result, path, context);
            
        }

        /// <summary>
        /// Forces the completion and loading of the remote result.
        /// </summary>
        private void DoLoadReferenceResult(System.Net.WebResponse response, string path, PDFContextBase context)
        {

            
            try
            {
                
                string css;
                using (var content = response.GetResponseStream())
                {
                    using (var reader = new System.IO.StreamReader(content))
                    {
                        css = reader.ReadToEnd();
                    }
                }
                this._innerItems = this.CreateInnerStyles(css, context);
            }
            catch(Exception ex)
            {
                if (context.Conformance == ParserConformanceMode.Lax)
                {
                    context.TraceLog.Add(TraceLevel.Error, "HTML", "Could not load link href the response from '" + path + "'", ex);
                }
                else
                    throw;
            }
            finally
            {
                response.Dispose();
            }
        }

        protected StyleCollection CreateInnerStyles(string content, PDFContextBase context)
        {
            var collection = new StyleCollection();

            if (context.TraceLog.ShouldLog(TraceLevel.Verbose))
                context.TraceLog.Add(TraceLevel.Verbose, "HTML", "Parsing the css selectors from string for link " + this.UniqueID);

            this.AddCssStyles(collection, content);
            
            return collection;
        }

        protected virtual void AddStylesToDocument(PDFContextBase context)
        {
            if(null != this.Styles && this.ShouldAddStyles(context.OutputFormat))
            {

                StyleGroup grp = new StyleGroup();
                foreach (var style in this.Styles)
                {
                    grp.Styles.Add(style);
                }

                this.Document.Styles.Add(grp);
                this._parsedGroup = grp;

            }
        }

        private bool ShouldAddStyles(OutputFormat format)
        {
            //If we have a media value and it's not for this format, then we don't add them
            if (null != Media && this.Media.IsMatchedTo(format) == false)
                return false;
            //If we have a relationship value and it's not a stylesheet, then we don't add them
            if (!string.IsNullOrEmpty(this.Relationship) && this.Relationship.Equals("stylesheet", StringComparison.OrdinalIgnoreCase) == false)
                return false;

            if (!this.Visible)
                return false;

            
            return true;
        }

        protected virtual void AddCssStyles(StyleCollection collection, string content)
        {
            bool parseCss = true;

            if(!string.IsNullOrEmpty(this.Relationship))
            {
                if (this.Relationship.Equals("stylesheet", StringComparison.OrdinalIgnoreCase) == false)
                    parseCss = false;
            }

            

            if (parseCss)
            {
                var parser = new Scryber.Styles.Parsing.CSSStyleParser(content);
                foreach (var style in parser)
                {
                    if (null != style)
                        collection.Add(style);
                }
            }
        }


    }
}
