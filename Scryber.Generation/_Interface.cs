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
using Scryber.Native;
using Scryber.Generation;

namespace Scryber
{

    #region public interface IPDFParser

    /// <summary>
    /// A parser that can read a stream to generate a PDFComponent
    /// </summary>
    public interface IPDFParser
    {

        /// <summary>
        /// Gets or sets the root component for the parser. 
        /// If set before parsing then this instance will be used as any event handler. 
        /// If not set, then the parser will set it with the top level parsed component
        /// </summary>
        object RootComponent { get; set; }

        /// <summary>
        /// Parses the specified stream using the resolver to load any referenced files and returns the PDFComponent representation
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="source"></param>
        /// <param name="istemplate">If the source to be parsed is a template (not a physical file) set this to true</param>
        /// <returns></returns>
        IPDFComponent Parse(string source, System.IO.Stream stream, ParseSourceType type);

        /// <summary>
        /// Parses the specified stream using the resolver to load any referenced files and returns the PDFComponent representation
        /// </summary>
        /// <param name="source"></param>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IPDFComponent Parse(string source, System.IO.TextReader reader, ParseSourceType type);

        /// <summary>
        /// Parses the specified stream using the resolver to load any referenced files and returns the PDFComponent representation
        /// </summary>
        /// <param name="source"></param>
        /// <param name="reaser"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IPDFComponent Parse(string source, System.Xml.XmlReader reaser, ParseSourceType type);
    }

    #endregion

    /// <summary>
    /// Interface for the parser factory
    /// </summary>
    public interface IPDFParserFactory
    {
        IPDFParser CreateParser(PDFGeneratorSettings settings);
    }

    /// <summary>
    /// Interface all template generators should implement
    /// </summary>
    public interface IPDFTemplateGenerator
    {
        void InitTemplate(string xmlContent, System.Xml.XmlNamespaceManager namespaces);
    }

    /// <summary>
    /// Interface for a text literal component that will be used by the parse when it encounters general textual content
    /// </summary>
    public interface IPDFTextLiteral : IPDFComponent
    {
        string Text { get; set; }
        TextFormat ReaderFormat { get; set; }
    }

    
}
