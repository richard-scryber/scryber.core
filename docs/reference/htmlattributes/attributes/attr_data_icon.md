---
layout: default
title: data-icon
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-icon : The Attachment Display Icon Attribute

The `data-icon` attribute controls the visual icon displayed for file attachments in PDF documents. When used with the `<object>` element, it specifies which icon type appears in the document at the attachment's location, providing visual indicators for embedded files accessible through PDF viewers.

## Usage

The `data-icon` attribute specifies attachment visualization:
- Controls icon appearance for `<object>` element attachments
- Displays clickable icons in the PDF document layout
- Supports five icon types: None, Pushpin, Paperclip, Tag, Graph
- Maps to the `DisplayIcon` property on IconAttachment component
- Default value is `None` (no icon displayed)
- Icons are standard PDF attachment annotation icons
- Can be styled with CSS for positioning and spacing
- Works independently of the attachment file type

```html
<!-- No icon (default) -->
<object data="document.pdf" type="application/pdf"></object>

<!-- Pushpin icon -->
<object data="important.pdf" type="application/pdf" data-icon="Pushpin"></object>

<!-- Paperclip icon -->
<object data="contract.pdf" type="application/pdf" data-icon="Paperclip"></object>

<!-- Tag icon -->
<object data="specs.pdf" type="application/pdf" data-icon="Tag"></object>

<!-- Graph icon -->
<object data="data.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        data-icon="Graph"></object>
```

---

## Supported Elements

The `data-icon` attribute is used exclusively with:

### Object Element
- `<object>` - Creates file attachments with optional display icons (primary use)

---

## Binding Values

The `data-icon` attribute supports data binding for dynamic icon selection:

```html
<!-- Simple dynamic icon -->
<object data="{{model.filePath}}"
        type="{{model.fileType}}"
        data-icon="{{model.iconType}}"></object>

<!-- Conditional icon based on importance -->
<object data="{{model.documentPath}}"
        type="application/pdf"
        data-icon="{{model.isImportant ? 'Pushpin' : 'Paperclip'}}"></object>

<!-- Icon based on file type -->
<object data="{{model.filePath}}"
        type="{{model.mimeType}}"
        data-icon="{{model.isSpreadsheet ? 'Graph' : 'Paperclip'}}"></object>

<!-- Multiple attachments with varying icons -->
<template data-bind="{{model.attachments}}">
    <object data="{{.path}}"
            type="{{.type}}"
            data-icon="{{.icon}}"
            alt="{{.description}}"></object>
</template>
```

**Data Model Example:**
```json
{
  "filePath": "report.pdf",
  "fileType": "application/pdf",
  "iconType": "Paperclip",
  "documentPath": "notice.pdf",
  "isImportant": true,
  "isSpreadsheet": false,
  "mimeType": "application/pdf",
  "attachments": [
    {
      "path": "document.pdf",
      "type": "application/pdf",
      "icon": "Paperclip",
      "description": "Main Document"
    },
    {
      "path": "data.xlsx",
      "type": "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      "icon": "Graph",
      "description": "Supporting Data"
    }
  ]
}
```

---

## Notes

### Available Icon Types

The `data-icon` attribute accepts the following AttachmentDisplayIcon enumeration values:

| Icon Value | Visual Appearance | Common Use Cases |
|-----------|-------------------|------------------|
| `None` | No icon displayed (default) | Background attachments, hidden files |
| `Pushpin` | Pushpin/thumbtack icon | Important notices, priority documents |
| `Paperclip` | Paperclip icon | General attachments, standard documents |
| `Tag` | Tag/label icon | Categorized files, reference materials |
| `Graph` | Graph/chart icon | Data files, spreadsheets, analytical documents |

```html
<!-- None - No visual indicator -->
<object data="background-data.pdf" type="application/pdf" data-icon="None"></object>

<!-- Pushpin - Important/priority -->
<object data="urgent-notice.pdf" type="application/pdf" data-icon="Pushpin"></object>

<!-- Paperclip - Standard attachment -->
<object data="report.pdf" type="application/pdf" data-icon="Paperclip"></object>

<!-- Tag - Categorized/reference -->
<object data="reference.pdf" type="application/pdf" data-icon="Tag"></object>

<!-- Graph - Data/analytics -->
<object data="analysis.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        data-icon="Graph"></object>
```

