using Microsoft.Extensions.Logging;

namespace Scryber.UnitConsoleTest;


public class TraceLogger : Scryber.Logging.TraceLog
{
    private readonly ILogger _logger;
    private readonly EventId _category;

    public TraceLogger(ILogger logger, TraceRecordLevel level, int id, string name) : base(level, name)
    {
        _logger = logger;
        _category = new EventId(id, name);
    }

    protected override void Record(string inset, TraceLevel level, TimeSpan timestamp, string category, string message, Exception ex)
    {
        if ((int)level >= (int)this.RecordLevel)
        {
            LogLevel msLevel;
            switch (level)
            {
                case TraceLevel.Failure:
                    msLevel = LogLevel.Critical;
                    break;
                case TraceLevel.Error:
                    msLevel = LogLevel.Error;
                    break;
                case TraceLevel.Warning:
                    msLevel = LogLevel.Warning;
                    break;
                case TraceLevel.Message:
                    msLevel = LogLevel.Information;
                    break;
                case TraceLevel.Verbose:
                    msLevel = LogLevel.Debug;
                    break;
                case TraceLevel.Debug:
                    msLevel = LogLevel.Trace;
                    break;
                default:
                    msLevel = LogLevel.None;
                    break;
            }
            
            _logger.Log(msLevel, _category, ex, message);
        }
    }
}