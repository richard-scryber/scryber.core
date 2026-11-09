using System;
using Scryber.Html.Components;
using Scryber.Options;

namespace Scryber.Modifications;

/// <summary>
/// Implements a TraceLog for a frame that records to the parent documents trace log with the frame name prepended to the category
/// </summary>
public class FrameTraceLogCollector : Logging.TraceLog
{
    private Logging.TraceLog ParentLog { get; }

    public FrameTraceLogCollector(string name, Logging.TraceLog parent) : this(parent.RecordLevel, name, parent)
    {
    }
    
    public FrameTraceLogCollector(TraceRecordLevel recordlevel, string name, Logging.TraceLog parent) : this(recordlevel, name, parent.Indent, parent)
    {
    }

    public FrameTraceLogCollector(TraceRecordLevel recordlevel, string name, string insetstring, Logging.TraceLog parent) 
        : base(recordlevel, name, insetstring)
    {
        this.ParentLog = parent ?? throw new ArgumentNullException("Parent log cannot be null");
    }

    protected override void Record(string inset, TraceLevel level, TimeSpan timestamp, string category, string message, Exception ex)
    {
        if (string.IsNullOrEmpty(category))
            category = this.Name;
        else
        {
            category = this.Name + " - " + category;
        }
        this.ParentLog.Add(level, category, message, ex);
    }
}