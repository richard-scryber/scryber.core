using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.OpenType;

namespace Scryber.Core.UnitTests.OpenType
{
    [TestClass()]
    [TestCategory("Big Endian")]
    public class BigEndianReadWrite_Test
    {

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

        private System.IO.MemoryStream _stream;
        private BigEndianReader _reader;
        private BigEndianWriter _writer;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _stream = new System.IO.MemoryStream();
            _reader = new BigEndianReader(_stream);
            _writer = new BigEndianWriter(_stream);
        }

        private void ResetPosition()
        {
            _stream.Position = 0;
        }

        public BigEndianReadWrite_Test()
        {
        }

        #region UInt16

        [TestMethod]
        public void UInt16ReadWrite()
        {
            UInt16 value;
            UInt16 result;

            value = 20;

            ResetPosition();
            _writer.WriteUInt16(value);
            ResetPosition();
            result = _reader.ReadUInt16();


            Assert.AreEqual(value, result);

            //Multiple bytes

            value = 30000;

            ResetPosition();
            _writer.WriteUInt16(value);
            ResetPosition();
            result = _reader.ReadUInt16();

            Assert.AreEqual(value, result);


            value = 65535;

            ResetPosition();
            _writer.WriteUInt16(value);
            ResetPosition();
            result = _reader.ReadUInt16();

            Assert.AreEqual(value, result);
        }

        #endregion

        [TestMethod]
        public void Int16ReadWrite()
        {
            Int16 value;
            Int16 result;

            value = 20;

            ResetPosition();
            _writer.WriteInt16(value);

            ResetPosition();
            result = _reader.ReadInt16();

            Assert.AreEqual(value, result);

            
            //Multiple bytes

            value = 30000;

            ResetPosition();
            _writer.WriteInt16(value);

            ResetPosition();
            result = _reader.ReadInt16();

            Assert.AreEqual(value, result);

            //Negative Values

            value = -30;

            ResetPosition();
            _writer.WriteInt16(value);

            ResetPosition();
            result = _reader.ReadInt16();

            Assert.AreEqual(value, result);

            //Max Value

            value = 32767;

            ResetPosition();
            _writer.WriteInt16(value);
            ResetPosition();
            result = _reader.ReadInt16();

            Assert.AreEqual(value, result);

            //Min Value

            value = -32768;

            ResetPosition();
            _writer.WriteInt16(value);
            ResetPosition();
            result = _reader.ReadInt16();

            Assert.AreEqual(value, result);
        }


        [TestMethod]
        public void UInt32ReadWrite()
        {
            UInt32 value;
            UInt32 result;

            value = 20;

            ResetPosition();
            _writer.WriteUInt32(value);
            ResetPosition();
            result = _reader.ReadUInt32();


            Assert.AreEqual(value, result);


            //Multiple bytes

            value = 300000000;

            ResetPosition();
            _writer.WriteUInt32(value);
            ResetPosition();
            result = _reader.ReadUInt32();

            Assert.AreEqual(value, result);

            //Max Values

            value = 4294967295;

            ResetPosition();
            _writer.WriteUInt32(value);
            ResetPosition();
            result = _reader.ReadUInt32();

            Assert.AreEqual(value, result);

        }


        [TestMethod]
        public void Int32ReadWrite()
        {
            Int32 value;
            Int32 result;

            value = 20;

            ResetPosition();
            _writer.WriteInt32(value);
            ResetPosition();
            result = _reader.ReadInt32();

            Assert.AreEqual(value, result);


            //Multiple bytes

            value = 300000000;

            ResetPosition();
            _writer.WriteInt32(value);
            ResetPosition();
            result = _reader.ReadInt32();

            Assert.AreEqual(value, result);

            //Negative Values

            value = -30;

            ResetPosition();
            _writer.WriteInt32(value);
            ResetPosition();
            result = _reader.ReadInt32();

            Assert.AreEqual(value, result);

            //Max Values

            value = 2147483647;

            ResetPosition();
            _writer.WriteInt32(value);
            ResetPosition();
            result = _reader.ReadInt32();

            Assert.AreEqual(value, result);

            //Min Values

            value = -2147483648;

            ResetPosition();
            _writer.WriteInt32(value);
            ResetPosition();
            result = _reader.ReadInt32();

            Assert.AreEqual(value, result);

        }



        [TestMethod]
        public void UInt64ReadWrite()
        {
            UInt64 value;
            UInt64 result;

            value = 20;

            ResetPosition();
            _writer.WriteUInt64(value);
            ResetPosition();
            result = _reader.ReadUInt64();


            Assert.AreEqual(value, result);


            //Multiple bytes

            value = 300000000;

            ResetPosition();
            _writer.WriteUInt64(value);
            ResetPosition();
            result = _reader.ReadUInt64();

            Assert.AreEqual(value, result);

            //Max Values

            value = 4294967295;

            ResetPosition();
            _writer.WriteUInt64(value);
            ResetPosition();
            result = _reader.ReadUInt64();

            Assert.AreEqual(value, result);

        }


        [TestMethod]
        public void Int64ReadWrite()
        {
            Int64 value;
            Int64 result;

            value = 20;

            ResetPosition();
            _writer.WriteInt64(value);
            ResetPosition();
            result = _reader.ReadInt64();

            Assert.AreEqual(value, result);


            //Multiple bytes

            value = 300000000;

            ResetPosition();
            _writer.WriteInt64(value);
            ResetPosition();
            result = _reader.ReadInt64();

            Assert.AreEqual(value, result);

            //Negative Values

            value = -30;

            ResetPosition();
            _writer.WriteInt64(value);
            ResetPosition();
            result = _reader.ReadInt64();

            Assert.AreEqual(value, result);

            //Int 32 Max Values + 1

            value = 2147483648;

            ResetPosition();
            _writer.WriteInt64(value);
            ResetPosition();
            result = _reader.ReadInt64();

            Assert.AreEqual(value, result);

            //Int 32 Min Values - 1

            value = -2147483649;

            ResetPosition();
            _writer.WriteInt64(value);
            ResetPosition();
            result = _reader.ReadInt64();

            Assert.AreEqual(value, result);

        }

    }
}
