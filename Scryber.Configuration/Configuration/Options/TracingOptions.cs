using System;
namespace Scryber.Configuration.Options
{
    public class TracingOptions
    {

        public TraceRecordLevel TraceLevel { get; set; }

        public TraceLogOption[] Logs { get; set; }

        public TracingOptions()
        {
        }
    }


    public class TraceLogOption
    {
        public string Name { get; set; }

        public string FactoryType { get; set; }

        public bool Enabled { get; set; }
    }
}
