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
using System.Collections;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scryber.Data.Enumerators;

namespace Scryber.Components
{
    public abstract class BindingTemplateComponent : VisualComponent
    {
        
        
        #region Inner Classes

        
        
        /// <summary>
        /// An immutable class that prescribes how a BindingTemplateComponent should behave when it data-binds the inner template content.
        /// </summary>
        protected class DataBindingBehaviour
        {
            /// <summary>
            /// If true then the component with this behaviour flag should loop through the
            /// current data. If false then the current data will not be checked.
            /// </summary>
            public bool EnumerateThoughData { get; private set; }

            /// <summary>
            /// If true, then the current context data will be updated for children to use. If both 'EnumerateThroughData' and 'SetContextData' are true,
            /// then the individual items will be used to set the context.
            /// If both 'ExpandObjectProperies' and 'SetContextData' are true, then the values of the individual properties of the context data
            /// will be used to set the context, and the ContextKey will be updated.
            /// </summary>
            public bool SetContextData { get; private set; }
            
            /// <summary>
            /// If true, then the current context data will be reflected for it's public instance properties and
            /// these can then be used (rather than enumerating through an array)
            /// </summary>
            public bool ExpandObjectProperties { get; private set; }

            /// <summary>
            /// If true, then the current DataContext index will be reset, and incremented on (each loop of) the binding.
            /// At the end of the counter value will always be restored back to the original value
            /// </summary>
            public bool IncrementContextIndex { get; private set; }
            
            /// <summary>
            /// Creates the default behaviour for the Binding Template Enumerating through the data (where possible),
            /// updating the current data, and setting the current index. Object properties will not be expanded.
            /// </summary>
            public DataBindingBehaviour() : this(enumerate: true, expandObject: false, setContextData: true, incrementIndex: true)
            {}

            /// <summary>
            /// Defines the behaviour of a binding template based on the flags
            /// </summary>
            /// <param name="enumerate">If true, then the component will try and enumerate through any data source available</param>
            /// <param name="setContextData">If true, then the component will update the BindingContext Current Data so child components can act on it.</param>
            /// <param name="incrementIndex">If true, then the component will reset the index to 0 at the start, and increment the index each time it binds a new template. Restoring the previous value </param>
            public DataBindingBehaviour(bool enumerate, bool expandObject, bool setContextData, bool incrementIndex)
            {
                this.EnumerateThoughData = enumerate;
                this.ExpandObjectProperties = expandObject;
                this.SetContextData = setContextData;
                this.IncrementContextIndex = incrementIndex;
            }
            
            
            
        }
        
        #endregion
        
        #region public event TemplateItemDataBoundHandler ItemDataBound

        private static readonly object ItemBoundEventKey = new object();

        /// <summary>
        /// Event that is raised when one item is databound onto the component
        /// </summary>
        [PDFAttribute("on-item-databound")]
        public event TemplateItemDataBoundHandler ItemDataBound
        {
            add { this.Events.AddHandler(ItemBoundEventKey, value); }
            remove { this.Events.RemoveHandler(ItemBoundEventKey, value); }
        }

        

        /// <summary>
        /// Raises the ItemDataBound event
        /// </summary>
        /// <param name="context"></param>
        /// <param name="item"></param>
        protected virtual void OnItemDataBound(DataContext context, IComponent item)
        {
            if (this.HasRegisteredEvents)
            {
                TemplateItemDataBoundHandler handler = this.Events[ItemBoundEventKey] as TemplateItemDataBoundHandler;
                if(null != handler)
                {
                    handler(this, new TemplateItemDataBoundArgs(item, context));
                }
                
            }
        }

        #endregion

        protected DataBindingBehaviour BindingBehaviour { get; set; }

        /// <summary>
        /// The list of Components that were added to the
        /// parents Component collection when the template was bound.
        /// </summary>
        private List<IComponent> _addedonbind;

        /// <summary>
        /// The parent Component the items were added to.
        /// </summary>
        private IContainerComponent _toparent;



        //
        // ctor(s)
        //

        protected BindingTemplateComponent(ObjectType type, DataBindingBehaviour behaviour)
            : base(type)
        {
            this.BindingBehaviour = behaviour ?? new DataBindingBehaviour();
        }


        //
        // methods
        //
        
