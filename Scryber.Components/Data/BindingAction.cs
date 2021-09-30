using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.Data
{
    /// <summary>
    /// A wrapper for binding asscoiated data to a component.
    /// </summary>
    internal class BindingAction
    {
        /// <summary>
        /// Gets or sets the data to be bound
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the component the data should be bound to.
        /// </summary>
        public IBindableComponent Component { get; set; }

        public IPDFDataSource Source { get; set; }


        public BindingAction(object data, IPDFDataSource source, IBindableComponent comp)
        {
            this.Data = data;
            this.Component = comp;
            this.Source = source;
        }
    }

    internal class BindingActionList : List<BindingAction>
    {
        public void Add(object data, IPDFDataSource source, IBindableComponent comp)
        {
            BindingAction action = new BindingAction(data, source, comp);
            this.Add(action);
        }
    }
}
