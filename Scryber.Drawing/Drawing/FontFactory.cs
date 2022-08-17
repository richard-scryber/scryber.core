using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Scryber.OpenType;
using Scryber.Options;
using Scryber.PDF.Resources;
using Scryber.Utilities;

namespace Scryber.Drawing
{
    public partial class FontFactory
    {
        //
        // Inner classes
        //
        
        
        #region private class LinkedFontReference : FontReference
        
        /// <summary>
        /// A chain of font references for a single style that are sorted by weight - thin to black
        /// </summary>
        private class LinkedFontReference : FontReference
        {
            public LinkedFontReference Next { get; set; }

            internal LinkedFontReference(IFontInfo info, ITypefaceInfo typeface) : base(info, typeface)
            {
            }

            internal LinkedFontReference(string familyName, FontStyle style, int weight, ITypefaceFont font)
                : base(familyName, style, weight, font, null)
            {
                
            }

            internal void Append(LinkedFontReference reference)
            {
                if(reference.Weight < this.Weight)
                    throw new InvalidOperationException("Cannot append a weight that is less than this weight");

                if (null == this.Next)
                    this.Next = reference;
                else if (reference.Weight == this.Weight)
                {} //do nothing
                else if (this.Next.Weight > reference.Weight)
                {
                    reference.Next = this.Next;
                    this.Next = reference;
                }
                else
                {
                    this.Next.Append(reference);
                }
            }


            internal FontReference GetFontWithStyle(FontStyle style, int weight)
            {
                if (style == this.Style && this.Weight == weight)
                    return this;
                else if (null == this.Next)
                    return null;
                else
                    return this.Next.GetFontWithStyle(style, weight);
            }
            
            internal FontReference GetFontWithStyle(FontStyle style, int minWeight, int maxWeight)
            {
                if (style == this.Style && this.Weight >= minWeight && this.Weight <= maxWeight)
                    return this;
                else if (null == this.Next)
                    return null;
                else
                    return this.Next.GetFontWithStyle(style, minWeight, maxWeight);
            }
        }
        
        #endregion
        
        #region private class LinkedStyleReference
        
        /// <summary>
        /// A linked set of Styles for fonts, with an inner linked list of weights
        /// </summary>
        private class LinkedStyleReference  : IEnumerable<FontReference>
        {
            public string FamilyName { get; private set; }
            
            public FontStyle Style { get; private set; }
            
            public  LinkedFontReference Weights { get; private set; }
            
            public LinkedStyleReference NextStyle { get; private set; }

            public LinkedStyleReference(string familyName, FontStyle style)
            {
                this.FamilyName = familyName;
                this.Style = style;
            }

            public void Add(LinkedFontReference font)
            {
                if (font.Style == this.Style)
                {
                    this.AddToWeights(font);
                }
                else
                {
                    if (null == this.NextStyle)
                        this.NextStyle = new LinkedStyleReference(this.FamilyName, font.Style);
                    
                    this.NextStyle.Add(font);
                }
            }

            private void AddToWeights(LinkedFontReference font)
            {
                if (null == this.Weights)
                    this.Weights = font;
                else if (font.Weight < this.Weights.Weight)
                {
                    font.Append(this.Weights);
                    this.Weights = font;
                }
                else
                {
                    this.Weights.Append(font);
                }
            }
            
            public FontReference GetFont(FontStyle style, int weight, bool nearest = false)
            {
                if (this.Style == style)
                {
                    return this.GetWeight(weight, nearest);
                }
                else if (null != this.NextStyle)
                    return this.NextStyle.GetFont(style, weight, nearest);
                else
                    return null;
            }
            
            private FontReference GetWeight(int weight, bool nearest)
            {
                var curr = this.Weights;
                while (null != curr)
                {
                    if (curr.Weight == weight)
                        return curr;
                    
                    //If we are using nearest, then as we are sorted by
                    //ascending weights, if curr is weightier - send that instead.
                    else if (nearest && curr.Weight > weight)
                        return curr;

                    curr = curr.Next;
                }

                //Nothing matched
                return curr;
            }
            
