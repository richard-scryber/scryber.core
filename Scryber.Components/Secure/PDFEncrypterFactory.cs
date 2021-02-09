using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Scryber.Secure
{
    internal abstract class PDFEncrypterFactory
    {
        internal const string PaddingBytesString = "28 BF 4E 5E 4E 75 8A 41 64 00 4E 56 FF FA 01 08 2E 2E 00 B6 D0 68 3E 80 2F 0C A9 FE 64 53 69 7A";
        internal static byte[] _padding = GetBytesFromHex(PaddingBytesString, true);
        internal const int PaddedPasswordStringLength = 32;


        /// <summary>
        /// Gets or sets the encryption version of this factory
        /// </summary>
        internal int Version
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the encryption revision of this factory
        /// </summary>
        internal int Revision
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the key length used for encryption
        /// </summary>
        internal int KeyLengthBytes
        {
            get;
            private set;
        }


        protected PDFEncrypterFactory(int version, int revision, int keylengthbytes)
        {
            this.Version = version;
            this.Revision = revision;
            this.KeyLengthBytes = keylengthbytes;
        }


        internal PDFEncryter InitEncrypter(SecureString ownerpassword, SecureString userpassword, PDFDocumentID documentid, PermissionFlags protection)
        {
            if (null == ownerpassword || ownerpassword.Length == 0)
                throw new ArgumentNullException("ownerpassword", "As a minimum the owner password is required");

            byte[] owner = null;
            byte[] user = null;
            PDFEncryter info;

            try
            {
                owner = ConvertToPaddedBytes(ownerpassword);
                user = ConvertToPaddedBytes(userpassword);
                info = this.InitEncrypter(owner, user, documentid, protection);
            }
            catch (Exception ex)
            {
                throw new PDFSecurityException("Could not create the encrypter information", ex);
            }
            finally
            {
                if (null != owner)
                    ZeroArray(owner);
                if (null != user)
                    ZeroArray(user);
            }

            return info;
        }


        internal PDFEncryter InitEncrypter(string ownerpassword, string userpassword, PDFDocumentID documentid, PermissionFlags protection)
        {
            if (string.IsNullOrEmpty(ownerpassword))
                throw new ArgumentNullException("ownerpassword", "As a minimum the owner password is required");

            byte[] owner = null;
            byte[] user = null;
            PDFEncryter info;

            try
            {
                owner = ConvertToPaddedBytes(ownerpassword);
                user = ConvertToPaddedBytes(userpassword);
                info = this.InitEncrypter(owner, user, documentid, protection);
            }
            catch (Exception ex)
            {
                throw new PDFSecurityException("Could not create the encrypter information", ex);
            }
            finally
            {
                if (null != owner)
                    ZeroArray(owner);
                if (null != user)
                    ZeroArray(user);
            }

            return info;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paddedownerpassword">The 32 byte padded owner password</param>
        /// <param name="paddeduserpassword"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        protected abstract PDFEncryter InitEncrypter(byte[] paddedownerpassword, byte[] paddeduserpassword, PDFDocumentID documentid, PermissionFlags protection);



        #region private static byte[] GetBytesFromHex(string hex, bool spaceseparated)

        /// <summary>
        /// Converts a hexadecimal string into a series of byte values.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static byte[] GetBytesFromHex(string hex, bool spaceseparated)
        {
            byte[] converted;
            if (spaceseparated)
            {
                string[] all = hex.Split(' ');
                converted = new byte[all.Length];
                for (int i = 0; i < all.Length; i++)
                {
                    string one = all[i];
                    byte b = byte.Parse(one, System.Globalization.NumberStyles.HexNumber);
                    converted[i] = b;
                }
                return converted;
            }
            else
            {
                converted = new byte[hex.Length / 2];

                for (int i = 0; i < converted.Length; i++)
                {
                    string part = hex.Substring(i * 2, 2);
                    byte b = byte.Parse(part, System.Globalization.NumberStyles.HexNumber);
                    converted[i] = b;
                }
                return converted;
            }

        }

        #endregion


        #region protected void ZeroArray(byte[] all)

        /// <summary>
        /// Clears a byte[] and sets all values to zero.
        /// </summary>
        /// <param name="all"></param>
        protected void ZeroArray(byte[] all)
        {
            for (int i = 0; i < all.Length; i++)
            {
                all[i] = 0;
            }
        }

        #endregion

        #region protected void ZeroArray(char[] all)

        /// <summary>
        /// Clears a char[] and sets all values to 'space's
        /// </summary>
        /// <param name="all"></param>
        protected void ZeroArray(char[] all)
        {
            if (null == all || all.Length == 0)
                return;

            for (int i = 0; i < all.Length; i++)
            {
                all[i] = ' ';
            }
        }

        #endregion

        #region protected byte[] ConvertToPaddedBytes(SecureString str) + 2 overloads

        /// <summary>
        /// Converts the secure string to a byte[] padded to a maximum 32 bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected byte[] ConvertToPaddedBytes(SecureString str)
        {
            byte[] padded = new byte[PaddedPasswordStringLength];
            if (null != str && str.Length > 0)
            {
                IntPtr passwordPointer = IntPtr.Zero;
                char[] passwordChars = null;
                try
                {
                    int passwordLength = str.Length;
                    passwordChars = new char[passwordLength];

                    // Copy the password from SecureString to our char array
                    passwordPointer = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(str);
                    System.Runtime.InteropServices.Marshal.Copy(passwordPointer, passwordChars, 0, passwordLength);
                }
                finally
                {
                    if (passwordPointer != IntPtr.Zero)
                        System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(passwordPointer);
                }
                for (int i = 0; i < passwordChars.Length; i++)
                {
                    padded[i] = (byte)passwordChars[i];
                }
                ZeroArray(passwordChars);
            }

            int start = str == null ? 0 : str.Length;
            for (int count = start, offset = 0; count < PaddedPasswordStringLength; count++, offset++)
            {
                padded[count] = _padding[offset];
            }
            return padded;
        }

        protected byte[] ConvertToPaddedBytes(string str)
        {
            char[] all = null;
            try
            {
                if (!string.IsNullOrEmpty(str))
                    all = str.ToCharArray();

                return ConvertToPaddedBytes(all);
            }
            finally
            {
                ZeroArray(all);
            }
        }

        protected byte[] ConvertToPaddedBytes(char[] chars)
        {
            byte[] padded = new byte[PaddedPasswordStringLength];

            if (null != chars && chars.Length > 0)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    if (i == PaddedPasswordStringLength)
                        break;
                    int c = chars[i];
                    if (c > 255)
                        throw new ArgumentOutOfRangeException("chars");
                    padded[i] = (byte)c;
                }

            }

            int start = chars == null ? 0 : chars.Length;
            for (int count = start, offset = 0; count < PaddedPasswordStringLength; count++, offset++)
            {
                padded[count] = _padding[offset];
            }
            return padded;
        }

        #endregion

        #region protected byte[] MD5(byte[] all)

        private System.Security.Cryptography.MD5 _md5;

        /// <summary>
        /// Performs an MD5 hash on the data
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        protected byte[] MD5(byte[] data)
        {
            if (null == _md5)
                _md5 = System.Security.Cryptography.MD5.Create();
            return _md5.ComputeHash(data);
        }

        #endregion

        #region protected byte[] ConvertIntToLowOrderArray(int value)

        /// <summary>
        /// Converts integer to byte array with the low order byte first
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected byte[] ConvertIntToLowOrderArray(int value)
        {
            byte[] all = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(all);
            return all;

        }

        #endregion


        #region protected byte[] GetBytes(int size, byte[] source)

        /// <summary>
        /// Copies 'size' bytes from the source into a new array of the required size and returns the result
        /// </summary>
        /// <param name="size"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        protected byte[] GetBytes(int size, byte[] source)
        {
            byte[] all = new byte[size];
            Array.Copy(source, 0, all, 0, size);
            return all;
        }

        #endregion

        //
        // static methods
        //

        #region internal static PDFSecurityInfoFactory CreateFactory(int version, int revison)

        /// <summary>
        /// Factory factory method.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="revison"></param>
        /// <returns></returns>
        internal static PDFEncrypterFactory CreateFactory(int version, int revison)
        {
            if (version == 1 && revison == 2)
                return new PDFEncrypterV1R2Factory();
            else if (version == 2 && revison == 3)
                return new PDFEncrypterV2R3Factory(PDFEncrypterV2R3.StandardKeyLength);
            else
                throw new ArgumentOutOfRangeException("version,revision", "Only version 1.2 or 2.3 are supported");
        }

        #endregion
    }

}
