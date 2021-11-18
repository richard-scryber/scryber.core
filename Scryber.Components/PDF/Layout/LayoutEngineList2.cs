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
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    public class LayoutEngineList2 : LayoutEnginePanel
    {
        //
        // const
        //

        private const string ListEngineLogCategory = "List Layout Engine";
        private static readonly Unit DefaultListItemAlley = 10;
        public static readonly Unit DefaultNumberWidth = Const.DefaultListNumberInset;
        public const HorizontalAlignment DefaultListItemAlignment = HorizontalAlignment.Right;
        
        /// <summary>
        /// Gets the list this engin is laying out
        /// </summary>
        protected ListBase List { get; }
        
        
        protected string GroupName { get; set; }


        //
        // .ctor
        //

        #region public LayoutEngineList(PDFList list, IPDFLayoutEngine parent)

        public LayoutEngineList2(ListBase list, IPDFLayoutEngine parent)
            : base(list, parent)
        {
            List = list;
        }

        #endregion

        //
        // main override
        //

        #region protected override void DoLayoutComponent()

        /// <summary>
        /// Performs the actual layout of the list and items in it.
        /// </summary>
        protected override void DoLayoutComponent()
        {
            if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Begin(TraceLevel.Verbose, ListEngineLogCategory,
                    $"Starting the layout of the list {this.List.ID}");

            this.OpenListNumbering();

            base.DoLayoutComponent();
            
            this.CloseListNumbering();
            
            if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.End(TraceLevel.Verbose, ListEngineLogCategory,
                    $"Completed the layout of the list {this.List.ID}");

        }

        #endregion

        //
        // open and close the list numbering
        //

        #region private void OpenListNumbering()

        private void OpenListNumbering()
        {
            string groupname = string.Empty;
            StyleValue<string> grp;

            if (this.FullStyle.TryGetValue(StyleKeys.ListGroupKey,out grp))
            {
                groupname = grp.Value(this.FullStyle);
            }
            ListNumbering numbering = this.Component.Document.ListNumbering;

            numbering.PushGroup(groupname, this.FullStyle);
            this.GroupName = groupname;

        }

        #endregion

        #region private void CloseListNumbering()

        /// <summary>
        /// Closes the current document list numbering
        /// </summary>
        private void CloseListNumbering()
        {
            if(this.Component.Document.ListNumbering.HasCurrentGroup)
                this.Component.Document.ListNumbering.PopGroup();
            this.GroupName = string.Empty;
        }

        #endregion
        
        //
        // inner classes
        //


    }
}
