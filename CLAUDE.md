# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Scryber.Core is a sophisticated PDF generation engine for .NET that converts HTML/XML templates with CSS styling into PDF documents. It supports .NET 9, 8, 6, and Standard 2.0, with WASM compatibility for Blazor applications.

---

## Using Scryber from NuGet Package

> **For developers using Scryber as a NuGet package** (not working with the source code)

### Installation

```bash
# Core library
dotnet add package Scryber.Core

# For ASP.NET MVC integration
dotnet add package Scryber.Core.Mvc
```

### Quick Start

```csharp
using Scryber.Components;
using System.IO;

// Parse HTML template
using (var doc = Document.ParseDocument("template.html"))
{
    // Add data to template
    doc.Params["model"] = new
    {
        title = "My Report",
        items = new[] { "Item 1", "Item 2" }
    };

    // Generate PDF - SaveAsPDF handles everything!
    using (var stream = new FileStream("output.pdf", FileMode.Create))
    {
        doc.SaveAsPDF(stream);
    }
}
```

**Important**: Don't call `doc.ProcessDocument()` - it doesn't exist. `SaveAsPDF()` handles all processing automatically.

### Supported HTML Elements

Scryber supports 80+ HTML elements. Common elements include:

**Layout & Structure**:
- `<div>`, `<span>`, `<p>`, `<section>`, `<article>`, `<header>`, `<footer>`, `<main>`, `<aside>`, `<nav>`

**Text & Formatting**:
- `<h1>` through `<h6>`, `<strong>`, `<em>`, `<b>`, `<i>`, `<u>`, `<small>`, `<mark>`, `<del>`, `<ins>`, `<sup>`, `<sub>`
- `<blockquote>`, `<pre>`, `<code>`

**Lists**:
- `<ul>`, `<ol>`, `<li>` - Full support for nested lists with CSS counters

**Tables**:
- `<table>`, `<thead>`, `<tbody>`, `<tfoot>`, `<tr>`, `<th>`, `<td>`
- Supports `colspan`, `rowspan`, and complex table layouts

**Images & Media**:
- `<img>` - JPEG, PNG, GIF, TIFF formats
- `<svg>` - Full SVG path support (circles, rectangles, paths, text)
- Data URLs: `<img src="data:image/png;base64,..." />`

**Forms** (visual only, not interactive):
- `<form>`, `<input>`, `<textarea>`, `<select>`, `<button>`, `<label>`, `<fieldset>`, `<legend>`

**Semantic**:
- `<time>`, `<address>`, `<details>`, `<summary>`

**Special**:
- `<br />`, `<hr />`, `<link>` (for CSS), `<style>`, `<template>` (for data binding)

### Supported CSS Properties

Scryber implements most CSS 2.1 properties plus common CSS3 features:

**Box Model**:
- `margin`, `margin-top/right/bottom/left`
- `padding`, `padding-top/right/bottom/left`
- `border`, `border-width`, `border-style`, `border-color`, `border-radius`
- `width`, `height`, `min-width`, `min-height`, `max-width`, `max-height`

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color`, `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`, `word-spacing`
- `white-space`, `text-indent`, `vertical-align`

**Background**:
- `background-color`, `background-image`, `background-position`, `background-repeat`, `background-size`

**Positioning**:
- `position` (relative, absolute), `top`, `right`, `bottom`, `left`
- `display` (block, inline, inline-block, none, table, table-row, table-cell)
- `float`, `clear`

**Layout**:
- `overflow` (hidden, visible, clip)
- `visibility` (visible, hidden)
- `z-index`

**Page**:
- `@page` rules: `size` (A4, Letter, custom), `margin`
- `page-break-before`, `page-break-after`, `page-break-inside`

**Lists**:
- `list-style-type`, `list-style-position`, `list-style-image`
- `counter-reset`, `counter-increment`, `content: counter()`

**Advanced**:
- CSS Variables: `--custom-property` and `var(--custom-property)`
- `calc()` expressions: `width: calc(100% - 20px)`
- Pseudo-elements: `:before`, `:after`

**Units Supported**: `pt`, `px`, `mm`, `cm`, `in`, `em`, `rem`, `%`

### Expression System

Template expressions use handlebars syntax: `{{expression}}`

**Access Data**:
```html
{{model.title}}
{{model.user.firstName}}
{{model.items[0].name}}
```

**Mathematical Operations**:
```html
{{price * 1.2}}
{{width + 10}}
{{total / count}}
<var data-id="result" data-value="{{value / maxValue * 100}}" />
```

**Important**: Use standard operators (`+`, `-`, `*`, `/`), not `calc()` function.

**Template Variables**:
```html
<!-- Define -->
<var data-id="myVar" data-value="{{someCalculation}}" />

