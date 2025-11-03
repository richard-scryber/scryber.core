---
layout: default
title: data-fit-to
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-fit-to : The PDF Link Destination Fit Mode Attribute

The `data-fit-to` attribute controls how PDF viewers display the destination when following internal document links. It specifies the zoom level and viewport positioning when navigating to anchored elements, providing precise control over the viewing experience for internal navigation.

## Summary

The `data-fit-to` attribute defines the display mode for link destinations:

- Controls zoom and viewport when following internal document links
- Supports standard PDF destination fit modes (FitH, FitV, FitB, FitR, Fit, XYZ)
- Works exclusively with anchor (`<a>`) elements linking to internal destinations
- Enables optimal viewing of linked content
- Provides user-friendly navigation experiences
- Maps to PDF specification destination fit types

This attribute is essential for:
- Creating table of contents with optimal destination views
- Building navigation systems with predictable viewing behavior
- Linking to figures, tables, and sections with appropriate zoom
- Controlling how bookmarks and internal links display targets
- Providing consistent navigation experiences
- Implementing PDF-specific navigation patterns

---

## Usage

The `data-fit-to` attribute is used exclusively with anchor (`<a>`) elements that link to internal destinations:

```html
<!-- Fit entire page in view -->
<a href="#section1" data-fit-to="FullPage">Go to Section 1</a>

<!-- Fit page width (default behavior) -->
<a href="#figure1" data-fit-to="PageWidth">View Figure 1</a>

<!-- Fit page height -->
<a href="#table1" data-fit-to="PageHeight">See Table 1</a>

<!-- Fit bounding box of target element -->
<a href="#chart" data-fit-to="BoundingBox">View Chart</a>

<!-- With data binding -->
<a href="#{{.targetId}}" data-fit-to="{{.fitMode}}">{{.linkText}}</a>
```

---

## Supported Elements

The `data-fit-to` attribute is supported exclusively on:

### Anchor Element
- `<a>` - The anchor/link element when linking to internal destinations (href="#...")

**Requirements**:
- Element must have `href` attribute with internal anchor (starts with `#`)
- Target element must have matching `id` attribute
- Not applicable to external URLs or file links

**Not supported on**:
- External links (href="http://...")
- File links (href="document.pdf")
- Action links (href="!NextPage")
- Elements other than `<a>`

---

## Binding Values

### Fit Mode Values

**Type**: `OutlineFit` enumeration
**Default**: `PageWidth`
**Binding**: Supports data binding expressions

#### Available Fit Modes

| Value | PDF Fit Type | Description | Best Used For |
|-------|--------------|-------------|---------------|
| `FullPage` | Fit | Displays entire page in viewer | Page-level navigation, TOC entries |
| `PageWidth` | FitH | Fits page width, maintains aspect ratio | Default behavior, most text content |
| `PageHeight` | FitV | Fits page height, maintains aspect ratio | Tall tables, vertical content |
| `BoundingBox` | FitR | Fits bounding box of target element | Specific figures, charts, diagrams |

```html
<!-- FullPage: Show entire page -->
<a href="#chapter1" data-fit-to="FullPage">Chapter 1</a>

<!-- PageWidth: Fit width (default) -->
<a href="#section2" data-fit-to="PageWidth">Section 2</a>

<!-- PageHeight: Fit height -->
<a href="#figure3" data-fit-to="PageHeight">Figure 3</a>

<!-- BoundingBox: Fit element bounds -->
<a href="#chart4" data-fit-to="BoundingBox">Chart 4</a>

<!-- Data binding -->
<template data-bind="{{model.tocEntries}}">
    <a href="#{{.targetId}}" data-fit-to="{{.displayMode}}">{{.title}}</a>
</template>
```

