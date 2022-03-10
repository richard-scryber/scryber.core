﻿using Scryber.Drawing;
using Scryber.Resources;
using SkiaSharp;

namespace Scryber.Styles
{
    [PDFParsableComponent("FontFace")]
    public class StyleFontFace : Style
    {

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


        public StyleFontFace() : base(PDFObjectTypes.Style)
        {
            _ff = new StyleItemFontFace();
            this.AddItem(_ff);
        }


        
        //
        // font face properties directly on the style
        //

        #region public string Source {get; set;}

        [PDFAttribute("src")]
        public PDFFontSource Source
        {
            get
            {
                PDFFontSource found;
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
        public PDFFontSelector FontFamily
        {
            get
            {
                PDFFontSelector found;
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
            this._ff.RemoveValue(StyleKeys.FontFaceFamilyKey);
        }

        #endregion

        #region public string FontWeightBold {get; set;}

        [PDFAttribute("font-weight")]
        public bool FontBold
        {
            get
            {
                bool found;
                if (this._ff.TryGetValue(StyleKeys.FontBoldKey, out found))
                    return found;
                else
                    return false;
            }
            set
            {
                this._ff.SetValue(StyleKeys.FontBoldKey, value);
            }
        }

        public void RemoveFontWeight()
        {
            this._ff.RemoveValue(StyleKeys.FontBoldKey);
        }

        #endregion

        #region public string FontStyleItalic {get; set;}

        [PDFAttribute("font-style")]
        public bool FontItalic
        {
            get
            {
                bool found;
                if (this._ff.TryGetValue(StyleKeys.FontItalicKey, out found))
                    return found;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.FontItalicKey, value);
            }
        }

        
        public void RemoveFontItalic()
        {
            this._ff.RemoveValue(StyleKeys.FontItalicKey);
        }

        #endregion

        //
        // override methods
        //

        public override void MergeInto(Style style, IPDFComponent Component, ComponentState state)
        {
            //Don't merge this one as any styles declared are for the font face not components.
        }


        protected override void DoDataBind(PDFDataContext context, bool includechildren)
        {
            base.DoDataBind(context, includechildren);
            
            var doc = context.Document;

            if (null != doc && null != this.Source && null != this.FontFamily)
            {
                PDFFontDefinition definition;

                if (this.TryGetFont(doc, context, out definition))
                {
                    string name = PDFFont.GetFullName(this.FontFamily.FamilyName, this.FontBold, this.FontItalic);
                    //PDFFontResource resource = PDFFontResource.Load(definition, name);

                    doc.EnsureResource(PDFFontResource.FontDefnResourceType, name, definition);
                }
                else
                    context.TraceLog.Add(TraceLevel.Warning, "CSS", "The font for " + this.Source.ToString() + " with name " + this.FontFamily + " could not be loaded");
            }
            else
                context.TraceLog.Add(TraceLevel.Warning, "CSS", "No font-family or src was specified for the @font-face rule.");
        }

        public override string ToString()
        {
            return "@font-face";
        }

        private bool TryGetFont(IPDFDocument doc, PDFContextBase context, out PDFFontDefinition definition)
        {
            var style = SKFontStyle.Normal;

            if (this.FontBold)
                style = SKFontStyle.Bold;
            if (this.FontItalic)
                style = SKFontStyle.Italic;
            if (this.FontItalic && this.FontBold)
                style = SKFontStyle.BoldItalic;

            string name = this.FontFamily.FamilyName;

            PDFFontFactory.TryEnsureFont(doc, context, this.Source, name, style, out definition);
            
            return null != definition;
        }
    }
}
