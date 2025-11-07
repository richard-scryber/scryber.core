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
using Scryber.PDF;
using Scryber.Options;

namespace Scryber
{

    

    /// <summary>
    /// Adds the factory method for the parsers configuration, that returns a dictionary of all the supported mime-types and their associated factories
    /// </summary>
    public static class ParserOptionExtensions
    {

        public static ParserFactoryDictionary GetParserFactories(this ParsingOptions options)
        {
            
            var all = new ParserFactoryDictionary();
            var standard = GetStandardFactories();

            if (null != options && null != options.Parsers && options.Parsers.Length > 0)
            {
                foreach (var configFactory in options.Parsers)
                {
                    var instance = configFactory.GetFactory();
                    if (null == instance)
                        throw new NullReferenceException("The configured factory instance for " + configFactory.Name + " was null");

                    var mimes = instance.SupportedTypes;
                    if (null == mimes || mimes.Length == 0)
                        throw new NullReferenceException("The configured factory for '" + configFactory.Name + "' returned null or an empty array for the SupportedTypes");

                    foreach (var mime in mimes)
                    {
                        if (!all.ContainsKey(mime))
                            all.Add(mime, instance);
                    }

                }
            }

            foreach (var instance in standard)
            {
                var mimes = instance.SupportedTypes;
                foreach (var mime in mimes)
                {
                    if (!all.ContainsKey(mime))
                        all.Add(mime, instance);
                }
            }

            return all;
        }


        private static IEnumerable<IParserFactory> GetStandardFactories()
        {
            return _standards;
        }

        private static IParserFactory[] _standards = new IParserFactory[] {

            new Scryber.Generation.PDFXMLReflectionParserFactory(),
            new Scryber.Html.Parsing.HTMLParserFactory(),
            new Scryber.Generation.PlainTextParserFactory()
        };


        

    }
}
