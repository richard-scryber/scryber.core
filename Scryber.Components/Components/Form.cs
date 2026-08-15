using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber.Components
{
    [PDFParsableComponent("Form")]
    public class Form : Panel
    {

        [PDFElement()]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        protected PDFAcrobatFormEntry FormEntry { get; set; }

        public Form() : this(ObjectTypes.Form)
        {

        }

        protected Form(ObjectType type)
            : base(type)
        {

        }


        public bool RegisterField(IPDFFormField field, PDFLayoutContext context)
        {
            if (null == this.FormEntry)
                throw new PDFLayoutException("This forms artefact has not been registered");

            return this.FormEntry.RegisterField(field, context);

        }

        protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Style fullstyle)
        {
            var name = string.IsNullOrEmpty(this.Name) ? this.UniqueID : this.Name;
            var entry = new PDF.PDFAcrobatFormEntry(name);
            context.DocumentLayout.RegisterCatalogEntry(context, PDFArtefactTypes.AcrobatForms, entry);
            this.FormEntry = entry;

            base.DoRegisterArtefacts(context, set, fullstyle);
        }

    }
}
