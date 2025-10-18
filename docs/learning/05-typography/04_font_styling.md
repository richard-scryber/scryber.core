---
layout: default
title: Font Styling & Transforms
nav_order: 4
parent: Typography & Fonts
parent_url: /learning/05-typography/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Font Styling & Transforms

Master advanced font styling techniques including text transforms, decorations, and visual effects.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Apply text transforms (uppercase, lowercase, capitalize)
- Use text decorations (underline, line-through)
- Control font rendering with text-rendering
- Apply text shadows effectively
- Create visual hierarchy with styling
- Understand PDF-specific limitations

---

## Text Transform

Change text case without modifying content.

### Transform Values

```css
/* Uppercase - ALL CAPS */
.uppercase {
    text-transform: uppercase;
}

/* Lowercase - all lowercase */
.lowercase {
    text-transform: lowercase;
}

/* Capitalize - First Letter Of Each Word */
.capitalize {
    text-transform: capitalize;
}

/* None - preserve original case */
.normal {
    text-transform: none;
}
```

### Practical Usage

```html
<p style="text-transform: uppercase;">
    this text will display as: THIS TEXT WILL DISPLAY AS
</p>

<p style="text-transform: lowercase;">
    THIS TEXT will display as: this text
</p>

<p style="text-transform: capitalize;">
    this text will display as: This Text Will Display As
</p>
```

### Common Applications

```css
/* Headings in all caps */
h1 {
    text-transform: uppercase;
    letter-spacing: 2pt;  /* Add spacing for readability */
}

/* Proper names */
.name {
    text-transform: capitalize;
}

/* Code or technical text */
.code-constant {
    text-transform: uppercase;
}

/* Form labels */
label {
    text-transform: capitalize;
}
```

---

## Text Decoration

Add visual emphasis with underlines and strikethroughs.

### Decoration Types

```css
/* No decoration */
.none {
    text-decoration: none;
}

/* Underline */
.underline {
    text-decoration: underline;
}

/* Overline (line above) */
.overline {
    text-decoration: overline;
}

/* Line-through (strikethrough) */
.line-through {
    text-decoration: line-through;
}

/* Multiple decorations */
.multiple {
    text-decoration: underline overline;
}
```

### Decoration Style (CSS3)

```css
/* Solid line (default) */
.solid {
    text-decoration: underline solid;
}

/* Dotted line */
.dotted {
    text-decoration: underline dotted;
}

/* Dashed line */
.dashed {
    text-decoration: underline dashed;
}

/* Wavy line */
.wavy {
    text-decoration: underline wavy;
}

/* Double line */
.double {
    text-decoration: underline double;
}
```

**Note:** Advanced decoration styles may have limited support in PDF. Test thoroughly.

### Decoration Color

```css
.colored-underline {
    text-decoration: underline;
    text-decoration-color: #2563eb;
}

/* Or shorthand */
.shorthand {
    text-decoration: underline solid #2563eb;
}
```

### Common Applications

```css
/* Remove underline from links */
a {
    text-decoration: none;
}

/* Deleted text */
.deleted {
    text-decoration: line-through;
    color: #dc2626;
}

/* Important terms */
.term {
    text-decoration: underline;
    text-decoration-color: #2563eb;
}

/* Emphasized links */
a:hover {
    text-decoration: underline;
}
```

---

## Text Shadow

Add shadows to text for depth and emphasis.

### Basic Syntax

```css
/* text-shadow: horizontal vertical blur color */
.shadow {
    text-shadow: 2pt 2pt 4pt rgba(0, 0, 0, 0.3);
}
```

### Shadow Examples

```css
/* Simple drop shadow */
.drop-shadow {
    text-shadow: 1pt 1pt 2pt rgba(0, 0, 0, 0.2);
}

/* Distant shadow */
.distant {
    text-shadow: 3pt 3pt 5pt rgba(0, 0, 0, 0.4);
}

/* Soft shadow */
.soft {
    text-shadow: 0pt 0pt 10pt rgba(0, 0, 0, 0.3);
}

/* Multiple shadows */
.multiple {
    text-shadow:
        1pt 1pt 2pt rgba(0, 0, 0, 0.2),
        -1pt -1pt 2pt rgba(255, 255, 255, 0.5);
}

/* Embossed effect */
.embossed {
    color: #333;
    text-shadow:
        0pt 1pt 0pt rgba(255, 255, 255, 0.8),
        0pt -1pt 0pt rgba(0, 0, 0, 0.3);
}
```

