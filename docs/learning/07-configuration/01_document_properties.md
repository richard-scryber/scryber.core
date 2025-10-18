---
layout: default
title: Document Properties
nav_order: 1
parent: Document Configuration
parent_url: /learning/07-configuration/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Document Properties

Learn how to set document metadata including title, author, subject, keywords, and custom properties for professional, searchable PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Set standard PDF metadata properties
- Configure document information
- Add custom document properties
- Understand PDF metadata standards
- Make documents searchable and discoverable
- Follow metadata best practices

---

## Standard Document Properties

### Basic Metadata

```csharp
using Scryber.Components;
using Scryber.PDF;
using System.IO;

// Load document
var doc = Document.ParseDocument("template.html");

// Set standard properties
doc.Info.Title = "Annual Financial Report";
doc.Info.Author = "John Smith";
doc.Info.Subject = "2024 Financial Results";
doc.Info.Keywords = "annual, report, financial, 2024, Q4";

// Generate PDF
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}
```

### All Standard Properties

```csharp
// Complete metadata
doc.Info.Title = "Product Catalog 2025";
doc.Info.Author = "Acme Corporation";
doc.Info.Subject = "Complete product listing and specifications";
doc.Info.Keywords = "catalog, products, 2025, specifications";
doc.Info.Creator = "Acme Catalog System v2.0";
doc.Info.Producer = "Scryber.Core";
doc.Info.CreationDate = DateTime.Now;
doc.Info.ModifiedDate = DateTime.Now;
```

---

## Property Descriptions

### Title

The document's title, shown in PDF viewer title bars and search results.

```csharp
// ❌ Poor title
doc.Info.Title = "Document";

// ❌ File name as title
doc.Info.Title = "report_2024_q4_final_v3.pdf";

// ✅ Descriptive title
doc.Info.Title = "Q4 2024 Financial Report";
```

**Best Practices:**
- Use descriptive, human-readable text
- Include key identifying information
- Keep under 100 characters
- Avoid file extensions or internal codes

### Author

The person or organization that created the document.

```csharp
// Individual author
doc.Info.Author = "Jane Doe";

// Organization
doc.Info.Author = "Acme Corporation";

// Department
doc.Info.Author = "Marketing Department, Acme Corp";

// Multiple authors
doc.Info.Author = "John Smith, Jane Doe, Bob Johnson";
```

### Subject

A brief description of the document's topic.

```csharp
// Clear, concise subject
doc.Info.Subject = "Quarterly financial performance and analysis";

// Topic description
doc.Info.Subject = "User guide for Model X-2000 Professional";

// Context
doc.Info.Subject = "Contract agreement between Acme Corp and Client XYZ";
```

### Keywords

Searchable terms for document discovery.

```csharp
// Comma-separated keywords
doc.Info.Keywords = "financial, report, quarterly, Q4, 2024, revenue, growth";

// Related terms
doc.Info.Keywords = "manual, user guide, instructions, setup, configuration";

// Product information
doc.Info.Keywords = "Model X-2000, specifications, features, warranty";
```

**Best Practices:**
- Use 5-10 relevant keywords
- Separate with commas
- Include synonyms and related terms
- Use lowercase for consistency

### Creator vs Producer

```csharp
// Creator: Application that created the original document
doc.Info.Creator = "Acme Invoice Generator v3.2";

// Producer: Library that converted to PDF (usually set automatically)
doc.Info.Producer = "Scryber.Core 6.0.1";
```

**Note:** `Producer` is often set automatically by Scryber.

### Dates

```csharp
// Creation date
doc.Info.CreationDate = DateTime.Now;

// Modification date
doc.Info.ModifiedDate = DateTime.Now;

// Specific date
doc.Info.CreationDate = new DateTime(2024, 12, 15, 10, 30, 0);

// UTC time
doc.Info.CreationDate = DateTime.UtcNow;
```

---

## Practical Examples

### Example 1: Invoice Metadata

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.IO;

public class InvoiceGenerator
{
    public void GenerateInvoice(Invoice invoice, Stream output)
    {
        // Load template
        var doc = Document.ParseDocument("invoice-template.html");

        // Set invoice-specific metadata
        doc.Info.Title = $"Invoice #{invoice.Number}";
        doc.Info.Author = invoice.CompanyName;
        doc.Info.Subject = $"Invoice for {invoice.CustomerName}";
        doc.Info.Keywords = $"invoice, {invoice.Number}, {invoice.Year}, " +
                           $"{invoice.CustomerName}, {invoice.CompanyName}";
        doc.Info.Creator = "Acme Invoice System";
        doc.Info.CreationDate = invoice.Date;

        // Bind data
        doc.Params["invoice"] = invoice;

        // Generate
        doc.SaveAsPDF(output);
    }
}

