using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
namespace Scryber.Svg.Components
{
    [PDFParsableComponent("tspan")]
    public class SVGTextSpan : TextLiteral, IPDFStyledComponent
    {

        #region public PDFStyle Style {get;set;} + public bool HasStyle{get;}

        private Style _style;

        /// <summary>
        /// Gets the applied style for this page Component
        /// </summary>
        [PDFAttribute("style")]
        public virtual Style Style
        {
            get
            {
                if (_style == null)
                {
                    _style = new Style();
                    _style.Priority = Style.DirectStylePriority;
                }
                return _style;
            }
            set
            {
                this._style = value;
            }
        }

        /// <summary>
        /// Gets the flag to indicate if this page Component has style 
        /// information associated with it.
        /// </summary>
        public virtual bool HasStyle
        {
            get { return this._style != null && this._style.HasValues; }
        }

        #endregion

        //TODO: Support the x and y components on the class, if really nescessary - feels overkill.
        

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        public SVGTextSpan() : base()
        {

        }

    }
}