**Data Model Example**:
```csharp
public enum DestinationFit
{
    FullPage,
    PageWidth,
    PageHeight,
    BoundingBox
}

public class TocEntry
{
    public string TargetId { get; set; }
    public string Title { get; set; }
    public DestinationFit DisplayMode { get; set; }
}

public class DocumentModel
{
    public List<TocEntry> TocEntries { get; set; } = new List<TocEntry>
    {
        new TocEntry
        {
            TargetId = "introduction",
            Title = "Introduction",
            DisplayMode = DestinationFit.FullPage
        },
        new TocEntry
        {
            TargetId = "figure1",
            Title = "Figure 1: Sales Chart",
            DisplayMode = DestinationFit.BoundingBox
        }
    };
}
```

---

## Notes

### How Destination Fit Works

When a user clicks a link with `data-fit-to`, the PDF viewer:

1. **Navigates**: Jumps to the target element with matching `id`
2. **Calculates View**: Determines viewport based on fit mode
3. **Adjusts Zoom**: Sets zoom level according to fit type
4. **Positions**: Centers or positions content appropriately

**FullPage** (Fit):
```html
<a href="#chapter1" data-fit-to="FullPage">Chapter 1</a>
```
- Shows entire page containing the target
- Zoom adjusts to fit full page in viewer
- Good for: Chapter starts, major sections

**PageWidth** (FitH - Fit Horizontal):
```html
<a href="#section2" data-fit-to="PageWidth">Section 2</a>
```
- Fits page width to viewer width
- Maintains aspect ratio, may require vertical scrolling
- **Default behavior** if not specified
- Good for: Most text content, default navigation

**PageHeight** (FitV - Fit Vertical):
```html
<a href="#table3" data-fit-to="PageHeight">Table 3</a>
```
- Fits page height to viewer height
- Maintains aspect ratio, may require horizontal scrolling
- Good for: Tall tables, vertical charts, portrait layouts

**BoundingBox** (FitR - Fit Rectangle):
```html
<a href="#chart4" data-fit-to="BoundingBox">Chart 4</a>
```
- Zooms to fit the target element's bounding box
- Shows just the target element, optimally sized
- Good for: Figures, charts, diagrams, specific elements

### PDF Fit Type Mapping

Scryber's `OutlineFit` values map to PDF specification fit types:

| OutlineFit Value | PDF Type | PDF Syntax | Description |
|-----------------|----------|------------|-------------|
| `FullPage` | Fit | `[page /Fit]` | Display entire page |
| `PageWidth` | FitH | `[page /FitH top]` | Fit horizontally at vertical position |
| `PageHeight` | FitV | `[page /FitV left]` | Fit vertically at horizontal position |
| `BoundingBox` | FitR | `[page /FitR left bottom right top]` | Fit rectangle (bounding box) |

**Note**: The PDF also supports FitB, FitBH, FitBV (fit bounding box variations) and XYZ (explicit destination coordinates), but these are not currently exposed through the `OutlineFit` enumeration.

### Default Behavior

If `data-fit-to` is not specified:

```html
<!-- These are equivalent -->
<a href="#section1">Go to Section 1</a>
<a href="#section1" data-fit-to="PageWidth">Go to Section 1</a>
```

The default is **PageWidth** (FitH), which provides good general-purpose navigation behavior for most content.

### Internal Links Only

`data-fit-to` only applies to internal document links:

```html
<!-- Works: internal anchor -->
<a href="#section1" data-fit-to="FullPage">Works</a>

<!-- Ignored: external URL -->
<a href="https://example.com" data-fit-to="FullPage">Ignored</a>

<!-- Ignored: file link -->
<a href="document.pdf" data-fit-to="FullPage">Ignored</a>

<!-- Ignored: PDF action -->
<a href="!NextPage" data-fit-to="FullPage">Ignored</a>
```

The attribute has no effect on external links, file references, or navigation actions.

### Target Element Requirements

For `data-fit-to` to work properly:

1. **Target must exist**: Element with matching `id` must be in document
2. **Target must be unique**: `id` values must be unique
3. **Target must be visible**: Hidden elements may not navigate correctly

