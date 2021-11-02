using System;
using Scryber.OpenType;
using Scryber.Options;
using Scryber.PDF.Resources;


namespace Scryber.Drawing
{
    public partial class FontFactory
    {
        
        #region public class FontReference
        
        /// <summary>
        /// A single font reference, to a Font Programme in a file
        /// </summary>
        public class FontReference
        {
            public string FamilyName { get; private set; }

            public FontStyle Style { get; private set; }

            public int Weight { get; private set; }
            
            protected IFontInfo Font { get; private set; }

            protected ITypefaceInfo Typeface { get; private set; }  
            
            /// <summary>
            /// Gets or sets the font definition for this reference
            /// </summary>
            internal PDFFontDefinition Definition { get; set; }
            
            internal bool IsLoaded { get; private set; }

            /// <summary>
            /// ivar for the thread locking value
            /// </summary>
            private object _loadLock;

            /// <summary>
            /// Returns the object to use for thread locking when loading the font definition
            /// </summary>
            public object LoadLock
            {
                get { return _loadLock; }
            }
            internal FontReference(IFontInfo info, ITypefaceInfo typeface)
                : this(info.FamilyName, GetStyleFromFont(info), (int)info.FontWeight, info, typeface)
            {
            }
            
            protected FontReference(string family, FontStyle style, int weight, IFontInfo info, ITypefaceInfo typeface)
            {
                this.FamilyName = family;
                this.Style = style;
                this.Weight = weight;
                this.Font = info;
                this.Typeface = typeface;
                this.IsLoaded = false;
                this._loadLock = new object();
            }
            
            //
            // methods
            //

            internal void LoadTypefaceDefinition(TypefaceReader withReader)
            {
                lock (_loadLock)
                {
                    if (this.IsLoaded == false)
                    {
                        this.DoLoadTypefaceDefinition(withReader);
                    }
                }
            }

            protected virtual void DoLoadTypefaceDefinition(TypefaceReader withreader)
            {
                ITypefaceFont loaded;
                if (this.Font is ITypefaceFont font)
                {
                    loaded = font;
                }
                else
                {
                    loaded = withreader.GetFont(this.Typeface, this.Font);
                    this.Font = loaded;
                }
                
                var data = loaded.GetFileData(DataFormat.TTF);
                
                if (this.Definition == null)
                {
                    
                    this.Definition =
                        PDFFontDefinition.LoadOpenTypeFontFile(data, this.FamilyName, this.Style, this.Weight, 0);
                }
                

                this.IsLoaded = true;
            }
        }
        
        #endregion
        
    }
}