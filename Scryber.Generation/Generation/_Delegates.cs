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
using System.Text.RegularExpressions;
using System.Xml;

namespace Scryber.Generation
{
    /// <summary>
    /// Delegate that can convert a string in the current XMLReader value to the requiredType 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate object PDFXmlConverter(XmlReader reader, Type requiredType, ParserSettings settings);

    /// <summary>
    /// Delegate method to replace the content of a handlebars helper expression (e.g. {{#each ..}}....{{/each}})
    /// with a formatted xml element that matches that functionality.
    /// </summary>
    public delegate string HandlebarMatchReplacer(HBarHelperSplitter splitter, Stack<Match> currentStack, Match newMatch);
}
