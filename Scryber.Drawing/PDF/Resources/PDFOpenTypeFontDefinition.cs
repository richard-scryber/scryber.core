using System;
using Newtonsoft.Json.Linq;
using Scryber.Drawing;
using Scryber.OpenType;
using Scryber.OpenType.SubTables;
using Scryber.Options;

namespace Scryber.PDF.Resources
{
    
    public class PDFOpenTypeFontDefinition : FontDefinition
    {
        public const string HeaderName = OpenType.TTF.TrueTypeTableNames.FontHeader;
        public const string HorizHeadName = OpenType.TTF.TrueTypeTableNames.HorizontalHeader;
        public const string PostName = OpenType.TTF.TrueTypeTableNames.PostscriptInformation;
        public const string OS2Name = OpenType.TTF.TrueTypeTableNames.WindowsMetrics;
        public const string NamingName = OpenType.TTF.TrueTypeTableNames.NamingTable;
        public const string CMapName = OpenType.TTF.TrueTypeTableNames.CharacterMapping;
        
        public const int NameIDFullFamily = 4;


        public static PDFOpenTypeFontDefinition Load(ITypefaceFont font, string family, FontStyle style, int weight, bool? embed)
        {
            var opentype = (IOpenTypeFont) font;

            bool success = opentype.TryGetTable<OpenType.SubTables.FontHeader>(HeaderName, out var header);
            success &= opentype.TryGetTable<OpenType.SubTables.HorizontalHeader>(HorizHeadName, out var hhea);
            success &= opentype.TryGetTable<OpenType.SubTables.PostscriptTable>(PostName, out var post);
            success &= opentype.TryGetTable<OpenType.SubTables.OS2Table>(OS2Name, out var os2);
            success &= opentype.TryGetTable<OpenType.SubTables.NamingTable>(NamingName, out var names);
            success &= opentype.TryGetTable<OpenType.SubTables.CMAPTable>(CMapName, out var cmap);
            if (!success)
                throw new PDFFontInitException("Could not load the open type font, as not ll tables were present");

            PDFFontDescriptor desc = new PDFFontDescriptor();

            desc.FontName = family;

            desc.ItalicAngle = (int) post.ItalicAngle;
            desc.StemV = 80;
            desc.Flags = 32;
            desc.AvgWidth = os2.XAverageCharWidth;

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
            desc.MaxWidth = header.XMax;
            desc.XHeight = os2.Height;

            if (embed?? IsEmbeddable(os2))
                desc.FontFile = font.GetFileData(DataFormat.TTF);
            else
                desc.FontFile = null;

            PDFOpenTypeFontDefinition defn = new PDFOpenTypeFontDefinition();
            defn.Family = family;
            defn.SubType = FontType.TrueType;
            defn.BaseType = names.GetInvariantName(NameIDFullFamily).Replace(" ", "");
            defn.Weight = weight;
            defn.Italic = (style != FontStyle.Regular);
            defn.WindowsName = defn.BaseType;
            defn.IsEmbedable = embed ?? IsEmbeddable(os2);
            defn.Descriptor = desc;

            int spaceWidth;
            defn.Widths = GetArrayFontWidths(out spaceWidth);
            defn.FontUnitsPerEm = header.UnitsPerEm;
            defn.SpaceWidthFontUnits = spaceWidth;
            defn.CMapEncoding = GetOptimumEncoding(cmap);
            defn.FontEncoding = ConvertCMapToFontEncoding(defn.CMapEncoding);
            
            return defn;
        }

        protected static CMapEncoding GetOptimumEncoding(CMAPTable cmap)
        {
            return CMapEncoding.Unicode_20;
        }

        protected static PDFFontWidths GetArrayFontWidths(out int spaceWidth)
        {
            spaceWidth = -1;
            return null;
        }

        protected static FontEncoding ConvertCMapToFontEncoding(CMapEncoding encoding)
        {
            return FontEncoding.UnicodeEncoding;
        }
        
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
        
    }
}