```html
<!-- Link -->
<a href="#figure1" data-fit-to="BoundingBox">View Figure 1</a>

<!-- Target -->
<div id="figure1">
    <img src="chart.png" style="width: 400pt; height: 300pt;" />
    <p>Figure 1: Annual Sales</p>
</div>
```

### Viewer Behavior Variations

Different PDF viewers may interpret fit modes slightly differently:

- **Adobe Acrobat**: Full spec compliance, all modes work consistently
- **Preview (macOS)**: Generally compliant, some fit modes may vary
- **Browser PDF viewers**: Variable support, may default to standard zoom
- **Mobile PDF viewers**: May adapt fit modes to screen size

Always test navigation in target PDF viewers when fit mode is critical.

### BoundingBox Fit Considerations

The `BoundingBox` fit mode zooms to the target element's bounds:

**Works well for**:
- Images and figures with defined dimensions
- Charts and diagrams
- Tables with borders
- Contained visual elements

**Less effective for**:
- Plain text paragraphs (may zoom too close)
- Elements without clear boundaries
- Inline content
- Small elements (may over-zoom)

```html
<!-- Good: figure with clear bounds -->
<div id="chart1" style="border: 1pt solid #ccc; padding: 10pt; width: 500pt;">
    <img src="sales-chart.png" width="480pt" height="360pt" />
    <p style="text-align: center;">Q4 Sales Performance</p>
</div>
<a href="#chart1" data-fit-to="BoundingBox">View Sales Chart</a>

<!-- Less ideal: plain text -->
<p id="note1">This is a note.</p>
<a href="#note1" data-fit-to="BoundingBox">View Note</a>
<!-- May zoom too close to text -->
```

### Table of Contents Best Practices

When building TOC links, choose fit modes based on content type:

```html
<nav>
    <h2>Table of Contents</h2>
    <ul>
        <!-- Chapters: show full page -->
        <li>
            <a href="#chapter1" data-fit-to="FullPage">1. Introduction</a>
        </li>

        <!-- Sections: fit width -->
        <li style="margin-left: 20pt;">
            <a href="#section1-1" data-fit-to="PageWidth">1.1 Background</a>
        </li>

        <!-- Figures: fit bounding box -->
        <li>
            <a href="#figure1" data-fit-to="BoundingBox">Figure 1: System Architecture</a>
        </li>

        <!-- Tables: fit height or width based on orientation -->
        <li>
            <a href="#table1" data-fit-to="PageHeight">Table 1: Results Summary</a>
        </li>
    </ul>
</nav>
```

### Bookmarks and Outlines

The `data-fit-to` attribute also affects PDF bookmark behavior when combined with `title` attribute:

```html
<!-- Creates bookmark with fit mode -->
<div id="chapter1">
    <h1 title="Chapter 1: Introduction">Chapter 1</h1>
</div>

<a href="#chapter1" data-fit-to="FullPage">Go to Chapter 1</a>
```

The fit mode specified in the link influences how bookmarks display their destinations.

### Accessibility Considerations

Fit modes can improve accessibility:

- **FullPage**: Good for users who need context
- **PageWidth**: Reduces horizontal scrolling needs
- **PageHeight**: Reduces vertical scrolling needs
- **BoundingBox**: Helpful for users with vision impairments (zooms to content)

Choose fit modes that make content easily viewable for your audience.

### Performance Impact

Fit mode has minimal performance impact:

- All modes are part of PDF specification
- No runtime calculation needed
- Viewer handles fit calculation efficiently
- No difference in document file size

### Testing Link Navigation

To test fit modes:

1. Generate PDF document
2. Open in target PDF viewer
3. Click links and observe:
   - Does it navigate to correct location?
   - Is zoom level appropriate?
   - Is content clearly visible?
   - Does it match expected behavior?