        /// <summary>
        /// Main override so that the data can be bound to the any templates
        /// </summary>
        /// <param name="context"></param>
        /// <param name="includeChildren"></param>
        protected override void DoDataBind(DataContext context, bool includeChildren)
        {
            if (_addedonbind != null && _addedonbind.Count > 0)
                this.ClearPreviousBoundComponents(_addedonbind, _toparent);
            else if(null == _addedonbind)
                _addedonbind = new List<IComponent>();

            //call the base method first
            base.DoDataBind(context, includeChildren);

            if (includeChildren)
            {
                IContainerComponent container = GetContainerParent();
                DoDataBindToContainer(context, container);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="container"></param>
        protected virtual void DoDataBindToContainer(DataContext context, IContainerComponent container)
        {
            //If we have a template and should be binding on it
            
            int oldindex = context.CurrentIndex;
            string oldkey = context.CurrentKey;
            
            try
            {
                int index = container.Content.IndexOf(this);

                DoBindDataIntoContainer(container, index, context);
                _toparent = container;

            }
            finally
            {
                context.CurrentIndex = oldindex;
                context.CurrentKey = oldkey;
            }
        }

        protected IContainerComponent GetContainerParent()
        {
            Component ele = this;
            while (null != ele)
            {
                Component par = ele.Parent;
                if (par == null)
                    throw RecordAndRaise.ArgumentNull(Errors.TemplateComponentParentMustBeContainer);

                else if ((par is IContainerComponent) == false)
                    ele = par;
                else
                    return par as IContainerComponent;
            }

            //If we get this far then we haven't got a viable container to add our items to.
            throw RecordAndRaise.ArgumentNull(Errors.TemplateComponentParentMustBeContainer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="containerposition"></param>
        /// <param name="template"></param>
        /// <param name="context"></param>
        protected virtual void DoBindDataIntoContainer(IContainerComponent container, int containerposition, DataContext context)
        {
            int prevcount = context.CurrentIndex;
            DataStack stack = context.DataStack;

            
            object data = stack.HasData ? context.DataStack.Current : null;
            IDataSource source = stack.HasData ? context.DataStack.Source : null;

            int count = 0;
            int added = 0;
            

            using (var enumerator = GetBehaviourEnumerator(this.BindingBehaviour, data, source, context))
            {
                while (enumerator.MoveNext())
                {
                    int number = 0;

                    ITemplate template = this.GetTemplateForBinding(context, count, added + containerposition);
                    if (null != template)
                        number = InstantiateAndAddWithTemplate(template, count, added + containerposition, container,
                            context);

                    count++;
                    added += number;
                }

                enumerator.Reset();
            }

            // if (CanEnumerate(data))
            // {
            //     if (context.ShouldLogDebug)
            //         context.TraceLog.Begin(TraceLevel.Verbose, "Binding Template", "Starting to bind enumerable data into container " + this.ID);
            //     
            //
            //     IEnumerable ienum = data as IEnumerable;
            //     enumerator = this.CreateEnumerator(ienum);
            //     while (enumerator.MoveNext())
            //     {
            //         context.CurrentIndex = count;
            //         context.DataStack.Push(enumerator.Current, source);
            //         int number = 0;
            //
            //         ITemplate template = this.GetTemplateForBinding(context, count, added + containerposition);
            //         if (null != template)
            //             number = InstantiateAndAddWithTemplate(template, count, added + containerposition, container, context);
            //
            //         context.DataStack.Pop();
            //
            //         
            //         if (context.ShouldLogDebug)
            //             context.TraceLog.Add(TraceLevel.Debug, "Binding Template", "Bound data into template index : " + count + " and added " + number + " components");
            //
            //         count++;
            //         added += number;
            //     }
            //     if (count == 0)
            //         context.TraceLog.Add(TraceLevel.Message, "Binding Template", "ZERO items were data bound with the Binding Template Component as MoveNext returned false from the start");
            //     
            //     if (context.ShouldLogDebug)
            //         context.TraceLog.End(TraceLevel.Verbose, "Binding Template", "Completed binding enumerable data into Binding Template Component, " + added + " components added, with " + count + " enumerable data items");
            //     else if(context.ShouldLogVerbose)
            //         context.TraceLog.Add(TraceLevel.Verbose, "Binding Template", "Completed binding enumerable data into Binding Template Component, " + added + " components added, with " + count + " enumerable data items");
            //     
            // }
            // else if (data != null)
            // {
            //     if (context.ShouldLogDebug)
            //         context.TraceLog.Begin(TraceLevel.Verbose, "Binding Template",
            //             "Starting to bind single data into Binding Template Component");
            //
            //     if (this.BindingBehaviour != DataBindingBehaviour.SetCurrentAndItterate)
            //     {
            //         //We set the 
            //         context.CurrentIndex = 1;
            //     }
            //     context.DataStack.Push(data, source);
            //
            //     ITemplate template = this.GetTemplateForBinding(context, count, added + containerposition);
            //     if(null != template)
            //         added += InstantiateAndAddWithTemplate(template, count, added + containerposition, container, context);
            //     context.DataStack.Pop();
            //
            //     if (context.ShouldLogDebug)
            //         context.TraceLog.End(TraceLevel.Verbose, "Binding Template", "Completed binding single data into the Binding Template Component, " + added + " components added.");
            //     else if (context.ShouldLogVerbose)
            //         context.TraceLog.Add(TraceLevel.Verbose, "Binding Template", "Completed binding single data into the Binding Template Component, " + added + " components added.");
            //     
            // }
            // else
            // {
            //     if (context.ShouldLogDebug)
            //         context.TraceLog.Begin(TraceLevel.Verbose, "Binding Template", "Starting to bind into Binding Template Component with NO context data");
            //
            //     context.CurrentIndex = 1;
            //     ITemplate template = this.GetTemplateForBinding(context, count, added + containerposition);
            //     if (null != template)
            //         added += InstantiateAndAddWithTemplate(template, count, added + containerposition, container, context);
            //
            //     if (context.ShouldLogDebug)
            //         context.TraceLog.End(TraceLevel.Verbose, "Binding Template", "Completed binding the Binding Template Component with NO data, " + added + " components added.");
            //     else if (context.ShouldLogVerbose)
            //         context.TraceLog.Add(TraceLevel.Verbose, "Binding Template", "Completed binding the Binding Template Component with NO data, " + added + " components added.");
            // }
            //
            // context.CurrentIndex = prevcount;
            
        }

        protected virtual Scryber.Data.Enumerators.IBindingEnumerator GetBehaviourEnumerator(DataBindingBehaviour behaviour, object value, IDataSource source, DataContext context)
        {
            if (value is string)
            {
                if (behaviour.ExpandObjectProperties)
                {
                    //Special implementation to go through each character
                    // new StringContentEnumerator();
                    throw new NotImplementedException("Not enumerating over strings just yet");
                }
                else
                {
                    return new Scryber.Data.Enumerators.SingleObjectEnumerator(value, source,
                        behaviour.IncrementContextIndex,
                        behaviour.SetContextData,
                        context);
                }
            }
            if (value is IDictionary dict)
            {
                if (behaviour.ExpandObjectProperties)
                {
                    return new Scryber.Data.Enumerators.DictionaryBindingEnumerator(dict, source,
                        behaviour.IncrementContextIndex,
                        behaviour.SetContextData,
                        context);
                }
                else
                {
                    return new Scryber.Data.Enumerators.SingleObjectEnumerator(value, source,
                        behaviour.IncrementContextIndex,
                        behaviour.SetContextData,
                        context);
                }
            }
            else if (value is JObject jobj)
            {
                if (behaviour.ExpandObjectProperties)
                {
                    return new JObjectEnumerator(jobj, source, behaviour.IncrementContextIndex,
                        behaviour.SetContextData, context);
                }
                else
                {
                    return new SingleObjectEnumerator(jobj, source, behaviour.IncrementContextIndex,
                        behaviour.SetContextData, context);
                }
            }
            else if (value is IEnumerable ienum)
            {
                if (behaviour.EnumerateThoughData)
                {
                    var enumerator = this.CreateEnumerator(ienum);

                    return new CollectionBindingEnumerator(enumerator, source, behaviour.IncrementContextIndex,
                        behaviour.SetContextData, context);
                }
                else
                {
                    return new SingleObjectEnumerator(value, source, behaviour.IncrementContextIndex,
                        behaviour.SetContextData, context);
                }
            }
            else if (behaviour.ExpandObjectProperties)
            {
                throw new NotImplementedException("Expand an object properties and enumerate over them");
            }
            else
            {
                return new Scryber.Data.Enumerators.SingleObjectEnumerator(value, source,
                    behaviour.IncrementContextIndex,
                    behaviour.SetContextData,
                    context);
            }
        }

        // protected bool CanEnumerate(object value)
        // {
        //     //First check the behaviour for itteration - if not, then don't do it.
        //     if (this.BindingBehaviour != DataBindingBehaviour.SetCurrentAndItterate)
        //     {
        //         return false;
        //     }
        //     if (value is IEnumerable)
        //     {
        //
        //         if (value is ICustomTypeDescriptor)
        //             return false; // Should not enumerate over these
        //         else if (value is string)
        //             return false; // Should not enumerate over strings
        //         else
        //             return true;
        //
        //     }
        //     else
        //         return false;
        // }

        /// <summary>
        /// Abstract method that all inheritors must override to return the template required for instaniating and binding.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected abstract ITemplate GetTemplateForBinding(DataContext context, int index, int count);


        #region protected virtual IEnumerator CreateEnumerator(IEnumerable enumerable)

        /// <summary>
        /// Returns an enumerator that will loop over all the items in the IEnumerable instance.
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        protected virtual IEnumerator CreateEnumerator(IEnumerable enumerable)
        {
            return enumerable.GetEnumerator();
        }

        #endregion

        #region protected virtual IEnumerator CreateSingleDataEnumerator(object data)

        /// <summary>
        /// returns an enumerator that will enumerate once over a single item
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual IEnumerator CreateSingleDataEnumerator(object data)
        {
            return new SingleItemEnumerator(data);
        }

        #endregion

        #region protected virtual int InstantiateAndAddFromTemplate(PDFDataContext context, int count, int index, IPDFContainerComponent container, IPDFTemplate template)

        private InitContext _initContext;
        private LoadContext _loadContext;

        private InitContext GetInitContext(DataContext dataContext)
        {
            if (null == _initContext)
                _initContext = new InitContext(dataContext.Items, dataContext.TraceLog, dataContext.PerformanceMonitor, this.Document, dataContext.Format);
            return _initContext;
        }

        private LoadContext GetLoadContext(DataContext dataContext)
        {
            if (null == _loadContext)
                _loadContext = new LoadContext(dataContext.Items, dataContext.TraceLog, dataContext.PerformanceMonitor, this.Document, dataContext.Format);
            return _loadContext;
        }

        /// <summary>
        /// Creates a new instance of the template and adds it to this components content
        /// </summary>
        /// <param name="context"></param>
        /// <param name="count"></param>
        /// <param name="index"></param>
        /// <param name="container"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        protected virtual int InstantiateAndAddWithTemplate(ITemplate template, int count, int index, IContainerComponent container, DataContext context)
        {

            if (null == template)
                return 0;

            InitContext init = GetInitContext(context);
            LoadContext load = GetLoadContext(context);
            
            IEnumerable<IComponent> created = template.Instantiate(count, this);
            int added = 0;
            if (created != null)
            {
                foreach (IComponent ele in ((IEnumerable)created))
                {
                    InsertComponentInContainer(container, index, ele, init, load);
                    if (ele is IBindableComponent)
                    {
                        ((IBindableComponent)ele).DataBind(context);
                    }
                    index++;
                    added++;

                    //raise the event
                    this.OnItemDataBound(context, ele);
                }

            }
            return added;
        }

        #endregion


        #region private void ClearPreviousBoundComponents(ICollection<IPDFComponent> all, IPDFContainerComponent container)

        /// <summary>
        /// Removes any previously bound Components
        /// </summary>
        /// <param name="all"></param>
        /// <param name="container"></param>
        private void ClearPreviousBoundComponents(ICollection<IComponent> all, IContainerComponent container)
        {
            //dispose and clear
            foreach (Component ele in all)
            {
                container.Content.Remove(ele);
                ele.Dispose();
            }
            all.Clear();
        }

        #endregion

        #region private void InsertComponentInContainer(IPDFContainerComponent container, int index, IPDFComponent ele, PDFInitContext init, PDFLoadContext load)

        /// <summary>
        /// Inserts a new Component in the container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="index"></param>
        /// <param name="ele"></param>
        private void InsertComponentInContainer(IContainerComponent container, int index, IComponent ele, InitContext init, LoadContext load)
        {
            ele.Init(init);
            IComponentList list = container.Content as IComponentList;
            list.Insert(index, ele);
            _addedonbind.Add(ele);
            ele.Load(load);
        }

        #endregion

        #region private class SingleItemEnumerator : IEnumerator


        /// <summary>
        /// Implements an IEnumerator for a single piece of data
        /// </summary>
        private class SingleItemEnumerator : IEnumerator
        {
            private int _index;
            private object _data;

            public object Current
            {
                get
                {
                    if (_index == 0)
                        return _data;
                    else
                        throw new ArgumentOutOfRangeException("index");
                }
            }

            public SingleItemEnumerator(object data)
            {
                this._data = data;
                this.Reset();
            }

            public bool MoveNext()
            {
                _index++;
                return _index == 0;
            }

            public void Reset()
            {
                _index = -1;
            }
        }

        #endregion
    }
}
