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


namespace Scryber.PDF.Native
{
    /// <summary>
    /// Encapsulates reference to a PDFIndirectObject
    /// </summary>
    public class PDFObjectRef : IPDFFileObject, IEquatable<PDFObjectRef>
    {
        private int _num, _gen;

        public ObjectType Type { get { return ObjectTypes.ObjectRef; } }


        /// <summary>
        /// Gets this references objects Number
        /// </summary>
        public int Number
        {
            get { return (null == this.Reference) ? _num : this.Reference.Number; }
        }

        /// <summary>
        /// Gets this references objects Generation
        /// </summary>
        public int Generation
        {
            get { return (null == this.Reference)? _gen : this.Reference.Generation; }
        }

        private IPDFIndirectObject _ref;

        /// <summary>
        /// Gets or sets the acutal object this reference refers to
        /// </summary>
        public IPDFIndirectObject Reference
        {
            get { return _ref; }
            set { this._ref = value; }
        }

        protected internal PDFObjectRef()
            : this(null)
        {
        }

        public PDFObjectRef(IPDFIndirectObject reference)
        {
            this._ref = reference;
        }

        public PDFObjectRef(int num, int gen)
            : this(null)
        {
            this._num = num;
            this._gen = gen;
        }

        public void WriteData(PDFWriter writer)
        {
            writer.WriteObjectRefS(this);
        }

        public override string ToString()
        {
            if (null == this.Reference)
                return this.Number.ToString() + " " + this.Generation.ToString() + " R";
            else
                return this.Reference.ToString();
        }

        public static PDFObjectRef Parse(string n)
        {
            int end;
            return Parse(n, 0, out end);

            
        }

        public static PDFObjectRef Parse(string value, int offset, out int end)
        {
            return PDFParserHelper.ParseObjectRef(value, offset, out end);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((PDFObjectRef)obj);
        }

        public bool Equals(PDFObjectRef other)
        {
            return this.Number == other.Number && this.Generation == other.Generation;
        }

        public override int GetHashCode()
        {
            return (this.Generation << 16) + this.Number;
        }

        //public static bool operator ==(PDFObjectRef one, PDFObjectRef two)
        //{
        //    if(null == one && null == two)
        //        return true;
        //    else if(null == one || null == two)
        //        return false;
        //    else
        //        return one.Equals(two);
        //}

        //public static bool operator !=(PDFObjectRef one, PDFObjectRef two)
        //{
        //    if (null == one && null == two)
        //        return false;
        //    else if (null == one || null == two)
        //        return true;
        //    else
        //        return !one.Equals(two);
        //}
    }
}
