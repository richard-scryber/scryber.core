using System;
using Scryber.Logging;
using Scryber.Styles;

namespace Scryber
{
    public class RenderContext : ContextStyleBase
    {
        
        public RenderContext(Style style, ItemCollection items, TraceLog log, PerformanceMonitor perfmon, IDocument document, OutputFormat format)
            : base(new StyleStack(style), items, log, perfmon, document, format)
        {
        }
    }
}
