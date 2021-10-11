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
using System.Xml.Serialization;
using System.Drawing;
using Scryber.Drawing;

namespace Scryber
{
    /// <summary>
    /// Defines the Sizing characteristics of a Page or group of pages
    /// </summary>
    public class PageSize : TypedObject
    {
        #region PaperSize {get;set;}

        private PaperSize _paper;
        /// <summary>
        /// Gets or sets the PaperSize from one of the standard values
        /// </summary>
        /// <value>The new paper size</value>
        public PaperSize PaperSize
        {
            get { return _paper; }
            set 
            {
                _paper = value;
                this.UpdateSizes();
            }
        }

        #endregion

        #region Width{get;set;} and Height {get;set;}

        private Drawing.Size _size;
        /// <summary>
        /// Gets or Sets the width of the Page (setting will change the PaperSize to Custom)
        /// </summary>
        /// <value>The new width</value>
        public Unit Width
        {
            get 
            { 
                return _size.Width;
            }
            set 
            {
                _size.Width = value;
                this.UpdatePage();
            }
        }

        /// <summary>
        /// Gets or sets the height of the page (setting will change the PaperSize to Custom)
        /// </summary>
        /// <value>The new height</value>
        public Unit Height
        {
            get { return _size.Height; }
            set 
            { 
                _size.Height = value;
                this.UpdatePage();
            }
        }

        #endregion

        public Drawing.Size Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
                this.UpdatePage();
            }
        }

        #region PaperOrientation {get;set}

        private PaperOrientation _orientation;

        /// <summary>
        /// Gets or sets the PaperOrientation of the page
        /// </summary>
        /// <value>The new orientation</value>
        public PaperOrientation Orientation
        {
            get { return _orientation; }
            set 
            { 
                _orientation = value;
                this.UpdateSizes();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new PageSize using the default PaperSize and Orientation
        /// </summary>
        public PageSize()
            : this(Const.DefaultPaperSize, Const.DefaultPaperOrientation)
        {
        }

        /// <summary>
        /// Creates a new PageSize using the paper and orientation
        /// </summary>
        /// <param name="paper">The specified paper size</param>
        /// <param name="orientation">The paper orientation</param>
        public PageSize(PaperSize paper, PaperOrientation orientation) 
            : base(ObjectTypes.PageSize)
        {
            this.PaperSize = paper;
            this.Orientation = orientation;
            this.UpdateSizes();
        }
        
        /// <summary>
        /// Creates an new custom page size
        /// </summary>
        /// <param name="size">The width and height of the page</param>
        public PageSize(Drawing.Size size)
            : base(ObjectTypes.PageSize)
        {
            this._size = size;
            this.UpdatePage();
        }


        #endregion

        #region UpdatesSizes(), UpdatePage()
        /// <summary>
        /// Sets the new page size based upon the paper and orientation
        /// </summary>
        private void UpdateSizes()
        {
            if (this.PaperSize != PaperSize.Custom)
            {
                Drawing.Size size = Papers.GetSizeInDeviceIndependentUnits(this.PaperSize);
                if (this.Orientation == PaperOrientation.Landscape)
                {
                    this._size = new Drawing.Size(size.Height, size.Width);
                }
                else
                {
                    this._size = size ;
                }
            }
        }

        /// <summary>
        /// Sets the paper and orientation based upon the size
        /// </summary>
        private void UpdatePage()
        {
            this._paper = Papers.GetPaperFromSize(this._size);
            this._orientation = this.Width > this.Height ? PaperOrientation.Landscape : PaperOrientation.Portrait;
        }

        #endregion

        #region static A4,A5,A3,Letter {get;}

        /// <summary>
        /// Returns a new A4 portrait page size
        /// </summary>
        public static PageSize A4
        {
            get { return new PageSize(Papers.ISO.A4, PaperOrientation.Portrait); }
        }

        /// <summary>
        /// Returns a new A5 portrait page size
        /// </summary>
        public static PageSize A5
        {
            get { return new PageSize(Papers.ISO.A5, PaperOrientation.Portrait); }
        }

        /// <summary>
        /// Returns a new A3 portrait page size
        /// </summary>
        public static PageSize A3
        {
            get { return new PageSize(Papers.ISO.A3, PaperOrientation.Portrait); }
        }

        /// <summary>
        /// Returns a new US Letter portrait page size
        /// </summary>
        public static PageSize Letter
        {
            get { return new PageSize(Papers.US.Letter, PaperOrientation.Portrait); }
        }

        #endregion
    }
}
