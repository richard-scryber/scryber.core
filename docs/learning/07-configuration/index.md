---
layout: default
title: Document Configuration
nav_order: 7
parent: Learning Guides
parent_url: /learning/
has_children: true
has_toc: true
---

# Document Configuration

Configure logging, security, conformance, and optimization for production-ready PDF documents.

---

## Table of Contents

1. [Document Properties](01_document_properties.md) - Title, author, metadata, custom properties
2. [Logging](02_logging.md) - Log levels, handlers, performance logging, troubleshooting
3. [Error Handling & Conformance](03_error_handling_conformance.md) - Strict/Lax modes, validation, recovery
4. [PDF Versions](04_pdf_versions.md) - PDF versions, PDF/A, PDF/X compliance
5. [Security](05_security.md) - Encryption, passwords, permissions
6. [Optimization & Performance](06_optimization_performance.md) - File size, compression, caching, scaling
7. [Production & Deployment](07_production_deployment.md) - Production config, monitoring, troubleshooting

---

## Overview

Moving from development to production requires proper configuration. This series covers everything you need to create secure, compliant, performant PDFs that are ready for enterprise deployment.

## What is Document Configuration?

Document configuration controls:

- **Metadata** - Title, author, keywords for document information
- **Logging** - Diagnostic information and error tracking
- **Error Handling** - How the system responds to problems
- **Conformance** - Strict vs lax HTML/CSS parsing
- **PDF Version** - PDF/A, PDF/X compliance
- **Security** - Encryption, passwords, and permissions
- **Performance** - Optimization and resource management

## Quick Example

```csharp
using Scryber.Components;
using Scryber.PDF;
using Scryber.Logging;

// Load document
var doc = Document.ParseDocument("template.html");

// Set metadata
doc.Info.Title = "Annual Report 2025";
doc.Info.Author = "Acme Corporation";
doc.Info.Subject = "Financial Results";
doc.Info.Keywords = "annual, report, 2025, financial";

// Configure logging
doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);

// Set conformance mode
doc.ConformanceMode = ParserConformanceMode.Lax;

// Set PDF version
doc.RenderOptions.PDFVersion = PDFVersion.PDF17;

// Add security
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions()
{
    UserPassword = "user123",
    OwnerPassword = "owner456",
    AllowPrinting = true,
    AllowCopying = false
};

// Optimize performance
doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
doc.RenderOptions.ImageCacheDurationMinutes = 60;

// Generate PDF
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}

// Review log
foreach (var entry in ((PDFCollectorTraceLog)doc.TraceLog).Entries)
{
    Console.WriteLine($"{entry.Level}: {entry.Message}");
}
```

## What You'll Learn

This series covers production-ready configuration:

### 1. [Document Properties](01_document_properties.md)
- Title, author, subject, keywords
- Creator and producer
- Metadata and custom properties
- Document information dictionary

### 2. [Logging](02_logging.md)
- **Scryber logging system**
- Log levels (Error, Warning, Info, Verbose)
- Configuring logging output
- Custom log handlers
- Performance and diagnostic logging
- Log analysis and troubleshooting

### 3. [Error Handling & Conformance](03_error_handling_conformance.md)
- Error handling strategies
- Try-catch patterns
- **Strict vs Lax conformance modes**
- HTML conformance
- Validation and error recovery
- Graceful degradation

### 4. [PDF Versions](04_pdf_versions.md)
- PDF version selection (1.4 - 2.0)
- PDF/A compliance (archival)
- PDF/X compliance (printing)
- Feature compatibility by version

### 5. [Security](05_security.md)
- Document encryption
- User and owner passwords
- Permission levels (printing, copying, editing)
- Form filling permissions
- Security best practices

### 6. [Optimization & Performance](06_optimization_performance.md)
- File size optimization
- Image compression
- Font subsetting
- Resource caching
- Performance best practices
- Memory management
- Benchmarking and scaling

### 7. [Production & Deployment](07_production_deployment.md)
- Production configuration
- Error handling in production
- Monitoring and alerting
- Backup and recovery
- Common deployment scenarios
- Troubleshooting guide

## Prerequisites

Before starting this series:

- **Complete [Getting Started](/learning/01-getting-started/)** - Basic Scryber knowledge
- **Understand C# Basics** - Configuration is typically done in code

## Key Concepts

