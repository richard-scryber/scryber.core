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
using System.Linq;
using System.Text;

namespace Scryber.Components
{
    public class RawTemplate :  BindingTemplateComponent
    {

        public RawTemplate()
            : base((ObjectType)"RawT")
        {
        }

        private ITemplate _value = null;
        /// <summary>
        /// The actual content of the template to convert to components.
        /// </summary>
        [PDFAttribute("value")]
        [PDFElement("")]
        public ITemplate Template
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        protected override ITemplate GetTemplateForBinding(DataContext context, int index, int count)
        {
            return _value;
        }
    }
}
