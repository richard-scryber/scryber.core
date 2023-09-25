using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Scryber.Drawing;
using Scryber.OpenType;
using Scryber.PDF.Resources;

namespace Scryber.Styles
{
    [PDFParsableComponent("FontFace")]
    public class StyleFontFace : Style, INamingContainer
    {

        private const string FontLogCategory = "Style Font Face";
        
        #region StyleItemFontFace inner class

        /// <summary>
        /// Just a wrapper class for the font-face properties
        /// </summary>
        private class StyleItemFontFace : StyleItemBase
        {
            public StyleItemFontFace() : base(StyleKeys.FontFaceItemKey)
            { }
        }

        #endregion;


        private StyleItemFontFace _ff;


        public StyleFontFace() : base(ObjectTypes.Style)
        {
            _ff = new StyleItemFontFace();
            this.AddItem(_ff);
        }


        
        //
        // font face properties directly on the style
        //

        #region public string Source {get; set;}

        [PDFAttribute("src")]
        public FontSource Source
        {
            get
            {
                FontSource found;
                if (this._ff.TryGetValue(StyleKeys.FontFaceSrcKey, out found))
                    return found;
                else
                    return null;
            }
            set
            {
                this._ff.SetValue(StyleKeys.FontFaceSrcKey, value);
            }
        }

        public void RemoveSource()
        {
            this._ff.RemoveValue(StyleKeys.FontFaceSrcKey);
        }

        #endregion

        #region public string FontFamily {get; set;}

        [PDFAttribute("font-family")]
        public FontSelector FontFamily
        {
            get
            {
                FontSelector found;
                if (this._ff.TryGetValue(StyleKeys.FontFamilyKey, out found))
                    return found;
                else
                    return null;
            }
            set
            {
                this._ff.SetValue(StyleKeys.FontFamilyKey, value);
            }
        }

        public void RemoveFontFamily()
        {
            this._ff.RemoveValue(StyleKeys.FontFamilyKey);
        }

        #endregion

        #region public string FontWeight {get; set;}

        [PDFAttribute("font-weight")]
        public int FontWeight
        {
            get
            {
                int found;
                if (this._ff.TryGetValue(StyleKeys.FontWeightKey, out found))
                    return found;
                else
                    return FontWeights.Regular;
            }
            set
            {
                this._ff.SetValue(StyleKeys.FontWeightKey, value);
            }
        }

        public void RemoveFontWeight()
        {
            this._ff.RemoveValue(StyleKeys.FontWeightKey);
        }

        #endregion

        #region public string FontStyleItalic {get; set;}

        [PDFAttribute("font-style")]
        public Drawing.FontStyle FontStyle
        {
            get
            {
                Drawing.FontStyle found;
                if (this._ff.TryGetValue(StyleKeys.FontStyleKey, out found))
                    return found;
                else
                    return Drawing.FontStyle.Regular;
            }
            set
            {
                this.SetValue(StyleKeys.FontStyleKey, value);
            }
        }

        
        public void RemoveFontItalic()
        {
            this._ff.RemoveValue(StyleKeys.FontStyleKey);
        }

        #endregion

        #region public IComponent Parent {get;set;}

        private IComponent _owner;

