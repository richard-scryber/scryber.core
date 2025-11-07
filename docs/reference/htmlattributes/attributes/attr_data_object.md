---
layout: default
title: data (object)
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data : The Object Data Source Attribute

The `data` attribute on the `<object>` element specifies the file source for PDF attachments. It accepts file paths (relative or absolute), URLs, and dynamic data binding expressions, enabling flexible file embedding in PDF documents. Attached files remain as separate entities within the PDF, accessible through PDF viewer attachment interfaces.

## Usage

The `data` attribute specifies the source file for attachment:
- Local file paths (relative or absolute)
- Remote URLs for downloading files
- Supports all file types (PDF, Office documents, images, archives, etc.)
- Maps to the `Source` property on the IconAttachment component
- Required attribute for functional object elements
- Used in conjunction with MIME type specification
- Supports dynamic data binding for runtime file selection
- Files are embedded as attachments, not rendered inline

```html
<!-- Local file attachment -->
<object data="documents/report.pdf" type="application/pdf"></object>

<!-- URL file attachment -->
<object data="https://example.com/files/data.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"></object>

<!-- Dynamic source -->
<object data="{{model.attachmentPath}}" type="{{model.mimeType}}"></object>

<!-- With display icon -->
<object data="contract.pdf" type="application/pdf" data-icon="Paperclip"></object>
```

---

## Supported Elements

The `data` attribute is used exclusively with:

### Object Element
- `<object>` - Creates file attachments in PDF documents (primary use)

**Note**: The `data` attribute on `<object>` has a different purpose than on other elements. It specifies the file to **attach** to the PDF, not render inline.

---

## Binding Values

The `data` attribute supports data binding for dynamic file attachment:

```html
<!-- Simple dynamic path -->
<object data="{{model.documentPath}}" type="application/pdf"
        data-icon="Paperclip"></object>

<!-- Constructed path -->
<object data="attachments/{{model.category}}/{{model.filename}}"
        type="{{model.fileType}}"></object>

<!-- Conditional source -->
<object data="{{model.useFinal ? model.finalDoc : model.draftDoc}}"
        type="application/pdf"
        alt="{{model.useFinal ? 'Final Version' : 'Draft Version'}}"></object>

<!-- URL with parameters -->
<object data="https://api.example.com/files/download?id={{model.fileId}}"
        type="application/pdf"></object>

<!-- Multiple attachments -->
<template data-bind="{{model.attachments}}">
    <object data="{{.filePath}}"
            type="{{.mimeType}}"
            data-icon="Paperclip"
            alt="{{.description}}"></object>
</template>
```

**Data Model Example:**
```json
{
  "documentPath": "reports/quarterly-report.pdf",
  "category": "legal",
  "filename": "contract.pdf",
  "fileType": "application/pdf",
  "useFinal": true,
  "finalDoc": "final-report.pdf",
  "draftDoc": "draft-report.pdf",
  "fileId": "12345",
  "attachments": [
    {
      "filePath": "docs/spec.pdf",
      "mimeType": "application/pdf",
      "description": "Technical Specification"
    },
    {
      "filePath": "data/results.xlsx",
      "mimeType": "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      "description": "Test Results"
    }
  ]
}
```

---

## Notes

### How Object Attachments Work

The `data` attribute on `<object>` creates **embedded file attachments** in PDF:

1. **File Loading**: The file specified in `data` is loaded from the path or URL
2. **Embedding**: The entire file is embedded into the PDF as a separate attachment
3. **Preservation**: The original file format and content remain unchanged
4. **Access**: PDF viewers provide attachment panels/icons for users to open/save files
5. **Icon Display**: Optional visual indicators (via `data-icon`) can appear in the document
6. **Metadata**: Filename, MIME type, and description are stored with the attachment

This is similar to email attachments - files are packaged with the document but remain distinct.

### File Path Types

The `data` attribute accepts different path formats:

#### 1. Relative Paths
Relative to the document or application location:

```html
<object data="contract.pdf" type="application/pdf"></object>              <!-- Same directory -->
<object data="attachments/report.pdf" type="application/pdf"></object>    <!-- Subdirectory -->
<object data="../shared/template.pdf" type="application/pdf"></object>    <!-- Parent directory -->
```

