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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Scryber.Logging;

namespace Scryber
{
    /// <summary>
    /// The context class for databinding
    /// </summary>
    public class DataContext : ContextBase
    {
        
        #region public DataStack DataStack {get;}

        private DataStack _datastack;
        
        /// <summary>
        /// Gets the current DataStack that holds the objects to bind to.
        /// </summary>
        public DataStack DataStack
        {
            get { return _datastack; }
        }

        #endregion

        #region  public int CurrentIndex {get;set;}

        private int _currindex;

        /// <summary>
        /// Gets or sets the current data-binding index
        /// </summary>
        public int CurrentIndex
        {
            get { return _currindex; }
            set { _currindex = value; }
        }

        #endregion

        #region public string CurrentKey {get; set;}
        
        private string _currentKey;

        /// <summary>
        /// Gets or sets the current data-binding key (or object property) name
        /// </summary>
        public string CurrentKey
        {
            get { return this._currentKey; }
            set { this._currentKey = value; }
        }
        
        #endregion

        #region public System.Xml.Xsl.XsltContext NamespaceResolver {get;set;}

        private System.Xml.IXmlNamespaceResolver _xslt;

        /// <summary>
        /// Gets or setsthe XSLT Context for this data context. 
        /// The XsltContext can be used to provide custom functions and values duing processing
        /// </summary>
        public System.Xml.IXmlNamespaceResolver NamespaceResolver
        {
            get { return _xslt; }
            set { _xslt = value; }
        }

        #endregion

        //
        // .ctors
        //

        #region public DataContext(PDFItemCollection items, PDFTraceLog log)

        /// <summary>
        /// Creates a new PDFDataContext with the item collection and trace log
        /// </summary>
        /// <param name="items"></param>
        /// <param name="log"></param>
        /// <param name="perfmon"></param>
        public DataContext(ItemCollection items, TraceLog log, PerformanceMonitor perfmon, IDocument document, OutputFormat format)
            : this(items, log, perfmon, new DataStack(), document, format)
        {
        }

        #endregion

        #region public DataContext(ItemCollection items, TraceLog log, DataStack stack)

        /// <summary>
        /// Creates a new PDFDataContext with the item collection, trace log, and data stack
        /// </summary>
        /// <param name="items"></param>
        /// <param name="log"></param>
        /// <param name="stack"></param>
        public DataContext(ItemCollection items, TraceLog log, PerformanceMonitor perfmon, DataStack stack, IDocument document, OutputFormat format)
            : base(items, log, perfmon, document, format)
        {
            this._datastack = stack;
        }

        #endregion

    }

    
}
