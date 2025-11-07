---
layout: default
title: Page Sizes & Orientation
nav_order: 1
parent: Layout & Positioning
parent_url: /learning/04-layout/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Page Sizes & Orientation

Master page size configuration and orientation control for professional PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Use standard page sizes (Letter, A4, Legal)
- Create custom page sizes
- Control page orientation (portrait, landscape)
- Mix different page sizes in one document
- Apply page sizes to specific sections
- Use the @page CSS rule

---

## Why Page Sizes Matter

PDF documents have fixed page dimensions, unlike web pages that can scroll infinitely. Understanding page sizes is essential for:
- **Print-ready output** - Matching printer paper sizes
- **Professional appearance** - Standard sizes for business documents
- **Layout control** - Designing for specific dimensions
- **Regional requirements** - Different regions use different standards (US Letter vs A4)

---

## Standard Page Sizes

### Letter (US Standard)

The most common page size in North America.

```css
@page {
    size: Letter;  /* 8.5 x 11 inches (612 x 792 points) */
}
```

**Dimensions:**
- Width: 8.5 inches (612 pt)
- Height: 11 inches (792 pt)
- Aspect ratio: 1:1.29

**Common uses:** Business documents, reports, letters, invoices

### A4 (International Standard)

The ISO standard used worldwide except North America.

```css
@page {
    size: A4;  /* 210 x 297 mm (595 x 842 points) */
}
```

**Dimensions:**
- Width: 210 mm (595 pt / 8.27 inches)
- Height: 297 mm (842 pt / 11.69 inches)
- Aspect ratio: 1:√2 (1:1.414)

**Common uses:** International documents, official forms, academic papers

### Legal

Taller US page size for legal documents.

```css
@page {
    size: Legal;  /* 8.5 x 14 inches (612 x 1008 points) */
}
```

**Dimensions:**
- Width: 8.5 inches (612 pt)
- Height: 14 inches (1008 pt)
- Aspect ratio: 1:1.65

**Common uses:** Legal documents, contracts, agreements

### Other Standard Sizes

```css
/* Tabloid / Ledger */
@page {
    size: Tabloid;  /* 11 x 17 inches (792 x 1224 pt) */
}

/* A3 (Larger A4) */
@page {
    size: A3;  /* 297 x 420 mm (842 x 1191 pt) */
}

/* A5 (Smaller A4) */
@page {
    size: A5;  /* 148 x 210 mm (420 x 595 pt) */
}

/* Executive */
@page {
    size: Executive;  /* 7.25 x 10.5 inches (522 x 756 pt) */
}
```

---

## Page Orientation

### Portrait (Default)

Vertical orientation - height greater than width.

```css
@page {
    size: Letter portrait;  /* Explicit portrait */
}

/* Or just */
@page {
    size: Letter;  /* Portrait is default */
}
```

**Characteristics:**
- Standard reading orientation
- More lines of text per page
- Traditional document format

### Landscape

Horizontal orientation - width greater than height.

```css
@page {
    size: Letter landscape;  /* 11 x 8.5 inches */
}
```

**Characteristics:**
- Wide content display
- Better for tables and charts
- Presentations and diagrams

**Dimensions swap:**
- Portrait Letter: 8.5" × 11"
- Landscape Letter: 11" × 8.5"

---

## Custom Page Sizes

Define exact dimensions when standard sizes don't fit.

### Using Points

```css
@page {
    size: 400pt 600pt;  /* Width x Height */
}
```

### Using Inches

```css
@page {
    size: 6in 9in;  /* 6 x 9 inches (common book size) */
}
```

### Using Centimeters

```css
@page {
    size: 15cm 21cm;  /* Custom metric size */
}
```

### Square Pages

```css
@page {
    size: 500pt 500pt;  /* Perfect square */
}
```

### Common Custom Sizes

```css
/* US Half Letter (5.5 x 8.5 inches) */
@page {
    size: 5.5in 8.5in;
}

/* Postcard (4 x 6 inches) */
@page {
    size: 4in 6in;
}

/* Business Card (3.5 x 2 inches) */
@page {
    size: 3.5in 2in;
}

/* Book sizes */
@page {
    size: 6in 9in;      /* Trade paperback */
}
@page {
    size: 5.5in 8.5in;  /* Digest */
}
@page {
    size: 5in 8in;      /* Mass market */
}
```

