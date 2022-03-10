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

#define FALLBACKTOREGULAR //comment out if we should not fall back to regular font style if the bold / italic variants are not found.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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
            internal SkiaSharp.SKFontStyle Style { get; private set; }
            internal string FilePath { get; private set; }
            internal byte[] FileData { get; private set; }
            internal int FileHeadOffset { get; private set; }

            internal PDFFontDefinition Definition { get; set; }

            internal bool LoadedDefintion { get { return null != Definition; } }

            //internal FontFamily SystemFamily { get; set; }

            internal object LoadLock { get; private set; }
            
            internal FontReference(string name, SkiaSharp.SKFontStyle style, string path, int fileHeadOffset)
            {
                //this.SystemFamily = family;
                this.FamilyName = name;
                this.Style = style;
                this.FilePath = path;
                this.FileHeadOffset = fileHeadOffset;
                this.LoadLock = new object();
            }

            internal FontReference(string name, SkiaSharp.SKFontStyle style, byte[] data, int fileHeadOffset)
            {
                //this.SystemFamily = family;
                this.FamilyName = name;
                this.Style = style;
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

            internal LinkedFontReference(string familyname, SkiaSharp.SKFontStyle style, string path, int fileHeadOffset)
                : base(familyname, style, path, fileHeadOffset)
            {
            }

            internal LinkedFontReference(string familyname, SkiaSharp.SKFontStyle style, byte[] data, int fileHeadOffset)
                : base(familyname, style, data, fileHeadOffset)
            {
            }

            internal void Append(LinkedFontReference reference)
            {
                if (null == this.Next)
                    this.Next = reference;
                else
                    this.Next.Append(reference);
            }

            internal LinkedFontReference GetStyle(SkiaSharp.SKFontStyle style)
            {
                if (style == this.Style)
                    return this;
                else if (null == this.Next)
                    return null;
                else
                    return this.Next.GetStyle(style);
            }
        }

        #endregion

        #region private class FamilyReference

        /// <summary>
        /// A family of Font References
        /// </summary>
        private class FamilyReference
        {
            //private FontFamily _sysfam;

            internal string FamilyName { get; private set; }

            /*
            internal FontFamily SystemFamily {
                get { return _sysfam; }
                set
                {
                    _sysfam = value;
                    LinkedFontReference link = _first;
                    while (null != link)
                    {
                        link.SystemFamily = value;
                        link = link.Next;
                    }
                }
            }

            */

            private LinkedFontReference _first;

            internal FontReference First
            {
                get { return _first; }
            }

            internal FontReference this[SkiaSharp.SKFontStyle style]
            {
                get
                {
                    if (null == _first)
                        return null;
                    else
                        return _first.GetStyle(style);
                }
            }

            internal FamilyReference(string name)
            {
                this.FamilyName = name;
            }

            internal FontReference Add(SkiaSharp.SKFontStyle style, string filepath, int fileHeadOffset)
            {

                LinkedFontReference fontref = new LinkedFontReference(this.FamilyName, style, filepath, fileHeadOffset);
                if (null == _first)
                    _first = fontref;
                else
                    _first.Append(fontref);

                return fontref;
            }

            internal FontReference Add(SkiaSharp.SKFontStyle style, byte[] data, int fileHeadOffset)
            {
                LinkedFontReference fontref = new LinkedFontReference(this.FamilyName, style, data, fileHeadOffset);
                if (null == _first)
                    _first = fontref;
                else
                    _first.Append(fontref);

                return fontref;
            }

            internal bool TryGetFont(SkiaSharp.SKFontStyle style, out FontReference font)
            {
                var found = _first;
                while(null != found)
                {
                    if(found.Style == style)
                    {
                        font = found;
                        return true;
                    }
                }

                font = null;
                return false;
            }
        }

        #endregion

        #region private class FamilyReferenceBag

        /// <summary>
        /// A collection of Font Family references that can be acessed by family name and font style. 
        /// Does not throw an exception if the font does not exist.
        /// </summary>
        private class FamilyReferenceBag
        {
            private Dictionary<string, FamilyReference> _families;
            //private FontCollection _collection;

            //internal FontCollection FontCollection
            //{
            //    get { return _collection; }
            //}

            internal FamilyReferenceBag()
            {
                //_collection = collection;
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

            internal FontReference this[string family, SkiaSharp.SKFontStyle style]
            {
                get
                {
                    FontReference fnt;
                    FamilyReference fam;
                    if (_families.TryGetValue(family, out fam))
                    {
                        fnt = fam[style];
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

            internal virtual FontReference AddFontFile(string family, SkiaSharp.SKFontStyle style, string path, int fileHeadOffset)
            {
                FamilyReference fam;
                if (_families.TryGetValue(family, out fam) == false)
                {
                    fam = new FamilyReference(family);
                    _families.Add(family, fam);
                }
                return fam.Add(style, path, fileHeadOffset);
            }

            internal FontReference AddFontResource(string family, SkiaSharp.SKFontStyle style, byte[] data, int fileHeadOffset)
            {
                FamilyReference fam;
                if (_families.TryGetValue(family, out fam) == false)
                {
                    fam = new FamilyReference(family);
                    _families.Add(family, fam);
                }
                return fam.Add(style, data, fileHeadOffset);
            }

            internal void AddFontFamily(string name, FamilyReference reference)
            {
                _families.Add(name, reference);
            }

            internal bool TryGetFamily(string name, out FamilyReference reference)
            {
                return _families.TryGetValue(name, out reference);
            }

            internal bool TryGetFont(string name, SkiaSharp.SKFontStyle style, out FontReference reference)
            {
                reference = null;
                FamilyReference family;
                if (_families.TryGetValue(name, out family) && family.TryGetFont(style, out reference))
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

        /*
        #region public static Font GetSystemFont(string family, SkiaSharp.SKFontStyle style, float size)

        /// <summary>
        /// Gets a System.Drawing.Font based upon the family, style and size.
        /// </summary>
        /// <param name="family">The name of the font family</param>
        /// <param name="style">The style of the font</param>
        /// <param name="size">The size of the font</param>
        /// <returns>A new System.Drawing.Font</returns>
        public static Font GetSystemFont(string family, SkiaSharp.SKFontStyle style, float size)
        {
            //Make sure we are initialized and OK
            AssertInitialized();
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            bool usesystem = config.FontOptions.UseSystemFonts;
            bool usesubstitute = config.FontOptions.FontSubstitution;
            
            if (string.IsNullOrEmpty(family))
                throw new ArgumentNullException("family");

            FontReference fref = _customfamilies[family, style];
            
            if (null == fref && usesystem)
                fref = _systemfamilies[family, style];
            if (null == fref)
                fref = _genericfamilies[family, style];

#if FALLBACKTOREGULAR
            if (null == fref && style != SkiaSharp.SKFontStyle.Normal)
                return GetSystemFont(family, SkiaSharp.SKFontStyle.Normal, size);
#endif
            if (null == fref)
            {
                if (usesubstitute)
                {
                    FamilyReference fam = _customfamilies[family];
                    if (null == fam && usesystem)
                        fam = _systemfamilies[family];

                    if (null != fam)
                        fref = fam.First;

                    //Fallback - use courier font definition
                    if (null == fref)
                    {
                        return new Font(GetSystemFontFamilyNameForStandardFont("Courier"), size, style, GraphicsUnit.Point);
                    }
                }
                else if (usesystem)
                    throw new NullReferenceException(String.Format(Errors.FontNotFound, family + " " + style.ToString()));
                else
                    throw new NullReferenceException(String.Format(Errors.FontNotFoundOrEnableSystem, family + " " + style.ToString()));
            }
            Font f;
            if (null == fref.SystemFamily)
                f = new Font(fref.FamilyName, size, fref.Style, GraphicsUnit.Point);
            else
                f = new Font(fref.SystemFamily, size, fref.Style, GraphicsUnit.Point);
            return f;
        }

        #endregion

        */

        #region public static PDFFontDefinition GetFontDefinition(PDFFont font)

        public static PDFFontDefinition GetFontDefinition(PDFFont font, bool throwNotFound = true)
        {
            //Make sure we are initialized and OK
            AssertInitialized();

            SkiaSharp.SKFontStyle fs = font.GetDrawingStyle();
            PDFFontSelector selector = font.Selector;
            while (null != selector)
            {
                var found = GetFontDefinition(selector.FamilyName, fs, false);
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

        #region public static bool IsFontDefined(string family, SkiaSharp.SKFontStyle style)
        /// <summary>
        /// Returns true if the available fonts contain one with the specified family and style
        /// </summary>
        /// <param name="family">The name of the font family (case insensitive)</param>
        /// <param name="style">GThe required stlye of the font</param>
        /// <returns>True if available, or false</returns>
        public static bool IsFontDefined(string family, SkiaSharp.SKFontStyle style)
        {
            //Make sure we are initialized and OK
            AssertInitialized();
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            bool usesystem = config.FontOptions.UseSystemFonts;

            if (string.IsNullOrEmpty(family))
                throw new ArgumentNullException("family");




            FontReference fref = _customfamilies[family, style];

            if (null == fref && usesystem)
                fref = _systemfamilies[family, style];

            if (null == fref)
                fref = _genericfamilies[family, style];

#if FALLBACKTOREGULAR
            if (null == fref && style != SkiaSharp.SKFontStyle.Normal && !PDFFont.IsStandardFontFamily(family))
                return IsFontDefined(family, SkiaSharp.SKFontStyle.Normal);
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
            SkiaSharp.SKFontStyle fontstyle = SkiaSharp.SKFontStyle.Normal;
            string family;
            if (pos > 0)
            {
                family = fullname.Substring(0, pos).Trim();
                string style = fullname.Substring(pos);
                if (style.IndexOf("bold", StringComparison.OrdinalIgnoreCase) > -1)
                    fontstyle = SkiaSharp.SKFontStyle.Bold;

                if (style.IndexOf("italic", StringComparison.OrdinalIgnoreCase) > -1)
                    fontstyle = SkiaSharp.SKFontStyle.Italic;

                if (style.IndexOf("bold", StringComparison.OrdinalIgnoreCase) > -1 && 
                    style.IndexOf("italic", StringComparison.OrdinalIgnoreCase) > -1)
                    fontstyle = SkiaSharp.SKFontStyle.BoldItalic;
            }
            else
                family = fullname.Trim();

            return GetFontDefinition(family, fontstyle);
            
        }

        #endregion

        #region public static PDFFontDefinition GetFontDefinition(string family, SkiaSharp.SKFontStyle style, bool throwNotFound = true)

        /// <summary>
        /// Gets the PDFFontDefinition for the specified famil and style
        /// </summary>
        /// <param name="family"></param>
        /// <param name="style"></param>
        /// <param name="throwNotFound">If true (default) then if the font cannot be found an exception will be raised.
        /// If not, then null will be returned.</param>
        /// <returns></returns>
        public static PDFFontDefinition GetFontDefinition(string family, SkiaSharp.SKFontStyle style, bool throwNotFound = true)
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
            
            FontReference fref = _customfamilies[family, style];
            
            
            if (null == fref && usesystem)
                fref = _systemfamilies[family, style];
            if (null == fref)
                fref = _genericfamilies[family, style];

            if (null == fref)
            {

                if (usesubstitute && style != SkiaSharp.SKFontStyle.Normal && !PDFFont.IsStandardFontFamily(family))
                    return GetFontDefinition(family, SkiaSharp.SKFontStyle.Normal, throwNotFound);

                //We dont have the explicit font so if we should substitue then 
                //try to find the family and return that otherwise use courier.
                if (usesubstitute)
                {
                    FamilyReference fam = _customfamilies[family];
                    if (null == fam && usesystem)
                        fam = _systemfamilies[family];

                    if (null != fam)
                        fref = fam.First;

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
                        defn = PDFFontDefinition.LoadOpenTypeFontFile(fref.FilePath, family, style, fref.FileHeadOffset);
                    else
                        //We do have the binary data so load from this
                        defn = PDFFontDefinition.LoadOpenTypeFontFile(fref.FileData, family, style, fref.FileHeadOffset);

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
            /* if (_customfamilies.TryGetFamily("Helvetica", out found) || _customfamilies.TryGetFamily("Arial", out found)
                || _systemfamilies.TryGetFamily("Helvetica", out found) || _systemfamilies.TryGetFamily("Arial", out found)
                || _systemfamilies.TryGetFamily("System Font", out found))
            {
                genericBag.AddFontFamily("Sans-Serif", found);
                genericBag.AddFontFamily("Helvetica", found);
            }
            else */

            if (_staticfamilies.TryGetFamily("Helvetica", out found))
            {
                genericBag.AddFontFamily("Sans-Serif", found);
                genericBag.AddFontFamily("Helvetica", found);
            }
            else
                throw new ConfigurationErrorsException("Could not find or load the standard font family for Helvetica");

            /*if (_customfamilies.TryGetFamily("Times New Roman",out found) || _customfamilies.TryGetFamily("Times", out found)
                || _systemfamilies.TryGetFamily("Times New Roman", out found) || _systemfamilies.TryGetFamily("Times", out found)
                || _systemfamilies.TryGetFamily(".New York", out found))
            {
                genericBag.AddFontFamily("Serif", found);
                genericBag.AddFontFamily("Times", found);
            }
            else */

            if (_staticfamilies.TryGetFamily("Times", out found) || _staticfamilies.TryGetFamily("Times New Roman", out found))
            {
                genericBag.AddFontFamily("Serif", found);
                genericBag.AddFontFamily("Times", found);
            }
            else
                throw new ConfigurationErrorsException("Could not find or load the standard font family for Times");

            /* if (_customfamilies.TryGetFamily("Courier", out found) || _customfamilies.TryGetFamily("Courier New", out found)
                || _systemfamilies.TryGetFamily("Courier", out found) || _systemfamilies.TryGetFamily("Courier New", out found))
            {
                genericBag.AddFontFamily("Monospace", found);
                genericBag.AddFontFamily("Courier", found);
            }
            else */

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

            /*if(_customfamilies.TryGetFamily("Zapf Dingbats", out found) || _systemfamilies.TryGetFamily("Zapf Dingbats", out found)
               || _customfamilies.TryGetFamily("Webdings", out found) || _systemfamilies.TryGetFamily("Webdings", out found))
            {
                genericBag.AddFontFamily("Zapf Dingbats", found);
            }
            else*/

            if (_staticfamilies.TryGetFamily("Zapf Dingbats", out found))
            {
                genericBag.AddFontFamily("Zapf Dingbats", found);
            }
            else
                throw new ConfigurationErrorsException("Could not find or load the standard font family for Zaph Dingbats");

            /* if (_customfamilies.TryGetFamily("Symbol", out found) || _systemfamilies.TryGetFamily("Symbol", out found))
            {
                genericBag.AddFontFamily("Symbol", found);
            }
            else */

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
                                        bag.AddFontFile(item.FamilyName, GetFontStyle(item.FontSelection, item.FontWeight), item.FullPath, item.HeadOffset);
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

                    SkiaSharp.SKFontStyle style = SKFontStyleMapping(map.Style);

                   
                    if (!string.IsNullOrEmpty(map.File))
                    {
                        //Load from a file
                        string path = map.File;

                        try
                        {
                            path = GetFullPath(path);
                        }
                        catch (Exception ex)
                        {
                            throw new ConfigurationErrorsException(String.Format(Errors.CouldNotLoadTheFontFile, path, ex.Message), ex);
                        }

                        ///Only support ttf not ttc
                        bag.AddFontFile(family, style, path, 0);
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
                                bag.AddFontFile(item.FamilyName, GetFontStyle(item.FontSelection, item.FontWeight), item.FullPath, item.HeadOffset);
                            }
                            
                        }

                    }
                }
            }

            


            return bag;
        }

        #endregion

        private static SkiaSharp.SKFontStyle SKFontStyleMapping(string styleName)
        {
            if (styleName == "Bold")
                return SkiaSharp.SKFontStyle.Bold;
            else if (styleName == "Italic")
                return SkiaSharp.SKFontStyle.Italic;
            else if (styleName == "BoldItalic")
                return SkiaSharp.SKFontStyle.BoldItalic;

            return SkiaSharp.SKFontStyle.Normal;
        }

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
                fRef = bag.AddFontResource("Courier", SkiaSharp.SKFontStyle.Normal, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("Fcour", "Courier", "Courier", "Courier New", false, false, CourierSpaceWidthFU, file); ;



                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Courier.CourierNewBold.ttf", out ttrRef);
                fRef = bag.AddFontResource("Courier", SkiaSharp.SKFontStyle.Bold, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FcourBo", "Courier-Bold", "Courier", "Courier New", true, false, CourierSpaceWidthFU, file);


                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Courier.CourierNewBoldItalic.ttf", out ttrRef);
                fRef = bag.AddFontResource("Courier", SkiaSharp.SKFontStyle.BoldItalic, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FcourBoOb", "Courier-BoldOblique", "Courier", "Courier New", true, true, CourierSpaceWidthFU, file);


                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Courier.CourierNewItalic.ttf", out ttrRef);
                fRef = bag.AddFontResource("Courier", SkiaSharp.SKFontStyle.Italic, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FcourOb", "Courier-Oblique", "Courier", "Courier New", false, true, CourierSpaceWidthFU, file);


                //Helvetica

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Helvetica.Helvetica.ttf", out ttrRef);
                fRef = bag.AddFontResource("Helvetica", SkiaSharp.SKFontStyle.Normal, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("Fhel", "Helvetica", "Helvetica", false, false, HelveticaSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Helvetica.HelveticaBold.ttf", out ttrRef);
                fRef = bag.AddFontResource("Helvetica", SkiaSharp.SKFontStyle.Bold, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FhelBl", "Helvetica-Bold", "Helvetica", true, false, HelveticaSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Helvetica.HelveticaBoldOblique.ttf", out ttrRef);
                fRef = bag.AddFontResource("Helvetica", SkiaSharp.SKFontStyle.BoldItalic, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FhelObBl", "Helvetica-BoldOblique", "Helvetica", true, true, HelveticaSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Helvetica.HelveticaOblique.ttf", out ttrRef);
                fRef = bag.AddFontResource("Helvetica", SkiaSharp.SKFontStyle.Italic, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FhelOb", "Helvetica-Oblique", "Helvetica", false, true, HelveticaSpaceWidthFU, file);


                //Symbol

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Symbol.Symbol.ttf", out ttrRef);
                fRef = bag.AddFontResource("Symbol", SkiaSharp.SKFontStyle.Normal, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdSymbolType1WinAnsi("Fsym", "Symbol", SymbolSpaceWidthFU, file);


                //Times

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Times.timesNewRoman.ttf", out ttrRef);
                fRef = bag.AddFontResource("Times", SkiaSharp.SKFontStyle.Normal, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("Ftimes", "Times-Roman", "Times", false, false, TimesSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Times.timesNewRomanBold.ttf", out ttrRef);
                fRef = bag.AddFontResource("Times", SkiaSharp.SKFontStyle.Bold, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FtimesBo", "Times-Bold", "Times", true, false, TimesSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Times.timesNewRomanBoldItalic.ttf", out ttrRef);
                fRef = bag.AddFontResource("Times", SkiaSharp.SKFontStyle.BoldItalic, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FtimesBoIt", "Times-BoldItalic", "Times", true, true, TimesSpaceWidthFU, file);

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Times.timesNewRomanItalic.ttf", out ttrRef);
                fRef = bag.AddFontResource("Times", SkiaSharp.SKFontStyle.Italic, bin, ttrRef.HeadOffset);
                file = new TTFFile(bin, ttrRef.HeadOffset);
                fRef.Definition = PDFFontDefinition.InitStdType1WinAnsi("FtimesIt", "Times-Italic", "Times", false, true, TimesSpaceWidthFU, file);



                //Zapf

                bin = GetFontBinary(assm, "Scryber.Text._FontResources.Zaph.ZapfDingbats.ttf", out ttrRef);
                fRef = bag.AddFontResource("Zapf Dingbats", SkiaSharp.SKFontStyle.Normal, bin, ttrRef.HeadOffset);
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

        #region private static SkiaSharp.SKFontStyle GetFontStyle(Scryber.OpenType.SubTables.FontSelection fs)

        /// <summary>
        /// Converts a OpenType.FontSelection to a Drawing.FontStyle
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        private static SkiaSharp.SKFontStyle GetFontStyle(Scryber.OpenType.SubTables.FontSelection fs, Scryber.OpenType.SubTables.WeightClass wc)
        {
            SkiaSharp.SKFontStyle style = SkiaSharp.SKFontStyle.Normal;

            if ((fs & Scryber.OpenType.SubTables.FontSelection.Italic) > 0)
                style = SkiaSharp.SKFontStyle.Italic;
            //if ((fs & Scryber.OpenType.SubTables.FontSelection.Underscore) > 0)
            //    style = SkiaSharp.SKFontStyle.Underline;
            //if ((fs & Scryber.OpenType.SubTables.FontSelection.Strikeout) > 0)
            //    style = SkiaSharp.SKFontStyle.Strikeout;
            if ((fs & Scryber.OpenType.SubTables.FontSelection.Bold) > 0)
                style = SkiaSharp.SKFontStyle.Bold;

            if (wc == OpenType.SubTables.WeightClass.Bold)
                style = SkiaSharp.SKFontStyle.Bold;

            return style;
        }

        #endregion

        //
        // PDFFontSource ensure methods
        //

        public static bool TryEnsureFont(IPDFComponent mapper, PDFContextBase context, PDFFontSource source, string familyName, SkiaSharp.SKFontStyle style, out PDFFontDefinition definition)
        {
            AssertInitialized();

            bool found = false;
            definition = null;

            PDFFontSource curr = source;
            
            while (null != curr)
            {
                if(curr.Type == FontSourceType.Local)
                {
                    definition = GetFontDefinition(curr.Source, style, false);
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

                        if (TryLoadRemoteDefinition(context, url, familyName, style, out definition))
                        {
                            lock (_initlock)
                            {
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

        private static bool TryLoadRemoteDefinition(PDFContextBase context, string url, string family, SkiaSharp.SKFontStyle style, out PDFFontDefinition definition)
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
                    definition = PDFFontDefinition.LoadOpenTypeFontFile(data, family, style, 0);
                }
                catch (Exception ex)
                {
                    context.TraceLog.Add(TraceLevel.Error, "FONT", "Could not load the font from the url " + url);
                    definition = null;
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

                    definition = PDFFontDefinition.LoadOpenTypeFontFile(url, family, style, 0);
                }
                catch
                {
                    context.TraceLog.Add(TraceLevel.Error, "FONT", "Could not open the font from the file path " + url);
                    definition = null;
                }
                finally
                {
                    if (null != monitor)
                        monitor.Dispose();
                }
            }
            else
                context.TraceLog.Add(TraceLevel.Error, "FONT", "Font from the path " + url + " could not be found");


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
