using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.Secure
{
    internal class PDFEncrypterV2R3Factory : PDFEncrypterFactory
    {


        private const int Rev3MD5Count = 50;
        private const int Rev3RC4Count = 20;
        

        internal PDFEncrypterV2R3Factory(int keylengthbytes)
            : base(2, 3, keylengthbytes)
        {
        }

        protected override PDFEncryter InitEncrypter(byte[] paddedownerpassword, byte[] paddeduserpassword, PDFDocumentID documentid, PermissionFlags protection)
        {
            if (null == documentid || null == documentid.One || documentid.One.Length == 0)
                throw new ArgumentNullException("documentid");

            byte[] O = GetOwnerValue(paddedownerpassword, paddeduserpassword, KeyLengthBytes);
            byte[] key = CreateEncryptionKey(O, paddeduserpassword, KeyLengthBytes, (int)protection, documentid.One);

            byte[] U = GetUserValue(key, documentid.One);

            PDFEncryter info = new PDFEncrypterV2R3(O, U, this.KeyLengthBytes, key, protection);

            return info;
        }

        internal byte[] GetOwnerValue(byte[] paddedownerpassword, byte[] paddeduserpassword, int keysize)
        {
            
            byte[] md5 = MD5(paddedownerpassword);

            //repeat 50 times
            for (int i = 0; i < Rev3MD5Count; ++i)
            {
                byte[] hashed = MD5(md5);
                Array.Copy(hashed, 0, md5, 0, keysize);
            }

            byte[] key = new byte[keysize];
            byte[] ownerValue = paddeduserpassword;
            
            
            for (byte op = 0; op < Rev3RC4Count; ++op)
            {
                for (int i = 0; i < key.Length; i++)
                    key[i] = (byte)(md5[i] ^ op);
                ownerValue = Scryber.Secure.RC4.Encrypt(key, ownerValue);
            }

            return ownerValue;
        }


        internal byte[] CreateEncryptionKey(byte[] ownerkey, byte[] paddeduser, int keySize, int permissions, byte[] docId)
        {
            byte[] all;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ms.Write(paddeduser, 0, paddeduser.Length);

                byte[] o = ownerkey;
                ms.Write(o, 0, o.Length);

                byte[] permission = new byte[4];
                permission[0] = (byte)permissions;
                permission[1] = (byte)(permissions >> 8);
                permission[2] = (byte)(permissions >> 16);
                permission[3] = (byte)(permissions >> 24); 
                ms.Write(permission, 0, permission.Length);

                ms.Write(docId, 0, docId.Length);

                ms.Position = 0;
                all = ms.ToArray();
                all = MD5(all);
            }

            byte[] key = new byte[keySize];
            Array.Copy(all, key, keySize);

            //Repeat another 50 times
            for (int i = 0; i < Rev3MD5Count; i++)
            {
                byte[] result = MD5(key);
                Array.Copy(result, 0, key, 0, keySize);
            }
            return key;
        }

        internal byte[] GetUserValue(byte[] encKey, byte[] docid)
        {
            byte[] full = new byte[_padding.Length + docid.Length];
            Array.Copy(_padding, full, _padding.Length);
            Array.Copy(docid, 0, full, _padding.Length, docid.Length);

            byte[] digest = MD5(full);
            byte[] userkey = new byte[16];

            Array.Copy(digest, 0, userkey, 0, 16);
            //for (int k = 16; k < 32; ++k)
            //    userkey[k] = 0;

            for (byte op = 0; op < Rev3RC4Count; ++op)
            {
                for (int i = 0; i < encKey.Length; i++)
                    digest[i] = (byte)(encKey[i] ^ op);
                byte[] result = Scryber.Secure.RC4.Encrypt(digest, userkey);
                Array.Copy(result, 0, userkey, 0, 16);
            }
            Array.Resize<byte>(ref userkey, 32);
            for (int k = 16; k < 32; ++k)
                userkey[k] = 0;

            return userkey;
        }
    }
}
