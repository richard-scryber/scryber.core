using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Svg.Components;
using System.IO;
using System.Linq;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.Svg;
using TransformOperation = Scryber.Drawing.TransformOperation;


namespace Scryber.Core.UnitTests.Svg
{

    [TestClass]
    public class SVGTransforms_Tests
    {
        
        #region public TestContext TestContext

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

        [TestMethod]
        public void SVGTransformParsing()
        {
            //rotate examples
            string toParse = "rotate(0.5)";
            
            var parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            var root = parsed.Root;
            var rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(Math.Round(0.5 * Math.PI/180.0, 3), Math.Round(rotate.AngleRadians, 3));
            
            toParse = "rotate(-0.5turn) ";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(-3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "rotate(180deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "scale(1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.0, Math.Round(scale.XScaleValue));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue));
            
            toParse = "scale(0.7)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.7, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(1.3, 0.4)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.4, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(-0.5,1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(0.7)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            
            toParse = "scaleX(1.3)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(-0.5)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "skew(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skew(15, 15)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            const double Deg2Rad = Math.PI / 180.0;

            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));

            toParse = "skew(-6, 18)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(Math.Round(-6 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(18 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            
           
            
            toParse = "skewX(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewX(-6)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(-6 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);
            
            toParse = "skewX(35)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);

            toParse = "skewX(234)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(Math.Round(234 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(-6)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(-6 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);
            
            toParse = "skewY(35)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);

            toParse = "skewY(234)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(Math.Round(234 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            
            toParse = "translate(200)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(200), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);

            toParse = "translate(50)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(50), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);
            
            toParse = "translate(100,200)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(100), translate.XOffset);
            Assert.AreEqual(Unit.Pt(200), translate.YOffset);
            
            toParse = "translate(100,50)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(100), translate.XOffset);
            Assert.AreEqual(Unit.Pt(50), translate.YOffset);
            
            toParse = "translate(-30, 210.5)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(-30), translate.XOffset);
            Assert.AreEqual(Unit.Pt(210.5), translate.YOffset);
            
            toParse = "translate(30%, -50%)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(30), translate.XOffset);
            Assert.AreEqual(Unit.Percent(-50), translate.YOffset);
            
            toParse = "translateX(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(42)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(42), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(-2.1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(-2.1), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(3)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(3), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(42)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(42.0), translate.YOffset);
            
            toParse = "translateY(-2.1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(-2.1), translate.YOffset);
            
            toParse = "translateY(3)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(3), translate.YOffset);
            
