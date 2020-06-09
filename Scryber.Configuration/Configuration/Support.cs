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
using System.Configuration;

namespace Scryber.Configuration
{
    [Obsolete("Use the IScryberCOnfigurationSerive, from the ServiceProvider", true)]
    public class Support
    {


        /// <summary>
        /// Attempts to retrieve a type for a specific full name (typename, assemby)
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        [Obsolete("Use the IScryberCOnfigurationSerive, from the ServiceProvider", true)]
        public static Type GetTypeFromName(string fullname)
        {
            Type type = null;

            try
            {               
                int index = fullname.IndexOf(',');
                if (index > 0)
                {
                    string typename = fullname.Substring(0, index);
                    string assmname = fullname.Substring(index + 1);
                    System.Reflection.Assembly assm = System.Reflection.Assembly.Load(assmname);
                    if (null == assm)
                        throw new NullReferenceException("No assembly could be loaded with the name '" + assmname + "'");

                    type = assm.GetType(typename, true, true);
                }
                else
                    type = Type.GetType(fullname, true, true);
                
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Could not create an instance of the type '" + fullname + "'", ex);
            }
            
            return type;
        }


        [Obsolete("Use the IScryberCOnfigurationSerive, from the ServiceProvider", true)]
        public static T GetInstanceFromTypeName<T>(string typename)
        {
            Type type = GetTypeFromName(typename);
            object activated;

            try
            {
                activated = Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Could not create an instance of the type '" + type.Name + "'. " + ex.Message, ex);
            }

            if (activated is T)
                return (T)activated;
            else
                throw new ConfigurationErrorsException("The instance of type '" + activated.GetType().Name + "' could not be converted or cast to a '" + typeof(T).Name + "'");

        }
    }
}
