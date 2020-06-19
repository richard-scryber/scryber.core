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
using System.Runtime.InteropServices;

namespace Scryber.Utilities
{
    /// <summary>
    /// Provides the basis for retrieving the current framework version (Based on the assembly file version)
    /// </summary>
    public static class FrameworkHelper
    {

        private static Version _pdfversion = GetFrameworkVersion();

        /// <summary>
        /// Returns the current scryber PDF framework version based on the assembly file version attribute
        /// </summary>
        public static Version CurrentVersion
        {
            get { return _pdfversion; }
        }

        private static Version GetFrameworkVersion()
        {
            Type me = typeof(FrameworkHelper);
            Assembly assm = me.Assembly;
            object[] all = assm.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (all != null && all.Length > 0)
            {
                AssemblyFileVersionAttribute vers = (AssemblyFileVersionAttribute)all[0];
                string s = vers.Version;
                if (!string.IsNullOrEmpty(s))
                {
                    return new Version(s);
                }
            }

            return new Version(0, 8, 0);
        }

        public static bool IsWindows() =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}
