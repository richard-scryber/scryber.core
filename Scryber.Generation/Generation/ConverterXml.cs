/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;

namespace Scryber.Generation
{
    /// <summary>
    /// Static class of all the converter methods that accept an XmlReader and convert to the required type (returning as an object)
    /// </summary>
    public static class ConverterXml
    {

        #region private static string GetReaderValue(XmlReader reader)

        /// <summary>
        /// Gets the textual value of the current xml reader node
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static string GetReaderValue(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Attribute)
                return reader.Value;

            else if (reader.NodeType == XmlNodeType.Element)
                return reader.ReadElementContentAsString();

            else if (reader.NodeType == XmlNodeType.Text)
                return reader.Value;

            else
                return reader.ReadContentAsString();
        }

        #endregion

        #region internal static object ToType(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a runtime type and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToType(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            string prefix;
            string name;
            int sep = value.IndexOf(':');
            if (sep > 0)
            {
                name = value.Substring(sep + 1);
                prefix = value.Substring(0, sep);
            }
            else
            {
                prefix = "";
                name = value;
            }

            string ns = reader.LookupNamespace(prefix);
            bool isremote;
            ParserClassDefinition cdefn;
            try
            {
                cdefn = ParserDefintionFactory.GetClassDefinition(name, ns, settings.ConformanceMode == ParserConformanceMode.Strict,  out isremote);
            }
            catch (Exception ex)
            {
                throw new PDFParserException(string.Format(Errors.RuntimeTypeCouldNotBeDeterminedForReference, value), ex);
            }

            if (isremote)
            {
                if (settings.ConformanceMode == ParserConformanceMode.Strict)
                    throw new ArgumentOutOfRangeException("isremote", String.Format(Errors.CannotUseRemoteTypeReferencesInATypeAttribute, reader.Value));
                else
                {
                    settings.TraceLog.Add(TraceLevel.Error, "XML Parser", String.Format(Errors.CannotUseRemoteTypeReferencesInATypeAttribute, reader.Value));
                    return null;
                }
            }

