/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Drawing;
using System.ComponentModel;

namespace Scryber.PDF
{
    /// <summary>
    /// A class that represents a full set of positioning data
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFPositionOptions
    {
        /// <summary>
        /// Gets or sets the position mode
        /// </summary>
        public PositionMode PositionMode { get; set; }

        /// <summary>
        /// Gets or set the float mode for this component
        /// </summary>
        public FloatMode FloatMode { get; set; }


        private Visibility _vis;
        /// <summary>
        /// Gets or sets the visibility of this content
        /// </summary>
        public Visibility Visibility
        {
            get { return _vis; }
            set { _vis = value; }
        }

        /// <summary>
        /// Gets or sets the X (horizontal) offset
        /// </summary>
        public Unit? X 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the Y (vertical offset)
        /// </summary>
        public Unit? Y { get; set; }

        /// <summary>
        /// Gets or sets the Right offset (an X value overrides any right value)
        /// </summary>
        public Unit? Right { get; set; }

        /// <summary>
        /// Gets or sets the Bottom offset (a Y value overrides any bottom value)
        /// </summary>
        public Unit? Bottom { get; set; }

        /// <summary>
        /// Gets or sets the Width
        /// </summary>
        public Unit? Width { get; set; }

        /// <summary>
        /// Gets or sets the Height
        /// </summary>
        public Unit? Height { get; set; }

        /// <summary>
        /// Gets or sets the Minimum Width
        /// </summary>
        public Unit? MinimumWidth { get; set; }

        /// <summary>
        /// Gets or sets the Minimum Height
        /// </summary>
        public Unit? MinimumHeight { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Width
        /// </summary>
        public Unit? MaximumWidth { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Height
        /// </summary>
        public Unit? MaximumHeight { get; set; }

        /// <summary>
        /// The form viewport if this has one.
        /// </summary>
        public Rect? ViewPort { get; set; }

        /// <summary>
        /// Gets or sets if these options specifiy that the copmonent these options refer to should fill the available horizontal space
        /// </summary>
        public bool FillWidth { get; set; }

        /// <summary>
        /// Gets or sets the overflow action to be taken when the available space is reached
        /// </summary>
        public OverflowAction OverflowAction { get; set; }

        /// <summary>
        /// Gets or sets the split options for child content
        /// </summary>
        public OverflowSplit OverflowSplit { get; set; }

        public Thickness ClipInset { get; set; }

        /// <summary>
        /// Gets or sets any margins
        /// </summary>
        public Thickness Margins { get; set; }

        /// <summary>
        /// Gets or sets any padding
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment on any child content
        /// </summary>
        public VerticalAlignment? VAlign { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment on any child content
        /// </summary>
        public HorizontalAlignment? HAlign { get; set; }

        /// <summary>
        /// Gets or sets the column count in a block
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Gets or sets the column alley width
        /// </summary>
        public Unit AlleyWidth { get; set; }

        /// <summary>
        /// Returns 
        /// </summary>
        public bool HasAlleyWidth { get { return this.AlleyWidth >= Unit.Zero; } }

        /// <summary>
        /// Gets or sets the widths of the columns
        /// </summary>
        public Unit[] ColumnWidths { get; set; }

        /// <summary>
        /// Gets or sets the direction or standard reading direction
        /// </summary>
        public TextDirection TextDirection
        {
            get;
            set;
        }

        
        /// <summary>
        /// Gets or sets any transformation matrix
        /// </summary>
        public PDFTransformationMatrix TransformMatrix
        {
            get;
            set;
        }

    
        /* Not currently supported
         * 
        
        /// <summary>
        /// Gets or sets the origin for any transformation
        /// </summary>
        public TransformationOrigin TransformationOrigin { get; set; }

        *
        */

        /// <summary>
        /// Returns true if these position options have an explict non-identity transformation matrix
        /// </summary>
        public bool HasTransformation
        {
            get { return null != this.TransformMatrix && !this.TransformMatrix.IsIdentity; }
        }

        /// <summary>
        /// Returns true if these position options have an explicit position mode of Relative, Absolute or Fixed
        /// </summary>
        public bool HasExplicitPositionMode
        {
            get
            {
                if (this.PositionMode == PositionMode.Absolute || this.PositionMode == PositionMode.Fixed ||
                    this.PositionMode == PositionMode.Relative)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Creates a new default position options
        /// </summary>
        public PDFPositionOptions()
        {
            this.PositionMode = Drawing.PositionMode.Block;
            this.OverflowAction = Drawing.OverflowAction.NewPage;
            this.OverflowSplit = Drawing.OverflowSplit.Any;
            this.ColumnCount = 1;
            this.AlleyWidth = -1;
            //this.TransformationOrigin = TransformationOrigin.CenterMiddle;
        }


        public PDFPositionOptions Clone()
        {
            var result = this.MemberwiseClone() as PDFPositionOptions;
            return result;
        }

    }
}
