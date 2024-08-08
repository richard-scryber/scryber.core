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

#define REGISTER_CHARACTERS
#define COUNTERS_AND_PSEUDOCONTENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
using Scryber.PDF;

namespace Scryber.PDF.Layout
{
    public abstract partial class LayoutEngineBase : IPDFLayoutEngine
    {

        internal const string LOG_CATEGORY = "Layout Engine";

        #region ivars

        /// <summary>
        /// An instance reference to the current block that was created and opened by this Panel
        /// Layout engine and therefore should be closed at the end. Can be updated by moving regions
        /// and pages as we close and open new blocks.
        /// </summary>
        private PDFLayoutBlock _currentBlock;

        /// <summary>
        /// An instance reference to the Component this engine is laying out
        /// </summary>
        private ContainerComponent _component;

        /// <summary>
        /// An instance reference to the current context
        /// </summary>
        private PDFLayoutContext _context;

        /// <summary>
        /// An instance reference to the FullStyle
        /// </summary>
        private Style _style;


        #endregion

        #region public PDFContainerComponent Component {get; protected set;}

        /// <summary>
        /// Gets the component this engine is laying out
        /// </summary>
        public ContainerComponent Component
        {
            get { return _component; }
            protected set { _component = value; }
        }

        #endregion

        #region protected PDFLayoutContext Context {get;}

        /// <summary>
        /// Gets the layout context associated with this layout engine
        /// </summary>
        public PDFLayoutContext Context
        {
            get { return _context; }
        }

        #endregion

        #region protected PDFStyle FullStyle {get;}

        /// <summary>
        /// Gets or sets the full style for the current component
        /// </summary>
        protected Style FullStyle
        {
            get { return _style; }
            set { _style = value; }
        }

        #endregion

        #region protected PDFLayoutDocument DocumentLayout

        /// <summary>
        /// Gets the document as the root of this layout context
        /// </summary>
        protected PDFLayoutDocument DocumentLayout
        {
            get
            {
                if (null == _context)
                    return null;
                else
                    return _context.DocumentLayout;
            }
        }

        #endregion

        #region protected PDFStyleStack StyleStack {get;}

        /// <summary>
        /// Gets the StyleStack associated with this layout
        /// </summary>
        protected StyleStack StyleStack
        {
            get { return this.Context.StyleStack; }
        }

        #endregion

        #region public bool ContinueLayout {get;set;}

        /// <summary>
        /// Returns false if we have come to the end of the available space and cannot continue. Inheritors can set this value
        /// </summary>
        public bool ContinueLayout
        {
            get;
            set;
        }

        #endregion

        #region protected PDFLayoutBlock CurrentBlock

        /// <summary>
        /// Gets or sets the current layout block
        /// </summary>
        protected PDFLayoutBlock CurrentBlock
        {
            get { return _currentBlock; }
            set { _currentBlock = value; }
        }

        #endregion

        #region public IPDFLayoutEngine ParentEngine {get;set;}

        /// <summary>
        /// Gets  the parent engine that called this one
        /// </summary>
        public IPDFLayoutEngine ParentEngine
        {
            get;
            private set;
        }

        #endregion

        //
        // ctor(s)
        //

        #region protected PDFBlockLayoutEngineBase(IPDFComponent component, IPDFLayoutEngine parent)

        /// <summary>
        /// protected constructor that accepts the component this engine should layout
        /// </summary>
        /// <param name="component"></param>
        protected LayoutEngineBase(ContainerComponent component, IPDFLayoutEngine parent)
        {
            this._component = component;
            this.ParentEngine = parent;
        }

        #endregion

        //
        // iIPDFLayoutEngine interface implementation
        //

        #region public void Layout(PDFLayoutContext context, PDFStyle fullstyle)

        /// <summary>
        /// Interface implementation of the LayoutBlock method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        public void Layout(PDFLayoutContext context, Style fullstyle)
        {
            this._context = context;

            if (null != context.DocumentLayout && null != context.DocumentLayout.CurrentPage)
                this._currentBlock = context.DocumentLayout.CurrentPage.CurrentBlock;

            this._style = fullstyle;



            StyleValue<PositionMode> found;
            if (this._style.TryGetValue(StyleKeys.PositionModeKey, out found) && found.Value(this._style) == PositionMode.Invisible)
            {
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, "Layout", "Skipping over the layout of component '" + this.Component.UniqueID + "' as it is invisible");
            }
            else
            {
#if COUNTERS_AND_PSEUDOCONTENT

                //counter updates for visible components before
                Style before;
                if (fullstyle.HasStates && fullstyle.TryGetStyleState(ComponentState.Before, out before))
                    this.EnsureCounterUpdatesForStyle(before);

                //counter updates for visible components on element

                this.EnsureCounterUpdatesForStyle(fullstyle);


                this.DoLayoutComponent();

                //counter updates for visible components after

                Style after;
                if (fullstyle.HasStates && fullstyle.TryGetStyleState(ComponentState.After, out after))
                    this.EnsureCounterUpdatesForStyle(after);

#else
                this.DoLayoutComponent();
#endif

            }
        }

#endregion


        //
        // abstract methods
        //

        /// <summary>
        /// Abstract method that inheritors must implement in order to actually 
        /// layout the component and any child components
        /// </summary>
        protected abstract void DoLayoutComponent();

        //
        // layout children methods
        //

        #region protected virtual void DoLayoutChildren()

        /// <summary>
        /// enumerates over any / each of the engines component children and performs the correct layout option for these.
        /// The component this engine is laying out should implement the IPDFContainerComponent
        /// </summary>
        protected virtual void DoLayoutChildren()
        {
            //Set this at the top
            this.ContinueLayout = true;
            ComponentList children;
            if (TryGetComponentChildren(this.Component, out children))
            {
                DoLayoutChildren(children);
            }
        }
        
        #endregion
        
        #region private bool TryGetComponentChildren(IComponent parent, out ComponentList children)

        /// <summary>
        /// Gets the list of child components and returns true if there are some
        /// </summary>
        /// <param name="parent">The parent component to get the children for.</param>
        /// <param name="children"></param>
        /// <returns></returns>
        private bool TryGetComponentChildren(IComponent parent, out ComponentList children)
        {
            children = GetComponentChildren(parent);

#if COUNTERS_AND_PSEUDOCONTENT

            //Apply the ::before and ::after states as children
            if (this.FullStyle.HasStates)
            {
                Style state;
                StyleValue<ContentDescriptor> content;

                if (this.FullStyle.TryGetStyleState(ComponentState.Before, out state)
                    && state.TryGetValue(StyleKeys.ContentTextKey, out content))
                {
                    if (null == children)
                        children = new ComponentList(parent as Component, StyleKeys.ContentTextKey.StyleValueKey);
                    AddBeforeContent(parent, children, state, content.Value(state));
                }

                if (this.FullStyle.TryGetStyleState(ComponentState.After, out state)
                    && state.TryGetValue(StyleKeys.ContentTextKey, out content))
                {
                    if (null == children)
                        children = new ComponentList(parent as Component, StyleKeys.ContentTextKey.StyleValueKey);
                    AddAfterContent(parent, children, state, content.Value(state));
                }
            }

#endif
            return null != children && children.Count > 0;

        }
        
        #endregion

        #region protected virtual ComponentList GetComponentChildren(IComponent parent)
        /// <summary>
        /// Returns the child components of the IPDFcomponent if any.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected virtual ComponentList GetComponentChildren(IComponent parent)
        {
            if (parent is IContainerComponent)
            {
                IContainerComponent contaner = parent as IContainerComponent;
                if (contaner.HasContent)
                    return contaner.Content;
            }
            return null;
        }

        #endregion
        
        #region protected virtual void DoLayoutChildren(PDFComponentList children)

        /// <summary>
        /// enumerates over each of the components in the specified list 
        /// and performs the layout of each individually
        /// </summary>
        /// <param name="children"></param>
        protected virtual void DoLayoutChildren(ComponentList children)
        {
            
            foreach (Component comp in children)
            {
                if (comp.Visible)
                    this.DoLayoutAChild(comp);

                if (this.ContinueLayout == false 
                    || this.DocumentLayout.CurrentPage.IsClosed 
                    || this.DocumentLayout.CurrentPage.CurrentBlock == null)
                    break;
            }
            
        }

        

        #endregion
        
        //
        // counters
        //
        
        
        #region protected virtual void EnsureCounterUpdatesForStyle(Style style)

        /// <summary>
        /// Updates any counters on the style - either reset or increment
        /// </summary>
        /// <param name="style"></param>
        protected virtual void EnsureCounterUpdatesForStyle(Style style)
        {
            StyleValue<CounterStyleValue> reset;
            StyleValue<CounterStyleValue> increment;

            if (style.TryGetValue(StyleKeys.CounterResetKey, out reset))
            {
                var val = reset.Value(style);
                while (null != val)
                {
                    this.ResetACounter(val, this.Component);
                    val = val.Next;
                }
            }

            if (style.TryGetValue(StyleKeys.CounterIncrementKey, out increment))
            {
                var val = increment.Value(style);
                while (null != val)
                {
                    this.IncrementACounter(val, this.Component);
                    val = val.Next;
                }
            }
        }

        #endregion

        #region protected bool ResetACounter(CounterStyleValue counter, Component current, bool resetIfNotFound = true)

