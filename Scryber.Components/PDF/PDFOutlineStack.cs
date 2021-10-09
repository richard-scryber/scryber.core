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
using Scryber.Styles;
//using Scryber.Drawing;
using Scryber.PDF.Native;
using Scryber.Drawing;

namespace Scryber.PDF
{
    internal class PDFOutlineStack : IArtefactCollection
    {

        #region ivars

        private Stack<PDFOutlineRef> _stack;
        private PDFOutlineRefCollection _roots;
        private string _name;

        #endregion

        /// <summary>
        /// Gets or sets the name of the collection
        /// </summary>
        string IArtefactCollection.CollectionName
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the collection of root outline items
        /// </summary>
        internal PDFOutlineRefCollection Roots
        {
            get { return _roots; }
        }

        /// <summary>
        /// Creates a new empty OutlineStack with the specified name
        /// </summary>
        /// <param name="name"></param>
        internal PDFOutlineStack(string name)
            : this(name, new PDFOutlineRefCollection())
        {
        }

        internal PDFOutlineStack(string name, PDFOutlineRefCollection roots)
        {
            this._name = name;
            this._roots = roots;
            this._stack = new Stack<PDFOutlineRef>();
        }

        internal void Clear()
        {
            this._roots.Clear();
            this._stack.Clear();
        }

        object IArtefactCollection.Register(IArtefactEntry entry)
        {
            if (entry is PDFOutlineRef)
                return this.Push((PDFOutlineRef)entry);
            else
                throw RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, entry, typeof(PDFOutlineRef).FullName);
        }

        internal object Push(PDFOutlineRef outlineref)
        {
            
            if (null == outlineref)
                throw RecordAndRaise.ArgumentNull("outlineref");
            if (null == outlineref.Outline)
                throw RecordAndRaise.ArgumentNull("outlineref.Outline");
            if (null == outlineref.Outline.BelongsTo)
                throw RecordAndRaise.ArgumentNull("outlineref.Outline.BelongsTo");

            if (_stack.Count == 0)
                _roots.Add(outlineref);
            else
                _stack.Peek().AddChild(outlineref);

            _stack.Push(outlineref);

            return outlineref.Outline.BelongsTo;
        }

        void IArtefactCollection.Close(object result)
        {
            this.Pop((IComponent)result);
        }

        internal void Pop(IComponent comp)
        {
            PDFOutlineRef last = _stack.Pop();
            if (last.Outline.BelongsTo != comp)
                throw RecordAndRaise.Operation(Errors.UnbalancedOutlineStack);
        }

        /// <summary>
        /// No Supported in the Outline Stack
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public PDFObjectRef[] OutputContentsToPDF(PDFRenderContext context, PDFWriter writer)
        {
            throw new NotSupportedException("This operation is not supported in Outline stacks");
        }

        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (this.Roots.Count > 0)
            {
                if (context.ShouldLogDebug)
                    context.TraceLog.Begin(TraceLevel.Verbose, "Outline Stack", "Starting to render the outline tree");

                PDFObjectRef outlines = writer.BeginObject();
                writer.BeginDictionary();
                writer.WriteDictionaryNameEntry("Type", "Outlines");
                PDFObjectRef first, last;
                int count;

                this.RenderOutlineCollection(this.Roots, outlines, context, writer, out first, out last, out count);

                if (null != first)
                    writer.WriteDictionaryObjectRefEntry("First", first);
                if (null != last)
                    writer.WriteDictionaryObjectRefEntry("Last", last);
                if (count > 0)
                    writer.WriteDictionaryNumberEntry("Count", count);

                writer.EndDictionary();
                writer.EndObject();//outlines

                if (context.ShouldLogDebug)
                    context.TraceLog.End(TraceLevel.Verbose, "Outline Stack", "Finished rendering the outline tree");
                else if(context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Outline Stack", "Rendered the outline tree to indirect object " + outlines + " with first " + first + ", last " + last + " and count " + count);
                
                return outlines;
            }
            else
                return null;
        }

        private void RenderOutlineCollection(PDFOutlineRefCollection col, PDFObjectRef parent, PDFRenderContext context, PDFWriter writer, out PDFObjectRef first, out PDFObjectRef last, out int count)
        {
            PDFObjectRef prev = null;
            List<PDFObjectRef> previousitems = new List<PDFObjectRef>();
            first = null;
            last = null;
            count = 0;
            int i = 0;
            do
            {
                int innercount;
                PDFObjectRef one = RenderOutlineItem(col[i], parent, prev, context, writer, out innercount);
                if (i == 0)
                {
                    first = one;
                }
                if (i == col.Count - 1)
                {
                    last = one;
                }
                i++;
                prev = one;
                previousitems.Add(one);
                count += innercount;

            } while (i < col.Count);

            //close all the dictionaries and object in reverse order
            //adding the next entry first if we are not the last entry
            for (int p = previousitems.Count - 1; p >= 0; p--)
            {
                if (p < previousitems.Count - 1)
                {
                    writer.WriteDictionaryObjectRefEntry("Next", previousitems[p + 1]);
                }
                writer.EndDictionary();
                writer.EndObject();
            }


        }

