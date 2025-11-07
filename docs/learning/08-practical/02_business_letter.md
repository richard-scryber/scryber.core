---
layout: default
title: Business Letter
nav_order: 2
parent: Practical Applications
parent_url: /learning/08-practical/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Business Letter

Create professional business correspondence with letterhead, proper formatting, signature blocks, and multi-page handling.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Design professional letterhead
- Format address blocks properly
- Structure letter body content
- Add signature blocks with images
- Handle multi-page letters
- Include headers and footers

---

## Complete Business Letter Template

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>{{letter.subject}}</title>
    <style>
        @page {
            size: letter;
            margin: 72pt 72pt 100pt 72pt;
        }

        body {
            font-family: 'Times New Roman', 'Georgia', serif;
            font-size: 12pt;
            line-height: 1.6;
            color: #000;
        }

        /* Letterhead */
        .letterhead {
            text-align: center;
            padding-bottom: 20pt;
            margin-bottom: 30pt;
            border-bottom: 2pt solid #2563eb;
        }

        .letterhead-logo {
            height: 60pt;
            margin-bottom: 10pt;
        }

        .letterhead h1 {
            font-size: 18pt;
            font-weight: bold;
            color: #2563eb;
            margin: 0 0 5pt 0;
        }

        .letterhead p {
            font-size: 10pt;
            margin: 2pt 0;
            color: #555;
        }

        /* Date and reference */
        .letter-date {
            text-align: right;
            margin-bottom: 30pt;
            font-size: 11pt;
        }

        .reference {
            margin-bottom: 20pt;
            font-size: 11pt;
        }

        .reference strong {
            display: inline-block;
            width: 80pt;
        }

        /* Address blocks */
        .sender-address {
            margin-bottom: 30pt;
        }

        .recipient-address {
            margin-bottom: 30pt;
        }

        .sender-address p, .recipient-address p {
            margin: 3pt 0;
            line-height: 1.4;
        }

        /* Salutation */
        .salutation {
            margin-bottom: 20pt;
            font-weight: normal;
        }

        /* Letter body */
        .letter-body {
            text-align: justify;
            margin-bottom: 30pt;
        }

        .letter-body p {
            margin-bottom: 12pt;
            text-indent: 0;
        }

        .letter-body p.indented {
            margin-left: 30pt;
            margin-right: 30pt;
        }

        /* Closing */
        .closing {
            margin-top: 30pt;
            margin-bottom: 60pt;
        }

        .closing p {
            margin: 3pt 0;
        }

        /* Signature block */
        .signature-block {
            margin-top: 50pt;
        }

        .signature-image {
            height: 40pt;
            margin-bottom: -10pt;
        }

        .signature-name {
            font-weight: bold;
            margin: 5pt 0;
        }

        .signature-title {
            font-style: italic;
            color: #555;
            margin: 2pt 0;
        }

        /* Enclosures */
        .enclosures {
            margin-top: 30pt;
            font-size: 11pt;
        }

        .enclosures p {
            margin: 3pt 0;
        }

        /* CC list */
        .cc-list {
            margin-top: 20pt;
            font-size: 11pt;
        }

        .cc-list p {
            margin: 3pt 0;
        }

        /* Footer */
        .letter-footer {
            position: absolute;
            bottom: 40pt;
            left: 72pt;
            right: 72pt;
            text-align: center;
            font-size: 9pt;
            color: #666;
            border-top: 1pt solid #e5e7eb;
            padding-top: 10pt;
        }

        /* Page numbers */
        .page-number {
            text-align: right;
            font-size: 10pt;
            color: #666;
            margin-top: 20pt;
        }

        /* For multi-page letters */
        .continuation-header {
            margin-bottom: 20pt;
            padding-bottom: 10pt;
            border-bottom: 1pt solid #e5e7eb;
            display: none;
        }

        .continuation-header.show {
            display: block;
        }

        .continuation-header p {
            margin: 3pt 0;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <!-- Letterhead (first page only) -->
    <div class="letterhead">
        <img src="{{company.logo}}" class="letterhead-logo" alt="{{company.name}}" />
        <h1>{{company.name}}</h1>
        <p>{{company.address}}</p>
        <p>{{company.city}}, {{company.state}} {{company.zip}}</p>
        <p>Phone: {{company.phone}} | Email: {{company.email}} | {{company.website}}</p>
    </div>

    <!-- Date -->
    <div class="letter-date">
        <p>{{letter.date}}</p>
    </div>

    <!-- Reference numbers (optional) -->
    {{#if letter.referenceNumber}}
    <div class="reference">
        <p><strong>Ref:</strong> {{letter.referenceNumber}}</p>
    </div>
    {{/if}}

    <!-- Recipient Address -->
    <div class="recipient-address">
        <p><strong>{{recipient.name}}</strong></p>
        {{#if recipient.title}}
        <p>{{recipient.title}}</p>
        {{/if}}
        {{#if recipient.company}}
        <p>{{recipient.company}}</p>
        {{/if}}
        <p>{{recipient.address}}</p>
        <p>{{recipient.city}}, {{recipient.state}} {{recipient.zip}}</p>
    </div>

    <!-- Subject line (optional) -->
    {{#if letter.subject}}
    <p style="font-weight: bold; margin-bottom: 20pt;">
        <strong>Re: {{letter.subject}}</strong>
    </p>
    {{/if}}

    <!-- Salutation -->
    <p class="salutation">Dear {{recipient.salutation}}:</p>

    <!-- Letter Body -->
    <div class="letter-body">
        {{#each letter.paragraphs}}
        <p>{{this}}</p>
        {{/each}}
    </div>

    <!-- Closing -->
    <div class="closing">
        <p>{{letter.closing}},</p>
    </div>

    <!-- Signature Block -->
    <div class="signature-block">
        {{#if signer.signature}}
        <img src="{{signer.signature}}" class="signature-image" alt="Signature" />
        {{/if}}
        <p class="signature-name">{{signer.name}}</p>
        <p class="signature-title">{{signer.title}}</p>
        {{#if signer.department}}
        <p class="signature-title">{{signer.department}}</p>
        {{/if}}
    </div>

    <!-- Enclosures (optional) -->
    {{#if letter.enclosures}}
    <div class="enclosures">
        <p><strong>Enclosures:</strong></p>
        {{#each letter.enclosures}}
        <p style="margin-left: 20pt;">• {{this}}</p>
        {{/each}}
    </div>
    {{/if}}

    <!-- CC list (optional) -->
    {{#if letter.ccList}}
    <div class="cc-list">
        <p><strong>CC:</strong></p>
        {{#each letter.ccList}}
        <p style="margin-left: 20pt;">{{this}}</p>
        {{/each}}
    </div>
    {{/if}}

    <!-- Footer -->
    <div class="letter-footer">
        <p>{{company.name}} | {{company.address}} | {{company.phone}}</p>
    </div>
</body>
</html>
```
{% endraw %}

---

## C# Letter Generator

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.Collections.Generic;
using System.IO;

public class BusinessLetterGenerator
{
    private readonly string _templatePath;

    public BusinessLetterGenerator(string templatePath)
    {
        _templatePath = templatePath;
    }

    public void GenerateLetter(LetterData letterData, Stream output)
    {
        // Load template
        var doc = Document.ParseDocument(_templatePath);

        // Set metadata
        doc.Info.Title = letterData.Letter.Subject ?? "Business Letter";
        doc.Info.Author = letterData.Signer.Name;
        doc.Info.Subject = letterData.Letter.Subject;
        doc.Info.CreationDate = DateTime.Now;

        // Bind data
        doc.Params["company"] = letterData.Company;
        doc.Params["recipient"] = letterData.Recipient;
        doc.Params["letter"] = letterData.Letter;
        doc.Params["signer"] = letterData.Signer;

        // Generate PDF
        doc.SaveAsPDF(output);
    }
}

// Data models
public class LetterData
{
    public CompanyInfo Company { get; set; }
    public RecipientInfo Recipient { get; set; }
    public Letter Letter { get; set; }
    public SignerInfo Signer { get; set; }
}

public class CompanyInfo
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
}

public class RecipientInfo
{
    public string Name { get; set; }
    public string Title { get; set; }
    public string Company { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Salutation { get; set; }  // "Mr. Smith", "Dr. Johnson", etc.
}

public class Letter
{
    public string Date { get; set; }
    public string ReferenceNumber { get; set; }
    public string Subject { get; set; }
    public List<string> Paragraphs { get; set; } = new List<string>();
    public string Closing { get; set; } = "Sincerely";
    public List<string> Enclosures { get; set; } = new List<string>();
    public List<string> CcList { get; set; } = new List<string>();
}

public class SignerInfo
{
    public string Name { get; set; }
    public string Title { get; set; }
    public string Department { get; set; }
    public string Signature { get; set; }  // Path to signature image
}
```

---

## Usage Example

```csharp
// Create letter data
var letterData = new LetterData
{
    Company = new CompanyInfo
    {
        Name = "Acme Corporation",
        Logo = "./images/acme-logo.png",
        Address = "123 Main Street",
        City = "New York",
        State = "NY",
        Zip = "10001",
        Phone = "(555) 123-4567",
        Email = "info@acme.com",
        Website = "www.acme.com"
    },
    Recipient = new RecipientInfo
    {
        Name = "John Smith",
        Title = "Director of Operations",
        Company = "Smith Industries",
        Address = "456 Oak Avenue",
        City = "Boston",
        State = "MA",
        Zip = "02101",
        Salutation = "Mr. Smith"
    },
    Letter = new Letter
    {
        Date = "October 17, 2025",
        ReferenceNumber = "ACM-2025-1017",
        Subject = "Partnership Proposal",
        Paragraphs = new List<string>
        {
            "I am writing to express our interest in establishing a strategic partnership between Acme Corporation and Smith Industries. Our companies share complementary strengths that could create significant value for both organizations.",

            "Acme Corporation has been a leader in technology solutions for over 20 years, serving clients across North America. We believe that combining our technical expertise with your distribution network would create exceptional opportunities for growth.",

            "I would welcome the opportunity to discuss this proposal in more detail. Our team has prepared a comprehensive presentation outlining the potential benefits and implementation strategy. Would you be available for a meeting during the week of November 1st?",

            "Thank you for considering this proposal. I look forward to hearing from you and exploring how our organizations can work together for mutual success."
        },
        Closing = "Sincerely",
        Enclosures = new List<string>
        {
            "Partnership Proposal Document",
            "Financial Projections",
            "Reference Letters"
        },
        CcList = new List<string>
        {
            "Sarah Johnson, VP of Business Development",
            "Michael Chen, Legal Counsel"
        }
    },
    Signer = new SignerInfo
    {
        Name = "Jane Doe",
        Title = "Chief Executive Officer",
        Department = "Executive Office",
        Signature = "./images/jane-signature.png"
    }
};

// Generate letter
var generator = new BusinessLetterGenerator("letter-template.html");

using (var output = new FileStream("partnership-letter.pdf", FileMode.Create))
{
    generator.GenerateLetter(letterData, output);
    Console.WriteLine($"Letter generated: {output.Name}");
}
```

---

## Step-by-Step Breakdown

### 1. Professional Letterhead

{% raw %}
```html
<div class="letterhead">
    <img src="{{company.logo}}" class="letterhead-logo" />
    <h1>{{company.name}}</h1>
    <p>{{company.address}}</p>
    <p>{{company.city}}, {{company.state}} {{company.zip}}</p>
    <p>Phone: {{company.phone}} | Email: {{company.email}}</p>
</div>
```
{% endraw %}

**Features:**
- Centered design
- Company logo
- Full contact information
- Bottom border for separation

### 2. Date and Reference

{% raw %}
```html
<div class="letter-date">
    <p>{{letter.date}}</p>
</div>

{{#if letter.referenceNumber}}
<div class="reference">
    <p><strong>Ref:</strong> {{letter.referenceNumber}}</p>
</div>
{{/if}}
```
{% endraw %}

**Features:**
- Right-aligned date
- Optional reference number
- Professional formatting

### 3. Address Block

{% raw %}
```html
<div class="recipient-address">
    <p><strong>{{recipient.name}}</strong></p>
    {{#if recipient.title}}
    <p>{{recipient.title}}</p>
    {{/if}}
    {{#if recipient.company}}
    <p>{{recipient.company}}</p>
    {{/if}}
    <p>{{recipient.address}}</p>
    <p>{{recipient.city}}, {{recipient.state}} {{recipient.zip}}</p>
</div>
```
{% endraw %}

**Features:**
- Complete recipient information
- Conditional title and company
- Standard postal format

### 4. Letter Body

{% raw %}
```html
<p class="salutation">Dear {{recipient.salutation}}:</p>

<div class="letter-body">
    {{#each letter.paragraphs}}
    <p>{{this}}</p>
    {{/each}}
</div>

<div class="closing">
    <p>{{letter.closing}},</p>
</div>
```
{% endraw %}

**Features:**
- Dynamic paragraph count
- Proper spacing
- Customizable closing

### 5. Signature Block

{% raw %}
```html
<div class="signature-block">
    {{#if signer.signature}}
    <img src="{{signer.signature}}" class="signature-image" />
    {{/if}}
    <p class="signature-name">{{signer.name}}</p>
    <p class="signature-title">{{signer.title}}</p>
</div>
```
{% endraw %}

**Features:**
- Optional signature image
- Signer name and title
- Professional spacing

---

## Common Variations

### Variation 1: Formal Government Letter

```css
.letterhead {
    text-align: left;
    border-bottom: 3pt double #000;
}

body {
    font-family: 'Courier New', monospace;
    font-size: 12pt;
}

.letter-body {
    text-align: left;
    line-height: 2;
}
```

### Variation 2: Modern Business Letter

```css
.letterhead {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    padding: 30pt;
    border-radius: 8pt;
}

.letterhead h1 {
    color: white;
    font-size: 24pt;
}

.letter-body {
    font-family: 'Helvetica', 'Arial', sans-serif;
    font-size: 11pt;
}
```

### Variation 3: Memo Style

{% raw %}
```html
<div class="memo-header">
    <table style="width: 100%; border-bottom: 2pt solid #000;">
        <tr>
            <td><strong>TO:</strong></td>
            <td>{{recipient.name}}</td>
        </tr>
        <tr>
            <td><strong>FROM:</strong></td>
            <td>{{signer.name}}</td>
        </tr>
        <tr>
            <td><strong>DATE:</strong></td>
            <td>{{letter.date}}</td>
        </tr>
        <tr>
            <td><strong>SUBJECT:</strong></td>
            <td>{{letter.subject}}</td>
        </tr>
    </table>
</div>
```
{% endraw %}

---

## Multi-Page Letter Handling

For letters that span multiple pages, add page headers:

{% raw %}
```html
<style>
    @page {
        @top-left {
            content: "{{recipient.name}}";
            font-size: 10pt;
            color: #666;
        }

        @top-right {
            content: "Page " counter(page) " of " counter(pages);
            font-size: 10pt;
            color: #666;
        }
    }

    /* Hide letterhead on subsequent pages */
    @page :first {
        @top-left { content: none; }
        @top-right { content: none; }
    }
</style>
```
{% endraw %}

---

## Tips & Tricks

### 1. Signature Positioning

```html
<div style="display: flex; justify-content: space-between; margin-top: 40pt;">
    <div class="signature-block">
        <img src="{{signer1.signature}}" style="height: 40pt;" />
        <p><strong>{{signer1.name}}</strong></p>
        <p>{{signer1.title}}</p>
    </div>

    <div class="signature-block">
        <img src="{{signer2.signature}}" style="height: 40pt;" />
        <p><strong>{{signer2.name}}</strong></p>
        <p>{{signer2.title}}</p>
    </div>
</div>
```

### 2. Confidential Watermark

```css
.watermark {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%) rotate(-45deg);
    font-size: 72pt;
    color: rgba(255, 0, 0, 0.1);
    font-weight: bold;
    z-index: -1;
}
```

{% raw %}
```html
{{#if letter.isConfidential}}
<div class="watermark">CONFIDENTIAL</div>
{{/if}}
```
{% endraw %}

### 3. Custom Footer with Logo

```html
<div class="letter-footer">
    <table style="width: 100%;">
        <tr>
            <td style="width: 60pt;">
                <img src="{{company.logoSmall}}" style="height: 20pt;" />
            </td>
            <td style="text-align: center;">
                <p>{{company.name}} | {{company.phone}}</p>
            </td>
            <td style="width: 60pt; text-align: right;">
                <p><page-number /></p>
            </td>
        </tr>
    </table>
</div>
```

---

## Try It Yourself

### Exercise 1: Create Custom Letterhead

Design a letterhead for your organization:
- Add your company logo
- Choose appropriate colors
- Include all contact information
- Test with different letter lengths

### Exercise 2: Multiple Signers

Modify the template to support:
- Two signers side-by-side
- Approval chain with multiple signatures
- Electronic signature blocks

### Exercise 3: Letter Types

Create specialized templates for:
- Job offer letters
- Recommendation letters
- Legal notices
- Thank you letters

---

## Common Pitfalls

### ❌ Inconsistent Spacing

```css
/* Too cramped */
.letter-body p {
    margin-bottom: 5pt;
}
```

✅ **Solution:**

```css
/* Professional spacing */
.letter-body p {
    margin-bottom: 12pt;
    line-height: 1.6;
}
```

### ❌ Missing Salutation

{% raw %}
```html
<!-- Generic greeting -->
<p>Dear Sir/Madam:</p>
```
{% endraw %}

✅ **Solution:**

{% raw %}
```html
<!-- Personalized -->
<p>Dear {{recipient.salutation}}:</p>
```
{% endraw %}

### ❌ Signature Too Large

```html
<img src="{{signer.signature}}" />
<!-- Signature fills entire page -->
```

✅ **Solution:**

```html
<img src="{{signer.signature}}"
     style="max-height: 40pt; max-width: 200pt;" />
```

---

## Letter Checklist

- [ ] Letterhead includes logo and full contact info
- [ ] Date is properly formatted
- [ ] Recipient address is complete
- [ ] Salutation is personalized
- [ ] Body paragraphs are well-spaced
- [ ] Closing is appropriate
- [ ] Signature block is professional
- [ ] Enclosures listed if applicable
- [ ] Footer appears on all pages
- [ ] Multi-page handling works correctly

---

## Best Practices

1. **Use Serif Fonts** - Times New Roman or Georgia for formal letters
2. **Standard Margins** - 1 inch (72pt) on all sides
3. **Proper Line Spacing** - 1.5 to 2.0 for readability
4. **Clear Hierarchy** - Use font sizes and weights appropriately
5. **Signature Space** - Leave adequate room for handwritten signatures
6. **Professional Tone** - Match design to letter formality
7. **Consistent Formatting** - Use templates for brand consistency
8. **Test Printing** - Verify layout on physical paper

---

## Next Steps

1. **[Report Template](03_report_template.md)** - Multi-section reports
2. **[Certificate Template](04_certificate_template.md)** - Awards and certificates
3. **[Multi-Language & Branded Documents](08_multi_language_branded.md)** - Enterprise templates

---

**Continue learning →** [Report Template](03_report_template.md)
