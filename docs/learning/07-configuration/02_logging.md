---
layout: default
title: Logging
nav_order: 2
parent: Document Configuration
parent_url: /learning/07-configuration/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Logging

Master Scryber's logging system to diagnose issues, monitor performance, and troubleshoot PDF generation in development and production.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Configure Scryber logging
- Use different log levels appropriately
- Implement custom log handlers
- Analyze log output for troubleshooting
- Configure logging for production
- Monitor PDF generation performance

---

## Log Levels

Scryber provides several logging levels:

```csharp
using Scryber.Logging;

public enum TraceRecordLevel
{
    Off,        // No logging
    Errors,     // Only errors
    Warnings,   // Errors and warnings
    Messages,   // Errors, warnings, and informational messages
    Verbose     // Everything (debugging)
}
```

---

## Basic Logging

### Console Logging

```csharp
using Scryber.Components;
using Scryber.Logging;
using System.IO;

// Create document
var doc = Document.ParseDocument("template.html");

// Enable console logging
doc.TraceLog = new PDFTraceLog();
doc.TraceLog.SetRecordLevel(TraceRecordLevel.Messages);

// Generate PDF
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}

// Output goes to console:
// [INFO] Document parsing started
// [INFO] Loading template: template.html
// [INFO] Rendering page 1
// [INFO] Document generated successfully
```

### Collector Logging

```csharp
using Scryber.Logging;

// Collect log entries for analysis
var logger = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);
doc.TraceLog = logger;

// Generate document
doc.SaveAsPDF(stream);

// Analyze collected entries
foreach (var entry in logger.Entries)
{
    Console.WriteLine($"[{entry.Level}] {entry.Category}: {entry.Message}");

    if (entry.Level == TraceRecordLevel.Errors)
    {
        // Handle errors
        HandleError(entry);
    }
}
```

---

## Log Level Selection

### Development

```csharp
// Development: Log everything
doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);
```

**Use Verbose when:**
- Debugging issues
- Understanding document generation flow
- Learning Scryber behavior
- Investigating performance problems

### Production

```csharp
// Production: Log warnings and errors only
doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
```

**Use Warnings when:**
- Running in production
- Want to know about potential issues
- Avoid excessive log volume
- Monitor system health

### Performance-Critical

```csharp
// Performance-critical: Errors only
doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Errors);

// Or disable logging entirely
doc.TraceLog = new PDFTraceLog();
doc.TraceLog.SetRecordLevel(TraceRecordLevel.Off);
```

---

## Custom Log Handlers

### File Logging

```csharp
using Scryber.Logging;
using System;
using System.IO;

public class FileTraceLog : PDFTraceLog
{
    private readonly string _logFilePath;
    private readonly object _lock = new object();

    public FileTraceLog(string logFilePath, TraceRecordLevel level)
    {
        _logFilePath = logFilePath;
        this.SetRecordLevel(level);
    }

    public override void Begin(TraceRecordLevel level, string category, string message)
    {
        if (!ShouldLog(level)) return;

        lock (_lock)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {category}: {message}";
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
    }

    protected override void End(TraceRecordLevel level, string category, string message)
    {
        // Optional: Log completion messages
    }

    private bool ShouldLog(TraceRecordLevel level)
    {
        return level <= this.RecordLevel;
    }
}

// Usage
var doc = Document.ParseDocument("template.html");
doc.TraceLog = new FileTraceLog("pdf-generation.log", TraceRecordLevel.Messages);
doc.SaveAsPDF(stream);
```

### Database Logging

```csharp
using Scryber.Logging;
using System;

public class DatabaseTraceLog : PDFTraceLog
{
    private readonly string _connectionString;
    private readonly string _sessionId;

    public DatabaseTraceLog(string connectionString, TraceRecordLevel level)
    {
        _connectionString = connectionString;
        _sessionId = Guid.NewGuid().ToString();
        this.SetRecordLevel(level);
    }

    public override void Begin(TraceRecordLevel level, string category, string message)
    {
        if (!ShouldLog(level)) return;

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO PDFLogs (SessionId, Timestamp, Level, Category, Message) " +
                    "VALUES (@SessionId, @Timestamp, @Level, @Category, @Message)",
                    connection);

                command.Parameters.AddWithValue("@SessionId", _sessionId);
                command.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                command.Parameters.AddWithValue("@Level", level.ToString());
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Message", message);

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            // Don't let logging errors break PDF generation
            Console.WriteLine($"Logging error: {ex.Message}");
        }
    }

    private bool ShouldLog(TraceRecordLevel level)
    {
        return level <= this.RecordLevel;
    }
}

// Usage
var doc = Document.ParseDocument("template.html");
doc.TraceLog = new DatabaseTraceLog("connection-string", TraceRecordLevel.Warnings);
doc.SaveAsPDF(stream);
```

### Structured Logging (Serilog, NLog)

