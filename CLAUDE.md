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

**Important - Margin Collapsing**: Unlike browsers, Scryber does NOT collapse adjacent top/bottom margins between sibling elements. If two siblings have `margin-bottom: 20pt` and `margin-top: 20pt`, the total space will be 40pt (not 20pt as in browsers). Plan your spacing accordingly and consider using smaller margins or margin on only one side.

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

**Important - Float Behavior in Scryber**:

1. **Element Order**: Elements with `float: right` must appear BEFORE non-floating inline content in HTML source order, otherwise they will wrap to the next line.

❌ **Wrong** (float: right wraps to next line):
```html
<div>
    <span class="label">Text</span>
    <span class="value" style="float: right;">Value</span>
</div>
```

✅ **Correct** (float: right appears first):
```html
<div>
    <span class="value" style="float: right;">Value</span>
    <span class="label">Text</span>
</div>
```

2. **Element Width**: Floating elements should have explicit width to prevent page overflow. If a floated element's width is not constrained, its inner content may use full width and push beyond page boundaries.

❌ **Wrong** (float: right with unconstrained width can overflow):
```html
<div class="header" style="float: right;">
    <h1>Long Title Text</h1>  <!-- Takes full width -->
</div>
```

✅ **Correct** (float: right with explicit width):
```html
<div class="header" style="float: right; width: 300pt;">
    <h1>Long Title Text</h1>  <!-- Constrained to 300pt -->
</div>
```

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

### Expression System & Data Binding

Scryber provides a comprehensive template expression system using Handlebars syntax with 6 helpers, 15 operators, and 90+ functions.

#### Core Expression Syntax

Template expressions use handlebars syntax: `{{expression}}`

**⚠️ IMPORTANT Syntax Rules**:

1. **Use `{{expression}}` NOT `${expression}`**
   - ✅ **Correct**: `{{model.price}}` - Handlebars syntax, will be evaluated
   - ❌ **Wrong**: `${model.price}` - JavaScript template literal syntax, will NOT work
   - ✅ **Dollar signs**: `${{model.price}}` - The `$` is just a character, `{{...}}` is the expression
   - **Why**: HTML templates are parsed, not evaluated as JavaScript

2. **Cannot call C# static methods in templates**
   - ❌ **Wrong**: `{{DateTime.Now}}`, `{{String.Format(...)}}`, `{{Math.PI}}`
   - ✅ **Correct**: Pass data from C# code, then reference in template
   - Templates can only access:
     - Data passed via `doc.Params["key"]`
     - Expression functions (like `format()`, `concat()`, `pi()`)
     - Current context (`this`, `model`, variables)

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

**Important**: Use standard operators (`+`, `-`, `*`, `/`), not `calc()` function in expressions.

**Template Variables**:
```html
<!-- Define -->
<var data-id="myVar" data-value="{{someCalculation}}" />

<!-- Use directly by name -->
{{myVar}}
{{myVar * 2}}
```

**Template Variables Inside Loops**:
```html
<!-- Variables inside loops update automatically each iteration -->
{{#each model.items}}
    <var data-id="itemX" data-value="{{this.value * 10}}" />
    <var data-id="itemY" data-value="{{110 + (@index * 35)}}" />

    <!-- Use the variables - they update each iteration -->
    <rect x="{{itemX}}" y="{{itemY}}" width="50" height="25" />
{{/each}}
```

**Important**: Don't try to create unique variable names with `@index`:

