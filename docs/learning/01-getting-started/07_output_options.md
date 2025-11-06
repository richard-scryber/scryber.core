---
layout: default
title: Output Options
nav_order: 7
parent: Getting Started
parent_url: /learning/01-getting-started/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Output Options

Master different ways to save, stream, and configure PDF output.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Save PDFs to files and streams
- Return PDFs from web applications
- Configure PDF output settings
- Set document properties and metadata
- Handle output errors gracefully

---

## Saving to File

### Basic File Save

```csharp
var doc = Document.ParseDocument("template.html");

using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}
```

### With Full Path

```csharp
string outputPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
    "output.pdf"
);

using (var stream = new FileStream(outputPath, FileMode.Create))
{
    doc.SaveAsPDF(stream);
}

Console.WriteLine($"PDF saved to: {outputPath}");
```

### Safe File Save with Error Handling

```csharp
public bool SavePdfSafely(Document doc, string outputPath)
{
    try
    {
        // Ensure directory exists
        string directory = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Save PDF
        using (var stream = new FileStream(outputPath, FileMode.Create))
        {
            doc.SaveAsPDF(stream);
        }

        Console.WriteLine($"✓ PDF saved: {outputPath}");
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ Error saving PDF: {ex.Message}");
        return false;
    }
}
```

---

## Streaming Output

### To Memory Stream

```csharp
using (var ms = new MemoryStream())
{
    doc.SaveAsPDF(ms);

    // Get byte array
    byte[] pdfBytes = ms.ToArray();

    // Use bytes (e.g., save, send, upload)
    File.WriteAllBytes("output.pdf", pdfBytes);
}
```

### ASP.NET Core - Return PDF from Controller

```csharp
using Microsoft.AspNetCore.Mvc;
using Scryber.Components;

public class PdfController : Controller
{
    [HttpGet("invoice/{id}")]
    public IActionResult GenerateInvoice(int id)
    {
        // Load template
        var doc = Document.ParseDocument("./Templates/invoice.html");

        // Pass data
        var invoice = GetInvoiceData(id);
        doc.Params["model"] = invoice;

        // Generate PDF to memory
        using (var ms = new MemoryStream())
        {
            doc.SaveAsPDF(ms);

            // Return as file download
            return File(
                ms.ToArray(),
                "application/pdf",
                $"invoice-{id}.pdf"
            );
        }
    }
}
```

### ASP.NET Core - Stream Directly to Response

```csharp
[HttpGet("report")]
public async Task GenerateReport()
{
    var doc = Document.ParseDocument("./Templates/report.html");
    doc.Params["model"] = GetReportData();

    // Set response headers
    Response.ContentType = "application/pdf";
    Response.Headers.Add("Content-Disposition", "inline; filename=report.pdf");

    // Stream directly to response
    await Response.StartAsync();
    doc.SaveAsPDF(Response.Body);
}
```

### ASP.NET Core - Force Download vs Inline Display

```csharp
public IActionResult DownloadPdf()
{
    var pdfBytes = GeneratePdfBytes();

    // Force download
    return File(pdfBytes, "application/pdf", "document.pdf");
}

public IActionResult ViewPdf()
{
    var pdfBytes = GeneratePdfBytes();

    // Display inline in browser
    return File(pdfBytes, "application/pdf");
}
```

---

## ASP.NET Core Integration

### Service Registration

```csharp
// Startup.cs or Program.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    // Register PDF service
    services.AddScoped<IPdfService, PdfService>();
}
```

### PDF Service Pattern

```csharp
public interface IPdfService
{
    byte[] GeneratePdf(string templatePath, object data);
}

public class PdfService : IPdfService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<PdfService> _logger;

    public PdfService(
        IWebHostEnvironment env,
        ILogger<PdfService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public byte[] GeneratePdf(string templatePath, object data)
    {
        try
        {
            // Resolve template path
            string fullPath = Path.Combine(
                _env.ContentRootPath,
                "Templates",
                templatePath
            );

            // Parse document
            var doc = Document.ParseDocument(fullPath);

            // Pass data
            doc.Params["model"] = data;

            // Generate PDF
            using (var ms = new MemoryStream())
            {
                doc.SaveAsPDF(ms);
                return ms.ToArray();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF from {Template}", templatePath);
            throw;
        }
    }
}
```

### Controller Usage

