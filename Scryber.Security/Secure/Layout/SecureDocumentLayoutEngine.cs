using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Secure.Components;

namespace Scryber.Secure.Layout
{
    public class SecureDocumentLayoutEngine : Scryber.Layout.LayoutEngineDocument
    {

        public SecureDocumentLayoutEngine(PDFSecureDocument doc, IPDFLayoutEngine parent, PDFLayoutContext context)
            : base(doc, parent, context)
        {
        }

        protected override void LayoutAllPages()
        {
            bool showbadge = this.ValidateSecureDocumentLicense();

            foreach (PDFPage page in this.Document.Pages)
            {


                if (showbadge)
                    page.SetShowBadge();

                if (page.Visible)
                    this.LayoutPage(page);
            }
        }

        private bool ValidateSecureDocumentLicense()
        {
            Type licensedType = typeof(PDFSecureDocument);
            string msg;
            IPDFLicense lic = Licensing.GetLicense(licensedType);
            PDFLicenseAction action = lic.Validate(out msg);

            if (action == PDFLicenseAction.None)
                return false;
            else
            {
                if ((action & PDFLicenseAction.ExcludeFromOutput) > 0)
                    throw new InvalidOperationException("Excluding from output is not appropriate for a document component");

                if ((action & PDFLicenseAction.LogMessage) > 0)
                {
                    PDFTraceLog log = this.Context.TraceLog;
                    log.Add(TraceLevel.Message, "Licensing", "No valid license for component " + licensedType.Name + " was found. " + msg);
                }
                if ((action & PDFLicenseAction.RaiseException) > 0)
                    throw new PDFLicenceException(licensedType, msg);

                if ((action & PDFLicenseAction.ShowBadge) > 0)
                    return true;

                //No action defined
                return false;
            }

        }
    }

}
