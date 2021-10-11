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
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Scryber.Drawing;
namespace Scryber.Components
{

    /// <summary>
    /// Defines a simple item value that can be 
    /// declared at the top of the document level
    /// </summary>
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public abstract class DocumentItemValue : IKeyValueProvider
    {
        public Type ItemType { get; set; }

       
        [PDFAttribute("id")]
        [PDFDesignable("ID", Category = "General", Priority = 1, Type = "ID", Bindable = false, JSOptions = "")]
        public string ID { get; set; }

        [PDFAttribute("query-string-overrides")]
        [PDFDesignable("Allow User Editing", Category = "General", Priority = 2, Type = "Boolean", Bindable = false, JSOptions = "")]
        public bool QueryStringOverrides
        {
            get; set;
        }

        [PDFAttribute("display-name")]
        [PDFDesignable("Editor Display Name", Category = "General", Priority = 3, Bindable = false)]
        public string DisplayName
        {
            get;
            set;
        }

        [PDFAttribute("required")]
        [PDFDesignable("Required", Category = "General", Priority = 6, Type = "Boolean", Bindable = false)]
        public bool Required
        {
            get;
            set;
        }


        [PDFAttribute("description")]
        [PDFDesignable("Editor Description", Category = "General", Priority = 4, Bindable = false)]
        public string Description
        {
            get;
            set;
        }

        const string ChoiceTypeOptions = "{\"id\":\"EditorControl\",\"name\":\"Controls\",\"type\":\"Core\"," +
            "\"params\":[]}";

        [PDFAttribute("editor")]
        [PDFDesignable("Editor Control", Category = "General", Priority = 5, Bindable = false, Type = "DataLookup", JSOptions = ChoiceTypeOptions)]
        public string Editor
        {
            get;
            set;
        }

        const string ProviderOptions = "{\"id\":\"DataProvider\",\"name\":\"Providers\",\"type\":\"Core\"," +
            "\"params\":[]}";

        [PDFAttribute("data-provider")]
        [PDFDesignable("Data Provider", Category = "Data", Priority = 1, Bindable = false, Type ="DataLookup", JSOptions = ProviderOptions)]
        public string DataProvider
        {
            get;
            set;
        }


        const string DataTypeOptions = "{\"id\":\"DataTypes\",\"name\":\"Types\",\"type\":\"Core\",\"parent\":\"DataProvider\"," +
            "\"params\":[" +
            "   {\"id\":\"DataProvider\",\"type\":\"Providers\"," +
            "\"value\":{\"type\":\"lookup\",\"component-value\":\"@data-provider\"}" +
            "   }" +
            "]}";

        [PDFAttribute("data-type")]
        [PDFDesignable("Data Type", Category = "Data", Priority = 2, Bindable = false, Type = "DataLookup", JSOptions = DataTypeOptions)]
        public string DataType
        {
            get;
            set;
        }

        [PDFAttribute("data-parent")]
        [PDFDesignable("Data Parent", Category = "Data", Priority = 3, Bindable = false)]
        public string DataParent
        {
            get;
            set;
        }


        protected DocumentItemValue(Type nativeType)
        {
            this.ItemType = nativeType;
            this.QueryStringOverrides = false;
        }

        public object GetNativeValue(string key, IComponent comp)
        { 
            return this.DoGetNativeValue(key, null, comp);
        }

        protected abstract object DoGetNativeValue(string key, string qsValue, IComponent comp);
        
        
        public void Init()
        {
            
        }

        public void SetNativeValue(string key, object value, IComponent owner)
        {
            if (key != this.ID)
                throw new InvalidOperationException("The keys do not match");

            try
            {
                if (value is String)
                    this.DoSetNativeValueFromString(value as string, owner);
                else
                    this.DoSetNativeValue(value, owner);
            }
            catch (Exception ex)
            {
                throw new Scryber.PDFDataException("Could not convert the value to a native type " + this.ItemType.ToString(), ex);
            }
        }

        public void SetValue(string key, string value, IComponent owner)
        {
            if (key != this.ID)
                throw new InvalidOperationException("The keys do not match");

            this.DoSetNativeValueFromString(value, owner);
        }

        protected abstract void DoSetNativeValueFromString(string value, IComponent owner);

        protected abstract void DoSetNativeValue(object value, IComponent owner);

    }


    [PDFParsableComponent("Int-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class IntegerItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Number", Bindable = false, JSOptions = "")]
        public int Value { get; set; }

