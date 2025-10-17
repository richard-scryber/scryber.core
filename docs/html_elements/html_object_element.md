---
layout: default
title: object
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;object&gt; : The Object/Attachment Element

The `<object>` element embeds external files as attachments in PDF documents. It creates file attachments that can be opened from within the PDF viewer, supporting various file types including documents, images, data files, and other binary content. Objects can optionally display as icons within the document.

## Usage

The `<object>` element creates file attachments that:
- Embed external files as PDF attachments using the `data` attribute
- Support various file types (PDF, images, documents, data files, etc.)
- Display optional attachment icons within the document
- Load files from URLs, local paths, or binary data
- Support MIME type specification for proper file handling
- Enable dynamic file attachment through data binding
- Can contain fallback content displayed in the document
- Allow description/alt text for accessibility
- Support styling through CSS and inline styles
- Display as inline-block elements by default

```html
<!-- Basic file attachment -->
<object data="document.pdf" type="application/pdf"></object>

<!-- Attachment with icon -->
<object data="report.pdf" type="application/pdf" data-icon="Pushpin"></object>

<!-- Attachment with description -->
<object data="spreadsheet.xlsx" type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        alt="Q4 Financial Spreadsheet"></object>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the object. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Object-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data` | string | **Required**. Source path or URL for the file to attach. Maps to the `Source` property. |
| `type` | MimeType | MIME type of the attached file (e.g., `application/pdf`, `image/jpeg`). |
| `alt` | string | Description text for the attachment (accessibility and document metadata). |
| `data-icon` | AttachmentDisplayIcon | Icon to display for the attachment: `None`, `Pushpin`, `Paperclip`, `Tag`, `Graph`. Default: `None`. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-file` | PDFEmbeddedFileData | Binds embedded file data object directly (binding only). |
| `data-file-data` | byte[] | Binary file data as byte array (binding only). |

### CSS Style Support

The `<object>` element supports extensive CSS styling:

**Sizing**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`

**Positioning**:
- `display`: `inline-block` (default), `block`, `inline`, `none`
- `position`: `static`, `relative`, `absolute`
- `float`: `left`, `right`, `none`
- `clear`: `both`, `left`, `right`, `none`
- `top`, `left`, `right`, `bottom` (for positioned elements)
- `vertical-align`: `top`, `middle`, `bottom`, `baseline`

**Spacing**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding` (all variants)

**Visual Effects**:
- `border`, `border-width`, `border-color`, `border-style`, `border-radius`
- `background`, `background-color`
- `opacity`

---

## Notes

### How Object Attachments Work in PDF

The `<object>` element creates **file attachments** in the PDF document:

1. **File Embedding**: The referenced file is embedded into the PDF as an attachment
2. **Viewer Access**: PDF viewers display attachment icons that users can click to open/save
3. **Separate Files**: Attachments remain as separate files within the PDF container
4. **File Preservation**: Original file format and content are preserved unchanged
5. **Icon Display**: Optional visual indicators (icons) can be placed in the document layout
6. **Metadata**: File name, MIME type, and description are stored with the attachment

This is similar to email attachments - files are packaged with the document but remain distinct entities.

### Object vs Iframe/Embed

Key differences between these elements:

**object**:
- Creates file **attachments** in the PDF
- Files are embedded but not rendered in the document flow
- Users open attachments separately in their viewer
- Supports any file type
- Optional icon display in document
- Best for: Supporting documents, data files, original source files

**iframe/embed**:
- **Parses and renders** external HTML/XML content inline
- Content becomes part of the document layout
- Users see rendered content directly
- Only supports parseable HTML/XML
- Best for: Template composition, content reuse

### The data Attribute

The `data` attribute specifies the file to attach and supports multiple source types:

1. **Relative File Paths**:
   ```html
   <object data="attachments/document.pdf" type="application/pdf"></object>
   ```

2. **Absolute File Paths**:
   ```html
   <object data="/documents/report.pdf" type="application/pdf"></object>
   ```

3. **Remote URLs**:
   ```html
   <object data="https://example.com/files/data.xlsx" type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"></object>
   ```

4. **Dynamic Sources via Data Binding**:
   ```html
   <object data="{{model.attachmentPath}}" type="{{model.mimeType}}"></object>
   ```

### MIME Types

Common MIME types for the `type` attribute:

**Documents**:
- `application/pdf` - PDF documents
- `application/msword` - Microsoft Word (.doc)
- `application/vnd.openxmlformats-officedocument.wordprocessingml.document` - Word (.docx)
- `application/vnd.ms-excel` - Microsoft Excel (.xls)
- `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet` - Excel (.xlsx)
- `application/vnd.ms-powerpoint` - PowerPoint (.ppt)
- `application/vnd.openxmlformats-officedocument.presentationml.presentation` - PowerPoint (.pptx)

**Images**:
- `image/jpeg` - JPEG images
- `image/png` - PNG images
- `image/gif` - GIF images
- `image/tiff` - TIFF images
- `image/svg+xml` - SVG images

**Text**:
- `text/plain` - Plain text files
- `text/csv` - CSV files
- `text/html` - HTML files
- `text/xml` - XML files

**Archives**:
- `application/zip` - ZIP archives
- `application/x-rar-compressed` - RAR archives

**Other**:
- `application/json` - JSON data
- `application/octet-stream` - Generic binary data

### Attachment Icons

The `data-icon` attribute displays visual icons in the document:

| Icon Value | Description | Visual Representation |
|-----------|-------------|----------------------|
| `None` | No icon displayed (default) | Attachment added without visual indicator |
| `Pushpin` | Pushpin/thumbtack icon | Commonly used for important attachments |
| `Paperclip` | Paperclip icon | Standard attachment indicator |
| `Tag` | Tag/label icon | Used for categorized attachments |
| `Graph` | Graph/chart icon | Suitable for data/analytical attachments |

When an icon is specified, a clickable icon appears in the document at the object's location.

### Binary Data Binding

For dynamic attachments from memory or databases, use the `data-file-data` attribute:

```html
<!-- With model.fileBytes containing byte[] data -->
<object data="filename.pdf"
        data-file-data="{{model.fileBytes}}"
        type="application/pdf"
        alt="Dynamically loaded document"></object>