        /// <summary>
        /// Resets a counter on a list item, or a component where the counter exists.
        /// </summary>
        /// <param name="counter">The counter to reset</param>
        /// <param name="current">The current component for the counter</param>
        /// <param name="resetIfNotFound">If true (default) then if the counter is not found in the component hierarchy, then it is set on the current component to the value</param>
        /// <returns>True if a counter was reset</returns>
        protected bool ResetACounter(CounterStyleValue counter, Component current, bool resetIfNotFound = true)
        {
            if (current.Type == ObjectTypes.OrderedList || current.Type == ObjectTypes.UnorderedList)
            {
                //ol and ul appear to be special cases where they always have the counter assigned on them
                //there is no other way to traverse the hierarchy.

                current.Counters.Reset(counter.Name, counter.Value);
                return true;
            }
            else
            {
                var comp = current.Parent;
                while (null != comp)
                {
                    int val;
                    if (comp.HasCounters && comp.Counters.TryGetValue(counter.Name, out val))
                    {
                        comp.Counters.Reset(counter.Name, counter.Value);
                        return true;
                    }
                    comp = comp.Parent; //move up the heirachy
                }

                if (resetIfNotFound)
                {
                    current.Counters.Reset(counter.Name, counter.Value);
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion

        #region protected bool IncrementACounter(CounterStyleValue counter, Component current, bool resetIfNotFound = false)

        /// <summary>
        /// Increments a counter on the current component, or a parent component where the counter is defined.
        /// </summary>
        /// <param name="counter">The counter (with name) to increment</param>
        /// <param name="current">The current component the increment style is set on</param>
        /// <param name="resetIfNotFound">If true then the counter will be reset and incremented on the current component if, and only if it is not found in the heirarchy. By default this is false.</param>
        /// <returns>True if a counter was incremented</returns>
        protected bool IncrementACounter(CounterStyleValue counter, Component current, bool resetIfNotFound = false)
        {
            var comp = current;
            while (null != comp)
            {
                int val;
                if (comp.HasCounters && comp.Counters.TryGetValue(counter.Name, out val))
                {
                    comp.Counters.Increment(counter.Name, counter.Value);
                    return true;
                }
                comp = comp.Parent; //move up the heirachy
            }

            if (resetIfNotFound)
            {
                current.Counters.Reset(counter.Name, 0);
                current.Counters.Increment(counter.Name, counter.Value);
                return true;
            }
            else
                return false;

        }

        #endregion
        
        #region protected void RemoveCountersFromStateStyle(Style style)

        /// <summary>
        /// Becuase increments and resets for a state have been performed before we get here (so we can get the actual text for the ::before or ::after spans,
        /// then we do not want them to be executed again.
        /// </summary>
        /// <param name="style"></param>
        protected void RemoveCountersFromStateStyle(Style style)
        {
            style.RemoveValue(StyleKeys.CounterResetKey);
            style.RemoveValue(StyleKeys.CounterIncrementKey);
        }

        #endregion
        
        // ::before and ::after content
        
        #region private void AddBeforeContent(IComponent parent, ComponentList children, Style before, ContentDescriptor value)
        
        private void AddBeforeContent(IComponent parent, ComponentList children, Style before, ContentDescriptor value)
        {
            Span span = new Span();
            span.ID = parent.ID + "__before";
            RemoveCountersFromStateStyle(before);
            span.Style = before;

            int index = 0;
            while (null != value)
            {
                this.AddCssContentToList(span, value, span.Contents, index);

                value = value.Next;
                index++;
            }
            if (span.Contents.Count == 1 && span.Contents[0] is Image)
            {
                var img = span.Contents[0] as Image;
                img.ID = parent.ID + "__before";
                img.Style = before;
                children.Insert(0, img);
            }
            else
                children.Insert(0, span);


        }
        
        #endregion

        #region private void AddAfterContent(IComponent parent, ComponentList children, Style after, ContentDescriptor value)
        
        private void AddAfterContent(IComponent parent, ComponentList children, Style after, ContentDescriptor value)
        {
            Span span = new Span();
            span.ID = parent.ID + "__after";
            RemoveCountersFromStateStyle(after);
            span.Style = after;


            int index = 0;
            while (null != value)
            {
                this.AddCssContentToList(span, value, span.Contents, index);

                value = value.Next;
                index++;
            }
            if (span.Contents.Count == 1 && span.Contents[0] is Image)
            {
                var img = span.Contents[0] as Image;
                img.ID = parent.ID + "__after";
                img.Style = after;
                children.Add(img);
            }
            else
                children.Add(span);


        }
        
        #endregion

        #region private bool AddCssContentToList(IStyledComponent wrapper, ContentDescriptor content, ComponentList children, int index)
        
        private bool AddCssContentToList(IStyledComponent wrapper, ContentDescriptor content, ComponentList children, int index)
        {
            bool added = false;
            TextLiteral text;

            switch (content.Type)
            {
                case ContentDescriptorType.Text:
                    var txtContent = (ContentTextDescriptor)content;
                    text = new TextLiteral(txtContent.Text);
                    children.Add(text);
                    added = true;
                    break;

                case ContentDescriptorType.Gradient:
                    var grad = (ContentGradientDescriptor)content;
                    wrapper.Style.SetValue(StyleKeys.BgImgSrcKey, grad.Text);
                    added = true;
                    break;

                case ContentDescriptorType.Image:
                    if (wrapper is Image)
                    {
                        (wrapper as Image).Source = (content as ContentImageDescriptor).Source;
                    }
                    else
                    {
                        var img = new Image();
                        img.Source = (content as ContentImageDescriptor).Source;
                        img.ElementName = "img";
                        img.PositionMode = PositionMode.Inline;
                        children.Add(img);
                        added = !string.IsNullOrEmpty(img.Source);
                    }
                    break;

                case ContentDescriptorType.Quote:
                    text = new TextLiteral((content as ContentQuoteDescriptor).Chars);
                    children.Add(text);
                    added = true;
                    break;

                case ContentDescriptorType.Attribute:
                    var defn = Scryber.Generation.ParserDefintionFactory.GetClassDefinition(wrapper.GetType());
                    var attr = content as ContentAttributeDescriptor;
                    if(defn.Attributes.TryGetPropertyDefinition(attr.Attribute, string.Empty, out var prop))
                    {
                        object val = prop.PropertyInfo.GetValue(wrapper);
                        if(null != val)
                        {
                            text = new TextLiteral(val.ToString());
                            children.Add(text);
                            added = true;
                        }
                    }
                    break;

                case ContentDescriptorType.Counter:
                    var counterDesc = content as ContentCounterDescriptor;
                    var counterValue = counterDesc.GetContent(this.Component);
                    if (!string.IsNullOrEmpty(counterValue))
                    {
                        text = new TextLiteral(counterValue);
                        children.Add(text);
                        added = true;
                    }
                    break;
                case ContentDescriptorType.Counters:
                    var countersDesc = content as ContentCountersDescriptor;
                    var countersValue = countersDesc.GetContent(this.Component);
                    if (!string.IsNullOrEmpty(countersValue))
                    {
                        text = new TextLiteral(countersValue);
                        children.Add(text);
                        added = true;
                    }
                    break;
                default:
                    break;
            }
            return added;
        }
        
        #endregion

        //
        // Child layout
        //
        
        #region protected virtual void DoLayoutAChild(PDFComponent comp)

        /// <summary>
        /// Lays out an individual component
        /// </summary>
        /// <param name="comp"></param>
        /// <remarks>Gets the applied style, and pushes this onto the stack.
        /// Extracts the full style for this component based on the style stack
        /// Calls the explict overload DoLayoutAChild(comp,full). </remarks>
        protected void DoLayoutAChild(Component comp)
        {


            string styleidentifier = null;
            bool found = false;
            Style full = null;
            Style applied = null;
            

            if (IsStyled(comp))
            {
                if (comp is IDataStyledComponent
                    && !string.IsNullOrEmpty((comp as IDataStyledComponent).DataStyleIdentifier))
                {
                    styleidentifier = (comp as IDataStyledComponent).DataStyleIdentifier;
                    if (this.Context.DocumentLayout.TryGetStyleWithIdentifier(styleidentifier, out applied, out full))
                    {
                        if (this.Context.ShouldLogDebug)
                            this.Context.TraceLog.Add(TraceLevel.Debug, LOG_CATEGORY, "Cache hit for the component style with identifier '" + styleidentifier + "'");

                        this.StyleStack.Push(applied);
                        found = true;
                    }
                }

                if (found == false)
                {
                    Context.PerformanceMonitor.Begin(PerformanceMonitorType.Style_Build);

                    applied = comp.GetAppliedStyle();
                    if (null != applied)
                        this.StyleStack.Push(applied);

                    full = this.BuildFullStyle(comp);

                    Context.PerformanceMonitor.End(PerformanceMonitorType.Style_Build);
                }

                if (found == false && !string.IsNullOrEmpty(styleidentifier))
                {
                    this.Context.DocumentLayout.SetStyleWithIdentifier(styleidentifier, applied, full);
                }
               
            }
            else
                full = this.FullStyle;

            CheckForPrecedingInlineWhitespace(comp, full);

            if (IsStyled(comp) && !IsText(comp))
            {
                StyleValue<bool> br;
                if (full.TryGetValue(StyleKeys.PageBreakBeforeKey, out br) && br.Value(full))
                {
                    this.DoLayoutPageBreak(comp, full);
                }
                else if (full.TryGetValue(StyleKeys.ColumnBreakBeforeKey, out br) && br.Value(full))
                {
                    this.DoLayoutColumnBreak(comp, full);
                }
            }

            PDFArtefactRegistrationSet artefacts = comp.RegisterLayoutArtefacts(this.Context, full);

            //perform the actual layout of the component with the full style
            this.DoLayoutAChild(comp, full);

            if (IsStyled(comp) && !IsText(comp))
            {
                StyleValue<bool> br;
                if (full.TryGetValue(StyleKeys.PageBreakAfterKey, out br) && br.Value(full))
                {
                    this.DoLayoutPageBreak(comp, full);
                }
                else if (full.TryGetValue(StyleKeys.ColumnBreakAfterKey, out br) && br.Value(full))
                {
                    this.DoLayoutColumnBreak(comp, full);
                }
            }

            //pop any child component style off the stack
            if (null != applied)
                this.StyleStack.Pop();

            //We want to add the child only if it should be rendered and we are a full block
            if (this.ContinueLayout)
                RegisterChildLayout(comp);

            //and close the artefacts
            if(null != artefacts)
                comp.CloseLayoutArtefacts(this.Context, artefacts, full);

        }

        private static bool IsStyled(IComponent comp)
        {
            return comp is IStyledComponent && !(comp is ILayoutBreak);
        }

        private static bool IsText(IComponent comp)
        {
            return comp is ITextComponent;
        }

        #endregion

        #region protected virtual void CheckForPrecedingInlineWhitespace(Component current, Style currentFullStyle)

        private PositionMode _lastPositionMode = PositionMode.Block;
        private Whitespace _lastWhitespace = null;

        /// <summary>
        /// If the current component is whitespace then it is remembers as the last whitespace.
        /// Otherwise if this component and the previous component were inline or inline block, and there was a whitespace before this one,
        /// Then we should add a space e.g. span whitespace span
        /// </summary>
        /// <param name="current">The current component that is being laidout</param>
        /// <param name="currentFullStyle"></param>
        protected virtual void CheckForPrecedingInlineWhitespace(Component current, Style currentFullStyle)
        {
            var currentIsText = IsText(current);
            var nextPosMode = currentIsText ? PositionMode.Inline : currentFullStyle.GetValue(StyleKeys.PositionModeKey, PositionMode.Block);
            if (_lastPositionMode == PositionMode.Inline || _lastPositionMode == PositionMode.InlineBlock)
            {
                if (current is Whitespace ws)
                {
                    //We always remember the last whitespace
                    _lastWhitespace = ws;
                }
                else
                {
                    if (_lastPositionMode == PositionMode.Inline || _lastPositionMode == PositionMode.InlineBlock)
                    {
                        //We are inline
                        if (null != _lastWhitespace && _lastWhitespace.ReaderFormat != TextFormat.Plain)
                        {
                            //we have had a whitespace before
                            if(currentIsText)
                            {
                                //Do Nothing as the text spacing should be handled by this component
                            }
                            else if (nextPosMode == PositionMode.Inline || nextPosMode == PositionMode.InlineBlock)
                            {
                                TextLiteral literal = new TextLiteral(" ", TextFormat.Plain);
                                literal.Parent = this.Component;

                                //TODO: Satisfy the edge case where the space overflows to the next line - it should be ignored, but a new line created.
                                this.DoLayoutTextComponent(literal, currentFullStyle);
                            }
                        }
                    }
                    //The last one (i.e. current) was not whitespace
                    _lastWhitespace = null;
                }
            }

            //Save the last position mode (always)
            _lastPositionMode = nextPosMode;

        }

        #endregion

        #region protected virtual Style BuildFullStyle(Component forComponent)

        /// <summary>
        /// Builds a comple full style for the specified component. All relative units will be converted to absolute units based on the current full style.
        /// </summary>
        /// <param name="forComponent">The component to create the full style for.</param>
        /// <returns>A full style</returns>
        protected virtual Style BuildFullStyle(Component forComponent)
        {
            var page = this.DocumentLayout.CurrentPage;
            var pgSize = page.Size;
            var container = this.DocumentLayout.CurrentPage.LastOpenBlock();
            Size containerSize;
            if (null != container)
                containerSize = container.CurrentRegion.TotalBounds.Size;
            else if (null != page.CurrentBlock)
                containerSize = page.CurrentBlock.CurrentRegion.TotalBounds.Size;
            else
                containerSize = page.Size;

            var font = this.FullStyle.CreateTextOptions();

            var fontSize = new Size(font.GetZeroCharWidth(), font.GetSize());

            var full = this.Context.StyleStack.GetFullStyle(forComponent, pgSize, new ParentComponentSizer(this.GetParentComponentSize), fontSize, Font.DefaultFontSize);
            return full;
        }

        #endregion

        #region protected virtual Size GetParentComponentSize(IComponent component, Style style, PositionMode withMode)

        protected virtual Size GetParentComponentSize(IComponent component, Style style, PositionMode withMode)
        {
            Size sz = Size.Empty;
            var pg = this.DocumentLayout.CurrentPage;

            if (withMode == PositionMode.Relative || withMode == PositionMode.Absolute)
            {
                var container = pg.LastOpenBlock();
                bool foundRelative = false;

                while(null != container)
                {
                    if(container.Position.PositionMode == PositionMode.Absolute
                        || container.Position.PositionMode == PositionMode.Fixed
                        || container.Position.PositionMode == PositionMode.Relative)
                    {
                        foundRelative = true;
                        sz = container.CurrentRegion.TotalBounds.Size;
                        if (container.Position.Padding.IsEmpty == false)
                        {
                            sz.Width += container.Position.Padding.Left + container.Position.Padding.Right;
                        }
                        break;
                    }
                    container = container.Parent as PDFLayoutBlock;
                }

                if (!foundRelative)
                {
                    sz = pg.Size;
                }
            }
            else if(withMode ==  PositionMode.Fixed)
            {
                sz = pg.Size;
            }
            else
            {
                var container = pg.LastOpenBlock();

                if (null != container)
                    sz = container.CurrentRegion.TotalBounds.Size;
                else if (null != pg.CurrentBlock)
                    sz = pg.CurrentBlock.CurrentRegion.TotalBounds.Size;
                else
                    sz = pg.Size;

            }
            return sz;
        }
        
        #endregion

        #region protected virtual void DoLayoutAChild(IPDFComponent comp, PDFStyle full)

        /// <summary>
        /// Based on the interface implementation of the component (Text, Image, Path , etc)
        /// - calls the appropriate layout method
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="full"></param>
        protected virtual void DoLayoutAChild(IComponent comp, Style full)
        {
            PDFLayoutRegion positioned = null;
            PDFPositionOptions options = null;
            bool decrementDepthAfter = false;
            
            //Here we can set up any required regions and then do the layout for the explicit types
            //If we have style then check if are actually relatively or absolutely positioned
            if (IsStyled(comp))
            {
                options = full.CreatePostionOptions(this.Context.PositionDepth > 0);

                if (options.PositionMode == PositionMode.Fixed)
                {
                    positioned = this.BeginNewAbsoluteRegionForChild(options, comp, full);
                    this.Context.PositionDepth += 1;
                    decrementDepthAfter = true;
                }
                else if (options.PositionMode == PositionMode.Absolute)
                {
                    positioned = this.BeginNewRelativeRegionForChild(options, comp, full);
                    this.Context.PositionDepth += 1;
                    decrementDepthAfter = true;
                }
                else if (options.PositionMode == PositionMode.InlineBlock)
                {
                    positioned = this.BeginNewInlineBlockRegionForChild(options, comp, full);
                }
                else if (options.FloatMode != FloatMode.None)
                {
                    positioned = this.BeginNewFloatingRegionForChild(options, comp, full);
                    this.Context.PositionDepth += 1;
                    decrementDepthAfter = true;
                }
            }



            if (comp is IPDFViewPortComponent)
                this.DoLayoutViewPortComponent(comp as IPDFViewPortComponent, full);

            else if (comp is ILayoutBreak)
            {
                ILayoutBreak lb = comp as ILayoutBreak;
                switch (lb.BreakType)
                {
                    case LayoutBreakType.Page:
                        this.DoLayoutPageBreak(comp as Component, full);
                        break;
                    case LayoutBreakType.Column:
                        this.DoLayoutColumnBreak(comp as Component, full);
                        break;
                    case LayoutBreakType.Line:
                        this.DoLayoutLineBreak(lb,full);
                        break;
                    default:
                        throw new IndexOutOfRangeException("IPDFLayoutBreak.BreakType");
                }
            }
            else if (comp is ITextComponent)
            {
                this.DoLayoutTextComponent(comp as ITextComponent, full);
            }
            else if (comp is IPDFImageComponent)
            {
                this.DoLayoutImageComponent(comp as IPDFImageComponent, full);
            }
            else if (comp is IGraphicPathComponent)
            {
                this.DoLayoutPathComponent(comp as IGraphicPathComponent, full);
            }
            else if(comp is ILayoutComponent)
            {
                this.DoLayoutVisualRenderComponent(comp as ILayoutComponent, full);
            }
            else if (comp is IInvisibleContainer)
            {
                this.DoLayoutInvisibleComponent(comp as IInvisibleContainer, full);
            }

            //close any relative or absolute region
            if (null != positioned)
            {
                if (positioned.IsClosed) //we have overflowed and need to get the latest.
                    positioned = this.CurrentBlock.PositionedRegions.Last();


                positioned.Close();

                if (decrementDepthAfter)
                    this.Context.PositionDepth -= 1;

                if (positioned is PDFLayoutPositionedRegion posReg && posReg.AssociatedRun != null)
                    posReg.AssociatedRun.Close();

                if (positioned.Contents.Count == 0 && positioned.Floats == null)
                {
                    if (Context.ShouldLogVerbose)
                        Context.TraceLog.Add(TraceLevel.Verbose, LOG_CATEGORY, "A positioned region was created but there is no content in the region (possible it does not fit), so removing it");
                    positioned.ExcludeFromOutput = true;
                    positioned.TotalBounds = Rect.Empty;
                }
                else if(positioned.PositionMode == PositionMode.Fixed)
                {
                    this.UpdateFixedRegionPosition(positioned, options);
                }
                else if(positioned.PositionMode == PositionMode.Absolute)
                {
                    if (options.FloatMode != FloatMode.None)
                    {
                        positioned = EnsurePositionedOnPage(positioned, options);
                        this.UpdateFloatingRegionPosition(positioned, options);
                    }
                    else
                    {
                        this.UpdateAbsoluteRegionPosition(positioned, options);
                    }
                    
                }
                
                //If we are relative and we some transformations to apply
                if (options.HasTransformation)
                {
                    ApplyRelativeTransformations(positioned, options);
                }
                
                
            }

        }

        #endregion

        #region protected virtual void UpdateFixedRegionPosition(PDFLayoutRegion positioned, PDFPositionOptions options)
        
        protected virtual void UpdateFixedRegionPosition(PDFLayoutRegion positioned, PDFPositionOptions options)
        {
            var pg = this.Context.DocumentLayout.CurrentPage;
            var bounds = positioned.TotalBounds;

            if (options.X.HasValue == false)
            {
                var w = pg.Width;

                if (options.Right.HasValue)
                {
                    //Move to the right
                    w -= (options.Right.Value + positioned.TotalBounds.Width);
                    bounds.X = w;
                }
                else
                {
                    var x = Unit.Zero;
                    var parent = positioned.Parent as PDFLayoutBlock;

                    while (null != parent)
                    {
                        x += parent.Position.Margins.Left + parent.Position.Padding.Left;
                        parent = parent.Parent as PDFLayoutBlock;
                    }
                    bounds.X = x;
                }
            }
            

            if (options.Y.HasValue == false)
            {
                if (options.Bottom.HasValue)
                {
                    var h = pg.Height;
                    h -= (options.Bottom.Value + positioned.TotalBounds.Height);

                    bounds.Y = h;
                }
                else
                {

                    var y = Unit.Zero;
                    var parent = positioned.Parent as PDFLayoutBlock;

                    while(null != parent)
                    {
                        y += parent.Height + parent.Position.Margins.Top + parent.Position.Padding.Top;

                        if (parent.CurrentRegion.CurrentItem is PDFLayoutLine line)
                            y += line.Height;

                        parent = parent.Parent as PDFLayoutBlock;
                    }
                    if(pg.HeaderBlock != null)
                    {
                        y += pg.HeaderBlock.Height;
                    }
                    bounds.Y = y;
                }
            }
            

            //Put the bounds back on to the region.
            positioned.TotalBounds = bounds;
        }

        #endregion
        
        #region protected PDFLayoutBlock GetClosestPositionedParent(PDFLayoutRegion region)
        protected PDFLayoutBlock GetClosestPositionedParent(PDFLayoutRegion region)
        {
            var block = region.GetParentBlock();

            while(null != block)
            {
                if (block.Position.PositionMode == PositionMode.Absolute)
                    return block;
                else if (block.Position.PositionMode == PositionMode.Fixed)
                    return block;
                else if (block.Position.PositionMode == PositionMode.Relative)
                    return block;
                else
                    block = block.GetParentBlock();
            }

            return null;
        }
        
        #endregion
        
        #region protected virtual void UpdateAbsoluteRegionPosition(PDFLayoutRegion region, PDFPositionOptions options)

        
        protected virtual void UpdateAbsoluteRegionPosition(PDFLayoutRegion region, PDFPositionOptions options)
        {
            var positioned = (PDFLayoutPositionedRegion)region;
            var pg = this.Context.DocumentLayout.CurrentPage;
            var bounds = positioned.TotalBounds;
            PDFLayoutBlock relativeTo = null;
            
            var parent = positioned.GetParentBlock();
            var parentOffset = Point.Empty;
            var firstParent = true;
            
            while (parent != null)
            {
                var mode = parent.Position.PositionMode;
                if (mode == PositionMode.Absolute ||
                    mode == PositionMode.Fixed ||
                    mode == PositionMode.Relative)
                {
                    relativeTo = parent;
                    break;
                }
                else
                {
                    parentOffset.Y += parent.CurrentRegion.Height;
                    
                    if (firstParent && parent.CurrentRegion.CurrentItem is PDFLayoutLine line)
                    {  parentOffset.Y += line.Height;}

                    parentOffset.Y += parent.Position.Padding.Top + parent.Position.Margins.Top;
                    parentOffset.X += parent.Position.Padding.Left + parent.Position.Margins.Left;
                    
                    firstParent = false;
                    parent = parent.Parent as PDFLayoutBlock;
                }

            }
            
            var offsetX = Unit.Zero;
            var offsetY = Unit.Zero;
            
            if (null == relativeTo)
            {
                //no positioned parent so we are relative to the structural parent
                //and it's current position - even if we have x and y values
                relativeTo = positioned.GetParentBlock();
                
                offsetX = relativeTo.CurrentRegion.OffsetX;
                offsetY = relativeTo.CurrentRegion.Height;

                if (relativeTo.CurrentRegion.CurrentItem is PDFLayoutLine line)
                    offsetY += line.Height;
                //reset the positioned offset;
                parentOffset = Point.Empty;
            }
            else
            {
                //We have a positioned parent
                
                if (options.Y.HasValue == false && options.Bottom.HasValue == false)
                {
                    //No vertical position - so relative to the positioned regions height
                    //including any current open line.

                    offsetY = relativeTo.CurrentRegion.Height + relativeTo.Position.Margins.Top;
                    var open = relativeTo.CurrentRegion.LastOpenBlock();

                    if (relativeTo.CurrentRegion.CurrentItem is PDFLayoutLine line2)
                        offsetY += line2.Height;
                }

                if (options.X.HasValue == false && options.Right.HasValue == false)
                {
                    //no horizontal position
                    offsetX = relativeTo.CurrentRegion.OffsetX;
                }
            }

            positioned.RelativeTo = relativeTo;
            positioned.RelativeOffset = new Point(offsetX + parentOffset.X, offsetY + parentOffset.Y);
            
        }
        
        #endregion

        #region protected virtual PDFLayoutRegion EnsurePositionedOnPage(PDFLayoutRegion lastPositioned, PDFPositionOptions options)

        /// <summary>
        /// Checks to make sure the positioned region is on the correct page, given rollover to a new page and the original page of the positioned region.
        /// </summary>
        /// <param name="lastPositioned"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected virtual PDFLayoutRegion EnsurePositionedOnPage(PDFLayoutRegion lastPositioned, PDFPositionOptions options)
        {
            var currPg = this.Context.DocumentLayout.CurrentPage;
            var lastPage = lastPositioned.GetLayoutPage();
            if (lastPage == currPg)
                return lastPositioned;
            else
            {
                PDFLayoutPositionedRegion posRegion = (PDFLayoutPositionedRegion)lastPositioned;

                // Chances are we moved to a new column or page
                //More the last positioned to the currPg.LastOpenBlock().PositionedRegions

                var prevParent = posRegion.Parent as PDFLayoutBlock;
                var newParent = currPg.LastOpenBlock();

                prevParent.PositionedRegions.Remove(posRegion);
                newParent.PositionedRegions.Add(posRegion);

                posRegion.SetParent(newParent);

                return lastPositioned; //TODO:Resolve the rolling over of content onto a new page for the float left.
            }
        }

        #endregion

        #region protected virtual void UpdateFloatingRegionPosition(PDFLayoutRegion positioned, PDFPositionOptions pos)

        protected virtual void UpdateFloatingRegionPosition(PDFLayoutRegion region, PDFPositionOptions options)
        {
            var rightOffset = (options.Right ?? Unit.Zero);
            
            //Knock out any explicit position values and set it to absolute.
            options.X = null;
            options.Y = null;
            options.Bottom = null;
            options.Right = null;
            options.PositionMode = PositionMode.Absolute;
            
            var positioned = (PDFLayoutPositionedRegion)region;
            var pg = this.Context.DocumentLayout.CurrentPage;
            var bounds = positioned.TotalBounds;
            PDFLayoutBlock relativeTo = null;
            
            var parent = positioned.GetParentBlock();
            var parentOffset = Point.Empty;
            var firstParent = true;
            
            while (parent != null)
            {
                var mode = parent.Position.PositionMode;
                if (mode == PositionMode.Absolute ||
                    mode == PositionMode.Fixed ||
                    mode == PositionMode.Relative)
                {
                    relativeTo = parent;
                    break;
                }
                else
                {
                    parentOffset.Y += parent.CurrentRegion.Height;
                    
                    //if (firstParent && parent.CurrentRegion.CurrentItem is PDFLayoutLine line)
                    //{  parentOffset.Y += line.Height;}

                    parentOffset.Y += parent.Position.Padding.Top + parent.Position.Margins.Top;
                    parentOffset.X += parent.Position.Padding.Left + parent.Position.Margins.Left;
                    
                    firstParent = false;
                    parent = parent.Parent as PDFLayoutBlock;
                }

            }
            
            var offsetX = Unit.Zero;
            var offsetY = Unit.Zero;
            
            if (null == relativeTo)
            {
                //no positioned parent so we are relative to the structural parent
                //and it's current position - even if we have x and y values
                relativeTo = positioned.GetParentBlock();
                
                offsetX = relativeTo.CurrentRegion.OffsetX;
                offsetY = relativeTo.CurrentRegion.Height;

                if (relativeTo.CurrentRegion.CurrentItem is PDFLayoutLine line)
                    offsetY += line.Height;
                
                //reset the positioned offset;
                parentOffset = Point.Empty;
            }
            else
            {
                //We have a positioned parent
                
                if (options.Y.HasValue == false && options.Bottom.HasValue == false)
                {
                    //No vertical position - so relative to the positioned regions height
                    //including any current open line.

                    offsetY = relativeTo.CurrentRegion.Height;// + relativeTo.Position.Margins.Top;
                    parentOffset.Y += relativeTo.Position.Margins.Top;
                    
                    var open = relativeTo.CurrentRegion.LastOpenBlock();
//                    
//TODO: check available space and if the current line can fit the floating region, move it to that line rather than shift down.
//
                    if (null != open)
                    {
                        if (open.CurrentRegion.CurrentItem is PDFLayoutLine line2)
                            offsetY += line2.Height;
                    }
                    else if (relativeTo.CurrentRegion.CurrentItem is PDFLayoutLine line3)
                        offsetY += line3.Height;
                }

                if (options.X.HasValue == false && options.Right.HasValue == false)
                {
                    //no horizontal position
                    offsetX = relativeTo.CurrentRegion.OffsetX;
                    //parentOffset.X += relativeTo.Position.Padding.Left;
                }
            }

            positioned.RelativeTo = relativeTo;
            positioned.RelativeOffset = new Point(offsetX + parentOffset.X, offsetY + parentOffset.Y);

            if (options.FloatMode == FloatMode.Left)
            {
                rightOffset = relativeTo.CurrentRegion.GetRightInset(offsetY, positioned.Height);
                if (positioned.TotalBounds.X + positioned.TotalBounds.Width + rightOffset >
                    relativeTo.CurrentRegion.TotalBounds.Width)
                {
                    //move down as we cannot fit on the line.
                    var requiredSpace = positioned.TotalBounds.Width;
                    var availableWidth = relativeTo.CurrentRegion.TotalBounds.Width - rightOffset;
                    var maxY = relativeTo.CurrentRegion.GetFloatsMaxVOffset(FloatMode.Left, availableWidth,
                        requiredSpace);
                    offsetY = maxY;
                    offsetX = relativeTo.CurrentRegion.GetLeftInset(offsetY, positioned.Height);
                    positioned.RelativeOffset = new Point(parentOffset.X, offsetY + parentOffset.Y);
                    bounds = positioned.TotalBounds;
                    bounds.X = offsetX;
                    positioned.TotalBounds = bounds;
                }

                if (offsetY + positioned.Height > relativeTo.CurrentRegion.AvailableHeight)
                {
                    PDFLayoutRegion newRegion = relativeTo.CurrentRegion;
                    PDFLayoutBlock newBlock = relativeTo;
                    bool newPage;
                    if (!this.MoveFloatToNextRegion(FloatMode.Left, positioned.TotalBounds.X, positioned, relativeTo))
                    {
                        if (relativeTo.Position.OverflowAction == OverflowAction.Truncate)
                        {
                            positioned.PositionOptions.Visibility = Visibility.Hidden;
                        }
                    }
                }
                else
                {
                    relativeTo.CurrentRegion.AddFloatingInset(options.FloatMode, positioned.TotalBounds.Width,
                        positioned.TotalBounds.X, offsetY, positioned.Height);
                }
            }
            else
            {
                var leftOffset = relativeTo.CurrentRegion.GetLeftInset(offsetY, positioned.Height);
                
                if (rightOffset + positioned.TotalBounds.Width + leftOffset > relativeTo.CurrentRegion.TotalBounds.Width)
                {
                    //move down as we cannot fit on the line.
                    var requiredSpace = positioned.TotalBounds.Width;
                    var availableWidth = relativeTo.CurrentRegion.TotalBounds.Width - leftOffset;
                    Unit maxY;
                    if (availableWidth < requiredSpace) //We have left floats pushing us down.
                        maxY = relativeTo.CurrentRegion.GetFloatsMaxVOffset(FloatMode.Left, availableWidth,
                            requiredSpace);
                    else //we have right floats pushing us down
                        maxY = relativeTo.CurrentRegion.GetFloatsMaxVOffset(FloatMode.Right, availableWidth, requiredSpace);
                    offsetY = maxY;
                    rightOffset = relativeTo.CurrentRegion.GetRightInset(offsetY , positioned.Height);
                    positioned.RelativeOffset = new Point(parentOffset.X, offsetY + parentOffset.Y);
                    bounds = positioned.TotalBounds;
                    bounds.X = offsetX;
                    positioned.TotalBounds =  bounds;
                }
                
                positioned.PositionOptions.Right = rightOffset;
                
                if (offsetY + positioned.Height > relativeTo.CurrentRegion.AvailableHeight)
                {
                    PDFLayoutRegion newRegion = relativeTo.CurrentRegion;
                    PDFLayoutBlock newBlock = relativeTo;
                    bool newPage;
                    if (!this.MoveFloatToNextRegion(FloatMode.Right, rightOffset, positioned, relativeTo))
                    {
                        if (relativeTo.Position.OverflowAction == OverflowAction.Truncate)
                        {
                            positioned.PositionOptions.Visibility = Visibility.Hidden;
                        }
                    }
                }
                else
                {
                    relativeTo.CurrentRegion.AddFloatingInset(options.FloatMode, positioned.TotalBounds.Width,
                        rightOffset, offsetY, positioned.Height);
                }
            }
        }

        private bool MoveFloatToNextRegion(FloatMode floatMode, Unit floatInset, PDFLayoutPositionedRegion positioned, PDFLayoutBlock relativeTo)
        {
            var newRegion = relativeTo.CurrentRegion;
            var newBlock = relativeTo;
            bool newPage;
            //POSTITIONED REGIONS IN THE HIERARCHY WILL NEVER FOARCE A NEW PAGE, OR COLUMN
            if (this.MoveToNextRegion(positioned.Height, ref newRegion, ref newBlock, out newPage))
            {
                newRegion.AddFloatingInset(floatMode, positioned.TotalBounds.Width,
                    floatInset, 0, positioned.Height);
                
                //swap the positioned region
                
                relativeTo.PositionedRegions.Remove(positioned);
                newBlock.PositionedRegions.Add(positioned);
                
                var run = positioned.AssociatedRun;
                run.Line.Runs.Remove(run);
                var line = newRegion.StartOrReturnCurrentLine(PositionMode.Inline);
                line.AddRun(run);
                
                //To do - update the current position.
                positioned.RelativeTo = newBlock;
                var offset = positioned.RelativeOffset;
                offset.Y = newRegion.UsedSize.Height;
                
                positioned.RelativeOffset = offset;
                return true;
            }
            else
                return false;
        }


        private bool TryGetFloatingRegionWidth(PDFLayoutRegion positioned, out Unit width, out bool isImage)
        {
            isImage = false;
            if(positioned.Contents.Count == 0)
            {
                width = -1;
                return false;
            }


            var first = positioned.Contents[0];
            if(first is PDFLayoutBlock)
            {
                width =  (first as PDFLayoutBlock).Width;
                return width > 0;
            }
            else if(first is PDFLayoutLine && positioned.Owner is ImageBase)
            {
                var line = (first as PDFLayoutLine);
                if (line.Runs.Count == 0)
                {
                    //We do have a case where the first line becomes empty (with zero size)
                    //And the image is put on the second line.
                    if (positioned.Contents.Count > 1 && positioned.Contents[1] is PDFLayoutLine)
                        line = positioned.Contents[1] as PDFLayoutLine;

                    //Do a check again for a run
                    if(line.Runs.Count == 0)
                    {
                        Context.TraceLog.Add(TraceLevel.Error, LOG_CATEGORY, "Could not get the width of the positioned region, it has probably moved onto a new page");
                        width = -1;
                        return false;
                    }
                }
                var run = line.Runs[0] as PDFLayoutComponentRun;
                width = run.Width;
                //HACK:The run size for images includes the margins, so do not apply to floats
                isImage = true;
                return width > 0;
            }
            else
            {
                if (this.Context.Conformance == ParserConformanceMode.Strict)
                    throw new InvalidOperationException(Errors.CanOnlyTransformBlockComponents);
                else
                {
                    Context.TraceLog.Add(TraceLevel.Error, LOG_CATEGORY, Errors.CanOnlyTransformBlockComponents);
                    width = -1;
                    return false;
                }
            }
        }

        #endregion

        #region protected virtual void ApplyRelativeTransformations(PDFLayoutRegion positioned, PDFPositionOptions pos)

        /// <summary>
        /// Takes into account any transformations on a relatively positioned elements adjusting the size of the region 
        /// containing the component after the transformation has been applied.
        /// </summary>
        /// <param name="positioned"></param>
        /// <param name="pos"></param>
        protected virtual void ApplyRelativeTransformations(PDFLayoutRegion positioned, PDFPositionOptions pos)
        {
            if (null == pos)
                throw new ArgumentNullException("pos");

            if (null == positioned)
                throw new ArgumentNullException("positioned");

            if (pos.PositionMode == PositionMode.Inline)
                throw new ArgumentOutOfRangeException("pos.PositionMode", "Can only be Relative or Absolute");

            //Get the block that sits in the positioned region
            if (positioned.Contents == null || positioned.Contents.Count != 1)
            {
                if (this.Context.Conformance == ParserConformanceMode.Strict)
                    throw new InvalidOperationException(Errors.CanOnlyTransformBlockComponents);
                else
                {
                    Context.TraceLog.Add(TraceLevel.Error, LOG_CATEGORY, Errors.CanOnlyTransformBlockComponents);
                    return;
                }
            }

            PDFLayoutBlock relBlock = positioned.Contents[0] as PDFLayoutBlock;
            if (null == relBlock)
            {
                if (this.Context.Conformance == ParserConformanceMode.Strict)
                    throw new InvalidOperationException(Errors.CanOnlyTransformBlockComponents);
                else
                {
                    Context.TraceLog.Add(TraceLevel.Error, LOG_CATEGORY, Errors.CanOnlyTransformBlockComponents);
                    return;
                }
            }

            this.Context.TraceLog.Add(TraceLevel.Verbose, LOG_CATEGORY, "Applying the Transformation matrix " + pos.TransformMatrix.ToString() + " to the bounds for block " + relBlock.ToString());

            //We need to adjust the size of the positioned region so it contains the transformed black as much as possible.

            PDFTransformationMatrix matrix = pos.TransformMatrix;

            Rect bounds = new Rect(Point.Empty, relBlock.TotalBounds.Size); //relBlock.TotalBounds;
            Rect transformed = matrix.TransformBounds(bounds, TransformationOrigin.CenterMiddle);

            Unit xpos = transformed.X;
            Unit ypos = transformed.Y;

            Unit height = transformed.Height;
            Unit width = transformed.Width;

            Unit xshift = Unit.Zero;
            Unit yshift = Unit.Zero;

            //move the position to zero and the shift to x - so any transforms will happen at the origin

            xshift = new Unit(-xpos.PointsValue, PageUnits.Points);
            xpos = 0;
            

            if (pos.X.HasValue)
            {
                //We have an explict x so we need to make sure we have that in the shift.
                //and our total width is increased by this.

                xshift += pos.X.Value;
                width += pos.X.Value;
            }

            //move the position to zero and the shift to y
             yshift = new Unit(-ypos.PointsValue, PageUnits.Points);
            ypos = 0;
            

            if (pos.Y.HasValue)
            {
                //again if we have a y value add this.
                yshift += pos.Y.Value;
                height += pos.Y.Value;
            }

            //Set the transformed offset of the block to xshift and yshift (which will be applied in the block rendering.
            relBlock.TransformedOffset = new Point(xshift, yshift);

            //declare the used size as width and height (inc. and explicit positioning)
            positioned.UsedSize = new Size(width, height);

            //and total bounds is also width and height with a zero, zero x and y
            positioned.TotalBounds = new Rect(xpos, ypos, width, height);

        }

        #endregion

        #region BeginNewXXXRegionForChild(posOptions, component, style)

        protected virtual PDFLayoutRegion BeginNewRelativeRegionForChild(PDFPositionOptions pos, IComponent comp, Style full)
        {
            PDFLayoutPage page = this.Context.DocumentLayout.CurrentPage;
            PDFLayoutBlock last = page.LastOpenBlock();
            PDFLayoutRegion rel = last.BeginNewPositionedRegion(pos, page, comp, full, isfloating: false);
            return rel;
        }

        protected virtual PDFLayoutRegion BeginNewAbsoluteRegionForChild(PDFPositionOptions pos, IComponent comp, Style full)
        {
            PDFLayoutPage page = this.Context.DocumentLayout.CurrentPage;
            PDFLayoutBlock last = page.LastOpenBlock();
            PDFLayoutRegion abs = last.BeginNewPositionedRegion(pos, page, comp, full, isfloating: false);
            
            return abs;
        }

        protected virtual PDFLayoutRegion BeginNewInlineBlockRegionForChild(PDFPositionOptions pos, IComponent comp, Style full)
        {
            PDFLayoutPage page = this.Context.DocumentLayout.CurrentPage;
            PDFLayoutBlock last = page.LastOpenBlock();
            PDFLayoutRegion ib = last.BeginNewPositionedRegion(pos, page, comp, full, isfloating: false);
            return ib;
        }


        protected virtual PDFLayoutRegion BeginNewFloatingRegionForChild(PDFPositionOptions pos, IComponent comp, Style full)
        {
            PDFLayoutPage page = this.Context.DocumentLayout.CurrentPage;
            PDFLayoutBlock last = page.LastOpenBlock();
            PDFLayoutRegion region = last.CurrentRegion;
            //Knock out any positioning, and treat the block as absolute.
            //We can then exclude this in the line widths for the parent it is relative to.
            
            pos.X = null;
            pos.Y = null;
            pos.Bottom = null;
            pos.Right = null;
            pos.PositionMode = PositionMode.Absolute;

            if (pos.FloatMode == FloatMode.Left)
            {
                var yoffset = region.UsedSize.Height;
                if (region.CurrentItem is PDFLayoutLine line && !(line.IsClosed) && !line.IsEmpty)
                    yoffset += line.Height;
                pos.X = region.GetLeftInset(yoffset, 1.0);
            }
            else if (pos.FloatMode == FloatMode.Right)
            {
                var yoffset = region.UsedSize.Height;
                if (region.CurrentItem is PDFLayoutLine line && !(line.IsClosed) && !line.IsEmpty)
                    yoffset += line.Height;
                pos.Right = region.GetRightInset(yoffset, 1.0);
            }

            PDFLayoutRegion ib = last.BeginNewPositionedRegion(pos, page, comp, full, isfloating: true);
            return ib;
        }

        #endregion

        //
        // DoLayoutXXX
        //

        #region protected virtual void DoLayoutPageBreak(IPDFLayoutBreak pgbreak, PDFStyle style)

        /// <summary>
        /// Breaks the current page layout and attempts to create a new page.
        /// </summary>
        /// <param name="pgbreak">The declared page break</param>
        /// <param name="style">The full style of the page break</param>
        protected virtual void DoLayoutPageBreak(Component pgbreak, Style style)
        {
            if (pgbreak.Visible == false)
                return;

            PDFLayoutBlock block = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion region = block.CurrentRegion;
            Stack<PDFLayoutBlock> depth = BuildLayoutStack(block);
            if (this.MoveToNextPage(pgbreak, style, depth, ref region, ref block) == false)
            {
                this.ContinueLayout = false;
            }
            else
            {
                this.ContinueLayout = true;
                Stack<PDFLayoutBlock> closeorder = new Stack<PDFLayoutBlock>(depth.Reverse());

                //We need to close from the inner to the outer
                while (closeorder.Count > 0)
                {
                    PDFLayoutBlock toclose = closeorder.Pop();
                    if (!toclose.IsClosed)
                        toclose.Close();
                }

                //and then start new blocks on the next page from outer to inner
                PDFLayoutBlock current = block;
                while (depth.Count > 0)
                {
                    PDFLayoutBlock child = depth.Pop();
                    if (child.Owner is Page)
                        continue;
                    else
                        current = child.Engine.CloseCurrentBlockAndStartNewInRegion(child, current.CurrentRegion);
                }
                region = current.CurrentRegion;
                this.CurrentBlock = current;
                block = current;
            }
        }

        /// <summary>
        /// Just builds the stack of layouts from our 
        /// current position to the top of the layout.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private static Stack<PDFLayoutBlock> BuildLayoutStack(PDFLayoutBlock block)
        {
            Stack<PDFLayoutBlock> depth = new Stack<PDFLayoutBlock>();
            PDFLayoutBlock curr = block;
            while (null != curr)
            {
                depth.Push(curr);
                curr = curr.Parent as PDFLayoutBlock;
            }
            return depth;
        }

        #endregion

        #region protected virtual void DoLayoutColumnBreak(Component colbreak, PDFStyle style)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colbreak"></param>
        /// <param name="style"></param>
        protected virtual void DoLayoutColumnBreak(Component colbreak, Style style)
        {
            if (colbreak.Visible == false)
                return;

            PDFLayoutBlock block = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion region = block.CurrentRegion;
            bool newpage;
            if (this.CanSplitOrHasMoreRegions(block))
            {
                if (!this.MoveToNextRegion(Unit.Zero, ref region, ref block, out newpage))
                {
                    this.ContinueLayout = false;
                }
            }
            else
                this.ContinueLayout = false;
        }

        /// <summary>
        /// Only for Explict column breaks. Checks to see if the current block cannot be split. If it can't and there are not more regions in the block, then it will return false.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool CanSplitOrHasMoreRegions(PDFLayoutBlock block)
        {
            if (block.Position.OverflowSplit == OverflowSplit.Never
                && block.CurrentRegion == block.Columns[block.ColumnOptions.ColumnCount - 1])
                return false;

            else
                return true;

        }

        #endregion

        #region protected virtual void DoLayoutLineBreak(IPDFLineBreak colbreak, PDFStyle style)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linebreak"></param>
        /// <param name="style"></param>
        protected virtual void DoLayoutLineBreak(ILayoutBreak linebreak, Style style)
        {
            if (linebreak is Component && !((Component)linebreak).Visible)
                return;
            const double DefaultLineLeading = 1.2;
            const double DefaultFontSize = 16;

            PDFLayoutBlock block = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion region = block.CurrentRegion;
            if (region.IsClosed == false)
            {
                if (region.HasOpenItem == false)
                {
                    PDFTextRenderOptions txtopts = style.CreateTextOptions();
                    Unit height;
                    if (txtopts.Leading.HasValue)
                        height = txtopts.Leading.Value;
                    else if (txtopts.Font != null && txtopts.Font.FontMetrics != null)
                        height = txtopts.Font.FontMetrics.TotalLineHeight;
                    else if (txtopts.Font != null)
                        height = txtopts.Font.Size * DefaultLineLeading;
                    else
                        height = DefaultFontSize * DefaultLineLeading;

                    bool newPage;

                    if (region.AvailableHeight < height)
                    {
                        if (this.IsLastInClippedBlock(region))
                        {
                            //We are ok, as we are clipped. Just continue
                        }
                        else if (!this.MoveToNextRegion(height, ref region, ref block, out newPage))
                        {
                            this.ContinueLayout = false;
                            return;
                        }
                    }


                    PDFLayoutLine line = region.BeginNewLine();
                    line.AddRun(new PDFTextRunSpacer(Unit.Zero, height, line, null, true));
                }
                region.CloseCurrentItem();
            }
        }

        #endregion

        #region protected virtual void DoLayoutInvisibleComponent(IPDFInvisibleContainer invisible, PDFStyle style)

        /// <summary>
        /// Laysout the individual components inside the invisible component
        /// </summary>
        /// <param name="invisible"></param>
        /// <param name="style"></param>
        protected virtual void DoLayoutInvisibleComponent(IInvisibleContainer invisible, Style style)
        {
            ComponentList children;

            //As an invisible component we need to put our style as the full style
            //so that inner unstyled components (test literals etc) use it.
            //Styled components will build their own from the full stack

            Style prev = this.FullStyle;
            this.FullStyle = style;

            

            //Extract the collection of child components in this container
            if (TryGetComponentChildren(invisible, out children))
            {
                this.DoLayoutChildren(children);
            }

            //And restore the previous style (could be one or 2 invsible containers nested).
            this.FullStyle = prev;
        }

        #endregion

        #region protected virtual void DoLayoutImageComponent(IPDFImageComponent image, PDFStyle style)

        /// <summary>
        /// Lays out an image component on the current page
        /// </summary>
        /// <param name="image">The image reference to layout</param>
        /// <param name="style">The image style</param>
        protected virtual void DoLayoutImageComponent(IPDFImageComponent image, Style style)
        {
            PDFLayoutBlock block = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion region = block.CurrentRegion;

            //Check the style content for an image source url
            //and if set, then apply it first.

            StyleValue<ContentDescriptor> value;

            if(style.TryGetValue(StyleKeys.ContentTextKey, out value))
            {
                ContentDescriptor current = value.Value(style);
                string imgUrl = null;
                while(null != current)
                {
                    if (current.Type == ContentDescriptorType.Image)
                        imgUrl = (current as ContentImageDescriptor).Source;
                    current = current.Next;
                }

                if (!string.IsNullOrEmpty(imgUrl) && image is Image)
                {
                    (image as Image).Source = imgUrl;
                    (image as Image).RegisterLayoutArtefacts(this.Context, style);                    
                }
            }

            Resources.PDFImageXObject imgx = image.GetImageObject(this.Context, style);

            if (null == imgx)
            {
                if(this.Context.ShouldLogMessage)
                    this.Context.TraceLog.Add(TraceLevel.Message, LOG_CATEGORY, "No image data for component '" + image.ToString() + "'. No layout required.");
                
                return;
            }

            PDFPositionOptions options = style.CreatePostionOptions(this.Context.PositionDepth > 0);

            PDFLayoutLine linetoAddTo = EnsureComponentLineAvailable(options);
            Size avail = new Size(linetoAddTo.AvailableWidth, linetoAddTo.Region.AvailableHeight);

            avail.Width -= options.Margins.Left + options.Margins.Right + options.Padding.Left + options.Padding.Right;
            avail.Height -= options.Margins.Top + options.Margins.Bottom + options.Padding.Top + options.Padding.Bottom;

            //Rects 
            //PDFRect border, content, total;
            //bool hasmargins, haspadding;
            //PDFThickness marginThick, padThick;

            Size sz = image.GetRequiredSizeForLayout(avail, this.Context, style);
            //sz = BuildContentSizes(style, options, sz, out border, out content, out total, out hasmargins, out haspadding, out marginThick, out padThick);

            AddComponentRunToLayoutWithSize(sz, image, style, ref linetoAddTo, options);

            return;
            
        }

        #endregion

        #region protected virtual void DoLayoutPathComponent(IPDFGraphicPathComponent comp, PDFStyle style)

        /// <summary>
        /// Performs the layout and arrangement of path components (implement the IPDFGraphicPathComponent interface)
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="style"></param>
        protected virtual void DoLayoutPathComponent(IGraphicPathComponent comp, Style style)
        {
            PDFPositionOptions options = style.CreatePostionOptions(this.Context.PositionDepth > 0);

            PDFLayoutLine linetoAddTo = EnsureComponentLineAvailable(options);
            Size avail = new Size(linetoAddTo.AvailableWidth, linetoAddTo.Region.AvailableHeight);

            avail.Width -= options.Margins.Left + options.Margins.Right + options.Padding.Left + options.Padding.Right;
            avail.Height -= options.Margins.Top + options.Margins.Bottom + options.Padding.Top + options.Padding.Bottom;

            GraphicsPath path = comp.CreatePath(avail, style);
            comp.Path = path;
            Size required = path.Bounds.Size;

            AddComponentRunToLayoutWithSize(required, comp, style, ref linetoAddTo, options);

        }

        #endregion

        #region protected virtual void DoLayoutTextComponent(IPDFViewPortComponent viewPort, PDFStyle style)

        /// <summary>
        /// Laysout a text component using the engine returned from the GetEngine invocation
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        protected virtual void DoLayoutTextComponent(ITextComponent text, Style style)
        {
            try
            {
                using (IPDFLayoutEngine engine = new Layout.LayoutEngineText(text, this))
                {
                    engine.Layout(this.Context, style);

                    PDFLayoutBlock block = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
                    if (null == block)
                        this.ContinueLayout = false;
                    else if(IsOutsideOfCurrentBlock(block))
                        this.ContinueLayout = false;
                }
            }
            catch (PDFLayoutException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msg = String.Format(Errors.LayoutFailedForComponent, text.ID, ex.Message);
                throw new PDFLayoutException(msg, ex);
            }
        }

        #endregion
        
        #region protected virtual void DoLayoutVisualRenderComponent(IPDFImageComponent image, PDFStyle style)

        /// <summary>
        /// Lays out an component that handles it's own rendering on the current page
        /// </summary>
        /// <param name="comp">The image reference to layout</param>
        /// <param name="style">The image style</param>
        protected virtual void DoLayoutVisualRenderComponent(ILayoutComponent comp, Style style)
        {
            PDFPositionOptions options = style.CreatePostionOptions(this.Context.PositionDepth > 0);

            PDFLayoutLine linetoAddTo = EnsureComponentLineAvailable(options);
            Size avail = new Size(linetoAddTo.AvailableWidth, linetoAddTo.Region.AvailableHeight);

            avail.Width -= options.Margins.Left + options.Margins.Right + options.Padding.Left + options.Padding.Right;
            avail.Height -= options.Margins.Top + options.Margins.Bottom + options.Padding.Top + options.Padding.Bottom;

            Size required = comp.GetRequiredSizeForLayout(avail, this.Context, style);

            AddComponentRunToLayoutWithSize(required, comp, style, ref linetoAddTo, options);

        }

        #endregion

        #region protected virtual void DoLayoutViewPortComponent(IPDFViewPortComponent viewPort, PDFStyle style)

        /// <summary>
        /// Laysout a ViewPort component using the engine returned from the GetEngine invocation
        /// </summary>
        /// <param name="viewPort"></param>
        /// <param name="style"></param>
        protected virtual void DoLayoutViewPortComponent(IPDFViewPortComponent viewPort, Style style)
        {
            try
            {
                using (IPDFLayoutEngine engine = viewPort.GetEngine(this, this.Context, style))
                {
                    engine.Layout(this.Context, style);
                }
                PDFLayoutBlock block = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
                if (null == block)
                    this.ContinueLayout = false;
                else if (IsOutsideOfCurrentBlock(block))
                    this.ContinueLayout = false;
            }
            catch (PDFLayoutException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msg = String.Format(Errors.LayoutFailedForComponent, viewPort.ID, ex.Message);
                throw new PDFLayoutException(msg, ex);
            }
        }

        #endregion

        //
        // overflow methods
        //
        
        #region protected bool IsLastInClippedBlock(PDFLayoutRegion container)

        protected bool IsLastInClippedBlock(PDFLayoutRegion container)
        {
            var parent = container.Parent as PDFLayoutBlock;

            while (null != parent)
            {
                //Do we have multiple columns
                if (parent.Columns.Length > 1)
                {
                    if (parent.CurrentRegion.NextRegion == null)
                    {
                        //we are on the last column
                        if ( parent.Position.OverflowAction == OverflowAction.Clip)
                            return true;
                    }
                    else
                    {
                        //we have more columns to overflow to
                        return false;
                    }
                    
                    
                }
                else if (parent.Position.OverflowAction == OverflowAction.Clip)
                {
                    //we have 1 column and are set to clip
                    return true;
                }
                

                parent = parent.GetParentBlock();
            }
            return false;
        }
        
        #endregion

        #region protected virtual bool IsOutsideOfCurrentBlock(PDFLayoutBlock ablock)
        
        protected virtual bool IsOutsideOfCurrentBlock(PDFLayoutBlock ablock)
        {
            if (ablock.Owner == this.Component)
                return false;

            else if ((ablock.Owner is PDFPageHeader) || (ablock.Owner is PDFPageFooter))
                return false;

            else if (this.FullStyle.Position.PositionMode == PositionMode.Inline)
                return false;

            else
                return true;
        }
        
        #endregion

        #region internal protected virtual bool MoveToNextRegion(ref PDFLayoutRegion region, ref PDFLayoutBlock block, out bool newPage)

        /// <summary>
        /// Attempts to move the cursor to the next available region. Either in the current block, 
        /// or another block in a new page
        /// </summary>
        /// <param name="region">Pass in the current region. Will be set to the next region</param>
        /// <param name="block">Pass in the current block. Will be set to the next block if it has changed</param>
        /// <param name="newPage">Set to true if we are now going to be working on another page</param>
        /// <returns>True is we can move to another page.</returns>
        internal protected virtual bool MoveToNextRegion(Unit requiredHeight, ref PDFLayoutRegion region, ref PDFLayoutBlock block, out bool newPage)
        {

            using (IDisposable record = Context.PerformanceMonitor.Record(PerformanceMonitorType.Content_Overflow, this.Component.UniqueID))
            {

                PDFLayoutRegion origRegion = region;
                PDFLayoutBlock origBlock = block;

                const string logCategory = "Region Overflow";

                //We do not overflow relative or absolute regions
                if (!this.CanOverflowFromCurrentRegion(region))
                {

                    if (this.Context.ShouldLogDebug)
                        this.Context.TraceLog.Add(TraceLevel.Debug, logCategory, "Current region does not support overflow - probably becuase it is relatively or absolutely positioned");

                    newPage = false;
                    return false;
                }

                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Begin(TraceLevel.Verbose, logCategory, "Starting the overflow to a new page or region");

                Stack<PDFLayoutBlock> reverseBlocks = new Stack<PDFLayoutBlock>();
                PDFLayoutBlock tomove = null;

                PDFLayoutBlock current = block;

                while (current != null)
                {
                    PDFLayoutRegion currentRegion = current.CurrentRegion; //hold a reference as MoveToNextRegion will loose this.

                    if (current.MoveToNextRegion(requiredHeight, this.Context))
                    {
                        if (this.Context.ShouldLogDebug)
                            this.Context.TraceLog.End(TraceLevel.Debug, logCategory, "Current block " + current + " has another region it can flow into. Moving to this region");

                        this.PushBlockStackOntoNewRegion(reverseBlocks, tomove, current, currentRegion, ref region, ref block);
                        newPage = false;
                        return true;
                    }
                    else if ((current.Position.Height.HasValue || current.Position.MaximumHeight.HasValue) //we are past the available height and are locked from overflowing further
                        && !(current.Owner is Page)) //unless we are a page - where the height can be set, but we can also overflow.
                    {
                        if (current.CurrentRegion.IsClosed == false)
                            current.CurrentRegion.Close();

                        if (this.Context.ShouldLogDebug)
                            this.Context.TraceLog.End(TraceLevel.Debug, logCategory, "Current block " + current + " has a fixed height. Cannot overflow");

                        this.ContinueLayout = false;
                        newPage = false;


                        return false;
                    }
                    else if (current.OverflowSplit == OverflowSplit.Any) //this component can be split so we are good to push it on the stack and continue on
                    {
                        reverseBlocks.Push(current);
                        current = current.Parent as PDFLayoutBlock;
                    }
                    else if (current.OverflowSplit == OverflowSplit.Never)
                    {
                        tomove = current;

                        if (reverseBlocks.Count > 0)
                            reverseBlocks.Clear();

                        current = current.Parent as PDFLayoutBlock;
                    }
                    else
                        throw new IndexOutOfRangeException("The overflow split value " + current.OverflowSplit + " is invalid or not accounted for");
                }

                //
                //We don't have any regions to move to, so we need to move to a new layout page if possible.
                //

                if (null == tomove && region.HasOpenItem)
                    region.CloseCurrentItem();

                if (null == tomove)
                {
                    if (region.IsClosed == false)
                        region.Close();
                    if (block.IsClosed == false)
                        block.Close();
                }


                if (tomove != null)
                {
                    if(tomove.Parent is PDFLayoutPage)
                    {
                        //We are at the top, and these cannot be moved.
                        newPage = false;
                        return false;
                    }

                    //remove the block that we have to move from it's parent and make sure it is open if it was before.
                    (tomove.Parent as PDFLayoutBlock).CurrentRegion.RemoveLastItem();
                }

                PDFLayoutBlock top = reverseBlocks.Pop(); //Get the top level block.
                newPage = top.Engine.MoveToNextPage(this.Component, this.FullStyle, reverseBlocks, ref region, ref block);


                if (newPage)
                {


                    current = block;
                    while (reverseBlocks.Count > 0)
                    {

                        PDFLayoutBlock child = reverseBlocks.Pop();
                        if (this.Context.ShouldLogDebug)
                            this.Context.TraceLog.Add(TraceLevel.Debug, logCategory, "Closing inner block '" + child + "' and starting a new region");
                        current = child.Engine.CloseCurrentBlockAndStartNewInRegion(child, current.CurrentRegion);
                    }


                    region = current.CurrentRegion;
                    block = current;

                    if (null != tomove)
                    {
                        if (this.Context.ShouldLogDebug)
                            this.Context.TraceLog.Add(TraceLevel.Debug, logCategory, "Have existing block '" + tomove + "' to move to the next region");

                        //and then add it back into the new region.
                        Unit origHeight = region.UsedSize.Height;
                        region.AddExistingItem(tomove);
                        tomove.ResetAvailableHeight(region.AvailableHeight - origHeight, true);
                        //Because we have moved the current block onto a new page - we are back with the original region and block
                        region = origRegion;
                        block = origBlock;
                    }

                }

                if (newPage)
                {
                    if (this.Context.ShouldLogDebug)
                        this.Context.TraceLog.End(TraceLevel.Verbose, logCategory, "Completed the overflow to a new page");
                    else if (this.Context.ShouldLogVerbose)
                        this.Context.TraceLog.Add(TraceLevel.Verbose, logCategory, "Completed the overflow and now on a new page");

                }
                else
                {
                    if (this.Context.ShouldLogDebug)
                        this.Context.TraceLog.End(TraceLevel.Verbose, logCategory, "No new page available to overflow");
                    else if (this.Context.ShouldLogVerbose)
                        this.Context.TraceLog.Add(TraceLevel.Verbose, logCategory, "No new page available to overflow");
                }


                return newPage;
            }

        }

        /// <summary>
        /// Checks the provided region to see if there is support for overflow
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        /// <remarks>The base method returns false if the region is set as absolute or relative</remarks>
        protected virtual bool CanOverflowFromCurrentRegion(PDFLayoutRegion region)
        {
            if (region.PositionMode == PositionMode.Absolute || region.PositionMode == PositionMode.Fixed) // || region.PositionMode == PositionMode.Relative)
                return false;
            else if (region.PositionMode == PositionMode.Relative)
                return true;
            else
                return true;
        }

        #endregion

        #region protected virtual void PushBlockStackOntoNewRegion(Stack<PDFLayoutBlock> stack, PDFLayoutBlock tomove, PDFLayoutBlock current, PDFLayoutRegion currentRegion, ref PDFLayoutRegion region, ref PDFLayoutBlock block)

        /// <summary>
        /// Creates an new set of blocks and regions based on the current heirarchy in the stack.
        /// Sets the block and region to the new lowest block and region after moving
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="tomove"></param>
        /// <param name="current"></param>
        /// <param name="currentRegion"></param>
        /// <param name="region"></param>
        /// <param name="block"></param>
        protected virtual void PushBlockStackOntoNewRegion(Stack<PDFLayoutBlock> stack, PDFLayoutBlock tomove, PDFLayoutBlock current, PDFLayoutRegion currentRegion, ref PDFLayoutRegion region, ref PDFLayoutBlock block)
        {

            const string logCategory = "Region Overflow";
            PDFLayoutPositionedRegion posRegion = null;
            bool posIsOpen = false;

            if (region.IsClosed == false && (null == tomove))//|| region != tomove.CurrentRegion))
                region.Close();

            if (tomove != null)
            {
                //remove the block that we have to move from it's parent so we don't close it
                if (currentRegion.Contents.Count > 0 && currentRegion.Contents[currentRegion.Contents.Count - 1] == tomove)
                    currentRegion.RemoveLastItem();
                else
                {
                    PDFLayoutRegion parentRegion = (tomove.Parent as PDFLayoutBlock).CurrentRegion;
                    if (parentRegion.Contents.Count > 0 && parentRegion.Contents[parentRegion.Contents.Count - 1] == tomove)
                        parentRegion.RemoveLastItem();
                }

                var mode = tomove.Position.PositionMode;

                if(mode == PositionMode.Fixed || mode == PositionMode.Absolute)
                {
                    posRegion = tomove.GetParentBlock().CurrentRegion as PDFLayoutPositionedRegion;
                    posIsOpen = !tomove.IsClosed;
                    posRegion.Contents.Remove(tomove);
                }
            }

            //Close from the inner to the outer
            List<PDFLayoutBlock> toclose = new List<PDFLayoutBlock>(stack);
            for (int i = toclose.Count - 1; i >= 0; i--)
            {
                PDFLayoutBlock child = toclose[i];
                if (!child.IsClosed)
                {
                    child.Close();
                    //If we are the last one then we use the stored currentRegion because it's parent block has already moved to the next region
                    if (i == 0)
                        currentRegion.AddToSize(child);
                    else
                        ((PDFLayoutBlock)child.Parent).CurrentRegion.AddToSize(child);
                }
            }

            //Build in next region from the outer to the inner
            while (stack.Count > 0)
            {

                PDFLayoutBlock child = stack.Pop();
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, logCategory, "Closing inner block '" + child + "' and starting a new region");
                current = child.Engine.CloseCurrentBlockAndStartNewInRegion(child, current.CurrentRegion);
            }


            region = current.CurrentRegion;
            block = current;

            if (null != tomove)
            {
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, logCategory, "Have existing block '" + tomove + "' to move to the next region");

                Unit origheight = region.UsedSize.Height;

                if (null != posRegion)
                {
                    posRegion.Contents.Add(tomove);
                    //we are positioned so move the postioned region, rather than the block iteslf.
                    region.AddExistingPositionedRegion(posRegion, tomove);

                    if (posIsOpen && posRegion.IsClosed)
                        posRegion.ReOpen(includeChildren : true);
                }
                else
                {
                    //add it back into the new region
                    region.AddExistingItem(tomove);
                }
                tomove.ResetAvailableHeight(region.AvailableHeight - origheight, true);
            }

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.End(TraceLevel.Verbose, logCategory, "Completed the overflow to a new region");
            else if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Add(TraceLevel.Verbose, logCategory, "Completed the overflow to a new region");


        }

        #endregion

        #region public virtual PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)

        /// <summary>
        /// Closes the current block and starts a new block in the current region
        /// </summary>
        /// <param name="blockToClose"></param>
        /// <param name="joinToRegion"></param>
        /// <returns></returns>
        public virtual PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
        {
            if (!blockToClose.IsClosed)
            {
                blockToClose.Close();
            }

            int repeatindex = blockToClose.BlockRepeatIndex + 1;

            PDFPositionOptions position = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);
            PDFColumnOptions options = this.FullStyle.CreateColumnOptions();

            Rect total = new Rect(new Point(0, joinToRegion.Height), joinToRegion.UnusedBounds.Size);

            if (position.Width.HasValue)
                total.Width = position.Width.Value;

            PDFLayoutBlock block = ((PDFLayoutBlock)joinToRegion.Parent).BeginNewBlock(this.Component, this, this.FullStyle, position.PositionMode);
            block.InitRegions(total, position, options, this.Context);
            block.BlockRepeatIndex = repeatindex;
            this.CurrentBlock = block;

            return block;
        }

