using Scryber.OpenType;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.Drawing
{
    public abstract class FontDefinition
    {
        
        #region public FontType SubType {get; set;}

        private FontType _subtype = FontType.Type1;
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

		public FontEncoding FontEncoding
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
        public OpenType.SubTables.CMapEncoding CMapEncoding
        {
            get { return _ttfEnc; }
            protected set { _ttfEnc = value; }
        }

        #endregion

        #region public PDFFontWidths Widths {get;set;}

        private PDFFontWidths _widths;

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
            set { _filepath = value; }
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

        /// <summary>
        /// Gets or sets teh Bold flag on this font definition
        /// </summary>
        public bool Bold
        {
            get { return _weight >= FontWeights.Bold; }
        }

        #endregion

        #region public int Weight {get;set;}

        private int _weight = FontWeights.Regular;

        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
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
        
        
        #region public bool CanMeasureStrings {get;}

        /// <summary>
        /// Returns true if this definition supports string measurement.
        /// </summary>
        /// <remarks>A definition can quickly measure strings if it has a reference to the OpenType font file</remarks>
        public virtual bool CanMeasureStrings
        {
            get { return false; }
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
            set { _embed = value; }
        }

        #endregion

        #region public bool IsStandard {get;}

        private bool _std;

        /// <summary>
        /// Gets the flag that identifies if this font is a standard font (does not have a PDFFontDescriptor or PDFWidths)
        /// </summary>
        public bool IsStandard
        {
            get { return _std; }
            set { _std = value; }
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
        public virtual bool IsUnicode
        {
            get
            {
                if (_isunicode.HasValue == false)
                    _isunicode = (this.IsStandard == false && this.FontEncoding == FontEncoding.UnicodeEncoding);
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
                    string fn = Font.GetFullName(this.Family,this.Bold,this.Italic);
                    
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
        public virtual string PostscriptFontName
        {
            get
            {
                return this.BaseType;
            }
        }

        #endregion

        #region public double SpaceWidthFontUnits {get;set;}

        private double _spaceWidthFU;

        public double SpaceWidthFontUnits
        {
            get { return _spaceWidthFU; }
            protected set { _spaceWidthFU = value; }
        }

        #endregion

        #region public double FontUnitsPerEm { get;set; }

        private double _unitsPerEm;

        public double FontUnitsPerEm
        {
            get { return _unitsPerEm; }
            set { _unitsPerEm = value; }
        }

        #endregion
        
        
        //
        // abstract methods
        //

        public abstract Size MeasureStringWidth(string chars, int startOffset, double fontSizePts,
            double availableWidth,
            bool wordBoundary, out int charsFitted);
        
        public  abstract Size MeasureStringWidth(string chars, int startOffset, double fontSizePts,
            double availableWidth, double? wordSpace, double? charSpace, double? hScale, bool vertical,
            bool wordBoundary, out int charsFitted);

        public abstract LineSize MeasureStringWidth(string chars, int startOffset, double fontSizePts,
            double availableWidth, TypeMeasureOptions options);

        public abstract TypeMeasureOptions GetMeasurementOptions(bool wordBoundary,
            double? wordSpace = null, double? charSpace = null, double? hScale = null, bool? vertical = null);

        public abstract FontMetrics GetFontMetrics(Unit fontSize);

        public abstract PDFObjectRef RenderToPDF(string name, PDFFontWidths widths, ContextBase context,
            PDFWriter writer);
    }
}