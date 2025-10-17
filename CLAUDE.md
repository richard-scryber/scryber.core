# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Scryber.Core is a sophisticated PDF generation engine for .NET that converts HTML/XML templates with CSS styling into PDF documents. It supports .NET 9, 8, 6, and Standard 2.0, with WASM compatibility for Blazor applications.

## Common Commands

### Building
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
- CSS properties: `color: {{model.color}};`
- Text content: `<span>{{model.name}}</span>`

Expression types:
- Variables: `{{model.title}}`
- Property paths: `{{model.user.name}}`
- Array indexing: `{{model.items[0]}}`
- Functions: `{{concat(model.first, ' ', model.last)}}`
- Math: `{{model.price * 1.1}}`
- Conditionals: `{{model.age > 18 ? 'Adult' : 'Minor'}}`

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
