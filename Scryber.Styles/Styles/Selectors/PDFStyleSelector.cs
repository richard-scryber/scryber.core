using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Selectors
{


    /// <summary>
    /// Defines a single style match, with a possible Ancestor matching style.
    /// </summary>
    public class PDFStyleSelector
    {

        #region public Type AppliedType { get; set; }

        private Type _appliedType;

        /// <summary>
        /// Gets or sets the type this style should match against
        /// </summary>
        public Type AppliedType
        {
            get { return _appliedType; }
            set { _appliedType = value; }
        }

        #endregion

        #region public string AppliedElement {get; set;}

        private string _element;

        /// <summary>
        /// Gets or sets the type of components this definition is applied to.
        /// </summary>
        public string AppliedElement
        {
            get { return _element; }
            set { _element = value; }
        }

        #endregion

        #region public PDFStyleClassMatch AppliedClass {get; set;}

        private PDFStyleClassSelector _class;

        /// <summary>
        /// Gets or sets the style-class that this definition is applied to.
        /// </summary>
        public PDFStyleClassSelector AppliedClass
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
        public ComponentState AppliedState
        {
            get { return _state; }
            set { _state = value; }
        }

        #endregion

        #region public bool IsEmpty {get;set;}

        public bool IsEmpty
        {
            get
            {
                if (null == this.AppliedElement && null == this.AppliedClass && string.IsNullOrEmpty(this.AppliedID))
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region public PDFStyleSelector Ancestor {get;set;}

        public PDFStyleSelector Ancestor
        {
            get;
            set;
        }

        #endregion

        #region public bool HasAncestor {get;}

        public bool HasAncestor
        {
            get { return this.Ancestor != null; }
        }

        #endregion

        #region public StylePlacement Placement {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public StylePlacement Placement
        {
            get; set;
        }

        #endregion

        #region public bool IsMatchedTo(IPDFStyledComponent component, ComponentState state)

        public bool IsMatchedTo(IPDFStyledComponent component, ComponentState state)
        {

            // check everything
            // return false if it's not a match
            //otherwise return true at the end.

            // fastest first - ID, Element, Class, Type

            if (string.IsNullOrEmpty(this.AppliedID) == false)
            {
                if (string.IsNullOrEmpty(component.ID))
                    return false;
                else if (!this.AppliedID.Equals(component.ID))
                    return false;
            }

            if (string.IsNullOrEmpty(this.AppliedElement) == false)
            {
                if (string.IsNullOrEmpty(component.ElementName))
                    return false;
                else if (!this.AppliedElement.Equals(component.ElementName))
                    return false;
            }

            if (null != this.AppliedClass)
            {
                if (string.IsNullOrEmpty(component.StyleClass))
                    return false;
                else if (!this.AppliedClass.IsMatchedTo(component, state))
                    return false;
            }

            if (null != this.AppliedType)
            {
                if ((this.AppliedType.IsInstanceOfType(component)) == false)
                    return false;
            }

            if (this.HasAncestor)
            {
                var parent = component.Parent;
                if (this.Ancestor.Placement == StylePlacement.DirectParent)
                {
                    while (null != parent && !(parent is IPDFStyledComponent)) // select the closest styled component
                        parent = parent.Parent;

                    if (null == parent)
                        return false;

                    if (this.Ancestor.IsMatchedTo(parent as IPDFStyledComponent, state))
                        return true;
                }
                else
                {
                    while (null != parent)
                    {
                        if (parent is IPDFStyledComponent)
                        {
                            if (this.Ancestor.IsMatchedTo(parent as IPDFStyledComponent, state))
                                return true;
                        }
                        parent = parent.Parent;
                    }
                }
                return false;
            }
            else
                return true;
        }

        #endregion

        #region public override string ToString()

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this.ToString(sb);
            return sb.ToString();
        }

        #endregion

        #region protected internal virtual void ToString(StringBuilder sb)

        protected internal virtual void ToString(StringBuilder sb)
        {
            if (this.HasAncestor)
            {
                this.Ancestor.ToString(sb);
            }

            if (sb.Length > 0)
                sb.Append(" ");
            if (!string.IsNullOrEmpty(this.AppliedElement))
                sb.Append(this.AppliedElement);
            else if (null != this.AppliedType)
                sb.Append(this.AppliedType.ToString());

            if (null != this.AppliedClass)
            {
                this.AppliedClass.ToString(sb);
            }

            if (!string.IsNullOrEmpty(this.AppliedID))
            {
                sb.Append("#");
                sb.Append(this.AppliedID);
            }

            if (this.Placement == StylePlacement.DirectParent)
                sb.Append(" >");
        }

        #endregion
    }

}