4. Test in multiple viewers for consistency

### Common Patterns

**Pattern 1: Chapter Navigation (Full Page)**
```html
<a href="#chapter1" data-fit-to="FullPage">Chapter 1</a>
<a href="#chapter2" data-fit-to="FullPage">Chapter 2</a>
```

**Pattern 2: Section Navigation (Page Width)**
```html
<a href="#section1" data-fit-to="PageWidth">Section 1</a>
<a href="#section2" data-fit-to="PageWidth">Section 2</a>
```

**Pattern 3: Figure Navigation (Bounding Box)**
```html
<a href="#figure1" data-fit-to="BoundingBox">Figure 1</a>
<a href="#figure2" data-fit-to="BoundingBox">Figure 2</a>
```

**Pattern 4: Mixed Content Navigation**
```html
<!-- Choose fit mode based on target type -->
<a href="#intro" data-fit-to="FullPage">Introduction</a>
<a href="#analysis" data-fit-to="PageWidth">Analysis</a>
<a href="#chart1" data-fit-to="BoundingBox">Chart 1</a>
<a href="#table1" data-fit-to="PageHeight">Table 1</a>
```

---

## Examples

### 1. Basic Table of Contents

Simple TOC with appropriate fit modes:

```html
<div style="page-break-after: always;">
    <h1>Table of Contents</h1>

    <div style="margin: 10pt 0;">
        <a href="#introduction" data-fit-to="FullPage"
           style="font-size: 14pt; font-weight: bold;">
            Introduction
        </a>
    </div>

    <div style="margin: 10pt 0 10pt 20pt;">
        <a href="#background" data-fit-to="PageWidth">1.1 Background</a>
    </div>

    <div style="margin: 10pt 0 10pt 20pt;">
        <a href="#objectives" data-fit-to="PageWidth">1.2 Objectives</a>
    </div>

    <div style="margin: 10pt 0;">
        <a href="#methodology" data-fit-to="FullPage"
           style="font-size: 14pt; font-weight: bold;">
            Methodology
        </a>
    </div>

    <div style="margin: 10pt 0 10pt 20pt;">
        <a href="#data-collection" data-fit-to="PageWidth">2.1 Data Collection</a>
    </div>
</div>

<!-- Content pages with ids -->
<div id="introduction" style="page-break-before: always;">
    <h1>Introduction</h1>
    <p>Content...</p>
</div>

<div id="background">
    <h2>Background</h2>
    <p>Content...</p>
</div>
```

### 2. Figure and Table References

Link to figures with optimal viewing:

```html
<div class="document-content">
    <h1>Analysis Report</h1>

    <p>
        The sales data shown in
        <a href="#figure1" data-fit-to="BoundingBox">Figure 1</a>
        demonstrates a clear upward trend.
    </p>

    <p>
        Detailed breakdowns are provided in
        <a href="#table1" data-fit-to="PageHeight">Table 1</a>
        below.
    </p>

    <!-- Later in document -->
    <div id="figure1" style="page-break-before: always; text-align: center;">
        <img src="sales-chart.png" style="width: 500pt; height: 350pt;" />
        <p><strong>Figure 1:</strong> Annual Sales Trends</p>
    </div>

    <div id="table1" style="page-break-before: always;">
        <h2>Table 1: Sales Breakdown by Region</h2>
        <table style="width: 100%; border-collapse: collapse;">
            <!-- Table content -->
        </table>
    </div>
</div>
```

### 3. Data-Driven Navigation Links

Generate navigation from data model:

```csharp
public class NavigationItem
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string FitMode { get; set; } // "FullPage", "PageWidth", etc.
    public int Level { get; set; }
}

public class DocumentModel
{
    public List<NavigationItem> NavItems { get; set; } = new List<NavigationItem>
    {
        new NavigationItem
        {
            Id = "chapter1",
            Title = "Chapter 1: Overview",
            FitMode = "FullPage",
            Level = 1
        },
        new NavigationItem
        {
            Id = "section1-1",
            Title = "1.1 Introduction",
            FitMode = "PageWidth",
            Level = 2
        },
        new NavigationItem
        {
            Id = "figure1-1",
            Title = "Figure 1.1: System Diagram",
            FitMode = "BoundingBox",
            Level = 3
        }
    };
}
```

