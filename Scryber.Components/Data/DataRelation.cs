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
    /// Base class for all relations
    /// </summary>
    public abstract class DataRelation : TypedObject, IBindableComponent
    {

        //
        // events to support databinding
        //

        #region public event PDFDataBindEventHandler DataBinding;

        public event DataBindEventHandler DataBinding;

        protected virtual void OnDataBinding(DataContext context)
        {
            if (null != this.DataBinding)
                this.DataBinding(this, new DataBindEventArgs(context));
        }

        #endregion

        #region public event PDFDataBindEventHandler DataBound;

        public event DataBindEventHandler DataBound;

        protected virtual void OnDataBound(DataContext context)
        {
            if (null != this.DataBound)
                this.DataBound(this, new DataBindEventArgs(context));
        }

        #endregion



        #region public string ChildCommand {get;set;}

        private string _childcommand;

        /// <summary>
        /// Gets or sets the name of the child command to which this relation points
        /// </summary>
        [PDFAttribute("command")]
        public string ChildCommand
        {
            get { return _childcommand; }
            set { _childcommand = value; }
        }

        #endregion

        #region public PDFDataRelationMatchList MatchOn

        private PDFDataRelationMatchList _matches;

        /// <summary>
        /// Gets the list of relation matches (parent to child) that define this relation
        /// </summary>
        [PDFArray(typeof(DataRelationMatch))]
        [PDFElement("")]
        public PDFDataRelationMatchList MatchOn
        {
            get
            {
                if (null == _matches)
                    _matches = new PDFDataRelationMatchList();
                return _matches;
            }
        }

        #endregion
        

        public DataRelation(ObjectType type)
            : base(type)
        {
        }

        /// <summary>
        /// Creates and adds the relation to the specified dataset.
        /// </summary>
        /// <param name="owner">The command on which this relation is defined</param>
        /// <param name="dataset">The dataset to add the relation to</param>
        public abstract void AddRelation(IDataSetProviderCommand parent, IDataSetProviderCommand child, DataSet dataset, DataContext context);


        #region public void DataBind(PDFDataContext context)

        /// <summary>
        /// Supports the databinding capabilites of the parameter by raising the events
        /// </summary>
        /// <param name="context"></param>
        public virtual void DataBind(DataContext context)
        {
            this.OnDataBinding(context);
            this.DoDataBind(context);
            this.OnDataBound(context);
        }

        #endregion

        protected virtual void DoDataBind(DataContext context)
        {
        }
    }



    public class PDFDataRelationList : List<DataRelation>
    {

        public void DataBind(DataContext context)
        {
            if (this.Count > 0)
            {
                foreach (DataRelation rel in this)
                {
                    rel.DataBind(context);
                }
            }
        }
    }

}
