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
using Scryber.Components;
using Scryber.Native;

namespace Scryber
{
    /// <summary>
    /// A document artefact that holds the unique names of all the components and destinations within the final rendered PDF Document
    /// </summary>
    [Obsolete("Superceded but the PDFCategorisedNameDictionary as that holds more than just destination names", true)]
    public class PDFNameDictionary : IArtefactCollection
    {

        #region ivars

        private string _name;
        private Dictionary<string, PDFDestination> _dests = new Dictionary<string,PDFDestination>();

        #endregion

        #region public string CollectionName {get;}

        /// <summary>
        /// Gets the name of this collection
        /// </summary>
        public string CollectionName
        {
            get { return _name; }
        }

        #endregion

        //
        // ctor
        //

        #region public PDFNameDictionary(string name)

        /// <summary>
        /// Creates a new Name dictionary
        /// </summary>
        /// <param name="name"></param>
        public PDFNameDictionary(string name)
        {
            _name = name;
        }

        #endregion

        //
        // methods
        //

        #region object IArtefactCollection.Register(IArtefactEntry entry)

        /// <summary>
        /// Implmentation of the IArtefactCollection Register method, to ensure that all names are included in the final PDF docucment. Calls RegisterDestination
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        object IArtefactCollection.Register(IArtefactEntry entry)
        {
            if(entry is ICategorisedArtefactNamesEntry)
            {
                ICategorisedArtefactNamesEntry catNameEntry = (ICategorisedArtefactNamesEntry)entry;

            }
            PDFDestination dest = (PDFDestination)entry;
            return this.RegisterDestination(dest);
        }

        #endregion

        #region internal protected virtual PDFDestination RegisterDestination(PDFDestination dest)

        /// <summary>
        /// Registers the actual destination and returns the destination
        /// </summary>
        /// <param name="dest"></param>
        /// <returns></returns>
        internal protected virtual PDFDestination RegisterDestination(PDFDestination dest)
        {
            if (null == dest)
                throw new ArgumentNullException("dest");

            _dests[dest.FullName] = dest;
            return dest;
        }

        #endregion

        #region void IArtefactCollection.Close(object result)

        /// <summary>
        /// Implmentation of the IArtefactCollection Close method.
        /// </summary>
        /// <param name="result"></param>
        void IArtefactCollection.Close(object result)
        {
            PDFDestination dest = result as PDFDestination;
            this.Close(dest);
        }

        #endregion

        #region protected virtual void Close(PDFDestination dest)

        /// <summary>
        /// Performs any required closure 
        /// </summary>
        /// <param name="dest"></param>
        protected virtual void Close(PDFDestination dest)
        {
            //Do nothing
        }

        #endregion

        #region public void Clear()

        /// <summary>
        /// Clears all the destinations in this name dictionary
        /// </summary>
        public void Clear()
        {
            _dests.Clear();
        }

        #endregion


        public PDFObjectRef[] OutputContentsToPDF(PDFRenderContext context, PDFWriter writer)
        {
            throw new NotSupportedException();
        }

        #region public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Outputs all the names within this dictionary to a new PDFObject in output writer and returns a reference to the dictionary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFObjectRef names = writer.BeginObject();
            writer.BeginDictionary();

            if (_dests.Count > 0)
            {
                List<string> all = new List<string>(_dests.Keys);

                all.Sort();
                PDFObjectRef dests = WriteDestinationNames(context, writer, all);

                writer.WriteDictionaryObjectRefEntry("Dests", dests);
            }
            writer.EndDictionary();

            writer.EndObject();

            return names;
        }


        #endregion

        #region private PDFObjectRef WriteDestinationNames(PDFRenderContext context, PDFWriter writer, IEnumerable<string> all)

        /// <summary>
        /// Writes all the destinations to the current PDF Object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        private PDFObjectRef WriteDestinationNames(PDFRenderContext context, PDFWriter writer, IEnumerable<string> all)
        {
            PDFObjectRef dests = writer.BeginObject();
            writer.BeginDictionary();

            //Write the names array
            writer.BeginDictionaryEntry("Names");
            writer.BeginArray();

            string firstname = string.Empty;
            string lastname = string.Empty;

            foreach (string name in all)
            {

                if (WriteDestination(context, writer, name))
                {
                    if (string.IsNullOrEmpty(firstname))
                        firstname = name;
                    lastname = name;
                }
            }
            writer.EndArray();
            writer.EndDictionaryEntry();

            //Write limits
            writer.BeginDictionaryEntry("Limits");
            writer.WriteArrayStringEntries(firstname, lastname);
            writer.EndDictionaryEntry();

            writer.EndDictionary();
            writer.EndObject();
            return dests;
        }

        #endregion

        #region internal bool WriteDestination(PDFRenderContext context, PDFWriter writer, string name)

        /// <summary>
        /// Writes a single destination
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal bool WriteDestination(PDFRenderContext context, PDFWriter writer, string name)
        {
            PDFDestination dest = _dests[name];

            writer.BeginArrayEntry();
            writer.WriteStringLiteral(name);
            writer.EndArrayEntry();

            writer.BeginArrayEntry();
            dest.OutputToPDF(context, writer);
            writer.EndArrayEntry();
            writer.WriteLine();
            return true;
        }

        #endregion
    }
}