        private PDFObjectRef RenderOutlineItem(PDFOutlineRef outlineref, PDFObjectRef parent, PDFObjectRef prev, PDFRenderContext context, PDFWriter writer, out int count)
        {
            Outline outline = outlineref.Outline;
            Scryber.Drawing.Color c = outlineref.GetColor();
            Scryber.Drawing.FontStyle fs = outlineref.GetFontStyle();
            int weight = outlineref.GetFontWeight();

            bool isopen = outlineref.GetIsOpen();
            count = 1;//this one
            PDFObjectRef item = writer.BeginObject();

            writer.BeginDictionary();
            writer.WriteDictionaryObjectRefEntry("Parent", parent);
            writer.WriteDictionaryStringEntry("Title", outline.Title);
            writer.WriteDictionaryStringEntry("Dest", outline.DestinationName);
            if (!c.IsEmpty)
            {
                writer.BeginDictionaryEntry("C");
                writer.BeginArray();
                writer.WriteRealS(c.Red, c.Green, c.Blue);
                writer.EndArray();
                writer.EndDictionaryEntry();
            }

            if (fs != Scryber.Drawing.FontStyle.Regular)
            {
                int f = 0;
                if (weight >= FontWeights.Bold)
                    f = 2;
                if ((fs & Drawing.FontStyle.Italic) > 0)
                    f += 1;
                writer.WriteDictionaryNumberEntry("F", f);
            }

            if (null != prev)
                writer.WriteDictionaryObjectRefEntry("Prev", prev);

            if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, "Outline Stack", "Rendered outline item " + item + " with title '" + outline.Title + " and destination name " + outline.DestinationName);

            if (outlineref.HasInnerItems)
            {
                if (context.ShouldLogDebug)
                    context.TraceLog.Begin(TraceLevel.Debug, "Outline Stack", "Started rendering inner outline items");


                int opencount;
                PDFObjectRef childfirst, childlast;
                this.RenderOutlineCollection(outlineref.InnerItems, item, context, writer, out childfirst, out childlast, out opencount);

                writer.WriteDictionaryObjectRefEntry("First", childfirst);
                writer.WriteDictionaryObjectRefEntry("Last", childlast);
                if (opencount > 0)
                {
                    if (isopen)
                    {
                        writer.WriteDictionaryNumberEntry("Count", opencount);
                        count += opencount;
                    }
                    else
                        writer.WriteDictionaryNumberEntry("Count", -opencount);
                }

                if (context.ShouldLogDebug)
                    context.TraceLog.End(TraceLevel.Debug, "Outlines", " Finished rendering inner outline items");
            }

            //we don't close the dictionary here as we need the next entry written
            //It should be closed in the calling method

            return item;
        }


    }

    internal class PDFOutlineRefCollection : List<PDFOutlineRef>
    {
    }


    internal class PDFOutlineRef : IArtefactEntry


    {
        private Outline _outline;
        private OutlineStyle _style;
        private PDFOutlineRefCollection _inneritems;

        /// <summary>
        /// Gets the PDFOutline this reference refers to.
        /// </summary>
        internal Outline Outline { get { return _outline; } }

        /// <summary>
        /// Gets the style associated with this outline
        /// </summary>
        internal OutlineStyle Style { get { return _style; } }

        /// <summary>
        /// Gets the collection of inner items - can be null
        /// </summary>
        internal PDFOutlineRefCollection InnerItems { get { return _inneritems; } }

        /// <summary>
        /// Returns true if this outline reference has any inner items
        /// </summary>
        internal bool HasInnerItems { get { return _inneritems != null && _inneritems.Count > 0; } }


        internal PDFOutlineRef(Outline outline, OutlineStyle style)
        {
            if (null == outline)
                throw RecordAndRaise.ArgumentNull("outline");

            _outline = outline;
            _style = style;
        }

        internal void AddChild(PDFOutlineRef outlineref)
        {
            if (null == _inneritems)
                _inneritems = new PDFOutlineRefCollection();
            _inneritems.Add(outlineref);
        }

        internal Scryber.Drawing.Color GetColor()
        {
            var c = this.Outline.Color;

            if (c.IsTransparent)
            {
                c = (null == this.Style) ? Color.Transparent : this.Style.Color;
            }

            return c;
        }

        internal Drawing.FontStyle GetFontStyle()
        {
            var fs = Drawing.FontStyle.Regular;
            

            if (this.Outline.HasItalic)
            {
                fs = Drawing.FontStyle.Italic;
            }
            else if (this.Style.FontItalic)
                fs = Drawing.FontStyle.Italic;

            return fs;
        }

        internal int GetFontWeight()
        {
            if (this.Outline.HasBold)
                return FontWeights.Bold;
            else if (this.Style.FontBold)
                return FontWeights.Bold;
            else
                return FontWeights.Regular;
        }

        public bool GetIsOpen()
        {
            bool styleOpen;
            bool open = false;
            if (this.Outline.HasOpen)
            {
                if (this.Outline.OutlineOpen)
                    open = true;
            }
            else if (this.Style.TryGetValue(StyleKeys.OutlineOpenKey, out styleOpen))
                open = styleOpen;

            return open;
        }

    }
}
