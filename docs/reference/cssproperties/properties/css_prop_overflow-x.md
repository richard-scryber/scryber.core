---
layout: default
title: overflow-x
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# overflow-x : Horizontal Overflow Property

The `overflow-x` property controls how content is handled when it exceeds the horizontal boundaries of its containing element. This allows independent control of horizontal overflow behavior, useful for wide tables, long text strings, and horizontally scrolling content in PDF documents.

## Usage

```css
selector {
    overflow-x: value;
}
```

The overflow-x property specifically controls horizontal overflow while leaving vertical overflow behavior unaffected. This enables fine-grained control over content clipping in specific directions.

---

## Supported Values

### visible (default)
Content is not clipped horizontally and may extend beyond the element's left and right boundaries. Overflow content is visible and may overlap adjacent elements.

### hidden
Horizontal overflow content is clipped and not visible. Content that extends beyond the element's width is cut off without any indication.

### scroll
Horizontal overflow content is clipped, and scrolling mechanisms are provided (in interactive contexts). For PDF generation, this typically means content is clipped similar to hidden.

### auto
Horizontal overflow content is clipped, and scrolling mechanisms are provided only when necessary. In PDF contexts, this typically behaves like hidden.

---

## Supported Elements

The `overflow-x` property can be applied to:
- Block elements with explicit width (`<div>`, `<section>`, `<article>`)
- Elements with defined width
- Container elements
- Tables and table cells
- Text containers
- Any element where horizontal content might exceed boundaries

---

## Notes

- The element must have an explicit width for overflow-x to take effect
- `overflow-x` affects only horizontal content overflow
- Use with `overflow-y` for independent control of both axes
- Particularly useful for wide tables that exceed page width
- Hidden horizontal overflow prevents content from extending into margins
- Long unbreakable strings (URLs, code) can be controlled with overflow-x
- In PDF generation, horizontal overflow affects how content fits within page constraints
- The shorthand `overflow` property sets both overflow-x and overflow-y simultaneously
- Horizontal clipping is immediate at the boundary without gradual fade effects
- Useful for creating fixed-width columns in multi-column layouts

---

## Data Binding

The overflow-x property works with data binding to create dynamic horizontal overflow handling based on content width, data types, and layout requirements. This enables responsive table layouts, adaptive content clipping, and conditional horizontal scrolling.

### Example 1: Dynamic horizontal overflow for tables

```html
<style>
    .table-container {
        padding: 10pt;
        border: 2pt solid #2563eb;
        background-color: #dbeafe;
    }
    .data-table {
        border-collapse: collapse;
    }
    .data-table th, .data-table td {
        padding: 8pt;
        border: 1pt solid #1f2937;
    }
</style>
<body>
    <!-- Conditionally clip wide tables based on page size -->
    <div class="table-container"
         style="width: {{pageWidth}}pt;
                overflow-x: {{clipWideContent ? 'hidden' : 'visible'}}">
        <table class="data-table" style="width: {{tableWidth}}pt">
            <tr>
                <th>Column 1</th>
                <th>Column 2</th>
                <th>Column 3</th>
                <th>{{showExtraColumns ? 'Column 4' : ''}}</th>
            </tr>
            <tr>
                <td>{{data.col1}}</td>
                <td>{{data.col2}}</td>
                <td>{{data.col3}}</td>
                <td style="display: {{showExtraColumns ? 'table-cell' : 'none'}}">{{data.col4}}</td>
            </tr>
        </table>
    </div>
</body>
```

### Example 2: Conditional overflow for long text strings

```html
<style>
    .text-display {
        padding: 10pt;
        background-color: #f3f4f6;
        border: 1pt solid #d1d5db;
        margin-bottom: 15pt;
        font-family: monospace;
        font-size: 9pt;
    }
</style>
<body>
    <!-- Apply overflow-x based on text length -->
    <div class="text-display"
         style="width: {{displayWidth}}pt;
                overflow-x: {{textLength > 100 ? 'hidden' : 'visible'}}">
        <strong>URL:</strong> {{url}}
    </div>

    <!-- Conditional clipping for different output formats -->
    <div class="text-display"
         style="width: 400pt;
                overflow-x: {{outputFormat === 'compact' ? 'hidden' : 'visible'}}">
        <strong>File Path:</strong> {{filePath}}
    </div>

    <!-- Dynamic overflow based on layout mode -->
    <div class="text-display"
         style="width: {{layoutMode === 'narrow' ? '300pt' : '500pt'}};
                overflow-x: {{layoutMode === 'narrow' ? 'hidden' : 'visible'}}">
        <strong>Command:</strong> {{commandString}}
    </div>
</body>
```

### Example 3: Responsive horizontal overflow for reports

