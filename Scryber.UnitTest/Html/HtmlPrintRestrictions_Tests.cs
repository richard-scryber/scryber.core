using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Secure;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// The print-restrictions meta, and the permission bits (/P) written for 128bit encryption.
    /// Each allowed word sets only its own bits.
    /// </summary>
    [TestClass]
    public class HtmlPrintRestrictions_Tests
    {
        // the permission bits in /P (bit 1 is the lowest)
        private const int PrintBit = 1 << 2;
        private const int ModifyBit = 1 << 3;
        private const int CopyBit = 1 << 4;
        private const int AnnotationsBit = 1 << 5;
        private const int FormsBit = 1 << 8;
        private const int AccessibilityBit = 1 << 9;
        private const int AssemblyBit = 1 << 10;
        private const int HighQualityPrintBit = 1 << 11;
        private const int AllBits = PrintBit | ModifyBit | CopyBit | AnnotationsBit | FormsBit | AccessibilityBit | AssemblyBit | HighQualityPrintBit;

        // bits 7, 8 and 13 to 32 must always be 1
        private const int ReservedBits = unchecked((int)0xFFFFF0C0);

        private static int GetPermissions(string restrictions)
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta name='print-restrictions' content='" + restrictions + "' />" +
                       "<meta name='print-encryption' content='128bit' /></head><body><p>Hello</p></body></html>";

            using (var doc = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new MemoryStream())
            {
                doc.RenderOptions.PasswordProvider = new DocumentPasswordProvider("owner-password");
                doc.SaveAsPDF(ms);

                var text = Encoding.Latin1.GetString(ms.ToArray());
                var match = Regex.Match(text, @"/Filter\s*/Standard.*?/P\s+(-?\d+)", RegexOptions.Singleline);
                Assert.IsTrue(match.Success, "The document was not encrypted");
                return int.Parse(match.Groups[1].Value);
            }
        }

        [TestMethod]
        public void PrintRestrictions_01_Printing_IsPrintAndHighQualityPrint()
        {
            var p = GetPermissions("printing");
            Assert.AreEqual(PrintBit | HighQualityPrintBit, p & AllBits);
            Assert.AreEqual(ReservedBits, p & ReservedBits, "The reserved bits are set");
        }

        [TestMethod]
        public void PrintRestrictions_02_Modifications_IsModifyAndAssemblyOnly()
        {
            var p = GetPermissions("modifications");
            Assert.AreEqual(ModifyBit | AssemblyBit, p & AllBits, "Document assembly does not also allow high quality printing");
            Assert.AreEqual(ReservedBits, p & ReservedBits, "The reserved bits are still set");
        }

        [TestMethod]
        public void PrintRestrictions_03_Forms_IsFormsOnly()
        {
            var p = GetPermissions("forms");
            Assert.AreEqual(FormsBit, p & AllBits, "Form filling does not also allow annotations");
        }

        [TestMethod]
        public void PrintRestrictions_04_EachWord_IsOnlyItsOwnBits()
        {
            Assert.AreEqual(CopyBit, GetPermissions("copying") & AllBits);
            Assert.AreEqual(AnnotationsBit, GetPermissions("annotations") & AllBits);
            Assert.AreEqual(AccessibilityBit, GetPermissions("accessibility") & AllBits);
        }

        [TestMethod]
        public void PrintRestrictions_05_All_RestrictsEverything()
        {
            var p = GetPermissions("all");
            Assert.AreEqual(0, p & AllBits);
            Assert.AreEqual(ReservedBits, p & ReservedBits);
        }

        [TestMethod]
        public void PrintRestrictions_06_EveryWord_AllowsEverything()
        {
            var p = GetPermissions("printing modifications copying annotations forms accessibility");
            Assert.AreEqual(AllBits, p & AllBits);
            Assert.AreEqual(ReservedBits, p & ReservedBits);
        }

        [TestMethod]
        public void PrintRestrictions_07_NoRestrictions_IsNotEncrypted()
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><title>Open</title></head><body><p>Hello</p></body></html>";

            using (var doc = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new MemoryStream())
            {
                Assert.IsFalse(doc.RenderOptions.Permissions.HasRestrictions, "Everything is allowed by default");
                doc.SaveAsPDF(ms);

                var text = Encoding.Latin1.GetString(ms.ToArray());
                Assert.IsFalse(text.Contains("/Encrypt"), "A document with no restrictions is not encrypted");
            }
        }

        [TestMethod]
        public void PrintRestrictions_08_None_IsNotEncrypted()
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta name='print-restrictions' content='none' /></head><body><p>Hello</p></body></html>";

            using (var doc = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new MemoryStream())
            {
                doc.SaveAsPDF(ms);

                var text = Encoding.Latin1.GetString(ms.ToArray());
                Assert.IsFalse(text.Contains("/Encrypt"), "No restrictions, so not encrypted");
            }
        }

        private static byte[] GenerateWithUserPassword(string user)
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta name='print-restrictions' content='printing' /></head><body><p>Hello</p></body></html>";

            using (var doc = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new MemoryStream())
            {
                doc.RenderOptions.PasswordProvider = new DocumentPasswordProvider("owner-password", user);
                doc.SaveAsPDF(ms);
                return ms.ToArray();
            }
        }

        [TestMethod]
        public void PrintRestrictions_09_ALongPassword_OnlyUsesTheFirst32Characters()
        {
            var data = GenerateWithUserPassword("0123456789abcdefghijklmnopqrstuvwxyz-0123456789");
            var text = Encoding.Latin1.GetString(data);
            Assert.IsTrue(text.Contains("/Encrypt"), "The document is encrypted, rather than failing on a password longer than 32 characters");
        }

        [TestMethod]
        public void PrintRestrictions_10_APasswordOutsideLatin1_IsAClearError()
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><body><p>Hello</p></body></html>";

            using (var doc = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new MemoryStream())
            {
                doc.RenderOptions.PasswordProvider = new DocumentPasswordProvider("owner-password", "пароль");
                try
                {
                    doc.SaveAsPDF(ms);
                    Assert.Fail("A password outside Latin-1 cannot be used");
                }
                catch (Exception ex)
                {
                    var inner = ex;
                    while (null != inner && !(inner is ArgumentOutOfRangeException))
                        inner = inner.InnerException;

                    Assert.IsNotNull(inner, "The error is the password characters: " + ex.Message);
                    Assert.IsTrue(inner.Message.Contains("Latin-1"));
                    Assert.IsFalse(ex.ToString().Contains("пароль"), "The password is not in the error");
                }
            }
        }
    }
}
