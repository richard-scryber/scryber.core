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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Scryber.Drawing;


namespace Scryber
{
    /// <summary>
    /// Encapsulates the numbering group options for a specific set of values.
    /// </summary>
    /// <remarks>A Document holds all the registered groups, and these are built with the numbering stack</remarks>
    public class ListNumberingGroup
    {

        #region private int StartIndex {get;set;}

        /// <summary>
        /// Gets or sets the starting index of this group.
        /// </summary>
        private int StartIndex
        {
            get;
            set;
        }

        #endregion

        #region public int NextIndex {get;}

        /// <summary>
        /// Gets the Next index of the group in the document
        /// </summary>
        public int NextIndex
        {
            get;
            private set;
        }

        #endregion

        #region public ListNumberingGroupType Type {get; set;}

        /// <summary>
        /// Gets or sets the list or numbering type
        /// </summary>
        public ListNumberingGroupStyle Type
        {
            get;
            set;
        }

        #endregion

        #region public bool ConcatenateWithParent {get;set;}

        /// <summary>
        /// If true the Number is concatenated (joined in the stack of current groups) to its parent Number
        /// </summary>
        public bool ConcatenateWithParent
        {
            get;
            set;
        }

        #endregion

        #region public string PostFix { get; set; }

        /// <summary>
        /// Gets or sets the characters that should be appended to any string value
        /// </summary>
        public string PostFix { get; set; }

        #endregion

        #region public string PreFix {get;set;}

        /// <summary>
        /// Gets or sets the prefix that should be applied to this numbering
        /// </summary>
        public string Prefix
        {
            get;
            set;
        }

        #endregion

        #region public NumberingOutputType Output { get; set; }

        /// <summary>
        /// Gets or sets the output type of the Numbering group - string or component.
        /// </summary>
        public NumberingOutputType Output { get; set; }

        #endregion

        // ctor(s)

        #region public PDFNumberingGroup(string name)

        /// <summary>
        /// Creates a new instance of the numbering group with the specificed name
        /// </summary>
        /// <param name="name"></param>
        public ListNumberingGroup()
            : this(1, ListNumberingGroupStyle.Decimals, true)
        {
        }

        #endregion

        #region public PDFNumberingGroup(int startIndex, NumberingGroupType type, bool concat)

        /// <summary>
        /// Creates a new instance of the numbering group with the spacified name, start index, numbering type and if values are con
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="type"></param>
        /// <param name="concat"></param>
        public ListNumberingGroup(int startIndex, ListNumberingGroupStyle type, bool concat)
        {
            this.StartIndex = startIndex;
            this.NextIndex = startIndex;
            this.Type = type;
            this.ConcatenateWithParent = concat;
            string post;
            this.Output = GetDefaultOutputTypeForNumbering(type, out post);
            this.PostFix = post;
        }

        

        #endregion

        // instance methods

        #region public virtual int Increment()

        /// <summary>
        /// Increments the current index and returns the index value
        /// </summary>
        public virtual int Increment()
        {
            int i = this.NextIndex;
            this.NextIndex++;
            return i;
        }

        #endregion

        #region public virtual string ToString(int depth) + ToString()

        /// <summary>
        /// Converts this numbering item to a string. 
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public virtual string ToString(int depth)
        {
            int i = this.NextIndex - 1;
            string value = this.ConvertToString(i, depth);

            if (null == value)
                return value;

            if (!string.IsNullOrEmpty(this.Prefix))
                value = this.Prefix + value;
            
            if (!string.IsNullOrEmpty(this.PostFix))
                value = value + this.PostFix;

            return value;
        }

        
        public override string ToString()
        {
            return this.ToString(0);
        }
        
        #endregion

        #region private string ConvertToString(int index, int depth)

