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
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.XPath;

namespace Scryber.Data
{
    [PDFParsableComponent("ObjectCommand")]
    public class ObjectProviderCommand : XPathProviderCommandBase
    {
        //
        // properties
        //

        #region public string MethodName {get;set;}

        /// <summary>
        /// Gets or sets the name of the method to invoke on the specified type
        /// </summary>
        [PDFAttribute("method")]
        public string MethodName
        {
            get;
            set;
        }

        #endregion

        #region public string TypeName {get;set;}

        /// <summary>
        /// Gets or sets the Type (class) that contains the required method to invoke
        /// </summary>
        [PDFAttribute("type")]
        public string TypeName
        {
            get;
            set;
        }

        #endregion

        #region public PDFObjectParameterList Parameters {get;set;} + bool HasParameters {get;}

        private PDFObjectParameterList _params;

        /// <summary>
        /// Gets or sets the list of parameters 
        /// </summary>
        [PDFElement("Parameters")]
        [PDFArray(typeof(ObjectParameter))]
        public PDFObjectParameterList Parameters
        {
            get
            {
                if (null == _params)
                    _params = new PDFObjectParameterList();
                return _params;
            }
        }

        /// <summary>
        /// Returns true if this command has one or more p
        /// </summary>
        public bool HasParameters
        {
            get { return _params != null && _params.Count > 0; }
        }

        #endregion

        //
        // ctors
        //

        #region public PDFObjectProviderCommand()

        /// <summary>
        /// Creates a new instance of the PDFObjectProviderCommand
        /// </summary>
        public ObjectProviderCommand()
            : this(PDFObjectTypes.ObjectCommandType)
        {
        }

        #endregion

        #region protected PDFObjectProviderCommand(PDFObjectType type)

