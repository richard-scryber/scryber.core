namespace Scryber.Drawing
{
    /// <summary>
    /// Base class that represents an image based on raster (pixel) data that can be used in a document
    /// </summary>
    public abstract class ImageRasterData : ImageData
    {

        #region public int PixelWidth {get;set;}

        private int _w;
        /// <summary>
        /// Gets the total number of pixels in one row of the image
        /// </summary>
        public int PixelWidth
        {
            get { return _w; }
            protected set { _w = value; }
        }

        #endregion

        #region public int PixelHeight {get;set;}

        private int _h;

        public int PixelHeight
        {
            get { return _h; }
            protected set { _h = value; }
        }

        #endregion


        #region  public int BitsPerColor {get;set;}

        private int _bitdepth;

        /// <summary>
        /// Gets or set the number of bits per single color sample
        /// </summary>
        public int BitsPerColor
        {
            get { return _bitdepth; }
            internal protected set { _bitdepth = value; }
        }

        #endregion


        #region public ColorSpace ColorSpace {get;set;}

        private ColorSpace _cs = ColorSpace.RGB;

        public ColorSpace ColorSpace
        {
            get { return _cs; }
            internal protected set { _cs = value; }
        }

        #endregion

        #region public int ColorsPerSample {get; set;}

        private int _colspsample;

        /// <summary>
        /// Gets the numbers of colour values that make up an individual pixel.
        /// </summary>
        public int ColorsPerSample
        {
            get { return _colspsample; }
            protected internal set { _colspsample = value; }
        }

        #endregion

        #region public int HorizontalResolution {get;set;}

        private int _hres;
        /// <summary>
        /// Gets the horizontal resolution of the image (pixels per inch)
        /// </summary>
        public int HorizontalResolution
        {
            get { return _hres; }
            internal protected set { _hres = value; }
        }

        #endregion

        #region public int VerticalResolution {get;set;}

        private int _vres;
        /// <summary>
        /// Gets the vertical resolution of the image (pixels per inch)
        /// </summary>
        public int VerticalResolution
        {
            get { return _vres; }
            internal protected set { _vres = value; }
        }

        #endregion

        #region public PDFUnit DisplayHeight {get;}

        /// <summary>
        /// Gets the natural height of the image in inches based on the number of pixels high and the vertical Resolution
        /// </summary>
        public Unit DisplayHeight
        {
            get { return new Unit(((double)this.PixelHeight) / (double)this.VerticalResolution, PageUnits.Inches); }
        }

        #endregion

        #region public PDFUnit DisplayWidth {get;}

        /// <summary>
        /// Gets the natural width of the image in inches based on the number of pixels wide and the horizontal Resolution
        /// </summary>
        public Unit DisplayWidth
        {
            get { return new Unit(((double)this.PixelWidth) / (double)this.HorizontalResolution, PageUnits.Inches); }
        }

        #endregion
        


        protected ImageRasterData(string source, int pixelWidth, int pixelHeight) : this(ObjectTypes.ImageData, source, pixelWidth, pixelHeight) { }
        
        protected ImageRasterData(ObjectType type, string source, int pixelWidth, int pixelHeight) : base(type, source)
        {
            this._w = pixelWidth;
            this._h = pixelHeight;
        }
        
        
        #region public override PDFSize GetSize()

        private const double DefaultResolution = 72.0;

        /// <summary>
        /// Gets the natural size of the image in page units
        /// </summary>
        /// <returns></returns>
        public override Size GetSize()
        {
            double hres = (double)this.HorizontalResolution;
            if (hres < 1.0)
                hres = DefaultResolution;
            double vres = (double)this.VerticalResolution;
            if (vres <= 1.0)
                vres = DefaultResolution;

            double w = ((double)this.PixelWidth) / hres;
            double h = ((double)this.PixelHeight) / vres;

            return new Size(new Unit(w, PageUnits.Inches), new Unit(h, PageUnits.Inches));
        }

        #endregion
        
    }
}