#### 2. Absolute Paths
Full file system paths:

```html
<!-- Windows -->
<object data="C:\Documents\Reports\report.pdf" type="application/pdf"></object>

<!-- Unix/Mac -->
<object data="/Users/username/documents/report.pdf" type="application/pdf"></object>
```

#### 3. URLs
HTTP/HTTPS URLs to remote files:

```html
<object data="https://example.com/files/document.pdf" type="application/pdf"></object>
<object data="https://cdn.example.com/data/spreadsheet.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"></object>
```

### Supported File Types

The `data` attribute works with any file type. Common examples:

| Category | File Types | MIME Type Example |
|----------|-----------|-------------------|
| **Documents** | PDF, Word, Excel, PowerPoint | `application/pdf` |
| **Images** | JPEG, PNG, GIF, TIFF, SVG | `image/jpeg` |
| **Text** | Plain text, CSV, HTML, XML | `text/plain` |
| **Archives** | ZIP, RAR, TAR | `application/zip` |
| **Data** | JSON, XML, CSV | `application/json` |
| **Other** | Any binary file | `application/octet-stream` |

```html
<!-- PDF document -->
<object data="report.pdf" type="application/pdf" data-icon="Paperclip"></object>

<!-- Excel spreadsheet -->
<object data="data.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        data-icon="Graph"></object>

<!-- Image file -->
<object data="diagram.png" type="image/png" data-icon="Tag"></object>

<!-- Archive -->
<object data="source-code.zip" type="application/zip" data-icon="Tag"></object>
```

### Binary Data Alternative

For files from memory or databases, use `data-file-data` instead:

```html
<!-- data attribute used as filename, binary content from data-file-data -->
<object data="generated-report.pdf"
        data-file-data="{{model.pdfBytes}}"
        type="application/pdf"
        data-icon="Paperclip"></object>
```

When `data-file-data` is provided:
- The `data` attribute value becomes the filename
- Binary content comes from `data-file-data`
- No file system access is required

### File Loading Process

When the object element processes the `data` attribute:

1. **Path Resolution**: The `data` path is resolved (relative to document location)
2. **Binary Check**: If `data-file-data` exists, it overrides file loading
3. **File Access**: File is read from disk or downloaded from URL
4. **Validation**: File existence and accessibility are verified
5. **Embedding**: File content is embedded into the PDF
6. **Metadata**: Filename from `data` and MIME type from `type` are stored

### Missing or Invalid Sources

If the `data` path is invalid or file doesn't exist:

**Strict Conformance Mode**:
- Error is thrown immediately
- Document generation stops
- Exception contains details about the missing file

**Lax Conformance Mode**:
- Error is logged to trace output
- Generation continues without the attachment
- No visual indicator appears in the document

Best practices:
```html
<!-- Always verify files exist before generation -->
<object data="verified-path/document.pdf"
        type="application/pdf"
        alt="Verified Document"
        data-icon="Paperclip"></object>
```

### PDF File Size Considerations

File attachments increase PDF size:

- The entire file is embedded (1MB attachment = 1MB+ PDF increase)
- Multiple attachments compound size increases
- No compression is applied to attachments
- Consider file size limits for distribution
- Alternative: Provide download links instead of embedding

```html
<!-- Small file - suitable for attachment -->
<object data="summary.pdf" type="application/pdf"></object>  <!-- 200KB -->

<!-- Large file - consider alternatives -->
<!-- Instead of: <object data="video.mp4" type="video/mp4"> -->
<!-- Consider: <a href="https://example.com/video.mp4">Download Video</a> -->
```

### Security Considerations

Be cautious with file attachments:

1. **Trusted Sources**: Only attach files from trusted sources
2. **Path Validation**: Sanitize and validate user-provided paths
3. **File Type Validation**: Verify file types match expectations
4. **Virus Scanning**: Scan user-uploaded files before attachment
5. **Size Limits**: Enforce maximum attachment sizes
6. **Access Control**: Ensure proper file system permissions
7. **URL Safety**: Validate URLs for HTTPS and trusted domains