        #endregion

        #region public virtual bool MoveToNextPage(Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region, ref PDFLayoutBlock block)

        /// <summary>
        /// Simply calls Move to next page on the parent engine
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="region"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public virtual bool MoveToNextPage(IComponent initiator, Style inititorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region, ref PDFLayoutBlock block)
        {
            return this.ParentEngine.MoveToNextPage(initiator, inititorStyle, depth, ref region, ref block);
        }

        #endregion

        //
        // support methods
        //

        #region protected PDFLayoutLine EnsureComponentLineAvailable(PDFPositionOptions options)

        /// <summary>
        /// Makes sure there is a line available for the layout of a component based on the required position mode, and returns it.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected PDFLayoutLine EnsureComponentLineAvailable(PDFPositionOptions options)
        {
            PDFLayoutBlock block = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion region = block.CurrentRegion;
            PDFLayoutLine linetoAddTo = null;
            PositionMode mode = options.PositionMode;

            if (region.HasOpenItem)
            {
                if (mode == PositionMode.Block)
                {
                    region.CloseCurrentItem();
                    linetoAddTo = region.BeginNewLine();
                }
                else if (!(region.CurrentItem is PDFLayoutLine))
                {
                    region.CloseCurrentItem();
                    linetoAddTo = region.BeginNewLine();
                }
                else
                    linetoAddTo = region.CurrentItem as PDFLayoutLine;
            }
            if (null == linetoAddTo)
                linetoAddTo = region.BeginNewLine();

            return linetoAddTo;
        }

