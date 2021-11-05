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
    /// Deletegate for the a Components Load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void LoadedEventHandler(object sender, LoadEventArgs args);

    /// <summary>
    /// Event args for a components Load event
    /// </summary>
    public class LoadEventArgs : EventArgs
    {
        public LoadContext Context { get; private set; }

        public LoadEventArgs(LoadContext context)
        {
            this.Context = context;
        }
    }

    /// <summary>
    /// Deletegate for the a Components Initialized Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void InitializedEventHandler(object sender, InitEventArgs args);

    /// <summary>
    /// Event args for a components Initialized event
    /// </summary>
    public class InitEventArgs : EventArgs
    {
        public InitContext Context { get; private set; }

        public InitEventArgs(InitContext context)
        {
            this.Context = context;
        }
    }



    /// <summary>
    /// Event handler for the databind event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void DataBindEventHandler(object sender, DataBindEventArgs e);

    /// <summary>
    /// Arguments for the PDFDataBindEventHandler
    /// </summary>
    public class DataBindEventArgs : EventArgs
    {
        /// <summary>
        /// instance storage of PFDataContext
        /// </summary>
        private DataContext _context;

        /// <summary>
        /// Gets the context associated with the current databind operation
        /// </summary>
        public DataContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Creates a new instance of the PDFDataBindEventArgs
        /// </summary>
        /// <param name="context"></param>
        public DataBindEventArgs(DataContext context)
        {
            this._context = context;
        }
    }


    /// <summary>
    /// Delegate to convert a string value to the required type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="requiredType"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    public delegate object ValueConverter(string value, Type requiredType, IFormatProvider formatProvider);


    
    /// <summary>
    /// A callback method that is raised from an IResourceRequester to load content from a remote resource.
    /// </summary>
    /// <param name="raiser">The component registered as the raiser of the request</param>
    /// <param name="request">The details of the request, including any result if completed</param>
    /// <param name="response">The response that was returned from a remote request if initiated</param>
    public delegate bool RemoteRequestCallback(IComponent raiser, IRemoteRequest request, System.IO.Stream response);
    

}
