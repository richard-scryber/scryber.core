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
using Scryber.Generation;

namespace Scryber.Binding
{
    /// <summary>
    /// Base class for all Binding expressions built by the BindingXPathExpressionFactory
    /// and hooked into the events on the component.
    /// </summary>
    public abstract class BindingXPathExpression
    {

        #region ivars

        private string _expr;
        private System.Xml.XPath.XPathExpression _compiled;
        private PDFValueConverter _converter;
        private System.Reflection.PropertyInfo _property;

        #endregion

        #region protected PDFValueConverter Converter {get;}

        /// <summary>
        /// Gets the converter associated with this instance to change the string value from the 
        /// XPath expression to the native value required for the property.
        /// </summary>
        protected PDFValueConverter Converter
        {
            get { return _converter; }
        }

        #endregion

        #region protected System.Reflection.PropertyInfo Property {get;}

        /// <summary>
        /// The property this expression is bound to.
        /// </summary>
        protected System.Reflection.PropertyInfo Property
        {
            get { return _property; }
        }

        #endregion

        //
        // .ctor
        //

        #region protected BindingXPathExpression()

        /// <summary>
        /// Protected constructor for the abstract type.
        /// </summary>
        protected BindingXPathExpression()
        {

        }

        #endregion

        //
        // public interface
        //

        #region public void BindComponent(object sender, PDFDataBindEventArgs args)

        /// <summary>
        /// Main method that sets the value of the senders property to the result of this instances XPath expression.
        /// Using the current data context.
        ///  Signature can be attached to an IPDFBindableComponent DataBound or DataBinding events.
        /// </summary>
        /// <param name="sender">The instance that raised the event</param>
        /// <param name="args"></param>
        public void BindComponent(object sender, PDFDataBindEventArgs args)
        {
            if (null == sender)
                throw new ArgumentNullException("sender");
            if (null == args)
                throw new ArgumentNullException("args");
            if (null == args.Context)
                throw new ArgumentNullException("args.Context");

            PDFDataStack stack = args.Context.DataStack;

            IPDFDataSource src = stack.Source;

            //Pop the current one so we can get the next one up (for count and position operations)
            object data = stack.Pop();

            try
            {
                DoBindComponent(sender, data, args.Context);
            }
            catch (Exception ex)
            {
                if (args.Context.Conformance == ParserConformanceMode.Lax)
                    args.Context.TraceLog.Add(TraceLevel.Error, "Item Binding", "Could not Set property '" + this.Property.Name + "': " + ex.Message, ex);
                else
                    throw;
            }
            finally
            {
                //Put the data we popped back on
                args.Context.DataStack.Push(data, src);
            }
        }

        #endregion

        //
        // abstract  method
        //

        #region protected abstract void DoBindComponent(object component, object data, PDFDataContext context);

        /// <summary>
        /// All inheritors must override this method to perform the actual evaluation and binding
        /// </summary>
        /// <param name="component">The component to bind the resultant value onto.</param>
        /// <param name="data">The current data value in the context to extract the value from.</param>
        /// <param name="context">The current context</param>
        protected abstract void DoBindComponent(object component, object data, PDFDataContext context);

        #endregion

        //
        // support methods
        //

        #region protected System.Xml.XPath.XPathExpression GetExpression(object data, PDFDataContext context)

        /// <summary>
        /// Converts the string expression on this instance to an actual XPathExpression with the current context
        /// </summary>
        /// <param name="data">The current data object on the stack</param>
        /// <param name="context">The current data context</param>
        /// <returns>A prepared XPath expression</returns>
        protected System.Xml.XPath.XPathExpression GetExpression(object data, PDFDataContext context)
        {
            if (null == _compiled)
            {
                _compiled = System.Xml.XPath.XPathExpression.Compile(this._expr, context.NamespaceResolver);
            }
            else
                _compiled.SetContext(context.NamespaceResolver);

            return _compiled;
        }

        #endregion

        #region protected void SetToConvertedValue(object instance, string value, PDFDataContext context)

        /// <summary>
        /// Attempts to convert the provided string value to the required type and set it as the value of the
        /// property on the passed object instance.
        /// </summary>
        /// <param name="instance">The instance the set the property value on.</param>
        /// <param name="value">The value to set</param>
        /// <param name="context">The current data context</param>
        protected void SetToConvertedValue(object instance, string value, PDFDataContext context)
        {
            try
            {
                object converted = this.Converter(value, this.Property.PropertyType, System.Globalization.CultureInfo.CurrentCulture);
                this.Property.SetValue(instance, converted, null);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not set property '" + this.Property.Name + "' to value '" + value + "' : " + ex.Message, ex);
            }
        }

        #endregion

        #region protected void SetToEmptyValue(object instance, PDFDataContext context)

        private static Type[] emptyTypeArray = new Type[] {};
        private static object[] emptyObjectArray = new object[] { };

