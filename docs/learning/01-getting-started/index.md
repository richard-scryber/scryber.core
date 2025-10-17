---
layout: default
title: Getting Started
nav_order: 1
parent: Learning Guides
parent_url: /learning/
has_children: true
has_toc: true
---

# Getting Started with Scryber.Core

Welcome to the Scryber.Core learning series! This comprehensive guide will take you from complete beginner to confident PDF document creator.

---

## Table of Contents

1. [Installation & Setup](01_installation_setup.md) - Get Scryber installed and configured
2. [Your First Document](02_first_document.md) - Create a complete PDF from scratch
3. [HTML to PDF](03_html_to_pdf.md) - Understand how HTML translates to PDF
4. [CSS Basics](04_css_basics.md) - Style your documents with CSS
5. [Pages & Sections](05_pages_sections.md) - Structure multi-page documents
6. [Basic Content](06_basic_content.md) - Add text, images, lists, and tables
7. [Output Options](07_output_options.md) - Save, stream, and configure output
8. [Troubleshooting](08_troubleshooting.md) - Solve common issues and debug effectively

---

## What is Scryber.Core?

Scryber.Core is a powerful .NET library that enables you to generate PDF documents using HTML and CSS. Unlike browser-based PDF generation, Scryber is designed specifically for server-side PDF creation, offering:

- **HTML & CSS Templating** - Use familiar web technologies to design PDFs
- **Data Binding** - Dynamic content with Handlebars-style expressions
- **No Browser Required** - Pure .NET solution, runs anywhere .NET runs
- **Full Control** - Programmatic access to all PDF features
- **Production Ready** - Built for high-volume, enterprise applications

## Who Should Use Scryber?

Scryber.Core is ideal for:

- .NET developers who need to generate PDFs programmatically
- Teams who want to use HTML/CSS skills for PDF design
- Applications requiring dynamic, data-driven PDFs
- Systems that need automated PDF generation at scale
- Projects where browser-based rendering isn't suitable

## Common Use Cases

- **Invoices and Receipts** - Generate customer-facing financial documents
- **Reports** - Create data-rich business reports with charts and tables
- **Certificates** - Produce personalized certificates and awards
- **Letters and Contracts** - Automated document generation
- **Catalogs and Brochures** - Marketing materials with product listings
- **Forms** - Generate fillable or print-ready forms

## Your First PDF in 5 Minutes

Here's a simple example to show you how easy it is:

### 1. Install the Package

```bash
dotnet add package Scryber.Core
```

### 2. Create a Simple Template

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>My First PDF</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40pt; }
        h1 { color: #336699; }
    </style>
</head>
<body>
    <h1>Hello, {{model.name}}!</h1>
    <p>Welcome to PDF generation with Scryber.Core.</p>
</body>
</html>
```
{% endraw %}

### 3. Generate the PDF

```csharp
using Scryber.Components;

// Load the template (XHTML format with namespace)
var doc = Document.ParseDocument("template.html");

// Or use ParseHTML for standard HTML5 (without namespace)
// var doc = Document.ParseHTML("template.html");

// Pass data to the template
doc.Params["model"] = new { name = "World" };

// Generate the PDF
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}
```

That's it! You've created your first PDF with dynamic content.

## XHTML vs HTML5 Format

Scryber supports two document formats:

### XHTML Format (Recommended)

Uses XML namespace and strict XHTML syntax:

{% raw %}
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
{% endraw %}

**Parse with:** `Document.ParseDocument()`

**Benefits:**
- Full XML namespace support
- Stricter validation
- Better for complex documents
- Supports Scryber-specific namespaces

### HTML5 Format

Standard HTML5 syntax without namespace:

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

**Parse with:** `Document.ParseHTML()`

**Benefits:**
- Familiar HTML5 syntax
- No namespace required
- Simpler for basic documents
- Works with existing HTML files

### When to Use Each

**Use XHTML (ParseDocument) when:**
- Building templates from scratch
- Need strict validation
- Using Scryber-specific elements
- Want explicit namespace control

**Use HTML5 (ParseHTML) when:**
- Converting existing HTML content
- Prefer simpler syntax
- Working with HTML5 tools
- Don't need custom namespaces

Both formats produce the same PDF output!

## What You'll Learn in This Series

This series covers the essential foundations of Scryber.Core:

1. **[Installation & Setup](01_installation_setup.md)** - Get Scryber installed and configured
2. **[Your First Document](02_first_document.md)** - Create a complete PDF from scratch, including XHTML vs HTML5 formats
3. **[HTML to PDF](03_html_to_pdf.md)** - Understand how HTML translates to PDF
4. **[CSS Basics](04_css_basics.md)** - Style your documents with CSS
5. **[Pages & Sections](05_pages_sections.md)** - Structure multi-page documents
6. **[Basic Content](06_basic_content.md)** - Add text, images, lists, and tables
7. **[Output Options](07_output_options.md)** - Save, stream, and configure output
8. **[Troubleshooting](08_troubleshooting.md)** - Solve common issues and debug effectively

## Prerequisites

Before starting this series, you should have:

- **Basic C# Knowledge** - Understanding of C# syntax and .NET development
- **HTML Fundamentals** - Familiarity with HTML structure and elements
- **CSS Basics** - Understanding of CSS selectors and properties
- **.NET Development Environment** - Visual Studio, VS Code, or Rider

No prior PDF generation experience is required!

## How to Use This Series

Each article in this series is designed to be:

- **Self-Contained** - Read in any order, though sequential is recommended
- **Practical** - Every concept includes working code examples
- **Progressive** - Complexity increases gradually
- **Applied** - "Try it yourself" exercises reinforce learning

### Recommended Learning Path

**For Complete Beginners:**
1. Read articles 1-3 to understand the basics
2. Complete the exercises in article 2
3. Experiment with styling in article 4
4. Build a multi-page document using article 5
5. Complete the series in order

**For Experienced Developers:**
1. Skim articles 1-2 for Scryber-specific concepts
2. Focus on articles 3-4 for HTML/CSS differences
3. Jump to specific topics as needed
4. Use article 8 as a reference for troubleshooting

## Additional Resources

- **[API Reference](/reference/)** - Complete API documentation
- **[HTML Element Reference](/reference/htmltags/)** - Supported HTML elements
- **[CSS Property Reference](/reference/css/)** - Supported CSS properties
- **[Code Examples](/examples/)** - Complete working examples

## Next Steps

Ready to begin? Start with [Installation & Setup](01_installation_setup.md) to get Scryber.Core installed and configured in your project.

If you run into issues, jump to [Troubleshooting](08_troubleshooting.md) for help.

## What's Next After This Series?

Once you've completed Getting Started, continue your learning journey:

- **[Data Binding & Expressions](/learning/02-data-binding/)** - Master dynamic content
- **[Styling & Appearance](/learning/03-styling/)** - Create beautiful documents
- **[Layout & Positioning](/learning/04-layout/)** - Control document structure
- **[Typography & Fonts](/learning/05-typography/)** - Work with fonts and text

---

**Ready to get started?** Head to [Installation & Setup](01_installation_setup.md) →