```html
<!-- Sanitize user input -->
<!-- Model: sanitizedPath from validated user input -->
<object data="{{model.sanitizedPath}}"
        type="{{model.validatedMimeType}}"
        data-icon="Paperclip"></object>
```

### Network Considerations

When using URLs in the `data` attribute:

- Ensure network access during PDF generation
- Remote files are downloaded at generation time
- Network failures will cause errors (in strict mode)
- Consider timeouts for slow connections
- HTTPS is recommended for security
- Large files may impact generation performance

```html
<!-- Ensure reliable network access -->
<object data="https://cdn.example.com/files/document.pdf"
        type="application/pdf"></object>
```

### Filename Extraction

The attachment filename is extracted from the `data` attribute:

```html
<!-- Filename: "report.pdf" -->
<object data="documents/report.pdf" type="application/pdf"></object>

<!-- Filename: "Q4-2024.xlsx" -->
<object data="https://example.com/files/Q4-2024.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"></object>

<!-- Filename: "generated.pdf" (when using binary data) -->
<object data="generated.pdf"
        data-file-data="{{model.bytes}}"
        type="application/pdf"></object>
```

### Combining with Display Icons

The `data` attribute works with `data-icon` for visual indicators:

```html
<!-- Attachment with pushpin icon -->
<object data="important.pdf"
        type="application/pdf"
        data-icon="Pushpin"
        alt="Important Notice"></object>

<!-- Attachment with paperclip icon -->
<object data="contract.pdf"
        type="application/pdf"
        data-icon="Paperclip"
        alt="Service Contract"></object>
```

See [data-icon attribute](/reference/htmlattributes/data-icon.html) for more details.

### Object vs Iframe/Embed

Important distinction:

**object with `data`**:
- Creates file **attachments** (not rendered)
- Files remain separate within PDF
- Users access via PDF viewer's attachment interface
- Supports any file type

**iframe/embed with `src`**:
- **Parses and renders** content inline
- Content becomes part of document layout
- Only supports HTML/XML content

```html
<!-- Attachment (object) -->
<object data="report.pdf" type="application/pdf" data-icon="Paperclip"></object>

<!-- Inline rendering (iframe) -->
<iframe src="content.html" width="500pt" height="400pt"></iframe>
```

---

## Examples

### Basic File Attachments

```html
<!-- Simple PDF attachment -->
<object data="report.pdf" type="application/pdf"></object>

<!-- Attachment with description -->
<object data="analysis.pdf"
        type="application/pdf"
        alt="Annual Sales Analysis"></object>

<!-- Attachment with icon -->
<object data="contract.pdf"
        type="application/pdf"
        data-icon="Paperclip"
        alt="Service Contract"></object>
```

### Different File Types

```html
<!-- PDF document -->
<object data="documentation.pdf"
        type="application/pdf"
        data-icon="Paperclip"></object>

<!-- Word document -->
<object data="requirements.docx"
        type="application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        data-icon="Paperclip"></object>

<!-- Excel spreadsheet -->
<object data="budget.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        data-icon="Graph"></object>

<!-- PowerPoint presentation -->
<object data="presentation.pptx"
        type="application/vnd.openxmlformats-officedocument.presentationml.presentation"
        data-icon="Paperclip"></object>

<!-- Image file -->
<object data="diagram.png"
        type="image/png"
        data-icon="Tag"></object>

<!-- ZIP archive -->
<object data="source-code.zip"
        type="application/zip"
        data-icon="Tag"></object>

<!-- CSV data -->
<object data="export.csv"
        type="text/csv"
        data-icon="Graph"></object>
```

### Files from Different Locations

```html
<!-- Same directory -->
<object data="local-file.pdf" type="application/pdf"></object>

<!-- Subdirectory -->
<object data="attachments/reports/annual-report.pdf"
        type="application/pdf"></object>

<!-- Parent directory -->
<object data="../shared/template.pdf" type="application/pdf"></object>

<!-- Absolute path (Windows) -->
<object data="C:\Documents\Reports\report.pdf" type="application/pdf"></object>

<!-- Absolute path (Unix/Mac) -->
<object data="/Users/username/documents/report.pdf" type="application/pdf"></object>

<!-- Remote URL -->
<object data="https://example.com/files/whitepaper.pdf"
        type="application/pdf"></object>

<!-- CDN URL -->
<object data="https://cdn.example.com/documents/guide.pdf"
        type="application/pdf"></object>
```