**Note:** Text shadows may have limited or no support in PDF generators. Test in your environment.

---

## Font Variant Numeric

Control number formatting.

```css
/* Tabular nums - fixed width for tables */
.tabular {
    font-variant-numeric: tabular-nums;
}

/* Lining nums - numbers same height as caps */
.lining {
    font-variant-numeric: lining-nums;
}

/* Oldstyle nums - varying heights */
.oldstyle {
    font-variant-numeric: oldstyle-nums;
}

/* Proportional - default width */
.proportional {
    font-variant-numeric: proportional-nums;
}
```

**Note:** Requires OpenType font features. Support varies.

---

## Practical Examples

### Example 1: Text Transform Showcase

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Text Transforms</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        h1 {
            font-size: 28pt;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 30pt;
            text-align: center;
        }

        /* ==============================================
           TRANSFORM EXAMPLES
           ============================================== */
        .example {
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
            padding: 20pt;
            margin-bottom: 20pt;
        }

        .example-label {
            font-size: 10pt;
            font-weight: bold;
            color: #666;
            margin-bottom: 10pt;
        }

        .example-text {
            font-size: 16pt;
            margin: 0;
        }

        /* Transforms */
        .uppercase-demo {
            text-transform: uppercase;
            letter-spacing: 1pt;
        }

        .lowercase-demo {
            text-transform: lowercase;
        }

        .capitalize-demo {
            text-transform: capitalize;
        }

        /* ==============================================
           SPECIFIC USE CASES
           ============================================== */
        .header-caps {
            text-transform: uppercase;
            font-size: 20pt;
            font-weight: bold;
            letter-spacing: 2pt;
            color: #2563eb;
            margin: 30pt 0 15pt 0;
        }

        .label {
            text-transform: capitalize;
            font-weight: 600;
            display: inline-block;
            width: 150pt;
        }

        .code-constant {
            text-transform: uppercase;
            font-family: "Courier New", monospace;
            background-color: #f9fafb;
            padding: 2pt 5pt;
            border-radius: 3pt;
        }
    </style>
</head>
<body>
    <h1>Text Transform Examples</h1>

    <!-- Transform demonstrations -->
    <div class="example">
        <div class="example-label">text-transform: uppercase</div>
        <p class="example-text uppercase-demo">
            this text is transformed to uppercase automatically
        </p>
    </div>

    <div class="example">
        <div class="example-label">text-transform: lowercase</div>
        <p class="example-text lowercase-demo">
            THIS TEXT IS TRANSFORMED TO LOWERCASE AUTOMATICALLY
        </p>
    </div>

    <div class="example">
        <div class="example-label">text-transform: capitalize</div>
        <p class="example-text capitalize-demo">
            this text capitalizes the first letter of each word
        </p>
    </div>

    <!-- Practical applications -->
    <h2 class="header-caps">Practical Applications</h2>

    <p>
        <span class="label">customer name:</span>
        john smith
    </p>

    <p>
        <span class="label">product code:</span>
        <span class="code-constant">prod_2024_001</span>
    </p>

    <p>
        <span class="label">status:</span>
        <span style="text-transform: uppercase; color: #059669; font-weight: bold;">active</span>
    </p>

    <div style="margin-top: 30pt; padding: 20pt; background-color: #f9fafb; border-radius: 5pt;">
        <h3 style="text-transform: uppercase; letter-spacing: 1pt; font-size: 12pt; margin-top: 0;">
            Important Notice
        </h3>
        <p style="margin-bottom: 0;">
            Text transforms allow you to control text case without modifying the underlying data.
            This is useful for consistent formatting and styling.
        </p>
    </div>
