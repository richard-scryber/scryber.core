using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFTableStyleTest and is intended
    ///to contain all PDFTableStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFContentDescriptorTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        //
        // content: ... parsing tests
        //

        [TestMethod]
        public void ContentTextDescriptorTest()
        {
            var actual = "\"This is the content\"";
            var parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentTextDescriptor));
            var text = parsed as ContentTextDescriptor;
            Assert.AreEqual("This is the content", text.Text);
            Assert.IsNull(text.Next);
            Assert.AreEqual(ContentDescriptorType.Text, text.Type);

            actual = "'Single quote test'";
            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentTextDescriptor));
            text = parsed as ContentTextDescriptor;
            Assert.AreEqual("Single quote test", text.Text);
            Assert.IsNull(text.Next);
            Assert.AreEqual(ContentDescriptorType.Text, text.Type);

            actual = "\"\\f6dd\"";
            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentTextDescriptor));
            text = parsed as ContentTextDescriptor;
            Assert.AreEqual(char.ConvertFromUtf32(int.Parse("f6dd", System.Globalization.NumberStyles.HexNumber)).ToString(), text.Text);
            Assert.IsNull(text.Next);
            Assert.AreEqual(ContentDescriptorType.Text, text.Type);

        }

        [TestMethod]
        public void ContentImageDescriptorTest()
        {
            var actual = "url(pathto.image/source.png)";
            var parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentImageDescriptor));
            var img = parsed as ContentImageDescriptor;
            Assert.AreEqual("url(pathto.image/source.png)", img.Text);
            Assert.AreEqual("pathto.image/source.png", img.Source);
            Assert.IsNull(img.Next);
            Assert.AreEqual(ContentDescriptorType.Image, img.Type);

            actual = "url(\"second.pathto.image/source.png\")";
            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentImageDescriptor));
            img = parsed as ContentImageDescriptor;
            Assert.AreEqual("url(\"second.pathto.image/source.png\")", img.Text);
            Assert.AreEqual("second.pathto.image/source.png", img.Source);
            Assert.IsNull(img.Next);
            Assert.AreEqual(ContentDescriptorType.Image, img.Type);

            actual = "url('second.pathto.image/source.png')";
            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentImageDescriptor));
            img = parsed as ContentImageDescriptor;
            Assert.AreEqual("url('second.pathto.image/source.png')", img.Text);
            Assert.AreEqual("second.pathto.image/source.png", img.Source);
            Assert.IsNull(img.Next);
            Assert.AreEqual(ContentDescriptorType.Image, img.Type);
        }

        [TestMethod]
        public void ContentGradientDescriptorTest()
        {
            //Linear

            var actual = "linear-gradient(#e66465, #9198e5)";
            var parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentGradientDescriptor));
            var grad = parsed as ContentGradientDescriptor;
            Assert.AreEqual("linear-gradient(#e66465, #9198e5)", grad.Text);
            Assert.IsInstanceOfType(grad.Gradient, typeof(GradientLinearDescriptor));

            var linear = grad.Gradient as GradientLinearDescriptor;

            Assert.AreEqual(GradientType.Linear, linear.GradientType);
            Assert.AreEqual(2, linear.Colors.Count);

            Assert.IsNull(grad.Next);
            Assert.AreEqual(ContentDescriptorType.Gradient, grad.Type);

            //Radial

            actual = "radial-gradient(#e66465, #9198e5)";
            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentGradientDescriptor));
            grad = parsed as ContentGradientDescriptor;
            Assert.AreEqual("radial-gradient(#e66465, #9198e5)", grad.Text);
            Assert.IsInstanceOfType(grad.Gradient, typeof(GradientRadialDescriptor));

            var radial = grad.Gradient as GradientRadialDescriptor;

            Assert.AreEqual(GradientType.Radial, radial.GradientType);
            Assert.AreEqual(2, radial.Colors.Count);

            Assert.IsNull(grad.Next);
            Assert.AreEqual(ContentDescriptorType.Gradient, grad.Type);

            //Repeating Linear

            actual = "repeating-linear-gradient(to top left, lightpink, lightpink 5px, white 5px, white 10px)";
            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentGradientDescriptor));
            grad = parsed as ContentGradientDescriptor;
            Assert.AreEqual("repeating-linear-gradient(to top left, lightpink, lightpink 5px, white 5px, white 10px)", grad.Text);
            Assert.IsInstanceOfType(grad.Gradient, typeof(GradientLinearDescriptor));

            linear = grad.Gradient as GradientLinearDescriptor;

            Assert.AreEqual(GradientType.Linear, linear.GradientType);
            Assert.IsTrue(linear.Repeating);
            Assert.AreEqual(4, linear.Colors.Count);

            Assert.IsNull(grad.Next);
            Assert.AreEqual(ContentDescriptorType.Gradient, grad.Type);

            //Repeating Radial

            actual = "repeating-radial-gradient(powderblue, powderblue 8px, white 8px,  white 16px )";
            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentGradientDescriptor));
            grad = parsed as ContentGradientDescriptor;
            Assert.AreEqual("repeating-radial-gradient(powderblue, powderblue 8px, white 8px,  white 16px )", grad.Text);
            Assert.IsInstanceOfType(grad.Gradient, typeof(GradientRadialDescriptor));

            radial = grad.Gradient as GradientRadialDescriptor;

            Assert.AreEqual(GradientType.Radial, radial.GradientType);
            Assert.AreEqual(4, radial.Colors.Count);
            Assert.IsTrue(radial.Repeating);

            Assert.IsNull(grad.Next);
            Assert.AreEqual(ContentDescriptorType.Gradient, grad.Type);
        }

        [TestMethod]
        public void ContentCounterDescriptorTest()
        {
            var actual = "counter(with-name)";
            var parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentCounterDescriptor));
            var count = parsed as ContentCounterDescriptor;
            Assert.AreEqual("counter(with-name)", count.Text);
            Assert.AreEqual("with-name", count.CounterName);
            Assert.IsNull(count.Next);
            Assert.AreEqual(ContentDescriptorType.Counter, count.Type);


            actual = "counter( with-a-name )";
            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentCounterDescriptor));
            count = parsed as ContentCounterDescriptor;
            Assert.AreEqual("counter( with-a-name )", count.Text);
            Assert.AreEqual("with-a-name", count.CounterName);
            Assert.IsNull(count.Next);
            Assert.AreEqual(ContentDescriptorType.Counter, count.Type);


            //Not supported format
            bool thrown = false;
            try
            {
                actual = "counter( with-a-name, \".\", decimal)";
                parsed = ContentDescriptor.Parse(actual);
            }
            catch(ArgumentException)
            {
                thrown = true;
            }
            Assert.IsTrue(thrown, "An exception was NOT thrown with a NON supported counter format");

            
        }

        [TestMethod()]
        public void ContentCountersDescriptorTest()
        {

            var actual = "counters(with-name,'.')";

            var parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.AreEqual(ContentDescriptorType.Counters, parsed.Type);
            Assert.IsInstanceOfType(parsed, typeof(ContentCountersDescriptor));

            var count = parsed as ContentCountersDescriptor;
            Assert.AreEqual("counters(with-name,'.')", count.Text);
            Assert.AreEqual("with-name", count.CounterName);
            Assert.AreEqual(".", count.Separator);
            Assert.AreEqual(ListNumberingGroupStyle.Decimals, count.Format);

            Assert.IsNull(count.Next);
           


            actual = "counters( with-a-name, \" -- \", upper-roman )";

            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.AreEqual(ContentDescriptorType.Counters, parsed.Type);
            Assert.IsInstanceOfType(parsed, typeof(ContentCountersDescriptor));

            count = parsed as ContentCountersDescriptor;
            Assert.AreEqual("counters( with-a-name, \" -- \", upper-roman )", count.Text);
            Assert.AreEqual("with-a-name", count.CounterName);
            Assert.AreEqual(" -- ", count.Separator);
            Assert.AreEqual(ListNumberingGroupStyle.UppercaseRoman, count.Format);

            Assert.IsNull(count.Next);

            
        }

        [TestMethod]
        public void ContentQuoteDescriptorTest()
        {
            var actual = "open-quote";
            var parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentQuoteDescriptor));
            var count = parsed as ContentQuoteDescriptor;
            Assert.AreEqual(actual, count.Text);
            Assert.AreEqual("“", count.Chars);
            Assert.IsNull(count.Next);
            Assert.AreEqual(ContentDescriptorType.Quote, count.Type);


            actual = "close-quote";
            parsed = ContentDescriptor.Parse(actual);
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(ContentQuoteDescriptor));
            count = parsed as ContentQuoteDescriptor;
            Assert.AreEqual(actual, count.Text);
            Assert.AreEqual("”", count.Chars);
            Assert.IsNull(count.Next);
            Assert.AreEqual(ContentDescriptorType.Quote, count.Type);
        }


        //
        //Actual CSS Parsing Styles
        //



    }
}