        /// <summary>
        /// Using the type of the property identifies the correct empty value and sets it.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="context"></param>
        protected void SetToEmptyValue(object instance, PDFDataContext context)
        {
            if (this.Property.PropertyType.IsClass)
                this.Property.SetValue(instance, null);

            else if (this.Property.PropertyType.IsEnum)
                this.Property.SetValue(instance, 0);
            else
            {
                object value;
                TypeCode code = Type.GetTypeCode(this.Property.PropertyType);
                switch (code)
                {
                    case TypeCode.Empty:
                    case TypeCode.Object:
                        var ctor = this.Property.PropertyType.GetConstructor(emptyTypeArray);
                        if (null != ctor)
                            value = ctor.Invoke(emptyObjectArray);
                        else
                            value = null;
                        break;
                    case TypeCode.DBNull:
                        value = DBNull.Value;
                        break;
                    case TypeCode.Boolean:
                        value = default(bool);
                        break;
                    case TypeCode.Char:
                        value = default(char);
                        break;
                    case TypeCode.SByte:
                        value = default(sbyte);
                        break;
                    case TypeCode.Byte:
                        value = default(byte);
                        break;
                    case TypeCode.Int16:
                        value = default(Int16);
                        break;
                    case TypeCode.UInt16:
                        value = default(UInt16);
                        break;
                    case TypeCode.Int32:
                        value = default(Int32);
                        break;
                    case TypeCode.UInt32:
                        value = default(UInt32);
                        break;
                    case TypeCode.Int64:
                        value = default(Int64);
                        break;
                    case TypeCode.UInt64:
                        value = default(UInt64);
                        break;
                    case TypeCode.Single:
                        value = default(Single);
                        break;
                    case TypeCode.Double:
                        value = default(Double);
                        break;
                    case TypeCode.Decimal:
                        value = default(Decimal);
                        break;
                    case TypeCode.DateTime:
                        value = default(DateTime);
                        break;
                    case TypeCode.String:
                        value = default(String);
                        break;
                    default:
                        value = null;
                        break;
                }
                this.Property.SetValue(instance, value);
            }
        }

        #endregion

        //
        // factory methods
        //

        #region public static BindingXPathExpression Create(string expr, PDFValueConverter convert, System.Reflection.PropertyInfo property)

        /// <summary>
        /// Returns a new concrete instance of a BindingXPathExpression that can be called from a components
        /// DataBinding event and set the value of the property.
        /// </summary>
        /// <param name="expr">The binding XPath expression to use</param>
        /// <param name="convert">A value converter to change the string result into the required type.</param>
        /// <param name="property">The property this expression is bound to.</param>
        /// <returns>The expression that ban be attached to an event</returns>
        public static BindingXPathExpression Create(string expr, PDFValueConverter convert, System.Reflection.PropertyInfo property)
        {
            if (null == convert)
                throw new ArgumentNullException("convert");
            if (null == property)
                throw new ArgumentNullException("property");
            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");

            //Run a quick compile to validat the return type
            System.Xml.XPath.XPathExpression compiled = System.Xml.XPath.XPathExpression.Compile(expr);
            BindingXPathExpression binding = null;
            switch (compiled.ReturnType)
            {

                case System.Xml.XPath.XPathResultType.Boolean:
                    binding = new BindingXPathBoolExpression();
                    break;
                
                case System.Xml.XPath.XPathResultType.NodeSet:
                    binding = new BindingXPathNodeSetExpression();
                    break;

                case System.Xml.XPath.XPathResultType.Number:
                case System.Xml.XPath.XPathResultType.String:
                    binding = new BindingXPathValueExpression();
                    break;

                case System.Xml.XPath.XPathResultType.Error:
                    throw new PDFParserException(String.Format(Errors.InvalidXPathExpression, expr));

                case System.Xml.XPath.XPathResultType.Any:
                default:
                    throw new PDFParserException(String.Format(Errors.ReturnTypeOfXPathExpressionCouldNotBeDetermined, expr));

            }

            binding._converter = convert;
            binding._expr = expr;
            binding._compiled = compiled;
            binding._property = property;

            return binding;
        }

        #endregion
    }

    /// <summary>
    /// Implements the BindingXPathExpression for an expression that results in a node set.
    /// </summary>
    public class BindingXPathNodeSetExpression : BindingXPathExpression
    {

        #region protected override void DoBindComponent(object component, object data, PDFDataContext context)

        /// <summary>
        /// Override method that performs the actual property setting from a nodeset result.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="data"></param>
        /// <param name="context"></param>
        protected override void DoBindComponent(object component, object data, PDFDataContext context)
        {
            System.Xml.XPath.XPathExpression expr = this.GetExpression(data, context);
            if (data is System.Xml.XPath.XPathNodeIterator)
                data = ((System.Xml.XPath.XPathNodeIterator)data).Current;
            System.Xml.XPath.XPathNavigator nav = (System.Xml.XPath.XPathNavigator)data;

            nav = nav.SelectSingleNode(expr);

            if (null != nav)
            {
                string result = nav.Value;

                if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Item Binding", "Setting property '" + this.Property.Name + "' with the XPath binding expression '" + expr.Expression + "' to value '" + result + "'");

                SetToConvertedValue(component, result, context);
            }
            else
            {
                if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Item Binding", "NULL value returned for expression '" + expr.Expression + "' so clearing property value '" + this.Property.Name + "'");
                SetToEmptyValue(component, context);
            }
        }