```

The system will:
1. Use the `data` attribute value as the filename
2. Load binary data from `data-file-data`
3. Create the attachment with the specified MIME type

### Fallback Content

The `<object>` element can contain fallback content displayed in the document:

```html
<object data="document.pdf" type="application/pdf" data-icon="Paperclip">
    <div style="padding: 10pt; background-color: #f0f0f0;">
        <strong>Attached:</strong> document.pdf
        <p>Click the paperclip icon to open the attached document.</p>
    </div>
</object>
```

### File Loading Process

When the object element is processed:

1. **Source Resolution**: The `data` attribute path/URL is resolved
2. **Binary Data Check**: If `data-file-data` is provided, it's used instead
3. **File Loading**: File content is loaded into memory
4. **Embedding**: File is embedded into the PDF with metadata
5. **Icon Placement**: If specified, icon is placed at the element's position
6. **Content Rendering**: Any fallback content is rendered in the document

### Error Handling

When file loading fails:

- In **Strict** conformance mode: Error is thrown, document generation stops
- In **Lax** conformance mode: Error is logged, generation continues
- The `alt` text or fallback content may be displayed
- Consider error handling for production systems

### Security Considerations

When attaching files:

1. **Trusted Sources**: Only attach files from trusted sources
2. **File Validation**: Validate file types and sizes before attachment
3. **User Input**: Sanitize any user-provided file paths
4. **Virus Scanning**: Consider virus scanning for user-uploaded attachments
5. **Size Limits**: Large attachments significantly increase PDF file size
6. **Local File Access**: Be cautious with file system access in web applications

### PDF File Size

File attachments increase PDF size:

- The entire file is embedded into the PDF
- Multiple attachments compound size increases
- Consider compression for large files
- Balance between convenience and file size
- Alternative: Provide download links instead of embeddings

### Class Hierarchy

In the Scryber codebase:

- `HTMLObject` extends `IconAttachment` → `VisualComponent` → `Component`
- Implements `IInvisibleContainer` interface
- Default display mode: `inline-block`
- Supports the `Contents` collection for fallback content
- The `data` attribute maps to the `Source` property
- Binary data is handled via `FileData` property and converted to `PDFEmbeddedFileData`

---

## Examples

### Basic File Attachment

```html
<!-- Simple PDF attachment -->
<object data="report.pdf" type="application/pdf"></object>

