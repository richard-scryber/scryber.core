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
using Scryber.PDF.Native;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Resources;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.Components
{
    /// <summary>
    /// The document that wraps an original document and appends the PDFCollectorTraceLog and other info
    /// </summary>
    internal class TraceLogDocument : Scryber.Components.Document
    {
        private PDFFile _file;
        /// <summary>
        /// Gets or sets the Source file this Trace Log Document appends to.
        /// </summary>
        public PDFFile OriginalSourceFile
        {
            get { return _file; }
            set { _file = value; }
        }
        
        private PDFDocumentGenerationData _data;

        internal PDFDocumentGenerationData GenerationData
        {
            get { return _data; }
            set { _data = value; }
        }

        internal PDFResourceCollection OwnerResources
        {
            get; private set;
        }

        internal TraceLogDocument(string name, PDFFile original, PDFDocumentGenerationData data, PDFResourceCollection resources)
            : base()
        {
            this.OriginalSourceFile = original;
            this.FileName = name;
            this.GenerationData = data;
            this.OwnerResources = resources;
            this.Info = data.DocumentInfo;
            this.ViewPreferences = data.DocumentViewerPrefs;
        }

        protected override void DoInit(InitContext context)
        {
            context.TraceLog = new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Off);
            
            this.InitStyles(context);
            this.InitContent(context);
            base.DoInit(context);
        }

        protected virtual void InitStyles(InitContext context)
        {
            PDFTraceLogStyles stylesDoc = new PDFTraceLogStyles();
            this.Styles.Add(stylesDoc);
            stylesDoc.Init(context);
        }

        private void InitContent(InitContext context)
        {
            Section logsect = new PDFTraceLogSection() { GenerationData = this.GenerationData, OwnerResources = this.OwnerResources };
            this.Pages.Add(logsect);

            return;
        }

        public override IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDF.PDFLayoutContext context)
        {
            return new PDFTraceLogLayoutEngineDocument(this, this.OriginalSourceFile, parent, context);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != this.OriginalSourceFile)
                    this.OriginalSourceFile.Dispose();
                this.OriginalSourceFile = null;
            }
            base.Dispose(disposing);
        }
    }
}