        #endregion
    }


    /// <summary>
    /// Implements the BindingXPathExpression for an expression that results in a single value
    /// </summary>
    public class BindingXPathValueExpression : BindingXPathExpression
    {

        #region protected override void DoBindComponent(object component, object data, PDFDataContext context)

        /// <summary>
        /// Override method that performs the actual property setting from a value result.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="data"></param>
        /// <param name="context"></param>
        protected override void DoBindComponent(object component, object data, PDFDataContext context)
        {
            System.Xml.XPath.XPathExpression expr = this.GetExpression(data, context);

            System.Xml.XPath.XPathNavigator nav;
            System.Xml.XPath.XPathNodeIterator itter;

            if (data is System.Xml.XPath.XPathNodeIterator)
            {
                itter = ((System.Xml.XPath.XPathNodeIterator)data);
                nav = itter.Current;
            }
            else
            {
                nav = (System.Xml.XPath.XPathNavigator)data;
                itter = context.DataStack.HasData ? context.DataStack.Current as System.Xml.XPath.XPathNodeIterator : null;
            }


            object value = nav.Evaluate(expr, itter);
            if (null != value)
            {
                string result = value.ToString();

                if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Item Binding", "Setting property '" + this.Property.Name + "' with the XPath binding expression '" + expr.Expression + "' to value '" + result + "'");

                SetToConvertedValue(component, result, context);
            }
            else
            {

                if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Item Binding", "NULL value returned for expression '" + expr.Expression + "' so clearing value '" + this.Property.Name + "'");
                SetToEmptyValue(component, context);
            }
                
        }

        #endregion
    }

    /// <summary>
    /// Implements the BindingXPathExpression for an expression that results in a boolean value
    /// </summary>
    public class BindingXPathBoolExpression : BindingXPathExpression
    {

        #region protected override void DoBindComponent(object component, object data, PDFDataContext context)

        /// <summary>
        /// Override method that performs the actual property setting from a boolean result.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="data"></param>
        /// <param name="context"></param>
        protected override void DoBindComponent(object component, object data, PDFDataContext context)
        {
            System.Xml.XPath.XPathExpression expr = this.GetExpression(data, context);
            System.Xml.XPath.XPathNavigator nav;
            System.Xml.XPath.XPathNodeIterator itter;

            if (data is System.Xml.XPath.XPathNodeIterator)
            {
                itter = ((System.Xml.XPath.XPathNodeIterator)data);
                nav = itter.Current;
            }
            else
            {
                nav = (System.Xml.XPath.XPathNavigator)data;
                itter = context.DataStack.HasData ? context.DataStack.Current as System.Xml.XPath.XPathNodeIterator : null;
            }

            bool value = (bool)nav.Evaluate(expr, itter);

            if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, "Item Binding", "Setting property '" + this.Property.Name + "' with the XPath binding expression '" + expr.Expression + "' to value '" + value + "'");

            this.Property.SetValue(component, value, null);
        }

        #endregion
    }

    /// <summary>
    /// Implements the BindingXPathExpresion for an expression that results in another XPathNavigator expression
    /// </summary>
    public class BindingXPathNavigatorExpression : BindingXPathExpression
    {

        #region protected override void DoBindComponent(object component, object data, PDFDataContext context)

        /// <summary>
        /// Override method that performs the actual property setting from a Navigator result.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="data"></param>
        /// <param name="context"></param>
        protected override void DoBindComponent(object component, object data, PDFDataContext context)
        {
            System.Xml.XPath.XPathExpression expr = this.GetExpression(data, context);

            if (data is System.Xml.XPath.XPathNodeIterator)
                data = ((System.Xml.XPath.XPathNodeIterator)data).Current;
            System.Xml.XPath.XPathNavigator nav = (System.Xml.XPath.XPathNavigator)data;


            System.Xml.XPath.XPathNodeIterator itter = nav.Select(expr);

            if (itter.CurrentPosition < 0)
                itter.MoveNext();
            
            string value = itter.Current.Value;
            object converted = this.Converter(value, this.Property.PropertyType, System.Globalization.CultureInfo.CurrentCulture);


            if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, "Item Binding", "Setting property '" + this.Property.Name + "' with the XPath binding expression '" + expr.Expression + "' to value '" + ((null == value) ? "NULL" : value) + "'");
                    
            this.Property.SetValue(component, converted, null);

        }

        #endregion
    }
}