// Usage
var invoice = new Invoice
{
    Number = "INV-2024-1234",
    CompanyName = "Acme Corporation",
    CustomerName = "Client XYZ",
    Year = 2024,
    Date = DateTime.Now
};

using (var output = new FileStream("invoice.pdf", FileMode.Create))
{
    var generator = new InvoiceGenerator();
    generator.GenerateInvoice(invoice, output);
}
```

### Example 2: Report with Dynamic Metadata

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.IO;
using System.Linq;

public class ReportGenerator
{
    public void GenerateReport(ReportData data, Stream output)
    {
        var doc = Document.ParseDocument("report-template.html");

        // Dynamic title based on report type
        doc.Info.Title = $"{data.ReportType} Report - {data.Period}";

        // Multiple authors
        if (data.Authors != null && data.Authors.Any())
        {
            doc.Info.Author = string.Join(", ", data.Authors);
        }

        // Comprehensive subject
        doc.Info.Subject = $"{data.ReportType} report covering {data.Period}. " +
                          $"Includes analysis of {string.Join(", ", data.Topics)}.";

        // Build keyword list
        var keywords = new List<string>
        {
            data.ReportType.ToLower(),
            data.Period.ToLower(),
            data.Year.ToString()
        };
        keywords.AddRange(data.Topics.Select(t => t.ToLower()));
        keywords.AddRange(data.Tags.Select(t => t.ToLower()));

        doc.Info.Keywords = string.Join(", ", keywords.Distinct());

        // System information
        doc.Info.Creator = $"{data.SystemName} v{data.SystemVersion}";
        doc.Info.CreationDate = DateTime.Now;

        // Bind data
        doc.Params["report"] = data;

        // Generate
        doc.SaveAsPDF(output);
    }
}

// Usage
var reportData = new ReportData
{
    ReportType = "Financial Analysis",
    Period = "Q4 2024",
    Year = 2024,
    Authors = new[] { "John Smith", "Jane Doe" },
    Topics = new[] { "Revenue", "Expenses", "Profitability" },
    Tags = new[] { "quarterly", "financial", "analysis" },
    SystemName = "Acme Reporting System",
    SystemVersion = "2.5.0"
};

using (var output = new FileStream("report.pdf", FileMode.Create))
{
    var generator = new ReportGenerator();
    generator.GenerateReport(reportData, output);
}
```

### Example 3: Document Management Integration

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.IO;

public class DocumentManager
{
    public void CreateDocument(DocumentMetadata metadata,
                               string templatePath,
                               Stream output)
    {
        var doc = Document.ParseDocument(templatePath);

        // Standard metadata from management system
        doc.Info.Title = metadata.Title;
        doc.Info.Author = metadata.Author;
        doc.Info.Subject = metadata.Description;
        doc.Info.Keywords = string.Join(", ", metadata.Tags);
        doc.Info.Creator = metadata.ApplicationName;
        doc.Info.CreationDate = metadata.CreatedDate;

        // Custom properties for document management
        // Note: Custom properties depend on your PDF generator's capabilities
        // Some systems use XMP metadata for extensibility

        doc.SaveAsPDF(output);

        // Log to document management system
        LogDocumentCreation(metadata, doc.Info);
    }

    private void LogDocumentCreation(DocumentMetadata metadata, DocumentInfo info)
    {
        Console.WriteLine($"Created: {info.Title}");
        Console.WriteLine($"Author: {info.Author}");
        Console.WriteLine($"Date: {info.CreationDate}");
        Console.WriteLine($"ID: {metadata.DocumentId}");
    }
}

// Document metadata from management system
public class DocumentMetadata
{
    public string DocumentId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string[] Tags { get; set; }
    public string ApplicationName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Department { get; set; }
    public string Category { get; set; }
}

// Usage
var metadata = new DocumentMetadata
{
    DocumentId = "DOC-2024-5678",
    Title = "Product Specifications",
    Author = "Engineering Team",
    Description = "Technical specifications for Model X-2000",
    Tags = new[] { "specifications", "technical", "model-x2000", "engineering" },
    ApplicationName = "Acme Document Manager",
    CreatedDate = DateTime.Now,
    Department = "Engineering",
    Category = "Technical Documentation"
};

