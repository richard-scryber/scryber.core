---
layout: default
title: PDF Versions
nav_order: 4
parent: Document Configuration
parent_url: /learning/07-configuration/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# PDF Versions

Learn about PDF versions, PDF/A archival compliance, PDF/X print compliance, and how to choose the right version for your documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Understand PDF version differences
- Set appropriate PDF versions
- Configure PDF/A compliance for archiving
- Configure PDF/X compliance for printing
- Choose the right version for your needs
- Handle version-specific features

---

## PDF Versions

###PDF Version Overview

```csharp
using Scryber.PDF;

// Available PDF versions
public enum PDFVersion
{
    PDF11,  // PDF 1.1
    PDF12,  // PDF 1.2
    PDF13,  // PDF 1.3
    PDF14,  // PDF 1.4 (Acrobat 5)
    PDF15,  // PDF 1.5 (Acrobat 6)
    PDF16,  // PDF 1.6 (Acrobat 7)
    PDF17,  // PDF 1.7 (Acrobat 8-11)
    PDF20   // PDF 2.0 (ISO 32000-2)
}
```

### Setting PDF Version

```csharp
using Scryber.Components;
using Scryber.PDF;

var doc = Document.ParseDocument("template.html");

// Set PDF version
doc.RenderOptions.PDFVersion = PDFVersion.PDF17;  // Most common

// Generate
doc.SaveAsPDF(stream);
```

---

## Version Selection Guide

### PDF 1.4 (Acrobat 5)

```csharp
doc.RenderOptions.PDFVersion = PDFVersion.PDF14;
```

**Features:**
- Basic encryption (40-bit, 128-bit)
- Transparency support
- Tagged PDF (accessibility)
- Form fields
- **Required for PDF/A-1**

**Use when:**
- PDF/A-1 compliance needed
- Maximum compatibility required
- Targeting older systems

### PDF 1.7 (Acrobat 8-11)

```csharp
doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
```

**Features:**
- 256-bit AES encryption
- Rich media (audio, video)
- 3D content
- Enhanced forms
- Better compression
- **Default for most applications**

**Use when:**
- Modern PDF generation (recommended)
- Enhanced security needed
- No legacy compatibility requirements

### PDF 2.0 (ISO 32000-2)

```csharp
doc.RenderOptions.PDFVersion = PDFVersion.PDF20;
```

**Features:**
- Latest standard
- Improved security
- Better accessibility
- Modern features

**Use when:**
- Cutting-edge features needed
- Future-proofing documents
- Viewer support confirmed

---

## PDF/A Compliance

PDF/A is a standard for archival documents, ensuring long-term readability.

### PDF/A-1b (Basic)

```csharp
using Scryber.PDF;

var doc = Document.ParseDocument("template.html");

// PDF/A-1b configuration
doc.RenderOptions.PDFVersion = PDFVersion.PDF14;  // Required
doc.RenderOptions.Conformance = PDFConformance.PDFA1B;

// PDF/A requirements:
// - All fonts must be embedded
// - No encryption allowed
// - No external dependencies
// - Color profiles required

// Generate
doc.SaveAsPDF(stream);
```

**PDF/A-1b Requirements:**
- ✅ All fonts embedded
- ✅ No encryption
- ✅ No external references
- ✅ Device-independent color
- ⚠️ No transparency

### PDF/A-2b

```csharp
doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
doc.RenderOptions.Conformance = PDFConformance.PDFA2B;
```

**Improvements over PDF/A-1:**
- ✅ JPEG2000 compression
- ✅ Transparency support
- ✅ Layers
- ✅ Digital signatures
- ✅ Embedded files (attachments)

### PDF/A-3b

```csharp
doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
doc.RenderOptions.Conformance = PDFConformance.PDFA3B;
```

**Improvements over PDF/A-2:**
- ✅ Embed any file format
- ✅ Structured data (XML, JSON)
- ✅ Invoice formats (ZUGFeRD, Factur-X)

---

## PDF/X Compliance

PDF/X is a standard for printing and prepress.

### PDF/X-1a

```csharp
doc.RenderOptions.PDFVersion = PDFVersion.PDF13;
doc.RenderOptions.Conformance = PDFConformance.PDFX1A;

// PDF/X-1a requirements:
// - CMYK or spot colors only (no RGB)
// - All fonts embedded
// - No encryption
// - Output intent profile required
```

**Use for:**
- Print-ready documents
- Professional printing
- Color-critical work

### PDF/X-3

