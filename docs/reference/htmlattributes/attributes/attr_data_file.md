---
layout: default
title: data-file and data-file-data
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-file and @data-file-data : The File Attachment Data Attributes

The `data-file` and `data-file-data` attributes enable advanced file attachment capabilities in PDF documents, allowing direct binding of embedded file data objects and binary file content. These attributes provide powerful mechanisms for attaching files from memory, databases, APIs, or other dynamic sources.

## Summary

The `data-file` and `data-file-data` attributes work together to create PDF file attachments from various data sources:

- **`data-file`**: Binds directly to a `PDFEmbeddedFileData` object (advanced scenarios)
- **`data-file-data`**: Accepts binary file content as a byte array for dynamic attachments

These attributes are essential for:
- Attaching files from database BLOBs
- Embedding dynamically generated files
- Including API-retrieved file content
- Attaching files from memory without disk I/O
- Creating attachments from byte arrays in your data model
- Building document assembly systems with embedded resources

---

## Usage

Both attributes are used exclusively with the `<object>` element and support data binding:

```html
<!-- Attach file from byte array -->
<object data="filename.pdf"
        data-file-data="{{model.fileBytes}}"
        type="application/pdf"></object>

<!-- Bind to PDFEmbeddedFileData object -->
<object data="document.pdf"
        data-file="{{model.embeddedFile}}"
        type="application/pdf"></object>

<!-- Combine with other object attributes -->
<object data="{{model.fileName}}"
        data-file-data="{{model.fileContent}}"
        type="{{model.mimeType}}"
        data-icon="Paperclip"
        alt="{{model.description}}"></object>
```

---

## Supported Elements

These attributes are supported on:

### Object Element
- `<object>` - The object/attachment element (exclusive use)

These attributes are specifically designed for file attachment scenarios and work only with the `<object>` element.

---

## Binding Values

### data-file-data

The `data-file-data` attribute accepts binary file content:

**Type**: `byte[]` (byte array)
**Binding**: Supports data binding expressions
**Usage**: Provide raw file bytes for attachment

```html
<!-- Simple byte array binding -->
<object data="report.pdf"
        data-file-data="{{model.pdfBytes}}"
        type="application/pdf"></object>

<!-- From database result -->
<object data="{{.fileName}}"
        data-file-data="{{.fileContent}}"
        type="{{.mimeType}}"></object>

<!-- With conditional binding -->
<object data="{{model.name}}"
        data-file-data="{{model.useCache ? model.cachedBytes : model.freshBytes}}"
        type="application/pdf"></object>
```

