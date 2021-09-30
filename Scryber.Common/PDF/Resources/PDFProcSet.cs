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
    public class PDFProcSet : PDFResource
    {
        public override string ResourceType
        {
            get { return PDFResource.ProcSetResourceType; }
        }

        public override string ResourceKey
        {
            get { return ResourceType; }
        }

        public PDFProcSet(params string[] names) : base(PDFObjectTypes.ProcSet)
        {
            this.Names = names;
        }

        #region INamedResource Members

        private string[] _names;

        public string[] Names
        {
            get { return _names; }
            set { _names = value; }
        }


        #endregion

        
        protected override PDFObjectRef DoRenderToPDF(PDFContextBase context, PDFWriter writer)
        {
            writer.WriteArrayNameEntries(this.Names);
            return null;
        }
    }
}