### Icon Behavior in PDF Viewers

When an icon is specified:

1. **Visual Indicator**: An icon appears at the object's location in the document
2. **Clickable**: Users can click the icon to access attachment options
3. **Standard Icons**: Icons are standard PDF attachment annotations recognized by all PDF viewers
4. **Viewer-Specific Rendering**: Exact icon appearance may vary slightly between PDF viewers
5. **Attachment Panel**: Attachments also appear in the viewer's attachment panel/list
6. **Accessibility**: Icons provide visual cues about attachment presence

### None vs Omitting data-icon

Both approaches hide the icon, but have subtle differences:

```html
<!-- Explicit None -->
<object data="document.pdf" type="application/pdf" data-icon="None"></object>

<!-- Omitting data-icon (defaults to None) -->
<object data="document.pdf" type="application/pdf"></object>

<!-- Both result in: No icon displayed, but attachment still embedded -->
```

**When to use None explicitly**:
- For clarity in code
- When dynamically switching between icon and no-icon
- When overriding default styles

### Icon Selection Guidelines

Choose icons based on attachment purpose:

**Pushpin** - Use for:
- Critical documents requiring immediate attention
- Important notices and alerts
- High-priority attachments
- Documents that should not be overlooked

```html
<object data="critical-update.pdf"
        type="application/pdf"
        data-icon="Pushpin"
        alt="Critical System Update"></object>
```

**Paperclip** - Use for:
- General document attachments
- Standard reference materials
- Common file attachments
- Default attachment indicator

```html
<object data="contract.pdf"
        type="application/pdf"
        data-icon="Paperclip"
        alt="Service Contract"></object>
```

**Tag** - Use for:
- Categorized documents
- Specification sheets
- Reference materials
- Supplementary files
- Source files and originals

```html
<object data="specifications.pdf"
        type="application/pdf"
        data-icon="Tag"
        alt="Technical Specifications"></object>
```

**Graph** - Use for:
- Spreadsheets and data files
- Charts and analytical documents
- Financial reports
- Statistical data
- CSV/JSON data files

```html
<object data="quarterly-data.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        data-icon="Graph"
        alt="Q4 Financial Data"></object>
```

**None** - Use for:
- Background data files
- Files that shouldn't distract from main content
- Attachments mentioned in text but not needing visual indicator
- Programmatic file embedding

```html
<object data="metadata.json"
        type="application/json"
        data-icon="None"
        alt="Document Metadata"></object>
```

### Icon Positioning and Styling

Icons can be positioned and styled using CSS:

```html
<!-- Inline icon with text -->
<div>
    <object data="report.pdf"
            type="application/pdf"
            data-icon="Paperclip"
            style="vertical-align: middle; margin-right: 5pt;"></object>
    <span>Annual Report (click paperclip to open)</span>
</div>

<!-- Floating icon -->
<object data="reference.pdf"
        type="application/pdf"
        data-icon="Pushpin"
        style="float: right; margin: 0 0 10pt 15pt;"></object>

<!-- Centered icon -->
<div style="text-align: center;">
    <object data="data.xlsx"
            type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            data-icon="Graph"></object>
</div>

<!-- Positioned icon -->
<object data="watermark.pdf"
        type="application/pdf"
        data-icon="Tag"
        style="position: absolute; top: 20pt; right: 20pt;"></object>
```

### Icon Size and Appearance

Icon size and appearance are controlled by the PDF viewer:
- Icons have standard sizes defined by PDF specifications
- Exact rendering varies by PDF viewer application
- Cannot be resized via CSS (they are PDF annotations, not images)
- Color and style are viewer-dependent

### Multiple Attachments with Different Icons

Use different icons to distinguish attachment types:

```html
<div class="attachments">
    <h3>Project Files</h3>

    <!-- Critical document - Pushpin -->
    <div>
        <object data="project-brief.pdf"
                type="application/pdf"
                data-icon="Pushpin"></object>
        <strong>Project Brief</strong> (Priority)
    </div>

    <!-- Standard documents - Paperclip -->
    <div>
        <object data="requirements.pdf"
                type="application/pdf"
                data-icon="Paperclip"></object>
        Requirements Document
    </div>

    <!-- Reference materials - Tag -->
    <div>
        <object data="specifications.pdf"
                type="application/pdf"
                data-icon="Tag"></object>
        Technical Specifications
    </div>

    <!-- Data files - Graph -->
    <div>
        <object data="budget.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph"></object>
        Budget Spreadsheet
    </div>
</div>
```

