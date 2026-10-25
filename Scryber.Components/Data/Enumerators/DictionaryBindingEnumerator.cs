using System;
using System.Collections;
using Scryber.Styles;

namespace Scryber.Data.Enumerators;

public class DictionaryBindingEnumerator : IBindingEnumerator
{

    protected IDictionary Content;
    protected IDataSource Source;
    
    protected IEnumerator KeyEnumerator;
    protected DataContext DataContext;
    protected bool UpdateCurrentIndex;
    protected bool UpdateContextData;
    
    public int CurrentIndex { get; private set; }
    
    public object CurrentKey { get; private set; }

    public int OriginalIndex { get; private set; }

    public string OriginalKey { get; private set; }

    private bool _originalIsLast;
    private bool _started;
    private bool _hasNext;
    private object _nextKey;

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
    
    public DictionaryBindingEnumerator(IDictionary content, IDataSource source, bool updateIndex, bool updateContextData, DataContext context)
    {
        this.Content = content;
        this.Source = source;
        this.CurrentKey = string.Empty;
        this.CurrentIndex = -1;
        
        this.DataContext = context;
        this.OriginalIndex = context.CurrentIndex;
        this.OriginalKey = context.CurrentKey;
        this._originalIsLast = context.CurrentIsLast;
        
        this.UpdateCurrentIndex = updateIndex;
        this.UpdateContextData = updateContextData;
        this._dataIsPushed = false;
    }

    public bool MoveNext()
    {
        if (null == this.DataContext)
            throw new ObjectDisposedException(nameof(DictionaryBindingEnumerator));

        if (this.CurrentIndex < 0)
        {
            this.KeyEnumerator = this.Content.Keys.GetEnumerator(); //disposed on reset()
        }
        else if (_dataIsPushed)
        {
            this.DataContext.DataStack.Pop();
            this._dataIsPushed = false;
        }
        
        this._current = null;
        
        if (null == KeyEnumerator)
            throw new NullReferenceException("There are no keys to enumerate");
        
        
        
        //Read ahead one key, so we know if the current item is the last.
        if (false == this._started)
        {
            this._started = true;
            this._hasNext = this.KeyEnumerator.MoveNext();
            this._nextKey = this._hasNext ? this.KeyEnumerator.Current : null;
        }

        var result = this._hasNext;
        object key = null;

        if (result)
        {
            key = this._nextKey;
            this._hasNext = this.KeyEnumerator.MoveNext();
            this._nextKey = this._hasNext ? this.KeyEnumerator.Current : null;
        }

        this.CurrentIndex++;
        
        if (result)
        {
            if (null == key)
                throw new ArgumentNullException(nameof(key));

            this.CurrentKey = key;

            var inner = this.Content[key];
            this._current = inner;

            if (this.UpdateCurrentIndex)
            {
                this.DataContext.CurrentIndex = this.CurrentIndex;
                this.DataContext.CurrentKey = Convert.ToString(key, System.Globalization.CultureInfo.InvariantCulture);
                this.DataContext.CurrentIsLast = !this._hasNext;
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
        if (null != this.KeyEnumerator)
        {
            if(this.KeyEnumerator is IDisposable disp)
                disp.Dispose();
            this.KeyEnumerator = null;
        }

        if (this._dataIsPushed)
        {
            this.DataContext.DataStack.Pop();
            this._dataIsPushed = false;
        }

        this.CurrentIndex = -1;
        this.CurrentKey = string.Empty;
        this._started = false;
        this._hasNext = false;
        this._nextKey = null;

        this.DataContext.CurrentIndex = this.OriginalIndex;
        this.DataContext.CurrentKey = this.OriginalKey;
        this.DataContext.CurrentIsLast = this._originalIsLast;
    }


    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Reset();
            this.KeyEnumerator = null;
            this.DataContext = null;
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
    }
}