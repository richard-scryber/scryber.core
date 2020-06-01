using Scryber.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Scryber;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFStream_Test and is intended
    ///to contain all PDFStream_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFStream_Test : IStreamFactory
    {
        #region IStreamFactory interface

        public PDFStream CreateStream(IStreamFilter[] filters, IIndirectObject forobject)
        {
            PDFIndirectObject indobj = (PDFIndirectObject)forobject;
            return new PDFStream(filters, indobj);
        }

        #endregion

        /// <summary>
        /// Proxy class for the PDFStream
        /// </summary>
        public class StreamProxy : PDFStream
        {
            public StreamProxy(IStreamFilter[] filters, PDFIndirectObject obj) 
                : base(filters, obj) { }

            public StreamProxy(Stream stream, IStreamFilter[] filters, PDFIndirectObject obj, bool ownsstream)
                : base(stream, filters, obj, ownsstream) { }

            public Stream InnerStreamProxy()
            {
                return this.InnerStream;
            }

            public bool OwnsStreamProxy()
            {
                return this.OwnsStream;
            }

            
        }

        public IStreamFilter[] _usefilters = new IStreamFilter[] { };

        #region public TestContext TestContext{get;set;}
    
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

        #endregion

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PDFStream Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFStreamConstructor_Test()
        {
            Stream stream = null;
            IStreamFilter[] filters = _usefilters;
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            bool ownstream = false;

            PDFStream target;
            try
            {
                target = new StreamProxy(stream, filters, indobj, ownstream);
                Assert.Fail("No Exception thrown for a null stream");
            }
            catch (ArgumentNullException)
            {
                TestContext.WriteLine("Sucessfully caught a null reference for the stream");
            }

            stream = new MemoryStream();
            
            target = new StreamProxy(stream, filters, indobj, ownstream);
            Assert.AreEqual(target.IndirectObject, indobj);
            Assert.AreEqual(filters, target.Filters);
            
            target.Dispose();
            stream.Dispose();
            indobj.Dispose();
        }

        /// <summary>
        ///A test for PDFStream Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFStreamConstructor_Test1()
        {
            IStreamFilter[] filters = _usefilters;
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(filters, indobj);
            Assert.IsNotNull(target);

            StreamProxy proxy = (StreamProxy)target;
            Assert.IsNotNull(proxy.InnerStreamProxy());
            Assert.IsTrue(proxy.OwnsStreamProxy());

            target.Dispose();
            indobj.Dispose();
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Dispose_Test()
        {
            IStreamFilter[] filters = _usefilters;
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            StreamProxy proxy = new StreamProxy(filters, indobj);
            PDFStream target = proxy;

            target.Dispose();
            //As the target owns the inner stream it should be disposed.
            Assert.IsNull(proxy.InnerStreamProxy());

            MemoryStream ms = new MemoryStream();
            bool ownsStream = false;
            proxy = new StreamProxy(ms, filters, indobj, ownsStream);
            target = proxy;
            
            target.Dispose();
            Assert.IsNull(proxy.InnerStreamProxy()); //reference should have gone from the stream

            ms.WriteByte(12); //this will fail if the stream has been disposed.

            ms.Dispose();
            indobj.Dispose();
            
        }

        

        

        /// <summary>
        ///A test for GetStreamData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void GetStreamData_Test()
        {
            MemoryStream ms = new MemoryStream();
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(ms, _usefilters, indobj, false);

            byte[] expected = new byte[] {};
            byte[] actual;
            actual = target.GetStreamData();
            Assert.AreEqual(expected.Length, actual.Length);

            expected = new byte[] { 1, 2, 3, 4, 5 };
            target.Write(expected);
            target.Flush();

            actual = target.GetStreamData();

            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }

            target.Dispose();
            ms.Dispose();
            indobj.Dispose();
        }

        

        /// <summary>
        ///A test for Write
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Write_Test()
        {
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(_usefilters, indobj);

            byte[] expected = new byte[] {0, 10, 1, 2, 3, 4, 5, 34, 35 };
            target.Write(expected, 2, 5);
            target.Flush();
            byte[] actual = target.GetStreamData();
            
            byte[] copied = new byte[5];
            Array.Copy(expected, 2, copied, 0, 5);

            CompareByteArray(copied, actual);

            target.Dispose();
            indobj.Dispose();
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Write_Test1()
        {
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(_usefilters, indobj);

            byte[] expected = new byte[] { 1, 2, 3, 4, 5 };
            target.Write(expected);
            target.Flush();
            byte[] actual = target.GetStreamData();

            CompareByteArray(expected, actual);

            target.Dispose();
            indobj.Dispose();
        }

        

        /// <summary>
        ///A test for Write
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Write_Test2()
        {
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(_usefilters, indobj);

            string source = "Hello world";
            target.Write(source);
            target.Flush();

            byte[] actual = target.GetStreamData();
            byte[] expected = System.Text.Encoding.ASCII.GetBytes(source);
           
            CompareByteArray(expected, actual);

            target.Dispose();
            indobj.Dispose();
        }

        /// <summary>
        ///A test for WriteLine
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteLine_Test()
        {
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(_usefilters, indobj);

            string source = "Hello world";
            target.WriteLine(source);
            target.Flush();

            byte[] actual = target.GetStreamData();
            byte[] expected = System.Text.Encoding.ASCII.GetBytes(source + "\r\n");

            CompareByteArray(expected, actual);

            target.Dispose();
            indobj.Dispose();
        }


        /// <summary>
        ///A test for WriteLine
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteLine_Test1()
        {
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(_usefilters, indobj);

            string source = "Hello world";
            target.Write(source);
            target.WriteLine();
            target.Flush();

            byte[] actual = target.GetStreamData();
            byte[] expected = System.Text.Encoding.ASCII.GetBytes(source + "\r\n");

            CompareByteArray(expected, actual);

            target.Dispose();
            indobj.Dispose();
        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteData_Test()
        {
            IStreamFilter[] filters = _usefilters;
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream stream = new StreamProxy(filters, indobj);
            PDFWriter writer = null;

            try
            {
                stream.WriteData(writer);
                Assert.Fail("Expected an exception to be raised for PDFStream.WriteData()");
            }
            catch (NotSupportedException)
            {
                //Should raise an exception
            }
            stream.Dispose();
            indobj.Dispose();
        }

        
        /// <summary>
        ///A test for WriteTo
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteTo_Test()
        {
            MemoryStream stream = new MemoryStream();

            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(_usefilters, indobj);

            string source = "Hello world";
            target.WriteLine(source);
            target.Flush();

            byte[] actual;
            byte[] expected = System.Text.Encoding.ASCII.GetBytes(source + "\r\n");

            target.WriteTo(stream);
            actual = stream.ToArray();

            CompareByteArray(expected, actual);

            target.Dispose();
            stream.Dispose();
            indobj.Dispose();
        }

        /// <summary>
        ///A test for WriteTo
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteTo_Test1()
        {
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(_usefilters, indobj);

            PDFIndirectObject otherobj = new PDFIndirectObject(this);

            PDFStream other = new StreamProxy(_usefilters, otherobj);

            string source = "Hello world";
            target.WriteLine(source);
            target.Flush();

            
            byte[] expected = target.GetStreamData();
            target.WriteTo(other);
            byte[] actual = other.GetStreamData();

            CompareByteArray(expected, actual);

            target.Dispose();
            other.Dispose();
            indobj.Dispose();
            otherobj.Dispose();
        }

        /// <summary>
        /// Support method to compare an array of bytes for equality
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        private static void CompareByteArray(byte[] expected, byte[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }


        /// <summary>
        ///A test for Filters
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Filters_Test()
        {
            IStreamFilter[] filters = _usefilters;
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(filters, indobj);

            IStreamFilter[] expected = _usefilters; 
            IStreamFilter[] actual;
            target.Filters = expected;
            actual = target.Filters;
            Assert.AreEqual(expected.Length, actual.Length);
            
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }

            target.Dispose();
            indobj.Dispose();
        }

        /// <summary>
        ///A test for IndirectObject
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void IndirectObject_Test()
        {
            MemoryStream ms = new MemoryStream();
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(ms, _usefilters, indobj, false);

            PDFIndirectObject actual;
            actual = target.IndirectObject;

            Assert.AreEqual(actual, indobj);

            target.Dispose();
            indobj.Dispose();
        }

        

        /// <summary>
        ///A test for Length
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Length_Test()
        {

            MemoryStream ms = new MemoryStream();
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(ms, _usefilters, indobj, false);

            long expected = ms.Length;
            long actual = target.Length;
            Assert.AreEqual(expected, actual);

            target.WriteLine("This is a test of the length");
            target.Write(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 });
            target.Flush();

            expected = ms.Length;
            actual = target.Length;

            Assert.AreEqual(expected, actual);

            target.Dispose();
            indobj.Dispose();
        }

        

        /// <summary>
        ///A test for Position
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Position_Test()
        {
            MemoryStream ms = new MemoryStream();
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(ms, _usefilters, indobj, false);

            long expected = ms.Position;
            long actual = target.Position;
            Assert.AreEqual(expected, actual);

            target.WriteLine("This is a test of the length");
            target.Write(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 });
            target.Flush();

            expected = ms.Position;
            actual = target.Position;

            Assert.AreEqual(expected, actual);
            
            ms.Dispose();
            target.Dispose();
            indobj.Dispose();

        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            IStreamFilter[] filters = _usefilters;
            PDFIndirectObject indobj = new PDFIndirectObject(this);
            PDFStream target = new StreamProxy(filters, indobj);
            PDFObjectType actual;
            actual = target.Type;
            PDFObjectType expected = PDFObjectTypes.Stream;

            Assert.AreEqual(expected, actual);

            target.Dispose();
            indobj.Dispose();
        }
    }
}
