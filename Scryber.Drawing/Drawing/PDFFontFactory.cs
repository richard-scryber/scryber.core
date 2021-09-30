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

#define FALLBACKTOREGULAR //comment out if we should not fall back to regular font style if the bold / italic variants are not found.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
//using System.Drawing;
using System.Drawing.Text;
using Scryber.Resources;
using Scryber.Configuration;
using System.Reflection;
using Scryber.OpenType;
using System.IO;
using System.CodeDom.Compiler;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Net.Http;

namespace Scryber.Drawing
{
    //TODO: change the enumeration names to PDF...

     
    public static class PDFFontFactory
    {
        //
        // Inner classes
        //

        #region private abstract class FontReference
        /// <summary>
        /// A unique font reference
        /// </summary>
        private abstract class FontReference
        {
            internal string FamilyName { get; private set; }
            internal Scryber.Drawing.FontStyle Style { get; private set; }
            internal int Weight { get; private set; }
            internal string FilePath { get; private set; }
            internal byte[] FileData { get; private set; }
            internal int FileHeadOffset { get; private set; }

            internal PDFFontDefinition Definition { get; set; }

            internal bool LoadedDefintion { get { return null != Definition; } }

            //internal FontFamily SystemFamily { get; set; }

            internal object LoadLock { get; private set; }
            
            internal FontReference(string name, Scryber.Drawing.FontStyle style, int weight, string path, int fileHeadOffset)
            {
                //this.SystemFamily = family;
                this.FamilyName = name;
                this.Style = style;
                this.Weight = weight;
                this.FilePath = path;
                this.FileHeadOffset = fileHeadOffset;
                this.LoadLock = new object();
            }

            internal FontReference(string name, Scryber.Drawing.FontStyle style, int weight, byte[] data, int fileHeadOffset)
            {
                //this.SystemFamily = family;
                this.FamilyName = name;
                this.Style = style;
                this.Weight = weight;
                this.FilePath = null;
                this.LoadLock = new object();
                this.FileHeadOffset = fileHeadOffset;
                this.FileData = data;
            }
        }

        #endregion

        #region private class LinkedFontReference : FontReference

        /// <summary>
        /// A unique font reference that can be chained
        /// </summary>
        private class LinkedFontReference : FontReference
        {
            internal LinkedFontReference Next { get; set; }

            internal LinkedFontReference(string familyname, Scryber.Drawing.FontStyle style, int weight, string path, int fileHeadOffset)
                : base(familyname, style, weight, path, fileHeadOffset)
            {
            }

            internal LinkedFontReference(string familyname, Scryber.Drawing.FontStyle style, int weight, byte[] data, int fileHeadOffset)
                : base(familyname, style, weight, data, fileHeadOffset)
            {
            }

            internal void Append(LinkedFontReference reference)
            {
                if (reference.Weight < this.Weight)
                    throw new InvalidOperationException("Cannot append a weight that is less than this weight");

                if (null == this.Next)
                {
                    this.Next = reference;
                }
                else if(reference.Weight == this.Weight)
                {
                    //Do nothing - we are the same
                }
                else if (this.Next.Weight > reference.Weight)
                {
                    reference.Next = this.Next;
                    this.Next = reference;
                }
                else
                   this.Next.Append(reference);
            }

            internal LinkedFontReference GetStyle(Scryber.Drawing.FontStyle style, int weight)
            {
                if (style == this.Style && this.Weight == weight)
                    return this;
                else if (null == this.Next)
                    return null;
                else
                    return this.Next.GetStyle(style, weight);
            }

            internal LinkedFontReference GetStyle(Scryber.Drawing.FontStyle style, int minWeight, int maxWeight)
            {
                if (style == this.Style && this.Weight >= minWeight && this.Weight <= maxWeight)
                    return this;
                else if (null == this.Next)
                    return null;
                else
                    return this.Next.GetStyle(style, minWeight, maxWeight);
            }
        }

        #endregion

        /// <summary>
        /// A linked set of styled families with a linked list of weights
        /// </summary>
        private class LinkedFamilyStyledReference
        {
            internal FamilyReference Family { get; private set; }

            internal FontStyle FontStyle { get; private set; }

            internal LinkedFontReference Weights { get; private set; }

            internal LinkedFamilyStyledReference Next { get; private set; }

            public LinkedFamilyStyledReference(FamilyReference family, FontStyle style)
            {
                this.Family = family;
                this.FontStyle = style;
            }

            public void Add(LinkedFontReference font)
            {
                if (font.FamilyName != this.Family.FamilyName)
                    throw new InvalidOperationException("The family names do not match.");

                if (font.Style == this.FontStyle)
                {
                    //We have a matching style so add to the weights
                    this.AddToWeights(font);
                }
                else
                {
                    if (null == this.Next)
                    {
                        //We dont have a matching style, and this is not the right style
                        //So create a new one.
                        LinkedFamilyStyledReference styled = new LinkedFamilyStyledReference(this.Family, font.Style);
                        this.Next = styled;
                    }

                    this.Next.Add(font);
                }
            }

            private void AddToWeights(LinkedFontReference font)
            {
                //Keep an ordered list of font weights
                //from minimum to maximum.

                if (null == this.Weights)
                    this.Weights = font;

                else if (font.Weight < this.Weights.Weight)
                {
                    font.Append(this.Weights);
                    this.Weights = font;
                }
                else
                    this.Weights.Append(font);
            }

