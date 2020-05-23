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
using Scryber.Native;
using Scryber.Drawing;

namespace Scryber
{
    /// <summary>
    /// A Specific location within a pdf document, that is stored in the Names dictionary of a PDF Document's catalog
    /// </summary>
    public class PDFDestination : IArtefactEntry, ICategorisedArtefactNamesEntry
    {
        public const string DestinationNamesDictionaryEntry = "Dests";

        /// <summary>
        /// 
        /// </summary>
        public string NamesCategory
        {
            get { return DestinationNamesDictionaryEntry; }
        }

        #region public PDFComponent Component { get; }

        /// <summary>
        /// Gets the component this destination is for.
        /// </summary>
        public PDFComponent Component { get; private set; }

        #endregion

        #region public OutlineFit Fit { get; }

        /// <summary>
        /// Gets the fitting mechanism for this destination
        /// </summary>
        public OutlineFit Fit { get; private  set; }

        #endregion

        #region public string Extension { get; set; }

        /// <summary>
        /// Gets or sets any extension that should be used to uniquely
        /// identify the desination in the document name dictionary
        /// </summary>
        public string Extension { get; set; }

        #endregion

        #region public string FullName {get;set;}

        private string _fullname;

        /// <summary>
        /// Gets (or sets) the full destination name for this instance within the document
        /// </summary>
        public string FullName
        {
            get
            {
                //If we have a value set for the full name then use it
                if (!string.IsNullOrEmpty(_fullname))
                    return _fullname;

                //otherwise calculate it from the component
                if (null == this.Component)
                    throw RecordAndRaise.NullReference(Errors.NullArgument, "this.Component");

                string name = this.Component.Name;

                if (string.IsNullOrEmpty(name))
                    name = this.Component.UniqueID;

                if (!string.IsNullOrEmpty(this.Extension))
                    return name + ":" + Extension;
                else
                    return name;
            }
            set
            {
                this._fullname = value;
            }
        }

        #endregion

        //
        // .ctor(s)
        //

        #region public PDFDestination(PDFComponent component, OutlineFit fit)

        /// <summary>
        /// Creates a new instance of the PDFDestination that specifies a specific location within the current PDF document
        /// </summary>
        /// <param name="component"></param>
        /// <param name="fit"></param>
        public PDFDestination(PDFComponent component, OutlineFit fit)
            : this(component, fit, string.Empty)
        {
        }

        #endregion

        #region public PDFDestination(PDFComponent component, OutlineFit fit, string extension)

        /// <summary>
        /// Creates a new instance of the PDFDestination that represents a spcific location within the current PDF Document
        /// </summary>
        /// <param name="component"></param>
        /// <param name="fit"></param>
        /// <param name="extension">Any required label extension to guaruntee uniqueness of the destination within the documents name dictionary</param>
        public PDFDestination(PDFComponent component, OutlineFit fit, string extension)
        {
            if (null == component)
                throw RecordAndRaise.ArgumentNull("component");

            this.Component = component;
            this.Fit = fit;
            this.Extension = extension;
        }

        #endregion

        //
        // methods
        //

        #region public PDFObjectRef OutputToPDF(PDFWriter writer, PDFRenderContext context)

        /// <summary>
        /// Renderes this destination within the current PDFObject stream of the provided writer.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFComponentArrangement arrange;
            arrange = this.GetFirstArrangementInTree(this.Component);

            if (null == arrange)
            {
                //The component does not have an arrangement so cannot output the destination
                writer.WriteNullS();

                if (context.TraceLog.ShouldLog(TraceLevel.Warning))
                    context.TraceLog.Add(TraceLevel.Warning, "Destination", "Destination to component " + this.Component.UniqueID + " cannot be written as it has no arrangement");
            
                return null;
            }

            int pgindex = arrange.PageIndex;
            PDFObjectRef oref = writer.PageRefs[pgindex];

            if (null == oref)
            {
                //No page, so cannot output the destination
                writer.WriteNullS();

                if (context.TraceLog.ShouldLog(TraceLevel.Warning))
                    context.TraceLog.Add(TraceLevel.Warning, "Destination", "Destination to component " + this.Component.UniqueID + " cannot be written as it has no page reference");

                return null;
            }

            writer.BeginArray();

            //Write the page reference
            writer.BeginArrayEntry();
            writer.WriteObjectRef(oref);
            writer.EndArrayEntry();

            //Write the page fit method
            switch (this.Fit)
            {
                case OutlineFit.FullPage:
                    writer.BeginArrayEntry();
                    writer.WriteName("Fit");
                    writer.EndArrayEntry();
                    break;
                case OutlineFit.PageWidth:
                    writer.BeginArrayEntry();
                    writer.WriteName("FitH");
                    writer.EndArrayEntry();
                    break;
                case OutlineFit.PageHeight:
                    writer.BeginArrayEntry();
                    writer.WriteName("FitV");
                    writer.EndArrayEntry();
                    break;
                case OutlineFit.BoundingBox:
                    writer.BeginArrayEntry();
                    writer.WriteName("FitR");
                    writer.EndArrayEntry();

                    PDFReal left = arrange.RenderBounds.X.RealValue;
                    PDFReal top = arrange.RenderBounds.Y.RealValue;
                    PDFReal right = left + arrange.RenderBounds.Width.RealValue;
                    PDFReal bottom = top + arrange.RenderBounds.Height.RealValue;
                    left = context.Graphics.GetXPosition(left);
                    right = context.Graphics.GetXPosition(right);
                    top = context.Graphics.GetYPosition(top);
                    bottom = context.Graphics.GetYPosition(bottom);
                    if (bottom < top)
                    {
                        PDFReal temp = top;
                        top = bottom;
                        bottom = temp;
                    }

                    if (left > right)
                    {
                        PDFReal temp = left;
                        left = right;
                        right = temp;
                    }
                   
                    writer.BeginArrayEntry();
                    left.WriteData(writer);
                    writer.EndArrayEntry();

                    writer.BeginArrayEntry();
                    bottom.WriteData(writer);
                    writer.EndArrayEntry();

                    writer.BeginArrayEntry();
                    right.WriteData(writer);
                    writer.EndArrayEntry();

                    writer.BeginArrayEntry();
                    top.WriteData(writer);
                    writer.EndArrayEntry();
                    break;
                default:
                    break;

            }
            writer.EndArray();

            if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, "Destination", "Added a destination to component " + this.Component.UniqueID + " with first arrangment on page " + pgindex + " (" + oref + ") with size fit of " + this.Fit);
                    
            return null;
        }

        /// <summary>
        /// returns the first available arrangement for the component or one of it's parents.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private PDFComponentArrangement GetFirstArrangementInTree(PDFComponent component)
        {
            while(null != component)
            {
               PDFComponentArrangement arrange = component.GetFirstArrangement();
                if (null != arrange)
                    return arrange;
                else
                    component = component.Parent;
            }
            return null;
            
        }

        #endregion
    }

}