            public IEnumerator<FontReference> GetEnumerator()
            {
                return new FontStyleReferenceEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
        
        #endregion
        
        #region private class FontStyleReferenceEnumerator
        
        /// <summary>
        /// Enumerates through a linked style reference
        /// </summary>
        private class FontStyleReferenceEnumerator : IEnumerator<FontReference>
        {
            private LinkedFontReference _curr;
            private LinkedStyleReference _root;

            public FontStyleReferenceEnumerator(LinkedStyleReference root)
            {
                _root = root;
                _curr = null;
            }

            
            public FontReference Current
            {
                get { return _curr; }
            }

            object IEnumerator.Current
            {
                get { return _curr; }
            }

            public void Dispose()
            {
                _curr = null;
                _root = null;
            }

            public bool MoveNext()
            {
                if (null == _root)
                    return false;

                if (null == _curr)
                    _curr = _root.Weights;
                else
                    _curr = _curr.Next;

                return null != _curr;
            }

            public void Reset()
            {
                _curr = null;
            }
        }
        
        #endregion
        
        #region private class FamilyReference
        
        /// <summary>
        /// A single family of fonts with the TypefaceInfo and the linked Styles and weights
        /// </summary>
        private class FamilyReference
        {
            public string FamilyName { get; private set; }

            public LinkedStyleReference Styles { get; private set; }

            public LinkedStyleReference this[FontStyle style]
            {
                get
                {
                    var curr = this.Styles;
                    while (null != curr)
                    {
                        if (curr.Style == style)
                            return curr;
                        else
                            curr = curr.NextStyle;
                    }

                    return null;
                }
            }
            public FamilyReference(string family)
            {
                this.FamilyName = family;
            }

            public FontReference Add(IFontInfo font, ITypefaceInfo typeface)
            {
                LinkedFontReference fontReference = new LinkedFontReference(font, typeface);
                if (null == this.Styles)
                    this.Styles = new LinkedStyleReference(this.FamilyName, GetStyleFromFont(font));
                
                this.Styles.Add(fontReference);

                return fontReference;
            }

            public FontReference Add(int fontWeight, FontStyle style, ITypefaceFont font)
            {
                LinkedFontReference fontReference = new LinkedFontReference(this.FamilyName, style, fontWeight, font);
                
                if (null == this.Styles)
                    this.Styles = new LinkedStyleReference(this.FamilyName, style);
                
                this.Styles.Add(fontReference);
                return fontReference;
            }

            internal bool TryGetFont(FontStyle style, int weight, out FontReference font)
            {
                if (null == this.Styles)
                {
                    font = null;
                    return false;
                }
                else
                {
                    font = this.Styles.GetFont(style, weight);
                    return null != font;
                }
            }
        }
        
        #endregion

        #region private class FamilyReferenceBag

        /// <summary>
        /// A collection of font families that can be checked and retrieved.
        /// </summary>
        private class FamilyReferenceBag
        {
            private readonly Dictionary<string, FamilyReference> _families;

            public FamilyReferenceBag()
            {
                _families = new Dictionary<string, FamilyReference>(StringComparer.OrdinalIgnoreCase);
            }

            public int Count
            {
                get { return _families.Count; }
            }

            public FamilyReference this[string family]
            {
                get
                {
                    FamilyReference fr;
                    if (_families.TryGetValue(family, out fr))
                        return fr;
                    else
                        return null;
                }
            }

            public FontReference this[string family, FontStyle style, int weight]
            {
                get
                {
                    var fam = this[family];
                    
                    if (null == fam) return null;
                    
                    if (fam.TryGetFont(style, weight, out var font))
                        return font;
                    else
                        return null;

                }
            }

            public int FillAllFamilies(List<String> familyNames)
            {
                int count = _families.Count;
                
                foreach (var name in _families.Keys)
                {
                    familyNames.Add(name);
                }

                return count;
            }

            public int AddTypefaceFonts(ITypefaceInfo info)
            {
                FamilyReference fam = null;
                string name;
                
                if (info.FontCount <= 0)
                    return 0;
                if (info.FontCount == 1)
                {
                    name = info.Fonts[0].FamilyName;
                    if (!_families.TryGetValue(name, out fam))
                    {
                        fam = new FamilyReference(name);
                        _families.Add(name, fam);
                    }
                    fam.Add(info.Fonts[0], info);
                    return 1;
                }

                foreach (var font in info.Fonts)
                {
                    name = font.FamilyName;
                    if (null == fam || fam.FamilyName != name)
                    {
                        if (!_families.TryGetValue(name, out fam))
                        {
                            fam = new FamilyReference(name);
                            _families.Add(name, fam);
                        }
                    }
                    fam.Add(font, info);
                }
                return info.FontCount;
            }

            public FontReference AddFont(ITypefaceInfo info, IFontInfo font, string familyName = null)
            {
                FamilyReference family;

                if (!_families.TryGetValue(familyName ?? font.FamilyName, out family))
                {
                    family = new FamilyReference(familyName ?? font.FamilyName);
                    _families.Add(familyName ?? family.FamilyName, family);
                }
                var reference = family.Add(font, info);
                return reference;
            }

            public FontReference AddFont(string familyName, int weight, FontStyle style, ITypefaceFont font)
            {
                FamilyReference family;
                if (!_families.TryGetValue(familyName, out family))
                {
                    family = new FamilyReference(familyName);
                    _families.Add(familyName, family);
                }

                var reference = family.Add(weight, style, font);
                return reference;
            }

            public void AddFontFamily(string name, FamilyReference family, bool overwrite = true)
            {
                if (_families.ContainsKey(name))
                {
                    if (overwrite)
                        _families.Remove(name);
                }
                
                _families.Add(name, family);
            }

            public bool TryGetFamily(string name, out FamilyReference found)
            {
                return _families.TryGetValue(name, out found);
            }
        }

        #endregion

        //
        // static variables
        //
        
        private static bool _initialized = false;
        private static object _initLock = new object();
        private static Exception _initEx = null;
        private static FamilyReferenceBag _system;
        private static FamilyReferenceBag _custom;
        private static FamilyReferenceBag _generic;
        private static FamilyReferenceBag _static;

        //
        // Initialization and loading of font families
        //

        public static FontDefinition GetFontDefinition(string fullName)
        {
            throw new NotImplementedException(
                "Need to check that this is still used, rather than the Font family, weight and style");
        }
        
        public static bool TryEnsureFont(IComponent mapper, ContextBase context, FontSource source, string familyName,
            Scryber.Drawing.FontStyle style, int weight, out FontDefinition definition)
        {
            throw new NotImplementedException("Need to check the validity of this, as may not be needed");
        }

        #region private static bool EnsureInitialized()

        /// <summary>
        /// Checks to make all the font families have been loaded in a thread safe way.
        /// </summary>
        /// <returns>true if the fonts are initialised, always true unless an exception is thrown</returns>
        /// <exception cref="PDFFontInitException">Thrown if there is an error when loading the fonts</exception>
        private static bool EnsureInitialized()
        {
            if (_initialized == false)
            {
                lock (_initLock)
                {
                    if (null != _initEx)
                    {
                        var msg = "The initialization of the fonts failed : " + _initEx.Message;
                        throw new PDFFontInitException(msg, _initEx);
                    }
                    if (_initialized == false)
                    {
                        try
                        {
                            UnsafeInitAllFontReferences();
                            _initialized = true;
                        }
                        catch (Exception ex)
                        {
                            _initEx = ex;
                            var msg = "The initialization of the fonts failed : " + _initEx.Message;
                            _initialized = false;
                            throw new PDFFontInitException(msg, _initEx);
                        }
                    }
                }
            }
            return _initialized;
        }
        
        #endregion

        #region private static void UnsafeInitAllFontReferences()
        
        private static void UnsafeInitAllFontReferences()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            FontOptions options;
            if (null == config || null == config.FontOptions)
                options = new FontOptions();
            else
                options = config.FontOptions;
            _system = UnsafeLoadSystemFonts(options);
            _custom = UnsafeLoadCustomFonts(options);
            _static = UnsafeLoadStaticFamilies(options);
            _generic = UnsafeLoadGenericFamilies(options);
        }
        
