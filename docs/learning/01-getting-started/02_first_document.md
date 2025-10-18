---
layout: default
title: Your First Document
nav_order: 2
parent: Getting Started
parent_url: /learning/01-getting-started/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Your First Document

Create your first PDF document from scratch using HTML and C# in minutes.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Create a basic HTML template for PDFs
- Parse HTML documents with Scryber
- Generate and save PDF files
- Understand XHTML vs HTML5 formats
- Add basic styling to your document

---

## Hello World PDF

Let's create the simplest possible PDF document:

### Step 1: Create the HTML Template

Create a file named `hello.html`:

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Hello World</title>
</head>
<body>
    <h1>Hello, World!</h1>
    <p>This is my first PDF generated with Scryber.Core.</p>
</body>
</html>
```

### Step 2: Generate the PDF

```csharp
using Scryber.Components;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Load the HTML template
        var doc = Document.ParseDocument("hello.html");

        // Generate the PDF
        using (var stream = new FileStream("hello.pdf", FileMode.Create))
        {
            doc.SaveAsPDF(stream);
        }

        Console.WriteLine("PDF created: hello.pdf");
    }
}
```

### Step 3: Run and View

```bash
dotnet run
```

Open `hello.pdf` - you should see your first PDF!

---

## XHTML vs HTML5 Format

Scryber supports two document formats:

### XHTML Format (Recommended)

Uses XML namespace:

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>XHTML Document</title>
</head>
<body>
    <p>Content here</p>
</body>
</html>
```

**Parse with:**
```csharp
var doc = Document.ParseDocument("template.html");
```

**Benefits:**
- Full XML namespace support
- Stricter validation
- Better for complex documents
- Supports Scryber-specific namespaces

### HTML5 Format

Standard HTML5 without namespace:

```html
<!DOCTYPE html>
<html>
<head>
    <title>HTML5 Document</title>
</head>
<body>
    <p>Content here</p>
</body>
</html>
```

**Parse with:**
```csharp
var doc = Document.ParseHTML("template.html");
```

**Benefits:**
- Familiar HTML5 syntax
- No namespace required
- Simpler for basic documents
- Works with existing HTML files

### Which Should You Use?

| Use XHTML When | Use HTML5 When |
|----------------|----------------|
| Building templates from scratch | Converting existing HTML |
| Need strict validation | Prefer simpler syntax |
| Using Scryber-specific elements | Working with HTML5 tools |
| Want explicit namespace control | Don't need custom namespaces |

**Both formats produce identical PDF output!**

---

## Complete First Document Example

Let's create a more complete document with styling:

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>My First PDF Document</title>
    <style>
        body {
            font-family: 'Helvetica', sans-serif;
            font-size: 11pt;
            margin: 40pt;
            color: #333;
        }

        h1 {
            color: #2563eb;
            font-size: 24pt;
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
        }

        h2 {
            color: #1e40af;
            font-size: 18pt;
            margin-top: 20pt;
            margin-bottom: 10pt;
        }

        p {
            line-height: 1.6;
            margin-bottom: 10pt;
        }

        .highlight {
            background-color: #fef3c7;
            padding: 10pt;
            border-left: 4pt solid #f59e0b;
            margin: 15pt 0;
        }

        .footer {
            position: fixed;
            bottom: 20pt;
            left: 40pt;
            right: 40pt;
            text-align: center;
            font-size: 9pt;
            color: #666;
            border-top: 1pt solid #ccc;
            padding-top: 10pt;
        }
    </style>
</head>
<body>
    <h1>Welcome to Scryber.Core</h1>

    <h2>Introduction</h2>
    <p>
        Scryber.Core is a powerful .NET library for generating PDF documents
        using HTML and CSS. This document demonstrates basic features.
    </p>

    <h2>Key Features</h2>
    <ul>
        <li>HTML & CSS templating</li>
        <li>Data binding with Handlebars syntax</li>
        <li>No browser required</li>
        <li>Full programmatic control</li>
        <li>Production-ready performance</li>
    </ul>

    <div class="highlight">
        <strong>Pro Tip:</strong> Use CSS for all styling to keep your
        templates clean and maintainable.
    </div>

    <h2>Getting Started</h2>
    <p>
        Creating PDFs with Scryber is as simple as writing HTML. You already
        have all the skills you need!
    </p>

    <div class="footer">
        Page <page-number /> of <page-count /> | Generated with Scryber.Core
    </div>
</body>
</html>
```
{% endraw %}

**C# Code:**

```csharp
using Scryber.Components;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Parse the document
            var doc = Document.ParseDocument("first-document.html");

            // Set document properties
            doc.Info.Title = "My First PDF Document";
            doc.Info.Author = "Your Name";
            doc.Info.Subject = "Learning Scryber.Core";

            // Generate the PDF
            using (var stream = new FileStream("first-document.pdf", FileMode.Create))
            {
                doc.SaveAsPDF(stream);
            }

            Console.WriteLine("✓ PDF created successfully: first-document.pdf");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }
    }
}
```

---

## Parsing From Different Sources

### From File

```csharp
// XHTML format
var doc = Document.ParseDocument("template.html");