```html
<style>
    .report-section {
        margin: 20pt;
    }
    .data-grid {
        border: 2pt solid #d1d5db;
        background-color: white;
    }
</style>
<body>
    <!-- Adapt overflow-x based on report configuration -->
    <div class="report-section">
        <h2>{{reportTitle}}</h2>
        <div class="data-grid"
             style="width: {{config.pageWidth}}pt;
                    overflow-x: {{config.allowHorizontalOverflow ? 'visible' : 'hidden'}}">
            <table style="width: {{dataColumns.length * 120}}pt">
                <thead>
                    <tr>
                        {{#each dataColumns}}
                        <th>{{this.name}}</th>
                        {{/each}}
                    </tr>
                </thead>
                <tbody>
                    {{#each dataRows}}
                    <tr>
                        {{#each this}}
                        <td>{{this}}</td>
                        {{/each}}
                    </tr>
                    {{/each}}
                </tbody>
            </table>
        </div>
    </div>
</body>
```

---

## Examples

### Example 1: Wide table with horizontal overflow

```html
<style>
    .table-container {
        width: 400pt;
        overflow-x: hidden;
        border: 2pt solid #2563eb;
        background-color: #dbeafe;
        padding: 10pt;
    }
    .wide-table {
        width: 600pt;
        border-collapse: collapse;
    }
    .wide-table th,
    .wide-table td {
        padding: 8pt;
        border: 1pt solid #1f2937;
        white-space: nowrap;
    }
    .wide-table th {
        background-color: #1f2937;
        color: white;
    }
</style>
<body>
    <h2>Table with Horizontal Clipping</h2>
    <div class="table-container">
        <table class="wide-table">
            <tr>
                <th>Column 1</th>
                <th>Column 2</th>
                <th>Column 3</th>
                <th>Column 4</th>
                <th>Column 5</th>
                <th>Column 6</th>
            </tr>
            <tr>
                <td>Data A1</td>
                <td>Data A2</td>
                <td>Data A3</td>
                <td>Data A4</td>
                <td>Data A5</td>
                <td>Data A6</td>
            </tr>
        </table>
    </div>
    <p>Columns that exceed the 400pt width are clipped.</p>
</body>
```

### Example 2: Long text strings with controlled overflow

```html
<style>
    .url-display {
        width: 300pt;
        overflow-x: hidden;
        padding: 10pt;
        background-color: #f3f4f6;
        border: 1pt solid #d1d5db;
        margin-bottom: 15pt;
        font-family: monospace;
        font-size: 9pt;
    }
    .label {
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 5pt;
    }
</style>
<body>
    <h2>URL Display with Overflow Control</h2>
    <div>
        <div class="label">Clipped URL (overflow-x: hidden):</div>
        <div class="url-display">
            https://example.com/very/long/path/that/extends/beyond/the/container/width/and/will/be/clipped
        </div>

        <div class="label">Long File Path:</div>
        <div class="url-display">
            /users/documents/projects/2025/q4/reports/financials/detailed_analysis_report_final_v3.pdf
        </div>
    </div>
</body>
```

### Example 3: Code block with horizontal overflow

```html
<style>
    .code-container {
        width: 450pt;
        overflow-x: hidden;
        background-color: #1f2937;
        color: #f3f4f6;
        padding: 15pt;
        font-family: 'Courier New', monospace;
        font-size: 9pt;
        line-height: 1.5;
        border-radius: 4pt;
    }
    .code-line {
        white-space: nowrap;
    }
</style>
<body>
    <h2>Code Snippet with Horizontal Clipping</h2>
    <div class="code-container">
        <div class="code-line">function calculateTotalRevenue(transactions, taxRate, discounts) {</div>
        <div class="code-line">    return transactions.reduce((sum, t) => sum + t.amount, 0) * (1 + taxRate) - discounts;</div>
        <div class="code-line">}</div>
    </div>
    <p style="margin-top: 10pt; font-size: 10pt; color: #6b7280;">
        Long code lines are clipped at the container boundary.
    </p>
</body>
```

### Example 4: Comparison of overflow-x values

```html
<style>
    .demo-container {
        margin-bottom: 20pt;
    }
    .demo-box {
        width: 300pt;
        padding: 10pt;
        border: 2pt solid #1f2937;
        background-color: #f9fafb;
        margin-bottom: 5pt;
    }
    .overflow-visible {
        overflow-x: visible;
    }
    .overflow-hidden {
        overflow-x: hidden;
    }
    .demo-label {
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 5pt;
    }
    .wide-content {
        width: 500pt;
        background-color: #dbeafe;
        padding: 8pt;
        border: 1pt solid #2563eb;
    }
</style>
<body>
    <h2>Overflow-X Property Comparison</h2>

    <div class="demo-container">
        <div class="demo-label">overflow-x: visible</div>
        <div class="demo-box overflow-visible">
            <div class="wide-content">
                This content is wider than the container (500pt vs 300pt) and
                will extend beyond the boundaries, potentially overlapping
                adjacent elements.
            </div>
        </div>
    </div>

    <div class="demo-container">
        <div class="demo-label">overflow-x: hidden</div>
        <div class="demo-box overflow-hidden">
            <div class="wide-content">
                This content is wider than the container (500pt vs 300pt) but
                is clipped at the boundary. The overflow is not visible.
            </div>
        </div>
    </div>
</body>
```

