---
layout: default
title: Production & Deployment
nav_order: 7
parent: Document Configuration
parent_url: /learning/07-configuration/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Production & Deployment

Master production-ready configuration, deployment strategies, monitoring, error handling, and troubleshooting for reliable PDF generation at scale.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Configure for production environments
- Implement proper error handling
- Set up monitoring and alerting
- Troubleshoot common issues
- Handle high-volume scenarios
- Deploy reliably
- Maintain production systems

---

## Production Configuration

### Complete Production Setup

```csharp
using Scryber.Components;
using Scryber.Logging;
using Scryber.PDF;
using System;
using System.IO;

public class ProductionPDFGenerator
{
    private readonly IConfiguration _config;
    private readonly ILogger<ProductionPDFGenerator> _logger;

    public ProductionPDFGenerator(IConfiguration config, ILogger<ProductionPDFGenerator> logger)
    {
        _config = config;
        _logger = logger;
    }

    public Document ConfigureForProduction(string templatePath)
    {
        var doc = Document.ParseDocument(templatePath);

        // Logging - warnings and errors only
        var scryberLogger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
        doc.TraceLog = scryberLogger;

        // Conformance - lax for user content
        doc.ConformanceMode = ParserConformanceMode.Lax;

        // PDF Version - modern standard
        doc.RenderOptions.PDFVersion = PDFVersion.PDF17;

        // Optimization
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.FontSubsetting = true;
        doc.RenderOptions.ImageQuality = 85;
        doc.RenderOptions.ImageCacheDurationMinutes = 120;

        // Security (if configured)
        var ownerPassword = _config["PDF:Security:OwnerPassword"];
        if (!string.IsNullOrEmpty(ownerPassword))
        {
            doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
            {
                OwnerPassword = ownerPassword,
                AllowPrinting = true,
                AllowCopying = false,
                AllowAccessibility = true
            };
        }

        return doc;
    }
}
```

---

## Error Handling

### Production Error Handling

```csharp
public class RobustPDFService
{
    private readonly ILogger<RobustPDFService> _logger;
    private readonly IMetrics _metrics;

    public async Task<PDFGenerationResult> GenerateAsync(
        string templatePath,
        object data,
        Stream output)
    {
        var result = new PDFGenerationResult();
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Validate inputs
            ValidateInputs(templatePath, data, output);

            // Generate with timeout
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            {
                await Task.Run(() =>
                {
                    var doc = Document.ParseDocument(templatePath);
                    ConfigureForProduction(doc);
                    doc.Params["data"] = data;
                    doc.SaveAsPDF(output);
                }, cts.Token);
            }

            result.Success = true;
            _metrics.RecordSuccess("pdf_generation");
        }
        catch (ArgumentException ex)
        {
            // Validation error
            result.Success = false;
            result.ErrorType = "Validation";
            result.ErrorMessage = ex.Message;
            _logger.LogWarning(ex, "Validation failed for PDF generation");
            _metrics.RecordFailure("pdf_generation", "validation");
        }
        catch (FileNotFoundException ex)
        {
            // Template not found
            result.Success = false;
            result.ErrorType = "TemplateNotFound";
            result.ErrorMessage = $"Template not found: {ex.FileName}";
            _logger.LogError(ex, "Template file not found");
            _metrics.RecordFailure("pdf_generation", "template_not_found");
        }
        catch (PDFException ex)
        {
            // PDF generation error
            result.Success = false;
            result.ErrorType = "PDFGeneration";
            result.ErrorMessage = ex.Message;
            _logger.LogError(ex, "PDF generation failed");
            _metrics.RecordFailure("pdf_generation", "pdf_error");
        }
        catch (TimeoutException ex)
        {
            // Generation timeout
            result.Success = false;
            result.ErrorType = "Timeout";
            result.ErrorMessage = "PDF generation timed out";
            _logger.LogError(ex, "PDF generation timeout");
            _metrics.RecordFailure("pdf_generation", "timeout");
        }
        catch (Exception ex)
        {
            // Unexpected error
            result.Success = false;
            result.ErrorType = "Unexpected";
            result.ErrorMessage = "An unexpected error occurred";
            _logger.LogCritical(ex, "Unexpected error in PDF generation");
            _metrics.RecordFailure("pdf_generation", "unexpected");
        }
        finally
        {
            stopwatch.Stop();
            result.GenerationTimeMs = stopwatch.ElapsedMilliseconds;
            _metrics.RecordTiming("pdf_generation", stopwatch.ElapsedMilliseconds);
        }

        return result;
    }

    private void ValidateInputs(string templatePath, object data, Stream output)
    {
        if (string.IsNullOrWhiteSpace(templatePath))
            throw new ArgumentException("Template path is required", nameof(templatePath));

        if (data == null)
            throw new ArgumentException("Data is required", nameof(data));

        if (output == null || !output.CanWrite)
            throw new ArgumentException("Output stream must be writable", nameof(output));
    }

    private void ConfigureForProduction(Document doc)
    {
        doc.TraceLog = new PDFTraceLog();
        doc.TraceLog.SetRecordLevel(TraceRecordLevel.Errors);
        doc.ConformanceMode = ParserConformanceMode.Lax;
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.FontSubsetting = true;
    }
}

public class PDFGenerationResult
{
    public bool Success { get; set; }
    public string ErrorType { get; set; }
    public string ErrorMessage { get; set; }
    public long GenerationTimeMs { get; set; }
}
```

