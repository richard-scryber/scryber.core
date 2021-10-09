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

namespace Scryber
{


    /// <summary>
    /// A collection of PageNumberingGroups
    /// </summary>
    public class PageNumbers
    {
        #region ivars

        private int _totalpages = -1;

        private PageNumberGroup _default; //The default style
        private PageNumberGroup _lastGroup; //The previous numbering group that needs to be closed at the next registration.
        private int _lastPageIndex; //The index of the last page that was registered or unregistered
        private List<PageNumberRegistration> _registrations; // All the page number style registrations
        private Stack<PageNumberGroup> _route; //A stack of the current numbering styles
        
        private Dictionary<string, PageNumberGroup> _namedGroups;
        #endregion

        
        //
        // properties
        //

        #region public int TotalPages {get;}

        /// <summary>
        /// Gets the total number of registered pages in the collection
        /// </summary>
        public int TotalPages
        {
            get { return _totalpages; }
        }

        #endregion

        #region public List<PageNumberRegistration> Registrations

        /// <summary>
        /// Gets all the registractions in this collection
        /// </summary>
        public List<PageNumberRegistration> Registrations
        {
            get { return _registrations; }
        }

        #endregion

        #region public PageNumberGroup Default

        /// <summary>
        /// Gets the default (root) registration and therfore default numbering group
        /// </summary>
        public PageNumberGroup DefaultGroup
        {
            get { return _default; }
        }

        #endregion

        #region public PageNumberGroup CurrentGroup

        /// <summary>
        /// Gets the current number grouping for pages
        /// </summary>
        public PageNumberGroup CurrentGroup
        {
            get
            {
                if (this._route.Count > 0)
                    return this._route.Peek();
                else
                    return this._default;
            }
        }

        #endregion


        //
        // ctor(s)
        //

        #region public PageNumbers()

        /// <summary>
        /// Default empty constructor
        /// </summary>
        public PageNumbers()
        {
            _default = GetDefaultGrouping();
            
        }

        #endregion



        //
        // public methods
        //

        public PageNumberGroup PushPageNumber(PageNumberOptions opts)
        {
            if (null != opts && opts.HasPageNumbering)
            {
                PageNumberGroup grp;

                if (string.IsNullOrEmpty(opts.NumberGroup) || !_namedGroups.TryGetValue(opts.NumberGroup, out grp))
                {
                    if (this._route.Count == 0)
                        GetGroupingFromOptions(this._default, opts, out grp);
                    
                    else
                        GetGroupingFromOptions(this._route.Peek(), opts, out grp);

                }

                //check if the pushed style is different from the current group or we don't have anything on the stack.
                if (null != grp)
                {
                    if (this._route.Count > 0 && _route.Peek().IsCounting)
                        this._lastGroup = this._route.Peek();

                    this._route.Push(grp);

                    if (!string.IsNullOrEmpty(grp.GroupName))
                        this._namedGroups.Add(grp.GroupName, grp);

                    return grp;
                }
            }

            return null;
        }


        public void PopNumberStyle(PageNumberGroup grp)
        {
            if (null != grp)
            {
                if (this._route.Pop().Equals(grp) == false)
                    throw new InvalidOperationException("Unbalanced stack");
                grp.EndCounting();
               
            }
        }

        /// <summary>
        /// Sets the current page number grouping based on the provided style. If this is different from the current style
        /// then the new grouping is returned. This should then be passed to Unregister when finished with.
        /// </summary>
        /// <param name="pgStyle"></param>
        /// <returns></returns>
        public void Register(int pageIndex)
        {
            this._totalpages = Math.Max(this._totalpages, pageIndex);

            //If we have an unclosed last group - after beginning a new group, then we need to close it.
            if(this._lastGroup != null && this._lastGroup.IsCounting)
            {
                this._lastGroup.IncludePage(pageIndex - 1);
                this._lastGroup.EndCounting();
                this._lastGroup = null;
            }

            if (this._route.Count == 0)
                this._route.Push(GetDefaultGrouping());

            PageNumberGroup grp;
            if (this._route.Count > 0)
            {
                grp = this._route.Peek();
            }
            else
                grp = this._default;
            
            if (!grp.IsCounting)
                grp.BeginCounting(pageIndex);

            grp.IncludePage(pageIndex);
            this._lastPageIndex = pageIndex;
        }


        public void UnRegister(int pageIndex)
        {
            this._totalpages = Math.Max(this._totalpages, pageIndex);
            if (this._route.Count == 0)
                throw new ArgumentNullException("No Current group on the stack to unregister. Make sure Register is always called first");
            
            PageNumberGroup grp = this._route.Peek();

            //If this is an unregister following a nested unregister then we won't be counting
            //But we know the previous page index, so we start from the one after
            if (!grp.IsCounting)
                grp.BeginCounting(this._lastPageIndex + 1);
            

            this.CurrentGroup.IncludePage(pageIndex);
            this._lastPageIndex = pageIndex;
        }


