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
using Scryber.Native;

namespace Scryber.Components
{
    /// <summary>
    /// Defines document level preferences for the viewer application
    /// </summary>
    public class DocumentViewPreferences
    {
        private object _hidetoolbar,
                       _hidemenubar,
                       _hidewindowui,
                       _fitwindow,
                       _centrewindow;

        //
        // properties
        //

        #region public bool HideToolbar {get;set;} + ClearHideToolbar()

        /// <summary>
        /// Flag to show or hide the toolbar in the PDF Viewer Application
        /// </summary>
        [PDFAttribute("hide-toolbar")]
        public bool HideToolbar
        {
            get
            {
                if (null == _hidetoolbar)
                    return false;
                else
                    return (bool)_hidetoolbar;
            }
            set
            {
                _hidetoolbar = value;
            }
        }

        /// <summary>
        /// Clears the flag for the toolbar so the viewer will use it's default
        /// </summary>
        public void ClearHideToolbar()
        {
            _hidetoolbar = null;
        }

        #endregion

        #region public bool HideMenubar {get;set;} + ClearHideMenubar()

        /// <summary>
        /// Flag to show or hide the menubar in the PDF Viewer Application
        /// </summary>
        [PDFAttribute("hide-menubar")]
        public bool HideMenubar
        {
            get
            {
                if (null == _hidemenubar)
                    return false;
                else
                    return (bool)_hidemenubar;
            }
            set
            {
                _hidemenubar = value;
            }
        }

        /// <summary>
        /// Clears the flag for the HideMenubar so the viewer will use its default
        /// </summary>
        public void ClearHideMenubar()
        {
            this._hidemenubar = null;
        }

        #endregion

        #region public bool HideWindowUI {get;set;} + ClearHideWindowUI()

        /// <summary>
        /// Flag to show or hide the window user interface elements in the PDF Viewer Application
        /// </summary>
        [PDFAttribute("hide-windowUI")]
        public bool HideWindowUI
        {
            get
            {
                if (null == _hidewindowui)
                    return false;
                else
                    return (bool)_hidewindowui;
            }
            set
            {
                _hidewindowui = value;
            }
        }

        /// <summary>
        /// Clears the flag for hiding the window UI so the viewer will use it's default value
        /// </summary>
        public void ClearHideWindowUI()
        {
            this._hidewindowui = null;
        }

        #endregion

        #region public bool FitWindow {get;set;} + ClearFitWindow()

        /// <summary>
        /// Flag to resize the document window to fit the documents first page
        /// </summary>
        [PDFAttribute("fit-window")]
        public bool FitWindow
        {
            get
            {
                if (null == _fitwindow)
                    return false;
                else
                    return (bool)_fitwindow;
            }
            set
            {
                this._fitwindow = value;
            }
        }

        /// <summary>
        /// Clears the flag for fitting the window so the viewer application will use its default
        /// </summary>
        public void ClearFitWindow()
        {
            this._fitwindow = null;
        }

        #endregion

        #region public bool CenterWindow {get;set;} + ClearCenterWindow()

        /// <summary>
        /// Flag to place the window at the screen centre
        /// </summary>
        [PDFAttribute("center-window")]
        public bool CenterWindow
        {
            get
            {
                if (null == _centrewindow)
                    return false;
                else
                    return (bool)_centrewindow;
            }
            set
            {
                _centrewindow = value;
            }
        }

        /// <summary>
        /// Clears the flag for centering the window so the viewer will user the default
        /// </summary>
        public void ClearCenterWindow()
        {
            this._centrewindow = null;
        }

        #endregion

        #region public PageDisplayMode PageDisplay {get;set;}

        private PageDisplayMode _pgdisplay = PageDisplayMode.Undefined;

        /// <summary>
        /// Gets or sets the page display mode for the document
        /// </summary>
        [PDFAttribute("page-display")]
        public PageDisplayMode PageDisplay
        {
            get { return _pgdisplay; }
            set { _pgdisplay = value; }
        }

        #endregion

        #region public PageLayoutMode PageLayout {get;set;}

        private PageLayoutMode _pglayout = PageLayoutMode.Undefined;

