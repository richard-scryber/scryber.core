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
            return this.stack.Pop();
        }


        public virtual DataStack Clone()
        {
            DataStack clone = this.MemberwiseClone() as DataStack;
            clone.sources = new Stack<IDataSource>(this.sources);
            clone.stack = new Stack<object>(this.stack);
            return clone;
        }

    }
}