            toParse = "matrix(1, 0, 0, 1, 0, 0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(1.0, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.0, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(0.0, matrix.MatrixValues[4]);
            Assert.AreEqual(0.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.4, 0, 0.5, 1.2, 60, 10)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.4, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.5, matrix.MatrixValues[2]);
            Assert.AreEqual(1.2, matrix.MatrixValues[3]);
            Assert.AreEqual(60.0, matrix.MatrixValues[4]);
            Assert.AreEqual(10.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.1, 1, -0.3, 1, 20, 20.2)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.1, matrix.MatrixValues[0]);
            Assert.AreEqual(1.0, matrix.MatrixValues[1]);
            Assert.AreEqual(-0.3, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(20.0, matrix.MatrixValues[4]);
            Assert.AreEqual(20.2, matrix.MatrixValues[5]);
        }

        [TestMethod]
        public void SVGTransformParsingSpaces()
        {
            //rotate examples
            string toParse = "rotate(3.142rad)";
            
            var parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            var root = parsed.Root;
            var rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(3.142, rotate.AngleRadians);
            
            toParse = "rotate(-0.5turn) ";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(-3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "rotate(180deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "scale(1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.0, Math.Round(scale.XScaleValue));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue));
            
            toParse = "scale(0.7)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.7, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(1.3 0.4)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.4, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(-0.5 1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(0.7)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            
            toParse = "scaleX(1.3)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(-0.5)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "skew(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skew(15deg 15deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            const double Deg2Rad = Math.PI / 180.0;

            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));

            toParse = "skew(-0.06turn 18deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(Math.Round((-0.06 * 360) * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(18 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skew(.312rad)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0.312, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.312, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewX(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewX(-0.6turn)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round((-0.6 * 360) * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);
            
            toParse = "skewX(35deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);

            toParse = "skewX(.234rad)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0.234, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(-0.6turn)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round((-0.6 * 360) * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);
            
            toParse = "skewY(35deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);

            toParse = "skewY(.234rad)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0.234, Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            
            toParse = "translate(200px)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Px(200), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);

            toParse = "translate(50%)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(50), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);
            
            toParse = "translate(100pt 200pt)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(100), translate.XOffset);
            Assert.AreEqual(Unit.Pt(200), translate.YOffset);
            
            toParse = "translate(100px 50%)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Px(100), translate.XOffset);
            Assert.AreEqual(Unit.Percent(50), translate.YOffset);
            
            toParse = "translate(-30%  210.5px)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(-30), translate.XOffset);
            Assert.AreEqual(Unit.Px(210.5), translate.YOffset);
            
            toParse = "translate(30%    -50%)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(30), translate.XOffset);
            Assert.AreEqual(Unit.Percent(-50), translate.YOffset);
            
            toParse = "translateX(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(42px)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Px(42.0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(-2.1rem)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.RootEm(-2.1), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(3ch)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Ch(3), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(42px)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Px(42.0), translate.YOffset);
            
            toParse = "translateY(-2.1rem)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.RootEm(-2.1), translate.YOffset);
            
            toParse = "translateY(3ch)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Ch(3), translate.YOffset);
            
            toParse = "matrix(1 0 0 1 0 0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(1.0, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.0, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(0.0, matrix.MatrixValues[4]);
            Assert.AreEqual(0.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.4  0  0.5  1.2 60 10)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.4, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.5, matrix.MatrixValues[2]);
            Assert.AreEqual(1.2, matrix.MatrixValues[3]);
            Assert.AreEqual(60.0, matrix.MatrixValues[4]);
            Assert.AreEqual(10.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.1, 1 -0.3, 1 20 20.2)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.1, matrix.MatrixValues[0]);
            Assert.AreEqual(1.0, matrix.MatrixValues[1]);
            Assert.AreEqual(-0.3, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(20.0, matrix.MatrixValues[4]);
            Assert.AreEqual(20.2, matrix.MatrixValues[5]);
        }
        [TestMethod]
        public void SVGTransformParsingChained()
        {
            string toParse = " translate(100pt, 30pt) rotate(20deg) scale(2) translate(-100pt, 30pt)";
            
            var parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            var curr = parsed.Root;
            
            Assert.AreEqual(curr.OperationType, MatrixTransformTypes.Translation);
            
            curr = curr.NextOp;
            Assert.IsNotNull(curr);
            Assert.AreEqual(curr.OperationType, MatrixTransformTypes.Rotate);
            
            curr = curr.NextOp;
            Assert.IsNotNull(curr);
            Assert.AreEqual(curr.OperationType, MatrixTransformTypes.Scaling);
            
            curr = curr.NextOp;
            Assert.IsNotNull(curr);
            Assert.AreEqual(curr.OperationType, MatrixTransformTypes.Translation);
            
            Assert.IsNull(curr.NextOp);
        }

        [TestMethod]
        public void SVGTransformOriginParsing()
        {
            
            
            Unit zeroPcnt = Unit.Percent(0);
            Unit fiftyPcnt = Unit.Percent(50.0);
            Unit hundredPcnt = Unit.Percent(100.0);
            
            string toParse = "2px";
            
            var parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(Unit.Px(2.0), parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);

            toParse = "center";

            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(fiftyPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);

            toParse = "center top";
            
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(fiftyPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(zeroPcnt, parsed.VerticalOrigin);

            toParse = "left top";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(zeroPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(zeroPcnt, parsed.VerticalOrigin);
            
            toParse = "left center";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(zeroPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);
            
            toParse = "center left";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(zeroPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);

            toParse = "100 left";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(zeroPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(100, parsed.VerticalOrigin);

            
            toParse = "100 bottom";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(100, parsed.HorizontalOrigin);
            Assert.AreEqual(hundredPcnt, parsed.VerticalOrigin);
            
            toParse = "100 center";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(100, parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);
            
            toParse = "100 20px";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(100, parsed.HorizontalOrigin);
            Assert.AreEqual(Unit.Px(20), parsed.VerticalOrigin);
        }
        
        

        
        [TestMethod]
        public void SVGTransformFlattenRelative()
        {
            string transformOps = "translate(20%, 10%)";
            var rectStyle = new StyleDefn(".rect");
            rectStyle.SetValue(StyleKeys.TransformOperationKey, SVGTransformOperationSet.Parse(transformOps));

            
            var doc = new HTMLDocument();
            doc.Styles.Add(rectStyle);
            
            var body = new HTMLBody()
            {
                Margins = Thickness.Empty(),
                Padding = Thickness.Empty()
            };
            
            doc.Body = body;

            var svg = new SVGCanvas()
            {
                Width = 300, 
                Height = 400, 
                BorderColor = StandardColors.Red, 
                BorderWidth = 1
            };
            body.Contents.Add(svg);

            SVGPath path = new SVGPath()
            {
                PathData = Scryber.Drawing.GraphicsPath.Parse("M 10,30 A 20,20 0,0,1 50,30 A 20,20 0,0,1 90,30 Q 90,60 50,90 Q 10,60 10,30 z"),
                StyleClass = "rect"
            };
            svg.Contents.Add(path);
            
            using (var stream = DocStreams.GetOutputStream("SVGTransformTranslateRelative.pdf"))
            {
                body.Style.OverlayGrid.ShowGrid = true;
                body.Style.OverlayGrid.GridColor = StandardColors.Aqua;
                body.Style.OverlayGrid.GridMajorCount = 5;
                body.Style.OverlayGrid.GridSpacing = 10;
                
                doc.SaveAsPDF(stream);
            }

            var arrange = path.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var full = arrange.FullStyle;
            Assert.IsNotNull(full);
            Assert.IsTrue(full.IsValueDefined(StyleKeys.TransformOperationKey));
            var set = full.GetValue(StyleKeys.TransformOperationKey, null);
            Assert.IsNotNull(set);
            Assert.IsNotNull(set.Root);
            var transform = set.Root as TransformTranslateOperation;
            Assert.IsNotNull(transform);
            var bounds = path.PathData.Bounds;
            Assert.AreEqual(Unit.Pt(60), transform.XOffset); //20% of 300
            Assert.AreEqual(Unit.Pt(40), transform.YOffset); //10% of 300
            Assert.Inconclusive("Need to update the arrangement bounds based on the transfomation");
            
            Assert.AreEqual(bounds.X + 60, arrange.RenderBounds.X); //bounds.x + XOffset
            Assert.AreEqual(bounds.Y + 40, arrange.RenderBounds.Y); //bounds.y + YOffset
            Assert.AreEqual(bounds.Width, arrange.RenderBounds.Width); //unchanged
            Assert.AreEqual(bounds.Height, arrange.RenderBounds.Height);
            
        }
        

        
        [TestMethod]
        public void SVGTransformOperationRotateDegrees()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_RotateDegrees.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationRotateDegrees.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);
                
                var poly2 = doc.FindAComponentById("Poly2");
                Assert.IsNotNull(poly2);
                
                var poly3 = doc.FindAComponentById("Poly3");
                Assert.IsNotNull(poly3);
                
                var poly4 = doc.FindAComponentById("Poly4");
                Assert.IsNotNull(poly4);
                
                var poly5 = doc.FindAComponentById("Poly5");
                Assert.IsNotNull(poly5);
                
                var poly6 = doc.FindAComponentById("Poly6");
                Assert.IsNotNull(poly6);
                
                var poly7 = doc.FindAComponentById("Poly7");
                Assert.IsNotNull(poly7);

                var use_1 = doc.FindAComponentById("Poly-1") as SVGUse;
                Assert.IsNotNull(use_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) *15.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                //TODO: check the render bounds
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 30.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly4.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 45.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly5.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 60.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly6.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 75.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly7.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 90.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;

                arrange = use_1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use_1.Contents.Count);
                var poly_1 = use_1.Contents[0];
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * -15.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }

        [TestMethod]
        public void SVGTransformOperationRotateRadians()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_RotateRadians.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationRotateRadians.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);
                
                var poly2 = doc.FindAComponentById("Poly2");
                Assert.IsNotNull(poly2);
                
                var poly3 = doc.FindAComponentById("Poly3");
                Assert.IsNotNull(poly3);
                
                var poly4 = doc.FindAComponentById("Poly4");
                Assert.IsNotNull(poly4);
                
                var poly5 = doc.FindAComponentById("Poly5");
                Assert.IsNotNull(poly5);
                
                var poly6 = doc.FindAComponentById("Poly6");
                Assert.IsNotNull(poly6);
                
                var poly7 = doc.FindAComponentById("Poly7");
                Assert.IsNotNull(poly7);

                var use_1 = doc.FindAComponentById("Poly-1") as SVGUse;
                Assert.IsNotNull(use_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(0.26, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                //TODO: check the render bounds
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(0.52, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly4.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(0.78, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly5.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(1.04, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly6.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(1.40, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly7.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(1.66, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;

                arrange = use_1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use_1.Contents.Count);
                var poly_1 = use_1.Contents[0];
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(-0.16, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        [TestMethod]
        public void SVGTransformOperationTranslate()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_Translate.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationTranslate.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var translate = transform.Root as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(50, translate.XOffset);
                Assert.AreEqual(0, translate.YOffset);
                Assert.IsNull(translate.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                translate = transform.Root as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(50, translate.XOffset);
                Assert.AreEqual(50, translate.YOffset);
                Assert.IsNull(translate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                translate = transform.Root as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(0, translate.XOffset);
                Assert.AreEqual(50, translate.YOffset);
                Assert.IsNull(translate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                translate = transform.Root as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(-50, translate.XOffset);
                Assert.AreEqual(0, translate.YOffset);
                Assert.IsNull(translate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        [TestMethod]
        public void SVGTransformOperationScaleOne()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_ScaleOne.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationScaleOne.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.5, scale.XScaleValue);
                Assert.AreEqual(1.5, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(2.0, scale.XScaleValue);
                Assert.AreEqual(2.0, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(3.0, scale.XScaleValue);
                Assert.AreEqual(3.0, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(0.5, scale.XScaleValue);
                Assert.AreEqual(0.5, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        
        [TestMethod]
        public void SVGTransformOperationScaleBoth()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_ScaleBoth.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationScaleBoth.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.5, scale.XScaleValue);
                Assert.AreEqual(0.75, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(2.0, scale.XScaleValue);
                Assert.AreEqual(1.0, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(3.0, scale.XScaleValue);
                Assert.AreEqual(1.5, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(0.85, scale.XScaleValue);
                Assert.AreEqual(0.65, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        [TestMethod]
        public void SVGTransformOperationSkewOneX()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_SkewOneX.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationSkewOneX.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;
                const double Deg2Rad = Math.PI / 180;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(20 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(0, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(40 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(0, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(60 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(0, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(-20 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(0, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        [TestMethod]
        public void SVGTransformOperationSkewOneY()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_SkewOneY.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationSkewOneY.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;
                const double Deg2Rad = Math.PI / 180;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(20 * Deg2Rad, skew.YAngleRadians);
                Assert.AreEqual(0, skew.XAngleRadians);
                Assert.IsNull(skew.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(40 * Deg2Rad, skew.YAngleRadians);
                Assert.AreEqual(0, skew.XAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(60 * Deg2Rad, skew.YAngleRadians);
                Assert.AreEqual(0, skew.XAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(-20 * Deg2Rad, skew.YAngleRadians);
                Assert.AreEqual(0, skew.XAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        
        [TestMethod]
        public void SVGTransformOperationSkewBoth()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_SkewBoth.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationSkewBoth.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;
                const double Deg2Rad = Math.PI / 180;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(20 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(10 * Deg2Rad, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(40 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(20 * Deg2Rad, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(60 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(30 * Deg2Rad, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(-20 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(-10 * Deg2Rad, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
            }
        }
        
        
        [TestMethod]
        public void SVGTransformOperationMatrix()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_Matrix.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationMatrix.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 20, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var matrix = transform.Root as Scryber.Drawing.TransformMatrixOperation;
                Assert.IsNotNull(matrix);
                var comps = matrix.MatrixValues;
                Assert.AreEqual(6, comps.Length);
                Assert.AreEqual(1.2, comps[0]);
                Assert.AreEqual(1, comps[1]);
                Assert.AreEqual(-1, comps[2]);
                Assert.AreEqual(1.2, comps[3]);
                Assert.AreEqual(30.0, comps[4]);
                Assert.AreEqual(40.0, comps[5]);
                Assert.IsNull(matrix.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                matrix = transform.Root as Scryber.Drawing.TransformMatrixOperation;
                Assert.IsNotNull(matrix);
                comps = matrix.MatrixValues;
                Assert.AreEqual(6, comps.Length);
                Assert.AreEqual(1.4, comps[0]);
                Assert.AreEqual(1, comps[1]);
                Assert.AreEqual(-1, comps[2]);
                Assert.AreEqual(1.4, comps[3]);
                Assert.AreEqual(40.0, comps[4]);
                Assert.AreEqual(50.0, comps[5]);
                Assert.IsNull(matrix.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                matrix = transform.Root as Scryber.Drawing.TransformMatrixOperation;
                Assert.IsNotNull(matrix);
                comps = matrix.MatrixValues;
                Assert.AreEqual(6, comps.Length);
                Assert.AreEqual(1.6, comps[0]);
                Assert.AreEqual(1, comps[1]);
                Assert.AreEqual(-1, comps[2]);
                Assert.AreEqual(1.6, comps[3]);
                Assert.AreEqual(50.0, comps[4]);
                Assert.AreEqual(60.0, comps[5]);
                Assert.IsNull(matrix.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                matrix = transform.Root as Scryber.Drawing.TransformMatrixOperation;
                Assert.IsNotNull(matrix);
                comps = matrix.MatrixValues;
                Assert.AreEqual(6, comps.Length);
                Assert.AreEqual(0.8, comps[0]);
                Assert.AreEqual(1, comps[1]);
                Assert.AreEqual(-1, comps[2]);
                Assert.AreEqual(0.8, comps[3]);
                Assert.AreEqual(10.0, comps[4]);
                Assert.AreEqual(20.0, comps[5]);
                Assert.IsNull(matrix.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }

        
        [TestMethod]
        public void SVGTransformOperationMultipleChained()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_MultipleChained.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationMultipleChained.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);
                
                var poly2 = doc.FindAComponentById("Poly2");
                Assert.IsNotNull(poly2);
                
                var poly3 = doc.FindAComponentById("Poly3");
                Assert.IsNotNull(poly3);
                
                var poly4 = doc.FindAComponentById("Poly4");
                Assert.IsNotNull(poly4);
                
                var poly5 = doc.FindAComponentById("Poly5");
                Assert.IsNotNull(poly5);
                
                var poly6 = doc.FindAComponentById("Poly6");
                Assert.IsNotNull(poly6);
                
                var poly7 = doc.FindAComponentById("Poly7");
                Assert.IsNotNull(poly7);

                var use_1 = doc.FindAComponentById("Poly-1") as SVGUse;
                Assert.IsNotNull(use_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) *10.0, rotate.AngleRadians);
                var translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(10, translate.XOffset);
                Assert.AreEqual(20, translate.YOffset);
                var scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.1, scale.XScaleValue);
                Assert.AreEqual(1.1, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                //TODO: check the render bounds
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 20.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(20, translate.XOffset);
                Assert.AreEqual(30, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.2, scale.XScaleValue);
                Assert.AreEqual(1.2, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly4.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 30.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(30, translate.XOffset);
                Assert.AreEqual(40, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.3, scale.XScaleValue);
                Assert.AreEqual(1.3, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly5.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 40.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(40, translate.XOffset);
                Assert.AreEqual(50, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.4, scale.XScaleValue);
                Assert.AreEqual(1.4, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly6.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 50.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(50, translate.XOffset);
                Assert.AreEqual(60, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.5, scale.XScaleValue);
                Assert.AreEqual(1.5, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly7.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 60.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(60, translate.XOffset);
                Assert.AreEqual(70, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.6, scale.XScaleValue);
                Assert.AreEqual(1.6, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;

                arrange = use_1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use_1.Contents.Count);
                var poly_1 = use_1.Contents[0];
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * -10.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(-10, translate.XOffset);
                Assert.AreEqual(-20, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(0.9, scale.XScaleValue);
                Assert.AreEqual(0.9, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }


        private ComponentArrangement AssertGetArrangement(Document doc, string compId, Type compType)
        {
            var comp = doc.FindAComponentById(compId);
            Assert.IsNotNull(comp);
            Assert.IsInstanceOfType(comp, compType);
            var arrange = comp.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            return arrange;
        }

        private TransformOperationSet AssertGetTransform(ComponentArrangement arrangement)
        {
            var style = arrangement.FullStyle;
            Assert.IsNotNull(style);
            var set = style.GetValue(StyleKeys.TransformOperationKey, null);
            Assert.IsNotNull(set);
            return set;
        }
        
        private void AssertTransformTranslate(TransformOperation op, Unit x, Unit y, bool hasNext)
        {
            var tranlate = op as TransformTranslateOperation;
            Assert.IsNotNull(tranlate);
            Assert.AreEqual(x, tranlate.XOffset);
            Assert.AreEqual(y, tranlate.YOffset);
            if(hasNext)
                Assert.IsNotNull(op.NextOp);
            else
                Assert.IsNull(op.NextOp);
        }
        
        private void AssertTransformSkew(TransformOperation op, double xdeg, double ydeg, bool hasNext)
        {
            var x = (Math.PI / 180.0) * xdeg;
            var y = (Math.PI / 180.0) * ydeg;
            
            var skew = op as TransformSkewOperation;
            Assert.IsNotNull(skew);
            Assert.AreEqual(x, skew.XAngleRadians);
            Assert.AreEqual(y, skew.YAngleRadians);
            if(hasNext)
                Assert.IsNotNull(op.NextOp);
            else
                Assert.IsNull(op.NextOp);
        }
        
        private void AssertTransformScale(TransformOperation op, double xscale, double yscale, bool hasNext)
        {
            
            var scale = op as TransformScaleOperation;
            Assert.IsNotNull(scale);
            Assert.AreEqual(xscale, scale.XScaleValue);
            Assert.AreEqual(yscale, scale.YScaleValue);
            
            if(hasNext)
                Assert.IsNotNull(op.NextOp);
            else
                Assert.IsNull(op.NextOp);
        }
        
        

        [TestMethod]
        public void SVGTransformOperationComponents()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_TransformComponents.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;


                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationTransformComponents.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.AppendTraceLog = false;
                    doc.SaveAsPDF(stream);
                }
                
                //Anchor link

                var arrange = AssertGetArrangement(doc, "link", typeof(SVGAnchor));
                var set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 0, 70, true);
                AssertTransformSkew(set.Root.NextOp, 30, 0, false);
                //Circle
                
                arrange = AssertGetArrangement(doc, "circ", typeof(SVGCircle));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 75, 90, true);
                AssertTransformSkew(set.Root.NextOp, 30, 0, false);

                //Ellipse
                
                arrange = AssertGetArrangement(doc, "elli", typeof(SVGEllipse));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 120, 90, true);
                AssertTransformSkew(set.Root.NextOp, 0, 30, false);
                
                //Group
                
                arrange = AssertGetArrangement(doc, "grp", typeof(SVGGroup));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 130, 60, true);
                AssertTransformSkew(set.Root.NextOp, 40, 0, false);

                //Rect
                
                arrange = AssertGetArrangement(doc, "rect", typeof(SVGRect));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 10, 25, true);
                AssertTransformScale(set.Root.NextOp, 4, 1, false);
                
                //Line
                
                arrange = AssertGetArrangement(doc, "ln", typeof(SVGLine));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 180, 60, true);
                AssertTransformSkew(set.Root.NextOp, 40, 0, false);
                
                //Polyline
                
                arrange = AssertGetArrangement(doc, "poly", typeof(SVGPolyLine));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 195, 60, true);
                AssertTransformSkew(set.Root.NextOp, 40, 0, false);

                //Polygon
                
                arrange = AssertGetArrangement(doc, "polyG", typeof(SVGPolygon));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 20, 60, true);
                AssertTransformSkew(set.Root.NextOp, -50, 0, false);
                
                //Path
                
                arrange = AssertGetArrangement(doc, "pth", typeof(SVGPath));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 225, 60, true);
                AssertTransformScale(set.Root.NextOp, 0.2, 0.2, true);
                AssertTransformSkew(set.Root.NextOp.NextOp, 30, 0, false);
                
                //Image
                
                arrange = AssertGetArrangement(doc, "img", typeof(SVGImage));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 15, 125, true);
                AssertTransformScale(set.Root.NextOp, 1.3, 0.1, true);
                AssertTransformSkew(set.Root.NextOp.NextOp, -10, 0, false);
                
                
                //Use - arrangement is null put style is passed down to the referenced component
                var use = doc.FindAComponentById("use") as SVGUse;
                Assert.IsNotNull(use);
                arrange = use.GetFirstArrangement();
                Assert.IsNull(arrange);
                var rect = use.Contents[0] as SVGRect;
                Assert.IsNotNull(rect);
                arrange = rect.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 120, 150, true);
                AssertTransformScale(set.Root.NextOp, 2, 0.2, false);
                
                //text
                
                arrange = AssertGetArrangement(doc, "txt", typeof(SVGText));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 10, 150, true);
                AssertTransformSkew(set.Root.NextOp, 25, 0, true);
                AssertTransformScale(set.Root.NextOp.NextOp, 1, -1, false);


            }
            
            
        }

        private void AssertOriginAreEqual(SVGPolyLine poly, Unit h, Unit v)
        {
            Assert.IsNotNull(poly);
            var arrange = poly.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var style = arrange.FullStyle;
            Assert.IsNotNull(style);
            var origin = style.GetValue(StyleKeys.TransformOriginKey, null);
            Assert.IsNotNull(origin);
            Assert.AreEqual(h, origin.HorizontalOrigin);
            Assert.AreEqual(v, origin.VerticalOrigin);
            
        }

        [TestMethod]
        public void SVGTransformRotateOrigins()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_RotateOrigins.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;


                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationRotateOrigins.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
                
                //Left Top

                var svg = doc.FindAComponentById("Canvas11");
                var poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                var poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                var poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 0, 0);
                AssertOriginAreEqual(poly2, 0, 0);
                AssertOriginAreEqual(poly3, 0, 0);

                // Center Top
                
                svg = doc.FindAComponentById("Canvas12");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 150, 0);
                AssertOriginAreEqual(poly2, 150, 0);
                AssertOriginAreEqual(poly3, 150, 0);
                
                // Right Top
                
                svg = doc.FindAComponentById("Canvas13");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 300, 0);
                AssertOriginAreEqual(poly2, 300, 0);
                AssertOriginAreEqual(poly3, 300, 0);
                
                //Left Center

                svg = doc.FindAComponentById("Canvas21");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 0, 150);
                AssertOriginAreEqual(poly2, 0, 150);
                AssertOriginAreEqual(poly3, 0, 150);

                // Center Center
                
                svg = doc.FindAComponentById("Canvas22");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 150, 150);
                AssertOriginAreEqual(poly2, 150, 150);
                AssertOriginAreEqual(poly3, 150, 150);
                
                // Right Center
                
                svg = doc.FindAComponentById("Canvas23");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 300, 150);
                AssertOriginAreEqual(poly2, 300, 150);
                AssertOriginAreEqual(poly3, 300, 150);
                
                //Left Bottom

                svg = doc.FindAComponentById("Canvas31");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 0, 300);
                AssertOriginAreEqual(poly2, 0, 300);
                AssertOriginAreEqual(poly3, 0, 300);

                // Center Bottom
                
                svg = doc.FindAComponentById("Canvas32");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 150, 300);
                AssertOriginAreEqual(poly2, 150, 300);
                AssertOriginAreEqual(poly3, 150, 300);
                
                // Right Bottom
                
                svg = doc.FindAComponentById("Canvas33");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 300, 300);
                AssertOriginAreEqual(poly2, 300, 300);
                AssertOriginAreEqual(poly3, 300, 300);
            }
            
            
        }
        
    }

}