```csharp
public class ReportController : Controller
{
    private readonly IPdfService _pdfService;

    public ReportController(IPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpGet("monthly-report")]
    public IActionResult MonthlyReport()
    {
        var data = GetMonthlyData();

        byte[] pdf = _pdfService.GeneratePdf("monthly-report.html", data);

        return File(pdf, "application/pdf", "monthly-report.pdf");
    }
}
```

---

## Document Properties and Metadata

### Setting Document Information

```csharp
var doc = Document.ParseDocument("template.html");

// Basic metadata
doc.Info.Title = "Monthly Sales Report";
doc.Info.Author = "Sales Department";
doc.Info.Subject = "Q4 2024 Sales Analysis";
doc.Info.Keywords = "sales, report, quarterly, analysis";

// Creator information
doc.Info.Creator = "Sales Reporting System v2.0";

// Generate with metadata
doc.SaveAsPDF("report.pdf");
```

These properties appear in PDF viewers:

![PDF Properties Dialog](../../images/pdf-properties.png)

### Complete Metadata Example

```csharp
public void SetDocumentMetadata(Document doc, string title, string author)
{
    doc.Info.Title = title;
    doc.Info.Author = author;
    doc.Info.Subject = $"Generated on {DateTime.Now:yyyy-MM-dd}";
    doc.Info.Keywords = "scryber, pdf, generated";
    doc.Info.Creator = "My Application";
    doc.Info.Producer = "Scryber.Core";

    // These are set automatically by Scryber:
    // doc.Info.CreationDate
    // doc.Info.ModificationDate
}
```

---

## PDF Version Selection

```csharp
var doc = Document.ParseDocument("template.html");

// Set PDF version
doc.RenderOptions.PDFVersion = PDFVersion.PDF15; // PDF 1.5
// or
doc.RenderOptions.PDFVersion = PDFVersion.PDF17; // PDF 1.7

doc.SaveAsPDF("output.pdf");
```

Available versions:
- `PDF12` - PDF 1.2
- `PDF13` - PDF 1.3
- `PDF14` - PDF 1.4
- `PDF15` - PDF 1.5 (default)
- `PDF16` - PDF 1.6
- `PDF17` - PDF 1.7

---

## Compression Settings

```csharp
var doc = Document.ParseDocument("template.html");

// Configure compression
doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;

// Options:
// - None (no compression)
// - FlateDecode (default, good compression)

doc.SaveAsPDF("output.pdf");
```

---

## Output Configuration Options

### Complete Configuration Example

```csharp
var doc = Document.ParseDocument("template.html");

// Document info
doc.Info.Title = "Sales Report";
doc.Info.Author = "John Doe";

// Render options
doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;

// Conformance mode (Strict vs Lax)
doc.ConformanceMode = ParserConformanceMode.Lax;

// Pass data
doc.Params["model"] = GetData();

// Generate
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}
```

---

## Batch Processing

### Generate Multiple PDFs

```csharp
public void GenerateInvoiceBatch(List<Invoice> invoices, string outputDirectory)
{
    // Ensure directory exists
    Directory.CreateDirectory(outputDirectory);

    foreach (var invoice in invoices)
    {
        try
        {
            // Load template
            var doc = Document.ParseDocument("./Templates/invoice.html");

            // Pass data
            doc.Params["model"] = invoice;

            // Set metadata
            doc.Info.Title = $"Invoice #{invoice.Number}";
            doc.Info.Subject = $"Invoice for {invoice.CustomerName}";

            // Save
            string outputPath = Path.Combine(
                outputDirectory,
                $"invoice-{invoice.Number}.pdf"
            );

            using (var stream = new FileStream(outputPath, FileMode.Create))
            {
                doc.SaveAsPDF(stream);
            }

            Console.WriteLine($"✓ Generated: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Failed invoice {invoice.Number}: {ex.Message}");
        }
    }
}
```

### Parallel Processing

```csharp
using System.Threading.Tasks;
using System.Collections.Concurrent;

public async Task GenerateBatchParallel(List<InvoiceData> invoices)
{
    var options = new ParallelOptions
    {
        MaxDegreeOfParallelism = 4 // Limit concurrent operations
    };

    await Parallel.ForEachAsync(invoices, options, async (invoice, ct) =>
    {
        try
        {
            var doc = Document.ParseDocument("template.html");
            doc.Params["model"] = invoice;

            using (var stream = new FileStream($"invoice-{invoice.Id}.pdf", FileMode.Create))
            {
                doc.SaveAsPDF(stream);
            }

            Console.WriteLine($"✓ Generated invoice {invoice.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Failed invoice {invoice.Id}: {ex.Message}");
        }
    });
}
```

