---
layout: default
title: object (attachment)
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;object type="application/attachment"&gt; : The File Attachment Element

Summary: The `<object>` element with `type="application/attachment"` embeds a local or remote file as an attachment in the PDF document. The attachment can be displayed with an icon and linked to, allowing users to extract and view the attached file from the PDF.

## Usage

The `<object>` element with `type="application/attachment"` allows you to attach files to a PDF document. The attached files are embedded in the PDF and can be accessed by PDF readers. You can display an icon for the attachment in the document and link to it.

```html
<object id="myAttachment"
        data="./documents/report.xlsx"
        type="application/attachment"
        data-icon="PaperClip"
        title="Excel Report"></object>
```

Remote files are also supported:

```html
<object id="remoteDoc"
        data="https://example.com/document.pdf"
        type="application/attachment"
        data-icon="PushPin"></object>
```

---

## Supported Attributes

- **data** - The path or URL to the file to attach (local or remote)
- **data-file** - Alternative to `data` attribute, useful for data binding
- **type** - Must be set to `"application/attachment"` for file attachments
- **data-icon** - The icon to display for the attachment. Values: `PushPin`, `Graph`, `PaperClip`, `Tag`, `None` (default: `PushPin`)
- **id** - Unique identifier for the attachment, used for linking
- **title** - Description or tooltip text for the attachment
- **hidden** - When set, the attachment icon is not displayed but the file is still embedded
- **data-never-cache** - When true, always reload the file instead of using cached version
- **style** - CSS styling for the attachment icon display

Standard CSS properties are supported for positioning and sizing the icon: `width`, `height`, `margin`, `padding`, `border`, `display`, `vertical-align`.

---

## Data Binding

The attachment element fully supports data binding for dynamic file paths and visibility control.

### Basic Data Binding

Bind the file path dynamically:

```html
<object id="invoiceAttachment"
        data-file="{{model.invoicePath}}"
        type="application/attachment"
        data-icon="PaperClip"></object>
```

### Conditional Visibility

Show or hide attachments based on data:

```html
<object id="optionalAttachment"
        data-file="{{model.attachmentPath}}"
        type="application/attachment"
        data-icon="PushPin"
        hidden="{{if(model.includeAttachment, '', 'hidden')}}"></object>
```

### Dynamic Icon and Title

Use data binding for attachment metadata:

```html
<object id="dynamicAttachment"
        data-file="{{model.filePath}}"
        type="application/attachment"
        data-icon="{{model.iconType}}"
        title="{{model.fileDescription}}"></object>
```

---

## Notes

- The attachment is embedded in the PDF document when it is generated
- Remote files (URLs) are fetched during document generation and embedded
- If the file cannot be loaded, a warning is logged but document generation continues
- Multiple attachments can be included in a single document
- When `data-icon="None"`, no icon is displayed but the file is still embedded
- Attachments without an icon can still be linked to using `<a href="#attachmentId">`
- The icon size defaults to the text height but can be controlled with `width` and `height` styles
- The `display` property can be set to `inline`, `inline-block`, or `block`
- Attached files can be extracted and opened from compatible PDF readers
- File paths are resolved relative to the document or as absolute paths/URLs

---

## Examples

### Example 1: Simple Local File Attachment

Attach a local PDF document with the default PushPin icon:

```html
<div>
    <p>See the attached document for more details.</p>
    <object id="details"
            data="./files/details.pdf"
            type="application/attachment"></object>
</div>
```

### Example 2: Attachment with Link

Display an attachment icon and provide a text link to it:

```html
<div style="margin: 10pt;">
    <object id="report"
            data="./reports/annual-report.pdf"
            type="application/attachment"
            data-icon="PaperClip"></object>
    <a href="#report">View Annual Report</a>
</div>
```

### Example 3: Remote File Attachment

Attach a file from a remote URL:

```html
<div>
    <p>Reference documentation:</p>
    <object id="apiDocs"
            data="https://api.example.com/docs/reference.pdf"
            type="application/attachment"
            data-icon="Graph"
            title="API Reference Documentation"></object>
</div>
```

### Example 4: Styled Attachment Icon

Apply custom styling to the attachment icon:

