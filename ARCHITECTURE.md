# Scryber.Core Architecture Document

## Table of Contents
1. [Overview](#overview)
2. [System Architecture](#system-architecture)
3. [Project Structure](#project-structure)
4. [Component Model](#component-model)
5. [PDF Generation Pipeline](#pdf-generation-pipeline)
6. [Subsystem Deep Dive](#subsystem-deep-dive)
7. [Data Flow](#data-flow)
8. [Design Patterns](#design-patterns)
9. [Extension Architecture](#extension-architecture)
10. [Performance Considerations](#performance-considerations)

## Overview

### Purpose
Scryber.Core is a .NET PDF generation engine that transforms HTML/XML templates with CSS styling into high-quality PDF documents. It bridges web technologies (HTML, CSS, JavaScript-like expressions) with PDF output, enabling developers to create complex documents using familiar web development patterns.

### Key Design Goals
- **Web-First**: Use HTML and CSS as primary authoring format
- **Data Binding**: Support dynamic content through expression evaluation
- **Extensibility**: Allow custom components, styles, and behaviors
- **Multi-Platform**: Support .NET 6, 8, 9, and Standard 2.0
- **WASM Compatible**: Run in Blazor WebAssembly environments
- **Performance**: Efficient layout and rendering for large documents
- **Standards Compliance**: Follow CSS box model and HTML semantics

### Technology Stack
- **.NET Multi-targeting**: net6.0, net8.0, net9.0, netstandard2.0
- **HTML Parsing**: HtmlAgilityPack for loose HTML, System.Xml for strict XHTML
- **Image Processing**: SixLabors.ImageSharp
- **Font Support**: Custom OpenType parser (Scryber.Core.OpenType)
- **Expression Engine**: Custom expression parser and evaluator
- **PDF Generation**: Direct PDF structure writing (no dependencies on external PDF libraries)

## System Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        Public API Layer                          │
│  Document.ParseDocument() / Document.ParseHtmlDocument()         │
│  Document.SaveAsPDF() / Document.SaveAsPDFAsync()                │
└───────────────────────────┬─────────────────────────────────────┘
                            │
┌───────────────────────────┴─────────────────────────────────────┐
│                    Component Tree Layer                          │
│  HTMLDiv, HTMLSpan, Page, Section, Table, Image, etc.           │
└───────────────────────────┬─────────────────────────────────────┘
                            │
        ┌───────────────────┼───────────────────┐
        │                   │                   │
        ▼                   ▼                   ▼
┌──────────────┐    ┌──────────────┐    ┌──────────────┐
│    Styles    │    │   Binding    │    │   Layout     │
│  Subsystem   │    │  Subsystem   │    │  Subsystem   │
│              │    │              │    │              │
│ CSS Parser   │    │  Expression  │    │   Engine     │
│ Selectors    │    │  Evaluator   │    │   Managers   │
│ Cascading    │    │  Data Path   │    │   Measurers  │
└──────┬───────┘    └──────┬───────┘    └──────┬───────┘
       │                   │                   │
       └───────────────────┼───────────────────┘
                           │
                ┌──────────┴───────────┐
                │                      │
                ▼                      ▼
        ┌──────────────┐      ┌──────────────┐
        │   Resources  │      │  PDF Writer  │
        │              │      │              │
        │  Fonts       │      │  Objects     │
        │  Images      │      │  Streams     │
        │  Shared      │      │  References  │
        └──────────────┘      └──────────────┘
```

### Layered Architecture

**Layer 1: Foundation (Scryber.Common)**
- Core interfaces and contracts
- PDF primitive types and structures
- Resource management abstractions
- Configuration and logging

**Layer 2: Specialized Services**
- **Drawing** (Scryber.Drawing): Graphics primitives, fonts, colors, SVG
- **Expressions** (Scryber.Expressive): Expression parsing and evaluation
- **Styles** (Scryber.Styles): CSS parsing and style management
- **Generation** (Scryber.Generation): Document parsing and binding
- **Imaging** (Scryber.Imaging): Image loading and format conversion

**Layer 3: Integration (Scryber.Components)**
- Component implementations
- HTML element mapping
- Layout engine
- PDF generation orchestration

**Layer 4: Framework Integration (Scryber.Components.Mvc)**
- ASP.NET MVC extensions
- HTTP response integration

## Project Structure

### Dependency Graph

```
Scryber.Common
    │
    ├─→ Scryber.Drawing ────────┐
    │       (fonts, graphics)    │
    │                            │
    ├─→ Scryber.Expressive ──────┤
    │       (expressions)        │
    │                            ▼
    ├─→ Scryber.Styles ←──── Scryber.Generation
    │       (CSS)              (parsing, binding)
    │                            │
    ├─→ Scryber.Imaging ─────────┤
    │       (images)             │
    │                            │
    └─→ Scryber.Components ◄─────┘
            (main engine)
                │
                ▼
        Scryber.Components.Mvc
            (ASP.NET)
```

### Project Responsibilities

#### Scryber.Common
**Purpose**: Foundation layer with core abstractions

**Key Namespaces**:
- `Scryber`: Core interfaces (`IComponent`, `IDocument`, `IBindableComponent`)
- `Scryber.PDF`: PDF primitive types (`PDFString`, `PDFNumber`, `PDFDictionary`)
- `Scryber.PDF.Native`: Low-level PDF reading and writing
- `Scryber.PDF.Resources`: Resource management (`ISharedResource`)
- `Scryber.Html`: HTML entity definitions
- `Scryber.Logging`: Trace and performance logging

**Key Types**:
- `IComponent`: Base interface for all components with lifecycle methods
- `IPDFComponent`: Components that can render to PDF
- `IResourceContainer`: Manages document-level resources
- `PDFObjectRef`: Indirect object references in PDF structure

#### Scryber.Drawing
**Purpose**: Graphics primitives and typography

**Key Namespaces**:
- `Scryber.Drawing`: Core types (`PDFColor`, `PDFUnit`, `PDFPoint`, `PDFRect`)
- `Scryber.Drawing.Fonts`: Font management and metrics
- `Scryber.Drawing.Svg`: SVG path parsing and rendering
- `Scryber.PDF.Resources`: Font resource generation

**Key Types**:
- `FontFactory`: Creates and caches font instances
- `PDFFontResource`: Manages font resources in PDF output
- `PDFSolidBrush`, `PDFSolidPen`: Drawing styles
- `SVGPath`: SVG path data parsing and rendering

**Design Notes**:
- Embeds standard PDF fonts as resources (Helvetica, Times, Courier, etc.)
- Uses Scryber.Core.OpenType for TrueType/OpenType parsing
- Font metrics used for text measurement during layout

#### Scryber.Expressive
**Purpose**: Expression parsing and evaluation engine

**Key Namespaces**:
- `Scryber.Expressive`: Core expression types and parser
- `Scryber.Expressive.Expressions`: Expression tree nodes
- `Scryber.Expressive.Functions`: Built-in functions
- `Scryber.Expressive.Operators`: Mathematical and logical operators

**Key Types**:
- `ExpressionParser`: Tokenizes and parses expression strings
- `IExpression`: Base interface for expression tree nodes
- `Context`: Evaluation context with variables and functions
- `BinaryExpressionBase`: Base for operator expressions
- `FunctionExpression`: Function call expressions
- `VariableExpression`: Variable lookup expressions

**Expression Syntax**:
```
Variables:    {{model.name}}
Properties:   {{model.user.firstName}}
Indexing:     {{model.items[0]}}
Math:         {{price * 1.2}}
Functions:    {{concat(firstName, ' ', lastName)}}
Conditionals: {{age >= 18 ? 'Adult' : 'Minor'}}
```

**Built-in Functions**:
- `concat(...)`: String concatenation
- `if(condition, true, false)`: Conditional evaluation
- `index()`: Current iteration index in templates
- `length(array)`: Array/collection length
- And more...

#### Scryber.Styles
**Purpose**: CSS parsing, selector matching, and style cascading

**Key Namespaces**:
- `Scryber.Styles`: Style classes and definitions
- `Scryber.Styles.Parsing`: CSS parser infrastructure
- `Scryber.Styles.Parsing.Typed`: Individual CSS property parsers
- `Scryber.Styles.Selectors`: Selector matching and specificity

**Key Types**:
- `CSSStyleParser`: Main CSS parsing entry point
- `StylesDocument`: Container for style collections (can be remote loaded)
- `CSSStyleItemReader`: Tokenizes CSS content
- Individual parsers: `CSSBackgroundParser`, `CSSFontParser`, `CSSBorderParser`, etc.
- `StyleMatcher`: Matches selectors to components
- `StyleStack`: Manages style inheritance and cascading

**CSS Feature Support**:
- Selectors: element, class, ID, attribute, pseudo-classes (`:before`, `:after`)
- Properties: Most CSS 2.1 properties plus common CSS3 features
- Variables: `var(--custom-property)`
- Calc: `calc(100% - 20px)` (partial support)
- Counters: `counter-reset`, `counter-increment`, `counter()`
- Content: `content` property for generated content

**Design Pattern**: Each CSS property has a dedicated typed parser class
- Example: `CSSFontParser` handles `font`, `font-family`, `font-size`, etc.
- Parsers convert CSS text values to typed style objects
- Allows clean separation and easy extension

#### Scryber.Generation
**Purpose**: Document parsing and data binding infrastructure

**Key Namespaces**:
- `Scryber.Generation`: Parser infrastructure and component creation
- `Scryber.Binding`: Data binding and expression evaluation
- `Scryber.Binding.Expressions`: Data path navigation

**Key Types**:
- `ParserDefintionFactory`: Creates parser definitions for component types
- `ParserControllerDefinition`: Defines controller attachments
- `BindingCalcExpressionFactory`: Creates binding expressions from templates
- `BindingCalcParser`: Integrates Expressive engine with template binding
- `ParserItemExpression`: XPath-like data navigation

**Binding Architecture**:
```
Template: <div>{{model.user.name}}</div>
           │
           ▼
BindingCalcParser.Parse("model.user.name")
           │
           ▼
Creates Expression Tree
           │
           ▼
DataBind phase evaluates with Context
           │
           ▼
Result written to component property
```

#### Scryber.Imaging
**Purpose**: Image loading, decoding, and PDF formatting

**Key Namespaces**:
- `Scryber.Imaging`: Factory infrastructure
- `Scryber.Imaging.Formatted`: PDF image data formatters

**Key Types**:
- `ImageFactoryList`: Manages registered image factories
- `ImageFactoryJpeg`, `ImageFactoryPng`, etc.: Format-specific factories
- `PDFImageData`: Abstract base for image data
- `PDFImageJpegData`: JPEG passthrough (no re-encoding)
- `PDFImageSharpRGB24Data`, `PDFImageSharpRGBA32Data`: Color format converters

**Design Notes**:
- Uses SixLabors.ImageSharp for decoding
- JPEG images passed through without re-encoding
- Other formats converted to RGB24 or RGBA32 for PDF
- Supports data URLs: `data:image/png;base64,...`

#### Scryber.Components
**Purpose**: Main PDF generation engine - orchestrates all subsystems

**Key Namespaces**:
- `Scryber.Components`: Core component types
- `Scryber.Html.Components`: 80+ HTML element implementations
- `Scryber.Html.Parsing`: HTML parsing and component factory
- `Scryber.PDF.Layout`: Layout engine and layout items
- `Scryber.PDF.Native`: PDF generation and writing
- `Scryber.Components.Lists`: List and list item components
- `Scryber.Components.Tables`: Table, row, and cell components

**Key Types**:
- `Document`: Root component and main public API
- `Page`, `PageBase`, `Section`: Page-level components
- `HTMLParser`: Parses HTML using HtmlAgilityPack
- `HTMLParserComponentFactory`: Maps HTML tags to component classes
- `PDFLayoutDocument`: Manages layout state
- `LayoutEngineDocument`, `LayoutEnginePage`, etc.: Layout engines
- `PDFLayoutPage`, `PDFLayoutBlock`, `PDFLayoutLine`: Layout items
- `PDFWriter`: Low-level PDF structure writing

**Component Hierarchy**:
```
Component (abstract base)
    │
    ├─→ VisualComponent
    │       │
    │       ├─→ ContainerComponent
    │       │       │
    │       │       ├─→ PageBase → Page, Section
    │       │       ├─→ Panel → Div, Span
    │       │       ├─→ ListItem
    │       │       └─→ TableCell
    │       │
    │       ├─→ Image
    │       ├─→ Label (text)
    │       └─→ Shape (line, rectangle, etc.)
    │
    ├─→ StylesDocument (external CSS)
    └─→ Template (repeating content)
```

**HTML Element Mapping** (examples):
- `<div>` → `HTMLDiv` → extends `Div` → extends `Panel`
- `<span>` → `HTMLSpan` → extends `Span` → extends `Panel`
- `<table>` → `HTMLTable` → extends `TableGrid`
- `<img>` → `HTMLImage` → extends `Image`
- `<p>` → `HTMLParagraph` → extends `Div`

## Component Model

### Component Lifecycle

All components implement `IComponent` with these lifecycle phases:

```
1. Construction
   Component created via factory or constructor

2. Init(InitContext)
   - Register with document by ID
   - Initialize child components
   - Set up component relationships

3. Load(LoadContext)
   - Load external resources (async)
   - Images, fonts, CSS files
   - Process remote references

4. DataBind(DataContext)
   - Evaluate {{...}} expressions
   - Populate templates
   - Apply dynamic data

5. Layout (implicit during render)
   - Measure component dimensions
   - Calculate positions
   - Handle page breaks
   - Create layout items

6. Render (implicit during SaveAsPDF)
   - Generate PDF structure
   - Write to output stream

7. Dispose()
   - Clean up resources
   - Release cached data
```

### Component Responsibilities

**Base Component**:
- Lifecycle management
- Parent/child relationships
- ID registration
- Style association

**Visual Component** (extends Component):
- Position and size
- Margins, padding, borders
- Background and fill
- Visibility

**Container Component** (extends Visual):
- Child management
- Layout strategy
- Content flow
- Page breaking

### Context Objects

Context objects thread through lifecycle phases without being stored in components:

**InitContext**:
- Document reference
- Trace logging
- Performance tracking

**LoadContext**:
- Async loading support
- Resource cache
- Base URL for relative paths

**DataContext**:
- Data stack (scoped variables)
- Expression evaluation
- Template iteration

**LayoutContext**:
- Current page
- Available space
- Font resources
- Graphics state

**RenderContext**:
- PDF writer
- Resource registration
- Current stream

## PDF Generation Pipeline

### Stage 1: Parsing

**Input**: HTML/XML string or file path
**Output**: Component tree

**Two Parser Paths**:

1. **XML Parser** (strict XHTML):
   - Uses `System.Xml.XmlReader`
   - Requires well-formed XML
   - Namespace-aware
   - Fast and memory-efficient

2. **HTML Parser** (loose HTML):
   - Uses `HtmlAgilityPack`
   - Tolerates malformed HTML
   - Auto-closes tags
   - Slightly slower

**Process**:
```
HTML String
    │
    ▼
HtmlDocument.Load() [HtmlAgilityPack]
    │
    ▼
HTMLParser.Parse(HtmlNode)
    │
    ├─→ HTMLParserComponentFactory.Create(tag name)
    │       │
    │       └─→ Creates component instance
    │
    └─→ Recursively parse children
            │
            ▼
        Complete Component Tree
```

**Component Creation**:
- Factory maintains dictionary: tag name → component type
- Example: `"div"` → `typeof(HTMLDiv)`
- Unknown tags create generic containers or are ignored
- Attributes parsed and applied to component properties

### Stage 2: Initialization

**Input**: Component tree
**Output**: Registered and initialized components

**Process**:
```
Document.Init(InitContext)
    │
    ├─→ Register component IDs
    │       (enables ID-based lookups)
    │
    ├─→ Initialize child components
    │       (recursive)
    │
    └─→ Set up resource containers
            (fonts, images)
```

**Key Activities**:
- Components register themselves by ID with document
- Parent-child relationships established
- Style classes validated
- Font families resolved to font definitions

### Stage 3: Loading

**Input**: Initialized component tree
**Output**: Tree with loaded external resources

**Process**:
```
Document.Load(LoadContext)
    │
    ├─→ Load external CSS files
    │       │
    │       └─→ StylesDocument.Load()
    │               │
    │               └─→ HTTP GET (async)
    │
    ├─→ Load external images
    │       │
    │       └─→ Image.Load()
    │               │
    │               ├─→ HTTP GET (async)
    │               └─→ ImageFactory.Load()
    │                       │
    │                       └─→ Decode to PDFImageData
    │
    └─→ Load external fonts
            │
            └─→ FontFactory.GetFont()
                    │
                    ├─→ HTTP GET (async) for web fonts
                    └─→ Parse TrueType/OpenType
```

**WASM Considerations**:
- All HTTP requests are async
- No blocking I/O allowed
- Can use `DocumentTimerExecution` to yield periodically
- Resources cached to avoid redundant downloads

### Stage 4: Data Binding

**Input**: Loaded component tree + data model
**Output**: Tree with evaluated expressions

**Process**:
```
Document.DataBind(DataContext)
    │
    └─→ For each component:
            │
            ├─→ Evaluate {{...}} in attributes
            │       │
            │       └─→ BindingCalcParser.Parse()
            │               │
            │               └─→ ExpressionParser.Parse()
            │                       │
            │                       └─→ Expression tree
            │
            ├─→ Evaluate {{...}} in text content
            │
            ├─→ Process templates (<template data-bind="...">)
            │       │
            │       └─→ For each item in bound collection:
            │               │
            │               ├─→ Clone template content
            │               ├─→ Push item to data stack
            │               └─→ DataBind clone
            │
            └─→ Recursively bind children
```

**Data Context Stack**:
- Context maintains stack of data scopes
- `model.name` resolves from current scope
- `.name` (dot prefix) means current item
- Template iterations push new scope

**Example**:
```html
<div>{{model.title}}</div>
<ul>
    <template data-bind="{{model.items}}">
        <li>{{.name}}</li>  <!-- . refers to current item -->
    </template>
</ul>
```

### Stage 5: Style Resolution

**Input**: Data-bound component tree + CSS
**Output**: Components with resolved styles

**Process**:
```
For each component:
    │
    ├─→ Collect applicable styles:
    │       │
    │       ├─→ Inline styles (highest priority)
    │       ├─→ ID selector styles
    │       ├─→ Class selector styles
    │       ├─→ Element selector styles
    │       └─→ Inherited styles
    │
    ├─→ Calculate specificity
    │       │
    │       └─→ Sort by: !important > inline > ID > class > element
    │
    ├─→ Apply cascading
    │       │
    │       └─→ Merge in specificity order
    │
    └─→ Resolve computed values
            │
            ├─→ Inherit from parent where applicable
            ├─→ Resolve relative units (%, em)
            └─→ Evaluate var() and calc()
```

**Style Inheritance**:
- Font properties inherit by default
- Box model properties don't inherit
- `inherit` keyword forces inheritance
- `initial` keyword resets to default

### Stage 6: Layout

**Input**: Styled component tree
**Output**: `PDFLayoutDocument` with positioned items

**Core Concept**: Two-stage rendering separates measurement from output

**Layout Process**:
```
Document.RenderToPDF(context)
    │
    └─→ LayoutEngineDocument.Layout()
            │
            ├─→ For each page component:
            │       │
            │       └─→ LayoutEnginePage.Layout()
            │               │
            │               ├─→ Measure header
            │               ├─→ Measure footer
            │               ├─→ Calculate content area
            │               │
            │               └─→ Layout content:
            │                       │
            │                       └─→ LayoutEnginePanel.Layout()
            │                               │
            │                               ├─→ For each child:
            │                               │       │
            │                               │       ├─→ Measure required space
            │                               │       ├─→ Apply positioning
            │                               │       └─→ Layout child content
            │                               │
            │                               └─→ Handle page breaks:
            │                                       │
            │                                       ├─→ If content overflow
            │                                       ├─→ Create continuation
            │                                       └─→ Split content across pages
            │
            └─→ Returns PDFLayoutDocument
```

**Layout Engines** (by component type):

- `LayoutEngineDocument`: Top-level coordinator
- `LayoutEnginePage`: Page layout with header/footer
- `LayoutEnginePanel`: Block and inline flow
- `LayoutEngineTable`: Table layout with colspan/rowspan
- `LayoutEngineList`: Numbered and bulleted lists
- `LayoutEngineText`: Text flow and line breaking

**Text Layout**:
```
LayoutEngineText.Layout(available width)
    │
    ├─→ Split text into words
    │
    ├─→ For each word:
    │       │
    │       ├─→ Measure word width with font metrics
    │       │
    │       ├─→ If fits on current line:
    │       │       └─→ Add to line
    │       │
    │       └─→ If doesn't fit:
    │               ├─→ Apply hyphenation if enabled
    │               ├─→ Break line
    │               └─→ Continue on next line
    │
    └─→ Create PDFLayoutLine items
```

**Layout Items** (output structure):
```
PDFLayoutDocument
    │
    └─→ Pages: List<PDFLayoutPage>
            │
            ├─→ HeaderBlock: PDFLayoutBlock
            ├─→ FooterBlock: PDFLayoutBlock
            │
            └─→ ContentBlock: PDFLayoutBlock
                    │
                    └─→ Columns: List<PDFLayoutRegion>
                            │
                            └─→ Contents: List<PDFLayoutItem>
                                    │
                                    ├─→ PDFLayoutBlock (container)
                                    ├─→ PDFLayoutLine (text)
                                    ├─→ PDFLayoutRun (content)
                                    └─→ PDFLayoutImage, etc.
```

**Box Model**:
```
┌─────────────────────────────────────┐
│ Margin (transparent)                │
│  ┌───────────────────────────────┐  │
│  │ Border                        │  │
│  │  ┌─────────────────────────┐  │  │
│  │  │ Padding                 │  │  │
│  │  │  ┌───────────────────┐  │  │  │
│  │  │  │ Content           │  │  │  │
│  │  │  │                   │  │  │  │
│  │  │  └───────────────────┘  │  │  │
│  │  │                         │  │  │
│  │  └─────────────────────────┘  │  │
│  │                               │  │
│  └───────────────────────────────┘  │
│                                     │
└─────────────────────────────────────┘
```

**Positioning Modes**:

1. **Block Flow** (default for divs):
   - Stacks vertically
   - Takes full width
   - Respects margins

2. **Inline Flow** (default for spans):
   - Flows horizontally
   - Wraps at container edge
   - Vertical alignment

3. **Relative Positioning**:
   - Offset from normal position
   - Space still reserved in flow

4. **Absolute Positioning**:
   - Removed from flow
   - Positioned relative to container
   - No space reserved

5. **Float** (left/right):
   - Removed from flow
   - Content wraps around
   - Cleared with `clear` property

### Stage 7: Rendering

**Input**: `PDFLayoutDocument`
**Output**: PDF byte stream

**Process**:
```
PDFLayoutDocument.OutputToPDF(PDFRenderContext, PDFWriter)
    │
    ├─→ Write PDF header
    │       (%PDF-1.4 or later)
    │
    ├─→ Write document catalog
    │       │
    │       ├─→ Pages tree root
    │       ├─→ Outlines (bookmarks)
    │       ├─→ Named destinations
    │       └─→ Metadata
    │
    ├─→ For each layout page:
    │       │
    │       ├─→ Create page object
    │       ├─→ Create content stream
    │       │       │
    │       │       └─→ Write drawing commands:
    │       │               │
    │       │               ├─→ Set graphics state
    │       │               ├─→ Draw backgrounds
    │       │               ├─→ Draw borders
    │       │               ├─→ Draw text
    │       │               ├─→ Draw images
    │       │               └─→ Draw shapes
    │       │
    │       └─→ Register page resources
    │
    ├─→ Write font resources
    │       │
    │       └─→ Font descriptors + font programs
    │
    ├─→ Write image resources
    │       │
    │       └─→ Image XObjects with data
    │
    ├─→ Write cross-reference table
    │       (byte offsets of all objects)
    │
    └─→ Write trailer
            (points to catalog and xref)
```

**PDF Structure** (simplified):
```
%PDF-1.4
1 0 obj          % Document Catalog
  << /Type /Catalog
     /Pages 2 0 R >>
endobj

2 0 obj          % Pages Tree
  << /Type /Pages
     /Kids [3 0 R]
     /Count 1 >>
endobj

3 0 obj          % Page 1
  << /Type /Page
     /Parent 2 0 R
     /Contents 4 0 R
     /Resources << /Font << /F1 5 0 R >>
                   /XObject << /Im1 6 0 R >> >> >>
endobj

4 0 obj          % Page Content Stream
  << /Length 123 >>
stream
BT
/F1 12 Tf
100 700 Td
(Hello World) Tj
ET
endstream
endobj

5 0 obj          % Font Resource
  << /Type /Font
     /Subtype /TrueType
     ... >>
endobj

6 0 obj          % Image Resource
  << /Type /XObject
     /Subtype /Image
     /Width 100
     /Height 100
     ... >>
endobj

xref             % Cross-reference table
0 7
0000000000 65535 f
0000000009 00000 n
...

trailer
<< /Size 7
   /Root 1 0 R >>
startxref
1234
%%EOF
```

## Subsystem Deep Dive

### CSS Parser Architecture

**Entry Point**: `CSSStyleParser.ParseCSS(string cssContent)`

**Parse Flow**:
```
CSS String
    │
    ▼
CSSStyleItemReader (tokenizer)
    │
    ├─→ Remove /* comments */
    ├─→ Identify selectors
    └─→ Extract property blocks
        │
        ▼
For each selector + properties:
    │
    ├─→ Create Style object
    │
    └─→ For each property:
            │
            └─→ Route to typed parser:
                    │
                    ├─→ CSSFontParser for font-*
                    ├─→ CSSColorParser for color, background-color
                    ├─→ CSSBorderParser for border-*
                    ├─→ CSSPaddingParser for padding-*
                    └─→ etc.
                        │
                        └─→ Parse value and set on Style
```

**Example CSS Property Parser**:
```csharp
// CSSFontParser.cs
public class CSSFontParser : CSSStyleAttributeParser<FontStyle>
{
    protected override bool DoSetStyleValue(Style style,
                                           CSSStyleItemReader reader,
                                           PDFContextBase context)
    {
        string value = reader.CurrentTextValue;

        if(property == "font-family")
        {
            style.Font.FontFamily = ParseFontFamily(value);
        }
        else if(property == "font-size")
        {
            style.Font.FontSize = ParseUnit(value);
        }
        // ... more properties

        return true;
    }
}
```

**CSS Specificity Calculation**:
```
Inline styles:      1000 points
ID selectors:       100 points
Class selectors:    10 points
Element selectors:  1 point

Example:
  div.header           → 1 + 10 = 11
  #main                → 100
  div#main.header      → 1 + 100 + 10 = 111
  style="..."          → 1000
```

### Expression Engine Architecture

**Expression Grammar**:
```
expression  := term (('+' | '-') term)*
term        := factor (('*' | '/') factor)*
factor      := number | string | variable | function | '(' expression ')'
variable    := identifier ('.' identifier | '[' expression ']')*
function    := identifier '(' arguments ')'
arguments   := expression (',' expression)*
```

**Parse Example**:
```
Input: "{{model.price * 1.2}}"

Tokenize:
  OPEN_BRACE, OPEN_BRACE
  IDENTIFIER("model")
  DOT
  IDENTIFIER("price")
  MULTIPLY
  NUMBER(1.2)
  CLOSE_BRACE, CLOSE_BRACE

Parse to Expression Tree:
  BinaryExpression(*)
    ├─→ PropertyExpression
    │     ├─→ VariableExpression("model")
    │     └─→ Property("price")
    └─→ ConstantExpression(1.2)

Evaluate with Context:
  context["model"] = { price: 10.0 }

  PropertyExpression.Evaluate()
    → model.price → 10.0

  BinaryExpression.Evaluate()
    → 10.0 * 1.2 → 12.0

Result: 12.0
```

**Function Evaluation**:
```csharp
// Built-in function: concat
public class ConcatFunction : IFunction
{
    public string Name => "concat";

    public object Evaluate(IExpression[] arguments, Context context)
    {
        var values = arguments.Select(a => a.Evaluate(context));
        return string.Concat(values);
    }
}

// Usage in template:
{{concat(user.firstName, ' ', user.lastName)}}
```

### Layout Engine Architecture

**Layout Engine Selection**:
```csharp
// LayoutEngineFactory.cs
public static IPDFLayoutEngine GetEngine(Component component)
{
    if(component is PageBase)
        return new LayoutEnginePage();
    else if(component is TableGrid)
        return new LayoutEngineTable();
    else if(component is ListOrdered || component is ListUnordered)
        return new LayoutEngineList();
    else if(component is Panel)
        return new LayoutEnginePanel();
    else if(component is Label || component is TextLiteral)
        return new LayoutEngineText();
    // ... more types
}
```

**Layout Algorithm** (simplified):
```csharp
// LayoutEnginePanel.cs
public override void Layout(PDFLayoutContext context,
                           Component component)
{
    Panel panel = (Panel)component;
    PDFRect availableSpace = context.Space;

    // Create block for this panel
    PDFLayoutBlock block = new PDFLayoutBlock();
    block.Position = availableSpace.Location;

    PDFUnit currentY = 0;

    foreach(var child in panel.Children)
    {
        // Get layout engine for child
        var childEngine = GetEngine(child);

        // Calculate available space for child
        PDFRect childSpace = new PDFRect(
            x: availableSpace.X + child.Margins.Left,
            y: currentY,
            width: availableSpace.Width - child.Margins.Horizontal,
            height: availableSpace.Height - currentY
        );

        // Layout child
        context.Space = childSpace;
        childEngine.Layout(context, child);

        // Get child's layout block
        PDFLayoutBlock childBlock = context.DocumentLayout.CurrentPage
                                          .LastBlock;

        // Move Y position down
        currentY += childBlock.Height + child.Margins.Bottom;

        // Check for page break
        if(currentY > availableSpace.Height)
        {
            // Create new page
            context.DocumentLayout.AddPage();
            currentY = 0;
        }

        // Add child block to panel block
        block.Add(childBlock);
    }

    block.Height = currentY;
    context.DocumentLayout.CurrentPage.Add(block);
}
```

**Text Line Breaking**:
```csharp
// LayoutEngineText.cs (simplified)
public void LayoutTextLine(string text, PDFUnit availableWidth,
                          PDFFont font, PDFUnit fontSize)
{
    List<string> words = SplitIntoWords(text);
    PDFLayoutLine currentLine = new PDFLayoutLine();
    PDFUnit currentWidth = 0;

    foreach(string word in words)
    {
        PDFUnit wordWidth = MeasureWord(word, font, fontSize);

        if(currentWidth + wordWidth <= availableWidth)
        {
            // Word fits on current line
            currentLine.Add(new PDFTextRun(word));
            currentWidth += wordWidth;
        }
        else
        {
            // Word doesn't fit - check hyphenation
            if(EnableHyphenation && wordWidth > availableWidth * 0.5)
            {
                var parts = HyphenateWord(word);
                currentLine.Add(new PDFTextRun(parts.First + "-"));
                FinishLine(currentLine);

                // Continue with remaining part
                currentLine = new PDFLayoutLine();
                currentLine.Add(new PDFTextRun(parts.Second));
                currentWidth = MeasureWord(parts.Second, font, fontSize);
            }
            else
            {
                // Break line and continue
                FinishLine(currentLine);
                currentLine = new PDFLayoutLine();
                currentLine.Add(new PDFTextRun(word));
                currentWidth = wordWidth;
            }
        }
    }

    FinishLine(currentLine);
}
```

### Resource Management

**Resource Lifecycle**:
```
1. Request Resource
   Component needs font/image during Load or Layout

2. Check Cache
   IResourceContainer.TryGetResource(key)

3. If Not Cached:
   ├─→ Load resource (file, HTTP, embedded)
   ├─→ Create resource object (PDFFontResource, PDFImageXObject)
   ├─→ Register with document: IResourceContainer.AddResource(key, resource)
   └─→ Return resource

4. If Cached:
   └─→ Return cached resource

5. During Render:
   ├─→ Resource registers itself in page resources dictionary
   └─→ PDF writer outputs resource object once

6. References:
   Multiple components reference same resource by name
   (e.g., /F1 for font, /Im1 for image)
```

**Example: Font Resource**:
```
Component A needs Helvetica 12pt
    │
    └─→ FontFactory.GetFont("Helvetica", 12)
            │
            ├─→ Check cache: "Helvetica-12"
            │       Not found
            │
            ├─→ Load Helvetica font definition
            ├─→ Create PDFFontResource
            ├─→ Cache: fonts["Helvetica-12"] = resource
            └─→ Return resource

Component B needs Helvetica 12pt
    │
    └─→ FontFactory.GetFont("Helvetica", 12)
            │
            └─→ Check cache: "Helvetica-12"
                    Found → Return cached resource

During Render:
    Page 1 renders Component A
        └─→ References font as /F1

    Page 2 renders Component B
        └─→ References same font as /F1

    Font written to PDF once:
        5 0 obj
          << /Type /Font
             /Subtype /TrueType
             /BaseFont /Helvetica
             ... >>
        endobj
```

## Data Flow

### Complete Example: HTML to PDF

**Input HTML**:
```html
<!DOCTYPE html>
<html>
<head>
    <style>
        .header {
            font-size: 24pt;
            color: #336699;
            margin-bottom: 20pt;
        }
        .item {
            margin: 10pt;
            padding: 5pt;
            border: 1pt solid black;
        }
    </style>
</head>
<body>
    <div class="header">{{model.title}}</div>
    <div>
        <template data-bind="{{model.items}}">
            <div class="item">{{.name}}: ${{.price}}</div>
        </template>
    </div>
</body>
</html>
```

**Data Model**:
```csharp
var model = new {
    title = "Product List",
    items = new[] {
        new { name = "Widget", price = 10.00 },
        new { name = "Gadget", price = 25.50 }
    }
};
```

**Data Flow**:

**1. Parse** (HTML → Components):
```
HTMLParser.Parse(html)
    │
    ├─→ HTMLBody
    │     │
    │     ├─→ HTMLDiv (class="header")
    │     │     └─→ TextLiteral("{{model.title}}")
    │     │
    │     └─→ HTMLDiv
    │           └─→ HTMLTemplate (data-bind="{{model.items}}")
    │                 └─→ HTMLDiv (class="item")
    │                       └─→ TextLiteral("{{.name}}: ${{.price}}")
```

**2. Init** (Register components):
```
Document.Init(context)
    └─→ Each component initializes
        (No visual change, just setup)
```

**3. Load** (External resources):
```
Document.Load(context)
    │
    └─→ StylesDocument.Load()
            │
            ├─→ CSSStyleParser.Parse(<style> content)
            │       │
            │       ├─→ Parse .header { font-size: 24pt; ... }
            │       │     └─→ Create Style with specificity 10
            │       │
            │       └─→ Parse .item { margin: 10pt; ... }
            │             └─→ Create Style with specificity 10
            │
            └─→ Register styles with document
```

**4. DataBind** (Evaluate expressions):
```
Document.DataBind(context)
    │
    ├─→ HTMLDiv (class="header")
    │     │
    │     └─→ TextLiteral
    │           │
    │           ├─→ Parse "{{model.title}}"
    │           ├─→ Evaluate with context (model.title = "Product List")
    │           └─→ Set text = "Product List"
    │
    └─→ HTMLTemplate (data-bind="{{model.items}}")
            │
            ├─→ Parse "{{model.items}}"
            ├─→ Evaluate → returns array with 2 items
            │
            └─→ For each item:
                    │
                    ├─→ Clone template content (HTMLDiv with TextLiteral)
                    ├─→ Push item to data stack
                    │
                    ├─→ DataBind clone:
                    │     │
                    │     └─→ Parse "{{.name}}: ${{.price}}"
                    │           │
                    │           ├─→ .name evaluates to "Widget" (1st) / "Gadget" (2nd)
                    │           ├─→ .price evaluates to 10.00 / 25.50
                    │           └─→ Result: "Widget: $10.00" / "Gadget: $25.50"
                    │
                    └─→ Add clone to parent

Result Component Tree:
    HTMLBody
      ├─→ HTMLDiv (class="header")
      │     └─→ TextLiteral("Product List")
      │
      └─→ HTMLDiv
            ├─→ HTMLDiv (class="item")
            │     └─→ TextLiteral("Widget: $10.00")
            │
            └─→ HTMLDiv (class="item")
                  └─→ TextLiteral("Gadget: $25.50")
```

**5. Style Resolution**:
```
For HTMLDiv (class="header"):
    │
    ├─→ Match selectors:
    │     └─→ .header (specificity: 10)
    │
    ├─→ Apply styles:
    │     ├─→ font-size: 24pt
    │     ├─→ color: #336699
    │     └─→ margin-bottom: 20pt
    │
    └─→ Store computed style

For each HTMLDiv (class="item"):
    │
    ├─→ Match selectors:
    │     └─→ .item (specificity: 10)
    │
    ├─→ Apply styles:
    │     ├─→ margin: 10pt
    │     ├─→ padding: 5pt
    │     └─→ border: 1pt solid black
    │
    └─→ Store computed style
```

**6. Layout**:
```
LayoutEngineDocument.Layout()
    │
    └─→ LayoutEnginePage.Layout()
            │
            └─→ LayoutEnginePanel.Layout(HTMLBody)
                    │
                    ├─→ Layout HTMLDiv.header:
                    │     │
                    │     ├─→ LayoutEngineText("Product List", 24pt)
                    │     │     │
                    │     │     ├─→ Measure: width=150pt, height=24pt
                    │     │     └─→ Create PDFLayoutLine
                    │     │
                    │     └─→ Add margin-bottom: 20pt
                    │           Total height: 44pt
                    │
                    ├─→ Layout HTMLDiv (container):
                    │     │
                    │     ├─→ Layout HTMLDiv.item (1):
                    │     │     │
                    │     │     ├─→ Add margin: 10pt
                    │     │     ├─→ Add padding: 5pt
                    │     │     ├─→ LayoutEngineText("Widget: $10.00")
                    │     │     │     └─→ Measure: 80pt x 12pt
                    │     │     ├─→ Add border: 1pt
                    │     │     └─→ Total: 102pt x 34pt
                    │     │
                    │     └─→ Layout HTMLDiv.item (2):
                    │           └─→ (same process)
                    │                 Total: 102pt x 34pt
                    │
                    └─→ Create PDFLayoutDocument:
                            │
                            └─→ Page 1:
                                  ├─→ Block (y=0, h=44pt): "Product List"
                                  ├─→ Block (y=44pt, h=34pt): "Widget: $10.00"
                                  └─→ Block (y=78pt, h=34pt): "Gadget: $25.50"
```

**7. Render** (Layout → PDF):
```
PDFLayoutDocument.OutputToPDF(writer)
    │
    ├─→ Write page object
    │
    ├─→ Write content stream:
    │     │
    │     ├─→ Block 1 (header):
    │     │     │
    │     │     ├─→ Set color: 0.2 0.4 0.6 rg
    │     │     ├─→ Set font: /F1 24 Tf
    │     │     ├─→ Position: 0 750 Td
    │     │     └─→ Draw text: (Product List) Tj
    │     │
    │     ├─→ Block 2 (item 1):
    │     │     │
    │     │     ├─→ Draw border:
    │     │     │     10 706 92 24 re
    │     │     │     S
    │     │     │
    │     │     ├─→ Set font: /F1 12 Tf
    │     │     ├─→ Position: 15 711 Td
    │     │     └─→ Draw text: (Widget: $10.00) Tj
    │     │
    │     └─→ Block 3 (item 2):
    │           └─→ (similar)
    │
    └─→ Write font resources:
          /Font << /F1 5 0 R >>

Result PDF:
    Page with formatted content
```

## Design Patterns

### 1. Component Pattern
**Purpose**: Uniform treatment of individual and composite components

**Structure**:
- `IComponent`: Common interface
- Leaf components: `Label`, `Image`, `Shape`
- Composite components: `Panel`, `Page`, `Table`

**Benefits**:
- Recursive operations (Init, Load, DataBind)
- Uniform lifecycle management
- Easy to add new component types

### 2. Factory Pattern
**Purpose**: Decouple component creation from usage

**Examples**:
- `HTMLParserComponentFactory`: HTML tag → Component
- `ImageFactoryList`: Image format → Image handler
- `FontFactory`: Font family → Font resource

**Benefits**:
- Centralized creation logic
- Easy to extend with new types
- Configuration-driven instantiation

### 3. Strategy Pattern
**Purpose**: Select algorithm at runtime

**Examples**:
- `IPDFLayoutEngine`: Different layout strategies per component type
- `CSSStyleAttributeParser<T>`: Different parsing strategies per CSS property
- `ImageFactoryBase`: Different decoding strategies per format

**Benefits**:
- Algorithm encapsulation
- Easy to add new strategies
- Runtime selection based on component type

### 4. Visitor Pattern (Context Objects)
**Purpose**: Separate operations from object structure

**Examples**:
- `InitContext`, `LoadContext`, `DataContext` passed through tree
- Operations (Init, Load, DataBind) implemented as methods
- Context accumulates state without modifying components

**Benefits**:
- Components remain stateless
- Easy to add new operations
- Clear separation of concerns

### 5. Template Method Pattern
**Purpose**: Define algorithm skeleton, defer steps to subclasses

**Examples**:
- `LayoutEngineBase.Layout()`: Common setup, subclasses implement specifics
- `CSSStyleAttributeParser.DoSetStyleValue()`: Framework calls, subclass implements

**Benefits**:
- Code reuse through inheritance
- Enforces consistent workflow
- Extension points for customization

### 6. Flyweight Pattern
**Purpose**: Share common data to reduce memory

**Examples**:
- `ISharedResource`: Fonts and images cached and shared
- Style objects marked immutable after calculation
- Font metrics shared across all uses

**Benefits**:
- Reduced memory footprint
- Smaller PDF file size
- Faster resource lookup

### 7. Builder Pattern
**Purpose**: Construct complex objects step by step

**Examples**:
- `PDFWriter`: Builds PDF structure incrementally
- Document construction through parsing
- Layout item construction during layout phase

**Benefits**:
- Stepwise construction
- Immutable result
- Clear construction process

## Extension Architecture

### Adding Custom Components

**1. Define Component Class**:
```csharp
public class CustomBanner : Panel
{
    public string BannerText { get; set; }
    public PDFColor BannerColor { get; set; } = PDFColors.Blue;

    protected override void DoDataBind(DataContext context)
    {
        base.DoDataBind(context);

        // Add custom data binding logic
        if(!string.IsNullOrEmpty(BannerText))
        {
            var label = new Label();
            label.Text = BannerText;
            label.ForeColor = BannerColor;
            this.Contents.Add(label);
        }
    }
}
```

**2. Register with Factory** (for HTML parsing):
```csharp
public class CustomComponentFactory : HTMLParserComponentFactory
{
    public CustomComponentFactory()
    {
        // Register custom tag
        this.RegisterTag("banner", typeof(CustomBanner));
    }
}

// Use custom factory
var parser = new HTMLParser(new CustomComponentFactory());
var doc = parser.Parse(html);
```

**3. Use in HTML**:
```html
<banner banner-text="Welcome!" banner-color="#FF0000" />
```

### Adding Custom CSS Properties

**1. Define Style Property**:
```csharp
public class CustomStyle : StyleBase
{
    public string CustomProperty { get; set; }
}
```

**2. Create Parser**:
```csharp
public class CSSCustomParser : CSSStyleAttributeParser<CustomStyle>
{
    public CSSCustomParser()
    {
        // Register CSS property name
        this.RegisterProperty("custom-property");
    }

    protected override bool DoSetStyleValue(Style style,
                                           CSSStyleItemReader reader,
                                           PDFContextBase context)
    {
        string value = reader.CurrentTextValue;

        if(reader.CurrentAttribute == "custom-property")
        {
            style.Custom.CustomProperty = value;
            return true;
        }

        return false;
    }
}
```

**3. Register Parser**:
```csharp
CSSStyleParser.RegisterParser(new CSSCustomParser());
```

**4. Use in CSS**:
```css
.my-class {
    custom-property: "my value";
}
```

### Adding Custom Expression Functions

**1. Implement Function**:
```csharp
public class UpperFunction : IFunction
{
    public string Name => "upper";

    public object Evaluate(IExpression[] arguments, Context context)
    {
        if(arguments.Length != 1)
            throw new ArgumentException("upper() requires 1 argument");

        var value = arguments[0].Evaluate(context);
        return value?.ToString().ToUpper();
    }
}
```

**2. Register Function**:
```csharp
var context = new Context();
context.RegisterFunction(new UpperFunction());
```

**3. Use in Template**:
```html
<div>{{upper(model.name)}}</div>
```

### Adding Custom Layout Engine

**1. Implement Engine**:
```csharp
public class CustomLayoutEngine : LayoutEngineBase
{
    public override void Layout(PDFLayoutContext context,
                                Component component)
    {
        CustomComponent custom = (CustomComponent)component;

        // Measure component
        PDFSize size = MeasureComponent(custom, context);

        // Create layout block
        PDFLayoutBlock block = context.DocumentLayout
                                     .CurrentPage
                                     .CreateBlock();
        block.Position = context.Space.Location;
        block.Size = size;

        // Layout children if any
        foreach(var child in custom.Contents)
        {
            LayoutChild(child, context);
        }

        // Register block
        context.DocumentLayout.CurrentPage.Add(block);
    }
}
```

**2. Register Engine**:
```csharp
public class CustomComponent : VisualComponent
{
    public override IPDFLayoutEngine GetEngine()
    {
        return new CustomLayoutEngine();
    }
}
```

## Performance Considerations

### Memory Management

**1. Resource Sharing**:
- Fonts and images cached at document level
- Multiple references to same resource
- Reduces memory footprint and PDF size

**2. Lazy Loading**:
- External resources loaded only when needed
- Images not decoded until layout phase
- CSS parsed on demand

**3. Layout Item Pooling**:
- Consider pooling for high-volume scenarios
- Current implementation creates new objects
- Potential optimization for large documents

### Layout Performance

**1. Two-Stage Rendering**:
- Layout phase separate from render phase
- Allows optimization and pre-calculation
- Can abort before rendering if layout fails

**2. Incremental Layout**:
- Components laid out as encountered
- Page breaks handled during layout
- No need to layout entire document at once

**3. Font Metrics Caching**:
- Character widths cached per font
- Reduces measurement overhead
- Critical for text-heavy documents

### Async Operations

**1. WASM Compatibility**:
- All I/O operations async
- No blocking calls
- Uses `async`/`await` throughout

**2. Parallel Resource Loading**:
- Images and fonts can load in parallel
- HttpClient for remote resources
- Reduces total load time

**3. Timer Execution**:
- `DocumentTimerExecution` yields periodically
- Keeps UI responsive during generation
- Essential for large documents in WASM

### PDF Output Optimization

**1. Stream Compression**:
- Content streams can be compressed
- Uses FlateDecode filter
- Reduces file size by 50-70%

**2. Resource Deduplication**:
- Identical resources referenced once
- Image hash checking
- Font embedding optimized

**3. Incremental Writing**:
- Objects written as created
- No need to buffer entire PDF
- Supports streaming to output

### Profiling and Diagnostics

**1. Trace Logging**:
- Performance logging available
- Track time per pipeline stage
- Identify bottlenecks

**2. Memory Profiling**:
- Monitor resource cache size
- Track layout item count
- Detect memory leaks

**3. Layout Diagnostics**:
- Can dump layout tree
- Visualize box model
- Debug positioning issues

## Conclusion

Scryber.Core's architecture enables sophisticated PDF generation through:

1. **Clean Separation**: Specialized projects with clear responsibilities
2. **Extensibility**: Multiple extension points for customization
3. **Performance**: Optimized resource management and lazy loading
4. **Standards Compliance**: CSS box model and HTML semantics
5. **Modern .NET**: Multi-targeting, async/await, WASM compatible

The pipeline architecture (Parse → Init → Load → DataBind → Style → Layout → Render) provides natural breakpoints for debugging and extension, while the component model ensures consistent behavior across all element types.

Understanding this architecture enables effective development, debugging, and extension of the Scryber.Core PDF generation engine.
