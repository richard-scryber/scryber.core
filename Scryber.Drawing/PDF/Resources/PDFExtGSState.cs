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
using Scryber.PDF.Native;


namespace Scryber.PDF.Resources
{
    public class PDFExtGSState : PDFResource
    {
        // State operators
        public static readonly PDFName ColorFillOpactity = (PDFName)"ca";
        public static readonly PDFName AlphaIsShape = (PDFName)"AIS";
        public static readonly PDFName ColorStrokeOpacity = (PDFName)"CA";

        public override string ResourceType
        {
            get { return PDFResource.GSStateResourceType; }
        }

        public override string ResourceKey
        {
            get { return ResourceType; }
        }

        private GSStateDictionary _states = new GSStateDictionary();

        public GSStateDictionary States
        {
            get { return _states; }
        }

        public PDFExtGSState()
            : base(ObjectTypes.ExtGState)
        {
            
        }
    
        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            PDFObjectRef oref = writer.BeginObject();
            writer.BeginDictionaryS();
            writer.WriteDictionaryNameEntry("Type", "ExtGState");
            foreach (PDFName name in this.States.Keys)
            {
                writer.BeginDictionaryEntry(name);
                this.States[name].WriteData(writer);
                writer.EndDictionaryEntry();
            }
            writer.EndDictionary();
            writer.EndObject();
            return oref;
        }
    }

    public class GSStateDictionary : PDFDictionary
    {
    }
}