```csharp
using Scryber.Logging;
using Serilog;

public class SerilogTraceLog : PDFTraceLog
{
    private readonly ILogger _logger;

    public SerilogTraceLog(ILogger logger, TraceRecordLevel level)
    {
        _logger = logger;
        this.SetRecordLevel(level);
    }

    public override void Begin(TraceRecordLevel level, string category, string message)
    {
        if (!ShouldLog(level)) return;

        switch (level)
        {
            case TraceRecordLevel.Errors:
                _logger.Error("[{Category}] {Message}", category, message);
                break;
            case TraceRecordLevel.Warnings:
                _logger.Warning("[{Category}] {Message}", category, message);
                break;
            case TraceRecordLevel.Messages:
                _logger.Information("[{Category}] {Message}", category, message);
                break;
            case TraceRecordLevel.Verbose:
                _logger.Debug("[{Category}] {Message}", category, message);
                break;
        }
    }

    private bool ShouldLog(TraceRecordLevel level)
    {
        return level <= this.RecordLevel;
    }
}

// Usage with Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/pdf-generation.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var doc = Document.ParseDocument("template.html");
doc.TraceLog = new SerilogTraceLog(Log.Logger, TraceRecordLevel.Messages);
doc.SaveAsPDF(stream);
```

---

## Practical Examples

### Example 1: Production Invoice Generator with Logging

```csharp
using Scryber.Components;
using Scryber.Logging;
using System;
using System.IO;
using System.Linq;

public class InvoiceGenerator
{
    private readonly string _logDirectory;

    public InvoiceGenerator(string logDirectory)
    {
        _logDirectory = logDirectory;
    }

    public void GenerateInvoice(Invoice invoice, Stream output)
    {
        // Set up logging for this invoice
        var logger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);

        try
        {
            // Load and configure document
            var doc = Document.ParseDocument("invoice-template.html");
            doc.TraceLog = logger;

            // Set metadata
            doc.Info.Title = $"Invoice {invoice.Number}";
            doc.Info.Author = "Acme Corp";

            // Bind data
            doc.Params["invoice"] = invoice;

            // Generate
            doc.SaveAsPDF(output);

            // Check for warnings
            var warnings = logger.Entries
                .Where(e => e.Level == TraceRecordLevel.Warnings)
                .ToList();

            if (warnings.Any())
            {
                LogWarningsToFile(invoice.Number, warnings);
            }

            Console.WriteLine($"Invoice {invoice.Number} generated successfully");
        }
        catch (Exception ex)
        {
            // Log error with all diagnostic information
            LogErrorToFile(invoice.Number, ex, logger.Entries);
            throw;
        }
    }

    private void LogWarningsToFile(string invoiceNumber, List<TraceRecord> warnings)
    {
        var logFile = Path.Combine(_logDirectory, $"warnings_{DateTime.Now:yyyyMMdd}.log");
        var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Invoice {invoiceNumber} - " +
                      $"{warnings.Count} warnings:{Environment.NewLine}";

        foreach (var warning in warnings)
        {
            logEntry += $"  - {warning.Category}: {warning.Message}{Environment.NewLine}";
        }

        File.AppendAllText(logFile, logEntry);
    }

    private void LogErrorToFile(string invoiceNumber, Exception ex, TraceRecordCollection entries)
    {
        var logFile = Path.Combine(_logDirectory, $"errors_{DateTime.Now:yyyyMMdd}.log");
        var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Invoice {invoiceNumber} FAILED{Environment.NewLine}" +
                      $"Error: {ex.Message}{Environment.NewLine}" +
                      $"Stack: {ex.StackTrace}{Environment.NewLine}" +
                      $"Log entries:{Environment.NewLine}";

        foreach (var entry in entries)
        {
            logEntry += $"  [{entry.Level}] {entry.Category}: {entry.Message}{Environment.NewLine}";
        }

        File.AppendAllText(logFile, logEntry);
    }
}

// Usage
var generator = new InvoiceGenerator("./logs");
using (var output = new FileStream("invoice.pdf", FileMode.Create))
{
    generator.GenerateInvoice(invoice, output);
}
```

### Example 2: Performance Monitoring

```csharp
using Scryber.Components;
using Scryber.Logging;
using System;
using System.Diagnostics;
using System.IO;

public class PerformanceMonitor
{
    public void GenerateWithPerformanceLogging(string templatePath, Stream output)
    {
        var logger = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);
        var stopwatch = Stopwatch.StartNew();

        var doc = Document.ParseDocument(templatePath);
        doc.TraceLog = logger;

        var parseTime = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"Parse time: {parseTime}ms");

        stopwatch.Restart();
        doc.SaveAsPDF(output);
        var renderTime = stopwatch.ElapsedMilliseconds;

        Console.WriteLine($"Render time: {renderTime}ms");
        Console.WriteLine($"Total time: {parseTime + renderTime}ms");

        // Analyze log for performance bottlenecks
        AnalyzePerformance(logger.Entries);
    }

    private void AnalyzePerformance(TraceRecordCollection entries)
    {
        Console.WriteLine("\nPerformance Analysis:");

        // Count operations by category
        var categories = entries.GroupBy(e => e.Category)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count);

        foreach (var cat in categories)
        {
            Console.WriteLine($"  {cat.Category}: {cat.Count} operations");
        }

        // Find slow operations (if logged with timing)
        var slowOps = entries
            .Where(e => e.Message.Contains("ms") || e.Message.Contains("slow"))
            .ToList();

        if (slowOps.Any())
        {
            Console.WriteLine("\nPotential bottlenecks:");
            foreach (var op in slowOps)
            {
                Console.WriteLine($"  - {op.Message}");
            }
        }
    }
}

// Usage
var monitor = new PerformanceMonitor();
using (var output = new FileStream("output.pdf", FileMode.Create))
{
    monitor.GenerateWithPerformanceLogging("template.html", output);
}
```