### Dynamic File Sources

```html
<!-- Model: { reportPath: "reports/Q4.pdf", reportType: "application/pdf" } -->

<!-- Simple binding -->
<object data="{{model.reportPath}}"
        type="{{model.reportType}}"
        data-icon="Paperclip"></object>

<!-- Constructed path -->
<!-- Model: { year: "2024", quarter: "Q4" } -->
<object data="reports/{{model.year}}/{{model.quarter}}-report.pdf"
        type="application/pdf"
        alt="{{model.quarter}} {{model.year}} Report"></object>

<!-- Conditional source -->
<!-- Model: { isProduction: true, prodFile: "prod.pdf", devFile: "dev.pdf" } -->
<object data="{{model.isProduction ? model.prodFile : model.devFile}}"
        type="application/pdf"
        alt="{{model.isProduction ? 'Production' : 'Development'}} Report"></object>

<!-- URL with parameters -->
<!-- Model: { documentId: "12345", version: "2" } -->
<object data="https://api.example.com/docs/{{model.documentId}}/v{{model.version}}"
        type="application/pdf"></object>
```

### Attachment List with Descriptions

```html
<div style="border: 1pt solid #336699; padding: 15pt;">
    <h3 style="margin-top: 0; color: #336699;">Supporting Documents</h3>

    <div style="margin: 10pt 0;">
        <object data="attachments/proposal.pdf"
                type="application/pdf"
                data-icon="Paperclip"
                alt="Project Proposal"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Project Proposal (PDF, 2.3 MB)</span>
    </div>

    <div style="margin: 10pt 0;">
        <object data="attachments/budget.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph"
                alt="Budget Spreadsheet"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Budget Spreadsheet (Excel, 156 KB)</span>
    </div>

    <div style="margin: 10pt 0;">
        <object data="attachments/timeline.pdf"
                type="application/pdf"
                data-icon="Tag"
                alt="Project Timeline"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Project Timeline (PDF, 890 KB)</span>
    </div>
</div>
```

### Multiple Attachments with Data Binding

```html
<!-- Model: { attachments: [
    { path: "file1.pdf", type: "application/pdf", name: "Technical Specs", icon: "Tag" },
    { path: "file2.xlsx", type: "application/vnd...sheet", name: "Cost Analysis", icon: "Graph" }
]} -->

<div class="attachments-section">
    <h2>Attached Documents</h2>

    <template data-bind="{{model.attachments}}">
        <div style="margin: 10pt 0; padding: 10pt;
                    border-left: 3pt solid #336699; background-color: #f9f9f9;">
            <object data="{{.path}}"
                    type="{{.type}}"
                    data-icon="{{.icon}}"
                    alt="{{.name}}"
                    style="vertical-align: middle; margin-right: 8pt;"></object>
            <strong>{{.name}}</strong>
        </div>
    </template>
</div>
```

### Contract with Multiple Exhibits

```html
<div class="contract">
    <h1>Service Agreement</h1>
    <p>This agreement is made between the parties...</p>

    <h2>Terms and Conditions</h2>
    <p>The following terms apply...</p>

    <div style="margin-top: 30pt; padding: 20pt;
                border: 2pt solid #336699; background-color: #f5f8fa;">
        <h2 style="margin-top: 0;">Exhibits</h2>

        <h3>Exhibit A - Technical Specifications</h3>
        <object data="exhibits/exhibit-a-technical-specs.pdf"
                type="application/pdf"
                data-icon="Paperclip"
                alt="Exhibit A"></object>
        <p>Detailed technical specifications for deliverables.</p>

        <h3>Exhibit B - Price Schedule</h3>
        <object data="exhibits/exhibit-b-pricing.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph"
                alt="Exhibit B"></object>
        <p>Complete pricing breakdown and payment schedule.</p>

        <h3>Exhibit C - Service Level Agreement</h3>
        <object data="exhibits/exhibit-c-sla.pdf"
                type="application/pdf"
                data-icon="Paperclip"
                alt="Exhibit C"></object>
        <p>Service level commitments and performance metrics.</p>

        <h3>Exhibit D - Compliance Certificates</h3>
        <object data="exhibits/exhibit-d-compliance.pdf"
                type="application/pdf"
                data-icon="Tag"
                alt="Exhibit D"></object>
        <p>ISO 9001 and industry compliance documentation.</p>
    </div>
</div>
```