```html
<nav>
    <h2>Contents</h2>
    <template data-bind="{{model.navItems}}">
        <div style="margin-left: {{(.level - 1) * 20}}pt; margin-bottom: 5pt;">
            <a href="#{{.id}}" data-fit-to="{{.fitMode}}">{{.title}}</a>
        </div>
    </template>
</nav>
```

### 4. Academic Paper with Multiple Reference Types

Different fit modes for different content types:

```html
<div class="paper">
    <h1>Research Paper</h1>

    <p>
        As shown in
        <a href="#fig1" data-fit-to="BoundingBox">Figure 1</a>,
        the correlation is significant. The complete dataset is presented in
        <a href="#table1" data-fit-to="PageHeight">Table 1</a>.
        Methodology details are in
        <a href="#methodology" data-fit-to="PageWidth">Section 2</a>.
    </p>

    <!-- Figures: use BoundingBox -->
    <div id="fig1" style="page-break-before: always; border: 1pt solid #ccc; padding: 20pt;">
        <img src="correlation-graph.png" style="width: 600pt; height: 400pt;" />
        <p style="text-align: center;">
            <strong>Figure 1:</strong> Correlation Analysis Results
        </p>
    </div>

    <!-- Tables: use PageHeight -->
    <div id="table1" style="page-break-before: always;">
        <h2>Table 1: Complete Dataset</h2>
        <table style="width: 100%;">
            <!-- Long table -->
        </table>
    </div>

    <!-- Sections: use PageWidth -->
    <div id="methodology" style="page-break-before: always;">
        <h2>2. Methodology</h2>
        <p>Detailed methodology description...</p>
    </div>
</div>
```

### 5. Interactive Document Map

Document with navigation hub:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Interactive Report</title>
    <style>
        .nav-hub {
            padding: 40pt;
            text-align: center;
        }

        .nav-section {
            display: inline-block;
            width: 200pt;
            margin: 20pt;
            padding: 20pt;
            border: 2pt solid #336699;
            border-radius: 10pt;
            text-align: center;
        }

        .nav-section h3 {
            color: #336699;
            margin-top: 0;
        }
    </style>
</head>
<body>
    <div class="nav-hub" style="page-break-after: always;">
        <h1>Annual Report 2024</h1>
        <p>Click any section to navigate</p>

        <div class="nav-section">
            <h3>Executive Summary</h3>
            <a href="#exec-summary" data-fit-to="FullPage">View Section</a>
        </div>

        <div class="nav-section">
            <h3>Financial Charts</h3>
            <a href="#financial-charts" data-fit-to="BoundingBox">View Charts</a>
        </div>

        <div class="nav-section">
            <h3>Data Tables</h3>
            <a href="#data-tables" data-fit-to="PageHeight">View Tables</a>
        </div>

        <div class="nav-section">
            <h3>Future Outlook</h3>
            <a href="#outlook" data-fit-to="PageWidth">View Section</a>
        </div>
    </div>

    <!-- Sections -->
    <div id="exec-summary" style="page-break-before: always;">
        <h1>Executive Summary</h1>
        <!-- Content -->
    </div>

    <div id="financial-charts" style="page-break-before: always;">
        <h1>Financial Charts</h1>
        <!-- Charts -->
    </div>
</body>
</html>
```

### 6. Glossary with Back References

Glossary terms that link back to definitions:

```html
<div class="content">
    <h1>Technical Documentation</h1>

    <p>
        The system uses advanced
        <a href="#term-encryption" data-fit-to="BoundingBox">encryption</a>
        to secure data. See also
        <a href="#term-authentication" data-fit-to="BoundingBox">authentication</a>.
    </p>

    <!-- More content -->