        #endregion
        
        #region private static string[] GetFontsDirectory()
        
        private const string FontsDirectoryName = "Fonts";
        /// <summary>
        /// Returns an array of known directories where fonts can be found.
        /// </summary>
        /// <returns></returns>
        private static string[] GetFontsDirectory()
        {

            List<string> paths = new List<string>();

            if (Scryber.Utilities.FrameworkHelper.IsMacOS())
            {
                var root = System.Environment.GetFolderPath(Environment.SpecialFolder.System) + "/Library/Fonts";
                paths.Add(root);
                paths.Add(System.Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
            }
            else
            {
                paths.Add(System.Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
            }

            string systempath = System.Environment.GetFolderPath(Environment.SpecialFolder.System);
            if (!systempath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                systempath = systempath + System.IO.Path.DirectorySeparatorChar.ToString();
            systempath = systempath + FontsDirectoryName;

            if (!paths.Contains(systempath, StringComparer.OrdinalIgnoreCase))
                paths.Add(systempath);


            return paths.ToArray();
        }
        
        #endregion

        #region private static string GetFullPath(string path)
        
        /// <summary>
        /// returns an expanded path to a file or directory, if the current path is not rooted.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetFullPath(string path)
        {
            if (!System.IO.Path.IsPathRooted(path))
            {
                path = System.IO.Path.Combine(System.Environment.CurrentDirectory, path);
            }
            return path;
        }
        
        #endregion
        
        #region private static FamilyReferenceBag UnsafeLoadSystemFonts(FontOptions options)
        
        /// <summary>
        /// Loads all the system fonts from the Fonts directory (and child directories), if the options stipulate we can UseSystemFonts
        /// </summary>
        /// <param name="options">The font configuration options</param>
        /// <returns>A bag of font references</returns>
        private static FamilyReferenceBag UnsafeLoadSystemFonts(FontOptions options)
        {
            FamilyReferenceBag system = new FamilyReferenceBag();
            if (options.UseSystemFonts)
            {
                using (TypefaceReader reader = new TypefaceReader())
                {
                    var paths = GetFontsDirectory();
                    
                    //foreach of the directories returned
                    foreach (var path in paths)
                    {
                        UnsafeReadFontsFromDirectory(reader, path, system);
                    }
                }
            }

            return system;
        }
        
        #endregion

        #region private static FamilyReferenceBag UnsafeLoadCustomFonts(FontOptions options)
        
        /// <summary>
        /// Loads any font files or default directories defined in the configuration
        /// </summary>
        /// <param name="options">The configuration options</param>
        /// <returns></returns>
        private static FamilyReferenceBag UnsafeLoadCustomFonts(FontOptions options)
        {
            FamilyReferenceBag custom = new FamilyReferenceBag();

            if (options.Register == null || options.Register.Length == 0) return custom;

            using (var reader = new TypefaceReader())
            {
                foreach (var known in options.Register)
                {
                    if (string.IsNullOrEmpty(known.File)) continue;
                    
                    var file = new FileInfo(GetFullPath(known.File));
                        
                    if(!file.Exists) continue;

                    var info = reader.ReadTypeface(file);
                    
                    if (null != info && string.IsNullOrEmpty(info.ErrorMessage))
                        custom.AddTypefaceFonts(info);
                }

                var defaultDir = options.DefaultDirectory;

                if (!string.IsNullOrEmpty(defaultDir))
                {
                    UnsafeReadFontsFromDirectory(reader, defaultDir, custom);
                }
            }

            return custom;
        }
        
        #endregion

        #region private static FamilyReferenceBag UnsafeLoadStaticFamilies(FontOptions options)
        
        private const int HelveticaSpaceWidthFu = 569;
        private const int TimesSpaceWidthFu = 512;
        private const int CourierSpaceWidthFu = 1228;
        private const int ZaphSpaceWidthFu = 544;
        private const int SymbolSpaceWidthFu = 512;
        
        /// <summary>
        /// Loads all the static families from the assembly resource file
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static FamilyReferenceBag UnsafeLoadStaticFamilies(FontOptions options)
        {
            var bag = new FamilyReferenceBag();

            var assm = typeof(FontFactory).Assembly;
            
            ITypefaceInfo found;
            using (var reader = new TypefaceReader())
            {

                // Courier

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Courier.CourierNew.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0], "Courier").Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("Fcour",
                        "Courier", "Courier", "Courier New", false, false, CourierSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Courier.CourierNewBold.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0], "Courier").Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("FcourBo",
                        "Courier-Bold", "Courier", "Courier New", true, false, CourierSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Courier.CourierNewBoldItalic.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0], "Courier").Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("FcourBoOb",
                        "Courier-BoldOblique", "Courier", "Courier New", true, true, CourierSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Courier.CourierNewItalic.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0], "Courier").Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("FcourOb",
                        "Courier-Oblique", "Courier", "Courier New", false, true, CourierSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);
                