---

## Setting Page Size for Entire Document

### In CSS (Recommended)

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Letter Document</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
        }
    </style>
</head>
<body>
    <h1>Document Content</h1>
    <p>This document uses Letter size pages.</p>
</body>
</html>
```

### In C# Code

```csharp
using Scryber.Components;
using Scryber.Drawing;

// Create document
var doc = Document.ParseHTML("template.html");

// Set page size programmatically (if not set in CSS)
// This is typically unnecessary if you use @page in CSS
// But useful for dynamic documents

// Generate PDF
using (var stream = new FileStream("output.pdf", FileMode.Create))
{
    doc.SaveAsPDF(stream);
}
```

---

## Mixing Page Sizes in One Document

Use sections with different page configurations.

### Multiple Orientations

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Mixed Orientations</title>
    <style>
        @page {
            size: Letter portrait;
            margin: 1in;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
        }

        /* Landscape section */
        .landscape-section {
            page-break-before: always;
            page-break-after: always;
        }

        /* Note: Orientation changes require section elements in practice */
        h1 {
            color: #1e40af;
            margin-bottom: 20pt;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        th, td {
            border: 1pt solid #d1d5db;
            padding: 8pt;
            text-align: left;
        }

        th {
            background-color: #2563eb;
            color: white;
        }
    </style>
</head>
<body>
    <!-- Portrait pages -->
    <h1>Introduction</h1>
    <p>This section is in portrait orientation, suitable for text-heavy content.</p>
    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>

    <!-- Landscape page for wide table -->
    <div class="landscape-section">
        <h1>Financial Data (Landscape)</h1>
        <table>
            <thead>
                <tr>
                    <th>Month</th>
                    <th>Revenue</th>
                    <th>Expenses</th>
                    <th>Profit</th>
                    <th>Growth</th>
                    <th>Forecast</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>January</td>
                    <td>$125,000</td>
                    <td>$85,000</td>
                    <td>$40,000</td>
                    <td>12%</td>
                    <td>$130,000</td>
                </tr>
                <tr>
                    <td>February</td>
                    <td>$138,000</td>
                    <td>$92,000</td>
                    <td>$46,000</td>
                    <td>15%</td>
                    <td>$145,000</td>
                </tr>
            </tbody>
        </table>
    </div>

    <!-- Back to portrait -->
    <h1>Conclusion</h1>
    <p>This final section returns to portrait orientation.</p>
</body>
</html>
```

---

## Page Size Quick Reference

| Size | Dimensions (in) | Dimensions (mm) | Dimensions (pt) | Common Use |
|------|----------------|-----------------|-----------------|------------|
| **Letter** | 8.5 × 11 | 216 × 279 | 612 × 792 | US documents |
| **Legal** | 8.5 × 14 | 216 × 356 | 612 × 1008 | Legal docs |
| **Tabloid** | 11 × 17 | 279 × 432 | 792 × 1224 | Posters |
| **A4** | 8.27 × 11.69 | 210 × 297 | 595 × 842 | International |
| **A3** | 11.69 × 16.54 | 297 × 420 | 842 × 1191 | Posters |
| **A5** | 5.83 × 8.27 | 148 × 210 | 420 × 595 | Small docs |
| **Executive** | 7.25 × 10.5 | 184 × 267 | 522 × 756 | Planners |

---

## Practical Examples

### Example 1: Business Letter (US)

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Business Letter</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
        }

        .letterhead {
            text-align: center;
            margin-bottom: 40pt;
            padding-bottom: 20pt;
            border-bottom: 2pt solid #2563eb;
        }

        .company-name {
            font-size: 24pt;
            font-weight: bold;
            color: #1e40af;
            margin-bottom: 10pt;
        }

        .date {
            margin-bottom: 30pt;
        }

        .recipient {
            margin-bottom: 30pt;
        }

        .closing {
            margin-top: 40pt;
        }
    </style>
