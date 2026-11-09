using System;
using System.Collections;

namespace Scryber.Data.Enumerators;

/// <summary>
/// Marks an enumerator whose Current item is only valid until the next call to MoveNext (e.g. an XPath iterator),
/// so the binding enumerator cannot read ahead to identify the last item.
/// </summary>
public interface INoReadAheadEnumerator
{
}

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

    private bool _originalIsLast;
    private string _originalKey;
    
    //The enumerator is read one item ahead, so we know if the current item is the last.
    private bool _hasNext;
    private object _next;
    private bool _started;


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
        this._originalIsLast = context.CurrentIsLast;
        this._originalKey = context.CurrentKey;

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

        bool result;
        object inner = null;

        if (this.UpdateCurrentIndex && false == (this.Enumerator is INoReadAheadEnumerator))
        {
            //Read ahead one item, so we know if the current one is the last.
            if (false == this._started)
            {
                this._started = true;
                this._hasNext = this.Enumerator.MoveNext();
                if (this._hasNext)
                    this._next = this.Enumerator.Current;
            }

            result = this._hasNext;
            if (result)
            {
                inner = this._next;
                this._hasNext = this.Enumerator.MoveNext();
                this._next = this._hasNext ? this.Enumerator.Current : null;
            }
        }
        else
        {
            result = this.Enumerator.MoveNext();
            if (result)
                inner = this.Enumerator.Current;
        }

        this.CurrentIndex++;

        if (result)
        {
            this._current = inner;

            if (this.UpdateCurrentIndex)
            {
                this.DataContext.CurrentIndex = this.CurrentIndex;
                //For a collection the key is the index (as in Handlebars)
                this.DataContext.CurrentKey = this.CurrentIndex.ToString(System.Globalization.CultureInfo.InvariantCulture);
                this.DataContext.CurrentIsLast = (this.Enumerator is INoReadAheadEnumerator) ? false : !this._hasNext;
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
        this._started = false;
        this._hasNext = false;
        this._next = null;
        this.DataContext.CurrentIndex = this.OriginalIndex;
        this.DataContext.CurrentIsLast = this._originalIsLast;
        this.DataContext.CurrentKey = this._originalKey;
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