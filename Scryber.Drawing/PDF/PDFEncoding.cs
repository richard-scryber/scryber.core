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
using System.Linq;
using System.Text;

namespace Scryber.PDF
{
    public class PDFEncoding
    {

        private System.Text.Encoding _sysEncoding;
        private byte[] _pre;

        /// <summary>
        /// Gets the bytes that should be written 
        /// before the start of a character string to identify the encoding
        /// </summary>
        public byte[] Prefix
        {
            get { return _pre; }
        }

        /// <summary>
        /// Returns true if this encoding has a prefix to be written
        /// </summary>
        public bool HasPrefix
        {
            get { return null != _pre && _pre.Length > 0; }
        }


        public PDFEncoding(System.Text.Encoding systemEncoding, byte[] prefix)
        {
            if (null == systemEncoding)
                throw new ArgumentNullException("systemEncoding");

            this._sysEncoding = systemEncoding;
            
            this._pre = prefix;
        }

        
        public byte[] GetBytes(string data)
        {
            return this._sysEncoding.GetBytes(data);
        }


        static PDFEncoding()
        {
#if !NETSTANDARD2_0
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _windows1252 = new PDFEncoding(System.Text.Encoding.GetEncoding("windows-1252"), null);

            if(null == _windows1252)
                _windows1252 = new PDFEncoding(System.Text.Encoding.ASCII, null);
#else
            _windows1252 = new PDFEncoding(System.Text.Encoding.ASCII, null);
#endif
            _unicodeBE = new PDFEncoding(System.Text.Encoding.GetEncoding("UTF-16BE"), null);
            _macroman = new PDFEncoding(System.Text.Encoding.Default, null);
            _pdfdoc = new PDFEncoding(System.Text.Encoding.ASCII, null);
        }

        private static PDFEncoding _windows1252;
        private static PDFEncoding _unicodeBE;
        private static PDFEncoding _macroman;
        private static PDFEncoding _pdfdoc;

        public static PDFEncoding WinAnsiEncoding
        {
            get { return _windows1252; }
        }

        public static PDFEncoding MacRomanEncoding
        {
            get { return _macroman; }
        }

        public static PDFEncoding PDFDocEncoding
        {
            get { return _pdfdoc; }
        }

        public static PDFEncoding UnicodeBigEndian
        {
            get { return _unicodeBE; }
        }

        public static PDFEncoding GetEncoding(FontEncoding fontencoding)
        {
            PDFEncoding enc;
            switch (fontencoding)
            {
                case FontEncoding.MacRomanEncoding:
                    enc = MacRomanEncoding;
                    break;
                case FontEncoding.WinAnsiEncoding:
                    if (null == WinAnsiEncoding)
                        throw new Exception("The windows Ansi encoding is not supported on this platform");
                    enc = WinAnsiEncoding;
                    break;
                case FontEncoding.PDFDocEncoding:
                    enc = PDFDocEncoding;
                    break;
                case FontEncoding.UnicodeEncoding:
                    enc = UnicodeBigEndian;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("fontenncoding");
            }
            return enc;
        }

        public static PDFEncoding GetEncoding(string sysname)
        {
            System.Text.Encoding found = System.Text.Encoding.GetEncoding(sysname);
            if (null == found)
                throw new ArgumentOutOfRangeException("sysname");
            return new PDFEncoding(found, null);
        }
    }
}