            public FontReference GetFont(FontStyle style, int weight, bool nearest = false)
            {
                if (this.FontStyle == style)
                {
                    return this.GetWeight(weight, nearest);
                }
                else if (null != this.Next)
                    return this.Next.GetFont(style, weight, nearest);
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
                    //assending weights, if curr is weightier - send that instead.
                    else if (nearest && curr.Weight > weight)
                        return curr;

                    curr = curr.Next;
                }

                //Nothing matched
                return curr;
            }

        }

        #region private class FamilyReference

        /// <summary>
        /// A family of Font styled references, that each contain weights
        /// </summary>
        private class FamilyReference
        {

            internal string FamilyName { get; private set; }

            private LinkedFamilyStyledReference _first;

            internal LinkedFamilyStyledReference First
            {
                get { return _first; }
            }

            internal FontReference this[Scryber.Drawing.FontStyle style, int weight]
            {
                get
                {
                    if (null == _first)
                        return null;
                    else
                    {
                        var found = _first.GetFont(style, weight);
                        return found;
                    }
                }
            }

            internal FamilyReference(string name)
            {
                this.FamilyName = name;
            }

            internal FontReference Add(Scryber.Drawing.FontStyle style, int weight, string filepath, int fileHeadOffset)
            {

                LinkedFontReference fontref = new LinkedFontReference(this.FamilyName, style, weight, filepath, fileHeadOffset);

                if (null == _first)
                    _first = new LinkedFamilyStyledReference(this, style);

                _first.Add(fontref);

                return fontref;
            }

            internal FontReference Add(Scryber.Drawing.FontStyle style, int weight, byte[] data, int fileHeadOffset)
            {
                LinkedFontReference fontref = new LinkedFontReference(this.FamilyName, style, weight, data, fileHeadOffset);

                if (null == _first)
                    _first = new LinkedFamilyStyledReference(this, style);

                _first.Add(fontref);

                return fontref;
            }

            internal bool TryGetFont(Scryber.Drawing.FontStyle style, int weight, out FontReference font)
            {
                var found = _first;
                if(null == found)
                {
                    font = null;
                    return false;
                }
                else
                {
                    font = found.GetFont(style, weight);
                    return null != font;
                }
                
            }
        }

        #endregion

        #region private class FamilyReferenceBag

        /// <summary>
        /// A collection of Font Family references that can be acessed by family name, style and weight. 
        /// Does not throw an exception if the font does not exist.
        /// </summary>
        private class FamilyReferenceBag
        {
            private Dictionary<string, FamilyReference> _families;
            

            internal FamilyReferenceBag()
            {
                _families = new Dictionary<string, FamilyReference>(StringComparer.OrdinalIgnoreCase);
            }

            internal FamilyReference this[string family]
            {
                get
                {
                    FamilyReference fm;
                    if (_families.TryGetValue(family, out fm))
                        return fm;
                    else
                        return null;
                }
            }

            internal FontReference this[string family, Scryber.Drawing.FontStyle style, int weight]
            {
                get
                {
                    FontReference fnt;
                    FamilyReference fam;
                    if (_families.TryGetValue(family, out fam) && fam.TryGetFont(style, weight, out fnt))
                    {
                        return fnt;
                    }
                    else
                        fnt = null;

                    return fnt;
                }
            }

            internal bool HasFamily(string family)
            {
                FamilyReference fam;
                if (_families.TryGetValue(family, out fam))
                    return true;
                else
                    return false;
            }

            internal void FillAllFamilies(List<string> families)
            {
                foreach (string name in this._families.Keys)
                {
                    families.Add(name);
                }
            }

            internal virtual FontReference AddFontFile(string family, Scryber.Drawing.FontStyle style, int weight, string path, int fileHeadOffset)
            {
                FamilyReference fam;
                if (_families.TryGetValue(family, out fam) == false)
                {
                    fam = new FamilyReference(family);
                    _families.Add(family, fam);
                }
                return fam.Add(style, weight, path, fileHeadOffset);
            }

            internal FontReference AddFontResource(string family, Scryber.Drawing.FontStyle style, int weight, byte[] data, int fileHeadOffset)
            {
                FamilyReference fam;
                if (_families.TryGetValue(family, out fam) == false)
                {
                    fam = new FamilyReference(family);
                    _families.Add(family, fam);
                }
                return fam.Add(style, weight, data, fileHeadOffset);
            }

            internal void AddFontFamily(string name, FamilyReference reference)
            {
                _families.Add(name, reference);
            }

            internal bool TryGetFamily(string name, out FamilyReference reference)
            {
                return _families.TryGetValue(name, out reference);
            }

            internal bool TryGetFont(string name, Scryber.Drawing.FontStyle style, int weight, out FontReference reference)
            {
                reference = null;
                FamilyReference family;
                if (_families.TryGetValue(name, out family) && family.TryGetFont(style, weight, out reference))
                    return true;
                else
                    return false;
            }
        }

        #endregion

  


        //
        //Static variables
        //

        
        private static FamilyReferenceBag _systemfamilies;
        private static FamilyReferenceBag _customfamilies;
        private static FamilyReferenceBag _genericfamilies;
        private static FamilyReferenceBag _staticfamilies;

