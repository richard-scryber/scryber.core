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

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---

## Summary

The `<frameset>` and `<frame>` elements are powerful components that enable merging existing PDFs with dynamically generated content, creating combined documents, and building complex multi-source PDF files. This allows you to overlay generated content onto existing PDFs, append documents, or create sophisticated document workflows.

--

## Usage

The `<frameset>` element acts as a container for multiple `<frame>` elements, where each frame can reference:
- An existing PDF file as the base document
- HTML/XHTML template files to merge
- Inline HTML content within the frame
- Remote or local file sources

### Key Capabilities

- **PDF Merging**: Combine existing PDFs with new content
- **Template Overlay**: Overlay generated content on top of existing PDF pages
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
| `data-over-repeat` | string | Controls how inner content overlays repeat on source pages. Options: `"None"`, `"First"`, `"Once"` (default), `"Last"`, `"Repeat"`. See Overlay Repeat Behavior section. |
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

### Overlay Repeat Behavior

When a frame has both a source file (typically a PDF) and inner content, the inner content acts as an overlay. The `data-over-repeat` attribute controls how this overlay repeats across pages:

| Value | Behavior |
|-------|----------|
| `"None"` | No overlay applied to any pages |
| `"First"` | Overlay applied only to the first page of the source |
| `"Once"` | **Default**. Overlay applied only to the first page of this frame's content |
| `"Last"` | Overlay applied only to the last page of the source |
| `"Repeat"` | Overlay applied to every page of the source |

**Example: Watermark on all pages**

{% raw %}
```html
<frameset>
    <frame src="document.pdf" data-over-repeat="Repeat">
        <html>
            <body>
                <div style="position: fixed; top: 50%; left: 50%;
                            transform: translate(-50%, -50%) rotate(-45deg);
                            font-size: 120pt; color: rgba(255, 0, 0, 0.15);">
                    {{model.status}}
                </div>
            </body>
        </html>
    </frame>
</frameset>
```
{% endraw %}

**Example: Header on first page only**

{% raw %}
```html
<frameset>
    <frame src="report.pdf" data-over-repeat="First">
        <html>
            <body>
                <div style="position: absolute; top: 20pt; left: 50pt; right: 50pt;">
                    <h1>{{model.reportTitle}}</h1>
                    <p>Generated: {{model.date}}</p>
                </div>
            </body>
        </html>
    </frame>
</frameset>
```
{% endraw %}

---

## Notes

### Important Limitations

1. **Single PDF Rule**: Only ONE PDF file can be referenced across all frames in a frameset
2. **Multiple References**: The same PDF can be referenced multiple times with different page selections
3. **Root Priority**: If no PDF is referenced, the first template becomes the root
4. **Order Matters**: Frames are processed in order, with later frames appending to eariler content.

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
{% raw %}<frameset>
    <!-- Existing PDF form -->
    <frame src="application-form.pdf"></frame>

    <!-- Last page generated content -->
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
</frameset>{% endraw %}
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
    <frame src="certificate-template.pdf">
        <!-- Overlay recipient information on the page -->
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
{% raw %}<frameset>
    <!-- Company letterhead PDF (first page only) -->
    <frame src="company-letterhead.pdf" data-page-start="0" data-page-count="1"></frame>

    <!-- Invoice content. -->
    <frame>
        <html><!-- automatically picks up the xhtml namespace from the outer document if set -->
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
</frameset>{% endraw %}
```

### Form Filling

```html
{% raw %}<frameset>
    <!-- Government or standard form PDF -->
    <frame src="tax-form.pdf">
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
</frameset>{% endraw %}
```

### Multi-Page Overlay

```html
<frameset>
    <!-- Base PDF with multiple pages -->
    <frame src="multi-page-template.pdf">
        <!-- First page overlay -->
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
{% raw %}<frameset>
    <frame src="main-document.pdf"></frame>

    <!-- Include cover page only if specified -->
    <frame src="cover-page.html"
           hidden="{{!model.includeCover ? 'hidden' : ''}}"></frame>

    <!-- Include terms only for certain contract types -->
    <frame src="standard-terms.html"
           hidden="{{model.contractType != 'standard' ? 'hidden' : ''}}"></frame>

    <frame src="enterprise-terms.html"
           hidden="{{model.contractType != 'enterprise' ? 'hidden' : ''}}"></frame>
</frameset>{% endraw %}
```

### Report with Executive Summary

```html
{% raw %}<frameset>
    <!-- Executive summary on company template -->
    <frame src="company-template.pdf" data-page-start="0" data-page-count="1">
        <html>
            <body style="margin: 150pt 50pt 50pt 50pt;">
                <h1>Executive Summary</h1>
                <p>{{model.executiveSummary}}</p>
            </body>
        </html>
    </frame>

    <!-- Detailed report content -->
    <frame src="detailed-report.html"></frame>
