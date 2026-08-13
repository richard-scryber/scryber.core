using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.Sockets;
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
        private static string DefaultPermissionsPolicy = "inline-styles any;inner-images any; inner-navigation any";
        
        private IRemoteRequest _executingRequest;
        private DocumentPermissionsPolicy _policy;
        
        protected IMatchedEnumerable FrameStyles { get; set; }

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
        public DocumentPermissionsPolicy AllowPolicy
        {
            get{ return this._policy; }
            set
            {
                this._policy = value;
            }
        }
        
        
        protected Component RootComponent { get; set; }

        public HTMLiFrame() : this(HTMLObjectTypes.IFrame)
        {
        }

        protected HTMLiFrame(ObjectType type): base(type)
        {
            this.AllowPolicy = DocumentPermissionsPolicy.Parse(DefaultPermissionsPolicy);
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
            this.EnsureContentLoaded(_initContext, _loadContext, _dataContext);
            
            this._dataContext = context;
            var stack = context.DataStack;
            var index = context.CurrentIndex;
            var key = context.CurrentKey;
            var paramValues = new Dictionary<string, object>();
            
            
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.DataPassthrough);
            var allow = policy != null && policy.IsAllowed("self");

            //Cannot pass data through, so we store the existing stack and make a blank one.
            if (!allow)
            {
                if (context.ShouldLogVerbose)
                {
                    context.TraceLog.Add(TraceLevel.Verbose, "iFrame", "Releasing current data stack, so no values are bound on the inner content.");
                }
                
                context.DataStack = new DataStack();
                context.CurrentIndex = -1;
                context.CurrentKey = string.Empty;
                var keys = this.Document.Params.Keys;
                
                foreach (var p in keys)
                {
                    paramValues[p.ToString()] = this.Document.Params[p.ToString()];
                }
                
                this.Document.Params.Clear();
            }

            try
            {
                base.DoDataBind(context, includeChildren);
            }
            finally
            {
                if (!allow)
                {
                    if (context.ShouldLogVerbose)
                    {
                        context.TraceLog.Add(TraceLevel.Verbose, "iFrame", "Restoring current data stack, so execution continues as normal after the iframe.");
                    }
                    
                    context.DataStack = stack;
                    context.CurrentIndex = index;
                    context.CurrentKey = key;
                    
                    foreach (var p in paramValues.Keys)
                    {
                        this.Document.Params[p] = paramValues[p];
                    }
                    
                }
            }

        }

        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.StylePassthrough);
            if (policy.IsAllowed("self"))
                return base.GetAppliedStyle(forComponent, baseStyle);
            else
            {
                //we are not allowed to pass through styles, but if we are allowed inner styles
                //then we need to apply them ourselves from the cached styles that
                //were set when the inner content was parsed.
                
                policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerStyles);
                if (policy.IsAllowed("self"))
                {
                    if (null != this.FrameStyles && this.FrameStyles.Count > 0)
                    {
                        foreach (var item in this.FrameStyles)
                        {
                            if (item is HTMLStyle htmlStyle)
                            {
                                htmlStyle.Styles.MergeInto(baseStyle, forComponent);
                            }
                            else if (item is HTMLLink htmlLink)
                            {
                                var css = htmlLink.LoadedStyles;
                                if (null != css)
                                {
                                    css.MergeInto(baseStyle, forComponent);
                                }
                            }
                        }
                    }
                }

                return baseStyle;
            }
        }

        protected virtual void ClearParsedContent()
        {
            this.Contents.Clear();
            this.FrameStyles = null;
            this.RootComponent = null;
            
            //TODO: Clear any styles from the outer document for good housekeeping
            
            _executingRequest = null;
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
        
        protected virtual int RemoveChildrenFromHierarchy(string matchingSelector, bool includeSelf = false)
        {
            int count = 0;
            var all = this.FindMatches(matchingSelector);
            if (all != null && all.Count > 0)
            {
                foreach (var comp in all)
                {
                    if (comp is Component c)
                    {
                        if(c == this && !includeSelf)
                            continue;
                        
                        if(((IContainerComponent)c.Parent).Content.Remove(c))
                            count++;
                    }
                }
            }
            return count;
        }
        
        protected virtual void EnsureCleanStyles()
        {
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerStyles);
            var allow = policy != null && policy.IsAllowed("self");
            if (!allow)
            {
                this.RemoveChildrenFromHierarchy("style");
            }
            else
            {
                policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.StylePassthrough);
                if (!policy.IsAllowed("self"))
                {
                    this.FrameStyles = this.FindMatches("style, link");
                }
            }
            policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerInlineStyle);
            allow = policy != null && policy.IsAllowed("self");
            if (!allow)
            {
                var all = this.FindMatches("iframe *");
                if (all != null && all.Count > 0)
                {
                    foreach (var comp in all)
                    {
                        if (comp is IStyledComponent style && style.HasStyle)
                        {
                            style.Style.Clear();
                        }
                    }
                }
            }
            this.DoApplyStylePolicy(false);
        }
        
        protected virtual void EnsureCleanLinks()
        {
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerLink);
            var allow = policy != null && policy.IsAllowed("self");
            if (!allow)
            {
                this.RemoveChildrenFromHierarchy("link");
            }
        }
        
        protected virtual void EnsureCleanNavigation()
        {
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerNavigation);
            var allow = policy != null && policy.IsAllowed("self");
            if (!allow)
            {
                var all = this.FindMatches("a");
                if (all != null && all.Count > 0)
                {
                    foreach (var comp in all)
                    {
                        if (comp is HTMLAnchor anchor)
                        {
                            anchor.File = string.Empty;
                        }
                    }
                }
            }
        }
        
        protected virtual void EnsureCleanImages()
        {
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerImages);
            var allow = policy != null && policy.IsAllowed("self");
            if (!allow)
            {
                this.RemoveChildrenFromHierarchy("img");
            }
        }
        
        protected virtual void EnsureCleanFrames()
        {
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerFrames);
            var allow = policy != null && policy.IsAllowed("self");
            if (!allow)
            {
                this.RemoveChildrenFromHierarchy("iframe, embed, object");
            }
        }
        
        protected virtual void EnsureCleanForms()
        {
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerForms);
            var allow = policy != null && policy.IsAllowed("self");
            if (!allow)
            {
                this.RemoveChildrenFromHierarchy("form, input, select, button", false);
            }
        }
        
        

        protected void CleanContents()
        {
            this.EnsureCleanStyles();
            this.EnsureCleanLinks();
            this.EnsureCleanNavigation();
            this.EnsureCleanImages();
            this.EnsureCleanFrames();
            this.EnsureCleanForms();
        }

        protected override void DoBindContentIntoComponent(Data.DataBindingContent data, DataContext context)
        {
            if (null == data)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(data.Content))
                return;

            if (null == data.Type)
                data.Type = this.Document.GetDefaultContentMimeType();

            var parser = this.Document.EnsureParser(data.Type);
            IComponent parsed;
            using (var sr = new System.IO.StringReader(data.Content))
                parsed = parser.Parse(null, sr, ParseSourceType.Template);

            if (parsed == null) return;

            var all = this.EnsureBodyContent(parsed, _initContext);
            if (all == null || all.Count == 0) return;

            if (data.Action == DataContentAction.Replace)
                this.InnerContent.Clear();

            if (data.Action == DataContentAction.PrePend)
            {
                var list = new List<Component>(all);
                for (int i = list.Count - 1; i >= 0; i--)
                    this.InnerContent.Insert(0, list[i]);
            }
            else
            {
                foreach (var comp in all)
                    this.InnerContent.Add(comp);
            }

            this.CleanContents();

            foreach (var comp in all)
                comp.DataBind(context);
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
                                var all = this.EnsureBodyContent(parsed, init);
                                if (all != null && all.Count > 0)
                                {
                                    this.Contents.AddRange(all);
                                    this.CleanContents();
                                    request.CompleteRequest(all, true, null);

                                    //We can check our stage based on the next context,
                                    //and ensure the previous context is completed.
                                   
                                    if (null != load)
                                        this.DoInitChildren(init);

                                    if (null != data)
                                        this.DoLoadChildren(load);
                                    
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
        
        
        
        

        protected virtual ICollection<Component> EnsureBodyContent(IComponent parsed, ContextBase context)
        {
            this.RootComponent = (Component)parsed;
            
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.OuterHtml);
            var allowOuterHtml = policy != null && policy.IsAllowed("self");
            
            var all = new List<Component>();
            if (parsed is HTMLFragmentWrapper fragment)
            {
                var contents = fragment.Content;
                
                if (contents != null)
                {
                    if (contents.Count == 1 && contents[0] is HTMLDocument doc)
                    {
                        if (allowOuterHtml)
                        {
                            var spoof = this.WrapContentsInSpoofDocument(doc, context);
                            contents.Clear();
                            contents.Add(spoof);
                        }
                        else
                        {
                            var article = this.WrapContentsInArticle(doc);
                            contents.Clear();
                            contents.Add(article);
                        }
                    }
                    
                    all.AddRange(contents);
                    
                }
            }
            else if (parsed is Component comp)
            {
                if (comp is HTMLDocument doc && null != doc.Body)
                {
                    if(allowOuterHtml)
                        comp = this.WrapContentsInSpoofDocument(doc, context);
                    else
                        comp = this.WrapContentsInArticle(doc);

                    //add html to the collection
                    all.Add(comp);

                    
                    

                    if (null != doc.Head.Contents && doc.Head.Contents.Count > 0)
                    {
                        //If we have a policy that allows inner styles then include them in the document.
                        
                    }
                }
                else
                {
                    all.Add(comp);
                }
            }

            return all;
        }

        private HTMLArticle WrapContentsInArticle(HTMLDocument doc)
        {
            var article =
                new HTMLArticle(); //No tag, class or id - therefore no pass through style anyway.
            article.Contents.AddRange(doc.Body.Contents);
            if (null != doc.Body.Header)
            {
                var htmlHead = new HTMLComponentHeader();
                article.Contents.Add(htmlHead);
                var template = doc.Body.Header;
                var headerContent = template.Instantiate(0, this);
                                
                foreach (var item in headerContent)
                {
                    if(item is Component comp)
                        htmlHead.Contents.Add(comp);
                }
            }
                            
            if (null != doc.Body.Footer)
            {
                var htmlFooter = new HTMLComponentFooter();
                article.Contents.Add(htmlFooter);
                var footerContent = doc.Body.Footer.Instantiate(0, this);
                                
                foreach (var item in footerContent)
                {
                    if(item is Component comp)
                        htmlFooter.Contents.Add(comp);
                }
            }
            
            if (null != doc.Head.BasePath && !string.IsNullOrEmpty(doc.Head.BasePath.Href))
                article.LoadedSource = doc.Head.BasePath.Href;

            return article;
        }

        private Component WrapContentsInSpoofDocument(HTMLDocument doc, ContextBase context)
        {
            var spoofHtml = new Div();
            spoofHtml.ElementName = doc.ElementName;
            spoofHtml.StyleClass = doc.StyleClass;
                    
            //We make a spoofed body element and add all the content to that.
            var spoofBody = new HTMLArticle();
            spoofBody.Style = doc.Body.Style;
            spoofBody.ElementName = doc.Body.ElementName;
            spoofBody.StyleClass = doc.Body.StyleClass;
            //add the body to html
            spoofHtml.Contents.Add(spoofBody);
                    
            //add contents to the body
            foreach (var inner in doc.Body.Contents)
            {
                spoofBody.Contents.Add(inner);
            }
            
            if (null != doc.Body.Header)
            {
                var htmlHead = new HTMLComponentHeader();
                spoofBody.Contents.Add(htmlHead);
                var template = doc.Body.Header;
                var headerContent = template.Instantiate(0, this);
                                
                foreach (var item in headerContent)
                {
                    if(item is Component comp)
                        htmlHead.Contents.Add(comp);
                }
            }
                            
            if (null != doc.Body.Footer)
            {
                var htmlFooter = new HTMLComponentFooter();
                spoofBody.Contents.Add(htmlFooter);
                
                var footerContent = doc.Body.Footer.Instantiate(0, this);
                                
                foreach (var item in footerContent)
                {
                    if(item is Component comp)
                        htmlFooter.Contents.Add(comp);
                }
            }
            
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerStyles);
            var allowStyle = policy != null && policy.IsAllowed("self");
            
            policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerLink);
            var allowLink = policy != null && policy.IsAllowed("self");
            
            if (allowStyle || allowLink)
            {
                var i = 0;
                //Check if we have styles and insert them.
                foreach (var inner in doc.Head.Contents)
                {
                    if (inner is HTMLStyle style)
                    {
                        if (allowStyle)
                        {
                            spoofHtml.Contents.Insert(i, style);
                            style.Visible = false;
                            i++;
                        }
                        else
                        {
                            ((IContainerComponent)style.Parent).Content.Remove(style);
                        }
                    }
                    else if (inner is HTMLLink link)
                    {
                        if (allowLink)
                        {
                            spoofHtml.Contents.Insert(i, link);
                            link.Visible = false;
                            i++;
                        }
                        else
                        {
                            ((IContainerComponent)link.Parent).Content.Remove(link);
                        }
                    }
                }
            }
            
            //Set the new base path for relative paths.
            if (null != doc.Head.BasePath && !string.IsNullOrEmpty(doc.Head.BasePath.Href))
                spoofHtml.LoadedSource = doc.Head.BasePath.Href;
                    
            return spoofHtml;
        }
        
        


        //
        // layout engine implementation
        //
        
        protected override IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            var engine = base.CreateLayoutEngine(parent, context, style);
            return new LayoutEngineFrameWrapping(this, engine);
        }

        private StyleStack _originalStyleStack = null;
        
        

        public void ApplyLayoutPolicy(LayoutContext context, ref Style style)
        {
            var policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.InnerStyles);
            var allow = policy != null && policy.IsAllowed("self");
            if (context.ShouldLogVerbose)
            {
                context.TraceLog.Add(TraceLevel.Verbose, "iFrame", "Setting the support for inner styles within the iframe to " + (allow ? "enabled" : "disabled"));
            }
            this.DoApplyStylePolicy(allow);
            
            policy = this.AllowPolicy.GetPolicy(PermissionPolicyType.StylePassthrough);
            allow = policy != null && policy.IsAllowed("self");

            if (context.ShouldLogVerbose)
            {
                context.TraceLog.Add(TraceLevel.Verbose, "iFrame", "Setting the support for pass through styles within the iframe to " + (allow ? "enabled" : "disabled"));
            }
            this.DoApplyContextStylePolicy(context, allow);
        }
        
        public void ReleaseLayoutPolicy(LayoutContext context, ref Style style)
        {
            if (context.ShouldLogVerbose)
            {
                context.TraceLog.Add(TraceLevel.Verbose, "iFrame", "Resetting the support for inner styles within the iframe to disabled");
            }
            //No matter what we do, we want to disable any inner styles from affecting further content.
            this.DoApplyStylePolicy(false);
            
            if (context.ShouldLogVerbose)
            {
                context.TraceLog.Add(TraceLevel.Verbose, "iFrame", "Resetting the support for pass through styles outside the iframe to enabled");
            }
            this.DoRestoreContextStylePolicy(context);
        }
        
        /// <summary>
        /// If Context styles are not 'allow'ed to be passed down into the frame contents,
        /// then this will clean the stack, except for the iFrame style. So any further components do not get
        ///  styles applied from definitions outside 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="allow"></param>
        protected virtual void DoApplyContextStylePolicy(LayoutContext context, bool allow)
        {
            _originalStyleStack = context.StyleStack.Clone();
            if (!allow)
            {
                var defaultStyle = new Style();
                this.Document.FillStdStyleValues(defaultStyle);
                context.StyleStack = new StyleStack(defaultStyle);
                var applied = this.GetAppliedStyle();
                context.StyleStack.Push(applied);
            }
        }
        
        protected virtual void DoRestoreContextStylePolicy(LayoutContext context)
        {
            if (null == _originalStyleStack)
                throw new InvalidOperationException(
                    "Can only call restore after completing a successful apply policy on the context style stack");
            
            context.StyleStack = _originalStyleStack;
        }
    }
}