        #endregion

        #region  protected bool AddComponentRunToLayoutWithSize(PDFSize required, IPDFComponent component, PDFStyle style, ref PDFLayoutLine linetoAddTo, PDFPositionOptions options)

        /// <summary>
        /// Support method that based on the required size of the component, 
        /// it will add a new component run to the layout applying margins, padding and
        /// overflowing onto a new region as required / supported.
        /// </summary>
        /// <param name="component">The component to add to the run.</param>
        /// <param name="style">The style associated with the component</param>
        /// <param name="required">The size required by the component for rendering.</param>
        /// <param name="linetoAddTo">The current line in the layout.</param>
        /// <param name="options">The position options</param>
        /// <returns>True if the component could be added (i.e. there was enough space) or false.</returns>
        protected bool AddComponentRunToLayoutWithSize(Size required, IComponent component, Style style, ref PDFLayoutLine linetoAddTo, PDFPositionOptions options, bool isInternalCall = false)
        {
            Rect content = new Rect(
                options.Padding.Top + options.Margins.Top,
                options.Padding.Left + options.Margins.Left,
                required.Width,
                required.Height);

            Rect border = new Rect(
                options.Margins.Top,
                options.Margins.Left,
                required.Width + options.Padding.Left + options.Padding.Right,
                required.Height + options.Padding.Top + options.Padding.Bottom);

            Rect total = new Rect(
                0,
                0,
                border.Width + options.Margins.Left + options.Margins.Right,
                border.Height + options.Margins.Top + options.Margins.Bottom);

            //Check that the new line height will fit of the page
            bool canfitVertical = linetoAddTo.Region.AvailableHeight >= total.Height;
            
            //If not - check that we can overflow onto new regions and pages
            if (!canfitVertical && !isInternalCall && this.ContainerCanOverflow(options))
            {
                linetoAddTo.Close();
                bool newPage;
                PDFLayoutRegion region = linetoAddTo.Region;
                PDFLayoutRegion orig = region;
                PDFLayoutBlock block = region.Parent as PDFLayoutBlock;

                //If we can overflow then move the line onto a new column page.
                if (this.MoveToNextRegion(total.Height, ref region, ref block, out newPage))
                {
                    if (region == orig)
                        region = block.CurrentRegion;
                    
                    linetoAddTo = region.BeginNewLine();
                }
                else
                {
                    if (options.OverflowAction == OverflowAction.Truncate)
                        region.AssertRemoveLastItem(linetoAddTo);

                    this.ContinueLayout = false;
                    return false;
                }
            }

            if (options.PositionMode != PositionMode.Inline)
            {
                if (component is IPDFImageComponent && !canfitVertical) //Did we overflow? If so recalculate an image size
                {
                    if(isInternalCall) //We are doing it again, so we should fail
                    {
                        this.ContinueLayout = false;
                        return false;  
                    }

                    content.Width = linetoAddTo.AvailableWidth;
                    content.Height = linetoAddTo.Region.AvailableHeight;
                    required = ((IPDFImageComponent)component).GetRequiredSizeForLayout(content.Size, this.Context, style);
                    return this.AddComponentRunToLayoutWithSize(required, component, style, ref linetoAddTo, options, true);
                }

                linetoAddTo.AddComponentRun(component, total, border, content, total.Height, options, style);
                //Close the current line because we are not inline

                linetoAddTo.Region.CloseCurrentItem();
            }
            else
            {
                //We are inline and we need to calculate the baseline offset based on the fact
                //that the image should sit on the baseline and push the text down so it can fit
                //but also obey the leading rules

                PDFTextRenderOptions txtOpts = style.CreateTextOptions();

                Unit baselineoffset = content.Height + options.Padding.Top + options.Padding.Bottom + options.Margins.Top + options.Margins.Bottom;


                Unit descenderHeight = txtOpts.GetDescender();

                if (txtOpts.Leading.HasValue && txtOpts.Leading.Value - descenderHeight > baselineoffset)
                    baselineoffset = txtOpts.Leading.Value - descenderHeight;

                //if (total.Height < baselineoffset + descenderHeight)
                //    total.Height = baselineoffset + descenderHeight;

                linetoAddTo.AddComponentRun(component, total, border, content, baselineoffset, options, style);
            }

            if (component is ILayoutComponent)
                ((ILayoutComponent)component).SetRenderSizes(content, border, total, style);

            return true;
        }

