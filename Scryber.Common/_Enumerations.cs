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

namespace Scryber
{
    /// <summary>
    /// Available trace levels with which to output messages
    /// </summary>
    /// <remarks>
    /// Each trace message can specify one of the following levels of severity. The levels are cascading with the <see cref="TraceRecordLevel"/>.
    /// If recording messages, then trace entries with the levels of Message, Warning, Error and Failure should also be recorded.
    /// If recording 'Errors', then only trace entries of Error and Failure should be recorded.
    /// </remarks>
    public enum TraceLevel
    {
        /// <summary>
        /// Identifies a complete and utter failure of the stystem that nothing can be done about to recover from.
        /// </summary>
        Failure = 20,

        /// <summary>
        /// Identfies that an error has occurred that can and should be external handled by the calling code.
        /// </summary>
        Error = 10,

        /// <summary>
        /// Identifies an invalid state of some object or component, but the system can continue as normal.
        /// </summary>
        Warning = 5,

        /// <summary>
        /// A message identifies no error or warning state, but may be of use in tracking program operation
        /// </summary>
        Message = 3,

        /// <summary>
        /// A verbose entry is recorded for extra information that can track the progress of document processing.
        /// </summary>
        Verbose = 2,

        /// <summary>
        /// A diagnostic message that should not normally be recorded, unless trying to diagnose a specific issue.
        /// </summary>
        Debug = 1,
    }

    /// <summary>
    /// Defines each of the levels for recording messages - the lower the number, the more information is provided.
    /// </summary>
    public enum TraceRecordLevel
    {
        Diagnostic = 0,
        Verbose = 2,
        Messages = 3,
        Warnings = 5,
        Errors = 10,
        Off = 100
    }

    /// <summary>
    /// Defines each of the performance monitor categories
    /// </summary>
    public enum PerformanceMonitorType : int
    {
        Parse_Files = 0,
        Document_Init_Stage = 1,
        Document_Load_Stage = 2,
        Document_Bind_Stage = 3,
        Document_Layout_Stage = 4,
        Document_Render_Stage = 5,
        Parse_Templates = 6,
        Layout_Pages = 7,
        Push_Component_Layout = 8,
        Content_Overflow = 9,
        Text_Layout = 10,
        Text_Measure = 11,
        Expression_Build = 12,
        Style_Build = 13,
        Table_Build_Process = 14,
        Image_Load = 15,
        Font_Load = 16,
        Licensing_Check = 17,
        Data_Load = 18,
        Encrypting_Streams = 19,
        Count = 20
    }

    /// <summary>
    /// The conformance mode of the parser - Lax will record errors, but not halt the flow, 
    /// Strict will always raise an execption if an unknow enitity is read.
    /// </summary>
    public enum ParserConformanceMode
    {
        Strict,
        Lax
    }

    /// <summary>
    /// The document will be compressed if set to Compress
    /// </summary>
    public enum OutputCompression
    {
        None = 0,
        Compress = 1
    }

    /// <summary>
    /// The type of compression to use - more granular than the OutputCompression options
    /// </summary>
    public enum OutputCompressionType
    {
        None = 0,
        FlateDecode = 1
    }

    

    public enum ImageCompressionType
    {
        None = 0,
        WebOptimize = 1
    }


    /// <summary>
    /// Varies how the textual data of a pdf file is output
    /// </summary>
    public enum OutputStringType
    {
        Text,
        Hex
    }

    /// <summary>
    /// Where the origin of drawing starts from (0,0) = Origin.
    /// </summary>
    public enum DrawingOrigin
    {
        /// <summary>
        /// 'Normal layout mode.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Drawing is bottom to top
        /// </summary>
        BottomLeft
    }

    public enum HorizontalAlignment
    {
        Left,
        Right,
        Center,
        Justified
    }

    public enum VerticalAlignment
    {
        Top,
        Middle,
        Bottom,
        Baseline,

    }

    public enum TextFormat
    {
        Plain,
        XML,
        XHTML
    }

    /// <summary>
    /// Defines (in abbreviated form) the reading or text layout direction
    /// </summary>
    public enum TextDirection
    {
        /// <summary>
        /// Left to Right - Western
        /// </summary>
        LTR,

        /// <summary>
        /// Top to Bottom - Mandarin, Catonese, Japanese etc. Not supported
        /// </summary>
        [Obsolete("Not currently supported",false)]
        TTB,

        /// <summary>
        /// Right to Left - Urdu, Hebrew etc.
        /// </summary>
        RTL,

        /// <summary>
        /// Right to Left excluding consecutive decimal digits 0-9 (.,) - Arabic
        /// </summary>
        [Obsolete("Not currently supported", false)]
        RTLND

    }

    public enum FontEncoding
    {
        MacRomanEncoding,
        WinAnsiEncoding,
        PDFDocEncoding,
        UnicodeEncoding
    }

    public enum PatternType
    {
        TilingPattern = 1,
        ShadingPattern = 2
    }

    public enum PatternPaintType
    {
        ColoredTile = 1,
        UncolouredTile = 2
    }

    public enum PatternTilingType
    {
        ConstantSpacing = 1,
        NoDistortion = 2,
        ConstantFast = 3
    }

    public enum ShadingType
    {
        Function = 1,
        Axial = 2,
        Radial = 3,
        FreeForm = 4,
        Lattice = 5,
        Coons = 6,
        Tensor = 7
    }