### Case Sensitivity

Icon values are case-sensitive:

```html
<!-- Correct -->
<object data="file.pdf" type="application/pdf" data-icon="Paperclip"></object>

<!-- Incorrect (will not work) -->
<object data="file.pdf" type="application/pdf" data-icon="paperclip"></object>
<object data="file.pdf" type="application/pdf" data-icon="PAPERCLIP"></object>
```

Valid values: `None`, `Pushpin`, `Paperclip`, `Tag`, `Graph` (exact case)

### Fallback Content with Icons

Combine icons with fallback content for clear communication:

```html
<object data="document.pdf"
        type="application/pdf"
        data-icon="Paperclip"
        alt="Important Document">
    <div style="padding: 10pt; border-left: 3pt solid #336699; background-color: #f0f8ff;">
        <strong>Attached:</strong> Important Document
        <p style="margin: 5pt 0 0 0; font-size: 9pt;">
            Click the paperclip icon to open the attached PDF file.
        </p>
    </div>
</object>
```

### Dynamic Icon Selection

Select icons based on business logic:

```html
<!-- Model: { attachment: { path: "file.pdf", priority: "high", category: "data" } } -->

<object data="{{model.attachment.path}}"
        type="application/pdf"
        data-icon="{{model.attachment.priority == 'high' ? 'Pushpin' :
                      model.attachment.category == 'data' ? 'Graph' :
                      'Paperclip'}}"></object>
```

### Accessibility Considerations

Icons are visual indicators; always provide alt text:

```html
<!-- Good: Icon with descriptive alt text -->
<object data="report.pdf"
        type="application/pdf"
        data-icon="Paperclip"
        alt="Annual Financial Report"></object>

<!-- Better: Icon with alt text and surrounding context -->
<div>
    <object data="report.pdf"
            type="application/pdf"
            data-icon="Paperclip"
            alt="Annual Financial Report"
            style="vertical-align: middle; margin-right: 5pt;"></object>
    <span>Annual Financial Report - Click icon to open</span>
</div>
```

### Icon in Document Structure

Icons appear at the object element's location:

```html
<!-- Icon in paragraph -->
<p>
    Please review the attached document
    <object data="review.pdf"
            type="application/pdf"
            data-icon="Pushpin"></object>
    before the meeting.
</p>

<!-- Icon in table cell -->
<table>
    <tr>
        <td>Document Name</td>
        <td style="text-align: center;">
            <object data="document.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"></object>
        </td>
    </tr>
</table>

<!-- Icon in header -->
<header>
    <h1>Report Title</h1>
    <object data="full-report.pdf"
            type="application/pdf"
            data-icon="Paperclip"
            style="float: right;"></object>
</header>
```

---

## Examples

### Basic Icon Usage

```html
<!-- Simple paperclip icon -->
<object data="document.pdf"
        type="application/pdf"
        data-icon="Paperclip"></object>

<!-- Important document with pushpin -->
<object data="urgent.pdf"
        type="application/pdf"
        data-icon="Pushpin"
        alt="Urgent Notice"></object>

<!-- Data file with graph icon -->
<object data="sales-data.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        data-icon="Graph"
        alt="Sales Data"></object>
```

### All Icon Types Demonstrated

```html
<div style="padding: 20pt;">
    <h2>Attachment Icon Types</h2>

    <div style="margin: 15pt 0;">
        <object data="files/no-icon.pdf"
                type="application/pdf"
                data-icon="None"></object>
        <strong>None:</strong> No icon displayed (attachment still embedded)
    </div>

    <div style="margin: 15pt 0;">
        <object data="files/important.pdf"
                type="application/pdf"
                data-icon="Pushpin"></object>
        <strong>Pushpin:</strong> Important or priority document
    </div>

    <div style="margin: 15pt 0;">
        <object data="files/standard.pdf"
                type="application/pdf"
                data-icon="Paperclip"></object>
        <strong>Paperclip:</strong> Standard document attachment
    </div>

    <div style="margin: 15pt 0;">
        <object data="files/reference.pdf"
                type="application/pdf"
                data-icon="Tag"></object>
        <strong>Tag:</strong> Reference or categorized material
    </div>

    <div style="margin: 15pt 0;">
        <object data="files/data.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph"></object>
        <strong>Graph:</strong> Data or analytical document
    </div>
</div>
```

