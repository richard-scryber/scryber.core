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
using System.Diagnostics;
using System.Text;
using Scryber.Styles;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.Components
{
    [PDFParsableComponent("Link")]
    public class Link : SpanBase
    {

        public const string LinkAnnotationChildEntries = "LinkChildren";
        public const string LinkArtefactName = "Link";
        public const string LinkEmbeddedEntryName = "FileObject";
        public const string LinkEmbeddedFileChildEntryName = "FileObjectChildren";
        public const string ComponentIDPrefix = "#";

        public Link()
            : this(ObjectTypes.Link)
        {
        }

        public Link(ObjectType type)
            : base(type)
        {

        }

        #region public PDFVisualComponentList Contents {get;}

        

        /// <summary>
        /// Gets the content collection of page Components in this panel
        /// </summary>
        [PDFArray(typeof(Component))]
        [PDFElement("")]
        public override ComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        

        #endregion


        #region public LinkAction Action {get;set;}

        private LinkAction _action = LinkAction.Undefined;
        /// <summary>
        /// Gets or sets the action type for this link. 
        /// If left undefined then the value will be (attempted to be) determined.
        /// </summary>
        [PDFAttribute("action")]
        public LinkAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        #endregion

        #region public string Destination {get;set;}

        private string _dest;

        /// <summary>
        /// Gets or sets the destination name or component (prefix with # to look for a component with the specidied ID, otherwise
        /// use the components name or unique id).
        /// </summary>
        [PDFAttribute("destination")]
        public virtual string Destination
        {
            get { return _dest; }
            set { _dest = value; }
        }

        #endregion

        #region public string File {get;set;}

        private string _file;

        /// <summary>
        /// Gets or sets the path to the remote file
        /// </summary>
        [PDFAttribute("file")]
        public virtual string File
        {
            get { return _file; }
            set { _file = value; }
        }

        #endregion

        #region public OutlineFit DestinationFit {get;set;}

        private OutlineFit _destfit = OutlineFit.PageWidth;
        /// <summary>
        /// Gets or sets the fit for the destination (only for local links)
        /// </summary>
        [PDFAttribute("destination-fit")]
        public virtual OutlineFit DestinationFit
        {
            get { return _destfit; }
            set { _destfit = value; }
        }

        #endregion

        #region public bool NewWindow {get;set;}

        private bool _newWindow;

        [PDFAttribute("new-window")]
        public bool NewWindow
        {
            get { return _newWindow; }
            set { _newWindow = value; }
        }

        #endregion


        #region public string AlternateText {get;set;}

        private string _alt;

        [PDFAttribute("alt")]
        public string AlternateText
        {
            get { return this._alt; }
            set { this._alt = value; }
        }

        #endregion


        protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Style fullstyle)
        {
            base.DoRegisterArtefacts(context, set, fullstyle);

            LinkAction actiontype = this.Action;

            if (actiontype == LinkAction.Undefined)
                actiontype = this.ResolveActionType(this.Destination, this.File);

            object[] entries = null;
            PDFAction action = null;
            PDFEmbeddedAttachment attachment = null;
            IconAttachment icon = null;
            
            if (this.IsNamedAction(actiontype))
            {
                PDFNamedAction named = new PDFNamedAction(this, this.Action);
                action = named;
            }
            else if (actiontype == LinkAction.Uri)
            {
                string url = this.File;
                if (!string.IsNullOrEmpty(url))
                    action = new PDFUriDestinationAction(this, actiontype, url);
                else
                    throw new PDFLinkException(Errors.NoFileLinkSpecifiedOnUriAction);
            }
            else if (actiontype == LinkAction.Destination)
            {
                //If we start with a # then we are an ID, otherwise we are a uniqueID or Name
                PDFDestination dest = null;
                Component comp;
                if (string.IsNullOrEmpty(this.Destination))
                {
                    //if there is no destination then there is no link. So do not register
                    comp = null;
                    return;
                }
                else if (this.Destination.StartsWith(ComponentIDPrefix))
                {
                    string id = this.Destination.Substring(ComponentIDPrefix.Length);
                    comp = this.Document.FindAComponentById(id);
                }
                else
                {
                    comp = this.Document.FindAComponentByName(this.Destination);
                }


                if (null == comp)
                {
                    if (context.Conformance == ParserConformanceMode.Strict)
                        throw new PDFLayoutException(string.Format(Errors.LinkToDestinationCouldNotBeMade,
                            this.Destination));
                    else
                        context.TraceLog.Add(TraceLevel.Error, "PDFLink",
                            string.Format(Errors.LinkToDestinationCouldNotBeMade, this.Destination));

                    //cannot continue as we dont have anything to link to
                    return;
                }
                else if (comp is IconAttachment)
                {
                    icon = (IconAttachment)comp;
                    attachment = icon.GetAttachment(context);
                    
                    
                }
                else
                {

                    dest = new PDFDestination(comp, this.DestinationFit, this.UniqueID);

                    PDFDestinationAction destact = new PDFDestinationAction(this, actiontype, dest);
                    action = destact;

                    object link =
                        context.DocumentLayout.RegisterCatalogEntry(context, PDFArtefactTypes.Names, dest);
                    set.SetArtefact(LinkArtefactName, link);

                }
                
            }
            else if (actiontype == LinkAction.ExternalDestination)
            {
                string name = this.Destination;
                string file = this.File;
                PDFRemoteDestinationAction remote = new PDFRemoteDestinationAction(this, actiontype, file, name);
                remote.NewWindow = this.NewWindow;

                action = remote;
            }
            else if (context.Conformance == ParserConformanceMode.Lax)
            {
                context.TraceLog.Add(TraceLevel.Warning, "Link",
                    "No action type could be determined on the link " + this.UniqueID);
                action = null;
            }
            else
                throw RecordAndRaise.Argument("actiontype");
            
            if (null != action)
            {
                entries = this.AddActionAnnotationToChildren(context, fullstyle, action);
                set.SetArtefact(LinkAnnotationChildEntries, entries);
            }
            else if (null != attachment)
            {
                entries = this.AddAttachmentAnnotationToChildren(context, fullstyle, attachment, icon);
                set.SetArtefact(LinkEmbeddedFileChildEntryName, entries);
            }

        }

        private object[] AddActionAnnotationToChildren(PDFLayoutContext context, Styles.Style style, PDFAction action)
        {
            List<object> entries = new List<object>();
            PDFLayoutPage pg = context.DocumentLayout.CurrentPage;

            FillActionAnnotations(context, style, action, entries, pg, this.Contents);
            return entries.ToArray();
        }

        private void FillActionAnnotations(PDFLayoutContext context, Styles.Style style,
                                           PDFAction action, List<object> entries, 
                                           PDFLayoutPage pg, ComponentList contents)
        {
            
            foreach (Component comp in contents)
            {
                PDFAnnotationLinkEntry annot;
                ComponentList inner;
                if (IsContainer(comp, out inner))
                {
                    FillActionAnnotations(context, style, action, entries, pg, inner);
                }
                else if (comp.Type == ObjectTypes.Whitespace)
                {
                    //Skip
                }
                else
                {
                    annot = new PDFAnnotationLinkEntry(comp, style);
                    annot.Action = action;

                    if (!string.IsNullOrEmpty(this.AlternateText))
                        annot.AlternateText = this.AlternateText;

                    object entry = pg.RegisterPageEntry(context, PDFArtefactTypes.Annotations, annot);

                    if (null != entry)
                        entries.Add(entry);
                }
            }
        }

        protected bool IsContainer(Component comp, out ComponentList list)
        {
            if (comp is Panel)
            {
                Panel panel = comp as Panel;
                if (panel.HasContent)
                {
                    list = panel.Contents;
                    return true;
                }
            }
            else if (comp is IInvisibleContainer)
            {
                IInvisibleContainer container = comp as IInvisibleContainer;
                if (container.HasContent)
                {
                    list = container.Content;
                    return true;
                }
            }

            list = null;
            return false;
        }

        private object[] AddAttachmentAnnotationToChildren(PDFLayoutContext context, Styles.Style style,
            PDFEmbeddedAttachment attach, IconAttachment icon)
        {
            PDFAttachmentAnnotationEntry entry =
                new PDFAttachmentAnnotationEntry(icon, this, attach, style);
            
            List<object> entries = new List<object>();
            PDFLayoutPage pg = context.DocumentLayout.CurrentPage;
            FillAttachmentAnnotations(context, style, attach, entries, pg, icon, this.Contents);
            
            return entries.ToArray();
        }
        
        private void FillAttachmentAnnotations(PDFLayoutContext context, Styles.Style style,
            PDFEmbeddedAttachment attach, List<object> entries, 
            PDFLayoutPage pg, IconAttachment icon, ComponentList contents)
        {
            
            foreach (Component comp in contents)
            {
                PDFAttachmentAnnotationEntry annot;
                ComponentList inner;
                if (IsContainer(comp, out inner))
                {
                    FillAttachmentAnnotations(context, style, attach, entries, pg, icon, inner);
                }
                else if (comp.Type == ObjectTypes.Whitespace)
                {
                    //Skip
                }
                else
                {
                    annot = new PDFAttachmentAnnotationEntry(icon, comp, attach, style);
                    

                    if (!string.IsNullOrEmpty(this.OutlineTitle))
                        annot.AlternateText = this.OutlineTitle;

                    object entry = pg.RegisterPageEntry(context, PDFArtefactTypes.Annotations, annot);

                    if (null != entry)
                        entries.Add(entry);
                }
            }
        }

        protected override void DoCloseLayoutArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet artefacts, Styles.Style fullstyle)
        {
            base.DoCloseLayoutArtefacts(context, artefacts, fullstyle);

            //Close the inner child annotation entries
            object entries;
            if (artefacts.TryGetArtefact(LinkAnnotationChildEntries, out entries))
            {
                object[] all = (object[])entries;
                //TODO: Check the use of a link that flows over more than one page
                PDFLayoutPage pg = context.DocumentLayout.CurrentPage;

                for (int i = all.Length - 1; i >= 0; i--)
                {
                    pg.CloseArtefactEntry(PDFArtefactTypes.Annotations, all[i]);
                }
            }

            //Close the actual link
            object link;
            if (artefacts.TryGetArtefact(LinkArtefactName, out link))
                context.DocumentLayout.CloseArtefactEntry(PDFArtefactTypes.Names, link);


            if (artefacts.TryGetArtefact(LinkEmbeddedFileChildEntryName, out entries))
            {
                object[] all = (object[])entries;
                //TODO: Check the use of a link that flows over more than one page
                PDFLayoutPage pg = context.DocumentLayout.CurrentPage;

                for (int i = all.Length - 1; i >= 0; i--)
                {
                    pg.CloseArtefactEntry(IconAttachment.AttachmentAnotationEntryArtefact, all[i]);
                }
            }

            if (artefacts.TryGetArtefact(LinkEmbeddedEntryName, out link))
            {
                context.DocumentLayout.CloseArtefactEntry(PDFArtefactTypes.Names, link);
            }
            
        }

        private bool IsNamedAction(LinkAction action)
        {
            switch (action)
            {
                case LinkAction.NextPage:
                case LinkAction.PrevPage:
                case LinkAction.FirstPage:
                case LinkAction.LastPage:
                    return true;

                default:
                    return false;

            }
        }

        /// <summary>
        /// Try to automatically determine
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual LinkAction ResolveActionType(string dest, string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return LinkAction.Destination;
            }
            else if (System.Uri.IsWellFormedUriString(file, UriKind.Absolute))
                return LinkAction.Uri;
            else
                return LinkAction.ExternalDestination;

        }

    }
}
