using System;
namespace Scryber
{
    /// <summary>
    /// Base class for the DocumentRenderOptions
    /// </summary>
    public abstract class DocumentOutputOptionsBase
    {

        public abstract OutputFormat Format { get; }

        public DocumentOutputOptionsBase()
        {
        }
    }
}