### Icon-Based Document Organization

```html
<div style="border: 2pt solid #336699; padding: 20pt;">
    <h2>Contract Documents</h2>

    <h3 style="color: #d9534f;">Priority Documents</h3>
    <div style="margin: 10pt 0;">
        <object data="contracts/master-agreement.pdf"
                type="application/pdf"
                data-icon="Pushpin"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Master Service Agreement (requires immediate review)</span>
    </div>

    <h3 style="color: #336699;">Supporting Documents</h3>
    <div style="margin: 10pt 0;">
        <object data="contracts/statement-of-work.pdf"
                type="application/pdf"
                data-icon="Paperclip"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Statement of Work</span>
    </div>

    <div style="margin: 10pt 0;">
        <object data="contracts/terms-and-conditions.pdf"
                type="application/pdf"
                data-icon="Paperclip"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Terms and Conditions</span>
    </div>

    <h3 style="color: #5cb85c;">Reference Materials</h3>
    <div style="margin: 10pt 0;">
        <object data="contracts/service-catalog.pdf"
                type="application/pdf"
                data-icon="Tag"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Service Catalog</span>
    </div>

    <h3 style="color: #f0ad4e;">Financial Data</h3>
    <div style="margin: 10pt 0;">
        <object data="contracts/pricing-schedule.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Pricing Schedule</span>
    </div>
</div>
```

### Dynamic Icon Selection

```html
<!-- Model: {
    attachments: [
        { file: "critical.pdf", type: "application/pdf", priority: "high", name: "Critical Update" },
        { file: "report.pdf", type: "application/pdf", priority: "normal", name: "Monthly Report" },
        { file: "data.xlsx", type: "application/vnd...sheet", priority: "normal", name: "Data Export" }
    ]
} -->

<template data-bind="{{model.attachments}}">
    <div style="margin: 10pt 0;">
        <object data="{{.file}}"
                type="{{.type}}"
                data-icon="{{.priority == 'high' ? 'Pushpin' :
                           .type.includes('sheet') ? 'Graph' :
                           'Paperclip'}}"
                alt="{{.name}}"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>{{.name}}</span>
        <span style="font-size: 9pt; color: #666;">
            {{.priority == 'high' ? ' (PRIORITY)' : ''}}
        </span>
    </div>
</template>
```

### Styled Attachment List

```html
<style>
    .attachment-item {
        padding: 12pt;
        margin: 8pt 0;
        border-left: 4pt solid #336699;
        background-color: #f9f9f9;
    }
    .attachment-icon {
        vertical-align: middle;
        margin-right: 10pt;
    }
</style>

<div>
    <h2>Project Deliverables</h2>

    <div class="attachment-item">
        <object data="deliverables/final-report.pdf"
                type="application/pdf"
                data-icon="Pushpin"
                class="attachment-icon"></object>
        <strong>Final Project Report</strong>
        <p style="margin: 5pt 0 0 30pt; font-size: 9pt; color: #666;">
            Complete project documentation and findings
        </p>
    </div>

    <div class="attachment-item">
        <object data="deliverables/source-code.zip"
                type="application/zip"
                data-icon="Tag"
                class="attachment-icon"></object>
        <strong>Source Code Archive</strong>
        <p style="margin: 5pt 0 0 30pt; font-size: 9pt; color: #666;">
            Complete source code and build scripts
        </p>
    </div>

    <div class="attachment-item">
        <object data="deliverables/test-results.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph"
                class="attachment-icon"></object>
        <strong>Test Results Data</strong>
        <p style="margin: 5pt 0 0 30pt; font-size: 9pt; color: #666;">
            Comprehensive test results and metrics
        </p>
    </div>
</div>
```

### Invoice with Supporting Documents

