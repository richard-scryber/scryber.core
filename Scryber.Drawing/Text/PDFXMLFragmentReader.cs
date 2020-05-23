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

namespace Scryber.Text
{
    public class PDFXMLFragmentReader : Scryber.Text.PDFTextReader
    {

        private const int StartIndex = -1;

        private int _index;
        private List<Scryber.Text.PDFTextOp> _all;
        private Exception _error;

        public override bool EOF
        {
            get
            {
                RethrowAnyError();
                return _all.Count <= _index;
            }
        }

        public override int Length
        {
            get
            {
                RethrowAnyError();
                return _all.Count;
            }
        }

        public override Scryber.Text.PDFTextOp Value
        {
            get
            {
                RethrowAnyError();
                if (EOF)
                    throw new InvalidOperationException("This reader has passed the end of the data");
                else if (_index <= StartIndex)
                    throw new InvalidOperationException("Call Read() before attempting to extract any value");

                return _all[_index];
            }
        }

        //
        // ctor(s)
        //

        public PDFXMLFragmentReader(string text, bool preserveWhitespace, PDFTraceLog log)
        {
            this.InitWithText(text, preserveWhitespace, log);
        }


        //
        // public / protected methods
        //

        public override bool Read()
        {
            _index++;
            return !EOF;
        }

        protected override void ResetTextMarkers()
        {
            this._index = StartIndex;
        }


        //
        // private implementation
        //

        private const int MinLoggingStringLength = 1000; //minimum length of the string before we start logging how long it took to parse the string.

        /// <summary>
        /// Initializes the XMLFragmentReader with a string
        /// </summary>
        /// <param name="text"></param>
        private void InitWithText(string text, bool preserveWhitespace, PDFTraceLog log)
        {
            System.Diagnostics.Stopwatch sw = null;
            List<Scryber.Text.PDFTextOp> all = null;

            //TODO: IMPORTANT. Change this to an actual reader implementation that does not create an array of all the lines
            try
            {
                if (text.Length > MinLoggingStringLength)
                    sw = System.Diagnostics.Stopwatch.StartNew();

                PDFXMLFragmentParser parser = new PDFXMLFragmentParser();
                all = parser.Parse(text, preserveWhitespace);

                if (null != sw)
                {
                    sw.Stop();
                    if (null != log && log.ShouldLog(TraceLevel.Debug))
                        log.Add(TraceLevel.Debug, "XML Fragment Parser", "Splitting out entries in a string of " + text.Length + " characters took " + sw.Elapsed);
                }
            }
            catch (Exception ex)
            {
                _error = ex;
                throw new PDFXmlFormatException("Could not parse the XML text in the provided string", ex);
            }

            //set the instance variable in pre-read state
            _all = all;
            _index = -1;
        }



        /// <summary>
        /// Checks the error instance variable, and if set re-throws wrapped in a new exception
        /// </summary>
        private void RethrowAnyError()
        {
            if (null != _error)
                throw new PDFXmlFormatException("Could not parse the XML text in the provided string", _error);
        }
    }
}