</div>

<div id="glossary" style="page-break-before: always;">
    <h1>Glossary</h1>

    <div id="term-encryption" style="border-left: 4pt solid #336699; padding: 10pt; margin: 15pt 0;">
        <h3 style="margin-top: 0; color: #336699;">Encryption</h3>
        <p>
            The process of encoding information to prevent unauthorized access.
        </p>
    </div>

    <div id="term-authentication" style="border-left: 4pt solid #336699; padding: 10pt; margin: 15pt 0;">
        <h3 style="margin-top: 0; color: #336699;">Authentication</h3>
        <p>
            The process of verifying the identity of a user or system.
        </p>
    </div>
</div>
```

### 7. Multi-Section Report with Quick Navigation

Report with navigation sidebar:

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        .page-layout {
            position: relative;
        }

        .nav-sidebar {
            position: fixed;
            left: 10pt;
            top: 100pt;
            width: 150pt;
            border: 1pt solid #ccc;
            padding: 10pt;
            background-color: #f9f9f9;
        }

        .content-main {
            margin-left: 180pt;
        }

        .nav-link {
            display: block;
            margin: 5pt 0;
            padding: 5pt;
            text-decoration: none;
            color: #336699;
        }

        .nav-link:hover {
            background-color: #e6f2ff;
        }
    </style>
</head>
<body>
    <div class="page-layout">
        <div class="nav-sidebar">
            <h3>Quick Navigation</h3>
            <a href="#overview" data-fit-to="FullPage" class="nav-link">Overview</a>
            <a href="#details" data-fit-to="PageWidth" class="nav-link">Details</a>
            <a href="#charts" data-fit-to="BoundingBox" class="nav-link">Charts</a>
            <a href="#data" data-fit-to="PageHeight" class="nav-link">Data</a>
        </div>

        <div class="content-main">
            <div id="overview">
                <h1>Overview</h1>
                <!-- Content -->
            </div>

            <div id="details" style="page-break-before: always;">
                <h1>Details</h1>
                <!-- Content -->
            </div>

            <div id="charts" style="page-break-before: always;">
                <h1>Charts</h1>
                <!-- Charts -->
            </div>

            <div id="data" style="page-break-before: always;">
                <h1>Data</h1>
                <!-- Tables -->
            </div>
        </div>
    </div>
</body>
</html>
```

### 8. Footer with Cross-References

Footer navigation with contextual fit modes:

```html
<footer style="position: fixed; bottom: 0; width: 100%; border-top: 1pt solid #ccc; padding: 10pt; background-color: #f9f9f9;">
    <div style="text-align: center;">
        <a href="#toc" data-fit-to="FullPage" style="margin: 0 10pt;">Contents</a> |
        <a href="#index" data-fit-to="PageWidth" style="margin: 0 10pt;">Index</a> |
        <a href="#references" data-fit-to="PageWidth" style="margin: 0 10pt;">References</a>
    </div>
</footer>
```

### 9. Image Gallery with Thumbnail Navigation

Gallery with links to full-size images:

