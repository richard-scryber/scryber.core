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
using System.Xml;
using Scryber.Drawing;
using Scryber.PDF;

namespace Scryber
{

    /// <summary>
    /// Deletegate for the a Components Rendered Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void RenderEventHandler(object sender, RenderEventArgs args);

    /// <summary>
    /// Event args for a components Initialized event
    /// </summary>
    public class RenderEventArgs : EventArgs
    {
        public RenderContext Context { get; private set; }

        public RenderEventArgs(RenderContext context)
        {
            this.Context = context;
        }
    }

    /// <summary>
    /// Delegate for a components Pre and Post Layout Events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void LayoutEventHandler(object sender, LayoutEventArgs args);

    /// <summary>
    /// Event args for a components Pre and Post Layout events
    /// </summary>
    public class LayoutEventArgs: EventArgs
    {
        public LayoutContext Context { get; private set; }

        public LayoutEventArgs(LayoutContext context)
        {
            this.Context = context;
        }

        
    }


    public delegate void TemplateItemDataBoundHandler(object sender, TemplateItemDataBoundArgs args);


    /// <summary>
    /// Event args class that has a reference to the item that was generated and bound.
    /// And the current data context for binding.
    /// </summary>
    public class TemplateItemDataBoundArgs : EventArgs
    {
        private IComponent _item;
        private DataContext _context;

        public IComponent Item
        {
            get { return _item; }
        }

        public DataContext Context
        {
            get { return _context; }
        }

        public TemplateItemDataBoundArgs(IComponent item, DataContext context)
        {
            this._item = item;
            this._context = context;
        }
    
    }

    /// <summary>
    /// Delegate for a component registration - explicit component rather than an event args class - could be used a lot
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="registered"></param>
    public delegate void ComponentRegisteredHandler(object sender, IComponent registered);



    /// <summary>
    /// Event that is raised when a remote file request is made on the document, with the associated request in the arguments.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <remarks>If the handler wants to make the request outside of the engine,
    /// then it should set the IsExecuting flag before returning back to the raiser.</remarks>
    public delegate void RemoteFileRequestEventHandler(object sender, RemoteFileRequestEventArgs args);


    /// <summary>
    /// Arguments for a remote file request
    /// </summary>
    public class RemoteFileRequestEventArgs : EventArgs
    {
        public RemoteFileRequest Request { get; private set; }

        public RemoteFileRequestEventArgs(RemoteFileRequest request)
        {
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
        }
    }

    
}
