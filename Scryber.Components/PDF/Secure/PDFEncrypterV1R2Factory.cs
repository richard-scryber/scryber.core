using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF.Secure
{
    internal class PDFEncrypterV1R2Factory : PDFEncrypterFactory
    {

        private const int Rev2MD5Count = 1;
        private const int Rev2RC4Length = 5;

        internal PDFEncrypterV1R2Factory()
            : base(2, 2, 5)
        {
        }


        protected override PDFEncryter InitEncrypter(byte[] paddedownerpassword, byte[] paddeduserpassword, PDFDocumentID documentid, PermissionFlags protection)
        {
            if (null == documentid || null == documentid.One || documentid.One.Length == 0)
                throw new ArgumentNullException("documentid");

            //TODO: Revision3 copyin
            byte[] O = GetOwnerValue(paddedownerpassword, paddeduserpassword, Rev2RC4Length);

            byte[] key = CreateEncryptionKey(paddedownerpassword, paddeduserpassword, Rev2RC4Length, (int)protection, documentid.One);

            //User key
            byte[] U = GetUserValue(key);


            PDFEncryter info = new PDFEncrypterV1R2(O, U, key, protection);
            return info;
        }


        /// <summary>
        /// Get's the value of O
        /// </summary>
        /// <param name="ownerpassword">The padded owner password</param>
        /// <param name="userpassword">The padded user password</param>
        /// <returns></returns>
        private byte[] GetOwnerValue(byte[] ownerpassword, byte[] userpassword, int keysize)
        {
            byte[] md5 = MD5(ownerpassword);
            byte[] key = GetBytes(keysize, md5);
            byte[] o = RC4.Encrypt(key, userpassword);
            return o;
        }

        private byte[] GetUserValue(byte[] key)
        {
            byte[] enc = RC4.Encrypt(key, _padding);
            return enc;
        }

        /// <summary>
        /// Creates an encryption key of the required size based on the password and other info
        /// </summary>
        /// <param name="password"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte[] CreateEncryptionKey(byte[] paddedowner, byte[] paddeduser, int keysize, int documentrestrictions, byte[] documentid)
        {
            byte[] all;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ms.Write(paddeduser, 0, paddeduser.Length);

                byte[] o = GetOwnerValue(paddedowner, paddeduser, Rev2RC4Length);
                ms.Write(o, 0, o.Length);

                byte[] pbytes = ConvertIntToLowOrderArray(documentrestrictions);
                ms.Write(pbytes, 0, pbytes.Length);

                byte[] id = documentid;
                ms.Write(id, 0, id.Length);

                ms.Position = 0;
                all = ms.ToArray();
                all = MD5(all);
            }

            byte[] key = new byte[keysize];
            Array.Copy(all, key, keysize);
            return key;
        }

    }
}
