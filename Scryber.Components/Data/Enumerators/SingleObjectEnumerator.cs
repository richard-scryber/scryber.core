using System;

namespace Scryber.Data.Enumerators;

public class SingleObjectEnumerator : IBindingEnumerator
{

    protected object Value;
    protected IDataSource Source;
    protected int CurrentIndex;
    private DataContext DataContext;
    private bool UpdateCurrentIndex;
    private bool UpdateContextData;
    private bool _dataIsPushed;
    
    public int OriginalIndex { get; private set; }

    public string OriginalKey { get; private set; }

    public object Current
    {
        get
        {
            if(this.CurrentIndex != 0)
                throw new InvalidOperationException(
                    "This enumerator has not been initialized. Always call MoveNext, before accessing the current item");
            return Value;
        }
        
    }

    public SingleObjectEnumerator(object value, IDataSource source, bool updateIndex, bool updateContextData, DataContext context)
    {
        this.Value = value;
        this.Source = source;
        this.CurrentIndex = -1;
        
        this.DataContext = context;
        this.OriginalIndex = context.CurrentIndex;
        this.OriginalKey = context.CurrentKey;
        
        this.UpdateCurrentIndex = updateIndex;
        this.UpdateContextData = updateContextData;
        this._dataIsPushed = false;
    }

    public bool MoveNext()
    {
        if(null == DataContext)
            throw new ObjectDisposedException(nameof(DictionaryBindingEnumerator));

        bool set;
        
        if (this.CurrentIndex < 0)
        {
            if (this.UpdateContextData)
            {
                this.DataContext.DataStack.Push(this.Value, this.Source);
                this._dataIsPushed = true;
            }

            this.CurrentIndex = 0; // will make Current return value.
            
            if (this.UpdateCurrentIndex)
            {
                this.DataContext.CurrentIndex = 0;
            }
            set = true;
        }
        else if (this.CurrentIndex == 0)
        {
            this.CurrentIndex = 1;
            set = false;

            if (this.UpdateContextData)
            {
                this.DataContext.DataStack.Pop();
                this._dataIsPushed = false;
            }
            
        }
        else
        {
            throw new ArgumentOutOfRangeException("index");
        }

        return set;
    }

    public void Reset()
    {
        if (this._dataIsPushed)
        {
            this.DataContext.DataStack.Pop();
            this._dataIsPushed = false;
        }

        this.CurrentIndex = -1;
        this.DataContext.CurrentIndex = this.OriginalIndex;

    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Reset();
            this.CurrentIndex = int.MaxValue; //Will blow if used again.
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
    }

    ~SingleObjectEnumerator()
    {
        this.Dispose(false);
    }
}