        /// <summary>
        /// Protected constructor inheritors can use to provide their own object type
        /// </summary>
        /// <param name="type"></param>
        protected ObjectProviderCommand(PDFObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // override methods
        //

        #region protected override void DoDataBind(PDFDataContext context)

        /// <summary>
        /// Extends the base implementation to ensure the parameters are also databound
        /// </summary>
        /// <param name="context"></param>
        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
        {
            base.DoDataBind(context, includeChildren);

            if (this.HasParameters)
                this.Parameters.DataBind(context);
        }

        #endregion

        #region public override object GetNullValue()
        /// <summary>
        /// Overrides the default implementation to ensure we return null
        /// </summary>
        /// <returns></returns>
        public override object GetNullValue()
        {
            return null;
        }

        #endregion

        #region protected override XPathNavigator DoLoadXPathData(PDFXPathDataSourceBase source, PDFDataContext context)

        /// <summary>
        /// Main entry point to load the XPath data from a defined method on a type.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override void DoEnsureDataLoaded(XPathDataSourceBase source, System.Data.DataSet ds, PDFDataContext context)
        {
            MethodInfo meth = AssertGetCallableMethod(source);

            object instance = GetCallableInstance(meth);
            object[] parameters = GetInvokingParameterValues(meth);

            object returnValue = InvokeMethod(meth, instance, parameters);

            if (null != returnValue)
            {
                XPathNavigator nav = GetNavigatorFromReturnValue(returnValue, meth.ReturnType);
                ds.ReadXml(nav.ReadSubtree());
            }

        }

        #endregion

        //
        // implementation
        //

        #region private object[] GetInvokingParameterValues(MethodInfo meth)

        /// <summary>
        /// Returns an array of parameter values in their native types, that can be passed to the method as an invocation
        /// </summary>
        /// <param name="meth"></param>
        /// <returns></returns>
        private object[] GetInvokingParameterValues(MethodInfo meth)
        {
            ParameterInfo[] param = meth.GetParameters();
            object[] values = new object[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                ParameterInfo pi = param[i];
                ObjectParameter objParam = this.Parameters[pi.Name];
                if (null == objParam)
                    throw new InvalidOperationException("Cannot find the parameter '" + pi.Name + "' we validated existed previously.");
                object value = objParam.GetNativeValue(this, pi.ParameterType);
                values[i] = value;
            }

            return values;
        }

        #endregion

        #region private object GetCallableInstance(MethodInfo meth)

        /// <summary>
        /// Returns either null if meth is static, or a new instance of the declaring type for the method.
        /// The declaring type must have a parameterless constructor
        /// </summary>
        /// <param name="meth">The method that we need to call</param>
        /// <returns>Null for static methods, or an instance of the declaring type.</returns>
        private object GetCallableInstance(MethodInfo meth)
        {
            if (meth.IsStatic)
                return null;
            else
            {
                object instance;
                try
                {

                    instance = System.Activator.CreateInstance(meth.DeclaringType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(string.Format(Errors.CannotCreateInstanceOfTypeOnCommand, meth.DeclaringType, this.ID),ex);
                }

                return instance;
            }
        }

        #endregion

        #region private object InvokeMethod(MethodInfo meth, object instance, object[] parameters)

        /// <summary>
        /// Actually invokes the required method on the instance (if any) with the parameters.
        /// </summary>
        /// <param name="meth"></param>
        /// <param name="instance"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object InvokeMethod(MethodInfo meth, object instance, object[] parameters)
        {
            return meth.Invoke(instance, parameters);
        }

        #endregion

        #region private XPathNavigator GetNavigatorFromReturnValue(object returnValue, System.Type type)

        /// <summary>
        /// Converts the return value from it's type to an XPathNavigator
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private XPathNavigator GetNavigatorFromReturnValue(object returnValue, System.Type type)
        {
            // null = null
            if (null == returnValue)
                return null;

            //Actual XPathNav
            else if (type == typeof(System.Xml.XPath.XPathNavigator))
                return (System.Xml.XPath.XPathNavigator)returnValue;

            //An XPathDocument
            else if (type == typeof(System.Xml.XPath.XPathDocument))
                return ((System.Xml.XPath.XPathDocument)returnValue).CreateNavigator();

            //An XMLDocument
            else if (type == typeof(System.Xml.XmlDocument))
                return ((System.Xml.XmlDocument)returnValue).CreateNavigator();

            //An XMLNode
            else if (type == typeof(System.Xml.XmlNode))
                return ((System.Xml.XmlNode)returnValue).CreateNavigator();

            //Linq Document
            else if (type == typeof(System.Xml.Linq.XDocument))
                return ((System.Xml.Linq.XDocument)returnValue).CreateNavigator();

            //Implements IXPathNavigable
            else if (returnValue is System.Xml.XPath.IXPathNavigable)
                return ((System.Xml.XPath.IXPathNavigable)returnValue).CreateNavigator();

            //Is IXmlSerializable
            else if (returnValue is System.Xml.Serialization.IXmlSerializable)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (System.Xml.XmlWriter w = System.Xml.XmlWriter.Create(ms))
                    {
                        ((System.Xml.Serialization.IXmlSerializable)returnValue).WriteXml(w);
                    }
                    ms.Flush();
                    ms.Position = 0;
                    XPathDocument doc = new XPathDocument(ms);
                    return doc.CreateNavigator();
                }
            }


            //Final option - serilize it to XML with the XmlSerializer
            else
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(type);
                    ser.Serialize(ms, returnValue);
                    ms.Flush();
                    ms.Position = 0;
                    XPathDocument doc = new XPathDocument(ms);
                    return doc.CreateNavigator();
                }
            }
        }

        #endregion

        //
        // Method Loading via reflection
        //

        #region private MethodInfo AssertGetCallableMethod(PDFXPathDataSourceBase source)

        /// <summary>
        /// Finds the matching method based on the TypeName and MethodName in this ObjectProviderCommand
        /// </summary>
        /// <param name="source">The source that holds this command</param>
        /// <returns></returns>
        private MethodInfo AssertGetCallableMethod(XPathDataSourceBase source)
        {
            if (string.IsNullOrEmpty(this.TypeName))
                throw new NullReferenceException(string.Format(Errors.TypeNameNotSetOnObjectDataSource, this.ID, source.ID));
            
            Type foundType = this.GetTypeFromName(this.TypeName);
            if (null == foundType)
                throw new NullReferenceException(string.Format(Errors.TypeCouldNotBeFoundForObjectData, this.TypeName, this.ID, source.ID));

            if(string.IsNullOrEmpty(this.MethodName))
                throw new NullReferenceException(string.Format(Errors.MethodNameNotSetOnObjectDataSource, this.ID, source.ID));

            MethodInfo[] meths = this.GetMethodOnType(this.MethodName, foundType);

            if(null == meths || meths.Length == 0)
                throw new NullReferenceException(string.Format(Errors.MethodCouldNotBeFoundForObjectData, this.MethodName, this.TypeName, this.ID, source.ID));

            MethodInfo matching;

            if (meths.Length > 1)
            {
                matching = FindMatchMethodSignatureToParameters(meths);
            }
            else
                matching = EnsureSignatureMatchesParameters(meths[0]);

            if(null == matching)
                throw new NullReferenceException(string.Format(Errors.MatchingMethodSignatureCouldNotBeFoundForObjectData, this.MethodName, this.TypeName, this.ID, source.ID));

            if(matching.ReturnType == null)
                throw new NullReferenceException(string.Format(Errors.MatchingMethodSignatureDoesNotHaveAReturnTypeObjectData, this.MethodName, this.TypeName, this.ID, source.ID));

            return matching;
        }

        #endregion

        #region private System.Type GetTypeFromName(string fullname)

        /// <summary>
        /// Returns the runtime type based on the full class name provided. If it is not found null will be returned
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        private System.Type GetTypeFromName(string fullname)
        {
            return System.Type.GetType(fullname, false);
        }

        #endregion

        #region private MethodInfo[] GetMethodOnType(string methname, System.Type foundType)

        /// <summary>
        /// Gets all the methods on the type provided that match the required name
        /// </summary>
        /// <param name="methname"></param>
        /// <param name="foundType"></param>
        /// <returns></returns>
        private MethodInfo[] GetMethodOnType(string methname, System.Type foundType)
        {
            MethodInfo[] allPublic = foundType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            
            List<MethodInfo> samename = new List<MethodInfo>(1); //High expectations of singular item

            foreach (MethodInfo mi in allPublic)
            {
                if (mi.Name == methname)
                    samename.Add(mi);
            }

            return samename.ToArray();
        }

        #endregion

        #region private MethodInfo FindMatchMethodSignatureToParameters(MethodInfo[] meths)

        /// <summary>
        /// Returns the single method out of the provided array that matches the signature based on this instances parameters
        /// </summary>
        /// <param name="meths"></param>
        /// <returns></returns>
        private MethodInfo FindMatchMethodSignatureToParameters(MethodInfo[] meths)
        {
            int paramcount = 0;
            if (this.HasParameters)
                paramcount = this.Parameters.Count;

            MethodInfo match = null;

            foreach (MethodInfo mi in meths)
            {
                if(this.EnsureSignatureMatchesParameters(mi) != null)
                {
                    if (match != null)
                        throw new InvalidOperationException("2 methods with the same name and signature on type '" + mi.DeclaringType.FullName + "' - not a good place to be");
                    match = mi;
                }
            }

            return match;
        }

        #endregion

        #region private MethodInfo EnsureSignatureMatchesParameters(MethodInfo methodInfo)

        /// <summary>
        /// If the provided method has a signature (parameter name based) that matches the parameters in this 
        /// ObjectCommand it is returned, otherwise null willbe returned
        /// </summary>
        /// <param name="methodInfo">The method to check</param>
        /// <returns>The same method if it is a match to this commands parameters</returns>
        private MethodInfo EnsureSignatureMatchesParameters(MethodInfo methodInfo)
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if(parameters.Length != this.Parameters.Count)
                return null;

            List<string> pnames = new List<string>(this.Parameters.Count);
            foreach (ObjectParameter op in this.Parameters)
            {
                pnames.Add(op.ParameterName);
            }

            foreach (ParameterInfo pi in parameters)
            {
                pnames.Remove(pi.Name);
            }

            //We started with the same number and have removed all the names of the parameters
            //so we know we have a direct match.

            if (pnames.Count == 0)
                return methodInfo;
            else
                return null;
        }

        #endregion
    }
}