        private static Dictionary<string, PDFFontDefinition> _remotefamilies;

        private static bool _init;
        private static Exception _initex;
        private static object _initlock;

        //
        // ..ctor
        //

        #region static PDFFontFactory()

        static PDFFontFactory()
        {
            _systemfamilies = null;
            _customfamilies = null;
            _staticfamilies = null;
            _init = false;
            _initlock = new object();
            _initex = null;
        }

        #endregion

        //
        // public retrieval methods
        //

        #region  public static string GetSystemFontFamilyNameForStandardFont(string pdffamilyname)

        /// <summary>
        /// Gets the system name of the font for the PDF family name
        /// </summary>
        /// <param name="pdffamilyname">The name of the PDF font family</param>
        /// <returns></returns>
        public static string GetSystemFontFamilyNameForStandardFont(string pdffamilyname)
        {
            if (string.IsNullOrEmpty(pdffamilyname))
                throw new ArgumentNullException("pdfFamilyName");

            string name = pdffamilyname.Replace(" ", "");
            string newName;
            switch (name.ToLower())
            {
                case "courier":
                    newName = "Courier New";
                    break;
                case "times":
                    newName = "Times New Roman";
                    break;
                case "arial":
                case "helvetica":
                    newName = "Arial";
                    break;
                case "symbol":
                    newName = "Symbol Regular";
                    break;
                case "zapfdingbats":
                    newName = "Zapf Dingbats";
                    break;
                default:
                    throw new ArgumentException(String.Format(Errors.FontNotFound, name),"font");

            }
            return newName;
        }

        #endregion


        #region public static PDFFontDefinition GetFontDefinition(PDFFont font)

        public static PDFFontDefinition GetFontDefinition(PDFFont font, bool throwNotFound = true)
        {
            //Make sure we are initialized and OK
            AssertInitialized();

            Scryber.Drawing.FontStyle fs = font.FontStyle;
            int weight = font.FontWeight;

            PDFFontSelector selector = font.Selector;

            while (null != selector)
            {
                var found = GetFontDefinition(selector.FamilyName, fs, weight, false);
                if (null != found)
                    return found;

                selector = selector.Next;
            }

            if (throwNotFound)
                throw new NullReferenceException(String.Format(Errors.FontNotFound, font.Selector.ToString() + " " + fs.ToString()));
            else
                return null;
        }

        #endregion

