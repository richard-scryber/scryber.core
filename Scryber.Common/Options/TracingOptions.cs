using System;
using System.Collections.Generic;
using Scryber.Logging;

namespace Scryber.Options
{
    public class TracingOptions
    {
        public const string TracingSection = ScryberOptions.ScryberSectionStub + "Tracing";
        private const TraceRecordLevel _defaultTraceLevel = TraceRecordLevel.Messages;

        public TraceRecordLevel TraceLevel { get; set; }

        public List<TraceLogOption> Loggers { get; set; }

        public TracingOptions()
        {
            this.TraceLevel = _defaultTraceLevel;
        }

        

        public TraceLog GetTraceLog()
        {
            List<TraceLog> instances = new List<TraceLog>();
            
            
            if (null == Loggers || this.Loggers.Count == 0)
                return new Scryber.Logging.DoNothingTraceLog(this.TraceLevel);
            else if (this.Loggers.Count == 1)
                return this.Loggers[0].GetFactory().CreateLog(this.TraceLevel, Loggers[0].Name);
            else
            {
                
                for(var i = 0; i < this.Loggers.Count; i++)
                {
                    var log = this.Loggers[i].GetFactory().CreateLog(this.TraceLevel, Loggers[i].Name);
                    instances.Add(log);
                }
                return new Logging.CompositeTraceLog(instances, "Composite");
            }
        }
    }


    public class TraceLogOption
    {
        public string Name { get; set; }

        public string FactoryType { get; set; }

        public string FactoryAssembly { get; set; }

        public bool Enabled { get; set; } = true;

        private IPDFTraceLogFactory _factory = null;
        
        internal IPDFTraceLogFactory GetFactory()
        {
            if(null == _factory)
                _factory = Utilities.TypeHelper.GetInstance<IPDFTraceLogFactory>(FactoryType, FactoryAssembly, false);
            if(null == _factory)
                throw new InvalidCastException("Could not create a TraceLogFactory for the configured type '" +  FactoryType + "' in the assembly '" + FactoryAssembly + "'. Either the type could not be found, or it does not support the IPDFTraceLogFactory interface.");
            return _factory;
        }
    }
}
