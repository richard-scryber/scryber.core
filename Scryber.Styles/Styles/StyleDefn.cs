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
using Scryber.Styles.Selectors;

namespace Scryber.Styles
{
    /// <summary>
    /// Defines a single style that has the capabilities to be applied to multiple page Components
    /// based upon the style matching string.
    /// </summary>
    [PDFParsableComponent("Style")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class StyleDefn : Style, INamingContainer
    {
        /// <summary>
        /// If true then the match has been built from the applied-xxx attributes.
        /// </summary>
        private bool _fromApplied = false;

        #region public Type AppliedType {get; set;}

        private Type _type;

        /// <summary>
        /// Gets or sets the type of components this definition is applied to. Use the matcher as a preference
        /// </summary>
        [PDFAttribute("applied-type")]
        [Obsolete("Use the style Match property (that is implicitly converted from a string instead) e.g. Match = 'html'", false)]
        public Type AppliedType
        {
            get { return _type; }
            set 
            {
                _type = value;
                //reset the match if from applied values
                if (_fromApplied)
                    _match = null;
            }
        }

        #endregion

        #region public string AppliedClass {get; set;}

        private string _class;

        /// <summary>
        /// Gets or sets the style-class that this definition is applied to. Use the style matcher as a preference
        /// </summary>
        [PDFAttribute("applied-class")]
        [Obsolete("Use the style Match property (that is implicitly converted from a string instead) e.g. Match = '.red'", false)]
        public string AppliedClass
        {
            get { return _class; }
            set { 
                _class = value; 
                //reset the match if from applied values
                if (_fromApplied)
                    _match = null;
            }
        }

        #endregion

        #region public string AppliedID {get;set;}

        private string _id;
    
        /// <summary>
        /// Gets or sets the id of the components this definition is applied to. Use the style matcher as a preference
        /// </summary>
        [PDFAttribute("applied-id")]
        [Obsolete("Use the style Match property (that is implicitly converted from a string instead) e.g. Match = '#id'", false)]
        public string AppliedID
        {
            get { return _id; }
            set { 
                _id = value;

                //reset the match if from applied values
                if (_fromApplied)
                    _match = null;
            }
        }

        #endregion

        #region public ComponentState AppliedState {get;set;}

        private ComponentState _state;

        /// <summary>
        /// Not currently supported
        /// </summary>
        [PDFAttribute("applied-state")]
        [Obsolete("Use the style Match property (that is implicitly converted from a string instead) e.g. Match = 'div::before'", false)]
        public ComponentState AppliedState
        {
            get { return _state; }
            set { 
                _state = value;
                //reset the match if from applied values
                if (_fromApplied)
                    _match = null;
            }
        }

        #endregion

        #region public PDFStyleMatcher Match {get; set;}

        private StyleMatcher _match;

        /// <summary>
        /// Gets or sets the selector to match this style on. Supports parsing or implicit conversion from a css style selector
        /// </summary>
        [PDFAttribute("match")]
        public StyleMatcher Match
        {
            get { return _match; }
            set {
                _match = value;
                _fromApplied = false;
            }
        }

        #endregion

        //
        // ctors
        //

        #region public PDFStyleDefn()

        public StyleDefn()
            : this(ObjectTypes.Style)
        {
        }

        #endregion

        #region public PDFStyleDefn(Type appliedtype, string appliedid, string appliedclassname)

        public StyleDefn(Type appliedtype, string appliedid, string appliedclassname)
            : this()
        {
            this._class = appliedclassname;
            this._id = appliedid;
            this._type = appliedtype;
        }

        #endregion

        #region public StyleDefn(string match)

        /// <summary>
        /// Creates a new style definition with the matching selector
        /// </summary>
        /// <param name="match"></param>
        public StyleDefn(string match)
            : this(ObjectTypes.Style)
        {
            this.Match = (StyleMatcher)(match);
        }

        #endregion

        #region protected PDFStyleDefn(PDFObjectType type)

        protected StyleDefn(ObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // methods
        //

        #region public virtual bool IsMatchedTo(IPDFComponent component)

        /// <summary>
        /// Returns true if the specified component is a match (should have applied) this style defintions styles
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        /// <remarks>There is one exception to the rule. If this is a catch all style (no applied-xxx) then 
        /// it is applied to the top level document only</remarks>
        public virtual bool IsMatchedTo(IComponent component, ComponentState state, out int priority)
        {
            
            if (null == component)
            {
                priority = 0;
                return false;
            }
            var match = this.AssertMatcher();
            return match.IsMatchedTo(component, state, out priority);

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
        public override void MergeInto(Style style, IComponent forComponent, ComponentState state)
        {
            int priority;

            if (this.IsMatchedTo(forComponent, state, out priority))
            {
                if (this.HasVariables)
                    this.MergeVariables(style);

                this.MergeInto(style, priority);
                
            }
        }

        #endregion

        #region protected virtual void MergeVariables(Style style)
        
        protected virtual void MergeVariables(Style style)
        {
            if (null == style)
                throw new ArgumentNullException(nameof(style));

            if(this.HasVariables)
            {
                foreach (var item in this.Variables)
                {
                    style.AddVariable(item.Value);
                }
            }
        }

        #endregion

        #region protected virtual PDFStyleMatcher AssertMatcher()

        protected virtual StyleMatcher AssertMatcher()
        {
            if(null == this._match)
            {
                this._fromApplied = true;
                if (string.IsNullOrEmpty(this.AppliedID) && string.IsNullOrEmpty(this.AppliedClass)
                    && (null == this.AppliedType))
                    return new StyleCatchAllMatcher();

                var stack = new StyleSelector() { AppliedClass = string.IsNullOrEmpty(this.AppliedClass) ? null : new StyleClassSelector(this.AppliedClass), 
                                                  AppliedID = this.AppliedID, 
                                                  AppliedType = this.AppliedType, 
                                                  AppliedState = this.AppliedState, 
                                                  Placement = StylePlacement.Any };

                this._match = new StyleMatcher(stack);
            }
            return this._match;
        }

        #endregion


        //
        // object overrides
        //

        #region public override string ToString()

        /// <summary>
        /// String representation of the style definition 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.Type.ToString());
            sb.Append(":{ ");
            var match = this.AssertMatcher();
            match.ToString(sb);
            sb.Append(" }");

            return sb.ToString();
        }

        #endregion

    }

    
}
