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
using System.Threading.Tasks;
using Scryber.Native;

namespace Scryber
{
    /// <summary>
    /// Overrides the default NameDictionary to hold a dictionary of Name Tree collections to support multiple categories of entries.
    /// </summary>
    public class PDFCategorisedNameDictionary : IArtefactCollection
    {
        /// <summary>
        /// Destinations were the original NameTree and do not currently support the ICategorisedArtefactNamesEntry
        /// </summary>
        public const string DestinationsName = "Dests";


        //
        // properties
        //

        #region protected Dictionary<string, PDFCategorisedNameTree> InnerDictionary {get;}

        /// <summary>
        /// The dictionary of name trees
        /// </summary>
        private Dictionary<string, PDFCategorisedNameTree> _dictionary;

        protected Dictionary<string, PDFCategorisedNameTree> InnerDictionary
        {
            get { return _dictionary; }
        }

        #endregion

        #region public string CollectionName {get;}

        private string _type;

        /// <summary>
        /// Gets the name of this collection in the catalog - in this instance Names
        /// </summary>
        public string CollectionName
        {
            get { return _type; }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFCategorisedNameDictionary()

        /// <summary>
        /// Create a new PDFCategorisedNameDictionary
        /// </summary>
        public PDFCategorisedNameDictionary(string type)
            : base()
        {
            _dictionary = new Dictionary<string, PDFCategorisedNameTree>();
            _type = type;
        }

        #endregion

        //
        // public methods
        //

        #region object IArtefactCollection.Register(IArtefactEntry entry)

        /// <summary>
        /// Registers and begins an artefact in the name dictionary
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        object IArtefactCollection.Register(IArtefactEntry entry)
        {
            PDFCategorisedNameTree col;
            object returnValue;

            if (entry is ICategorisedArtefactNamesEntry)
            {
                ICategorisedArtefactNamesEntry catEntry = (ICategorisedArtefactNamesEntry)entry;
                if (_dictionary.TryGetValue(catEntry.NamesCategory, out col) == false)
                {
                    col = CreateTreeForCategory(catEntry.NamesCategory);
                    _dictionary.Add(catEntry.NamesCategory, col);
                }
                returnValue = col.Push(catEntry.FullName, catEntry);
            }
            //else if (entry is PDFDestination) //Default is Destination as this was generated before the Attachments or any other ICategorizedArtefactNameEntry implementation
            //{
            //    PDFDestination dest = (PDFDestination)entry;
            //    if (_dictionary.TryGetValue(DestinationsName, out col) == false)
            //    {
            //        col = CreateTreeForCategory(DestinationsName);
            //        _dictionary.Add(DestinationsName, col);
            //    }
            //    returnValue = col.Push(dest.FullName, dest);
            //}
            else
                throw new PDFException(Errors.UnknownArtefactForNamesDictionary);

            return returnValue;
        }

        #endregion

        #region public void Close(object registration)

        /// <summary>
        /// Closes a previous artefact registration popping it off the stack of it's name tree collection
        /// </summary>
        /// <param name="registration"></param>
        public void Close(object registration)
        {
            PDFCategorisedNameTree col;

            if (registration is ICategorisedArtefactNamesEntry)
            {
                ICategorisedArtefactNamesEntry catEntry = (ICategorisedArtefactNamesEntry)registration;
                if (_dictionary.TryGetValue(catEntry.NamesCategory, out col))
                {
                    col.Pop(catEntry.FullName, catEntry);
                }
                else
                    throw new PDFException(catEntry.NamesCategory + " Names dictionary entry has not previously been registered");
            }
            else if (registration is PDFDestination) //Default is Destination as this was generated before the support for other name trees
            {
                PDFDestination dest = (PDFDestination)registration;
                if (_dictionary.TryGetValue(DestinationsName, out col))
                {
                    col.Pop(dest.FullName, dest);
                }
                else
                    throw new PDFException("Destinations Names dictionary entry has not previously been registered");
            }
            else
                throw new PDFException("Unknown Artefact entry for the Names dictionary");
        }

        #endregion


        public PDFObjectRef[] OutputContentsToPDF(PDFRenderContext context, PDFWriter writer)
        {
            List<PDFObjectRef> refs = new List<PDFObjectRef>();

            foreach (KeyValuePair<string, PDFCategorisedNameTree> kvp in this.InnerDictionary)
            {
                PDFObjectRef oref = kvp.Value.OutputToPDF(writer, context);
                if (null != oref)
                    refs.Add(oref);
            }
            return refs.ToArray();
        }

        #region public PDFObjectRef OutputToPDF(PDFWriter writer, PDFRenderContext context)

        /// <summary>
        /// Outputs the entire contents of this name dictionary to the specified writer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFObjectRef names = null;
            if (this.InnerDictionary.Count > 0)
            {
                names = writer.BeginObject();
                writer.BeginDictionary();

                foreach (KeyValuePair<string,PDFCategorisedNameTree> kvp in this.InnerDictionary)
                {
                    PDFObjectRef oref = kvp.Value.OutputToPDF(writer, context);
                    if (null != oref)
                        writer.WriteDictionaryObjectRefEntry(kvp.Key, oref);
                }

                writer.EndDictionary();
                writer.EndObject();
            }
            return names;
        }

        #endregion 

        //
        // implementation methods
        //

        #region protected virtual PDFCategorisedNameTree CreateTreeForCategory(string category)

        /// <summary>
        /// Creates a new PDFCategorisedNameTree for the category name
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        protected virtual PDFCategorisedNameTree CreateTreeForCategory(string category)
        {
            return new PDFCategorisedNameTree(category);
        }

        #endregion

        #region protected virtual bool TryGetExistingNameTree(string category, out PDFCategorisedNameTree tree)

        /// <summary>
        /// Attempts to retrieve an existing name tree collection in this dictionary
        /// </summary>
        /// <param name="category"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        protected virtual bool TryGetExistingNameTree(string category, out PDFCategorisedNameTree tree)
        {
            return this.InnerDictionary.TryGetValue(category, out tree);
        }

        #endregion
    }

    
}
