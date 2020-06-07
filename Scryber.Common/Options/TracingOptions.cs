using System;

namespace Scryber.Options
{
    public class TracingOptions
    {
        public const string TracingSection = ScryberOptions.ScryberSectionStub + "Tracing";

        public TraceRecordLevel TraceLevel { get; set; }

        public TraceLogOption[] Loggers { get; set; }

        public TracingOptions()
        {
        }
    }


    public class TraceLogOption
    {
        public string Name { get; set; }

        public string FactoryType { get; set; }

        public string FactoryAssembly { get; set; }

        public bool Enabled { get; set; } = true;

        
    }
}