```csharp
doc.RenderOptions.PDFVersion = PDFVersion.PDF13;
doc.RenderOptions.Conformance = PDFConformance.PDFX3;
```

**Improvements over PDF/X-1a:**
- ✅ Allows RGB and LAB color spaces
- ✅ ICC profiles for color management
- ✅ More flexible color workflow

---

## Practical Examples

### Example 1: Archive-Compliant Report

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.IO;

public class ArchiveReportGenerator
{
    public void GenerateArchiveCompliantReport(ReportData data, Stream output)
    {
        var doc = Document.ParseDocument("report-template.html");

        // Configure for PDF/A-2b compliance
        doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
        doc.RenderOptions.Conformance = PDFConformance.PDFA2B;

        // Metadata (required for PDF/A)
        doc.Info.Title = $"Annual Report {data.Year}";
        doc.Info.Author = data.CompanyName;
        doc.Info.Subject = "Financial Report";
        doc.Info.CreationDate = DateTime.Now;

        // NO ENCRYPTION for PDF/A
        doc.RenderOptions.SecurityOptions = null;

        // Font embedding (required for PDF/A)
        doc.RenderOptions.FontSubsetting = false;  // Embed full fonts

        // Bind data
        doc.Params["report"] = data;

        try
        {
            doc.SaveAsPDF(output);
            Console.WriteLine("PDF/A-2b compliant report generated");
        }
        catch (PDFException ex)
        {
            Console.WriteLine($"PDF/A compliance error: {ex.Message}");
            throw;
        }
    }
}

// Usage
var generator = new ArchiveReportGenerator();
var reportData = new ReportData
{
    Year = 2024,
    CompanyName = "Acme Corporation"
};

using (var output = new FileStream("annual-report.pdf", FileMode.Create))
{
    generator.GenerateArchiveCompliantReport(reportData, output);
}
```

### Example 2: Print-Ready Brochure

```csharp
public class PrintBrochureGenerator
{
    public void GeneratePrintReadyBrochure(BrochureData data, Stream output)
    {
        var doc = Document.ParseDocument("brochure-template.html");

        // Configure for PDF/X-3
        doc.RenderOptions.PDFVersion = PDFVersion.PDF13;
        doc.RenderOptions.Conformance = PDFConformance.PDFX3;

        // Print metadata
        doc.Info.Title = $"{data.ProductName} Brochure";
        doc.Info.Creator = "Marketing Department";

        // NO ENCRYPTION for PDF/X
        doc.RenderOptions.SecurityOptions = null;

        // Embed all fonts (required for PDF/X)
        doc.RenderOptions.FontSubsetting = false;

        // Bind data
        doc.Params["brochure"] = data;

        doc.SaveAsPDF(output);
        Console.WriteLine("PDF/X-3 compliant brochure generated");
    }
}
```

### Example 3: Version-Specific Feature Handling

```csharp
public class VersionAwareGenerator
{
    public void GenerateDocument(DocumentOptions options, Stream output)
    {
        var doc = Document.ParseDocument("template.html");

        // Set version based on requirements
        if (options.RequiresArchiving)
        {
            // PDF/A for archiving
            doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
            doc.RenderOptions.Conformance = PDFConformance.PDFA2B;
            doc.RenderOptions.SecurityOptions = null;  // No encryption in PDF/A
        }
        else if (options.RequiresPrinting)
        {
            // PDF/X for printing
            doc.RenderOptions.PDFVersion = PDFVersion.PDF13;
            doc.RenderOptions.Conformance = PDFConformance.PDFX3;
            doc.RenderOptions.SecurityOptions = null;  // No encryption in PDF/X
        }
        else if (options.RequiresHighSecurity)
        {
            // Modern PDF with encryption
            doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
            doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
            {
                UserPassword = options.UserPassword,
                OwnerPassword = options.OwnerPassword,
                AllowPrinting = options.AllowPrinting,
                AllowCopying = false
            };
        }
        else
        {
            // Standard modern PDF
            doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
        }

        doc.SaveAsPDF(output);
    }
}

