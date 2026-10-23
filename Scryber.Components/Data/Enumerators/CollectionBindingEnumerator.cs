using System;
using System.Collections;

namespace Scryber.Data.Enumerators;

public class CollectionBindingEnumerator : IBindingEnumerator
{
    protected IEnumerable Content;
    protected IDataSource Source;
    
    protected IEnumerator Enumerator;
    protected DataContext DataContext;
    protected bool UpdateCurrentIndex;
    protected bool UpdateContextData;
    
    public int CurrentIndex { get; private set; }

    public int OriginalIndex { get; private set; }


    private bool _dataIsPushed;
    
    private object _current;
    public object Current
    {
        get
        {
            if (this.CurrentIndex < 0)
                throw new InvalidOperationException(
                    "This enumerator has not been initialized. Always call MoveNext, before accessing the current item");
            return _current;
        }
    }

    public CollectionBindingEnumerator(IEnumerator enumerator, IDataSource source, bool updateIndex,
        bool updateContextData, DataContext context)
    {
        this.Enumerator = enumerator;
        this.Source = source;

        this.CurrentIndex = -1;
        this.DataContext = context ?? throw new ArgumentNullException(nameof(context));
        this.OriginalIndex = context.CurrentIndex;

        this.UpdateCurrentIndex = updateIndex;
        this.UpdateContextData = updateContextData;
        this._dataIsPushed = false;
    }

    public bool MoveNext()
    {
        if (null == this.DataContext)
            throw new ObjectDisposedException(nameof(CollectionBindingEnumerator));
        
        if (_dataIsPushed)
        {
            this.DataContext.DataStack.Pop();
            this._dataIsPushed = false;
        }
        
        this._current = null;
        
        if (null == Enumerator)
            throw new NullReferenceException("The enumeratable instance did not return an enumerator");

        var result = this.Enumerator.MoveNext();
        this.CurrentIndex++;

        if (result)
        {
            var inner = this.Enumerator.Current;
            this._current = inner;

            if (this.UpdateCurrentIndex)
            {
                this.DataContext.CurrentIndex = this.CurrentIndex;
            }

            if (this.UpdateContextData)
            {
                var source = this.Source;
                this.DataContext.DataStack.Push(inner, source);
                this._dataIsPushed = true;
            }
        }

        return result;

    }


    public void Reset()
    {
        if (null != this.Enumerator)
        {

            this.Enumerator.Reset();
        }

        this.ResetBindingState();
    }

    /// <summary>
    /// Restores the data stack and index without resetting the inner enumerator,
    /// as many enumerators (e.g. Linq iterators) do not support Reset, and it is not needed on disposal.
    /// </summary>
    private void ResetBindingState()
    {
        if (null == this.DataContext)
            return;

        if (this._dataIsPushed)
        {
            this.DataContext.DataStack.Pop();
            this._dataIsPushed = false;
        }

        this.CurrentIndex = -1;
        this.DataContext.CurrentIndex = this.OriginalIndex;
    }

    protected virtual void Dispose(bool diposing)
    {
        if (diposing)
        {
            this.ResetBindingState();
            this.Enumerator = null;
            this.DataContext = null;
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
    }

    ~CollectionBindingEnumerator()
    {
        this.Dispose(false);
    }
}