        /// <summary>
        /// Gets or sets the Page Layout mode for this document
        /// </summary>
        [PDFAttribute("page-layout")]
        public PageLayoutMode PageLayout
        {
            get { return _pglayout; }
            set { _pglayout = value; }
        }

        #endregion

        private PageDisplayMode _nonFSPageDisplay = PageDisplayMode.Undefined;

        /// <summary>
        /// Gets or sets the display mode to use when exiting full screen mode
        /// </summary>
        [PDFAttribute("non-full-screen-display")]
        public PageDisplayMode NonFullScreenPageDisplay
        {
            get { return _nonFSPageDisplay; }
            set { _nonFSPageDisplay = value; }
        }
        //
        // methods
        //


        public virtual PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (this.IsEmpty())
            {
                if (context.ShouldLogMessage)
                    context.TraceLog.Add(TraceLevel.Message, "Viewer Preferences", "No values set on the document viewer preferences so not outputting");
                return null;
            }
            else
            {
                PDFObjectRef oref = writer.BeginObject();
                writer.BeginDictionary();
                this.WriteOptionalFlag("CenterWindow", _centrewindow, context, writer);
                this.WriteOptionalFlag("FitWindow", _fitwindow, context, writer);
                this.WriteOptionalFlag("HideWindowUI", _hidewindowui, context, writer);
                this.WriteOptionalFlag("HideToolbar", _hidetoolbar, context, writer);
                this.WriteOptionalFlag("HideMenubar", _hidemenubar, context, writer);
                if (this.NonFullScreenPageDisplay != PageDisplayMode.Undefined)
                    this.WriteOptionalName("NonFullScreenPageMode", this.NonFullScreenPageDisplay.ToString(), context, writer);
                writer.EndDictionary();
                writer.EndObject();

                return oref;

            }
        }

        private void WriteOptionalFlag(string name, object value, PDFRenderContext context, PDFWriter writer)
        {
            if (null == value)
                return;
            else
            {
                writer.BeginDictionaryEntry(name);
                writer.WriteBoolean((bool)value);
                writer.EndDictionaryEntry();

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Viewer Preferences", String.Format("Output /{0} value to {1}", name, value));
            }
        }

        private void WriteOptionalName(string name, string value, PDFRenderContext context, PDFWriter writer)
        {
            if (String.IsNullOrEmpty(value))
                return;
            else
            {
                writer.BeginDictionaryEntry(name);
                writer.WriteName(value);
                writer.EndDictionaryEntry();

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Viewer Preferences", String.Format("Output /{0} value to {1}", name, value));
            }
        }



        protected virtual bool IsEmpty()
        {
            if (null != this._centrewindow)
                return false;
            else if (null != this._fitwindow)
                return false;
            else if (null != this._hidemenubar)
                return false;
            else if (null != this._hidetoolbar)
                return false;
            else if (null != this._hidewindowui)
                return false;
            else
                return true;
        }

        public string GetPageDisplayName(PageDisplayMode mode)
        {
            string name;
            switch (mode)
            {
                case PageDisplayMode.Undefined:
                    name = string.Empty;
                    break;
                case PageDisplayMode.None:
                    name = "UseNone";
                    break;
                case PageDisplayMode.Outlines:
                    name = "UseOutlines";
                    break;
                case PageDisplayMode.Thumbnails:
                    name = "UseThumbs";
                    break;
                case PageDisplayMode.FullScreen:
                    name = "FullScreen";
                    break;
                case PageDisplayMode.Attachments:
                    name = "UseAttachments";
                    break;
                default:
                    name = string.Empty;
                    break;
            }
            return name;
        }

        public string GetPageLayoutName(PageLayoutMode mode)
        {
            string name;
            switch (mode)
            {
                case PageLayoutMode.Undefined:
                    name = string.Empty;
                    break;
                case PageLayoutMode.SinglePage:
                case PageLayoutMode.TwoPageLeft:
                case PageLayoutMode.TwoPageRight:
                case PageLayoutMode.OneColumn:
                case PageLayoutMode.TwoColumnLeft:
                case PageLayoutMode.TwoColumnRight:
                    name = mode.ToString();
                    break;
                default:
                    name = string.Empty;
                    break;
            }
            return name;
        }
    }
}
