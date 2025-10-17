---
layout: default
title: frameset and frame
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;frameset&gt; and &lt;frame&gt; : PDF Merging and Document Combination Elements

The `<frameset>` and `<frame>` elements are powerful Scryber components that enable merging existing PDFs with dynamically generated content, creating combined documents, and building complex multi-source PDF files. This allows you to overlay generated content onto existing PDFs, append documents, or create sophisticated document workflows.

## Usage

The `<frameset>` element acts as a container for multiple `<frame>` elements, where each frame can reference:
- An existing PDF file as the base document
- HTML/XHTML template files to merge
- Inline HTML content within the frame
- Remote or local file sources

### Key Capabilities

- **PDF Merging**: Combine existing PDFs with new content
- **Template Overlay**: Overlay generated content on existing PDF pages
- **Document Concatenation**: Append multiple documents together
- **Page Selection**: Extract specific pages from source PDFs
- **Dynamic Content**: Generate content that interacts with existing PDFs
- **Multi-Source**: Reference multiple templates and one PDF file

```html
<frameset>
    <!-- Base PDF file -->
    <frame src="template.pdf"></frame>

    <!-- Add content from HTML template -->
    <frame src="content.html"></frame>

    <!-- Inline content -->
    <frame>
        <html>
            <body>
                <div>Additional content</div>
            </body>
        </html>
    </frame>
</frameset>
```

---

## Supported Attributes

### &lt;frameset&gt; Attributes

The `<frameset>` element acts as a container and has no specific attributes beyond standard container properties.

| Attribute | Type | Description |
|-----------|------|-------------|
| None | - | Container element only, manages child frames |

### &lt;frame&gt; Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `src` | string | Path to the PDF or HTML template file. Can be relative or absolute path. |
| `type` | MIME type | Optional. Specifies the content type: "application/pdf" for PDFs or "text/html" for templates. Auto-detected if omitted. |
| `data-page-start` | integer | Zero-based index of the first page to include from the source. Default is 0 (first page). |
| `data-page-count` | integer | Number of pages to include from the source. Default is all pages (int.MaxValue). |
| `data-content` | string | Inline HTML content to use instead of a file source. |
| `hidden` | string | Set to "hidden" to skip this frame. Useful for conditional inclusion. |

---

## Frame Types and Behavior

### Root Frame (PDF Base)

The first PDF referenced in any frame becomes the **root document**. Only one PDF file can be referenced across all frames:

```html
<frameset>
    <!-- This becomes the root PDF -->
    <frame src="base-template.pdf"></frame>

    <!-- Additional content overlays on the PDF -->
    <frame src="overlay-content.html"></frame>
</frameset>
```

### Template Frames (HTML/XHTML)

Template frames are HTML documents that get merged with the root PDF:

```html
<frameset>
    <frame src="template.pdf"></frame>
    <frame src="page1-content.html"></frame>
    <frame src="page2-content.html"></frame>
</frameset>
```

### Inline Content Frames

Frames can contain inline HTML documents:

```html
<frameset>
    <frame src="template.pdf"></frame>
    <frame>
        <html>
            <body>
                <div style="padding: 20pt;">
                    <h1>Dynamic Content</h1>
                    <p>This content is generated inline.</p>
                </div>
            </body>
        </html>
    </frame>
</frameset>
```

---

## Notes

### Important Limitations

1. **Single PDF Rule**: Only ONE PDF file can be referenced across all frames in a frameset
2. **Multiple References**: The same PDF can be referenced multiple times with different page selections
3. **Root Priority**: If no PDF is referenced, the first template becomes the root
4. **Order Matters**: Frames are processed in order, with later frames overlaying earlier content

### Page Selection

Use `data-page-start` and `data-page-count` to select specific pages:

- `data-page-start="0"`: Start at first page (default)
- `data-page-count="1"`: Include only one page
- Omit `data-page-count` to include all pages from start index

### File Path Resolution

Paths in the `src` attribute can be:
- **Relative**: Resolved relative to the current document
- **Absolute**: Full file system or URL path
- **Virtual**: May use application-specific path mapping

### Async Loading

Frame content is loaded during the data binding phase and may use async operations for remote files.

---

## Examples

### Basic PDF with Overlay Content

