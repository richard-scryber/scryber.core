---
layout: default
title: size
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# size : Page Size Property (for @page rule)

The `size` property is used within the `@page` CSS at-rule to specify the size and orientation of pages in generated PDF documents. This property allows you to set standard paper sizes (like A4, Letter, Legal) or define custom dimensions. It's essential for controlling the physical dimensions of your PDF output.

## Usage

```css
@page {
    size: value;
}
```

The size property is used exclusively within `@page` rules to define page dimensions. It can accept standard paper size keywords, orientation keywords, or explicit width and height values.

---

## Supported Values

### Standard Paper Sizes

#### A4
ISO standard paper size: 210mm × 297mm (8.27" × 11.69"). Most commonly used internationally.

#### A3
ISO standard paper size: 297mm × 420mm (11.69" × 16.54"). Double the size of A4.

#### A5
ISO standard paper size: 148mm × 210mm (5.83" × 8.27"). Half the size of A4.

#### Letter
US standard paper size: 8.5" × 11" (215.9mm × 279.4mm). Most commonly used in North America.

#### Legal
US legal paper size: 8.5" × 14" (215.9mm × 355.6mm). Used for legal documents.

### Orientation Keywords

#### portrait
Sets the page orientation so that the height is greater than the width. This is the default orientation for most documents.

#### landscape
Sets the page orientation so that the width is greater than the height. Rotates standard sizes by 90 degrees.

### Custom Dimensions

You can specify custom page dimensions using length units:
- `size: 200mm 300mm;` - Width 200mm, Height 300mm
- `size: 8in 10in;` - Width 8 inches, Height 10 inches
- `size: 600pt 800pt;` - Width 600 points, Height 800 points

Supported units: `mm`, `cm`, `in`, `pt`, `px`

### Combined Values

You can combine standard sizes with orientation:
- `size: A4 portrait;` - A4 in portrait orientation
- `size: A4 landscape;` - A4 in landscape orientation
- `size: Letter landscape;` - US Letter in landscape

---

## Supported Elements

The `size` property is only used within `@page` at-rules and cannot be applied directly to HTML elements. It affects the entire page context for PDF generation.

---

## Notes

- The `size` property only works within `@page` at-rules
- Default page size is typically A4 portrait if not specified
- Portrait orientation is assumed unless landscape is specified
- Custom dimensions require both width and height values
- Using standard sizes ensures consistency across different PDF viewers
- Margins are separate from page size and defined using margin properties within `@page`
- Different page sizes can be defined for named pages using `@page :name` syntax
- Landscape orientation rotates the page 90 degrees clockwise
- Units should be appropriate for print: `mm`, `cm`, `in`, or `pt` are recommended
- The `size` property affects the physical dimensions of generated PDF pages
- Some paper sizes (like A4) are more common internationally, while others (like Letter) are region-specific

---

## Data Binding

The `size` property within `@page` rules supports data binding, allowing you to dynamically set page dimensions based on configuration, document type, or user preferences. This enables flexible document generation with variable page sizes and orientations.

### Example 1: Configuration-driven page sizes

```html
<style>
    @page {
        size: {{config.paperSize || 'A4'}} {{config.orientation || 'portrait'}};
        margin: {{config.margins || '25mm'}};
    }
    body {
        font-family: {{config.fontFamily || 'Arial, sans-serif'}};
        font-size: {{config.fontSize || 11}}pt;
    }
    .document-header {
        font-size: {{config.headerSize || 18}}pt;
        color: {{config.primaryColor || '#1e3a8a'}};
        margin-bottom: 20pt;
    }
</style>
<body>
    <h1 class="document-header">{{documentTitle}}</h1>
    <p>This document uses {{config.paperSize}} paper in {{config.orientation}} orientation.</p>
</body>
```

### Example 2: Document type-specific page layouts