</body>
</html>
```

### Example 2: Text Decorations

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Text Decorations</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Georgia, "Times New Roman", serif;
            font-size: 11pt;
            line-height: 1.8;
            margin: 0;
        }

        h1 {
            font-family: Helvetica, sans-serif;
            font-size: 28pt;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 30pt;
            text-align: center;
        }

        h2 {
            font-family: Helvetica, sans-serif;
            font-size: 20pt;
            color: #2563eb;
            margin-top: 30pt;
            margin-bottom: 15pt;
        }

        p {
            margin-bottom: 15pt;
        }

        /* ==============================================
           DECORATION STYLES
           ============================================== */
        .underline {
            text-decoration: underline;
        }

        .line-through {
            text-decoration: line-through;
            color: #dc2626;
        }

        .overline {
            text-decoration: overline;
        }

        /* Links */
        a {
            color: #2563eb;
            text-decoration: none;  /* No underline by default */
        }

        a:hover {
            text-decoration: underline;
        }

        /* Special emphasis */
        .important-term {
            text-decoration: underline;
            text-decoration-color: #2563eb;
            font-weight: 600;
        }

        /* Deleted content */
        .deleted {
            text-decoration: line-through;
            color: #999;
        }

        /* Updated content */
        .updated {
            text-decoration: underline;
            text-decoration-color: #059669;
            color: #059669;
        }

        /* Abbreviations */
        abbr {
            text-decoration: underline dotted;
            cursor: help;
        }

        /* ==============================================
           DOCUMENT MARKUP
           ============================================== */
        .changes {
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
            padding: 20pt;
            margin: 25pt 0;
        }

        .changes h3 {
            font-family: Helvetica, sans-serif;
            margin-top: 0;
            font-size: 14pt;
            color: #2563eb;
        }
    </style>
</head>
<body>
    <h1>Text Decoration Techniques</h1>

    <p>
        Text decorations provide visual emphasis and meaning. This document demonstrates
        various <span class="important-term">decoration techniques</span> for professional
        PDF documents.
    </p>

    <h2>Basic Decorations</h2>

    <p>
        <span class="underline">Underlined text</span> draws attention to important content.
        <span class="line-through">Strikethrough text</span> indicates removed or outdated information.
        <span class="overline">Overline text</span> is less common but occasionally useful.
    </p>

    <h2>Links and References</h2>

    <p>
        Links like <a href="#">this example link</a> typically use color to indicate
        interactivity. Hover effects (underlines) help users identify clickable elements,
        though this is less relevant in static PDF documents.
    </p>

    <h2>Document Revisions</h2>

    <div class="changes">
        <h3>Change Log</h3>

        <p>
            The project deadline has been <span class="deleted">moved to June 15</span>
            <span class="updated">extended to June 30</span> to accommodate additional testing.
        </p>

        <p>
            Budget allocation: <span class="deleted">$50,000</span>
            <span class="updated">$75,000</span>
        </p>
    </div>

    <h2>Technical Terms</h2>

    <p>
        When using abbreviations like <abbr title="Portable Document Format">PDF</abbr> or
        <abbr title="HyperText Markup Language">HTML</abbr>, decorations can indicate
        that additional information is available.
    </p>

    <p style="margin-top: 30pt; padding: 15pt; background-color: #eff6ff; border-left: 4pt solid #2563eb;">
        <strong>Best Practice:</strong> Use text decorations sparingly and purposefully.
        Too many decorations create visual noise and reduce readability.
    </p>
</body>
</html>
```