### Invoice with Supporting Documents

```html
<div class="invoice">
    <h1>Invoice #INV-2024-0123</h1>
    <p>Date: December 15, 2024</p>

    <table style="width: 100%; margin: 20pt 0;">
        <tr>
            <th style="text-align: left;">Description</th>
            <th style="text-align: right;">Amount</th>
        </tr>
        <tr>
            <td>Professional Services</td>
            <td style="text-align: right;">$5,000.00</td>
        </tr>
        <tr>
            <td>Materials</td>
            <td style="text-align: right;">$1,200.00</td>
        </tr>
        <tr style="font-weight: bold;">
            <td>Total</td>
            <td style="text-align: right;">$6,200.00</td>
        </tr>
    </table>

    <div style="margin-top: 30pt; padding: 15pt;
                border-top: 2pt solid #333; background-color: #f9f9f9;">
        <h3>Supporting Documents</h3>

        <div style="margin: 10pt 0;">
            <object data="receipts/receipt-materials.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    style="margin-right: 5pt;"></object>
            Materials Receipt - $1,200.00
        </div>

        <div style="margin: 10pt 0;">
            <object data="timesheets/december-timesheet.xlsx"
                    type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    data-icon="Graph"
                    style="margin-right: 5pt;"></object>
            December Timesheet - 50 hours
        </div>

        <div style="margin: 10pt 0;">
            <object data="work-orders/wo-2024-456.pdf"
                    type="application/pdf"
                    data-icon="Tag"
                    style="margin-right: 5pt;"></object>
            Work Order #WO-2024-456
        </div>
    </div>
</div>
```

### Research Paper with Supplementary Files

```html
<div class="paper">
    <h1>Advanced Machine Learning Techniques for Data Classification</h1>
    <p class="authors">Dr. Jane Smith, Dr. John Doe</p>

    <h2>Abstract</h2>
    <p>This paper presents novel machine learning approaches...</p>

    <h2>Methodology</h2>
    <p>Our research methodology involved...</p>

    <h2>Results</h2>
    <p>The experimental results demonstrate...</p>

    <h2>Supplementary Materials</h2>
    <div style="background-color: #f0f0f0; padding: 20pt; margin: 20pt 0;">
        <p>The following supplementary files are attached to this paper:</p>

        <div style="margin: 15pt 0;">
            <object data="supplementary/dataset.csv"
                    type="text/csv"
                    data-icon="Graph"
                    alt="Research Dataset"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>dataset.csv</strong> (5.2 MB) - Complete research dataset with 100,000 samples
        </div>

        <div style="margin: 15pt 0;">
            <object data="supplementary/analysis-code.zip"
                    type="application/zip"
                    data-icon="Tag"
                    alt="Analysis Code"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>analysis-code.zip</strong> (1.8 MB) - Python analysis scripts and Jupyter notebooks
        </div>

        <div style="margin: 15pt 0;">
            <object data="supplementary/extended-results.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    alt="Extended Results"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>extended-results.pdf</strong> (3.5 MB) - Complete result tables and visualizations
        </div>

        <div style="margin: 15pt 0;">
            <object data="supplementary/model-weights.json"
                    type="application/json"
                    data-icon="Tag"
                    alt="Model Weights"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>model-weights.json</strong> (890 KB) - Trained model weights and parameters
        </div>
    </div>
</div>
```

### Styled Attachment Cards

