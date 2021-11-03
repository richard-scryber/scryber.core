using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Scryber.Drawing;
using Scryber.OpenType;
using Scryber.OpenType.SubTables;
using Scryber.OpenType.TTF;
using Scryber.Options;
using Scryber.PDF.Native;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// An OpenType Font definition. This is based on the FontDefinition and supports writing to a PDF File.
    /// Use the Load() method with an ITypeFaceFont to create new instances.
    /// </summary>
    public class PDFOpenTypeFontDefinition : FontDefinition
    {
        //
        // constants
        //
        
        /// <summary>
        /// Defines the font glyph unit size for PDF documents.
        /// </summary>
        public const int PDFGlyphUnits = 1000;
        
        
        public const string HeaderName = OpenType.TTF.TrueTypeTableNames.FontHeader;
        public const string HorizHeadName = OpenType.TTF.TrueTypeTableNames.HorizontalHeader;
        public const string PostName = OpenType.TTF.TrueTypeTableNames.PostscriptInformation;
        public const string OS2Name = OpenType.TTF.TrueTypeTableNames.WindowsMetrics;
        public const string NamingName = OpenType.TTF.TrueTypeTableNames.NamingTable;
        public const string CMapName = OpenType.TTF.TrueTypeTableNames.CharacterMapping;
        public const string HMetricsName = OpenType.TTF.TrueTypeTableNames.HorizontalMetrics;
        
        public const int NameIDFullFamily = 4;

        /// <summary>
        /// Defines the preferred encodings in their preference order
        /// </summary>
        private static readonly CMapEncoding[] PreferredCharacterEncodings = {
            OpenType.SubTables.CMapEncoding.WindowsUnicode,
            OpenType.SubTables.CMapEncoding.UnicodeDefault,
            OpenType.SubTables.CMapEncoding.Unicode_20,
            OpenType.SubTables.CMapEncoding.MacRoman
        };
        
        //
        // properties
        //
        
        #region public IOpenTypeFont OpenTypeFont { get; protected set; }
        
        /// <summary>
        /// Gets the OpenTypeFont that this descriptor is base on.
        /// </summary>
        public IOpenTypeFont OpenTypeFont { get; protected set; }
        
        #endregion

        #region public override bool CanMeasureStrings {get;}
        
        /// <summary>
        /// returns true if this descriptor can measure the size of strings
        /// </summary>
        public override bool CanMeasureStrings
        {
            get { return null != this.OpenTypeFont; }
        }
        
        #endregion

        #region public override bool IsUnicode {get;}
        
        /// <summary>
        /// Returns true if this font is a Unicode font (has more than the ASCII character set
        /// </summary>
        public override bool IsUnicode
        {
            get
            {
                return null != this.OpenTypeFont && this.IsStandard == false &&
                       this.FontEncoding == FontEncoding.UnicodeEncoding;
            }
        }
        
        #endregion

        #region protected IFontMetrics FontMetrics {get;set;}
        
        private IFontMetrics _metrics;
        
        /// <summary>
        /// Gets or sets the metrics for this font definition
        /// </summary>
        protected IFontMetrics FontMetrics
        {
            get { return _metrics; }
            set { _metrics = value; }
        }
        
        #endregion
        
        //
        // .ctor
        //
        
        #region protected PDFOpenTypeFontDefinition()
        
        /// <summary>
        /// The default constructor for the open type font descriptor. Inheritors can call if making their own subclass
        /// </summary>
        protected PDFOpenTypeFontDefinition() : base()
        {}
        
        #endregion

        //
        // object overrides
        //
        
        #region public override bool Equals(object obj) + 1 overload
        
        /// <summary>
        /// Returns true if obj is a PDFOpenTypeFontDefinition and they are both equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is PDFOpenTypeFontDefinition)
                return this.Equals((PDFOpenTypeFontDefinition) obj);
            else
                return false;
        }

        /// <summary>
        /// Returns true if the definition matches this definition
        /// </summary>
        /// <param name="defn"></param>
        /// <returns></returns>
        public bool Equals(PDFOpenTypeFontDefinition defn)
        {
            if (null == defn)
                return false;
            else if (this.FullName == defn.FullName)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if this font definition matches the specified font.
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
        /// Overrides the base implementation to return the hash of this definitions full name.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.FullName.GetHashCode();
        }
        
        #endregion

        #region public override string ToString()
        
        /// <summary>
        /// Returns a string representation of this font definition.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Open Type Font Definition for : " + this.FullName;
        }
        
        #endregion
        
        
        //
        // string measurement
        //

        private static readonly TypeMeasureOptions WordBreaks = TypeMeasureOptions.BreakOnWords;
        private static readonly TypeMeasureOptions Default = TypeMeasureOptions.Default;
        
        #region public override TypeMeasureOptions GetMeasurementOptions(..)
        
        public override TypeMeasureOptions GetMeasurementOptions(bool wordBoundary, double? wordSpace = null, double? charSpace = null, double? hScale = null, bool? vertical = null)
        {
            if (hScale.HasValue)
                throw new NotSupportedException("Horizontal scaling is not currently supported");
            if(vertical.HasValue)
                throw new NotSupportedException("Vertical text is not currently supported");

            if (wordSpace.HasValue || charSpace.HasValue)
                return new TypeMeasureOptions()
                    {WordSpacing = wordSpace, CharacterSpacing = charSpace, BreakOnWordBoundaries = wordBoundary};
            
            else if (wordBoundary)
                return WordBreaks;
            else
                return Default;
        }
        
        #endregion

        #region protected virtual IFontMetrics GetFontMetrics(TypeMeasureOptions options)
        
        /// <summary>
        /// Returns the font metrics for this font, either stored locally or from the font itself if the font units are explicit
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected virtual IFontMetrics GetFontMetrics(TypeMeasureOptions options)
        {
            if (null == this.FontMetrics || options.FontUnits != FontUnitType.UseFontPreference)
                this.FontMetrics = this.OpenTypeFont.GetMetrics(options);
            return this.FontMetrics;
        }
        
        #endregion
        
        #region public override Size MeasureStringWidth(...) 3 overloads.
        
        
        public override Size MeasureStringWidth(string chars, int startOffset, double fontSizePts, double avaialableWidth,
            bool wordBoundary, out int charsFitted)
        {
            if (!this.CanMeasureStrings)
                throw new InvalidOperationException(
                    "This font definition does not support measuring strings, as the TypefaceFont is not set");

            var options = this.GetMeasurementOptions(wordBoundary);
            var measure = this.GetFontMetrics(options);

            var line = measure.MeasureLine(chars, startOffset, fontSizePts, avaialableWidth, options);
            
            charsFitted = line.CharsFitted;
            return new Size(line.RequiredWidth, line.RequiredHeight);
        }

        public override Size MeasureStringWidth(string chars, int startOffset, double fontSizePts,
            double avaialableWidth, double? wordSpace, double? charSpace, double? hScale, bool vertical,
            bool wordBoundary, out int charsFitted)
        {
            if (!this.CanMeasureStrings)
                throw new InvalidOperationException(
                    "This font definition does not support measuring strings, as the TypefaceFont is not set");

            var options = this.GetMeasurementOptions(wordBoundary, wordSpace, charSpace, hScale);
            var measure = this.GetFontMetrics(options);

            var line = measure.MeasureLine(chars, startOffset, fontSizePts, avaialableWidth, options);
            
            charsFitted = line.CharsFitted;
            return new Size(line.RequiredWidth, line.RequiredHeight);
        }

        public override LineSize MeasureStringWidth(string chars, int startOffset, double fontSizePts, double avaialableWidth,
            TypeMeasureOptions options)
        {
            var metrics = this.GetFontMetrics(options);

            return metrics.MeasureLine(chars, startOffset, fontSizePts, avaialableWidth, options);
        }
        
        #endregion

        #region public override FontMetrics GetFontMetrics(Unit fontSize)
        
        /// <summary>
        /// Returns the font metrics based on the OpenType font, if available, or standard font metrics.
        /// </summary>
        /// <param name="fontSize">The size of the font to get the metrics for.</param>
        /// <returns></returns>
        public override FontMetrics GetFontMetrics(Unit fontSize)
        {
            if (this.CanMeasureStrings)
            {
                var options = this.GetMeasurementOptions(false);
                var metrics = this.GetFontMetrics(options);
                double scale = fontSize.PointsValue / (double) (metrics.FUnitsPerEm);

                double ascent = metrics.AscenderHeightFU * scale;
                double descent = metrics.DescenderHeightFU * scale;
                double line = (metrics.LineSpaceingFU + metrics.AscenderHeightFU - metrics.DescenderHeightFU) * scale;
                double exHeight = metrics.xAvgWidthFU * scale;
                double zeroWidth = metrics.xAvgWidthFU * scale;

                return new FontMetrics(fontSize.PointsValue, ascent, descent, line, exHeight, zeroWidth);
            }
            else
            {
                double ascent = fontSize.PointsValue * 0.75;
                double descent = fontSize.PointsValue * 0.25;
                double line = fontSize.PointsValue * 1.2;
                double exHeight = fontSize.PointsValue * 0.5;
                double zeroWidth = fontSize.PointsValue * 0.5;
                return new FontMetrics(fontSize.PointsValue, ascent, descent, line, exHeight, zeroWidth);
            }
        }
        
        #endregion
        
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
                return GetCompositeFontWidths(this.OpenTypeFont, out var unitsPerEm, out var spaceWidth);
            }
            else
                return this.Widths;
        }

        #endregion
        
        
        //
        // rendering
        //

        #region public override PDFObjectRef RenderToPDF(string name, PDFFontWidths widths, ContextBase context, PDFWriter writer)
        
        /// <summary>
        /// Overrides the base method to perform the actual rendering of the font to the Writer. The font can be rendered as a Standard font (built in), Composite (unicode) or Ansi font.
        /// </summary>
        /// <param name="name">The font resource name</param>
        /// <param name="widths">The widths to use</param>
        /// <param name="context">The current render context</param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override PDFObjectRef RenderToPDF(string name, PDFFontWidths widths, ContextBase context,
            PDFWriter writer)
        {
            if(this.IsStandard)
            {
                return this.RenderStandardFont(name, context, writer);
            }
            else if(this.IsUnicode)
            {
                string subsetName = PDFFontDescriptor.CreateSubset();
                return this.RenderCompositeFont(subsetName, widths, context, writer);
            }
            else
                return this.RenderAnsiFont(name, widths, context, writer);
        }
        
        #endregion
        
        #region public virtual PDFObjectRef RenderCompositeFont(string name, PDFUsedFontWidths used, PDFContextBase context, PDFWriter writer)
        
        /// <summary>
        /// Renders the PDFFontDefinition as a composite font - complete font definition
        /// </summary>
        /// <param name="subset">The name postfix for the font (e.g XABCDEF) that will uniquely identify the font from it's based type</param>
        /// <param name="widths">The font widths to render</param>
        /// <param name="context">The current context</param>
        /// <param name="writer">The PDF Writer to write to</param>
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
        
        #endregion
        
        #region private PDFObjectRef WriteUnicodeFontInfo(string fullname, PDFFontWidths widths, ContextBase context, PDFWriter writer)
        

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
        
        #endregion
        
        #region private PDFObjectRef WriteDescendantFontInfo(string fullname, PDFFontWidths widths, ContextBase context, PDFWriter writer)
        
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
        /// <returns>Any Object reference for the PDFIndirectObject rendered</returns>
        public virtual PDFObjectRef RenderStandardFont(string name, ContextBase context, PDFWriter writer)
        {
            return this.RenderAnsiFont(name, null, context, writer);
        }

        #endregion
        
        #region public virtual PDFObjectRef RenderAnsiFont(string name, PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// Renders the PDFFontDefinition onto the PDFWriter
        /// </summary>
        /// <param name="name">The font name</param>
        /// <param name="widths">The font character widths</param>
        /// <param name="context">The current context</param>
        /// <param name="writer">The writer to write the font to</param>
        /// <returns>A reference to the PDFObject written</returns>
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
        // Init Standard font methods
        //
        
        internal static PDFOpenTypeFontDefinition InitStdSymbolType1WinAnsi(string name, string basetype, int spaceWidthFU, IOpenTypeFont font = null)
        {
            PDFOpenTypeFontDefinition f = new PDFOpenTypeFontDefinition();
            f.SupportsVariants = false;
            f.SubType = FontType.Type1;
            f.BaseType = basetype;
            f.FontEncoding = FontEncoding.WinAnsiEncoding;
            f.Family = basetype;
            f.Weight = FontWeights.Regular;
            f.Italic = false;
            f.SpaceWidthFontUnits = 577;
            f.FontUnitsPerEm = 2048;
            f.OpenTypeFont = font;
            if (null != font)
            {
                var metrics = font.GetMetrics(TypeMeasureOptions.Default);
                f.FontUnitsPerEm = metrics.FUnitsPerEm;
                f.SpaceWidthFontUnits = metrics.xAvgWidthFU;
                
                if (font.TryGetTable<OpenType.SubTables.CMAPTable>(CMapName, out var table))
                    f.CMapEncoding = GetOptimumEncoding(table, out var map);
            }

            f.IsStandard = true;

            return f;
        }
        internal static PDFOpenTypeFontDefinition InitStdType1WinAnsi(string name, string basetype, string family, bool bold, bool italic, int spaceWidthFU, IOpenTypeFont font = null)
        {
            return InitStdType1WinAnsi(name, basetype, family, family, bold, italic, spaceWidthFU, font);
        }

        internal static PDFOpenTypeFontDefinition InitStdType1WinAnsi(string name, string basetype, string family, string winName, bool bold, bool italic, int spaceWidthFU, IOpenTypeFont font = null)
        {
            PDFOpenTypeFontDefinition f = new PDFOpenTypeFontDefinition();
            f.SubType = FontType.Type1;
            f.BaseType = basetype;
            f.FontEncoding = FontEncoding.WinAnsiEncoding;
            f.Family = family;
            f.WindowsName = winName;
            f.Weight = bold ? FontWeights.Bold : FontWeights.Regular;
            f.Italic = italic;
            f.IsStandard = true;
            f.OpenTypeFont = font;
            f.SpaceWidthFontUnits = spaceWidthFU;
            
            if (null != font)
            {
                var metrics = font.GetMetrics(TypeMeasureOptions.Default);
                f.FontUnitsPerEm = metrics.FUnitsPerEm;
                f.SpaceWidthFontUnits = metrics.xAvgWidthFU;
                
                if (font.TryGetTable<OpenType.SubTables.CMAPTable>(CMapName, out var table))
                    f.CMapEncoding = GetOptimumEncoding(table, out var map);
            }

            return f;

        }
        
        
        //
        // static load methods
        //

        /// <summary>
        /// Loads a new OpenType font definition based on a Scryber.OpenType.ITypefaceFont
        /// </summary>
        /// <param name="font">The font to use</param>
        /// <param name="family">The font family name</param>
        /// <param name="style">The font style</param>
        /// <param name="weight">The font weight</param>
        /// <param name="embed">optional value to indicate if the font should be embedded</param>
        /// <returns>The fully formed FontDefinition</returns>
        /// <exception cref="PDFFontInitException"></exception>
        public static PDFOpenTypeFontDefinition Load(ITypefaceFont font, string family, FontStyle style, int weight, bool? embed)
        {
            var opentype = (IOpenTypeFont) font;

            bool success = opentype.TryGetTable<OpenType.SubTables.FontHeader>(HeaderName, out var header);
            success &= opentype.TryGetTable<OpenType.SubTables.HorizontalHeader>(HorizHeadName, out var hhea);
            success &= opentype.TryGetTable<OpenType.SubTables.PostscriptTable>(PostName, out var post);
            success &= opentype.TryGetTable<OpenType.SubTables.OS2Table>(OS2Name, out var os2);
            success &= opentype.TryGetTable<OpenType.SubTables.NamingTable>(NamingName, out var names);
            success &= opentype.TryGetTable<OpenType.SubTables.CMAPTable>(CMapName, out var cmap);
            success &= opentype.TryGetTable<HorizontalMetrics>(HMetricsName, out var hmtx);
            
            if (!success)
                throw new PDFFontInitException("Could not load the open type font, as not ll tables were present");

            PDFFontDescriptor desc = new PDFFontDescriptor();
            
            desc.FontName = family;

            desc.ItalicAngle = (int) post.ItalicAngle;
            desc.StemV = 80;
            desc.Flags = 32;
            desc.AvgWidth = os2.XAverageCharWidth;

            desc.BoundingBox = new int[] { 
                (header.XMin * PDFGlyphUnits) / header.UnitsPerEm, 
                (header.YMin * PDFGlyphUnits) / header.UnitsPerEm, 
                (header.XMax * PDFGlyphUnits) / header.UnitsPerEm, 
                (header.YMax * PDFGlyphUnits) / header.UnitsPerEm
            };
            
            if ((os2.Selection & FontSelection.UseTypographicSizes) > 0)
            {
                desc.Ascent = os2.TypoAscender;
                desc.Descent = os2.TypoDescender;
                desc.Leading = os2.TypoLineGap;
            }
            else
            {
                desc.Ascent = hhea.Ascender;
                desc.Descent = hhea.Descender;
                desc.Leading = hhea.LineGap;
            }
            desc.CapHeight = os2.CapHeight;
            desc.MaxWidth = (header.XMax * PDFGlyphUnits) / header.UnitsPerEm;
            desc.XHeight = os2.Height;

            if (embed?? IsEmbeddable(os2))
                desc.FontFile = font.GetFileData(DataFormat.TTF);
            else
                desc.FontFile = null;

            PDFOpenTypeFontDefinition defn = new PDFOpenTypeFontDefinition();
            defn.OpenTypeFont = opentype;
            defn.Family = family;
            defn.SubType = FontType.TrueType;
            defn.BaseType = names.GetInvariantName(NameIDFullFamily).Replace(" ", "");
            defn.Weight = weight;
            defn.Italic = (style != FontStyle.Regular);
            defn.WindowsName = defn.BaseType;
            defn.IsEmbedable = embed ?? IsEmbeddable(os2);
            defn.Descriptor = desc;

            int spaceWidth;
            CMAPSubTable mapping;
            defn.CMapEncoding = GetOptimumEncoding(cmap, out mapping);
            defn.Widths = GetArrayFontWidths(header, mapping, defn.CMapEncoding, hmtx, out spaceWidth);
            defn.FontUnitsPerEm = header.UnitsPerEm;
            defn.SpaceWidthFontUnits = spaceWidth;
            defn.FontEncoding = ConvertCMapToFontEncoding(defn.CMapEncoding);
            
            return defn;
        }

        #region protected static CMapEncoding GetOptimumEncoding(CMAPTable cmap, out CMAPSubTable chars)

        /// <summary>
        /// Gets an available character mapping encoding within the
        /// CMap table based on the preferred order.
        /// </summary>
        /// <param name="cmap">The character map table encoding</param>
        /// <param name="chars">Set to the character mapping </param>
        /// <returns>A preferred encoding</returns>
        /// <exception cref="NullReferenceException">If there are no character mappings that are in the preferred selection</exception>
        protected static CMapEncoding GetOptimumEncoding(CMAPTable cmap, out CMAPSubTable chars)
        {
            chars = null;
            CMapEncoding foundenc = CMapEncoding.Unicode_20;
            
            foreach (OpenType.SubTables.CMapEncoding enc in PreferredCharacterEncodings)
            {
                chars = cmap.GetOffsetTable(enc);
                foundenc = enc;

                if (null != chars)
                    break;
            }

            if (null == chars)
                throw new NullReferenceException("The specified font does not contain a known character mapping");
            else
                return foundenc;
        }
        
        #endregion

        #region private static PDFFontWidths GetCompositeFontWidths(Scryber.OpenType.TTFFile ttf)

        /// <summary>
        /// Creates an returns a Composite Font Widths instance for the Open Type font. 
        /// Composite Fonts Widths can describe full unicode character sets.
        /// </summary>
        /// <param name="font">The font programme to get the composite widths for</param>
        /// <param name="unitsPerEm">Set to the font programmes units per em</param>
        /// <param name="spaceWidth">Set to the PDF unit width of a space ' '</param>
        /// <returns></returns>
        private static PDFFontWidths GetCompositeFontWidths(IOpenTypeFont font, out int unitsPerEm, out int spaceWidth)
        {
            FontHeader head;
            if (!font.TryGetTable<FontHeader>(HeaderName, out head))
                throw new InvalidOperationException("The required font header table was not present in the file");
            CMAPTable cmap;
            if(!font.TryGetTable<CMAPTable>(CMapName, out cmap))
                throw new InvalidOperationException("The required font character mapping table was not present in the file");
            HorizontalMetrics hmtx;
            if(!font.TryGetTable<HorizontalMetrics>(HMetricsName, out hmtx))
                throw new InvalidOperationException("The required horizontal metrics table was not present in the file");

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
            
            PDFCompositeFontWidths comp = new PDFCompositeFontWidths(foundenc, chars, hmtx, unitsPerEm, PDFGlyphUnits);
            
            spaceWidth = comp.GetGlyphWidthForCharacter(' ');

            return comp;
        }

        #endregion
        
        #region protected static PDFFontWidths GetArrayFontWidths(FontHeader header, CMAPSubTable chars, CMapEncoding enc, HorizontalMetrics hmtx, out int spaceWidth)

        protected static PDFFontWidths GetArrayFontWidths(FontHeader header, CMAPSubTable chars, CMapEncoding enc, HorizontalMetrics hmtx, out int spaceWidth)
        {
            int unitsPerEm = header.UnitsPerEm;
            spaceWidth = 0;

            List<int> all = new List<int>();
            int first = 0;
            int last = 255;
            for (int i = first; i <= last; i++)
            {
                int moffset = (int)chars.GetCharacterGlyphOffset((char)i);
                //System.Diagnostics.Debug.WriteLine("Character '" + chars[i].ToString() + "' (" + ((byte)chars[i]).ToString() + ") has offset '" + moffset.ToString() + "' in mac encoding and '" + woffset + "' in windows encoding");

                if (moffset >= hmtx.HMetrics.Count)
                    moffset = hmtx.HMetrics.Count - 1;
                Scryber.OpenType.SubTables.HMetric metric = hmtx.HMetrics[moffset];

                int value = metric.AdvanceWidth; // - metric.LeftSideBearing;

                if (i == 32) //Space Width
                    spaceWidth = value;

                value = (value * PDFGlyphUnits) / unitsPerEm;
                all.Add(value);
            }

            PDFFontWidths widths = new PDFArrayFontWidths(first, last, all, enc); //chars, hmtc
            return widths;
        }
        
        #endregion

        #region protected static FontEncoding ConvertCMapToFontEncoding(CMapEncoding encoding)

        /// <summary>
        /// Converts a know and found encoding to one of the FontEncoding enumeration values.
        /// </summary>
        /// <param name="encoding">The encoding to convert</param>
        /// <returns>The available font encoding.</returns>
        protected static FontEncoding ConvertCMapToFontEncoding(CMapEncoding encoding)
        {
            switch (encoding.Platform)
            {
                case Scryber.OpenType.SubTables.CharacterPlatforms.Unicode:
                    return FontEncoding.UnicodeEncoding;

                case Scryber.OpenType.SubTables.CharacterPlatforms.Macintosh:
                    return FontEncoding.MacRomanEncoding;

                case Scryber.OpenType.SubTables.CharacterPlatforms.Windows:
                    switch (encoding.Encoding)
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
        
        #region private static bool IsEmbeddable(OpenType.SubTables.OS2Table os2)
        
        /// <summary>
        /// Returns true if the font descriptor based OpenType font can be embedded (based on the flags in the os2 table)
        /// </summary>
        /// <param name="os2">The OS2Table to check</param>
        /// <returns></returns>
        private static bool IsEmbeddable(OpenType.SubTables.OS2Table os2)
        {
            
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
        
    }
}