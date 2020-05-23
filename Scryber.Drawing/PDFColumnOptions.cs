using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;

namespace Scryber
{
    public class PDFColumnOptions
    {
        /// <summary>
        /// Gets or sets the width of the alley between the columns
        /// </summary>
        public PDFUnit AlleyWidth { get; set; }

        /// <summary>
        /// Gets or sets the individual width of the columns
        /// </summary>
        public PDFColumnWidths ColumnWidths { get; set; }

        /// <summary>
        /// Flag to identify if the columns should flow from one to the next
        /// </summary>
        public bool AutoFlow { get; set; }

        /// <summary>
        /// Gets or sets the number of columns in the block
        /// </summary>
        public int ColumnCount { get; set; }

        public PDFColumnOptions()
        {
            this.AutoFlow = true;
        }

        
    }
}