```html
<div style="padding: 10pt; background-color: #f0f0f0;">
    <object id="styledAttachment"
            data="./images/photo.jpg"
            type="application/attachment"
            data-icon="Tag"
            style="width: 30pt; height: 30pt; margin: 5pt; border: solid 2pt red;"></object>
    <span>Photo attachment</span>
</div>
```

### Example 5: Hidden Attachment (No Icon)

Embed a file without displaying an icon:

```html
<div>
    <object id="hiddenAttachment"
            data="./data/metadata.xml"
            type="application/attachment"
            data-icon="None"></object>
    <a href="#hiddenAttachment">Download Metadata</a>
</div>
```

### Example 6: Multiple Attachments

Include several attachments in a document:

```html
<div>
    <h2>Supporting Documents</h2>
    <ul>
        <li>
            <object id="contract"
                    data="./legal/contract.pdf"
                    type="application/attachment"
                    data-icon="PaperClip"></object>
            <a href="#contract">Contract</a>
        </li>
        <li>
            <object id="invoice"
                    data="./finance/invoice.pdf"
                    type="application/attachment"
                    data-icon="PaperClip"></object>
            <a href="#invoice">Invoice</a>
        </li>
        <li>
            <object id="receipt"
                    data="./finance/receipt.pdf"
                    type="application/attachment"
                    data-icon="PaperClip"></object>
            <a href="#receipt">Receipt</a>
        </li>
    </ul>
</div>
```

### Example 7: Data-Bound Attachment Path

Use data binding to specify the attachment file dynamically:

```html
<object id="customerDoc"
        data-file="{{model.documentPath}}"
        type="application/attachment"
        data-icon="PushPin"
        title="{{model.documentName}}"></object>
<a href="#customerDoc">{{model.documentName}}</a>
```

With data:
```json
{
    "model": {
        "documentPath": "./customers/customer-123.pdf",
        "documentName": "Customer Agreement"
    }
}
```

### Example 8: Conditional Attachment with Binding

Show the attachment only when data is available:

```html
<div>
    <object id="optionalReport"
            data-file="{{report.filePath}}"
            type="application/attachment"
            data-icon="Graph"
            hidden="{{if(report.filePath, '', 'hidden')}}"></object>
    <span>{{if(report.filePath, 'Report attached', 'No report available')}}</span>
</div>
```

With data:
```json
{
    "report": {
        "filePath": "./reports/q4-report.pdf"
    }
}
```

### Example 9: Different Icon Types

Demonstrate various icon types:

```html
<div style="line-height: 25pt;">
    <p>
        <object id="pin" data="./file1.pdf" type="application/attachment"
                data-icon="PushPin"></object> PushPin icon
    </p>
    <p>
        <object id="clip" data="./file2.pdf" type="application/attachment"
                data-icon="PaperClip"></object> PaperClip icon
    </p>
    <p>
        <object id="graph" data="./file3.pdf" type="application/attachment"
                data-icon="Graph"></object> Graph icon
    </p>
    <p>
        <object id="tag" data="./file4.pdf" type="application/attachment"
                data-icon="Tag"></object> Tag icon
    </p>
</div>
```

### Example 10: Attachment with Complex Link

Create a rich link to an attachment:

```html
<div>
    <object id="imageAttachment"
            data="./images/landscape.jpg"
            type="application/attachment"
            data-icon="None"></object>
    <a href="#imageAttachment" style="text-decoration: none;">
        <img src="./images/landscape-thumb.jpg" style="width: 50pt; height: 30pt;" />
        <span style="vertical-align: middle;">View full-size image</span>
        <span style="font-size: 8pt; font-style: italic; color: gray;"> (attachment)</span>
    </a>
</div>
```

### Example 11: Attachment in Repeating Template

Attach files from a list of documents:

```html
<h2>Project Files</h2>
<table>
    <thead>
        <tr>
            <th>Document</th>
            <th>Description</th>
            <th>Attachment</th>
        </tr>
    </thead>
    <tbody>
        {{#each documents}}
        <tr>
            <td>{{this.name}}</td>
            <td>{{this.description}}</td>
            <td>
                <object id="doc{{@index}}"
                        data-file="{{this.path}}"
                        type="application/attachment"
                        data-icon="PaperClip"></object>
                <a href="#doc{{@index}}">View</a>
            </td>
        </tr>
        {{/each}}
    </tbody>
</table>
```

