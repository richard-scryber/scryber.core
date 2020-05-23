using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Scryber.Generation
{
    /// <summary>
    /// Encapsulates single declared member outlet on a controller 
    /// and allows setting of the value of the outlet
    /// </summary>
    public class ParserControllerOutlet
    {
        //
        // properties
        //

        #region public MemberInfo OutletMember { get; private set; }

        /// <summary>
        /// Gets or sets the Member this outlet refers to.
        /// </summary>
        public MemberInfo OutletMember { get; private set; }

        #endregion

        #region public bool IsProperty { get; private set; }

        /// <summary>
        /// Returns true if this outlet is a property (false for a field)
        /// </summary>
        public bool IsProperty { get; private set; }

        #endregion

        #region public string ID { get; private set; }

        /// <summary>
        /// Gets the name of this Outlet
        /// </summary>
        public string ID { get; private set; }

        #endregion

        #region public bool Required { get; private set; }

        /// <summary>
        /// True if this outlet must be assigned a value before parsing a template has finished.
        /// </summary>
        public bool Required { get; private set; }

        #endregion

        //
        // .ctor
        //

        #region public ParserControllerOutlet(MemberInfo outlet, string id, bool required)

        /// <summary>
        /// Creates a new Parser Outlet with the memeber, name and required flag
        /// </summary>
        /// <param name="outlet">Must be a read write instance property or field</param>
        /// <param name="id">The id of the outlet - will default to the outlet member name if this is not set</param>
        /// <param name="required">True if this outlet must be assigned a value</param>
        public ParserControllerOutlet(MemberInfo outlet, string id, bool required)
        {
            if (null == outlet)
                throw new ArgumentNullException("outlet");

            if (string.IsNullOrEmpty(id))
                id = outlet.Name;

            this.OutletMember = outlet;
            this.ID = id;
            this.Required = required;

            ValidateMember(outlet);
        }

        #endregion

        //
        // methods
        //

        #region protected virtual void ValidateMember(MemberInfo outlet)

        /// <summary>
        /// Validates that the outlet member is supported
        /// </summary>
        /// <param name="outlet"></param>
        protected virtual void ValidateMember(MemberInfo outlet)
        {
            //Validate the type of member and it's reflected attributes - read write instance field or property

            if (outlet.MemberType == MemberTypes.Property)
            {
                PropertyInfo prop = (PropertyInfo)outlet;
                if (prop.CanWrite == false)
                    throw new NotSupportedException(string.Format(Errors.OutletsMustBeReadWrite, outlet.Name));
                else if (prop.GetSetMethod().IsStatic)
                    throw new NotSupportedException(string.Format(Errors.OutletsCannotBeStatic, outlet.Name));

                this.IsProperty = true;
            }
            else if (outlet.MemberType == MemberTypes.Field)
            {
                FieldInfo fld = (FieldInfo)outlet;
                if (fld.IsInitOnly == true || fld.IsLiteral == true)
                    throw new NotSupportedException(string.Format(Errors.OutletsMustBeReadWrite, outlet.Name));
                else if (fld.IsStatic)
                    throw new NotSupportedException(string.Format(Errors.OutletsCannotBeStatic, outlet.Name));

                this.IsProperty = false;
            }
            else
                throw new NotSupportedException(string.Format(Errors.OutletsCanOnlyBePropertiesOrFields, outlet.Name));
        }

        #endregion

        #region public virtual void SetValue(object controller, object value)

        /// <summary>
        /// Sets the value of this Outlets member on the provided controller to the value passed.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="value"></param>
        public virtual void SetValue(object controller, object value)
        {
            try
            {
                if (this.IsProperty)
                    ((PropertyInfo)this.OutletMember).SetValue(controller, value, null);
                else
                    ((FieldInfo)this.OutletMember).SetValue(controller, value);
            }
            catch (Exception ex)
            {
                throw new PDFParserException(string.Format(Errors.CouldNotSetOutletPropertyValue, this.OutletMember.Name), ex);
            }
        }

        #endregion

    }

    
    /// <summary>
    /// A list of Controller Outlets that can be accessed by index or by id of the outlet
    /// </summary>
    public class ParserControllerOutletList : System.Collections.ObjectModel.KeyedCollection<string,ParserControllerOutlet>
    {

        #region protected override string GetKeyForItem(ParserControllerOutlet item)

        /// <summary>
        /// Overrides the abstract method to return the name of the outlet as a key value.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(ParserControllerOutlet item)
        {
            return item.ID;
        }

        #endregion

        #region public bool TryGetOutlet(string name, out ParserControllerOutlet outlet)

        /// <summary>
        /// Attempts to retrieve the outlet by name, returning true if found otherwise false.
        /// </summary>
        /// <param name="name">The name of the outlet looking for</param>
        /// <param name="outlet">Set to the found outlet, or null</param>
        /// <returns>True if a matching outlet was found.</returns>
        public bool TryGetOutlet(string name, out ParserControllerOutlet outlet)
        {
            if (this.Count == 0)
            {
                outlet = null;
                return false;
            }
            else
                return this.Dictionary.TryGetValue(name, out outlet);
        }

        #endregion

    }
}
