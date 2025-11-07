---
layout: default
title: Certificate Template
nav_order: 4
parent: Practical Applications
parent_url: /learning/08-practical/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Certificate Template

Create professional certificates for awards, achievements, training completion, and recognition with elegant design and personalization.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Design elegant certificate layouts
- Use landscape orientation
- Add decorative borders and backgrounds
- Include dynamic recipient information
- Add signatures and seals
- Create various certificate styles

---

## Complete Certificate Template

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>{{certificate.title}}</title>
    <style>
        @page {
            size: 11in 8.5in landscape;
            margin: 0;
        }

        body {
            margin: 0;
            padding: 0;
            font-family: 'Georgia', 'Times New Roman', serif;
            position: relative;
            height: 8.5in;
            width: 11in;
        }

        /* Background and border */
        .certificate-container {
            position: relative;
            width: 100%;
            height: 100%;
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .certificate-inner {
            width: 10in;
            height: 7.5in;
            background: white;
            border: 20pt double #2563eb;
            padding: 40pt;
            position: relative;
            box-shadow: 0 10pt 30pt rgba(0, 0, 0, 0.2);
        }

        /* Decorative corners */
        .corner {
            position: absolute;
            width: 60pt;
            height: 60pt;
            border: 3pt solid #2563eb;
        }

        .corner-top-left {
            top: 30pt;
            left: 30pt;
            border-right: none;
            border-bottom: none;
        }

        .corner-top-right {
            top: 30pt;
            right: 30pt;
            border-left: none;
            border-bottom: none;
        }

        .corner-bottom-left {
            bottom: 30pt;
            left: 30pt;
            border-right: none;
            border-top: none;
        }

        .corner-bottom-right {
            bottom: 30pt;
            right: 30pt;
            border-left: none;
            border-top: none;
        }

        /* Header */
        .certificate-header {
            text-align: center;
            margin-bottom: 30pt;
        }

        .certificate-logo {
            height: 60pt;
            margin-bottom: 15pt;
        }

        .certificate-type {
            font-size: 14pt;
            color: #666;
            text-transform: uppercase;
            letter-spacing: 3pt;
            margin-bottom: 10pt;
        }

        .certificate-title {
            font-size: 32pt;
            font-weight: bold;
            color: #2563eb;
            margin: 0;
            font-family: 'Georgia', serif;
        }

        /* Decorative line */
        .decorative-line {
            width: 200pt;
            height: 2pt;
            background: linear-gradient(to right, transparent, #2563eb, transparent);
            margin: 20pt auto;
        }

        /* Main content */
        .certificate-content {
            text-align: center;
            margin: 30pt 0;
        }

        .presented-to {
            font-size: 14pt;
            color: #666;
            font-style: italic;
            margin-bottom: 15pt;
        }

        .recipient-name {
            font-size: 36pt;
            font-weight: bold;
            color: #1e3a8a;
            margin: 15pt 0;
            font-family: 'Georgia', serif;
            border-bottom: 2pt solid #2563eb;
            display: inline-block;
            padding-bottom: 5pt;
            min-width: 400pt;
        }

        .achievement-text {
            font-size: 13pt;
            line-height: 1.8;
            color: #333;
            margin: 25pt 60pt;
            text-align: center;
        }

        .achievement-details {
            font-size: 12pt;
            color: #555;
            margin: 20pt 0;
        }

        .achievement-details strong {
            color: #2563eb;
        }

        /* Date and location */
        .certificate-meta {
            display: flex;
            justify-content: space-around;
            margin: 30pt 80pt;
            font-size: 11pt;
            color: #666;
        }

        .meta-item {
            text-align: center;
        }

        .meta-label {
            font-weight: bold;
            color: #2563eb;
            margin-bottom: 5pt;
            text-transform: uppercase;
            font-size: 9pt;
            letter-spacing: 1pt;
        }

        .meta-value {
            font-size: 12pt;
            color: #333;
        }

        /* Signatures */
        .signatures {
            display: flex;
            justify-content: space-around;
            margin-top: 40pt;
            padding: 0 80pt;
        }

        .signature-block {
            text-align: center;
            flex: 1;
            margin: 0 20pt;
        }

        .signature-line {
            border-top: 2pt solid #333;
            width: 180pt;
            margin: 0 auto 10pt auto;
        }

        .signature-image {
            height: 50pt;
            margin-bottom: -15pt;
        }

        .signature-name {
            font-size: 12pt;
            font-weight: bold;
            color: #333;
            margin: 5pt 0;
        }

        .signature-title {
            font-size: 10pt;
            color: #666;
            font-style: italic;
        }

        /* Seal */
        .certificate-seal {
            position: absolute;
            bottom: 40pt;
            left: 40pt;
            width: 80pt;
            height: 80pt;
        }

        /* Footer */
        .certificate-footer {
            position: absolute;
            bottom: 15pt;
            left: 0;
            right: 0;
            text-align: center;
            font-size: 8pt;
            color: #999;
        }

        /* Ribbon decoration */
        .ribbon {
            position: absolute;
            top: 40pt;
            right: -10pt;
            width: 60pt;
            height: 80pt;
            background: linear-gradient(135deg, #2563eb 0%, #1e40af 100%);
            box-shadow: 0 4pt 8pt rgba(0, 0, 0, 0.2);
        }

        .ribbon::after {
            content: '';
            position: absolute;
            bottom: -15pt;
            left: 0;
            right: 0;
            height: 15pt;
            background: #1e40af;
            clip-path: polygon(0 0, 50% 100%, 100% 0);
        }

        /* Watermark */
        .watermark {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-size: 150pt;
            color: rgba(37, 99, 235, 0.03);
            font-weight: bold;
            z-index: 0;
            pointer-events: none;
        }
    </style>
</head>
<body>
    <div class="certificate-container">
        <div class="certificate-inner">
            <!-- Decorative corners -->
            <div class="corner corner-top-left"></div>
            <div class="corner corner-top-right"></div>
            <div class="corner corner-bottom-left"></div>
            <div class="corner corner-bottom-right"></div>

            <!-- Watermark -->
            <div class="watermark">{{certificate.watermarkText}}</div>

            <!-- Ribbon (optional) -->
            {{#if certificate.showRibbon}}
            <div class="ribbon"></div>
            {{/if}}

            <!-- Header -->
            <div class="certificate-header">
                {{#if organization.logo}}
                <img src="{{organization.logo}}" class="certificate-logo" alt="{{organization.name}}" />
                {{/if}}

                <p class="certificate-type">{{certificate.type}}</p>
                <h1 class="certificate-title">{{certificate.title}}</h1>
            </div>

            <div class="decorative-line"></div>

            <!-- Main Content -->
            <div class="certificate-content">
                <p class="presented-to">This certificate is proudly presented to</p>

                <div class="recipient-name">{{recipient.name}}</div>

                <p class="achievement-text">
                    {{certificate.achievementText}}
                </p>

                {{#if certificate.details}}
                <p class="achievement-details">
                    <strong>{{certificate.detailsLabel}}:</strong> {{certificate.details}}
                </p>
                {{/if}}

                {{#if certificate.additionalInfo}}
                <p class="achievement-details">
                    {{certificate.additionalInfo}}
                </p>
                {{/if}}
            </div>

            <!-- Date and Location -->
            <div class="certificate-meta">
                <div class="meta-item">
                    <div class="meta-label">Date</div>
                    <div class="meta-value">{{certificate.date}}</div>
                </div>

                {{#if certificate.location}}
                <div class="meta-item">
                    <div class="meta-label">Location</div>
                    <div class="meta-value">{{certificate.location}}</div>
                </div>
                {{/if}}

                {{#if certificate.credentialId}}
                <div class="meta-item">
                    <div class="meta-label">Credential ID</div>
                    <div class="meta-value">{{certificate.credentialId}}</div>
                </div>
                {{/if}}
            </div>

            <!-- Signatures -->
            <div class="signatures">
                {{#each signers}}
                <div class="signature-block">
                    <div class="signature-line">
                        {{#if this.signatureImage}}
                        <img src="{{this.signatureImage}}" class="signature-image" alt="Signature" />
                        {{/if}}
                    </div>
                    <div class="signature-name">{{this.name}}</div>
                    <div class="signature-title">{{this.title}}</div>
                </div>
                {{/each}}
            </div>

            <!-- Seal (optional) -->
            {{#if organization.seal}}
            <img src="{{organization.seal}}" class="certificate-seal" alt="Official Seal" />
            {{/if}}

            <!-- Footer -->
            <div class="certificate-footer">
                <p>{{organization.name}} | {{organization.website}}</p>
                {{#if certificate.verificationUrl}}
                <p>Verify at: {{certificate.verificationUrl}}</p>
                {{/if}}
            </div>
        </div>
    </div>
</body>
</html>
```
{% endraw %}

---

## C# Certificate Generator

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.Collections.Generic;
using System.IO;

public class CertificateGenerator
{
    private readonly string _templatePath;

    public CertificateGenerator(string templatePath)
    {
        _templatePath = templatePath;
    }

    public void GenerateCertificate(CertificateData certificateData, Stream output)
    {
        // Load template
        var doc = Document.ParseDocument(_templatePath);

        // Set metadata
        doc.Info.Title = $"{certificateData.Certificate.Title} - {certificateData.Recipient.Name}";
        doc.Info.Author = certificateData.Organization.Name;
        doc.Info.Subject = certificateData.Certificate.Type;
        doc.Info.CreationDate = DateTime.Now;

        // Bind data
        doc.Params["organization"] = certificateData.Organization;
        doc.Params["recipient"] = certificateData.Recipient;
        doc.Params["certificate"] = certificateData.Certificate;
        doc.Params["signers"] = certificateData.Signers;

        // Configure for high quality
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.ImageQuality = 95;  // High quality for certificates

        // Generate PDF
        doc.SaveAsPDF(output);
    }
}

// Data models
public class CertificateData
{
    public OrganizationInfo Organization { get; set; }
    public RecipientInfo Recipient { get; set; }
    public Certificate Certificate { get; set; }
    public List<Signer> Signers { get; set; } = new List<Signer>();
}

public class OrganizationInfo
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Seal { get; set; }
    public string Website { get; set; }
}

public class RecipientInfo
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string EmployeeId { get; set; }
}

public class Certificate
{
    public string Type { get; set; }  // "Certificate of Achievement", "Completion", etc.
    public string Title { get; set; }
    public string AchievementText { get; set; }
    public string Details { get; set; }
    public string DetailsLabel { get; set; }
    public string AdditionalInfo { get; set; }
    public string Date { get; set; }
    public string Location { get; set; }
    public string CredentialId { get; set; }
    public string VerificationUrl { get; set; }
    public string WatermarkText { get; set; }
    public bool ShowRibbon { get; set; }
}

public class Signer
{
    public string Name { get; set; }
    public string Title { get; set; }
    public string SignatureImage { get; set; }
}
```

---

## Usage Example

```csharp
// Create certificate data
var certificateData = new CertificateData
{
    Organization = new OrganizationInfo
    {
        Name = "Acme Professional Development Institute",
        Logo = "./images/acme-logo.png",
        Seal = "./images/official-seal.png",
        Website = "www.acme-institute.com"
    },
    Recipient = new RecipientInfo
    {
        Name = "Sarah Johnson",
        Email = "sarah.johnson@example.com",
        EmployeeId = "EMP-12345"
    },
    Certificate = new Certificate
    {
        Type = "Certificate of Completion",
        Title = "Certificate of Excellence",
        AchievementText = "For successfully completing the Advanced Leadership Development Program with distinction, demonstrating exceptional commitment to professional growth and outstanding leadership capabilities.",
        Details = "Advanced Leadership Development Program - Level 3",
        DetailsLabel = "Program",
        AdditionalInfo = "60 Hours | 6.0 CEUs",
        Date = "October 17, 2025",
        Location = "New York, NY",
        CredentialId = "CERT-2025-AL-001234",
        VerificationUrl = "verify.acme-institute.com/CERT-2025-AL-001234",
        WatermarkText = "ACME",
        ShowRibbon = true
    },
    Signers = new List<Signer>
    {
        new Signer
        {
            Name = "Dr. Michael Chen",
            Title = "Program Director",
            SignatureImage = "./images/chen-signature.png"
        },
        new Signer
        {
            Name = "Jane Doe",
            Title = "Chief Executive Officer",
            SignatureImage = "./images/doe-signature.png"
        }
    }
};

// Generate certificate
var generator = new CertificateGenerator("certificate-template.html");

using (var output = new FileStream("certificate-sarah-johnson.pdf", FileMode.Create))
{
    generator.GenerateCertificate(certificateData, output);
    Console.WriteLine($"Certificate generated for {certificateData.Recipient.Name}");
    Console.WriteLine($"Credential ID: {certificateData.Certificate.CredentialId}");
}
```

---

## Common Certificate Variations

### Variation 1: Training Certificate

{% raw %}
```html
<p class="achievement-text">
    Has successfully completed <strong>{{training.hours}} hours</strong> of training in
</p>

<div style="font-size: 18pt; font-weight: bold; color: #2563eb; margin: 20pt 0;">
    {{training.courseName}}
</div>

<p class="achievement-details">
    <strong>Topics Covered:</strong><br />
    {{#each training.topics}}
    • {{this}}<br />
    {{/each}}
</p>
```
{% endraw %}

### Variation 2: Award Certificate

{% raw %}
```html
<p class="presented-to">Is hereby recognized with the</p>

<div style="font-size: 28pt; font-weight: bold; color: #d97706; margin: 20pt 0;">
    {{award.title}}
</div>

<p class="achievement-text">
    In recognition of {{award.reason}}
</p>

<div style="text-align: center; margin: 25pt 0;">
    <img src="{{award.badgeImage}}" style="width: 100pt; height: 100pt;" />
</div>
```
{% endraw %}

### Variation 3: Membership Certificate

{% raw %}
```html
<p class="achievement-text">
    Is hereby recognized as an <strong>{{membership.level}}</strong> member of
</p>

<div style="font-size: 20pt; font-weight: bold; color: #2563eb; margin: 15pt 0;">
    {{organization.fullName}}
</div>

<div class="certificate-meta" style="margin-top: 30pt;">
    <div class="meta-item">
        <div class="meta-label">Member Since</div>
        <div class="meta-value">{{membership.startDate}}</div>
    </div>
    <div class="meta-item">
        <div class="meta-label">Valid Until</div>
        <div class="meta-value">{{membership.expiryDate}}</div>
    </div>
    <div class="meta-item">
        <div class="meta-label">Membership ID</div>
        <div class="meta-value">{{membership.id}}</div>
    </div>
</div>
```
{% endraw %}

---

## Design Tips & Tricks

### 1. Elegant Border Variations

```css
/* Classic double border */
.certificate-inner {
    border: 20pt double #2563eb;
}

/* Ornate border */
.certificate-inner {
    border: 15pt solid #2563eb;
    border-image: url('./images/ornate-border.png') 30 repeat;
}

/* Gold foil effect */
.certificate-inner {
    border: 10pt solid;
    border-image: linear-gradient(135deg, #ffd700, #ffed4e, #ffd700) 1;
}
```

### 2. Background Patterns

```css
/* Subtle pattern */
.certificate-inner {
    background-image: url('./images/certificate-pattern.png');
    background-size: 200pt;
    background-repeat: repeat;
    background-opacity: 0.03;
}

/* Gradient background */
.certificate-container {
    background: linear-gradient(135deg,
        #667eea 0%,
        #764ba2 50%,
        #f093fb 100%);
}
```

### 3. Signature Styles

{% raw %}
```html
<!-- Handwritten signature with typed name below -->
<div class="signature-block">
    <img src="{{this.signatureImage}}"
         style="height: 50pt; filter: brightness(0);" />
    <div class="signature-line"></div>
    <div class="signature-name">{{this.name}}</div>
    <div class="signature-title">{{this.title}}</div>
</div>

<!-- Digital signature with date -->
<div class="signature-block">
    <div style="font-family: 'Brush Script MT', cursive; font-size: 20pt;">
        {{this.name}}
    </div>
    <div class="signature-line"></div>
    <div class="signature-title">
        {{this.title}}<br />
        Digitally signed on {{certificate.date}}
    </div>
</div>
```
{% endraw %}

### 4. QR Code for Verification

```html
<div style="position: absolute; bottom: 20pt; right: 20pt; text-align: center;">
    <img src="{{certificate.qrCode}}"
         style="width: 60pt; height: 60pt; border: 2pt solid #2563eb;" />
    <p style="font-size: 7pt; margin-top: 5pt; color: #666;">
        Scan to verify
    </p>
</div>
```

---

## Bulk Certificate Generation

```csharp
public class BulkCertificateGenerator
{
    private readonly CertificateGenerator _generator;
    private readonly string _outputDirectory;

    public BulkCertificateGenerator(string templatePath, string outputDirectory)
    {
        _generator = new CertificateGenerator(templatePath);
        _outputDirectory = outputDirectory;

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }
    }

    public void GenerateCertificates(
        OrganizationInfo organization,
        Certificate certificateTemplate,
        List<Signer> signers,
        List<RecipientInfo> recipients)
    {
        int count = 0;

        foreach (var recipient in recipients)
        {
            var certificateData = new CertificateData
            {
                Organization = organization,
                Recipient = recipient,
                Certificate = CloneCertificate(certificateTemplate, recipient),
                Signers = signers
            };

            // Generate unique credential ID
            certificateData.Certificate.CredentialId =
                $"CERT-{DateTime.Now.Year}-{(count + 1):D6}";

            // Generate filename
            var filename = $"{recipient.Name.Replace(" ", "-")}-{certificateData.Certificate.CredentialId}.pdf";
            var filepath = Path.Combine(_outputDirectory, filename);

            using (var output = new FileStream(filepath, FileMode.Create))
            {
                _generator.GenerateCertificate(certificateData, output);
                count++;
                Console.WriteLine($"Generated {count}/{recipients.Count}: {filename}");
            }
        }

        Console.WriteLine($"\nCompleted: {count} certificates generated in {_outputDirectory}");
    }

    private Certificate CloneCertificate(Certificate template, RecipientInfo recipient)
    {
        // Clone and customize certificate for each recipient
        return new Certificate
        {
            Type = template.Type,
            Title = template.Title,
            AchievementText = template.AchievementText,
            Details = template.Details,
            DetailsLabel = template.DetailsLabel,
            Date = template.Date,
            Location = template.Location,
            WatermarkText = template.WatermarkText,
            ShowRibbon = template.ShowRibbon
        };
    }
}

// Usage
var recipients = new List<RecipientInfo>
{
    new RecipientInfo { Name = "John Smith", Email = "john@example.com" },
    new RecipientInfo { Name = "Jane Doe", Email = "jane@example.com" },
    new RecipientInfo { Name = "Bob Johnson", Email = "bob@example.com" }
};

var bulkGenerator = new BulkCertificateGenerator(
    "certificate-template.html",
    "./certificates"
);

bulkGenerator.GenerateCertificates(
    organization,
    certificateTemplate,
    signers,
    recipients
);
```

---

## Try It Yourself

### Exercise 1: Custom Design

Create a certificate with your own style:
- Choose a color scheme
- Design custom borders
- Add decorative elements
- Test with different text lengths

### Exercise 2: Multiple Templates

Build certificate templates for:
- Course completion
- Employee of the month
- Years of service
- Professional certification

### Exercise 3: Verification System

Implement certificate verification:
- Generate unique credential IDs
- Create QR codes linking to verification page
- Build verification database
- Create web page for verification

---

## Common Pitfalls

### ❌ Text Overflow

```html
<!-- Long name breaks layout -->
<div class="recipient-name">{{recipient.name}}</div>
```

✅ **Solution:**

```css
.recipient-name {
    font-size: 36pt;
    max-width: 600pt;
    word-wrap: break-word;
    overflow-wrap: break-word;
}
```

### ❌ Low-Quality Signature Images

```html
<!-- Pixelated signature -->
<img src="signature.jpg" style="height: 50pt;" />
```

✅ **Solution:**

```html
<!-- Use high-resolution PNG with transparency -->
<img src="signature.png"
     style="height: 50pt; image-rendering: high-quality;" />
```

### ❌ Poor Print Quality

```csharp
// Low quality output
doc.RenderOptions.ImageQuality = 70;
```

✅ **Solution:**

```csharp
// High quality for certificates
doc.RenderOptions.ImageQuality = 95;
doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
```

---

## Certificate Checklist

- [ ] Landscape orientation configured
- [ ] Organization logo included
- [ ] Recipient name prominently displayed
- [ ] Achievement clearly stated
- [ ] Date and location included
- [ ] Signatures added
- [ ] Credential ID generated
- [ ] Verification method provided
- [ ] High-quality image settings
- [ ] Print test completed

---

## Best Practices

1. **Landscape Orientation** - Standard for certificates
2. **High-Quality Images** - Use 300 DPI or higher
3. **Elegant Fonts** - Serif fonts for formal certificates
4. **Clear Hierarchy** - Recipient name should be most prominent
5. **Unique IDs** - Generate unique credential identifiers
6. **Verification** - Include QR code or URL
7. **Print Quality** - Test print before bulk generation
8. **Proper Spacing** - Allow white space for elegance

---

## Next Steps

1. **[Data-Driven Report](05_data_driven_report.md)** - Database integration
2. **[Catalog & Brochure](06_catalog_brochure.md)** - Product listings
3. **[Multi-Language & Branded Documents](08_multi_language_branded.md)** - Enterprise templates

---

**Continue learning →** [Data-Driven Report](05_data_driven_report.md)
