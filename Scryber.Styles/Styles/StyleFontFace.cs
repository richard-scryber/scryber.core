using System;
using System.Drawing;
using System.Net.Sockets;
using Scryber.Drawing;
using Scryber.PDF.Resources;

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

        //
        // override methods
        //

        public override void MergeInto(Style style, IComponent Component, ComponentState state)
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
                    string name = PDFFont.GetFullName(this.FontFamily.FamilyName, this.FontWeight, this.FontStyle);
                    //PDFFontResource resource = PDFFontResource.Load(definition, name);

                    doc.EnsureResource(PDFFontResource.FontDefnResourceType, name, definition);
                }
                else
                    context.TraceLog.Add(TraceLevel.Warning, "CSS", "The font for " + this.Source.ToString() + " with name " + this.FontFamily + " could not be loaded. It may be an unsupported font file.");
            }
            else
                context.TraceLog.Add(TraceLevel.Warning, "CSS", "No font-family or src was specified for the @font-face rule.");
        }

        public override string ToString()
        {
            return "@font-face";
        }

        

        
        private bool TryGetFont(IDocument doc, PDFContextBase context, out PDFFontDefinition definition)
        {
            Drawing.FontStyle style = this.FontStyle;
            int weight = this.FontWeight;
            string name = this.FontFamily.FamilyName;

            PDFFontFactory.TryEnsureFont(doc, context, this.Source, name, style, weight, out definition);
            if (null != definition)
            {
                return true;
            }
            else
                return false;
        }
        

    }
}
