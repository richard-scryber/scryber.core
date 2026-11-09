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
using System.Text;
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    public class PDFAnnotationCollection : IArtefactCollection
    {
        private List<PDFAnnotationEntry> _annots = new List<PDFAnnotationEntry>();
        private string _name;

        string IArtefactCollection.CollectionName
        {
            get { return _name; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int Count
        {
            get { return _annots.Count; }
        }

        public PDFAnnotationEntry this[int index]
        {
            get { return this._annots[index]; }
        }


        internal PDFAnnotationCollection(string catalogname)
        {
            _name = catalogname;
        }

        object IArtefactCollection.Register(IArtefactEntry catalogobject)
        {
            if (null == catalogobject)
                throw RecordAndRaise.ArgumentNull("catalogobject");

            if (!(catalogobject is PDFAnnotationEntry))
                throw RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, catalogobject.GetType(), typeof(PDFAnnotationEntry));
            
            _annots.Add((PDFAnnotationEntry)catalogobject);
            return catalogobject;
        }

        void IArtefactCollection.Close(object registration)
        {
           
        }

        public PDFObjectRef[] OutputContentsToPDF(PDFRenderContext context, PDFWriter writer)
        {
            List<PDFObjectRef> entries = new List<PDFObjectRef>();
            foreach(PDFAnnotationEntry entry in this._annots)
            {
                var orefs = entry.OutputToPDF(context, writer);
                
                if (orefs != null)
                {
                    foreach (var oref in orefs)
                    {
                        entries.Add(oref);
                    }
                }
            }

            return entries.ToArray();
        }

        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Verbose, "Annotation Collection", "Outputting the doument annotations to the writer");
            
            PDFObjectRef annot = writer.BeginObject();
            List<PDFObjectRef> entries = new List<PDFObjectRef>();
            //TODO:Render annotations
            foreach (PDFAnnotationEntry entry in this._annots)
            {
                var orefs = entry.OutputToPDF(context, writer);
                
                if (orefs != null)
                {
                    foreach (var oref in orefs)
                    {
                        entries.Add(oref);
                    }
                }
            }

            writer.WriteArrayRefEntries(entries.ToArray());
            writer.EndObject();

            if (context.ShouldLogDebug)
                context.TraceLog.End(TraceLevel.Verbose, "Annotation Collection", "Completed the output of " + entries.Count + " annotations on the writer");
            

            return annot;
        }
    }
}