</head>
<body>
    <div class="letterhead">
        <div class="company-name">Acme Corporation</div>
        <p>123 Business Street, Suite 100<br/>
        New York, NY 10001<br/>
        (555) 123-4567</p>
    </div>

    <div class="date">January 15, 2025</div>

    <div class="recipient">
        <strong>Mr. John Smith</strong><br/>
        XYZ Company<br/>
        456 Commerce Ave<br/>
        Boston, MA 02101
    </div>

    <p>Dear Mr. Smith,</p>

    <p>Thank you for your recent inquiry regarding our services. We are pleased to provide you with the information you requested.</p>

    <p>Our company has been serving businesses like yours for over 20 years, providing comprehensive solutions tailored to your specific needs.</p>

    <p>We would be happy to schedule a consultation at your convenience to discuss how we can help your organization achieve its goals.</p>

    <div class="closing">
        <p>Sincerely,</p>
        <p style="margin-top: 60pt;">
            <strong>Jane Doe</strong><br/>
            Sales Director<br/>
            Acme Corporation
        </p>
    </div>
</body>
</html>
```

### Example 2: International Report (A4)

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>International Report</title>
    <style>
        @page {
            size: A4;
            margin: 2.5cm;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
        }

        .cover {
            text-align: center;
            padding-top: 100pt;
        }

        h1 {
            font-size: 36pt;
            color: #1e40af;
            margin-bottom: 20pt;
        }

        h2 {
            font-size: 24pt;
            color: #2563eb;
            margin-top: 40pt;
            margin-bottom: 15pt;
            border-bottom: 2pt solid #e5e7eb;
            padding-bottom: 10pt;
        }

        h3 {
            font-size: 16pt;
            color: #3b82f6;
            margin-top: 20pt;
            margin-bottom: 10pt;
        }

        .metadata {
            margin-top: 60pt;
            font-size: 12pt;
            color: #666;
        }

        .section {
            page-break-before: always;
        }
    </style>
</head>
<body>
    <!-- Cover Page -->
    <div class="cover">
        <h1>Annual Report 2024</h1>
        <p style="font-size: 18pt; color: #666;">Financial Performance and Strategic Outlook</p>

        <div class="metadata">
            <p><strong>Published:</strong> January 2025</p>
            <p><strong>Company:</strong> Global Industries Ltd.</p>
            <p><strong>Region:</strong> Europe</p>
        </div>
    </div>

    <!-- Content Pages -->
    <div class="section">
        <h2>Executive Summary</h2>
        <p>This report provides a comprehensive overview of our financial performance and strategic initiatives for the fiscal year 2024.</p>

        <h3>Key Highlights</h3>
        <ul>
            <li>Revenue growth of 15% year-over-year</li>
            <li>Expanded operations to 3 new markets</li>
            <li>Launched 12 new products</li>
            <li>Increased customer satisfaction by 22%</li>
        </ul>

        <h3>Financial Performance</h3>
        <p>Our financial results demonstrate strong growth across all key metrics, with particular strength in our core business segments.</p>
    </div>

    <div class="section">
        <h2>Market Analysis</h2>
        <p>The global market landscape continues to evolve, presenting both opportunities and challenges for our organization.</p>
    </div>
</body>
</html>
```

### Example 3: Custom Size Certificate

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Certificate</title>
    <style>
        @page {
            size: 11in 8.5in;  /* Landscape Letter */
            margin: 0.5in;
        }

        body {
            font-family: Georgia, serif;
            text-align: center;
            padding: 40pt;
        }

        .certificate {
            border: 5pt solid #1e40af;
            padding: 40pt;
            border-radius: 10pt;
            background-color: #fffef9;
            min-height: 480pt;
            display: table;
            width: 100%;
        }

        .content {
            display: table-cell;
            vertical-align: middle;
        }

        h1 {
            font-size: 48pt;
            color: #1e40af;
            margin-bottom: 30pt;
            font-weight: normal;
            letter-spacing: 2pt;
        }

        .award-text {
            font-size: 18pt;
            margin-bottom: 40pt;
        }

        .recipient {
            font-size: 36pt;
            font-weight: bold;
            color: #2563eb;
            margin: 40pt 0;
            padding: 20pt;
            border-top: 2pt solid #d1d5db;
            border-bottom: 2pt solid #d1d5db;
        }

        .description {
            font-size: 14pt;
            margin-bottom: 40pt;
            line-height: 1.8;
        }

        .signature {
            margin-top: 60pt;
            font-size: 12pt;
        }

        .signature-line {
            border-top: 1pt solid #333;
            width: 200pt;
            margin: 40pt auto 10pt auto;
        }
    </style>
