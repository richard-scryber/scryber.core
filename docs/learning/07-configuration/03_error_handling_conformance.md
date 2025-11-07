---
layout: default
title: Error Handling & Conformance
nav_order: 3
parent: Document Configuration
parent_url: /learning/07-configuration/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Error Handling & Conformance

Master error handling strategies and conformance modes to build robust PDF generation systems that gracefully handle invalid input and unexpected situations.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Implement proper error handling
- Choose between Strict and Lax conformance modes
- Handle parsing and rendering errors
- Validate input before generation
- Build fault-tolerant systems
- Recover from common errors

---

## Conformance Modes

Scryber provides two conformance modes for HTML/CSS parsing:

### Strict Mode

```csharp
using Scryber.Components;
using Scryber.PDF;

var doc = Document.ParseDocument("template.html");
doc.ConformanceMode = ParserConformanceMode.Strict;

// Strict mode behavior:
// - Fails on invalid HTML/CSS
// - Throws exceptions for errors
// - Ensures standards compliance
// - Best for controlled environments
```

**Use Strict Mode when:**
- Templates are controlled by your team
- You want to catch errors early
- Standards compliance is critical
- Testing and validation

### Lax Mode

```csharp
var doc = Document.ParseDocument("template.html");
doc.ConformanceMode = ParserConformanceMode.Lax;

// Lax mode behavior:
// - Attempts to recover from errors
// - Logs warnings instead of failing
// - Skips invalid elements
// - Best for user-generated content
```

**Use Lax Mode when:**
- Handling user-generated HTML
- Graceful degradation needed
- Can't guarantee input quality
- Production systems

---

## Basic Error Handling

### Try-Catch Pattern

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.IO;

public void GeneratePDF(string templatePath, Stream output)
{
    try
    {
        var doc = Document.ParseDocument(templatePath);
        doc.SaveAsPDF(output);
        Console.WriteLine("PDF generated successfully");
    }
    catch (PDFException ex)
    {
        // Scryber-specific errors
        Console.WriteLine($"PDF Generation Error: {ex.Message}");
        throw;
    }
    catch (FileNotFoundException ex)
    {
        // File errors
        Console.WriteLine($"Template not found: {ex.FileName}");
        throw;
    }
    catch (Exception ex)
    {
        // Other errors
        Console.WriteLine($"Unexpected error: {ex.Message}");
        throw;
    }
}
```

### With Logging

```csharp
using Scryber.Logging;