```html
<style>
    @page report-page {
        size: {{reportType === 'executive' ? 'Letter' : 'A4'}} portrait;
        margin: {{reportType === 'executive' ? '1in' : '25mm'}};
    }
    @page chart-page {
        size: {{reportType === 'executive' ? 'Letter' : 'A4'}} landscape;
        margin: {{reportType === 'executive' ? '0.5in' : '15mm'}};
    }

    .report-content {
        page: report-page;
    }
    .chart-section {
        page: chart-page;
    }
</style>
<body>
    <div class="report-content">
        <h1>{{reportTitle}}</h1>
        <p>Standard report content...</p>
    </div>

    <div class="chart-section">
        <h2>Data Visualizations</h2>
        <p>Charts in landscape format...</p>
    </div>
</body>
```

### Example 3: Custom page dimensions from data

```html
<style>
    @page custom-size {
        size: {{pageWidth}}mm {{pageHeight}}mm;
        margin: {{pageMarginTop}}mm {{pageMarginRight}}mm {{pageMarginBottom}}mm {{pageMarginLeft}}mm;
    }
    @page standard {
        size: A4 portrait;
        margin: 25mm;
    }

    .custom-section {
        page: custom-size;
    }
    .standard-section {
        page: standard;
    }
    .page-info {
        font-size: 10pt;
        color: #6b7280;
        margin-bottom: 20pt;
    }
</style>
<body>
    {{#if useCustomSize}}
    <div class="custom-section">
        <div class="page-info">Custom page size: {{pageWidth}}mm × {{pageHeight}}mm</div>
        <h1>{{customSectionTitle}}</h1>
        <p>{{customSectionContent}}</p>
    </div>
    {{/if}}

    <div class="standard-section">
        <h1>{{standardSectionTitle}}</h1>
        <p>{{standardSectionContent}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Standard A4 portrait document

```html
<style>
    @page {
        size: A4;
        margin: 20mm;
    }
    body {
        font-family: Arial, sans-serif;
        font-size: 11pt;
        line-height: 1.6;
    }
    h1 {
        font-size: 18pt;
        color: #1e3a8a;
        margin-bottom: 15pt;
    }
</style>
<body>
    <h1>Business Report</h1>
    <p>This document is formatted for standard A4 paper size (210mm × 297mm)
    in portrait orientation, which is the international standard for business
    documents.</p>
