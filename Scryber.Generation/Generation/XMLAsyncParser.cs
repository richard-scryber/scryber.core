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

#define PROCESS_HANDLEBAR_HELPERS

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Data.Common;

using Scryber.Logging;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Scryber.Generation
{
    public class XMLAsyncParser : XMLParser, IComponentParserAsync
    {
        

        //
        // ctor
        //

        #region public XMLAsyncParser(PDFGeneratorSettings settings)

        /// <summary>
        /// Instantiates a new PDFXMLParser that will use the provided PDFGeneratorSettings as rules for parsing content
        /// </summary>
        /// <param name="settings"></param>
        public XMLAsyncParser(ParserSettings settings) : base(settings)
        {
            if(settings.ResolverAsync == null)
                throw new ArgumentNullException("settings.ResolverAsync");
        }

        #endregion

        //
        // public interface
        //

        
        #region public Task<IPDFComponent> ParseAsync(string source, Stream stream, bool istemplate) + 2 overloads

        /// <summary>
        /// Parses a stream (with the source value set to an appropriate value)
        /// </summary>
        /// <param name="source">An identifier (usually a file path or unique id) that can be recognised further into the execution by any reference resolver</param>
        /// <param name="stream">The stream of data to parse</param>
        /// <param name="istemplate">True if this stream is a template content (rather than a complete standalone file)</param>
        /// <returns>The parsed component</returns>
        public async Task<IComponent> ParseAsync(string source, Stream stream, ParseSourceType type)
        {
            using (XmlReader reader = CreateXmlReader(stream))
            {
                return await ParseAsync(source, reader, type);
            }
        }

        /// <summary>
        /// Parses a text reader (with the source value set to an appropriate value)
        /// </summary>
        /// <param name="source">An identifier (usually a file path or unique id) that can be recognised further into the execution by any reference resolver</param>
        /// <param name="reader">The TextReader of information to parse</param>
        /// <param name="istemplate">True if this stream is a template content (rather than a complete standalone file)</param>
        /// <returns>The parsed component</returns>
        public async Task<IComponent> ParseAsync(string source, TextReader reader, ParseSourceType type)
        {
            using (XmlReader xreader = CreateXmlReader(reader))
            {
                return await this.ParseAsync(source, xreader, type);
            }
        }

        /// <summary>
        /// Parses an XML reader (with the source value set to an appropriate value)
        /// </summary>
        /// <param name="source">An identifier (usually a file path or unique id) that can be recognised further into the execution by any reference resolver</param>
        /// <param name="stream">The stream of data to parse</param>
        /// <param name="istemplate">True if this stream is a template content (rather than a complete standalone file)</param>
        /// <returns></returns>
        /// <returns></returns>
        public async Task<IComponent> ParseAsync(string source, XmlReader reader, ParseSourceType type)
        {
            try
            {
                return await DoParseAsync(source, reader, type);
            }
            catch(Exception ex)
            {
                if (string.IsNullOrEmpty(source))
                {
                    source = "Dynamic";
                }
                throw new PDFParserException(string.Format(Errors.CouldNotParseSource, source, ex.Message), ex);
            }
        }

        #endregion

    }
}
