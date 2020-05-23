using System;
using System.Collections.Generic;
using System.Linq;
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

using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Scryber;
using Scryber.Drawing;

namespace Scryber.Styles
{
    [PDFParsableComponent("Shape")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFShapeStyle : PDFStyleItemBase
    {

        #region public int VertexCount {get;set;} + RemoveVertexCount()

        [PDFAttribute("vertex-count")]
        public int VertexCount
        {
            get
            {
                int f;
                if (this.TryGetValue(PDFStyleKeys.ShapeVertexCountKey, out f))
                    return f;
                else
                    return 4;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ShapeVertexCountKey, value);
            }
        }

        public void RemoveVertexCount()
        {
            this.RemoveValue(PDFStyleKeys.ShapeVertexCountKey);
        }

        #endregion

        #region public int VertexStep {get;set;} + RemoveVertexStep()

        [PDFAttribute("vertex-step")]
        public int VertexStep
        {
            get
            {
                int f;
                if (this.TryGetValue(PDFStyleKeys.ShapeVertexStepKey, out f))
                    return f;
                else
                    return 1;

            }
            set
            {
                this.SetValue(PDFStyleKeys.ShapeVertexStepKey, value);
            }
        }

        public void RemoveVertexStep()
        {
            this.RemoveValue(PDFStyleKeys.ShapeVertexStepKey);
        }

        #endregion

        #region public bool Closed {get;set;} + RemoveClosed()

        [PDFAttribute("closed")]
        public bool Closed
        {
            get
            {
                bool f;
                if (this.TryGetValue(PDFStyleKeys.ShapeClosedKey, out f))
                    return f;
                else
                    return true;

            }
            set
            {
                this.SetValue(PDFStyleKeys.ShapeClosedKey, value);
            }
        }

        public void RemoveClosed()
        {
            this.RemoveValue(PDFStyleKeys.ShapeClosedKey);
        }

        #endregion

        #region public double Rotation {get;set;} + RemoveRotation()

        [PDFAttribute("rotate")]
        public double Rotation
        {
            get
            {
                double f;
                if (this.TryGetValue(PDFStyleKeys.ShapeRotationKey, out f))
                    return f;
                else
                    return 0.0;

            }
            set
            {
                this.SetValue(PDFStyleKeys.ShapeRotationKey, value);
            }
        }

        public void RemoveRotation()
        {
            this.RemoveValue(PDFStyleKeys.ShapeRotationKey);
        }

        #endregion

        //
        // .ctor
        //

        public PDFShapeStyle()
            : base(PDFStyleKeys.ShapeItemKey)
        {
        }
    }
}