using (var output = new FileStream("specs.pdf", FileMode.Create))
{
    var manager = new DocumentManager();
    manager.CreateDocument(metadata, "spec-template.html", output);
}
```

---

## Try It Yourself

### Exercise 1: Dynamic Metadata

Create a document generator that:
- Accepts metadata as parameters
- Sets all standard properties
- Includes creation date
- Builds keyword list from tags
- Tests with different data

### Exercise 2: Metadata Validation

Build a validator that:
- Checks required properties are set
- Ensures title length < 100 characters
- Validates keyword format
- Warns about empty properties
- Returns validation results

### Exercise 3: Metadata Template

Design a metadata template for:
- Your organization's documents
- Consistent author format
- Standard keyword categories
- Creator identification
- Reusable across document types

---

## Common Pitfalls

### ❌ Not Setting Metadata

```csharp
// Generates PDF with no useful metadata
var doc = Document.ParseDocument("template.html");
doc.SaveAsPDF(stream);
```

✅ **Solution:**

```csharp
// Always set at least title and author
var doc = Document.ParseDocument("template.html");
doc.Info.Title = "Annual Report 2024";
doc.Info.Author = "Acme Corporation";
doc.SaveAsPDF(stream);
```

### ❌ Using Technical Values

```csharp
// Not user-friendly
doc.Info.Title = "rpt_fin_2024_q4_final_v3";
doc.Info.Keywords = "rpt,fin,q4,2024";
```

✅ **Solution:**

```csharp
// Human-readable
doc.Info.Title = "Q4 2024 Financial Report";
doc.Info.Keywords = "financial, report, quarterly, Q4, 2024";
```

### ❌ Forgetting Keywords

```csharp
// Not searchable
doc.Info.Title = "Product Catalog";
doc.Info.Author = "Sales Team";
// No keywords set
```

✅ **Solution:**

```csharp
// Searchable and discoverable
doc.Info.Title = "Product Catalog 2025";
doc.Info.Author = "Sales Team";
doc.Info.Keywords = "catalog, products, 2025, pricing, specifications";
```

---

## Metadata Best Practices Checklist

- [ ] Title is descriptive and under 100 characters
- [ ] Author identified (person or organization)
- [ ] Subject provides context
- [ ] Keywords include 5-10 relevant terms
- [ ] Creator identifies generating application
- [ ] Creation date set
- [ ] Keywords are comma-separated
- [ ] No technical codes in user-facing properties
- [ ] Tested in PDF viewer (metadata displays correctly)

---

## Best Practices

1. **Always Set Title** - Most important property for users
2. **Descriptive, Not Technical** - Human-readable values
3. **Include Keywords** - Improves searchability
4. **Use Consistent Formatting** - Organization standards
5. **Set Creation Date** - Document versioning and history
6. **Identify Creator** - Application or system name
7. **Validate Metadata** - Check before generation
8. **Test in Viewers** - Verify metadata displays correctly

---

## Viewing Metadata

### In PDF Viewers

**Adobe Acrobat:**
- File → Properties → Description tab

**Preview (Mac):**
- Tools → Show Inspector → Document info

**PDF viewers show:**
- Title in window/tab
- Author, Subject, Keywords in properties
- Creation and modification dates

---

## Integration Patterns

### Metadata from Database

```csharp
public void GenerateFromDatabase(int documentId, Stream output)
{
    // Retrieve metadata from database
    var metadata = db.Documents
        .Where(d => d.Id == documentId)
        .Select(d => new
        {
            d.Title,
            d.Author,
            d.Description,
            d.Keywords,
            d.CreatedDate
        })
        .FirstOrDefault();

    // Generate document
    var doc = Document.ParseDocument("template.html");
    doc.Info.Title = metadata.Title;
    doc.Info.Author = metadata.Author;
    doc.Info.Subject = metadata.Description;
    doc.Info.Keywords = metadata.Keywords;
    doc.Info.CreationDate = metadata.CreatedDate;
    doc.SaveAsPDF(output);
}
```

### Metadata from User Input

```csharp
[HttpPost]
public IActionResult Generate(DocumentRequest request)
{
    // Validate user input
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return BadRequest("Title is required");
    }

    // Generate with user-provided metadata
    var doc = Document.ParseDocument("template.html");
    doc.Info.Title = request.Title;
    doc.Info.Author = request.Author ?? "Unknown";
    doc.Info.Subject = request.Description;
    doc.Info.Keywords = request.Keywords;

    using (var output = new MemoryStream())
    {
        doc.SaveAsPDF(output);
        return File(output.ToArray(), "application/pdf", $"{request.Title}.pdf");
    }
}
```

---

## Next Steps

1. **[Logging](02_logging.md)** - Configure diagnostic logging
2. **[Error Handling & Conformance](03_error_handling_conformance.md)** - Handle errors gracefully
3. **[Security](05_security.md)** - Protect your documents

---

**Continue learning →** [Logging](02_logging.md)
