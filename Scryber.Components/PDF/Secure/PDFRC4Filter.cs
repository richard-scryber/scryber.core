using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF.Secure
{
    internal class PDFRC4Filter : IStreamFilter
    {
        #region IStreamFilter Members

        /// <summary>
        /// Gets the name of the Secure filter - set operation does nothing
        /// </summary>
        public string FilterName
        {
            get
            {
                return "RC4Filter";
            }
            set
            {
                ;
            }
        }

        private byte[] _key;

        /// <summary>
        /// Gets the Encryption Key for this filter
        /// </summary>
        internal byte[] EncryptionKey
        {
            get { return _key; }
        }

        private PDFEncrypterStd _enc;

        /// <summary>
        /// Gets the encrypted that spawned this filter instance.
        /// </summary>
        internal PDFEncrypterStd Encrypter
        {
            get { return _enc; }
        }


        internal PDFRC4Filter(byte[] key, PDFEncrypterStd enc)
        {
            this._key = key;
            this._enc = enc;
        }

        
        /// <summary>
        /// Filters the input stream and writes to the output stream.
        /// </summary>
        /// <param name="read"></param>
        /// <param name="write"></param>
        public void FilterStream(System.IO.Stream read, System.IO.Stream write)
        {
            byte[] all = new byte[(int)(read.Length - read.Position)];
            read.Read(all, 0, all.Length);
            all = FilterStream(all);
            write.Write(all, 0, all.Length);
        }

        /// <summary>
        /// Filters the input data and returns the encrypted output.
        /// </summary>
        /// <param name="orig"></param>
        /// <returns></returns>
        public byte[] FilterStream(byte[] orig)
        {
            byte[] encrypted = RC4.Encrypt(this.EncryptionKey, orig);
            return encrypted;
        }

        #endregion

        #region private byte[] MD5(byte[] data)

        private System.Security.Cryptography.MD5 _md5;

        /// <summary>
        /// MD5 hashes the input data and returns the result
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] MD5(byte[] data)
        {
            if (null == _md5)
                _md5 = System.Security.Cryptography.MD5.Create();
            return _md5.ComputeHash(data);
        }

        #endregion
    }
}
