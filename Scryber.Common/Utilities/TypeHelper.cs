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

namespace Scryber.Utilities
{
    public static class TypeHelper
    {

        public static DataType GetDataTypeFor(System.Type type)
        {
            DataType converted;

            if (type == null)
                return DataType.Unknown;
            else
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.DBNull:
                    case TypeCode.Empty:
                        converted = DataType.Unknown;
                        break;
                    case TypeCode.Object:

                        converted = GetOjectType(type);
                        break;

                    case TypeCode.Boolean:
                        converted = DataType.Boolean;
                        break;
                    case TypeCode.Char:
                        converted = DataType.String;
                        break;
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.Int16:
                    case TypeCode.Byte:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                        converted = DataType.Integer;
                        break;

                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        converted = DataType.Double;
                        break;
                    case TypeCode.DateTime:
                        converted = DataType.DateTime;
                        break;
                    case TypeCode.String:
                        converted = DataType.String;
                        break;
                    default:
                        converted = GetOjectType(type);
                        break;
                }
            }
            return converted;
        }

        private static DataType GetOjectType(Type type)
        {

            if (type == typeof(Guid))
                return DataType.Guid;
            else if (type == typeof(Uri))
                return DataType.Url;
            else if (typeof(System.Security.Principal.IIdentity).IsAssignableFrom(type))
                return DataType.User;
            else if (type.IsEnum)
                return DataType.Choice;
            else if (type == typeof(byte[]))
                return DataType.BinaryFile;
            else if (typeof(System.Collections.ICollection).IsAssignableFrom(type))
                return DataType.Array;
            else
                return DataType.Custom;
            
        }

        /// <summary>
        /// Gets the fully qualified namespace and asembly name for a runtime type.
        /// </summary>
        /// <param name="fortype"></param>
        /// <returns></returns>
        public static string GetNamespaceAndAssemblyName(Type fortype)
        {
            string ns = fortype.Namespace;
            string assm = fortype.Assembly.FullName;
            return GetNamespaceAndAssemblyName(ns, assm);
        }

        /// <summary>
        /// Gets the formatted string for a Namspace and Assembly
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="assm"></param>
        /// <returns></returns>
        public static string GetNamespaceAndAssemblyName(string ns, string assm)
        {
            if (string.IsNullOrEmpty(ns))
                throw new ArgumentNullException("ns");
            if(string.IsNullOrEmpty(assm))
                throw new ArgumentNullException("assm");

            return string.Format("{0}, {1}", ns, assm);
        }

        private static char AssemblySeparator = ',';

        public static Type GetType(string typename, string assembly = "", bool throwOnNotFound = false)
        {
            if (string.IsNullOrEmpty(typename))
                throw new ArgumentNullException("typename");

            if(string.IsNullOrEmpty(assembly) && typename.IndexOf(AssemblySeparator) > 0)
            {
                int index = typename.IndexOf(AssemblySeparator);
                assembly = typename.Substring(index + 1).Trim();
                typename = typename.Substring(0, index).Trim();
            }

            if(string.IsNullOrEmpty(assembly))
            {
                System.Reflection.Assembly[] loaded = AppDomain.CurrentDomain.GetAssemblies();
                foreach (System.Reflection.Assembly assm in loaded)
                {
                    Type found = assm.GetType(typename);
                    if (null != found)
                        return found;
                }

                if (throwOnNotFound)
                    throw new NullReferenceException("The type with name '" + typename + "' could not be found in any of the loaded assemblies");

                return null;
            }
            else
            {
                System.Reflection.Assembly assm = System.Reflection.Assembly.Load(assembly);
                if (null == assm)
                    throw new NullReferenceException("An assembly with the name '" + assembly + "' could not be loaded");
                Type found = assm.GetType(typename);

                if(null == found && throwOnNotFound)
                    throw new NullReferenceException("The type with name '" + typename + "' could not be found in the assembly '" + assembly + "'");

                return found;
            }
        }
    }
}