    /// <summary>
    /// Defines the method the document was loaded
    /// </summary>
    public enum ParserLoadType
    {
        ReflectiveParser,
        Generation,
        WebBuildProvider,
        None
    }

    /// <summary>
    /// The action the Parser should take if a referenced file is missing / inaccessible
    /// </summary>
    public enum ParserReferenceMissingAction
    {
        RaiseException,
        LogError,
        DoNothing
    }

    /// <summary>
    /// Defines the name dictionary content of the generated pdf.
    /// </summary>
    /// <remarks>All will output an entry in the name dictionary for each rendered component - regardless of whether the name value is set.
    /// Explict will only generate a value for a component with a name set, or referenced from another component.</remarks>
    public enum ComponentNameOutput
    {
        All,
        ExplicitOnly
    }

    /// <summary>
    /// Defines the output Compliance type of the PDF document
    /// </summary>
    /// <remarks>Will be extended to include the PDFA PDFX/1/2/3 etc Compliance modes for a PDF document</remarks>
    public enum OutputCompliance
    {
        None,
        Other
    }



    /// <summary>
    /// All the available numbering styles for page numbers
    /// </summary>
    public enum PageNumberStyle
    {
        None,
        Decimals,
        UppercaseRoman,
        LowercaseRoman,
        UppercaseLetters,
        LowercaseLetters
    }

    public enum PageRotationAngles
    {
        Normal = 0,
        RotateRight = 90,
        UpsideDown = 180,
        RotateLeft = 270
    }

    /// <summary>
    /// Specifies the action to take when updating existing pages in a document.
    /// </summary>
    public enum ModifiedContentAction
    {
        Replace,
        OnTop,
        Underneath
    }

    /// <summary>
    /// Specifies the type of modification to make to existing content
    /// </summary>
    public enum ModificationType
    {
        None,
        Update,
        Insert,
        Delete,
        Append
    }


    /// <summary>
    /// The available stages in document generation.
    /// </summary>
    public enum DocumentGenerationStage
    {
        None = 0,
        Initialized = 1,
        Loaded = 2,
        Bound = 3,
        LayingOut = 4,
        Laidout = 5,
        Written = 6,
        Disposed = 7
    }

    /// <summary>
    /// The discreet list of known data types
    /// </summary>
    public enum DataType
    {
        String,
        Integer,
        Double,
        DateTime,
        Boolean,
        Guid,
        Text,
        Html,
        User,
        UserGroup,
        Lookup,
        Image,
        BinaryFile,
        Url,
        Array,
        Choice,
        Unknown,
        Custom
    }

    public enum ParseSourceType
    {
        /// <summary>
        /// Defines that the parser should treat the source as a full document from a file residing on the local file-system
        /// </summary>
        LocalFile,

        /// <summary>
        /// Defines a full document that has come from an embedded resource
        /// </summary>
        Resource,

        /// <summary>
        /// Defines that the parser should trat the source as a full document from a remore resource (e.g. Uri)
        /// </summary>
        RemoteFile,

        /// <summary>
        /// Defines that the parser should treat the content as a full document, built dymanically from code.
        /// </summary>
        DynamicContent,

        /// <summary>
        /// Defines that the parser should treat the content as a (optionally repeatable) fragment, not a whole document.
        /// </summary>
        Template,

        /// <summary>
        /// Does not define how the parser should treat the content, could be a document, fragment, or just some text. Not recommended for internally known parsers.
        /// </summary>
        Other
    }
    
    
    /// <summary>
    /// Defines the output stage in document processing a log entry should be written. Can be combined into multiple stages.
    /// </summary>
    [Flags]
    public enum LogEntryStages
    {
        /// <summary>
        /// Will never be written
        /// </summary>
        None = 0,
        /// <summary>
        /// Written at loading time
        /// </summary>
        Loading = 1,
        /// <summary>
        /// Written at the tie of data binding
        /// </summary>
        Binding = 2,
        /// <summary>
        /// Written at the time of document layout. All components should have their final value
        /// </summary>
        Layout = 4,
        /// <summary>
        /// Written at the time the document layout content is actually written to a stream of file.
        /// </summary>
        Rendering = 8,
        Any = 15
    }

    public static class EnumParser
    {

        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object parsed)
        {
#if NETSTANDARD2_0

            string found;
            if (ignoreCase)
            {
                found = Enum.GetNames(enumType).FirstOrDefault((val) => { return string.Equals(val, value, StringComparison.OrdinalIgnoreCase) ? true : false; });
                if (string.IsNullOrEmpty(found))
                {
                    parsed = null;
                    return false;
                }
                else
                {
                    parsed = Enum.Parse(enumType, value, ignoreCase);
                    return true;
                }
            }
            else
            {
                found = Enum.GetNames(enumType).FirstOrDefault((val) => { return string.Equals(val, value, StringComparison.Ordinal) ? true : false; });
                if (string.IsNullOrEmpty(found))
                {
                    parsed = null;
                    return false;
                }
                else
                {
                    parsed = Enum.Parse(enumType, value, ignoreCase);
                    return true;
                }
            }
#else
            return Enum.TryParse(enumType, value, ignoreCase, out parsed);
#endif
            }
    }

}
