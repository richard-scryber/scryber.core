using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Secure;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// The managed MD5 used by the PDF standard security handler (System.Security.Cryptography.MD5 is not in the browser),
    /// and documents encrypted with it, checked with the platform MD5 and the PDF specification's own algorithms.
    /// </summary>
    [TestClass]
    public class HtmlEncryptionMD5_Tests
    {
        private static string Hex(byte[] data)
        {
            return Convert.ToHexString(data).ToLowerInvariant();
        }

        [TestMethod]
        public void EncryptionMD5_01_RFC1321Vectors()
        {
            //RFC 1321 appendix A.5
            var vectors = new[]
            {
                ("", "d41d8cd98f00b204e9800998ecf8427e"),
                ("a", "0cc175b9c0f1b6a831c399e269772661"),
                ("abc", "900150983cd24fb0d6963f7d28e17f72"),
                ("message digest", "f96b697d7cb7938d525a2f31aaf161d0"),
                ("abcdefghijklmnopqrstuvwxyz", "c3fcd3d76192e4007dfb496cca67e13b"),
                ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", "d174ab98d277d9f5a5611c2c9f419d9f"),
                ("12345678901234567890123456789012345678901234567890123456789012345678901234567890", "57edf4a22be3c955ac49da2e2107b67a")
            };

            foreach (var (input, expected) in vectors)
            {
                var hash = MD5Managed.ComputeHash(Encoding.ASCII.GetBytes(input));
                Assert.AreEqual(expected, Hex(hash), "The MD5 of '" + input + "'");
            }
        }

        [TestMethod]
        public void EncryptionMD5_02_MatchesThePlatformMD5_ForEveryLengthTo200()
        {
            //every length covers the padding boundaries at 55, 56, 63, 64, 65, 119, 120, 127 and 128
            var random = new Random(1321);
            for (int length = 0; length <= 200; length++)
            {
                var data = new byte[length];
                random.NextBytes(data);

                var expected = System.Security.Cryptography.MD5.HashData(data);
                CollectionAssert.AreEqual(expected, MD5Managed.ComputeHash(data), "The hash of " + length + " bytes");
            }
        }

        [TestMethod]
        public void EncryptionMD5_03_MatchesThePlatformMD5_ForLargeData()
        {
            var random = new Random(29);
            foreach (var length in new[] { 1000, 4096, 65535, 1024 * 1024 + 7 })
            {
                var data = new byte[length];
                random.NextBytes(data);

                var expected = System.Security.Cryptography.MD5.HashData(data);
                CollectionAssert.AreEqual(expected, MD5Managed.ComputeHash(data), "The hash of " + length + " bytes");
            }
        }

        [TestMethod]
        public void EncryptionMD5_04_OffsetAndCount_HashOnlyThatRange()
        {
            var data = new byte[300];
            new Random(10).NextBytes(data);

            foreach (var (offset, count) in new[] { (0, 0), (1, 55), (7, 56), (100, 64), (3, 200), (299, 1), (300, 0) })
            {
                var expected = System.Security.Cryptography.MD5.HashData(data.AsSpan(offset, count));
                CollectionAssert.AreEqual(expected, MD5Managed.ComputeHash(data, offset, count), "The hash of " + count + " bytes from " + offset);
            }

            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => MD5Managed.ComputeHash(data, 250, 51));
            Assert.ThrowsExactly<ArgumentNullException>(() => MD5Managed.ComputeHash(null));
        }

        //
        // encrypted documents, checked with the platform MD5 and RC4 written here
        //

        private const string OwnerPassword = "the-owner-password";
        private const string UserPassword = "open-sesame";
        private const string Content = "Hello Encrypted World";

        //the text is written as a hex string
        private static readonly string ContentHex = "<" + Convert.ToHexString(Encoding.ASCII.GetBytes(Content)) + ">";

        private static readonly byte[] Padding =
        {
            0x28, 0xBF, 0x4E, 0x5E, 0x4E, 0x75, 0x8A, 0x41, 0x64, 0x00, 0x4E, 0x56, 0xFF, 0xFA, 0x01, 0x08,
            0x2E, 0x2E, 0x00, 0xB6, 0xD0, 0x68, 0x3E, 0x80, 0x2F, 0x0C, 0xA9, 0xFE, 0x64, 0x53, 0x69, 0x7A
        };

        private class EncryptionDictionary
        {
            public int R; public int Length; public int P;
            public byte[] O; public byte[] U; public byte[] Id;
            public string Text;
        }

        private static EncryptionDictionary Generate(string encryption, string user)
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta name='print-restrictions' content='printing' />" +
                       "<meta name='print-encryption' content='" + encryption + "' /></head><body><p>" + Content + "</p></body></html>";

            using (var doc = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new MemoryStream())
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.RenderOptions.PasswordProvider = new DocumentPasswordProvider(OwnerPassword, user);
                doc.SaveAsPDF(ms);

                var text = Encoding.Latin1.GetString(ms.ToArray());
                var dict = Regex.Match(text, @"<<\s*/Filter\s*/Standard(?<body>.*?)>>", RegexOptions.Singleline);
                Assert.IsTrue(dict.Success, "The document is encrypted");
                var body = dict.Groups["body"].Value;

                int Number(string name, int fallback)
                {
                    var match = Regex.Match(body, "/" + name + @"\s+(-?\d+)");
                    return match.Success ? int.Parse(match.Groups[1].Value) : fallback;
                }

                byte[] FromHex(string source, string pattern) => Convert.FromHexString(Regex.Match(source, pattern).Groups[1].Value);

                return new EncryptionDictionary
                {
                    R = Number("R", 0),
                    Length = Number("Length", 40),
                    P = Number("P", 0),
                    O = FromHex(body, @"/O\s*<([0-9A-Fa-f]+)>"),
                    U = FromHex(body, @"/U\s*<([0-9A-Fa-f]+)>"),
                    Id = FromHex(text, @"/ID\s*\[\s*<([0-9A-Fa-f]+)>"),
                    Text = text
                };
            }
        }

        private static byte[] Pad(byte[] password)
        {
            return password.Take(32).Concat(Padding).Take(32).ToArray();
        }

        private static byte[] MD5(byte[] data)
        {
            return System.Security.Cryptography.MD5.HashData(data);
        }

        /// <summary>The file key from a padded user password (algorithm 2)</summary>
        private static byte[] FileKey(EncryptionDictionary enc, byte[] paddedUser)
        {
            var n = enc.Length / 8;
            var key = MD5(paddedUser.Concat(enc.O).Concat(BitConverter.GetBytes(enc.P)).Concat(enc.Id).ToArray());
            if (enc.R >= 3)
            {
                for (var i = 0; i < 50; i++)
                    key = MD5(key.Take(n).ToArray());
            }
            return key.Take(n).ToArray();
        }

        /// <summary>The check of a user password (algorithms 4 and 5), returning the file key when it opens the document</summary>
        private static byte[] OpenWithUser(EncryptionDictionary enc, byte[] paddedUser)
        {
            var key = FileKey(enc, paddedUser);

            if (enc.R == 2)
                return Rc4(key, Padding).SequenceEqual(enc.U) ? key : null;

            var u = Rc4(key, MD5(Padding.Concat(enc.Id).ToArray()));
            for (byte i = 1; i <= 19; i++)
                u = Rc4(key.Select(b => (byte)(b ^ i)).ToArray(), u);

            return u.SequenceEqual(enc.U.Take(16)) ? key : null;
        }

        /// <summary>The check of an owner password (algorithm 7) - it decrypts O to the padded user password</summary>
        private static byte[] OpenWithOwner(EncryptionDictionary enc, string owner)
        {
            var n = enc.Length / 8;
            var hash = MD5(Pad(Encoding.Latin1.GetBytes(owner)));
            if (enc.R >= 3)
            {
                for (var i = 0; i < 50; i++)
                    hash = MD5(hash);
            }
            var key = hash.Take(n).ToArray();

            byte[] user;
            if (enc.R == 2)
                user = Rc4(key, enc.O);
            else
            {
                user = enc.O;
                for (int i = 19; i >= 0; i--)
                    user = Rc4(key.Select(b => (byte)(b ^ i)).ToArray(), user);
            }

            return OpenWithUser(enc, user);
        }

        /// <summary>Decrypts every stream with its object key (algorithm 1), and returns them all as one string</summary>
        private static string DecryptStreams(EncryptionDictionary enc, byte[] fileKey)
        {
            var all = new StringBuilder();
            foreach (Match obj in Regex.Matches(enc.Text, @"(\d+)\s+(\d+)\s+obj\b(?<dict>(?:(?!endobj).)*?)stream\r?\n", RegexOptions.Singleline))
            {
                var length = Regex.Match(obj.Groups["dict"].Value, @"/Length\s+(\d+)");
                if (!length.Success)
                    continue;

                var num = int.Parse(obj.Groups[1].Value);
                var gen = int.Parse(obj.Groups[2].Value);
                var start = obj.Index + obj.Length;
                var data = Encoding.Latin1.GetBytes(enc.Text.Substring(start, int.Parse(length.Groups[1].Value)));

                var objKey = MD5(fileKey.Concat(new[] { (byte)num, (byte)(num >> 8), (byte)(num >> 16), (byte)gen, (byte)(gen >> 8) }).ToArray());
                objKey = objKey.Take(Math.Min(fileKey.Length + 5, 16)).ToArray();

                all.Append(Encoding.Latin1.GetString(Rc4(objKey, data)));
            }
            return all.ToString();
        }

        private static byte[] Rc4(byte[] key, byte[] data)
        {
            var s = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
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

        private static void AssertOpens(string encryption, int revision, int keyBits)
        {
            var enc = Generate(encryption, UserPassword);
            Assert.AreEqual(revision, enc.R, "The security revision");
            Assert.AreEqual(keyBits, enc.Length, "The key length");
            Assert.IsFalse(enc.Text.Contains(Content) || enc.Text.Contains(ContentHex), "The content is not readable without decrypting it");

            var userKey = OpenWithUser(enc, Pad(Encoding.Latin1.GetBytes(UserPassword)));
            Assert.IsNotNull(userKey, "The document opens with the user password");
            Assert.IsNull(OpenWithUser(enc, Pad(Encoding.Latin1.GetBytes(""))), "The document does not open without a password");
            Assert.IsNull(OpenWithUser(enc, Pad(Encoding.Latin1.GetBytes("open-sesame!"))), "The document does not open with the wrong password");

            var ownerKey = OpenWithOwner(enc, OwnerPassword);
            Assert.IsNotNull(ownerKey, "The document opens with the owner password");
            CollectionAssert.AreEqual(userKey, ownerKey, "Both passwords give the same file key");
            Assert.IsNull(OpenWithOwner(enc, "not-the-owner"), "The document does not open with the wrong owner password");

            Assert.IsTrue(DecryptStreams(enc, userKey).Contains(ContentHex), "The page content decrypts with the file key");

            //and with no user password, it opens with an empty one
            var open = Generate(encryption, null);
            var openKey = OpenWithUser(open, Pad(Array.Empty<byte>()));
            Assert.IsNotNull(openKey, "Restricted, but no password is needed to open it");
            Assert.IsNotNull(OpenWithOwner(open, OwnerPassword), "and the owner password still opens it");
            Assert.IsTrue(DecryptStreams(open, openKey).Contains(ContentHex));
        }

        [TestMethod]
        public void EncryptionMD5_05_128Bit_OpensWithEachPassword()
        {
            AssertOpens("128bit", 3, 128);
        }

        [TestMethod]
        public void EncryptionMD5_06_40Bit_OpensWithEachPassword()
        {
            AssertOpens("40bit", 2, 40);
        }
    }
}
