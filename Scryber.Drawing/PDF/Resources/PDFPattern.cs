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
using Scryber.PDF.Native;
using Scryber.Drawing;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// Collective base class for all tiling and shading patterns
    /// </summary>
    public abstract class PDFPattern : PDFResource
    {

        #region public IPDFComponent OwningComponent {get; set}

        /// <summary>
        /// Gets or sets the component that owns (displays) this pattern
        /// </summary>
        public IComponent OwningComponent
        {
            get;
            set;
        }

        #endregion

        #region public PatternType PatternType {get;}

        /// <summary>
        /// Gets the type of pattern this instance represents (Tiling or Shading)
        /// </summary>
        public PatternType PatternType
        { 
            get; 
            private set;
        }

        #endregion

        #region public override string ResourceKey {get;}

        private string _key;

        /// <summary>
        /// Gets the unique resource key for this pattern
        /// </summary>
        public override string ResourceKey
        {
            get 
            {
                return _key;
            }
        }

        #endregion

        #region public override string ResourceType

        /// <summary>
        /// Gets the type of resource - Always PatternResourceType
        /// </summary>
        public override string ResourceType
        {
            get { return PDFResource.PatternResourceType; }
        }

        #endregion

        #region protected PDFPattern(IPDFComponent owner, PatternType type, string fullkey)

        /// <summary>
        /// Protected constructor for a new pattern with the owner, type and full resource key.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="type"></param>
        /// <param name="fullkey"></param>
        protected PDFPattern(IComponent owner, PatternType type, string fullkey)
            : base(ObjectTypes.Pattern)
        {
            if (null == owner)
                throw new ArgumentNullException("owner");

            this.OwningComponent = owner;
            this.PatternType = type;
            this._key = fullkey;
            this.Container = owner as IResourceContainer;
        }

        #endregion
    }


    /// <summary>
    /// Base abstract tiling pattern class
    /// </summary>
    public abstract class PDFTilingPattern : PDFPattern
    {
        #region public PatternPaintType PaintType {get;set;}

        /// <summary>
        /// Gets or sets the paint type for this pattern
        /// </summary>
        public PatternPaintType PaintType
        {
            get;
            set;
        }

        #endregion

        #region public PatternTilingType TilingType {get;set;}

        /// <summary>
        /// Gets or sets the Tiling type for this pattern
        /// </summary>
        public PatternTilingType TilingType
        {
            get;
            set;
        }

        #endregion

        #region public PDFPoint Start {get;set;}

        /// <summary>
        /// Gets or sets the offset from the bottom left corner of the
        /// page (0,0) that the pattern repeats (top left of the pattern).
        /// </summary>
        public Point Start
        {
            get;
            set;
        }

        #endregion

        #region public PDFSize Step {get;set;}

        /// <summary>
        /// Gets or sets the distance between the start of each tile
        /// </summary>
        public Size Step
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Protected 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="fullkey"></param>
        protected PDFTilingPattern(IComponent container, string fullkey)
            : base(container, PatternType.TilingPattern, fullkey)
        {
            this.TilingType = PatternTilingType.NoDistortion;
            this.PaintType = PatternPaintType.ColoredTile;
        }

    }

}