public class DocumentOptions
{
    public bool RequiresArchiving { get; set; }
    public bool RequiresPrinting { get; set; }
    public bool RequiresHighSecurity { get; set; }
    public string UserPassword { get; set; }
    public string OwnerPassword { get; set; }
    public bool AllowPrinting { get; set; }
}
```

---

## Feature Compatibility

### Version-Specific Features

| Feature | Min Version | Notes |
|---------|-------------|-------|
| Basic encryption | PDF 1.4 | 128-bit |
| AES encryption | PDF 1.6 | 256-bit in PDF 1.7+ |
| Transparency | PDF 1.4 | Not in PDF/A-1 |
| Layers (OCG) | PDF 1.5 | |
| Rich media | PDF 1.7 | |
| Attachments | PDF 1.4 | Allowed in PDF/A-3 |

### Compliance Restrictions

| Feature | PDF/A | PDF/X | Standard |
|---------|-------|-------|----------|
| Encryption | ❌ | ❌ | ✅ |
| External refs | ❌ | ❌ | ✅ |
| Font embedding | Required | Required | Optional |
| Transparency | PDF/A-2+ | ✅ | ✅ |
| JavaScript | ❌ | ❌ | ✅ |
| Multimedia | ❌ | ❌ | ✅ |

---

## Try It Yourself

### Exercise 1: Compliance Tester

Create a tool that:
- Generates same document in different versions
- Tests PDF/A compliance
- Tests PDF/X compliance
- Compares file sizes
- Documents compatibility issues

### Exercise 2: Version Selector

Build a version selector that:
- Analyzes document requirements
- Recommends appropriate version
- Checks feature compatibility
- Validates configuration

### Exercise 3: Compliance Validator

Implement a validator that:
- Checks if document meets PDF/A requirements
- Verifies font embedding
- Checks for external references
- Reports compliance issues

---

## Common Pitfalls

### ❌ Encryption with PDF/A

```csharp
// PDF/A doesn't allow encryption
doc.RenderOptions.Conformance = PDFConformance.PDFA2B;
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    UserPassword = "secret"  // ❌ Will fail!
};
```

✅ **Solution:**

```csharp
// No encryption for PDF/A
doc.RenderOptions.Conformance = PDFConformance.PDFA2B;
doc.RenderOptions.SecurityOptions = null;  // ✅
```

### ❌ Wrong Version for Compliance

```csharp
// PDF/A-1 requires PDF 1.4
doc.RenderOptions.PDFVersion = PDFVersion.PDF17;  // ❌ Wrong!
doc.RenderOptions.Conformance = PDFConformance.PDFA1B;
```

✅ **Solution:**

```csharp
// Correct version for PDF/A-1
doc.RenderOptions.PDFVersion = PDFVersion.PDF14;  // ✅
doc.RenderOptions.Conformance = PDFConformance.PDFA1B;
```

### ❌ Not Embedding Fonts

```csharp
// PDF/A requires full font embedding
doc.RenderOptions.Conformance = PDFConformance.PDFA2B;
doc.RenderOptions.FontSubsetting = true;  // ❌ Subsetting not ideal
```

✅ **Solution:**

```csharp
// Embed complete fonts for PDF/A
doc.RenderOptions.Conformance = PDFConformance.PDFA2B;
doc.RenderOptions.FontSubsetting = false;  // ✅ Full embedding
```

---

## Version Selection Checklist

- [ ] Requirements analyzed (archival, printing, security)
- [ ] Appropriate version chosen
- [ ] Compliance configured if needed
- [ ] Encryption compatibility checked
- [ ] Font embedding configured
- [ ] External references avoided for compliance
- [ ] Tested with target PDF viewer
- [ ] Validated against compliance standard

---

## Best Practices

1. **Use PDF 1.7 by Default** - Modern and widely supported
2. **PDF/A for Archiving** - Long-term preservation
3. **PDF/X for Printing** - Professional print workflows
4. **No Encryption with Compliance** - PDF/A and PDF/X
5. **Embed All Fonts** - Required for compliance
6. **Test Compliance** - Validate with tools
7. **Document Requirements** - Know your needs upfront
8. **Version Consistency** - Don't mix incompatible features

---

## Compliance Validation

### External Tools

**For PDF/A:**
- VeraPDF (free, open-source validator)
- Adobe Preflight (Acrobat Pro)
- PDF-Tools (various validators)

**For PDF/X:**
- Adobe Preflight (Acrobat Pro)
- Enfocus PitStop
- callas pdfToolbox

### Basic Validation

```csharp
// After generation, validate using external tools
// Example: VeraPDF command-line
// verapdf --format text document.pdf
```

---

## Next Steps

1. **[Security](05_security.md)** - Encryption and permissions
2. **[Optimization & Performance](06_optimization_performance.md)** - File size and speed
3. **[Production & Deployment](07_production_deployment.md)** - Production configuration

---

**Continue learning →** [Security](05_security.md)
