using System;
using Scryber.Logging;
using Scryber.Styles;

namespace Scryber
{
    public abstract class LayoutContext : ContextStyleBase
    {
        
        /// <summary>
        /// Gets or sets the current layout depth of positioned regions
        /// </summary>
        public int PositionDepth { get; set; }  
        
        public LayoutContext(Style style, ItemCollection items, TraceLog log, PerformanceMonitor perfmon, IDocument document, OutputFormat format)
            : base(new StyleStack(style), items, log, perfmon, document, format)
        {
            this.PositionDepth = 0;
        }

        protected abstract IDocumentLayout DoGetLayout();

        /// <summary>
        /// Gets the specific document layout within this context.
        /// It should either be an IDocumentLayout or one of the specific format implementations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetLayout<T>() where T : IDocumentLayout
        {
            IDocumentLayout layout = this.DoGetLayout();
            if (layout.Format == this.Format)
                return (T)layout;
            else
                throw new NotSupportedException("The format for this context's layout is not matching the required type " + typeof(T));
        }
    }
}