</frameset>{% endraw %}
```

### Contract Assembly

```html
{% raw %}<frameset>
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
</frameset>{% endraw %}
```

### Watermarking Existing PDF

```html
<frameset>
    <!-- Original PDF document -->
    <frame src="original-document.pdf">
        <!-- Watermark overlay -->
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
{% raw %}<frameset>
    <frame src="template.pdf" data-content="{{model.htmlContent}}" ></frame>
</frameset>{% endraw %}
```

### Multi-Language Documents

```html
{% raw %}<frameset>
    <!-- English version -->
    <frame src="content-en.html"
           hidden="{{model.language != 'en' ? 'hidden' : ''}}"></frame>

    <!-- Spanish version -->
    <frame src="content-es.html"
           hidden="{{model.language != 'es' ? 'hidden' : ''}}"></frame>

    <!-- French version -->
    <frame src="content-fr.html"
           hidden="{{model.language != 'fr' ? 'hidden' : ''}}"></frame>
</frameset>{% endraw %}
```

### Medical Records with Privacy Notice

```html
{% raw %}<frameset>
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
</frameset>{% endraw %}
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
{% raw %}<frameset>
    <!-- Badge template with design -->
    <frame src="badge-template.pdf">
        <html>
            <body>
                <template data-bind="{{model.attendees}}">
                    <!-- 4 badges per column, 3 columns -->
                    <!-- force a page break on each 12th -->
                    <div style="page-break-before: always" hidden="{{index() % 12 == 0 ? 'visible' : 'hidden' }}" ></div>

                    <var data-id="badge_column" data-value= "{{index() % 4}}" ></var>
                    <var data-id="badge_offset" data-value= "{{(index() mod 4) * 140}}"></var>

                <div style="position: relative; left: {{(badge_column * 200pt)}}; top: {{(badge_offset * 120pt)}};" >
                    <!-- Name -->
                    <div style="position: absolute; left: 20pt; top: 20pt;
                                font-size: 18pt; font-weight: bold; text-align: center;">
                        {{model.attendeeName}}
                    </div>

                    <!-- Company -->
                    <div style="position: absolute; left: 50%; top: 240pt;
                                font-size: 12pt; text-align: center;">
                        {{model.companyName}}
                    </div>

                    <!-- Badge type -->
                    <div style="position: absolute; left: 50%; top: 280pt;
                                font-size: 10pt; text-align: center; color: #666;">
                        {{model.badgeType}}
                    </div>

                    <!-- QR Code -->
                    <div style="position: absolute; left: 50%; top: 320pt;>
                        <img src="{{model.qrCodeUrl}}" style="width: 80pt; height: 80pt;"/>
                    </div>
                </div>
            </body>
        </html>
    </frame>

    <!-- Attendee information overlay -->
    <frame>

    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: Watermark on All Pages

```html
{% raw %}<frameset>
    <!-- Apply "CONFIDENTIAL" watermark to every page -->
    <frame src="document.pdf" data-over-repeat="Repeat">
        <html>
            <body>
                <div style="position: fixed; top: 50%; left: 50%;
                            transform: translate(-50%, -50%) rotate(-45deg);
                            font-size: 96pt; font-weight: bold;
                            color: rgba(255, 0, 0, 0.1);
                            pointer-events: none;">
                    CONFIDENTIAL
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: Page Numbers on All Pages

```html
{% raw %}<frameset>
    <!-- Add page numbers to every page of existing PDF -->
    <frame src="legacy-document.pdf" data-over-repeat="Repeat">
        <html>
            <body>
                <div style="position: fixed; bottom: 20pt; right: 50pt;
                            font-size: 10pt; color: #666;">
                    Page <page-number /> of <page-count />
                </div>
                <div style="position: fixed; bottom: 20pt; left: 50pt;
                            font-size: 10pt; color: #666;">
                    Document #{{model.documentId}}
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: First Page Header