                // Helvetica

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Helvetica.Helvetica.ttf", out found))
                    bag.AddFont(found, found.Fonts[0]).Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("Fhel",
                        "Helvetica", "Helvetica", false, false, HelveticaSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Helvetica.HelveticaBold.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0]).Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("FhelBl",
                        "Helvetica-Bold", "Helvetica", true, false, HelveticaSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Helvetica.HelveticaBoldOblique.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0]).Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("FhelObBl",
                        "Helvetica-BoldOblique", "Helvetica", true, true, HelveticaSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Helvetica.HelveticaOblique.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0]).Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("FhelOb",
                        "Helvetica-Oblique", "Helvetica", false, true, HelveticaSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);
                
                // Times
                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Times.timesNewRoman.ttf", out found))
                    bag.AddFont(found, found.Fonts[0]).Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("Ftimes",
                        "Times-Roman", "Times", false, false, TimesSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Times.timesNewRomanBold.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0]).Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("FtimesBo",
                        "Times-Bold", "Times", true, false, TimesSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Times.timesNewRomanBoldItalic.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0]).Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("FtimesBoIt",
                        "Times-BoldItalic", "Times", true, true, TimesSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Times.timesNewRomanItalic.ttf",
                    out found))
                    bag.AddFont(found, found.Fonts[0]).Definition = PDFOpenTypeFontDefinition.InitStdType1WinAnsi("FtimesIt",
                        "Times-Italic", "Times", false, true, TimesSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);
                
                // Symbol

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Symbol.Symbol.ttf", out found))
                    bag.AddFont(found, found.Fonts[0]).Definition =
                        PDFOpenTypeFontDefinition.InitStdSymbolType1WinAnsi("Fsym", "Symbol", SymbolSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);
                
                // Zapf

                if (TryReadFontBinary(reader, assm, "Scryber.Text._FontResources.Zaph.ZapfDingbats.ttf", out found))
                    bag.AddFont(found, found.Fonts[0]).Definition =
                        PDFOpenTypeFontDefinition.InitStdSymbolType1WinAnsi("Fzapf", "ZapfDingbats", ZaphSpaceWidthFu, found.Fonts[0] as IOpenTypeFont);
            }
            

            return bag;
        }
        
        #endregion

        #region private static FamilyReferenceBag UnsafeLoadGenericFamilies(FontOptions options)
        
        /// <summary>
        /// Adds the generic fonts to the a family reference bag and returns. The generic fonts are 'Sans-Serif', 'Serif' etc.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="PDFFontInitException">Thrown if the required fonts could not be found</exception>
        private static FamilyReferenceBag UnsafeLoadGenericFamilies(FontOptions options)
        {
            FamilyReference found;
            
            var genericBag = new FamilyReferenceBag();

            if (_static.TryGetFamily("Helvetica", out found))
            {
                genericBag.AddFontFamily("Sans-Serif", found);
                genericBag.AddFontFamily("Helvetica", found);
            }
            else
                throw new PDFFontInitException("Could not find or load the standard font family for Helvetica");

            if (_static.TryGetFamily("Times", out found) || _static.TryGetFamily("Times New Roman", out found))
            {
                genericBag.AddFontFamily("Serif", found);
                genericBag.AddFontFamily("Times", found);
            }
            else
                throw new PDFFontInitException("Could not find or load the standard font family for Times");

            if (_static.TryGetFamily("Courier", out found) || _static.TryGetFamily("Courier New", out found))
            {
                genericBag.AddFontFamily("Monospace", found);
                genericBag.AddFontFamily("Courier", found);
            }
            else
                throw new PDFFontInitException("Could not find or load the standard font family for Courier");

            if (null != _system && _system.TryGetFamily("Comic Sans MS", out found))
            {
                genericBag.AddFontFamily("Cursive", found);
            }
            else if(null != _custom && _custom.TryGetFamily("Comic Sans MS", out found))
            {
                genericBag.AddFontFamily("Cursive", found);
            }
            
            if (_static.TryGetFamily("Zapf Dingbats", out found))
            {
                genericBag.AddFontFamily("Zapf Dingbats", found);
            }
            
            if (_static.TryGetFamily("Symbol", out found))
            {
                genericBag.AddFontFamily("Symbol", found);
            }
            
            return genericBag;
        }
        
        #endregion
        
        #region private static bool TryReadFontBinary(TypefaceReader reader, Assembly assm, string name, out ITypefaceInfo info)
        
        private static bool TryReadFontBinary(TypefaceReader reader, Assembly assm, string name, out ITypefaceInfo info)
        {
            using (var stream = assm.GetManifestResourceStream(name))
            {
                if (null == stream)
                    throw new NullReferenceException("The binary resource stream for " + name +
                                                     " could not be found in the assembly " + assm.FullName);
                
                info = reader.ReadTypeface(stream, name);
                
                if (info == null || !string.IsNullOrEmpty(info.ErrorMessage))
                    return false;
                
                for (var i = 0; i < info.FontCount; i++)
                {
                    if (info.Fonts[i] is Scryber.OpenType.Utility.SingleTypefaceInfo sti)
                        stream.Position = sti.OffsetInFile;
                    
                    ITypefaceFont one = reader.GetFont(stream, name, info.Fonts[i]);
                    info.Fonts[i] = one;
                }

                return info.FontCount > 0;
            }
            
        }
        
        #endregion
        
        #region private static void UnsafeReadFontsFromDirectory(TypefaceReader reader, string path, FamilyReferenceBag into)
        
        /// <summary>
        /// Reads all the fonts from the specified directory path, and adds them to family reference bag.
        /// </summary>
        /// <param name="reader">The typeface reader to use</param>
        /// <param name="path">The path to read the files from</param>
        /// <param name="into">THe bag to add them too</param>
        private static void UnsafeReadFontsFromDirectory(TypefaceReader reader, string path, FamilyReferenceBag into)
        {
            if (string.IsNullOrEmpty(path)) return;
            
            var dir = new DirectoryInfo(path);
            
            if (!dir.Exists) return;
            
            //load all the fonts in this directory and any child directories
            //keeping a check on any errors
            ITypefaceInfo[] all = reader.ReadAllTypefaces(dir, null, true, true);
            foreach (var typefaceInfo in all)
            {
                if (null != typefaceInfo && string.IsNullOrEmpty(typefaceInfo.ErrorMessage))
                    into.AddTypefaceFonts(typefaceInfo);
            }
        }
        
        #endregion
        
        #region private static FontStyle GetStyleFromFont(IFontInfo font)
        
        private static FontStyle GetStyleFromFont(IFontInfo font)
        {
            return ((font.Selections & FontSelection.Italic) == FontSelection.Italic) ? FontStyle.Italic : FontStyle.Regular;
        }
        
        #endregion
        
        
        //
        // Font retrieval and access
        //

        #region public static IEnumerable<FontReference> GetAllFontsForFamilyAndStyle(string familyName, FontStyle style)
        
        public static IEnumerable<FontReference> GetAllFontsForFamilyAndStyle(string familyName, FontStyle style)
        {
            EnsureInitialized();
            List<FontReference> found = new List<FontReference>();

            var family = _custom[familyName];
            
            if(null != family)
            {
                var styleRef = family[style];
                if (null != styleRef)
                    found.AddRange(styleRef);
            }

            family = _static[familyName];

            if (null != family)
            {
                var styleRef = family[style];
                if (null != styleRef)
                    found.AddRange(styleRef);
            }

            family = _system[familyName];

            if (null != family)
            {
                var styleRef = family[style];
                if (null != styleRef)
                    found.AddRange(styleRef);
            }

            family = _generic[familyName];

            if (null != family)
            {
                var styleRef = family[style];
                if (null != styleRef)
                    found.AddRange(styleRef);
            }

            return found;
        }

        #endregion

        public static FontDefinition GetFontDefinition(Font forFont)
        {
            return GetFontDefinition(forFont.FamilyName, forFont.FontStyle, forFont.FontWeight);
        }

        public static FontDefinition GetFontDefinition(String family, FontStyle style, int weight,
            bool throwNotFound = true)
        {
            EnsureInitialized();
            
            if (string.IsNullOrEmpty(family))
                throw new ArgumentNullException(nameof(family));

            FontReference fref;
            
            fref = _custom[family, style, weight];

            if (null == fref)
                fref = _static[family, style, weight];

            if (null == fref)
                fref = _system[family, style, weight];
            
            if (null == fref)
                fref = _generic[family, style, weight];

            if (null == fref)
            {
                if (throwNotFound)
                    throw new PDFFontInitException("Could not find the font " + family + " with weight " + weight +
                                                   " and style " + style);
                else
                    return null;
            }
            
            if (fref.IsLoaded == false)
            {
                LoadFontDefinition(fref);
            }

            return fref.Definition;

        }

        private static void LoadFontDefinition(FontReference reference)
        {

            if (reference.IsLoaded == false)
            {
                using (var reader = new TypefaceReader())
                {
                    reference.LoadTypefaceDefinition(reader);
                }
            }

        }

        public static IEnumerable<FontDefinition> LoadFontDefinitions(string family, int weight, FontStyle style, string source, Stream fromStream)
        {
            EnsureInitialized();
            
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));
            lock (_initLock)
            {
                using (var reader = new TypefaceReader())
                {
                    IEnumerable<ITypefaceFont> fonts;

                    if (fromStream.CanSeek == false)
                    {
                        using (var seekable = GetSeekableStream(fromStream))
                        {
                            fonts = reader.GetFonts(seekable, source);
                        }
                    }
                    else
                        fonts = reader.GetFonts(fromStream, source);

                    List<FontDefinition> all = new List<FontDefinition>();
                    foreach (var font in fonts)
                    {
                        if (font.IsValid)
                        {
                            var reference = _custom.AddFont(family, weight, style, font);
                            FontDefinition matched = PDFOpenTypeFontDefinition.Load(font, family, style, weight, null);
                            reference.Definition = matched;
                            all.Add(matched);
                        }
                    }

                    return all;
                }
            }
        }

        private static Stream GetSeekableStream(Stream basicStream)
        {
            //var count = 2048;
            //var buffer = new byte[count];

            var ms = new MemoryStream();
            basicStream.CopyTo(ms);
            
            // while ((count = basicStream.Read(buffer, 0, count)) > 0)
            // {
            //     ms.Write(buffer, 0, count);
            // }
            //
            ms.Position = 0;
            
            return ms;
        }
    }
}