        /// <summary>
        /// Based on this groups numbering type converts the index and depth to a string value
        /// </summary>
        /// <param name="index">The current numbering index</param>
        /// <param name="depth">The depth of this group within it's stack</param>
        /// <returns></returns>
        private string ConvertToString(int index, int depth)
        {
            switch (this.Type)
            {
                case ListNumberingGroupStyle.None:
                    return string.Empty;

                case ListNumberingGroupStyle.Decimals:
                    return index.ToString();

                case ListNumberingGroupStyle.UppercaseRoman:
                    return Scryber.Utilities.NumberHelper.GetRomanUpper(index);

                case ListNumberingGroupStyle.LowercaseRoman:
                    return Scryber.Utilities.NumberHelper.GetRomanLower(index);

                case ListNumberingGroupStyle.UppercaseLetters:
                    return Scryber.Utilities.NumberHelper.GetLetterUpper(index);

                case ListNumberingGroupStyle.LowercaseLetters:
                    return Scryber.Utilities.NumberHelper.GetLetterLower(index);

                case ListNumberingGroupStyle.Bullet:
                    return Scryber.Utilities.NumberHelper.GetPointValue(depth);

                //case ListNumberingGroupStyle.Labels:
                //case ListNumberingGroupStyle.Image:
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion

        #region public virtual void Reset()

        /// <summary>
        /// Resets this numbering group to its starting set up.
        /// </summary>
        public virtual void Reset()
        {
            this.NextIndex = this.StartIndex;
        }

        #endregion

        #region private NumberingOutputType GetDefaultOutputTypeForNumbering(NumberingGroupType type, out string postfix)
        /// <summary>
        /// based on the numbering type - returns the expected output type and aslo and postfix for the string types.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        private NumberingOutputType GetDefaultOutputTypeForNumbering(ListNumberingGroupStyle type, out string postfix)
        {
            switch (type)
            {
                case ListNumberingGroupStyle.None:
                case ListNumberingGroupStyle.Decimals:
                case ListNumberingGroupStyle.UppercaseRoman:
                case ListNumberingGroupStyle.LowercaseRoman:
                case ListNumberingGroupStyle.UppercaseLetters:
                case ListNumberingGroupStyle.LowercaseLetters:
                    postfix = ".";
                    return NumberingOutputType.String;

                case ListNumberingGroupStyle.Bullet:
                    postfix = null;
                    return NumberingOutputType.String;

                //case ListNumberingGroupStyle.Labels:
                //    postfix = null;
                //    return NumberingOutputType.Component;

                //case ListNumberingGroupStyle.Image:
                //    postfix = null;
                //    return NumberingOutputType.Component;

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        #endregion
    }


    /// <summary>
    /// SubClass of the PDFNumberingGroup for images (holds the fuly qualified path to the image)
    /// </summary>
    [Obsolete("Images are not currently supported")]
    public class PDFListNumberingImageGroup : ListNumberingGroup
    {
        #region public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the path to the image to be rendered in the list item or numbered component
        /// </summary>
        public string ImagePath
        {
            get;
            set;
        }

        #endregion

        // .ctor(s)

        #region public PDFNumberingImageGroup()

        /// <summary>
        /// Empty constructor - must set image path manually
        /// </summary>
        public PDFListNumberingImageGroup()
            : this(null)
        {
        }

        #endregion

        #region public PDFNumberingImageGroup(string imgpath)

        /// <summary>
        /// Creates a new nmbering image group with ths specified path to the image to use.
        /// Must be fully resolved.
        /// </summary>
        /// <param name="imgpath"></param>
        public PDFListNumberingImageGroup(string imgpath)
            : base(0, ListNumberingGroupStyle.None, false)
        {
            this.Output = NumberingOutputType.Component;
            this.ImagePath = imgpath;
        }

        #endregion

        // inastance methods

        #region public override string ToString()

        public override string ToString()
        {
            return String.Format("ImageGroup: {0}", this.ImagePath);
        }

        #endregion

    }


    /// <summary>
    /// Represents a stack of numbering groups - as a hierarchy of applied numbers in the document
    /// </summary>
    /// <remarks>As the numbering depth increases so the Numbering groups are pushed onto the stack.
    /// The stack keeps a track of the current depth and the individual groups then trak their index</remarks>
    public class PDFListNumberingStack
    {
        #region public string Name {get;set;}

         /// <summary>
        /// Gets or sets the name of this numbering group stack
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        #endregion

        #region public PDFNumberingGroup Current {get;}

        private List<ListNumberingGroup> _items;

        /// <summary>
        /// Gets the current numbering group in this stack
        /// </summary>
        public ListNumberingGroup Current
        {
            get
            {
                int count = this.Count;
                if (count == 0)
                    return null;
                else
                    return this._items[count - 1];
            }
        }

        #endregion

        #region public int Count {get;}

        /// <summary>
        /// Gets the current numbering stack depth
        /// </summary>
        public int Count
        {
            get { return this._items.Count; }
        }

        #endregion

        // ctor

        #region public PDFNumberingGroupStack(string name)

        /// <summary>
        /// Create a new Numbering Group stack with the specified group name
        /// </summary>
        /// <param name="name"></param>
        public PDFListNumberingStack(string name)
        {
            this.Name = name;
            this._items = new List<ListNumberingGroup>();
        }

        #endregion

        // instance methods

        #region public void Push(NumberingGroupType type, string prefix, string postfix, bool concatenate)

        public void Push(ListNumberingGroupStyle type, string prefix, string postfix, bool concatenate)
        {
            ListNumberingGroup group = new ListNumberingGroup(1, type, concatenate);
            group.PostFix = postfix;
            group.Prefix = prefix;
            this.Push(group);
        }

        #endregion

        #region  public void Push(PDFNumberingGroup grp)

        /// <summary>
        /// Pushes the specified group onto the stack
        /// </summary>
        /// <param name="grp"></param>
        public void Push(ListNumberingGroup grp)
        {
            if (null == grp)
                throw new ArgumentNullException("grp");

            this._items.Add(grp);
        }

        #endregion

        #region public PDFNumberingGroup Pop()

        /// <summary>
        /// Pops the current group from the stack and returns it.
        /// </summary>
        /// <returns></returns>
        public ListNumberingGroup Pop()
        {
            if (this._items.Count == 0)
                throw new IndexOutOfRangeException();

            ListNumberingGroup grp = this.Current;
            this._items.RemoveAt(this.Count - 1);

            return grp;
        }

        #endregion

        #region public string Increment()
        /// <summary>
        /// Takes the existing numbering groups in this stack and increments the last one. 
        /// Returns a full string of the last group along with any (contiguously) concatenated values from higher in the stack
        /// </summary>
        /// <returns></returns>
        public string Increment()
        {
            if (this.Count == 0)
                return string.Empty;

            
            //Get the highest group that is concatenated
            int index = this.Count - 1;
            while (index >= 0)
            {
                if (_items[index].ConcatenateWithParent == false)
                    break;
                else if (index == 0)
                    break;
                else
                    index--;
            }

            if (index < 0)
                return string.Empty;

            //Concatenate all the values together
            StringBuilder sb = new StringBuilder();

            for (int depth = index; depth < this.Count; depth++)
            {
                ListNumberingGroup curr = this._items[depth];
                if (depth == this.Count - 1)
                    curr.Increment();

                string value = curr.ToString(depth);
                if (null != value)
                    sb.Append(value);
            }
            return sb.ToString();
        }

        #endregion
    }


    /// <summary>
    /// A collection of PDFNumberingStacks that can be accessed either by name (case sensitive) or index
    /// </summary>
    public class PDFListNumberingStackCollection : System.Collections.ObjectModel.KeyedCollection<string, PDFListNumberingStack>
    {

        #region protected override string GetKeyForItem(PDFNumberingStack item)

        /// <summary>
        /// Returns the statck items name
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(PDFListNumberingStack item)
        {
            return item.Name;
        }

        #endregion

        #region public bool TryGetValue(string name, out PDFNumberingStack item)

        /// <summary>
        /// Attempts to retrieve the stack with the specified name. If found in this collection then returns true (otherwise false)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out PDFListNumberingStack item)
        {
            if (null == this.Dictionary)
            {
                item = null;
                return false;
            }
            else
                return this.Dictionary.TryGetValue(name, out item);
        }

        #endregion
    }
}
