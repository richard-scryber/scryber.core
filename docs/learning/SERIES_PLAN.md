# Scryber.Core Learning Series Plan

This document outlines the complete learning series for Scryber.Core, organized into 8 comprehensive series covering everything from basics to advanced features.

## Series Overview

1. **Getting Started** (9 articles) - Foundation and basics
2. **Data Binding & Expressions** (9 articles) - Dynamic content and data
3. **Styling & Appearance** (9 articles) - Visual design and styling
4. **Layout & Positioning** (9 articles) - Page structure and positioning
5. **Typography & Fonts** (9 articles) - Text formatting and fonts
6. **Content Components** (9 articles) - Images, SVG, lists, tables
7. **Document Configuration** (8 articles) - Logging, security, conformance
8. **Practical Applications** (9 articles) - Real-world examples and patterns

**Total: 71 articles across 8 series**

---

## Series 1: Getting Started (9 articles)

**Target Audience:** Complete beginners to Scryber
**Prerequisites:** Basic HTML/CSS knowledge
**Goal:** Create your first PDF document and understand core concepts

### Articles:

1. **index.md** - Getting Started Overview
   - What is Scryber.Core?
   - Use cases and when to use Scryber
   - Installation and setup
   - Your first "Hello World" PDF
   - Overview of the series

2. **01_installation_setup.md**
   - NuGet package installation
   - Project configuration
   - Required dependencies
   - IDE setup and tooling

3. **02_first_document.md**
   - HTML document structure for PDF
   - Basic document creation in C#
   - Using PDFDocument class
   - Generating and saving PDFs
   - DOCTYPE and namespaces

4. **03_html_to_pdf.md**
   - HTML elements supported
   - Converting HTML to PDF
   - Differences from browser rendering
   - Common gotchas

5. **04_css_basics.md**
   - CSS in Scryber documents
   - Inline styles vs stylesheets
   - Supported CSS properties
   - CSS selectors overview

6. **05_pages_sections.md**
   - Pages and sections
   - Headers and footers
   - Multi-section documents
   - Section breaks

7. **06_basic_content.md**
   - Paragraphs, headings, and text
   - Links and anchors
   - Basic lists and tables
   - Simple images

8. **07_output_options.md**
   - Saving to file
   - Streaming to response
   - Output configuration
   - File naming and paths

9. **08_troubleshooting.md**
   - Common errors and solutions
   - Debugging techniques
   - Logging basics
   - Getting help and resources

---

## Series 2: Data Binding & Expressions (9 articles)

**Target Audience:** Users familiar with basic Scryber documents
**Prerequisites:** Series 1 completed
**Goal:** Master dynamic content generation with data binding

### Articles:

1. **index.md** - Data Binding Overview
   - What is data binding?
   - Handlebars-style syntax
   - Data context and models
   - Series roadmap

