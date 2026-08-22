using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.PDF.Resources;

namespace Scryber.Core.UnitTests.Drawing
{
    /// <summary>
    /// A single PDFFontWidths instance is shared across every document being rendered in the
    /// process: FontDefinition.GetWidths() hands the definition's own instance to each document
    /// for non-standard, non-Unicode fonts, and the FontFactory caches definitions for the life
    /// of the process. Rendering documents in parallel therefore calls RegisterGlyphs on one
    /// instance from many threads at once, so it must hold no per-call mutable state.
    /// </summary>
    [TestClass()]
    public class FontWidthsConcurrency_Test
    {
        private const int FirstChar = 32;
        private const int LastChar = 126;

        public TestContext TestContext { get; set; }

        private static PDFArrayFontWidths GetWidths()
        {
            var all = Enumerable.Repeat(500, LastChar - FirstChar + 1);
            return new PDFArrayFontWidths(FirstChar, LastChar, all, Scryber.OpenType.SubTables.CMapEncoding.MacRoman);
        }

        /// <summary>
        /// Regression test. RegisterGlyphs used to accumulate into a StringBuilder cached in a
        /// field, so two threads would reset and grow the same buffer at once. That threw
        /// ArgumentOutOfRangeException (Parameter 'chunkLength') out of StringBuilder.ToString,
        /// and when the interleaving missed that guard it silently returned another caller's
        /// partial text, writing the wrong glyphs into the PDF.
        /// </summary>
        [TestMethod()]
        public void RegisterGlyphs_SharedInstanceIsSafeAcrossThreads()
        {
            const string Content = "The quick brown fox jumps over the lazy dog, 0123456789.";
            const int Iterations = 2000;

            var widths = GetWidths();
            var threads = Math.Max(4, Environment.ProcessorCount);

            var errors = new ConcurrentQueue<string>();

            Parallel.For(0, threads, new ParallelOptions { MaxDegreeOfParallelism = threads }, _ =>
            {
                for (var i = 0; i < Iterations; i++)
                {
                    try
                    {
                        var glyphs = widths.RegisterGlyphs(Content);

                        //Every character is inside the width range, so the glyphs come back unchanged.
                        if (!string.Equals(Content, glyphs, StringComparison.Ordinal))
                            errors.Enqueue("Returned '" + glyphs + "' instead of the registered content.");
                    }
                    catch (Exception ex)
                    {
                        errors.Enqueue(ex.GetType().Name + ": " + ex.Message);
                    }
                }
            });

            Assert.AreEqual(0, errors.Count,
                "Concurrent calls to RegisterGlyphs on a shared instance failed. First failure: " +
                (errors.TryDequeue(out var first) ? first : string.Empty));
        }

        /// <summary>
        /// The offset overload takes a slice of a longer run, which is how the layout engine calls
        /// it once a line has been broken. It shares the same buffer, so it needs the same cover.
        /// </summary>
        [TestMethod()]
        public void RegisterGlyphsWithOffset_SharedInstanceIsSafeAcrossThreads()
        {
            const string Content = "Registering a longer run of characters so the slices differ in length.";
            const int Iterations = 2000;

            var widths = GetWidths();
            var threads = Math.Max(4, Environment.ProcessorCount);

            var errors = new ConcurrentQueue<string>();

            Parallel.For(0, threads, new ParallelOptions { MaxDegreeOfParallelism = threads }, index =>
            {
                //Each thread takes a different slice, so a shared buffer shows up as one thread
                //receiving another thread's length rather than only as an exception.
                var offset = index % 8;
                var count = Content.Length - offset;
                var expected = Content.Substring(offset, count);

                for (var i = 0; i < Iterations; i++)
                {
                    try
                    {
                        var glyphs = widths.RegisterGlyphs(Content, offset, count);

                        if (!string.Equals(expected, glyphs, StringComparison.Ordinal))
                            errors.Enqueue("Returned '" + glyphs + "' instead of '" + expected + "'.");
                    }
                    catch (Exception ex)
                    {
                        errors.Enqueue(ex.GetType().Name + ": " + ex.Message);
                    }
                }
            });

            Assert.AreEqual(0, errors.Count,
                "Concurrent calls to the offset overload of RegisterGlyphs failed. First failure: " +
                (errors.TryDequeue(out var first) ? first : string.Empty));
        }

        /// <summary>
        /// Characters outside the width range map to '?', and that substitution has to survive
        /// the change from the buffered implementation.
        /// </summary>
        [TestMethod()]
        public void RegisterGlyphs_SubstitutesCharactersOutsideTheWidthRange()
        {
            var widths = GetWidths();

            Assert.AreEqual("ab", widths.RegisterGlyphs("ab"), "Characters in range should be unchanged");
            Assert.AreEqual("?", widths.RegisterGlyphs("€"), "A character above LastChar should map to '?'");
            Assert.AreEqual("a?b", widths.RegisterGlyphs("a€b"), "Only the out of range character should be replaced");
            Assert.AreEqual("b", widths.RegisterGlyphs("abc", 1, 1), "The offset overload should return just the slice");
            Assert.AreEqual(string.Empty, widths.RegisterGlyphs("abc", 0, 0), "An empty count should return an empty string");
        }
    }
}
