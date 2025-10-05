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
using System.Threading.Tasks;
using System.Xml.Xsl;

using Scryber;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.PDF;
using Scryber.PDF.Resources;
using Scryber.Svg;
using Scryber.Text;

namespace Scryber.Styles
{
    /// <summary>
    /// Abstract base class for all full styles (contains the StyleItems, AND the values associated with those items).
    /// </summary>
    public abstract class StyleBase : TypedObject
    {
        private const int DirectFillFactor = 5;
        private const int InheritedFillFactor = 5;

        private StyleValueDictionary _direct = null;
        private StyleValueDictionary _inherited = null;
        private StyleItemCollection _items = null;

        #region public bool Immutable {get;set;}

        private bool _isimmutable;

        
        /// <summary>
        /// Gets or sets the flag that marks this style as immutable
        /// </summary>
        public bool Immutable
        {
            get { return this._isimmutable; }
            set { this._isimmutable = value; }
        }

        #endregion

        #region public bool HasValues {get;}

        /// <summary>
        /// Returns true if this style has one or more values
        /// </summary>
        public bool HasValues
        {
            get
            {
                if (null != _direct && _direct.Count > 0)
                    return true;
                else if (null != _inherited && _inherited.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region public PDFStyleItemCollection Items

        /// <summary>
        /// Gets the collection of StyleItems that have been instaniated 
        /// (There may be style values that are defined in this Style which do not have a corresponding StyleItem)
        /// </summary>
        public virtual StyleItemCollection StyleItems
        {
            get
            {
                if (null == _items)
                    _items = new StyleItemCollection(this);
                return _items;
            }
        }

        #endregion

        #region protected StyleValueDictionary DirectValues

        /// <summary>
        /// Gets the dictionary of values applied directly to this style
        /// </summary>
        protected StyleValueDictionary DirectValues
        {
            get
            {
                if (null == _direct)
                    _direct = new StyleValueDictionary(DirectFillFactor);
                return _direct;
            }
        }

        #endregion

        #region protected StyleValueDictionary InheritedValues

        /// <summary>
        /// Gets the dictionary of values applied directly to this style
        /// </summary>
        protected StyleValueDictionary InheritedValues
        {
            get
            {
                if (null == _inherited)
                    _inherited = new StyleValueDictionary(InheritedFillFactor);
                return _inherited;
            }
        }

        #endregion

        #region public int ValueCount {get;}

        /// <summary>
        /// Gets the number of Values in this style
        /// </summary>
        public int ValueCount
        {
            get
            {
                int valCount = 0;
                if (null != _inherited)
                    valCount += _inherited.Count;
                if (null != _direct)
                    valCount += _direct.Count;
                return valCount;
            }
        }

        #endregion

        //
        // ctor
        //

        #region protected PDFStyleBase()

        protected StyleBase(ObjectType type)
            : base(type)
        {
            _isimmutable = false;
        }

        #endregion

        //
        // public methods
        //

        #region public virtual void MergeInto(PDFStyleBase style, int priority) + 1 overload

        
        /// <summary>
        /// Merges all the style values in this style into the provided style.
        /// This will overwrite existing values in the provided style.
        /// </summary>
        /// <param name="style"></param>
        public virtual void MergeInto(StyleBase style, int priority)
        {
            if (null == style)
                throw new ArgumentNullException("style");

            style.BeginStyleChange();

            if (null != this._direct && this._direct.Count > 0)
            {
                foreach (KeyValuePair<StyleKey, StyleValueBase> kvp in this._direct)
                {
                    style.DirectValues.SetPriorityValue(kvp.Key, kvp.Value, priority);
                }
            }

            if (null != this._inherited && this._inherited.Count > 0)
            {
                foreach (KeyValuePair<StyleKey, StyleValueBase> kvp in this._inherited)
                {
                    style.InheritedValues.SetPriorityValue(kvp.Key, kvp.Value, priority);
                }
            }

            
        }

        #endregion

        #region public virtual void MergeInto(PDFStyleBase style) + 1 overload


        /// <summary>
        /// Merges all the style values in this style into the provided style.
        /// This will overwrite existing values in the provided style as long as they are higher priority
        /// </summary>
        /// <param name="style"></param>
        public virtual void MergeInto(StyleBase style)
        {
            if (null == style)
                throw new ArgumentNullException("style");

            style.BeginStyleChange();

            if (null != this._direct && this._direct.Count > 0)
            {
                foreach (KeyValuePair<StyleKey, StyleValueBase> kvp in this._direct)
                {
                    style.DirectValues.SetPriorityValue(kvp.Key, kvp.Value, kvp.Value.Priority);
                }
            }

            if (null != this._inherited && this._inherited.Count > 0)
            {
                foreach (KeyValuePair<StyleKey, StyleValueBase> kvp in this._inherited)
                {
                    style.InheritedValues.SetPriorityValue(kvp.Key, kvp.Value, kvp.Value.Priority);
                }
            }
            

        }

        #endregion

        #region public virtual void MergeInto(PDFStyle style, Scryber.IPDFComponent Component, Scryber.ComponentState state)

        /// <summary>
        /// Merges all the style values in this style into the provided style, based on the component.
        /// This will overwrite existing values in the provided style.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="Component"></param>
        public virtual void MergeInto(Style style, Scryber.IComponent Component)
        {
            this.MergeInto(style, 0);
        }

        #endregion

        #region public void MergeInherited(PDFStyle style, Scryber.IPDFComponent Component, bool replace)

        /// <summary>
        /// Merges all the inherited values from this style into the provided style based on the component and replace flag
        /// </summary>
        /// <param name="style"></param>
        /// <param name="Component"></param>
        /// <param name="replace"></param>
        public void MergeInherited(Style style, bool replace, int priority)
        {
            if (null == style)
                throw new ArgumentNullException("style");

            style.BeginStyleChange();

            if(null != this._inherited && this._inherited.Count > 0)
            {
                if (replace)
                {
                    foreach (KeyValuePair<StyleKey, StyleValueBase> kvp in this._inherited)
                    {
                        style.InheritedValues[kvp.Key] = kvp.Value.CloneWithPriority(priority);   
                    }
                }
                else
                {
                    foreach (KeyValuePair<StyleKey, StyleValueBase> kvp in this._inherited)
                    {
                        if (!style.InheritedValues.ContainsKey(kvp.Key))
                            style.InheritedValues.SetPriorityValue(kvp.Key, kvp.Value, priority);
                    }
                }
            }
        }

        #endregion

        

        #region public void Clear()

        /// <summary>
        /// Removes all items from this Style
        /// </summary>
        public void Clear()
        {
            this.BeginStyleChange();
            this.DoClear();
        }

        #endregion

        //
        // protected methods
        //

        #region protected virtual void BeginStyleChange()

        /// <summary>
        /// Makes sure this style can be modified. Throws an invalid operation excpetion if it cannot.
        /// </summary>
        protected virtual void BeginStyleChange()
        {
            if (this.Immutable)
                throw new InvalidOperationException("Cannot modify this style as it is immutable");
        }

        #endregion

        #region protected virtual void DoClear()

        /// <summary>
        /// Removes all items and values from this style
        /// </summary>
        protected virtual void DoClear()
        {
            if (null != _direct)
                this._direct.Clear();
            if (null != _inherited)
                this._inherited.Clear();
            if (null != _items)
                this._items.Clear();
        }

        #endregion


        
        #region protected virtual void DoDataBind(PDFDataContext context, bool includechildren)

        protected virtual void DoDataBind(DataContext context, bool includechildren)
        {

            if (includechildren && this.StyleItems.Count > 0)
            {
                foreach (StyleItemBase item in this.StyleItems)
                {
                    item.DataBind(context);
                }
            }

            if (this.IsValueDefined(StyleKeys.BgImgSrcKey))
                this.EnsureCSSImage(context, StyleKeys.BgImgSrcKey, "background");
            if (this.IsValueDefined(StyleKeys.FillImgSrcKey))
                this.EnsureCSSImage(context, StyleKeys.FillImgSrcKey, "fill");
            if (this.IsValueDefined(StyleKeys.ContentTextKey))
                this.EnsureContentImage(context, this.GetValue(StyleKeys.ContentTextKey, null));
        }

        protected virtual void EnsureContentImage(DataContext context, ContentDescriptor descriptor)
        {
            while(null != descriptor)
            {
                if (descriptor.Type == ContentDescriptorType.Image)
                {
                    var imgDesc = (descriptor as ContentImageDescriptor);
                    var updated = this.EnsureCSSImage(context, imgDesc.Source, "content");
                    imgDesc.Source = updated;
                }

                descriptor = descriptor.Next;
            }
        }

        protected virtual void EnsureCSSImage(DataContext context, StyleKey<string> key, string type)
        {
            var source = this.GetValue(key, "");

            var mapped = EnsureCSSImage(context, source, type);

            this.SetValue(key, mapped);

        }

        protected virtual string EnsureCSSImage(DataContext context, string source, string type)
        {
            

            if(IsGradientImageSrc(source, out var desc))
            {
                return source;
            }
            else if (IsDataImage(source))
            {
                return source;
            }


            var mapped = this.MapPath(source);

            if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, "Styles", "Mapped path for '" + source + "' to '" + mapped + "'");


            IResourceRequester requester = context.Document as IResourceRequester;
            if(null == requester)
            {
                context.TraceLog.Add(TraceLevel.Warning, "Styles", "Cannot pre-load " + type + " images as the document does not support resource requests");
                return source;
            }

            //We just make sure the image is loaded
            var existing = context.Document.GetResource(PDFResource.XObjectResourceType, mapped, true);

            if (context.ShouldLogMessage)
                context.TraceLog.Add(TraceLevel.Message, "Styles", type + " image resource requested for path '" + mapped + "' and " + (existing != null ? existing.ToString() : "nothing") + " returned");

            return mapped;
        }

        #endregion

        public virtual string MapPath(string path)
        {
            return path;
        }

        //
        // implementation
        //

        #region public bool IsValueDefined(StyleKey key)

        /// <summary>
        /// Returns true if this style contains the provided key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsValueDefined(StyleKey key)
        {
            if (null == key)
                throw new ArgumentNullException(nameof(key));
            else if (key.Inherited)
                return (null != this._inherited && this._inherited.ContainsKey(key));
            else
                return (null != this._direct && this._direct.ContainsKey(key));
        }

        #endregion

        #region internal void AddItem(StyleItemBase baseitem)

        /// <summary>
        /// Adds the style item to this styles collection.
        /// NOTE: If the collection already contains an item with the same key then an exception is raised.
        /// </summary>
        /// <param name="baseitem"></param>
        internal void AddItem(StyleItemBase baseitem)
        {
            if (null == baseitem)
                throw new ArgumentNullException("baseitem");

            this.StyleItems.Add(baseitem);
        }

        #endregion

        #region internal void AddValue(StyleValueBase basevalue)

        /// <summary>
        /// Adds the value to this style
        /// </summary>
        /// <param name="baseValue"></param>
        internal void AddValue(StyleValueBase baseValue)
        {
            BeginStyleChange();

            if (null == baseValue)
                throw new ArgumentNullException(nameof(baseValue));
            else if (null == baseValue.Key)
                throw new ArgumentNullException(nameof(baseValue.Key));

            else if (baseValue.Key.Inherited)
                this.InheritedValues.Add(baseValue.Key, baseValue);
            else
                this.DirectValues.Add(baseValue.Key, baseValue);
        }

        #endregion

        #region internal void AddValueRange(IEnumerable<StyleValueBase> all)

        /// <summary>
        /// Adds a range of values to this style
        /// </summary>
        /// <param name="all"></param>
        internal void AddValueRange(IEnumerable<StyleValueBase> all)
        {
            if (null == all)
                return;

            BeginStyleChange();

            foreach (StyleValueBase item in all)
            {
                if (item.Key.Inherited)
                    this.InheritedValues.Add(item.Key, item);
                else
                    this.DirectValues.Add(item.Key, item);
            }
        }

        #endregion

        #region public bool RemoveValue(StyleKey key)

        /// <summary>
        /// Removes any value form this style associated with the specified key. 
        /// Returns true if a value was removed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveValue(StyleKey key)
        {
            BeginStyleChange();

            if (null == key)
                throw new ArgumentNullException("key");

            else if (key.Inherited)
            {
                if (null == _inherited)
                    return false;
                else
                    return _inherited.Remove(key);
            }
            else
            {
                if (null == _direct)
                    return false;
                else
                    return _direct.Remove(key);
            }
        }

        #endregion

        #region public bool RemoveItemStyleValues(PDFStyleKey itemkey)

        /// <summary>
        /// Removes all the style values assocated with the provided item style in this style
        /// </summary>
        /// <param name="itemkey"></param>
        /// <returns></returns>
        public bool RemoveItemStyleValues(StyleKey itemkey)
        {
            bool removed = false;
            if (itemkey.IsItemKey == false)
                throw new InvalidOperationException("itemkey is not actually an item key - use RemoveValue for value keys");

            if (itemkey.Inherited)
            {
                if (null != _inherited && _inherited.Count > 0)
                {
                    List<StyleKey> found = new List<StyleKey>();
                    foreach (KeyValuePair<StyleKey,StyleValueBase> exist in this._inherited)
                    {
                        if (exist.Key.StyleItemKey == itemkey.StyleItemKey)
                            found.Add(exist.Key);
                    }

                    foreach (StyleKey toremove in found)
                    {
                        this._inherited.Remove(toremove);
                        
                    }

                    removed = found.Count > 0;
                }
            }
            else
            {
                if (null != _direct && _direct.Count > 0)
                {
                    List<StyleKey> found = new List<StyleKey>();
                    foreach (KeyValuePair<StyleKey, StyleValueBase> exist in this._direct)
                    {
                        if (exist.Key.StyleItemKey == itemkey.StyleItemKey)
                            found.Add(exist.Key);
                    }

                    foreach (StyleKey toremove in found)
                    {
                        this._direct.Remove(toremove);
                    }

                    removed = found.Count > 0;
                }
            }

            return removed;
        }

        #endregion

        #region internal bool TryGetBaseItem(StyleKey key, out StyleItemBase found)

        /// <summary>
        /// Looks for a StyleItem in this Style, and returns true if one was found.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        internal bool TryGetBaseItem(StyleKey key, out StyleItemBase found)
        {
            if(null == _items)
            {
                found = null;
                return false;
            }
            else
                return _items.TryGetItem(key, out found);
        }

        #endregion

        #region public bool TryGetBaseValue(StyleKey key, out StyleValueBase found)

        /// <summary>
        /// Looks up a StyleValue in this Style, and returns true if one was found.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        public bool TryGetBaseValue(StyleKey key, out StyleValueBase found)
        {
            if (null == key)
                throw new ArgumentNullException(nameof(key));

            if (key.Inherited)
            {
                if(null == _inherited || _inherited.Count == 0)
                {
                    found = null;
                    return false;
                }
                else
                    return _inherited.TryGetValue(key, out found);
            }
            else
            {
                if(null == _direct || _direct.Count == 0)
                {
                    found = null;
                    return false;
                }
                else
                    return _direct.TryGetValue(key, out found);
            }
        }

        #endregion

        #region internal StyleValueBase[] RemoveItemStyleValues(StyleItemBase style)

        /// <summary>
        /// Removes all the style values from this style that belong to the specified style item
        /// </summary>
        /// <param name="styleKey"></param>
        /// <returns></returns>
        internal StyleValueBase[] RemoveAndReturnItemStyleValues(StyleItemBase style)
        {
            List<StyleKey> matching = new List<StyleKey>();
            List<StyleValueBase> values = new List<StyleValueBase>();
            int count = 0;
            if(null == style)
                throw new ArgumentNullException("style");

            Scryber.ObjectType itemkey = style.ItemKey.StyleItemKey;

            if (null != this._direct && this._direct.Count > 0)
            {
                //remove all the style values from the direct dictionary
                //that have the same item key as the provided style item key.
                foreach (KeyValuePair<StyleKey,StyleValueBase> exist in this._direct)
                {
                    if (exist.Key.StyleItemKey == itemkey)
                    {
                        matching.Add(exist.Key);
                        values.Add(exist.Value);
                    }
                }
                if (matching.Count > 0)
                {
                    count += matching.Count;
                    foreach (StyleKey exist in matching)
                    {
                        _direct.Remove(exist);
                    }
                }
            }

            if(null != this._inherited && this._inherited.Count > 0)
            {
                //remove all the style values from the inherited dictionary
                //that have the same item key as the provided style item key.
                
                matching.Clear();

                foreach (KeyValuePair<StyleKey, StyleValueBase> exist in this._inherited)
                {
                    if (exist.Key.StyleItemKey == itemkey)
                    {
                        matching.Add(exist.Key);
                        values.Add(exist.Value);
                    }
                }
                if (matching.Count > 0)
                {
                    count += matching.Count;
                    foreach (StyleKey exist in matching)
                    {
                        _inherited.Remove(exist);
                    }
                }
            }
            if (count > 0)
                return values.ToArray();
            else
                return EmptyStyleValueArray;
        }

        private static readonly StyleValueBase[] EmptyStyleValueArray = new StyleValueBase[] { };

        #endregion

        #region internal Scryber.Drawing.PDFThickness GetThickness(bool inherited, StyleKey all, StyleKey top, StyleKey left, StyleKey bottom, StyleKey right)

        /// <summary>
        /// Based on the provided style keys, builds a new PDFThickness with all values set and returns it.
        /// </summary>
        /// <param name="inherited"></param>
        /// <param name="all"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="bottom"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal bool TryGetThickness(bool inherited, StyleKey<Unit> all, StyleKey<Unit> top, StyleKey<Unit> left, StyleKey<Unit> bottom, StyleKey<Unit> right, out Thickness thickness)
        {
            Dictionary<StyleKey, StyleValueBase> lookup = inherited ? _inherited : _direct;

            thickness = new Scryber.Drawing.Thickness();
            bool hasvalues = false;
            StyleValueBase found;

            if(null == lookup || lookup.Count == 0)
            {
                thickness = Thickness.Empty();
                return false;
            }

            if (lookup.TryGetValue(all, out found))
            {
                thickness.SetAll(((StyleValue<Unit>)found).Value(this));
                hasvalues = true;
            }

            if (lookup.TryGetValue(top, out found))
            {
                thickness.Top = ((StyleValue<Unit>)found).Value(this);
                hasvalues = true;
            }

            if (lookup.TryGetValue(left, out found))
            {
                thickness.Left = ((StyleValue<Unit>)found).Value(this);
                hasvalues = true;
            }

            if (lookup.TryGetValue(bottom, out found))
            {
                thickness.Bottom = ((StyleValue<Unit>)found).Value(this);
                hasvalues = true;
            }

            if (lookup.TryGetValue(right, out found))
            {
                thickness.Right = ((StyleValue<Unit>)found).Value(this);
                hasvalues = true;
            }

            return hasvalues;
        }

        #endregion

        #region internal void SetThickness(bool inherited, Scryber.Drawing.PDFThickness thickness, StyleKey top, StyleKey left, StyleKey bottom, StyleKey right)

        /// <summary>
        /// Based on the specified thickness, sets all the style values in this style to the correct value. 
        /// Replaces any that may already exist
        /// </summary>
        /// <param name="inherited"></param>
        /// <param name="thickness"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="bottom"></param>
        /// <param name="right"></param>
        internal void SetThickness(bool inherited, Scryber.Drawing.Thickness thickness, StyleKey<Unit> top, StyleKey<Unit> left, StyleKey<Unit> bottom, StyleKey<Unit> right)
        {
            Dictionary<StyleKey, StyleValueBase> lookup = inherited ? this.InheritedValues : this.DirectValues;

            lookup[top] = new StyleValue<Unit>(top, thickness.Top);
            lookup[left] = new StyleValue<Scryber.Drawing.Unit>(left, thickness.Left);
            lookup[bottom] = new StyleValue<Scryber.Drawing.Unit>(bottom, thickness.Bottom);
            lookup[right] = new StyleValue<Scryber.Drawing.Unit>(right, thickness.Right);
        }

        #endregion


        //
        // DoCreateXXX methods
        //

        #region internal protected virtual PDFPageSize DoCreatePageSize()

        /// <summary>
        /// Implementation of the PDFPageSize creation. Inheritors can override
        /// </summary>
        /// <returns></returns>
        internal protected virtual PageSize DoCreatePageSize()
        {
            StyleValue<Unit> w;
            StyleValue<Unit> h;

            //We use the explicit Position width and height
            bool hasw = this.TryGetValue(StyleKeys.SizeWidthKey, out w);
            bool hash = this.TryGetValue(StyleKeys.SizeHeightKey, out h);

            //If they are not set then we fall back to the Page width and height
            if (!hasw)
                hasw = this.TryGetValue(StyleKeys.PageWidthKey, out w);
            if (!hash)
                hash = this.TryGetValue(StyleKeys.PageHeightKey, out h);

            if (hasw && hash)
                return new PageSize(new Size(w.Value(this), h.Value(this)));
            else
            {
                //if we don't have any explicit sizes - and we need both we use the paper size
                PaperSize sz;
                PaperOrientation orient;
                StyleValue<PaperSize> szVal;
                StyleValue<PaperOrientation> orientVal;
                if (this.TryGetValue(StyleKeys.PagePaperSizeKey, out szVal))
                    sz = szVal.Value(this);
                else
                    sz = Scryber.Const.DefaultPaperSize;

                if (this.TryGetValue(StyleKeys.PageOrientationKey, out orientVal))
                    orient = orientVal.Value(this);
                else
                    orient = Scryber.Const.DefaultPaperOrientation;

                return new PageSize(sz, orient);
            }
        }

        #endregion

        #region internal protected virtual PDFPositionOptions DoCreatePositionOptions(bool isInPositioned)

        /// <summary>
        /// Implementation of the PDFPositionOptions creation
        /// </summary>
        /// <param name="isInPositioned">Specifies if the position options a being created for a component that is within a positioned region</param>
        /// <returns></returns>
        internal protected virtual PDFPositionOptions DoCreatePositionOptions(bool isInPositioned)
        {
            PDFPositionOptions options = new PDFPositionOptions();
            StyleValue<PositionMode> posmode;
            StyleValue<DisplayMode> dispMode;
            
            StyleValue<bool> xobj;

            if (this.TryGetValue(StyleKeys.PositionModeKey, out posmode))
                options.PositionMode = posmode.Value(this);
            else
                options.PositionMode = PositionMode.Static;

            if (this.TryGetValue(StyleKeys.PositionDisplayKey, out dispMode))
                options.DisplayMode = dispMode.Value(this);
            else
                options.DisplayMode = DisplayMode.Block;
            

            StyleValue<bool> b;
            if (options.PositionMode == PositionMode.Absolute || options.PositionMode == PositionMode.Fixed)
            {
                options.FillWidth = false;
                if (options.DisplayMode == DisplayMode.Inline || options.DisplayMode == DisplayMode.TableCell)
                    options.DisplayMode = DisplayMode.Block;
            }
            else if (isInPositioned)
                options.FillWidth = false;
            else if (options.DisplayMode == DisplayMode.Inline || options.DisplayMode == DisplayMode.InlineBlock)
                options.FillWidth = false;
            else if (this.TryGetValue(StyleKeys.SizeFullWidthKey, out b))
                options.FillWidth = b.Value(this);

            if (this.TryGetValue(StyleKeys.PositionXObjectKey, out xobj))
            {
                options.XObjectRender = xobj.Value(this);
                
                if (options.DisplayMode == DisplayMode.Inline)
                    options.DisplayMode = DisplayMode.InlineBlock; //XObjects should always be either as a block, or as an inline-block.
            }
            else
            {
                options.XObjectRender = false;
            }

            StyleValue<Unit> unit;

            // X
            if (this.TryGetValue(StyleKeys.PositionXKey, out unit))
            {
                options.X = unit.Value(this);

                //if (options.PositionMode != PositionMode.Absolute)
                //    options.PositionMode = PositionMode.Relative;
            }
            else
                options.X = null;

            // Y
            if (this.TryGetValue(StyleKeys.PositionYKey, out unit))
            {
                options.Y = unit.Value(this);

                //if (options.PositionMode != PositionMode.Absolute)
                //    options.PositionMode = PositionMode.Relative;
            }
            else
                options.Y = null;

            if(this.TryGetValue(StyleKeys.PositionRightKey, out unit))
            {
                options.Right = unit.Value(this);
            }
            else
            {
                options.Right = null;
            }

            if(this.TryGetValue(StyleKeys.PositionBottomKey, out unit))
            {
                options.Bottom = unit.Value(this);
            }
            else
            {
                options.Bottom = null;
            }

            // Width
            if (this.TryGetValue(StyleKeys.SizeWidthKey, out unit))
            {
                if (Unit.IsAutoValue(unit.Value(this)))
                    options.FillWidth = true;
                else
                {
                    options.Width = unit.Value(this);
                    options.FillWidth = false;
                }
            }
            else
                options.Width = null;

            // Height
            if (this.TryGetValue(StyleKeys.SizeHeightKey, out unit))
            {
                if (Unit.IsAutoValue(unit.Value(this)))
                {
                    ; //default is to shrink anyway
                }
                else
                {
                    options.Height = unit.Value(this);
                }
            }
            else
                options.Height = null;

            // MinimumWidth
            if (this.TryGetValue(StyleKeys.SizeMinimumWidthKey, out unit))
            {
                options.MinimumWidth = unit.Value(this);
                options.FillWidth = false;
            }
            else
                options.MinimumWidth = null;

            // MinimumHeight
            if (this.TryGetValue(StyleKeys.SizeMinimumHeightKey, out unit))
            {
                options.MinimumHeight = unit.Value(this);
            }
            else
                options.MinimumHeight = null;

            // MaximumWidth
            if (this.TryGetValue(StyleKeys.SizeMaximumWidthKey, out unit))
            {
                options.MaximumWidth = unit.Value(this);
                options.FillWidth = false;
            }
            else
                options.MaximumWidth = null;

            // MaximumHeight
            if (this.TryGetValue(StyleKeys.SizeMaximumHeightKey, out unit))
            {
                options.MaximumHeight = unit.Value(this);
            }
            else
                options.MaximumHeight = null;

            //viewport

            StyleValue<Rect> rect;
            if (this.TryGetValue(StyleKeys.PositionViewPort, out rect))
            {
                options.ViewPort = rect.Value(this);
                
                //As we have a view port - then check the alignment ratios.

                if (this.TryGetValue(StyleKeys.ViewPortAspectRatioStyleKey, out var aspect))
                {
                    options.ViewPortRatio = aspect.Value(this);
                }
            }

            //alignment

            StyleValue<VerticalAlignment> valign;
            if (this.TryGetValue(StyleKeys.PositionVAlignKey, out valign))
                options.VAlign = valign.Value(this);
            //else
            //   options.VAlign = Const.DefaultVerticalAlign;

            StyleValue<HorizontalAlignment> halign;
            StyleValue<TextDirection> direction;

            if (this.TryGetValue(StyleKeys.PositionHAlignKey, out halign))
                options.HAlign = halign.Value(this);
            else if (this.TryGetValue(StyleKeys.TextDirectionKey, out direction) && direction.Value(this) == TextDirection.RTL)
                options.HAlign = HorizontalAlignment.Right;
            else
                options.HAlign = Const.DefaultHorizontalAlign;

            // overflow

            StyleValue<OverflowAction> action;
            bool hasaction;

            if (this.TryGetValue(StyleKeys.OverflowActionKey, out action))
            {
                options.OverflowAction = action.Value(this);
                hasaction = true;
            }
            else
            {
                hasaction = false;
            }

            StyleValue<OverflowSplit> split;
            if (this.TryGetValue(StyleKeys.OverflowSplitKey, out split))
                options.OverflowSplit = split.Value(this);
            else
                options.OverflowSplit = OverflowSplit.Any;

            Thickness thickness;

            //clipping

            if (this.TryGetThickness(StyleKeys.ClipItemKey.Inherited, StyleKeys.ClipAllKey, StyleKeys.ClipTopKey, StyleKeys.ClipLeftKey, StyleKeys.ClipBottomKey, StyleKeys.ClipRightKey, out thickness))
            {
                options.ClipInset = thickness;

                //If the overflow action has not been set, but we have a clipping value, 
                //then we need to set the action to Clip.
                if (!hasaction)
                    options.OverflowAction = OverflowAction.Clip;
            }
            else
                options.ClipInset = Thickness.Empty();
            

            //margins

            if (this.TryGetThickness(StyleKeys.MarginsItemKey.Inherited, StyleKeys.MarginsAllKey,
                    StyleKeys.MarginsTopKey, StyleKeys.MarginsLeftKey, StyleKeys.MarginsBottomKey,
                    StyleKeys.MarginsRightKey, out thickness))
            {
                if (Unit.IsAutoValue(thickness.Top))
                {
                    thickness.Top = Unit.Zero;
                }

                if (Unit.IsAutoValue(thickness.Bottom))
                {
                    thickness.Bottom = Unit.Zero;
                }

                if (Unit.IsAutoValue(thickness.Left))
                {
                    thickness.Left = Unit.Zero;
                    options.AutoMarginLeft = true;
                }

                if (Unit.IsAutoValue(thickness.Right))
                {
                    thickness.Right = Unit.Zero;
                    options.AutoMarginRight = true;
                }
                options.Margins = thickness;
            }
            else
                options.Margins = Thickness.Empty();



            if (options.XObjectRender) 
            {
                //padding, columns, float do not apply to xObject rendering.
            }
            else
            {
                //padding
                if (this.TryGetThickness(StyleKeys.PaddingItemKey.Inherited, StyleKeys.PaddingAllKey,
                        StyleKeys.PaddingTopKey, StyleKeys.PaddingLeftKey, StyleKeys.PaddingBottomKey,
                        StyleKeys.PaddingRightKey, out thickness))
                {
                    options.Padding = thickness;
                }
                else
                    options.Padding = Thickness.Empty();


                //columns

                StyleValue<int> colcount;
                if (this.TryGetValue(StyleKeys.ColumnCountKey, out colcount) && colcount.Value(this) > 0)
                    options.ColumnCount = colcount.Value(this);

                if (this.TryGetValue(StyleKeys.ColumnAlleyKey, out unit))
                    options.AlleyWidth = unit.Value(this);

                // float

                StyleValue<FloatMode> floatm;
                if (this.TryGetValue(StyleKeys.PositionFloat, out floatm))
                {
                    options.FloatMode = floatm.Value(this);
                    options.FillWidth = false;

                    //absolute and fixed knock out any float
                    if (options.PositionMode == PositionMode.Absolute || options.PositionMode == PositionMode.Fixed)
                        options.FloatMode = FloatMode.None;
                    //otherwise if we are actually floating - then we are always a block.
                    else if (options.FloatMode != FloatMode.None)
                        options.DisplayMode = DisplayMode.Block;

                }
            }

            // transformations

            TransformOperationSet transforms = null;
            

            if (this.IsValueDefined(StyleKeys.TransformOperationKey))
            {
                transforms = this.GetValue(StyleKeys.TransformOperationKey, null);
                
                if (null != transforms && transforms.IsIdentity)
                    transforms = null; //identity will do nothing
                

                //otherwise make sure we are positioned as absolute or relative.
                else if (options.PositionMode != PositionMode.Absolute || options.PositionMode != PositionMode.Fixed)
                {
                    options.PositionMode = PositionMode.Absolute;

                    var origin = this.GetValue(StyleKeys.TransformOriginKey, null);
                    if (null != origin)
                    {
                        options.TransformationOrigin = origin;
                    }
                }
            }

            options.Transformations = transforms;

            return options;
        }

        #endregion

        #region internal protected virtual PDFFont DoCreateFont()

        /// <summary>
        /// Creates a PDFFont from this styles value
        /// </summary>
        /// <param name="force">If true, then even if there are no values in the style, a default font will be returned.</param>
        /// <returns></returns>
        internal protected virtual Font DoCreateFont(bool force)
        {
            bool hasvalues = false;
            StyleValue<FontSelector> familyVal;
            StyleValue<Unit> sizeVal;
            StyleValue<int> boldVal;
            StyleValue<Drawing.FontStyle> italicVal;

            FontSelector family;
            Unit size;
            int weight = FontWeights.Regular;
            Drawing.FontStyle style = Drawing.FontStyle.Regular;
            

            if (this.TryGetValue(StyleKeys.FontFamilyKey, out familyVal) && (null != familyVal.Value(this)))
            {
                family = familyVal.Value(this);
                hasvalues = true;
            }
            else
                family = new FontSelector(Const.DefaultFontFamily);

            if (this.TryGetValue(StyleKeys.FontSizeKey, out sizeVal))
            {
                size = sizeVal.Value(this);
                hasvalues = true;
            }
            else
                size = new Unit(Const.DefaultFontSize, PageUnits.Points);

            if (this.TryGetValue(StyleKeys.FontWeightKey, out boldVal))
            {
                hasvalues = true;
                weight = boldVal.Value(this);
            }

            if (this.TryGetValue(StyleKeys.FontStyleKey, out italicVal))
            {
                hasvalues = true;
                style = italicVal.Value(this);
            }

            if (force || hasvalues)
            {
                Font font = new Font(family, size, weight, style);
                return font;
            }
            else
                return null;
            

        }

        #endregion

        #region internal protected virtual PDFPageNumberOptions DoCreatePageNumberOptions()

        /// <summary>
        /// Creates the PDFPageNumberOptions from this styles values
        /// </summary>
        /// <returns></returns>
        internal protected virtual PageNumberOptions DoCreatePageNumberOptions()
        {
            PageNumberOptions opts = new PageNumberOptions();
            StyleValue<PageNumberStyle> style;
            StyleValue<string> grp;
            StyleValue<int> start;
            StyleValue<string> format;
            StyleValue<int> grphint;
            StyleValue<int> totalhint;

            bool hasvalues = false;

            if(this.TryGetValue(StyleKeys.PageNumberStyleKey,out style))
            {
                opts.NumberStyle = style.Value(this);
                hasvalues = true;
            }
            if (this.TryGetValue(StyleKeys.PageNumberGroupKey, out grp))
            {
                opts.NumberGroup = grp.Value(this);
                hasvalues = true;
            }
            if (this.TryGetValue(StyleKeys.PageNumberStartKey, out start))
            {
                opts.StartIndex = start.Value(this);
                hasvalues = true;
            }
            if (this.TryGetValue(StyleKeys.PageNumberFormatKey, out format))
            {
                opts.Format = format.Value(this);
                hasvalues = true;
            }
            if (this.TryGetValue(StyleKeys.PageNumberGroupHintKey, out grphint))
            {
                opts.GroupCountHint = grphint.Value(this);
                hasvalues = true;
            }
            if (this.TryGetValue(StyleKeys.PageNumberTotalHintKey, out totalhint))
            {
                opts.TotalCountHint = totalhint.Value(this);
                hasvalues = true;
            }
            opts.HasPageNumbering = hasvalues;
            return opts;
        }

        #endregion

        #region internal protected virtual PDFTextRenderOptions DoCreateTextOptions()

        /// <summary>
        /// Implementation of the PDFTextRenderOptions creation. Inheritors can override
        /// </summary>
        /// <returns></returns>
        internal protected virtual PDFTextRenderOptions DoCreateTextOptions()
        {
            PDFTextRenderOptions options = new PDFTextRenderOptions();

            options.Font = this.DoCreateFont(false);
            options.FillBrush = this.DoCreateFillBrush();
            options.Stroke = this.DoCreateStrokePen();
            
            //If we are inline positioned - then add any padding, background and border
            if (this.TryGetValue(StyleKeys.PositionDisplayKey, out StyleValue<DisplayMode> mode) && mode.Value(this) == DisplayMode.Inline)
            {
                options.Padding = this.DoCreatePaddingThickness();
                options.Background = this.DoCreateBackgroundBrush();
                

                options.Border = this.DoCreatePenBorders();

                options.BorderRadius = 0;

                if(this.TryGetValue(StyleKeys.BorderCornerRadiusKey, out StyleValue<Unit> rad))
                {
                    options.BorderRadius = rad.Value(this);
                }
            }
            else
            {
                //we are a block or other - so no padding, background and border on text options.
                options.Padding = null;
                options.Background = null;
                options.Border = null;
            }

            options.InlineMargins = this.DoCreateInlineMarginSize();

            options.InlinePadding = this.DoCreateInlinePaddingSize();

            StyleValue<Unit> flindent;
            if (this.TryGetValue(StyleKeys.TextFirstLineIndentKey,out flindent))
                options.FirstLineInset = flindent.Value(this);

            StyleValue<Unit> space;
            if (this.TryGetValue(StyleKeys.TextWordSpacingKey, out space))
                options.WordSpacing = space.Value(this);

            if (this.TryGetValue(StyleKeys.TextCharSpacingKey, out space))
                options.CharacterSpacing = space.Value(this);

            StyleValue<Text.WordWrap> wrap;
            StyleValue<Text.WordHyphenation> hyphen;
            if (this.TryGetValue(StyleKeys.TextWordWrapKey, out wrap))
            {
                options.WrapText = wrap.Value(this);

                //Check the hypenation if and only if we can wrap text
                if (options.WrapText != Text.WordWrap.NoWrap)
                {
                    if (this.TryGetValue(StyleKeys.TextWordHyphenation, out hyphen))
                    {
                        options.WrapText = (hyphen.Value(this) == Text.WordHyphenation.Auto) ? Text.WordWrap.Character : Text.WordWrap.Word;

                        if (options.WrapText == Text.WordWrap.Character)
                        {
                            options.HyphenationStrategy = DoCreateHyphenationStrategy();
                        }
                    }
                }
            }
            else if (this.TryGetValue(StyleKeys.TextWordHyphenation, out hyphen))
            {
                options.WrapText = (hyphen.Value(this) == Text.WordHyphenation.Auto) ? Text.WordWrap.Character : Text.WordWrap.Word;

                if (options.WrapText == Text.WordWrap.Character)
                {
                    options.HyphenationStrategy = DoCreateHyphenationStrategy();
                }
            }

            StyleValue<double> hscale;
            if (this.TryGetValue(StyleKeys.TextHorizontalScaling, out hscale))
                options.CharacterHScale = hscale.Value(this);

            StyleValue<Scryber.TextDirection> dir;
            if (this.TryGetValue(StyleKeys.TextDirectionKey, out dir))
                options.TextDirection = dir.Value(this);

            StyleValue<Unit> lead;
            if (this.TryGetValue(StyleKeys.TextLeadingKey, out lead))
            {
                var value = lead.Value(this);
                if (Unit.IsAutoValue(value))
                    options.Leading = null;
                else
                    options.Leading = value;
            }

            StyleValue<Text.TextDecoration> decor;
            if (this.TryGetValue(StyleKeys.TextDecorationKey, out decor))
                options.TextDecoration = decor.Value(this);

            StyleValue<bool> frombase;
            if (this.TryGetValue(StyleKeys.TextPositionFromBaseline, out frombase))
                options.DrawTextFromTop = !frombase.Value(this);

            return options;
        }

        private PDFHyphenationStrategy DoCreateHyphenationStrategy()
        {
            var type = Scryber.Text.WordHyphenation.Auto;
            if (this.TryGetValue(StyleKeys.TextWordHyphenation, out var found))
                type = found.Value(this);
            
            if(type == WordHyphenation.None) // No Hyphenation
                return PDFHyphenationStrategy.None;

            else if(this.TryGetValue(StyleKeys.TextHyphenationMinLength, out var foundInt))
            {
                //We have a explicit length, so it is a custom Hyphenation Strategy
                int minWordLength = foundInt.Value(this);
                int minCharsBefore = PDFHyphenationStrategy.DefaultMinCharsBefore;
                int minCharsAfter = PDFHyphenationStrategy.DefaultMinCharsAfter;
                char append = PDFHyphenationStrategy.DefaultAppendChar;

                if (this.TryGetValue(StyleKeys.TextHyphenationMinBeforeBreak, out foundInt))
                    minCharsBefore = foundInt.Value(this);

                if (this.TryGetValue(StyleKeys.TextHyphenationMinAfterBreak, out foundInt))
                    minCharsAfter = foundInt.Value(this);

                if (this.TryGetValue(StyleKeys.TextHyphenationCharAppend, out var foundChar))
                    append = foundChar.Value(this);
                
                //No style key for the pre-pend value by design.

                PDFHyphenationStrategy strategy = new PDFHyphenationStrategy(append, null, minWordLength, minCharsBefore, minCharsAfter);
                
                return strategy;
            }
            else if (this.TryGetValue(StyleKeys.TextHyphenationCharAppend, out var foundChar))
            {
                char append = foundChar.Value(this);
                PDFHyphenationStrategy strategy = new PDFHyphenationStrategy(append, null);

                return strategy;
            }
            else
            {
                //Default as no explicit values.
                return PDFHyphenationStrategy.Default;

            }

        }

        #endregion

        #region internal protected virtual PDFColumnOptions DoCreateColumnOptions()

        internal protected virtual PDFColumnOptions DoCreateColumnOptions()
        {
            return new PDFColumnOptions()
            {
                AlleyWidth = this.GetValue(StyleKeys.ColumnAlleyKey, ColumnsStyle.DefaultAlleyWidth),
                ColumnCount = this.GetValue(StyleKeys.ColumnCountKey, 1),
                ColumnWidths = this.GetValue(StyleKeys.ColumnWidthKey,ColumnWidths.Empty),
                AutoFlow = this.GetValue(StyleKeys.ColumnFlowKey, ColumnsStyle.DefaultAutoFlow)
            };
        }

        #endregion

        protected internal virtual PDFPenBorders DoCreatePenBorders()
        {
            StyleValue<LineType> baseLine;
            StyleValue<Dash> baseDash;
            StyleValue<Unit> baseWidth;
            StyleValue<Color> baseColor;

            var all = this.DoCreateBorderPen(out baseColor, out baseWidth, out baseLine, out baseDash);

            Sides side = 0;



            var left = this.DoCreateBorderSidePen(Sides.Left, StyleKeys.BorderLeftColorKey,
                                                              StyleKeys.BorderLeftWidthKey,
                                                              StyleKeys.BorderLeftStyleKey,
                                                              StyleKeys.BorderLeftDashKey,
                                                              baseColor, baseWidth, baseLine, baseDash);

            var top = this.DoCreateBorderSidePen(Sides.Top, StyleKeys.BorderTopColorKey,
                                                              StyleKeys.BorderTopWidthKey,
                                                              StyleKeys.BorderTopStyleKey,
                                                              StyleKeys.BorderTopDashKey,
                                                              baseColor, baseWidth, baseLine, baseDash);

            var right = this.DoCreateBorderSidePen(Sides.Right, StyleKeys.BorderRightColorKey,
                                                              StyleKeys.BorderRightWidthKey,
                                                              StyleKeys.BorderRightStyleKey,
                                                              StyleKeys.BorderRightDashKey,
                                                              baseColor, baseWidth, baseLine, baseDash);

            var bottom = this.DoCreateBorderSidePen(Sides.Bottom, StyleKeys.BorderBottomColorKey,
                                                              StyleKeys.BorderBottomWidthKey,
                                                              StyleKeys.BorderBottomStyleKey,
                                                              StyleKeys.BorderBottomDashKey,
                                                              baseColor, baseWidth, baseLine, baseDash);
            Unit? corner;

            StyleValue<Unit> cornerValue;
            if (this.TryGetValue(StyleKeys.BorderCornerRadiusKey, out cornerValue))
                corner = cornerValue.Value(this);
            else
                corner = null;

            if (null == left)
                side |= Sides.Left;
            if (null == top)
                side |= Sides.Top;
            if (null == right)
                side |= Sides.Right;
            if (null == bottom)
                side |= Sides.Bottom;

            PDFPenBorders full = new PDFPenBorders()
            {
                AllPen = all,
                AllSides = side,
                LeftPen = left,
                RightPen = right,
                TopPen = top,
                BottomPen = bottom,
                CornerRadius = corner
            };

            return full;
        }


        private static Dash DefaultDash = new Dash(new int[] { 4 }, 0);
        private static Unit DefaultWidth = new Unit(1, PageUnits.Points);

        /// <summary>
        /// The size of the x step if it is not repeated in the x direction
        /// </summary>
        public static readonly Unit NoXRepeatStepSize = int.MaxValue;

        /// <summary>
        /// The size of the y step if it is not repeated in the y direction
        /// </summary>
        public static readonly Unit NoYRepeatStepSize = int.MaxValue;

        /// <summary>
        /// The size value if repeating should use the natural zise of the pattern or image
        /// </summary>
        public static readonly Unit RepeatNaturalSize = 0;

        internal protected virtual PDFPen DoCreateBorderSidePen(Sides side, StyleKey<Color> sideColor, StyleKey<Unit> sideWidth, StyleKey<LineType> sideLine, StyleKey<Dash> sideDash,
            StyleValue<Color> baseColor, StyleValue<Unit> baseWidth, StyleValue<LineType> baseLine, StyleValue<Dash> baseDash)
        {
            PDFPen pen = null;

            StyleValue<LineType> lineValue;
            StyleValue<Dash> dashValue;
            StyleValue<Color> colValue;
            StyleValue<Unit> widthValue;

            LineType line = LineType.None;
            Color col = Color.Transparent;
            Unit width = 0;
            Dash dash = null;


            //Logic
            //If we have an explicit styleValue, then we definiteiy have
            //a specific left pen in that style


            if (this.TryGetValue(sideLine, out lineValue))
            {
                if (baseLine != null && baseLine.Priority > lineValue.Priority)
                    lineValue = baseLine;

                if (lineValue.Value(this) == LineType.None)
                    return new PDFNoPen();

                line = lineValue.Value(this);

                //now either use explicits or full border values

                if (this.TryGetValue(sideColor, out colValue))
                {
                    if (baseColor != null && baseColor.Priority > colValue.Priority)
                        colValue = baseColor;

                    col = colValue.Value(this);
                }
                else if (baseColor != null)
                {
                    col = baseColor.Value(this);
                }
                else
                {
                    col = StandardColors.Black;
                }

                if (this.TryGetValue(sideWidth, out widthValue))
                {
                    if (baseWidth != null && baseWidth.Priority > widthValue.Priority)
                        widthValue = baseWidth;

                    width = widthValue.Value(this);
                }
                else if (baseWidth != null)
                {
                    width = baseWidth.Value(this);
                }
                else
                    width = 1;

                if (this.TryGetValue(sideDash, out dashValue))
                {
                    if (baseDash != null && baseDash.Priority > dashValue.Priority)
                        dashValue = baseDash;

                    dash = dashValue.Value(this);
                }
                else if (baseDash != null)
                {
                    dash = baseDash.Value(this);
                }
                else
                    dash = null;
            }
            else if (this.TryGetValue(sideColor, out colValue))
            {
                //We have an explicit color, so we need a style set
                if (null == baseLine || baseLine.Value(this) == LineType.None)
                    return null;

                line = baseLine.Value(this);
                col = colValue.Value(this);

                if (this.TryGetValue(sideWidth, out widthValue))
                {
                    if (baseWidth != null && baseWidth.Priority > widthValue.Priority)
                        widthValue = baseWidth;

                    width = widthValue.Value(this);
                }
                else if (baseWidth != null)
                {
                    width = baseWidth.Value(this);
                }
                else
                    width = 1;

                if (this.TryGetValue(sideDash, out dashValue))
                {
                    if (null != baseDash && baseDash.Priority > dashValue.Priority)
                        dashValue = baseDash;

                    dash = dashValue.Value(this);
                }
                else if (baseDash != null)
                {
                    dash = baseDash.Value(this);
                }
                else
                    dash = null;

            }
            else if(this.TryGetValue(sideWidth, out widthValue))
            { 
                //We have an explicit width, so we need a style set
                if (null == baseLine || baseLine.Value(this) == LineType.None)
                    return null;

                if (null != baseWidth && baseWidth.Priority > widthValue.Priority)
                    widthValue = baseWidth;

                width = widthValue.Value(this);
                line = baseLine.Value(this);

                if (baseColor != null) //We know there is no explicit side color
                    col = baseColor.Value(this);
                else
                    col = StandardColors.Black;

                if (this.TryGetValue(sideDash, out dashValue))
                {
                    if (null != baseDash && baseDash.Priority > dashValue.Priority)
                        dashValue = baseDash;

                    dash = dashValue.Value(this);
                }
                else if (null != baseDash)
                    dash = baseDash.Value(this);
                else
                    dash = null;

            }
            else
            {
                return null; //nothing set for this side - so it would fall back to the base anyway
            }


            if (line == LineType.None || col.IsTransparent || width <= 0)
                pen = null; //This is an explicit pen that will not draw anything.

            else if (line == LineType.Dash)
            {
                pen = new PDFDashPen(dash, col, width);
            }
            else if (line == LineType.Solid)
                pen = new PDFSolidPen(col, width);
            else
                pen = null; // not supporting the pattern on borders

            if (null != pen)
                this.ApplyBorderAttributes(pen);

            return pen;
        }

        #region internal protected virtual PDFPen DoCreateBorderPen()

        internal protected virtual PDFPen DoCreateBorderPen(out StyleValue<Color> c, out StyleValue<Unit> width, out StyleValue<LineType> penstyle, out StyleValue<Dash> dash)
        {
            PDFPen pen = null;


            this.TryGetValue(StyleKeys.BorderStyleKey, out penstyle);
            this.TryGetValue(StyleKeys.BorderColorKey, out c);
            this.TryGetValue(StyleKeys.BorderWidthKey, out width);
            this.TryGetValue(StyleKeys.BorderDashKey, out dash);

            if (null != penstyle)
            {

                if (penstyle.Value(this) == LineType.None)
                    return new PDFNoPen();

                //Check the color and width

                if (null != c && c.Value(this).IsTransparent)
                    return null;

                if (null != width && width.Value(this) <= 0)
                    return null;

                //create a pen based on the explicit style
                var type = penstyle.Value(this);
                if (type == LineType.Solid)
                {
                    pen = new PDFSolidPen() {
                        Color = (null == c) ? StandardColors.Black : c.Value(this),
                        Width = (null == width) ? 1.0 : width.Value(this) };
                }
                else if (type == LineType.Dash)
                {
                    if (null != dash)
                        pen = new PDFDashPen(dash.Value(this))
                        {
                            Color = (null == c) ? StandardColors.Black : c.Value(this),
                            Width = (null == width) ? 1.0 : width.Value(this)
                        };
                    else // set as Dash, but there is none so use default
                        pen = new PDFDashPen(DefaultDash)
                        {
                            Color = (null == c) ? StandardColors.Black : c.Value(this),
                            Width = (null == width) ? 1.0 : width.Value(this)
                        };
                }
                else if (type == LineType.None)
                {
                    pen = new PDFNoPen();
                }
                else if (type == LineType.Pattern)
                {
                    pen = new PDFNoPen();
                }
                else
                {
                    throw new IndexOutOfRangeException(StyleKeys.BorderStyleKey.ToString());
                }

            }
            else if (dash != null)
            {
                if (c != null && c.Value(this).IsTransparent)
                {
                    return null;
                }
                if (width != null && width.Value(this) <= 0)
                {
                    return null;
                }


                pen = new PDFDashPen(dash.Value(this))
                {
                    Color = (null == c) ? StandardColors.Black : c.Value(this),
                    Width = (null == width) ? 1.0 : width.Value(this)
                };

            }
            else if (c != null)
            {
                if (c.Value(this).IsTransparent)
                    return null;

                if (width != null && width.Value(this) <= 0)
                    return null;

                pen = new PDFSolidPen()
                {
                    Color = (c == null) ? StandardColors.Black : c.Value(this),
                    Width = (null == width) ? DefaultWidth : width.Value(this)
                };
            }
            else if (width != null) //just width set
            {
                if (width.Value(this) <= 0)
                    return null;

                pen = new PDFSolidPen() { Color = StandardColors.Black, Width = width.Value(this) };
            }

            
            //no values set
            if (null == pen)
                return null;

            this.ApplyBorderAttributes(pen);

            return pen;
        }



        #endregion

        private void ApplyBorderAttributes(PDFPen pen)
        {
            StyleValue<LineJoin> join;
            StyleValue<LineCaps> caps;
            StyleValue<float> mitre;
            StyleValue<double> opacity;

            if (this.TryGetValue(StyleKeys.BorderJoinKey, out join))
                pen.LineJoin = join.Value(this);

            if (this.TryGetValue(StyleKeys.BorderEndingKey, out caps))
                pen.LineCaps = caps.Value(this);

            if (this.TryGetValue(StyleKeys.BorderMitreKey, out mitre))
                pen.MitreLimit = mitre.Value(this);

            if (this.TryGetValue(StyleKeys.BorderOpacityKey, out opacity))
                pen.Opacity = (Scryber.PDF.Native.PDFReal)opacity.Value(this);
        }

        #region internal protected virtual PDFPen DoCreateStrokePen()

        internal protected virtual PDFPen DoCreateStrokePen()
        {
            PDFPen pen;

            StyleValue<LineType> penstyle;
            StyleValue<Dash> dash;
            StyleValue<Unit> dashOffset;
            StyleValue<Color> c;
            StyleValue<Unit> width;


            if (this.TryGetValue(StyleKeys.StrokeStyleKey, out penstyle))
            {
                //We have an explict style

                if (penstyle.Value(this) == LineType.None)
                    return null;

                //Check the color and width

                if (this.TryGetValue(StyleKeys.StrokeColorKey, out c) && c.Value(this).IsTransparent)
                    return null;

                if (this.TryGetValue(StyleKeys.StrokeWidthKey, out width) && width.Value(this) <= 0)
                    return null;

                //create a pen based on the explicit style

                if (penstyle.Value(this) == LineType.Solid)
                {
                    pen = new PDFSolidPen() { Color = (null == c) ? StandardColors.Black : c.Value(this), Width = (null == width) ? 1.0 : width.Value(this) };
                }
                else if (penstyle.Value(this) == LineType.Dash)
                {
                    if (this.TryGetValue(StyleKeys.StrokeDashKey, out dash))
                    {
                        var dashpen = new PDFDashPen(dash.Value(this))
                        {
                            Color = (null == c) ? StandardColors.Black : c.Value(this),
                            Width = (null == width) ? 1.0 : width.Value(this),
                        };

                        if (this.TryGetValue(StyleKeys.StrokeDashOffsetKey, out dashOffset))
                        {
                            double phase;
                            var offset = dashOffset.Value(this);
                            if (offset.IsRelative)
                            {
                                if (offset.Units == PageUnits.Percent)
                                {
                                    phase = (offset.Value / 100) * dashpen.Dash.PatternTotal;
                                }
                                else
                                {
                                    throw new NotSupportedException(
                                        "The only supported relative units for a dash offset are % (percentage)");
                                }
                            }
                            else
                            {
                                phase = offset.PointsValue;
                            }

                            dashpen.Dash = new Dash(dashpen.Dash.Pattern, (int) phase);


                        }

                        pen = dashpen;
                    }
                    else // set as Dash, but there is none so use default
                        pen = new PDFDashPen(DefaultDash)
                        {
                            Color = (null == c) ? StandardColors.Black : c.Value(this),
                            Width = (null == width) ? 1.0 : width.Value(this)
                        };
                }
                else
                    throw new IndexOutOfRangeException(StyleKeys.StrokeStyleKey.ToString());

            }
            else if (this.TryGetValue(StyleKeys.StrokeDashKey, out dash))
            {
                if (this.TryGetValue(StyleKeys.StrokeColorKey, out c) && c.Value(this).IsTransparent)
                    return null;
                if (this.TryGetValue(StyleKeys.StrokeWidthKey, out width) && width.Value(this) <= 0)
                    return null;

                double phase = dash.Value(this).Phase;
                if (this.TryGetValue(StyleKeys.StrokeDashOffsetKey, out dashOffset))
                {
                    var offset = dashOffset.Value(this);
                    if (offset.IsRelative)
                    {
                        if (offset.Units == PageUnits.Percent)
                        {
                            phase = (offset.Value / 100) * dash.Value(this).PatternTotal;
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "The only supported relative units for a dash offset are % (percentage)");
                        }
                    }
                    else
                    {
                        phase = offset.PointsValue;
                    }
                }

                pen = new PDFDashPen(new Dash(dash.Value(this).Pattern, (int) phase))
                {
                    Color = (null == c) ? StandardColors.Black : c.Value(this),
                    Width = (null == width) ? 1.0 : width.Value(this)
                };
            }
            else if (this.TryGetValue(StyleKeys.StrokeColorKey, out c))
            {
                if (c.Value(this).IsTransparent)
                    return null;
                if (this.TryGetValue(StyleKeys.StrokeWidthKey, out width) && width.Value(this) <= 0)
                    return null;

                pen = new PDFSolidPen()
                {
                    Color = c.Value(this),
                    Width = (null == width) ? DefaultWidth : width.Value(this)
                };
            }
            else if (this.TryGetValue(StyleKeys.StrokeWidthKey, out width)) //just width set
            {
                if (width.Value(this) <= 0)
                    return null;

                if (this.TryGetValue(StyleKeys.SVGGeometryInUseKey, out var inUse) && inUse.Value(this) == true)
                    return null; //for SVG - if color is not set then no stroke

                pen = new PDFSolidPen()
                {
                    Color = (null == c) ? StandardColors.Black : c.Value(this),
                    Width = (null == width) ? DefaultWidth : width.Value(this)
                };
            }
            else //no values set
                return null;

            this.ApplyStrokeAttributes(pen);

            return pen;
        }

        #endregion

        private void ApplyStrokeAttributes(PDFPen pen)
        {
            StyleValue<LineJoin> join;
            StyleValue<LineCaps> caps;
            StyleValue<float> mitre;
            StyleValue<double> opacity;

            if (this.TryGetValue(StyleKeys.StrokeJoinKey, out join))
                pen.LineJoin = join.Value(this);

            if (this.TryGetValue(StyleKeys.StrokeEndingKey, out caps))
                pen.LineCaps = caps.Value(this);

            if (this.TryGetValue(StyleKeys.StrokeMitreKey, out mitre))
                pen.MitreLimit = mitre.Value(this);

            if (this.TryGetValue(StyleKeys.StrokeOpacityKey, out opacity))
                pen.Opacity = (Scryber.PDF.Native.PDFReal)opacity.Value(this);
        }

        #region internal protected virtual PDFBrush DoCreateBackgroundBrush()

        
        private static System.Text.RegularExpressions.Regex backgroundSplitter = new System.Text.RegularExpressions.Regex(",(?![^(]*\\))");
        /// <summary>
        /// Creates a background brush based on this styles values
        /// </summary>
        /// <returns></returns>
        internal protected virtual PDFBrush DoCreateBackgroundBrush()
        {
            

            StyleValue<Drawing.FillType> fillstyle;
            StyleValue<string> imgsrc;
            StyleValue<double> opacity;
            StyleValue<Color> color;
            if (this.TryGetValue(StyleKeys.BgStyleKey, out fillstyle))
            {
                if (fillstyle.Value(this) == Drawing.FillType.None)
                    return null;
            }

            PDFBrush brush = null;

            if ((this).TryGetValue(StyleKeys.BgColorKey, out color) && !color.Value(this).IsTransparent && (fillstyle == null || fillstyle.Value(this) == Drawing.FillType.Solid))
            {
                PDFSolidBrush solid = new PDFSolidBrush(color.Value(this));

                //if we have an opacity set it.
                if ((this).TryGetValue(StyleKeys.BgOpacityKey, out opacity))
                    solid.Opacity = opacity.Value(this);

                brush = solid;
            }

            //If we have an image source and we are set to use an image fill style (or it has not been specified)
            if ((this).TryGetValue(StyleKeys.BgImgSrcKey, out imgsrc) && !string.IsNullOrEmpty(imgsrc.Value(this)))
            {
                GradientDescriptor found;
                if(this.IsGradientImageSrc(imgsrc.Value(this), out found))
                {
                    if (found == null)
                        brush = null;
                    else if (found.GradientType == GradientType.Linear)
                        brush = new PDFGradientLinearBrush((GradientLinearDescriptor)found);
                    else if (found.GradientType == GradientType.Radial)
                        brush = new PDFGradientRadialBrush((GradientRadialDescriptor)found);
                    else
                        brush = null;
                }
                else if ((fillstyle == null || fillstyle.Value(this) == Drawing.FillType.Image))
                {
                    PatternRepeat repeat = PatternRepeat.RepeatBoth;
                    StyleValue<PatternRepeat> repeatValue;

                    if ((this).TryGetValue(StyleKeys.BgRepeatKey, out repeatValue))
                        repeat = repeatValue.Value(this);

                    if (repeat == PatternRepeat.Fill)
                    {
                        PDFFullImageBrush full = new PDFFullImageBrush(imgsrc.Value(this));
                        if ((this).TryGetValue(StyleKeys.BgOpacityKey, out opacity))
                            full.Opacity = opacity.Value(this);

                        if (null != brush)
                            full.UnderBrush = brush;

                        brush = full;

                    }
                    else
                    {
                        PDFImageBrush img = new PDFImageBrush(imgsrc.Value(this));
                        StyleValue<Unit> unitValue;

                        if (repeat == PatternRepeat.RepeatX || repeat == PatternRepeat.RepeatBoth)
                            img.XStep = (this).TryGetValue(StyleKeys.BgXStepKey, out unitValue) ? unitValue.Value(this) : RepeatNaturalSize;
                        else
                            img.XStep = NoXRepeatStepSize;

                        if (repeat == PatternRepeat.RepeatY || repeat == PatternRepeat.RepeatBoth)
                            img.YStep = (this).TryGetValue(StyleKeys.BgYStepKey, out unitValue) ? unitValue.Value(this) : RepeatNaturalSize;
                        else
                            img.YStep = NoYRepeatStepSize;

                        img.XPostion = (this).TryGetValue(StyleKeys.BgXPosKey, out unitValue) ? unitValue.Value(this) : Unit.Zero;
                        img.YPostion = (this).TryGetValue(StyleKeys.BgYPosKey, out unitValue) ? unitValue.Value(this) : Unit.Zero;

                        if ((this).TryGetValue(StyleKeys.BgXSizeKey, out unitValue))
                            img.XSize = unitValue.Value(this);

                        if ((this).TryGetValue(StyleKeys.BgYSizeKey, out unitValue))
                            img.YSize = unitValue.Value(this);

                        if ((this).TryGetValue(StyleKeys.BgOpacityKey, out opacity))
                            img.Opacity = opacity.Value(this);

                        if (null != brush)
                            img.UnderBrush = brush;

                        brush = img;
                    }
                }
                
                
            }

            return brush;
        }

        protected virtual bool IsGradientImageSrc(string value, out GradientDescriptor descriptor)
        {
            if (!string.IsNullOrEmpty(value) && Parsing.Typed.CSSUrlStyleParser.IsGradient(value, out string grad))
            {
                if (GradientDescriptor.TryParse(grad, out descriptor))
                    return true;
                else
                {
                    descriptor = null;
                    return true;
                }
            }
            else
            {
                descriptor = null;
                return false;
            }
        }


        protected virtual bool IsDataImage(string value)
        {
            if (value.StartsWith("data:image/"))
                return true;
            else
                return false;
        }

        #endregion

        #region internal protected virtual PDFBrush DoCreateFillBrush()

        /// <summary>
        /// Creates a fill brush
        /// </summary>
        /// <returns></returns>
        internal protected virtual PDFBrush DoCreateFillBrush()
        {
            StyleValue<Drawing.FillType> fillstyle;

            StyleValue<string> imgsrc;
            StyleValue<double> opacity;
            StyleValue<Color> color;

            if (this.TryGetValue(StyleKeys.FillStyleKey, out fillstyle))
            {
                if (fillstyle.Value(this) == Drawing.FillType.None)
                    return null;
            }

            bool useSVG = this.GetValue(StyleKeys.SVGGeometryInUseKey, false);
            
            //check for the SVG requirement
            if (useSVG)
            {
                var fillValue = this.GetValue(StyleKeys.SVGFillKey, SVGFillColorValue.Black);
                var opacityValue = this.GetValue(StyleKeys.FillOpacityKey, 1.0);

                return fillValue.GetBrush(opacityValue);
            }
            //If we have an image source and we are set to use an image fill style (or it has not been specified)
            else if ((this).TryGetValue(StyleKeys.FillImgSrcKey, out imgsrc) && !string.IsNullOrEmpty(imgsrc.Value(this)) && (fillstyle == null || fillstyle.Value(this) == Drawing.FillType.Image))
            {
                PatternRepeat repeat = PatternRepeat.RepeatBoth;
                StyleValue<PatternRepeat> repeatValue;

                if ((this).TryGetValue(StyleKeys.FillRepeatKey, out repeatValue))
                    repeat = repeatValue.Value(this);

                PDFBrush brush;
                if (repeat == PatternRepeat.Fill)
                {
                    PDFFullImageBrush full = new PDFFullImageBrush(imgsrc.Value(this));
                    if ((this).TryGetValue(StyleKeys.FillOpacityKey, out opacity))
                        full.Opacity = opacity.Value(this);

                    brush = full;
                }
                else
                {
                    PDFImageBrush img = new PDFImageBrush(imgsrc.Value(this));
                    StyleValue<Unit> unitValue;

                    if (repeat == PatternRepeat.RepeatX || repeat == PatternRepeat.RepeatBoth)
                        img.XStep = (this).TryGetValue(StyleKeys.FillXStepKey, out unitValue) ? unitValue.Value(this) : RepeatNaturalSize;
                    else
                        img.XStep = NoXRepeatStepSize;

                    if (repeat == PatternRepeat.RepeatY || repeat == PatternRepeat.RepeatBoth)
                        img.YStep = (this).TryGetValue(StyleKeys.FillYStepKey, out unitValue) ? unitValue.Value(this) : RepeatNaturalSize;
                    else
                        img.YStep = NoYRepeatStepSize;

                    img.XPostion = (this).TryGetValue(StyleKeys.FillXPosKey, out unitValue) ? unitValue.Value(this) : Unit.Zero;
                    img.YPostion = (this).TryGetValue(StyleKeys.FillYPosKey, out unitValue) ? unitValue.Value(this) : Unit.Zero;

                    if ((this).TryGetValue(StyleKeys.FillXSizeKey, out unitValue))
                        img.XSize = unitValue.Value(this);

                    if ((this).TryGetValue(StyleKeys.FillYSizeKey, out unitValue))
                        img.YSize = unitValue.Value(this);

                    if ((this).TryGetValue(StyleKeys.FillOpacityKey, out opacity))
                        img.Opacity = opacity.Value(this);

                    brush = img;
                }
                return brush;
            }

            //if we have a colour and a solid style (or no style)
            else if ((this).TryGetValue(StyleKeys.FillColorKey, out color) && !color.Value(this).IsTransparent && (fillstyle == null || fillstyle.Value(this) == Drawing.FillType.Solid))
            {
                PDFSolidBrush solid = new PDFSolidBrush(color.Value(this));

                //if we have an opacity set it.
                if ((this).TryGetValue(StyleKeys.FillOpacityKey, out opacity))
                    solid.Opacity = opacity.Value(this);

                return solid;
            }
            else
                return null;
        }

        #endregion

        #region internal protected virtual PDFThickness DoCreateMarginsThickness()

        internal protected virtual Thickness DoCreateMarginsThickness()
        {
            Thickness thickness;
            if (this.TryGetThickness(StyleKeys.MarginsItemKey.Inherited, StyleKeys.MarginsAllKey, StyleKeys.MarginsTopKey, StyleKeys.MarginsLeftKey, StyleKeys.MarginsBottomKey, StyleKeys.MarginsRightKey, out thickness))
                return thickness;
            else
                return Thickness.Empty();
        }

        #endregion

        #region internal protected virtual PDFThickness DoCreatePaddingThickness()

        internal protected virtual Thickness DoCreatePaddingThickness()
        {
            Thickness thickness;
            if (this.TryGetThickness(StyleKeys.PaddingItemKey.Inherited, StyleKeys.PaddingAllKey, StyleKeys.PaddingTopKey, StyleKeys.PaddingLeftKey, StyleKeys.PaddingBottomKey, StyleKeys.PaddingRightKey, out thickness))
                return thickness;
            else
                return Thickness.Empty();
        }

        #endregion

        internal protected virtual Thickness? DoCreateInlineMarginSize()
        {
            Unit start;
            Unit end;
            var hasValues = false;
            if (this.TryGetValue(StyleKeys.MarginsInlineStart, out StyleValue<Unit> sv))
            {
                start = sv.Value(this);
                hasValues = true;
            }
            else
                start = Unit.Zero;

            if (this.TryGetValue(StyleKeys.MarginsInlineEnd, out StyleValue<Unit> ev))
            {
                end = ev.Value(this);
                hasValues = true;
            }
            else
                end = Unit.Zero;

            if (hasValues)
                return new Thickness(Unit.Zero, end, Unit.Zero, start);
            else
            {
                return null;
            }
        }
        
        internal protected virtual Thickness? DoCreateInlinePaddingSize()
        {
            Unit start;
            Unit end;
            var hasValues = false;
            
            if (this.TryGetValue(StyleKeys.PaddingInlineStart, out StyleValue<Unit> sv))
            {
                start = sv.Value(this);
                hasValues = true;
            }
            else
                start = Unit.Zero;

            if (this.TryGetValue(StyleKeys.PaddingInlineEnd, out StyleValue<Unit> ev))
            {
                end = ev.Value(this);
                hasValues = true;
            }
            else
                end = Unit.Zero;

            if (hasValues)
                return new Thickness(Unit.Zero, end, Unit.Zero, start);
            else
            {
                return null;
            }
        }

        #region internal protected virtual PDFThickness DoCreateClippingThickness()

        internal protected virtual Thickness DoCreateClippingThickness()
        {
            Thickness thickness;
            if(this.TryGetThickness(StyleKeys.ClipItemKey.Inherited, StyleKeys.ClipAllKey, StyleKeys.ClipTopKey, StyleKeys.ClipLeftKey, StyleKeys.ClipBottomKey, StyleKeys.ClipRightKey, out thickness))
                return thickness;
            else
                return Thickness.Empty();
        }

        #endregion

        #region internal protected virtual PDFPen DoCreateOverlayGridPen()

        internal protected virtual PDFPen DoCreateOverlayGridPen(bool forMajor)
        {
            StyleValue<bool> show;
            StyleValue<Color> color;
            StyleValue<double> opacity;
            if(this.TryGetValue(StyleKeys.OverlayShowGridKey,out show) && show.Value(this))
            {
                if (this.TryGetValue(StyleKeys.OverlayColorKey, out color) == false)
                    color = new StyleValue<Color>(StyleKeys.OverlayColorKey, OverlayGridStyle.DefaultGridColor);
                if (this.TryGetValue(StyleKeys.OverlayOpacityKey, out opacity) == false)
                    opacity = new StyleValue<double>(StyleKeys.OverlayOpacityKey, OverlayGridStyle.DefaultGridOpacity);
                PDFPen solid;
                if (forMajor)
                {
                    solid = new PDFSolidPen(color.Value(this), OverlayGridStyle.DefaultGridPenMajorWidth)
                        { Opacity = opacity.Value(this) };
                }
                else
                    solid = new PDFSolidPen(color.Value(this), OverlayGridStyle.DefaultGridPenWidth)
                        { Opacity = opacity.Value(this) };
                
                return solid;
            }
            return null;
        }

        #endregion
    }

