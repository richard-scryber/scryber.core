---
layout: default
title: Troubleshooting
nav_order: 8
parent: Getting Started
parent_url: /learning/01-getting-started/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Troubleshooting

Diagnose and fix common issues when generating PDF documents with Scryber.Core.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Diagnose common PDF generation errors
- Use logging to debug issues
- Fix template and parsing problems
- Resolve path and resource loading issues
- Optimize performance

---

## Common Errors and Solutions

### Error: "Could not find file"

**Problem:** Template file not found

```
System.IO.FileNotFoundException: Could not find file 'template.html'
```

**Causes:**
- Incorrect file path
- File doesn't exist
- Wrong working directory

**Solutions:**

```csharp
// ❌ Relative path might not resolve
var doc = Document.ParseDocument("template.html");

// ✅ Use absolute path
string templatePath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "Templates",
    "template.html"
);
var doc = Document.ParseDocument(templatePath);

// ✅ Or use full path
string fullPath = Path.GetFullPath("./Templates/template.html");
var doc = Document.ParseDocument(fullPath);

// ✅ Check if file exists first
if (!File.Exists(templatePath))
{
    throw new FileNotFoundException($"Template not found: {templatePath}");
}
```

### Error: "Root element is missing"

**Problem:** Invalid XML/HTML structure

```
System.Xml.XmlException: Root element is missing
```

**Causes:**
- Missing `<html>` tag
- Unclosed tags
- Invalid XML

**Solutions:**

```html
<!-- ❌ Missing root element -->
<body>
    <p>Content</p>
</body>

<!-- ✅ Complete structure -->
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document</title>
</head>
<body>
    <p>Content</p>
</body>
</html>
```

### Error: "Namespace prefix is not defined"

**Problem:** Using ParseDocument without XML namespace

```
System.Xml.XmlException: The 'html' start tag on line 2 position 2 does not match the end tag of 'body'
```

**Causes:**
- Using `ParseDocument` on HTML5 (no namespace)
- Missing namespace declaration

**Solutions:**

```html
<!-- ❌ ParseDocument requires namespace -->
<html>
<body>Content</body>
</html>

<!-- ✅ Option 1: Add namespace -->
<html xmlns='http://www.w3.org/1999/xhtml'>
<body>Content</body>
</html>

<!-- ✅ Option 2: Use ParseHTML instead -->
```

```csharp
// For HTML without namespace
var doc = Document.ParseHTML("template.html");

// For XHTML with namespace
var doc = Document.ParseDocument("template.html");
```

### Error: "Object reference not set to an instance"

**Problem:** Null reference exception

```
System.NullReferenceException: Object reference not set to an instance of an object
```

**Common causes:**

```csharp
// ❌ Document not initialized
Document doc = null;
doc.SaveAsPDF("output.pdf"); // NullReferenceException

// ❌ Missing data
doc.Params["model"] = null; // May cause null reference in template

// ✅ Check for null
if (doc != null && data != null)
{
    doc.Params["model"] = data;
    doc.SaveAsPDF("output.pdf");
}
```

### Error: "Access to path is denied"

**Problem:** No permission to write file

```
System.UnauthorizedAccessException: Access to the path 'C:\output.pdf' is denied
```

**Solutions:**

```csharp
// ✅ Write to user documents folder
string outputPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
    "output.pdf"
);

// ✅ Ensure directory exists and is writable
string directory = Path.GetDirectoryName(outputPath);
if (!Directory.Exists(directory))
{
    Directory.CreateDirectory(directory);
}

// ✅ Check permissions
var fileInfo = new FileInfo(outputPath);
if (fileInfo.Exists && fileInfo.IsReadOnly)
{
    fileInfo.IsReadOnly = false;
}
```

---

## Logging and Diagnostics

### Enable Logging

```csharp
using Scryber;

// Set log level
TraceLog.SetLogLevel(TraceRecordLevel.Verbose);

// Create custom log handler
var logger = new ConsoleTraceLog(TraceRecordLevel.Verbose);

// Use logger
var doc = Document.ParseDocument("template.html");
doc.Params["model"] = data;

using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream, logger);
}
```