public void GeneratePDFWithLogging(string templatePath, Stream output)
{
    var logger = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);

    try
    {
        var doc = Document.ParseDocument(templatePath);
        doc.TraceLog = logger;
        doc.SaveAsPDF(output);
    }
    catch (Exception ex)
    {
        // Log includes diagnostic information
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine("\nDiagnostic Log:");

        foreach (var entry in logger.Entries)
        {
            Console.WriteLine($"[{entry.Level}] {entry.Message}");
        }

        throw;
    }
}
```

---

## Conformance-Based Error Handling

### Strict Mode with Validation

```csharp
public void GenerateStrictPDF(string templatePath, Stream output)
{
    var logger = new PDFCollectorTraceLog(TraceRecordLevel.Verbose);

    try
    {
        var doc = Document.ParseDocument(templatePath);
        doc.ConformanceMode = ParserConformanceMode.Strict;
        doc.TraceLog = logger;

        doc.SaveAsPDF(output);
    }
    catch (PDFException ex)
    {
        // Document has invalid HTML/CSS
        Console.WriteLine("Document validation failed:");
        Console.WriteLine(ex.Message);

        // Show specific errors from log
        var errors = logger.Entries
            .Where(e => e.Level == TraceRecordLevel.Errors)
            .ToList();

        foreach (var error in errors)
        {
            Console.WriteLine($"  - {error.Category}: {error.Message}");
        }

        throw;
    }
}
```

### Lax Mode with Warning Checks

```csharp
public bool GenerateLaxPDF(string templatePath, Stream output)
{
    var logger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);

    try
    {
        var doc = Document.ParseDocument(templatePath);
        doc.ConformanceMode = ParserConformanceMode.Lax;
        doc.TraceLog = logger;

        doc.SaveAsPDF(output);

        // Check for warnings
        var warnings = logger.Entries
            .Where(e => e.Level == TraceRecordLevel.Warnings)
            .ToList();

        if (warnings.Any())
        {
            Console.WriteLine($"PDF generated with {warnings.Count} warnings:");
            foreach (var warning in warnings.Take(5))
            {
                Console.WriteLine($"  - {warning.Message}");
            }

            // Return false to indicate issues (but PDF was generated)
            return false;
        }

        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"PDF generation failed: {ex.Message}");
        throw;
    }
}
```

---

## Input Validation

### Pre-Generation Validation

```csharp
public class DocumentValidator
{
    public ValidationResult ValidateTemplate(string templatePath)
    {
        var result = new ValidationResult();

        // Check file exists
        if (!File.Exists(templatePath))
        {
            result.Errors.Add($"Template file not found: {templatePath}");
            return result;
        }

        // Check file is readable
        try
        {
            var content = File.ReadAllText(templatePath);

            // Basic HTML validation
            if (!content.Contains("<html") && !content.Contains("<HTML"))
            {
                result.Warnings.Add("Template may not be valid HTML");
            }

            // Check for required elements
            if (!content.Contains("<body") && !content.Contains("<BODY"))
            {
                result.Warnings.Add("Template missing body element");
            }
        }
        catch (Exception ex)
        {
            result.Errors.Add($"Cannot read template: {ex.Message}");
            return result;
        }

        result.IsValid = !result.Errors.Any();
        return result;
    }

    public ValidationResult ValidateData(object data)
    {
        var result = new ValidationResult();

        if (data == null)
        {
            result.Errors.Add("Data cannot be null");
            result.IsValid = false;
            return result;
        }

        // Add specific data validation logic here

        result.IsValid = !result.Errors.Any();
        return result;
    }
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public List<string> Warnings { get; set; } = new List<string>();
}

// Usage
var validator = new DocumentValidator();
var validationResult = validator.ValidateTemplate("template.html");

if (!validationResult.IsValid)
{
    Console.WriteLine("Validation failed:");
    foreach (var error in validationResult.Errors)
    {
        Console.WriteLine($"  - {error}");
    }
    return;
}

// Proceed with generation
GeneratePDF("template.html", output);
```

---

## Practical Examples

### Example 1: Robust Invoice Generator

```csharp
using Scryber.Components;
using Scryber.Logging;
using Scryber.PDF;
using System;
using System.IO;
using System.Linq;

public class RobustInvoiceGenerator
{
    private readonly string _templatePath;
    private readonly ILogger _logger;

    public RobustInvoiceGenerator(string templatePath, ILogger logger)
    {
        _templatePath = templatePath;
        _logger = logger;
    }

    public GenerationResult Generate(Invoice invoice, Stream output)
    {
        var result = new GenerationResult { InvoiceNumber = invoice.Number };
        var scryberLogger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);

