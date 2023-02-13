using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Selectors
{


    /// <summary>
    /// Defines a single style match, with a possible Ancestor matching style.
    /// </summary>
    public class StyleSelector
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

        private StyleClassSelector _class;

        /// <summary>
        /// Gets or sets the style-class that this definition is applied to.
        /// </summary>
        public StyleClassSelector AppliedClass
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

        private ComponentState _state = ComponentState.Normal;

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

        public StyleSelector Ancestor
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

        #region public int Priority {get;}

        private int _priority = -1;

        public int Priority
        {
            get
            {
                if (_priority < 0)
                {
                    int depth = 0;
                    _priority = this.CalcPriority(ref depth);
                }
                return _priority;
            }
        }

        #endregion

        #region protected virtual int CalcPriority(ref int depth)

        private const int ElementPriority = 1;
        private const int ClassPriority = 2;
        private const int DoubleClassPriority = 3;
        private const int TripleClassPriority = 4;
        private const int IDPriority = 5;

        private static readonly int[] AncestorFactors = new int[] { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000 };
        private static readonly int[] DirectAncestorFactor = new int[] { 2, 20, 200, 2000, 20000, 200000, 2000000, 20000000 };


        protected virtual int CalcPriority(ref int depth)
        {
            var val = 0;
            var mine = 0;

            if (null != this.Ancestor)
            {
                val = this.Ancestor.CalcPriority(ref depth);
                depth += 1;
            }

            if (string.IsNullOrEmpty(this.AppliedElement) == false || null != this.AppliedType)
                mine += ElementPriority;

            if (null != this.AppliedClass)
            {
                if (this.AppliedClass.AndClass != null)
                {
                    if (this.AppliedClass.AndClass.AndClass != null)
                        mine += TripleClassPriority;
                    else
                        mine += DoubleClassPriority;
                }
                else
                    mine += ClassPriority;
            }
            if (string.IsNullOrEmpty(this.AppliedID) == false)
                mine += IDPriority;

            if (depth >= AncestorFactors.Length)
                depth = AncestorFactors.Length - 1;

            //Fun the factoring for the depth 1, 10, 100, 1000 etc.
            if (depth == 0)
                val = mine;

            else if (null != this.Ancestor && this.Ancestor.Placement == StylePlacement.DirectParent)
                //Direct has higher precedence still
                val += (mine * DirectAncestorFactor[depth]);
            else
                val += (mine * AncestorFactors[depth]);

            return val;
        }

        #endregion

        #region public bool IsMatchedTo(IPDFStyledComponent component, ComponentState state)

        public bool IsMatchedTo(IStyledComponent component, ComponentState state, out int priority)
        {
            priority = 0;
            // check everything
            // return false if it's not a match
            //otherwise return true at the end.

            // fastest first - state, ID, Element, Class, Type

            //If we are only for a specific state
            if (this.AppliedState != ComponentState.Normal)
            {
                //and the state we are looking for is not us, then we don't match
                if (state != this.AppliedState)
                    return false;
            }

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
                var parentPriority = 0;
                var parent = component.Parent;
                if (this.Ancestor.Placement == StylePlacement.DirectParent)
                {
                    while (null != parent && !(parent is IStyledComponent)) // select the closest styled component
                        parent = parent.Parent;

                    if (null == parent)
                        return false;

                    if (this.Ancestor.IsMatchedTo(parent as IStyledComponent, ComponentState.Normal, out parentPriority))
                    {
                        priority = this.Priority;
                        return true;
                    }
                }
                else
                {
                    while (null != parent)
                    {
                        if (parent is IStyledComponent)
                        {
                            if (this.Ancestor.IsMatchedTo(parent as IStyledComponent, state, out parentPriority))
                            {
                                priority = this.Priority;
                                return true;
                            }
                        }
                        parent = parent.Parent;
                    }
                }
                return false;
            }
            else
            {
                priority = this.Priority;
                return true;
            }
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

            if(this.AppliedState != ComponentState.Normal)
            {
                switch(this.AppliedState)
                {
                    case (ComponentState.After):
                        sb.Append("::after");
                        break;
                    case (ComponentState.Before):
                        sb.Append("::before");
                        break;
                    case (ComponentState.Over):
                        sb.Append(":hover");
                        break;
                    default:
                        break;
                }
            }

            if (this.Placement == StylePlacement.DirectParent)
                sb.Append(" >");
        }

        #endregion
    }

}
