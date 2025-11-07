---
layout: default
title: Optimization & Performance
nav_order: 6
parent: Document Configuration
parent_url: /learning/07-configuration/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Optimization & Performance

Master performance optimization techniques to create efficient, fast-generating PDFs with minimal file sizes for production environments.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Optimize file sizes
- Improve generation speed
- Configure compression
- Implement resource caching
- Manage memory efficiently
- Benchmark performance
- Scale PDF generation

---

## Compression

### Enable Compression

```csharp
using Scryber.Components;
using Scryber.PDF;

var doc = Document.ParseDocument("template.html");

// Enable compression
doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;

// Result: Typically 40-60% file size reduction
doc.SaveAsPDF(stream);
```

### Compression Types

```csharp
// No compression
doc.RenderOptions.Compression = PDFCompressionType.None;

// FlateDecode (recommended)
doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;

// File size comparison:
// No compression: 2.5 MB
// FlateDecode:    1.0 MB (60% reduction)
```

---

## Image Optimization

### Image Quality

```csharp
// Balance quality vs file size
doc.RenderOptions.ImageQuality = 85;  // 0-100

// Quality levels:
// 100 - Maximum quality, large files
// 85  - High quality, good compression (recommended)
// 70  - Medium quality, smaller files
// 50  - Lower quality, smallest files
```

### Image Caching

```csharp
// Cache remote images
doc.RenderOptions.ImageCacheDurationMinutes = 60;

// Benefits:
// - Faster repeated generation
// - Reduced network calls
// - Lower bandwidth usage
```

---

## Font Optimization

### Font Subsetting

```csharp
// Embed only used characters
doc.RenderOptions.FontSubsetting = true;

// File size impact:
// Full font embedding:    2.0 MB
// Font subsetting:        200 KB (90% reduction)
```

**When to use subsetting:**
- ✅ Standard documents
- ✅ Production environments
- ✅ File size is concern

**When to embed full fonts:**
- ⚠️ PDF/A compliance
- ⚠️ Extensive character sets
- ⚠️ Dynamic content with unknown characters

---

## Resource Caching

### Template Caching

```csharp
// Cache parsed templates
private static readonly Dictionary<string, Document> _templateCache =
    new Dictionary<string, Document>();

public Document GetCachedTemplate(string templatePath)
{
    if (!_templateCache.ContainsKey(templatePath))
    {
        var doc = Document.ParseDocument(templatePath);
        _templateCache[templatePath] = doc;
    }

    return _templateCache[templatePath];
}
```

**Caution:** Be careful with template caching as it keeps documents in memory.

### Image Caching

```csharp
// Automatic image caching
doc.RenderOptions.ImageCacheDurationMinutes = 120;

// Caches images for 2 hours
// Subsequent generations reuse cached images
```

---

## Performance Monitoring

### Basic Benchmarking

```csharp
using System.Diagnostics;

public class PerformanceBenchmark
{
    public BenchmarkResult BenchmarkGeneration(string templatePath, Stream output)
    {
        var result = new BenchmarkResult();
        var stopwatch = Stopwatch.StartNew();

        // Parse time
        var doc = Document.ParseDocument(templatePath);
        result.ParseTimeMs = stopwatch.ElapsedMilliseconds;

        // Render time
        stopwatch.Restart();
        doc.SaveAsPDF(output);
        result.RenderTimeMs = stopwatch.ElapsedMilliseconds;

        // File size
        result.FileSizeBytes = output.Length;

        return result;
    }
}

public class BenchmarkResult
{
    public long ParseTimeMs { get; set; }
    public long RenderTimeMs { get; set; }
    public long FileSizeBytes { get; set; }

    public long TotalTimeMs => ParseTimeMs + RenderTimeMs;
    public double FileSizeMB => FileSizeBytes / (1024.0 * 1024.0);
}
```

---

## Practical Examples

### Example 1: Optimized Invoice Generator

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.IO;

public class OptimizedInvoiceGenerator
{
    private readonly string _templatePath;

    public OptimizedInvoiceGenerator(string templatePath)
    {
        _templatePath = templatePath;
    }

    public void GenerateInvoice(Invoice invoice, Stream output)
    {
        var doc = Document.ParseDocument(_templatePath);

        // Optimize for file size
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.FontSubsetting = true;
        doc.RenderOptions.ImageQuality = 85;

        // Optimize for performance
        doc.RenderOptions.ImageCacheDurationMinutes = 60;

        // Set metadata
        doc.Info.Title = $"Invoice {invoice.Number}";
        doc.Info.Author = invoice.CompanyName;

        // Bind data
        doc.Params["invoice"] = invoice;

        // Generate
        doc.SaveAsPDF(output);
    }
}

// Usage
var generator = new OptimizedInvoiceGenerator("invoice-template.html");
using (var output = new FileStream("invoice.pdf", FileMode.Create))
{
    generator.GenerateInvoice(invoice, output);
    Console.WriteLine($"File size: {output.Length / 1024} KB");
}
```

### Example 2: High-Performance Report Generator

```csharp
using Scryber.Components;
using Scryber.Logging;
using Scryber.PDF;
using System;
using System.Diagnostics;
using System.IO;