        try
        {
            // Validate input
            ValidateInvoice(invoice);

            // Load and configure document
            var doc = Document.ParseDocument(_templatePath);
            doc.ConformanceMode = ParserConformanceMode.Lax;  // Graceful degradation
            doc.TraceLog = scryberLogger;

            // Set metadata
            doc.Info.Title = $"Invoice {invoice.Number}";
            doc.Info.Author = invoice.CompanyName;

            // Bind data
            doc.Params["invoice"] = invoice;

            // Generate
            doc.SaveAsPDF(output);

            // Check for warnings
            var warnings = scryberLogger.Entries
                .Where(e => e.Level == TraceRecordLevel.Warnings)
                .Select(e => e.Message)
                .ToList();

            if (warnings.Any())
            {
                result.Warnings.AddRange(warnings);
                _logger.LogWarning($"Invoice {invoice.Number} generated with {warnings.Count} warnings");
            }

            result.Success = true;
            result.Message = "Invoice generated successfully";
        }
        catch (ArgumentException ex)
        {
            // Validation error
            result.Success = false;
            result.Message = $"Invalid invoice data: {ex.Message}";
            _logger.LogError(ex, $"Validation failed for invoice {invoice.Number}");
        }
        catch (PDFException ex)
        {
            // PDF generation error
            result.Success = false;
            result.Message = $"PDF generation failed: {ex.Message}";
            result.LogEntries.AddRange(scryberLogger.Entries.Select(e => e.Message));
            _logger.LogError(ex, $"PDF generation failed for invoice {invoice.Number}");
        }
        catch (Exception ex)
        {
            // Unexpected error
            result.Success = false;
            result.Message = $"Unexpected error: {ex.Message}";
            _logger.LogError(ex, $"Unexpected error generating invoice {invoice.Number}");
        }

        return result;
    }

    private void ValidateInvoice(Invoice invoice)
    {
        if (string.IsNullOrWhiteSpace(invoice.Number))
            throw new ArgumentException("Invoice number is required");

        if (string.IsNullOrWhiteSpace(invoice.CompanyName))
            throw new ArgumentException("Company name is required");

        if (invoice.Items == null || !invoice.Items.Any())
            throw new ArgumentException("Invoice must have at least one item");

        if (invoice.Total <= 0)
            throw new ArgumentException("Invoice total must be positive");
    }
}

public class GenerationResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string InvoiceNumber { get; set; }
    public List<string> Warnings { get; set; } = new List<string>();
    public List<string> LogEntries { get; set; } = new List<string>();
}

// Usage
var generator = new RobustInvoiceGenerator("invoice-template.html", logger);

using (var output = new FileStream("invoice.pdf", FileMode.Create))
{
    var result = generator.Generate(invoice, output);

    if (result.Success)
    {
        Console.WriteLine($"✓ {result.Message}");

        if (result.Warnings.Any())
        {
            Console.WriteLine($"  {result.Warnings.Count} warnings (see logs)");
        }
    }
    else
    {
        Console.WriteLine($"✗ {result.Message}");

        if (result.LogEntries.Any())
        {
            Console.WriteLine("  Diagnostic information:");
            foreach (var entry in result.LogEntries.Take(10))
            {
                Console.WriteLine($"    - {entry}");
            }
        }
    }
}
```

### Example 2: User-Generated Content Handler

```csharp
public class UserContentHandler
{
    public PDFGenerationResult GenerateFromUserContent(
        string userHtml,
        object userData,
        Stream output)
    {
        var result = new PDFGenerationResult();
        var logger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);

        try
        {
            // Sanitize user HTML (basic example - use proper sanitization library)
            var sanitizedHtml = SanitizeHtml(userHtml);

            // Create document from string
            var doc = Document.ParseDocument(new StringReader(sanitizedHtml));

            // ALWAYS use Lax mode for user content
            doc.ConformanceMode = ParserConformanceMode.Lax;
            doc.TraceLog = logger;

            // Bind user data
            doc.Params["data"] = userData;

            // Generate
            doc.SaveAsPDF(output);

            // Collect warnings for user feedback
            result.Success = true;
            result.Warnings = logger.Entries
                .Where(e => e.Level == TraceRecordLevel.Warnings)
                .Select(e => new UserFriendlyWarning
                {
                    Message = SimplifyWarningMessage(e.Message),
                    Category = e.Category
                })
                .ToList();

            if (result.Warnings.Any())
            {
                result.Message = $"PDF generated with {result.Warnings.Count} issues";
            }
            else
            {
                result.Message = "PDF generated successfully";
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = "Failed to generate PDF from provided content";
            result.TechnicalDetails = ex.Message;
        }

        return result;
    }

