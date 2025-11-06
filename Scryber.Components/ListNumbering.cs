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
using Scryber.Styles;

namespace Scryber
{
    public class ListNumbering
    {
        private List<PDFListNumberGroup> _namedGroups;
        private List<PDFListNumberGroup> _currentGroups;

        /// <summary>
        /// Returns true if there is a current numbering group
        /// </summary>
        public bool HasCurrentGroup
        {
            get
            {
                return _currentGroups.Count > 0;
            }
        }

        public PDFListNumberGroup CurrentGroup
        {
            get
            {
                if (!HasCurrentGroup)
                    throw new IndexOutOfRangeException(Errors.NoCurrentNumberGroup);
                return this._currentGroups[this._currentGroups.Count - 1];
            }
        }

        public List<PDFListNumberGroup> NamedGroups
        {
            get { return _namedGroups; }
        }

        public ListNumbering()
        {
            this._namedGroups = new List<PDFListNumberGroup>();
            this._currentGroups = new List<PDFListNumberGroup>();
        }

        /// <summary>
        /// Pushes either an existing known group with the specified name, or a new one (remembering it if there is a name) onto the current Numbering stack
        /// </summary>
        /// <param name="name">The name of the group to retireve or create</param>
        /// <param name="liststyle"></param>
        public void PushGroup(string name, Styles.Style liststyle)
        {
            PDFListNumberGroup grp;

            if (string.IsNullOrEmpty(name) || this.TryGetNamedGroup(name, out grp) == false)
            {
                int startindex = 1;
                ListNumberingGroupStyle style = liststyle.GetValue(StyleKeys.ListNumberStyleKey, ListNumberingGroupStyle.Decimals);
                string prefix = liststyle.GetValue(StyleKeys.ListPrefixKey, String.Empty);
                string postfix = liststyle.GetValue(StyleKeys.ListPostfixKey, String.Empty);
                bool concat = liststyle.GetValue(StyleKeys.ListConcatKey, false);

                grp = new PDFListNumberGroup(name, startindex, style, prefix, postfix, concat);

                if (!string.IsNullOrEmpty(name))
                    _namedGroups.Add(grp);
            }
            this._currentGroups.Add(grp);
        }

        public void PushGroup(string name, ListNumberingGroupStyle style, string prefix, string postfix, bool concat, int startIndex)
        {
            PDFListNumberGroup grp;

            if (string.IsNullOrEmpty(name) || this.TryGetNamedGroup(name, out grp) == false)
            {
                grp = new PDFListNumberGroup(name, startIndex, style, prefix, postfix, concat);
                
                //Add it to the named groups if we have a name
                if (!string.IsNullOrEmpty(name))
                    _namedGroups.Add(grp);
            }
            this._currentGroups.Add(grp);
        }

        public void PopGroup()
        {
            if(!this.HasCurrentGroup)
                throw new IndexOutOfRangeException(Errors.NoCurrentNumberGroup);

            this._currentGroups.RemoveAt(this._currentGroups.Count - 1);
        }

        public PDFListNumberGroup GetGroup(string name)
        {
            foreach (PDFListNumberGroup grp in this.NamedGroups)
            {
                if (grp.Name == name)
                    return grp;
            }
            return null;
        }

        public string Increment()
        {
            if (!this.HasCurrentGroup)
                throw new IndexOutOfRangeException(Errors.NoCurrentNumberGroup);

            //Get the highest group that is concatenated
            int last = this._currentGroups.Count - 1;
            int index = last;

            while (index >= 0)
            {
                if (_currentGroups[index].Concatenate == false)
                    break;
                else if (index == 0)
                    break;
                else
                    index--;
            }
            
            

            if (index < 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int depth = index; depth <= last; depth++)
            {
                PDFListNumberGroup grp = this._currentGroups[depth];
                //If we are the last group then we should increment the value
                if (depth == last)
                    grp.Increment();

                if (!string.IsNullOrEmpty(grp.PreFix))
                    sb.Append(grp.PreFix);

                string value = GetStringValue(grp.CurrentIndex, depth, grp.Style);

                if (!string.IsNullOrEmpty(value))
                    sb.Append(value);

                if (!string.IsNullOrEmpty(grp.PostFix))
                    sb.Append(grp.PostFix);
                
            }
            return sb.ToString();
        }

