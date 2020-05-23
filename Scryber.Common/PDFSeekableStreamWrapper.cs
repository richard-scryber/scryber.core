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

namespace Scryber
{
    /// <summary>
    /// Wraps a stream that is not seekable (e.g. HttpResponse stream) with a new stream that is seekable and this is then output to the wrapped stream when it is disposed
    /// </summary>
    internal class PDFSeekableStreamWrapper : Native.PDFStream
    {
        private System.IO.Stream _nonseekable;
        private bool _ownedNonSeekable;

        public PDFSeekableStreamWrapper(System.IO.Stream nonseekable, IStreamFilter[] filters, Native.PDFIndirectObject indobj, bool ownstream)
            : base(GetSeekableStream(), filters, indobj, true) //Pass true to the base ownstream because we do and we want it to be closed.
        {
            if (null == nonseekable)
                throw new ArgumentNullException("nonseekable");
            _nonseekable = nonseekable;
            _ownedNonSeekable = ownstream;
        }


        private const int bufferSize = 2048;

        protected override void Dispose(bool disposing)
        {
            if (disposing && _nonseekable != null)
            {
                System.IO.MemoryStream ms = (System.IO.MemoryStream)this.InnerStream;
                ms.Position = 0;
                byte[] buffer = new byte[bufferSize];
                int num;

                while ((num = ms.Read(buffer,0,buffer.Length)) != 0)
                {
                    _nonseekable.Write(buffer, 0, num);
                }

                //ms.WriteTo(_nonseekable);

                if (_ownedNonSeekable && null != _nonseekable)
                {
                    _nonseekable.Dispose();
                    _nonseekable = null;
                }
            }

            base.Dispose(disposing);
        }


        private static System.IO.Stream GetSeekableStream()
        {
            return new System.IO.MemoryStream();
        }
    }
}
