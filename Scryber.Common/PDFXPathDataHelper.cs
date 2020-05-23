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
using System.ComponentModel;
using System.Reflection;
using System.Xml.XPath;

namespace Scryber
{
    [Obsolete("No longer using the XPathDataHelper", true)]
    public static class PDFXPathDataHelper
    {
        /// <summary>
        /// Returns the value results of an xpath select expression against the current data context.
        /// </summary>
        /// <param name="expr">The expression to get the value of</param>
        /// <param name="context">The current data context</param>
        /// <returns>The resultant value of the expression in the current data context</returns>
        [Obsolete("XPathDataHelper is no longer used or supported", true)]
        public static object EvaluateValueExpression(XPathExpression expr, PDFDataContext context)
        {
            throw new NotSupportedException("No Longer using the XPathDataHelper");

            //if (null == expr)
            //    throw new ArgumentNullException("expr");
            //if (null == context)
            //    throw new ArgumentNullException("context");
            //if (false == context.DataStack.HasData)
            //    throw new ArgumentOutOfRangeException("context.DataStack.Current");
            //if (null == context.DataStack.Current)
            //    throw new ArgumentNullException("context.DataStack.Current");

            //object currentData = context.DataStack.Pop();
            //object result = null;

            

            //try
            //{
            //    //Need the current data to be an XPathNavigator 
            //    System.Xml.XPath.XPathNavigator navigator;
            //    if (currentData is System.Xml.XPath.XPathNavigator)
            //    {
            //        navigator = currentData as System.Xml.XPath.XPathNavigator;
            //    }
            //    else if (currentData is System.Xml.XPath.IXPathNavigable)
            //    {
            //        System.Xml.XPath.IXPathNavigable nav = (System.Xml.XPath.IXPathNavigable)currentData;
            //        navigator = nav.CreateNavigator();
            //    }
            //    else
            //        throw new InvalidCastException(Errors.DatabindingSourceNotXPath);

            //    // For position() and data context relative functions we also need to provide the
            //    // iterator the current XPathNavigator we extracted from if available.

            //    System.Xml.XPath.XPathNodeIterator parentItterator;

            //    if (context.DataStack.HasData && context.DataStack.Current is System.Xml.XPath.XPathNodeIterator)
            //        parentItterator = context.DataStack.Current as System.Xml.XPath.XPathNodeIterator;
            //    else
            //        parentItterator = null;

            //    if (null != context.NamespaceResolver)
            //        expr.SetContext(context.NamespaceResolver);

            //    result = navigator.Evaluate(expr, parentItterator);
            //}
            //finally
            //{
            //    //make sure we put the stack back to the original state.
                
            //    context.DataStack.Push(currentData, null);
            //}

            //return result;
        }

        /// <summary>
        /// Returns the value results of an xpath select expression against the current data context.
        /// </summary>
        /// <param name="expr">The expression to get the value of</param>
        /// <param name="context">The current data context</param>
        /// <returns>The resultant value of the expression in the current data context</returns>
        [Obsolete("XPathDataHelper is no longer used or supported", true)]
        public static System.Xml.XPath.XPathNavigator EvaluateSingleNodeExpression(XPathExpression expr, PDFDataContext context)
        {
            throw new NotSupportedException("No Longer using the XPathDataHelper");

            //if (null == expr)
            //    throw new ArgumentNullException("expr");
            //if (null == context)
            //    throw new ArgumentNullException("context");
            //if (false == context.DataStack.HasData)
            //    throw new ArgumentOutOfRangeException("context.DataStack.Current");
            //if (null == context.DataStack.Current)
            //    throw new ArgumentNullException("context.DataStack.Current");

            //object currentData = context.DataStack.Pop();

            


            //Need the current data to be an XPathNavigator 
            //System.Xml.XPath.XPathNavigator navigator;
            //try
            //{
                
            //    if (currentData is System.Xml.XPath.XPathNavigator)
            //    {
            //        navigator = currentData as System.Xml.XPath.XPathNavigator;
            //    }
            //    else if (currentData is System.Xml.XPath.IXPathNavigable)
            //    {
            //        System.Xml.XPath.IXPathNavigable nav = (System.Xml.XPath.IXPathNavigable)currentData;
            //        navigator = nav.CreateNavigator();
            //    }
            //    else
            //        throw new InvalidCastException(Errors.DatabindingSourceNotXPath);

            //    // For position() and data context relative functions we also need to provide the
            //    // iterator the current XPathNavigator we extracted from if available.

            //    System.Xml.XPath.XPathNodeIterator parentItterator;

            //    if (context.DataStack.HasData && context.DataStack.Current is System.Xml.XPath.XPathNodeIterator)
            //        parentItterator = context.DataStack.Current as System.Xml.XPath.XPathNodeIterator;
            //    else
            //        parentItterator = null;

            //    if (null != context.NamespaceResolver)
            //        expr.SetContext(context.NamespaceResolver);

            //    navigator = navigator.SelectSingleNode(expr);
            //}
            //finally
            //{
            //    //make sure we put the stack back to the original state.
                
            //    context.DataStack.Push(currentData, null);
            //}

            //return navigator;
        }




