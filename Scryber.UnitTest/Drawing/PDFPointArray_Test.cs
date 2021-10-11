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
            PointArray ary = new PointArray();
            Assert.IsNotNull(ary);
            Assert.AreEqual(ary.Count, 0);
        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayConstructor_2()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(1, 1));
            list.Add(new Point(2, 2));
            list.Add(new Point(3, 3));

            PointArray ary = new PointArray(list);
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
            
            Point pt1 = new Point(1, 1);
            Point pt2 = new Point(2, 2);
            Point pt3 = new Point(3, 3);

            PointArray ary = new PointArray(pt1, pt2, pt3);
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
            Unit x1 = 1;
            Unit y1 = 2;
            Unit x2 = 3;
            Unit y2 = 4;
            Unit x3 = 5;
            Unit y3 = 6;

            PointArray ary = new PointArray(x1, y1, x2, y2, x3, y3);

            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            Assert.AreEqual(new Point(x1, y1), ary[0]);
            Assert.AreEqual(new Point(x2, y2), ary[1]);
            Assert.AreEqual(new Point(x3, y3), ary[2]);

        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayAdd()
        {
            Point pt1 = new Point(1, 1);
            Point pt2 = new Point(2, 2);
            Point pt3 = new Point(3, 3);

            PointArray ary = new PointArray();
            Assert.IsNotNull(ary);
            Assert.AreEqual(0, ary.Count);

            ary.Add(pt1);
            ary.Add(pt2);
            ary.Add(pt3);
            Assert.AreEqual(3, ary.Count);

            Point pt4 = new Point(4, 5);
            ary.Add(pt4);

        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayItem()
        {
            Point pt1 = new Point(1, 1);
            Point pt2 = new Point(2, 2);
            Point pt3 = new Point(3, 3);

            PointArray ary = new PointArray();
            Assert.IsNotNull(ary);
            Assert.AreEqual(0, ary.Count);

            ary.Add(pt1);
            ary.Add(pt2);
            ary.Add(pt3);
            Assert.AreEqual(3, ary.Count);

            Assert.AreEqual(pt2, ary[1]);

            Point pt4 = new Point(4, 5);
            ary.Add(pt4);

            Assert.AreEqual(pt4, ary[3]);

            try
            {
                Point notvalid = ary[-1];
                throw new InvalidOperationException("No exception thrown when accessing a point out side of the bounds of the PointArray");
            }
            catch (ArgumentOutOfRangeException)
            {
                //Expected
            }

            try
            {
                Point notvalid = ary[ary.Count];
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
            List<Point> list = new List<Point>();
            list.Add(new Point(1, 1));
            list.Add(new Point(2, 2));
            list.Add(new Point(3, 3));

            PointArray ary = new PointArray(list);
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

            Point pt1 = new Point(1, 1);
            Point pt2 = new Point(2, 2);
            Point pt3 = new Point(3, 3);

            PointArray ary = new PointArray(pt1, pt2, pt3);
            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            ary.Clear();

            Assert.AreEqual(0, ary.Count);

            try
            {
                Point notvalid = ary[0];
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

            Point pt1 = new Point(1, 1);
            Point pt2 = new Point(2, 2);
            Point pt3 = new Point(3, 3);

            PointArray ary = new PointArray(pt1, pt2, pt3);
            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            bool wasremoved = ary.Remove(pt2);

            Assert.AreEqual(true, wasremoved);
            Assert.AreEqual(2, ary.Count);
            Assert.AreEqual(pt3, ary[1]);

            try
            {
                Point notvalid = ary[2];
                throw new InvalidOperationException("No exception thrown when accessing a point out side of the bounds of the PointArray");
            }
            catch (ArgumentOutOfRangeException)
            {
                //Expected
            }

            //not present in array
            wasremoved = ary.Remove(new Point(4, 4));

            Assert.AreEqual(false, wasremoved);
            Assert.AreEqual(2, ary.Count);
            Assert.AreEqual(pt3, ary[1]);
        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PointArrayRemoveAt()
        {

            Point pt1 = new Point(1, 1);
            Point pt2 = new Point(2, 2);
            Point pt3 = new Point(3, 3);

            PointArray ary = new PointArray(pt1, pt2, pt3);
            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            ary.RemoveAt(1);

            Assert.AreEqual(2, ary.Count);
            Assert.AreEqual(pt3, ary[1]);

            try
            {
                Point notvalid = ary[2];
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

            Point pt1 = new Point(1, 1);
            Point pt2 = new Point(2, 2);
            Point pt3 = new Point(3, 3);

            PointArray ary = new PointArray(pt1, pt2, pt3);
            Assert.IsNotNull(ary);
            Assert.AreEqual(3, ary.Count);

            Point[] all = ary.ToArray();
            Assert.AreEqual(3, all.Length);

            Assert.AreEqual(pt1, all[0]);
            Assert.AreEqual(pt2, all[1]);
            Assert.AreEqual(pt3, all[2]);

        }
    }
}