❌ **Wrong** (nested binding doesn't work):
```html
{{#each collection}}
    <var data-id="myVar_{{@index}}" data-value="{{calculation}}" />
    <rect x="{{myVar_{{@index}}}}" />  <!-- Doesn't work! -->
{{/each}}
```

✅ **Correct** (variables update each iteration):
```html
{{#each collection}}
    <var data-id="myVar" data-value="{{calculation}}" />
    <rect x="{{myVar}}" />  <!-- Works! Updates each iteration -->
{{/each}}
```

#### Handlebars Helpers (Block-Level Control Structures)

Scryber implements 6 Handlebars helpers that compile to underlying Scryber XML components:

**1. `{{#each}}` - Iteration Helper**
```html
{{#each model.items}}
    <p>{{this.name}}</p>
{{/each}}
```
- **Compiles to**: `<template data-bind="{{collection}}">`  using ForEach component
- **Special variables**: `@index` (zero-based), `@first` (boolean), `@last` (boolean)
- **Context**: `this` refers to current item, `../` accesses parent scope
```html
{{#each model.products}}
    <p>{{@index}}. {{this.name}} - ${{this.price}}</p>
    {{#if @first}}<hr />{{/if}}
{{/each}}
```

**2. `{{#with}}` - Context Switching**
```html
{{#with model.user}}
    <p>Name: {{this.firstName}} {{this.lastName}}</p>
    <p>Email: {{this.email}}</p>
{{/with}}
```
- **Compiles to**: `<template data-bind="{{object}}">` with context switching
- **Aliasing**: `{{#with object as | alias |}}`
- **Parent access**: Use `../` to access parent scope

**3. `{{#if}}` / `{{else if}}` / `{{else}}` - Conditionals**
```html
{{#if model.score >= 90}}
    <p class="grade-a">Excellent!</p>
{{else if model.score >= 70}}
    <p class="grade-b">Good</p>
{{else}}
    <p class="grade-c">Needs improvement</p>
{{/if}}
```
- **Compiles to**: `<choose><when><otherwise>` element structure
- **Supported operators**: `==`, `!=`, `<`, `<=`, `>`, `>=`, `&&`, `||`
- **Works with**: `#each` and `#with` for fallback cases

**4. `{{log}}` - Debugging Helper**
```html
{{log "Debug message: " model.value}}
{{log "Error occurred" level="error" category="validation"}}
```
- **Levels**: `debug` (Verbose), `info` (Message), `warn` (Warning), `error` (Error)
- **Category**: Optional category for filtering logs
- **Output**: Writes to Scryber trace log during data binding

#### Binding Operators (15 Total)

Operators work in expressions with proper precedence levels:

**Arithmetic Operators** (Precedence 3-5):
- `+` Addition (precedence 5)
- `-` Subtraction (precedence 5)
- `*` Multiplication (precedence 4)
- `/` Division (precedence 4)
- `%` Modulus (precedence 4)
- `^` Power/Exponentiation (precedence 3)

**Comparison Operators** (Precedence 6-7):
- `==` Equality (precedence 7)
- `!=` Inequality (precedence 7)
- `<` Less than (precedence 6)
- `<=` Less than or equal (precedence 6)
- `>` Greater than (precedence 6)
- `>=` Greater than or equal (precedence 6)

**Logical Operators** (Precedence 9-10):
- `&&` Logical AND (precedence 9)
- `||` Logical OR (precedence 10)
- `??` Null coalescing (precedence 8)

**Example with precedence**:
```html
{{model.quantity * model.price + model.tax}}  <!-- * before + -->
{{model.score > 70 && model.attendance >= 0.8}}  <!-- > before && -->
{{model.value ?? model.defaultValue}}  <!-- Use default if null -->
```

#### Expression Functions (90+ Functions)

Functions are organized into 8 categories:

**Conversion Functions** (7 functions):
- `int()`, `long()`, `double()`, `decimal()` - Numeric conversions
- `bool()` - Boolean conversion
- `date()` - Date parsing/conversion
- `typeof()` - Type information

**String Functions** (18 functions):
- `format(value, formatString)` - Format numbers/dates (also aliased as `string()`)
- `concat(str1, str2, ...)` - Concatenate strings
- `join(array, delimiter)` - Join array with delimiter
- `substring(str, start, length)` - Extract substring
- `replace(str, find, replaceWith)` - Replace text
- `toLower(str)`, `toUpper(str)` - Case conversion
- `trim(str)`, `trimEnd(str)` - Remove whitespace
- `length(str)` - String length
- `contains(str, search)`, `startsWith(str, prefix)`, `endsWith(str, suffix)` - String testing
- `indexOf(str, search)` - Find position
- `padLeft(str, length, char)`, `padRight(str, length, char)` - Padding
- `split(str, delimiter)` - Split into array
- `regexIsMatch(str, pattern)`, `regexMatches(str, pattern)`, `regexReplace(str, pattern, replacement)` - Regex operations

**Mathematical Functions** (21 functions):
- `abs()`, `ceiling()`, `floor()`, `round()`, `truncate()` - Rounding
- `sqrt()`, `pow()`, `exp()`, `log()`, `log10()` - Power and logarithms
- `sign()` - Sign determination
- `sin()`, `cos()`, `tan()`, `asin()`, `acos()`, `atan()` - Trigonometry
- `degrees()`, `radians()` - Angle conversion
- `pi()`, `e()` - Constants
- `random()` - Random number generation

**Date/Time Functions** (19 functions):
- **Add functions** (6): `addDays()`, `addMonths()`, `addYears()`, `addHours()`, `addMinutes()`, `addSeconds()`, `addMilliseconds()`
- **Between functions** (4): `daysBetween()`, `hoursBetween()`, `minutesBetween()`, `secondsBetween()`
- **Extract functions** (9): `yearOf()`, `monthOfYear()`, `dayOfMonth()`, `dayOfWeek()`, `dayOfYear()`, `hourOf()`, `minuteOf()`, `secondOf()`, `millisecondOf()`

**Logical Functions** (3 functions):
- `if(condition, trueValue, falseValue)` - Inline conditional (ternary)
- `ifError(expression, fallbackValue)` - Error handling with fallback
- `in(value, collection)` - Membership test

**Collection Functions** (13 functions):
- `count(collection)` - Count items
- `countOf(collection, .property, value)` - Conditional count
- `sum(collection)` - Sum values
- `sumOf(collection, .property)` - Sum property values
- `min(collection)`, `max(collection)` - Find extremes
- `minOf(collection, .property)`, `maxOf(collection, .property)` - Property extremes
- `collect(collection, .property)` - Extract property array
- `selectWhere(collection, .property, value)` - Filter collection
- `firstWhere(collection, .property, value)` - Find first match
- `sortBy(collection, .property)` - Sort ascending
- `reverse(collection)` - Reverse order

**Important**: Property parameters use dot notation (`.property`), not strings (`'property'`)

**Statistical Functions** (5 functions):
- `average(collection)`, `averageOf(collection, .property)` - Arithmetic mean
- `mean(collection)` - Mathematical mean (synonym for average)
- `median(collection)` - Middle value (robust against outliers)
- `mode(collection)` - Most frequent value

**CSS Functions** (2 functions):
- `calc(expression)` - Generate CSS calc() expression for dynamic styles
- `var(variableName, fallbackValue)` - Reference CSS custom properties

#### Common Expression Patterns

**Inline Conditionals**:
```html
{{if(age >= 18, 'Adult', 'Minor')}}
{{if(stock > 0, concat('$', string(price)), 'Out of Stock')}}
```

**Collection Operations**:
```html
<p>Total: ${{sumOf(model.items, .price)}}</p>
<p>Average: ${{round(averageOf(model.items, .price), 2)}}</p>
<p>Items: {{count(model.items)}}</p>
```

**Date Formatting**:
```html
<p>Date: {{format(model.orderDate, 'MMMM dd, yyyy')}}</p>
<p>Time: {{format(model.timestamp, 'h:mm tt')}}</p>
<p>Days until delivery: {{daysBetween(model.today, model.deliveryDate)}}</p>
```

**String Manipulation**:
```html
<p>{{toUpper(model.code)}}</p>
<p>{{concat(model.firstName, ' ', model.lastName)}}</p>
<p>{{join(model.tags, ', ')}}</p>
```

**Statistical Analysis**:
```html
<p>Mean: {{round(mean(model.scores), 1)}}</p>
<p>Median: {{median(model.scores)}}</p>
<p>Range: {{min(model.scores)}} - {{max(model.scores)}}</p>
```

#### Context Navigation

```html
{{#each model.items}}
    <!-- Current item -->
    {{this.name}}

    <!-- Dot prefix = current item -->
    {{.name}}

    <!-- Root parameters (no ../ needed) -->
    {{model.title}}

    <!-- Parent scope with ../ (for nested loops) -->
    {{#each this.children}}
        {{../this.name}}  <!-- Parent loop item -->
        {{model.title}}   <!-- Root parameter - no ../ needed -->
    {{/each}}

    <!-- Special iteration variables -->
    {{@index}}  <!-- Zero-based index -->
    {{@first}}  <!-- true if first item -->
    {{@last}}   <!-- true if last item -->
{{/each}}
```

**Important**: Root parameters (like `model`) are always accessible directly - you don't need `../` to access them:
- ✅ Correct: `{{model.propertyName}}`
- ❌ Wrong: `{{../model.propertyName}}`

The parent selector `../` is only needed when navigating between nested loops, not for accessing root parameters.

#### Critical Best Practice: Static Data for PDFs

**IMPORTANT**: Templates cannot call static C# methods like `DateTime.Now`:

❌ **Wrong** - Static methods don't work in templates:
```html
<!-- This will NOT work - templates can't call C# static methods -->
<p>Generated: {{DateTime.Now}}</p>
<p>Date: {{DateTime.Today}}</p>
```

✅ **Correct** - Pass data from C# code:
```csharp
// C# code - DateTime.Now is fine here
doc.Params["model"] = new {
    generatedDate = DateTime.Now,  // OK in C# code
    reportDate = DateTime.Today     // OK in C# code
};
```

```html
<!-- Template - use the passed data -->
<p>Generated: {{format(model.generatedDate, 'yyyy-MM-dd HH:mm:ss')}}</p>
<p>Date: {{format(model.reportDate, 'MMMM dd, yyyy')}}</p>
```

**For Documentation & Examples**: Use fixed dates for reproducible output:
```csharp
// Use fixed dates in documentation examples
doc.Params["model"] = new {
    reportDate = new DateTime(2024, 3, 15),  // Fixed date
    dueDate = new DateTime(2024, 4, 14)      // Predictable output
};
```

This ensures:
- Templates work (can't call C# static methods)
- PDF output is reproducible (no live timestamps changing on each generation)
- Examples are testable (predictable output)

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
    <var data-id="barX" data-value="{{@index * 40}}" />
    <var data-id="barHeight" data-value="{{this.value}}" />

    <rect x="{{barX}}"
          y="{{100 - barHeight}}"
          width="35"
          height="{{barHeight}}"
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
4. **Expressions not evaluating** - Use `{{expression}}` NOT `${expression}`. Scryber uses Handlebars syntax, not JavaScript template literals
5. **DateTime.Now not working** - Templates can't call static C# methods. Using `{{DateTime.Now}}` won't work. Pass dates from C# code: `doc.Params["date"] = DateTime.Now;` then use `{{date}}` in template
6. **Images not loading** - Check file paths are absolute or use data URLs
7. **CSS not applied** - Ensure `<link>` has correct `href` path

### Debug Trace Logging

Scryber can append a detailed processing trace log to the end of generated PDFs for debugging and performance analysis.

**Method 1: Processing Instruction in Template**
```html
<!DOCTYPE html>
<?scryber append-log='true' ?>
<html>
<!-- your template content -->
</html>
```

**Method 2: Programmatically on Document**
```csharp
using (var doc = Document.ParseDocument("template.html"))
{
    doc.AppendTraceLog = true;  // Enable trace log
    doc.Params["model"] = data;

    using (var stream = new FileStream("output.pdf", FileMode.Create))
    {
        doc.SaveAsPDF(stream);
    }
}
```

The trace log includes:
- Parse time
- Data binding time
- Layout calculation time
- Rendering time
- Component breakdown
- Performance metrics

**Use Cases**:
- Debug template processing issues
- Identify performance bottlenecks
- Verify component initialization
- Understand PDF generation pipeline

**Important**: Remove `appendlog='true'` from production templates as it increases file size and exposes internal processing details.

### Documentation Structure

Scryber documentation is organized into two main sections:

#### `/docs/learning/` - General Information & Tutorials
Contains articles, guides, and learning materials for understanding Scryber concepts and features.

#### `/docs/reference/` - Technical Reference Documentation
Comprehensive reference documentation organized by feature area:

**1. `/reference/htmlelements/` - HTML Elements Reference**
Documentation for supported HTML elements (div, p, table, etc.)

**2. `/reference/htmlattributes/` - HTML Attributes Reference**
Documentation for HTML attributes (class, style, id, data-*, etc.)

**3. `/reference/cssselectors/` - CSS Selectors Reference**
Documentation for supported CSS selectors and specificity rules

**4. `/reference/cssproperties/` - CSS Properties Reference**
Documentation for supported CSS properties (margin, padding, color, font-family, etc.)

**5. `/reference/svgelements/` - SVG Elements Reference**
Documentation for SVG elements (svg, path, rect, circle, etc.)

**6. `/reference/svgattributes/` - SVG Attributes Reference**
Documentation for SVG-specific attributes (viewBox, fill, stroke, etc.)

**7. `/reference/binding/` - Data Binding Reference** (129 files - newly created)
Complete reference for the template expression system:

- **`helpers/`** (6 files) - Handlebars helper documentation
  - each.md, with.md, if.md, else.md, elseif.md, log.md
  - Shows underlying XML compilation for each helper

- **`operators/`** (15 files) - Operator documentation with precedence
  - Arithmetic: addition.md, subtraction.md, multiplication.md, division.md, modulus.md, power.md
  - Comparison: equality.md, inequality.md, lessthan.md, lessorequal.md, greaterthan.md, greaterorequal.md
  - Logical: and.md, or.md, nullcoalesce.md

- **`functions/`** (108 files) - Expression function documentation by category:
  - Conversion (7), String (18), Mathematical (21)
  - Date/Time: Add (6), Between (4), Extract (9)
  - Logical (3), Collection (13), Statistical (5), CSS (2)

**Documentation Features**:
- Each reference file includes signature, parameters, return type, multiple examples with data/output
- Helper files show underlying XML compilation (e.g., `{{#each}}` compiles to `<template>`)
- All examples use fixed dates (never DateTime.Now) for PDF static document compatibility
- Cross-references between related items
- Jekyll front matter for website navigation

**Template Files** (in `/reference/binding/` for consistency):
- `helper_template.md` - Template for new Handlebars helpers
- `operator_template.md` - Template for new operators
- `function_template.md` - Template for new expression functions

### Further Reading

- **[ARCHITECTURE.md](./ARCHITECTURE.md)** - Deep dive into internal architecture, pipeline stages, and component system
- **[Scryber Documentation](https://scrybercore.readthedocs.io/)** - Official documentation with examples
- **[GitHub Samples](https://github.com/richard-scryber/scryber.core.samples)** - Working examples and templates
- **[Learning Documentation](./docs/learning/)** - Tutorials and conceptual articles
- **[Reference Documentation](./docs/reference/)** - Complete technical reference:
  - [HTML Elements](./docs/reference/htmlelements/) - Supported HTML elements
  - [HTML Attributes](./docs/reference/htmlattributes/) - HTML attribute reference
  - [CSS Selectors](./docs/reference/cssselectors/) - CSS selector support and specificity
  - [CSS Properties](./docs/reference/cssproperties/) - Supported CSS properties
  - [SVG Elements](./docs/reference/svgelements/) - SVG element reference
  - [SVG Attributes](./docs/reference/svgattributes/) - SVG attribute reference
  - [Data Binding](./docs/reference/binding/) - Helpers, operators, and functions (129 files)

The ARCHITECTURE.md file provides comprehensive details about:
- Complete PDF generation pipeline (Parse → Init → Load → DataBind → Style → Layout → Render)
- Component model and lifecycle
- CSS parser architecture
- Expression engine internals
- Layout engine details
- Extension points for custom components

---

## Creating Sample Projects and CLI Tools

When creating sample projects that demonstrate Scryber capabilities:

### Project Structure Pattern

Use a consistent directory structure for sample projects:

```
sample-project/
├── templates/          # HTML templates
├── styles/            # CSS stylesheets
├── data/              # Sample JSON data files
├── images/            # Images and logos (SVG recommended)
├── output/            # Generated PDFs (git-ignored)
├── Program.cs         # CLI executable
├── ProjectName.csproj # Project file
└── README.md          # Documentation
```

### .csproj Configuration for Samples

Sample projects should:
1. **Multi-target frameworks** for broad compatibility: `net6.0;net8.0;net9.0`
2. **Copy resource files** to output directory automatically
3. **Reference NuGet packages** (not project references) to simulate real-world usage

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFrameworks>net9.0;net8.0;net6.0</TargetFrameworks>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <!-- Use NuGet package, not project reference -->
    <PackageReference Include="Scryber.Core" Version="9.1.1-rc.4" />
    <PackageReference Include="Scryber.Core.OpenType" Version="5.0.3" />
    <PackageReference Include="System.Text.Json" Version="9.0.0" />
  </ItemGroup>

  <!-- Copy resource files to output directory -->
  <ItemGroup>
    <None Update="templates/**/*.*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="styles/**/*.*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="data/**/*.*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="images/**/*.*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
</Project>
```

**Important Notes**:
- **Scryber.Core.OpenType dependency**: When using Scryber.Core from NuGet, you must also add `Scryber.Core.OpenType` package reference explicitly. This is required for font rendering.
- **RC version format**: Use correct version format for pre-release packages: `9.1.1-rc.4` (with hyphen and dot), not `9.1.1.0-rc4`
- **System.Text.Json version**: Use latest stable version to avoid security vulnerabilities in older versions

### CLI Tool Pattern

Sample CLI tools should follow this pattern:

```csharp
using System;
using System.IO;
using System.Text.Json;
using Scryber.Components;

namespace Scryber.Samples.ProjectName
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Parse arguments
                string dataFile = args.Length > 0 ? args[0] : "data/default.json";
                string outputFile = args.Length > 1 ? args[1] : "output/report.pdf";
                string templateFile = "templates/template.html";

                // Create output directory if needed
                string? outputDir = System.IO.Path.GetDirectoryName(outputFile);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Validate input files exist
                if (!File.Exists(dataFile))
                {
                    Console.WriteLine($"ERROR: Data file not found: {dataFile}");
                    return;
                }

                // Load JSON data
                Console.WriteLine($"Loading data from: {dataFile}");
                string jsonContent = File.ReadAllText(dataFile);
                var model = JsonSerializer.Deserialize<object>(jsonContent);

                // Parse template and generate PDF
                Console.WriteLine($"Parsing template: {templateFile}");
                using (var doc = Document.ParseDocument(templateFile))
                {
                    doc.Params["model"] = model;

                    Console.WriteLine($"Generating PDF: {outputFile}");
                    using (var stream = new FileStream(outputFile, FileMode.Create))
                    {
                        doc.SaveAsPDF(stream);
                    }
                }

                Console.WriteLine("✓ PDF generated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
```

**Important**: Use `System.IO.Path` fully qualified to avoid ambiguity with `Scryber.Components.Path` class.

### Running Multi-Targeted Sample Projects

When running a project that targets multiple frameworks, specify the framework explicitly:

```bash
# Specify framework explicitly
dotnet run --framework net9.0

# Or build for specific framework
dotnet build --framework net8.0
dotnet run --framework net8.0 --no-build
```

### Template Design for Samples

Sample templates should showcase Scryber capabilities:

**1. Use CSS Variables for Theming**:
```css
:root {
  /* Theme: Corporate (Blue) */
  --color-primary: #2563EB;
  --color-success: #22C55E;
  --color-warning: #EAB308;

  /* Typography */
  --font-family: 'Helvetica', 'Arial', sans-serif;
  --font-size-base: 10pt;
}
```

**2. Demonstrate Expression Capabilities**:
```html
<!-- Mathematical calculations -->
<span>{{model.budget.spent / model.budget.total * 100}}%</span>

<!-- Date formatting -->
<span>{{format(model.reportDate, 'MMMM dd, yyyy')}}</span>

<!-- Collection operations -->
<span>Average: {{averageOf(model.items, .value)}}</span>
<span>Total: {{count(model.items)}} items</span>
```

**3. Use Template Variables for Reusable Calculations**:
```html
<!-- Calculate once, use multiple times -->
<var data-id="percentage" data-value="{{value / total * 100}}" />
<rect width="{{percentage * 5}}" height="40" />
<text>{{format(percentage, 'N1')}}%</text>
```

**4. Show Conditional Rendering**:
```html
{{#each model.items}}
  {{#if this.value >= 100}}
    <div class="high-value">{{this.name}}</div>
  {{else if this.value >= 50}}
    <div class="medium-value">{{this.name}}</div>
  {{else}}
    <div class="low-value">{{this.name}}</div>
  {{/if}}
{{/each}}
```

**5. Include SVG Charts with Dynamic Data**:
```html
<svg width="600" height="200" viewBox="0 0 600 200">
  {{#each model.dataPoints}}
    <!-- Variables update each iteration - no @index suffix needed -->
    <var data-id="barX" data-value="{{@index * 50}}" />
    <var data-id="barHeight" data-value="{{this.value / model.maxValue * 180}}" />

    <rect x="{{barX}}"
          y="{{200 - barHeight}}"
          width="40"
          height="{{barHeight}}"
          fill="#2563EB" />
  {{/each}}
</svg>
```

**Note**: Variables inside loops update automatically - don't use `data-id="var_{{@index}}"` patterns. Root parameters like `model` are always accessible directly without `../` prefix.

### HTML/XML Parsing Considerations

**XML Entity Escaping**: When using HTML entities in templates that will be parsed as XML, remember to escape special characters:

❌ **Wrong**:
```html
<h2>Risks & Issues</h2>  <!-- Ampersand will cause parse error -->
```

✅ **Correct**:
```html
<h2>Risks &amp; Issues</h2>  <!-- Properly escaped -->
```

**SVG Attribute Best Practices**: SVG text elements support both numeric and keyword font-weight values:

✅ **Numeric values** (most precise):
```html
<text x="50" y="90" font-weight="700">Label</text>
<!-- font-weight values: 100, 200, 300, 400 (normal), 500, 600, 700 (bold), 800, 900 -->
```

✅ **Keyword values** (also supported):
```html
<text x="50" y="90" font-weight="bold">Label</text>
<text x="50" y="90" font-weight="normal">Label</text>
<text x="50" y="90" font-weight="light">Label</text>
<text x="50" y="90" font-weight="bolder">Label</text>
<text x="50" y="90" font-weight="lighter">Label</text>
```

✅ **CSS classes** (for complex styling):
```html
<!-- In CSS -->
.chart-label-bold {
  font-weight: bold;
  font-size: 14pt;
}

<!-- In SVG -->
<text x="50" y="90" class="chart-label-bold">Label</text>
```

### JSON Data Structure Guidelines

Keep sample data simple and focused:

```json
{
  "reportDate": "2024-03-15T00:00:00",
  "title": "Sample Report",
  "summary": {
    "total": 150000,
    "spent": 75000
  },
  "items": [
    {
      "name": "Item 1",
      "value": 100,
      "status": "Active"
    }
  ]
}
```

**Guidelines**:
- Use ISO 8601 date format with time component: `"2024-03-15T00:00:00"`
- Use simple property names that are self-documenting
- Include variety of data types (strings, numbers, booleans, dates, arrays, nested objects)
- Keep sample data realistic but concise

### README Documentation for Samples

Each sample should include a comprehensive README.md covering:

1. **What the sample demonstrates** - List Scryber features showcased
2. **File structure** - Explain organization
3. **Building and running** - Commands to build and execute
4. **Customization** - How to modify colors, data, layout
5. **JSON data structure** - Document expected data format
6. **Troubleshooting** - Common issues and solutions
7. **Scryber expression examples** - Highlight interesting techniques used

### Common Pitfalls in Sample Projects

**Path Resolution Issues**:
```csharp
// ❌ Wrong - ambiguous reference
using Scryber.Components;  // Contains Path class
var outputDir = Path.GetDirectoryName(file);  // Ambiguous!

// ✅ Correct - fully qualified
var outputDir = System.IO.Path.GetDirectoryName(file);
```

**Missing Output Directory**:
```csharp
// ✅ Always ensure output directory exists
string? outputDir = System.IO.Path.GetDirectoryName(outputFile);
if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
{
    Directory.CreateDirectory(outputDir);
}
```

**Framework Selection**:
```bash
# ❌ Wrong - ambiguous which framework to use
dotnet run

# ✅ Correct - explicit framework
dotnet run --framework net9.0
```

**Float: Right Element Ordering**:
```html
<!-- ❌ Wrong - float: right wraps to next line -->
<div class="header">
    <span class="title">My Title</span>
    <span class="date" style="float: right;">2024-03-15</span>
</div>

<!-- ✅ Correct - float: right appears first -->
<div class="header">
    <span class="date" style="float: right;">2024-03-15</span>
    <span class="title">My Title</span>
</div>
```

**Important**: In Scryber, `float: right` elements must appear BEFORE non-floating inline content in HTML source order to prevent wrapping to a new line.

**Float Width Issues**:
```html
<!-- ❌ Wrong - unconstrained width can cause overflow -->
<div class="title-section" style="float: right;">
    <h1>Very Long Project Status Report Title</h1>
</div>

<!-- ✅ Correct - explicit width prevents overflow -->
<div class="title-section" style="float: right; width: 300pt;">
    <h1>Very Long Project Status Report Title</h1>
</div>
```

**Important**: Floating elements should have explicit width to prevent their inner content from using full width and causing page overflow.

**Margin Collapsing**:
```css
/* ❌ Problem - margins don't collapse (40pt total space) */
.section {
    margin-bottom: 20pt;
}
.section + .section {
    margin-top: 20pt;  /* Adds to margin-bottom, not collapsed */
}

/* ✅ Solution - use margin on one side only */
.section {
    margin-bottom: 12pt;  /* Reduced margin, one side only */
}
```

**Important**: Unlike browsers, Scryber does NOT collapse adjacent top/bottom margins between siblings. Plan spacing with smaller margins or use margin on only one side.

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

## Expression System Architecture

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

### Handlebars Helper Implementation

Handlebars helpers are parsed and transformed into Scryber XML components:

**Helper Mapping System** (`Scryber.Generation/Generation/Handlebars/`):
- `HBarHelperMapping.cs` - Maps helper names to handler classes
- Each helper has dedicated handler: `HBarEach`, `HBarWith`, `HBarIf`, `HBarElse`, `HBarElseIf`, `HBarLog`
- Helpers compile to XML during parsing phase before component tree creation
- Transformation happens via `DocumentHBarExpression.ProcessHandleBars()`

**Compilation Examples**:
```handlebars
{{#each items}}...{{/each}}
```
Becomes:
```xml
<template data-bind="{{items}}">...</template>
```

```handlebars
{{#if condition}}...{{else if other}}...{{else}}...{{/if}}
```
Becomes:
```xml
<choose>
  <when test="{{condition}}">...</when>
  <when test="{{other}}">...</when>
  <otherwise>...</otherwise>
</choose>
```

**Special Variables**:
- `@index`, `@first`, `@last` in `{{#each}}` are handled by ForEach component's internal iteration context
- Context navigation (`this`, `../`) managed by binding context stack during data binding phase

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

**Variables Inside Loops**: Variables defined inside `{{#each}}` loops update automatically with each iteration:

```html
{{#each model.dataPoints}}
    <var data-id="barX" data-value="{{@index * 50}}" />
    <var data-id="barHeight" data-value="{{this.value / model.maxValue * 180}}" />

    <!-- Variables update each iteration - no unique names needed -->
    <rect x="{{barX}}" y="{{200 - barHeight}}" width="40" height="{{barHeight}}" />
{{/each}}
```

❌ **Don't** try to create unique variable names with `@index` - nested binding doesn't work:
```html
<!-- This won't work -->
<var data-id="bar_{{@index}}" data-value="{{calculation}}" />
<rect x="{{bar_{{@index}}}}" />
```

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
4. **Custom Expression Functions**:
   - Create function in `Scryber.Generation/Binding/Functions/` directory
   - Organize by category (e.g., `String/`, `Math/`, `DateTime/`, etc.)
   - Register in `BindingCalcExpressionFactory.cs`
   - Add documentation file in `docs/reference/binding/functions/`
5. **Custom Handlebars Helpers**:
   - Create handler class implementing helper interface in `Scryber.Generation/Generation/Handlebars/`
   - Pattern: `HBarYourHelper.cs` extending appropriate base class
   - Register in `HBarHelperMapping.cs` dictionary
   - Define XML output format (what component structure it compiles to)
   - Add documentation file in `docs/reference/binding/helpers/`
6. **Custom Binding Operators**:
   - Add to `Scryber.Expressive` expression parser
   - Define precedence level (lower = higher priority)
   - Add documentation file in `docs/reference/binding/operators/`
7. **Custom Layout Engines**: Implement `IPDFLayoutEngine` interface
8. **Custom Image Formats**: Extend `ImageFactoryBase` and register in `ImageFactoryList`

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

### Expression Engine & Data Binding
- `Scryber.Expressive/ExpressionParser.cs` - Expression tokenization and parsing
- `Scryber.Generation/Binding/BindingCalcParser.cs` - Template binding integration
- `Scryber.Generation/Binding/BindingCalcExpressionFactory.cs` - Function registration (90+ functions)
- `Scryber.Generation/Binding/Functions/` - All expression function implementations organized by category:
  - `String/` - String manipulation functions
  - `Math/` - Mathematical functions
  - `DateTime/` - Date and time functions
  - `Collection/` - Array/collection operations
  - `Logical/` - Conditional and logical functions
  - `Statistical/` - Statistical analysis functions
  - `Conversion/` - Type conversion functions
  - `CSS/` - CSS helper functions

### Handlebars Helpers
- `Scryber.Generation/Generation/Handlebars/HBarHelperMapping.cs` - Helper name to handler mapping
- `Scryber.Generation/Generation/Handlebars/HBarEach.cs` - `{{#each}}` iteration helper
- `Scryber.Generation/Generation/Handlebars/HBarWith.cs` - `{{#with}}` context switching
- `Scryber.Generation/Generation/Handlebars/HBarIf.cs` - `{{#if}}` conditional
- `Scryber.Generation/Generation/Handlebars/HBarElse.cs` - `{{else}}` fallback
- `Scryber.Generation/Generation/Handlebars/HBarElseIf.cs` - `{{else if}}` alternative condition
- `Scryber.Generation/Generation/Handlebars/HBarLog.cs` - `{{log}}` debugging helper
- `Scryber.Generation/Generation/Handlebars/DocumentHBarExpression.cs` - Handlebars processing orchestration

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