        public PageNumberData GetPageData(int pageindex)
        {
            PageNumberRegistration reg = null;
            for (int i = 0; i < this._registrations.Count; i++)
            {
                reg = this._registrations[i];
                if (reg.IsClosed == false || reg.LastPageIndex >= pageindex)
                    break;
            }
            if (null == reg)
                return GetPageDataWithGroup(pageindex, this.CurrentGroup);
            else
                return GetPageDataWithRegistration(pageindex, reg); ;
        }


        public void StartNumbering(PageNumberOptions opts)
        {

            this._registrations = new List<PageNumberRegistration>();
            this._route = new Stack<PageNumberGroup>();
            this._namedGroups = new Dictionary<string, PageNumberGroup>();

            PageNumberGroup grp;
            if (null != opts && opts.HasPageNumbering && GetGroupingFromOptions(this._default, opts, out grp))
            {
                this._route.Push(grp);
            }

        }

        public void EndNumbering()
        {
            
            while (this._route.Count > 0)
            {
                PageNumberGroup grp = this._route.Pop();
                if(grp.IsCounting)
                    grp.EndCounting();
            }
            
        }

        public string GetPageLabel(int pageindex)
        {
            foreach (PageNumberRegistration reg in this._registrations)
            {
                
                if (reg.FirstPageIndex <= pageindex && (reg.IsClosed == false || reg.LastPageIndex >= pageindex))
                    return reg.GetPageLabel(pageindex);
            }
            return _default.GetPageLabel(pageindex);
        }

        //
        // private implementation
        //

        private PageNumberData GetPageDataWithGroup(int pageIndex, PageNumberGroup grp)
        {
            string label = grp.GetPageLabel(pageIndex);
            string lastLabel = grp.GetPageLabel(this.TotalPages);
            int grpNum = pageIndex + grp.NumberStart;
            int lastGrpNum = this.TotalPages + 1;
            int globalPageNum = pageIndex + 1;
            int globalLastPageNum = _totalpages + 1;

            PageNumberData data = new PageNumberData(grp)
            {
                Label = label,
                LastLabel = lastLabel,
                GroupNumber = grpNum,
                GroupLastNumber = lastGrpNum,
                PageNumber = globalPageNum,
                LastPageNumber = globalLastPageNum
            };
            return data;
        }

        private PageNumberData GetPageDataWithRegistration(int pageindex, PageNumberRegistration reg)
        {
            string label = reg.GetPageLabel(pageindex);
            string lastLabel = reg.GetPageLabel(reg.LastPageIndex);

            int grpNum = (pageindex - reg.FirstPageIndex) + 1;
            int lastGrpNum = (reg.LastPageIndex - reg.FirstPageIndex) + 1;
            int globalPageNum = pageindex + 1;
            int globalLastPageNum =  _totalpages + 1;

            PageNumberData data = new PageNumberData(reg.Group)
            {
                Label = label,
                LastLabel = lastLabel,
                GroupNumber = grpNum,
                GroupLastNumber = lastGrpNum,
                PageNumber = globalPageNum,
                LastPageNumber = globalLastPageNum
            };
            return data;
        }
        

        #region private PDFPageNumberGroup GetDefaultGrouping()

        /// <summary>
        /// Creates a default grouping
        /// </summary>
        /// <returns></returns>
        private PageNumberGroup GetDefaultGrouping()
        {
            PageNumberGroup group = new PageNumberGroup(this, string.Empty, PageNumberStyle.Decimals, 1);
            return group;
        }

        #endregion

        #region private static PDFPageNumberingGroup GetGroupingFromOptions(PDFPageNumberGroup template, PDFPageNumberOptions opts, out PDFPageNumberGroup updated)

        /// <summary>
        /// Extracts out the relevant information from the style and builds a group
        /// or returns the DefaultGrouping if there is no relevant information
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private static bool GetGroupingFromOptions(PageNumberGroup template, PageNumberOptions opts, out PageNumberGroup updated)
        {
            if (null == opts)
                throw new ArgumentNullException("opts");
            if (null == template)
                throw new ArgumentNullException("template");

            bool modified = false;
            PageNumberStyle numStyle = template.NumberStyle;
            int numStart = template.NumberStart;
            string grpName = template.GroupName;
            

            if (!string.IsNullOrEmpty(opts.NumberGroup))
            {
                if (opts.NumberGroup == template.GroupName)
                    throw new InvalidOperationException("The new style and the template have the same name - they should use the same options");
                grpName = opts.NumberGroup;
                modified = true;
            }

            
            if (opts.NumberStyle.HasValue)// && style.NumberStyle != numStyle)
            {
                modified = true;
                numStyle = opts.NumberStyle.Value;
            }

            if (opts.StartIndex.HasValue)// && style.NumberStartIndex != numStart)
            {
                modified = true;
                numStart = opts.StartIndex.Value;
            }

            if (modified)
            {
                updated = new PageNumberGroup(template.Owner, grpName, numStyle, numStart);
                return modified;
            }
            else
            {
                updated = null;
                return false;
            }
            
        }

        #endregion


    }

}