```html
<div>
    <h1>Invoice #INV-2024-1234</h1>
    <p>Total Amount: $15,250.00</p>

    <div style="margin-top: 30pt; padding: 20pt; background-color: #f5f8fa;
                border: 1pt solid #ddd;">
        <h2 style="margin-top: 0;">Supporting Documentation</h2>

        <div style="margin: 10pt 0;">
            <object data="invoices/receipt-001.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Receipt #001 - Materials ($8,500)
        </div>

        <div style="margin: 10pt 0;">
            <object data="invoices/timesheet.xlsx"
                    type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    data-icon="Graph"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Timesheet - Labor Hours (80 hours @ $50/hr)
        </div>

        <div style="margin: 10pt 0;">
            <object data="invoices/work-order.pdf"
                    type="application/pdf"
                    data-icon="Tag"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Work Order #WO-2024-456 - Authorization
        </div>
    </div>
</div>
```

### Research Paper with Supplementary Data

```html
<div>
    <h1>Research Paper: Machine Learning Applications</h1>

    <h2>Abstract</h2>
    <p>This paper presents...</p>

    <h2>Methodology</h2>
    <p>Our research involved...</p>

    <h2>Results</h2>
    <p>The findings demonstrate...</p>

    <div style="margin-top: 40pt; padding: 20pt; border: 2pt solid #336699;">
        <h2 style="margin-top: 0;">Supplementary Materials</h2>

        <div style="margin: 15pt 0;">
            <object data="supplements/dataset.csv"
                    type="text/csv"
                    data-icon="Graph"
                    alt="Complete Dataset"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>dataset.csv</strong> - Complete research dataset (100,000 samples)
        </div>

        <div style="margin: 15pt 0;">
            <object data="supplements/analysis-code.zip"
                    type="application/zip"
                    data-icon="Tag"
                    alt="Analysis Code"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>analysis-code.zip</strong> - Python analysis scripts
        </div>

        <div style="margin: 15pt 0;">
            <object data="supplements/extended-results.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    alt="Extended Results"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>extended-results.pdf</strong> - Complete result tables
        </div>
    </div>
</div>
```

### Categorized Attachment Table

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th style="padding: 10pt; text-align: left;">Category</th>
            <th style="padding: 10pt; text-align: left;">Document</th>
            <th style="padding: 10pt; text-align: center;">Attachment</th>
        </tr>
    </thead>
    <tbody>
        <tr style="border-bottom: 1pt solid #ddd;">
            <td style="padding: 10pt;">Priority</td>
            <td style="padding: 10pt;">Action Required Notice</td>
            <td style="padding: 10pt; text-align: center;">
                <object data="files/action-required.pdf"
                        type="application/pdf"
                        data-icon="Pushpin"></object>
            </td>
        </tr>
        <tr style="border-bottom: 1pt solid #ddd; background-color: #f9f9f9;">
            <td style="padding: 10pt;">Documents</td>
            <td style="padding: 10pt;">Service Agreement</td>
            <td style="padding: 10pt; text-align: center;">
                <object data="files/agreement.pdf"
                        type="application/pdf"
                        data-icon="Paperclip"></object>
            </td>
        </tr>
        <tr style="border-bottom: 1pt solid #ddd;">
            <td style="padding: 10pt;">Reference</td>
            <td style="padding: 10pt;">Technical Specifications</td>
            <td style="padding: 10pt; text-align: center;">
                <object data="files/specs.pdf"
                        type="application/pdf"
                        data-icon="Tag"></object>
            </td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="padding: 10pt;">Data</td>
            <td style="padding: 10pt;">Financial Analysis</td>
            <td style="padding: 10pt; text-align: center;">
                <object data="files/analysis.xlsx"
                        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        data-icon="Graph"></object>
            </td>
        </tr>
    </tbody>
</table>
```

### Multi-Language Document Package

```html
<div>
    <h1>Product Documentation Package</h1>

    <h2 style="margin-top: 30pt;">User Manuals</h2>
    <div style="padding: 15pt; background-color: #f9f9f9;">
        <div style="margin: 10pt 0;">
            <object data="manuals/manual-en.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            User Manual (English)
        </div>
        <div style="margin: 10pt 0;">
            <object data="manuals/manual-es.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            User Manual (Spanish)
        </div>
        <div style="margin: 10pt 0;">
            <object data="manuals/manual-fr.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            User Manual (French)
        </div>
    </div>

    <h2 style="margin-top: 30pt;">Quick Start Guides</h2>
    <div style="padding: 15pt; background-color: #f9f9f9;">
        <div style="margin: 10pt 0;">
            <object data="guides/quickstart-en.pdf"
                    type="application/pdf"
                    data-icon="Tag"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Quick Start Guide (English)
        </div>
        <div style="margin: 10pt 0;">
            <object data="guides/quickstart-es.pdf"
                    type="application/pdf"
                    data-icon="Tag"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Quick Start Guide (Spanish)
        </div>
    </div>
