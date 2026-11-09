using System;
using Scryber.Logging;

namespace Scryber.Html.Components;

/// <summary>
/// Invisible component that outputs an entry to the trace log.
/// </summary>
[PDFParsableComponent("log")]
public class HTMLLogEntry : Scryber.Components.Component
{

    public static string DefaultCategory = "Document";
    public static TraceLevel DefaultLevel = Scryber.TraceLevel.Message;
    
    [PDFAttribute("data-level")]
    public TraceLevel TraceLevel { get; set; }
    
    [PDFAttribute("data-category")]
    public string Category { get; set; }

    private string _message;
    
    [PDFAttribute("data-message")]
    public string Message {
        get { return _message; }
        set
        {
            this._message = value;
        } 
    }

    public HTMLLogEntry() : this(ObjectTypes.ComponentLogEntry)
    {
        this.TraceLevel = DefaultLevel;
        this.Category = DefaultCategory;
    }
    
    protected HTMLLogEntry(ObjectType type) : base(type)
    {}
    

    protected override void OnDataBound(DataContext context)
    {
        base.OnDataBound(context);
        this.RecordMessage(context);
    }

    protected virtual void RecordMessage(ContextBase context)
    {
        if (null != context && null != context.TraceLog)
        {
            var level = this.TraceLevel;
            var category = this.Category;
            var message = this.Message;
            
            if (string.IsNullOrEmpty(message)) message = "[ EMPTY ]";
                
            context.TraceLog.Add(level, category, message);
            
        }

    }
    
}