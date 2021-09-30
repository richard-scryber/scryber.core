using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF.Secure
{
    /// <summary>
    /// Static (sealed) class for encrypting and decrypting using RC4 cryptography
    /// </summary>
    public static class RC4
    {
        /// <summary>
        /// Encrypts a string (data) using the specified key and returns the resultant string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encrypt(string key, string data)
        {
            Encoding unicode = Encoding.Unicode;
            byte[] keyB = unicode.GetBytes(key);
            byte[] dataB = unicode.GetBytes(data);
            byte[] enc = Encrypt(keyB, dataB);

            return Convert.ToBase64String(enc);
        }

        /// <summary>
        /// Decrypts a string (data) using the specified key and returns the resultant string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Decrypt(string key, string data)
        {
            Encoding unicode = Encoding.Unicode;
            byte[] keyB = unicode.GetBytes(key);
            byte[] dataB = unicode.GetBytes(data);
            byte[] dec = Decrypt(keyB, dataB);
            return unicode.GetString(dec);
        }

        /// <summary>
        /// Encrypts the binary data using the specified binary key and returns the resultant binary data.
        /// </summary>
        /// <param name="key">the binary key</param>
        /// <param name="data">the binary data to encrypt</param>
        /// <returns>The encrypted data as a byte array</returns>
        public static byte[] Encrypt(byte[] key, byte[] data)
        {
            return EncryptOutput(key, data).ToArray();
        }

        /// <summary>
        /// Decrypts the binary data using the specified key and returns the decrypted data.
        /// </summary>
        /// <param name="key">The key to use to decrypt</param>
        /// <param name="data">The data to decrypt</param>
        /// <returns>The decrypted data as a byte array</returns>
        public static byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptOutput(key, data).ToArray();
        }

        //
        // encryption methods
        //

        private static byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 256)
              .Select(i => (byte)i)
              .ToArray();

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;

                Swap(s, i, j);
            }

            return s;
        }

        private static IEnumerable<byte> EncryptOutput(byte[] key, IEnumerable<byte> data)
        {
            byte[] s = EncryptInitalize(key);

            int i = 0;
            int j = 0;

            return data.Select((b) =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;

                Swap(s, i, j);

                return (byte)(b ^ s[(s[i] + s[j]) & 255]);
            });
        }

        private static void Swap(byte[] s, int i, int j)
        {
            byte c = s[i];

            s[i] = s[j];
            s[j] = c;
        }
    }
}