### Document Metadata

```csharp
doc.Info.Title = "My Document";
doc.Info.Author = "John Doe";
doc.Info.Subject = "Important Information";
doc.Info.Keywords = "pdf, scryber, documentation";
doc.Info.Creator = "Scryber.Core";
```

### Logging Levels

```
Verbose  → Everything (debugging)
Messages → Informational messages
Warnings → Potential issues
Errors   → Critical problems
Off      → No logging
```

### Conformance Modes

**Strict Mode:**
- Fails on invalid HTML/CSS
- Ensures standards compliance
- Best for controlled environments

**Lax Mode:**
- Attempts to recover from errors
- Logs warnings instead of failing
- Best for user-generated content

### Security Options

```csharp
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions()
{
    // Passwords
    UserPassword = "user123",      // Required to open
    OwnerPassword = "owner456",    // Required to change permissions

    // Permissions
    AllowPrinting = true,
    AllowCopying = false,
    AllowAnnotations = false,
    AllowFormFilling = true,
    AllowAccessibility = true,
    AllowDocumentAssembly = false,
    AllowHighQualityPrinting = true
};
```

## Logging Configuration

### Console Logging

```csharp
// Simple console output
doc.TraceLog = new PDFTraceLog();
doc.TraceLog.SetRecordLevel(TraceRecordLevel.Messages);
```

### Collector Logging

```csharp
// Collect entries for analysis
var logger = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);
doc.TraceLog = logger;

// Generate document
doc.SaveAsPDF(stream);

// Analyze log entries
foreach (var entry in logger.Entries)
{
    if (entry.Level == TraceRecordLevel.Errors)
    {
        Console.WriteLine($"ERROR: {entry.Message}");
    }
}
```

### Custom Logging

```csharp
public class CustomTraceLog : PDFTraceLog
{
    public override void Begin(TraceRecordLevel level, string category, string message)
    {
        // Custom logging to database, file, etc.
        LogToDatabase(level, category, message);
    }
}

doc.TraceLog = new CustomTraceLog();
```

## Error Handling Patterns

### Basic Try-Catch

```csharp
try
{
    var doc = Document.ParseDocument("template.html");
    doc.SaveAsPDF(stream);
}
catch (PDFException ex)
{
    // Scryber-specific errors
    Console.WriteLine($"PDF Error: {ex.Message}");
}
catch (Exception ex)
{
    // Other errors
    Console.WriteLine($"Error: {ex.Message}");
}
```

### Conformance-Based Error Handling

```csharp
// Strict mode - fail fast
doc.ConformanceMode = ParserConformanceMode.Strict;

try
{
    doc.SaveAsPDF(stream);
}
catch (PDFException ex)
{
    // Document has invalid HTML/CSS
    LogError(ex);
    throw;
}

// Lax mode - attempt recovery
doc.ConformanceMode = ParserConformanceMode.Lax;
doc.SaveAsPDF(stream);

// Check log for warnings
if (logger.HasErrors)
{
    // Document generated but with issues
    NotifyAdmin(logger.Entries);
}
```

## PDF/A Compliance

### PDF/A-1b (Basic)

```csharp
doc.RenderOptions.PDFVersion = PDFVersion.PDF14;
doc.RenderOptions.Conformance = PDFConformance.PDFA1B;

// Requirements:
// - All fonts must be embedded
// - No encryption allowed
// - No external dependencies
```

### PDF/A-2b

```csharp
doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
doc.RenderOptions.Conformance = PDFConformance.PDFA2B;

// Allows:
// - JPEG2000 compression
// - Transparency
// - Layers
```

## Security Examples

### Read-Only PDF

```csharp
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions()
{
    UserPassword = "",              // No password to open
    OwnerPassword = "secret123",    // Password to change permissions
    AllowPrinting = true,
    AllowCopying = false,
    AllowAnnotations = false,
    AllowFormFilling = false
};
```

### Confidential Document

```csharp
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions()
{
    UserPassword = "confidential",   // Password required to open
    OwnerPassword = "admin123",
    AllowPrinting = false,
    AllowCopying = false,
    AllowAnnotations = false
};
```

## Performance Optimization

### Image Compression

```csharp
doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
doc.RenderOptions.ImageCacheDurationMinutes = 60;

// Image quality (0-100, higher = better quality, larger file)
doc.RenderOptions.ImageQuality = 85;
```

