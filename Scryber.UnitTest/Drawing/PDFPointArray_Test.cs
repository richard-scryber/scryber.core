using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Drawing
{
    [TestClass()]
    public class PDFPointArray_Test
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

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayConstructor_1()
        {
            PDFPointArray ary = new PDFPointArray();
            Assert.IsNotNull(ary);
            Assert.AreEqual(ary.Count, 0);
        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayConstructor_2()
        {
            List<PDFPoint> list = new List<PDFPoint>();
            list.Add(new PDFPoint(1, 1));
            list.Add(new PDFPoint(2, 2));
            list.Add(new PDFPoint(3, 3));

            PDFPointArray ary = new PDFPointArray(list);
            Assert.IsNotNull(ary);
            Assert.AreEqual(list.Count, ary.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i], ary[i]);
            }
        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayConstructor_3()
        {
            
            PDFPoint pt1 = new PDFPoint(1, 1);
            PDFPoint pt2 = new PDFPoint(2, 2);
            PDFPoint pt3 = new PDFPoint(3, 3);

            PDFPointArray ary = new PDFPointArray(pt1, pt2, pt3);
            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);
            Assert.AreEqual(pt1, ary[0]);
            Assert.AreEqual(pt2, ary[1]);
            Assert.AreEqual(pt3, ary[2]);
            
        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayConstructor_4()
        {
            PDFUnit x1 = 1;
            PDFUnit y1 = 2;
            PDFUnit x2 = 3;
            PDFUnit y2 = 4;
            PDFUnit x3 = 5;
            PDFUnit y3 = 6;

            PDFPointArray ary = new PDFPointArray(x1, y1, x2, y2, x3, y3);

            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            Assert.AreEqual(new PDFPoint(x1, y1), ary[0]);
            Assert.AreEqual(new PDFPoint(x2, y2), ary[1]);
            Assert.AreEqual(new PDFPoint(x3, y3), ary[2]);

        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayAdd()
        {
            PDFPoint pt1 = new PDFPoint(1, 1);
            PDFPoint pt2 = new PDFPoint(2, 2);
            PDFPoint pt3 = new PDFPoint(3, 3);

            PDFPointArray ary = new PDFPointArray();
            Assert.IsNotNull(ary);
            Assert.AreEqual(0, ary.Count);

            ary.Add(pt1);
            ary.Add(pt2);
            ary.Add(pt3);
            Assert.AreEqual(3, ary.Count);

            PDFPoint pt4 = new PDFPoint(4, 5);
            ary.Add(pt4);

        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayItem()
        {
            PDFPoint pt1 = new PDFPoint(1, 1);
            PDFPoint pt2 = new PDFPoint(2, 2);
            PDFPoint pt3 = new PDFPoint(3, 3);

            PDFPointArray ary = new PDFPointArray();
            Assert.IsNotNull(ary);
            Assert.AreEqual(0, ary.Count);

            ary.Add(pt1);
            ary.Add(pt2);
            ary.Add(pt3);
            Assert.AreEqual(3, ary.Count);

            Assert.AreEqual(pt2, ary[1]);

            PDFPoint pt4 = new PDFPoint(4, 5);
            ary.Add(pt4);

            Assert.AreEqual(pt4, ary[3]);

            try
            {
                PDFPoint notvalid = ary[-1];
                throw new InvalidOperationException("No exception thrown when accessing a point out side of the bounds of the PointArray");
            }
            catch (ArgumentOutOfRangeException)
            {
                //Expected
            }

            try
            {
                PDFPoint notvalid = ary[ary.Count];
                throw new InvalidOperationException("No exception thrown when accessing a point out side of the bounds of the PointArray");
            }
            catch (ArgumentOutOfRangeException)
            {
                //Expected
            }


        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayAddRange()
        {
            List<PDFPoint> list = new List<PDFPoint>();
            list.Add(new PDFPoint(1, 1));
            list.Add(new PDFPoint(2, 2));
            list.Add(new PDFPoint(3, 3));

            PDFPointArray ary = new PDFPointArray(list);
            Assert.IsNotNull(ary);
            Assert.AreEqual(list.Count, ary.Count);
            

            //Add the range and make sure they are appended.
            ary.AddRange(list);
            Assert.AreEqual(list.Count * 2, ary.Count);

            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i], ary[i + list.Count]);
            }
        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayClear()
        {

            PDFPoint pt1 = new PDFPoint(1, 1);
            PDFPoint pt2 = new PDFPoint(2, 2);
            PDFPoint pt3 = new PDFPoint(3, 3);

            PDFPointArray ary = new PDFPointArray(pt1, pt2, pt3);
            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            ary.Clear();

            Assert.AreEqual(0, ary.Count);

            try
            {
                PDFPoint notvalid = ary[0];
                throw new InvalidOperationException("No exception thrown when accessing a point out side of the bounds of the PointArray");
            }
            catch (ArgumentOutOfRangeException)
            {
                //Expected
            }
        }

        

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayRemove()
        {

            PDFPoint pt1 = new PDFPoint(1, 1);
            PDFPoint pt2 = new PDFPoint(2, 2);
            PDFPoint pt3 = new PDFPoint(3, 3);

            PDFPointArray ary = new PDFPointArray(pt1, pt2, pt3);
            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            bool wasremoved = ary.Remove(pt2);

            Assert.AreEqual(true, wasremoved);
            Assert.AreEqual(2, ary.Count);
            Assert.AreEqual(pt3, ary[1]);

            try
            {
                PDFPoint notvalid = ary[2];
                throw new InvalidOperationException("No exception thrown when accessing a point out side of the bounds of the PointArray");
            }
            catch (ArgumentOutOfRangeException)
            {
                //Expected
            }

            //not present in array
            wasremoved = ary.Remove(new PDFPoint(4, 4));

            Assert.AreEqual(false, wasremoved);
            Assert.AreEqual(2, ary.Count);
            Assert.AreEqual(pt3, ary[1]);
        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayRemoveAt()
        {

            PDFPoint pt1 = new PDFPoint(1, 1);
            PDFPoint pt2 = new PDFPoint(2, 2);
            PDFPoint pt3 = new PDFPoint(3, 3);

            PDFPointArray ary = new PDFPointArray(pt1, pt2, pt3);
            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            ary.RemoveAt(1);

            Assert.AreEqual(2, ary.Count);
            Assert.AreEqual(pt3, ary[1]);

            try
            {
                PDFPoint notvalid = ary[2];
                throw new InvalidOperationException("No exception thrown when accessing a point out side of the bounds of the PointArray");
            }
            catch (ArgumentOutOfRangeException)
            {
                //Expected
            }

            try
            {
                ary.RemoveAt(2);
                throw new InvalidOperationException("No exception thrown when removing a point out side of the bounds of the PointArray");
            }
            catch (ArgumentOutOfRangeException)
            {
                //Expected
            }
        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayToArray_3()
        {

            PDFPoint pt1 = new PDFPoint(1, 1);
            PDFPoint pt2 = new PDFPoint(2, 2);
            PDFPoint pt3 = new PDFPoint(3, 3);

            PDFPointArray ary = new PDFPointArray(pt1, pt2, pt3);
            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            PDFPoint[] all = ary.ToArray();
            Assert.AreEqual(3, all.Length);

            Assert.AreEqual(pt1, all[0]);
            Assert.AreEqual(pt2, all[1]);
            Assert.AreEqual(pt3, all[2]);

        }
    }
}