---

## Monitoring and Metrics

### Application Insights Integration

```csharp
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

public class MonitoredPDFGenerator
{
    private readonly TelemetryClient _telemetry;

    public MonitoredPDFGenerator(TelemetryClient telemetry)
    {
        _telemetry = telemetry;
    }

    public void Generate(string templatePath, object data, Stream output)
    {
        using (var operation = _telemetry.StartOperation<RequestTelemetry>("GeneratePDF"))
        {
            try
            {
                operation.Telemetry.Properties["template"] = templatePath;

                var doc = Document.ParseDocument(templatePath);
                doc.Params["data"] = data;
                doc.SaveAsPDF(output);

                // Track success
                _telemetry.TrackMetric("PDF.FileSize", output.Length);
                _telemetry.TrackEvent("PDF.Generated.Success");

                operation.Telemetry.Success = true;
            }
            catch (Exception ex)
            {
                _telemetry.TrackException(ex);
                operation.Telemetry.Success = false;
                throw;
            }
        }
    }
}
```

### Health Checks

```csharp
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class PDFGenerationHealthCheck : IHealthCheck
{
    private readonly string _testTemplatePath;

    public PDFGenerationHealthCheck(string testTemplatePath)
    {
        _testTemplatePath = testTemplatePath;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Test generation with simple template
            using (var output = new MemoryStream())
            {
                var doc = Document.ParseDocument(_testTemplatePath);
                doc.SaveAsPDF(output);

                if (output.Length > 0)
                {
                    return HealthCheckResult.Healthy("PDF generation is working");
                }
                else
                {
                    return HealthCheckResult.Degraded("PDF generated but file is empty");
                }
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("PDF generation failed", ex);
        }
    }
}

// In Startup.cs or Program.cs
services.AddHealthChecks()
    .AddCheck<PDFGenerationHealthCheck>("pdf_generation");
```

---

## Deployment Strategies

### Environment Configuration

```json
// appsettings.Production.json
{
  "PDF": {
    "TemplatePath": "/app/templates",
    "OutputPath": "/app/output",
    "Logging": {
      "Level": "Warning"
    },
    "Optimization": {
      "Compression": true,
      "FontSubsetting": true,
      "ImageQuality": 85,
      "ImageCacheDurationMinutes": 120
    },
    "Security": {
      "OwnerPassword": "#{PDF_OWNER_PASSWORD}#",  // From Azure Key Vault
      "AllowPrinting": true,
      "AllowCopying": false
    },
    "Limits": {
      "MaxFileSizeBytes": 52428800,  // 50 MB
      "GenerationTimeoutSeconds": 30
    }
  }
}
```

### Docker Deployment

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PDFService/PDFService.csproj", "PDFService/"]
RUN dotnet restore "PDFService/PDFService.csproj"
COPY . .
WORKDIR "/src/PDFService"
RUN dotnet build "PDFService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PDFService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# Install fonts
RUN apt-get update && apt-get install -y \
    fonts-liberation \
    fonts-dejavu-core \
    && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .
COPY templates /app/templates

