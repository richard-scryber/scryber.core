---
layout: default
title: Attachments & Embedded Content
nav_order: 7
parent: Content Components
parent_url: /learning/06-content/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Attachments & Embedded Content

Learn how to embed files in PDFs, attach external documents, include modular content, and create self-contained PDF packages.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Attach files to PDFs
- Embed external content
- Include HTML fragments
- Create modular document structures
- Use iframe for content inclusion
- Build PDF portfolios
- Understand attachment limitations

---

## File Attachments

### Basic Attachment

File attachments allow you to embed external files within a PDF, making the PDF a container for multiple documents.

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document with Attachment</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            margin: 0;
        }
    </style>
</head>
<body>
    <h1>Contract Agreement</h1>

    <p>
        Please review the attached exhibits and supporting documentation.
    </p>

    <!-- Attach a file -->
    <object id="exhibit-a"
            data-file="./documents/exhibit-a.pdf"
            type="application/attachment"
            data-icon="PaperClip">
    </object>

    <p>
        <a href="#exhibit-a">View Exhibit A (attached)</a>
    </p>
</body>
</html>
```

**Note:** Attachment support and implementation may vary by PDF generator and viewer.

### Attachment Icons

```html
<!-- Different icon types -->
<object data-file="./docs/document.pdf"
        type="application/attachment"
        data-icon="PaperClip">  <!-- or "Graph", "PushPin", "Tag" -->
</object>
```

**Common Icons:**
- `PaperClip` - Default attachment icon
- `PushPin` - Pin icon
- `Graph` - Chart/graph icon
- `Tag` - Label/tag icon

---

## Dynamic Attachments

### Data-Bound Attachments

{% raw %}
```html
<h1>Invoice with Supporting Documents</h1>

<p>Attached files:</p>