</div>
```

### Compliance Documentation

```html
<div>
    <h1>Compliance Certificate Package</h1>
    <p>This document includes all required compliance certifications:</p>

    <div style="margin: 30pt 0;">
        <h2>Critical Certifications (Review Required)</h2>
        <div style="border-left: 4pt solid #d9534f; padding-left: 15pt;">
            <div style="margin: 10pt 0;">
                <object data="compliance/iso-9001.pdf"
                        type="application/pdf"
                        data-icon="Pushpin"
                        alt="ISO 9001 Certificate"
                        style="vertical-align: middle; margin-right: 5pt;"></object>
                <strong>ISO 9001:2015 Certificate</strong> - Expires: 2025-06-30
            </div>
            <div style="margin: 10pt 0;">
                <object data="compliance/safety-cert.pdf"
                        type="application/pdf"
                        data-icon="Pushpin"
                        alt="Safety Certificate"
                        style="vertical-align: middle; margin-right: 5pt;"></object>
                <strong>Product Safety Certificate</strong> - Expires: 2025-03-15
            </div>
        </div>

        <h2 style="margin-top: 30pt;">Supporting Certifications</h2>
        <div style="border-left: 4pt solid #336699; padding-left: 15pt;">
            <div style="margin: 10pt 0;">
                <object data="compliance/quality-audit.pdf"
                        type="application/pdf"
                        data-icon="Paperclip"
                        style="vertical-align: middle; margin-right: 5pt;"></object>
                Quality Audit Report - Q4 2024
            </div>
            <div style="margin: 10pt 0;">
                <object data="compliance/test-results.xlsx"
                        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        data-icon="Graph"
                        style="vertical-align: middle; margin-right: 5pt;"></object>
                Compliance Test Results
            </div>
        </div>

        <h2 style="margin-top: 30pt;">Reference Documentation</h2>
        <div style="border-left: 4pt solid #5cb85c; padding-left: 15pt;">
            <div style="margin: 10pt 0;">
                <object data="compliance/standards-guide.pdf"
                        type="application/pdf"
                        data-icon="Tag"
                        style="vertical-align: middle; margin-right: 5pt;"></object>
                Industry Standards Guide
            </div>
            <div style="margin: 10pt 0;">
                <object data="compliance/procedures.pdf"
                        type="application/pdf"
                        data-icon="Tag"
                        style="vertical-align: middle; margin-right: 5pt;"></object>
                Compliance Procedures Manual
            </div>
        </div>
    </div>
</div>
```

### Financial Report Package

```html
<div>
    <h1>Annual Financial Report 2024</h1>

    <h2>Executive Summary</h2>
    <p>The company achieved record revenue...</p>

    <div style="margin-top: 40pt; padding: 25pt; border: 2pt solid #336699;
                background-color: #f5f8fa;">
        <h2 style="margin-top: 0;">Attached Financial Documents</h2>

        <h3 style="color: #d9534f;">Critical Documents</h3>
        <div style="margin: 10pt 0 10pt 20pt;">
            <object data="financials/audited-statements.pdf"
                    type="application/pdf"
                    data-icon="Pushpin"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Audited Financial Statements (CPA Certified)
        </div>

        <h3 style="color: #336699; margin-top: 20pt;">Financial Statements</h3>
        <div style="margin: 10pt 0 10pt 20pt;">
            <object data="financials/balance-sheet.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Balance Sheet - December 31, 2024
        </div>
        <div style="margin: 10pt 0 10pt 20pt;">
            <object data="financials/income-statement.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Income Statement - FY 2024
        </div>
        <div style="margin: 10pt 0 10pt 20pt;">
            <object data="financials/cash-flow.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Cash Flow Statement - FY 2024
        </div>

        <h3 style="color: #5cb85c; margin-top: 20pt;">Supporting Data</h3>
        <div style="margin: 10pt 0 10pt 20pt;">
            <object data="financials/detailed-ledger.xlsx"
                    type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    data-icon="Graph"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Detailed General Ledger (Excel)
        </div>
        <div style="margin: 10pt 0 10pt 20pt;">
            <object data="financials/transaction-data.csv"
                    type="text/csv"
                    data-icon="Graph"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Transaction Data Export (CSV)
        </div>

        <h3 style="color: #f0ad4e; margin-top: 20pt;">Reference Materials</h3>
        <div style="margin: 10pt 0 10pt 20pt;">
            <object data="financials/accounting-policies.pdf"
                    type="application/pdf"
                    data-icon="Tag"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Accounting Policies and Procedures
        </div>
        <div style="margin: 10pt 0 10pt 20pt;">
            <object data="financials/notes-to-statements.pdf"
                    type="application/pdf"
                    data-icon="Tag"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            Notes to Financial Statements
        </div>
    </div>
