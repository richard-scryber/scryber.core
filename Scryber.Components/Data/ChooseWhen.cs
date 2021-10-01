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
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("When")]
    public class ChooseWhen : ChooseTemplateContainer
    {
        private bool _test;

        [PDFAttribute("test", BindingOnly = true)]
        public bool Test
        {
            get { return _test; }
            set
            {
                _test = value;
            }
        }

        object _setvisible = null;

        /// <summary>
        /// The boolean value that dictates if this Choose component will generate one of it's inner content template.
        /// </summary>
        [PDFAttribute("visible")]
        public override bool Visible
        {
            get
            {
                if (null == _setvisible)
                    return base.Visible;
                else
                    return (bool)_setvisible;
            }
            set
            {
                this._setvisible = value;
            }
        }

        /// <summary>
        /// Returns true if this component has had a value set 
        /// </summary>
        public bool HasVisibleValue
        {
            get { return null != _setvisible && _setvisible is Boolean; }
        }

        public ChooseWhen()
            : base(ObjectTypes.DataWhen)
        {
        }

        public virtual bool EvaluateTest(PDFDataContext context)
        {
            /* if (!string.IsNullOrEmpty(_test))
            {
                IPDFDataSource currSource = context.DataStack.Source;
                object currData = context.DataStack.Current;

                this.Visible = this.EvaluateTestExpression(this.Test, currSource, currData, context);
            } */

            if (this.Test && this.Visible)
                return true;
            else
                return false;
        }

        protected virtual bool EvaluateTestExpression(string expr, IPDFDataSource source, object data, PDFDataContext context)
        {
            return source.EvaluateTestExpression(expr, data, context);
        }
    }



    public class PDFChooseWhenList : ComponentWrappingList<ChooseWhen>
    {
        public PDFChooseWhenList(ComponentList inner)
            : base(inner)
        {
        }
    }
}