ENTRYPOINT ["dotnet", "PDFService.dll"]
```

---

## Troubleshooting Guide

### Common Issues and Solutions

#### 1. Font Not Found

**Symptoms:**
- Missing text in PDF
- Warning: "Font 'XYZ' not found"

**Solutions:**
```csharp
// Install fonts on server
// Linux: apt-get install fonts-liberation

// Or provide font files
@font-face {
    font-family: 'CustomFont';
    src: url('./fonts/CustomFont.ttf') format('truetype');
}
```

#### 2. Memory Issues

**Symptoms:**
- OutOfMemoryException
- Slow generation
- Server crashes

**Solutions:**
```csharp
// 1. Dispose properly
using (var doc = Document.ParseDocument(path))
{
    using (var output = new FileStream("output.pdf", FileMode.Create))
    {
        doc.SaveAsPDF(output);
    }
}

// 2. Limit concurrent generation
var semaphore = new SemaphoreSlim(4);  // Max 4 concurrent
await semaphore.WaitAsync();
try
{
    GeneratePDF();
}
finally
{
    semaphore.Release();
}

// 3. Increase available memory
// Docker: docker run -m 2g ...
```

#### 3. Timeout Issues

**Symptoms:**
- Generation takes too long
- Request timeouts

**Solutions:**
```csharp
// 1. Optimize templates
// - Reduce image sizes
// - Simplify complex layouts

// 2. Implement timeout
using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
{
    await GenerateAsync(cts.Token);
}

// 3. Queue for async processing
// Submit → Queue → Background worker → Notify
```

#### 4. Template Not Found

**Symptoms:**
- FileNotFoundException
- "Template path not found"

**Solutions:**
```csharp
// 1. Use absolute paths
var templatePath = Path.Combine(_config["PDF:TemplatePath"], "invoice.html");

// 2. Verify paths in container
// Docker: COPY templates /app/templates

// 3. Check file permissions
// Linux: chmod 644 /app/templates/*.html
```

---

## Production Checklist

### Before Deployment

- [ ] Error handling implemented
- [ ] Logging configured (Warnings level)
- [ ] Metrics/monitoring set up
- [ ] Health checks implemented
- [ ] Configuration externalized
- [ ] Secrets managed securely
- [ ] Resource limits configured
- [ ] Fonts available on server

### After Deployment

- [ ] Health check endpoint tested
- [ ] Generation tested with real data
- [ ] Performance benchmarked
- [ ] Error alerts configured
- [ ] Logs reviewed
- [ ] Memory usage monitored
- [ ] File sizes validated
- [ ] Backup/recovery tested

---

## Scaling Considerations

### Queue-Based Architecture

```csharp
// 1. API receives request
[HttpPost("generate")]
public async Task<IActionResult> QueueGeneration([FromBody] PDFRequest request)
{
    // Queue the request
    var jobId = Guid.NewGuid().ToString();
    await _queue.EnqueueAsync(new PDFJob
    {
        JobId = jobId,
        TemplateId = request.TemplateId,
        Data = request.Data
    });

    // Return job ID
    return Accepted(new { JobId = jobId });
}

// 2. Background worker processes
public class PDFGenerationWorker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var job = await _queue.DequeueAsync(stoppingToken);
            if (job != null)
            {
                await ProcessJobAsync(job);
            }
        }
    }
}

// 3. Client polls for status
[HttpGet("status/{jobId}")]
public async Task<IActionResult> GetStatus(string jobId)
{
    var status = await _statusStore.GetAsync(jobId);
    return Ok(status);
}
```

### Load Balancing

```
Client → Load Balancer → [PDF Service 1]
                       → [PDF Service 2]
                       → [PDF Service 3]
```

---

## Best Practices Summary

1. **Externalize Configuration** - Use appsettings.json and environment variables
2. **Proper Error Handling** - Catch and log all exceptions
3. **Monitor Everything** - Metrics, logs, health checks
4. **Optimize for Production** - Compression, caching, subsetting
5. **Implement Timeouts** - Prevent hanging requests
6. **Use Queues** - For async processing at scale
7. **Health Checks** - Monitor service availability
8. **Secure Secrets** - Use Key Vault or similar

---

## Next Steps

You've completed the Document Configuration series! Continue your journey:

1. **[Practical Applications](/learning/08-practical/)** - Real-world document examples
2. **Review previous series** - Reinforce learning
3. **Build your own** - Apply what you've learned

---

**Continue learning →** [Practical Applications Series](/learning/08-practical/)
