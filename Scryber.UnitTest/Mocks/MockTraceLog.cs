using System;
using Scryber.Logging;

namespace Scryber.UnitTests.Mocks
{
    public class MockTraceLog : IPDFTraceLogFactory
    {
        public MockTraceLog()
        {
        }

        public TraceLog CreateLog(TraceRecordLevel level, string name)
        {
            return new Scryber.Logging.DoNothingTraceLog(level);
        }
    }

    public class MockTraceLog2 : IPDFTraceLogFactory
    {
        public MockTraceLog2()
        {
        }

        public TraceLog CreateLog(TraceRecordLevel level, string name)
        {
            return new Scryber.Logging.DoNothingTraceLog(level);
        }
    }
}
