using Scryber.PDF.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using System.Collections.Generic;
using System.Collections;
using Scryber.PDF;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFArray_Test and is intended
    ///to contain all PDFArray_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFArray_Test
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
        ///A test for PDFArray Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFArrayConstructor_Test()
        {
            IEnumerable<IFileObject> items = null; // TODO: Initialize to an appropriate value
            PDFArray target = new PDFArray(items);
            Assert.AreEqual(target.Count, 0);

            items = new IFileObject[] { new PDFNumber(1), new PDFReal(2.0) };
            target = new PDFArray(items);

            Assert.AreEqual(2, target.Count, "Initiailized array with entries was not created");
        }

        /// <summary>
        ///A test for PDFArray Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFArrayConstructor_Test1()
        {
            PDFArray target = new PDFArray();
            Assert.AreEqual(0, target.Count, "Inited array was not empty");
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Add_Test()
        {
            PDFArray target = new PDFArray();
            IFileObject item = new PDFNumber(1);
            target.Add(item);
            Assert.AreEqual(1, target.Count);

            target.Add(item);
            Assert.AreEqual(2, target.Count, "Entries not added to array");

        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Clear_Test()
        {
            PDFArray target = new PDFArray(new IFileObject[] { new PDFNumber(1), new PDFNumber(2) });
            Assert.AreEqual(2, target.Count);
            target.Clear();
            Assert.AreEqual(0, target.Count, "Entries not cleared from array");
        }

        /// <summary>
        ///A test for Contains
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Contains_Test()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2) };
            PDFArray target = new PDFArray(all);
            
            Assert.AreEqual(all[0], all[0], "Basic assertion that PDFNumbers are equal failed");

            IFileObject tofind = all[0];
            bool contains = target.Contains(tofind);
            Assert.IsTrue(contains);

            tofind = all[1];
            contains = target.Contains(tofind);
            Assert.IsTrue(contains);

            tofind = new PDFNumber(4);
            contains = target.Contains(tofind);
            Assert.IsFalse(contains);

        }

        /// <summary>
        ///A test for CopyTo
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void CopyTo_Test()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2), new PDFNumber(3) };
            PDFArray target = new PDFArray(all);


            IFileObject[] array = new IFileObject[all.Length];
            int arrayIndex = 0;
            target.CopyTo(array, arrayIndex);

            Assert.AreEqual(all[0], array[0]);
            Assert.AreEqual(all[1], array[1]);
            Assert.AreEqual(all[2], array[2]);

            array = new IFileObject[all.Length + 2];
            arrayIndex = 2;
            target.CopyTo(array, arrayIndex);

            Assert.IsNull(array[0]);
            Assert.IsNull(array[1]);

            Assert.AreEqual(all[0], array[2]);
            Assert.AreEqual(all[1], array[3]);
            Assert.AreEqual(all[2], array[4]);

        }

        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Indexer_Test()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2), new PDFNumber(3) };
            PDFArray target = new PDFArray(all);

            IFileObject expected = all[0];
            IFileObject actual = target[0];
            Assert.AreEqual(expected, actual);

            expected = all[1];
            actual = target[1];
            Assert.AreEqual(expected, actual);

            expected = all[2];
            actual = target[2];
            Assert.AreEqual(expected, actual);

            try
            {
                actual = target[-1];
                Assert.Fail("Should have raised an Argument out of range exception");
            }
            catch (ArgumentOutOfRangeException)
            {
                TestContext.WriteLine("Successfully caught the Argument out of range exception");
            }

            try
            {
                actual = target[4];
                Assert.Fail("Should have raised an Argument out of range exception");
            }
            catch (ArgumentOutOfRangeException)
            {
                TestContext.WriteLine("Successfully caught the Argument out of range exception");
            }
        }

        /// <summary>
        ///A test for GetEnumerator
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void GetEnumerator_Test()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2), new PDFNumber(3) };
            PDFArray target = new PDFArray(all);

            int index = 0;
            foreach (IFileObject found in target)
            {
                Assert.AreEqual(all[index], found);
                index++;
            }
        }

        /// <summary>
        ///A test for System.Collections.IEnumerable.GetEnumerator
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("PDF Native")]
        public void GetEnumerator_Test1()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2), new PDFNumber(3) };
            IEnumerable target = new PDFArray(all);

            int index = 0;
            foreach (IFileObject found in target)
            {
                Assert.AreEqual(all[index], found);
                index++;
            }
        }

        /// <summary>
        ///A test for Scryber.IObjectContainer.Add
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("PDF Native")]
        public void Add_Test1()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2), new PDFNumber(3) };
            IObjectContainer target = new PDFArray(all);

            IFileObject obj = new PDFNumber(4);
            target.Add(obj);
            Assert.AreEqual(4, ((PDFArray)target).Count);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Remove_Test()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2), new PDFNumber(3) };
            PDFArray target = new PDFArray(all);
            IFileObject toremove = all[1]; //Number 2

            bool expected = true;
            bool actual;
            actual = target.Remove(toremove);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(2, target.Count);

            toremove = new PDFNumber(4);
            expected = false;
            actual = target.Remove(toremove);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(2, target.Count);
        }

        /// <summary>
        ///A test for RemoveAt
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void RemoveAt_Test()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2), new PDFNumber(3) };
            PDFArray target = new PDFArray(all);
            int index  = 1;

            Assert.AreEqual(3, target.Count);
            target.RemoveAt(index);
            Assert.AreEqual(2, target.Count);

            Assert.AreEqual(all[0], target[0]);
            Assert.AreEqual(all[2], target[1]);
        }



        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteData_Test()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2), new PDFNumber(3) };
            

            PDFArray target = new PDFArray(all);

            string actual;

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                using (PDFWriter writer = new PDFWriter14(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic)))
                {
                    target.WriteData(writer);
                    writer.InnerStream.Flush();

                }
                stream.Position = 0;

                using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                {
                    actual = sr.ReadToEnd();
                }
            }

            PDFArray parsed = PDFArray.Parse(actual.Trim());

            Assert.AreEqual(parsed.Count, target.Count);
            Assert.AreEqual(parsed[0], target[0]);
            Assert.AreEqual(parsed[1], target[1]);
            Assert.AreEqual(parsed[2], target[2]);

        }

        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Count_Test()
        {
            IFileObject[] all = new IFileObject[] { new PDFNumber(1), new PDFNumber(2), new PDFNumber(3) };
            PDFArray target = new PDFArray(all);
            int expected = all.Length;
            int actual = target.Count;
            Assert.AreEqual(expected, actual,"PDFArray.Count returned the incorrect value");

            target.Add(new PDFNumber(4));
            expected += 1;
            actual = target.Count;
            Assert.AreEqual(expected, actual, "PDFArray.Count returned the incorrect value");


        }

        

        /// <summary>
        ///A test for IsReadOnly
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void IsReadOnly_Test()
        {
            PDFArray target = new PDFArray();
            bool expected = false;
            bool actual;
            actual = target.IsReadOnly;
            Assert.AreEqual(expected, actual, "PDFArray.IsReadOnly returned true - which is wrong");
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            PDFArray target = new PDFArray();
            ObjectType actual;
            actual = target.Type;
            ObjectType expected = PDFObjectTypes.Array;
            Assert.AreEqual(expected, actual);
        }
    }
}