### Example 5: Financial report with wide data

```html
<style>
    .report-section {
        width: 500pt;
        margin: 20pt;
    }
    .data-table-wrapper {
        overflow-x: hidden;
        border: 2pt solid #d1d5db;
        background-color: white;
    }
    .financial-table {
        width: 800pt;
        border-collapse: collapse;
        font-size: 10pt;
    }
    .financial-table th {
        background-color: #1f2937;
        color: white;
        padding: 10pt;
        text-align: right;
        border: 1pt solid #374151;
    }
    .financial-table td {
        padding: 8pt;
        text-align: right;
        border: 1pt solid #d1d5db;
    }
    .row-label {
        text-align: left !important;
        font-weight: bold;
        color: #1e3a8a;
    }
</style>
<body>
    <div class="report-section">
        <h2>Quarterly Financial Summary</h2>
        <p style="font-size: 10pt; color: #6b7280; margin-bottom: 10pt;">
            Wide table content is clipped to fit within report constraints.
        </p>
        <div class="data-table-wrapper">
            <table class="financial-table">
                <thead>
                    <tr>
                        <th class="row-label">Category</th>
                        <th>Q1 2025</th>
                        <th>Q2 2025</th>
                        <th>Q3 2025</th>
                        <th>Q4 2025</th>
                        <th>Total</th>
                        <th>Variance</th>
                        <th>% Change</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="row-label">Revenue</td>
                        <td>$125,000</td>
                        <td>$138,000</td>
                        <td>$145,000</td>
                        <td>$152,000</td>
                        <td>$560,000</td>
                        <td>+$27,000</td>
                        <td>+21.6%</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
```

### Example 6: Product specifications with long values

```html
<style>
    .spec-container {
        width: 400pt;
        margin: 20pt;
        border: 2pt solid #e5e7eb;
        background-color: white;
        padding: 20pt;
    }
    .spec-row {
        display: table;
        width: 100%;
        margin-bottom: 10pt;
        font-size: 11pt;
    }
    .spec-label {
        display: table-cell;
        width: 150pt;
        font-weight: bold;
        color: #1f2937;
        vertical-align: top;
    }
    .spec-value {
        display: table-cell;
        overflow-x: hidden;
        color: #6b7280;
    }
</style>
<body>
    <div class="spec-container">
        <h2 style="margin-top: 0;">Product Specifications</h2>

        <div class="spec-row">
            <div class="spec-label">Model Number:</div>
            <div class="spec-value">PRO-WIDGET-2025-ADVANCED-EDITION-PLATINUM</div>
        </div>

        <div class="spec-row">
            <div class="spec-label">Serial:</div>
            <div class="spec-value">SN-2025-10-14-ABC123-XYZ789-MANUFACTURING-BATCH-042</div>
        </div>

        <div class="spec-row">
            <div class="spec-label">Dimensions:</div>
            <div class="spec-value">24" W x 18" D x 36" H</div>
        </div>

        <div class="spec-row">
            <div class="spec-label">Configuration:</div>
            <div class="spec-value">STANDARD-CONFIG-WITH-ADVANCED-FEATURES-AND-EXTENDED-WARRANTY-COVERAGE</div>
        </div>
    </div>
</body>
```

### Example 7: Newsletter with controlled column widths

```html
<style>
    .newsletter-column {
        width: 250pt;
        overflow-x: hidden;
        float: left;
        margin-right: 20pt;
        padding: 15pt;
        background-color: #f9fafb;
        border: 2pt solid #e5e7eb;
    }
    .column-title {
        font-size: 16pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
        white-space: nowrap;
    }
    .column-content {
        font-size: 10pt;
        line-height: 1.6;
    }
    .reference-link {
        font-family: monospace;
        font-size: 8pt;
        color: #2563eb;
        background-color: #dbeafe;
        padding: 3pt;
        display: block;
        margin-top: 10pt;
    }
</style>
<body>
    <h1 style="text-align: center;">Tech Newsletter</h1>
    <div style="overflow: auto;">
        <div class="newsletter-column">
            <div class="column-title">Industry News and Updates Today</div>
            <div class="column-content">
                Latest developments in PDF generation technology and document
                automation systems.
            </div>
            <div class="reference-link">
                https://example.com/very/long/url/that/will/be/clipped/in/display
            </div>
        </div>
        <div class="newsletter-column">
            <div class="column-title">Tutorial: Advanced Layout Techniques</div>
            <div class="column-content">
                Learn how to control overflow behavior for professional
                document layouts.
            </div>
        </div>
    </div>
</body>
```