```html
<frameset>
    <!-- Existing PDF form -->
    <frame src="application-form.pdf"></frame>

    <!-- Overlay generated data -->
    <frame>
        <html>
            <body>
                <div style="position: absolute; left: 100pt; top: 200pt;">
                    <span>{{model.applicantName}}</span>
                </div>
                <div style="position: absolute; left: 100pt; top: 250pt;">
                    <span>{{model.applicationDate}}</span>
                </div>
            </body>
        </html>
    </frame>
</frameset>
```

### Merging Multiple Templates

```html
<frameset>
    <!-- Cover page -->
    <frame src="cover-page.html"></frame>

    <!-- Table of contents -->
    <frame src="toc.html"></frame>

    <!-- Main content -->
    <frame src="main-content.html"></frame>

    <!-- Appendix -->
    <frame src="appendix.html"></frame>
</frameset>
```

### Extracting Specific PDF Pages

```html
<frameset>
    <!-- Use only pages 1-3 from source PDF (0-indexed) -->
    <frame src="large-document.pdf" data-page-start="0" data-page-count="3"></frame>

    <!-- Add summary page -->
    <frame src="summary.html"></frame>
</frameset>
```

### Certificate Generation

```html
<frameset>
    <!-- Pre-designed certificate template PDF -->
    <frame src="certificate-template.pdf"></frame>

    <!-- Overlay recipient information -->
    <frame>
        <html>
            <body>
                <!-- Recipient name -->
                <div style="position: absolute; left: 50%; top: 300pt;
                            transform: translateX(-50%);
                            font-size: 24pt; font-weight: bold;">
                    {{model.recipientName}}
                </div>

                <!-- Certificate number -->
                <div style="position: absolute; left: 100pt; bottom: 100pt;
                            font-size: 10pt; color: #666;">
                    Certificate #{{model.certificateNumber}}
                </div>

                <!-- Date -->
                <div style="position: absolute; right: 100pt; bottom: 100pt;
                            font-size: 10pt; color: #666;">
                    {{model.issueDate}}
                </div>
            </body>
        </html>
    </frame>
</frameset>
```

### Invoice with Letterhead

```html
<frameset>
    <!-- Company letterhead PDF (first page only) -->
    <frame src="company-letterhead.pdf" data-page-start="0" data-page-count="1"></frame>

    <!-- Invoice content -->
    <frame>
        <html>
            <body>
                <div style="margin: 150pt 50pt 50pt 50pt;">
                    <h1>Invoice #{{model.invoiceNumber}}</h1>

                    <table style="width: 100%; margin-top: 20pt;">
                        <thead>
                            <tr>
                                <th>Description</th>
                                <th style="text-align: right;">Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                            <template data-bind="{{model.items}}">
                                <tr>
                                    <td>{{.description}}</td>
                                    <td style="text-align: right;">${{.amount}}</td>
                                </tr>
                            </template>
                        </tbody>
                        <tfoot>
                            <tr style="font-weight: bold;">
                                <td>Total:</td>
                                <td style="text-align: right;">${{model.total}}</td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </body>
        </html>
    </frame>
</frameset>
```

### Form Filling

```html
<frameset>
    <!-- Government or standard form PDF -->
    <frame src="tax-form.pdf"></frame>

    <!-- Fill in form fields with absolute positioning -->
    <frame>
        <html>
            <body>
                <!-- Field 1: Name -->
                <div style="position: absolute; left: 120pt; top: 180pt;
                            font-family: Courier; font-size: 10pt;">
                    {{model.taxpayerName}}
                </div>

                <!-- Field 2: SSN -->
                <div style="position: absolute; left: 120pt; top: 220pt;
                            font-family: Courier; font-size: 10pt;">
                    {{model.ssn}}
                </div>

                <!-- Field 3: Income -->
                <div style="position: absolute; left: 400pt; top: 320pt;
                            font-family: Courier; font-size: 10pt; text-align: right;">
                    {{model.totalIncome}}
                </div>
            </body>
        </html>
    </frame>
</frameset>
```

### Multi-Page Overlay

```html
<frameset>
    <!-- Base PDF with multiple pages -->
    <frame src="multi-page-template.pdf"></frame>

    <!-- First page overlay -->
    <frame>
        <html>
            <body>
                <div style="position: absolute; right: 50pt; top: 50pt;">
                    <span style="color: red; font-weight: bold;">CONFIDENTIAL</span>
                </div>
            </body>
        </html>
    </frame>

    <!-- Additional content pages -->
    <frame src="appendix.html"></frame>
</frameset>
```