        /// <summary>
        /// Returns the node itterator results of an xpath select expression against the current data context.
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [Obsolete("XPathDataHelper is no longer used or supported", true)]
        public static System.Xml.XPath.XPathNodeIterator EvaluateSelectExpression(System.Xml.XPath.XPathExpression expr, PDFDataContext context)
        {
            throw new NotSupportedException("No Longer using the XPathDataHelper");

            //if (null == expr)
            //    throw new ArgumentNullException("expr");
            //if (null == context)
            //    throw new ArgumentNullException("context");
            //if (false == context.DataStack.HasData)
            //    throw new ArgumentOutOfRangeException("context.DataStack.Current");
            //if (null == context.DataStack.Current)
            //    throw new ArgumentNullException("context.DataStack.Current");

            //object currentData = context.DataStack.Pop();
            //System.Xml.XPath.XPathNodeIterator itter;

            

            //try
            //{
            //    //Need the current data to be an XPathNavigator 
            //    System.Xml.XPath.XPathNavigator navigator;
            //    if (currentData is System.Xml.XPath.XPathNavigator)
            //    {
            //        navigator = currentData as System.Xml.XPath.XPathNavigator;
            //    }
            //    else if (currentData is System.Xml.XPath.IXPathNavigable)
            //    {
            //        System.Xml.XPath.IXPathNavigable nav = (System.Xml.XPath.IXPathNavigable)currentData;
            //        navigator = nav.CreateNavigator();
            //    }
            //    else
            //        throw new InvalidCastException(Errors.DatabindingSourceNotXPath);

            //    // For position() and data context relative functions we also need to provide the
            //    // iterator the current XPathNavigator we extracted from if available.

            //    System.Xml.XPath.XPathNodeIterator parentItterator;

            //    if (context.DataStack.HasData && context.DataStack.Current is System.Xml.XPath.XPathNodeIterator)
            //        parentItterator = context.DataStack.Current as System.Xml.XPath.XPathNodeIterator;
            //    else
            //        parentItterator = null;

            //    if (null != context.NamespaceResolver)
            //        expr.SetContext(context.NamespaceResolver);

            //    itter = navigator.Select(expr);
            //}
            //finally
            //{
            //    //make sure we put the stack back to the original state.
                
            //    context.DataStack.Push(currentData, null);
            //}

            //return itter;
        }

        /// <summary>
        /// Tests an XPath expression against the current data and returns true if the expression returns a (True) value.
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="currentData"></param>
        /// <returns></returns>
        [Obsolete("XPathDataHelper is no longer used or supported",true)]
        public static bool EvaluateTestExpression(System.Xml.XPath.XPathExpression expr, PDFDataContext context)
        {
            throw new NotSupportedException("No Longer using the XPathDataHelper");

            //object value = EvaluateValueExpression(expr, context);

            //if (null == value)
            //    return false;
            //else if (value is bool)
            //    return (bool)value;
            //else
            //    return false;
        }