### Example 3: Combined Styling

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Combined Font Styling</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
            margin: 0;
        }

        /* ==============================================
           HEADER STYLING
           ============================================== */
        .document-header {
            text-align: center;
            padding-bottom: 20pt;
            margin-bottom: 30pt;
            border-bottom: 3pt solid #2563eb;
        }

        .document-header h1 {
            font-size: 32pt;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 2pt;
            color: #1e40af;
            margin: 0 0 10pt 0;
        }

        .document-header .subtitle {
            font-size: 14pt;
            text-transform: capitalize;
            color: #666;
            font-style: italic;
        }

        /* ==============================================
           SECTION HEADERS
           ============================================== */
        h2 {
            font-size: 20pt;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1pt;
            color: #2563eb;
            margin-top: 30pt;
            margin-bottom: 15pt;
            padding-bottom: 8pt;
            border-bottom: 2pt solid #e5e7eb;
        }

        h3 {
            font-size: 14pt;
            font-weight: 600;
            text-transform: capitalize;
            color: #3b82f6;
            margin-top: 20pt;
            margin-bottom: 10pt;
        }

        /* ==============================================
           TEXT ELEMENTS
           ============================================== */
        p {
            margin-top: 0;
            margin-bottom: 12pt;
        }

        /* Labels */
        .label {
            display: inline-block;
            font-size: 9pt;
            text-transform: uppercase;
            letter-spacing: 0.5pt;
            font-weight: 600;
            color: #666;
            width: 120pt;
        }

        /* Values */
        .value {
            font-weight: 500;
        }

        /* Status indicators */
        .status {
            display: inline-block;
            padding: 3pt 8pt;
            border-radius: 3pt;
            font-size: 9pt;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5pt;
        }

        .status-active {
            background-color: #d1fae5;
            color: #065f46;
        }

        .status-pending {
            background-color: #fef3c7;
            color: #92400e;
        }

        .status-inactive {
            background-color: #fee2e2;
            color: #991b1b;
        }

        /* Pricing */
        .price {
            font-size: 24pt;
            font-weight: 700;
            color: #2563eb;
        }

        .price-label {
            font-size: 10pt;
            text-transform: uppercase;
            letter-spacing: 1pt;
            color: #666;
            font-weight: 600;
        }

        /* ==============================================
           DATA TABLE
           ============================================== */
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        thead th {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
            text-align: left;
            font-size: 10pt;
            text-transform: uppercase;
            letter-spacing: 0.5pt;
        }

        td {
            padding: 10pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }
    </style>
</head>
<body>
    <!-- Document Header -->
    <div class="document-header">
        <h1>Annual Report 2024</h1>
        <div class="subtitle">financial performance and strategic overview</div>
    </div>

    <!-- Executive Summary -->
    <h2>Executive Summary</h2>

    <p>
        This report presents a comprehensive analysis of our performance throughout the
        fiscal year 2024, highlighting key achievements and strategic initiatives.
    </p>

    <!-- Key Metrics -->
    <h2>Key Metrics</h2>

    <div style="display: table; width: 100%; margin-bottom: 25pt;">
        <div style="display: table-cell; width: 33.33%; padding: 15pt; text-align: center; border: 1pt solid #e5e7eb;">
            <div class="price-label">Revenue</div>
            <div class="price">$12.5M</div>
        </div>
        <div style="display: table-cell; width: 33.33%; padding: 15pt; text-align: center; border: 1pt solid #e5e7eb; border-left: none;">
            <div class="price-label">Growth</div>
            <div class="price" style="color: #059669;">+15%</div>
        </div>
        <div style="display: table-cell; width: 33.33%; padding: 15pt; text-align: center; border: 1pt solid #e5e7eb; border-left: none;">
            <div class="price-label">Customers</div>
            <div class="price">1,247</div>
        </div>
    </div>

    <!-- Project Status -->
    <h2>Project Status</h2>

    <p>
        <span class="label">Project Alpha:</span>
        <span class="value">Implementation Phase</span>
        <span class="status status-active">Active</span>
    </p>

    <p>
        <span class="label">Project Beta:</span>
        <span class="value">Planning Phase</span>
        <span class="status status-pending">Pending</span>
    </p>

    <p>
        <span class="label">Project Gamma:</span>
        <span class="value">On Hold</span>
        <span class="status status-inactive">Inactive</span>
    </p>

    <!-- Performance Table -->
    <h3>Quarterly Performance</h3>

    <table>
        <thead>
            <tr>
                <th>Quarter</th>
                <th style="text-align: right;">Revenue</th>
                <th style="text-align: right;">Growth</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Q1 2024</td>
                <td style="text-align: right;">$2.8M</td>
                <td style="text-align: right; color: #059669;">+12%</td>
                <td><span class="status status-active">Target Met</span></td>
            </tr>
            <tr>
                <td>Q2 2024</td>
                <td style="text-align: right;">$3.1M</td>
                <td style="text-align: right; color: #059669;">+15%</td>
                <td><span class="status status-active">Target Met</span></td>
            </tr>
            <tr>
                <td>Q3 2024</td>
                <td style="text-align: right;">$3.3M</td>
                <td style="text-align: right; color: #059669;">+18%</td>
                <td><span class="status status-active">Target Exceeded</span></td>
            </tr>
            <tr>
                <td>Q4 2024</td>
                <td style="text-align: right;">$3.3M</td>
                <td style="text-align: right; color: #059669;">+14%</td>
                <td><span class="status status-active">Target Met</span></td>
            </tr>
        </tbody>
    </table>

    <p style="margin-top: 30pt; font-size: 9pt; color: #666; text-align: center;">
        <span style="text-transform: uppercase; letter-spacing: 0.5pt;">Confidential</span> — For Internal Use Only
    </p>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Text Transform Practice

