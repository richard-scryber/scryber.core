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
using Scryber;
using Scryber.Drawing;
using System.ComponentModel;

namespace Scryber.Styles
{
    [PDFParsableComponent("Overflow")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OverflowStyle : StyleItemBase
    {

        #region public OverflowAction Action {get;set;} + RemoveAction()

        [PDFAttribute("action")]
        public OverflowAction Action
        {
            get
            {
                OverflowAction act;
                if (this.TryGetValue(StyleKeys.OverflowActionKey, out act))
                    return act;
                else
                    return OverflowAction.None;
            }
            set
            {
                this.SetValue(StyleKeys.OverflowActionKey, value);
            }
        }

        public void RemoveAction()
        {
            this.RemoveValue(StyleKeys.OverflowActionKey);
        }

        #endregion

        #region public OverflowSplit Split {get;set;}

        [PDFAttribute("split")]
        public OverflowSplit Split
        {
            get
            {
                OverflowSplit split;
                if (this.TryGetValue(StyleKeys.OverflowSplitKey, out split))
                    return split;
                else
                    return OverflowSplit.Any;
            }
            set
            {
                this.SetValue(StyleKeys.OverflowSplitKey, value);
            }
        }

        public void RemoveSplit()
        {
            this.RemoveValue(StyleKeys.OverflowSplitKey);
        }

        #endregion


        public OverflowStyle()
            : base(StyleKeys.OverflowItemKey)
        {
        }
    }
}