        /// <summary>
        /// Returns the required binding data based upon the specified parameters or null if there is no required source.
        /// </summary>
        /// <param name="datasourceComponent">The Component in the document that implements the IPDFDataSource interface. 
        /// Set it to null to ignore this parameter.
        /// If it is set, and so is the select path, then this path will be used 
        /// by the IPDFDataSource to extract the required source.
        /// </param>
        /// <param name="datasourcevalue">The value of the data source. 
        /// If not null then the value will be returned unless a select path is set. 
        /// If the select path is set then this value must be IXPathNavigable and the returned value will be the result of a select on the navigator</param>
        /// <param name="stack">If neither the Component or value are set, then the current data from the stack will be 
        /// used (if and only if theres is a select path). If there is no select path then null will be returned.
        /// If there is a select path then the current data must implement IXPathNavigable</param>
        /// <param name="selectpath">The path to  use to extract values</param>
        /// <returns>The required data or null.</returns>
        public static object GetBindingData(IPDFDataSource datasourceComponent, object datasourcevalue, string selectpath, PDFDataContext context)
        {

            throw new NotSupportedException("No Longer using the XPathDataHelper");

            //object data = null;

            //try
            //{

            //    if (null != datasourceComponent)
            //    {
            //        data = datasourceComponent.Select(selectpath, context);
            //    }
            //    else if (datasourcevalue != null)
            //    {
            //        if (!string.IsNullOrEmpty(selectpath))
            //        {
            //            if (datasourcevalue is System.Xml.XPath.IXPathNavigable)
            //            {
            //                System.Xml.XPath.XPathNavigator nav = ((System.Xml.XPath.IXPathNavigable)data).CreateNavigator();
            //                System.Xml.XPath.XPathNodeIterator itter = nav.Select(selectpath, context.NamespaceResolver);

            //                data = itter;
            //            }
            //            else
            //                new ArgumentException(Errors.DatabindingSourceNotXPath, "selectpath");
            //        }
            //        else
            //            data = datasourcevalue;
            //    }
            //    else if (!string.IsNullOrEmpty(selectpath))
            //    {
            //        data = context.DataStack.Current;
            //        if (null == data || !(data is System.Xml.XPath.IXPathNavigable))
            //            new ArgumentException(Errors.DatabindingSourceNotXPath, "stack.Current");
            //        data = ((System.Xml.XPath.IXPathNavigable)data).CreateNavigator().Select(selectpath, context.NamespaceResolver);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new PDFException("Could not get the binding data for expression '" + selectpath + "'", ex);
            //}

            //return data;
        }


        //
        // Legacy methods - now private only
        //

        #region Legacy data code

        public static readonly char[] ExpressionPartSeparator = new char[] { '.' };
        public static readonly char[] IndexExpressionStartChars = new char[] { '[', '(' };
        public static readonly char[] IndexExpressionEndChars = new char[] { ']', ')' };

        /// <summary>
        /// A cache of the property descriptors for each reflected type. 
        /// The concurrent dictionary is thread safe.
        /// </summary>
        private static Dictionary<Type, PropertyDescriptorCollection> _propertyCache = new Dictionary<Type, PropertyDescriptorCollection>();
        private static object _dictionarylock = new object();
        
        [Obsolete("Don't use this",true)]
        private static object Evaluate(object container, string expr)
        {
            if (String.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");
            expr = expr.Trim();
            if (expr.Length == 0)
                throw new ArgumentNullException("expr");
            if (null == container)
                return null;
            string[] parts = expr.Split(ExpressionPartSeparator);

            return Evaluate(container, parts);
        }

        private static object Evaluate(object container, string[] exprParts)
        {
            object value = container;
            for (int i = 0; i < exprParts.Length; i++)
            {
                string name = exprParts[i];
                if (name.IndexOfAny(IndexExpressionStartChars) < 0)
                {
                    value = GetPropertyValue(value, name);
                }
                else
                    value = GetIndexedPropertyValue(value, name);
            }

