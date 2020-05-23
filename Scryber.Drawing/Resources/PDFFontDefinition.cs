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
using System.Xml.Serialization;
using Scryber.Drawing;
using Scryber.Native;

namespace Scryber.Resources
{
    /// <summary>
    /// A complete font definition. Also contains the Descriptor, widths, and any embeddable file data.
    /// </summary>
    /// <remarks>
    /// Font Definitions are unique to a document and instances are not shared across multiple files
    /// </remarks>
    public class PDFFontDefinition
    {

        //  
        // properties
        //

        #region public FontType SubType {get; set;}

        private FontType _subtype = FontType.Type1;

		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.Bindable(true)]
		[System.ComponentModel.DefaultValue(FontType.Type1)]
		[System.ComponentModel.Description("The FontType for the Font : TrueType, Type1 etc.")]
        public FontType SubType
        {
            get { return _subtype; }
            set 
            {
                _subtype = value;
                
            }
        }

        #endregion

        #region public string BaseType {get;set;}

        private string _base = "";

		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.Bindable(true)]
		[System.ComponentModel.DefaultValue("")]
		[System.ComponentModel.Description("The BaseType for the Font : Helvetica, Helvetica-Bold, Times New Roman")]
		public string BaseType
        {
            get { return _base; }
            set 
            { 
                _base = value;
                
            }
        }

        #endregion

        #region public FontEncoding Encoding {get;set;}

        private FontEncoding _enc = FontEncoding.WinAnsiEncoding;

		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.Bindable(true)]
		[System.ComponentModel.DefaultValue(FontEncoding.WinAnsiEncoding)]
		[System.ComponentModel.Description("The Encoding for the Font : MacRomanEncoding, Win32Encoding")]
		public FontEncoding Encoding
        {
            get { return _enc; }
            set 
            { 
                _enc = value;
                
            }
        }

        #endregion

        #region internal OpenType.SubTables.CMapEncoding TTFEncoding

        private OpenType.SubTables.CMapEncoding _ttfEnc;

        /// <summary>
        /// Gets the Open Type Character Map encoding for the underlying font.
        /// </summary>
        internal OpenType.SubTables.CMapEncoding TTFEncoding
        {
            get { return _ttfEnc; }
            private set { _ttfEnc = value; }
        }

        #endregion

        #region public PDFFontWidths Widths {get;set;}

        private PDFFontWidths _widths;

		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.Bindable(true)]
		[System.ComponentModel.DefaultValue((object)null)]
		[System.ComponentModel.Description("The character widths of the font")]
		public PDFFontWidths Widths
        {
            get { return _widths; }
            set { _widths = value; }
        }

        #endregion

        #region public virtual bool SupportsVariants {get;}

        private bool _suportsVariants = true;

        /// <summary>
        /// Gets the flag to identify if this font supports variants such as bold and italic
        /// </summary>
        public bool SupportsVariants
        {
            get { return _suportsVariants; }
            protected set { _suportsVariants = value; }
        }

        #endregion

        #region public string Family

        private string _family;

        /// <summary>
        /// Gets or sets the family name for this font
        /// </summary>
        public string Family
        {
            get { return _family; }
            set { _family = value; }
        }

        #endregion

        #region public string FilePath {get;}

        private string _filepath;
        /// <summary>
        /// Gets the full path to the file this FontDescriptor was loaded from
        /// </summary>
        public string FilePath
        {
            get { return this._filepath; }
            private set { _filepath = value; }
        }

        #endregion

        #region public string WindowsName

        private string _winName;

        /// <summary>
        /// /Gets or sets the windows name for this font. If not set, returns the standard family name
        /// </summary>
        public string WindowsName
        {
            get 
            {
                if (string.IsNullOrEmpty(_winName))
                    return this.Family;
                else
                    return _winName;
            }
            set { _winName = value; }
        }

        #endregion

        #region public bool Bold {get;set;}

        private bool _bold;

        /// <summary>
        /// Gets or sets teh Bold flag on this font definition
        /// </summary>
        public bool Bold
        {
            get { return _bold; }
            set { _bold = value; }
        }

        #endregion

        #region public bool Italic {get;set;}

        private bool _ital;

        public bool Italic
        {
            get { return _ital; }
            set { _ital = value; }
        }

        #endregion

        #region private Scryber.OpenType.TTFFile TTFFile

        /// <summary>
        /// Gets the OpenType font file associated with this descriptor
        /// </summary>
        private Scryber.OpenType.TTFFile TTFFile
        {
            get;
            set;
        }

        #endregion

        #region public bool CanMeasureStrings {get;}

        /// <summary>
        /// Returns true if this definition supports string measurement.
        /// </summary>
        /// <remarks>A definition can quickly measure strings if it has a reference to the OpenType font file</remarks>
        public bool CanMeasureStrings
        {
            get { return null != TTFFile; }
        }

        #endregion

        #region public PDFFontDescriptor Descriptor {get; set;}

        private PDFFontDescriptor _desc; 

        /// <summary>
        /// Gets or sets the PDFFontDescriptor for this font definition
        /// </summary>
        public PDFFontDescriptor Descriptor
        {
            get { return _desc; }
            set { _desc = value; }
        }

        #endregion

        #region public bool IsEmbedable {get;set;}

        private bool _embed;
        /// <summary>
        /// Gets the embedable flag
        /// </summary>
        public bool IsEmbedable
        {
            get { return _embed; }
            private set { _embed = value; }
        }

        #endregion

        #region public bool IsStandard {get;}

        /// <summary>
        /// Gets the flag that identifies if this font is a standard font (does not have a PDFFontDescriptor or PDFWidths)
        /// </summary>
        public bool IsStandard
        {
            get { return this._desc == null && this._widths == null; }
        }

        #endregion

        #region public bool IsUnicode {get;}

        /// <summary>
        /// private ca
        /// </summary>
        private bool? _isunicode;

        /// <summary>
        /// Returns true if this is an embeddable unicode font
        /// </summary>
        public bool IsUnicode
        {
            get
            {
                //if (this.Widths == null)
                //    return false;
                //else if (this.Widths.LastChar <= 255)
                //    return false;
                if (_isunicode.HasValue == false)
                    _isunicode = (this.IsStandard == false && this.TTFFile != null && this.Encoding == FontEncoding.UnicodeEncoding);
                return _isunicode.Value;
            }
        }

        #endregion

        #region public string FulName {get;}

        private string _fullname;
        /// <summary>
        /// Gets the calculated full name of the font based
        /// upon the Family name, bold and italic flag
        /// </summary>
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fullname))
                {
                    string fn = PDFFont.GetFullName(this.Family,this.Bold,this.Italic);
                    
                    return fn;
                }
                else
                    return _fullname;
            }
            set { _fullname = value; }
        }

        #endregion

        #region public string PostscriptFontName {get;}

        private const int PostscriptFontNameIndex = 6;

        /// <summary>
        /// Gets the full postscript font name
        /// </summary>
        public string PostscriptFontName
        {
            get
            {
                if (this.TTFFile != null)
                    return this.TTFFile.Tables.Names.Names[PostscriptFontNameIndex].InvariantName;
                else
                    return this.BaseType;
            }
        }

        #endregion


        //TODO: Calculate these values.

        private double _spaceWidthFU;

        public double SpaceWidthFontUnits
        {
            get { return _spaceWidthFU; }
            protected set { _spaceWidthFU = value; }
        }

        private double _unitsPerEm;

        public double FontUnitsPerEm
        {
            get { return _unitsPerEm; }
            set { _unitsPerEm = value; }
        }

        //
        // ctor
        //

        #region protected PDFFontDefinition()

        /// <summary>
        /// protected constructor - can only be created with the static factory methods
        /// </summary>
        protected PDFFontDefinition()
        {
            
        }


        #endregion

        //
        // instance methods
        //



        #region public System.Drawing.Font GetSystemFont(float pointsize)

        /// <summary>
        /// Gets the System.Drawing.Font that represents this PDFFontDefintion
        /// </summary>
        /// <param name="pointsize"></param>
        /// <returns></returns>
        public System.Drawing.Font GetSystemFont(float pointsize)
        {
            System.Drawing.FontStyle fs = System.Drawing.FontStyle.Regular;
            if (this.Bold)
                fs |= System.Drawing.FontStyle.Bold;
            if (this.Italic)
                fs |= System.Drawing.FontStyle.Italic;

            System.Drawing.Font f = new System.Drawing.Font(this.WindowsName, pointsize, fs);
            return f;
        }

        #endregion

        // object overrides

        #region public override bool Equals(object obj) + 2 overload

        /// <summary>
        /// Overrides the default implementation to return true if the object 
        /// is a font definition and its Equal to this font definition
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is PDFFontDefinition)
                return this.Equals((PDFFontDefinition)obj);
            else if (obj is PDFFont)
                return this.Equals((PDFFont)obj);
                return false;
        }

        /// <summary>
        /// Returns true if the PDFFontDefinition has the same full name as this definition
        /// </summary>
        /// <param name="defn"></param>
        /// <returns></returns>
        public bool Equals(PDFFontDefinition defn)
        {
            if (null == defn)
                return false;
            else
                return this.FullName == defn.FullName;
        }

        /// <summary>
        /// Returns true if the PDFFont has the same full name as this definition
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public bool Equals(PDFFont font)
        {
            if (null == font)
                return false;
            else
                return this.FullName == font.FullName;
        }

        #endregion

        #region public override int GetHashCode()

        /// <summary>
        /// Overrides the default behaviour to return the hash of the full name
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.FullName.GetHashCode();
        }

        #endregion

        #region public override string ToString()

        /// <summary>
        /// returns a string representation of this PDFFontDefintion (its full name)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.FullName;
        }

        #endregion

        // widths and measurement

        #region public PDFSize MeasureStringWidth(string chars, int startOffset, double fontSize, double available, bool wordBoundary, out int charsfitted)

        /// <summary>
        /// Measures the width of a string in PDFUnits (not font units) based on the provided values
        /// </summary>
        /// <param name="chars">The string to measure</param>
        /// <param name="startOffset">The offset in the string to start measuing from</param>
        /// <param name="fontSize">The size in points of the rendered glyphs</param>
        /// <param name="available">The available width in points to fit the string into.</param>
        /// <param name="wordBoundary">If true then strings will be fitted based on wordboundaries rather than characters.</param>
        /// <param name="charsfitted">Set to the number of characters of the string that were actually fitted in the width.</param>
        /// <returns>The actual size consumed by the allocated characters.</returns>
        public PDFSize MeasureStringWidth(string chars, int startOffset, double fontSize, double available, bool wordBoundary, out int charsfitted)
        {
            if (null == this.TTFFile)
                throw new NullReferenceException(string.Format(Errors.FontDefinitionDoesNotHaveFile, this.FullName));
            OpenType.SubTables.CMapEncoding encoding = AssertGetTTFEncoding();

            System.Drawing.SizeF size = this.TTFFile.MeasureString(encoding, chars, startOffset, fontSize, available, wordBoundary, out charsfitted);
            return new PDFSize(size.Width, size.Height);
        }

        #endregion

        #region public PDFSize MeasureStringWidth(string chars, int startOffset, double fontSize, double available, double wordSpace, double charSpace, double hScale, bool vertical, bool wordBoundary, out int charsfitted)

        /// <summary>
        /// Measures the width of a string in PDFUnits (not font units) based on the provided values
        /// </summary>
        /// <param name="chars">The string to measure</param>
        /// <param name="startOffset">The offset in the string to start measuing from</param>
        /// <param name="fontSize">The size in points of the rendered glyphs</param>
        /// <param name="available">The available width in points to fit the string into.</param>
        /// <param name="wordBoundary">If true then strings will be fitted based on wordboundaries rather than characters.</param>
        /// <param name="charsfitted">Set to the number of characters of the string that were actually fitted in the width.</param>
        /// <returns>The actual size consumed by the allocated characters.</returns>
        public PDFSize MeasureStringWidth(string chars, int startOffset, double fontSize, double available, double? wordSpace, double charSpace, double hScale, bool vertical, bool wordBoundary, out int charsfitted)
        {
            if (null == this.TTFFile)
                throw new NullReferenceException(string.Format(Errors.FontDefinitionDoesNotHaveFile, this.FullName));
            OpenType.SubTables.CMapEncoding encoding = AssertGetTTFEncoding();

            System.Drawing.SizeF size = this.TTFFile.MeasureString(encoding, chars, startOffset, fontSize, available, wordSpace, charSpace, hScale, vertical, wordBoundary, out charsfitted);
            return new PDFSize(size.Width, size.Height);
        }

        private OpenType.SubTables.CMapEncoding AssertGetTTFEncoding()
        {
            var enc = this.TTFEncoding;
            if (null == enc)
                throw new PDFRenderException("The font " + this.FullName + " does not have a character mapping table that can be used for string measurement");
            return enc;
        }

        #endregion

        #region public PDFFontWidths GetWidths()

        /// <summary>
        /// Returns the PDFFontWidths for this font definition
        /// </summary>
        /// <returns></returns>
        public PDFFontWidths GetWidths()
        {

            
            if (this.IsStandard)
                return new PDFEmptyFontWidths();

            else if (this.IsUnicode)
            {
                int unitsPerEm, spaceWidth;
                return GetCompositeFontWidths(this.TTFFile, out unitsPerEm, out spaceWidth);
            }
            else
                return this.Widths;
        }

        #endregion

        // Rendering


        #region public virtual PDFObjectRef RenderCompositeFont(string name, PDFUsedFontWidths used, PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// Renders the PDFFontDefinition as a composite font - complete font definition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="used"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual PDFObjectRef RenderCompositeFont(string subset, PDFFontWidths widths, PDFContextBase context, PDFWriter writer)
        {
            string fullfontname = PDFFontDescriptor.GetSubsetName(subset, this.PostscriptFontName);

            if (context.ShouldLogVerbose)
                context.TraceLog.Begin(TraceLevel.Verbose, "Font Definition", "Beginning the render of composite font definition " + fullfontname);

            PDFObjectRef font = writer.BeginObject(fullfontname);
            writer.WriteCommentLine("Font Definition for font named: " + this.FullName);
            writer.BeginDictionary();

            writer.BeginDictionaryEntry("Type");
            writer.WriteName("Font");
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("Subtype");
            writer.WriteName("Type0");
            writer.EndDictionaryEntry();



            writer.BeginDictionaryEntry("BaseFont");
            writer.WriteName(fullfontname);
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("Encoding");
            writer.WriteName("Identity-H");
            writer.EndDictionaryEntry();

            PDFObjectRef tounicode = this.WriteUnicodeFontInfo(fullfontname, widths, context, writer);
            if (null != tounicode)
            {
                writer.BeginDictionaryEntry("ToUnicode");
                writer.WriteObjectRef(tounicode);
                writer.EndDictionaryEntry();
            }

            PDFObjectRef descendant = this.WriteDescendantFontInfo(fullfontname, widths, context, writer);
            if (null != descendant)
            {
                writer.BeginDictionaryEntry("DescendantFonts");
                writer.BeginArray();
                writer.BeginArrayEntry();
                writer.WriteObjectRef(descendant);
                writer.EndArrayEntry();
                writer.EndArray();
                writer.EndDictionaryEntry();
            }
            writer.EndDictionary();
            writer.EndObject();

            if (context.ShouldLogVerbose)
                context.TraceLog.End(TraceLevel.Verbose, "Font Definition", "Completed the render of composite font definition " + fullfontname);
            else if (context.ShouldLogMessage)
                context.TraceLog.Add(TraceLevel.Message, "Font Definition", "Rendered the composite font " + fullfontname + " to the file");


            return font;
        }


        private const string suffix = "endcmap CMapName currentdict /CMap defineresource pop end end";

        private PDFObjectRef WriteUnicodeFontInfo(string fullname, PDFFontWidths widths, PDFContextBase context, PDFWriter writer)
        {
            if (widths is PDFCompositeFontWidths)
            {
                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Font Definition", "Rendering the Unicode CID map stream");

                PDFCompositeFontWidths comp = (PDFCompositeFontWidths)widths;

                PDFObjectRef tounicode = writer.BeginObject();
                writer.BeginStream(tounicode);

                //Prefix
                writer.WriteRaw("/CIDInit /ProcSet findresource begin");
                writer.WriteLine();
                writer.WriteRaw("12 dict begin");
                writer.WriteLine();
                writer.WriteRaw("begincmap");
                writer.WriteLine();
                writer.WriteRaw("/CIDSystemInfo << /Registry (TTX+0)/Ordering (T42UV)/Supplement 0>> def");
                writer.WriteLine();
                writer.WriteRaw("/CMapName /TTX+0 def /CMapType 2 def");
                writer.WriteLine();

                //we only support one code space range
                writer.WriteRaw("1 begincodespacerange");
                writer.WriteLine();
                //writer.WriteRaw("<" + comp.MinGlyphIndex.ToString("X4") + "><" + comp.MaxGlyphIndex.ToString("X4") + ">");
                writer.WriteRaw("<0000><FFFF>");
                writer.WriteLine();
                writer.WriteRaw("endcodespacerange");
                writer.WriteLine();
                writer.WriteRaw(comp.Count.ToString());
                writer.WriteRaw(" beginbfrange");
                writer.WriteLine();
                int count = 0;
                foreach (int glyphindex in comp.RegisterdGlyphOffsets)
                {
                    char ch = comp.GetCharacterForGlyphOffset(glyphindex);
                    string g = "<" + Convert.ToString(glyphindex, 16).PadLeft(4, '0') + ">";
                    string c = "<" + Convert.ToString((int)ch, 16).PadLeft(4, '0') + ">";
                    writer.WriteRaw(g);
                    writer.WriteRaw(g);
                    writer.WriteRaw(c);
                    writer.WriteLine();
                    count++;
                }
                writer.WriteRaw("endbfrange");
                writer.WriteLine();

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Font Definition", "Rendered " + count + " glyph offsets");

                //Suffix
                writer.WriteRaw("endcmap CMapName currentdict /CMap defineresource pop end end");
                long len = writer.EndStream();
                writer.BeginDictionary();
                writer.BeginDictionaryEntry("Length");
                writer.WriteNumber(len);
                writer.EndDictionaryEntry();
                writer.EndDictionary();
                writer.EndObject();

                return tounicode;
            }
            else
                return null;
        }

        private PDFObjectRef WriteDescendantFontInfo(string fullname, PDFFontWidths widths, PDFContextBase context, PDFWriter writer)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Font Definition", "Rendering the Descendant font information in Unicode font");

            PDFObjectRef font = writer.BeginObject();
            writer.WriteCommentLine("Font Definition for postscript font named: " + this.PostscriptFontName);
            writer.BeginDictionary();

            writer.BeginDictionaryEntry("Type");
            writer.WriteName("Font");
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("Subtype");
            writer.WriteName("CIDFontType2");
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("BaseFont");
            writer.WriteName(fullname);
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("CIDToGIDMap");
            writer.WriteName("Identity");
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("CIDSystemInfo");
            writer.BeginDictionary();
            writer.WriteDictionaryStringEntry("Ordering", "Identity");
            writer.WriteDictionaryStringEntry("Registry", "Adobe");
            writer.WriteDictionaryNumberEntry("Supplement", 0);
            writer.EndDictionary();
            writer.EndDictionaryEntry();

            if (null != widths)
            {
                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Font Definition", "Rendering the Glyph widths");

#if ReferenceWidths
                PDFObjectRef w = widths.RenderToPDF(context, writer);
                if(null != w)
                    writer.WriteDictionaryObjectRefEntry("W", w);
#else
                writer.BeginDictionaryEntry("W");
                widths.RenderWidthsArrayToPDF(context, writer);
                writer.EndDictionaryEntry();

                writer.BeginDictionaryEntry("DW");
                writer.WriteNumber(1000);
                writer.EndDictionaryEntry();
#endif
            }
            if (null != this.Descriptor)
            {

                PDFObjectRef desc = this.Descriptor.RenderToPDF(fullname, context, writer);
                if (null != desc)
                    writer.WriteDictionaryObjectRefEntry("FontDescriptor", desc);
            }
            writer.EndDictionary();
            writer.EndObject();
            return font;
        }

        #endregion

        #region public virtual PDFObjectRef RenderStandardFont(string name, PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// Renders a standard Ansi font which is one of the 16 known PDF Fonts.
        /// </summary>
        /// <param name="name">The resource name of the font</param>
        /// <param name="context">The current render context</param>
        /// <param name="writer">The PDFWriter to render the font to</param>
        /// <returns>Any Object reference for the PDFInderectObject rendered</returns>
        public virtual PDFObjectRef RenderStandardFont(string name, PDFContextBase context, PDFWriter writer)
        {
            return this.RenderAnsiFont(name, null, context, writer);
        }

        #endregion

        #region public virtual PDFObjectRef RenderAnsiFont(string name, PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// Renders the PDFFontDefinition onto the PDFWriter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual PDFObjectRef RenderAnsiFont(string name, PDFFontWidths widths, PDFContextBase context, PDFWriter writer)
        {
            if (context.ShouldLogVerbose)
                context.TraceLog.Begin(TraceLevel.Verbose, "Font Definition", "Rendering ANSI font '" + this.FullName + "' to the document");


            PDFObjectRef font = writer.BeginObject(name);
            writer.WriteCommentLine("Font Definition for postscript font named: " + this.PostscriptFontName);
            writer.BeginDictionary();
            writer.BeginDictionaryEntry("Type");
            writer.WriteName("Font");
            writer.EndDictionaryEntry();
            writer.BeginDictionaryEntry("Subtype");
            writer.WriteName(this.SubType.ToString());
            writer.EndDictionaryEntry();
            writer.BeginDictionaryEntry("Name");
            writer.WriteName(name);
            writer.EndDictionaryEntry();
            writer.BeginDictionaryEntry("BaseFont");
            writer.WriteName(this.BaseType);
            writer.EndDictionaryEntry();
            writer.BeginDictionaryEntry("Encoding");
            writer.WriteName(this.Encoding.ToString());
            writer.EndDictionaryEntry();

            //Render the widths
            if (widths != null && widths.IsEmpty == false)
            {
                PDFObjectRef wid = widths.RenderToPDF(context, writer);

                if (null != wid)
                {
                    writer.BeginDictionaryEntry("FirstChar");
                    writer.WriteNumber(widths.FirstChar);
                    writer.EndDictionaryEntry();
                    writer.BeginDictionaryEntry("LastChar");
                    writer.WriteNumber(widths.LastChar);
                    writer.EndDictionaryEntry();
                    writer.BeginDictionaryEntry("Widths");
                    writer.WriteObjectRef(wid);
                    writer.EndDictionaryEntry();
                }
            }

            //Render the FontDescriptor
            if (this.Descriptor != null)
            {
                PDFObjectRef desc = this.Descriptor.RenderToPDF(name, context, writer);
                if (null != desc)
                {
                    writer.BeginDictionaryEntry("FontDescriptor");
                    writer.WriteObjectRef(desc);
                    writer.EndDictionaryEntry();
                }
            }


            writer.EndDictionary();
            writer.EndObject();

            if (context.ShouldLogVerbose)
                context.TraceLog.End(TraceLevel.Verbose, "Font Definition", "Completed rendering ANSI font '" + this.FullName + "' to the document");
            else if (context.ShouldLogMessage)
                context.TraceLog.Add(TraceLevel.Message, "Font Definition", "Rendered ANSI font '" + this.FullName + "' to the document");

            return font;
        }

        #endregion


        //
        // static methods
        //

        private const int NameIDSubFamily = 2;
        private const int NameIDFamily = 1;
        private const int NameIDFullFamily = 4;

        /// <summary>
        /// Defines the font glyph unit size to one point size.
        /// </summary>
        public const int PDFGlyphUnits = 1000;


        #region internal static PDFFontDefinition LoadStandardFont(PDFFont font) + 1 overload

        /// <summary>
        /// Loads a single standard font definition (one of the base 16 fonts defined in PDF - Helvetica. Times etc).
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        internal static PDFFontDefinition LoadStandardFont(PDFFont font)
        {
            PDFFontDefinition defn;
            if (font.IsStandard)
            {
                defn = PDFFonts.GetStandardFontDefinition(font);
            }
            else
            {
                throw new InvalidOperationException("The font requested is not a standard font");
            }
            return defn;
        }

        /// <summary>
        /// Loads a single standard font definition based on the name and style (one of the base 16 fonts defined in PDF - Helvetica. Times etc).
        /// </summary>
        /// <param name="family"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static PDFFontDefinition LoadStandardFont(string family, System.Drawing.FontStyle style)
        {
            PDFFontDefinition defn;
            if (PDFFont.IsStandardFontFamily(family))
                defn = PDFFonts.GetStandardFontDefinition(family, style);
            else
                throw new InvalidOperationException("The font requested is not a standard font");
            return defn;
        }

        #endregion

        #region internal static PDFFontDefinition LoadOpenTypeFontFile(string path, string familyname, System.Drawing.FontStyle style)

        /// <summary>
        /// Loads an OpenType font file from the specified path based on the name and style
        /// </summary>
        /// <param name="path">The path to the Open Type font file</param>
        /// <param name="familyname">The font family name in the file</param>
        /// <param name="style">The style (bold, italic etc) for the font</param>
        /// <returns>The new PDFFontDefinition</returns>
        internal static PDFFontDefinition LoadOpenTypeFontFile(string path, string familyname, System.Drawing.FontStyle style, int headOffset)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            PDFFontDefinition defn = null;
            try
            {
                Scryber.OpenType.TTFFile ttf = new Scryber.OpenType.TTFFile(path, headOffset);
                defn = LoadOpenTypeFontFile(ttf, familyname, style);
                defn.FilePath = path;
            }
            catch (Scryber.PDFException) { throw; }
            catch (Exception ex)
            {
                throw new Scryber.PDFException(string.Format(Errors.CouldNotLoadTheFontFile, familyname, "See inner exeption for more details."), ex);
            }
            return defn;
        }

        #endregion

        #region internal static PDFFontDefinition LoadOpenTypeFontFile(byte[] data, string familyname, System.Drawing.FontStyle style)

        /// <summary>
        /// Loads an OpenType font from the provided binary data based on the family name and font style
        /// </summary>
        /// <param name="data">The complete binary data for the OpenType font</param>
        /// <param name="familyname">The family name (Helvetica, Arial, etc) for the font</param>
        /// <param name="style">The style (Bold, Italic, etc) for the font</param>
        /// <returns>A new PDFFontDefinition for the font</returns>
        internal static PDFFontDefinition LoadOpenTypeFontFile(byte[] data, string familyname, System.Drawing.FontStyle style, int headOffset)
        {
            if (null == data || data.Length == 0)
                throw new ArgumentNullException("data");

            PDFFontDefinition defn = null;
            try
            {
                Scryber.OpenType.TTFFile ttf = new OpenType.TTFFile(data, headOffset);
                defn = LoadOpenTypeFontFile(ttf, familyname, style);
            }
            catch (Scryber.PDFException) { throw; }
            catch (Exception ex)
            {
                throw new Scryber.PDFException(string.Format(Errors.CouldNotLoadTheFontResource, familyname, "See inner exeption for more details."), ex);
            }
            return defn;
        }

        #endregion

        #region internal static PDFFontDefinition LoadOpenTypeFontFile(Scryber.OpenType.TTFFile ttf, string familyname, System.Drawing.FontStyle style)

        /// <summary>
        /// Loads and returns a new PDFFontDefinition for the provided OpenType font with the name and style
        /// </summary>
        /// <param name="ttf">The open type font data for this font</param>
        /// <param name="familyname">The font family name</param>
        /// <param name="style">The font style</param>
        /// <returns>A new PDFFontDefinition</returns>
        internal static PDFFontDefinition LoadOpenTypeFontFile(Scryber.OpenType.TTFFile ttf, string familyname, System.Drawing.FontStyle style)
        {
            if (null == ttf)
                throw new ArgumentNullException("ttf");
            if (string.IsNullOrEmpty(familyname))
                throw new ArgumentNullException("familyname");

            PDFFontDefinition defn = null;
            try
            {

                defn = new PDFFontDefinition();
                defn.Family = familyname;
                defn.SubType = FontType.TrueType;
                defn.TTFFile = ttf;
                
                defn.BaseType = ttf.Tables.Names.GetInvariantName(NameIDFullFamily).Replace(" ", "");
                defn.Bold = (style & System.Drawing.FontStyle.Bold) > 0;
                defn.Italic = (style & System.Drawing.FontStyle.Italic) > 0;
                defn.WindowsName = defn.BaseType;
                defn.IsEmbedable = IsEmbeddable(ttf);
                defn.Descriptor = GetFontDescriptor(defn.BaseType, defn.IsEmbedable, ttf);

                Scryber.OpenType.SubTables.CMapEncoding enc = GetOptimumCmapEncoding(ttf);
                
                int spaceWidth;
                int unitsPerEm;
                defn.Widths = GetArrayFontWidths(ttf, out unitsPerEm, out spaceWidth);

                defn.FontUnitsPerEm = unitsPerEm;
                defn.SpaceWidthFontUnits = spaceWidth;
                defn.TTFEncoding = enc;
                defn.Encoding = ConvertCMapToFontEncoding(enc);
            }
            catch (Exception ex)
            {
                throw new Scryber.PDFException(string.Format(Errors.CouldNotLoadTheFontFile, ttf.Path, ex.Message), ex);
            }

            return defn;
        }

        #endregion

        // private static loading helpers

        /// <summary>
        /// The CMAP enconding types in the preferred order of loading
        /// </summary>
        private static Scryber.OpenType.SubTables.CMapEncoding[] PreferredCharacterEncodings = new OpenType.SubTables.CMapEncoding[] {
                                                                                                    OpenType.SubTables.CMapEncoding.WindowsUnicode,
                                                                                                    OpenType.SubTables.CMapEncoding.UnicodeDefault,
                                                                                                    OpenType.SubTables.CMapEncoding.Unicode_20,
                                                                                                    OpenType.SubTables.CMapEncoding.MacRoman};

        #region private static bool IsEmbeddable(Scryber.OpenType.TTFFile ttf)

        /// <summary>
        /// Returns true if the TTFFile can be embedded in the PDF Document
        /// </summary>
        /// <param name="ttf">The Open Type instance to check</param>
        /// <returns>True is this font can be embedded</returns>
        private static bool IsEmbeddable(Scryber.OpenType.TTFFile ttf)
        {
            Scryber.OpenType.SubTables.OS2Table os2 = ttf.Tables.WindowsMetrics;

            if (os2.FSType == Scryber.OpenType.SubTables.FontRestrictions.InstallableEmbedding)
                return true;
            else if ((os2.FSType & Scryber.OpenType.SubTables.FontRestrictions.NoEmbedding) > 0)
                return false;
            else if ((os2.FSType & Scryber.OpenType.SubTables.FontRestrictions.PreviewPrintEmbedding) > 0 ||
                     (os2.FSType & Scryber.OpenType.SubTables.FontRestrictions.EditableEmbedding) > 0)
                return true;
            else
                return false;

        }

        #endregion

        #region private static Scryber.OpenType.SubTables.CMapEncoding GetOptimumCmapEncoding(Scryber.OpenType.TTFFile ttf)

        /// <summary>
        /// Returns the optimum character encoding map for the open type font file
        /// </summary>
        /// <param name="ttf">The open type font to get the character mapping for</param>
        /// <returns></returns>
        private static Scryber.OpenType.SubTables.CMapEncoding GetOptimumCmapEncoding(Scryber.OpenType.TTFFile ttf)
        {
            Scryber.OpenType.SubTables.CMAPTable cmap = ttf.Tables.CMap;

            Scryber.OpenType.SubTables.CMAPSubTable chars = null;
            OpenType.SubTables.CMapEncoding foundenc = OpenType.SubTables.CMapEncoding.UnicodeDefault;

            foreach (OpenType.SubTables.CMapEncoding enc in PreferredCharacterEncodings)
            {
                chars = cmap.GetOffsetTable(enc);
                foundenc = enc;

                if (null != chars)
                    break;
            }


            if (null == chars)
                throw new NullReferenceException("The specifed font does not contain a known character mapping");
            else
                return foundenc;
        }

        #endregion

        #region private static FontEncoding ConvertCMapToFontEncoding(Scryber.OpenType.SubTables.CMapEncoding enc)

        /// <summary>
        /// Returns scryber's FontEncoding value associated with the CMAP Encoding specified.
        /// </summary>
        /// <param name="enc">The Character Map encoding</param>
        /// <returns>An appropriate encoding value</returns>
        private static FontEncoding ConvertCMapToFontEncoding(Scryber.OpenType.SubTables.CMapEncoding enc)
        {
            switch (enc.Platform)
            {
                case Scryber.OpenType.SubTables.CharacterPlatforms.Unicode:
                    return FontEncoding.UnicodeEncoding;

                case Scryber.OpenType.SubTables.CharacterPlatforms.Macintosh:
                    return FontEncoding.MacRomanEncoding;

                case Scryber.OpenType.SubTables.CharacterPlatforms.Windows:
                    switch (enc.Encoding)
                    {
                        case 1: //Unicode BMP
                            return FontEncoding.UnicodeEncoding;
                        default:
                            return FontEncoding.WinAnsiEncoding;
                    }


                case Scryber.OpenType.SubTables.CharacterPlatforms.Other:
                default:
                    return FontEncoding.PDFDocEncoding;
            }
        }

        #endregion

        #region private static PDFFontWidths GetCompositeFontWidths(Scryber.OpenType.TTFFile ttf)

        /// <summary>
        /// Creates an returns a Composite Font Widths instance for the Open Type font. 
        /// Composite Fonts Widths can describe full unicode charactersets.
        /// </summary>
        /// <param name="ttf"></param>
        /// <returns></returns>
        private static PDFFontWidths GetCompositeFontWidths(Scryber.OpenType.TTFFile ttf, out int unitsPerEm, out int spaceWidth)
        {
            Scryber.OpenType.SubTables.FontHeader head = ttf.Tables.FontHeader;
            Scryber.OpenType.SubTables.CMAPTable cmap = ttf.Tables.CMap;

            
            Scryber.OpenType.SubTables.CMAPSubTable chars = null;
            OpenType.SubTables.CMapEncoding foundenc = OpenType.SubTables.CMapEncoding.UnicodeDefault;
            foreach (OpenType.SubTables.CMapEncoding enc in PreferredCharacterEncodings)
            {
                chars = cmap.GetOffsetTable(enc);
                foundenc = enc;

                if (null != chars)
                    break;
            }


            if (null == chars)
                throw new NullReferenceException("The specifed font does not contain a known character mapping");

            unitsPerEm = head.UnitsPerEm;
            Scryber.OpenType.SubTables.HorizontalMetrics hmtc = ttf.Tables.HorizontalMetrics;

            PDFCompositeFontWidths comp = new PDFCompositeFontWidths(foundenc, chars, hmtc, unitsPerEm, PDFGlyphUnits);
            
            spaceWidth = comp.GetGlyphWidthForCharacter(' ');

            return comp;
        }

        #endregion

        #region private static PDFFontWidths GetArrayFontWidths(Scryber.OpenType.TTFFile ttf)

        /// <summary>
        /// Creates and returns an Array font widths that can only support the standard ANSII character set.
        /// </summary>
        /// <param name="ttf"></param>
        /// <returns></returns>
        private static PDFFontWidths GetArrayFontWidths(Scryber.OpenType.TTFFile ttf, out int unitsPerEm, out int spaceWidth)
        {
            Scryber.OpenType.SubTables.FontHeader head = ttf.Tables.FontHeader;
            Scryber.OpenType.SubTables.CMAPTable cmap = ttf.Tables.CMap;

            Scryber.OpenType.SubTables.CMAPSubTable chars = null;
            OpenType.SubTables.CMapEncoding foundenc = OpenType.SubTables.CMapEncoding.UnicodeDefault;
            foreach (OpenType.SubTables.CMapEncoding enc in PreferredCharacterEncodings)
            {
                chars = cmap.GetOffsetTable(enc);
                foundenc = enc;

                if (null != chars)
                    break;
            }
            
            
            if (null == chars)
                throw new NullReferenceException("The specifed font does not contain a known character mapping");

            unitsPerEm = head.UnitsPerEm;
            spaceWidth = 0;

            List<int> all = new List<int>();
            Scryber.OpenType.SubTables.HorizontalMetrics hmtc = ttf.Tables.HorizontalMetrics;
            int first = 0;
            int last = 255;
            for (int i = first; i <= last; i++)
            {
                int moffset = (int)chars.GetCharacterGlyphOffset((char)i);
                //System.Diagnostics.Debug.WriteLine("Character '" + chars[i].ToString() + "' (" + ((byte)chars[i]).ToString() + ") has offset '" + moffset.ToString() + "' in mac encoding and '" + woffset + "' in windows encoding");

                if (moffset >= hmtc.HMetrics.Count)
                    moffset = hmtc.HMetrics.Count - 1;
                Scryber.OpenType.SubTables.HMetric metric = hmtc.HMetrics[moffset];

                int value = metric.AdvanceWidth; // - metric.LeftSideBearing;

                if (i == 32) //Space Width
                    spaceWidth = value;

                value = (value * PDFGlyphUnits) / unitsPerEm;
                all.Add(value);
            }

            PDFFontWidths widths = new PDFArrayFontWidths(first, last, all, foundenc); //chars, hmtc
            return widths;

        }

        #endregion

        #region private static PDFFontDescriptor GetFontDescriptor(string name, bool embed, Scryber.OpenType.TTFFile ttf)

        /// <summary>
        /// Gets and returns a font descriptor for the Open Type font file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="embed"></param>
        /// <param name="ttf"></param>
        /// <returns></returns>
        private static PDFFontDescriptor GetFontDescriptor(string name, bool embed, Scryber.OpenType.TTFFile ttf)
        {
            PDFFontDescriptor desc = new PDFFontDescriptor();
            desc.FontName = name;
            desc.Flags = 0;//TODO:Flags
            

            Scryber.OpenType.SubTables.FontHeader head = ttf.Tables.FontHeader;
            int unitsperem = head.UnitsPerEm;
            desc.BoundingBox = new int[] { (head.XMin * PDFGlyphUnits) / unitsperem, 
                                           (head.YMin * PDFGlyphUnits) / unitsperem, 
                                           (head.XMax * PDFGlyphUnits) / unitsperem, 
                                           (head.YMax * PDFGlyphUnits) / unitsperem };
            Scryber.OpenType.SubTables.HorizontalHeader hhea = ttf.Tables.HorizontalHeader;
            Scryber.OpenType.SubTables.PostscriptTable post = ttf.Tables.PostscriptInformation;
            Scryber.OpenType.SubTables.OS2Table os2 = ttf.Tables.WindowsMetrics;
            desc.ItalicAngle = (int)post.ItalicAngle;

            desc.StemV = 80;// (int)os2.WidthClass;
            desc.Flags = 32;
            desc.AvgWidth = 342;
            desc.Ascent = ((int)os2.TypoAscender * PDFGlyphUnits) / unitsperem;
            desc.CapHeight = (os2.CapHeight * PDFGlyphUnits) / unitsperem;
            desc.Descent = (os2.TypoDescender * PDFGlyphUnits) / unitsperem;
            //MaxWidth 1086/FontWeight 700/XHeight 250/Leading 32
            desc.MaxWidth = (head.XMax * PDFGlyphUnits) / unitsperem;
            desc.XHeight = 250;
            desc.Leading = 32;
            
            if (embed)
                desc.FontFile = ttf.FileData;
            else
                desc.FontFile = null;

            return desc;
        }

        #endregion


        #region internal static PDFFont InitStdType1WinAnsi(string name, string basetype)

        internal static PDFFontDefinition InitStdType1WinAnsi(string name, string basetype, int spaceWidthFU)
        {
            return InitStdType1WinAnsi(name, basetype, basetype, false, false, spaceWidthFU);
        }

        internal static PDFFontDefinition InitStdSymbolType1WinAnsi(string name, string basetype, int spaceWidthFU)
        {
            PDFFontDefinition f = new PDFFontDefinition();
            f.SupportsVariants = false;
            f.SubType = FontType.Type1;
            f.BaseType = basetype;
            f.Encoding = FontEncoding.WinAnsiEncoding;
            f.Family = basetype;
            f.Bold = false;
            f.Italic = false;
            f.SpaceWidthFontUnits = 577;
            f.FontUnitsPerEm = 2048;
            return f;
        }

        internal static PDFFontDefinition InitStdType1WinAnsi(string name, string basetype, string family, bool bold, bool italic, int spaceWidthFU)
        {
            return InitStdType1WinAnsi(name, basetype, family, family, bold, italic, spaceWidthFU);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="basetype"></param>
        /// <param name="family"></param>
        /// <param name="winName"></param>
        /// <param name="bold"></param>
        /// <param name="italic"></param>
        /// <param name="spaceWidthFU">Width of a space in font units where there are 2048 font units per em</param>
        /// <returns></returns>
        internal static PDFFontDefinition InitStdType1WinAnsi(string name, string basetype, string family, string winName, bool bold, bool italic, int spaceWidthFU)
        {
            PDFFontDefinition f = new PDFFontDefinition();
            f.SubType = FontType.Type1;
            f.BaseType = basetype;
            f.Encoding = FontEncoding.WinAnsiEncoding;
            f.Family = family;
            f.WindowsName = winName;
            f.Bold = bold;
            f.Italic = italic;
            
            f.FontUnitsPerEm = 2048;
            f.SpaceWidthFontUnits = spaceWidthFU;

            return f;

        }


        #endregion


    }

}
