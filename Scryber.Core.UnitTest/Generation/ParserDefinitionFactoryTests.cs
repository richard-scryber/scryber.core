using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber;
using Scryber.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.Core.UnitTests.Generation
{

   

    [TestClass()]
    public class ParserDefinitionFactoryTests
    {
        /// <summary>
        /// These are the fake classes used in the Parser testing, in ParserTestClasses.cs
        /// </summary>
        public const string NamespaceAssembly = "Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, " 
                                               + "Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe";

        
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


        [TestMethod()]
        [TestCategory("Parser")]
        public void GetClassDefinitionTest()
        {
            ParserClassDefinition defn = ParserDefintionFactory.GetClassDefinition(typeof(Fakes.ParserRootOne));
            Assert.IsNotNull(defn);
            this.ValidateRootOneDefn(defn);

            defn = ParserDefintionFactory.GetClassDefinition(typeof(Fakes.ParserInnerComplex));
            this.ValidateInnerDefn(defn);
        }


        [TestMethod()]
        [TestCategory("Parser")]
        public void GetClassDefinitionTest1()
        {
            bool isremote = false;
            ParserClassDefinition defn = ParserDefintionFactory.GetClassDefinition("Root1", NamespaceAssembly, true, out isremote);
            Assert.IsNotNull(defn);
            ValidateRootOneDefn(defn);

            defn = ParserDefintionFactory.GetClassDefinition("Inner", NamespaceAssembly, true, out isremote);
            Assert.IsNotNull(defn);
            ValidateInnerDefn(defn);

            defn = ParserDefintionFactory.GetClassDefinition("Inner-Ref", NamespaceAssembly, true, out isremote);
            Assert.IsNotNull(defn);
            Assert.IsTrue(isremote);
            ValidateInnerDefn(defn);

        }

        /// <summary>
        /// Validates that the expected class definition matches the loaded definition for the Fakes.ParserRootOne class
        /// </summary>
        /// <param name="defn"></param>
        private void ValidateRootOneDefn(ParserClassDefinition defn)
        {
            Assert.AreEqual(defn.ClassType, typeof(Fakes.ParserRootOne));

            //Check the required versions
            Assert.AreEqual(new Version(0, 8, 0, 0), defn.MinRequiredFramework);
            Assert.AreEqual(new Version(1, 0, 0, 0), defn.MaxSupportedFramework);
            Assert.IsTrue(defn.IsMinFrameworkSupported);
            Assert.IsTrue(defn.IsMaxFrameworkSupported);

            //name attribute
            Assert.AreEqual(1, defn.Attributes.Count);
            ParserAttributeDefinition name = defn.Attributes[0] as ParserAttributeDefinition;
            Assert.IsNotNull(name, "No item was returned by index or it is not an Attribute definition");
            Assert.AreEqual("name", name.Name, "Name of the attribute definition and the name in the colection do not match");
            Assert.IsNotNull(name.Converter, "No converter for the string attribute 'name'");
            Assert.IsFalse(name.IsCodeDomGenerator);

            Assert.AreEqual(2, defn.Elements.Count);

            //First element - complex
            ParserComplexElementDefiniton complex = defn.Elements["Complex-Element"] as ParserComplexElementDefiniton;
            Assert.IsNotNull(complex);
            Assert.AreEqual("Complex-Element", complex.Name);
            Assert.AreEqual(String.Empty, complex.NameSpace);
            Assert.AreEqual(DeclaredParseType.ComplexElement, complex.ParseType);
            Assert.IsNotNull(complex.PropertyInfo);
            Assert.AreEqual("Complex", complex.PropertyInfo.Name);
            Assert.AreEqual(typeof(Fakes.ParserInnerComplex), complex.ValueType);


            //Second element - collection of ParserInnerComplex with explicit namespace
            ParserArrayDefinition collection = defn.Elements["Collection-One", NamespaceAssembly] as ParserArrayDefinition;
            Assert.IsNotNull(collection);
            Assert.AreEqual("Collection-One", collection.Name);
            Assert.IsFalse(collection.IsCustomParsable);
            //Assert.AreEqual(NamespaceAssembly, collection.NameSpace);
            Assert.AreEqual(DeclaredParseType.ArrayElement, collection.ParseType);
            Assert.AreEqual("CollectionOne", collection.PropertyInfo.Name);
            Assert.AreEqual(typeof(Fakes.ParserCollection), collection.ValueType);
            Assert.AreEqual(typeof(Fakes.ParserInnerComplex), collection.ContentType);


            //Third element - default collection of ParserInnerBase
            collection = defn.DefaultElement as ParserArrayDefinition;
            Assert.IsNotNull(collection);
            Assert.AreEqual("", collection.Name);
            Assert.IsFalse(collection.IsCustomParsable);
            Assert.AreEqual(string.Empty, collection.NameSpace);
            Assert.AreEqual(DeclaredParseType.ArrayElement, collection.ParseType);
            Assert.AreEqual("DefaultCollection", collection.PropertyInfo.Name);
            Assert.AreEqual(typeof(Fakes.ParserCollection), collection.ValueType);
            Assert.AreEqual(typeof(Fakes.ParserInnerBase), collection.ContentType);

        }

        /// <summary>
        /// Validates the attributes on the Fakes.ParserInnerComplex type
        /// </summary>
        /// <param name="defn"></param>
        private void ValidateInnerDefn(ParserClassDefinition defn)
        {
            Assert.AreEqual(5, defn.Attributes.Count);
            Assert.AreEqual(0, defn.Elements.Count);
            Assert.IsNull(defn.DefaultElement);
            Assert.IsTrue(defn.IsMaxFrameworkSupported); //not defined
            Assert.IsTrue(defn.IsMinFrameworkSupported); //not defined

            ParserAttributeDefinition attr;
            ParserPropertyDefinition prop;
            Assert.IsTrue(defn.Attributes.TryGetPropertyDefinition("index", string.Empty, out prop)); //defined on base class
            Assert.AreEqual("Index", prop.PropertyInfo.Name);
            attr = prop as ParserAttributeDefinition;
            Assert.IsNotNull(attr);

            Assert.IsFalse(defn.Attributes.TryGetPropertyDefinition("another-name", string.Empty, out prop));
            
            
            Assert.IsFalse(defn.Attributes.TryGetPropertyDefinition("AnotherIndex", string.Empty, out prop)); //Does not exist
            Assert.IsFalse(defn.Attributes.TryGetPropertyDefinition("inherits", string.Empty, out prop));//defined but overridden
            Assert.IsTrue(defn.Attributes.TryGetPropertyDefinition("concrete", NamespaceAssembly, out prop));//the override

            Assert.IsTrue(defn.Attributes.TryGetPropertyDefinition("size", null, out prop));
            Assert.IsTrue(defn.Attributes.TryGetPropertyDefinition("ro-size", null, out prop));
            Assert.IsTrue(defn.Attributes.TryGetPropertyDefinition("date", null, out prop));
            
            //TODO: Events
        }



        [TestMethod()]
        [TestCategory("Parser")]
        public void LookupAssemblyForXmlNamespaceTest()
        {
            //Assert.Inconclusive("The look up of an assembly name based on a namespace needs implementing");

        }

       

        [TestMethod()]
        [TestCategory("Parser")]
        public void IsSimpleObjectTypeTest()
        {
            IFormatProvider invariant = System.Globalization.CultureInfo.InvariantCulture;
            PDFValueConverter converter;
            
            //Enumueration
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(Scryber.DrawingOrigin), out converter));
            Assert.AreEqual(Scryber.DrawingOrigin.BottomLeft, converter.Invoke("BottomLeft", typeof(Scryber.DrawingOrigin), invariant), "Enum failed");

            //Boolean
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(bool), out converter));
            Assert.AreEqual(true, converter.Invoke("true", typeof(bool), invariant), "Boolean failed");

            //Byte
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(byte), out converter));
            Assert.AreEqual((byte)2, converter.Invoke("2", typeof(byte), invariant), "Byte failed");

            //Char
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(char), out converter));
            Assert.AreEqual('c', converter.Invoke("c", typeof(char), invariant), "Char failed");

            //DateTime
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(DateTime), out converter));
            Assert.AreEqual(new DateTime(2014, 01, 14, 12, 11, 10), converter.Invoke("2014-01-14 12:11:10", typeof(DateTime), invariant), "DateTime failed");

            //Decimal
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(decimal), out converter));
            Assert.AreEqual(-12.5M, converter.Invoke("-12.5", typeof(decimal), invariant), "Decimal failed");

            //Double
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(double), out converter));
            Assert.AreEqual(12.50, converter.Invoke("12.5", typeof(double), invariant), "Double failed");

            //Int16
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(short), out converter));
            Assert.AreEqual((short)12, converter.Invoke("12", typeof(short), invariant), "Short failed");

            //Int32
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(int), out converter));
            Assert.AreEqual(-12, converter.Invoke("-12", typeof(int), invariant), "Int32 failed");

            //Int64
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(long), out converter));
            Assert.AreEqual((long)12, converter.Invoke("12", typeof(long), invariant), "Long failed");

            //SByte
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(SByte), out converter));
            Assert.AreEqual((SByte)(-12), converter.Invoke("-12", typeof(sbyte), invariant), "SByte failed");

            //Float
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(float), out converter));
            Assert.AreEqual(12.4F, converter.Invoke("12.4", typeof(float), invariant), "Float failed");

            //String
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(string), out converter));
            Assert.AreEqual("value", converter.Invoke("value", typeof(string), invariant), "String failed");

            //UInt16
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(ushort), out converter));
            Assert.AreEqual((ushort)12, converter.Invoke("12", typeof(ushort), invariant), "UShort failed");

            //Int16
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(uint), out converter));
            Assert.AreEqual((uint)12, converter.Invoke("12", typeof(uint), invariant), "UInt32 failed");

            //Int16
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(ulong), out converter));
            Assert.AreEqual((ulong)12, converter.Invoke("12", typeof(ulong), invariant), "UInt64 failed");

            //Guid
            string guidS = "{62261BCA-DEE5-4285-A406-8236BC8025FA}";
            Guid guid = new Guid(guidS);
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(Guid), out converter));
            Assert.AreEqual(guid, converter.Invoke(guidS, typeof(Guid), invariant), "Guid failed");

            TimeSpan time = new TimeSpan(234567);
            string timeS = time.ToString();
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(TimeSpan), out converter));
            Assert.AreEqual(time, converter.Invoke(timeS, typeof(TimeSpan), invariant), "TimeSpan failed");

            string url = "http://www.scryber.co.uk";
            Uri uri = new Uri(url);
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(Uri), out converter));
            Assert.AreEqual(uri, converter.Invoke(url, typeof(Uri), invariant), "URI failed");

            Fakes.ParserRootOne complex = new Fakes.ParserRootOne();
            Assert.IsFalse(ParserDefintionFactory.IsSimpleObjectType(complex.GetType(), out converter));
            Assert.IsNull(converter);
        }

        [TestMethod()]
        [TestCategory("Parser")]
        public void IsSimpleObjectExplictCultureTest()
        {
            IFormatProvider french = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
            PDFValueConverter converter;

            //Enumueration
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(Scryber.DrawingOrigin), out converter));
            Assert.AreEqual(Scryber.DrawingOrigin.BottomLeft, converter.Invoke("BottomLeft", typeof(Scryber.DrawingOrigin), french), "Enum failed");

            //Boolean
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(bool), out converter));
            Assert.AreEqual(true, converter.Invoke("true", typeof(bool), french), "Boolean failed");

            //Byte
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(byte), out converter));
            Assert.AreEqual((byte)2, converter.Invoke("2", typeof(byte), french), "Byte failed");

            //Char
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(char), out converter));
            Assert.AreEqual('c', converter.Invoke("c", typeof(char), french), "Char failed");

            //DateTime
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(DateTime), out converter));
            Assert.AreEqual(new DateTime(2014, 05, 25, 12, 11, 10), converter.Invoke("25 mai 2014 12:11:10", typeof(DateTime), french), "DateTime failed");

            //Decimal
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(decimal), out converter));
            Assert.AreEqual(-12.5M, converter.Invoke("-12,5", typeof(decimal), french), "Decimal failed");

            //Double
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(double), out converter));
            Assert.AreEqual(12.50, converter.Invoke("12,5", typeof(double), french), "Double failed");

            //Int16
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(short), out converter));
            Assert.AreEqual((short)12, converter.Invoke("12", typeof(short), french), "Short failed");

            //Int32
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(int), out converter));
            Assert.AreEqual(-12, converter.Invoke("-12", typeof(int), french), "Int32 failed");

            //Int64
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(long), out converter));
            Assert.AreEqual((long)12, converter.Invoke("12", typeof(long), french), "Long failed");

            //SByte
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(SByte), out converter));
            Assert.AreEqual((SByte)(-12), converter.Invoke("-12", typeof(sbyte), french), "SByte failed");

            //Float
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(float), out converter));
            Assert.AreEqual(12.4F, converter.Invoke("12,4", typeof(float), french), "Float failed");

            //String
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(string), out converter));
            Assert.AreEqual("value", converter.Invoke("value", typeof(string), french), "String failed");

            //UInt16
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(ushort), out converter));
            Assert.AreEqual((ushort)12, converter.Invoke("12", typeof(ushort), french), "UShort failed");

            //Int16
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(uint), out converter));
            Assert.AreEqual((uint)12, converter.Invoke("12", typeof(uint), french), "UInt32 failed");

            //Int16
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(ulong), out converter));
            Assert.AreEqual((ulong)12, converter.Invoke("12", typeof(ulong), french), "UInt64 failed");

            //Guid
            string guidS = "{62261BCA-DEE5-4285-A406-8236BC8025FA}";
            Guid guid = new Guid(guidS);
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(Guid), out converter));
            Assert.AreEqual(guid, converter.Invoke(guidS, typeof(Guid), french), "Guid failed");

            TimeSpan time = new TimeSpan(234567);
            string timeS = time.ToString();
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(TimeSpan), out converter));
            Assert.AreEqual(time, converter.Invoke(timeS, typeof(TimeSpan), french), "TimeSpan failed");

            string url = "http://www.scryber.co.uk";
            Uri uri = new Uri(url);
            Assert.IsTrue(ParserDefintionFactory.IsSimpleObjectType(typeof(Uri), out converter));
            Assert.AreEqual(uri, converter.Invoke(url, typeof(Uri), french), "URI failed");

            Fakes.ParserRootOne complex = new Fakes.ParserRootOne();
            Assert.IsFalse(ParserDefintionFactory.IsSimpleObjectType(complex.GetType(), out converter));
            Assert.IsNull(converter);
        }




        [TestMethod()]
        [TestCategory("Parser")]
        public void IsCustomParsableObjectTypeTest()
        {
            IFormatProvider invariant = System.Globalization.CultureInfo.InvariantCulture;
            PDFValueConverter convert;
            Type type = typeof(Scryber.Drawing.PDFThickness); //thickness is parsable
            string thickvalue = "12 45.3in 12.5 4mm";

            bool expected = true;
            bool actual = ParserDefintionFactory.IsCustomParsableObjectType(type, out convert);

            Assert.AreEqual(expected, actual, "PDFThickness is not registered as parsable");
            Assert.IsNotNull(convert);
            Scryber.Drawing.PDFThickness thick = (Scryber.Drawing.PDFThickness)convert(thickvalue, type, invariant);
            Assert.AreEqual(thick.Top, (Scryber.Drawing.PDFUnit)12);
            Assert.AreEqual(thick.Right, new Scryber.Drawing.PDFUnit(45.3, Scryber.Drawing.PageUnits.Inches));
            Assert.AreEqual(thick.Bottom, (Scryber.Drawing.PDFUnit)12.5);
            Assert.AreEqual(thick.Left, new Scryber.Drawing.PDFUnit(4, Scryber.Drawing.PageUnits.Millimeters));

            type = typeof(Scryber.Drawing.PDFPen); //pen is not parsable
            expected = false;
            actual = ParserDefintionFactory.IsCustomParsableObjectType(type, out convert);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(convert);

        }



        [TestMethod()]
        [TestCategory("Parser")]
        public void InvalidAttributeTests()
        {
            bool isremote = false;
            try
            {
                ParserClassDefinition defn = ParserDefintionFactory.GetClassDefinition("Invalid-Inherits", NamespaceAssembly, true, out isremote);
                Assert.IsNotNull(defn);
                throw new InvalidOperationException("No Exception raised for the 'inherits' attribute");
            }
            catch (PDFParserException)
            {
                //Successfully caught the parser exception for invalid attribute name
            }

            try
            {
                ParserClassDefinition defn = ParserDefintionFactory.GetClassDefinition("Invalid-Code", NamespaceAssembly, true, out isremote);
                Assert.IsNotNull(defn);
                throw new InvalidOperationException("No Exception raised for the 'inherits' attribute");
            }
            catch (PDFParserException)
            {
                //Successfully caught the parser exception for invalid attribute name
            }

        }

    }


   
}