<!-- Attachment with description -->
<object data="analysis.pdf" type="application/pdf" alt="Annual Sales Analysis"></object>
```

### Attachments with Icons

```html
<div style="padding: 20pt;">
    <h2>Attached Documents</h2>

    <!-- Pushpin icon -->
    <object data="important.pdf" type="application/pdf"
            data-icon="Pushpin" alt="Important Notice"></object>

    <!-- Paperclip icon -->
    <object data="contract.pdf" type="application/pdf"
            data-icon="Paperclip" alt="Service Contract"></object>

    <!-- Tag icon -->
    <object data="specifications.pdf" type="application/pdf"
            data-icon="Tag" alt="Technical Specifications"></object>

    <!-- Graph icon -->
    <object data="data.xlsx"
            type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            data-icon="Graph" alt="Financial Data"></object>
</div>
```

### Styled Attachments

```html
<style>
    .attachment {
        display: inline-block;
        margin: 10pt;
        padding: 10pt;
        border: 1pt solid #ccc;
        background-color: #f9f9f9;
        border-radius: 4pt;
    }
</style>

<object data="document.pdf" type="application/pdf"
        data-icon="Paperclip" class="attachment"></object>

<object data="spreadsheet.xlsx"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        data-icon="Graph" class="attachment"></object>
```

### Attachment List

```html
<div style="border: 1pt solid #336699; padding: 15pt; margin: 20pt 0;">
    <h3 style="margin-top: 0; color: #336699;">Supporting Documents</h3>

    <div style="margin: 10pt 0;">
        <object data="proposal.pdf" type="application/pdf"
                data-icon="Paperclip" alt="Project Proposal"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Project Proposal (PDF)</span>
    </div>

    <div style="margin: 10pt 0;">
        <object data="budget.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph" alt="Budget Spreadsheet"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Budget Spreadsheet (Excel)</span>
    </div>

    <div style="margin: 10pt 0;">
        <object data="timeline.pdf" type="application/pdf"
                data-icon="Tag" alt="Project Timeline"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Project Timeline (PDF)</span>
    </div>
</div>
```

### Attachments with Fallback Content

```html
<object data="report.pdf" type="application/pdf" data-icon="Pushpin">
    <div style="border-left: 4pt solid #336699; padding: 10pt; background-color: #f0f8ff;">
        <strong>Attached Document:</strong> Quarterly Report
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
            Click the pushpin icon to open the attached PDF document.
        </p>
    </div>
</object>
```

### Multiple File Types

```html
<div class="attachments-section">
    <h2>Project Files</h2>

    <!-- PDF document -->
    <object data="documentation.pdf" type="application/pdf"
            data-icon="Paperclip" alt="Technical Documentation"></object>

    <!-- Word document -->
    <object data="requirements.docx"
            type="application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            data-icon="Paperclip" alt="Requirements Document"></object>

    <!-- Excel spreadsheet -->
    <object data="costs.xlsx"
            type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            data-icon="Graph" alt="Cost Analysis"></object>

    <!-- Image file -->
    <object data="diagram.png" type="image/png"
            data-icon="Tag" alt="System Diagram"></object>

    <!-- ZIP archive -->
    <object data="source-code.zip" type="application/zip"
            data-icon="Tag" alt="Source Code Archive"></object>
</div>
```

### Dynamic Attachment Binding

```html
<!-- With model = { attachmentPath: "report.pdf", attachmentType: "application/pdf", attachmentName: "Monthly Report" } -->

<object data="{{model.attachmentPath}}"
        type="{{model.attachmentType}}"
        data-icon="Paperclip"
        alt="{{model.attachmentName}}"></object>
```

### Binary Data Attachment

```html
<!-- With model.fileBytes containing byte[] data from database/API -->

<object data="{{model.fileName}}"
        data-file-data="{{model.fileBytes}}"
        type="{{model.mimeType}}"
        data-icon="Paperclip"
        alt="{{model.description}}"></object>
```

### Conditional Attachments

```html
<!-- With model = { includeAttachment: true, filePath: "document.pdf" } -->

<div>
    <h2>Report</h2>
    <p>Main report content...</p>

    <object data="{{model.filePath}}"
            type="application/pdf"
            data-icon="Paperclip"
            hidden="{{model.includeAttachment ? '' : 'hidden'}}"></object>
</div>
```

### Repeating Attachments

```html
<!-- With model.attachments = [
    {path: "file1.pdf", type: "application/pdf", name: "File 1"},
    {path: "file2.pdf", type: "application/pdf", name: "File 2"}
] -->

<div class="attachments">
    <h3>Attached Files</h3>
    <template data-bind="{{model.attachments}}">
        <div style="margin: 10pt 0;">
            <object data="{{.path}}"
                    type="{{.type}}"
                    data-icon="Paperclip"
                    alt="{{.name}}"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <span>{{.name}}</span>
        </div>
    </template>
