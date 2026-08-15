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

        /// <summary>
        /// The URL a submit-behaviour field within this form should submit to. A generic PDF
        /// forms concept (not HTML-specific) - PDFSubmitFormAction doesn't care whether the form
        /// came from HTML markup or native XML, so this lives on the base Form rather than HTMLForm.
        /// </summary>
        [PDFAttribute("action")]
        public string Action { get; set; }

        /// <summary>
        /// "get" or "post" (matching HTML's own form method values) - "post" (the HTML default)
        /// if not set. Only "get" is checked for specifically; anything else submits as post.
        /// </summary>
        [PDFAttribute("method")]
        public string Method { get; set; }

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
