---
layout: default
title: Form Template
nav_order: 7
parent: Practical Applications
parent_url: /learning/08-practical/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Form Template

Create professional print-and-fill forms with clear fields, instructions, checkboxes, and proper spacing for handwritten input.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Design print-and-fill forms
- Create form fields with proper spacing
- Add checkboxes and signature lines
- Include instructions and guidelines
- Handle multi-page forms
- Generate fillable form PDFs

---

## Complete Form Template

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>{{form.title}}</title>
    <style>
        @page {
            size: letter;
            margin: 54pt;
        }

        body {
            font-family: 'Helvetica', 'Arial', sans-serif;
            font-size: 10pt;
            line-height: 1.4;
            color: #000;
        }

        /* Form header */
        .form-header {
            border-bottom: 3pt solid #2563eb;
            padding-bottom: 15pt;
            margin-bottom: 30pt;
        }

        .form-header h1 {
            font-size: 20pt;
            color: #2563eb;
            margin: 0 0 5pt 0;
        }

        .form-header p {
            margin: 3pt 0;
            font-size: 9pt;
            color: #666;
        }

        /* Form sections */
        .form-section {
            margin-bottom: 25pt;
            page-break-inside: avoid;
        }

        .section-title {
            background-color: #eff6ff;
            color: #1e40af;
            font-size: 12pt;
            font-weight: bold;
            padding: 8pt 12pt;
            margin-bottom: 15pt;
            border-left: 4pt solid #2563eb;
        }

        /* Form fields */
        .form-field {
            margin-bottom: 15pt;
        }

        .field-label {
            font-weight: bold;
            margin-bottom: 3pt;
            font-size: 9pt;
        }

        .field-label.required::after {
            content: ' *';
            color: #dc2626;
        }

        .field-input {
            border-bottom: 1pt solid #000;
            min-height: 12pt;
            padding: 2pt 0;
        }

        .field-input.short {
            width: 150pt;
        }

        .field-input.medium {
            width: 250pt;
        }

        .field-input.long {
            width: 100%;
        }

        .field-input.multiline {
            border: 1pt solid #666;
            min-height: 60pt;
            padding: 5pt;
        }

        .field-help {
            font-size: 8pt;
            color: #666;
            font-style: italic;
            margin-top: 2pt;
        }

        /* Two-column layout */
        .form-row {
            display: flex;
            gap: 20pt;
            margin-bottom: 15pt;
        }

        .form-col {
            flex: 1;
        }

        .form-col.col-narrow {
            flex: 0 0 150pt;
        }

        .form-col.col-wide {
            flex: 1;
        }

        /* Checkboxes */
        .checkbox-group {
            margin: 10pt 0;
        }

        .checkbox-item {
            display: flex;
            align-items: center;
            margin-bottom: 8pt;
        }

        .checkbox {
            width: 12pt;
            height: 12pt;
            border: 1.5pt solid #000;
            margin-right: 8pt;
            flex-shrink: 0;
        }

        .checkbox-label {
            font-size: 9pt;
        }

        /* Radio buttons */
        .radio-group {
            margin: 10pt 0;
        }

        .radio-item {
            display: inline-flex;
            align-items: center;
            margin-right: 20pt;
            margin-bottom: 8pt;
        }

        .radio {
            width: 12pt;
            height: 12pt;
            border: 1.5pt solid #000;
            border-radius: 50%;
            margin-right: 6pt;
            flex-shrink: 0;
        }

        .radio-label {
            font-size: 9pt;
        }

        /* Signature section */
        .signature-section {
            margin-top: 30pt;
            page-break-inside: avoid;
        }

        .signature-line {
            border-bottom: 1pt solid #000;
            width: 250pt;
            margin: 30pt 0 5pt 0;
        }

        .signature-label {
            font-size: 9pt;
            color: #666;
        }

        .signature-grid {
            display: grid;
            grid-template-columns: 1fr 150pt;
            gap: 30pt;
            margin-top: 30pt;
        }

        /* Instructions box */
        .instructions {
            background-color: #fffbeb;
            border: 1pt solid #f59e0b;
            border-radius: 4pt;
            padding: 12pt;
            margin-bottom: 20pt;
        }

        .instructions h3 {
            margin: 0 0 8pt 0;
            font-size: 11pt;
            color: #d97706;
        }

        .instructions ul {
            margin: 8pt 0;
            padding-left: 20pt;
        }

        .instructions li {
            margin: 4pt 0;
            font-size: 9pt;
        }

        /* Table for structured data */
        .data-table {
            width: 100%;
            border-collapse: collapse;
            margin: 15pt 0;
        }

        .data-table th {
            background-color: #f3f4f6;
            border: 1pt solid #666;
            padding: 6pt;
            font-size: 9pt;
            text-align: left;
        }

        .data-table td {
            border: 1pt solid #666;
            padding: 6pt;
            min-height: 20pt;
        }

        /* Required field notice */
        .required-notice {
            font-size: 8pt;
            color: #666;
            margin-bottom: 15pt;
        }

        .required-notice::before {
            content: '* ';
            color: #dc2626;
            font-weight: bold;
        }

        /* Page footer */
        .form-footer {
            position: absolute;
            bottom: 40pt;
            left: 54pt;
            right: 54pt;
            text-align: center;
            font-size: 8pt;
            color: #666;
            border-top: 1pt solid #e5e7eb;
            padding-top: 8pt;
        }

        /* Privacy notice */
        .privacy-notice {
            background-color: #f9fafb;
            border: 1pt solid #e5e7eb;
            padding: 10pt;
            margin-top: 20pt;
            font-size: 7pt;
            color: #666;
            line-height: 1.3;
        }
    </style>