</div>
```

### Positioned Attachments

```html
<!-- Attachment in top-right corner -->
<div style="position: relative; min-height: 100pt;">
    <object data="watermark.pdf" type="application/pdf"
            data-icon="Tag"
            style="position: absolute; top: 10pt; right: 10pt;"></object>

    <p>Document content here...</p>
</div>
```

### Floating Attachments

```html
<div>
    <object data="reference.pdf" type="application/pdf"
            data-icon="Pushpin"
            style="float: right; margin: 0 0 10pt 15pt;"></object>

    <p>
        This document references the attached PDF file. The attachment icon
        floats to the right with text wrapping around it naturally.
    </p>
    <p>
        Additional content continues to flow around the floating attachment icon.
    </p>
</div>
<div style="clear: both;"></div>
```

### Attachment Section with Descriptions

```html
<div style="border: 2pt solid #336699; padding: 20pt; margin: 20pt 0; background-color: #f5f8fa;">
    <h2 style="margin-top: 0; color: #336699;">Referenced Documents</h2>

    <table style="width: 100%; border-collapse: collapse;">
        <tr>
            <td style="padding: 10pt; border-bottom: 1pt solid #ccc; width: 40pt;">
                <object data="attachment1.pdf" type="application/pdf"
                        data-icon="Paperclip"></object>
            </td>
            <td style="padding: 10pt; border-bottom: 1pt solid #ccc;">
                <strong>Financial Statement</strong><br/>
                Complete financial analysis for Q4 2024
            </td>
        </tr>
        <tr>
            <td style="padding: 10pt; border-bottom: 1pt solid #ccc;">
                <object data="attachment2.xlsx"
                        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        data-icon="Graph"></object>
            </td>
            <td style="padding: 10pt; border-bottom: 1pt solid #ccc;">
                <strong>Budget Breakdown</strong><br/>
                Detailed budget allocation spreadsheet
            </td>
        </tr>
        <tr>
            <td style="padding: 10pt;">
                <object data="attachment3.pdf" type="application/pdf"
                        data-icon="Tag"></object>
            </td>
            <td style="padding: 10pt;">
                <strong>Compliance Certificate</strong><br/>
                ISO 9001 certification documentation
            </td>
        </tr>
    </table>
</div>
```

### Remote File Attachment

```html
<!-- Attach file from remote URL -->
<object data="https://example.com/files/whitepaper.pdf"
        type="application/pdf"
        data-icon="Paperclip"
        alt="Industry Whitepaper"></object>

<!-- Attach from API endpoint -->
<object data="https://api.example.com/documents/{{model.docId}}/download"
        type="application/pdf"
        data-icon="Paperclip"
        alt="Generated Report"></object>
```

### Image Attachments

```html
<div class="image-attachments">
    <h3>Original Image Files</h3>

    <!-- High-resolution image attachment -->
    <object data="photos/highres-photo.png"
            type="image/png"
            data-icon="Tag"
            alt="High Resolution Photo"></object>

    <!-- TIFF image attachment -->
    <object data="scans/document-scan.tiff"
            type="image/tiff"
            data-icon="Tag"
            alt="Scanned Document"></object>

    <!-- SVG source file -->
    <object data="graphics/logo.svg"
            type="image/svg+xml"
            data-icon="Tag"
            alt="Vector Logo Source"></object>
</div>
```

### Data Files Attachment

```html
<div class="data-files">
    <h3>Supporting Data Files</h3>

    <!-- CSV data -->
    <div style="margin: 10pt 0;">
        <object data="data/sales-data.csv"
                type="text/csv"
                data-icon="Graph"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Sales Data (CSV)</span>
    </div>

    <!-- JSON data -->
    <div style="margin: 10pt 0;">
        <object data="data/configuration.json"
                type="application/json"
                data-icon="Tag"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Configuration Data (JSON)</span>
    </div>

    <!-- XML data -->
    <div style="margin: 10pt 0;">
        <object data="data/export.xml"
                type="text/xml"
                data-icon="Tag"
                style="vertical-align: middle; margin-right: 5pt;"></object>
        <span>Data Export (XML)</span>
    </div>
</div>
```

### Contract with Attachments

```html
<!DOCTYPE html>
<html>
<head>
    <title>Service Agreement</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40pt; }
        .attachment-section {
            margin-top: 30pt;
            padding: 20pt;
            background-color: #f9f9f9;
            border: 1pt solid #ddd;
        }
    </style>