### Custom Log Handler

```csharp
public class CustomTraceLog : TraceLog
{
    private readonly ILogger _logger;

    public CustomTraceLog(ILogger logger) : base(TraceRecordLevel.Verbose)
    {
        _logger = logger;
    }

    protected override void WriteLog(TraceRecord record)
    {
        string message = $"[{record.Level}] {record.Category}: {record.Message}";

        switch (record.Level)
        {
            case TraceRecordLevel.Error:
                _logger.LogError(message);
                break;
            case TraceRecordLevel.Warning:
                _logger.LogWarning(message);
                break;
            case TraceRecordLevel.Message:
                _logger.LogInformation(message);
                break;
            case TraceRecordLevel.Verbose:
                _logger.LogDebug(message);
                break;
        }
    }
}
```

### Log Levels

| Level | When to Use |
|-------|-------------|
| `Error` | Only errors |
| `Warning` | Errors and warnings |
| `Message` | Info, warnings, and errors |
| `Verbose` | Everything (debugging) |

---

## Template Debugging

### Test Template Validity

```csharp
public bool IsValidTemplate(string templatePath, out string error)
{
    error = null;
    try
    {
        var doc = Document.ParseDocument(templatePath);
        return true;
    }
    catch (XmlException ex)
    {
        error = $"XML Error at line {ex.LineNumber}: {ex.Message}";
        return false;
    }
    catch (Exception ex)
    {
        error = $"Error: {ex.Message}";
        return false;
    }
}
```

### Validate Template Structure

```csharp
public List<string> ValidateTemplate(string templatePath)
{
    var issues = new List<string>();

    try
    {
        var doc = Document.ParseDocument(templatePath);

        // Check for head section
        if (string.IsNullOrEmpty(doc.Info.Title))
        {
            issues.Add("Missing document title");
        }

        // Check for body
        if (doc.Pages.Count == 0)
        {
            issues.Add("No content pages generated");
        }

        // Try to generate PDF
        using (var ms = new MemoryStream())
        {
            doc.SaveAsPDF(ms);

            if (ms.Length == 0)
            {
                issues.Add("Generated PDF is empty");
            }
        }
    }
    catch (Exception ex)
    {
        issues.Add($"Template error: {ex.Message}");
    }

    return issues;
}
```

---

## Resource Loading Issues

### Image Not Found

**Problem:** Images don't appear in PDF

```html
<!-- ❌ Relative path might not resolve -->
<img src="./images/logo.png" />
```

**Debug:**

```csharp
// Check working directory
Console.WriteLine($"Working directory: {Directory.GetCurrentDirectory()}");

// Check if image exists
string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "images", "logo.png");
Console.WriteLine($"Image exists: {File.Exists(imagePath)}");
```

**Solutions:**

```html
<!-- ✅ Use absolute path -->
<img src="C:/Projects/MyApp/images/logo.png" />

<!-- ✅ Or use base tag -->
<head>
    <base href="file:///C:/Projects/MyApp/" />
</head>
<body>
    <img src="images/logo.png" />
</body>

<!-- ✅ Or embed as base64 -->
<img src="data:image/png;base64,iVBORw0KG..." />
```

### External Stylesheet Not Loading

**Problem:** External CSS not applied

```html
<!-- ❌ Path issues -->
<link rel="stylesheet" href="styles.css" />
```

**Solutions:**

```html
<!-- ✅ Use full path -->
<link rel="stylesheet" href="file:///C:/Projects/MyApp/styles/common.css" />

<!-- ✅ Or use base tag -->
<head>
    <base href="file:///C:/Projects/MyApp/" />
    <link rel="stylesheet" href="styles/common.css" />
</head>

<!-- ✅ Or embed styles -->
<head>
    <style>
        /* Styles here */
    </style>
</head>
```

### Remote Resource Timeout

**Problem:** Remote images/fonts timeout

```html
<img src="https://example.com/slow-loading-image.jpg" />
```