### Font Subsetting

```csharp
// Only embed used characters
doc.RenderOptions.FontSubsetting = true;

// File size: 2MB → 200KB
```

### Resource Caching

```csharp
// Cache remote resources
doc.RenderOptions.ImageCacheDurationMinutes = 120;

// Improves performance for repeated generation
```

## Real-World Configuration

### Production Invoice Generator

```csharp
public class InvoiceGenerator
{
    public void GenerateInvoice(Invoice invoice, Stream output)
    {
        var doc = Document.ParseDocument("invoice-template.html");

        // Metadata
        doc.Info.Title = $"Invoice {invoice.Number}";
        doc.Info.Author = "Acme Corp";
        doc.Info.Subject = $"Invoice for {invoice.CustomerName}";
        doc.Info.Keywords = $"invoice,{invoice.Number},{invoice.Year}";

        // Production logging
        var logger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
        doc.TraceLog = logger;

        // Lax mode for customer data
        doc.ConformanceMode = ParserConformanceMode.Lax;

        // Security
        doc.RenderOptions.SecurityOptions = new PDFSecurityOptions()
        {
            AllowPrinting = true,
            AllowCopying = false
        };

        // Optimization
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.FontSubsetting = true;

        // Data binding
        doc.Params["invoice"] = invoice;

        // Generate
        try
        {
            doc.SaveAsPDF(output);

            // Log any warnings
            if (logger.Entries.Any(e => e.Level == TraceRecordLevel.Warnings))
            {
                LogWarnings(invoice.Number, logger.Entries);
            }
        }
        catch (PDFException ex)
        {
            LogError(invoice.Number, ex);
            throw;
        }
    }
}
```

### Archive-Compliant Reports

```csharp
public void GenerateArchiveReport(Report report, Stream output)
{
    var doc = Document.ParseDocument("report-template.html");

    // PDF/A compliance
    doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
    doc.RenderOptions.Conformance = PDFConformance.PDFA2B;

    // Strict conformance for compliance
    doc.ConformanceMode = ParserConformanceMode.Strict;

    // Detailed logging
    doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);

    // No security (PDF/A doesn't allow encryption)
    doc.RenderOptions.SecurityOptions = null;

    // Full font embedding required
    doc.RenderOptions.FontSubsetting = false;

    doc.Params["report"] = report;
    doc.SaveAsPDF(output);
}
```

## Learning Path

**Recommended progression:**

1. **Set Document Properties** - Metadata basics
2. **Configure Logging** - Diagnostic information
3. **Handle Errors** - Conformance and recovery
4. **Choose PDF Version** - Compliance requirements
5. **Add Security** - Protection and permissions
6. **Optimize Performance** - Production-ready
7. **Deploy to Production** - Best practices

## Tips for Success

1. **Log Everything in Development** - Use Verbose level
2. **Log Warnings in Production** - Balance detail and noise
3. **Use Lax Mode for User Content** - Graceful degradation
4. **Use Strict Mode for Controlled Content** - Ensure quality
5. **Always Set Metadata** - Helps users find documents
6. **Test Security Settings** - Verify permissions work
7. **Benchmark Performance** - Optimize based on data
8. **Cache Resources** - Reduce generation time

## Common Pitfalls

❌ **Not handling errors**
```csharp
// Fails silently
doc.SaveAsPDF(stream);
```

✅ **Proper error handling**
```csharp
try
{
    doc.SaveAsPDF(stream);
}
catch (PDFException ex)
{
    Log.Error($"PDF generation failed: {ex.Message}");
    throw;
}
```

❌ **No logging in production**
```csharp
// Can't diagnose issues
doc.TraceLog = null;
```

✅ **Always log warnings and errors**
```csharp
doc.TraceLog = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
```

## Next Steps

Ready to configure production-ready documents? Start with [Document Properties](01_document_properties.md) to learn about metadata.

Jump to specific topics:
- [Logging](02_logging.md) for diagnostic information
- [Security](05_security.md) for protection
- [Optimization & Performance](06_optimization_performance.md) for production

---

**Related Series:**
- [Getting Started](/learning/01-getting-started/) - Basic concepts
- [Practical Applications](/learning/08-practical/) - Real-world examples

---

**Begin configuration →** [Document Properties](01_document_properties.md)
