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
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Native;

namespace Scryber.PDF
{

    #region public abstract class PDFAction

    /// <summary>
    /// Base class for an action to be taken within a PDF Document
    /// </summary>
    public abstract class PDFAction
    {
        /// <summary>
        /// The component this action is for
        /// </summary>
        public Component Component { get; private set; }

        /// <summary>
        /// The type of action this references
        /// </summary>
        public LinkAction ActionType { get; private set; }

        

        
        /// <summary>
        /// Protected constructor for the base action
        /// </summary>
        /// <param name="component"></param>
        /// <param name="action"></param>
        protected PDFAction(Component component, LinkAction action)
        {
            this.Component = component;
            this.ActionType = action;
        }

        /// <summary>
        /// Inheritors must implment this method to generate their own code to render the link action
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public abstract PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer);

        
    }

    #endregion

    #region public class PDFNamedAction : PDFAction

    /// <summary>
    /// A specific named action known to the PDF reader application - Next Page, Previous page, etc.
    /// </summary>
    public class PDFNamedAction : PDFAction
    {

        /// <summary>
        /// Creates a new known named action with the link action as specified
        /// </summary>
        /// <param name="component">The component that owns this action (in this case can be null)</param>
        /// <param name="action">The action to perform</param>
        public PDFNamedAction(Component component, LinkAction action)
            : base(component, action)
        {
            AssertLinkAction(action);
        }


        /// <summary>
        /// Renders the action data within the current object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Action");
            writer.WriteDictionaryNameEntry("S", "Named");
            string name = this.GetNameForAction(this.ActionType);
            writer.WriteDictionaryNameEntry("N", name);
            writer.EndDictionary();

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Named Action", "Added named action " + name + "for annotation");
            return null;
        }

        /// <summary>
        /// Throws exception if the action is not supported
        /// </summary>
        /// <param name="action"></param>
        private void AssertLinkAction(LinkAction action)
        {
            this.GetNameForAction(action);
        }

        /// <summary>
        /// Gets the known PDF name for the specified action. Only the supported named actions will return a value.
        /// Other action types with throw an ArgumentOutOfRange excpetion
        /// </summary>
        /// <param name="linkAction"></param>
        /// <returns></returns>
        private string GetNameForAction(LinkAction linkAction)
        {
            string name;
            switch (linkAction)
            {
                case LinkAction.Undefined:
                case LinkAction.Uri:
                case LinkAction.Destination:
                case LinkAction.ExternalDestination:
                case LinkAction.Launch:
                    throw RecordAndRaise.ArgumentOutOfRange("linkAction");

                case LinkAction.NextPage:
                    name = "NextPage";
                    break;
                case LinkAction.PrevPage:
                    name = "PrevPage";
                    break;
                case LinkAction.FirstPage:
                    name = "FirstPage";
                    break;
                case LinkAction.LastPage:
                    name = "LastPage";
                    break;
                default:
                    throw RecordAndRaise.ArgumentOutOfRange("linkAction");
            }
            return name;
        }
    }

    #endregion

    #region public class PDFDestinationAction : PDFAction

    /// <summary>
    /// Represents a link GoTo action to a specific LOCAL destination.
    /// </summary>
    public class PDFDestinationAction : PDFAction
    {
        /// <summary>
        /// Gets or sets the destination for this action
        /// </summary>
        public PDFDestination Destination { get; set; }

        /// <summary>
        /// Creates a new destination action.
        /// </summary>
        /// <param name="owner">The owner of this action</param>
        /// <param name="action">The action type to perform - must be LinkAction.Destination</param>
        /// <param name="destination">The destination to navigate to.</param>
        public PDFDestinationAction(Component owner, LinkAction action, PDFDestination destination)
            : base(owner, action)
        {
            this.Destination = destination;
            AssertIsDestination(action);
        }

        /// <summary>
        /// Overrides the base abstract method to write the action dictionary to the current object.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (null == this.Destination)
                throw new NullReferenceException(string.Format("Destination cannot be null for the destination action on component '{0}'", this.Component));

            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Action");
            writer.WriteDictionaryNameEntry("S", "GoTo");

            //The destination should be registered with the Name Dictionary
            //so we use the full name here to refer to it.
            writer.WriteDictionaryStringEntry("D", this.Destination.FullName);
            writer.EndDictionaryEntry();

            writer.EndDictionary();

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Destination Action", "Added destination action " + this.Destination.FullName + "for annotation");
            return null;
        }

        /// <summary>
        /// Makes sure the current action is the required Destination action
        /// </summary>
        /// <param name="action"></param>
        private void AssertIsDestination(LinkAction action)
        {
            if (action != LinkAction.Destination)
                throw new ArgumentOutOfRangeException("action");
        }
    }

    #endregion

    #region public class PDFRemoteDestinationAction : PDFAction

    /// <summary>
    /// Representas a link go to action to a destination in a remote file (rather than in this current file
    /// </summary>
    public class PDFRemoteDestinationAction : PDFAction
    {
        /// <summary>
        /// Gets or sets the path to the file this destination refers to.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the destination name within the file 
        /// </summary>
        public string DestinationName { get; set; }

        /// <summary>
        /// If true the file will be opened by the reader in a new document window (otherwise it will replace the current document window
        /// </summary>
        public bool NewWindow { get; set; }

        /// <summary>
        /// Creates a new remote destination action to link to a destination in another file.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="action">Must be RemoteDestination</param>
        /// <param name="file">The path to the file where this is a link action to</param>
        /// <param name="name">The name of the item in the remote file to link to</param>
        public PDFRemoteDestinationAction(Component owner, LinkAction action, string file, string name)
            : base(owner, action)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file", Errors.NoRemoteDestinationSpecifiedOnLink);
            this.File = file;
            
            this.DestinationName = name;
            AssertIsRemoteDestination(action);
        }

        /// <summary>
        /// Overrides the abstract base method to render the remote destination
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Action");
            writer.WriteDictionaryNameEntry("S", "GoToR");
            writer.WriteDictionaryStringEntry("F", this.File);
            if (!string.IsNullOrEmpty(this.DestinationName))
                writer.WriteDictionaryStringEntry("D", this.DestinationName);
            writer.WriteDictionaryBooleanEntry("NewWindow", this.NewWindow);
            writer.EndDictionary();

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Remote Action", "Added remote destination action to file " + this.File);

            return null;
        }

        /// <summary>
        /// Makes sure the current action is the required Destination action
        /// </summary>
        /// <param name="action"></param>
        private void AssertIsRemoteDestination(LinkAction action)
        {
            if (action != LinkAction.ExternalDestination)
                throw new ArgumentOutOfRangeException("action");
        }
    }

    #endregion

    #region public class PDFUriDestinationAction : PDFAction

    /// <summary>
    /// A link action to a URI that will be launched by the underlying operating system 
    /// </summary>
    public class PDFUriDestinationAction : PDFAction
    {
        /// <summary>
        /// Gets or sets the Url for this action
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Creates a new instance of the Uri destination action
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="action">Must be an ExternalDestination</param>
        /// <param name="url"></param>
        public PDFUriDestinationAction(Component owner, LinkAction action, string url)
            : base(owner, action)
        {
            this.Url = url;
            AssertIsUrlDestination(action);
        }

        /// <summary>
        /// Overrides the abstract base implementation to render the URI action dictionary to current PDFObject stream in the writer. Returns null.
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="writer">The writer to write to</param>
        /// <returns></returns>
        public override PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Action");
            writer.WriteDictionaryNameEntry("S", "URI");
            writer.WriteDictionaryStringEntry("URI", this.Url);
            writer.EndDictionary();

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Uri Action", "Added Uri destination action to location " + this.Url);

            return null;
        }

        /// <summary>
        /// Makes sure the current action is the required ExternalDestination action
        /// </summary>
        /// <param name="action"></param>
        private void AssertIsUrlDestination(LinkAction action)
        {
            if (action != LinkAction.Uri)
                throw new ArgumentOutOfRangeException("action");
        }

    }

    #endregion

    #region public class PDFSubmitFormAction : PDFAction

    /// <summary>
    /// A submit-form action, triggered by a button/input with submit behaviour - matches an
    /// HTML &lt;form&gt;'s own submission, encoding field values as an HTML-style form post/get
    /// (the ExportFormat flag) rather than PDF's legacy FDF format.
    /// </summary>
    public class PDFSubmitFormAction : PDFAction
    {
        private const int ExportFormatFlag = 4;  //submit as HTML form data, not FDF
        private const int GetMethodFlag = 8;     //GET instead of the default POST

        /// <summary>
        /// The URL to submit to.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// "get" or "post" (case-insensitive, matching the HTML method attribute) - anything
        /// else (including unset) submits as post, matching HTML's own default.
        /// </summary>
        public string Method { get; set; }

        public PDFSubmitFormAction(Component owner, string url, string method = null)
            : base(owner, LinkAction.SubmitForm)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));
            this.Url = url;
            this.Method = method;
        }

        public override PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            int flags = ExportFormatFlag;
            if (!string.IsNullOrEmpty(this.Method) && this.Method.Equals("get", StringComparison.OrdinalIgnoreCase))
                flags |= GetMethodFlag;

            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Action");
            writer.WriteDictionaryNameEntry("S", "SubmitForm");

            //The #FDF suffix tells Acrobat to expect and parse an FDF response (Content-Type
            //application/vnd.fdf) rather than trying to render whatever the server sends back
            //as if it were HTML/JSON - without it, Acrobat errors with "Cannot process content
            //of type ..." on anything except a full replacement PDF. The fragment is client-side
            //only (never sent to the server), so it's safe to always add.
            var url = this.Url.Contains("#") ? this.Url : this.Url + "#FDF";

            writer.BeginDictionaryEntry("F");
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Filespec");
            writer.WriteDictionaryStringEntry("F", url);
            writer.WriteDictionaryNameEntry("FS", "URL");
            writer.EndDictionary();
            writer.EndDictionaryEntry();

            writer.WriteDictionaryNumberEntry("Flags", flags);
            writer.EndDictionary();

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Submit Form Action", "Added submit-form action to " + this.Url);

            return null;
        }
    }

    #endregion

    #region public class PDFResetFormAction : PDFAction

    /// <summary>
    /// A reset-form action, triggered by a button/input with reset behaviour - clears every
    /// field back to its default value, matching an HTML &lt;button type="reset"&gt;.
    /// </summary>
    public class PDFResetFormAction : PDFAction
    {
        public PDFResetFormAction(Component owner)
            : base(owner, LinkAction.ResetForm)
        {
        }

        public override PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Action");
            writer.WriteDictionaryNameEntry("S", "ResetForm");
            writer.EndDictionary();

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Reset Form Action", "Added reset-form action for " + this.Component.UniqueID);

            return null;
        }
    }

    #endregion
}