</head>
<body>
    <!-- Form Header -->
    <div class="form-header">
        <h1>{{form.title}}</h1>
        <p><strong>Form Number:</strong> {{form.number}} | <strong>Revision:</strong> {{form.revision}} | <strong>Date:</strong> {{form.date}}</p>
        {{#if form.department}}
        <p><strong>Department:</strong> {{form.department}}</p>
        {{/if}}
    </div>

    <!-- Instructions -->
    {{#if form.instructions}}
    <div class="instructions">
        <h3>Instructions</h3>
        <ul>
            {{#each form.instructions}}
            <li>{{this}}</li>
            {{/each}}
        </ul>
    </div>
    {{/if}}

    <p class="required-notice">Required fields</p>

    <!-- Personal Information Section -->
    <div class="form-section">
        <div class="section-title">1. Personal Information</div>

        <div class="form-row">
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label required">First Name</div>
                    <div class="field-input long"></div>
                </div>
            </div>
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label">Middle Initial</div>
                    <div class="field-input short"></div>
                </div>
            </div>
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label required">Last Name</div>
                    <div class="field-input long"></div>
                </div>
            </div>
        </div>

        <div class="form-row">
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label required">Date of Birth</div>
                    <div class="field-input medium"></div>
                    <div class="field-help">MM/DD/YYYY</div>
                </div>
            </div>
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label required">Social Security Number</div>
                    <div class="field-input medium"></div>
                    <div class="field-help">XXX-XX-XXXX</div>
                </div>
            </div>
        </div>

        <div class="form-field">
            <div class="field-label required">Email Address</div>
            <div class="field-input long"></div>
        </div>

        <div class="form-row">
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label required">Phone Number</div>
                    <div class="field-input long"></div>
                    <div class="field-help">(XXX) XXX-XXXX</div>
                </div>
            </div>
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label">Alternate Phone</div>
                    <div class="field-input long"></div>
                </div>
            </div>
        </div>
    </div>

    <!-- Address Section -->
    <div class="form-section">
        <div class="section-title">2. Address Information</div>

        <div class="form-field">
            <div class="field-label required">Street Address</div>
            <div class="field-input long"></div>
        </div>

        <div class="form-field">
            <div class="field-label">Address Line 2 (Apt, Suite, Unit)</div>
            <div class="field-input long"></div>
        </div>

        <div class="form-row">
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label required">City</div>
                    <div class="field-input long"></div>
                </div>
            </div>
            <div class="form-col col-narrow">
                <div class="form-field">
                    <div class="field-label required">State</div>
                    <div class="field-input long"></div>
                </div>
            </div>
            <div class="form-col col-narrow">
                <div class="form-field">
                    <div class="field-label required">ZIP Code</div>
                    <div class="field-input long"></div>
                </div>
            </div>
        </div>
    </div>

    <!-- Multiple Choice Section -->
    <div class="form-section">
        <div class="section-title">3. Preferences</div>

        <div class="form-field">
            <div class="field-label required">Preferred Contact Method</div>
            <div class="radio-group">
                <div class="radio-item">
                    <div class="radio"></div>
                    <span class="radio-label">Email</span>
                </div>
                <div class="radio-item">
                    <div class="radio"></div>
                    <span class="radio-label">Phone</span>
                </div>
                <div class="radio-item">
                    <div class="radio"></div>
                    <span class="radio-label">Mail</span>
                </div>
            </div>
        </div>

        <div class="form-field">
            <div class="field-label">Areas of Interest (Check all that apply)</div>
            <div class="checkbox-group">
                <div class="checkbox-item">
                    <div class="checkbox"></div>
                    <span class="checkbox-label">Product Information</span>
                </div>
                <div class="checkbox-item">
                    <div class="checkbox"></div>
                    <span class="checkbox-label">Technical Support</span>
                </div>
                <div class="checkbox-item">
                    <div class="checkbox"></div>
                    <span class="checkbox-label">Training & Education</span>
                </div>
                <div class="checkbox-item">
                    <div class="checkbox"></div>
                    <span class="checkbox-label">Billing & Payments</span>
                </div>
                <div class="checkbox-item">
                    <div class="checkbox"></div>
                    <span class="checkbox-label">Other: <span style="border-bottom: 1pt solid #000; display: inline-block; width: 250pt; margin-left: 5pt;"></span></span>
                </div>
            </div>
        </div>
    </div>

    <!-- Comments Section -->
    <div class="form-section">
        <div class="section-title">4. Additional Information</div>

        <div class="form-field">
            <div class="field-label">Comments or Special Requests</div>
            <div class="field-input multiline long"></div>
        </div>
    </div>

    <!-- Signature Section -->
    <div class="signature-section">
        <div class="section-title">5. Certification and Signature</div>

        <p style="margin: 15pt 0; font-size: 9pt;">
            I certify that the information provided in this form is true, complete, and accurate to the best of my knowledge. I understand that providing false information may result in denial or termination of services.
        </p>

        <div class="signature-grid">
            <div>
                <div class="signature-line"></div>
                <div class="signature-label">Signature</div>
            </div>
            <div>
                <div class="signature-line"></div>
                <div class="signature-label">Date</div>
            </div>
        </div>

        <div style="margin-top: 30pt;">
            <div class="field-label">Printed Name</div>
            <div class="field-input" style="width: 250pt;"></div>
        </div>
    </div>

    <!-- Privacy Notice -->
    <div class="privacy-notice">
        <strong>Privacy Notice:</strong> The information collected on this form is used solely for the purposes stated. We do not share your personal information with third parties except as required by law. For our complete privacy policy, visit {{company.website}}/privacy.
    </div>

    <!-- Office Use Only Section (Optional) -->
    {{#if form.includeOfficeSection}}
    <div class="form-section" style="border-top: 2pt dashed #666; padding-top: 20pt; margin-top: 30pt;">
        <div class="section-title" style="background-color: #f3f4f6; color: #666;">For Office Use Only</div>

        <div class="form-row">
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label">Date Received</div>
                    <div class="field-input long"></div>
                </div>
            </div>
            <div class="form-col">
                <div class="form-field">
                    <div class="field-label">Received By</div>
                    <div class="field-input long"></div>
                </div>
            </div>
        </div>

        <div class="form-field">
            <div class="field-label">Application Number</div>
            <div class="field-input medium"></div>
        </div>

        <div class="form-field">
            <div class="field-label">Notes</div>
            <div class="field-input multiline long"></div>
        </div>
    </div>
    {{/if}}

    <!-- Footer -->
    <div class="form-footer">
        <p>{{company.name}} | {{form.number}} Rev. {{form.revision}} | Page <page-number /> of <page-count /></p>
        {{#if company.phone}}
        <p>Questions? Call {{company.phone}} or email {{company.email}}</p>
        {{/if}}
    </div>
</body>
</html>
```
{% endraw %}

---

## C# Form Generator

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.Collections.Generic;
using System.IO;

public class FormGenerator
{
    private readonly string _templatePath;

    public FormGenerator(string templatePath)
    {
        _templatePath = templatePath;
    }

    public void GenerateForm(FormData formData, Stream output)
    {
        // Load template
        var doc = Document.ParseDocument(_templatePath);

        // Set metadata
        doc.Info.Title = formData.Form.Title;
        doc.Info.Author = formData.Company.Name;
        doc.Info.Subject = $"{formData.Form.Number} - {formData.Form.Title}";
        doc.Info.CreationDate = DateTime.Now;

        // Bind data
        doc.Params["company"] = formData.Company;
        doc.Params["form"] = formData.Form;

        // Configure for printing
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;

        // Generate PDF
        doc.SaveAsPDF(output);
    }

    public void GeneratePrefilled(FormData formData, object filledData, Stream output)
    {
        // For pre-filled forms, you would:
        // 1. Load the template
        // 2. Bind both form structure and filled data
        // 3. Generate with data already populated

        var doc = Document.ParseDocument(_templatePath);

        // Set metadata
        doc.Info.Title = $"{formData.Form.Title} - Prefilled";
        doc.Info.Author = formData.Company.Name;
        doc.Info.CreationDate = DateTime.Now;

        // Bind form structure
        doc.Params["company"] = formData.Company;
        doc.Params["form"] = formData.Form;

        // Bind filled data
        doc.Params["applicant"] = filledData;

        // Generate
        doc.SaveAsPDF(output);
    }
}

// Data models
public class FormData
{
    public CompanyInfo Company { get; set; }
    public Form Form { get; set; }
}

public class CompanyInfo
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
}

public class Form
{
    public string Title { get; set; }
    public string Number { get; set; }
    public string Revision { get; set; }
    public string Date { get; set; }
    public string Department { get; set; }
    public List<string> Instructions { get; set; }
    public bool IncludeOfficeSection { get; set; }
}
```

---

## Usage Example

```csharp
// Create form data
var formData = new FormData
{
    Company = new CompanyInfo
    {
        Name = "Acme Corporation",
        Phone = "(555) 123-4567",
        Email = "info@acme.com",
        Website = "www.acme.com"
    },
    Form = new Form
    {
        Title = "Customer Registration Form",
        Number = "FORM-001",
        Revision = "2.1",
        Date = "October 2025",
        Department = "Customer Service",
        Instructions = new List<string>
        {
            "Please print clearly in black or blue ink",
            "Complete all required fields marked with an asterisk (*)",
            "Sign and date the form at the bottom",
            "Return completed form to: Acme Corporation, 123 Main St, New York, NY 10001",
            "Or email scanned copy to: forms@acme.com"
        },
        IncludeOfficeSection = true
    }
};

// Generate blank form
var generator = new FormGenerator("form-template.html");

using (var output = new FileStream("customer-registration-form.pdf", FileMode.Create))
{
    generator.GenerateForm(formData, output);
    Console.WriteLine("Form generated successfully!");
}
```

---

## Try It Yourself

### Exercise 1: Custom Form

Create a form for your organization:
- Job application
- Service request
- Feedback survey
- Registration form

### Exercise 2: Dynamic Fields

Build forms with dynamic sections:
- Repeating sections (multiple addresses)
- Conditional fields (show/hide based on selection)
- Table rows for multiple entries
- Calculated fields

### Exercise 3: Fillable PDF

Generate truly fillable PDFs:
- Add interactive form fields
- Include field validation
- Implement calculations
- Export form data

---

## Best Practices

1. **Clear Labels** - Use descriptive field labels
2. **Adequate Space** - Leave room for handwritten entries
3. **Required Indicators** - Mark required fields with *
4. **Instructions** - Provide clear filling instructions
5. **Logical Flow** - Group related fields together
6. **Professional Layout** - Use consistent spacing and alignment
7. **Print Testing** - Test how forms print on paper
8. **Accessibility** - Ensure forms are screen-reader friendly

---

## Next Steps

1. **[Multi-Language & Branded Documents](08_multi_language_branded.md)** - Enterprise templates
2. **[Invoice Template](01_invoice_template.md)** - Review business documents
3. **[Data-Driven Report](05_data_driven_report.md)** - Dynamic content

---

**Continue learning →** [Multi-Language & Branded Documents](08_multi_language_branded.md)
