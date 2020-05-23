using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("TextField")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withField")]
    public class PDFWithTextField : PDFWithBoundField
    {
        private string _value;

        [PDFAttribute("value")]
        [PDFDesignable("Value", Category = "Data", Priority = 5, Type = "String")]
        public string Value {
            get { return this._value; }
            set { this._value = value; }
        }



        private string _autobindValue;

        public PDFWithTextField():
            this((PDFObjectType)"WtTf")
        {

        }

        public PDFWithTextField(PDFObjectType type) : base(type)
        {

        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);
        }

        public override PDFVisualComponent DoBuildItemField(PDFContextBase context)
        {
            PDFField field = new PDFField();
            field.Value = (null == this.Value) ? null : this.Value.ToString();
            return field;
        }

        public override void SetDataSourceBindingItem(PDFDataItem item, PDFDataContext context)
        {
            this._autobindValue = item.RelativePath;
            this.DataBinding += PDFWithTextField_DataBinding;
            base.SetDataSourceBindingItem(item, context);
        }

        private void PDFWithTextField_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            object val = AssertGetDataItemValue(this._autobindValue, e);
            if (null != val)
                this.Value = val.ToString();
        }

        

    }

    [PDFParsableComponent("DateField")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withField")]
    public class PDFWithDateField : PDFWithBoundField
    {
        private DateTime _value;

        [PDFAttribute("value")]
        [PDFDesignable("Value", Category = "Data", Priority = 5, Type = "Date")]
        public DateTime Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        [PDFAttribute("date-format", Scryber.Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Date Format", Category = "General", Priority = 5, Type = "DateFormat")]
        public string DateFormat
        {
            get { return this.Style.Text.DateFormat; }
            set { this.Style.Text.DateFormat = value; }
        }

        private string _autobindValue;

        public PDFWithDateField() :
            this((PDFObjectType)"WtDf")
        {

        }

        public PDFWithDateField(PDFObjectType type) : base(type)
        {

        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);
        }

        public override PDFVisualComponent DoBuildItemField(PDFContextBase context)
        {
            PDFDate field = new PDFDate();

            field.Value = this.Value;
            field.DateFormat = this.DateFormat;
            return field;
        }

        public override void SetDataSourceBindingItem(PDFDataItem item, PDFDataContext context)
        {
            this._autobindValue = item.RelativePath;
            this.DataBinding += PDFWithTextField_DataBinding;
            base.SetDataSourceBindingItem(item, context);
        }

        private void PDFWithTextField_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            object val = AssertGetDataItemValue(this._autobindValue, e);
            if (null != val)
            {
                if (val is string)
                {
                    this.Value = DateTime.Parse("yyyy-MM-dd'T'hh:mm:ss");
                }
                else
                {
                    this.Value = (DateTime)val;
                }
                
            }
        }



    }

    [PDFParsableComponent("NumberField")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withField")]
    public class PDFWithNumberField : PDFWithBoundField
    {
        private double _value;

        [PDFAttribute("value")]
        [PDFDesignable("Value", Category = "Data", Priority = 5, Type = "Date")]
        public double Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        [PDFAttribute("number-format", Scryber.Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Number Format", Category = "General", Priority = 5, Type = "NumberFormat")]
        public string NumberFormat
        {
            get { return this.Style.Text.NumberFormat; }
            set { this.Style.Text.NumberFormat = value; }
        }

        private string _autobindValue;

        public PDFWithNumberField() :
            this((PDFObjectType)"WtNf")
        {

        }

        public PDFWithNumberField(PDFObjectType type) : base(type)
        {

        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);
        }

        public override PDFVisualComponent DoBuildItemField(PDFContextBase context)
        {
            PDFNumber field = new PDFNumber();

            field.Value = this.Value;
            field.NumberFormat = this.NumberFormat;
            return field;
        }

        public override void SetDataSourceBindingItem(PDFDataItem item, PDFDataContext context)
        {
            this._autobindValue = item.RelativePath;
            this.DataBinding += PDFWithTextField_DataBinding;
            base.SetDataSourceBindingItem(item, context);
        }

        private void PDFWithTextField_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            object val = AssertGetDataItemValue(this._autobindValue, e);
            if (null != val)
            {
                this.Value = Convert.ToDouble(val);
            }
        }
        
    }

    [PDFParsableComponent("BoolField")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withField")]
    public class PDFWithBooleanField : PDFWithBoundField
    {
        private bool _value;

        [PDFAttribute("value")]
        [PDFDesignable("Value", Category = "Data", Priority = 5, Type = "Date")]
        public bool Value
        {
            get { return this._value; }
            set { this._value = value; }
        }


        private string _autobindValue;

        public PDFWithBooleanField() :
            this((PDFObjectType)"WtNf")
        {

        }

        public PDFWithBooleanField(PDFObjectType type) : base(type)
        {

        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);
        }

        public override PDFVisualComponent DoBuildItemField(PDFContextBase context)
        {
            PDFField field = new PDFField();
            field.Value = this.Value.ToString();
            
            return field;
        }

        public override void SetDataSourceBindingItem(PDFDataItem item, PDFDataContext context)
        {
            this._autobindValue = item.RelativePath;
            this.DataBinding += PDFWithTextField_DataBinding;
            base.SetDataSourceBindingItem(item, context);
        }

        private void PDFWithTextField_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            object val = AssertGetDataItemValue(this._autobindValue, e);
            if (null != val)
            {
                this.Value = Convert.ToBoolean(val);
            }
        }



    }

    [PDFParsableComponent("UrlField")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withField")]
    public class PDFWithUrlField : PDFWithBoundField
    {
        private string _value;

        [PDFAttribute("url")]
        [PDFDesignable("Link Url", Category = "Data", Priority = 5, Type = "String")]
        public string LinkUrl
        {
            get { return this._value; }
            set { this._value = value; }
        }

        private string _text;

        [PDFAttribute("text")]
        [PDFDesignable("Link Text", Category = "Data", Priority = 5, Type = "String")]
        public string LinkText
        {
            get { return this._text; }
            set { this._text = value; }
        }


        private string _autobindValue;

        public PDFWithUrlField() :
            this((PDFObjectType)"WtUf")
        {

        }

        public PDFWithUrlField(PDFObjectType type) : base(type)
        {
            
        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);
        }

        public override PDFVisualComponent DoBuildItemField(PDFContextBase context)
        {
            PDFLink link = new PDFLink();
            link.Action = LinkAction.Uri;
            link.File = this.LinkUrl;
            PDFLabel label = new PDFLabel();
            label.Text = string.IsNullOrEmpty(this.LinkText) ? this.LinkUrl : this.LinkText;
            link.Contents.Add(label);

            return link;
        }

        public override void SetDataSourceBindingItem(PDFDataItem item, PDFDataContext context)
        {
            this._autobindValue = item.RelativePath;
            this.DataBinding += PDFWithTextField_DataBinding;
            base.SetDataSourceBindingItem(item, context);
        }

        private void PDFWithTextField_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            object val = AssertGetDataItemValue(this._autobindValue, e);
            if (null != val)
            {
                this.LinkUrl = Convert.ToString(val);
            }
        }



    }

    [PDFParsableComponent("ImageField")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withField")]
    public class PDFWithImageField : PDFWithBoundField
    {
        private string _value;

        [PDFAttribute("url")]
        [PDFDesignable("Source Url", Category = "Data", Priority = 5, Type = "Date")]
        public string SourceUrl
        {
            get { return this._value; }
            set { this._value = value; }
        }


        private string _autobindValue;

        public PDFWithImageField() :
            this((PDFObjectType)"WtIf")
        {

        }

        public PDFWithImageField(PDFObjectType type) : base(type)
        {

        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);
        }

        public override PDFVisualComponent DoBuildItemField(PDFContextBase context)
        {
            PDFImage link = new PDFImage();
            link.Source = this.SourceUrl;

            return link;
        }

        public override void SetDataSourceBindingItem(PDFDataItem item, PDFDataContext context)
        {
            this._autobindValue = item.RelativePath;
            this.DataBinding += PDFWithTextField_DataBinding;
            base.SetDataSourceBindingItem(item, context);
        }

        private void PDFWithTextField_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            object val = AssertGetDataItemValue(this._autobindValue, e);
            if (null != val)
            {
                this.SourceUrl = Convert.ToString(val);
            }
        }



    }

    [PDFParsableComponent("HtmlField")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withField")]
    public class PDFWithHtmlField : PDFWithBoundField
    {
        private string _value;

        [PDFElement()]
        [PDFAttribute("html")]
        [PDFDesignable("Html Contents", Category = "Data", Priority = 5, Type = "String")]
        public string HtmlContent
        {
            get { return this._value; }
            set { this._value = value; }
        }

        [PDFAttribute("format")]
        [PDFDesignable("Html Format", Category = "General", Priority = 5)]
        public Html.HtmlFormatType Format { get; set; }


        private string _autobindValue;

        public PDFWithHtmlField() :
            this((PDFObjectType)"WtHf")
        {

        }

        public PDFWithHtmlField(PDFObjectType type) : base(type)
        {

        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);
        }

        public override PDFVisualComponent DoBuildItemField(PDFContextBase context)
        {
            Scryber.Components.PDFHtmlFragment fragment = new Components.PDFHtmlFragment();
            fragment.RawContents = this.HtmlContent;
            fragment.Format = this.Format;
            return fragment;
        }

        public override void SetDataSourceBindingItem(PDFDataItem item, PDFDataContext context)
        {
            this._autobindValue = item.RelativePath;
            this.DataBinding += PDFWithTextField_DataBinding;
            base.SetDataSourceBindingItem(item, context);
        }

        private void PDFWithTextField_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            object val = AssertGetDataItemValue(this._autobindValue, e);
            if (null != val)
            {
                this.HtmlContent = Convert.ToString(val);
            }
        }



    }
}
