using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// Reads the standard security handler entries (/Encrypt) of a PDF, and checks passwords against them with an implementation
    /// of the PDF specification algorithms (2, 5 and 7, revision 3) that is separate from the one Scryber writes with -
    /// so a test can check a document opens with (or without) a password, as a viewer would.
    /// </summary>
    public class PdfStandardSecurity
    {
        private static readonly byte[] Padding =
        {
            0x28, 0xBF, 0x4E, 0x5E, 0x4E, 0x75, 0x8A, 0x41, 0x64, 0x00, 0x4E, 0x56, 0xFF, 0xFA, 0x01, 0x08,
            0x2E, 0x2E, 0x00, 0xB6, 0xD0, 0x68, 0x3E, 0x80, 0x2F, 0x0C, 0xA9, 0xFE, 0x64, 0x53, 0x69, 0x7A
        };

        public int Version { get; private set; }
        public int Revision { get; private set; }
        public int KeyLengthBits { get; private set; }
        public int Permissions { get; private set; }
        public byte[] Owner { get; private set; }
        public byte[] User { get; private set; }
        public byte[] FileID { get; private set; }

        /// <summary>
        /// Reads the encryption entries from the PDF data, or returns null if the document is not encrypted
        /// </summary>
        public static PdfStandardSecurity Read(byte[] data)
        {
            var pdf = Encoding.Latin1.GetString(data);
            var encrypt = Regex.Match(pdf, @"<<\s*/Filter\s*/Standard(?<dict>.*?)>>", RegexOptions.Singleline);
            if (!encrypt.Success)
                return null;

            var dict = encrypt.Groups["dict"].Value;
            var id = Regex.Match(pdf, @"/ID\s*\[\s*<(?<id>[0-9A-Fa-f]+)>");
            Assert.IsTrue(id.Success, "The document has no file /ID");

            return new PdfStandardSecurity()
            {
                Version = int.Parse(Regex.Match(dict, @"/V\s+(\d+)").Groups[1].Value),
                Revision = int.Parse(Regex.Match(dict, @"/R\s+(\d+)").Groups[1].Value),
                KeyLengthBits = int.Parse(Regex.Match(dict, @"/Length\s+(\d+)").Groups[1].Value),
                Permissions = int.Parse(Regex.Match(dict, @"/P\s+(-?\d+)").Groups[1].Value),
                Owner = FromHex(Regex.Match(dict, @"/O\s*<([0-9A-Fa-f]+)>").Groups[1].Value),
                User = FromHex(Regex.Match(dict, @"/U\s*<([0-9A-Fa-f]+)>").Groups[1].Value),
                FileID = FromHex(id.Groups["id"].Value)
            };
        }

        /// <summary>
        /// Permission bits (1 based, as the specification): 3 print, 4 modify, 5 copy, 6 annotate, 9 fill forms,
        /// 10 accessibility, 11 assemble, 12 high quality print
        /// </summary>
        public bool IsAllowed(int bit)
        {
            return (this.Permissions & (1 << (bit - 1))) != 0;
        }

        /// <summary>
        /// True if the document opens with this user password (an empty string for no password)
        /// </summary>
        public bool IsUserPassword(string password)
        {
            return IsPaddedUserPassword(Pad(password));
        }

        /// <summary>
        /// True if this is the owner password of the document
        /// </summary>
        public bool IsOwnerPassword(string password)
        {
            this.AssertRevision3();

            //Algorithm 7: the owner key decrypts /O to the padded user password
            var hash = Md5(Pad(password));
            for (var i = 0; i < 50; i++)
                hash = Md5(Take(hash, this.KeyLength));
            var key = Take(hash, this.KeyLength);

            var user = (byte[])this.Owner.Clone();
            for (var i = 19; i >= 0; i--)
                user = Rc4(Xor(key, i), user);

            return IsPaddedUserPassword(user);
        }

        private int KeyLength
        {
            get { return this.KeyLengthBits / 8; }
        }

        private bool IsPaddedUserPassword(byte[] padded)
        {
            this.AssertRevision3();

            //Algorithm 2: the file key
            var input = new byte[32 + this.Owner.Length + 4 + this.FileID.Length];
            Buffer.BlockCopy(padded, 0, input, 0, 32);
            Buffer.BlockCopy(this.Owner, 0, input, 32, this.Owner.Length);
            var p = BitConverter.GetBytes(this.Permissions);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(p);
            Buffer.BlockCopy(p, 0, input, 32 + this.Owner.Length, 4);
            Buffer.BlockCopy(this.FileID, 0, input, 36 + this.Owner.Length, this.FileID.Length);

            var hash = Md5(input);
            for (var i = 0; i < 50; i++)
                hash = Md5(Take(hash, this.KeyLength));
            var key = Take(hash, this.KeyLength);

            //Algorithm 5: the first 16 bytes of /U
            var withId = new byte[32 + this.FileID.Length];
            Buffer.BlockCopy(Padding, 0, withId, 0, 32);
            Buffer.BlockCopy(this.FileID, 0, withId, 32, this.FileID.Length);

            var u = Rc4(key, Md5(withId));
            for (var i = 1; i <= 19; i++)
                u = Rc4(Xor(key, i), u);

            for (var i = 0; i < 16; i++)
            {
                if (u[i] != this.User[i])
                    return false;
            }
            return true;
        }

        private void AssertRevision3()
        {
            Assert.AreEqual(3, this.Revision, "Only revision 3 of the standard security handler is checked");
        }

        private static byte[] Pad(string password)
        {
            var bytes = Encoding.Latin1.GetBytes(password ?? string.Empty);
            var padded = new byte[32];
            var count = Math.Min(32, bytes.Length);
            Buffer.BlockCopy(bytes, 0, padded, 0, count);
            Buffer.BlockCopy(Padding, 0, padded, count, 32 - count);
            return padded;
        }

        private static byte[] Md5(byte[] data)
        {
            return System.Security.Cryptography.MD5.HashData(data);
        }

        private static byte[] Take(byte[] data, int count)
        {
            var result = new byte[count];
            Buffer.BlockCopy(data, 0, result, 0, count);
            return result;
        }

        private static byte[] Xor(byte[] key, int value)
        {
            var result = new byte[key.Length];
            for (var i = 0; i < key.Length; i++)
                result[i] = (byte)(key[i] ^ value);
            return result;
        }

        private static byte[] Rc4(byte[] key, byte[] data)
        {
            var s = new byte[256];
            for (var i = 0; i < 256; i++)
                s[i] = (byte)i;

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + s[i] + key[i % key.Length]) & 0xFF;
                (s[i], s[j]) = (s[j], s[i]);
            }

            var result = new byte[data.Length];
            for (int n = 0, i = 0, j = 0; n < data.Length; n++)
            {
                i = (i + 1) & 0xFF;
                j = (j + s[i]) & 0xFF;
                (s[i], s[j]) = (s[j], s[i]);
                result[n] = (byte)(data[n] ^ s[(s[i] + s[j]) & 0xFF]);
            }
            return result;
        }

        private static byte[] FromHex(string hex)
        {
            var result = new byte[hex.Length / 2];
            for (var i = 0; i < result.Length; i++)
                result[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            return result;
        }
    }
}
