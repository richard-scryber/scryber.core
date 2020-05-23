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
using System.Reflection;

namespace Scryber.Generation
{
    public class ParserEventDefinition
    {
        private EventInfo _event;
        private string _name;

        internal EventInfo Event
        {
            get { return _event; }
        }

        internal Type DelegateMethodType
        {
            get { return _event.EventHandlerType; }
        }

        internal string Name
        {
            get { return _name; }
        }


        internal ParserEventDefinition(string name, EventInfo evt)
        {
            if (null == evt)
                throw new ArgumentNullException("evt");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this._event = evt;
            this._name = name;
        }

    }

    public class ParserEventDefinitionCollection : System.Collections.ObjectModel.KeyedCollection<string, ParserEventDefinition>
    {
        protected override string GetKeyForItem(ParserEventDefinition item)
        {
            return item.Name;
        }

        internal bool TryGetPropertyDefinition(string name, out ParserEventDefinition defn)
        {
            if (this.Count == 0)
            {
                defn = null;
                return false;
            }
            else
                return this.Dictionary.TryGetValue(name, out defn);
        }
    }
}
