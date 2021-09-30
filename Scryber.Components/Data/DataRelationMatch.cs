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
using System.Data;

namespace Scryber.Data
{
    /// <summary>
    /// Defines a matching mapping between a parent and child column
    /// </summary>
    [PDFRequiredFramework("0.9")]
    [PDFParsableComponent("Match")]
    public class DataRelationMatch : PDFObject
    {

        #region public string ChildName {get;set;}

        private string _childname;
        /// <summary>
        /// Gets or sets the name of the child column in this mapping
        /// </summary>
        [PDFAttribute("child")]
        public string ChildName
        {
            get { return _childname; }
            set { _childname = value; }
        }

        #endregion

        #region public string ParentName {get;set;}

        private string _parentname;

        /// <summary>
        /// Gets or sets the name of the parent column in this mapping
        /// </summary>
        [PDFAttribute("parent")]
        public string ParentName
        {
            get { return _parentname; }
            set { _parentname = value; }
        }

        #endregion

        //
        // ctor
        //

        public DataRelationMatch()
            : this(PDFObjectTypes.SqlRelationMappingType)
        {
        }

        protected DataRelationMatch(ObjectType type)
            : base(type)
        {
        }

    }

    /// <summary>
    /// A list of relation matches
    /// </summary>
    public class PDFDataRelationMatchList : List<DataRelationMatch>
    {
    }
}
