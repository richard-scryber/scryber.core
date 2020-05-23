using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Scryber.Drawing;

namespace Scryber.Styles
{
    [PDFParsableComponent("Size")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFSizeStyle : PDFStyleItemBase
    {

        public PDFSizeStyle()
            : base(PDFStyleKeys.SizeItemKey)
        {
        }


        #region public PDFUnit Width {get;set;} + RemoveWidth()

        [PDFAttribute("width")]
        [PDFDesignable("Width", Category = "Size", Priority = 1, Type = "PDFUnit")]
        public PDFUnit Width
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.SizeWidthKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;

            }
            set
            {
                this.SetValue(PDFStyleKeys.SizeWidthKey, value);
            }
        }

        public void RemoveWidth()
        {
            this.RemoveValue(PDFStyleKeys.SizeWidthKey);
        }

        #endregion

        #region public PDFUnit Height {get;set;} + RemoveHeight()

        [PDFAttribute("height")]
        [PDFDesignable("Height", Category = "Size", Priority = 1, Type = "PDFUnit")]
        public PDFUnit Height
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.SizeHeightKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;

            }
            set
            {
                this.SetValue(PDFStyleKeys.SizeHeightKey, value);
            }
        }

        public void RemoveHeight()
        {
            this.RemoveValue(PDFStyleKeys.SizeHeightKey);
        }

        #endregion


        #region public PDFUnit MinimumWidth {get;set;} + RemoveMinimumWidth()

        [PDFAttribute("min-width")]
        public PDFUnit MinimumWidth
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.SizeMinimumWidthKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;

            }
            set
            {
                this.SetValue(PDFStyleKeys.SizeMinimumWidthKey, value);
            }
        }

        public void RemoveMinimumWidth()
        {
            this.RemoveValue(PDFStyleKeys.SizeMinimumWidthKey);
        }

        #endregion

        #region public PDFUnit MinimumHeight {get;set;} + RemoveMinimumHeight()

        [PDFAttribute("min-height")]
        public PDFUnit MinimumHeight
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.SizeMinimumHeightKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;

            }
            set
            {
                this.SetValue(PDFStyleKeys.SizeMinimumHeightKey, value);
            }
        }

        public void RemoveMinimumHeight()
        {
            this.RemoveValue(PDFStyleKeys.SizeMinimumHeightKey);
        }

        #endregion

        #region public PDFUnit MaximumWidth {get;set;} + RemoveMaximumWidth()

        [PDFAttribute("max-width")]
        public PDFUnit MaximumWidth
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.SizeMaximumWidthKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;

            }
            set
            {
                this.SetValue(PDFStyleKeys.SizeMaximumWidthKey, value);
            }
        }

        public void RemoveMaximumWidth()
        {
            this.RemoveValue(PDFStyleKeys.SizeMaximumWidthKey);
        }

        #endregion

        #region public PDFUnit MaximumHeight {get;set;} + RemoveMaximumHeight()

        [PDFAttribute("max-height")]
        public PDFUnit MaximumHeight
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.SizeMaximumHeightKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;

            }
            set
            {
                this.SetValue(PDFStyleKeys.SizeMaximumHeightKey, value);
            }
        }

        public void RemoveMaximumHeight()
        {
            this.RemoveValue(PDFStyleKeys.SizeMaximumHeightKey);
        }

        #endregion

        #region public bool FullWidth {get;set;}

        /// <summary>
        /// Gets or sets the full width flag. If true then the component will attempt to stretch across the available width
        /// </summary>
        [PDFAttribute("full-width")]
        [PDFDesignable("Full Width", Category = "General", Priority = 1, Type = "Boolean")]
        public bool FullWidth
        {
            get
            {
                bool b;
                if (this.TryGetValue(PDFStyleKeys.SizeFullWidthKey, out b))
                    return b;
                else
                    return false;
            }
            set
            {
                this.SetValue(PDFStyleKeys.SizeFullWidthKey, value);
            }
        }

        public void RemoveFillWidth()
        {
            this.RemoveValue(PDFStyleKeys.SizeFullWidthKey);
        }

        #endregion
    }
}