        #region public static bool IsFontDefined(string family, System.Drawing.FontStyle style)
        /// <summary>
        /// Returns true if the available fonts contain one with the specified family and style
        /// </summary>
        /// <param name="family">The name of the font family (case insensitive)</param>
        /// <param name="style">GThe required stlye of the font</param>
        /// <returns>True if available, or false</returns>
        public static bool IsFontDefined(string family, Scryber.Drawing.FontStyle style, int weight)
        {
            //Make sure we are initialized and OK
            AssertInitialized();
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            bool usesystem = config.FontOptions.UseSystemFonts;

            if (string.IsNullOrEmpty(family))
                throw new ArgumentNullException("family");




            FontReference fref = _customfamilies[family, style, weight];

            if (null == fref && usesystem)
                fref = _systemfamilies[family, style, weight];

            if (null == fref)
                fref = _genericfamilies[family, style, weight];

#if FALLBACKTOREGULAR


            if (null == fref && weight != FontWeights.Regular && !PDFFont.IsStandardFontFamily(family))
            {
                return IsFontDefined(family, style, FontWeights.Regular);
            }
#endif

            if (null == fref)
            {
                //check if we are a standard font (that is not available in the custom or system families
                if (PDFFont.IsStandardFontFamily(family))
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        #endregion

        #region public static string[] GetAllCustomFontFamilies()

        public static string[] GetAllCustomFontFamilies()
        {
            AssertInitialized();
            List<string> all = new List<string>();
            if (_customfamilies != null)
                _customfamilies.FillAllFamilies(all);
            return all.ToArray();
        }

        #endregion

        #region public static string[] GetAllSystemFontFamilies()

        public static string[] GetAllSystemFontFamilies()
        {
            AssertInitialized();
            List<string> all = new List<string>();
            if (_systemfamilies != null)
                _systemfamilies.FillAllFamilies(all);
            return all.ToArray();


        }

        #endregion

        #region public static string[] GetAllGenericFontFamilies()

        public static string[] GetAllGenericFontFamilies()
        {
            string[] found = new string[] { "Sans-Serif", "Serif", "Cursive", "Monospace" };
            return found;
        }

        #endregion

        #region public static PDFFontDefinition GetFontDefinition(string fullname)

        public static PDFFontDefinition GetFontDefinition(string fullname)
        {
            int pos = fullname.IndexOf(",");
            Scryber.Drawing.FontStyle fontstyle = Scryber.Drawing.FontStyle.Regular;
            int weight = FontWeights.Regular;

            string family;
            if (pos > 0)
            {
                family = fullname.Substring(0, pos).Trim();
                string style = fullname.Substring(pos);

                if (style.IndexOf("bold", StringComparison.OrdinalIgnoreCase) > -1)
                    weight = FontWeights.Bold;
                else if (style.IndexOf("light", StringComparison.OrdinalIgnoreCase) > -1)
                    weight = FontWeights.Light;
                else if (style.IndexOf("black", StringComparison.OrdinalIgnoreCase) > -1)
                    weight = FontWeights.Black;

                if (style.IndexOf("italic", StringComparison.OrdinalIgnoreCase) > -1)
                    fontstyle = Scryber.Drawing.FontStyle.Italic;
                else if (style.IndexOf("oblique", StringComparison.OrdinalIgnoreCase) > -1)
                    fontstyle = FontStyle.Oblique;
            }
            else
                family = fullname.Trim();

            return GetFontDefinition(family, fontstyle, weight);
            
        }

        #endregion

        #region public static PDFFontDefinition GetFontDefinition(string family, System.Drawing.FontStyle style, bool throwNotFound = true)

        /// <summary>
        /// Gets the PDFFontDefinition for the specified famil and style
        /// </summary>
        /// <param name="family"></param>
        /// <param name="style"></param>
        /// <param name="throwNotFound">If true (default) then if the font cannot be found an exception will be raised.
        /// If not, then null will be returned.</param>
        /// <returns></returns>
        public static PDFFontDefinition GetFontDefinition(string family, Scryber.Drawing.FontStyle style, int weight, bool throwNotFound = true)
        {
            //Make sure we are initialized and OK
            AssertInitialized();

            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            bool usesystem = config.FontOptions.UseSystemFonts;
            bool usesubstitute = config.FontOptions.FontSubstitution;
            
            if (string.IsNullOrEmpty(family))
                throw new ArgumentNullException("family");


            if (null == _customfamilies)
                throw new InvalidOperationException("The custom families is not set");
            
            FontReference fref = _customfamilies[family, style, weight];
            
            
            if (null == fref && usesystem)
                fref = _systemfamilies[family, style, weight];
            if (null == fref)
                fref = _genericfamilies[family, style, weight];

            if (null == fref)
            {

                if (usesubstitute && weight != FontWeights.Regular && !PDFFont.IsStandardFontFamily(family))
                    return GetFontDefinition(family, style, FontWeights.Regular, throwNotFound);

                //We dont have the explicit font so if we should substitue then 
                //try to find the family and return that otherwise use courier.
                if (usesubstitute)
                {
                    FamilyReference fam = _customfamilies[family];
                    if (null == fam && usesystem)
                        fam = _systemfamilies[family];

                    if (null != fam)
                    {
                        fref = fam[FontStyle.Regular, FontWeights.Regular];
                    }
                    //Fallback - use courier font definition
                    if (null == fref)
                    {
                        if (throwNotFound)
                            throw new NullReferenceException(String.Format(Errors.FontNotFound, family + " " + style.ToString()));
                        else
                            return null;
                    }
                }
                else
                {
                    if (throwNotFound)
                        throw new NullReferenceException(String.Format(Errors.FontNotFound, family + " " + style.ToString()));
                    else
                        return null;
                }
            }

            lock (fref.LoadLock)
            {
                if(fref.LoadedDefintion == false)
                {
                    PDFFontDefinition defn;
                    
                    if (null == fref.FileData)
                        //Load from a file path if we don't have the binary data
                        defn = PDFFontDefinition.LoadOpenTypeFontFile(fref.FilePath, family, style, weight, fref.FileHeadOffset);
                    else
                        //We do have the binary data so load from this
                        defn = PDFFontDefinition.LoadOpenTypeFontFile(fref.FileData, family, style, weight, fref.FileHeadOffset);

                    fref.Definition = defn;
                }
            }
            return fref.Definition;
        }

        #endregion

        //
        // private methods
        //

        #region private static void AssertInitialized()

        /// <summary>
        /// Thread safe intiailization. Must be called before the custom and system families are used
        /// </summary>
        private static void AssertInitialized()
        {

            if (_init == false) //First check outside a lock
            {
                lock (_initlock) //inside is thread safe
                {
                    //if we have an error then re-throw
                    if(null != _initex)
                    {
                        string msg = String.Format(Errors.CouldNotInitializeTheFonts, _initex.Message);
                        throw new System.Configuration.ConfigurationErrorsException(msg, _initex);
                    }
                    else if (_init == false) //we have not started the initialization
                    {
                        try
                        {
                            //Set init here. We only want to do it once, even if if fails
                            _systemfamilies = LoadSystemFonts();
                            _customfamilies = LoadCustomFamilies();
                            _staticfamilies = LoadStaticFamilies();
                            _genericfamilies = LoadGenericFamilies();
                            _remotefamilies = new Dictionary<string, PDFFontDefinition>(StringComparer.InvariantCultureIgnoreCase);

                            _init = true;

                        }
                        catch (Exception ex)
                        {
                            _initex = ex;
                            string msg = String.Format(Errors.CouldNotInitializeTheFonts, ex.Message);
                            throw new System.Configuration.ConfigurationErrorsException(msg, ex);
                        }
                    }
                }
            }
            else if (null != _initex)
            {
                string msg = String.Format(Errors.CouldNotInitializeTheFonts, _initex.Message);
                throw new System.Configuration.ConfigurationErrorsException(msg, _initex);
            }
        }

        #endregion

        #region private static string[] GetFontsDirectory()

        private static string[] _fontspaths = null;
        private const string FontsDirectoryName = "Fonts";

        private static string[] GetFontsDirectory()
        {
            
            if (null == _fontspaths)
            {
                List<string> paths = new List<string>();

                if (Scryber.Utilities.FrameworkHelper.IsMacOS())
                {
                    var root = System.Environment.GetFolderPath(Environment.SpecialFolder.System) + "/Library/Fonts";
                    paths.Add(root);
                    //TODO: Add child folders

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


                _fontspaths = paths.ToArray();
            }
            return _fontspaths;
        }

        #endregion

        #region private static FamilyReferenceBag LoadGenericFamilies()

        private static FamilyReferenceBag LoadGenericFamilies()
        {
            
            
            FamilyReferenceBag genericBag = new FamilyReferenceBag();

            FamilyReference found;


            if (_staticfamilies.TryGetFamily("Helvetica", out found))
            {
                genericBag.AddFontFamily("Sans-Serif", found);
                genericBag.AddFontFamily("Helvetica", found);
            }
            else
                throw new ConfigurationErrorsException("Could not find or load the standard font family for Helvetica");


            if (_staticfamilies.TryGetFamily("Times", out found) || _staticfamilies.TryGetFamily("Times New Roman", out found))
            {
                genericBag.AddFontFamily("Serif", found);
                genericBag.AddFontFamily("Times", found);
            }
            else
                throw new ConfigurationErrorsException("Could not find or load the standard font family for Times");


            if (_staticfamilies.TryGetFamily("Courier", out found))
            {
                genericBag.AddFontFamily("Monospace", found);
                genericBag.AddFontFamily("Courier", found);
            }
            else
                throw new ConfigurationErrorsException("Could not find or load the standard font family for Courier");

            if (null != _systemfamilies && _systemfamilies.TryGetFamily("Comic Sans MS", out found))
            {
                genericBag.AddFontFamily("Cursive", found);
            }
            else if(null != _customfamilies && _customfamilies.TryGetFamily("Comic Sans MS", out found))
            {
                genericBag.AddFontFamily("Cursive", found);
            }


            if (_staticfamilies.TryGetFamily("Zapf Dingbats", out found))
            {
                genericBag.AddFontFamily("Zapf Dingbats", found);
            }
            else
                throw new ConfigurationErrorsException("Could not find or load the standard font family for Zaph Dingbats");


            if (_staticfamilies.TryGetFamily("Symbol", out found))
            {
                genericBag.AddFontFamily("Symbol", found);
            }
            else
                throw new ConfigurationErrorsException("Could not find or load the standard font family for Symbol");

            return genericBag;
        }

        #endregion

        #region private static FamilyReferenceBag LoadSystemFonts()

        /// <summary>
        /// Loads all the system TrueType fonts from the system fonts folder
        /// </summary>
        /// <returns></returns>
        private static FamilyReferenceBag LoadSystemFonts()
        {
            
            //InstalledFontCollection install = new InstalledFontCollection();
            FamilyReferenceBag bag = new FamilyReferenceBag();
            
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            //Check to see if we are allowed to use the system fonts
            if (config.FontOptions.UseSystemFonts)
            {
                try
                {
                    string[] paths = GetFontsDirectory();
                    foreach (var path in paths)
                    {


                        if (!string.IsNullOrEmpty(path))
                        {
                            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
                            if (dir.Exists)
                            {
                                Scryber.OpenType.TTFRef[] all = Scryber.OpenType.TTFRef.LoadRefs(dir);
                                foreach (Scryber.OpenType.TTFRef item in all)
                                {
                                    if (item.IsValid)
                                    {
                                        bag.AddFontFile(item.FamilyName, GetFontStyle(item.FontSelection, item.FontWeight), GetFontWeight(item.FontSelection, item.FontWeight), item.FullPath, item.HeadOffset);
                                    }
                                }

                            }
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    throw new Exception("Could not load the system fonts", ex);
                }
            }
            return bag;
        }

        
        #endregion

        #region private static FamilyReferenceBag LoadCustomFamilies()

        /// <summary>
        /// Loads all the custom fonts definied in the configuration file
        /// </summary>
        /// <returns></returns>
        private static FamilyReferenceBag LoadCustomFamilies()
        {
            //PrivateFontCollection priv = new PrivateFontCollection();
            FamilyReferenceBag bag = new FamilyReferenceBag();

            //Load the explicit entries first
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            var known = config.FontOptions.Register;
            
            if ((null != known) && (known.Length > 0))
            {
                //For each of the configuration entries either load from the resources, or load from the file
                Dictionary<string, System.Resources.ResourceManager> mgrs = new Dictionary<string, System.Resources.ResourceManager>(StringComparer.OrdinalIgnoreCase);

                foreach (var map in known)
                {
                    string family = map.Family;
                    string style = map.Style;
                    int weight = map.Weight;

                   
                    if (!string.IsNullOrEmpty(map.File))
                    {
                        //Load from a file
                        string path = map.File;
                        FontStyle face = FontStyle.Regular;

                        try
                        {
                            path = GetFullPath(path);
                        }
                        catch (Exception ex)
                        {
                            throw new ConfigurationErrorsException(String.Format(Errors.CouldNotLoadTheFontFile, path, ex.Message), ex);
                        }

                        if(weight <= 0)
                        {
                            if(!string.IsNullOrEmpty(style))
                            {
                                if (style.IndexOf("bold", StringComparison.OrdinalIgnoreCase) > -1)
                                    weight = FontWeights.Bold;
                                else if (style.IndexOf("black", StringComparison.OrdinalIgnoreCase) > -1)
                                    weight = FontWeights.Black;
                                else if (style.IndexOf("light", StringComparison.OrdinalIgnoreCase) > -1)
                                    weight = FontWeights.Light;
                            }
                        }

                        if(!string.IsNullOrEmpty(style))
                        {
                            if (style.IndexOf("italic", StringComparison.OrdinalIgnoreCase) > -1)
                                face = FontStyle.Italic;
                            else if (style.IndexOf("oblique", StringComparison.OrdinalIgnoreCase) > -1)
                                face = FontStyle.Oblique;
                        }


                        ///Only support ttf not ttc
                        bag.AddFontFile(family, face, weight, path, 0);
                    }
                    else
                    {
                        throw new ConfigurationErrorsException(String.Format(Errors.FontMappingMustHaveFilePathOrResourceName, family));
                    }
                }

            }

            string defaultdir = config.FontOptions.DefaultDirectory;

            

            if (!string.IsNullOrEmpty(defaultdir))
            {
                //Make sure we are not just referencing the fonts directory.
                var system = GetFontsDirectory();
                if (Array.IndexOf(system, defaultdir) < 0)
                {
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(defaultdir);

                    if (dir.Exists)
                    {
                        //Load all the fonts from the directory
                        Scryber.OpenType.TTFRef[] all = Scryber.OpenType.TTFRef.LoadRefs(dir);

                        foreach (Scryber.OpenType.TTFRef item in all)
                        {
                            if (item.IsValid)
                            {
                                //font file added  - now register the family and style against the path
                                bag.AddFontFile(item.FamilyName, GetFontStyle(item.FontSelection, item.FontWeight), GetFontWeight(item.FontSelection, item.FontWeight), item.FullPath, item.HeadOffset);
                            }
                            
                        }

                    }
                }
            }

            


            return bag;
        }

        #endregion

        #region private static FamilyReferenceBag LoadStaticFamilies()

        const int HelveticaSpaceWidthFU = 569;
        const int TimesSpaceWidthFU = 512;
        const int CourierSpaceWidthFU = 1228;
        const int ZaphSpaceWidthFU = 544;
        const int SymbolSpaceWidthFU = 512;

        private static FamilyReferenceBag LoadStaticFamilies()
        {
            FamilyReferenceBag bag = new FamilyReferenceBag();

            try
            {
                var assm = typeof(PDFFontFactory).Assembly;
                TTFRef ttrRef;
                byte[] bin;
                FontReference fRef;
                TTFFile file;
                //Courier
                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Courier.CourierNew.ttf", out ttrRef);
                fRef = bag.AddFontResource("Courier", Scryber.Drawing.FontStyle.Regular, FontWeights.Regular, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("Fcour", "Courier", "Courier", "Courier New", false, false, CourierSpaceWidthFU, file); ;



                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Courier.CourierNewBold.ttf", out ttrRef);
                fRef = bag.AddFontResource("Courier", Scryber.Drawing.FontStyle.Regular, FontWeights.Bold, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FcourBo", "Courier-Bold", "Courier", "Courier New", true, false, CourierSpaceWidthFU, file);


                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Courier.CourierNewBoldItalic.ttf", out ttrRef);
                fRef = bag.AddFontResource("Courier", Scryber.Drawing.FontStyle.Italic, FontWeights.Bold, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FcourBoOb", "Courier-BoldOblique", "Courier", "Courier New", true, true, CourierSpaceWidthFU, file);


                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Courier.CourierNewItalic.ttf", out ttrRef);
                fRef = bag.AddFontResource("Courier", Scryber.Drawing.FontStyle.Italic, FontWeights.Regular, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FcourOb", "Courier-Oblique", "Courier", "Courier New", false, true, CourierSpaceWidthFU, file);


                //Helvetica

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Helvetica.Helvetica.ttf", out ttrRef);
                fRef = bag.AddFontResource("Helvetica", Scryber.Drawing.FontStyle.Regular, FontWeights.Regular, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("Fhel", "Helvetica", "Helvetica", false, false, HelveticaSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Helvetica.HelveticaBold.ttf", out ttrRef);
                fRef = bag.AddFontResource("Helvetica", Scryber.Drawing.FontStyle.Regular, FontWeights.Bold, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FhelBl", "Helvetica-Bold", "Helvetica", true, false, HelveticaSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Helvetica.HelveticaBoldOblique.ttf", out ttrRef);
                fRef = bag.AddFontResource("Helvetica", Scryber.Drawing.FontStyle.Italic, FontWeights.Bold, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FhelObBl", "Helvetica-BoldOblique", "Helvetica", true, true, HelveticaSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Helvetica.HelveticaOblique.ttf", out ttrRef);
                fRef = bag.AddFontResource("Helvetica", Scryber.Drawing.FontStyle.Italic, FontWeights.Regular, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FhelOb", "Helvetica-Oblique", "Helvetica", false, true, HelveticaSpaceWidthFU, file);


                //Symbol

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Symbol.Symbol.ttf", out ttrRef);
                fRef = bag.AddFontResource("Symbol", Scryber.Drawing.FontStyle.Regular, FontWeights.Regular, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdSymbolType1WinAnsi("Fsym", "Symbol", SymbolSpaceWidthFU, file);


                //Times

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Times.timesNewRoman.ttf", out ttrRef);
                fRef = bag.AddFontResource("Times", Scryber.Drawing.FontStyle.Regular, FontWeights.Regular, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("Ftimes", "Times-Roman", "Times", false, false, TimesSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Times.timesNewRomanBold.ttf", out ttrRef);
                fRef = bag.AddFontResource("Times", Scryber.Drawing.FontStyle.Regular, FontWeights.Bold, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FtimesBo", "Times-Bold", "Times", true, false, TimesSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Times.timesNewRomanBoldItalic.ttf", out ttrRef);
                fRef = bag.AddFontResource("Times", Scryber.Drawing.FontStyle.Italic, FontWeights.Bold, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FtimesBoIt", "Times-BoldItalic", "Times", true, true, TimesSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Times.timesNewRomanItalic.ttf", out ttrRef);
                fRef = bag.AddFontResource("Times", Scryber.Drawing.FontStyle.Italic, FontWeights.Regular, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FtimesIt", "Times-Italic", "Times", false, true, TimesSpaceWidthFU, file);



                //Zapf

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Zaph.ZapfDingbats.ttf", out ttrRef);
                fRef = bag.AddFontResource("Zapf Dingbats", Scryber.Drawing.FontStyle.Regular, FontWeights.Regular, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdSymbolType1WinAnsi("Fzapf", "ZapfDingbats", ZaphSpaceWidthFU, file);
                fRef.Definition.Family = "Zapf Dingbats";
                fRef.Definition.WindowsName = "WingDings";
            }
            catch(Exception ex)
            {
                throw new PDFLayoutException("The static resource fonts could not be loaded from the Scryber.Drawing assembly, please check the dll structure", ex);
            }

            return bag;
        }

        #endregion

        #region private static byte[] GetFontBinary(Assembly assembly, string rsrc, out TTFRef fontRef)

        private static byte[] GetFontBinary(Assembly assembly, string rsrc, out TTFRef fontRef)
        {
            if (null == assembly)
                throw new ArgumentNullException("assembly", "The assembly for loading the resource was null");

            var stream = assembly.GetManifestResourceStream(rsrc);

            if (null == stream)
                throw new PDFException("The resource font in assembly " + assembly.FullName + " could not be found");

            byte[] data;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                ms.Flush();
                ms.Position = 0;

                using (var ber = new BigEndianReader(ms))
                {
                    fontRef = TTFRef.LoadRef(ber, rsrc);
                }
                data = ms.ToArray();
            }
            return data;
        }

        #endregion

        #region private static System.Resources.ResourceManager LoadResourceManager(string basename)

        /// <summary>
        /// Loads a resorce manager for a specific base name on the format 'basename [, assemblyname]'
        /// </summary>
        /// <param name="basename"></param>
        /// <returns></returns>
        /// <remarks>If the assembly name is not provided then the current executing assembly is used</remarks>
        private static System.Resources.ResourceManager LoadResourceManager(string basename)
        {
            //TODO: Check if we are wa web project and load from the web project assembly rather than the executing assembly.
            System.Resources.ResourceManager mgr;
            System.Reflection.Assembly assm;

            try
            {
                if (!string.IsNullOrEmpty(basename))
                {
                    int offset = basename.IndexOf(',');
                    if (offset > 0)
                    {
                        string assmname = basename.Substring(offset + 1).Trim();
                        basename = basename.Substring(0, offset).Trim();

                        assm = System.Reflection.Assembly.Load(assmname);
                    }
                    else
                        assm = System.Reflection.Assembly.GetExecutingAssembly();
                }
                else
                    assm = System.Reflection.Assembly.GetExecutingAssembly();

                mgr = new System.Resources.ResourceManager(basename, assm);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(String.Format(Errors.CouldNotLoadTheResourceManagerForBase, basename), ex);
            }
            return mgr;
        }

        #endregion

        #region private static string GetFullPath(string path)

        /// <summary>
        /// Gets the full path to a file if it is not rooted
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetFullPath(string path)
        {
            if (!System.IO.Path.IsPathRooted(path))
            {
                //if (System.Web.HttpContext.Current != null)
                //    path = System.Web.HttpContext.Current.Server.MapPath(path);
                //else
                path = System.IO.Path.Combine(System.Environment.CurrentDirectory, path);
            }
            return path;
        }

        #endregion

        #region private static System.Drawing.FontStyle GetFontStyle(Scryber.OpenType.SubTables.FontSelection fs)

        /// <summary>
        /// Converts a OpenType.FontSelection to a Drawing.FontStyle
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        private static Scryber.Drawing.FontStyle GetFontStyle(Scryber.OpenType.SubTables.FontSelection fs, Scryber.OpenType.SubTables.WeightClass wc)
        {
            Scryber.Drawing.FontStyle style = Scryber.Drawing.FontStyle.Regular;

            if ((fs & Scryber.OpenType.SubTables.FontSelection.Italic) > 0)
                style |= Scryber.Drawing.FontStyle.Italic;

            return style;
        }

        #endregion

        #region private static System.Drawing.FontStyle GetFontStyle(Scryber.OpenType.SubTables.FontSelection fs)

        /// <summary>
        /// Converts a OpenType.FontSelection to a Drawing.FontStyle
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        private static int GetFontWeight(Scryber.OpenType.SubTables.FontSelection fs, Scryber.OpenType.SubTables.WeightClass wc)
        {
            int weight = FontWeights.Regular;

            if ((fs & Scryber.OpenType.SubTables.FontSelection.Bold) > 0)
                weight = FontWeights.Bold;

            if (wc > 0 && wc <= OpenType.SubTables.WeightClass.Black)
                weight = (int)wc;

            return weight;
        }

        #endregion

        //
        // PDFFontSource ensure methods
        //

        public static bool TryEnsureFont(IPDFComponent mapper, PDFContextBase context, PDFFontSource source, string familyName, Scryber.Drawing.FontStyle style, int weight, out PDFFontDefinition definition)
        {
            AssertInitialized();

            bool found = false;
            definition = null;

            PDFFontSource curr = source;
            
            while (null != curr)
            {
                if(curr.Type == FontSourceType.Local)
                {
                    definition = GetFontDefinition(curr.Source, style, weight, false);
                    if(definition != null)
                    {
                        if (context.ShouldLogVerbose)
                            context.TraceLog.Add(TraceLevel.Verbose, "FONT", "Font " + curr.Source + " found in existing fonts");

                        found = true;
                        break;
                    }
                }
                else if(curr.Type == FontSourceType.Url)
                {
                    var url = curr.Source;
                    if(!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        url = mapper.MapPath(url);
                    }
                    
                    if (_remotefamilies.TryGetValue(url, out definition))
                    {
                        found = null != definition;
                        if (found)
                        {
                            if (context.ShouldLogVerbose)
                                context.TraceLog.Add(TraceLevel.Verbose, "FONT", "Font " + curr.Source + " found in previously loaded remote fonts with url " + url);

                            break;
                        }
                    }
                    else if (CanUseFormat(curr.Format, url))
                    {
                        if (context.ShouldLogMessage)
                            context.TraceLog.Begin(TraceLevel.Message, "FONT", "Initiating the remote load of font " + curr.Source + " from url " + url);

                        if (TryLoadRemoteDefinition(context, url, familyName, style, weight, out definition))
                        {
                            lock (_initlock)
                            {
                                definition.FilePath = url;
                                _remotefamilies[url] = definition;
                            }
                            found = null != definition;
                        }

                        if (context.ShouldLogMessage)
                        {
                            if (null == definition)
                                context.TraceLog.Add(TraceLevel.Message, "FONT", "Ended the remote load of font " + curr.Source + " from url " + url + " but no font was able to be loaded");
                            else
                                context.TraceLog.Add(TraceLevel.Message, "FONT", "Ended the remote load of font " + curr.Source + " from url " + url + " and font definition was found : " + definition.FullName);
                        }

                        if (found) //The returned definition can be null.
                            break;
                    }
                }
                curr = curr.Next;
            }

            return found;
        }

        private static bool TryLoadRemoteDefinition(PDFContextBase context, string url, string family, Scryber.Drawing.FontStyle style, int weight, out PDFFontDefinition definition)
        {
            bool tried = true;
            definition = null;
            IDisposable monitor = null;


            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                HttpClient client = null;


                try
                {
                    if (context.PerformanceMonitor != null && context.PerformanceMonitor.RecordMeasurements)
                        monitor = context.PerformanceMonitor.Record(PerformanceMonitorType.Font_Load, url);
                    client = new System.Net.Http.HttpClient();
                    var data = client.GetByteArrayAsync(url).Result;
                    definition = PDFFontDefinition.LoadOpenTypeFontFile(data, family, style, weight, 0);
                }
                catch (Exception ex)
                {
                    context.TraceLog.Add(TraceLevel.Error, "FONT", "Could not load the font from the url " + url);
                    definition = null;
                    tried = false;
                }
                finally
                {
                    if (null != client)
                        client.Dispose();

                    if (null != monitor)
                        monitor.Dispose();
                }
            }
            else if (System.IO.File.Exists(url))
            {
                try
                {
                    if (context.PerformanceMonitor != null && context.PerformanceMonitor.RecordMeasurements)
                        monitor = context.PerformanceMonitor.Record(PerformanceMonitorType.Font_Load, url);

                    definition = PDFFontDefinition.LoadOpenTypeFontFile(url, family, style, weight, 0);
                }
                catch
                {
                    context.TraceLog.Add(TraceLevel.Error, "FONT", "Could not open the font from the file path " + url);
                    definition = null;
                    tried = false;
                }
                finally
                {
                    if (null != monitor)
                        monitor.Dispose();
                }
            }
            else
            {
                tried = false;
                context.TraceLog.Add(TraceLevel.Error, "FONT", "Font from the path " + url + " could not be found");
            }

            return tried;
        }

        private static bool CanUseFormat(FontSourceFormat format, string url)
        {
            switch (format)
            {
                case (FontSourceFormat.OpenType):
                case (FontSourceFormat.TrueType):
                case (FontSourceFormat.EmbeddedOpenType):
                    return true;

                case (FontSourceFormat.Default):
                    if (url.EndsWith(".ttf") || url.EndsWith(".otf"))
                        return true;
                    else
                        return false;

                default:
                    return false; 
            }
        }

    }
}