With data:
```json
{
    "documents": [
        {"name": "Proposal", "description": "Project proposal", "path": "./docs/proposal.pdf"},
        {"name": "Budget", "description": "Budget breakdown", "path": "./docs/budget.xlsx"},
        {"name": "Timeline", "description": "Project timeline", "path": "./docs/timeline.pdf"}
    ]
}
```

### Example 12: Inline-Block Attachments

Display attachments as inline-block elements with spacing:

```html
<div style="padding: 10pt;">
    <object id="doc1"
            data="./file1.pdf"
            type="application/attachment"
            data-icon="PaperClip"
            style="display: inline-block; margin: 5pt; width: 25pt; height: 25pt;"></object>
    <object id="doc2"
            data="./file2.pdf"
            type="application/attachment"
            data-icon="PaperClip"
            style="display: inline-block; margin: 5pt; width: 25pt; height: 25pt;"></object>
    <object id="doc3"
            data="./file3.pdf"
            type="application/attachment"
            data-icon="PaperClip"
            style="display: inline-block; margin: 5pt; width: 25pt; height: 25pt;"></object>
</div>
```

### Example 13: Attachment with Vertical Alignment

Control the vertical alignment of inline attachment icons:

```html
<div style="line-height: 30pt; font-size: 16pt;">
    <span>Baseline: </span>
    <object id="baseline" data="./file.pdf" type="application/attachment"
            data-icon="Tag" style="vertical-align: baseline;"></object>
    <span> Top: </span>
    <object id="top" data="./file.pdf" type="application/attachment"
            data-icon="Tag" style="vertical-align: top;"></object>
    <span> Bottom: </span>
    <object id="bottom" data="./file.pdf" type="application/attachment"
            data-icon="Tag" style="vertical-align: bottom;"></object>
</div>
```

### Example 14: Attachment with Document Parameters

Store attachment information in document parameters:

```html
<var data-id="totalAttachments" data-value="3" />

<div>
    <p>This document includes {{Document.Params.totalAttachments}} attachments:</p>
    <object id="att1" data="./file1.pdf" type="application/attachment"
            data-icon="PaperClip"></object>
    <a href="#att1">Attachment 1</a><br/>
    <object id="att2" data="./file2.pdf" type="application/attachment"
            data-icon="PaperClip"></object>
    <a href="#att2">Attachment 2</a><br/>
    <object id="att3" data="./file3.pdf" type="application/attachment"
            data-icon="PaperClip"></object>
    <a href="#att3">Attachment 3</a>
</div>
```

### Example 15: Attachments with Custom Descriptions

Provide descriptive information for each attachment:

```html
<div>
    <h3>Technical Documentation</h3>
    <object id="userManual"
            data="./docs/user-manual.pdf"
            type="application/attachment"
            data-icon="Graph"
            title="Complete user manual with installation and configuration instructions"></object>
    <a href="#userManual">User Manual</a> - Installation and setup guide

    <br/><br/>

    <object id="apiRef"
            data="./docs/api-reference.pdf"
            type="application/attachment"
            data-icon="Graph"
            title="API reference documentation for developers"></object>
    <a href="#apiRef">API Reference</a> - Developer documentation
</div>
```

### Example 16: Dynamic Icon Selection

Choose the icon type based on data:

```html
{{#each files}}
<div style="margin: 5pt;">
    <object id="file{{@index}}"
            data-file="{{this.path}}"
            type="application/attachment"
            data-icon="{{this.iconType}}"
            title="{{this.description}}"></object>
    <a href="#file{{@index}}">{{this.name}}</a>
</div>
{{/each}}
```

With data:
```json
{
    "files": [
        {"name": "Report", "path": "./report.pdf", "iconType": "Graph", "description": "Annual report"},
        {"name": "Contract", "path": "./contract.pdf", "iconType": "PaperClip", "description": "Legal contract"},
        {"name": "Data", "path": "./data.csv", "iconType": "Tag", "description": "Raw data file"}
    ]
}
```

