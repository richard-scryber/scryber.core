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
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("Choose")]
    public class Choose :  BindingTemplateComponent
    {

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
        

        private PDFChooseWhenList _whens;

        [PDFArray(typeof(ChooseWhen))]
        [PDFElement("")]
        public PDFChooseWhenList Whens
        {
            get
            {
                if (null == _whens)
                    _whens = new PDFChooseWhenList(this.InnerContent);
                return _whens;
            }
        }

        private ChooseOtherwise _otherwise;

        [PDFElement("Otherwise")]
        public ChooseOtherwise Otherwise
        {
            get { return _otherwise; }
            set 
            {
                if (null != _otherwise && _otherwise.Parent == this)
                    _otherwise.Parent = null;

                _otherwise = value;

                if (null != _otherwise && _otherwise.Parent != this)
                    _otherwise.Parent = this;
            }
        }

        public Choose()
            : this(PDFObjectTypes.NoOp)
        {
        }

        public Choose(PDFObjectType type)
            : base(type)
        {
        }


        

        protected override IPDFTemplate GetTemplateForBinding(PDFDataContext context, int index, int count)
        {

            //If we are not visible then don't do anything and return null

            if (this.Visible == false)
                return null;

            IPDFTemplate tempate = null;
            bool found = false;

            foreach (ChooseWhen where in this.Whens)
            {
                if (where.EvaluateTest(context))
                {
                    if (where.Template != null)
                    {
                        tempate = where.Template;
                    }
                    found = true;
                    break;
                }
            }
            //If we have a template and should be binding on it
            if (!found && this.Otherwise != null && this.Otherwise.Template != null)
            {
                tempate =  Otherwise.Template;
            }

            return tempate;
        }
    }
}