```html
{% raw %}<frameset>
    <!-- Add cover information only to first page -->
    <frame src="report.pdf" data-over-repeat="First">
        <html>
            <body>
                <div style="position: absolute; top: 20pt; left: 50pt; right: 50pt;
                            padding: 15pt; background-color: rgba(37, 99, 235, 0.1);
                            border-left: 4pt solid #2563eb;">
                    <h1 style="margin: 0; color: #2563eb;">{{model.title}}</h1>
                    <p style="margin: 5pt 0 0 0; font-size: 10pt; color: #666;">
                        Generated: {{model.date}} | Author: {{model.author}}
                    </p>
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: Last Page Footer

```html
{% raw %}<frameset>
    <!-- Add signature block only to last page -->
    <frame src="contract.pdf" data-over-repeat="Last">
        <html>
            <body>
                <div style="position: absolute; bottom: 100pt; left: 50pt; right: 50pt;">
                    <div style="display: table; width: 100%;">
                        <div style="display: table-row;">
                            <div style="display: table-cell; width: 50%; padding-right: 20pt;">
                                <div style="border-top: 1pt solid #000; padding-top: 5pt;">
                                    <strong>{{model.party1Name}}</strong><br/>
                                    Date: _________________
                                </div>
                            </div>
                            <div style="display: table-cell; width: 50%; padding-left: 20pt;">
                                <div style="border-top: 1pt solid #000; padding-top: 5pt;">
                                    <strong>{{model.party2Name}}</strong><br/>
                                    Date: _________________
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: Conditional Watermark

```html
{% raw %}<frameset>
    <!-- Apply different watermarks based on document status -->
    <frame src="document.pdf" data-over-repeat="Repeat"
           hidden="{{model.status != 'draft' ? 'hidden' : ''}}">
        <html>
            <body>
                <div style="position: fixed; top: 50%; left: 50%;
                            transform: translate(-50%, -50%) rotate(-45deg);
                            font-size: 96pt; color: rgba(128, 128, 128, 0.1);">
                    DRAFT
                </div>
            </body>
        </html>
    </frame>

    <frame src="document.pdf" data-over-repeat="Repeat"
           hidden="{{model.status != 'approved' ? 'hidden' : ''}}">
        <html>
            <body>
                <div style="position: fixed; top: 50%; left: 50%;
                            transform: translate(-50%, -50%) rotate(-45deg);
                            font-size: 96pt; color: rgba(0, 128, 0, 0.1);">
                    APPROVED
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: Bates Numbering

```html
{% raw %}<frameset>
    <!-- Add Bates numbering to each page -->
    <frame src="legal-document.pdf" data-over-repeat="Repeat">
        <html>
            <body>
                <div style="position: fixed; bottom: 10pt; right: 10pt;
                            font-family: Courier; font-size: 9pt;
                            background-color: white; padding: 2pt 5pt;
                            border: 1pt solid #000;">
                    {{model.batesPrefix}}<page-number format="000000" />
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: Dynamic Headers and Footers

```html
{% raw %}<frameset>
    <!-- Add repeating header/footer with dynamic content -->
    <frame src="presentation.pdf" data-over-repeat="Repeat">
        <html>
            <body>
                <!-- Header on every page -->
                <div style="position: fixed; top: 15pt; left: 50pt; right: 50pt;
                            border-bottom: 1pt solid #2563eb; padding-bottom: 5pt;">
                    <div style="display: table; width: 100%;">
                        <div style="display: table-row;">
                            <div style="display: table-cell; width: 70%;">
                                <span style="font-weight: bold; color: #2563eb;">
                                    {{model.companyName}}
                                </span>
                            </div>
                            <div style="display: table-cell; width: 30%; text-align: right;">
                                <span style="font-size: 9pt; color: #666;">
                                    {{model.presentationDate}}
                                </span>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Footer on every page -->
                <div style="position: fixed; bottom: 15pt; left: 50pt; right: 50pt;
                            border-top: 1pt solid #ccc; padding-top: 5pt;">
                    <div style="display: table; width: 100%;">
                        <div style="display: table-row;">
                            <div style="display: table-cell; width: 50%;">
                                <span style="font-size: 8pt; color: #666;">
                                    {{model.confidentialityNotice}}
                                </span>
                            </div>
                            <div style="display: table-cell; width: 50%; text-align: right;">
                                <span style="font-size: 9pt; color: #333;">
                                    Page <page-number /> of <page-count />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: First and Last Page Special Treatment

```html
{% raw %}<frameset>
    <!-- Cover page overlay (first page only) -->
    <frame src="blank-template.pdf" data-page-count="1" data-over-repeat="First">
        <html>
            <body style="background: linear-gradient(to bottom, #1e40af, #3b82f6);">
                <div style="position: absolute; top: 50%; left: 50%;
                            transform: translate(-50%, -50%); text-align: center;">
                    <h1 style="color: white; font-size: 36pt; margin: 0;">
                        {{model.reportTitle}}
                    </h1>
                    <p style="color: white; font-size: 14pt; margin-top: 20pt;">
                        {{model.subtitle}}
                    </p>
                </div>
            </body>
        </html>
    </frame>

    <!-- Main content pages (no overlay) -->
    <frame src="report-content.pdf" data-page-start="1"></frame>

    <!-- Back cover (last page only) -->
    <frame src="blank-template.pdf" data-page-count="1" data-over-repeat="Last">
        <html>
            <body style="background-color: #f3f4f6;">
                <div style="position: absolute; bottom: 50pt; left: 50pt; right: 50pt;
                            text-align: center;">
                    <p style="font-size: 10pt; color: #666;">
                        © {{model.year}} {{model.companyName}}. All rights reserved.
                    </p>
                    <p style="font-size: 9pt; color: #999; margin-top: 5pt;">
                        {{model.contactInfo}}
                    </p>
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: Data-Bound Conditional Control