```html
<div class="gallery-page">
    <h1>Image Gallery</h1>

    <!-- Thumbnails -->
    <div style="text-align: center;">
        <a href="#image1" data-fit-to="BoundingBox">
            <img src="thumb1.jpg" style="width: 100pt; height: 75pt; margin: 5pt;" />
        </a>
        <a href="#image2" data-fit-to="BoundingBox">
            <img src="thumb2.jpg" style="width: 100pt; height: 75pt; margin: 5pt;" />
        </a>
        <a href="#image3" data-fit-to="BoundingBox">
            <img src="thumb3.jpg" style="width: 100pt; height: 75pt; margin: 5pt;" />
        </a>
    </div>

    <!-- Full-size images on separate pages -->
    <div id="image1" style="page-break-before: always; text-align: center;">
        <img src="full1.jpg" style="max-width: 600pt; max-height: 800pt;" />
        <p>Image 1: Description</p>
    </div>

    <div id="image2" style="page-break-before: always; text-align: center;">
        <img src="full2.jpg" style="max-width: 600pt; max-height: 800pt;" />
        <p>Image 2: Description</p>
    </div>

    <div id="image3" style="page-break-before: always; text-align: center;">
        <img src="full3.jpg" style="max-width: 600pt; max-height: 800pt;" />
        <p>Image 3: Description</p>
    </div>
</div>
```

### 10. Legal Document with Citation Links

Legal document with precise section references:

```html
<div class="legal-document">
    <h1>Service Agreement</h1>

    <p>
        As specified in
        <a href="#section3" data-fit-to="PageWidth">Section 3</a>,
        the parties agree to the terms outlined in
        <a href="#exhibit-a" data-fit-to="BoundingBox">Exhibit A</a>.
    </p>

    <!-- Section 3 -->
    <div id="section3" style="page-break-before: always;">
        <h2>3. Terms and Conditions</h2>
        <p>Detailed terms...</p>
    </div>

    <!-- Exhibit A -->
    <div id="exhibit-a" style="page-break-before: always; border: 2pt solid #000; padding: 20pt;">
        <h2>Exhibit A: Price Schedule</h2>
        <table style="width: 100%;">
            <!-- Pricing table -->
        </table>
    </div>
</div>
```

### 11. Technical Manual with Procedural Links

Manual with step-by-step navigation:

```html
<div class="manual">
    <h1>Installation Manual</h1>

    <p>
        Begin with
        <a href="#step1" data-fit-to="PageWidth">Step 1: System Requirements</a>.
        If you encounter issues, refer to
        <a href="#troubleshooting" data-fit-to="PageWidth">Troubleshooting</a>.
        Configuration diagrams are shown in
        <a href="#diagram1" data-fit-to="BoundingBox">Figure 1</a>.
    </p>

    <!-- Steps -->
    <div id="step1" style="page-break-before: always;">
        <h2>Step 1: System Requirements</h2>
        <p>Requirements list...</p>
        <p>
            Next:
            <a href="#step2" data-fit-to="PageWidth">Step 2: Installation</a>
        </p>
    </div>

    <div id="step2" style="page-break-before: always;">
        <h2>Step 2: Installation</h2>
        <p>Installation instructions...</p>
        <p>
            Previous:
            <a href="#step1" data-fit-to="PageWidth">Step 1</a> |
            Next:
            <a href="#step3" data-fit-to="PageWidth">Step 3</a>
        </p>
    </div>

    <!-- Diagram -->
    <div id="diagram1" style="page-break-before: always; text-align: center;">
        <img src="config-diagram.png" style="width: 600pt; height: 450pt;" />
        <p><strong>Figure 1:</strong> System Configuration</p>
    </div>
</div>
```

### 12. Financial Report with Data Section Links

Report with links to detailed financial data:

```html
<div class="financial-report">
    <h1>Quarterly Financial Report</h1>

    <h2>Executive Summary</h2>
    <p>
        Revenue increased by 15% (see
        <a href="#revenue-chart" data-fit-to="BoundingBox">Revenue Chart</a>).
        Detailed breakdown available in
        <a href="#financial-statements" data-fit-to="PageHeight">Financial Statements</a>.
    </p>

    <!-- Chart -->
    <div id="revenue-chart" style="page-break-before: always; text-align: center; padding: 40pt;">
        <img src="revenue-chart.png" style="width: 600pt; height: 400pt;" />
        <p><strong>Revenue Trend:</strong> Q1-Q4 2024</p>
    </div>

    <!-- Financial statements (tall table) -->
    <div id="financial-statements" style="page-break-before: always;">
        <h2>Financial Statements</h2>
        <table style="width: 100%; border-collapse: collapse;">
            <!-- Long financial table -->
        </table>
    </div>
</div>
```

