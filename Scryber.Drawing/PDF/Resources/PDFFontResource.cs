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
using Scryber.PDF.Native;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// A PDFFontResource is a single resource in a PDFDocument. There should only ever be one font resource for a single font (Family + Style)
    /// </summary>
    public class PDFFontResource : PDFResource, IEquatable<PDFFontResource>
    {
        /// <summary>
        /// An inner class that allows multiple matches to particular font resources.
        /// </summary>
        private class FontResourceMatch
        {
            public string FamilyName;
            public int Weight;
            public bool Italic;

            public FontResourceMatch Next;

            public bool IsMatch(string family, int weight, bool italic)
            {
                if(string.Equals(this.FamilyName, family, StringComparison.OrdinalIgnoreCase))
                {
                    if(this.Weight == weight)
                    {
                        if (this.Italic == italic)
                            return true;
                    }
                }

                if (null != Next)
                    return Next.IsMatch(family, weight, italic);
                else
                    return false;
            }

            public bool AddMatch(string family, int weight, bool italic)
            {
                if (null != Next)
                    return Next.AddMatch(family, weight, italic);
                else
                {
                    Next = new FontResourceMatch() { FamilyName = family, Weight = weight, Italic = italic };
                    return true;
                }
            }
        }

        private string _fontName;
        private FontResourceMatch _matches;
        private PDFFontDefinition _defn;
        private PDFFontWidths _widths;
        private string _rsrcKey = null;

        //
        // public properties
        //

        #region public string FontName {get;}

        /// <summary>
        /// Gets the full name of this font - family + style
        /// </summary>
        public string FontName
        {
            get { return _fontName; }
        }

        #endregion

        #region public PDFFontDefinition Definition {get;}

        /// <summary>
        /// Gets the underlying definition of this font resource
        /// </summary>
        public PDFFontDefinition Definition
        {
            get { return _defn; }
        }

        #endregion

        #region public PDFFontWidths Widths {get;}

        /// <summary>
        /// Gets the font widths associated with this resource
        /// </summary>
        public PDFFontWidths Widths
        {
            get { return _widths; }
        }

        #endregion

        #region public FontEncoding Encoding {get;}

        /// <summary>
        /// Gets the encoding of this Font resource
        /// </summary>
        public FontEncoding Encoding
        {
            get { return _defn.Encoding; }
        }

        #endregion

        #region public override string ResourceType {get;}

        /// <summary>
        /// Overrides the base PDFResource property to return the FontDefinitionResourceType
        /// </summary>
        public override string ResourceType
        {
            get { return PDFResource.FontDefnResourceType; }
        }

        #endregion

        #region public override string ResourceKey {get;}

        /// <summary>
        /// Gets the resource key for this font resource. If not set explicitly via SetResourceKey(string), then it will return the definition name
        /// </summary>
        public override string ResourceKey
        {
            get
            {
                if (string.IsNullOrEmpty(_rsrcKey))
                    return this.Definition.FullName;
                else
                    return _rsrcKey;
            }
        }

        #endregion

        
        //
        // .ctor
        //

        #region private PDFFontResource(PDFFontDefinition defn, string resourceName)

        private PDFFontResource(PDFFontDefinition defn, PDFFontWidths widths, string resourceName)
            : base(PDFObjectTypes.FontResource)
        {
            if (null == defn)
                throw new ArgumentNullException("defn");

            _defn = defn;
            _widths = widths;
            _fontName = defn.FullName;
            _matches = new FontResourceMatch() { FamilyName = _defn.Family, Weight = _defn.Weight, Italic = _defn.Italic };

            this.Name = (Native.PDFName)resourceName;
        }

        #endregion

        //
        // public methods
        //

        #region public void SetResourceKey(string key)

        /// <summary>
        /// Sets the resource key to a specific value
        /// </summary>
        /// <param name="key"></param>
        public void SetResourceKey(string key)
        {
            this._rsrcKey = key;
        }

        #endregion

        /// <summary>
        /// Registers a font style that will also match this resource
        /// </summary>
        /// <param name="familyName"></param>
        /// <param name="weight"></param>
        /// <param name="style"></param>
        public void RegisterSubstitution(string familyName, int weight, FontStyle style)
        {
            this._matches.AddMatch(familyName, weight, style == FontStyle.Italic);
        }

        #region public bool Equals(PDFFontResource other) + 2 overloads

        /// <summary>
        /// Returns true if this font resource is the same as the other font resource (names match)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PDFFontResource other)
        {
            if (null == other)
                throw new ArgumentNullException("other");

            return this.Name == other.Name;
        }

        /// <summary>
        /// Returns true if this font resource refers to the same underlying font as the provided PDFFont
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public bool Equals(PDFFont font)
        {
            if (null == font)
                throw new ArgumentNullException("font");

            return this.Definition.FullName == font.FullName;
        }

        /// <summary>
        /// Returns true if the provided resourcetype is the same as this font resources type, and the name is the same as this resources key
        /// </summary>
        /// <param name="resourcetype"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public override bool Equals(string resourcetype, string name)
        {
            return String.Equals(this.ResourceType, resourcetype) && String.Equals(this.ResourceKey, name);
        }

        #endregion

        public bool Equals(string familyName, int fontWeight, FontStyle style)
        {
            return this._matches.IsMatch(familyName, fontWeight, style == FontStyle.Italic);
        }

        

        //
        // rendering
        //

        #region protected override PDFObjectRef DoRenderToPDF(PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// Overrides the base implementation to render the font resource to the writer 
        /// with the current context and returns any indierct object reference.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        /// <remarks>Should only be called once for any document as the underlying resource holds any indirect object reference.</remarks>
        protected override PDFObjectRef DoRenderToPDF(PDFContextBase context, PDFWriter writer)
        {
            return this.Definition.RenderToPDF(this.Name.Value, this.Widths, context, writer);
        }

        #endregion

        //
        // static methods
        //

        #region public static PDFFontResource Load(PDFFontDefinition defn, string resourceName)

        /// <summary>
        /// Loads a PDFFontResource based on the definition and name
        /// </summary>
        /// <param name="defn"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static PDFFontResource Load(PDFFontDefinition defn, string resourceName)
        {
            if (null == defn)
                throw new ArgumentNullException("defn");

            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException("resourceName");

            PDFFontWidths widths = defn.GetWidths();
            PDFFontResource rsrc = new PDFFontResource(defn, widths, resourceName);
            
            return rsrc;
        }

        #endregion


    }
}