public class HighPerformanceReportGenerator
{
    private readonly string _templatePath;

    public HighPerformanceReportGenerator(string templatePath)
    {
        _templatePath = templatePath;
    }

    public GenerationMetrics GenerateReport(ReportData data, Stream output)
    {
        var metrics = new GenerationMetrics { ReportId = data.ReportId };
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Parse
            var doc = Document.ParseDocument(_templatePath);
            metrics.ParseTimeMs = stopwatch.ElapsedMilliseconds;

            // Configure for maximum performance
            ConfigureForPerformance(doc);

            // Bind data
            stopwatch.Restart();
            doc.Params["report"] = data;
            metrics.DataBindingTimeMs = stopwatch.ElapsedMilliseconds;

            // Render
            stopwatch.Restart();
            doc.SaveAsPDF(output);
            metrics.RenderTimeMs = stopwatch.ElapsedMilliseconds;

            // Metrics
            metrics.FileSizeBytes = output.Length;
            metrics.Success = true;
        }
        catch (Exception ex)
        {
            metrics.Success = false;
            metrics.Error = ex.Message;
        }

        return metrics;
    }

    private void ConfigureForPerformance(Document doc)
    {
        // Compression
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;

        // Font optimization
        doc.RenderOptions.FontSubsetting = true;

        // Image optimization
        doc.RenderOptions.ImageQuality = 85;
        doc.RenderOptions.ImageCacheDurationMinutes = 120;

        // Minimal logging for performance
        doc.TraceLog = new PDFTraceLog();
        doc.TraceLog.SetRecordLevel(TraceRecordLevel.Errors);
    }
}

public class GenerationMetrics
{
    public string ReportId { get; set; }
    public bool Success { get; set; }
    public string Error { get; set; }
    public long ParseTimeMs { get; set; }
    public long DataBindingTimeMs { get; set; }
    public long RenderTimeMs { get; set; }
    public long FileSizeBytes { get; set; }

    public long TotalTimeMs => ParseTimeMs + DataBindingTimeMs + RenderTimeMs;
    public double FileSizeMB => FileSizeBytes / (1024.0 * 1024.0);

    public void PrintSummary()
    {
        Console.WriteLine($"Report: {ReportId}");
        Console.WriteLine($"  Parse:  {ParseTimeMs}ms");
        Console.WriteLine($"  Bind:   {DataBindingTimeMs}ms");
        Console.WriteLine($"  Render: {RenderTimeMs}ms");
        Console.WriteLine($"  Total:  {TotalTimeMs}ms");
        Console.WriteLine($"  Size:   {FileSizeMB:F2} MB");
    }
}

// Usage
var generator = new HighPerformanceReportGenerator("report-template.html");
using (var output = new FileStream("report.pdf", FileMode.Create))
{
    var metrics = generator.GenerateReport(reportData, output);
    metrics.PrintSummary();
}
```

### Example 3: Bulk Generation Optimizer

```csharp
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

public class BulkGenerationOptimizer
{
    private readonly string _templatePath;
    private readonly int _maxParallelism;

    public BulkGenerationOptimizer(string templatePath, int maxParallelism = 4)
    {
        _templatePath = templatePath;
        _maxParallelism = maxParallelism;
    }

    public async Task<BulkGenerationResult> GenerateBulk(
        List<DocumentData> documents,
        string outputDirectory)
    {
        var result = new BulkGenerationResult
        {
            TotalDocuments = documents.Count
        };

        var stopwatch = Stopwatch.StartNew();

        // Parallel generation
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = _maxParallelism
        };

        var successCount = 0;
        var failureCount = 0;

        Parallel.ForEach(documents, options, docData =>
        {
            try
            {
                var outputPath = Path.Combine(outputDirectory, $"{docData.Id}.pdf");

                using (var output = new FileStream(outputPath, FileMode.Create))
                {
                    GenerateSingleDocument(docData, output);
                    Interlocked.Increment(ref successCount);
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref failureCount);
                Console.WriteLine($"Failed to generate {docData.Id}: {ex.Message}");
            }
        });

        stopwatch.Stop();

        result.SuccessCount = successCount;
        result.FailureCount = failureCount;
        result.TotalTimeMs = stopwatch.ElapsedMilliseconds;
        result.AverageTimeMs = result.TotalTimeMs / documents.Count;

        return result;
    }

    private void GenerateSingleDocument(DocumentData data, Stream output)
    {
        var doc = Document.ParseDocument(_templatePath);

        // Optimize
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.FontSubsetting = true;
        doc.RenderOptions.ImageQuality = 85;

        // Minimal logging
        doc.TraceLog = new PDFTraceLog();
        doc.TraceLog.SetRecordLevel(TraceRecordLevel.Off);

        // Bind and generate
        doc.Params["data"] = data;
        doc.SaveAsPDF(output);
    }
}