<ul>
    {{#each attachments}}
    <li>
        <object id="attachment-{{@index}}"
                data-file="{{this.path}}"
                type="application/attachment"
                data-icon="PaperClip">
        </object>
        <a href="#attachment-{{@index}}">{{this.name}}</a>
    </li>
    {{/each}}
</ul>
```
{% endraw %}

### Conditional Attachments

{% raw %}
```html
{{#if invoice.hasReceipt}}
<p>Receipt attached:</p>
<object id="receipt"
        data-file="{{invoice.receiptPath}}"
        type="application/attachment"
        data-icon="PaperClip">
</object>
<a href="#receipt">View Receipt</a>
{{/if}}
```
{% endraw %}

---

## Content Inclusion

### Include HTML Fragments

While not direct file embedding, you can modularize content:

**header.html:**
```html
<header style="border-bottom: 2pt solid #2563eb; padding-bottom: 20pt;">
    <img src="./images/logo.png" alt="Logo" style="width: 150pt;" />
    <h1 style="font-size: 24pt; color: #1e40af;">Company Name</h1>
</header>
```

**main-document.html:**
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Main Document</title>
</head>
<body>
    <!-- Content inclusion depends on your PDF generator -->
    <!-- Some support <iframe>, <object>, or server-side includes -->

    <h1>Document Content</h1>
    <p>Main document body...</p>
</body>
</html>
```

---

## Practical Examples

### Example 1: Contract with Exhibits

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Contract Agreement</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Georgia, "Times New Roman", serif;
            font-size: 11pt;
            line-height: 1.7;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            margin: 0 0 20pt 0;
            color: #1e40af;
            text-align: center;
        }

        h2 {
            font-size: 18pt;
            margin: 30pt 0 15pt 0;
            color: #2563eb;
        }

        p {
            margin: 0 0 12pt 0;
            text-align: justify;
        }

        /* ==============================================
           ATTACHMENT SECTION
           ============================================== */
        .attachments {
            margin-top: 40pt;
            padding: 20pt;
            border: 2pt solid #e5e7eb;
            border-radius: 5pt;
            background-color: #f9fafb;
        }

        .attachments h3 {
            font-size: 14pt;
            margin: 0 0 15pt 0;
            color: #1e40af;
        }

        .attachment-item {
            padding: 10pt;
            margin-bottom: 10pt;
            background-color: white;
            border: 1pt solid #d1d5db;
            border-radius: 3pt;
        }

        .attachment-link {
            color: #2563eb;
            text-decoration: none;
            font-weight: 600;
        }

        .attachment-link:before {
            content: "📎 ";
        }

        .attachment-description {
            font-size: 10pt;
            color: #666;
            margin-top: 5pt;
        }

        /* ==============================================
           SIGNATURE SECTION
           ============================================== */
        .signatures {
            margin-top: 60pt;
            display: table;
            width: 100%;
        }

        .signature-block {
            display: table-cell;
            width: 50%;
            padding: 20pt;
            vertical-align: top;
        }

        .signature-line {
            border-top: 1pt solid #000;
            margin-top: 40pt;
            padding-top: 5pt;
        }

        .signature-label {
            font-size: 10pt;
            color: #666;
        }
    </style>
</head>
<body>
    <h1>Service Agreement</h1>

    <h2>Article 1: Scope of Services</h2>
    <p>
        The Service Provider agrees to provide the services outlined in Exhibit A attached hereto and incorporated by reference. The Client agrees to compensate the Service Provider according to the fee schedule detailed in Exhibit B.
    </p>

    <h2>Article 2: Term and Termination</h2>
    <p>
        This Agreement shall commence on the Effective Date and continue for a period of twelve (12) months, unless terminated earlier in accordance with the provisions set forth in Exhibit C.
    </p>

    <h2>Article 3: Confidentiality</h2>
    <p>
        Both parties agree to maintain confidentiality of proprietary information as detailed in the Non-Disclosure Agreement attached as Exhibit D.
    </p>

    <!-- Attachments Section -->
    <div class="attachments">
        <h3>Attached Exhibits</h3>

        <div class="attachment-item">
            <object id="exhibit-a"
                    data-file="./exhibits/scope-of-services.pdf"
                    type="application/attachment"
                    data-icon="PaperClip">
            </object>
            <a href="#exhibit-a" class="attachment-link">Exhibit A: Scope of Services</a>
            <div class="attachment-description">
                Detailed description of services to be provided, deliverables, and timelines.
            </div>
        </div>

        <div class="attachment-item">
            <object id="exhibit-b"
                    data-file="./exhibits/fee-schedule.pdf"
                    type="application/attachment"
                    data-icon="Graph">
            </object>
            <a href="#exhibit-b" class="attachment-link">Exhibit B: Fee Schedule</a>
            <div class="attachment-description">
                Payment terms, billing rates, and expense reimbursement policies.
            </div>
        </div>

        <div class="attachment-item">
            <object id="exhibit-c"
                    data-file="./exhibits/termination-terms.pdf"
                    type="application/attachment"
                    data-icon="Tag">
            </object>
            <a href="#exhibit-c" class="attachment-link">Exhibit C: Termination Terms</a>
            <div class="attachment-description">
                Conditions for early termination, notice requirements, and wind-down procedures.
            </div>
        </div>

        <div class="attachment-item">
            <object id="exhibit-d"
                    data-file="./exhibits/nda.pdf"
                    type="application/attachment"
                    data-icon="PushPin">
            </object>
            <a href="#exhibit-d" class="attachment-link">Exhibit D: Non-Disclosure Agreement</a>
            <div class="attachment-description">
                Confidentiality obligations, permitted disclosures, and term of confidentiality.
            </div>
        </div>
    </div>

    <!-- Signature Section -->
    <div class="signatures">
        <div class="signature-block">
            <div class="signature-line">
                <div class="signature-label">Service Provider</div>
            </div>
            <p style="margin-top: 20pt;">
                Date: _________________
            </p>
        </div>

        <div class="signature-block">
            <div class="signature-line">
                <div class="signature-label">Client</div>
            </div>
            <p style="margin-top: 20pt;">
                Date: _________________
            </div>
        </div>
    </div>
</body>
</html>
```

### Example 2: Research Report with Supplementary Materials

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Research Report</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            margin: 0 0 10pt 0;
            color: #1e40af;
        }

        .subtitle {
            font-size: 14pt;
            color: #666;
            margin: 0 0 30pt 0;
        }

        h2 {
            font-size: 18pt;
            margin: 30pt 0 15pt 0;
            color: #2563eb;
        }

        p {
            margin: 0 0 12pt 0;
        }

        /* ==============================================
           SUPPLEMENTARY MATERIALS
           ============================================== */
        .supplement-box {
            margin: 20pt 0;
            padding: 15pt;
            background-color: #eff6ff;
            border-left: 4pt solid #2563eb;
        }

        .supplement-title {
            font-weight: 700;
            color: #1e40af;
            margin-bottom: 10pt;
        }

        .supplement-list {
            list-style: none;
            padding-left: 0;
            margin: 0;
        }

        .supplement-list li {
            padding: 8pt;
            margin-bottom: 5pt;
            background-color: white;
            border-radius: 3pt;
        }

        .supplement-link {
            color: #2563eb;
            text-decoration: none;
            font-weight: 600;
        }
    </style>
</head>
<body>
    <h1>{{report.title}}</h1>
    <p class="subtitle">{{report.subtitle}}</p>

    <h2>Executive Summary</h2>
    <p>
        This research report presents findings from our comprehensive study on {{report.topic}}. The full dataset, methodology documentation, and statistical analysis scripts are attached for reproducibility and verification.
    </p>

    <h2>Methodology</h2>
    <p>
        Our research methodology is documented in detail in the attached Technical Appendix. All data collection instruments, sampling strategies, and analytical approaches are fully described.
    </p>

    <h2>Key Findings</h2>
    <p>
        {{report.findings}}
    </p>

    <h2>Supplementary Materials</h2>

    <div class="supplement-box">
        <div class="supplement-title">Attached Documents</div>

        <ul class="supplement-list">
            {{#each report.attachments}}
            <li>
                <object id="attachment-{{@index}}"
                        data-file="{{this.filePath}}"
                        type="application/attachment"
                        data-icon="{{this.icon}}">
                </object>
                <a href="#attachment-{{@index}}" class="supplement-link">
                    {{this.title}}
                </a>
                <div style="font-size: 10pt; color: #666; margin-top: 5pt;">
                    {{this.description}}
                </div>
            </li>
            {{/each}}
        </ul>
    </div>

    <h2>Conclusions</h2>
    <p>
        {{report.conclusions}}
    </p>

    <h2>References</h2>
    <p style="font-size: 10pt;">
        {{report.references}}
    </p>
</body>
</html>
```
{% endraw %}

---

## Try It Yourself

### Exercise 1: Invoice with Receipt

Create an invoice that:
- Shows line items and totals
- Attaches a PDF receipt
- Includes payment confirmation
- Links to the attachment

### Exercise 2: Proposal with Appendices

Build a business proposal with:
- Main proposal content
- Multiple attached appendices (specs, pricing, timeline)
- Links to each attachment
- Styled attachment section

### Exercise 3: Report Package

Design a comprehensive report with:
- Executive summary in main PDF
- Detailed data tables attached
- Chart images attached
- Methodology document attached
- Data-bound attachment list

---

## Common Pitfalls

### ❌ Invalid File Paths

```html
<!-- Relative path may not resolve -->
<object data-file="documents/file.pdf"
        type="application/attachment">
</object>
```

✅ **Solution:**

```html
<!-- Use absolute or properly resolved paths -->
<object data-file="./documents/file.pdf"
        type="application/attachment">
</object>
```

### ❌ Missing File

```html
<!-- File doesn't exist at specified path -->
<object data-file="./missing-file.pdf"
        type="application/attachment">
</object>
```

✅ **Solution:**

- Verify file exists before generation
- Use conditional attachment with data binding
- Provide fallback or error handling

### ❌ Large File Sizes

```html
<!-- Attaching huge files bloats PDF -->
<object data-file="./huge-video.mp4"
        type="application/attachment">
</object>
```

✅ **Solution:**

- Compress attachments before embedding
- Consider external hosting for large files
- Link to external resources instead
- Limit attachment file sizes

---

## Attachment Limitations

### PDF Generator Support

- ✅ Most PDF generators support basic attachments
- ⚠️ Icon support varies by generator
- ⚠️ Some PDF viewers don't show attachments well
- ⚠️ Mobile PDF viewers have limited support

### File Type Restrictions

- ✅ PDF attachments widely supported
- ✅ Images (PNG, JPEG) usually supported
- ⚠️ Office documents (DOCX, XLSX) may have issues
- ⚠️ Executables often blocked for security

### Best Practices

1. **Test Attachments** - Verify in target PDF viewer
2. **Limit File Sizes** - Keep under 10MB per attachment
3. **Use PDF Format** - Most compatible for attachments
4. **Provide Links** - Always include clickable links
5. **Document Attachments** - List what's attached
6. **Alternative Access** - Provide external download links

---

## Attachment Checklist

- [ ] File paths are correct and accessible
- [ ] Files exist at specified locations
- [ ] File sizes are reasonable (< 10MB each)
- [ ] Attachment IDs are unique
- [ ] Links to attachments work
- [ ] Icons are appropriate
- [ ] Tested in target PDF viewer
- [ ] Alternative access method provided

---

## Best Practices

1. **Verify Files Exist** - Check before PDF generation
2. **Use Relative Paths** - More portable than absolute
3. **Limit File Sizes** - Compress attachments
4. **Provide Context** - Describe what's attached
5. **Test Thoroughly** - Different PDF viewers
6. **Security Consideration** - Don't attach executables
7. **Fallback Options** - External links as backup
8. **Document Structure** - Clear attachment organization

---

## Alternative Approaches

### External Hosting

Instead of embedding, link to externally hosted files:

```html
<p>
    Supporting documents:
    <ul>
        <li><a href="https://example.com/docs/appendix-a.pdf">Appendix A (PDF, 2.5 MB)</a></li>
        <li><a href="https://example.com/docs/data-table.xlsx">Data Tables (Excel, 1.8 MB)</a></li>
    </ul>
</p>
```

### QR Codes

Link to documents via QR codes (generate separately):

```html
<div style="text-align: center;">
    <p>Scan to access supplementary materials:</p>
    <img src="./qrcodes/supplements-qr.png"
         alt="QR Code"
         style="width: 100pt; height: 100pt;" />
</div>
```

---

## Next Steps

1. **[Content Best Practices](08_content_best_practices.md)** - Optimization and performance
2. **[Practical Applications](/learning/08-practical/)** - Real-world examples
3. **[Configuration](/learning/07-configuration/)** - Document settings

---

**Continue learning →** [Content Best Practices](08_content_best_practices.md)