### Conditional Frame Inclusion

```html
<frameset>
    <frame src="main-document.pdf"></frame>

    <!-- Include cover page only if specified -->
    <frame src="cover-page.html"
           hidden="{{!model.includeCover ? 'hidden' : ''}}"></frame>

    <!-- Include terms only for certain contract types -->
    <frame src="standard-terms.html"
           hidden="{{model.contractType != 'standard' ? 'hidden' : ''}}"></frame>

    <frame src="enterprise-terms.html"
           hidden="{{model.contractType != 'enterprise' ? 'hidden' : ''}}"></frame>
</frameset>
```

### Report with Executive Summary

```html
<frameset>
    <!-- Executive summary on company template -->
    <frame src="company-template.pdf" data-page-start="0" data-page-count="1"></frame>

    <frame>
        <html>
            <body style="margin: 150pt 50pt 50pt 50pt;">
                <h1>Executive Summary</h1>
                <p>{{model.executiveSummary}}</p>
            </body>
        </html>
    </frame>

    <!-- Detailed report content -->
    <frame src="detailed-report.html"></frame>
</frameset>
```

### Contract Assembly

```html
<frameset>
    <!-- Standard contract first page -->
    <frame src="contract-template.pdf" data-page-start="0" data-page-count="1"></frame>

    <!-- Custom terms -->
    <frame>
        <html>
            <body>
                <div style="padding: 50pt;">
                    <h2>Agreement Terms</h2>

                    <p><strong>Party A:</strong> {{model.partyA}}</p>
                    <p><strong>Party B:</strong> {{model.partyB}}</p>
                    <p><strong>Effective Date:</strong> {{model.effectiveDate}}</p>

                    <h3>Terms and Conditions</h3>
                    <template data-bind="{{model.terms}}">
                        <div style="margin: 10pt 0;">
                            <strong>{{.title}}</strong>
                            <p>{{.content}}</p>
                        </div>
                    </template>
                </div>
            </body>
        </html>
    </frame>

    <!-- Standard contract last pages -->
    <frame src="contract-template.pdf" data-page-start="1"></frame>
</frameset>
```

### Watermarking Existing PDF

```html
<frameset>
    <!-- Original PDF document -->
    <frame src="original-document.pdf"></frame>

    <!-- Watermark overlay -->
    <frame>
        <html>
            <body>
                <div style="position: fixed; top: 50%; left: 50%;
                            transform: translate(-50%, -50%) rotate(-45deg);
                            font-size: 120pt; color: rgba(255, 0, 0, 0.15);
                            z-index: 1000; pointer-events: none;">
                    DRAFT
                </div>
            </body>
        </html>
    </frame>
</frameset>
```

### Dynamic Content Insertion

```html
<frameset>
    <frame src="template.pdf"></frame>

    <frame data-content="{{model.htmlContent}}"></frame>
</frameset>
```

### Multi-Language Documents

```html
<frameset>
    <!-- English version -->
    <frame src="content-en.html"
           hidden="{{model.language != 'en' ? 'hidden' : ''}}"></frame>

    <!-- Spanish version -->
    <frame src="content-es.html"
           hidden="{{model.language != 'es' ? 'hidden' : ''}}"></frame>

    <!-- French version -->
    <frame src="content-fr.html"
           hidden="{{model.language != 'fr' ? 'hidden' : ''}}"></frame>
</frameset>
```

### Purchase Order with Branding