**Data Model Example (C#)**:
```csharp
public class DocumentModel
{
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
    public string MimeType { get; set; }
    public string Description { get; set; }
}

// Usage
var model = new DocumentModel
{
    FileName = "report.pdf",
    FileContent = File.ReadAllBytes("path/to/file.pdf"),
    MimeType = "application/pdf",
    Description = "Monthly Report"
};
```

### data-file

The `data-file` attribute binds to embedded file data objects:

**Type**: `PDFEmbeddedFileData`
**Binding**: Binding-only attribute (not settable via static markup)
**Usage**: Advanced scenarios with pre-loaded file data objects

```html
<!-- Bind to pre-loaded embedded file object -->
<object data="document.pdf"
        data-file="{{model.embeddedFileObject}}"
        type="application/pdf"></object>
```

**Data Model Example (C#)**:
```csharp
public class AdvancedModel
{
    public PDFEmbeddedFileData EmbeddedFileObject { get; set; }
}

// Pre-processing
var embedded = PDFEmbeddedFileData.LoadFileFromData(
    context,
    fileBytes,
    "filename.pdf"
);
model.EmbeddedFileObject = embedded;
```

---

## Notes

### Attribute Priority

When multiple data sources are specified, they are evaluated in this order:

1. **`data-file`** - If set, this embedded file object is used directly
2. **`data-file-data`** - If `data-file` is null, binary data is processed
3. **`data` attribute** - If both above are null, file is loaded from path/URL

```html
<!-- This will use data-file-data if fileBytes is not null -->
<object data="fallback.pdf"
        data-file-data="{{model.fileBytes}}"
        type="application/pdf"></object>
```

### Filename from data Attribute

When using `data-file-data`, the `data` attribute value becomes the filename:

```html
<!-- The attachment will be named "monthly-report.pdf" -->
<object data="monthly-report.pdf"
        data-file-data="{{model.reportBytes}}"
        type="application/pdf"></object>

<!-- Dynamic filename -->
<object data="{{model.reportName}}.pdf"
        data-file-data="{{model.reportBytes}}"
        type="application/pdf"></object>
```

The `data` attribute is **required** even when using `data-file-data` to provide the attachment name.

### Binary Data Format

The `data-file-data` attribute expects raw binary file content:

- **Format**: Byte array (`byte[]`)
- **Source**: Any binary data source (file, database, API, memory)
- **Size**: No hard limit, but consider PDF file size implications
- **Encoding**: Raw binary, not base64 or other encodings
- **Validation**: Ensure data is valid for the specified MIME type

```csharp
// Load from file system
byte[] fileData = File.ReadAllBytes("document.pdf");

// Load from database
byte[] dbData = GetFileFromDatabase(documentId);

// Load from HTTP API
byte[] apiData = await httpClient.GetByteArrayAsync(apiUrl);

// From stream
using (var stream = GetFileStream())
{
    using (var memStream = new MemoryStream())
    {
        stream.CopyTo(memStream);
        byte[] streamData = memStream.ToArray();
    }
}
```

### MIME Type Requirement

Always specify the correct MIME type for proper file handling:

```html
<!-- PDF -->
<object data="file.pdf"
        data-file-data="{{model.bytes}}"
        type="application/pdf"></object>

<!-- Excel spreadsheet -->
<object data="data.xlsx"
        data-file-data="{{model.bytes}}"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"></object>

<!-- Image -->
<object data="photo.jpg"
        data-file-data="{{model.bytes}}"
        type="image/jpeg"></object>

<!-- Generic binary -->
<object data="file.bin"
        data-file-data="{{model.bytes}}"
        type="application/octet-stream"></object>
```

### Error Handling

File data loading follows standard error handling:

**Strict Conformance Mode**:
- Invalid binary data throws `PDFDataException`
- Missing or corrupt data stops document generation
- Detailed error messages indicate the problem

**Lax Conformance Mode**:
- Errors are logged but don't stop generation
- Attachment may be skipped on error
- Fallback content (if provided) is displayed

```html
<!-- With error fallback -->
<object data="report.pdf"
        data-file-data="{{model.reportBytes}}"
        type="application/pdf"
        data-icon="Paperclip">
    <div style="padding: 10pt; background-color: #fff3cd;">
        <strong>Note:</strong> Report attachment unavailable
    </div>
</object>
```

### Performance Considerations

When attaching large files or multiple attachments:

1. **Memory Usage**: Binary data is loaded into memory
2. **PDF Size**: Attachments increase output PDF file size
3. **Processing Time**: Large attachments take longer to embed
4. **Streaming**: Consider streaming for very large files
5. **Caching**: Cache frequently used attachments when possible

```csharp
// Example: Check file size before attaching
if (fileData.Length > 10 * 1024 * 1024) // 10MB
{
    // Log warning or use alternative approach
    logger.LogWarning($"Large attachment: {fileData.Length} bytes");
}
```

### Database Integration

Common pattern for database-stored files:

```csharp
// Database entity
public class AttachmentEntity
{
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
    public string ContentType { get; set; }
}

// Query and bind
var attachments = await db.Attachments
    .Where(a => a.ReportId == reportId)
    .ToListAsync();

var model = new
{
    Attachments = attachments
};
```

```html
<!-- Template usage -->
<template data-bind="{{model.Attachments}}">
    <object data="{{.FileName}}"
            data-file-data="{{.FileContent}}"
            type="{{.ContentType}}"
            data-icon="Paperclip"></object>
</template>
```

### Security Considerations

When accepting binary file data:

1. **Validate Content**: Verify file type matches declared MIME type
2. **Size Limits**: Enforce maximum file size limits
3. **Virus Scanning**: Scan user-uploaded content
4. **Content Filtering**: Filter dangerous file types
5. **Access Control**: Verify user permissions for file access

```csharp
// Example validation
public byte[] ValidateAndGetFileData(byte[] input, string declaredType)
{
    if (input == null || input.Length == 0)
        throw new ArgumentException("Empty file data");

    if (input.Length > MaxFileSize)
        throw new ArgumentException("File too large");

    // Verify file signature matches declared type
    if (!FileTypeValidator.IsValidType(input, declaredType))
        throw new ArgumentException("File type mismatch");

    return input;
}
```

### Null Data Handling

When binary data is null or empty:

```html
<!-- Safe binding with null check -->
<object data="{{model.fileName}}"
        data-file-data="{{model.fileBytes}}"
        type="application/pdf"
        hidden="{{model.fileBytes == null ? 'hidden' : ''}}"></object>
```

If `data-file-data` evaluates to null:
- The object attempts to load from the `data` attribute path
- If no file exists at the path, standard error handling applies
- Use `hidden` attribute to conditionally show/hide based on data availability

### Working with PDFEmbeddedFileData

Advanced scenarios using the `data-file` attribute:

```csharp
// Create embedded file data
var embeddedFile = PDFEmbeddedFileData.LoadFileFromData(
    context,
    fileBytes,
    "filename.pdf"
);

// Optionally set properties
embeddedFile.Description = "Monthly financial report";
embeddedFile.CreationDate = DateTime.Now;

// Add to model
model.EmbeddedFile = embeddedFile;
```

```html
<!-- Use in template -->
<object data="report.pdf"
        data-file="{{model.embeddedFile}}"
        type="application/pdf"
        data-icon="Paperclip"></object>
```

---

## Examples

### 1. Basic Byte Array Attachment

Simple file attachment from byte array:

```html
<!-- Model: { fileName: "document.pdf", fileData: byte[], mimeType: "application/pdf" } -->

<object data="{{model.fileName}}"
        data-file-data="{{model.fileData}}"
        type="{{model.mimeType}}"
        data-icon="Paperclip"></object>
```

### 2. Database BLOB Attachment

Attach files stored in database:

```csharp
// Database model
public class DocumentRecord
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] Content { get; set; }
    public string MimeType { get; set; }
}

// Load from database
var documents = await db.Documents
    .Where(d => d.CategoryId == categoryId)
    .ToListAsync();
```

```html
<!-- Template -->
<h2>Supporting Documents</h2>
<template data-bind="{{model.Documents}}">
    <div style="margin: 10pt 0;">
        <object data="{{.Name}}"
                data-file-data="{{.Content}}"
                type="{{.MimeType}}"
                data-icon="Paperclip"
                style="margin-right: 5pt;"></object>
        <span>{{.Name}}</span>
    </div>
</template>
```

### 3. API-Retrieved File Content

Attach files retrieved from external API:

```csharp
public class ReportModel
{
    public async Task LoadAttachments()
    {
        using (var client = new HttpClient())
        {
            // Download file from API
            FileBytes = await client.GetByteArrayAsync(
                $"https://api.example.com/files/{FileId}"
            );
        }
    }

    public string FileName { get; set; }
    public byte[] FileBytes { get; set; }
}
```

```html
<object data="{{model.FileName}}"
        data-file-data="{{model.FileBytes}}"
        type="application/pdf"
        data-icon="Paperclip"
        alt="API Retrieved Document"></object>
```

### 4. Dynamically Generated File

Attach programmatically generated file content:

```csharp
public class InvoiceModel
{
    public byte[] GenerateCSVAttachment()
    {
        var csv = new StringBuilder();
        csv.AppendLine("Date,Description,Amount");

        foreach (var item in LineItems)
        {
            csv.AppendLine($"{item.Date},{item.Description},{item.Amount}");
        }

        return Encoding.UTF8.GetBytes(csv.ToString());
    }

    public List<LineItem> LineItems { get; set; }
    public byte[] CSVData => GenerateCSVAttachment();
}
```

```html
<h2>Invoice Details</h2>
<!-- Invoice content here -->

<div style="margin-top: 20pt;">
    <h3>Attachments</h3>
    <object data="invoice-details.csv"
            data-file-data="{{model.CSVData}}"
            type="text/csv"
            data-icon="Graph"
            alt="Invoice Line Items CSV"></object>
</div>
```

### 5. Conditional File Attachment

Attach files conditionally based on data availability:

```html
<!-- Model: { hasAttachment: true, fileName: "report.pdf", fileData: byte[] } -->

<div class="report">
    <h2>Monthly Report</h2>
    <p>Report content...</p>

    <object data="{{model.fileName}}"
            data-file-data="{{model.fileData}}"
            type="application/pdf"
            data-icon="Paperclip"
            hidden="{{model.hasAttachment ? '' : 'hidden'}}"></object>
</div>
```

### 6. Multiple File Attachments from Array

Attach multiple files from byte array collection:

```csharp
public class AttachmentInfo
{
    public string FileName { get; set; }
    public byte[] Content { get; set; }
    public string MimeType { get; set; }
    public string Description { get; set; }
}

public class ReportModel
{
    public List<AttachmentInfo> Attachments { get; set; }
}
```

```html
<div class="attachments-section">
    <h3>Attached Files</h3>
    <template data-bind="{{model.Attachments}}">
        <div style="border-bottom: 1pt solid #ddd; padding: 10pt;">
            <object data="{{.FileName}}"
                    data-file-data="{{.Content}}"
                    type="{{.MimeType}}"
                    data-icon="Paperclip"
                    alt="{{.Description}}"
                    style="vertical-align: middle; margin-right: 5pt;"></object>
            <strong>{{.FileName}}</strong><br/>
            <span style="color: #666; font-size: 9pt;">{{.Description}}</span>
        </div>
    </template>
</div>
```

### 7. File from MemoryStream

Attach file content from memory stream:

```csharp
public class DocumentGenerator
{
    public byte[] GeneratePDFContent()
    {
        using (var stream = new MemoryStream())
        {
            // Generate PDF content
            var doc = new PDFDocument();
            // ... add content ...
            doc.SaveAs(stream);
            return stream.ToArray();
        }
    }
}

public class Model
{
    public byte[] GeneratedPDF { get; set; }
}
```

```html
<object data="generated-report.pdf"
        data-file-data="{{model.GeneratedPDF}}"
        type="application/pdf"
        data-icon="Paperclip"
        alt="Generated Report"></object>
```

### 8. Image File from Byte Array

Attach image files from binary data:

```csharp
public class ImageAttachment
{
    public string FileName { get; set; }
    public byte[] ImageData { get; set; }
    public string Format { get; set; } // "jpeg", "png", etc.

    public string MimeType => $"image/{Format}";
}
```

```html
<div class="image-gallery">
    <h3>High-Resolution Images</h3>
    <template data-bind="{{model.Images}}">
        <object data="{{.FileName}}"
                data-file-data="{{.ImageData}}"
                type="{{.MimeType}}"
                data-icon="Tag"
                alt="High Resolution {{.FileName}}"></object>
    </template>
</div>
```

### 9. Compressed File Attachment

Attach ZIP archives from byte arrays:

```csharp
public class SourceCodePackage
{
    public byte[] CreateZipArchive()
    {
        using (var stream = new MemoryStream())
        {
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                // Add files to archive
                foreach (var file in SourceFiles)
                {
                    var entry = archive.CreateEntry(file.Name);
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(file.Content, 0, file.Content.Length);
                    }
                }
            }
            return stream.ToArray();
        }
    }

    public List<SourceFile> SourceFiles { get; set; }
    public byte[] ZipData => CreateZipArchive();
}
```

```html
<object data="source-code.zip"
        data-file-data="{{model.ZipData}}"
        type="application/zip"
        data-icon="Tag"
        alt="Source Code Archive"></object>
```

### 10. Excel Spreadsheet from Byte Array

Attach Excel files stored as binary:

```csharp
public class ExcelReport
{
    public byte[] CreateSpreadsheet()
    {
        using (var stream = new MemoryStream())
        {
            // Use library like EPPlus or ClosedXML
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("Data");
                // ... populate data ...
                package.SaveAs(stream);
            }
            return stream.ToArray();
        }
    }

    public byte[] SpreadsheetData => CreateSpreadsheet();
}
```

```html
<object data="financial-data.xlsx"
        data-file-data="{{model.SpreadsheetData}}"
        type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        data-icon="Graph"
        alt="Financial Data Spreadsheet"></object>
```

### 11. Fallback Content with Binary Data

Provide fallback when binary data is unavailable:

```html
<object data="{{model.attachmentName}}"
        data-file-data="{{model.attachmentBytes}}"
        type="application/pdf"
        data-icon="Paperclip">
    <div style="padding: 15pt; border: 1pt solid #ccc; background-color: #f9f9f9;">
        <strong>Attachment: {{model.attachmentName}}</strong>
        <p>
            This document includes an attachment.
            If the paperclip icon appears, click it to open the attached file.
        </p>
        <p style="color: #666; font-size: 9pt;">
            File type: PDF | Size: {{model.attachmentSize}} KB
        </p>
    </div>
</object>
```

### 12. Certificate/Credential Attachment

Attach security certificates or credentials:

```csharp
public class CertificateAttachment
{
    public string CertificateName { get; set; }
    public byte[] CertificateData { get; set; }
    public DateTime ExpirationDate { get; set; }
}
```

```html
<div class="certificates">
    <h3>Security Certificates</h3>
    <template data-bind="{{model.Certificates}}">
        <div style="margin: 15pt 0; padding: 10pt; border: 1pt solid #336699;">
            <object data="{{.CertificateName}}.cer"
                    data-file-data="{{.CertificateData}}"
                    type="application/x-x509-ca-cert"
                    data-icon="Tag"
                    style="margin-right: 5pt;"></object>
            <strong>{{.CertificateName}}</strong><br/>
            <span style="font-size: 9pt; color: #666;">
                Expires: {{.ExpirationDate}}
            </span>
        </div>
    </template>
</div>
```

### 13. JSON Configuration File

Attach JSON configuration as binary data:

```csharp
public class ConfigurationModel
{
    public byte[] GetConfigurationBytes()
    {
        var config = new
        {
            Version = "2.0",
            Settings = ApplicationSettings,
            Timestamp = DateTime.Now
        };

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return Encoding.UTF8.GetBytes(json);
    }

    public object ApplicationSettings { get; set; }
    public byte[] ConfigData => GetConfigurationBytes();
}
```

```html
<object data="configuration.json"
        data-file-data="{{model.ConfigData}}"
        type="application/json"
        data-icon="Tag"
        alt="Application Configuration"></object>
```

### 14. Signed Document with Signature File

Attach digital signature files:

```csharp
public class SignedDocument
{
    public byte[] DocumentBytes { get; set; }
    public byte[] SignatureBytes { get; set; }
    public string SignerName { get; set; }
    public DateTime SignedDate { get; set; }
}
```

```html
<div class="signed-document">
    <h2>Signed Contract</h2>
    <p>This contract was digitally signed by {{model.SignerName}} on {{model.SignedDate}}.</p>

    <div class="attachments">
        <h3>Attached Files</h3>

        <!-- Original document -->
        <div style="margin: 10pt 0;">
            <object data="contract.pdf"
                    data-file-data="{{model.DocumentBytes}}"
                    type="application/pdf"
                    data-icon="Paperclip"
                    alt="Contract Document"></object>
            Original Contract
        </div>

        <!-- Signature file -->
        <div style="margin: 10pt 0;">
            <object data="contract-signature.p7s"
                    data-file-data="{{model.SignatureBytes}}"
                    type="application/pkcs7-signature"
                    data-icon="Tag"
                    alt="Digital Signature"></object>
            Digital Signature
        </div>
    </div>
</div>
```

### 15. Medical Record with DICOM Attachments

Attach medical imaging or other specialized files:

```csharp
public class MedicalRecord
{
    public string PatientName { get; set; }
    public byte[] DicomImageData { get; set; }
    public byte[] ReportPDF { get; set; }
    public DateTime ScanDate { get; set; }
}
```

```html
<div class="medical-record">
    <h2>Patient: {{model.PatientName}}</h2>
    <p>Scan Date: {{model.ScanDate}}</p>

    <div class="attachments">
        <h3>Attached Files</h3>

        <!-- DICOM image -->
        <div style="margin: 10pt 0;">
            <object data="ct-scan.dcm"
                    data-file-data="{{model.DicomImageData}}"
                    type="application/dicom"
                    data-icon="Graph"
                    alt="CT Scan Image"></object>
            CT Scan (DICOM Format)
        </div>

        <!-- Report -->
        <div style="margin: 10pt 0;">
            <object data="radiology-report.pdf"
                    data-file-data="{{model.ReportPDF}}"
                    type="application/pdf"
                    data-icon="Paperclip"
                    alt="Radiology Report"></object>
            Radiology Report
        </div>
    </div>
</div>
```

### 16. Version-Controlled Document Attachments

Attach multiple versions of a document:

```csharp
public class DocumentVersion
{
    public int Version { get; set; }
    public byte[] Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Author { get; set; }
}

public class VersionedDocument
{
    public List<DocumentVersion> Versions { get; set; }
}
```

```html
<div class="document-versions">
    <h3>Document History</h3>
    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #f0f0f0;">
                <th style="padding: 8pt; text-align: left;">Version</th>
                <th style="padding: 8pt; text-align: left;">Date</th>
                <th style="padding: 8pt; text-align: left;">Author</th>
                <th style="padding: 8pt; text-align: left;">Attachment</th>
            </tr>
        </thead>
        <tbody>
            <template data-bind="{{model.Versions}}">
                <tr style="border-bottom: 1pt solid #ddd;">
                    <td style="padding: 8pt;">v{{.Version}}</td>
                    <td style="padding: 8pt;">{{.CreatedDate}}</td>
                    <td style="padding: 8pt;">{{.Author}}</td>
                    <td style="padding: 8pt;">
                        <object data="document-v{{.Version}}.pdf"
                                data-file-data="{{.Content}}"
                                type="application/pdf"
                                data-icon="Paperclip"
                                alt="Version {{.Version}}"></object>
                    </td>
                </tr>
            </template>
        </tbody>
    </table>
</div>
```

---

## See Also

- [object element](/reference/htmltags/object.html) - Object/attachment element documentation
- [data attribute](/reference/htmlattributes/data.html) - File path/URL attribute
- [data-icon attribute](/reference/htmlattributes/data-icon.html) - Attachment icon display
- [type attribute](/reference/htmlattributes/type.html) - MIME type specification
- [Data Binding](/reference/binding/) - Data binding and expressions
- [IconAttachment Component](/reference/components/iconattachment.html) - Base attachment component
- [PDF Attachments](/reference/pdf/attachments.html) - PDF attachment specifications

---
