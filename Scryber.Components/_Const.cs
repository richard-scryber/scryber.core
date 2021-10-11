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
using Scryber.Drawing;

namespace Scryber
{
    public class Const
    {
        public const string ArrayStringStart = "[";
        public const string ArrayStringEnd = "]";
        public const string ArrayStringSeparator = ", ";
        public const char ArraySplitChar = ',';

        /// <summary>
        /// When making a circle out of bezier paths, this factor determines the arc shape
        /// </summary>
        public const double GraphicsCircularityFactor = 0.552285;

        /// <summary>
        /// Unit calculation - the number of PDF page units in a point
        /// </summary>
        public const double PageUnitsPerPoint = 1.0;

        /// <summary>
        /// Unit calculation - the number of PDF Page units in an inch
        /// </summary>
        public const double PageUnitsPerInch = 72.0 * PageUnitsPerPoint; //72

        /// <summary>
        /// Unit calculation - the number of PDF Page units in a millimetre
        /// </summary>
        public const double PageUnitsPerMM = PageUnitsPerInch / 25.4; //2.83

        /// <summary>
        /// The prefix used to define an event handler reference
        /// </summary>
        public const string DefaultParserEventPrefix = "on-";

        public static readonly Unit DefaultListNumberInset = 30;
        public static readonly Unit DefaultDefinitionListInset = 100;
        public const HorizontalAlignment DefaultListNumberAlignment = HorizontalAlignment.Right;

        /// <summary>
        /// The prefix for a referencing a component by ID rather than name
        /// </summary>
        public const string ComponentIDPrefix = "#";

        /// <summary>
        /// The separator used when joining ids together
        /// </summary>
        public const string UniqueIDSeparator = "_";
        

        public static readonly string ScryberProducer = "Scryber PDF Generator " + Utilities.FrameworkHelper.CurrentVersion;
        public const string PDFMimeType = "application/pdf";

        public const int StreamEncoding = 1252;

        /// <summary>
        /// All the characters that should be escaped
        /// </summary>
        public static readonly char[] EscapeChars = new char[] { '\\', '(', ')' };

        /// <summary>
        /// String representations of the escape characters
        /// </summary>
        public static readonly string[] EscapeStrings = new string[] { "\\", "(", ")" };

        /// <summary>
        /// String representations of the escaped versions of the characters to replace
        /// Matches both count and order of the EscapeStrings
        /// </summary>
        public static readonly string[] ReplaceStrings = new string[] { "\\\\", "\\(", "\\)" };

        public const string DefaultBulletFont = "Symbol";

        /// <summary>
        /// The name of the processing instructions that relate to scryber itself
        /// </summary>
        public const string ScryberProcessingInstructions = "scryber";

        public const string RootPseudoClass = ":root";

        //
        // namespace constants
        //


        public const string PDFDataNamespace = "Scryber.Data, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe";
        public const string PDFComponentNamespace = "Scryber.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe";
        public const string PDFStylesNamespace = Scryber.Styles.Style.PDFStylesNamespace;
        

        //
        // PDF Name constants
        //

        internal const string PageTreeName = "PDFPageTree";


        //
        // Document output formats
        //

        public const string DocFormat_AnyFormatName = "Any";
        public const string DocFormat_NoneFormatname = "None";

        //
        // Public key for licensing components
        //

        public const string PublicScryberLicenceKey = "<DSAKeyValue>" + 
                                                       "<P>6QtXI2JHZqHx+IiA7w6I7S+fss0l1d/dWrnVYGPyVbjZxoYAX9B4vTNNtPtF7Ft7WfhOW91q1HaHClvzftbdeOHN7C6Lb8/vh07FHmbjQphdJoMTuQeiCa+04gD1EI+lRgBW8eB/9wBiCKlqf07mteLE9AotcWZcsj0RHeGb73U=</P>" + 
                                                       "<Q>2FTPYMiP0o4k6UabE7vnU9dzIxs=</Q>" + 
                                                       "<G>LBGmWb4RBh+Zg8aYm/9JMKctRGYC/uI72nBjDV7/rX1vaUvi9XxvTHPssrh+cWmk+4iINnlMsWtjb8OBBL94ihxOqSChcki894DJoIAh7KgL6qo5FOHW1L3K0CMWJ+AyL7znsNNxwoTHeSugbi1+6jHvgv+Pa63HNNbnW9Z8LoQ=</G>" + 
                                                       "<Y>O9ZjjUa2E0qicGlXXp8sPJLmPH+KBYnm7Wgu78BgovFGcIpb8CsdDoBEjJBl7pNPuMOZQgAVf5oyrwWtwJ4udNH7hLkoz3El9qexYQMrWQoEPBj/DpWyHphcr8J9Wj2GcIEmqcD+ShOvDUqdZATcwBCkyLhEFsOueYZKVIQBr2w=</Y>" + 
                                                       "<J>AAAAARPHFdLxbP4v78GUf/wBO8ILK/TIO29u+npNTeEEq7lsoBBCj1xdIQyHnSouWWGdQPxR7504FNkSgQuuf6Hp3RU6cXwvPpFlRhye+gPo8UkxIi7cyNOLNm6HJvYVt4BdCdYPCJ4J2hxYOnVRnA==</J>" +
                                                       "<Seed>KXBEQO2ArdSX2kTKtC+LICAdS6s=</Seed>" + 
                                                       "<PgenCounter>HQ==</PgenCounter>" + 
                                                      "</DSAKeyValue>";

        public static Dictionary<DataType, Type> TypeMappings;


        static Const()
        {
            TypeMappings = new Dictionary<DataType, Type>();
            TypeMappings[DataType.Image] = typeof(byte[]);
            TypeMappings[DataType.Boolean] = typeof(bool);
            TypeMappings[DataType.DateTime] = typeof(DateTime);
            TypeMappings[DataType.Double] = typeof(double);
            TypeMappings[DataType.Guid] = typeof(Guid);
            TypeMappings[DataType.Html] = typeof(string);
            TypeMappings[DataType.Integer] = typeof(int);
            TypeMappings[DataType.Lookup] = typeof(string);
            TypeMappings[DataType.String] = typeof(string);
            TypeMappings[DataType.Text] = typeof(string);
            TypeMappings[DataType.Unknown] = typeof(object);
            TypeMappings[DataType.Url] = typeof(Uri);
            TypeMappings[DataType.User] = typeof(string);
            TypeMappings[DataType.UserGroup] = typeof(string);
            
        }

        
    }
}
