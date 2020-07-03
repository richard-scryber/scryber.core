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
namespace Scryber.Components
{

    /// <summary>
    /// Defines a simple item value that can be 
    /// declared at the top of the document level
    /// </summary>
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public abstract class PDFDocumentItemValue : IKeyValueProvider
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


        protected PDFDocumentItemValue(Type nativeType)
        {
            this.ItemType = nativeType;
            this.QueryStringOverrides = false;
        }

        public object GetNativeValue(string key, IPDFComponent comp)
        { 
            return this.DoGetNativeValue(key, null, comp);
        }

        protected abstract object DoGetNativeValue(string key, string qsValue, IPDFComponent comp);
        
        
        public void Init()
        {
            
        }

        public void SetValue(string key, string value, IPDFComponent owner)
        {
            this.DoSetNativeValueFromString(value);
        }

        protected abstract void DoSetNativeValueFromString(string value);

    }

    [PDFParsableComponent("Int-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFIntegerItemValue : PDFDocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Number", Bindable = false, JSOptions = "")]
        public int Value { get; set; }

        public PDFIntegerItemValue()
            : base(typeof(int))
        {
            this.Value = 0;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
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
        protected override void DoSetNativeValueFromString(string value)
        {
            int val;
            if (int.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = 0;
        }
    }


    [PDFParsableComponent("String-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFStringItemValue : PDFDocumentItemValue
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

        public PDFStringItemValue()
            : base(typeof(string))
        {
            this.Value = string.Empty;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            if (!string.IsNullOrEmpty(qsValue))
                return qsValue;
            else
                return this.Value;
        }
        protected override void DoSetNativeValueFromString(string value)
        {
            this.Value = value;
        }
    }

    [PDFParsableComponent("Guid-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFGuidItemValue : PDFDocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "String", Bindable = false, JSOptions = "")]
        public Guid Value { get; set; }

        public PDFGuidItemValue()
            : base(typeof(Guid))
        {
            this.Value = Guid.Empty;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            Guid val;
            if (string.IsNullOrEmpty(qsValue) || !Guid.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            Guid val;
            if (Guid.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = Guid.Empty;
        }
    }


    [PDFParsableComponent("Double-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFDoubleItemValue : PDFDocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Number", Bindable = false, JSOptions = "")]
        public double Value { get; set; }

        public PDFDoubleItemValue()
            : base(typeof(double))
        {
            this.Value = 0.0;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            double val;
            if (string.IsNullOrEmpty(qsValue) || !double.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            double val;
            if (double.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = 0;
        }
    }

    [PDFParsableComponent("Bool-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFBooleanItemValue : PDFDocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Boolean", Bindable = false, JSOptions = "")]
        public bool Value { get; set; }

        public PDFBooleanItemValue()
            : base(typeof(bool))
        {
            this.Value = false;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            bool val;
            if (string.IsNullOrEmpty(qsValue) || !bool.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            bool val;
            if (bool.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = false;
        }
    }

    [PDFParsableComponent("Date-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFDateItemValue : PDFDocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Date", Bindable = false, JSOptions = "")]
        public DateTime Value { get; set; }

        public PDFDateItemValue()
            : base(typeof(int))
        {
            this.Value = DateTime.Now;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            DateTime val;
            if (string.IsNullOrEmpty(qsValue) || !DateTime.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            DateTime val;
            if (DateTime.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = DateTime.MinValue;
        }
    }


    [PDFParsableComponent("Unit-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFUnitItemValue : PDFDocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "PDFUnit", Bindable = false, JSOptions = "")]
        public PDFUnit Value { get; set; }

        public PDFUnitItemValue()
            : base(typeof(PDFUnit))
        {
            this.Value = PDFUnit.Empty;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            PDFUnit val;
            if (string.IsNullOrEmpty(qsValue) || !PDFUnit.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            PDFUnit val;
            if (PDFUnit.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = PDFUnit.Empty;
        }
    }



    [PDFParsableComponent("Color-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFColorItemValue : PDFDocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "PDFColor", Bindable = false, JSOptions = "")]
        public PDFColor Value { get; set; }

        public PDFColorItemValue()
            : base(typeof(PDFColor))
        {
            this.Value = null;
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            PDFColor val;
            if (string.IsNullOrEmpty(qsValue) || !PDFColor.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            PDFColor val;
            if (PDFColor.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = PDFColor.Transparent;
        }
    }


    [PDFParsableComponent("Thickness-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFThicknessItemValue : PDFDocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Thickness", Bindable = false, JSOptions = "")]
        public PDFThickness Value { get; set; }

        public PDFThicknessItemValue()
            : base(typeof(PDFColor))
        {
            this.Value = PDFThickness.Empty();
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            PDFThickness val;
            if (string.IsNullOrEmpty(qsValue) || !PDFThickness.TryParse(qsValue, out val))
                val = this.Value;
            return val;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            PDFThickness val;
            if (PDFThickness.TryParse(value, out val))
                this.Value = val;
            else
                this.Value = PDFThickness.Empty();
        }
    }

    [PDFParsableComponent("Enum-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFEnumItemValue : PDFDocumentItemValue
    {
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "Data", Priority = 3, Type = "Select", Bindable = false, JSOptions = "")]
        public string EnumValue { get; set; }

        [PDFAttribute("type")]
        public string EnumType { get; set; }

        public PDFEnumItemValue()
            : base(typeof(Enum))
        {

        }

        
        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
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

        protected override void DoSetNativeValueFromString(string value)
        {
            this.EnumValue = value;
        }

    }

    [PDFParsableComponent("Xml-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFXmlItemValue : PDFDocumentItemValue
    {
        [PDFElement()]
        public System.Xml.XmlNode XmlData { get; set; }

        public PDFXmlItemValue()
            : base(typeof(System.Xml.XmlNode))
        {
        }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            return this.XmlData;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            throw new NotSupportedException();
        }

    }


    [PDFParsableComponent("Template-Param")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_documentParam")]
    public class PDFTemplateItemValue : PDFDocumentItemValue
    {
        [PDFElement()]
        [PDFTemplate()]
        public IPDFTemplate Template
        {
            get;
            set;
        }

        public PDFTemplateItemValue()
            : base(typeof(IPDFTemplate))
        {
        }

        protected override object DoGetNativeValue(string key, string sqValue, IPDFComponent comp)
        {
            return this.Template;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            throw new NotSupportedException();
        }

    }

    [PDFParsableComponent("Object-Param")]
    public class PDFObjectItemValue : PDFDocumentItemValue
    {
        public object Value
        {
            get;
            set;
        }


        public PDFObjectItemValue() : base(typeof(Object))
        { }

        protected override object DoGetNativeValue(string key, string qsValue, IPDFComponent comp)
        {
            return this.Value;
        }

        protected override void DoSetNativeValueFromString(string value)
        {
            throw new NotSupportedException();
        }


    }


}
