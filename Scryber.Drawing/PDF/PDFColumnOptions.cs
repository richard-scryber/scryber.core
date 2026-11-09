using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;

namespace Scryber.PDF
{
    public class PDFColumnOptions
    {
        /// <summary>
        /// Gets or sets the width of the alley between the columns
        /// </summary>
        public Unit AlleyWidth { get; set; }

        /// <summary>
        /// Gets or sets the individual width of the columns
        /// </summary>
        public ColumnWidths ColumnWidths { get; set; }
        
        /// <summary>
        /// Gets or sets the pen to draw column rules (mid spaced in the alley between the columns).
        /// </summary>
        public Scryber.PDF.Graphics.PDFPen ColumnRule {get; set;}

        /// <summary>
        /// Flag to identify if the columns should flow from one to the next. Default = true
        /// </summary>
        public bool AutoFlow { get; set; }
        
        /// <summary>
        /// Specifies how the columns should be filled (if AutoFlow is true).
        /// </summary>
        public ColumnFillMode FillMode { get; set; }
        
        
        

        /// <summary>
        /// Gets or sets the number of columns in the block
        /// </summary>
        public int ColumnCount { get; set; }

        public PDFColumnOptions()
        {
            this.AutoFlow = true;
            this.FillMode = ColumnFillMode.Auto ;
        }

        
    }
}