        #endregion

        #region protected void RegisterChildLayout(IPDFComponent comp)

        /// <summary>
        /// When a child component has completed it's layout then this is called to register it's completion
        /// </summary>
        /// <param name="comp">The component that was laid out</param>
        protected virtual void RegisterChildLayout(IComponent comp)
        {
            if (this.CurrentBlock != null)
                this.CurrentBlock.ChildComponents.Add(comp);
        }

        #endregion

        #region protected virtual bool ContainerCanOverflow(PDFPositionOptions options)

        /// <summary>
        /// Returns true if the current overflow action allows this container to overflow onto a new region.
        /// </summary>
        /// <param name="options">The current position options</param>
        /// <returns></returns>
        protected virtual bool ContainerCanOverflow(PDFPositionOptions options)
        {
            if (options.OverflowAction == OverflowAction.Clip)
                return false;
            else
                return true;
        }

        #endregion

        #region private PDFSize BuildContentSizes(......)

        /// <summary>
        /// Based on the style and position options this method generates the required size 
        /// of an element along with - 1. the total, border, content rectangles. 
        /// 2. The margins and padding around the element
        /// </summary>
        /// <param name="style"></param>
        /// <param name="options"></param>
        /// <param name="origsize"></param>
        /// <param name="border"></param>
        /// <param name="content"></param>
        /// <param name="total"></param>
        /// <param name="hasmargins"></param>
        /// <param name="haspadding"></param>
        /// <param name="marginThick"></param>
        /// <param name="padThick"></param>
        /// <returns></returns>
        protected Size BuildContentSizes(Style style, PDFPositionOptions options, Size origsize, out Rect border, out Rect content, out Rect total, out bool hasmargins, out bool haspadding, out Thickness marginThick, out Thickness padThick)
        {

            //Extract the margins and padding from the options

            marginThick = options.Margins;
            padThick = options.Padding;
            hasmargins = !marginThick.IsEmpty;
            haspadding = !padThick.IsEmpty;

            //Create the image size based on any defined width and/or height.
            if (options.Width.HasValue && options.Height.HasValue)
                origsize = new Size(options.Width.Value, options.Height.Value);

            else if (options.Width.HasValue)
            {
                if (origsize.Width > 0)
                    origsize = new Size(options.Width.Value, origsize.Height * (options.Width.Value.PointsValue / origsize.Width.PointsValue));
                else
                    origsize = new Size(options.Width.Value, 0);
            }
            else if (options.Height.HasValue)
            {
                if (origsize.Height > 0)
                    origsize = new Size(origsize.Width * (options.Height.Value.PointsValue / origsize.Height.PointsValue), options.Height.Value);
                else
                    origsize = new Size(0, options.Height.Value);
            }

            content = new Rect(marginThick.Top + padThick.Top, marginThick.Left + padThick.Left, origsize.Width, origsize.Height);
            border = new Rect(marginThick.Top, marginThick.Left, origsize.Width + padThick.Left + padThick.Right, origsize.Height + padThick.Top + padThick.Right);
            total = new Rect(0, 0, border.Width + marginThick.Left + marginThick.Right, border.Height + marginThick.Top + marginThick.Bottom);
            return origsize;

        }

