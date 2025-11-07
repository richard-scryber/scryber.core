---
layout: default
title: Text Alignment & Justification
nav_order: 6
parent: Typography & Fonts
parent_url: /learning/05-typography/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Text Alignment & Justification

Master text alignment, justification, and vertical alignment for professional PDF document layout.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Apply horizontal text alignment
- Use text justification effectively
- Control vertical alignment
- Understand alignment in different contexts
- Handle alignment in tables and cells
- Create professional, aligned layouts

---

## Horizontal Text Alignment

### Alignment Values

```css
/* Left alignment (default for LTR) */
.left {
    text-align: left;
}

/* Right alignment */
.right {
    text-align: right;
}

/* Center alignment */
.center {
    text-align: center;
}

/* Justified (flush left and right) */
.justified {
    text-align: justify;
}
```

### Common Applications

```css
/* Body text - left aligned */
p {
    text-align: left;
}

/* Headings - centered */
h1 {
    text-align: center;
}

/* Dates, page numbers - right aligned */
.date {
    text-align: right;
}

/* Formal documents - justified */
.formal {
    text-align: justify;
}
```

---

## Justification

Spreads text to fill the full line width.

### Basic Justification

```css
p {
    text-align: justify;
}
```

### Justification Considerations

**✅ Good for:**
- Formal documents
- Books and magazines
- Multi-column layouts
- Professional reports

**⚠️ Be careful with:**
- Short lines (creates large gaps)
- Long words (awkward spacing)
- Narrow columns

### Hyphenation (CSS3)

```css
p {
    text-align: justify;
    hyphens: auto;  /* Enable hyphenation */
}
```

**Note:** Hyphenation support varies. Check your PDF generator.

---

## Vertical Alignment

Controls vertical positioning within a line or cell.

### In Inline Context

```css
/* Align with baseline */
.baseline {
    vertical-align: baseline;
}

/* Align with top of line */
.top {
    vertical-align: top;
}

/* Align with middle */
.middle {
    vertical-align: middle;
}

/* Align with bottom */
.bottom {
    vertical-align: bottom;
}

/* Text-top (top of text, not line) */
.text-top {
    vertical-align: text-top;
}

/* Text-bottom */
.text-bottom {
    vertical-align: text-bottom;
}
```

### In Table Cells

```css
td {
    vertical-align: top;  /* Common for data tables */
}

th {
    vertical-align: middle;  /* Center in header cells */
}
```

### With Numeric Values

```css
/* Raise above baseline */
.superscript {
    vertical-align: 5pt;
}

/* Lower below baseline */
.subscript {
    vertical-align: -3pt;
}

/* Percentage of line-height */
.raised {
    vertical-align: 20%;
}
```

---

## Alignment in Tables

### Cell Alignment

```css
/* Header cells - center */
thead th {
    text-align: center;
    vertical-align: middle;
}

/* Data cells - left, top */
tbody td {
    text-align: left;
    vertical-align: top;
}

/* Numeric columns - right */
.number-column {
    text-align: right;
}
```

---

## Practical Examples

### Example 1: Document with Varied Alignment

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Text Alignment</title>
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

        /* ==============================================
           CENTERED HEADER
           ============================================== */
        .document-header {
            text-align: center;
            margin-bottom: 40pt;
            padding-bottom: 20pt;
            border-bottom: 2pt solid #2563eb;
        }

        .document-header h1 {
            font-size: 28pt;
            margin: 0 0 10pt 0;
            color: #1e40af;
        }

        .document-header .date {
            font-size: 12pt;
            color: #666;
        }

        /* ==============================================
           BODY TEXT
           ============================================== */
        h2 {
            font-size: 20pt;
            text-align: left;
            margin: 30pt 0 15pt 0;
            color: #2563eb;
        }

        p {
            text-align: justify;  /* Justified body text */
            margin: 0 0 14pt 0;
        }

        /* ==============================================
           SPECIAL ELEMENTS
           ============================================== */
        .signature {
            text-align: right;
            margin-top: 40pt;
            font-style: italic;
        }

        blockquote {
            text-align: left;  /* Quotes not justified */
            border-left: 4pt solid #2563eb;
            padding-left: 20pt;
            margin: 20pt 0;
            font-style: italic;
            color: #444;
        }

        .footer-text {
            text-align: center;
            font-size: 9pt;
            color: #666;
            margin-top: 40pt;
            padding-top: 20pt;
            border-top: 1pt solid #d1d5db;
        }
    </style>
</head>
<body>
    <!-- Centered header -->
    <div class="document-header">
        <h1>Annual Report</h1>
        <p class="date">Fiscal Year 2024</p>
    </div>

    <!-- Left-aligned heading, justified body -->
    <h2>Executive Summary</h2>

    <p>
        This report provides a comprehensive analysis of our organization's performance throughout the fiscal year 2024. We are pleased to present substantial growth across all key metrics, demonstrating the effectiveness of our strategic initiatives and the dedication of our team members.
    </p>

    <p>
        Our revenue increased by fifteen percent year-over-year, reaching twelve point five million dollars. This growth was accompanied by significant expansion into new markets and the successful launch of multiple product lines. Customer satisfaction scores improved by twenty-two percent, reflecting our continued commitment to excellence.
    </p>

    <!-- Left-aligned quote (not justified) -->
    <blockquote>
        "Success is not final, failure is not fatal: it is the courage to continue that counts."
        <br/>— Winston Churchill
    </blockquote>

    <p>
        Looking ahead to 2025, we remain focused on sustainable growth, operational excellence, and continued innovation. Our strategic priorities include market expansion, product development, and organizational capability building.
    </p>

    <!-- Right-aligned signature -->
    <div class="signature">
        Jane Doe<br/>
        Chief Executive Officer
    </div>

    <!-- Centered footer -->
    <p class="footer-text">
        © 2025 Acme Corporation | Confidential
    </p>
