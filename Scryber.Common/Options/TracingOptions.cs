using System;
using System.Collections.Generic;

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

        private Scryber.IPDFTraceLogFactory[] _factories;

        public Scryber.PDFTraceLog GetTraceLog()
        {
            if(null == _factories)
            {
                List<IPDFTraceLogFactory> all = new List<IPDFTraceLogFactory>();
                if(null != Loggers)
                {
                    foreach (var one in Loggers)
                    {
                        if(one.Enabled)
                        {
                            var instance = Utilities.TypeHelper.GetInstance<IPDFTraceLogFactory>(one.FactoryType, one.FactoryAssembly, true);
                            all.Add(instance);
                        }
                    }
                }
                _factories = all.ToArray();
            }

            if (_factories.Length == 0)
                return new Scryber.Logging.DoNothingTraceLog(this.TraceLevel);
            else if (_factories.Length == 1)
                return _factories[0].CreateLog(this.TraceLevel, Loggers[0].Name);
            else
            {
                List<PDFTraceLog> instances = new List<PDFTraceLog>();
                for(var i = 0; i < _factories.Length; i++)
                {
                    instances.Add(_factories[i].CreateLog(this.TraceLevel, Loggers[i].Name));
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

        
    }
}