public class BulkGenerationResult
{
    public int TotalDocuments { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public long TotalTimeMs { get; set; }
    public long AverageTimeMs { get; set; }

    public void PrintSummary()
    {
        Console.WriteLine($"\nBulk Generation Results:");
        Console.WriteLine($"  Total:     {TotalDocuments}");
        Console.WriteLine($"  Success:   {SuccessCount}");
        Console.WriteLine($"  Failed:    {FailureCount}");
        Console.WriteLine($"  Total Time: {TotalTimeMs}ms");
        Console.WriteLine($"  Avg Time:   {AverageTimeMs}ms per document");
        Console.WriteLine($"  Throughput: {TotalDocuments / (TotalTimeMs / 1000.0):F1} docs/sec");
    }
}

// Usage
var optimizer = new BulkGenerationOptimizer("template.html", maxParallelism: 4);
var result = await optimizer.GenerateBulk(documents, "./output");
result.PrintSummary();
```

---

## Memory Management

### Disposal Pattern

```csharp
// ✅ Proper disposal
using (var doc = Document.ParseDocument("template.html"))
{
    using (var output = new FileStream("output.pdf", FileMode.Create))
    {
        doc.SaveAsPDF(output);
    }
} // doc is disposed here

// ❌ Not disposing
var doc = Document.ParseDocument("template.html");
doc.SaveAsPDF(output);
// Memory not released!
```

### Large Document Handling

```csharp
// For very large documents, generate in chunks if possible
// Or increase available memory

// In Program.cs or startup:
GC.Collect();  // Force garbage collection between batches
GC.WaitForPendingFinalizers();
```

---

## Performance Targets

### Generation Speed

| Document Type | Target Time | Max Time |
|---------------|-------------|----------|
| Simple invoice | < 500ms | < 2s |
| Standard report | < 2s | < 5s |
| Complex report | < 5s | < 15s |
| Large catalog | < 15s | < 60s |

### File Sizes

| Content Type | Target Size | Notes |
|--------------|-------------|-------|
| Text-only | < 100 KB | With compression |
| With images | < 2 MB | Optimized images |
| Image-heavy | < 10 MB | Quality vs size |
| Catalog | < 50 MB | Consider splitting |

---

## Try It Yourself

### Exercise 1: Optimization Comparison

Compare performance with different settings:
- No compression vs FlateDecode
- Full fonts vs subsetting
- Different image quality levels
- Measure time and file size

### Exercise 2: Performance Profiler

Build a profiler that:
- Measures parse/render times separately
- Tracks file sizes
- Identifies bottlenecks
- Generates performance report

### Exercise 3: Bulk Generator

Create a bulk generator that:
- Processes multiple documents
- Uses parallel processing
- Tracks success/failure rates
- Calculates throughput

---

## Common Pitfalls

### ❌ No Compression

```csharp
// Large file sizes
doc.RenderOptions.Compression = PDFCompressionType.None;
// Result: 2.5 MB
```

✅ **Solution:**

```csharp
// Smaller files
doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
// Result: 1.0 MB (60% smaller)
```

### ❌ Full Font Embedding

```csharp
// Unnecessary for most documents
doc.RenderOptions.FontSubsetting = false;
// Adds 1-2 MB per font
```

✅ **Solution:**

```csharp
// Subset fonts
doc.RenderOptions.FontSubsetting = true;
// Adds only 50-200 KB per font
```

### ❌ Excessive Logging in Production

```csharp
// Slows down generation
doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);
```

✅ **Solution:**

```csharp
// Minimal logging
doc.TraceLog = new PDFTraceLog();
doc.TraceLog.SetRecordLevel(TraceRecordLevel.Errors);
```

---

## Optimization Checklist

- [ ] Compression enabled (FlateDecode)
- [ ] Font subsetting enabled
- [ ] Image quality configured (85)
- [ ] Image caching enabled
- [ ] Logging level appropriate
- [ ] Disposal pattern used
- [ ] Performance benchmarked
- [ ] File sizes acceptable

---

## Best Practices

1. **Always Enable Compression** - 40-60% file size reduction
2. **Use Font Subsetting** - Unless PDF/A or special requirements
3. **Optimize Images** - Before adding to PDF
4. **Quality 85** - Good balance for images
5. **Cache Resources** - For repeated generations
6. **Minimal Logging** - In production
7. **Proper Disposal** - Use using statements
8. **Benchmark Everything** - Measure before optimizing

---

## Scaling Strategies

### Vertical Scaling

```csharp
// Increase resources per instance
// - More CPU cores
// - More RAM
// - Faster storage (SSD)
```

### Horizontal Scaling

```csharp
// Multiple instances
// - Load balancer
// - Queue-based processing
// - Distributed caching
```

### Queue-Based Processing

```csharp
// Async generation
// 1. Accept request → queue
// 2. Background worker processes queue
// 3. Notify completion
// 4. Provide download link
```

---

## Next Steps

1. **[Production & Deployment](07_production_deployment.md)** - Deploy to production
2. **[Practical Applications](/learning/08-practical/)** - Real-world examples
3. **Review previous topics** - Reinforce learning

---

**Continue learning →** [Production & Deployment](07_production_deployment.md)
