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

namespace Scryber
{

    #region public interface IPDFStyledComponent : IPDFComponent

    /// <summary>
    /// Defines an interface for Components that support the application of state attributes and
    /// can also get the full style defined in the Components document for their type, id, and class
    /// </summary>
    public interface IPDFStyledComponent : IPDFComponent
    {
        /// <summary>
        /// Gets or sets the class name of styles to apply
        /// </summary>
        string StyleClass { get; set; }

        /// <summary>
        /// Gets the explicit style of the component
        /// </summary>
        Styles.PDFStyle Style { get; }

        /// <summary>
        /// Return true if this StyledComponent has a defined style
        /// </summary>
        bool HasStyle { get; }

        /// <summary>
        /// Gets the style applied to this component
        /// </summary>
        /// <returns></returns>
        Styles.PDFStyle GetAppliedStyle();
    }

    #endregion
}
