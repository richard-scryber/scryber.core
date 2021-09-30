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
using Scryber.Styles;

namespace Scryber
{
    /// <summary>
    /// Manages all the list numbering references in a PDF Document
    /// </summary>
    public class DocumentListNumbering
    {

        #region ivars

        //list of all the known groups and stacks that hve been created for the document
        private PDFListNumberingStackCollection _listStacks = new PDFListNumberingStackCollection();

        //the current stack that is being used to enumerate in at the moment.
        private PDFListNumberingStack _currentStack = null;

        #endregion

        //
        // properties
        //

        #region public bool HasNumberStack {get;}

        /// <summary>
        /// Returns true if we have a current numbering stack to build on.
        /// </summary>
        public bool HasNumberStack
        {
            get { return _currentStack != null; }
        }

        #endregion

        #region public PDFListNumberingStack CurrentNumberStack {get;}

        /// <summary>
        /// Gets the current numbering stack
        /// </summary>
        public PDFListNumberingStack CurrentNumberStack
        {
            get { return _currentStack; }
        }

        #endregion

        //
        // ctor
        //

        #region public PDFDocumentListNumbering()

        /// <summary>
        /// Creates a new instance of the list numbering for a document
        /// </summary>
        public DocumentListNumbering()
        {
        }

        #endregion

        //
        // public methods
        //

        #region public PDFListNumberingStack OpenNumberingStack(string name)

        /// <summary>
        /// Opens an existing stack if the names match, or begins a new stack (setting it as current)
        /// </summary>
        /// <param name="name">The name of the stack to open. If the name is not null or empty</param>
        /// <returns>The appropriate numbering stack.</returns>
        /// <remarks>Named stacks are remembered throughout the lisf of the document, so they can be re-opened at any point.
        /// Un-named stacks will only persist until they are completely colsed.</remarks>
        public PDFListNumberingStack OpenNumberingStack(string name, ListStyle initStyle)
        {
            //if we have a current stack and either both names are empty, or the names match - then this is the one.
            if (_currentStack != null && ((string.IsNullOrEmpty(name) && string.IsNullOrEmpty(_currentStack.Name)) || string.Equals(name, _currentStack.Name)))
                return _currentStack;

            //otherwise if we do have a name, then lets chack and see if we have a reference.
            else if (_listStacks.Count > 0 && !string.IsNullOrEmpty(name))
            {
                foreach (PDFListNumberingStack stack in _listStacks)
                {
                    if (string.Equals(name, stack.Name))
                    {
                        _currentStack = stack;
                        return stack;
                    }
                }
            }

            //we don't have an existing stack so if we are not a group then we want to be pushed onto the current stack (if there is one)
            PDFListNumberingStack newStack;
            if (null != _currentStack && string.IsNullOrEmpty(name))
                newStack = _currentStack;
            else //otherwise just create one.
            {
                newStack = new PDFListNumberingStack(name);

                //we only remember named stacks - so we can refer back to them.
                if (!string.IsNullOrEmpty(name))
                    _listStacks.Add(newStack);
            }
            ListNumberingGroupStyle type = initStyle.NumberingStyle;

            //TODO: Add pre, post and concat to style
            string prefix = initStyle.NumberPrefix;
            string postfix = initStyle.NumberPostfix;
            bool concatenate = initStyle.ConcatenateWithParent;

            newStack.Push(type, prefix, postfix, concatenate);

            _currentStack = newStack;
            return _currentStack;
        }

        #endregion

        #region public void CloseNumberingStack()

        /// <summary>
        /// Closes the current stack, so it is no longer current. 
        /// It is still retained in the list, so can be joined again, if the name of the stack is not empty.
        /// </summary>
        public void CloseNumberingStack()
        {
            if (null == _currentStack)
                throw new NullReferenceException("Current numbering stack is empty");

            //We pop the last one if there is more than one current list stack
            if (_currentStack.Count > 1)
                _currentStack.Pop();
            else
                _currentStack = null;
        }

        #endregion
    }
}