        public IntegerItemValue()
            : base(typeof(int))
        {
            this.Value = 0;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            int val;
            if (string.IsNullOrEmpty(qsValue) || !TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected virtual bool TryParse(string value, out int parsed)
        {
            parsed = 0;
            string num = "";
            int index = 0;
            while (index < value.Length && Char.IsDigit(value[index]))
            {
                num += value[index];
                index++;
            }
            if (!string.IsNullOrEmpty(num))
                return int.TryParse(num, out parsed);
            else
                return false;

        }

        
        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            int val;
            if (int.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = 0;
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.Value = 0;
            else
                this.Value = (int)value;

        }
    }


    [PDFParsableComponent("String-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class StringItemValue : DocumentItemValue
    {
        private string _value;

        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "String", Bindable = false, JSOptions = "")]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public StringItemValue()
            : base(typeof(string))
        {
            this.Value = string.Empty;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            if (!string.IsNullOrEmpty(qsValue))
                return qsValue;
            else
                return this.Value;
        }
        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            this.Value = value;
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.Value = null;
            else
                this.Value = value.ToString();
        }
    }


    [PDFParsableComponent("Guid-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class GuidItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "String", Bindable = false, JSOptions = "")]
        public Guid Value { get; set; }

        public GuidItemValue()
            : base(typeof(Guid))
        {
            this.Value = Guid.Empty;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            Guid val;
            if (string.IsNullOrEmpty(qsValue) || !Guid.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            Guid val;
            if (Guid.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = Guid.Empty;
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.Value = Guid.Empty;
             else
                this.Value = (Guid)value;

        }
    }


    [PDFParsableComponent("Double-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class DoubleItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Number", Bindable = false, JSOptions = "")]
        public double Value { get; set; }

        public DoubleItemValue()
            : base(typeof(double))
        {
            this.Value = 0.0;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            double val;
            if (string.IsNullOrEmpty(qsValue) || !double.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            double val;
            if (double.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = 0;
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.Value = 0;
            else
                this.Value = (double)value;
        }
    }


    [PDFParsableComponent("Bool-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class BooleanItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Boolean", Bindable = false, JSOptions = "")]
        public bool Value { get; set; }

        public BooleanItemValue()
            : base(typeof(bool))
        {
            this.Value = false;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            bool val;
            if (string.IsNullOrEmpty(qsValue) || !bool.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            bool val;
            if (bool.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = false;
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.Value = false;
            else
                this.Value = (bool)value;
        }
    }


    [PDFParsableComponent("Date-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class DateItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Date", Bindable = false, JSOptions = "")]
        public DateTime Value { get; set; }

        public DateItemValue()
            : base(typeof(int))
        {
            this.Value = DateTime.Now;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            DateTime val;
            if (string.IsNullOrEmpty(qsValue) || !DateTime.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            DateTime val;
            if (DateTime.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = DateTime.MinValue;
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.Value = DateTime.MinValue;
            else
                this.Value = (DateTime)value;
        }
    }


    [PDFParsableComponent("Unit-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class UnitItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "PDFUnit", Bindable = false, JSOptions = "")]
        public Unit Value { get; set; }

        public UnitItemValue()
            : base(typeof(Unit))
        {
            this.Value = Unit.Empty;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            Unit val;
            if (string.IsNullOrEmpty(qsValue) || !Unit.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            Unit val;
            if (Unit.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = Unit.Empty;
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.Value = Unit.Empty;
            else
                this.Value = (Unit)value;
        }
    }


    [PDFParsableComponent("Color-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class ColorItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "PDFColor", Bindable = false, JSOptions = "")]
        public Color Value { get; set; }

        public ColorItemValue()
            : base(typeof(Color))
        {
            this.Value = Color.Transparent;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            Color val;
            if (string.IsNullOrEmpty(qsValue) || !Color.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            Color val;
            if (string.IsNullOrEmpty(value))
                this.Value = Color.Transparent;

            else if (Color.TryParse(value, out val))
                this.Value = val;
            else
                throw new InvalidCastException("Could not parse the value '" + value + "' to a PDFColor");
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.Value = Color.Transparent;
            else
                this.Value = (Color)value;
        }
    }


    [PDFParsableComponent("Thickness-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class ThicknessItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Thickness", Bindable = false, JSOptions = "")]
        public Thickness Value { get; set; }

        public ThicknessItemValue()
            : base(typeof(Color))
        {
            this.Value = Thickness.Empty();
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            Thickness val;
            if (string.IsNullOrEmpty(qsValue) || !Thickness.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            Thickness val;
            if (Thickness.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = Thickness.Empty();
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.Value = Thickness.Empty();
            else
                this.Value = (Thickness)value;
        }
    }

    [PDFParsableComponent("Enum-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class EnumItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Select", Bindable = false, JSOptions = "")]
        public string EnumValue { get; set; }

        [PDFAttribute("type")]
        public string EnumType { get; set; }

        public EnumItemValue()
            : base(typeof(Enum))
        {

        }

        
        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            string val = null;
            if (string.IsNullOrEmpty(qsValue))
                val = this.EnumValue;

            if (string.IsNullOrEmpty(this.EnumType))
                return null;
            else if (string.IsNullOrEmpty(val))
                return null;
            else
            {
                object parsed;
                try
                {
                    Type eType = Type.GetType(this.EnumType);
                    if (null == eType)
                        throw new NullReferenceException("The binding enumeration type '" + this.EnumType + "' could not be found.");
                    parsed = Enum.Parse(eType, val);
                }
                catch (Exception ex)
                {
                    throw new PDFParserException(string.Format(Errors.CouldNotParseValue_2, this.EnumValue, this.EnumType), ex);
                }

                return parsed;
            }
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            this.EnumValue = value;
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.EnumValue = null;
            else
            {
                Type eType = Type.GetType(this.EnumType);
                if (null == eType)
                    throw new NullReferenceException("The binding enumeration type '" + this.EnumType + "' could not be found.");

                object parsed = Enum.Parse(eType, value.ToString());

                this.EnumValue = parsed.ToString();
            }
        }

    }

    [PDFParsableComponent("Xml-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class XmlItemValue : DocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        public System.Xml.XmlNode XmlData { get; set; }

        public XmlItemValue()
            : base(typeof(System.Xml.XmlNode))
        {
        }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            return this.XmlData;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            if (string.IsNullOrEmpty(value))
                this.XmlData = null;
            else
            {
                var doc = new XmlDocument();
                doc.InnerXml = value;
                this.XmlData = doc.FirstChild;
            }
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (null == value)
                this.XmlData = null;
            else if (value is System.Xml.Linq.XNode)
            {
                using (var reader = ((System.Xml.Linq.XNode)value).CreateReader())
                {
                    var doc = new System.Xml.XmlDocument();
                    doc.Load(reader);
                    this.XmlData = doc.DocumentElement;
                }
            }
            else if (value is System.Xml.XPath.XPathNavigator)
            {
                using (var reader = ((System.Xml.XPath.XPathNavigator)value).ReadSubtree())
                {
                    var doc = new System.Xml.XmlDocument();
                    doc.Load(reader);
                    this.XmlData = doc.DocumentElement;
                }
            }
            else if (value is XmlNode)
                this.XmlData = (XmlNode)value;

            else
                throw new InvalidCastException("Could not convert the value to an XmlNode, value should be provided as an XmlNode, XPathNavigator or a Linq XNode");
            
        }

    }


    [PDFParsableComponent("Template-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class TemplateItemValue : DocumentItemValue
    {
        [PDFElement()]
        [PDFTemplate()]
        public ITemplate Template
        {
            get;
            set;
        }

        public TemplateItemValue()
            : base(typeof(ITemplate))
        {

        }

        protected override object DoGetNativeValue(string key, string sqValue, IComponent comp)
        {
            return this.Template;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            var namespaceManager = GetNamespaceManager(owner);
            this.Template = new Data.ParsableTemplateGenerator(value, namespaceManager);
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            this.Template = (ITemplate)value;
        }

        protected virtual XmlNamespaceManager GetNamespaceManager(IComponent owner)
        {
            System.Xml.NameTable nt = new System.Xml.NameTable();
            System.Xml.XmlNamespaceManager mgr = new System.Xml.XmlNamespaceManager(nt);
            IRemoteComponent parsed = this.GetParsedParent(owner);
            IDictionary<string, string> parsedNamespaces = null;

            //add the namespaces of the last parsed document so we can infer any declarations
            if (null != parsed)
            {
                parsedNamespaces = parsed.GetDeclaredNamespaces();
                if (null != parsedNamespaces)
                {
                    foreach (string prefix in parsedNamespaces.Keys)
                    {
                        mgr.AddNamespace(prefix, parsedNamespaces[prefix]);
                    }
                }
            }

            
            return mgr;
        }

        protected IRemoteComponent GetParsedParent(IComponent component)
        {
            if (component is IRemoteComponent)
            {
                IRemoteComponent remote = (IRemoteComponent)component;
                return remote;
            }

            if (null != component.Parent)
                return GetParsedParent(component.Parent);
            else
                return null;
        }

    }

    [PDFParsableComponent("Object-Param")]
    public class ObjectItemValue : DocumentItemValue
    {
        public object Value
        {
            get;
            set;
        }


        [PDFAttribute("type")]
        public string ObjectType
        {
            get;
            set;
        }

        public ObjectItemValue() : base(typeof(Object))
        { }

        protected override object DoGetNativeValue(string key, string qsValue, IComponent comp)
        {
            return this.Value;
        }

        protected override void DoSetNativeValueFromString(string value, IComponent owner)
        {
            throw new NotSupportedException();
        }

        protected override void DoSetNativeValue(object value, IComponent owner)
        {
            if (!string.IsNullOrEmpty(this.ObjectType))
            {
                var t = Type.GetType(this.ObjectType);
                if (null == t)
                    throw new NullReferenceException("The object type '" + this.ObjectType + "' could not be found");

                if (null == value)
                    this.Value = null;
                else if (t.IsAssignableFrom(value.GetType()))
                    this.Value = value;
                else
                    throw new InvalidCastException("The value cannot be assigned to the parameter as the types are not compatible");
            }
            else
                this.Value = value;
        }

    }


}
