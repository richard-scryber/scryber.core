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
using System.Linq;
using System.Text;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Represents a bunch of characters on the page
    /// </summary>
    public class PDFTextRunCharacter : PDFTextRun
    {
        #region ivars

        private PDFSize _measuredSize;
        private string _chars;
        private PDFUnit _extra;

        #endregion

        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets the width of this character run
        /// </summary>
        public override PDFUnit Width
        {
            get { return this._measuredSize.Width; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets the height of this character run
        /// </summary>
        public override PDFUnit Height
        {
            get { return this._measuredSize.Height; }
        }

        #endregion

        #region public string Characters {get;set;}

        /// <summary>
        /// Gets or sets the characters in this run
        /// </summary>
        public string Characters
        {
            get { return _chars; }
            set { _chars = value; }
        }

        #endregion

        public PDFUnit ExtraSpace
        {
            get { return _extra; }
            set { _extra = value; }
        }

        public PDFTextRunCharacter(PDFSize size, string characters, PDFLayoutLine line, IComponent owner)
            : base(line, owner)
        {
            this._measuredSize = size;
            this._chars = characters;
        }


        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            context.Graphics.FillText(this.Characters);
            return null;
        }

        public override void SetMaxWidth(PDFUnit width)
        {
            base.SetMaxWidth(width);
            this._measuredSize.Width = width;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Characters))
                return "[Empty]";
            if (this.Characters.Length > 50)
                return this.Characters.Substring(0, 20) + "..." + this.Characters.Substring(this.Characters.Length - 20);
            else
                return this.Characters;
        }
    }

    /// <summary>
    /// Represents a bunch or characters on the page with an offset and count in the full string
    /// </summary>
    public class PDFTextRunPartialCharacter : PDFTextRun
    {
         #region ivars

        private PDFSize _measuredSize;
        private int _offset, _count;
        private string _chars;

        #endregion

        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets the width of this character run
        /// </summary>
        public override PDFUnit Width
        {
            get { return this._measuredSize.Width; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets the height of this character run
        /// </summary>
        public override PDFUnit Height
        {
            get { return this._measuredSize.Height; }
        }

        #endregion

        #region public string Characters {get;set;}

        /// <summary>
        /// Gets or sets the full characters this partial is part of
        /// </summary>
        public string Characters
        {
            get { return _chars; }
            set { _chars = value; }
        }

        #endregion

        #region public int StartOffset {get;set;}

        /// <summary>
        /// Gets or sets the starting offset within the characters that this is part of
        /// </summary>
        public int StartOffset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        #endregion

        #region public int CharacterCount {get;set;}

        /// <summary>
        /// Gets or sets the number of characters length in this partial text run.
        /// </summary>
        public int CharacterCount
        {
            get { return _count; }
            set { _count = value; }
        }

        #endregion

        public PDFTextRunPartialCharacter(PDFSize size, string characters, int offset, int count, PDFLayoutLine line, IComponent owner)
            : base(line, owner)
        {
            this._measuredSize = size;
            this._chars = characters;
            this._offset = offset;
            this._count = count;
        }


        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            context.Graphics.FillText(this.Characters, this.StartOffset, this.CharacterCount);
            return null;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Characters))
                return "[Empty]";
            if (this.CharacterCount > 50)
                return this.Characters.Substring(this.StartOffset, 20) + "..." + this.Characters.Substring((this.StartOffset + this.CharacterCount) - 20);
            else
                return this.Characters.Substring(this.StartOffset,this.CharacterCount);
        }
    }
}
