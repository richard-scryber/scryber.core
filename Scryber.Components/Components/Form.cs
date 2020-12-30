using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

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

        public Form() : this(PDFObjectTypes.Form)
        {

        }

        protected Form(PDFObjectType type)
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
            base.DoRegisterArtefacts(context, set, fullstyle);
            
            //object entry = context.DocumentLayout.RegisterCatalogEntry(context, PDFArtefactTypes.AcrobatForms, new PDFAcrobatFormEntry(this.ID));
            //this.FormEntry = (PDFAcrobatFormEntry)entry;
        }

        protected override void DoCloseLayoutArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet artefacts, Style fullstyle)
        {
            //artefacts.Document.CloseArtefactEntry(PDFArtefactTypes.AcrobatForms, this.FormEntry);
            //base.DoCloseLayoutArtefacts(context, artefacts, fullstyle);
        }
    }
}
