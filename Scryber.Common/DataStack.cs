/*  Copyright 2016 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *  See the Scryber.Licenseing.txt file for the fill license governing this code
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Scryber
{
    /// <summary>
    /// A stack of data and source instances that represent the current stack of data in a binding operation.
    /// </summary>
    public class DataStack
    {
        private Stack<object> stack = new Stack<object>();
        private Stack<IDataSource> sources = new Stack<IDataSource>();
        private Stack<bool> collections = new Stack<bool>();
        
        /// <summary>
        /// Returns true if there is at least one object on the data stack
        /// </summary>
        public bool HasData
        {
            get { return this.stack != null && this.stack.Count > 0; }
        }

        /// <summary>
        /// Gets the current object on this data stack
        /// </summary>
        public object Current
        {
            get
            {
                if (this.HasData == false)
                    throw new InvalidOperationException(CommonErrors.NoDataContextOnTheStack);
                return this.stack.Peek();
            }
        }

        public IDataSource Source
        {
            get
            {
                if (this.HasData == false)
                    throw new InvalidOperationException(CommonErrors.NoDataContextOnTheStack);
                return this.sources.Peek();
            }
        }

        /// <summary>
        /// Pushes a new object onto the data stack
        /// </summary>
        /// <param name="data"></param>
        public void Push(object data, IDataSource source)
        {
            //if (null == source)
            //    throw new ArgumentNullException("source");

            this.stack.Push(data);
            this.sources.Push(source);
            this.collections.Push(false);
        }

        /// <summary>
        /// Pushes a collection that is about to be enumerated onto the data stack (e.g. for an each loop).
        /// The items from the collection are then pushed above it, but it is not considered
        /// a context in its own right when finding the parent of the current item.
        /// </summary>
        public void PushCollection(object data, IDataSource source)
        {
            this.stack.Push(data);
            this.sources.Push(source);
            this.collections.Push(true);
        }

        /// <summary>
        /// Finds the data in the context outside the current one (the '../' reference), without changing the stack.
        /// If the current item came from an enumerated collection, the collection itself is skipped.
        /// </summary>
        /// <param name="parent">Set to the parent data</param>
        /// <returns>True if there is a parent context, otherwise false</returns>
        public bool TryGetParent(out object parent)
        {
            parent = null;
            if (this.stack.Count < 2)
                return false;

            var data = this.stack.ToArray(); //top first
            var colls = this.collections.ToArray();
            var index = 1;

            if (colls[index] && colls.Length > 2)
                index++; //skip the enumerated collection frame

            if (colls[index])
                return false; //only the collection remains, so there is no outer context

            parent = data[index];
            return true;
        }

        /// <summary>
        /// Pops (and returns) the current data object from the stack
        /// </summary>
        /// <returns></returns>
        public object Pop()
        {
            if(this.HasData == false)
                throw new InvalidOperationException(CommonErrors.NoDataContextOnTheStack);
            this.sources.Pop();
            this.collections.Pop();
            return this.stack.Pop();
        }


        public virtual DataStack Clone()
        {
            DataStack clone = this.MemberwiseClone() as DataStack;
            //Copy the stacks in the correct order, as the enumerable constructor reverses them
            clone.sources = new Stack<IDataSource>(this.sources.Reverse());
            clone.stack = new Stack<object>(this.stack.Reverse());
            clone.collections = new Stack<bool>(this.collections.Reverse());
            return clone;
        }

    }
}