**Solutions:**

```csharp
// Configure timeout
var doc = Document.ParseDocument("template.html");
doc.RemoteRequests.Timeout = TimeSpan.FromSeconds(30);
doc.SaveAsPDF("output.pdf");

// Or cache remote resources locally
string localImage = DownloadAndCache("https://example.com/image.jpg");
// Use local path in template
```

---

## Performance Issues

### Slow PDF Generation

**Problem:** PDF takes too long to generate

**Diagnose:**

```csharp
var stopwatch = Stopwatch.StartNew();

// Parse
var parseStart = stopwatch.Elapsed;
var doc = Document.ParseDocument("template.html");
Console.WriteLine($"Parse: {stopwatch.Elapsed - parseStart}");

// Bind data
var bindStart = stopwatch.Elapsed;
doc.Params["model"] = GetData();
doc.DataBind(null);
Console.WriteLine($"Data bind: {stopwatch.Elapsed - bindStart}");

// Render
var renderStart = stopwatch.Elapsed;
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}
Console.WriteLine($"Render: {stopwatch.Elapsed - renderStart}");

stopwatch.Stop();
Console.WriteLine($"Total: {stopwatch.Elapsed}");
```

**Solutions:**

1. **Cache templates:**

```csharp
private static readonly Dictionary<string, Document> _templateCache =
    new Dictionary<string, Document>();

public Document GetTemplate(string templatePath)
{
    if (!_templateCache.ContainsKey(templatePath))
    {
        _templateCache[templatePath] = Document.ParseDocument(templatePath);
    }

    return _templateCache[templatePath];
}
```

2. **Optimize images:**

```html
<!-- ❌ Large unoptimized images -->
<img src="photo.jpg" />  <!-- 5MB file -->

<!-- ✅ Optimized and sized -->
<img src="photo-optimized.jpg" style="width: 300pt;" />  <!-- 200KB -->
```

3. **Simplify CSS:**

```css
/* ❌ Complex selectors */
body div.content div.section div.subsection p.text span.highlight {
    color: red;
}

/* ✅ Simple selectors */
.highlight {
    color: red;
}
```

4. **Specify table widths:**

```html
<!-- ❌ Auto-calculate widths (slower) -->
<table>
    <tr>
        <td>Content</td>
        <td>More content</td>
    </tr>
</table>

<!-- ✅ Explicit widths (faster) -->
<table>
    <colgroup>
        <col style="width: 50%;" />
        <col style="width: 50%;" />
    </colgroup>
    <tr>
        <td>Content</td>
        <td>More content</td>
    </tr>
</table>
```

### Memory Issues

**Problem:** Out of memory errors

**Solutions:**

```csharp
// ✅ Dispose properly
using (var doc = Document.ParseDocument("template.html"))
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}

// ✅ Process in batches
foreach (var batch in items.Chunk(100))
{
    ProcessBatch(batch);
    GC.Collect(); // Force garbage collection between batches
}

// ✅ Stream large images
// Instead of loading entire image into memory,
// stream from disk or URL
```

---

## Data Binding Issues

### Expression Not Evaluating

**Problem:** Data binding expressions show as literal text

{% raw %}
```html
<!-- Shows: {{model.name}} instead of actual name -->
<p>{{model.name}}</p>
```
{% endraw %}

**Causes:**
- Data not passed to document
- Wrong property name
- Null data

**Debug:**

```csharp
// Check if data is passed
if (doc.Params["model"] == null)
{
    Console.WriteLine("Error: No model data passed");
}

// Check property names
var data = doc.Params["model"];
var properties = data.GetType().GetProperties();
foreach (var prop in properties)
{
    Console.WriteLine($"Property: {prop.Name} = {prop.GetValue(data)}");
}
```

**Solutions:**

```csharp
// ✅ Ensure data is passed
var model = new
{
    name = "John Doe",
    email = "john@example.com"
};
doc.Params["model"] = model;

// ✅ Verify property names match
// Template uses: {{model.name}}
// Data must have: .name property
```

