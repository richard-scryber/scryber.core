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
using Scryber;
using Scryber.Components;

namespace Scryber.Data
{
    /// <summary>
    /// Abstract base class for template elements that instantiate inner contents
    /// when databinding
    /// </summary>
    public abstract class CodeDomTemplateGenerator : ITemplate
    {
        public IEnumerable<IComponent> Instantiate(int index, IComponent owner)
        {
            //Create the container
            TemplateInstance instance = new TemplateInstance();

            //Init all the custom contents
            this.InitializeComponents(instance);

            //return wrapped in an array
            return new IComponent[] { instance };

        }

        protected abstract void InitializeComponents(TemplateInstance instance);

    }
}
