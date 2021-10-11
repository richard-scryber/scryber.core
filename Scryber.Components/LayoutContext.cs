using System;
using Scryber.Logging;
using Scryber.Styles;

namespace Scryber
{
    public class LayoutContext : ContextStyleBase
    {
        public LayoutContext(Style style, ItemCollection items, TraceLog log, PerformanceMonitor perfmon, IDocument document, OutputFormat format)
            : base(new StyleStack(style), items, log, perfmon, document, format)
        {
        }
    }
}