        #endregion

        #region private PDFLayoutLine GetOpenLine(PDFUnit requiredwidth, PDFLayoutRegion region, PositionMode mode)

        /// <summary>
        /// Gets the next open line to append content to
        /// </summary>
        /// <param name="requiredwidth"></param>
        /// <param name="region"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        protected PDFLayoutLine GetOpenLine(Unit requiredwidth, PDFLayoutRegion region, PositionMode mode)
        {
            PDFLayoutLine linetoAddTo;
            switch (mode)
            {
                case PositionMode.Absolute:
                case PositionMode.Relative:
                case PositionMode.Block:
                    // Close the current line and start a new one
                    if (region.HasOpenItem)
                        region.CloseCurrentItem();
                    linetoAddTo = region.BeginNewLine();
                    break;

                case PositionMode.Inline:
                    //Check that the width fits on the current line
                    if (region.HasOpenItem && region.CurrentItem is PDFLayoutLine)
                    {
                        linetoAddTo = region.CurrentItem as PDFLayoutLine;

                        //If not then close the current line and add start a new line
                        if (requiredwidth > Unit.Zero && linetoAddTo.CanFitWidth(requiredwidth) == false)
                        {
                            region.CloseCurrentItem();
                            linetoAddTo = region.BeginNewLine();
                        }
                    }
                    else
                        linetoAddTo = region.BeginNewLine();

                    break;
                default:
                    throw new ArgumentOutOfRangeException("PositionMode");
            }
            return linetoAddTo;
        }

