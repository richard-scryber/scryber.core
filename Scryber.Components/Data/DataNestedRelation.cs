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
    /// Defines a relation where all child elements are nested within the 
    /// parent item when serialized to xml
    /// </summary>
    [PDFRequiredFramework("0.9")]
    [PDFParsableComponent("Nested")]
    public class DataNestedRelation : DataRelation
    {
        
        public DataNestedRelation()
            : this(PDFObjectTypes.SqlNestedRelationType)
        {
        }

        protected DataNestedRelation(ObjectType type)
            : base(type)
        {
        }


        public override void AddRelation(IPDFDataSetProviderCommand parentCommand, IPDFDataSetProviderCommand childCommand, System.Data.DataSet dataset, PDFDataContext context)
        {
            string childtablename = childCommand.GetDataTableName(dataset);
            if (string.IsNullOrEmpty(childtablename))
                throw new ArgumentNullException("owner.CommandName");

            string parenttablename = parentCommand.GetDataTableName(dataset);

            if (string.IsNullOrEmpty(parenttablename))
                throw new ArgumentNullException("this.Parent");

            int matchcount = this.MatchOn.Count;
            if (matchcount < 1)
                throw new ArgumentOutOfRangeException("this.MatchOn");

            DataTable child = dataset.Tables[childtablename];
            if (null == child)
            {
                context.TraceLog.Add(TraceLevel.Message, SqlProviderCommand.SqlCommandLog, "Relation not created between '" + childtablename + "' and '" + parenttablename + "' as one or more tables are not in data set");
                return;
            }
            DataTable parent = dataset.Tables[parenttablename];
            if (null == parent)
            {
                context.TraceLog.Add(TraceLevel.Message, SqlProviderCommand.SqlCommandLog, "Relation not created between '" + childtablename + "' and '" + parenttablename + "' as one or more tables are not in data set");
                return;
            }

            DataColumn[] childmatches = new DataColumn[matchcount];
            DataColumn[] parentmatches = new DataColumn[matchcount];

            for (int i = 0; i < matchcount; i++)
            {
                DataRelationMatch match = this.MatchOn[i];

                if (string.IsNullOrEmpty(match.ChildName))
                    throw new NullReferenceException(string.Format(Errors.NullChildColumnForRelationToTable, childtablename, parentCommand.ID));

                DataColumn col = child.Columns[match.ChildName];
                if (null == col)
                    throw new NullReferenceException(string.Format(Errors.TableDoesNotContainColumn, match.ChildName, parentCommand.ID, childtablename));

                childmatches[i] = col;

                if (string.IsNullOrEmpty(match.ParentName))
                    throw new NullReferenceException(string.Format(Errors.NullParentColumnForRelationToTable, parenttablename, parentCommand.ID));

                col = parent.Columns[match.ParentName];
                if (null == col)
                    throw new NullReferenceException(string.Format(Errors.TableDoesNotContainColumn, match.ParentName));

                parentmatches[i] = col;
            }

            System.Data.DataRelation rel = new System.Data.DataRelation(childtablename + "_2_" + parenttablename, parentmatches, childmatches);
            rel.Nested = true;
            
            dataset.Relations.Add(rel);
        }
    }

}