        private string GetStringValue(int value, int depth, ListNumberingGroupStyle style)
        {
            string s = null;
            switch (style)
            {
                case ListNumberingGroupStyle.None:
                    break;
                case ListNumberingGroupStyle.Decimals:
                    s = value.ToString();
                    break;
                case ListNumberingGroupStyle.UppercaseRoman:
                    s = Scryber.Utilities.NumberHelper.GetRomanUpper(value);
                    break;
                case ListNumberingGroupStyle.LowercaseRoman:
                    s = Scryber.Utilities.NumberHelper.GetRomanLower(value);
                    break;
                case ListNumberingGroupStyle.UppercaseLetters:
                    s = Scryber.Utilities.NumberHelper.GetLetterUpper(value);
                    break;
                case ListNumberingGroupStyle.LowercaseLetters:
                    s = Scryber.Utilities.NumberHelper.GetLetterLower(value);
                    break;
                case ListNumberingGroupStyle.Bullet:
                    s = Scryber.Utilities.NumberHelper.GetPointValue(depth);
                    break;
                case ListNumberingGroupStyle.Labels:
                    s = string.Empty; //label style but does not have any text associated with the item
                    break;
                //case ListNumberingGroupStyle.Image:
                //    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException("style");
            }
            return s;
        }

        private bool TryGetNamedGroup(string name, out PDFListNumberGroup grp)
        {
            foreach (PDFListNumberGroup item in _namedGroups)
            {
                if (string.Equals(item.Name, name))
                {
                    grp = item;
                    return true;
                }
            }
            grp = null;
            return false;
        }
    }


    public class PDFListNumberGroup
    {
        /// <summary>
        /// Gets the name of this group.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the start index of this group
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Gets the next index of the number in this group
        /// </summary>
        public int NextIndex { get; private set; }

        /// <summary>
        /// Gets the current value index of this group
        /// </summary>
        public int CurrentIndex
        {
            get { return NextIndex - 1; }
        }

        /// <summary>
        /// Gets the numbering style of this group
        /// </summary>
        public ListNumberingGroupStyle Style { get; private set; }

        /// <summary>
        /// Gets the postfix string to be applied to this list groups number
        /// </summary>
        public string PostFix { get; private set; }

        /// <summary>
        /// Gets the prefix string to be applied to this groups number
        /// </summary>
        public string PreFix { get; private set; }

        /// <summary>
        /// True if this group should be concatenated with the previous group entry
        /// </summary>
        public bool Concatenate { get; private set; }

        /// <summary>
        /// Creates a new number group with the spacified values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startindex"></param>
        /// <param name="style"></param>
        /// <param name="prefix"></param>
        /// <param name="postfix"></param>
        /// <param name="concat"></param>
        public PDFListNumberGroup(string name, int startindex, ListNumberingGroupStyle style, string prefix, string postfix, bool concat)
        {
            this.Name = name;
            this.StartIndex = startindex;
            this.NextIndex = startindex;
            this.Style = style;
            this.PreFix = prefix;
            this.PostFix = postfix;
            this.Concatenate = concat;
        }

        /// <summary>
        /// Increments to the next number and returns the current value
        /// </summary>
        /// <returns></returns>
        public int Increment()
        {
            int i = this.NextIndex;
            this.NextIndex++;
            return i;
        }

        /// <summary>
        /// Rolls back the enumeration to the start.
        /// </summary>
        public void Reset()
        {
            this.NextIndex = this.StartIndex;
        }

        public NumberingOutputType GetOutputType()
        {
            //if (this.Style == ListNumberingGroupStyle.Image)
            //    return NumberingOutputType.Component;
            //else
                return NumberingOutputType.String;
        }
    }
}
