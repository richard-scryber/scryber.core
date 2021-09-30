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
    /// <summary>
    /// Represents two byte-strings to uniquely identify a document. Use the Create overloads to instaniate a new one.
    /// Can also be set in the document xml file as a comma separated pair of Guid byte strings
    /// </summary>
    [PDFParsableValue()]
    public class PDFDocumentID
    {
        #region public byte[] One {get;}

        /// <summary>
        /// Gets the first byte array identifier for this DocumentID
        /// </summary>
        public byte[] One
        {
            get;
            private set;
        }

        #endregion

        #region public byte[] Two {get;}

        /// <summary>
        /// Gets the second byte array for this DocumentID
        /// </summary>
        public byte[] Two
        {
            get;
            private set;
        }

        #endregion

        #region private .ctor

        /// <summary>
        /// Private constructor - use the Create static method instead
        /// </summary>
        private PDFDocumentID()
        {
        }

        #endregion




        //
        // Static Parse - supports the PDFParsableValue attribute
        //

        #region public static PDFDocumentID Parse(string value)

        /// <summary>
        /// Parses a string value into a PDFDocumentID with the expected format as either a single Guid or a comma separated pair of Guids.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PDFDocumentID Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(CommonErrors.CannotParseTheDocumentIDs);

            try
            {
                string[] both = value.Split(',');
                Guid one = new Guid(both[0].Trim());

                Guid two;
                if (both.Length > 1)
                    two = new Guid(both[1].Trim());
                else
                    two = Guid.NewGuid();

                PDFDocumentID ids = PDFDocumentID.Create(one, two);
                return ids;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CommonErrors.CannotParseTheDocumentIDs, "value", ex);
            }
        }

        #endregion

        //
        // create factory methods
        //

        #region public static PDFDocumentID Create()

        /// <summary>
        /// Creates and returns a new unique PDFDocumentID with 2 unique new guid values
        /// </summary>
        /// <returns></returns>
        public static PDFDocumentID Create()
        {
            Guid one = Guid.NewGuid();
            Guid two = Guid.NewGuid();
            return Create(one, two);
        }

        #endregion

        #region public static PDFDocumentID Create(Guid one)

        /// <summary>
        /// Creates and returns a new unique PDFDocumentID with the first value as specified and the second as a new unique guid
        /// </summary>
        /// <param name="one">The first (known) Guid value</param>
        /// <returns></returns>
        public static PDFDocumentID Create(Guid one)
        {
            Guid two = Guid.NewGuid();
            return Create(one, two);
        }

        #endregion

        #region public static PDFDocumentID Create(Guid one, Guid two)

        /// <summary>
        /// Creates and returns a new PDFDocumentID with the known guid values
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static PDFDocumentID Create(Guid one, Guid two)
        {
            if (one == Guid.Empty)
                throw new ArgumentOutOfRangeException(CommonErrors.DocumentIDCannotBeEmpty);
            if (two == Guid.Empty)
                throw new ArgumentOutOfRangeException(CommonErrors.DocumentIDCannotBeEmpty);

            byte[] oneB = one.ToByteArray();
            byte[] twoB = two.ToByteArray();
            return Create(oneB, twoB);
        }

        #endregion

        #region public static PDFDocumentID Create(byte[] one)

        /// <summary>
        /// Creates and returns a new PDFDocumentID with the first known byte[] and the second as a new unique Guid
        /// </summary>
        /// <param name="one"></param>
        /// <returns></returns>
        public static PDFDocumentID Create(byte[] one)
        {
            Guid two = Guid.NewGuid();
            byte[] twob = two.ToByteArray();
            return Create(one, twob);
        }

        #endregion

        #region public static PDFDocumentID Create(byte[] one, byte[] two)

        /// <summary>
        /// Creates and returns a new PDFDocumentID with the known binary values
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static PDFDocumentID Create(byte[] one, byte[] two)
        {
            if (null == one || one.Length == 0)
                throw new ArgumentOutOfRangeException(CommonErrors.DocumentIDCannotBeEmpty);
            if (null == two || two.Length == 0)
                throw new ArgumentOutOfRangeException(CommonErrors.DocumentIDCannotBeEmpty);

            PDFDocumentID ids = new PDFDocumentID();
            ids.One = one;
            ids.Two = two;

            return ids;
        }

        #endregion
    }
}
