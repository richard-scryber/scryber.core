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

namespace Scryber.Styles
{
    public class PDFStyleStack
    {
        

        private List<PDFStyle> _styles;

        public int Count
        {
            get { return this._styles.Count; }
        }

        public PDFStyle Current
        {
            get
            {
                int index = this.Count - 1;
                if (index < 0)
                    throw new InvalidOperationException("Cannot pop the style from an empty stack. The style stack has become unbalanced and has no items in it.");
                return _styles[index];
            }
        }

        public PDFStyleStack(PDFStyle root)
        {
            this._styles = new List<PDFStyle>();
            this._styles.Add(root);
        }

        public void Push(PDFStyle style)
        {
            this._styles.Add(style);
        }

        public PDFStyle Pop()
        {
            int index = this.Count - 1;
            if(index < 0)
                throw new InvalidOperationException("Cannot pop the style from an empty stack. The style stack has become unbalanced and has no items in it.");
            PDFStyle last = this._styles[index];
            this._styles.RemoveAt(index);

            return last;
        }

        /// <summary>
        /// Creates a new style, populates all based upon the current styles
        /// </summary>
        /// <param name="Component"></param>
        /// <returns></returns>
        public PDFStyle GetFullStyle(IPDFComponent Component)
        {
            PDFStyle style = BuildFullStyle(Component);
            return style;
        }

        private PDFStyle BuildFullStyle(IPDFComponent Component)
        {
            PDFStyle style = new PDFStyleFull();

            int last = this._styles.Count - 1;
            if (last >= 0)
            {
                for (int i = 0; i < last; i++)
                {
                    //As these are styles from parents, then any inherited values should be replaced by
                    //explicit values on the last style
                    // so set the style priority to 0

                    this._styles[i].MergeInherited(style, replace:true, priority:0);
                }

                //This will use to the priority of the value itself to be used
                this._styles[last].MergeInto(style);
            }

            style = style.Flatten();
            return style;
        }

        /// <summary>
        /// Create a new clone of the current stack with new references to the styles in the stack (inner styles are not cloned)
        /// </summary>
        /// <returns></returns>
        public PDFStyleStack Clone()
        {
            PDFStyleStack styles = this.MemberwiseClone() as PDFStyleStack;
            styles._styles = new List<PDFStyle>();
            for (int i = 0; i < this._styles.Count; i++)
            {
                styles._styles.Add(this._styles[i]);
            }
            return styles;
        }
    }
}