2. **01_data_binding_basics.md**
   - Basic {{expression}} syntax
   - Binding to properties
   - Passing data to documents (C#, JSON, XML)
   - Data context and simple examples

3. **02_expression_functions.md**
   - Function syntax
   - String functions (concat, substring, upper, lower)
   - Math functions (add, subtract, multiply, divide, calc)
   - Date functions
   - Conditional functions (if, choose)
   - Comparison and logical operators

4. **03_template_iteration.md**
   - Template element basics
   - data-bind attribute
   - Iteration with {{#each}}
   - {{@index}} and {{@key}}
   - Nested loops

5. **04_conditional_rendering.md**
   - {{#if}} and {{#unless}} helpers
   - {{else}} and {{#else}}
   - if() function for inline conditionals
   - Conditional sections and visibility

6. **05_variables_params.md**
   - &lt;var&gt; element for storing values
   - data-id and data-value attributes
   - Document.Params access
   - Variable scope and context
   - Storing calculated values

7. **06_context_scope.md**
   - Understanding data context
   - Parent context access (..)
   - Root context (@@root)
   - Context in nested structures
   - Property access patterns

8. **07_formatting_output.md**
   - Number formatting
   - Date formatting
   - Currency formatting
   - Custom format strings
   - Advanced string manipulation

9. **08_advanced_patterns.md**
   - Complex expressions
   - Aggregation (sum, count, avg)
   - Performance considerations
   - Error handling and null values
   - Best practices

---

## Series 3: Styling & Appearance (9 articles)

**Target Audience:** Users comfortable with basic documents and data binding
**Prerequisites:** Series 1 completed
**Goal:** Master visual styling and appearance customization

### Articles:

1. **index.md** - Styling & Appearance Overview
   - CSS in PDF generation
   - Style inheritance and cascade
   - Supported properties
   - Series overview

2. **01_css_selectors_specificity.md**
   - Element, class, and ID selectors
   - Attribute selectors
   - Pseudo-classes
   - Specificity and cascade rules
   - Style inheritance

3. **02_colors_backgrounds.md**
   - Color formats (hex, rgb, named)
   - Color and background-color properties
   - Transparency and opacity
   - Background images
   - Background positioning and repeat

4. **03_borders_spacing.md**
   - Border width, style, color
   - Individual border sides
   - Border radius
   - Margin and padding properties
   - Box model

5. **04_units_measurements.md**
   - Absolute units (pt, px, in, cm, mm)
   - Relative units (%, em, rem)
   - calc() function for calculations
   - Unit conversions
   - Best practices for each unit type

6. **05_text_styling.md**
   - Font size, weight, and style
   - Text color and alignment
   - Line height and letter spacing
   - Text decoration and transform
   - Text indentation

7. **06_display_visibility.md**
   - Display property (block, inline, inline-block, none)
   - Visibility property
   - Hidden attribute
   - Conditional display with data binding

8. **07_style_organization.md**
   - Inline vs embedded vs external styles
   - Creating reusable style classes
   - Multiple classes
   - Style element and external stylesheets
   - Organizing and maintaining styles

9. **08_styling_best_practices.md**
   - Performance tips
   - Browser vs PDF differences
   - Common pitfalls
   - Troubleshooting styling issues
   - Maintainable patterns

---

## Series 4: Layout & Positioning (9 articles)

**Target Audience:** Users familiar with basic styling
**Prerequisites:** Series 1, 3 recommended
**Goal:** Master page layout, positioning, and document structure

### Articles:

1. **index.md** - Layout & Positioning Overview
   - Page-based layout model
   - Positioning concepts
   - Flow and breaks
   - Series roadmap

2. **01_page_sizes_orientation.md**
   - Standard page sizes (Letter, A4, Legal)
   - Custom page sizes
   - Portrait vs landscape
   - Setting page size in CSS and sections
   - Mixing orientations

3. **02_page_margins.md**
   - Page margins and printable area
   - Setting margins in CSS
   - Margin boxes
   - Section-specific margins

4. **03_sections.md**
   - Section element
   - Multiple sections in one document
   - Section-specific settings (size, orientation, margins)
   - Section breaks

5. **04_page_breaks.md**
   - page-break-before and page-break-after
   - page-break-inside
   - Avoiding breaks in content
   - Controlling pagination
   - Orphans and widows

6. **05_column_layout.md**
   - Column count and width
   - Column gap and rules
   - Column breaks
   - Multi-column content
   - Column spans

7. **06_positioning.md**
   - Position property (static, relative, absolute)
   - Top, left, right, bottom
   - Positioning context
   - Overlays and watermarks
   - Position mode for precise placement

8. **07_headers_footers.md**
   - Page headers and footers
   - Running headers/footers
   - First/last page variations
   - Continuation headers/footers
   - Dynamic content in headers
   - Page numbers

9. **08_layout_best_practices.md**
   - Layout strategies
   - Performance considerations
   - Cross-page content handling
   - Common layout patterns
   - Troubleshooting layout issues

---

## Series 5: Typography & Fonts (9 articles)

**Target Audience:** Users wanting to customize text appearance
**Prerequisites:** Series 1 completed
**Goal:** Master font usage, including custom and remote fonts

### Articles:

1. **index.md** - Typography & Fonts Overview
   - Typography in PDF
   - Font embedding
   - Font sources
   - Series overview

2. **01_font_basics.md**
   - Font families and font stack
   - font-family, font-size, font-weight, font-style
   - Generic font families
   - Standard 14 PDF fonts
   - When to use standard fonts

3. **02_custom_fonts.md**
   - Loading custom fonts
   - Font file formats (TTF, OTF, WOFF)
   - @font-face declaration
   - Font registration and paths
   - Font embedding and subsetting

4. **03_google_fonts.md**
   - Using Google Fonts in PDFs
   - Loading from Google Fonts
   - Font selection and pairing
   - Performance considerations
   - Best practices

5. **04_font_awesome.md**
   - Font Awesome integration
   - Icon fonts in PDFs
   - Using icon classes
   - Icon sizing and styling
   - Other icon font libraries

6. **05_web_fonts.md**
   - Loading remote fonts
   - Web font URLs and CDNs
   - Font caching
   - Fallback strategies
   - Cross-origin considerations

7. **06_text_metrics.md**
   - Line height (leading)
   - Letter spacing (tracking)
   - Word spacing
   - Text indentation
   - Optimal readability settings

8. **07_counters.md**
   - CSS counters for numbering
   - counter-reset and counter-increment
   - counter() and counters() functions
   - Nested counters
   - Custom counter styles
   - Practical examples (chapters, sections)

9. **08_typography_best_practices.md**
   - Font selection and pairing
   - Readability guidelines
   - Performance and file size
   - Font licensing compliance
   - Accessibility considerations

---

## Series 6: Content Components (9 articles)

**Target Audience:** Users ready to work with complex content
**Prerequisites:** Series 1, 2 recommended
**Goal:** Master images, SVG, tables, and other content types

### Articles:

1. **index.md** - Content Components Overview
   - Content types in Scryber
   - Component overview
   - Best practices
   - Series roadmap

2. **01_images.md**
   - Image element and formats (PNG, JPEG, GIF, SVG)
   - Image sizing and positioning
   - Local vs remote images
   - Image styling (borders, margins, alignment)
   - Data binding image sources
   - Base64 embedded images

3. **02_svg_basics.md**
   - SVG element overview
   - Inline SVG vs SVG files
   - SVG sizing and viewBox
   - SVG positioning

4. **03_svg_drawing.md**
   - SVG shapes (rect, circle, ellipse, polygon, path)
   - SVG lines and polylines
   - SVG text elements
   - SVG styling and attributes
   - Data binding in SVG
   - Dynamic charts and visualizations

5. **04_lists.md**
   - Ordered lists (ol) and unordered lists (ul)
   - List styling and custom markers
   - Nested lists
   - Definition lists
   - List data binding

6. **05_tables_basics.md**
   - Table structure (thead, tbody, tfoot)
   - Rows and columns
   - Table borders and styling
   - Cell spacing, padding, and alignment
   - Column widths and groups

7. **06_tables_advanced.md**
   - Dynamic table rows with data binding
   - Template binding in tables
   - Calculated columns
   - Spanning cells (colspan, rowspan)
   - Repeating headers on pages
   - Table page breaks

8. **07_attachments_embedded.md**
   - File attachments (object element)
   - Attachment icons and styling
   - Embedding files in PDFs
   - Data-bound attachments
   - Embed and iframe elements
   - Content inclusion and modular documents

9. **08_content_best_practices.md**
   - Performance optimization
   - Image and SVG optimization
   - Table performance
   - Accessibility considerations
   - Common patterns and troubleshooting

---

## Series 7: Document Configuration (8 articles)

**Target Audience:** Users needing production-ready documents
**Prerequisites:** Series 1 completed
**Goal:** Configure logging, security, conformance, and optimization

### Articles:

1. **index.md** - Document Configuration Overview
   - Configuration options
   - Production considerations
   - Security and compliance
   - Series overview

2. **01_document_properties.md**
   - Title, author, subject, keywords
   - Creator and producer
   - Metadata and custom properties
   - Document information dictionary

3. **02_logging.md**
   - Scryber logging system
   - Log levels (Error, Warning, Info, Verbose)
   - Configuring logging output
   - Custom log handlers
   - Performance and diagnostic logging
   - Log analysis and troubleshooting

4. **03_error_handling_conformance.md**
   - Error handling strategies
   - Try-catch patterns
   - Strict vs Lax conformance modes
   - HTML conformance
   - Validation and error recovery
   - Graceful degradation

5. **04_pdf_versions.md**
   - PDF version selection
   - PDF/A compliance (archival)
   - PDF/X compliance (printing)
   - Feature compatibility by version

6. **05_security.md**
   - Document encryption
   - User and owner passwords
   - Permission levels (printing, copying, editing)
   - Form filling permissions
   - Security best practices

7. **06_optimization_performance.md**
   - File size optimization
   - Image compression
   - Font subsetting
   - Resource caching
   - Performance best practices
   - Memory management
   - Benchmarking and scaling

8. **07_production_deployment.md**
   - Production configuration
   - Error handling in production
   - Monitoring and alerting
   - Backup and recovery
   - Common deployment scenarios
   - Troubleshooting guide

---

## Series 8: Practical Applications (9 articles)

**Target Audience:** All users wanting real-world examples
**Prerequisites:** Varies by article
**Goal:** Build complete, real-world documents

### Articles:

1. **index.md** - Practical Applications Overview
   - Learning by example
   - Pattern library
   - Complete examples
   - Series guide

2. **01_invoice_template.md**
   - Invoice structure and layout
   - Company header and branding
   - Line items with data binding
   - Calculations (subtotals, tax, total)
   - Payment terms and footer

3. **02_business_letter.md**
   - Letterhead design
   - Address blocks
   - Letter body formatting
   - Signature blocks
   - Multi-page letter handling

4. **03_report_template.md**
   - Report cover page
   - Table of contents
   - Sections and chapters
   - Charts and graphs (SVG)
   - Executive summary
   - Page numbering and headers

5. **04_certificate_template.md**
   - Certificate design with borders
   - Backgrounds and decorative elements
   - Dynamic names and dates
   - Signatures and seals
   - Landscape orientation

6. **05_data_driven_report.md**
   - Loading data from APIs/databases
   - Dynamic tables with calculations
   - Conditional sections
   - Charts and visualizations
   - Summary statistics and KPIs

7. **06_catalog_brochure.md**
   - Product catalog layout
   - Grid layouts with images
   - Product descriptions and pricing
   - Category pages
   - Multi-column design

8. **07_form_template.md**
   - Form structure and layout
   - Form fields and labels
   - Checkboxes and instructions
   - Print-and-fill forms
   - Form validation display

9. **08_multi_language_branded.md**
   - Multi-language document patterns
   - Localization and i18n
   - Brand guidelines implementation
   - Color schemes and typography standards
   - Reusable branded components
   - Template management and automation

---

## Implementation Plan

### Phase 1: Core Series (Months 1-2)
- Series 1: Getting Started (9 articles)
- Series 2: Data Binding & Expressions (9 articles)
- Priority: High - Foundation for all other content

### Phase 2: Visual & Layout (Months 3-4)
- Series 3: Styling & Appearance (9 articles)
- Series 4: Layout & Positioning (9 articles)
- Series 5: Typography & Fonts (9 articles)
- Priority: High - Essential for document design

### Phase 3: Content & Configuration (Month 5)
- Series 6: Content Components (9 articles)
- Series 7: Document Configuration (8 articles)
- Priority: Medium - Advanced features

### Phase 4: Examples & Patterns (Month 6)
- Series 8: Practical Applications (9 articles)
- Priority: Medium - Reinforces learning

### Documentation Standards

Each article should include:
- Clear learning objectives
- Code examples (HTML/CSS)
- C# code where applicable
- Screenshots of output
- Common pitfalls section
- "Try it yourself" exercises
- Links to related articles
- Complete working examples

### File Structure

```
docs/
├── learning/
│   ├── SERIES_PLAN.md
│   ├── 01-getting-started/
│   │   ├── index.md
│   │   ├── 01_installation_setup.md
│   │   ├── 02_first_document.md
│   │   └── ...
│   ├── 02-data-binding/
│   │   ├── index.md
│   │   ├── 01_data_binding_basics.md
│   │   └── ...
│   ├── 03-styling/
│   │   ├── index.md
│   │   └── ...
│   ├── 04-layout/
│   ├── 05-typography/
│   ├── 06-content/
│   ├── 07-configuration/
│   └── 08-practical/
└── examples/
    ├── invoice/
    ├── report/
    ├── letter/
    └── ...
```

---

## Success Metrics

- **Completeness**: All 106 articles written and reviewed
- **Quality**: Each article tested with working code examples
- **Usability**: Clear progression from beginner to advanced
- **Coverage**: All major features documented
- **Maintainability**: Easy to update as library evolves

## Notes

- Each index.md serves as both overview and navigation
- Cross-linking between series is encouraged
- Examples should be progressively complex
- Real-world use cases prioritized
- Community feedback incorporated
- Regular updates as library evolves