---

## Azure Functions

### HTTP Trigger Function

```csharp
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Scryber.Components;

public class PdfFunction
{
    [Function("GeneratePdf")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        // Parse request body
        var requestData = await req.ReadFromJsonAsync<PdfRequest>();

        // Generate PDF
        var doc = Document.ParseDocument("./Templates/template.html");
        doc.Params["model"] = requestData;

        using (var ms = new MemoryStream())
        {
            doc.SaveAsPDF(ms);

            // Create response
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/pdf");
            response.Headers.Add("Content-Disposition", "attachment; filename=document.pdf");

            await response.Body.WriteAsync(ms.ToArray());
            return response;
        }
    }
}
```

---

## Error Handling Best Practices

### Comprehensive Error Handling

```csharp
public class PdfGenerationResult
{
    public bool Success { get; set; }
    public byte[] PdfData { get; set; }
    public string ErrorMessage { get; set; }
    public Exception Exception { get; set; }
}

public PdfGenerationResult GeneratePdfSafely(string templatePath, object data)
{
    var result = new PdfGenerationResult();

    try
    {
        // Validate inputs
        if (string.IsNullOrEmpty(templatePath))
            throw new ArgumentNullException(nameof(templatePath));

        if (!File.Exists(templatePath))
            throw new FileNotFoundException("Template not found", templatePath);

        // Generate PDF
        var doc = Document.ParseDocument(templatePath);
        doc.Params["model"] = data;

        using (var ms = new MemoryStream())
        {
            doc.SaveAsPDF(ms);
            result.PdfData = ms.ToArray();
            result.Success = true;
        }
    }
    catch (FileNotFoundException ex)
    {
        result.ErrorMessage = $"Template file not found: {ex.FileName}";
        result.Exception = ex;
    }
    catch (PDFException ex)
    {
        result.ErrorMessage = $"PDF generation error: {ex.Message}";
        result.Exception = ex;
    }
    catch (Exception ex)
    {
        result.ErrorMessage = $"Unexpected error: {ex.Message}";
        result.Exception = ex;
    }

    return result;
}
```

### Usage

```csharp
var result = GeneratePdfSafely("invoice.html", invoiceData);

if (result.Success)
{
    File.WriteAllBytes("output.pdf", result.PdfData);
    Console.WriteLine("✓ PDF generated successfully");
}
else
{
    Console.WriteLine($"✗ Error: {result.ErrorMessage}");
    _logger.LogError(result.Exception, "PDF generation failed");
}
```

---

## Try It Yourself

### Exercise 1: File Output Service

Create a service that:
- Accepts template path and data
- Saves to specified output directory
- Returns the full path to saved file
- Logs success/failure

### Exercise 2: Web API Endpoint

Create an ASP.NET Core endpoint that:
- Accepts JSON data in POST request
- Generates PDF from template
- Returns PDF with appropriate headers
- Handles errors gracefully

### Exercise 3: Batch Processor

Create a console application that:
- Reads a CSV file of data
- Generates one PDF per row
- Saves to organized folder structure
- Provides progress feedback

---

## Common Pitfalls

### ❌ Not Disposing Streams

```csharp
var stream = new FileStream("output.pdf", FileMode.Create);
doc.SaveAsPDF(stream);
// Stream not closed!
```

✅ **Solution:** Use `using` statement

```csharp
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}
```

### ❌ Incorrect Content-Type

```csharp
return File(pdfBytes, "application/octet-stream", "doc.pdf");
```

✅ **Solution:** Use correct MIME type

```csharp
return File(pdfBytes, "application/pdf", "doc.pdf");
```

### ❌ Missing Error Handling

```csharp
var doc = Document.ParseDocument(template);
doc.SaveAsPDF("output.pdf");
```

✅ **Solution:** Add try-catch

```csharp
try
{
    var doc = Document.ParseDocument(template);
    doc.SaveAsPDF("output.pdf");
}
catch (Exception ex)
{
    _logger.LogError(ex, "PDF generation failed");
    throw;
}
```

---

## Next Steps

Now that you can handle PDF output:

1. **[Troubleshooting](08_troubleshooting.md)** - Debug common issues
2. **[Document Configuration](/learning/07-configuration/)** - Advanced settings
3. **[Data Binding](/learning/02-data-binding/)** - Dynamic content

---

**Continue learning →** [Troubleshooting](08_troubleshooting.md)
