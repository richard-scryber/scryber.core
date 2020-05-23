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
using System.Drawing;
using System.ComponentModel;

namespace Scryber.Styles
{
    /// <summary>
    /// Defines a single style that has the capabilities to be applied to multiple page Components
    /// based upon their ID, Type, or class
    /// </summary>
    [PDFParsableComponent("Style")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFStyleDefn : PDFStyle, IPDFNamingContainer
    {

        #region public Type AppliedType {get; set;}

        private Type _type;

        /// <summary>
        /// Gets or sets the type of components this definition is applied to.
        /// </summary>
        [PDFAttribute("applied-type")]
        public Type AppliedType
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion

        #region public string AppliedClass {get; set;}

        private string _class;

        /// <summary>
        /// Gets or sets the style-class that this definition is applied to.
        /// </summary>
        [PDFAttribute("applied-class")]
        public string AppliedClass
        {
            get { return _class; }
            set { _class = value; }
        }

        #endregion

        #region public string AppliedID {get;set;}

        private string _id;
    
        /// <summary>
        /// Gets or sets the id of the components this definition is applied to.
        /// </summary>
        [PDFAttribute("applied-id")]
        public string AppliedID
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion

        #region public ComponentState AppliedState {get;set;}

        private ComponentState _state;

        /// <summary>
        /// Not currently supported
        /// </summary>
        [PDFAttribute("applied-state")]
        public ComponentState AppliedState
        {
            get { return _state; }
            set { _state = value; }
        }

        #endregion

        //
        // ctors
        //

        #region public PDFStyleDefn()

        public PDFStyleDefn()
            : this(PDFObjectTypes.Style)
        {
        }

        #endregion

        #region public PDFStyleDefn(Type appliedtype, string appliedid, string appliedclassname)

        public PDFStyleDefn(Type appliedtype, string appliedid, string appliedclassname)
            : this()
        {
            this._class = appliedclassname;
            this._id = appliedid;
            this._type = appliedtype;
        }

        #endregion 

        #region protected PDFStyleDefn(PDFObjectType type)

        protected PDFStyleDefn(PDFObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // methods
        //

        #region public bool IsCatchAllStyle()

        /// <summary>
        /// Retruns true if this definition is 'catch all' (does not have any selectors applied)
        /// </summary>
        /// <returns></returns>
        public bool IsCatchAllStyle()
        {
            //We are catch all if we have no specific applied options
            return null == this.AppliedType
                        && string.IsNullOrEmpty(this.AppliedClass)
                        && string.IsNullOrEmpty(this.AppliedID);
        }

        #endregion

        #region public bool IsClassNameMatch(string classname)

        public const char ClassNameSeparator = ' ';
        public const StringComparison ClassNameComparer = StringComparison.Ordinal; //CaseSensitive

        /// <summary>
        /// Returns true if the specified class name is considered a match for this definitions class name
        /// </summary>
        /// <param name="classname"></param>
        /// <returns></returns>
        public bool IsClassNameMatch(string classname)
        {
            if (string.IsNullOrEmpty(classname))
                return false;// string.IsNullOrEmpty(this.AppliedClass);

            if (classname.IndexOf(ClassNameSeparator) > 0)
            {
                int index = 0;
                do
                {
                    index = classname.IndexOf(this.AppliedClass, index, ClassNameComparer);
                    if (index >= 0)
                    {
                        if (IsDistinctClassName(this.AppliedClass, classname, index))
                            return true;
                        else
                            index += this.AppliedClass.Length;
                    }
                }
                while (index > 0);

                return false;
            }
            else
                return string.Equals(this.AppliedClass, classname, ClassNameComparer);
        }

        private bool IsDistinctClassName(string match, string all, int position)
        {
            bool startsdistinct = false;
            bool endsdistinct = false;

            if (position == 0 || char.IsWhiteSpace(all, position - 1))
                startsdistinct = true;
            if (position + match.Length == all.Length || char.IsWhiteSpace(all, position + match.Length))
                endsdistinct = true;

            return startsdistinct && endsdistinct;
        }

        #endregion

        #region public virtual bool IsMatchedTo(IPDFComponent component)

        /// <summary>
        /// Returns true if the specified component is a match (should have applied) this style defintions styles
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        /// <remarks>There is one exception to the rule. If this is a catch all style (no applied-xxx) then 
        /// it is applied to the top level document only</remarks>
        public virtual bool IsMatchedTo(IPDFComponent component)
        {
            if (null == component)
                return false;

            if (this.IsCatchAllStyle())
            {
                //Special case - empty defintion is always applied to the document but no further
                if (component is IPDFDocument)
                    return true;
                else
                    return false;
            }

            if (string.IsNullOrEmpty(this.AppliedID) == false)
            {
                if (this.AppliedID.Equals(component.ID) == false)
                    return false;
            }

            if (null != this.AppliedType)
            {
                if (this.AppliedType.IsAssignableFrom(component.GetType()) == false)
                    return false;
            }

            if (string.IsNullOrEmpty(this.AppliedClass) == false)
            {
                if ((component is IPDFStyledComponent) == false ||
                    (this.IsClassNameMatch((component as IPDFStyledComponent).StyleClass)) == false)
                    return false;
            }

            //We know it is not empty and that everything defined does match
            return true;
        }

        #endregion

        #region public override void MergeInto(PDFStyle style, IPDFComponent forComponent, ComponentState state)

        /// <summary>
        /// If this style definition should be applied to the specified component (in the state) then 
        /// merges all the assigned properties into the provided style
        /// </summary>
        /// <param name="style"></param>
        /// <param name="forComponent"></param>
        /// <param name="state"></param>
        public override void MergeInto(PDFStyle style, IPDFComponent forComponent, ComponentState state)
        {
            if (this.IsMatchedTo(forComponent))
                base.MergeInto(style, forComponent, state);
        }

        #endregion

        //
        // object overrides
        //

        #region public override string ToString()

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Type.ToString());
            sb.Append(":{");
            bool hasitems = false;
            if (!string.IsNullOrEmpty(this.AppliedClass))
            {
                sb.Append("class=");
                sb.Append(this.AppliedClass);
                hasitems = true;
            }
            if (null != this.AppliedType)
            {
                if (hasitems)
                    sb.Append(";");
                sb.Append("type=");
                sb.Append(this.AppliedType.ToString());
                hasitems = true;
            }
            if (!string.IsNullOrEmpty(this.AppliedID))
            {
                if (hasitems)
                    sb.Append("; ");
                sb.Append("id=");
                sb.Append(this.AppliedID);
            }
            sb.Append("}");

            return sb.ToString();
        }

        #endregion

    }

    
}