            return value;
        }

        private static object GetPropertyValue(object container, string propertyName)
        {
            if (null == container)
                throw new ArgumentNullException("container");
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");

            PropertyDescriptor desc = GetPropertiesFromCache(container).Find(propertyName, true);
            if (null == desc)
                throw new NullReferenceException(String.Format(CommonErrors.DatabindingPropertyNotFound, propertyName));

            return desc.GetValue(container);
        }

        private static object GetIndexedPropertyValue(object container, string propertyIndexedName)
        {
            if (null == container)
                throw new ArgumentNullException("container");
            if (string.IsNullOrEmpty(propertyIndexedName))
                throw new ArgumentNullException("propertyIndexedName");

            //object value = null;
            //bool flag = false;

            int start = propertyIndexedName.IndexOfAny(IndexExpressionStartChars);
            int end = propertyIndexedName.IndexOfAny(IndexExpressionEndChars, start + 1);
            
            if (start < 0 || end < 0 || end == start + 1)
                throw new ArgumentException(String.Format(CommonErrors.InvalidIndexerExpression, propertyIndexedName), "propertyIndexedName");

            string propname = null;
            string indexer = propertyIndexedName.Substring(start+1,(end-start)-1).Trim();
            object indexerValue = null;
            bool isint = false;

            if (start != 0)
            {
                propname = propertyIndexedName.Substring(0, start);
            }
            if (indexer.Length != 0)
            {
                if (indexer[0] == '"' && indexer[indexer.Length - 1] == '"')
                    //string indexer delimited by double qotes
                    indexerValue = (string)indexer.Substring(1, indexer.Length - 2);
                else if (indexer[0] == '\'' && indexer[indexer.Length - 1] == '\'')
                    //string indexer delimited by single quotes
                    indexerValue = (string)indexer.Substring(1, indexer.Length - 2);
                else if (char.IsDigit(indexer, 0))
                {
                    int num; //try to get an integer indexer
                    isint = int.TryParse(indexer, out num);
                    if (isint)
                        indexerValue = (int)num;
                    else
                        //first digit was a number, but still treat as a string
                        indexerValue = (string)indexer;
                }
                else
                    indexerValue = (string)indexer;
            }

            if (null == indexerValue)
                throw new ArgumentException(String.Format(CommonErrors.InvalidIndexerExpression, propertyIndexedName), "propertyIndexedName");

            object propertyValue = null;

            if (!string.IsNullOrEmpty(propname))
                propertyValue = GetPropertyValue(container, propname);
            else
                propertyValue = container;

            if (propertyValue == null)
                return null;

            Array array = propertyValue as Array;
            if (null != array && isint)
                return array.GetValue((int)indexerValue);

            if (propertyValue is System.Collections.IList && isint)
                return ((System.Collections.IList)propertyValue)[(int)indexerValue];

            PropertyInfo info = propertyValue.GetType().GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new Type[] { indexerValue.GetType() }, null);
            if(null == info)
                throw new ArgumentException(String.Format(CommonErrors.InvalidIndexerExpression, propertyIndexedName), "propertyIndexedName");

            return info.GetValue(propertyValue, new object[] { indexerValue });

        }

        private static PropertyDescriptorCollection GetPropertiesFromCache(object container)
        {
            
            if (container is ICustomTypeDescriptor)
            {
                return TypeDescriptor.GetProperties(container);
            }
            lock (_dictionarylock)
            {
                PropertyDescriptorCollection col = null;
                Type key = container.GetType();
                if (!_propertyCache.TryGetValue(key, out col))
                {
                    col = TypeDescriptor.GetProperties(key);
                    _propertyCache[key] = col;
                }

                return col;
            }
        }


        private static Type GetSimpleTypeFromName(string systemName)
        {
            return Type.GetType(systemName);
        }

        #endregion
    }
}
