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
using System.Text;
using Scryber.Logging;

namespace Scryber
{
    /// <summary>
    /// The context class for databinding
    /// </summary>
    public class PDFDataContext : PDFContextBase
    {
        
        #region public PDFDataStack DataStack {get;}

        private PDFDataStack _datastack;
        
        /// <summary>
        /// Gets the current DataStack that holds the objects to bind to.
        /// </summary>
        public PDFDataStack DataStack
        {
            get { return _datastack; }
        }

        #endregion

        #region  public int CurrentIndex {get;set;}

        private int _currindex;

        /// <summary>
        /// Gets or sets the current databinding index
        /// </summary>
        public int CurrentIndex
        {
            get { return _currindex; }
            set { _currindex = value; }
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

        #region public PDFDataContext(PDFItemCollection items, PDFTraceLog log)

        /// <summary>
        /// Creates a new PDFDataContext with the item collection and trace log
        /// </summary>
        /// <param name="items"></param>
        /// <param name="log"></param>
        public PDFDataContext(ItemCollection items, TraceLog log, PerformanceMonitor perfmon, IDocument document)
            : this(items, log, perfmon, new PDFDataStack(), document)
        {
        }

        #endregion

        #region public PDFDataContext(PDFItemCollection items, PDFTraceLog log, PDFDataStack stack)

        /// <summary>
        /// Creates a new PDFDataContext with the item collection, trace log, and data stack
        /// </summary>
        /// <param name="items"></param>
        /// <param name="log"></param>
        /// <param name="stack"></param>
        public PDFDataContext(ItemCollection items, TraceLog log, PerformanceMonitor perfmon, PDFDataStack stack, IDocument document)
            : base(items, log, perfmon, document)
        {
            this._datastack = stack;
        }

        #endregion

    }

    
}