<!-- Use directly by name -->
{{myVar}}
{{myVar * 2}}
```

**Built-in Functions**:
- `concat(str1, str2, ...)` - String concatenation
- `if(condition, trueValue, falseValue)` - Conditional
- `format(value, formatString)` - Number/date formatting
  - `{{format(price, 'C2')}}` → $10.50
  - `{{format(count, 'N0')}}` → 1,234
  - `{{format(date, 'yyyy-MM-dd')}}` → 2024-03-15
- `length(array)` - Array/collection length
- `index()` - Current iteration index
- `upper(str)`, `lower(str)` - Case conversion

**Conditionals**:
```html
{{#if condition}}
    <p>Condition is true</p>
{{/if}}

<!-- Inline -->
{{if(age >= 18, 'Adult', 'Minor')}}

<!-- With operators -->
{{#if price > 100}}
    <span class="expensive">{{price}}</span>
{{/if}}
```

**Loops**:
```html
{{#each model.items}}
    <div>{{this.name}}: {{this.value}}</div>
{{/each}}

<!-- Access index -->
{{#each model.items}}
    <li>Item {{index()}}: {{this.name}}</li>
{{/each}}

<!-- Nested loops -->
{{#each model.categories}}
    <h2>{{this.name}}</h2>
    {{#each this.items}}
        <p>{{this.name}}</p>
    {{/each}}
{{/each}}
```

**Context Navigation**:
```html
{{#each model.items}}
    <!-- Current item -->
    {{this.name}}

    <!-- Dot prefix = current item -->
    {{.name}}

    <!-- Parent scope with ../ -->
    {{../model.title}}
{{/each}}
```

### Common Patterns

**1. External CSS for Clean Separation**:
```html
<html>
<head>
    <link rel="stylesheet" href="styles.css" type="text/css" />
</head>
<body>
    <div class="header">{{model.title}}</div>
</body>
</html>
```

**2. JSON Data Binding**:
```csharp
// Scryber handles JSON automatically
string jsonContent = File.ReadAllText("data.json");
var model = JsonSerializer.Deserialize<object>(jsonContent);

doc.Params["model"] = model;  // That's it!
```

**3. Dynamic Tables**:
```html
<table>
    <thead>
        <tr>
            <th>Name</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        {{#each model.products}}
        <tr>
            <td>{{this.name}}</td>
            <td>{{format(this.price, 'C2')}}</td>
        </tr>
        {{/each}}
    </tbody>
</table>
```

**4. Conditional Styling**:
```html
{{#each model.items}}
<div class="{{if(this.isActive, 'active', 'inactive')}}">
    {{this.name}}
</div>
{{/each}}
```

**5. SVG Graphics**:
```html
<svg width="200" height="100">
    {{#each model.data}}
    <rect x="{{@index * 40}}"
          y="{{100 - this.value}}"
          width="35"
          height="{{this.value}}"
          fill="#336699" />
    {{/each}}
</svg>
```

**6. Page Headers/Footers**:
```html
<html>
<head>
    <style>
        @page {
            size: A4;
            margin: 20mm;
        }
    </style>
</head>
<body>
    <header>
        <div>Report: {{model.title}}</div>
    </header>

    <main>
        <!-- Content -->
    </main>

    <footer>
        <div>Page <page-number /></div>
    </footer>
</body>
</html>
```

**7. Multi-File Structure (Template + CSS + Data)**:

**template.html**:
```html
<html>
<head>
    <link rel="stylesheet" href="styles.css" />
</head>
<body>
    {{#each model.sections}}
        <section class="report-section">
            <h2>{{this.title}}</h2>
            <p>{{this.content}}</p>
        </section>
    {{/each}}
</body>
</html>
```

**styles.css**:
```css
@page { size: A4; margin: 20mm; }
body { font-family: Arial; }
.report-section { margin-bottom: 30pt; }
```

**data.json**:
```json
{
  "sections": [
    { "title": "Introduction", "content": "..." },
    { "title": "Analysis", "content": "..." }
  ]
}
```

**generator.cs**:
```csharp
var model = JsonSerializer.Deserialize<object>(File.ReadAllText("data.json"));

using (var doc = Document.ParseDocument("template.html"))
{
    doc.Params["model"] = model;
    using (var stream = new FileStream("report.pdf", FileMode.Create))
    {
        doc.SaveAsPDF(stream);
    }
}
```

### ASP.NET MVC Integration

```csharp
using Scryber.Components.Mvc;

public class ReportController : Controller
{
    public IActionResult DownloadPdf()
    {
        var model = new ReportViewModel
        {
            Title = "Sales Report",
            Data = GetSalesData()
        };

        // Return PDF directly
        return this.PDF("~/Views/Report/Template.html", model);
    }
}
```

### Performance Tips

1. **Reuse Templates**: Parse template once, generate multiple PDFs with different data
2. **External Resources**: Use absolute paths or data URLs for images in production
3. **WASM**: Use `SaveAsPDFAsync()` for Blazor WebAssembly
4. **Large Documents**: Enable compression in PDF writer for smaller file sizes
5. **Fonts**: Standard PDF fonts (Helvetica, Times, Courier) are embedded - no external files needed

### Troubleshooting

**Common Issues**:

1. **"ProcessDocument() not found"** - Don't call it! Use `SaveAsPDF()` directly
2. **Variables not working** - Access by name: `{{varName}}`, not `{{Document.Params.varName}}`
3. **Math not working** - Use standard operators: `{{a + b}}`, not `{{calc(a, '+', b)}}`
4. **Images not loading** - Check file paths are absolute or use data URLs
5. **CSS not applied** - Ensure `<link>` has correct `href` path

### Further Reading

- **[ARCHITECTURE.md](./ARCHITECTURE.md)** - Deep dive into internal architecture, pipeline stages, and component system
- **[Scryber Documentation](https://scrybercore.readthedocs.io/)** - Official documentation with examples
- **[GitHub Samples](https://github.com/richard-scryber/scryber.core.samples)** - Working examples and templates

The ARCHITECTURE.md file provides comprehensive details about:
- Complete PDF generation pipeline (Parse → Init → Load → DataBind → Style → Layout → Render)
- Component model and lifecycle
- CSS parser architecture
- Expression engine internals
- Layout engine details
- Extension points for custom components

---

## Common Commands (For Source Code Development)

### Building Command Line
```bash
# Build entire solution
dotnet build Scryber.Core.sln

# Build specific configuration
dotnet build Scryber.Core.sln -c Release
dotnet build Scryber.Core.sln -c Debug
```

### Testing
```bash
# Run all tests
dotnet test Scryber.Core.sln

# Run specific test project
dotnet test Scryber.UnitTest/Scryber.UnitTests.csproj
dotnet test Scryber.UnitSamples/Scryber.UnitSamples.csproj
dotnet test Scryber.UnitLayouts/Scryber.UnitLayouts.csproj

# Run a single test
dotnet test --filter "FullyQualifiedName~TestMethodName"
```

### Packaging
```bash
# Pack the main library
dotnet pack Scryber.Components/Scryber.Components.csproj -c Release

# Pack MVC extension
dotnet pack Scryber.Components.Mvc/Scryber.Components.Mvc.csproj -c Release
```

## Architecture Overview

Scryber follows a multi-stage pipeline architecture with clear separation of concerns across specialized projects.

### Core Projects and Dependencies

**Dependency Flow**: Common → Drawing/Expressive → Styles/Generation → Imaging → Components → Components.Mvc

1. **Scryber.Common** - Foundation layer defining core interfaces and contracts
   - Lifecycle interfaces: `IComponent`, `IDocument`, `IBindableComponent`, `IRemoteComponent`
   - Low-level PDF structure handling (PDF/Native, PDF/Resources, PDF/Parsing)
   - HTML entity definitions, configuration, caching, and logging abstractions

2. **Scryber.Drawing** - Graphics primitives and typography
   - Font system with embedded standard fonts (Helvetica, Times, Courier, Symbol, ZapfDingbats)
   - TrueType/OpenType support via Scryber.Core.OpenType package
   - Drawing primitives (colors, units, points, rectangles, pen, brush)
   - SVG path and element rendering

3. **Scryber.Expressive** - Expression engine for template expressions
   - Parses and evaluates handlebars syntax `{{...}}`
   - Expression tree: variables, properties, functions, operators, indexers
   - Built-in functions: concat, if, index, and more
   - Used throughout templates and CSS for dynamic content

4. **Scryber.Styles** - CSS parsing and style management
   - CSS parser using individual typed parsers for each property (CSS*Parser classes)
   - Selector matching and specificity calculation
   - Cascading and inheritance rules
   - Supports CSS variables `var(--name)` and `calc()` expressions

5. **Scryber.Generation** - Document parsing and data binding
   - Binding expression infrastructure connecting to Expressive engine
   - Parser definitions for XML/HTML attributes and templates
   - XPath-like data path navigation

6. **Scryber.Imaging** - Image loading and processing
   - Format-specific factories for JPEG, PNG, GIF, TIFF
   - Data URL support for embedded base64 images
   - Uses SixLabors.ImageSharp for image processing
   - Optimized image data conversion for PDF inclusion

7. **Scryber.Components** - Main PDF generation engine
   - Orchestrates all subsystems to produce PDF output
   - 80+ HTML element implementations
   - Full layout engine with box model, flow, positioning, tables, lists
   - Complete PDF generation pipeline (see below)

8. **Scryber.Components.Mvc** - ASP.NET MVC integration
   - `PDFViewResult` ActionResult for PDF responses
   - Extension methods for controllers: `PDFAsync()`

### PDF Generation Pipeline

Documents flow through these discrete stages:

```
Parse → Init → Load → DataBind → Style Resolution → Layout → Render
```

**1. Parsing** (`Document.ParseDocument()` or `Document.ParseHtmlDocument()`)
- XML parser for strict XHTML (System.Xml)
- HTML parser for loose HTML (HtmlAgilityPack)
- Creates component tree from markup

**2. Initialization** (`Init()`)
- Registers components with document
- Sets up resource containers
- Resolves font references

**3. Loading** (`Load()`)
- Loads external resources (images, CSS, fonts)
- Async loading for WASM compatibility
- Processes remote references

**4. Data Binding** (`DataBind()`)
- Evaluates `{{...}}` expressions via Expressive engine
- Populates templates with data
- Supports complex object models

**5. Style Resolution**
- Merges CSS rules from multiple sources
- Applies selector matching and specificity
- Resolves computed styles with cascading

**6. Layout** (`RenderToPDF()` → Layout phase)
- Layout engines: `LayoutEngineDocument`, `LayoutEnginePage`, `LayoutEnginePanel`, `LayoutEngineTable`, `LayoutEngineList`, `LayoutEngineText`
- Measures all components
- Calculates positions and sizes
- Handles page breaks and flowing content
- Text line breaking with hyphenation support
- Creates `PDFLayoutDocument` with `PDFLayoutPage` objects

**7. Rendering** (`OutputToPDF()`)
- Generates PDF structure via `PDFWriter`
- Writes pages, resources (fonts, images), catalog
- Applies compression and security

**All In One** ( 'SaveAsPDF()')
- Does stages 2 to 7 in one go.

## Key Architectural Patterns

### Component Model
- All elements implement `IComponent` with lifecycle methods: `Init()`, `Load()`, `DataBind()`, `Dispose()`
- Components form tree hierarchy with `Document` at root
- Parent/child relationships enable path resolution and resource sharing

### Factory Pattern
- `HTMLParserComponentFactory`: Maps HTML tags to component instances
- `ImageFactoryList`: Creates image handlers based on format
- `FontFactory`: Creates font instances

### Context Threading
- Context objects passed through pipeline stages: `InitContext`, `LoadContext`, `DataContext`, `LayoutContext`, `RenderContext`
- Keeps component state immutable while threading operation context

### Resource Management
- `ISharedResource` interface for fonts and images
- Resources cached at document level, referenced multiple times
- Reduces PDF file size through sharing

## Expression System

Handlebars syntax `{{expression}}` works in:
- HTML attributes: `<div style="{{model.style}}">`
- CSS properties: `color: {{model.color}};`, can also use the `calc(...)` syntax rather than handlebars.
- Text content: `<span>{{model.name}}</span>`

Expression types:
- Variables: `{{model.title}}`
- Property paths: `{{model.user.name}}`
- Array indexing: `{{model.items[0]}}`
- Functions: `{{concat(model.first, ' ', model.last)}}`
- Math: `{{model.price * 1.1}}`
- Conditionals: `{{model.age > 18 ? 'Adult' : 'Minor'}}`

## Template Best Practices

### Document Generation Pattern

**Correct Pattern** - Simple and clean:
```csharp
using (var doc = Document.ParseDocument("template.html"))
{
    doc.Params["model"] = dataModel;

    using (var stream = new FileStream("output.pdf", FileMode.Create))
    {
        doc.SaveAsPDF(stream);  // This handles everything!
    }
}
```

**Important**: Do NOT call `doc.ProcessDocument()` - it doesn't exist and isn't needed. `SaveAsPDF()` handles all processing stages automatically (Init, Load, DataBind, Style Resolution, Layout, Render).

### JSON Data Binding

Scryber handles JSON data binding automatically - no complex conversion needed:

```csharp
// Read and deserialize JSON
string jsonContent = File.ReadAllText("data.json");
var model = JsonSerializer.Deserialize<object>(jsonContent);

// Pass directly to template - Scryber handles all binding
using (var doc = Document.ParseDocument("template.html"))
{
    doc.Params["model"] = model;
    // ... generate PDF
}
```

**What Scryber handles automatically:**
- Object property access: `{{model.propertyName}}`
- Nested object navigation: `{{model.user.address.city}}`
- Array iteration: `{{#each model.items}}`
- Type conversions (strings, numbers, booleans, dates)
- Conditional logic: `{{#if model.condition}}`

### Mathematical Expressions

Use standard mathematical notation in templates:

**Correct:**
```html
<var data-id="total" data-value="{{price * quantity}}" />
<div style="width: {{baseWidth + 10}}pt;">
<rect height="{{value / maxValue * 200}}" />
```

**Incorrect:** ~~`{{calc(price, '*', quantity)}}`~~ - Don't use `calc()` function

Supported operators: `+`, `-`, `*`, `/`, `%`

### Template Variables

Variables stored with `<var>` are accessed directly by name:

```html
<!-- Define variable -->
<var data-id="barHeight" data-value="{{revenue / maxValue * 200}}" />

<!-- Use variable directly by name -->
<rect height="{{barHeight}}" />
<text y="{{240 - barHeight}}">{{format(revenue, 'C0')}}</text>

<!-- Conditional with variable -->
{{#if barHeight > 30}}
    <text>Show label</text>
{{/if}}
```

**Important**: Access variables by name only, NOT `Document.Params.varName` or `params.varName`.

### External CSS Files

Templates can link to external CSS files for clean separation:

```html
<head>
    <link rel="stylesheet" href="styles.css" type="text/css" />
</head>
```

Benefits:
- Separates structure (HTML) from styling (CSS)
- Easier for designers to maintain styles
- Reusable across multiple templates
- Cleaner template code

### Recommended File Structure

For complex reports, use three-file separation:

1. **Template (HTML)**: Structure and layout logic
   ```html
   <html>
       <head>
           <link rel="stylesheet" href="styles.css" />
       </head>
       <body>
           {{#each model.items}}
               <div class="item">{{this.name}}</div>
           {{/each}}
       </body>
   </html>
   ```

2. **Styles (CSS)**: Visual design and branding
   ```css
   @page { size: A4; margin: 20mm; }
   body { font-family: Arial; }
   .item { padding: 10pt; }
   ```

3. **Data (JSON)**: Content and information
   ```json
   {
       "items": [
           { "name": "Item 1" },
           { "name": "Item 2" }
       ]
   }
   ```

This separation enables:
- Content editors work on JSON without touching code
- Designers work on CSS independently
- Developers focus on template logic
- Easy localization by swapping JSON files

## Layout Engine Details

### Box Model
Standard CSS box model: margin → border → padding → content

### Layout Modes
- **Flow Layout**: Block and inline (default HTML behavior)
- **Positioned Layout**: Relative and absolute positioning
- **Table Layout**: Full table support with colspan/rowspan
- **List Layout**: Numbered and bulleted lists with CSS counters

### Text Layout
- Line breaking with hyphenation (`hyphens` CSS property)
- White space handling: `normal`, `nowrap`, `pre`
- Text overflow and clipping
- Multi-font text runs (inline style changes)

## Font Handling

- **Standard Fonts**: PDF standard fonts embedded as resources
- **TrueType/OpenType**: Full support via `Scryber.Core.OpenType` package
- **Google Fonts**: Can load from external URLs
- **Font Fallback**: Chain of fallbacks when exact font not found
- **Note**: Font subsetting not implemented (embeds full fonts)

## Image Handling

- **Formats**: JPEG, PNG, GIF, TIFF
- **Data URLs**: `src="data:image/..."` for embedded images
- **Remote Loading**: Async HTTP requests
- **Color Formats**: RGB24, RGBA32, ARGB32, BGR24
- **Optimization**: JPEG pass-through (no re-encoding)

## WASM Compatibility

All code must be WASM-compatible:
- All remote resource loading is asynchronous
- No blocking I/O operations
- `DocumentTimerExecution` allows yielding during generation
- Use `SaveAsPDFTimer()` for async PDF generation in WASM

## Extension Points

When adding new functionality:

1. **Custom Components**: Implement `IComponent` or extend existing base classes
2. **Custom HTML Elements**: Add to `HTMLParserComponentFactory.DefaultTags` dictionary
3. **Custom CSS Properties**: Create new `CSSStyleAttributeParser<T>` subclass in `Scryber.Styles/Styles/Parsing/Typed/`
4. **Custom Expression Functions**: Register with `Expressive.Context`
5. **Custom Layout Engines**: Implement `IPDFLayoutEngine` interface
6. **Custom Image Formats**: Extend `ImageFactoryBase` and register in `ImageFactoryList`

## Important Files

### Entry Points
- `Scryber.Components/Document.cs` - Main public API for document creation and parsing
- `Scryber.Components/Html/Parsing/HTMLParser.cs` - HTML parsing entry point
- `Scryber.Styles/Styles/Parsing/CSSStyleParser.cs` - CSS parsing entry point

### Layout System
- `Scryber.Components/PDF/Layout/PDFLayoutDocument.cs` - Layout state management
- `Scryber.Components/PDF/Layout/LayoutEngine*.cs` - Layout engine implementations
- `Scryber.Components/PDF/Layout/PDFLayout*.cs` - Layout item hierarchy

### PDF Generation
- `Scryber.Common/PDF/Native/PDFWriter*.cs` - Low-level PDF structure writing
- `Scryber.Components/PDF/Native/PDFWriter*.cs` - High-level PDF writing

### Expression Engine
- `Scryber.Expressions/ExpressionParser.cs` - Expression tokenization and parsing
- `Scryber.Generation/Binding/BindingCalcParser.cs` - Template binding integration

## Multi-Framework Targeting

Projects target multiple frameworks: `net6.0;net8.0;net9.0;netstandard2.0`

When working with framework-specific code:
- Use `#if NET6_0_OR_GREATER` preprocessor directives
- Ensure WASM compatibility (no platform-specific APIs)
- Test across all target frameworks when possible

## Version Information

Current version: 9.1.0.7-beta (as of last update)
- Version defined in `Scryber.Components/Scryber.Components.csproj`
- Also in `Directory.Build.props` for assembly versioning