        #endregion

        #region protected void InitBlock(PDFLayoutBlock block, PDFRect bounds, PDFStyle style)

        /// <summary>
        /// Initializes the margins, padding etc for a block
        /// </summary>
        /// <param name="block"></param>
        /// <param name="bounds"></param>
        /// <param name="style"></param>
        protected void InitBlock(PDFLayoutBlock block, Rect bounds, Style style)
        {
            PDFPositionOptions options = style.CreatePostionOptions(this.Context.PositionDepth > 0);
            PDFColumnOptions columns = style.CreateColumnOptions();
            
            block.InitRegions(bounds, options, columns, this.Context);
        }

        #endregion

        #region protected PDFRect GetContentRectFromBounds(PDFRect bounds ...)

        /// <summary>
        /// Calculates the available content rect of this block based on the bounds
        /// </summary>
        /// <param name="bounds">The total available size</param>
        /// <param name="margthick">set to the applied margins</param>
        /// <param name="padthick">set to the applied padding</param>
        /// <returns>The available content rectangle</returns>
        protected Rect GetContentRectFromBounds(Rect bounds, Thickness margthick, Thickness padthick)
        {
            Rect contentrect = bounds;

            if (margthick.IsEmpty == false)
            {
                contentrect = contentrect.Inset(margthick);
            }

            if (padthick.IsEmpty == false)
            {
                contentrect = contentrect.Inset(padthick);
            }

            return contentrect;
        }

        #endregion

        
        //
        // disposal
        //

        #region public void Dispose()

        /// <summary>
        /// Interaface implementation of the IDisposable interface
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion

        #region protected virtual void Dispose(bool disposing)

        /// <summary>
        /// Inheritors can override this method to dispose of any unmanaged resources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            
        }

        #endregion

        #region protected virtual void DoReset(PDFContainerComponent container, IPDFLayoutEngine parent)

        /// <summary>
        /// Clears out any existing logic and assigns new instance variables so the engine can be reused
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parent"></param>
        protected virtual void DoReset(ContainerComponent container, IPDFLayoutEngine parent)
        {
            this._component = container;
            this._currentBlock = null;
            this._style = null;
            this.ParentEngine = parent;
        }

        #endregion
    }
}
