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
    /// <summary>
    /// A linked list of events handlers for types of events
    /// </summary>
    public sealed class ComponentEventList : IDisposable
    {
        #region ivars

        EventListEntry _head; //First entry
        Scryber.IComponent _owner; //The component that owns the events

        #endregion

        #region public Delegate this[object key] {get;}

        /// <summary>
        /// Gets the registered delegate based on the key if one exists.
        /// </summary>
        /// <param name="key">The object key the event is registered under</param>
        /// <returns>The associated Delegate method or null if one is not registered for the specified object</returns>
        public Delegate this[object key]
        {
            get
            {
                EventListEntry entry = null;
                if ((this._owner != null))
                {
                    entry = this.Find(key);
                }
                if (entry != null)
                {
                    return entry.Handler;
                }
                return null;
            }
        }

        #endregion

        #region public bool IsEmpty {get;}

        /// <summary>
        /// Returns true if this event list has no registered event delegates
        /// </summary>
        public bool IsEmpty
        {
            get { return null == _head; }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFEventList(Scryber.Components.PDFComponent owner)

        /// <summary>
        /// Creates a new instance of the Event List for the specified component
        /// </summary>
        /// <param name="owner"></param>
        public ComponentEventList(Scryber.Components.Component owner)
        {
            _owner = owner;
        }

        #endregion

        //
        // public methods
        //

        #region public void AddHandler(object key, Delegate handler)

        /// <summary>
        /// Adds a delegate to this event list based on the specified key.
        /// </summary>
        /// <param name="key">The key that identifies the handler delegate type</param>
        /// <param name="handler">The delegate to add</param>
        public void AddHandler(object key, Delegate handler)
        {
            EventListEntry exist = this.Find(key);
            if (null == exist)
            {
                this._head = new EventListEntry(handler, key, this._head);
            }
            else
                exist.Handler = Delegate.Combine(exist.Handler, handler);
        }

        #endregion

        #region public void RemoveHandler(object key, Delegate handler)

        /// <summary>
        /// Removes the specified delegate from this list for the specified type
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        public void RemoveHandler(object key, Delegate handler)
        {
            EventListEntry exist = this.Find(key);
            if (null == exist)
            {
                exist.Handler = Delegate.Remove(exist.Handler, handler);
            }
        }

        #endregion

        #region public void Dispose()

        /// <summary>
        /// Disposes of this event list
        /// </summary>
        public void Dispose()
        {
            _head = null;
            _owner = null;
        }

        #endregion

        //
        // private implementation
        //

        #region private EventListEntry Find(object key)

        /// <summary>
        /// looks for an event list entry that matches the specified key
        /// </summary>
        /// <param name="key">The key to use</param>
        /// <returns>The matching entry, or null if no matching entry was found</returns>
        private EventListEntry Find(object key)
        {
            EventListEntry current = this._head;
            while (current != null)
            {
                if (current.Key == key)
                    return current;

                current = current.Next;
            }
            return current;
        }

        #endregion

        //
        // inner classes
        //

        #region private class EventListEntry

        /// <summary>
        /// A linked list entry for a Delegate, the Object key associated with that delegate, and a reference to the next entry in the linked list.
        /// </summary>
        private class EventListEntry
        {
            internal Delegate Handler;
            internal object Key;
            internal EventListEntry Next;

            internal EventListEntry(Delegate handler, object key, EventListEntry next)
            {
                this.Handler = handler;
                this.Key = key;
                this.Next = next;
            }
        }

        #endregion
    }
}
