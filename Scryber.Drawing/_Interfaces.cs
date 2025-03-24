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
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.PDF.Native;
using Scryber.Svg;

namespace Scryber
{

    /// <summary>
    /// ImageDataFactory's load and provide image data based on a specific path. 
    /// They are registered in the configuration file in the Imaging element of the scryber group
    /// </summary>
    public interface IPDFImageDataFactory
    {

        /// <summary>
        /// Return true if any data retrieved from the path should be cached.
        /// </summary>
        bool ShouldCache { get; }

        /// <summary>
        /// Load the image data from a specific source and return it.
        /// </summary>
        /// <param name="document">The root level document</param>
        /// <param name="path">The specified path to the source</param>
        /// <returns>The loaded image data</returns>
        ImageData LoadImageData(IDocument document, IComponent owner, string path);
    }

    /// <summary>
    /// Defines the base capability of a Path Adorner that add graphic features to an individual path.
    /// </summary>
    public interface IPathAdorner
    {
        /// <summary>
        /// Gets the ID of the path adornment
        /// </summary>
        string ID { get; }
        
        AdornmentOrientationValue Orientation { get; }
        PDFName OutputAdornment(PDFGraphics toGraphics, PathAdornmentInfo info, ContextBase context);
    }

    
}
