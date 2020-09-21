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

namespace Scryber.Styles
{
    /// <summary>
    /// Abstract base class for all style values. The generic type PDFStyeValue&lt;T&gt; is the concrete implementation for strongly typed values
    /// </summary>
    public abstract class PDFStyleValueBase
    {
        public StyleKey Key { get; set; }

        public int Priority { get; set; }

        protected PDFStyleValueBase(StyleKey key)
        {
            this.Key = key;
        }

        public abstract object GetValue();


        public PDFStyleValueBase CloneWithPriority(int priority)
        {
            var instance = this.MemberwiseClone() as PDFStyleValueBase;
            instance.Priority = priority;
            return instance;
        }

    }



    /// <summary>
    /// Strongly typed style value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StyleValue<T> : PDFStyleValueBase
    {

        public T Value { get; private set; }

        public StyleValue(PDFStyleKey<T> key, T value)
            : base(key)
        {
            this.Value = value;
        }

        public void SetValue(T value)
        {
            this.Value = value;
        }

        public override object GetValue()
        {
            return Value;
        }
    }


}
