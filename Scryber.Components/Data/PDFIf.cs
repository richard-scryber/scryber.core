﻿/*  Copyright 2012 PerceiveIT Limited
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
    /// <summary>
    /// Defines a template that will be bound and included
    /// if the test expression returns true or not null
    /// </summary>
    [PDFParsableComponent("If")]
    public class PDFIf : PDFBindingTemplateComponent
    {

        private string _test;

        /// <summary>
        /// Gets or sets the test expression for the if entry
        /// </summary>
        [PDFAttribute("test")]
        public string Test
        {
            get { return _test; }
            set
            {
                _test = value;
            }
        }



        
        object _setvisible = null;

        /// <summary>
        /// The boolean value that dictates if this If component will generate it's inner content template.
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



        #region public virtual IPDFTemplate Template {get;}

        private IPDFTemplate _template;

        /// <summary>
        /// Gets or sets the IPDFTemplate used to instantiate child Components
        /// </summary>
        [PDFTemplate()]
        [PDFElement("Template")]
        [PDFAttribute("template")]
        public virtual IPDFTemplate Template
        {
            get { return _template; }
            set { _template = value; }
        }

        public bool HasTemplate
        {
            get { return null != this.Template; }
        }

        #endregion


        //
        // .ctors
        //

        /// <summary>
        /// Use the NoOp opbject type so that inner content is not generated
        /// </summary>
        public PDFIf()
            : this(PDFObjectTypes.NoOp)
        {
        }

        protected PDFIf(PDFObjectType type)
            : base(type)
        {
        }

        //
        // binding methods
        //


        protected override IPDFTemplate GetTemplateForBinding(PDFDataContext context, int index, int count)
        {
            return this.Template;
        }

        protected override void DoDataBindToContainer(PDFDataContext context, IPDFContainerComponent container)
        {
            if (!string.IsNullOrEmpty(this.Test))
            {
                IPDFDataSource currSource = context.DataStack.Source;
                object currData = context.DataStack.Current;

                //check the result and only call the base method if the return value is true
                this.Visible = this.EvaluateTestExpression(this.Test, currSource, currData, context);   
            }


            if(this.HasVisibleValue && this.Visible)
                base.DoDataBindToContainer(context, container);
            
        }

        protected virtual bool EvaluateTestExpression(string expr, IPDFDataSource source, object data, PDFDataContext context)
        {
            return source.EvaluateTestExpression(expr, data, context);
        }

    }
}