### Example 8: Data export summary with overflow protection

```html
<style>
    .export-summary {
        width: 450pt;
        margin: 30pt auto;
        border: 3pt solid #1f2937;
        background-color: white;
    }
    .summary-header {
        background-color: #1f2937;
        color: white;
        padding: 15pt;
        font-size: 16pt;
        font-weight: bold;
    }
    .summary-body {
        padding: 20pt;
    }
    .summary-item {
        margin-bottom: 15pt;
        padding-bottom: 15pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    .item-label {
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 5pt;
    }
    .item-value {
        overflow-x: hidden;
        font-family: monospace;
        font-size: 9pt;
        padding: 8pt;
        background-color: #f3f4f6;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="export-summary">
        <div class="summary-header">Data Export Summary</div>
        <div class="summary-body">
            <div class="summary-item">
                <div class="item-label">Export File Path:</div>
                <div class="item-value">
                    /var/data/exports/2025/october/financial_reports/quarterly_analysis_detailed_version_final_approved.xlsx
                </div>
            </div>

            <div class="summary-item">
                <div class="item-label">Generated Hash:</div>
                <div class="item-value">
                    SHA256:a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0u1v2w3x4y5z6a7b8c9d0e1f2
                </div>
            </div>

            <div class="summary-item">
                <div class="item-label">Records Exported:</div>
                <div class="item-value">
                    12,847 records
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 9: Certificate with long names

```html
<style>
    .certificate-container {
        width: 600pt;
        margin: 40pt auto;
        padding: 40pt;
        border: 8pt double #1e3a8a;
        background-color: #fffef7;
    }
    .cert-title {
        text-align: center;
        font-size: 32pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 30pt;
    }
    .cert-content {
        text-align: center;
        font-size: 14pt;
        line-height: 2;
    }
    .recipient-line {
        overflow-x: hidden;
        font-size: 24pt;
        font-weight: bold;
        margin: 20pt 0;
        padding: 10pt;
        border-bottom: 2pt solid #1f2937;
    }
    .organization-line {
        overflow-x: hidden;
        font-size: 16pt;
        font-style: italic;
        color: #6b7280;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="certificate-container">
        <div class="cert-title">Certificate of Completion</div>
        <div class="cert-content">
            <div>This certifies that</div>
            <div class="recipient-line">
                Dr. Elizabeth Alexandra Montgomery-Williams III, PhD, MBA
            </div>
            <div>has successfully completed</div>
            <div class="organization-line">
                The Advanced Professional Development Program in Enterprise
                Document Generation and Management Systems
            </div>
            <div style="margin-top: 30pt;">October 14, 2025</div>
        </div>
    </div>
</body>
```

### Example 10: Technical documentation with command examples

```html
<style>
    .doc-section {
        width: 500pt;
        margin: 20pt;
        padding: 20pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .section-title {
        font-size: 18pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 15pt;
    }
    .command-block {
        overflow-x: hidden;
        background-color: #1f2937;
        color: #10b981;
        font-family: 'Courier New', monospace;
        font-size: 10pt;
        padding: 12pt;
        margin: 10pt 0;
        border-radius: 4pt;
    }
    .command-prompt {
        color: #fbbf24;
    }
    .description {
        font-size: 11pt;
        line-height: 1.6;
        color: #4b5563;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="doc-section">
        <div class="section-title">Installation Commands</div>

        <div class="description">
            Install the package using the following command:
        </div>
        <div class="command-block">
            <span class="command-prompt">$</span> npm install @scryber/pdf-generator --save-dev --legacy-peer-deps --verbose --no-optional
        </div>

        <div class="description">
            Configure the environment variables:
        </div>
        <div class="command-block">
            <span class="command-prompt">$</span> export PDF_OUTPUT_PATH=/var/application/documents/generated/pdfs/output/production/
        </div>

        <div class="description">
            Run the generation script:
        </div>
        <div class="command-block">
            <span class="command-prompt">$</span> node scripts/generate-report.js --input=data/q4-financials.json --output=reports/ --format=pdf --template=corporate
        </div>
    </div>
</body>
```

---

## See Also

- [overflow](/reference/cssproperties/css_prop_overflow) - Control both horizontal and vertical overflow
- [overflow-y](/reference/cssproperties/css_prop_overflow-y) - Control vertical overflow behavior
- [width](/reference/cssproperties/css_prop_width) - Set element width
- [max-width](/reference/cssproperties/css_prop_max-width) - Set maximum width constraint
- [white-space](/reference/cssproperties/css_prop_white-space) - Control text wrapping behavior
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