---

## Testing Strategies

### Unit Test Template Generation

```csharp
[Test]
public void TestInvoiceGeneration()
{
    // Arrange
    var testData = new Invoice
    {
        Number = "INV-001",
        CustomerName = "Test Customer",
        Items = new List<InvoiceItem>
        {
            new InvoiceItem { Description = "Item 1", Amount = 100.00m }
        },
        Total = 100.00m
    };

    // Act
    var doc = Document.ParseDocument("./Templates/invoice.html");
    doc.Params["model"] = testData;

    byte[] pdfBytes;
    using (var ms = new MemoryStream())
    {
        doc.SaveAsPDF(ms);
        pdfBytes = ms.ToArray();
    }

    // Assert
    Assert.IsNotNull(pdfBytes);
    Assert.Greater(pdfBytes.Length, 0);
    Assert.That(pdfBytes[0], Is.EqualTo(0x25)); // PDF signature '%'
}
```

### Integration Test with File Output

```csharp
[Test]
public void TestPdfFileGeneration()
{
    // Arrange
    string outputPath = Path.Combine(Path.GetTempPath(), "test.pdf");

    try
    {
        // Act
        var doc = Document.ParseDocument("template.html");
        doc.SaveAsPDF(outputPath);

        // Assert
        Assert.That(File.Exists(outputPath));
        var fileInfo = new FileInfo(outputPath);
        Assert.Greater(fileInfo.Length, 0);

        // Verify PDF signature
        byte[] header = new byte[4];
        using (var fs = File.OpenRead(outputPath))
        {
            fs.Read(header, 0, 4);
        }
        Assert.That(header, Is.EqualTo(new byte[] { 0x25, 0x50, 0x44, 0x46 })); // %PDF
    }
    finally
    {
        // Cleanup
        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }
    }
}
```

---

## Getting Help

### Check Documentation

1. **[HTML Element Reference](/reference/htmltags/)** - Supported elements
2. **[CSS Property Reference](/reference/css/)** - Supported CSS
3. **[API Documentation](/api/)** - Complete API reference

### Community Resources

- **[GitHub Issues](https://github.com/richard-scryber/scryber.core/issues)** - Report bugs
- **[GitHub Discussions](https://github.com/richard-scryber/scryber.core/discussions)** - Ask questions
- **[Examples Repository](https://github.com/richard-scryber/scryber.core.examples)** - Sample code

### Reporting Issues

When reporting an issue, include:

1. **Scryber version:**
   ```csharp
   Console.WriteLine(typeof(Document).Assembly.GetName().Version);
   ```

2. **Minimal reproducible example:**
   - Simplest template that demonstrates the issue
   - Minimal C# code
   - Sample data if applicable

3. **Error message and stack trace:**
   ```csharp
   try
   {
       // Your code
   }
   catch (Exception ex)
   {
       Console.WriteLine($"Error: {ex.Message}");
       Console.WriteLine($"Stack: {ex.StackTrace}");
   }
   ```

4. **Environment:**
   - .NET version
   - Operating system
   - Hosting environment (console app, ASP.NET, Azure, etc.)

---

## Quick Troubleshooting Checklist

When encountering issues, check:

- [ ] File paths are correct and files exist
- [ ] Template has valid HTML/XHTML structure
- [ ] Using correct parsing method (ParseDocument vs ParseHTML)
- [ ] Data is passed to document via `Params`
- [ ] Property names in template match data model
- [ ] Images and resources are accessible
- [ ] Streams are disposed properly
- [ ] Sufficient permissions to write files
- [ ] Error handling is in place
- [ ] Logging is enabled for debugging

---

## Next Steps

Now that you can troubleshoot issues:

1. **[Data Binding](/learning/02-data-binding/)** - Make documents dynamic
2. **[Document Configuration](/learning/07-configuration/)** - Logging and error handling
3. **[Practical Applications](/learning/08-practical/)** - Real-world examples

---

**Start building dynamic documents →** [Data Binding & Expressions](/learning/02-data-binding/)