```html
<frameset>
    <!-- Company branded header (page 1 of template) -->
    <frame src="company-branding.pdf" data-page-start="0" data-page-count="1"></frame>

    <!-- PO content -->
    <frame>
        <html>
            <body style="margin: 120pt 50pt 50pt 50pt;">
                <h1>Purchase Order #{{model.poNumber}}</h1>

                <div style="display: table; width: 100%; margin: 20pt 0;">
                    <div style="display: table-row;">
                        <div style="display: table-cell; width: 50%;">
                            <strong>Vendor:</strong><br/>
                            {{model.vendorName}}<br/>
                            {{model.vendorAddress}}
                        </div>
                        <div style="display: table-cell; width: 50%;">
                            <strong>Ship To:</strong><br/>
                            {{model.shipToName}}<br/>
                            {{model.shipToAddress}}
                        </div>
                    </div>
                </div>

                <table style="width: 100%; margin-top: 30pt;">
                    <thead>
                        <tr>
                            <th>Item</th>
                            <th>Description</th>
                            <th style="text-align: right;">Qty</th>
                            <th style="text-align: right;">Price</th>
                            <th style="text-align: right;">Total</th>
                        </tr>
                    </thead>
                    <tbody>
                        <template data-bind="{{model.lineItems}}">
                            <tr>
                                <td>{{.itemNumber}}</td>
                                <td>{{.description}}</td>
                                <td style="text-align: right;">{{.quantity}}</td>
                                <td style="text-align: right;">${{.unitPrice}}</td>
                                <td style="text-align: right;">${{.total}}</td>
                            </tr>
                        </template>
                    </tbody>
                </table>
            </body>
        </html>
    </frame>
</frameset>
```

### Medical Records with Privacy Notice

```html
<frameset>
    <!-- HIPAA privacy notice (required first page) -->
    <frame src="hipaa-notice.pdf" data-page-start="0" data-page-count="1"></frame>

    <!-- Patient information -->
    <frame>
        <html>
            <body style="padding: 50pt;">
                <h1>Patient Medical Record</h1>

                <div style="margin: 20pt 0;">
                    <strong>Patient:</strong> {{model.patientName}}<br/>
                    <strong>DOB:</strong> {{model.dateOfBirth}}<br/>
                    <strong>MRN:</strong> {{model.medicalRecordNumber}}
                </div>

                <h2>Visit History</h2>
                <template data-bind="{{model.visits}}">
                    <div style="margin: 15pt 0; padding: 10pt; border: 1pt solid #ccc;">
                        <strong>{{.visitDate}}</strong> - {{.visitType}}<br/>
                        <em>Provider:</em> {{.providerName}}<br/>
                        <p>{{.notes}}</p>
                    </div>
                </template>
            </body>
        </html>
    </frame>
</frameset>
```

### Paginated Report Assembly

```html
<frameset>
    <!-- Title page -->
    <frame src="title-page.html"></frame>

    <!-- Table of contents -->
    <frame src="toc-page.html"></frame>

    <!-- Chapter 1 -->
    <frame src="chapter-1.html"></frame>

    <!-- Chapter 2 -->
    <frame src="chapter-2.html"></frame>

    <!-- Chapter 3 -->
    <frame src="chapter-3.html"></frame>

    <!-- Bibliography from existing PDF -->
    <frame src="references.pdf" data-page-start="10" data-page-count="5"></frame>
</frameset>
```

### Ticket or Badge Generation

```html
<frameset>
    <!-- Badge template with design -->
    <frame src="badge-template.pdf"></frame>

    <!-- Attendee information overlay -->
    <frame>
        <html>
            <body>
                <!-- Name -->
                <div style="position: absolute; left: 50%; top: 200pt;
                            transform: translateX(-50%);
                            font-size: 18pt; font-weight: bold; text-align: center;">
                    {{model.attendeeName}}
                </div>

                <!-- Company -->
                <div style="position: absolute; left: 50%; top: 240pt;
                            transform: translateX(-50%);
                            font-size: 12pt; text-align: center;">
                    {{model.companyName}}
                </div>

                <!-- Badge type -->
                <div style="position: absolute; left: 50%; top: 280pt;
                            transform: translateX(-50%);
                            font-size: 10pt; text-align: center; color: #666;">
                    {{model.badgeType}}
                </div>

                <!-- QR Code -->
                <div style="position: absolute; left: 50%; top: 320pt;
                            transform: translateX(-50%);">
                    <img src="{{model.qrCodeUrl}}" style="width: 80pt; height: 80pt;"/>
                </div>
            </body>
        </html>
    </frame>
</frameset>
```

---

## See Also

- [Data Binding](/reference/binding/) - Complete guide to data binding expressions
- [template](/reference/htmltags/template.html) - Template element for repeating content
- [if](/reference/htmltags/if.html) - Conditional rendering element
- [Document Modification](/reference/modification/) - PDF modification and merging concepts
- [File Paths](/reference/paths/) - Path resolution and mapping
- [Remote Content](/reference/remote/) - Loading remote files and resources

---