### Example 17: Attachment with Padding and Margins

Control spacing around the attachment icon:

```html
<div style="border: solid 1pt blue; padding: 10pt;">
    <p>
        Text before
        <object id="paddedAttachment"
                data="./document.pdf"
                type="application/attachment"
                data-icon="PushPin"
                style="margin: 10pt; padding: 5pt; border: solid 1pt red;"></object>
        Text after
    </p>
</div>
```

### Example 18: Never-Cache Remote Attachment

Always fetch the latest version of a remote file:

```html
<object id="liveData"
        data="https://api.example.com/data/latest.csv"
        type="application/attachment"
        data-icon="Graph"
        data-never-cache="true"
        title="Latest data feed (always fresh)"></object>
<a href="#liveData">Download Latest Data</a>
```

### Example 19: Attachments in Invoice with Totals

Combine attachments with document parameters in an invoice:

```html
<var data-id="invoiceNumber" data-value="{{invoice.number}}" />
<var data-id="hasAttachment" data-value="{{if(invoice.attachmentPath, 'Yes', 'No')}}" />

<div>
    <h1>Invoice #{{Document.Params.invoiceNumber}}</h1>
    <p>Total: ${{invoice.total}}</p>

    {{#if invoice.attachmentPath}}
    <div style="margin-top: 20pt; padding: 10pt; border: solid 1pt gray;">
        <p><strong>Supporting Documentation:</strong></p>
        <object id="invoiceAttachment"
                data-file="{{invoice.attachmentPath}}"
                type="application/attachment"
                data-icon="PaperClip"
                title="{{invoice.attachmentDescription}}"></object>
        <a href="#invoiceAttachment">{{invoice.attachmentDescription}}</a>
    </div>
    {{/if}}

    <footer style="margin-top: 30pt; font-size: 8pt; color: gray;">
        <p>Attachment included: {{Document.Params.hasAttachment}}</p>
    </footer>
</div>
```

With data:
```json
{
    "invoice": {
        "number": "INV-2025-001",
        "total": "1250.00",
        "attachmentPath": "./receipts/receipt-001.pdf",
        "attachmentDescription": "Itemized receipt"
    }
}
```

### Example 20: Conditional Multiple Attachments

Display attachments only when they exist in the data:

```html
<div>
    <h2>Available Documents</h2>

    {{#if resources.specification}}
    <div style="margin: 5pt;">
        <object id="spec"
                data-file="{{resources.specification}}"
                type="application/attachment"
                data-icon="Graph"></object>
        <a href="#spec">Technical Specification</a>
    </div>
    {{/if}}

    {{#if resources.manual}}
    <div style="margin: 5pt;">
        <object id="manual"
                data-file="{{resources.manual}}"
                type="application/attachment"
                data-icon="PaperClip"></object>
        <a href="#manual">User Manual</a>
    </div>
    {{/if}}

    {{#if resources.drawings}}
    <div style="margin: 5pt;">
        <object id="drawings"
                data-file="{{resources.drawings}}"
                type="application/attachment"
                data-icon="PushPin"></object>
        <a href="#drawings">Technical Drawings</a>
    </div>
    {{/if}}

    {{#unless resources.specification}}
    {{#unless resources.manual}}
    {{#unless resources.drawings}}
    <p style="color: gray; font-style: italic;">No attachments available</p>
    {{/unless}}
    {{/unless}}
    {{/unless}}
</div>
```

With data:
```json
{
    "resources": {
        "specification": "./docs/spec.pdf",
        "manual": "./docs/manual.pdf"
    }
}
```

---

## See Also

- [&lt;a&gt; - Hyperlinks](/reference/htmltags/a.html) for linking to attachments
- [&lt;img&gt; - Images](/reference/htmltags/img.html) for embedding images
- [&lt;var&gt; - Variables](/reference/htmltags/var.html) for storing document parameters
- [Data Binding](/reference/data-binding.html) for dynamic content
- [CSS Display Property](/reference/css/display.html) for controlling layout
- [PDF Attachments Guide](/guides/attachments.html) for detailed attachment usage

---
