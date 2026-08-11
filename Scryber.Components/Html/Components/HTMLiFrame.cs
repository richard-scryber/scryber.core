using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("iframe")]
    public class HTMLiFrame : Div
    {
        private IRemoteRequest _executingRequest;
        private string _policy;

        [PDFAttribute("class")]
        public override string StyleClass
        {
            get => base.StyleClass;
            set => base.StyleClass = value;
        }

        [PDFAttribute("style")]
        public override Style Style
        {
            get => base.Style;
            set => base.Style = value;
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

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get { return base.Contents; }
        }

        private string _src;
        
        [PDFAttribute("src")]
        public string Source
        {
            get { return this._src; }
            set
            {
                this.ClearParsedContent();
                this._src = value;
            }
        }

        [PDFAttribute("allow")]
        public string AllowPolicy
        {
            get{ return this._policy; }
            set
            {
                this._policy = value;
                this.ApplyFramePolicy(value);
            }
        }

        [PDFAttribute("data-passthrough")]
        public bool DataPassthrough
        {
            get;
            set;
        }
        
        [PDFAttribute("data-style-passthrough")]
        public bool StylePassthrough
        {
            get;
            set;
        }

        [PDFAttribute("data-allow-styles")]
        public bool AllowInnerStyles
        {
            get; 
            set;
        }
        
        
        protected Component RootComponent { get; set; }

        public HTMLiFrame() : this(HTMLObjectTypes.IFrame)
        {
        }

        protected HTMLiFrame(ObjectType type): base(type)
        {
            this.DataPassthrough = false;
            this.AllowInnerStyles = false;
            this.StylePassthrough = false;
        }
        
        
        private InitContext _initContext;
        private LoadContext _loadContext;
        private DataContext _dataContext;

        protected override void DoInit(InitContext context)
        {
            _initContext = context;
            this.EnsureContentLoaded(_initContext, _loadContext, _dataContext);
            base.DoInit(context);
        }

        protected override void DoLoad(LoadContext context)
        {
            this._loadContext = context;
            this.EnsureContentLoaded(_initContext, _loadContext, _dataContext);
            base.DoLoad(context);
        }

        protected override void DoDataBind(DataContext context, bool includeChildren)
        {
            this._dataContext = context;
            this.EnsureContentLoaded(_initContext, _loadContext, _dataContext);
            base.DoDataBind(context, includeChildren);
        }

        protected virtual void ClearParsedContent()
        {
            this.Contents.Clear();
            _executingRequest = null;
        }

        private static string DefaultPolicy = "";
        
        public void ApplyFramePolicy(string policies)
        {
            if (string.IsNullOrEmpty(policies))
                policies = HTMLiFrame.DefaultPolicy;
            this._policy = policies;
            
            var all = policies.Split(';', StringSplitOptions.RemoveEmptyEntries);
            
            bool dataPassthrough = false;
            bool allowStyles = false;
            bool stylePassthrough =  false;
            
            for (var i = 0; i < all.Length; i++)
            {
                var policy = all[i].ToLower().Trim();
                
                switch (policy)
                {
                    case("data-passthrough"):
                        dataPassthrough = true;
                        break;
                    case("inner-styles"):
                        allowStyles = true;
                        break;
                    case("style-passthrough"):
                        stylePassthrough = true;
                        break;
                    default:
                        break;
                }
            }
            
            if(dataPassthrough)
                this.DataPassthrough = true;
            if (allowStyles)
                this.AllowInnerStyles = true;
            if (stylePassthrough)
                this.StylePassthrough = true;
            
        }

        protected virtual void DoApplyStylePolicy(bool apply)
        {
            var all = this.FindMatches("style");
            if (all != null && all.Count > 0)
            {
                foreach (var comp in all)
                {
                    if (comp is HTMLStyle style)
                    {
                        style.Visible = apply;
                    }
                }
            }
        }
        
        protected virtual void CleanStyles()
        {
            this.DoApplyStylePolicy(false);
        }

        private void EnsureContentLoaded(InitContext init, LoadContext load, DataContext data)
        {
            if (null == this._executingRequest)
            {
                if (!string.IsNullOrEmpty(this.Source))
                {
                    var type = this.DataContentType;
                    if (null == type)
                        type = this.Document.GetDefaultContentMimeType();
                    
                    var srcPath = this.MapPath(this.Source);
                    var parser = this.Document.EnsureParser(type);
                    var duration = TimeSpan.Zero;
                    
                    this._executingRequest = this.Document.RegisterRemoteFileRequest(type.ToString(), srcPath, duration,
                        (IComponent comp, IRemoteRequest request, System.IO.Stream stream) =>
                        {
                            try
                            {
                                var parsed = parser.Parse(srcPath, stream, ParseSourceType.Template);
                                var all = this.EnsureBodyContent(parsed);
                                if (all != null && all.Count > 0)
                                {
                                    this.Contents.AddRange(all);
                                    this.CleanStyles();
                                    request.CompleteRequest(all, true, null);
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            catch (Exception ex)
                            {
                                request.CompleteRequest(null, false, ex);
                                return false;
                            }
                        });
                }
            }
        }
        
        
        
        

        protected virtual ICollection<Component> EnsureBodyContent(IComponent parsed)
        {
            this.RootComponent = (Component)parsed;
            
            var all = new List<Component>();
            if (parsed is HTMLFragmentWrapper fragment)
            {
                var contents = fragment.Content;
                
                if (contents != null)
                {
                    if (contents.Count == 1 && contents[0] is HTMLDocument doc)
                    {
                        contents = doc.Body.Contents;
                    }
                    
                    all.AddRange(contents);
                    
                }
            }
            else if (parsed is Component comp)
            {
                if (comp is HTMLDocument doc && null != doc.Body)
                {
                    foreach (var inner in doc.Body.Contents)
                    {
                        all.Add(inner);
                    }

                }
                else
                {
                    all.Add(comp);
                }
            }

            return all;
        }


        protected override IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            var engine = base.CreateLayoutEngine(parent, context, style);
            return new LayoutEngineFrameWrapping(this, engine);
        }


        public void ApplyLayoutPolicy(LayoutContext context, ref Style style)
        {
            this.DoApplyStylePolicy(this.AllowInnerStyles);
        }
        
        public void ReleaseLayoutPolicy(LayoutContext context, ref Style style)
        {
            //No matter what we do, we want to disable any inner styles from affecting further content.
            this.DoApplyStylePolicy(false);
        }
    }
}