</head>
<body>
    <h1>Service Agreement</h1>
    <p>This agreement is made between...</p>

    <!-- Contract content -->
    <h2>Terms and Conditions</h2>
    <p>Contract terms here...</p>

    <!-- Attachments section -->
    <div class="attachment-section">
        <h3>Exhibit A - Technical Specifications</h3>
        <object data="exhibits/technical-specs.pdf"
                type="application/pdf"
                data-icon="Paperclip"
                alt="Technical Specifications"></object>

        <h3>Exhibit B - Price Schedule</h3>
        <object data="exhibits/pricing.xlsx"
                type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                data-icon="Graph"
                alt="Pricing Schedule"></object>

        <h3>Exhibit C - Service Level Agreement</h3>
        <object data="exhibits/sla.pdf"
                type="application/pdf"
                data-icon="Paperclip"
                alt="Service Level Agreement"></object>
    </div>

    <!-- Signature section -->
    <div style="margin-top: 40pt;">
        <p>Signed: _______________________</p>
        <p>Date: _______________________</p>
    </div>
</body>
</html>
```

### Research Paper with Supplementary Files

```html
<div class="paper">
    <h1>{{model.paperTitle}}</h1>
    <p class="authors">{{model.authors}}</p>

    <h2>Abstract</h2>
    <p>{{model.abstract}}</p>

    <h2>Content</h2>
    <p>{{model.content}}</p>

    <h2>Supplementary Materials</h2>
    <div style="background-color: #f0f0f0; padding: 15pt; margin: 15pt 0;">
        <!-- Dataset -->
        <div style="margin: 10pt 0;">
            <object data="supplementary/dataset.csv"
                    type="text/csv"
                    data-icon="Graph"
                    alt="Research Dataset"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>Dataset.csv</strong> - Complete research dataset
        </div>

        <!-- Analysis scripts -->
        <div style="margin: 10pt 0;">
            <object data="supplementary/analysis-scripts.zip"
                    type="application/zip"
                    data-icon="Tag"
                    alt="Analysis Scripts"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>analysis-scripts.zip</strong> - Statistical analysis code
        </div>

        <!-- Extended results -->
        <div style="margin: 10pt 0;">
            <object data="supplementary/extended-results.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    alt="Extended Results"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>extended-results.pdf</strong> - Complete result tables
        </div>
    </div>
</div>
```

### Invoice with Supporting Documents

```html
<div class="invoice">
    <h1>Invoice #{{model.invoiceNumber}}</h1>
    <p>Date: {{model.invoiceDate}}</p>

    <!-- Invoice content -->
    <table style="width: 100%; margin: 20pt 0;">
        <tr>
            <th>Description</th>
            <th>Amount</th>
        </tr>
        <tr>
            <td>{{model.description}}</td>
            <td>{{model.amount}}</td>
        </tr>
    </table>

    <!-- Attachments -->
    <div style="margin-top: 30pt; padding-top: 20pt; border-top: 2pt solid #333;">
        <h3>Supporting Documents</h3>

        <div style="margin: 10pt 0;">
            <object data="receipts/receipt-001.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    alt="Receipt 001"
                    style="margin-right: 5pt;"></object>
            Receipt for materials
        </div>

        <div style="margin: 10pt 0;">
            <object data="receipts/receipt-002.pdf"
                    type="application/pdf"
                    data-icon="Paperclip"
                    alt="Receipt 002"
                    style="margin-right: 5pt;"></object>
            Receipt for labor
        </div>

        <div style="margin: 10pt 0;">
            <object data="timesheets/timesheet.xlsx"
                    type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    data-icon="Graph"
                    alt="Timesheet"
                    style="margin-right: 5pt;"></object>
            Detailed timesheet
        </div>
    </div>
</div>
```

---

## See Also

- [iframe](/reference/htmltags/iframe.html) - Iframe element for embedded HTML content
- [embed](/reference/htmltags/embed.html) - Embed element for embedded HTML content
- [img](/reference/htmltags/img.html) - Image element
- [a](/reference/htmltags/a.html) - Anchor/link element
- [IconAttachment Component](/reference/components/iconattachment.html) - Base attachment component
- [PDF Attachments](/reference/pdf/attachments.html) - PDF attachment specifications
- [Data Binding](/reference/binding/) - Data binding and expressions
- [File Resources](/reference/resources/files.html) - Working with file resources

---
