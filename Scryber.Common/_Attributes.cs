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

    //
    // Amalgamation of the attribute classes used in the scryber library
    //


    #region PDFParsableValue Attribute

    /// <summary>
    /// Placeholder attribute that defines that a class can be parsed from a string 
    /// using a public static Parse(string) method on the class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class PDFParsableValueAttribute : Attribute
    {
        /// <summary>
        /// Returns true if the class or struct this attribute decorates has different string formats for different cultures 
        /// </summary>
        public bool CultureDependant { get; set; }

        public PDFParsableValueAttribute()
        {
            CultureDependant = false;
        }
    }

    #endregion

    #region PDFParsableComponent Attribute

    /// <summary>
    /// Identifies a root complex component that the PDFParser can reflect and parse. 
    /// This attribute is not inherited for good reason
    /// </summary>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PDFParsableComponentAttribute : Attribute
    {
        private string _name;

        /// <summary>
        /// Gets or Sets the name of the elementthe parser should interpret 
        /// as a reference to the type this attribute is declared on.
        /// </summary>
        public string ElementName
        {
            get { return _name; }
            set { _name = value; }
        }


        

        /// <summary>
        /// Creates a new instance of the PDFComponent Attribute with the specific name
        /// </summary>
        /// <param name="name">The Components name</param>
        public PDFParsableComponentAttribute(string name)
        {
            this._name = name;
        }
    }

    #endregion

    #region PDFRemoteParsableComponent Attribute

    /// <summary>
    /// Defines a parsable complex component that can be referenced from an separate file 
    /// ('source' is the name of the attribute that indicates the source file)
    /// </summary>
    /// <remarks>This attribute is not inherited</remarks>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PDFRemoteParsableComponentAttribute : Attribute
    {
        private string _name;

        /// <summary>
        /// Gets or Sets the name of the element the parser should interpret 
        /// as a reference to the type this attribute is declared on.
        /// </summary>
        public string ElementName
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the name of the attribute on the remote component, that will specify the source location.
        /// </summary>
        public string SourceAttribute { get; set; }

        /// <summary>
        /// Creates a new instance of the PDFComponent Attribute with the specific name
        /// </summary>
        /// <param name="name">The Components name</param>
        public PDFRemoteParsableComponentAttribute(string name)
        {
            this._name = name;
            this.SourceAttribute = "source";
        }
    }

    #endregion

    #region PDFParserIgnore Attribute

    /// <summary>
    /// Any class, method, property or event decorated with this attribute will be ignored by the PDFParser (if the ignore flag is set to false)
    /// </summary>
    [Serializable()]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class PDFParserIgnoreAttribute : Attribute
    {
        private bool _ignore;

        /// <summary>
        /// Gets or sets whether to ignore the declaration that this attribute is applied to.
        /// </summary>
        public bool Ignore { get { return _ignore; } set { _ignore = value; } }

        /// <summary>
        /// Sets Ignore to true by default.
        /// </summary>
        public PDFParserIgnoreAttribute() : this(true) { }

        /// <summary>
        /// Sets Ignore to the value ofthe ignore parameter.
        /// </summary>
        /// <param name="ignore"></param>
        public PDFParserIgnoreAttribute(bool ignore) { this._ignore = ignore; }

    }

    #endregion

    #region PDFAttribute Attribute

    /// <summary>
    /// Defines that a property can be included as an PDF Components attribute
    /// </summary>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Event, AllowMultiple = false, Inherited = true)]
    public class PDFAttributeAttribute : Attribute
    {
        private string _name;

        /// <summary>
        /// Gets or Sets the name of the PDF attribute - cannot be null or empty
        /// </summary>
        public string AttributeName
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _namespace;

        /// <summary>
        /// Gets or sets the namespace of the PDF attribute
        /// </summary>
        public string AttributeNamespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }

        /// <summary>
        /// By default this is false, if true, then the value of the attribute can only be set in code, or by binding to a value.
        /// It does not support explicit setting of the value.
        /// </summary>
        public bool BindingOnly
        {
            get;
            set;
        }
        public PDFAttributeAttribute(string name)
            : this(name, string.Empty)
        {
        }

        /// <summary>
        /// Creates a new instance of the PDFAttribute Attribute with the specific name and namespace
        /// </summary>
        /// <param name="name">The attributes name - cannot be null or empty</param>
        /// <param name="ns">The namespace of the attribute.
        /// An empty value will be treated as the namespace of the element it is declared on</param>
        public PDFAttributeAttribute(string name, string ns)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this._name = name;
            if (null == ns)
                ns = string.Empty;
            this._namespace = ns;
        }
    }

    #endregion

    #region PDFElement Attribute

    /// <summary>
    /// Identifies a property that can be included when parsing a component as a complex element or a collection of elements
    /// </summary>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Event, AllowMultiple = true, Inherited = true)]
    public class PDFElementAttribute : Attribute
    {
        private string _name;
        private string _ns;
        private bool _ignoreInner = false;
        /// <summary>
        /// Gets or sets the name of the element that can be parsed to the value of the property this attribute is declared on.
        /// If the name is empty then it is the default property.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string NameSpace
        {
            get { return _ns; }
            set { _ns = value; }
        }

        public bool IgnoreInnerTags
        {
            get { return _ignoreInner; }
            set { _ignoreInner = value; }
        }

        /// <summary>
        /// Defines an element in the PDF file that is the default inner element
        /// </summary>
        public PDFElementAttribute()
            : this(string.Empty,string.Empty)
        { }
        
        public PDFElementAttribute(string name)
            : this(name, string.Empty)
        {
        }

        public PDFElementAttribute(string name, string ns)
        {
            this._name = name;
            this._ns = ns;
        }
    }

    #endregion

    #region PDFArray Attribute

    /// <summary>
    /// Defines the property as an array of elements with a specific base type
    /// </summary>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PDFArrayAttribute : Attribute
    {
        private Type _basetype;

        /// <summary>
        /// Defines the base type of all the contained elements if this is a collection
        /// </summary>
        public Type ContentBaseType
        {
            get { return _basetype; }
            set { _basetype = value; }
        }

        
        public PDFArrayAttribute()
            : this(typeof(PDFObject))
        {
        }

        public PDFArrayAttribute(Type basetype)
        {
            _basetype = basetype;
        }

    }

    #endregion

    #region PDFTemplate Attribute

    /// <summary>
    /// Defines the property as a TemplateContainer
    /// </summary>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PDFTemplateAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the base type for the content of the template
        /// </summary>
        public Type BaseContentType { get; set; }

        /// <summary>
        /// If set then the contents will be laidout as a block rather than simply flowing with content.
        /// This allows styles and classes, but will always be on it's own
        /// </summary>
        public bool IsBlock { get; set; }

        public PDFTemplateAttribute()
            : this(null)
        { }

        public PDFTemplateAttribute(Type baseContent)
        { }
    }

    #endregion

    
    #region PDFRequiredFramework Attribute
    
    /// <summary>
    /// Defines the minimum (and optionally maximum) version of framework (Scryber.Common) this class can be used with.
    /// The XML Parser will check this when loading components to ensure they are valid.
    /// </summary>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PDFRequiredFrameworkAttribute : Attribute
    {
        /// <summary>
        /// A reference empty version - can be compared to each version property on 
        /// an instance of this class to check if it's empty
        /// </summary>
        public static Version Empty = new Version(0, 0);

        private Version _minvers;

        /// <summary>
        /// Gets or sets the minimum required version
        /// </summary>
        public Version Minimum
        {
            get { return _minvers; }
            set { _minvers = value; }
        }

        public Version _maxvers;

        /// <summary>
        /// Gets or sets the maximum required version
        /// </summary>
        public Version Maximum
        {
            get { return _maxvers; }
            set { _maxvers = value; }
        }

        //
        // .ctor
        //

        /// <summary>
        /// Creates a new empty PDFRequiredFramework attribute
        /// </summary>
        public PDFRequiredFrameworkAttribute()
            : this(Empty, Empty)
        {
        }

        /// <summary>
        /// Creates a new PDFRequiredFramework attribute with a minimum version
        /// </summary>
        /// <param name="minimum"></param>
        public PDFRequiredFrameworkAttribute(string minimum)
            : this(new Version(minimum), Empty)
        {
        }

        private PDFRequiredFrameworkAttribute(Version min, Version max)
        {
            this._minvers = min;
            this._maxvers = max;
        }

        /// <summary>
        /// Creates a new PDFRequiredFramework attribute with a minimum and maximum attribute
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public PDFRequiredFrameworkAttribute(string minimum, string maximum)
            : this(new Version(minimum), new Version(maximum))
        {
        }
    }

    #endregion

    //
    // controller attributes
    //

    #region PDFOutlet Attribute

    /// <summary>
    /// Applied to properties and variables on controllers for PDF documents, pages etc so 
    /// the declared components can be set at run time
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public class PDFOutletAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the optional ID of the component this outlet should refer to.
        /// If not set, then the name of the member will be used as the ID.
        /// </summary>
        public string ComponentID { get; set; }

        /// <summary>
        /// If true then any template must assign a component to the member this attribute is declared on.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// If set to false then the member this attribute is applied to will not be considered an outlet
        /// </summary>
        public bool IsOutlet { get; set; }

        /// <summary>
        /// Marks this field or property as an Outlet that can be assigned a value when an associated template is parsed.
        /// The outlet name is by default the same as the property or field name, and it is not required.
        /// </summary>
        public PDFOutletAttribute()
        {
            this.IsOutlet = true;
        }

        /// <summary>
        /// Marks this field or property as an Outlet that can be assigned a value when an associated template is parsed.
        /// The outlet id is the provided componentID, and it is not required.
        /// </summary>
        /// <param name="componentID"></param>
        public PDFOutletAttribute(string componentID)
        {
            this.ComponentID = componentID;
            this.IsOutlet = true;
        }
    }

    #endregion

    #region PDFAction Attribute

    /// <summary>
    /// Applied to methods on controller classes for PDF documents so that they can be registered for events
    /// and invoked during the document lifecycle at runtime
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PDFActionAttribute : Attribute
    {
        /// <summary>
        /// The name in the document (or page or component) template that refers to this method
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag that identifies if this is an action (default is true, but can be switched off if set to false).
        /// </summary>
        public bool IsAction { get; set; }

        /// <summary>
        /// Marks the associated method as a registerable event handler 
        /// by default with the same name as the actual method itself
        /// </summary>
        public PDFActionAttribute()
        {
            this.IsAction = true;
        }

        /// <summary>
        /// Marks the associated method as a registerable event handler
        /// with the specified name.
        /// </summary>
        /// <param name="name"></param>
        public PDFActionAttribute(string name)
        {
            this.Name = name;
            this.IsAction = true;
        }
    }

    #endregion

    //
    // Designer attributes
    //

    /// <summary>
    /// Declares a property or fields as visible in the studio designer
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class PDFDesignableAttribute : Attribute
    {
        public PDFDesignableAttribute(): this(string.Empty)
        {
        }

        public PDFDesignableAttribute(string name)
        {
            this.Name = name;
            this.Bindable = true;
        }

        public string Name { get; set; }

        public string Category { get; set; }

        public int Priority { get; set; }

        public string Type { get; set; }

        public bool Ignore { get; set; }

        public string Validate { get; set; }

        public string JSOptions { get; set; }

        public bool BlockOnly { get; set; }

        public bool Bindable { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PDFJSConvertorAttribute : Attribute
    {
        public PDFJSConvertorAttribute(string type)
        {
            this.JSType = type;
        }

        public string JSType { get; set; }

        public string JSParams { get; set; }

        public string JSFile { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PDFDesignerToolboxAttribute : Attribute
    {
        public string Category { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string XmlCode { get; set; }

    }
    
}
