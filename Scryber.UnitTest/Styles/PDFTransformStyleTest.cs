using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Styles
{
    //TODO: Transform Styles
    
    
    /// <summary>
    ///This is a test class for PDFTransformStyleTest and is intended
    ///to contain all PDFTransformStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFTransformStyleTest
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



        /// <summary>
        ///A test for PDFTransformStyle Constructor
        ///</summary>
        [TestMethod()]
        public void TransformStyleConstructorTest()
        {
            TransformStyle target = new TransformStyle();
            Assert.IsNull(target.Operations);
            Assert.IsTrue(TransformOperation.IsNotSet(target.Rotate));
            Assert.IsTrue(TransformOperation.IsNotSet(target.ScaleX));
            Assert.IsTrue(TransformOperation.IsNotSet(target.ScaleY));
            Assert.IsTrue(TransformOperation.IsNotSet(target.SkewX));
            Assert.IsTrue(TransformOperation.IsNotSet(target.SkewY));
            Assert.IsTrue(TransformOperation.IsNotSet(target.TranslateX));
            Assert.IsTrue(TransformOperation.IsNotSet(target.TranslateY));

            Assert.IsFalse(TransformOperation.IsSet(target.Rotate));
            Assert.IsFalse(TransformOperation.IsSet(target.ScaleX));
            Assert.IsFalse(TransformOperation.IsSet(target.ScaleY));
            Assert.IsFalse(TransformOperation.IsSet(target.SkewX));
            Assert.IsFalse(TransformOperation.IsSet(target.SkewY));
            Assert.IsFalse(TransformOperation.IsSet(target.TranslateX));
            Assert.IsFalse(TransformOperation.IsSet(target.TranslateY));
        }


        /// <summary>
        ///A test for Operations
        ///</summary>
        [TestMethod()]
        public void OperationsTest()
        {
            TransformStyle target = new TransformStyle();
            var expected = new TransformOperation(TransformType.Translate, 20.1F, 30.3F);
            
            target.Operations = expected;
            var actual = target.Operations;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Type, TransformType.Translate);
            Assert.AreEqual(actual.Value1, 20.1F);
            Assert.AreEqual(actual.Value2, 30.3F);
        }

        /// <summary>
        ///A test for Operations
        ///</summary>
        [TestMethod()]
        public void OperationsNextTest()
        {
            TransformStyle target = new TransformStyle();
            var expected = new TransformOperation(TransformType.Translate, 20.1F, 30.3F);
            var next = new TransformOperation(TransformType.Scale, 2.0F, 1.0F);
            expected.Next = next;

            target.Operations = expected;
            var actual = target.Operations;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Type, TransformType.Translate);
            Assert.AreEqual(actual.Value1, 20.1F);
            Assert.AreEqual(actual.Value2, 30.3F);

            actual = actual.Next;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Type, TransformType.Scale);
            Assert.AreEqual(actual.Value1, 2.0F);
            Assert.AreEqual(actual.Value2, 1.0F);
        }

        /// <summary>
        ///A test for Operations
        ///</summary>
        [TestMethod()]
        public void OperationsTryGetTest()
        {
            TransformStyle target = new TransformStyle();
            var expected = new TransformOperation(TransformType.Translate, 20.1F, 30.3F);
            var next = new TransformOperation(TransformType.Scale, 2.0F, 1.0F);
            expected.Next = next;

            target.Operations = expected;
            TransformOperation actual;

            Assert.IsTrue(target.Operations.TryGetType(TransformType.Translate, out actual));


            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Type, TransformType.Translate);
            Assert.AreEqual(actual.Value1, 20.1F);
            Assert.AreEqual(actual.Value2, 30.3F);
            
            
            Assert.IsTrue(target.Operations.TryGetType(TransformType.Scale, out actual));

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Type, TransformType.Scale);
            Assert.AreEqual(actual.Value1, 2.0F);
            Assert.AreEqual(actual.Value2, 1.0F);

            Assert.IsFalse(target.Operations.TryGetType(TransformType.Rotate, out actual));
            Assert.IsNull(actual);
        }


        /// <summary>
        ///A test for Operations
        ///</summary>
        [TestMethod()]
        public void OperationsRemoveTest()
        {
            TransformStyle target = new TransformStyle();
            var expected = new TransformOperation(TransformType.Translate, 20.1F, 30.3F);
            var next = new TransformOperation(TransformType.Scale, 2.0F, 1.0F);
            expected.Next = next;

            target.Operations = expected;
            TransformOperation actual = TransformOperation.Remove(TransformType.Translate, expected);

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Type, TransformType.Scale);
            Assert.AreEqual(actual.Value1, 2.0F);
            Assert.AreEqual(actual.Value2, 1.0F);

            Assert.IsFalse(actual.TryGetType(TransformType.Translate, out actual));
            Assert.IsNull(actual);
        }




        /// <summary>
        ///A test for OffsetH
        ///</summary>
        [TestMethod()]
        public void TranslateXTest()
        {
            TransformStyle target = new TransformStyle(); 
            float expected = 10.1F;
            float actual;
            target.TranslateX = expected;
            actual = target.TranslateX;
            Assert.AreEqual(expected, actual);

            //Check that the operations are stored correctly
            Assert.IsNotNull(target.Operations);
            Assert.AreEqual(TransformType.Translate, target.Operations.Type);
            Assert.AreEqual(10.1F, target.Operations.Value1);
            Assert.IsTrue(TransformOperation.IsNotSet(target.Operations.Value2));
            Assert.IsNull(target.Operations.Next);
        }

        /// <summary>
        ///A test for OffsetV
        ///</summary>
        [TestMethod()]
        public void TranslateYTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 20.1F;
            float actual;
            target.TranslateY = expected;
            actual = target.TranslateY;
            Assert.AreEqual(expected, actual);

            //Check that the operations are stored correctly
            Assert.IsNotNull(target.Operations);
            Assert.AreEqual(TransformType.Translate, target.Operations.Type);
            Assert.IsTrue(TransformOperation.IsNotSet(target.Operations.Value1));
            Assert.AreEqual(20.1F, target.Operations.Value2);
            Assert.IsNull(target.Operations.Next);
        }

        /// <summary>
        ///A test for RemoveOffsetH
        ///</summary>
        [TestMethod()]
        public void RemoveTranslateXTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 20.1F;
            float actual;
            target.TranslateX = expected;
            actual = target.TranslateX;
            Assert.AreEqual(expected, actual);

            target.RemoveTranslateX();
            Assert.IsTrue(TransformOperation.IsNotSet(target.TranslateX));
            Assert.IsNull(target.Operations);

        }

        /// <summary>
        ///A test for RemoveOffsetV
        ///</summary>
        [TestMethod()]
        public void RemoveTranslateYTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 10.1F;
            float actual;
            target.TranslateY = expected;
            actual = target.TranslateY;
            Assert.AreEqual(expected, actual);

            target.RemoveTranslateY();
            Assert.IsTrue(TransformOperation.IsNotSet(target.TranslateY));
            Assert.IsNull(target.Operations);
        }


        /// <summary>
        ///A test for Rotate
        ///</summary>
        [TestMethod()]
        public void RotateTest()
        {
            TransformStyle target = new TransformStyle(); 
            float expected = 20.2F;
            float actual;
            target.Rotate = expected;
            actual = target.Rotate;
            Assert.AreEqual(expected, actual);


            //Check that the operations are stored correctly
            Assert.IsNotNull(target.Operations);
            Assert.AreEqual(TransformType.Rotate, target.Operations.Type);
            Assert.AreEqual(20.2F, target.Operations.Value1);
            Assert.IsTrue(TransformOperation.IsNotSet(target.Operations.Value2));
            Assert.IsNull(target.Operations.Next);
        }

        /// <summary>
        ///A test for RemoveRotate
        ///</summary>
        [TestMethod()]
        public void RemoveRotateTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 20.2F;
            float actual;
            target.Rotate = expected;
            actual = target.Rotate;
            Assert.AreEqual(expected, actual);

            target.RemoveRotate();

            Assert.IsTrue(TransformOperation.IsNotSet(target.Rotate));
            Assert.IsNull(target.Operations);
        }

        /// <summary>
        ///A test for ScaleX
        ///</summary>
        [TestMethod()]
        public void ScaleXTest()
        {

            TransformStyle target = new TransformStyle();
            float expected = 25.1F;
            float actual;
            target.ScaleX = expected;
            actual = target.ScaleX;
            Assert.AreEqual(expected, actual);

            //Check that the operations are stored correctly
            Assert.IsNotNull(target.Operations);
            Assert.AreEqual(TransformType.Scale, target.Operations.Type);
            Assert.AreEqual(25.1F, target.Operations.Value1);
            Assert.IsTrue(TransformOperation.IsNotSet(target.Operations.Value2));
            Assert.IsNull(target.Operations.Next);
        }

        /// <summary>
        ///A test for ScaleY
        ///</summary>
        [TestMethod()]
        public void ScaleYTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 30.1F;
            float actual;
            target.ScaleY = expected;
            actual = target.ScaleY;
            Assert.AreEqual(expected, actual);

            //Check that the operations are stored correctly
            Assert.IsNotNull(target.Operations);
            Assert.AreEqual(TransformType.Scale, target.Operations.Type);
            Assert.IsTrue(TransformOperation.IsNotSet(target.Operations.Value1));
            Assert.AreEqual(30.1F, target.Operations.Value2);
            Assert.IsNull(target.Operations.Next);
        }

        /// <summary>
        ///A test for RemoveScaleX
        ///</summary>
        [TestMethod()]
        public void RemoveScaleXTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 20.2F;
            float actual;
            target.ScaleX = expected;
            actual = target.ScaleX;
            Assert.AreEqual(expected, actual);

            target.RemoveScaleX();

            Assert.IsTrue(TransformOperation.IsNotSet(target.ScaleX));
            Assert.IsNull(target.Operations);
        }

        /// <summary>
        ///A test for RemoveScaleY
        ///</summary>
        [TestMethod()]
        public void RemoveScaleYTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 20.2F;
            float actual;
            target.ScaleY = expected;
            actual = target.ScaleY;
            Assert.AreEqual(expected, actual);

            target.RemoveScaleY();

            Assert.IsTrue(TransformOperation.IsNotSet(target.ScaleY));
            Assert.IsNull(target.Operations);
        }

        /// <summary>
        ///A test for SkewX
        ///</summary>
        [TestMethod()]
        public void SkewXTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 35.1F;
            float actual;
            target.SkewX = expected;
            actual = target.SkewX;
            Assert.AreEqual(expected, actual);

            //Check that the operations are stored correctly
            Assert.IsNotNull(target.Operations);
            Assert.AreEqual(TransformType.Skew, target.Operations.Type);
            Assert.AreEqual(35.1F, target.Operations.Value1);
            Assert.IsTrue(TransformOperation.IsNotSet(target.Operations.Value2));
            Assert.IsNull(target.Operations.Next);
        }

        /// <summary>
        ///A test for SkewY
        ///</summary>
        [TestMethod()]
        public void SkewYTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 40.1F;
            float actual;
            target.SkewY = expected;
            actual = target.SkewY;
            Assert.AreEqual(expected, actual);

            //Check that the operations are stored correctly
            Assert.IsNotNull(target.Operations);
            Assert.AreEqual(TransformType.Skew, target.Operations.Type);
            Assert.IsTrue(TransformOperation.IsNotSet(target.Operations.Value1));
            Assert.AreEqual(40.1F, target.Operations.Value2);
            Assert.IsNull(target.Operations.Next);
        }

        /// <summary>
        ///A test for RemoveSkewX
        ///</summary>
        [TestMethod()]
        public void RemoveSkewXTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 20.2F;
            float actual;
            target.SkewX = expected;
            actual = target.SkewX;
            Assert.AreEqual(expected, actual);

            target.RemoveSkewX();

            Assert.IsTrue(TransformOperation.IsNotSet(target.SkewX));
            Assert.IsNull(target.Operations);
        }

        /// <summary>
        ///A test for RemoveSkewY
        ///</summary>
        [TestMethod()]
        public void RemoveSkewYTest()
        {
            TransformStyle target = new TransformStyle();
            float expected = 20.2F;
            float actual;
            target.SkewY = expected;
            actual = target.SkewY;
            Assert.AreEqual(expected, actual);

            target.RemoveSkewY();

            Assert.IsTrue(TransformOperation.IsNotSet(target.SkewY));
            Assert.IsNull(target.Operations);
        }


        [TestMethod]
        public void SetAndRemoveMultipleTest()
        {
            TransformStyle target = new TransformStyle();
            float skewx = 20.2F;
            float skewy = 25.1F;
            float rotate = 0.53F;
            target.SkewX = skewx;
            target.SkewY = skewy;
            target.Rotate = rotate;

            Assert.AreEqual(skewx, target.SkewX);
            Assert.AreEqual(skewy, target.SkewY);
            Assert.AreEqual(rotate, target.Rotate);

            var op = target.Operations;
            Assert.IsNotNull(op);
            Assert.AreEqual(TransformType.Skew, op.Type);
            Assert.AreEqual(skewx, op.Value1);
            Assert.AreEqual(skewy, op.Value2);

            target.RemoveSkewY();

            Assert.IsTrue(TransformOperation.IsNotSet(target.SkewY));
            Assert.AreEqual(skewx, target.SkewX);

            op = target.Operations;
            Assert.IsNotNull(op);
            Assert.AreEqual(TransformType.Skew, op.Type);
            Assert.AreEqual(skewx, op.Value1);
            Assert.IsTrue(TransformOperation.IsNotSet(op.Value2));

            Assert.IsNotNull(op.Next);
            op = op.Next;

            Assert.IsNotNull(op);
            Assert.AreEqual(TransformType.Rotate, op.Type);
            Assert.AreEqual(rotate, op.Value1);
            Assert.IsTrue(TransformOperation.IsNotSet(op.Value2));

            target.RemoveRotate();

            op = target.Operations;
            Assert.IsNotNull(op);
            Assert.AreEqual(TransformType.Skew, op.Type);
            Assert.AreEqual(skewx, op.Value1);
            Assert.IsTrue(TransformOperation.IsNotSet(op.Value2));

            Assert.IsNull(op.Next); //Rotate is removed
        }


        [TestMethod]
        public void GetMatrixTest()
        {
            TransformStyle target = new TransformStyle();
            float skewx = 2.1F;
            float skewy = 2.5F;
            float rotate = (float)((Math.PI / 180) * 90);

            target.SkewX = skewx;
            target.SkewY = skewy;
            target.Rotate = rotate;

            Assert.AreEqual(skewx, target.SkewX);
            Assert.AreEqual(skewy, target.SkewY);
            Assert.AreEqual(rotate, target.Rotate);

            var op = target.Operations;
            Assert.IsNotNull(op);

            var matrix = op.GetMatrix(Scryber.Drawing.MatrixOrder.Append);
            var values = matrix.Components;

            Assert.AreEqual(2.1, Math.Round(values[0],1));
            Assert.AreEqual(1, Math.Round(values[1], 1));
            Assert.AreEqual(-1, Math.Round(values[2], 1));
            Assert.AreEqual(-2.5, Math.Round(values[3], 1));
            Assert.AreEqual(0, Math.Round(values[4], 1));
            Assert.AreEqual(0, Math.Round(values[5], 1));
        }
    }

}