</body>
</html>
```

### Example 2: Table with Proper Alignment

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Table Alignment</title>
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

        h1 {
            text-align: center;
            font-size: 24pt;
            margin: 0 0 30pt 0;
            color: #1e40af;
        }

        /* ==============================================
           TABLE ALIGNMENT
           ============================================== */
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        /* Headers - centered, middle-aligned */
        thead th {
            background-color: #2563eb;
            color: white;
            padding: 12pt;
            text-align: center;
            vertical-align: middle;
            font-weight: bold;
        }

        /* Body cells - varied alignment */
        tbody td {
            padding: 10pt;
            border-bottom: 1pt solid #e5e7eb;
            vertical-align: top;  /* Top-align by default */
        }

        /* Text columns - left */
        td:nth-child(1),
        td:nth-child(2) {
            text-align: left;
        }

        /* Number columns - right */
        td:nth-child(3),
        td:nth-child(4) {
            text-align: right;
            font-family: "Courier New", monospace;
        }

        /* Status column - center */
        td:nth-child(5) {
            text-align: center;
        }

        tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        /* ==============================================
           STATUS BADGES
           ============================================== */
        .status {
            display: inline-block;
            padding: 4pt 10pt;
            border-radius: 12pt;
            font-size: 9pt;
            font-weight: 600;
            text-transform: uppercase;
        }

        .status-active {
            background-color: #d1fae5;
            color: #065f46;
        }

        .status-pending {
            background-color: #fef3c7;
            color: #92400e;
        }
    </style>
</head>
<body>
    <h1>Sales Report Q4 2024</h1>

    <table>
        <thead>
            <tr>
                <th>Salesperson</th>
                <th>Region</th>
                <th>Revenue</th>
                <th>Target</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>John Smith</td>
                <td>Northeast</td>
                <td>$450,000</td>
                <td>$400,000</td>
                <td><span class="status status-active">Active</span></td>
            </tr>
            <tr>
                <td>Sarah Johnson</td>
                <td>Southeast</td>
                <td>$380,000</td>
                <td>$375,000</td>
                <td><span class="status status-active">Active</span></td>
            </tr>
            <tr>
                <td>Michael Brown</td>
                <td>Midwest</td>
                <td>$290,000</td>
                <td>$300,000</td>
                <td><span class="status status-pending">Pending</span></td>
            </tr>
            <tr>
                <td>Emily Davis</td>
                <td>West</td>
                <td>$520,000</td>
                <td>$425,000</td>
                <td><span class="status status-active">Active</span></td>
            </tr>
        </tbody>
    </table>

    <p style="text-align: right; font-size: 10pt; color: #666; margin-top: 20pt;">
        Generated: January 15, 2025
    </p>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Alignment Showcase

Create a document demonstrating:
- Centered title
- Left-aligned headings
- Justified body text
- Right-aligned signature

### Exercise 2: Table Alignment

Create a financial table with:
- Centered headers
- Left-aligned text columns
- Right-aligned number columns
- Proper vertical alignment

### Exercise 3: Business Letter

Design a business letter with:
- Centered letterhead
- Left-aligned content
- Right-aligned date
- Right-aligned signature

---

## Common Pitfalls

### ❌ Justifying Short Lines

```css
.narrow-column {
    width: 150pt;
    text-align: justify;  /* Creates large gaps! */
}
```

✅ **Solution:**

```css
.narrow-column {
    width: 150pt;
    text-align: left;  /* Better for narrow text */
}
```

### ❌ Not Aligning Numbers Right

```css
/* Numbers left-aligned - hard to compare */
.numbers {
    text-align: left;
}
```

✅ **Solution:**

```css
.numbers {
    text-align: right;  /* Easy to compare */
}
```

### ❌ Centering Everything

```css
/* Everything centered - confusing! */
* {
    text-align: center;
}
```

✅ **Solution:**

```css
/* Selective centering */
h1 {
    text-align: center;
}

p {
    text-align: left;  /* Body text left */
}
```

---

## Text Alignment Checklist

- [ ] Body text left-aligned or justified
- [ ] Numbers right-aligned in tables
- [ ] Headers centered when appropriate
- [ ] Signatures right-aligned
- [ ] Table cells properly aligned
- [ ] Vertical alignment set for tables
- [ ] No justification on narrow columns

---

## Best Practices

1. **Body text:** Left or justified
2. **Numbers:** Right-aligned
3. **Headings:** Left or centered
4. **Tables:** Varied by column type
5. **Avoid justify:** On narrow columns
6. **Vertical align:** Top for table cells
7. **Be consistent:** Same alignment for same elements

---

## Next Steps

1. **[Advanced Typography](07_advanced_typography.md)** - Drop caps, special features
2. **[Typography Best Practices](08_typography_best_practices.md)** - Professional patterns
3. **[Content Components](/learning/06-content/)** - Images, lists, tables

---

**Continue learning →** [Advanced Typography](07_advanced_typography.md)
