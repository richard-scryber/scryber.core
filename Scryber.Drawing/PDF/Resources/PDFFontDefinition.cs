﻿/*  Copyright 2012 PerceiveIT Limited
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
using Scryber.PDF.Native;
using Scryber.OpenType;
using Scryber.OpenType.SubTables;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// A complete font definition. Also contains the Descriptor, widths, and any embeddable file data.
    /// </summary>
    /// <remarks>
    /// Font Definitions are unique to a document and instances are not shared across multiple files
    /// </remarks>
    [Obsolete("Use the base FontDefinition or the PDFOpenTypeFontDefinition classes", true)]
    public class PDFFontDefinition : FontDefinition
    {

        //  
        // properties
        //


        #region private Scryber.OpenType.TTFFile TTFFile {get;set;}

        /// <summary>
        /// Gets the OpenType font file associated with this descriptor
        /// </summary>
        private Scryber.OpenType.TTFFile TTFFile
        {
            get;
            set;
        }

        #endregion

        #region public override bool CanMeasureStrings {get;}

        /// <summary>
        /// Returns true if this definition supports string measurement.
        /// </summary>
        /// <remarks>A definition can quickly measure strings if it has a reference to the OpenType font file</remarks>
        public override bool CanMeasureStrings
        {
            get { return null != TTFFile; }
        }

        #endregion


        #region public override bool IsUnicode {get;}

        /// <summary>
        /// private ca
        /// </summary>
        private bool? _isunicode;

        /// <summary>
        /// Returns true if this is an embeddable unicode font
        /// </summary>
        public override bool IsUnicode
        {
            get
            {
                if (_isunicode.HasValue == false)
                    _isunicode = (this.IsStandard == false && this.TTFFile != null && this.FontEncoding == FontEncoding.UnicodeEncoding);
                return _isunicode.Value;
            }
        }

        #endregion
        

        #region public override string PostscriptFontName {get;}

        private const int PostscriptFontNameIndex = 6;

        /// <summary>
        /// Gets the full postscript font name
        /// </summary>
        public override string PostscriptFontName
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
            else if (obj is Font)
                return this.Equals((Font)obj);

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
        public bool Equals(Font font)
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
        public override Size MeasureStringWidth(string chars, int startOffset, double fontSize, double available, bool wordBoundary, out int charsfitted)
        {
            if (null == this.TTFFile)
                throw new NullReferenceException(string.Format(Errors.FontDefinitionDoesNotHaveFile, this.FullName));
            OpenType.SubTables.CMapEncoding encoding = AssertGetTTFEncoding();

            var size = this.TTFFile.MeasureString(encoding, chars, startOffset, fontSize, available, wordBoundary, out charsfitted);
            return new Size(size.RequiredWidth, size.RequiredHeight);
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
        public override Size MeasureStringWidth(string chars, int startOffset, double fontSize, double available, 
            double? wordSpace, double? charSpace, double? hScale, bool vertical, bool wordBoundary, out int charsfitted)
        {
            if (null == this.TTFFile)
                throw new NullReferenceException(string.Format(Errors.FontDefinitionDoesNotHaveFile, this.FullName));
            OpenType.SubTables.CMapEncoding encoding = AssertGetTTFEncoding();

            var size = this.TTFFile.MeasureString(encoding, chars, startOffset, fontSize, available, wordSpace, 
                charSpace.HasValue ? charSpace.Value : 0.0, 
                hScale.HasValue ? hScale.Value : 1.0, 
                vertical, wordBoundary, wordBoundary, out charsfitted);
            
            return new Size(size.RequiredWidth, size.RequiredHeight);
        }

        public override LineSize MeasureStringWidth(string chars, int startOffset, double fontSizePts, double avaialableWidth,
            TypeMeasureOptions options)
        {
            int fitted;
            CMapEncoding encoding = AssertGetTTFEncoding();
            return this.TTFFile.MeasureString(encoding, chars, startOffset, fontSizePts, avaialableWidth, options.BreakOnWordBoundaries, out fitted, options.FontUnits);
        }

        public override TypeMeasureOptions GetMeasurementOptions(bool wordBoundary, double? wordSpace = null, double? charSpace = null,
            double? hScale = null, bool? vertical = null)
        {
            return new TypeMeasureOptions()
            {
                BreakOnWordBoundaries = wordBoundary,
                CharacterSpacing = charSpace, WordSpacing = wordSpace,
            };
        }

        private OpenType.SubTables.CMapEncoding AssertGetTTFEncoding()
        {
            var enc = this.CMapEncoding;
            var offset = this.TTFFile.Tables.CMap.GetOffsetTable(enc);
            if (null == offset)
                throw new PDFRenderException("The font " + this.FullName + " does not have a character mapping table that can be used for string measurement");
            return enc;
        }

        #endregion

        public override FontMetrics GetFontMetrics(Unit fontSize)
        {
            double scale = fontSize.PointsValue / ((double)this.FontUnitsPerEm);
            if (null != this.Descriptor)
            {
                double ascent = (double)this.Descriptor.Ascent * scale;
                double descent = Math.Abs((double)this.Descriptor.Descent * scale);

                double line = (double)(this.Descriptor.Leading * scale);
                double exheight = (double)(this.Descriptor.XHeight * scale);
                double zerowidth = (double)(this.Descriptor.AvgWidth * scale);
                return new FontMetrics(fontSize.PointsValue, ascent, descent, line, ascent, exheight, zerowidth);
            }
            else if (this.IsStandard)
            {
                double ascent = fontSize.PointsValue * 0.75;
                double descent = fontSize.PointsValue * 0.25;
                double line = fontSize.PointsValue * 1.2;
                double exheight = fontSize.PointsValue * 0.5;
                double zerowidth = fontSize.PointsValue * 0.5;
                return new FontMetrics(fontSize.PointsValue, ascent, descent, line, ascent, exheight, zerowidth);
            }
            else
                throw new InvalidOperationException("The font does not have a descriptor");
 
        }


        #region public PDFFontWidths GetWidths()

        /// <summary>
        /// Returns the PDFFontWidths for this font definition
        /// </summary>
        /// <returns></returns>
        public override PDFFontWidths GetWidths()
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

        #region public PDFObjectRef RenderToPDF(string name, PDFContextBase context, PDFWriter writer)

        public override PDFObjectRef RenderToPDF(string rsrcName, PDFFontWidths widths, ContextBase context, PDFWriter writer)
        {
            if(this.IsStandard)
            {
                return this.RenderStandardFont(rsrcName, context, writer);
            }
            else if(this.IsUnicode)
            {
                string subsetName = PDFFontDescriptor.CreateSubset();
                return this.RenderCompositeFont(subsetName, widths, context, writer);
            }
            else
                return this.RenderAnsiFont(rsrcName, widths, context, writer);
        }

        #endregion

        #region public virtual PDFObjectRef RenderCompositeFont(string name, PDFUsedFontWidths used, PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// Renders the PDFFontDefinition as a composite font - complete font definition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="used"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual PDFObjectRef RenderCompositeFont(string subset, PDFFontWidths widths, ContextBase context, PDFWriter writer)
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

        private PDFObjectRef WriteUnicodeFontInfo(string fullname, PDFFontWidths widths, ContextBase context, PDFWriter writer)
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

        private PDFObjectRef WriteDescendantFontInfo(string fullname, PDFFontWidths widths, ContextBase context, PDFWriter writer)
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
        public virtual PDFObjectRef RenderStandardFont(string name, ContextBase context, PDFWriter writer)
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
        public virtual PDFObjectRef RenderAnsiFont(string name, PDFFontWidths widths, ContextBase context, PDFWriter writer)
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
            writer.WriteName(this.FontEncoding.ToString());
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
            if (this.Descriptor != null && this.IsEmbedable)
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



        #region internal static PDFFontDefinition LoadOpenTypeFontFile(string path, string familyname, Scryber.Drawing.FontStyle style)

        /// <summary>
        /// Loads an OpenType font file from the specified path based on the name and style
        /// </summary>
        /// <param name="path">The path to the Open Type font file</param>
        /// <param name="familyname">The font family name in the file</param>
        /// <param name="style">The style (bold, italic etc) for the font</param>
        /// <returns>The new PDFFontDefinition</returns>
        internal static PDFFontDefinition LoadOpenTypeFontFile(string path, string familyname, Scryber.Drawing.FontStyle style, int weight, int headOffset)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            PDFFontDefinition defn = null;
            try
            {

                Scryber.OpenType.TTFFile ttf = new Scryber.OpenType.TTFFile(path, headOffset);

                defn = LoadOpenTypeFontFile(ttf, familyname, style, weight);
                defn.FilePath = path;
            }
            catch (Scryber.PDFException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Scryber.PDFException(
                    string.Format(Errors.CouldNotLoadTheFontFile, familyname, "See inner exception for more details."),
                    ex);
            }

            return defn;
        }

        #endregion

        #region internal static PDFFontDefinition LoadOpenTypeFontFile(byte[] data, string familyname, Scryber.Drawing.FontStyle style)

        /// <summary>
        /// Loads an OpenType font from the provided binary data based on the family name and font style
        /// </summary>
        /// <param name="data">The complete binary data for the OpenType font</param>
        /// <param name="familyname">The family name (Helvetica, Arial, etc) for the font</param>
        /// <param name="style">The style (Bold, Italic, etc) for the font</param>
        /// <returns>A new PDFFontDefinition for the font</returns>
        internal static PDFFontDefinition LoadOpenTypeFontFile(byte[] data, string familyname, Scryber.Drawing.FontStyle style, int weight, int headOffset)
        {
            if (null == data || data.Length == 0)
                throw new ArgumentNullException("data");

            PDFFontDefinition defn = null;
            try
            {
                Scryber.OpenType.TTFFile ttf = new OpenType.TTFFile(data, headOffset);
                defn = LoadOpenTypeFontFile(ttf, familyname, style, weight);
            }
            catch (Scryber.PDFException) { throw; }
            catch (Exception ex)
            {
                throw new Scryber.PDFException(string.Format(Errors.CouldNotLoadTheFontResource, familyname, "See inner exeption for more details."), ex);
            }
            return defn;
        }

        #endregion

        #region internal static PDFFontDefinition LoadOpenTypeFontFile(Scryber.OpenType.TTFFile ttf, string familyname, Scryber.Drawing.FontStyle style)

        /// <summary>
        /// Loads and returns a new PDFFontDefinition for the provided OpenType font with the name and style
        /// </summary>
        /// <param name="ttf">The open type font data for this font</param>
        /// <param name="familyname">The font family name</param>
        /// <param name="style">The font style</param>
        /// <returns>A new PDFFontDefinition</returns>
        internal static PDFFontDefinition LoadOpenTypeFontFile(Scryber.OpenType.TTFFile ttf, string familyname, Scryber.Drawing.FontStyle style, int weight)
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
                defn.Weight = weight;
                defn.Italic = (style != FontStyle.Regular);
                defn.WindowsName = defn.BaseType;
                defn.IsEmbedable = IsEmbeddable(ttf);
                defn.Descriptor = GetFontDescriptor(defn.BaseType, defn.IsEmbedable, ttf);

                var enc = GetOptimumCmapEncoding(ttf);
                
                int spaceWidth;
                int unitsPerEm;
                defn.Widths = GetArrayFontWidths(ttf, out unitsPerEm, out spaceWidth);

                defn.FontUnitsPerEm = unitsPerEm;
                defn.SpaceWidthFontUnits = spaceWidth;
                defn.CMapEncoding = enc;
                defn.FontEncoding = ConvertCMapToFontEncoding(enc);
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

            if (os2.FSType == Scryber.OpenType.FontRestrictions.InstallableEmbedding)
                return true;
            else if ((os2.FSType & Scryber.OpenType.FontRestrictions.NoEmbedding) > 0)
                return false;
            else if ((os2.FSType & Scryber.OpenType.FontRestrictions.PreviewPrintEmbedding) > 0 ||
                     (os2.FSType & Scryber.OpenType.FontRestrictions.EditableEmbedding) > 0)
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
            desc.AvgWidth = os2.XAverageCharWidth;

#if USETYPOGRAPHICLEADING

            desc.Ascent = os2.TypoAscender;
            desc.CapHeight = os2.CapHeight;
            desc.Descent = os2.TypoDescender;
            desc.Leading = os2.TypoLineGap - os2.TypoDescender + os2.TypoAscender;
#else
            desc.Descent = os2.TypoDescender;
            desc.CapHeight = os2.CapHeight;
            desc.Ascent = (int)(unitsperem + os2.TypoDescender); //2048 + (-250)
            desc.Leading = (int)(unitsperem * 1.2);
#endif

            
            //MaxWidth 1086/FontWeight 700/XHeight 250/Leading 32
            desc.MaxWidth = (head.XMax * PDFGlyphUnits) / unitsperem;
            desc.XHeight = os2.CapHeight;

            
            if (embed)
                desc.FontFile = ttf.FileData;
            else
                desc.FontFile = null;

            return desc;
        }

#endregion


        //Standard font definitions
        const int HelveticaSpaceWidthFU = 569;
        const int TimesSpaceWidthFU = 512;
        const int CourierSpaceWidthFU = 1228;
        const int ZaphSpaceWidthFU = 544;
        const int SymbolSpaceWidthFU = 512;

#region Helvetica, HelveticaBold, HelveticaOblique, HelveticaBoldOblique

        private static PDFFontDefinition _helvetica = null;

        public static PDFFontDefinition Helvetica
        {
            get
            {
                if (_helvetica == null)
                {
                    _helvetica = PDFFontDefinition.InitStdType1WinAnsi("Fhel", "Helvetica", "Helvetica", false, false, HelveticaSpaceWidthFU);
                }
                return _helvetica;
            }

        }


        private static PDFFontDefinition _helveticabold = null;

        public static PDFFontDefinition HelveticaBold
        {
            get
            {
                if (_helveticabold == null)
                    _helveticabold = PDFFontDefinition.InitStdType1WinAnsi("FhelBl", "Helvetica-Bold", "Helvetica", true, false, HelveticaSpaceWidthFU);
                return _helveticabold;
            }
        }


        private static PDFFontDefinition _helveticaobl = null;

        public static PDFFontDefinition HelveticaOblique
        {
            get
            {
                if (_helveticaobl == null)
                    _helveticaobl = PDFFontDefinition.InitStdType1WinAnsi("FhelOb", "Helvetica-Oblique", "Helvetica", false, true, HelveticaSpaceWidthFU);
                return _helveticaobl;
            }
        }


        private static PDFFontDefinition _helveticabolobl = null;

        public static PDFFontDefinition HelveticaBoldOblique
        {
            get
            {
                if (_helveticabolobl == null)
                    _helveticabolobl = PDFFontDefinition.InitStdType1WinAnsi("FhelObBl", "Helvetica-BoldOblique", "Helvetica", true, true, HelveticaSpaceWidthFU);
                return _helveticabolobl;
            }
        }


#endregion

#region TimesRoman, TimesBold, TimesItalic, TimesBoldItalic

        private static PDFFontDefinition _times = null;

        public static PDFFontDefinition TimesRoman
        {
            get
            {
                if (_times == null)
                    _times = PDFFontDefinition.InitStdType1WinAnsi("Ftimes", "Times-Roman", "Times", false, false, TimesSpaceWidthFU);
                return _times;
            }
        }


        private static PDFFontDefinition _timesbold = null;

        public static PDFFontDefinition TimesBold
        {
            get
            {
                if (_timesbold == null)
                    _timesbold = PDFFontDefinition.InitStdType1WinAnsi("FtimesBo", "Times-Bold", "Times", true, false, TimesSpaceWidthFU);
                return _timesbold;
            }
        }


        private static PDFFontDefinition _timesboldital = null;

        public static PDFFontDefinition TimesBoldItalic
        {
            get
            {
                if (_timesboldital == null)
                    _timesboldital = PDFFontDefinition.InitStdType1WinAnsi("FtimesBoIt", "Times-BoldItalic", "Times", true, true, TimesSpaceWidthFU);
                return _timesboldital;
            }
        }


        private static PDFFontDefinition _timesital = null;

        public static PDFFontDefinition TimesItalic
        {
            get
            {
                if (_timesital == null)
                    _timesital = PDFFontDefinition.InitStdType1WinAnsi("FtimesIt", "Times-Italic", "Times", false, true, TimesSpaceWidthFU);
                return _timesital;
            }
        }

#endregion

#region Courier, CourierBold, CourierOblique, CourierBoldOblique

        private static PDFFontDefinition _cour = null;

        public static PDFFontDefinition Courier
        {
            get
            {
                if (_cour == null)
                    _cour = PDFFontDefinition.InitStdType1WinAnsi("Fcour", "Courier", "Courier", "Courier New", false, false, CourierSpaceWidthFU);
                return _cour;
            }
        }


        private static PDFFontDefinition _courbold = null;

        public static PDFFontDefinition CourierBold
        {
            get
            {
                if (_courbold == null)
                    _courbold = PDFFontDefinition.InitStdType1WinAnsi("FcourBo", "Courier-Bold", "Courier", "Courier New", true, false, CourierSpaceWidthFU);
                return _courbold;
            }
        }

        private static PDFFontDefinition _courital = null;

        public static PDFFontDefinition CourierOblique
        {
            get
            {
                if (_courital == null)
                    _courital = PDFFontDefinition.InitStdType1WinAnsi("FcourOb", "Courier-Oblique", "Courier", "Courier New", false, true, CourierSpaceWidthFU);
                return _courital;
            }
        }

        private static PDFFontDefinition _courboldital = null;

        public static PDFFontDefinition CourierBoldOblique
        {
            get
            {
                if (_courboldital == null)
                    _courboldital = PDFFontDefinition.InitStdType1WinAnsi("FcourBoOb", "Courier-BoldOblique", "Courier", "Courier New", true, true, CourierSpaceWidthFU);
                return _courboldital;
            }
        }

#endregion

#region ZapfDingbats

        private static PDFFontDefinition _zaph = null;

        public static PDFFontDefinition ZapfDingbats
        {
            get
            {
                if (_zaph == null)
                {
                    _zaph = PDFFontDefinition.InitStdSymbolType1WinAnsi("Fzapf", "ZapfDingbats", ZaphSpaceWidthFU);
                    _zaph.Family = "Zapf Dingbats";
                    _zaph.WindowsName = "WingDings";
                }
                return _zaph;
            }
        }

#endregion

#region Symbol

        private static PDFFontDefinition _sym = null;

        public static PDFFontDefinition Symbol
        {
            get
            {
                if (_sym == null)
                    _sym = PDFFontDefinition.InitStdSymbolType1WinAnsi("Fsym", "Symbol", SymbolSpaceWidthFU);
                return _sym;
            }
        }

#endregion


#region internal static PDFFont InitStdType1WinAnsi(string name, string basetype)

        internal static PDFFontDefinition InitStdType1WinAnsi(string name, string basetype, int spaceWidthFU, TTFFile file = null)
        {
            return InitStdType1WinAnsi(name, basetype, basetype, false, false, spaceWidthFU, file);
        }

        internal static PDFFontDefinition InitStdSymbolType1WinAnsi(string name, string basetype, int spaceWidthFU, TTFFile file = null)
        {
            PDFFontDefinition f = new PDFFontDefinition();
            f.SupportsVariants = false;
            f.SubType = FontType.Type1;
            f.BaseType = basetype;
            f.FontEncoding = FontEncoding.WinAnsiEncoding;
            f.Family = basetype;
            f.Weight = FontWeights.Regular;
            f.Italic = false;
            f.SpaceWidthFontUnits = 577;
            f.FontUnitsPerEm = 2048;
            f.TTFFile = file;
            f.CMapEncoding = GetOptimumCMapEncoding(file);
            f.IsStandard = true;

            return f;
        }

        internal static PDFFontDefinition InitStdType1WinAnsi(string name, string basetype, string family, bool bold, bool italic, int spaceWidthFU, TTFFile file = null)
        {
            return InitStdType1WinAnsi(name, basetype, family, family, bold, italic, spaceWidthFU, file);
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
        internal static PDFFontDefinition InitStdType1WinAnsi(string name, string basetype, string family, string winName, bool bold, bool italic, int spaceWidthFU, TTFFile file = null)
        {
            PDFFontDefinition f = new PDFFontDefinition();
            f.SubType = FontType.Type1;
            f.BaseType = basetype;
            f.FontEncoding = FontEncoding.WinAnsiEncoding;
            f.Family = family;
            f.WindowsName = winName;
            f.Weight = bold ? FontWeights.Bold : FontWeights.Regular;
            f.Italic = italic;
            
            f.FontUnitsPerEm = 2048;
            f.SpaceWidthFontUnits = spaceWidthFU;
            f.TTFFile = file;
            f.CMapEncoding = GetOptimumCMapEncoding(file);
            f.IsStandard = true;
            return f;

        }


#endregion


        private static CMapEncoding GetOptimumCMapEncoding(TTFFile file)
        {
            var maps = file.Tables.CMap;
            if (null == maps || maps.NumberOfTables == 0)
                throw new Scryber.PDFException("The truetype file does not have any character mapping tables that can be read");

            var first = CMapEncoding.UnicodeDefault;
            var second = CMapEncoding.Unicode_20;
            var third = CMapEncoding.WindowsUnicode;
            var fourth = CMapEncoding.MacRoman;
            var found = new CMAPRecord[4];

            foreach (var map in maps.Records)
            {
                if (map.Encoding.Equals(first))
                    found[0] = map;
                else if (map.Encoding.Equals(second))
                    found[1] = map;
                else if (map.Encoding.Equals(third))
                    found[2] = map;
                else if (map.Encoding.Equals(fourth))
                    found[3] = map;
            }

            foreach (var map in found)
            {
                if (null != map)
                    return map.Encoding;
            }

            throw new Scryber.PDFException("The truetype file does not have any character mapping tables that can be used." +
                " Mappings supported are Unicode Unicode, Unicode Unicode_20, Windows Unicode and Mac Roman");
        }

    }

}
