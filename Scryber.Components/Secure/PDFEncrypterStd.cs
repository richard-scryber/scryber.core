using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Native;

namespace Scryber.Secure
{
    internal abstract class PDFEncrypterStd : PDFEncryter
    {


        internal PDFEncrypterStd(int version, int revision, int keysize, byte[] owner, byte[] user, byte[] key, PermissionFlags protection)
            : base(version, revision, keysize, owner, user, key, protection)
        {
        }

        internal override IStreamFilter CreateEncryptionFilter(int num, int gen)
        {
            byte[] key = GetEncryptionKey(num, gen);
            PDFRC4Filter filter = new PDFRC4Filter(key, this);

            return filter;
        }

        private byte[] GetEncryptionKey(int number, int generation)
        {
            int NumLengthBytes = 3;
            int GenLengthBytes = 2;
            byte[] full = new byte[this.EncryptionKeySizeBytes + NumLengthBytes + GenLengthBytes];
            Array.Copy(this.KeyStem, full, this.EncryptionKeySizeBytes);

            byte[] numlow = this.GetLowOrderArrayFromInt(number);
            byte[] genlow = this.GetLowOrderArrayFromInt(generation);
            Array.Copy(numlow, 0, full, this.EncryptionKeySizeBytes, NumLengthBytes);

            Array.Copy(genlow, 0, full, this.EncryptionKeySizeBytes + NumLengthBytes, GenLengthBytes);
            byte[] hash = this.MD5(full);

            //Take the Key Size and add 5 to it upto a max of 16 
            //This then identifies the bytes to use for the key
            int HashSizePlus = 5;
            int MaxHashKeySize = 16;
            int hashlength = this.EncryptionKeySizeBytes + HashSizePlus;
            if (hashlength > MaxHashKeySize)
                hashlength = MaxHashKeySize;

            Array.Resize(ref hash, hashlength);

            return hash;
        }

        internal override PDFObjectRef WriteTo(PDFWriter writer)
        {
            PDFObjectRef oref = writer.BeginObject("StandardEncryption");
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Filter", "Standard");
            this.WriteStandardSecurityEntries(writer);
            writer.EndDictionary();
            writer.EndObject();
            return oref;
        }

        protected virtual void WriteStandardSecurityEntries(PDFWriter writer)
        {
            writer.WriteDictionaryNumberEntry("V", this.EncryptionVersion);
            writer.WriteDictionaryNumberEntry("R", this.EncryptionRevision);
            writer.WriteDictionaryNumberEntry("Length", this.EncryptionKeySizeBytes * 8);
            writer.BeginDictionaryEntry("O");
            writer.WriteStringHex(this.OwnerHash);
            writer.EndDictionaryEntry();
            writer.BeginDictionaryEntry("U");
            writer.WriteStringHex(this.UserHash);
            writer.EndDictionaryEntry();
            writer.WriteDictionaryNumberEntry("P", (int)this.ProtectionFlags);

        }

        #region protected byte[] GetLowOrderArrayFromInt(int value)

        /// <summary>
        /// Converts integer to byte array with the low order byte first
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected byte[] GetLowOrderArrayFromInt(int value)
        {
            byte[] all = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(all);
            return all;

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
