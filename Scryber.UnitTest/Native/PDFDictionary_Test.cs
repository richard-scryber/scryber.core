using Scryber.PDF.Native;
using Scryber.PDF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using System.Collections.Generic;
using System.Collections;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFDictionary_Test and is intended
    ///to contain all PDFDictionary_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFDictionary_Test
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
        ///A test for PDFDictionary Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFDictionaryConstructor_Test()
        {
            PDFDictionary target = new PDFDictionary();
            int expected = 0;
            int actual = target.Count;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Add_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1); 
            target.Add(key, value);
            Assert.AreEqual(1, target.Count);

            PDFName key2 = new PDFName("Item2");
            target.Add(key2, value);
            Assert.AreEqual(2, target.Count);

            try
            {
                target.Add(key, value);
                Assert.Fail("Did not raise argument exception for duplicate key");
            }
            catch (ArgumentException)
            {
                TestContext.WriteLine("Successfully caught a duplicate key exception");
            }

        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Clear_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            target.Add(key, value);

            PDFName key2 = new PDFName("Item2");
            target.Add(key2, value);
            Assert.AreEqual(2, target.Count);

            target.Clear();
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>
        ///A test for ContainsKey
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void ContainsKey_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            target.Add(key, value);

            PDFName key2 = new PDFName("Item2");
            target.Add(key2, value);
            Assert.AreEqual(2, target.Count);

            bool expected = true;
            bool actual;
            actual = target.ContainsKey(key);
            Assert.AreEqual(expected, actual);

            key = new PDFName("NotIncluded");
            expected = false;
            actual = target.ContainsKey(key);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetEnumerator
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void GetEnumerator_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            target.Add(key, value);

            PDFName key2 = new PDFName("Item2");
            target.Add(key2, value);
            Assert.AreEqual(2, target.Count);


            int expected = 2;
            int actual = 0;
            foreach (KeyValuePair<PDFName,IFileObject> pair in target)
            {
                actual++;
            }
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Remove_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            target.Add(key, value);

            PDFName key2 = new PDFName("Item2");
            target.Add(key2, value);
            Assert.AreEqual(2, target.Count);

            bool expected = true;
            bool actual;
            actual = target.Remove(key);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, target.Count);

            expected = false;
            key = new PDFName("NotFound");
            actual = target.Remove(key);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, target.Count);

        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Scryber.Native.PDFName,Scryber.IFileObject>>.Add
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("PDF Native")]
        public void Add_Test1()
        {
            ICollection<KeyValuePair<PDFName, IFileObject>> target = new PDFDictionary(); 
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            KeyValuePair<PDFName, IFileObject> item = new KeyValuePair<PDFName, IFileObject>(key, value); // TODO: Initialize to an appropriate value
            target.Add(item);
            Assert.AreEqual(1, target.Count);

            PDFName key2 = new PDFName("Item2");
            item = new KeyValuePair<PDFName, IFileObject>(key2, value);
            target.Add(item);
            Assert.AreEqual(2, target.Count);
        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Scryber.Native.PDFName,Scryber.IFileObject>>.Contains
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("PDF Native")]
        public void Contains_Test()
        {
            ICollection<KeyValuePair<PDFName, IFileObject>> target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            KeyValuePair<PDFName, IFileObject> item = new KeyValuePair<PDFName, IFileObject>(key, value);
            target.Add(item);

            PDFName key2 = new PDFName("Item2");
            item = new KeyValuePair<PDFName, IFileObject>(key2, value);
            target.Add(item);

            bool expected = true;
            bool actual = target.Contains(item);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Scryber.Native.PDFName,Scryber.IFileObject>>.CopyTo
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("PDF Native")]
        public void CopyTo_Test()
        {
            PDFDictionary dictionary = new PDFDictionary();
            ICollection<KeyValuePair<PDFName, IFileObject>> target = dictionary;
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            KeyValuePair<PDFName, IFileObject> item = new KeyValuePair<PDFName, IFileObject>(key, value); // TODO: Initialize to an appropriate value
            target.Add(item);

            PDFName key2 = new PDFName("Item2");
            item = new KeyValuePair<PDFName, IFileObject>(key2, value);
            target.Add(item);

            KeyValuePair<PDFName, IFileObject>[] array = new KeyValuePair<PDFName,IFileObject>[3];
            int arrayIndex = 1;
            target.CopyTo(array, arrayIndex);

            Assert.IsNotNull(array[1]);
            Assert.IsNotNull(array[2]);

            PDFName arraykey = array[1].Key;
            IFileObject expected = dictionary[arraykey];
            IFileObject actual = array[1].Value;

            Assert.AreEqual(expected, actual);

            arraykey = array[2].Key;
            expected = dictionary[arraykey];
            actual = array[2].Value;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Scryber.Native.PDFName,Scryber.IFileObject>>.Remove
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("PDF Native")]
        public void Remove_Test1()
        {
            ICollection<KeyValuePair<PDFName, IFileObject>> target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            KeyValuePair<PDFName, IFileObject> item = new KeyValuePair<PDFName, IFileObject>(key, value); // TODO: Initialize to an appropriate value
            target.Add(item);

            PDFName key2 = new PDFName("Item2");
            item = new KeyValuePair<PDFName, IFileObject>(key2, value);
            target.Add(item);

            Assert.AreEqual(2, target.Count);

            bool expected = true;
            bool actual = target.Remove(item);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, target.Count);

        }

        /// <summary>
        ///A test for System.Collections.IEnumerable.GetEnumerator
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("PDF Native")]
        public void GetEnumerator_Test1()
        {
            ICollection<KeyValuePair<PDFName, IFileObject>> target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            KeyValuePair<PDFName, IFileObject> item = new KeyValuePair<PDFName, IFileObject>(key, value);
            target.Add(item);

            PDFName key2 = new PDFName("Item2");
            item = new KeyValuePair<PDFName, IFileObject>(key2, value);
            target.Add(item);

            int index = 0;
            foreach (KeyValuePair<PDFName,IFileObject> pair in target)
            {
                index++;
            }

            Assert.AreEqual(2, index);
        }

        /// <summary>
        ///A test for TryGetValue
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void TryGetValue_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            target.Add(key,value);

            PDFName key2 = new PDFName("Item2");
            target.Add(key2,value);

            bool expected = true;
            bool actual;

            IFileObject valueExpected = value;
            actual = target.TryGetValue(key, out value);

            Assert.AreEqual(valueExpected, value);
            Assert.AreEqual(expected, actual);

            expected = false;
            key = new PDFName("NotFound");
            actual = target.TryGetValue(key, out value);

            Assert.AreEqual(expected, actual);
            Assert.IsNull(value);

        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteData_Test()
        {
            //TODO: PDFDictionary.WriteData test
        }

        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Count_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            target.Add(key, value);

            PDFName key2 = new PDFName("Item2");
            target.Add(key2, value);

            int expected = 2;
            int actual;
            actual = target.Count;
            Assert.AreEqual(expected, actual, "Count returned the  wrong value");
        }

        
        /// <summary>
        ///A test for IsReadOnly
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void IsReadOnly_Test()
        {
            PDFDictionary target = new PDFDictionary();
            bool expected = false;
            bool actual;
            actual = target.IsReadOnly;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Item_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            target.Add(key, value);

            PDFName key2 = new PDFName("Item2");
            IFileObject value2 = new PDFNumber(2);
            target.Add(key2, value2);

            
            IFileObject actual;
            actual = target[key];

            Assert.AreEqual(value, actual);

            actual = target[key2];
            Assert.AreEqual(value2, actual);

            target[key2] = value;
            actual = target[key2];
            Assert.AreEqual(value, actual);

            PDFName newkey = new PDFName("NewKey");

            try
            {
                actual = target[newkey];
                Assert.Fail("Did not raise an exception for an invalid key");
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                this.TestContext.WriteLine("Successfully caught the argument exception");
            }

            PDFNumber newValue = new PDFNumber(5);
            target[newkey] = newValue;
            actual = target[newkey];

            Assert.AreEqual(newValue, actual);

        }

        /// <summary>
        ///A test for Keys
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Keys_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            target.Add(key, value);

            PDFName key2 = new PDFName("Item2");
            IFileObject value2 = new PDFNumber(2);
            target.Add(key2, value2);

            ICollection<PDFName> actual;
            actual = target.Keys;

            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);

            bool hasone = false;
            bool hastwo = false;

            foreach (PDFName item in actual)
            {
                if (item.Equals(key))
                    hasone = true;
                else if (item.Equals(key2))
                    hastwo = true;
                else
                    Assert.Fail("Unknown key returned");
            }

            Assert.IsTrue(hasone);
            Assert.IsTrue(hastwo);
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            PDFDictionary target = new PDFDictionary();
            ObjectType expected = ObjectTypes.Dictionary;

            ObjectType actual;
            actual = target.Type;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Values
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Values_Test()
        {
            PDFDictionary target = new PDFDictionary();
            PDFName key = new PDFName("Item1");
            IFileObject value = new PDFNumber(1);
            target.Add(key, value);

            PDFName key2 = new PDFName("Item2");
            IFileObject value2 = new PDFNumber(2);
            target.Add(key2, value2);

            ICollection<IFileObject> actual;
            actual = target.Values;

            bool hasone = false;
            bool hastwo = false;

            foreach (IFileObject item in actual)
            {
                if (item.Equals(value))
                    hasone = true;
                else if (item.Equals(value2))
                    hastwo = true;
                else
                    Assert.Fail("Unknown value returned");
            }

            Assert.IsTrue(hasone);
            Assert.IsTrue(hastwo);
        }
    }
}