</div>
```

### Legal Document Bundle

```html
<div>
    <h1>Legal Document Package</h1>
    <p>Complete legal documentation for Project Alpha</p>

    <table style="width: 100%; margin-top: 30pt; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #336699; color: white;">
                <th style="padding: 12pt; text-align: left;">Priority</th>
                <th style="padding: 12pt; text-align: left;">Document Name</th>
                <th style="padding: 12pt; text-align: center;">Icon</th>
                <th style="padding: 12pt; text-align: left;">Status</th>
            </tr>
        </thead>
        <tbody>
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 10pt;">
                    <span style="color: #d9534f; font-weight: bold;">HIGH</span>
                </td>
                <td style="padding: 10pt;">Master Service Agreement</td>
                <td style="padding: 10pt; text-align: center;">
                    <object data="legal/master-agreement.pdf"
                            type="application/pdf"
                            data-icon="Pushpin"></object>
                </td>
                <td style="padding: 10pt;">Requires Signature</td>
            </tr>
            <tr style="border-bottom: 1pt solid #ddd; background-color: #f9f9f9;">
                <td style="padding: 10pt;">
                    <span style="color: #336699;">MEDIUM</span>
                </td>
                <td style="padding: 10pt;">Statement of Work</td>
                <td style="padding: 10pt; text-align: center;">
                    <object data="legal/sow.pdf"
                            type="application/pdf"
                            data-icon="Paperclip"></object>
                </td>
                <td style="padding: 10pt;">For Review</td>
            </tr>
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 10pt;">
                    <span style="color: #336699;">MEDIUM</span>
                </td>
                <td style="padding: 10pt;">Non-Disclosure Agreement</td>
                <td style="padding: 10pt; text-align: center;">
                    <object data="legal/nda.pdf"
                            type="application/pdf"
                            data-icon="Paperclip"></object>
                </td>
                <td style="padding: 10pt;">Signed</td>
            </tr>
            <tr style="border-bottom: 1pt solid #ddd; background-color: #f9f9f9;">
                <td style="padding: 10pt;">
                    <span style="color: #5cb85c;">LOW</span>
                </td>
                <td style="padding: 10pt;">Terms and Conditions</td>
                <td style="padding: 10pt; text-align: center;">
                    <object data="legal/terms.pdf"
                            type="application/pdf"
                            data-icon="Tag"></object>
                </td>
                <td style="padding: 10pt;">Reference</td>
            </tr>
            <tr style="background-color: #f9f9f9;">
                <td style="padding: 10pt;">
                    <span style="color: #5cb85c;">LOW</span>
                </td>
                <td style="padding: 10pt;">Legal Opinion</td>
                <td style="padding: 10pt; text-align: center;">
                    <object data="legal/opinion.pdf"
                            type="application/pdf"
                            data-icon="Tag"></object>
                </td>
                <td style="padding: 10pt;">Reference</td>
            </tr>
        </tbody>
    </table>