</head>
<body>
    <div class="certificate">
        <div class="content">
            <h1>Certificate of Achievement</h1>

            <p class="award-text">This certificate is proudly presented to</p>

            <div class="recipient">John Anderson</div>

            <p class="description">
                In recognition of outstanding performance and dedication<br/>
                in completing the Advanced Training Program<br/>
                with exceptional results
            </p>

            <div class="signature">
                <div class="signature-line"></div>
                <p><strong>Dr. Sarah Johnson</strong><br/>
                Program Director<br/>
                January 15, 2025</p>
            </div>
        </div>
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Standard Sizes

Create three documents:
- US Letter business report (portrait)
- A4 international document (portrait)
- Legal contract (portrait)

Compare the dimensions and see how content flows differently.

### Exercise 2: Orientation Change

Create a document with:
- Portrait first page (introduction)
- Landscape second page (wide table)
- Portrait third page (conclusion)

Use page-break-before to control page breaks.

### Exercise 3: Custom Size

Create a custom-sized document:
- 6 x 9 inch book page
- Design a book cover or chapter page
- Include appropriate margins and typography

---

## Common Pitfalls

### ❌ Forgetting to Specify Page Size

```css
/* No @page rule - defaults to Letter */
body {
    margin: 40pt;
}
```

✅ **Solution:** Always specify @page

```css
@page {
    size: A4;  /* Explicit size */
    margin: 2.5cm;
}
```

### ❌ Mixing Orientation Without Page Breaks

```html
<div style="page-orientation: landscape;">
    <!-- This doesn't work -->
</div>
```

✅ **Solution:** Use sections with page breaks

```html
<div style="page-break-before: always;">
    <!-- Landscape content on new page -->
</div>
```

### ❌ Using Viewport Units

```css
.element {
    width: 100vw;  /* Viewport width - doesn't work predictably */
}
```

✅ **Solution:** Use percentages or fixed units

```css
.element {
    width: 100%;  /* Percentage works */
    /* Or */
    width: 612pt;  /* Fixed width */
}
```

### ❌ Inconsistent Units

```css
@page {
    size: 8.5in 792pt;  /* Mixing units - confusing */
}
```

✅ **Solution:** Use consistent units

```css
@page {
    size: 8.5in 11in;  /* Consistent inches */
    /* Or */
    size: 612pt 792pt;  /* Consistent points */
}
```

### ❌ Ignoring Margins

```css
@page {
    size: Letter;
    /* No margin specified */
}
```

✅ **Solution:** Always set margins

```css
@page {
    size: Letter;
    margin: 1in;  /* Standard margin */
}
```

---

## Best Practices

1. **Use standard sizes** when possible (Letter for US, A4 for international)
2. **Set @page rule** at the top of your stylesheet
3. **Include margins** in your @page definition
4. **Use landscape** for wide tables and charts
5. **Test with actual paper** if printing
6. **Document your choices** - comment why you chose specific sizes
7. **Consider your audience** - US vs international standards

---

## Page Size Checklist

- [ ] @page rule defined in CSS
- [ ] Appropriate size for content (Letter, A4, or custom)
- [ ] Margins specified (typically 0.5in to 1in)
- [ ] Orientation chosen (portrait or landscape)
- [ ] Page breaks controlled where needed
- [ ] Consistent units used throughout
- [ ] Tested output matches expectations

---

## Next Steps

Now that you understand page sizes and orientation:

1. **[Margins & Padding](02_margins_padding.md)** - Control page margins and content spacing
2. **[Sections & Page Breaks](03_sections_page_breaks.md)** - Manage multi-page layouts
3. **[Headers & Footers](07_headers_footers.md)** - Add repeating content

---

**Continue learning →** [Margins & Padding](02_margins_padding.md)
