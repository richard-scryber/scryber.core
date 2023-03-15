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

namespace Scryber.PDF
{
    /// <summary>
    /// A collection of string based names against artefact entries. Only one entry per name can exist. Rendered as a tree in the PDF
    /// </summary>
    public class PDFCategorisedNameTree
    {
        /// <summary>
        /// NOT CURRENTLY USED - The maximum number of entires in each leaf list before being broken into multiple lists with an over arching branch list
        /// </summary>
        private const int TreeBreakPoint = 1000;


        /// <summary>
        /// The Type name of this collection in the names dictionary
        /// </summary>
        public string NameType
        {
            get;
            private set;
        }


        protected SortedList<string, IArtefactEntry> InnerEntries
        {
            get;
            private set;
        }

        public IArtefactEntry this[string name]
        {
            get { return this.InnerEntries[name]; }
        }



        public PDFCategorisedNameTree(string nametype)
        {
            this.NameType = nametype;
            this.InnerEntries = new SortedList<string, IArtefactEntry>();
        }

        /// <summary>
        /// Sets an artefact in the collection (sorted based on the name), and pushes it onto this collections current stack
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entry"></param>
        public object Push(string name, IArtefactEntry entry)
        {
            this.InnerEntries[name] = entry;
            return entry;
        }

        public void Pop(string name, IArtefactEntry entry)
        {
            
        }

        public Native.PDFObjectRef OutputToPDF(PDFWriter writer, PDFRenderContext context)
        {
            List<ICategorisedArtefactNamesEntry> entries = new List<ICategorisedArtefactNamesEntry>();
            
            Native.PDFObjectRef oref = null;
            if (this.InnerEntries.Count == 0)
                    return oref;
            if (this.NameType == PDFCategorisedNameDictionary.DestinationsName)
            {
                oref = writer.BeginObject();
                writer.BeginDictionary();
                writer.BeginDictionaryEntry("Names");


                string first = null;
                string last = null;
                writer.BeginArray();
                foreach (KeyValuePair<string, IArtefactEntry> kvp in this.InnerEntries)
                {
                    if (string.IsNullOrEmpty(first))
                        first = kvp.Key;
                    else
                        last = kvp.Key;

                    writer.BeginArrayEntry();
                    writer.WriteStringLiteral(kvp.Key);
                    writer.EndArrayEntry();

                    writer.BeginArrayEntry();
                    ((PDFDestination)kvp.Value).OutputToPDF(context, writer);
                    writer.EndArrayEntry();
                    writer.WriteLine();
                }

                writer.EndArray();
                writer.EndDictionaryEntry();

                if (!string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(last))
                {
                    writer.BeginDictionaryEntry("Limits");
                    writer.WriteArrayStringEntries(true, first, last);
                    writer.EndDictionaryEntry();
                    
                }

                writer.EndDictionary();
                writer.EndObject();
            }
            else //we contain ICategorizedArtefactNameEntry(s) 
            {
                oref = writer.BeginObject();
                writer.BeginDictionary();
                writer.BeginDictionaryEntry("Names");


                //string first = null;
                //string last = null;

                writer.BeginArray();
                foreach (KeyValuePair<string, IArtefactEntry> kvp in this.InnerEntries)
                {
                    //if (string.IsNullOrEmpty(first))
                    //    first = kvp.Key;
                    //else
                    //    last = kvp.Key;

                    writer.BeginArrayEntry();
                    writer.WriteStringLiteral(kvp.Key);
                    writer.EndArrayEntry();

                    writer.BeginArrayEntry();
                    ICategorisedArtefactNamesEntry entry = (ICategorisedArtefactNamesEntry)kvp.Value;

                    Native.PDFObjectRef entryOref = entry.OutputToPDF(context, writer);
                    if (null != entryOref)
                        writer.WriteObjectRef(entryOref);

                    writer.EndArrayEntry();
                    writer.WriteLine();
                }

                writer.EndArray();
                writer.EndDictionaryEntry();

                //Limits only required on Intermediate and Leaf nodes of a tree, not the root
                //if (!string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(last))
                //{
                //    writer.BeginDictionaryEntry("Limits");
                //    writer.WriteArrayStringEntries(true, first, last);
                //    writer.EndDictionaryEntry();

                //}

                writer.EndDictionary();
                writer.EndObject();
            }
            
            return oref;
        }
    }
}