### 13. Educational Textbook with Chapter Navigation

Textbook with consistent navigation patterns:

```csharp
public class Chapter
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int Number { get; set; }
}

public class TextbookModel
{
    public List<Chapter> Chapters { get; set; }
}
```

```html
<div class="textbook">
    <div style="page-break-after: always;">
        <h1>Table of Contents</h1>
        <template data-bind="{{model.chapters}}">
            <div style="margin: 10pt 0;">
                <a href="#chapter{{.number}}" data-fit-to="FullPage"
                   style="font-size: 14pt;">
                    Chapter {{.number}}: {{.title}}
                </a>
            </div>
        </template>
    </div>

    <!-- Chapters -->
    <template data-bind="{{model.chapters}}">
        <div id="chapter{{.number}}" style="page-break-before: always;">
            <h1>Chapter {{.number}}: {{.title}}</h1>
            <!-- Chapter content -->
        </div>
    </template>
</div>
```

### 14. Product Catalog with Detail Views

Catalog with links to product details:

```html
<div class="catalog">
    <h1>Product Catalog</h1>

    <!-- Product list -->
    <div class="product-list">
        <div class="product-summary">
            <h3>Widget Pro 3000</h3>
            <p>High-performance widget for professional use.</p>
            <a href="#product-widget-3000" data-fit-to="FullPage">View Details</a> |
            <a href="#specs-widget-3000" data-fit-to="BoundingBox">View Specifications</a>
        </div>
    </div>

    <!-- Product detail page -->
    <div id="product-widget-3000" style="page-break-before: always;">
        <h2>Widget Pro 3000</h2>
        <p>Comprehensive product description...</p>
    </div>

    <!-- Specifications -->
    <div id="specs-widget-3000" style="border: 2pt solid #336699; padding: 20pt; margin: 20pt 0;">
        <h3>Technical Specifications</h3>
        <table style="width: 100%;">
            <!-- Specs table -->
        </table>
    </div>
</div>
```

### 15. Back-to-Top Links with Consistent Fit Mode

Document with return-to-top navigation:

```html
<!DOCTYPE html>
<html>
<body>
    <div id="top">
        <h1>Long Document</h1>
        <p>Table of contents...</p>
    </div>

    <div id="section1" style="page-break-before: always;">
        <h2>Section 1</h2>
        <p>Content for section 1...</p>
        <p style="text-align: right;">
            <a href="#top" data-fit-to="FullPage">↑ Back to Top</a>
        </p>
    </div>

    <div id="section2" style="page-break-before: always;">
        <h2>Section 2</h2>
        <p>Content for section 2...</p>
        <p style="text-align: right;">
            <a href="#top" data-fit-to="FullPage">↑ Back to Top</a>
        </p>
    </div>

    <div id="section3" style="page-break-before: always;">
        <h2>Section 3</h2>
        <p>Content for section 3...</p>
        <p style="text-align: right;">
            <a href="#top" data-fit-to="FullPage">↑ Back to Top</a>
        </p>
    </div>
</body>
</html>
```

---

## See Also

- [a element](/reference/htmltags/a.html) - Anchor/link element documentation
- [href attribute](/reference/htmlattributes/href.html) - Link destination attribute
- [id attribute](/reference/htmlattributes/id.html) - Element identifier for link targets
- [title attribute](/reference/htmlattributes/title.html) - Bookmark title attribute
- [PDF Links](/reference/pdf/links.html) - PDF link and navigation specification
- [PDF Destinations](/reference/pdf/destinations.html) - PDF destination types
- [Document Navigation](/reference/navigation/) - Navigation patterns and best practices
- [Internal Links](/reference/links/internal.html) - Internal document linking

---