</div>
```

### Project Deliverables with Icon Legend

```html
<div>
    <h1>Project Deliverables - Phase 3</h1>

    <div style="padding: 15pt; background-color: #f0f8ff; border: 1pt solid #336699;
                margin: 20pt 0;">
        <h3 style="margin-top: 0;">Icon Legend</h3>
        <div style="display: flex; gap: 20pt;">
            <div style="flex: 1;">
                <object data="legend/dummy1.pdf" type="application/pdf"
                        data-icon="Pushpin"></object>
                <strong>Pushpin:</strong> Critical/Priority
            </div>
            <div style="flex: 1;">
                <object data="legend/dummy2.pdf" type="application/pdf"
                        data-icon="Paperclip"></object>
                <strong>Paperclip:</strong> Standard Document
            </div>
            <div style="flex: 1;">
                <object data="legend/dummy3.pdf" type="application/pdf"
                        data-icon="Tag"></object>
                <strong>Tag:</strong> Reference Material
            </div>
            <div style="flex: 1;">
                <object data="legend/dummy4.pdf" type="application/vnd...sheet"
                        data-icon="Graph"></object>
                <strong>Graph:</strong> Data/Analytics
            </div>
        </div>
    </div>

    <h2>Deliverable Documents</h2>

    <div style="margin: 10pt 0;">
        <object data="deliverables/final-presentation.pdf"
                type="application/pdf"
                data-icon="Pushpin"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <strong>Final Project Presentation</strong> - Due: Jan 30, 2025
    </div>

    <div style="margin: 10pt 0;">
        <object data="deliverables/technical-documentation.pdf"
                type="application/pdf"
                data-icon="Paperclip"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        Technical Documentation - Version 3.0
    </div>

    <div style="margin: 10pt 0;">
        <object data="deliverables/user-guide.pdf"
                type="application/pdf"
                data-icon="Tag"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        User Guide and Training Materials
    </div>

    <div style="margin: 10pt 0;">
        <object data="deliverables/performance-metrics.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        Performance Metrics and KPIs
    </div>

    <div style="margin: 10pt 0;">
        <object data="deliverables/source-code.zip"
                type="application/zip"
                data-icon="Tag"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        Complete Source Code Package
    </div>
</div>
```

### Healthcare Records with Icons

```html
<!-- Model: {
    patient: { name: "John Doe", id: "P-12345" },
    records: [
        { file: "lab-results.pdf", type: "Lab Results", priority: true, date: "2024-01-15" },
        { file: "imaging.pdf", type: "Imaging Report", priority: true, date: "2024-01-14" },
        { file: "medication.pdf", type: "Medication List", priority: false, date: "2024-01-10" },
        { file: "history.pdf", type: "Medical History", priority: false, date: "2024-01-01" }
    ]
} -->

<div>
    <h1>Medical Records: {{model.patient.name}}</h1>
    <p>Patient ID: {{model.patient.id}}</p>

    <div style="margin-top: 30pt;">
        <h2>Attached Medical Records</h2>

        <template data-bind="{{model.records}}">
            <div style="margin: 12pt 0; padding: 12pt;
                        border-left: 4pt solid {{.priority ? '#d9534f' : '#336699'}};
                        background-color: #f9f9f9;">
                <object data="records/{{.file}}"
                        type="application/pdf"
                        data-icon="{{.priority ? 'Pushpin' : 'Paperclip'}}"
                        style="vertical-align: middle; margin-right: 5pt;"></object>
                <strong>{{.type}}</strong>
                <span style="margin-left: 10pt; font-size: 9pt; color: #666;">
                    {{.date}}
                </span>
                <span style="margin-left: 10pt; font-size: 9pt; color: {{.priority ? '#d9534f' : '#666'}};">
                    {{.priority ? '(REQUIRES REVIEW)' : ''}}
                </span>
            </div>
        </template>
    </div>
</div>
```

---

## See Also

- [object](/reference/htmltags/object.html) - Object element (uses data-icon attribute)
- [data](/reference/htmlattributes/data.html) - File source for attachments
- [data-file-data](/reference/htmlattributes/data-file-data.html) - Binary file data for attachments
- [type](/reference/htmlattributes/type.html) - MIME type specification
- [alt](/reference/htmlattributes/alt.html) - Alternative text/description
- [Data Binding](/reference/binding/) - Dynamic content and attribute values
- [PDF Attachments](/reference/pdf/attachments.html) - PDF attachment specifications
- [IconAttachment Component](/reference/components/iconattachment.html) - Base attachment component

---