        /// <summary>
        /// Gets or sets the owning component for this style definition - could be a remote style link, or a style group collection
        /// </summary>
        public IComponent Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
            }
        }

        #endregion


        //
        // override methods
        //

        public override string MapPath(string path)
        {
            if (null != this.Owner)
                return this.Owner.MapPath(path);
            else
                throw new Exception("The style defn " + this.ToString() + " does not have an owner");
            //return base.MapPath(path);
        }

        public override void MergeInto(Style style, IComponent Component)
        {
            //Don't merge this one as any styles declared are for the font face not components.
        }


        protected override void DoDataBind(DataContext context, bool includechildren)
        {
            base.DoDataBind(context, includechildren);

            //TODO: Validate if this is a remote request 

            if (context.Document is IResourceRequester doc && null != this.Source && null != this.FontFamily)
            {
                FontDefinition definition;
                string name = Drawing.Font.GetFullName(this.FontFamily.FamilyName, this.FontWeight, this.FontStyle);
                
                if (this.TryGetFont(doc, context, out definition))
                {
                    if (context.ShouldLogVerbose)
                        context.TraceLog.Add(TraceLevel.Verbose, FontLogCategory, "Existing font resource found for " + this.FontFamily.FamilyName + ", " + this.FontStyle + " " + this.FontWeight + ". No remote request required");

                    context.Document.EnsureResource(PDFResource.FontDefnResourceType, name, definition);
                }
                else
                {
                    
                    var source = GetSupportedSource(this.Source);
                    if (null == source)
                    {
                        if (context.Conformance == ParserConformanceMode.Strict)
                            throw new NotSupportedException("The source " + this.Source + " is not supported");
                        else
                        {
                            context.TraceLog.Add(TraceLevel.Error, FontLogCategory, "The source '" + this.Source.ToString() + " is not supported");
                            
                        }
                    }
                    else
                    {
                        this.EnsureSourceMapped(source, context);
                        
                        if (context.ShouldLogMessage)
                            context.TraceLog.Add(TraceLevel.Message, FontLogCategory,
                                "Initiating the remote request for font " + name + " from source " + source.Source);

                        doc.RequestResource(source.Type.ToString(), source.Source, ResolveFontRequest, context.Document,
                            context);
                    }
                }
            }
            else
                context.TraceLog.Add(TraceLevel.Warning, FontLogCategory, "No font-family or src was specified for the @font-face rule.");
        }

        protected void EnsureSourceMapped(FontSource source, ContextBase context)
        {
            if (null != source)
            {
                if (!string.IsNullOrEmpty(source.Source))
                {
                    var mapped = this.MapPath(source.Source);
                    if (context.ShouldLogVerbose)
                        context.TraceLog.Add(TraceLevel.Verbose, "Font Source", "Mapped font source from '" + (source.Source ?? "NULL") + "' to full path '" + (mapped ?? "NULL") + "'");

                    source.SetMappedSource(mapped);
                }
                EnsureSourceMapped(source.Next, context);
            }
            
        }

        private bool ResolveFontRequest(IComponent owner, IRemoteRequest request, Stream response)
        {
            ContextBase context = (ContextBase) request.Arguments;
            
            FontDefinition definition;
            IDocument doc = (IDocument) owner;
            var fullName =
                Scryber.Drawing.Font.GetFullName(this.FontFamily.FamilyName, this.FontWeight, this.FontStyle);

            if (null != response)
            {
                if (context.ShouldLogDebug)
                    context.TraceLog.Begin(TraceLevel.Verbose, FontLogCategory,
                        "Response stream received for the font definition of " + fullName);

                var all = FontFactory.LoadFontDefinitions(this.FontFamily.FamilyName, this.FontWeight, this.FontStyle,
                    request.FilePath, response);

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Verbose, FontLogCategory,
                        " Font factory has loaded" + all.ToArray().Length + " fonts from the stream.");

                //do a check to make sure the font is there
                definition =
                    FontFactory.GetFontDefinition(this.FontFamily.FamilyName, this.FontStyle, this.FontWeight, true);

                request.CompleteRequest(definition, true, null);

                if (null != definition)
                {
                    if (context.ShouldLogDebug)
                        context.TraceLog.End(TraceLevel.Verbose, FontLogCategory,
                            "Completed the loading of font definition " + definition.ToString());

                    else if (context.ShouldLogMessage)
                        context.TraceLog.Add(TraceLevel.Message, FontLogCategory,
                            "Completed the loading of font definition " + definition.ToString() + " from file " +
                            request.FilePath);
                }
            }
            else
            {
                if(context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, FontLogCategory, "Response stream was not set, so assigning the direct result as expected to be a font definition for source " + request.FilePath);
                
                definition = (FontDefinition) request.Result;
            }

            if (null != definition)
            {
                doc.EnsureResource(PDFResource.FontDefnResourceType, fullName, definition);
                
                if(context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, FontLogCategory, "Added the font " + fullName + " to the document resources for " + definition.ToString());
                
                return true;
            }
            else
            {
                context.TraceLog.Add(TraceLevel.Warning, FontLogCategory, "Could not load a font definition for the font " + fullName + " from the source " + request.FilePath);
                return false;
            }


        }

        public override string ToString()
        {
            return "@font-face";
        }

        protected virtual FontSource GetSupportedSource(FontSource source)
        {
            //Prefer the TTF format, but if woff is there, then use it.
            FontSource woff = null;

            while (null != source)
            {
                if (source.Format == FontSourceFormat.WOFF)
                {
                    woff = source;
                    source = source.Next;
                }
                else if (source.Format == FontSourceFormat.TrueType)
                    return source;
                else if (source.Format == FontSourceFormat.OpenType)
                    return source;
                else if (null == source.Next && source.Format == FontSourceFormat.Default)
                {
                    if (source.Source.EndsWith(".ttf")) // url(.....ttf)
                    {
                        return new FontSource(source.Type, source.Source, FontSourceFormat.TrueType);
                    }
                    else if (source.Source.EndsWith(".otf")) // url(....otf)
                    {
                        return new FontSource(source.Type, source.Source, FontSourceFormat.OpenType);
                    }
                    else
                        return woff;
                }
                else
                    source = source.Next;
            }

            return woff;
        }
        
        
        private bool TryGetFont(IResourceRequester requestor, ContextBase context, out FontDefinition definition)
        {
            Drawing.FontStyle style = this.FontStyle;
            int weight = this.FontWeight;
            string name = this.FontFamily.FamilyName;

            definition = FontFactory.GetFontDefinition(name, style, weight, throwNotFound: false);
            if (null != definition)
            {
                return true;
            }
            else
                return false;
        }
        

    }
}
