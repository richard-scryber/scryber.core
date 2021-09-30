using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.PDF
{
    public class PDFAcrobatFormButtonWidget : PDFAcrobatFormFieldWidget
    {

        public PDFAcrobatFormButtonWidget(string name)
            : base(name, string.Empty, string.Empty, FormInputFieldType.Button, FormFieldOptions.None)
        {

        }
    }
}