</body>
```

### Example 2: US Letter landscape for charts

```html
<style>
    @page {
        size: Letter landscape;
        margin: 15mm;
    }
    .chart-container {
        width: 100%;
        padding: 20pt;
    }
    .chart-title {
        font-size: 20pt;
        font-weight: bold;
        text-align: center;
        margin-bottom: 20pt;
    }
    .chart-placeholder {
        width: 100%;
        height: 400pt;
        background-color: #f3f4f6;
        border: 2pt solid #9ca3af;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 18pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="chart-container">
        <h1 class="chart-title">Annual Revenue Growth 2020-2025</h1>
        <div class="chart-placeholder">
            [Wide Chart Display - Optimized for Landscape]
        </div>
    </div>
</body>
```

### Example 3: A5 booklet pages

```html
<style>
    @page {
        size: A5 portrait;
        margin: 15mm 20mm;
    }
    body {
        font-family: 'Times New Roman', serif;
        font-size: 10pt;
        line-height: 1.8;
    }
    .chapter-title {
        font-size: 16pt;
        font-weight: bold;
        margin: 20pt 0 15pt 0;
        text-align: center;
    }
    .body-text {
        text-align: justify;
        hyphens: auto;
    }
</style>
<body>
    <h1 class="chapter-title">Chapter 1: Introduction</h1>
    <div class="body-text">
        <p>This booklet uses the compact A5 format (148mm × 210mm), which is
        half the size of A4 and perfect for pocket guides, handbooks, and
        small publications.</p>
    </div>
</body>
```

### Example 4: Legal size document

```html
<style>
    @page {
        size: Legal;
        margin: 1in;
    }
    .legal-header {
        text-align: center;
        font-weight: bold;
        font-size: 14pt;
        margin-bottom: 30pt;
        text-transform: uppercase;
    }
    .contract-text {
        font-family: 'Times New Roman', serif;
        font-size: 12pt;
        line-height: 2;
        text-align: justify;
    }
    .clause {
        margin: 20pt 0;
        padding-left: 30pt;
    }
</style>
<body>
    <div class="legal-header">
        Service Agreement
    </div>
    <div class="contract-text">
        <div class="clause">
            <strong>1. PARTIES.</strong> This agreement is made between...
        </div>
        <div class="clause">
            <strong>2. SERVICES.</strong> The provider agrees to furnish...
        </div>
    </div>
</body>
```

### Example 5: Custom size for invitation

```html
<style>
    @page {
        size: 150mm 200mm;
        margin: 0;
    }
    .invitation-container {
        width: 100%;
        height: 100vh;
        background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        text-align: center;
        padding: 30mm;
    }
    .invitation-ornament {
        font-size: 48pt;
        color: #f59e0b;
        margin-bottom: 20pt;
    }
    .invitation-title {
        font-size: 28pt;
        font-weight: bold;
        color: #92400e;
        margin: 15pt 0;
        font-family: serif;
    }
    .invitation-details {
        font-size: 14pt;
        color: #78350f;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="invitation-container">
        <div class="invitation-ornament">✦</div>
        <div class="invitation-title">You Are Invited</div>
        <div class="invitation-details">
            Annual Gala Dinner<br/>
            Saturday, April 15, 2025<br/>
            7:00 PM
        </div>
    </div>
</body>
```

### Example 6: A3 poster landscape

```html
<style>
    @page {
        size: A3 landscape;
        margin: 20mm;
    }
    .poster {
        width: 100%;
        height: 100vh;
        background-color: #1e3a8a;
        color: white;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        text-align: center;
    }
    .poster-headline {
        font-size: 64pt;
        font-weight: bold;
        margin-bottom: 30pt;
        text-shadow: 3pt 3pt 6pt rgba(0,0,0,0.3);
    }
    .poster-subheadline {
        font-size: 32pt;
        margin-bottom: 40pt;
    }
    .poster-details {
        font-size: 20pt;
        line-height: 1.8;
    }
</style>
<body>
    <div class="poster">
        <h1 class="poster-headline">Technology Summit 2025</h1>
        <p class="poster-subheadline">Innovation • Collaboration • Future</p>
        <div class="poster-details">
            March 25-27, 2025<br/>
            Convention Center<br/>
            Register at www.summit2025.com
        </div>
    </div>
</body>
```

### Example 7: Named pages with different sizes

```html
<style>
    @page cover {
        size: A4 landscape;
        margin: 0;
    }
    @page content {
        size: A4 portrait;
        margin: 25mm;
    }
    .cover-page {
        page: cover;
        width: 100%;
        height: 100vh;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }
    .content-page {
        page: content;
    }
    .cover-title {
        font-size: 52pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="cover-page">
        <h1 class="cover-title">Annual Report 2025</h1>
    </div>

    <div class="content-page">
        <h2>Executive Summary</h2>
        <p>Content pages use portrait orientation...</p>
    </div>
</body>
```

### Example 8: Square format for photo book

```html
<style>
    @page {
        size: 210mm 210mm;
        margin: 10mm;
    }
    .photo-page {
        display: flex;
        flex-direction: column;
        gap: 10pt;
    }
    .photo-frame {
        width: 100%;
        height: 180mm;
        background-color: #e5e7eb;
        border: 3pt solid white;
        box-shadow: 0 2pt 8pt rgba(0,0,0,0.2);
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .photo-caption {
        text-align: center;
        font-size: 12pt;
        font-style: italic;
        color: #6b7280;
    }
</style>
<body>
    <div class="photo-page">
        <div class="photo-frame">
            [Photo: Sunset at the Beach]
        </div>
        <div class="photo-caption">
            Summer Vacation 2025 - Mediterranean Coast
        </div>
    </div>
</body>
```

### Example 9: Business card size

```html
<style>
    @page {
        size: 90mm 50mm;
        margin: 0;
    }
    .business-card {
        width: 100%;
        height: 100%;
        background: linear-gradient(135deg, #1e3a8a 0%, #3b82f6 100%);
        color: white;
        padding: 5mm;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
    }
    .card-name {
        font-size: 16pt;
        font-weight: bold;
    }
    .card-title {
        font-size: 10pt;
        margin-top: 2mm;
    }
    .card-contact {
        font-size: 8pt;
        line-height: 1.6;
    }
</style>
<body>
    <div class="business-card">
        <div>
            <div class="card-name">John Smith</div>
            <div class="card-title">Senior Consultant</div>
        </div>
        <div class="card-contact">
            john.smith@company.com<br/>
            +1 (555) 123-4567<br/>
            www.company.com
        </div>
    </div>
</body>
```

### Example 10: Large format technical drawing

```html
<style>
    @page {
        size: 420mm 594mm;  /* A2 size */
        margin: 10mm;
    }
    .drawing-sheet {
        width: 100%;
        height: 100%;
        border: 2pt solid black;
        position: relative;
    }
    .title-block {
        position: absolute;
        bottom: 0;
        right: 0;
        width: 200mm;
        background-color: #f9fafb;
        border: 1pt solid black;
        padding: 10mm;
    }
    .drawing-title {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 5mm;
    }
    .drawing-info {
        font-size: 10pt;
        line-height: 1.6;
    }
</style>
<body>
    <div class="drawing-sheet">
        <div class="title-block">
            <div class="drawing-title">Building Floor Plan</div>
            <div class="drawing-info">
                Project: Office Complex<br/>
                Scale: 1:100<br/>
                Date: March 2025<br/>
                Drawing No: A-101
            </div>
        </div>
    </div>
</body>
```

### Example 11: Envelope size

```html
<style>
    @page {
        size: 220mm 110mm;  /* DL envelope */
        margin: 0;
    }
    .envelope {
        width: 100%;
        height: 100%;
        padding: 20mm;
        background-color: white;
        position: relative;
    }
    .return-address {
        font-size: 9pt;
        line-height: 1.4;
    }
    .recipient-address {
        position: absolute;
        top: 50mm;
        left: 80mm;
        font-size: 11pt;
        line-height: 1.6;
    }
</style>
<body>
    <div class="envelope">
        <div class="return-address">
            Your Company Name<br/>
            123 Business Street<br/>
            City, State 12345
        </div>
        <div class="recipient-address">
            <strong>Mr. John Doe</strong><br/>
            456 Customer Avenue<br/>
            Town, State 67890
        </div>
    </div>
</body>
```

### Example 12: Presentation slides

```html
<style>
    @page {
        size: 254mm 190.5mm;  /* 10" x 7.5" - 4:3 aspect ratio */
        margin: 20mm;
    }
    .slide {
        width: 100%;
        height: 100%;
        background: linear-gradient(135deg, #1e3a8a 0%, #3b82f6 100%);
        color: white;
        padding: 30mm;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }
    .slide-title {
        font-size: 36pt;
        font-weight: bold;
        margin-bottom: 20pt;
    }
    .slide-content {
        font-size: 18pt;
        line-height: 1.8;
    }
    .slide-bullet {
        margin: 12pt 0;
        padding-left: 20pt;
    }
</style>
<body>
    <div class="slide">
        <h1 class="slide-title">Key Benefits</h1>
        <div class="slide-content">
            <div class="slide-bullet">• Increased Efficiency</div>
            <div class="slide-bullet">• Cost Reduction</div>
            <div class="slide-bullet">• Improved Quality</div>
            <div class="slide-bullet">• Enhanced Scalability</div>
        </div>
    </div>
</body>
```

### Example 13: Mixed orientation document

```html
<style>
    @page portrait {
        size: A4 portrait;
        margin: 25mm;
    }
    @page landscape {
        size: A4 landscape;
        margin: 20mm;
    }
    .portrait-content {
        page: portrait;
    }
    .landscape-content {
        page: landscape;
    }
    .wide-table {
        width: 100%;
        border-collapse: collapse;
    }
    .wide-table th,
    .wide-table td {
        border: 1pt solid #d1d5db;
        padding: 8pt;
        font-size: 10pt;
    }
</style>
<body>
    <div class="portrait-content">
        <h1>Project Report</h1>
        <p>This section uses portrait orientation for standard content...</p>
    </div>

    <div class="landscape-content">
        <h2>Detailed Data Table</h2>
        <table class="wide-table">
            <tr>
                <th>Month</th>
                <th>Region A</th>
                <th>Region B</th>
                <th>Region C</th>
                <th>Region D</th>
                <th>Region E</th>
                <th>Total</th>
            </tr>
            <tr>
                <td>January</td>
                <td>$45,000</td>
                <td>$38,000</td>
                <td>$52,000</td>
                <td>$41,000</td>
                <td>$49,000</td>
                <td>$225,000</td>
            </tr>
        </table>
    </div>

    <div class="portrait-content">
        <h2>Analysis</h2>
        <p>Back to portrait for detailed analysis...</p>
    </div>
</body>
```

### Example 14: Tabloid size for architectural plans

```html
<style>
    @page {
        size: 11in 17in;  /* Tabloid/Ledger size */
        margin: 0.5in;
    }
    .plan-sheet {
        width: 100%;
        height: 100%;
    }
    .plan-header {
        background-color: #1f2937;
        color: white;
        padding: 10pt;
        margin-bottom: 15pt;
    }
    .project-title {
        font-size: 20pt;
        font-weight: bold;
    }
    .drawing-area {
        width: 100%;
        height: 85%;
        border: 2pt solid #374151;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="plan-sheet">
        <div class="plan-header">
            <div class="project-title">Residential Building - Ground Floor Plan</div>
            <div>Scale: 1:50 | Date: March 2025 | Sheet: A-001</div>
        </div>
        <div class="drawing-area">
            <!-- Architectural drawing content -->
        </div>
    </div>
</body>
```

### Example 15: Ticket size with custom dimensions

```html
<style>
    @page {
        size: 180mm 65mm;
        margin: 0;
    }
    .ticket {
        width: 100%;
        height: 100%;
        background: linear-gradient(90deg, #7c3aed 0%, #a855f7 100%);
        color: white;
        display: flex;
        position: relative;
    }
    .ticket-main {
        flex: 1;
        padding: 10mm;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
    }
    .ticket-stub {
        width: 40mm;
        background-color: #6b21a8;
        padding: 10mm;
        border-left: 2pt dashed white;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        text-align: center;
    }
    .event-name {
        font-size: 20pt;
        font-weight: bold;
    }
    .event-details {
        font-size: 12pt;
        line-height: 1.6;
    }
    .ticket-number {
        font-size: 14pt;
        font-weight: bold;
        margin-top: 10mm;
    }
</style>
<body>
    <div class="ticket">
        <div class="ticket-main">
            <div>
                <div class="event-name">Summer Music Festival 2025</div>
                <div class="event-details">
                    Saturday, July 15, 2025<br/>
                    Gates Open: 6:00 PM<br/>
                    Central Park Amphitheater
                </div>
            </div>
            <div>
                <strong>General Admission</strong><br/>
                Ticket #: 12345678
            </div>
        </div>
        <div class="ticket-stub">
            <div>
                <strong>Summer<br/>Festival</strong><br/>
                July 15<br/>
                2025
            </div>
            <div class="ticket-number">#12345678</div>
        </div>
    </div>
</body>
```

---

## See Also

- [@page rule](/reference/css_atrules/css_atrule_page) - Define page properties and margins
- [page](/reference/cssproperties/css_prop_page) - Specify named page for element
- [break-before](/reference/cssproperties/css_prop_break-before) - Control page breaks before elements
- [break-after](/reference/cssproperties/css_prop_break-after) - Control page breaks after elements
- [margin (in @page)](/reference/css_atrules/css_atrule_page#margins) - Set page margins

---