// HTML5 format
var doc = Document.ParseHTML("template.html");
```

### From String

```csharp
string html = @"
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head><title>From String</title></head>
<body><h1>Hello!</h1></body>
</html>";

using (var reader = new StringReader(html))
{
    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
}
```

### From Stream

```csharp
using (var fileStream = File.OpenRead("template.html"))
{
    var doc = Document.ParseDocument(fileStream, ParseSourceType.LocalFile);
}
```

### From URL (Remote)

```csharp
var doc = Document.ParseDocument(
    "https://example.com/template.html",
    ParseSourceType.RemoteFile
);
```

---

## Saving PDFs

### To File

```csharp
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}
```

### To Memory Stream (for HTTP Response)

```csharp
using (var ms = new MemoryStream())
{
    doc.SaveAsPDF(ms);
    byte[] pdfBytes = ms.ToArray();
    return File(pdfBytes, "application/pdf", "document.pdf");
}
```

### To Response Stream (ASP.NET Core)

```csharp
public IActionResult GeneratePdf()
{
    var doc = Document.ParseDocument("template.html");

    return new FileStreamResult(
        doc.SaveAsPDF(),
        "application/pdf"
    )
    {
        FileDownloadName = "document.pdf"
    };
}
```

---

## Document Properties

Set metadata for your PDFs:

```csharp
var doc = Document.ParseDocument("template.html");

// Document information
doc.Info.Title = "Invoice #12345";
doc.Info.Author = "Acme Corporation";
doc.Info.Subject = "Monthly Invoice";
doc.Info.Keywords = "invoice, billing, payment";

// Creator information
doc.Info.Creator = "My Application v1.0";

// Generate with metadata
doc.SaveAsPDF("invoice.pdf");
```

These properties appear in PDF viewers:

![PDF Properties](../../images/pdf-properties.png)

---

## Common Patterns

### 1. Template + Data Pattern

```csharp
public byte[] GeneratePdf(string templatePath, object data)
{
    var doc = Document.ParseDocument(templatePath);

    // Pass data to template
    doc.Params["model"] = data;

    using (var ms = new MemoryStream())
    {
        doc.SaveAsPDF(ms);
        return ms.ToArray();
    }
}
```

### 2. Reusable Document Service

```csharp
public class PdfService
{
    private readonly string _templateDirectory;

    public PdfService(string templateDirectory)
    {
        _templateDirectory = templateDirectory;
    }

    public byte[] GenerateFromTemplate(string templateName, object data)
    {
        string templatePath = Path.Combine(_templateDirectory, templateName);
        var doc = Document.ParseDocument(templatePath);

        doc.Params["model"] = data;

        using (var ms = new MemoryStream())
        {
            doc.SaveAsPDF(ms);
            return ms.ToArray();
        }
    }
}
```

Usage:

```csharp
var pdfService = new PdfService("./Templates");
byte[] pdf = pdfService.GenerateFromTemplate("invoice.html", invoiceData);
```

---

## Try It Yourself

### Exercise 1: Personal Letter

Create a PDF with:
- Your name as the title
- Three sections (Introduction, Skills, Contact)
- Different colors for each section heading
- A footer with page numbers

### Exercise 2: Styled List

Create a PDF with:
- An unordered list of your favorite things
- Custom list markers using CSS
- Background colors alternating between items
- A border around the entire list

### Exercise 3: Parse Methods

Create the same document three ways:
1. From an `.html` file using `ParseDocument`
2. From a string using `StringReader`
3. Using `ParseHTML` instead of `ParseDocument`

Compare the results - they should be identical!

---

## Common Pitfalls

### ❌ Forgetting the Namespace

```html
<!-- This won't work with ParseDocument -->
<html>
<body>Hello</body>
</html>
```

✅ **Solution:** Add the namespace or use ParseHTML

```html
<html xmlns='http://www.w3.org/1999/xhtml'>
<body>Hello</body>
</html>
```

### ❌ Not Disposing Streams

```csharp
var stream = new FileStream("output.pdf", FileMode.Create);
doc.SaveAsPDF(stream);
// Stream not disposed!
```

✅ **Solution:** Use `using` statements

```csharp
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}
```

### ❌ Relative Paths Without Context

```csharp
// May not find the file
var doc = Document.ParseDocument("template.html");
```

✅ **Solution:** Use absolute or properly resolved paths

```csharp
string templatePath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "Templates",
    "template.html"
);
var doc = Document.ParseDocument(templatePath);
```

---

## Next Steps

Now that you can create basic documents:

1. **[HTML to PDF](03_html_to_pdf.md)** - Learn which HTML elements are supported
2. **[CSS Basics](04_css_basics.md)** - Master styling your documents
3. **[Data Binding](/learning/02-data-binding/)** - Make documents dynamic

---

## Additional Resources

- **[HTML Element Reference](/reference/htmltags/)** - All supported HTML elements
- **[Document API](/api/document/)** - Complete API documentation
- **[Code Examples](/examples/)** - More complete examples

---

**Continue learning →** [HTML to PDF](03_html_to_pdf.md)
