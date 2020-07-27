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
    /// Represents the style options for a single group of pages.
    /// If the group has a name it can be restarted at any point in the document
    /// </summary>
    public class PDFPageNumberGroup
    {
        //
        // inner classes to encapsulate the finer details of this group
        //

        #region private class NumberGroupSequence

        /// <summary>
        /// Represents a sequence of contiguous page numbers in this group
        /// </summary>
        private class NumberGroupSequence
        {
            /// <summary>
            /// Gets or sets the first page index of this sequence
            /// </summary>
            public int StartPageIndex { get; set; }

            /// <summary>
            /// Gets or sets the last page index of this sequence
            /// </summary>
            public int EndPageIndex { get; set; }

            /// <summary>
            /// Gets or sets the count of pages that were part of this group before this sequence was started
            /// </summary>
            public int PreviousCount { get; set; }

            /// <summary>
            /// Gets or sets the total number of pages in this sequence (inclusive)
            /// </summary>
            public int SequencePageCount { get { return (EndPageIndex - StartPageIndex) + 1; } }
        }

        #endregion

        #region private class NumberGroupOptions

        /// <summary>
        /// Represents the style options for this group
        /// </summary>
        private class NumberGroupOptions
        {
            public PageNumberStyle Style { get; set; }

            public int StartNumber { get; set; }

        }

        #endregion

        private NumberGroupOptions _options;
        private NumberGroupSequence _currSequence = null;

        //
        // group properties
        //
        
        #region public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the name of the group that this page numbering represents
        /// </summary>
        public string GroupName { get; set; }

        #endregion

        #region public int GroupPageCount { get; protected set; }

        /// <summary>
        /// Gets the total number of pages that this group has been registed with
        /// </summary>
        public int GroupPageCount { get; protected set; }

        #endregion

        #region public PDFPageNumbers Owner { get; set; }

        /// <summary>
        /// Gets the page numbers owner of this group.
        /// </summary>
        public PDFPageNumbers Owner { get; set; }

        #endregion

        #region public bool IsCounting {get; private set;}

        /// <summary>
        /// returns true if this group is currently counting pages
        /// </summary>
        public bool IsCounting
        {
            get { return null != _currSequence; }
        }

        #endregion

        //
        // style option properties
        //

        #region public int NumberStart {get;}

        /// <summary>
        /// Gets the first ONE based number for this group
        /// </summary>
        public int NumberStart
        {
            get { return this._options.StartNumber; }
        }

        #endregion

        

        #region public PageNumberStyle NumberStyle {get;}

        /// <summary>
        /// Gets the display style for this group
        /// </summary>
        public PageNumberStyle NumberStyle
        {
            get { return this._options.Style; }
        }

        #endregion

        //
        // ctor
        //

        public PDFPageNumberGroup(PDFPageNumbers owner, string groupName, PageNumberStyle style, int startindex)
        {
            this.Owner = owner;
            this.GroupName = groupName;
            this.GroupPageCount = 0;
            this._options = new NumberGroupOptions() { Style = style, StartNumber = startindex };
            this._currSequence = null;
        }

        //
        // methods
        //


        #region public override string ToString()

        /// <summary>
        /// Returns a string representation of this Number group
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Number group '{2}': {0}, {1}, Page count: {3}", 
                this.NumberStyle, this.NumberStart, this.GroupName, this.GroupPageCount);
        }

        #endregion

        #region public void BeginCounting(int pgIndex)

        /// <summary>
        /// Starts the counting of pages (from the specified page index)
        /// </summary>
        /// <param name="pgIndex"></param>
        public void BeginCounting(int pgIndex)
        {
            if (null != _currSequence)
                throw new InvalidOperationException("This group is already counting - use the IsCounting property to check first");
            
            _currSequence = new NumberGroupSequence() { StartPageIndex = pgIndex, EndPageIndex = pgIndex, PreviousCount = this.GroupPageCount };

        }

        #endregion

        #region public void IncludePage(int index)

        /// <summary>
        /// Includes the current page index in the sequence
        /// </summary>
        /// <param name="index"></param>
        public void IncludePage(int index)
        {
            if (!this.IsCounting)
                throw new InvalidOperationException("This group is not currently counting - use the isCounting property to check");

            _currSequence.EndPageIndex = Math.Max(_currSequence.EndPageIndex, index);
        }

        #endregion

        #region public void EndCounting()

        /// <summary>
        /// Completes a sequence of pages and adds the registation of page numbers to this 
        /// </summary>
        public void EndCounting()
        {
            if(!this.IsCounting)
                throw new InvalidOperationException("This group is not currently counting - use the isCounting property to check");
            
            NumberGroupSequence seq = this._currSequence;

            PDFPageNumberRegistration reg = new PDFPageNumberRegistration(seq.StartPageIndex, seq.EndPageIndex, this);
            //If we have an existing count to this group then we need to add the numbering offset to the sequence
            reg.PreviousLinkedRegistrationPageCount = this.GroupPageCount;

            this.Owner.Registrations.Add(reg);
            this.GroupPageCount += this._currSequence.SequencePageCount;
            this._currSequence = null;
        }

        #endregion

        #region internal string GetPageLabel(int pageindex)

        /// <summary>
        /// Returns a string label representing the ONE page index in the style of this group
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        internal string GetPageLabel(int pageindex)
        {
            pageindex = pageindex + this._options.StartNumber;
            string value;

            switch (this._options.Style)
            {
                case PageNumberStyle.None:
                    value = String.Empty;
                    break;
                case PageNumberStyle.Decimals:
                    value = pageindex.ToString();
                    break;
                case PageNumberStyle.UppercaseRoman:
                    value = Scryber.Utilities.NumberHelper.GetRomanUpper(pageindex);
                    break;
                case PageNumberStyle.LowercaseRoman:
                    value = Scryber.Utilities.NumberHelper.GetRomanLower(pageindex);
                    break;
                case PageNumberStyle.UppercaseLetters:
                    value = Scryber.Utilities.NumberHelper.GetLetterUpper(pageindex);
                    break;
                case PageNumberStyle.LowercaseLetters:
                    value = Scryber.Utilities.NumberHelper.GetLetterLower(pageindex);
                    break;
                default:
                    throw new IndexOutOfRangeException("group.NumberStyle");
            }
            
            return value;

        }

        #endregion

    }
}
