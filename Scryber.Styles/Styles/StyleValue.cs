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
using System.Threading.Tasks;
using Scryber.Drawing;

namespace Scryber.Styles
{
    /// <summary>
    /// Abstract base class for all style values. The generic type PDFStyeValue&lt;T&gt; is the concrete implementation for strongly typed values
    /// </summary>
    public abstract class StyleValueBase
    {
        public StyleKey Key { get; set; }

        public int Priority { get; set; }
        
        //TODO: Implement the initial and inherit options.
        
        public bool IsInitial { get; set; }
        
        public bool IsInherit { get; set; }

        protected StyleValueBase(StyleKey key)
        {
            this.Key = key;
        }

        public abstract object GetValue(Style forStyle);


        public StyleValueBase CloneWithPriority(int priority)
        {
            var instance = this.MemberwiseClone() as StyleValueBase;
            instance.Priority = priority;
            return instance;
        }

        public virtual void FlattenValue(StyleKey key, Style forStyle, Size page, Size container, Size font, Unit rootFont )
        {
            if (key.CanBeRelative)
                key.FlattenValue(forStyle, page, container, font, rootFont);
            //Base Implementation does nothing
        }
    }



    /// <summary>
    /// Strongly typed style value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StyleValue<T> : StyleValueBase
    {

        private T _value;

        public StyleValue(StyleKey<T> key, T value)
            : base(key)
        {
            this._value = value;
        }

        public void SetValue(T value)
        {
            this._value = value;
        }

        public virtual T Value(StyleBase forStyle)
        {
            return _value;
        }

        public override object GetValue(Style forStyle)
        {
            object result = Value(forStyle);
            
            return result;
        }
    }


}
