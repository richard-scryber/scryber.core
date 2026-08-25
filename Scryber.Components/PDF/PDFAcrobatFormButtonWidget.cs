using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    public class PDFAcrobatFormButtonWidget : PDFAcrobatFormFieldWidget
    {

        public PDFAcrobatFormButtonWidget(string name)
            : base(name, string.Empty, string.Empty, FormInputFieldType.Button, FormFieldOptions.None)
        {
        }

        protected override IEnumerable<PDFObjectRef> DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            var xObjectNormal = this._states.ContainsKey(FormFieldAppearanceState.Normal) 
                ? this._states[FormFieldAppearanceState.Normal] : null;
            var xObjectOver = this._states.ContainsKey(FormFieldAppearanceState.Over)
                ? this._states[FormFieldAppearanceState.Over] : xObjectNormal;
            var xObjectDown = this._states.ContainsKey(FormFieldAppearanceState.Down)
                ? this._states[FormFieldAppearanceState.Down] : xObjectNormal;

            if (null == xObjectNormal)
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new InvalidOperationException(
                        "The button does not have a normal appearance. Cannot render");

                context.TraceLog.Add(TraceLevel.Error, "Button", "The button does not have a normal appearance defined. The stated dictionary must at least have a normal appearance.");
                return null;
            }
            return base.DoOutputToPDF(context, writer);
        }
    }
}