```html
<style>
    .attachment-card {
        display: inline-block;
        width: 200pt;
        margin: 10pt;
        padding: 15pt;
        border: 1pt solid #ddd;
        border-radius: 8pt;
        background-color: #fff;
        text-align: center;
    }
    .attachment-icon {
        display: block;
        margin: 0 auto 10pt auto;
    }
</style>

<div style="text-align: center;">
    <div class="attachment-card">
        <object data="files/report.pdf"
                type="application/pdf"
                data-icon="Paperclip"
                class="attachment-icon"></object>
        <strong>Annual Report</strong><br/>
        <span style="font-size: 9pt; color: #666;">PDF, 2.5 MB</span>
    </div>

    <div class="attachment-card">
        <object data="files/data.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph"
                class="attachment-icon"></object>
        <strong>Financial Data</strong><br/>
        <span style="font-size: 9pt; color: #666;">Excel, 450 KB</span>
    </div>

    <div class="attachment-card">
        <object data="files/photos.zip"
                type="application/zip"
                data-icon="Tag"
                class="attachment-icon"></object>
        <strong>Photo Archive</strong><br/>
        <span style="font-size: 9pt; color: #666;">ZIP, 15.3 MB</span>
    </div>
</div>
```

### Remote File Attachments

```html
<!-- Attach file from company CDN -->
<object data="https://cdn.example.com/documents/whitepaper-2024.pdf"
        type="application/pdf"
        data-icon="Paperclip"
        alt="2024 Industry Whitepaper"></object>

<!-- Attach from API endpoint -->
<object data="https://api.example.com/reports/generate?id={{model.reportId}}"
        type="application/pdf"
        data-icon="Paperclip"
        alt="Generated Report {{model.reportId}}"></object>

<!-- Attach from secure document server -->
<object data="https://secure.example.com/files/{{model.userId}}/{{model.documentName}}"
        type="{{model.documentType}}"
        data-icon="Pushpin"
        alt="{{model.documentTitle}}"></object>
```

### Conditional Attachments

```html
<!-- Model: { includeFinancials: true, financialsPath: "financials.xlsx" } -->

<div>
    <h2>Project Report</h2>
    <p>Project overview and details...</p>

    <!-- Only attach if included -->
    <object data="{{model.financialsPath}}"
            type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            data-icon="Graph"
            alt="Financial Details"
            hidden="{{model.includeFinancials ? '' : 'hidden'}}"></object>
</div>
```

### Attachment Table Layout

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th style="padding: 10pt; text-align: left;">Document</th>
            <th style="padding: 10pt; text-align: left;">Description</th>
            <th style="padding: 10pt; text-align: center;">Attachment</th>
        </tr>
    </thead>
    <tbody>
        <tr style="border-bottom: 1pt solid #ddd;">
            <td style="padding: 10pt;">Financial Statement</td>
            <td style="padding: 10pt;">Complete Q4 financial analysis</td>
            <td style="padding: 10pt; text-align: center;">
                <object data="files/financial-statement.pdf"
                        type="application/pdf"
                        data-icon="Paperclip"></object>
            </td>
        </tr>
        <tr style="border-bottom: 1pt solid #ddd;">
            <td style="padding: 10pt;">Budget Breakdown</td>
            <td style="padding: 10pt;">Detailed budget allocation</td>
            <td style="padding: 10pt; text-align: center;">
                <object data="files/budget.xlsx"
                        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        data-icon="Graph"></object>
            </td>
        </tr>
        <tr style="border-bottom: 1pt solid #ddd;">
            <td style="padding: 10pt;">Compliance Certificate</td>
            <td style="padding: 10pt;">ISO 9001 certification</td>
            <td style="padding: 10pt; text-align: center;">
                <object data="files/compliance.pdf"
                        type="application/pdf"
                        data-icon="Tag"></object>
            </td>
        </tr>
    </tbody>
</table>
```

### Floating Attachments

```html
<div>
    <object data="reference-guide.pdf"
            type="application/pdf"
            data-icon="Pushpin"
            alt="Reference Guide"
            style="float: right; margin: 0 0 15pt 15pt;"></object>

    <p>
        This document provides comprehensive information about the system.
        The attached reference guide (see pushpin icon to the right) contains
        additional technical details and specifications.
    </p>

    <p>
        The reference guide includes API documentation, configuration examples,
        and troubleshooting procedures. All users should review the attached
        document for complete system information.
    </p>

    <p>
        For questions regarding the content in the reference guide, please
        contact the technical support team.
    </p>