    private string SanitizeHtml(string html)
    {
        // Use HtmlSanitizer or similar library in production
        // This is a simplified example
        return html;
    }

    private string SimplifyWarningMessage(string technicalMessage)
    {
        // Convert technical messages to user-friendly ones
        if (technicalMessage.Contains("font not found"))
            return "Some text may not display with the requested font";

        if (technicalMessage.Contains("image not found"))
            return "Some images could not be loaded";

        if (technicalMessage.Contains("invalid CSS"))
            return "Some styling may not be applied correctly";

        return "Minor issue detected (document still generated)";
    }
}

public class PDFGenerationResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string TechnicalDetails { get; set; }
    public List<UserFriendlyWarning> Warnings { get; set; } = new List<UserFriendlyWarning>();
}

public class UserFriendlyWarning
{
    public string Message { get; set; }
    public string Category { get; set; }
}
```

---

## Try It Yourself

### Exercise 1: Validation Framework

Build a validation framework that:
- Checks template exists
- Validates HTML structure
- Verifies required data fields
- Returns detailed validation results
- Tests with various inputs

### Exercise 2: Error Recovery

Implement error recovery that:
- Catches specific exception types
- Logs detailed error information
- Attempts alternative approaches
- Provides fallback behavior
- Tests with invalid inputs

### Exercise 3: Conformance Comparison

Create a comparison tool that:
- Generates same document in Strict and Lax modes
- Compares results
- Shows differences in error handling
- Documents best practices

---

## Common Pitfalls

### ❌ No Error Handling

```csharp
// Fails silently or crashes
var doc = Document.ParseDocument("template.html");
doc.SaveAsPDF(stream);
```

✅ **Solution:**

```csharp
try
{
    var doc = Document.ParseDocument("template.html");
    doc.SaveAsPDF(stream);
}
catch (PDFException ex)
{
    Logger.LogError($"PDF generation failed: {ex.Message}");
    throw;
}
```

### ❌ Wrong Conformance Mode

```csharp
// Strict mode with user-generated content - will fail often
doc.ConformanceMode = ParserConformanceMode.Strict;
```

✅ **Solution:**

```csharp
// Lax mode for user content
doc.ConformanceMode = ParserConformanceMode.Lax;
```

### ❌ Ignoring Warnings

```csharp
// Warnings ignored - issues not addressed
var logger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
doc.TraceLog = logger;
doc.SaveAsPDF(stream);
// Never check logger.Entries
```

✅ **Solution:**

```csharp
var logger = new PDFCollectorTraceLog(TraceRecordLevel.Warnings);
doc.TraceLog = logger;
doc.SaveAsPDF(stream);

// Check and report warnings
if (logger.Entries.Any(e => e.Level == TraceRecordLevel.Warnings))
{
    ReportWarnings(logger.Entries);
}
```

---

## Error Handling Checklist

- [ ] Try-catch blocks around PDF generation
- [ ] Specific exception handling (PDFException, FileNotFoundException)
- [ ] Logging enabled for diagnostics
- [ ] Appropriate conformance mode chosen
- [ ] Input validation before generation
- [ ] Warnings checked and reported
- [ ] Error messages user-friendly
- [ ] Recovery strategies implemented

---

## Best Practices

1. **Always Use Try-Catch** - Never assume success
2. **Lax Mode for User Content** - Graceful degradation
3. **Strict Mode for Testing** - Catch issues early
4. **Log Everything** - Diagnostic information crucial
5. **Validate Input** - Before attempting generation
6. **Check Warnings** - Even in Lax mode
7. **User-Friendly Messages** - Translate technical errors
8. **Test Error Paths** - Verify error handling works

---

## Next Steps

1. **[PDF Versions](04_pdf_versions.md)** - Version and compliance settings
2. **[Security](05_security.md)** - Protect your documents
3. **[Production & Deployment](07_production_deployment.md)** - Production best practices

---

**Continue learning →** [PDF Versions](04_pdf_versions.md)