Create a document with:
- Uppercase headers with letter spacing
- Capitalized labels
- Lowercase technical identifiers
- Mixed transforms in context

### Exercise 2: Decoration Styles

Create a style guide showing:
- Different underline styles
- Strikethrough for deletions
- Color-coded decorations
- Practical use cases

### Exercise 3: Professional Document

Design a professional report with:
- Uppercase section headers
- Capitalized labels
- Status indicators with transforms
- Decorated important terms

---

## Common Pitfalls

### ❌ Overusing Uppercase

```css
/* Everything in uppercase - hard to read! */
body {
    text-transform: uppercase;
}
```

✅ **Solution:** Use selectively

```css
h1, h2 {
    text-transform: uppercase;
    letter-spacing: 1pt;  /* Add spacing for readability */
}
```

### ❌ Excessive Decorations

```css
.too-much {
    text-decoration: underline overline;
    text-transform: uppercase;
    font-weight: bold;
    color: red;  /* Too many effects! */
}
```

✅ **Solution:** Use restraint

```css
.balanced {
    text-decoration: underline;
    text-decoration-color: #2563eb;
}
```

### ❌ No Letter Spacing with Uppercase

```css
.cramped {
    text-transform: uppercase;  /* No letter-spacing */
}
```

✅ **Solution:** Add letter spacing

```css
.readable {
    text-transform: uppercase;
    letter-spacing: 1pt;  /* Improves readability */
}
```

### ❌ Assuming Shadow Support

```css
.fancy {
    text-shadow: 2pt 2pt 5pt rgba(0, 0, 0, 0.5);  /* May not work in PDF! */
}
```

✅ **Solution:** Test or avoid

```css
/* Use simpler styling for PDF */
.reliable {
    font-weight: bold;
    color: #2563eb;
}
```

---

## Font Styling Checklist

- [ ] Text transforms used purposefully
- [ ] Letter spacing added to uppercase text
- [ ] Decorations enhance, not distract
- [ ] Tested in generated PDF
- [ ] No reliance on unsupported features (shadows)
- [ ] Consistent styling throughout document
- [ ] Accessible color choices for decorations

---

## Best Practices

1. **Use uppercase sparingly** - Headers and labels only
2. **Add letter spacing** - When using uppercase
3. **Purposeful decorations** - Convey meaning, not just style
4. **Test in PDF** - Not all effects supported
5. **Maintain readability** - Don't sacrifice clarity for style
6. **Be consistent** - Same styling for same purposes
7. **Consider accessibility** - Sufficient color contrast
8. **Use restraint** - Less is often more

---

## Next Steps

Now that you understand font styling:

1. **[Text Spacing](05_text_spacing.md)** - Line height, letter spacing, word spacing
2. **[Text Alignment](06_text_alignment.md)** - Alignment and justification
3. **[Typography Best Practices](08_typography_best_practices.md)** - Professional patterns

---

**Continue learning →** [Text Spacing](05_text_spacing.md)
