using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using System.Collections.Generic;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFGraphicsPath_Test and is intended
    ///to contain all PDFGraphicsPath_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class GraphicsPath_Test
    {

        public class PDFGraphicsProxy : GraphicsPath
        {
            
            public PDFGraphicsProxy()
                : base()
            {
            }
        }

        #region public TestContext TestContext {get;set;}
        
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



        #region public void GraphicsPathConstructor_Test1()

        /// <summary>
        ///A test for PDFGraphicsPath Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void GraphicsPathConstructor_Test1()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.CurrentPath != null);
            Assert.IsTrue(target.HasCurrentPath);
            Assert.IsNotNull(target.Paths);
            Assert.IsTrue(target.Paths.Count == 1);
        }

        #endregion

        #region public void BeginPath_Test()

        /// <summary>
        ///A test for BeginPath
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void BeginPath_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            target.BeginPath();
            Assert.IsTrue(target.HasCurrentPath);
            Assert.IsTrue(target.Paths.Count == 2);
            Assert.AreEqual(target.CurrentPath.Count,0);
            
        }

        #endregion

        #region public void ClosePath_Test()

        /// <summary>
        ///A test for ClosePath
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void ClosePath_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            bool end = false;
            target.ClosePath(end);
            Assert.IsTrue(target.Paths.Count == 1,"Paths count after close was not 1");
            Assert.IsTrue(target.Paths[0].Count == 1, "First path does not have one entry");
            Assert.IsTrue(target.Paths[0].Operations.Count == 1,"First path operations count was not 1");
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathCloseData), "Opertation type was not a PathClose");

            //should still have a current path.
            Assert.IsTrue(target.HasCurrentPath,"There is no current path");
            Assert.IsNotNull(target.CurrentPath);


            end = true;
            target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            target.ClosePath(true);
            Assert.IsTrue(target.Paths.Count == 1, "Paths count after close was not 1");
            Assert.IsTrue(target.Paths[0].Count == 1, "First path does not have one entry");
            Assert.IsTrue(target.Paths[0].Operations.Count == 1, "First path operations count was not 1");
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathCloseData), "Opertation type was not a PathClose");

            //should NOT have a current path.
            Assert.IsFalse(target.HasCurrentPath, "There is a current path after ending it"); 
            Assert.IsNull(target.CurrentPath, "Current path is not null after ending path");
        }

        #endregion

        #region public void EndPath_Test()

        /// <summary>
        ///A test for EndPath
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void EndPath_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            target.EndPath();
            Assert.IsTrue(target.Paths.Count == 1, "Paths count after close was not 1");
            Assert.IsTrue(target.Paths[0].Count == 0, "First path should not have any entries");
            
            //should NOT have a current path.
            Assert.IsFalse(target.HasCurrentPath, "There is a current path after ending it");
            Assert.IsNull(target.CurrentPath, "Current path is not null after ending path");

        }

        #endregion

        #region public void MoveTo_Test()

        /// <summary>
        ///A test for MoveTo
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void MoveTo_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint pos = new PDFPoint(10,10);
            target.MoveTo(pos);

            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathMoveData));
            PathMoveData data = (PathMoveData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.MoveTo, pos);

            pos = new PDFPoint(40, 40);
            target.MoveTo(pos);
            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathMoveData));
            data = (PathMoveData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.MoveTo, pos);
        }

        #endregion

        #region public void MoveBy_Test()

        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void MoveBy_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint pos = new PDFPoint(10, 10);
            target.MoveTo(pos);

            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathMoveData));
            PathMoveData data = (PathMoveData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.MoveTo, pos);

            PDFPoint pos2 = new PDFPoint(40, 40);
            target.MoveBy(pos2);
            PDFPoint total = new PDFPoint(pos.X + pos2.X, pos.Y + pos2.Y);

            Assert.AreEqual(target.Cursor, total);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathMoveData));
            data = (PathMoveData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.MoveTo, total);
        }

        #endregion

        #region public void LineTo_Test()

        /// <summary>
        ///A test for LineTo
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void LineTo_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint pos = new PDFPoint(10, 10);
            target.LineTo(pos);

            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathLineData));
            PathLineData data = (PathLineData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.LineTo, pos);

            PDFPoint pos2 = new PDFPoint(40, 40);
            target.LineTo(pos2);
            
            Assert.AreEqual(target.Cursor, pos2);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathLineData));
            data = (PathLineData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.LineTo, pos2);
            
        }

        #endregion

        #region public void LineFor_Test()

        /// <summary>
        ///A test for LineTo
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void LineFor_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint pos = new PDFPoint(10, 10);
            target.LineTo(pos);

            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathLineData));
            PathLineData data = (PathLineData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.LineTo, pos);

            PDFPoint pos2 = new PDFPoint(40, 40);
            target.LineFor(pos2);
            PDFPoint total = new PDFPoint(pos.X + pos2.X, pos.Y + pos2.Y);

            Assert.AreEqual(target.Cursor, total);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathLineData));
            data = (PathLineData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.LineTo, total);

        }

        #endregion

        #region public void VerticalLineTo_Test()

        /// <summary>
        ///A test for VerticalLineTo
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void VerticalLineTo_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint pos = new PDFPoint(10, 10);
            target.LineTo(pos);

            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathLineData));
            PathLineData data = (PathLineData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.LineTo, pos);

            PDFUnit v = 40;
            target.VerticalLineTo(v);

            pos = new PDFPoint(pos.X, v);
            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathLineData));
            data = (PathLineData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.LineTo, pos);

        }

        #endregion

        #region public void VerticalLineFor_Test()

        /// <summary>
        ///A test for VerticalLineFor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void VerticalLineFor_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint pos = new PDFPoint(10, 10);
            target.LineTo(pos);

            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathLineData));
            PathLineData data = (PathLineData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.LineTo, pos);

            PDFUnit v = 40;
            target.VerticalLineFor(v);

            pos = new PDFPoint(pos.X, pos.Y + v);
            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathLineData));
            data = (PathLineData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.LineTo, pos);

        }

        #endregion

        #region public void HorizontalLineTo_Test()

        /// <summary>
        ///A test for HorizontalLineTo
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void HorizontalLineTo_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint pos = new PDFPoint(10, 10);
            target.LineTo(pos);

            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathLineData));
            PathLineData data = (PathLineData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.LineTo, pos);

            PDFUnit h = 40;
            target.HorizontalLineTo(h);

            pos = new PDFPoint(h, pos.Y);
            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathLineData));
            data = (PathLineData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.LineTo, pos);

        }

        #endregion

        #region public void HorizontalLineFor_Test()

        /// <summary>
        ///A test for HorizontalLineFor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void HorizontalLineFor_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint pos = new PDFPoint(10, 10);
            target.LineTo(pos);

            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathLineData));
            PathLineData data = (PathLineData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.LineTo, pos);

            PDFUnit h = 40;
            target.HorizontalLineFor(h);

            pos = new PDFPoint(pos.X + h, pos.Y);
            Assert.AreEqual(target.Cursor, pos);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathLineData));
            data = (PathLineData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.LineTo, pos);

        }

        #endregion

        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void CubicTo_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint end = new PDFPoint(100, 100);
            PDFPoint handleStart = new PDFPoint(0, 50);
            PDFPoint handleEnd = new PDFPoint(50, 100);

            target.CubicCurveTo(end, handleStart, handleEnd);

            Assert.AreEqual(target.Cursor, end);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathBezierCurveData));
            
            PathBezierCurveData data = (PathBezierCurveData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.Points.Length, 3);
            Assert.IsTrue(data.HasStartHandle);
            Assert.IsTrue(data.HasEndHandle);
            Assert.AreEqual(data.EndPoint, end);
            Assert.AreEqual(data.StartHandle, handleStart);
            Assert.AreEqual(data.EndHandle, handleEnd);

        }

        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void CubicToWithHandleStart_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint end = new PDFPoint(100, 100);
            PDFPoint handleStart = new PDFPoint(0, 50);
            PDFPoint handleEnd = new PDFPoint(50, 100);

            target.CubicCurveToWithHandleStart(end, handleStart);

            Assert.AreEqual(target.Cursor, end);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathBezierCurveData));

            PathBezierCurveData data = (PathBezierCurveData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.Points.Length, 3);
            Assert.IsTrue(data.HasStartHandle);
            Assert.IsFalse(data.HasEndHandle);
            Assert.AreEqual(data.EndPoint, end);
            Assert.AreEqual(data.StartHandle, handleStart);
            Assert.AreEqual(data.EndHandle, PDFPoint.Empty);
        }

        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void CubicToWithHandleEnd_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            Assert.AreEqual(target.Cursor, PDFPoint.Empty);

            PDFPoint end = new PDFPoint(100, 100);
            PDFPoint handleStart = new PDFPoint(0, 50);
            PDFPoint handleEnd = new PDFPoint(50, 100);

            target.CubicCurveToWithHandleEnd(end, handleEnd);

            Assert.AreEqual(target.Cursor, end);
            Assert.AreEqual(target.Paths[0].Operations.Count, 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathBezierCurveData));

            PathBezierCurveData data = (PathBezierCurveData)target.Paths[0].Operations[0];
            Assert.AreEqual(data.Points.Length, 3);
            Assert.IsFalse(data.HasStartHandle);
            Assert.IsTrue(data.HasEndHandle);
            Assert.AreEqual(data.EndPoint, end);
            Assert.AreEqual(data.StartHandle, PDFPoint.Empty);
            Assert.AreEqual(data.EndHandle, handleEnd);
        }

        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void CubicFor_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            PDFPoint pos = new PDFPoint(10, 10);
            target.MoveTo(pos);
            Assert.AreEqual(target.Cursor, pos);

            PDFPoint end = new PDFPoint(100, 100);
            PDFPoint handleStart = new PDFPoint(0, 50);
            PDFPoint handleEnd = new PDFPoint(50, 100);

            target.CubicCurveFor(end, handleStart, handleEnd);

            end = end.Offset(pos);
            handleEnd = handleEnd.Offset(pos);
            handleStart = handleStart.Offset(pos);

            Assert.AreEqual(target.Cursor, end);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathBezierCurveData));

            PathBezierCurveData data = (PathBezierCurveData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.Points.Length, 3);
            Assert.IsTrue(data.HasStartHandle);
            Assert.IsTrue(data.HasEndHandle);
            Assert.AreEqual(data.EndPoint, end);
            Assert.AreEqual(data.StartHandle, handleStart);
            Assert.AreEqual(data.EndHandle, handleEnd);
        }

        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void CubicForWithHandleStart_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            PDFPoint pos = new PDFPoint(10, 10);
            target.MoveTo(pos);
            Assert.AreEqual(target.Cursor, pos);

            PDFPoint end = new PDFPoint(100, 100);
            PDFPoint handleStart = new PDFPoint(0, 50);
            PDFPoint handleEnd = new PDFPoint(50, 100);

            target.CubicCurveForWithHandleStart(end, handleStart);

            end = end.Offset(pos);
            handleEnd = handleEnd.Offset(pos);
            handleStart = handleStart.Offset(pos);

            Assert.AreEqual(target.Cursor, end);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathBezierCurveData));

            PathBezierCurveData data = (PathBezierCurveData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.Points.Length, 3);
            Assert.IsTrue(data.HasStartHandle);
            Assert.IsFalse(data.HasEndHandle);
            Assert.AreEqual(data.EndPoint, end);
            Assert.AreEqual(data.StartHandle, handleStart);
            Assert.AreEqual(data.EndHandle, PDFPoint.Empty);
        }

        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void CubicForWithHandleEnd_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            PDFPoint pos = new PDFPoint(10, 10);
            target.MoveTo(pos);
            Assert.AreEqual(target.Cursor, pos);

            PDFPoint end = new PDFPoint(100, 100);
            PDFPoint handleStart = new PDFPoint(0, 50);
            PDFPoint handleEnd = new PDFPoint(50, 100);

            target.CubicCurveForWithHandleEnd(end, handleEnd);

            end = end.Offset(pos);
            handleEnd = handleEnd.Offset(pos);
            handleStart = handleStart.Offset(pos);

            Assert.AreEqual(target.Cursor, end);
            Assert.AreEqual(target.Paths[0].Operations.Count, 2);
            Assert.IsInstanceOfType(target.Paths[0].Operations[1], typeof(PathBezierCurveData));

            PathBezierCurveData data = (PathBezierCurveData)target.Paths[0].Operations[1];
            Assert.AreEqual(data.Points.Length, 3);
            Assert.IsFalse(data.HasStartHandle);
            Assert.IsTrue(data.HasEndHandle);
            Assert.AreEqual(data.EndPoint, end);
            Assert.AreEqual(data.StartHandle, PDFPoint.Empty);
            Assert.AreEqual(data.EndHandle, handleEnd);
        }

        


        /// <summary>
        ///A test for Bounds
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void Bounds_Test()
        {
            GraphicsPath target = new GraphicsPath();
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.HasCurrentPath);

            PDFPoint pos = new PDFPoint(10, 10);
            target.MoveTo(pos);

            PDFPoint end = new PDFPoint(100, 100);
            PDFPoint handleStart = new PDFPoint(0, 50);
            PDFPoint handleEnd = new PDFPoint(50, 100);

            target.CubicCurveForWithHandleEnd(end, handleEnd);

            end = end.Offset(pos);
            handleEnd = handleEnd.Offset(pos);
            handleStart = handleStart.Offset(pos);

            PDFRect bounds = target.Bounds;

            Assert.AreEqual(bounds.X, PDFUnit.Zero);
            Assert.AreEqual(bounds.Y, PDFUnit.Zero);
            Assert.AreEqual(bounds.Width, end.X);
            Assert.AreEqual(bounds.Height, end.Y);
        }

        /* Path construction options (matches svg)
         
          
                Com.    Parameters	    Name	            Description
                M       x,y	            moveto	            Moves pen to specified point x,y without drawing.
                m	    x,y	            moveto	            Moves pen to specified point x,y relative to current pen location, without drawing.
 
                L	    x,y	            lineto	            Draws a line from current pen location to specified point x,y .
                l	    x,y	            lineto	            Draws a line from current pen location to specified point x,y relative to current pen location.
 
                H	    x	            horizontal 	        Draws a horizontal line to the point defined by (specified x, pens current y).
                h	    x       	    horizontal 	        Draws a horizontal line to the point defined by (pens current x + specified x, pens current y). The x is relative to the current pens x position.
 
                V	    y	            vertical	        Draws a vertical line to the point defined by (pens current x, specified y).
                v	    y	            vertical	        Draws a vertical line to the point defined by (pens current x, pens current y + specified y). The y is relative to the pens current y-position.
 
                C	x1,y1 x2,y2 x,y	    curveto	            Draws a cubic Bezier curve from current pen point to x,y. x1,y1 and x2,y2 are start and end control points of the curve, controlling how it bends.
                c	x1,y1 x2,y2 x,y	    curveto	            Same as C, but interprets coordinates relative to current pen point.
 
                S	x2,y2 x,y	        smooth curve	    Draws a cubic Bezier curve from current pen point to x,y. x2,y2 is the end control point. The start control point is is assumed to be the same as the end control point of the previous curve.
                s	x2,y2 x,y	        smooth curveto	    Same as S, but interprets coordinates relative to current pen point.
 
                Q	x1,y1 x,y	        quadratic curveto	Draws a quadratic Bezier curve from current pen point to x,y. x1,y1 is the control point controlling how the curve bends.
                q	x1,y1 x,y	        quadratic curveto	Same as Q, but interprets coordinates relative to current pen point.
 
                T	x,y	                smooth quadratic    Draws a quadratic Bezier curve from current pen point to x,y. The control point is assumed to be the same as the last control point used.
                t	x,y	                smooth quadratic	Same as T, but interprets coordinates relative to current pen point.
 
                A	rx,ry               elliptical arc      Draws an elliptical arc from the current point to the point x,y. rx and ry are the elliptical radius in x and y direction.
                    x-axis-rotation                         The x-rotation determines how much the arc is to be rotated around the x-axis. It only seems to have an effect when rx and ry have different values. 
                    large-arc-flag,                         The large-arc-flag indicated if this is the long path or shorter path between start and end of the arc on the ellipse 
                    sweepflag                               The sweep-flag determines the direction to draw the arc in. 0 = Negative, 1 = Positive
                    x,y		
                
                a	rx,ry               elliptical arc	    Same as A, but interprets coordinates relative to current pen point.
                    x-axis-rotation 
                    large-arc-flag,
                    sweepflag 
                    x,y	
 
                Z	 	                closepath	        Closes the path by drawing a line from current point to first point.
                z	 	                closepath	        Closes the path by drawing a line from current point to first point.

         * 
         */

        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void ParseEmpty_Test()
        {
            GraphicsPath target = GraphicsPath.Parse("");
            Assert.IsNotNull(target);
            Assert.IsTrue(target.CurrentPath != null);
            Assert.IsTrue(target.HasCurrentPath);
            Assert.IsNotNull(target.Paths);
            Assert.IsTrue(target.Paths.Count == 1);
        }

        [TestMethod()]
        [TestCategory("Graphics Path")]
        public void ParseClose_Test()
        {

            GraphicsPath target = GraphicsPath.Parse("Z");
            Assert.IsNotNull(target);
            Assert.IsTrue(target.CurrentPath != null);
            Assert.IsTrue(target.HasCurrentPath);
            Assert.IsNotNull(target.Paths);
            Assert.IsTrue(target.Paths.Count == 1);
            Assert.IsTrue(target.Paths[0].Operations.Count == 1);
            Assert.IsInstanceOfType(target.Paths[0].Operations[0], typeof(PathCloseData));
        }

        [TestCategory("Graphics Path")]
        [TestMethod()]
        public void ParsePathRegEx_Text()
        {
            var pathData = @"M-84.1487,-15.8513 a22.4171,22.4171 0 1 0 0,31.7026 
            h168.2974 a22.4171,22.4171 0 1 0 0,-31.7026 M70.491,50.826c-2.232,1.152-6.913,2.304-12.817,2.304c-13.682,0-23.906-8.641-23.906-24.626
c0-15.266,10.297-25.49,25.346-25.49c5.977,0,9.865,1.296,11.521,2.16l-1.584,5.112C66.747,9.134,63.363,8.27,59.33,8.27
c-11.377,0-18.938,7.272-18.938,20.018c0,11.953,6.841,19.514,18.578,19.514c3.888,0,7.777-0.792,10.297-2.016L70.491,50.826z";
            
            var operators = @"(?=[MZLHVCSQTAmzlhvcsqta])";
            string argSeparators = @"[\s,]|(?=-)";
            
            var tokens = System.Text.RegularExpressions.Regex.Split(pathData, operators, System.Text.RegularExpressions.RegexOptions.Multiline);
            
            foreach (string match in tokens)
            {
                string op = match.Trim();
                if (string.IsNullOrEmpty(op) == false)
                {
                    char cmd = op[0];
                    op = op.Substring(1);
                    TestContext.WriteLine("Found Command: '{0}'", cmd);
                    
                    var args = System.Text.RegularExpressions.Regex.Split(op, argSeparators);
                    
                    foreach (string arg in args)
                    {
                        if (!string.IsNullOrEmpty(arg.Trim()))
                        {
                            //PDFUnit unit = PDFUnit.Parse(arg.Trim());
                            TestContext.WriteLine("     Found Command Argument: '{0}'", arg);
                        }
                    }
                }
            }
        }

        [TestCategory("Graphics Path")]
        [TestMethod()]
        public void SVGParser_Test()
        {
            var pathData = @"M-84.1487,-15.8513h168.2974 V10.053 H-2Z";
            
            PDFSVGPathDataParser parser = new PDFSVGPathDataParser(true, null);
            GraphicsPath path = new GraphicsPath();

            parser.ParseSVG(path, pathData);
            Assert.AreEqual(path.Paths.Count, 1);
            Assert.AreEqual(path.Paths[0].Operations.Count, 5);

            //M-84.1487,-15.8513
            Assert.IsInstanceOfType(path.Paths[0].Operations[0], typeof(PathMoveData));
            PathMoveData move = (PathMoveData)path.Paths[0].Operations[0];
            Assert.AreEqual(move.MoveTo.X.PointsValue, -84.1487);
            Assert.AreEqual(move.MoveTo.Y.PointsValue, -15.8513);

            //h168.2974
            Assert.IsInstanceOfType(path.Paths[0].Operations[1], typeof(PathLineData));
            PathLineData line = (PathLineData)path.Paths[0].Operations[1];
            Assert.AreEqual(line.LineTo.X.PointsValue, -84.1487 + 168.2974);
            Assert.AreEqual(line.LineTo.Y.PointsValue, -15.8513);

            //V10.053
            Assert.IsInstanceOfType(path.Paths[0].Operations[2], typeof(PathLineData));
            line = (PathLineData)path.Paths[0].Operations[2];
            Assert.AreEqual(line.LineTo.X.PointsValue, -84.1487 + 168.2974);
            Assert.AreEqual(line.LineTo.Y.PointsValue, 10.053);

            //H-2
            Assert.IsInstanceOfType(path.Paths[0].Operations[3], typeof(PathLineData));
            line = (PathLineData)path.Paths[0].Operations[3];
            Assert.AreEqual(line.LineTo.X.PointsValue, -2.0);
            Assert.AreEqual(line.LineTo.Y.PointsValue, 10.053);

            //Z close
            Assert.IsInstanceOfType(path.Paths[0].Operations[path.Paths[0].Operations.Count -1], typeof(PathCloseData));
        }

        [TestCategory("Graphics Path")]
        [TestMethod()]
        public void SVGParserFull_Test()
        {
            var pathData = @"M-84.1487,-15.8513h168.2974 V10.053 H-2a22.4171,22.4171 0 1 0 0,31.7026 
            h168.2974 A22.4171,22.4171 0 1 0 0,-31.7026 M70.491,50.826c-2.232,1.152-6.913,2.304-12.817,2.304c-13.682,0-23.906-8.641-23.906-24.626
c0-15.266,10.297-25.49,25.346-25.49c5.977,0,9.865,1.296,11.521,2.16L-1.584,5.112C66.747,9.134,63.363,8.27,59.33,8.27
c-11.377,0-18.938,7.272-18.938,20.018c0,11.953,6.841,19.514,18.578,19.514c3.888,0,7.777-0.792,10.297-2.016L70.491,50.826z";

            PDFSVGPathDataParser parser = new PDFSVGPathDataParser(true, null);
            GraphicsPath path = new GraphicsPath();

            parser.ParseSVG(path, pathData);
            Assert.AreEqual(path.Paths.Count, 1);

            //Check the operation count
            Assert.AreEqual(path.Paths[0].Operations.Count, 19);

            //M-84.1487,-15.8513
            Assert.IsInstanceOfType(path.Paths[0].Operations[0], typeof(PathMoveData), "Operation 6 Invalid");
            PathMoveData move = (PathMoveData)path.Paths[0].Operations[0];
            Assert.AreEqual(move.MoveTo.X.PointsValue, -84.1487);
            Assert.AreEqual(move.MoveTo.Y.PointsValue, -15.8513);

            //h168.2974
            Assert.IsInstanceOfType(path.Paths[0].Operations[1], typeof(PathLineData), "Operation 1 Invalid");
            PathLineData line = (PathLineData)path.Paths[0].Operations[1];
            Assert.AreEqual(line.LineTo.X.PointsValue, -84.1487 + 168.2974);
            Assert.AreEqual(line.LineTo.Y.PointsValue, -15.8513);

            //V10.053
            Assert.IsInstanceOfType(path.Paths[0].Operations[2], typeof(PathLineData), "Operation 2 Invalid");
            line = (PathLineData)path.Paths[0].Operations[2];
            Assert.AreEqual(line.LineTo.X.PointsValue, -84.1487 + 168.2974);
            Assert.AreEqual(line.LineTo.Y.PointsValue, 10.053);

            //H-2
            Assert.IsInstanceOfType(path.Paths[0].Operations[3], typeof(PathLineData), "Operation 3 Invalid");
            line = (PathLineData)path.Paths[0].Operations[3];
            Assert.AreEqual(line.LineTo.X.PointsValue, -2.0);
            Assert.AreEqual(line.LineTo.Y.PointsValue, 10.053);

            //a22.4171,22.4171 0 1 0 0,31.7026 
            Assert.IsInstanceOfType(path.Paths[0].Operations[4], typeof(PathArcData), "Operation 4 Invalid");

            //h168.2974
            Assert.IsInstanceOfType(path.Paths[0].Operations[5], typeof(PathLineData), "Operation 5 Invalid");

            //A22.4171,22.4171 0 1 0 0,-31.7026 
            Assert.IsInstanceOfType(path.Paths[0].Operations[6], typeof(PathArcData), "Operation 6 Invalid");
            PathArcData arc = (PathArcData)path.Paths[0].Operations[6];
            Assert.AreEqual(22.4171, arc.RadiusX, "Operation 6 Invalid - Radius X");
            Assert.AreEqual(22.4171, arc.RadiusY, "Operation 6 Invalid - Radius Y");
            Assert.AreEqual(0, arc.XAxisRotation, "Operation 6 Invalid - Rotation");
            Assert.AreEqual(1, (int)arc.ArcSize, "Operation 6 Invalid - Large Arc");
            Assert.AreEqual(0, (int)arc.ArcSweep, "Operation 6 Invalid - Sweep");
            Assert.AreEqual(0.0, arc.EndPoint.X, "Operation 6 Invalid - Endpoint X");
            Assert.AreEqual(-31.7026, arc.EndPoint.Y, "Operation 6 Invalid - EndPoint Y");

            //M70.491,50.826
            Assert.IsInstanceOfType(path.Paths[0].Operations[7], typeof(PathMoveData), "Operation 7 Invalid");

            //c-2.232,1.152-6.913,2.304-12.817,2.304
            Assert.IsInstanceOfType(path.Paths[0].Operations[8], typeof(PathBezierCurveData), "Operation 8 Invalid");

            //c-13.682,0-23.906-8.641-23.906-24.626
            Assert.IsInstanceOfType(path.Paths[0].Operations[9], typeof(PathBezierCurveData), "Operation 9 Invalid");

            //c0-15.266,10.297-25.49,25.346-25.49
            Assert.IsInstanceOfType(path.Paths[0].Operations[10], typeof(PathBezierCurveData), "Operation 10 Invalid");

            //c5.977,0,9.865,1.296,11.521,2.16
            Assert.IsInstanceOfType(path.Paths[0].Operations[11], typeof(PathBezierCurveData), "Operation 11 Invalid");
            
            //L-1.584,5.112
            Assert.IsInstanceOfType(path.Paths[0].Operations[12], typeof(PathLineData), "Operation 12 Invalid");
            line = (PathLineData)path.Paths[0].Operations[12];

            Assert.AreEqual(-1.584, line.LineTo.X);
            Assert.AreEqual(5.112, line.LineTo.Y);

            //C66.747,9.134,63.363,8.27,59.33,8.27
            Assert.IsInstanceOfType(path.Paths[0].Operations[13], typeof(PathBezierCurveData), "Operation 13 Invalid");
            PathBezierCurveData curve = (PathBezierCurveData)path.Paths[0].Operations[13];
            Assert.AreEqual(66.747, curve.StartHandle.X);
            Assert.AreEqual(9.134, curve.StartHandle.Y);
            Assert.AreEqual(63.363, curve.EndHandle.X);
            Assert.AreEqual(8.27, curve.EndHandle.Y);
            Assert.AreEqual(59.33, curve.EndPoint.X);
            Assert.AreEqual(8.27, curve.EndPoint.Y);

            //c-11.377,0-18.938,7.272-18.938,20.018
            Assert.IsInstanceOfType(path.Paths[0].Operations[14], typeof(PathBezierCurveData), "Operation 14 Invalid");
            
            //c0,11.953,6.841,19.514,18.578,19.514
            Assert.IsInstanceOfType(path.Paths[0].Operations[15], typeof(PathBezierCurveData), "Operation 15 Invalid");
            
            //c3.888,0,7.777-0.792,10.297-2.016
            Assert.IsInstanceOfType(path.Paths[0].Operations[16], typeof(PathBezierCurveData), "Operation 16 Invalid");
            
            //L70.491,50.826
            Assert.IsInstanceOfType(path.Paths[0].Operations[17], typeof(PathLineData), "Operation 17 Invalid");
            
            //Z close
            Assert.IsInstanceOfType(path.Paths[0].Operations[18], typeof(PathCloseData), "Operation 18 Invalid");
        }

    }
}