</div>
<div style="clear: both;"></div>
```

### Version-Controlled Attachments

```html
<!-- Model: {
    documents: [
        { name: "Spec", file: "spec-v3.pdf", version: "3.0", date: "2024-01-15" },
        { name: "Design", file: "design-v2.pdf", version: "2.1", date: "2024-01-20" }
    ]
} -->

<div>
    <h2>Project Documentation</h2>

    <template data-bind="{{model.documents}}">
        <div style="margin: 15pt 0; padding: 15pt;
                    border: 1pt solid #ddd; background-color: #fafafa;">
            <div style="display: flex; align-items: center;">
                <object data="documents/{{.file}}"
                        type="application/pdf"
                        data-icon="Paperclip"
                        style="margin-right: 10pt;"></object>
                <div>
                    <strong>{{.name}} Document</strong><br/>
                    <span style="font-size: 9pt; color: #666;">
                        Version {{.version}} - Updated {{.date}}
                    </span>
                </div>
            </div>
        </div>
    </template>
</div>
```

### Image Source Files as Attachments

```html
<div>
    <h2>Design Assets</h2>
    <p>High-resolution source files for all images used in this document:</p>

    <div style="padding: 15pt; background-color: #f9f9f9;">
        <!-- Attach high-res PNG -->
        <div style="margin: 10pt 0;">
            <object data="assets/logo-highres.png"
                    type="image/png"
                    data-icon="Tag"
                    alt="High Resolution Logo"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <span>Logo (PNG, 300 DPI, 5.2 MB)</span>
        </div>

        <!-- Attach TIFF scan -->
        <div style="margin: 10pt 0;">
            <object data="assets/document-scan.tiff"
                    type="image/tiff"
                    data-icon="Tag"
                    alt="Original Document Scan"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <span>Scanned Document (TIFF, 600 DPI, 12.8 MB)</span>
        </div>

        <!-- Attach SVG source -->
        <div style="margin: 10pt 0;">
            <object data="assets/diagram.svg"
                    type="image/svg+xml"
                    data-icon="Tag"
                    alt="Vector Diagram Source"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <span>Diagram Vector Source (SVG, 245 KB)</span>
        </div>
    </div>
</div>
```

### Data Export Attachments

```html
<div>
    <h2>Exported Data Files</h2>
    <p>The following data exports are attached for further analysis:</p>

    <div style="border: 1pt solid #336699; padding: 20pt; margin: 15pt 0;">
        <!-- CSV data -->
        <div style="margin: 10pt 0;">
            <object data="exports/sales-data.csv"
                    type="text/csv"
                    data-icon="Graph"
                    alt="Sales Data Export"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>sales-data.csv</strong> - Complete sales transactions (25,000 records)
        </div>

        <!-- JSON export -->
        <div style="margin: 10pt 0;">
            <object data="exports/customer-data.json"
                    type="application/json"
                    data-icon="Tag"
                    alt="Customer Data Export"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>customer-data.json</strong> - Customer database export (5,200 customers)
        </div>

        <!-- XML export -->
        <div style="margin: 10pt 0;">
            <object data="exports/inventory.xml"
                    type="text/xml"
                    data-icon="Tag"
                    alt="Inventory Export"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>inventory.xml</strong> - Current inventory levels (1,500 items)
        </div>
    </div>
</div>
```

---

## See Also

- [object](/reference/htmltags/object.html) - Object element (uses data attribute)
- [data-icon](/reference/htmlattributes/data-icon.html) - Display icon for attachments
- [data-file-data](/reference/htmlattributes/data-file-data.html) - Binary file data for attachments
- [type](/reference/htmlattributes/type.html) - MIME type specification
- [alt](/reference/htmlattributes/alt.html) - Alternative text/description
- [Data Binding](/reference/binding/) - Dynamic content and attribute values
- [PDF Attachments](/reference/pdf/attachments.html) - PDF attachment specifications
- [File Resources](/reference/resources/files.html) - Working with file resources

---