---

## Log Analysis and Troubleshooting

### Common Error Patterns

```csharp
public class LogAnalyzer
{
    public void AnalyzeLogs(TraceRecordCollection entries)
    {
        // Check for errors
        var errors = entries.Where(e => e.Level == TraceRecordLevel.Errors).ToList();
        if (errors.Any())
        {
            Console.WriteLine($"ERRORS FOUND: {errors.Count}");
            foreach (var error in errors)
            {
                Console.WriteLine($"  - {error.Category}: {error.Message}");
            }
        }

        // Check for warnings
        var warnings = entries.Where(e => e.Level == TraceRecordLevel.Warnings).ToList();
        if (warnings.Any())
        {
            Console.WriteLine($"\nWARNINGS: {warnings.Count}");
            foreach (var warning in warnings)
            {
                Console.WriteLine($"  - {warning.Category}: {warning.Message}");
            }
        }

        // Check for common issues
        CheckForCommonIssues(entries);
    }

    private void CheckForCommonIssues(TraceRecordCollection entries)
    {
        // Missing resources
        if (entries.Any(e => e.Message.Contains("not found") || e.Message.Contains("missing")))
        {
            Console.WriteLine("\n⚠ Possible missing resources detected");
        }

        // Font issues
        if (entries.Any(e => e.Category.Contains("Font") && e.Level == TraceRecordLevel.Warnings))
        {
            Console.WriteLine("\n⚠ Font loading issues detected");
        }

        // Image issues
        if (entries.Any(e => e.Category.Contains("Image") && e.Level == TraceRecordLevel.Warnings))
        {
            Console.WriteLine("\n⚠ Image loading issues detected");
        }

        // Performance concerns
        var verboseCount = entries.Count(e => e.Level == TraceRecordLevel.Verbose);
        if (verboseCount > 10000)
        {
            Console.WriteLine($"\n⚠ High operation count ({verboseCount}) - potential performance issue");
        }
    }
}
```

---

## Try It Yourself

### Exercise 1: Custom Logger

Implement a custom logger that:
- Writes to a file
- Rotates log files daily
- Includes timestamps
- Filters by level
- Tests with document generation

### Exercise 2: Log Dashboard

Create a simple dashboard that:
- Collects logs from multiple generations
- Shows error/warning counts
- Displays recent entries
- Highlights critical issues

### Exercise 3: Performance Profiler

Build a profiler that:
- Measures generation time
- Tracks operations by category
- Identifies slowest operations
- Generates performance report

---

## Common Pitfalls

### ❌ No Logging in Production

```csharp
// Can't diagnose issues
doc.TraceLog = null;
doc.SaveAsPDF(stream);
```

✅ **Solution:**

```csharp
// Always log at least warnings
doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
doc.SaveAsPDF(stream);
```

### ❌ Too Much Logging in Production

```csharp
// Excessive log volume, performance impact
doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);
```

✅ **Solution:**

```csharp
// Appropriate level for production
doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
```

### ❌ Ignoring Log Output

```csharp
// Logs generated but never reviewed
var logger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
doc.TraceLog = logger;
doc.SaveAsPDF(stream);
// Log entries never checked
```

✅ **Solution:**

```csharp
var logger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
doc.TraceLog = logger;
doc.SaveAsPDF(stream);

// Check for issues
if (logger.Entries.Any(e => e.Level == TraceRecordLevel.Warnings))
{
    AnalyzeAndReport(logger.Entries);
}
```

---

## Logging Best Practices Checklist

- [ ] Logging enabled in development (Verbose)
- [ ] Logging enabled in production (Warnings or Messages)
- [ ] Log output reviewed regularly
- [ ] Errors logged with context
- [ ] Performance logged for optimization
- [ ] Log level appropriate for environment
- [ ] Custom logger implemented if needed
- [ ] Log rotation configured for file logging

---

## Best Practices

1. **Always Log** - Even if only errors
2. **Verbose in Development** - See everything during testing
3. **Warnings in Production** - Balance detail and performance
4. **Review Logs Regularly** - Don't just collect
5. **Log Context** - Include document identifiers
6. **Handle Logging Errors** - Don't break generation
7. **Rotate Log Files** - Prevent disk space issues
8. **Structured Logging** - Use existing frameworks when possible

---

## Next Steps

1. **[Error Handling & Conformance](03_error_handling_conformance.md)** - Handle errors gracefully
2. **[Security](05_security.md)** - Secure your documents
3. **[Optimization & Performance](06_optimization_performance.md)** - Optimize generation

---

**Continue learning →** [Error Handling & Conformance](03_error_handling_conformance.md)