            if (null == cdefn)
            {
                settings.TraceLog.Add(TraceLevel.Error, "XML Parser", String.Format(Errors.RuntimeTypeCouldNotBeDeterminedForReference, reader.Value));
                return typeof(NeverBeActivated);
            }
            else
                return cdefn.ClassType;
        }

        /// <summary>
        /// This is a fallback class that will be returned by the string ToType converter if the type cannot be evaluated and we are operating in lax mode
        /// </summary>
        private class NeverBeActivated
        {
        }

        #endregion

        #region internal static object ToInt32(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to an Int32 (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToInt32(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);

            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return int.Parse(value, settings.SpecificCulture);
            else
                return int.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToInt16(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to an Int16 (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToInt16(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);

            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return short.Parse(value, settings.SpecificCulture);
            else
                return short.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToInt64(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to an Int64 (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToInt64(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);

            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return long.Parse(value, settings.SpecificCulture);
            else
                return long.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToUInt32(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        
        /// <summary>
        /// Converts the current reader value to a UInt32 (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToUInt32(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);

            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return uint.Parse(value, settings.SpecificCulture);
            else
                return uint.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToUInt16(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a UInt16 (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToUInt16(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);

            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return ushort.Parse(value, settings.SpecificCulture);
            else
                return ushort.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToUInt64(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a UInt64 (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToUInt64(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);

            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return ulong.Parse(value, settings.SpecificCulture);
            else
                return ulong.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToFloat(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a floating point (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToFloat(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);

            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return float.Parse(value, settings.SpecificCulture);
            else
                return float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToDouble(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a double (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToDouble(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);

            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return double.Parse(value, settings.SpecificCulture);
            else
                return double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToDecimal(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a decimal (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToDecimal(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);

            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return decimal.Parse(value, settings.SpecificCulture);
            else
                return decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToString(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a string and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToString(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            return GetReaderValue(reader);
        }

        #endregion

        #region internal static object ToDateTime(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a date time (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToDateTime(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return DateTime.Parse(value, settings.SpecificCulture);
            else
                return DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToTimeSpan(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a time span (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToTimeSpan(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return TimeSpan.Parse(value, settings.SpecificCulture);
            else
                return TimeSpan.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToEnum(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to an enumerated value (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToEnum(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (value.IndexOf(' ') > -1)
                value = value.Replace(' ', ',');
            object result;
            if (Enum.TryParse(requiredType, value, true, out result))
                return result;
            else if (settings.ConformanceMode == ParserConformanceMode.Lax)
            {
                settings.TraceLog.Add(TraceLevel.Error, "Parser", "Could not convert the value of " + value + " to a " + requiredType.Name);
                return DBNull.Value;
            }
            else
                throw new ArgumentException("Could not convert the value of " + value + " to a " + requiredType.Name, "value");
        }

        #endregion

        #region internal static object ToByte(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a byte (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToByte(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return byte.Parse(value, settings.SpecificCulture);
            else
                return byte.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToSByte(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a signed byte (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToSByte(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (settings.HasSpecificCulture)
                return sbyte.Parse(value, settings.SpecificCulture);
            else
                return sbyte.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region internal static object ToChar(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a char (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToChar(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;
            else
                return char.Parse(value);
        }

        #endregion

        #region internal static object ToGuid(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a guid (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToGuid(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            if (string.IsNullOrEmpty(value))
                return Guid.Empty;
            else
                return new Guid(value);
        }

        #endregion

        #region internal static object ToBool(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a boolean (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToBool(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            return bool.Parse(value);
        }

        #endregion

        #region internal static object ToDBNull(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to dbnull (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToDBNull(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader); //still need to read ahead
            return DBNull.Value;
        }

        #endregion

        #region internal static object ToUri(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a url (optionally based on the current settings culture) and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToUri(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            string value = GetReaderValue(reader);
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            return new Uri(value);
        }

        #endregion

        #region internal static object ToXPathNavigable(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to an XPath navigable instance and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToXPathNavigable(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            while (reader.NodeType == XmlNodeType.Whitespace || reader.NodeType == XmlNodeType.Comment)
                reader.Read();

            XmlReader inner = reader.ReadSubtree();
            System.Xml.XPath.XPathNavigator nav = new System.Xml.XPath.XPathDocument(inner).CreateNavigator();
            System.Xml.XPath.IXPathNavigable navigable = nav;
            return navigable;
        }

        #endregion

        #region internal static object ToXPathNavigator(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to an XPath Navigator and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToXPathNavigator(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            while (reader.NodeType == XmlNodeType.Whitespace || reader.NodeType == XmlNodeType.Comment)
                reader.Read();

            XmlReader inner = reader.ReadSubtree();
            System.Xml.XPath.XPathNavigator nav = new System.Xml.XPath.XPathDocument(inner).CreateNavigator();
            return nav;
        }

        #endregion

        #region internal static object ToXmlNode(XmlReader reader, Type requiredType, PDFGeneratorSettings setting)

        /// <summary>
        /// Converts the current reader value to an XML Node and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        internal static object ToXmlNode(XmlReader reader, Type requiredType, PDFGeneratorSettings setting)
        {
            while (reader.NodeType == XmlNodeType.Whitespace || reader.NodeType == XmlNodeType.Comment)
                reader.Read();
            

            if (reader.NodeType == XmlNodeType.EndElement)
                return null; //blank value
            else
            {
                XmlReader inner = reader.ReadSubtree();
                System.Xml.XmlDocument doc = new XmlDocument();
                doc.Load(inner);
                return doc.FirstChild;
            }
        }

        #endregion

        #region internal static object ToPDFTemplate(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)

        /// <summary>
        /// Converts the current reader value to a PDF Template and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static object ToPDFTemplate(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
        {
            object obj = Activator.CreateInstance(settings.TempateGeneratorType);
            IPDFTemplateGenerator gen = (IPDFTemplateGenerator)obj;
            string content = reader.ReadInnerXml();
            gen.InitTemplate(content, new System.Xml.XmlNamespaceManager(reader.NameTable));
            return gen;
        }

        #endregion

        #region internal static object ToXmlDocument(XmlReader reader, Type requiredType, PDFGeneratorSettings setting)

        /// <summary>
        /// Converts the current reader value to an XML Document and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="requiredType"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        internal static object ToXmlDocument(XmlReader reader, Type requiredType, PDFGeneratorSettings setting)
        {
            XmlReader inner = reader.ReadSubtree();
            System.Xml.XmlDocument doc = new XmlDocument();
            return doc;
        }

        #endregion

        //
        // Parseable Converters - implments parsing on classes that 
        //

        #region internal static ParseableConverter GetParserConverter(Type t)

        /// <summary>
        /// Cached list of reflected members
        /// </summary>
        private static List<ParseableConverter> _parsables = new List<ParseableConverter>();

        /// <summary>
        /// Gets the ParsableConverter for a specific type. 
        /// The Type must as a minumum has a public static Parse(string) or Parse(string, IFormatProvider) 
        /// method that returns an instancve of type t
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <remarks>The method is not thread safe, but as ParsableConveter is immutable the worst that can happen is we get a couple of 
        /// instance that parse the same type in our cached collection. Next time the first one will be used.
        /// </remarks>
        public static ParseableConverter GetParserConverter(Type t)
        {
            //do we have this already

            foreach (ParseableConverter pc in _parsables)
            {
                if (pc.Type == t)
                {
                    return pc;
                }
            }

            // not added yet so reflect the methods to find the 'Parse' methods.

            ParseableConverter conv;
            MethodInfo parse = null;
            MethodInfo parselocal = null;
            MethodInfo parsexml = null;
            MethodInfo parsexmllocal = null;

            MethodInfo[] mis = t.GetMethods(BindingFlags.Static | BindingFlags.Public);

            foreach (MethodInfo mi in mis)
            {
                if (mi.ReturnType != t)
                    continue;
                if (mi.Name != "Parse")
                    continue;
                ParameterInfo[] pis = mi.GetParameters();
                if (pis.Length == 1)
                {
                    //we have a match for the single parameter signature
                    if (pis[0].ParameterType == typeof(string))
                        parse = mi;
                    else if (pis[0].ParameterType == typeof(System.Xml.XmlReader))
                        parsexml = mi;
                    else
                        continue;
                }
                else if (pis.Length == 2)
                {
                    //we have a match for the double parameter signature.
                    if (pis[0].ParameterType == typeof(string) && pis[1].ParameterType == typeof(IFormatProvider))
                        parselocal = mi;
                    else if (pis[0].ParameterType == typeof(System.Xml.XmlReader) && pis[1].ParameterType == typeof(IFormatProvider))
                        parsexmllocal = mi;
                    else
                        continue;
                }
            }

            //if no matches then throw an exception
            if (null == parse && null == parselocal && null == parsexml && null == parsexmllocal)
                throw new PDFParserException(string.Format(Errors.ParsableValueMustHaveParseMethod, t));

            //dynamically generate the generic type for each of the delegates and a matching ParsableConverter.

            Type gen = typeof(Parse<>);
            Type genwithtype = gen.MakeGenericType(t);
            Delegate parseMethod = null == parse ? null : Delegate.CreateDelegate(genwithtype, parse);

            gen = typeof(ParseWithFormat<>);
            genwithtype = gen.MakeGenericType(t);
            Delegate parseLocalMethod = null == parselocal ? null : Delegate.CreateDelegate(genwithtype, parselocal);

            gen = typeof(ParseXml<>);
            genwithtype = gen.MakeGenericType(t);
            Delegate parseXmlMethod = null == parsexml ? null : Delegate.CreateDelegate(genwithtype, parsexml);

            gen = typeof(ParseXmlWithFormat<>);
            genwithtype = gen.MakeGenericType(t);
            Delegate parseXmlLocalMethod = null == parsexmllocal ? null : Delegate.CreateDelegate(genwithtype, parsexmllocal);


            Type genconverter = typeof(ParseableConverter<>).MakeGenericType(t);

            //create instances and assign values

            object instance = Activator.CreateInstance(genconverter);
            conv = (ParseableConverter)instance;
            conv.Parse = parseMethod;
            conv.ParseWithFormat = parseLocalMethod;
            conv.ParseXml = parseXmlMethod;
            conv.ParseXmlWithFormat = parseXmlLocalMethod;

            //store in the collection for later retireval.
            _parsables.Add(conv);

            return conv;
        }

        #endregion

        

        /// <summary>
        /// Gets the PDFXmlConverter delegate method for the type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static PDFXmlConverter GetParsableXmlConverter(Type t)
        {
            ParseableConverter conv = GetParserConverter(t);
            return conv.XmlConverter;
        }

        // Creating generic classes and methods from reflection to
        // hold and call a classes Parse method on other types that can convert themselves from strings to the required type.

        // delegates

        #region internal delegate T Parse<T>(string value);

        /// <summary>
        /// Simple parse method that returns the required type (culture independant)
        /// </summary>
        /// <typeparam name="T">The type of the value that will be returned</typeparam>
        /// <param name="value">The string to parse</param>
        /// <returns></returns>
        internal delegate T Parse<T>(string value);

        #endregion

        #region internal delegate T ParseXml<T>(XmlReader reader);

        /// <summary>
        /// Simple parse method that returns the required type (culture independant) based on the current node of the XMLReader
        /// </summary>
        /// <typeparam name="T">The type of the value that will be returned</typeparam>
        /// <param name="reader">The XmlReader to parse the value from</param>
        /// <returns></returns>
        internal delegate T ParseXml<T>(System.Xml.XmlReader reader);

        #endregion

        #region internal delegate T ParseWithFormat<T>(string value, IFormatProvider provider);

        /// <summary>
        /// Extended parse method that also accepts the format provider as well as the string and returns the required type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        internal delegate T ParseWithFormat<T>(string value, IFormatProvider provider);

        #endregion

        #region internal delegate T ParseXmlWithFormat<T>(XmlReader value, IFormatProvider provider);

        /// <summary>
        /// Extended parse method that also accepts the format provider as well as the XmlReader and returns the required type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        internal delegate T ParseXmlWithFormat<T>(System.Xml.XmlReader reader, IFormatProvider provider);

        #endregion

        //
        // classes
        //

        #region internal abstract class ParseableConverter

        /// <summary>
        /// Base abstract class that encapsulates the parsing methods and ways of calling. 
        /// Holds a reference to the type and the Parse method(s) on the type
        /// that are called at runtime to dynamically parse unknown types.
        /// </summary>
        public abstract class ParseableConverter
        {
            /// <summary>
            /// Gets the Type this parsable converter returns from it's parse methods
            /// </summary>
            internal Type Type { get; private set; }

            /// <summary>
            /// Method so they convert from the current XMLReader value to the required type. Inheritors must override
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="requiredType"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            internal abstract object ConvertXml(XmlReader reader, Type requiredType, PDFGeneratorSettings settings);

            /// <summary>
            /// Method so they convert from the current string value to the required type. Inheritors must override
            /// </summary>
            /// <param name="value"></param>
            /// <param name="requiredType"></param>
            /// <param name="provider"></param>
            /// <returns></returns>
            internal abstract object ConvertValue(string value, Type requiredType, IFormatProvider provider);

            /// <summary>
            /// Reference to the XmlConverter delegate method
            /// </summary>
            internal PDFXmlConverter XmlConverter { get; private set; }

            /// <summary>
            /// Reference to the Value convevertor delegate method
            /// </summary>
            internal PDFValueConverter ValueConverter { get; private set; }

            /// <summary>
            /// Reference to the actual simple parse method on the class itself
            /// </summary>
            internal abstract Delegate Parse { get; set; }

            /// <summary>
            /// Reference to the actual formatting parse method on the class itself.
            /// </summary>
            internal abstract Delegate ParseWithFormat { get; set; }

            /// <summary>
            /// Reference to any parse method that takes an xml reader
            /// </summary>
            internal abstract Delegate ParseXml { get; set; }

            /// <summary>
            /// Reference to any formatting parse method that takes and xml reader
            /// </summary>
            internal abstract Delegate ParseXmlWithFormat { get; set; }

            /// <summary>
            /// Protecxted constructor that accepts the type and 
            /// </summary>
            /// <param name="parsetype"></param>
            protected ParseableConverter(Type parsetype)
            {
                this.Type = parsetype;
                XmlConverter = new PDFXmlConverter(this.ConvertXml);
                ValueConverter = new PDFValueConverter(this.ConvertValue);
            }
        }

        #endregion

        #region internal class ParseableConverter<T> : ParseableConverter

        /// <summary>
        /// Concrete generic class that implements the Parsable converter where the parse methods can be retrieved from
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal class ParseableConverter<T> : ParseableConverter
        {

            #region internal override Delegate Parse {get;set;}

            private Parse<T> _parse;

            /// <summary>
            /// Gets or sets the parse method - which needs to be a strongly typed method with the Parse&lt;T&gt; signature
            /// </summary>
            internal override Delegate Parse
            {
                get { return _parse; }
                set { _parse = (Parse<T>)value; }

            }

            #endregion

            #region internal override Delegate ParseWithFormat {get;set;}

            private ParseWithFormat<T> _parseWithFormat;
            /// <summary>
            /// Gets or sets the parse with format method - which needs to be a strongly typed method with the ParseWithFormat&lt;T&gt; signature
            /// </summary>
            internal override Delegate ParseWithFormat
            {
                get
                {
                    return _parseWithFormat;
                }
                set
                {
                    _parseWithFormat = (ParseWithFormat<T>)value;
                }
            }

            #endregion

            #region internal override Delegate Parse {get;set;}

            private ParseXml<T> _parsexml;

            /// <summary>
            /// Gets or sets the parse method - which needs to be a strongly typed method with the Parse&lt;T&gt; signature
            /// </summary>
            internal override Delegate ParseXml
            {
                get { return _parsexml; }
                set { _parsexml = (ParseXml<T>)value; }

            }

            #endregion

            #region internal override Delegate ParseXmlWithFormat {get;set;}

            private ParseXmlWithFormat<T> _parseXmlWithFormat;
            /// <summary>
            /// Gets or sets the parse with format method - which needs to be a strongly typed method with the ParseWithFormat&lt;T&gt; signature
            /// </summary>
            internal override Delegate ParseXmlWithFormat
            {
                get
                {
                    return _parseXmlWithFormat;
                }
                set
                {
                    _parseXmlWithFormat = (ParseXmlWithFormat<T>)value;
                }
            }

            #endregion

            //
            // ctor
            //

            public ParseableConverter()
                : base(typeof(T))
            {
            }

            /// <summary>
            /// Implements the convert Xml method to take the current value from the reader
            /// then execute either the ParseWithFormat or Parse delegates to return a parsed value of the correct type.
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="requiredType"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            internal override object ConvertXml(XmlReader reader, Type requiredType, PDFGeneratorSettings settings)
            {
                
                if(null != _parseXmlWithFormat)
                {
                    if (settings.HasSpecificCulture)
                        return _parseXmlWithFormat(reader, settings.SpecificCulture);
                    else
                        return _parseXmlWithFormat(reader, System.Globalization.CultureInfo.InvariantCulture);
                }
                else if(null != _parsexml)
                {
                    return _parsexml(reader);
                }

                string value = GetReaderValue(reader);

                if (null != _parseWithFormat)
                {
                    if (settings.HasSpecificCulture)
                        return _parseWithFormat(value, settings.SpecificCulture);
                    else
                        return _parseWithFormat(value, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                    return _parse(value);
            }

            /// <summary>
            /// Implements the convert value method to take the string
            /// then execute either the ParseWithFormat or Parse delegates to return a parsed value of the correct type.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="requiredType"></param>
            /// <param name="provider"></param>
            /// <returns></returns>
            internal override object ConvertValue(string value, Type requiredType, IFormatProvider provider)
            {
                if (null != _parseWithFormat)
                    return _parseWithFormat(value, provider);
                else
                    return _parse(value);
            }
        }

        #endregion


    }
}