    public static class PDFStyleBaseExtensions
    {
        public static T GetValue<T>(this StyleBase stylebase, StyleKey<T> key, T defaultValue)
        {
            StyleValueBase exist;
            StyleValue<T> found;
            if (stylebase.TryGetBaseValue(key, out exist))
            {
                found = (StyleValue<T>)exist;
                return found.Value(stylebase);
            }
            else
                return defaultValue;
        }

        public static void SetValue<T>(this StyleBase stylebase, StyleKey<T> key, T value)
        {
            StyleValue<T> match;
            if(stylebase.TryGetValue(key,out match))
            {
                match.SetValue(value);
            }
            else
            {
                match = new StyleValue<T>(key, value);
                stylebase.AddValue(match);
            }
        }

        public static void SetMargins(this StyleBase stylebase, Thickness thickness)
        {
            stylebase.SetThickness(StyleKeys.MarginsItemKey.Inherited, thickness, StyleKeys.MarginsTopKey, StyleKeys.MarginsLeftKey, StyleKeys.MarginsBottomKey, StyleKeys.MarginsRightKey);
        }


        public static void SetPadding(this StyleBase stylebase, Thickness thickness)
        {
            stylebase.SetThickness(StyleKeys.PaddingItemKey.Inherited, thickness, StyleKeys.PaddingTopKey, StyleKeys.PaddingLeftKey, StyleKeys.PaddingBottomKey, StyleKeys.PaddingRightKey);
        }


        public static bool TryGetItem<T>(this StyleBase stylebase, StyleKey itemkey, out T found) where T : StyleItemBase
        {
            StyleItemBase exist;
            if (stylebase.TryGetBaseItem(itemkey, out exist))
            {
                found = (T)exist;
                return true;
            }
            else
            {
                found = null;
                return false;
            }
        }

        public static bool TryGetValue<T>(this StyleBase stylebase, StyleKey<T> key, out StyleValue<T> found)
        {
            StyleValueBase exist;
            if (stylebase.TryGetBaseValue(key, out exist))
            {
                found = (StyleValue<T>)exist;
                return true;
            }
            else
            {
                found = null;
                return false;
            }
        }

        public static T GetOrCreateItem<T>(this StyleBase stylebase, StyleKey key) where T : StyleItemBase, new()
        {
            T found;
            if (stylebase.TryGetItem(key, out found) == false)
            {
                found = new T();
                stylebase.AddItem(found);
            }
            return found;
        }
    }
}