```html
{% raw %}<frameset>
    <!-- Dynamically control overlay repeat based on data -->
    <frame src="document.pdf" data-over-repeat="{{if(model.showWatermark, 'Repeat', 'None')}}">
        <html>
            <body>
                <div style="position: fixed; top: 50%; left: 50%;
                            transform: translate(-50%, -50%) rotate(-45deg);
                            font-size: 96pt; color: rgba(255, 0, 0, 0.1);">
                    {{model.watermarkText}}
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: Dynamic Selection Based on Document Type

```html
{% raw %}<frameset>
    <!-- Use different overlay patterns based on document settings -->
    <frame src="document.pdf"
           data-over-repeat="{{if(model.headerOnAllPages, 'Repeat', if(model.headerOnFirstOnly, 'First', 'None'))}}">
        <html>
            <body>
                <div style="position: fixed; top: 20pt; left: 50pt; right: 50pt;
                            padding: 10pt; border-bottom: 2pt solid #2563eb;">
                    <h2 style="margin: 0; color: #2563eb;">{{model.documentTitle}}</h2>
                    <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
                        {{model.documentNumber}}
                    </p>
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: User Preference Controlled

```html
{% raw %}<frameset>
    <!-- Page numbers controlled by user preference -->
    <frame src="report.pdf"
           data-over-repeat="{{if(model.preferences.showPageNumbers, 'Repeat', 'None')}}">
        <html>
            <body>
                <div style="position: fixed; bottom: 20pt; text-align: center; width: 100%;">
                    <span style="font-size: 10pt; color: #666;">
                        Page <page-number /> of <page-count />
                    </span>
                </div>
            </body>
        </html>
    </frame>

    <!-- Confidentiality notice on first and last pages only -->
    <frame src="report.pdf"
           data-over-repeat="{{if(model.isConfidential, 'Repeat', 'None')}}">
        <html>
            <body>
                <div style="position: fixed; top: 10pt; right: 20pt;
                            background-color: #fee; padding: 5pt 10pt;
                            border: 2pt solid #f00; border-radius: 3pt;">
                    <strong style="color: #f00; font-size: 9pt;">
                        {{model.confidentialityLevel}}
                    </strong>
                </div>
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

### Overlay Repeat: Complex Conditional Logic

```html
{% raw %}<frameset>
    <!-- Different overlay behaviors based on multiple conditions -->
    <frame src="document.pdf"
           data-over-repeat="{{if(model.documentType == 'draft', 'Repeat',
                                if(model.documentType == 'final', 'First',
                                if(model.documentType == 'archived', 'None', 'Once')))}}">
        <html>
            <body>
                <!-- Draft: watermark on all pages -->
                {{#if model.documentType == 'draft'}}
                <div style="position: fixed; top: 50%; left: 50%;
                            transform: translate(-50%, -50%) rotate(-45deg);
                            font-size: 96pt; color: rgba(255, 165, 0, 0.1);">
                    DRAFT
                </div>
                {{/if}}

                <!-- Final: header on first page -->
                {{#if model.documentType == 'final'}}
                <div style="position: absolute; top: 30pt; left: 50pt; right: 50pt;
                            text-align: center; border-bottom: 2pt solid #10b981;">
                    <h1 style="color: #10b981; margin: 0;">FINAL VERSION</h1>
                    <p style="font-size: 10pt; color: #666; margin: 5pt 0;">
                        Approved: {{model.approvalDate}}
                    </p>
                </div>
                {{/if}}

                <!-- Default: simple identifier -->
                {{#if model.documentType != 'archived' && model.documentType != 'draft' && model.documentType != 'final'}}
                <div style="position: absolute; top: 20pt; right: 20pt;
                            font-size: 9pt; color: #666;">
                    ID: {{model.documentId}}
                </div>
                {{/if}}
            </body>
        </html>
    </frame>
</frameset>{% endraw %}
```

---

## See Also

- [Data Binding](/learing/binding/) - Complete guide to data binding expressions
- [template](html_template_element.html) - Template element for repeating content
- [if](html_if_element.html) - Conditional rendering element
- [Document Modification](/reference/modification/) - PDF modification and merging concepts
- [File Paths](/learning/templates/paths.html) - Path resolution and mapping

---
