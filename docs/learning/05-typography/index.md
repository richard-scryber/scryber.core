---
layout: default
title: Typography & Fonts
nav_order: 5
parent: Learning Guides
parent_url: /learning/
has_children: true
has_toc: false
---

# Typography & Fonts

Master typography and font usage, including font properties, custom fonts, text spacing, alignment, and advanced typographic techniques for professional PDF documents.

---

## Table of Contents

1. [Font Basics](01_font_basics.md) - Font families, sizes, weights, styles, system fonts
2. [Custom Fonts](02_custom_fonts.md) - Loading TTF/OTF fonts, @font-face, embedding
3. [Web Fonts](03_web_fonts.md) - Using Google Fonts locally in PDFs
4. [Font Styling](04_font_styling.md) - Text transforms, decorations, shadows
5. [Text Spacing](05_text_spacing.md) - Line height, letter spacing, vertical rhythm
6. [Text Alignment](06_text_alignment.md) - Horizontal/vertical alignment, justification
7. [Advanced Typography](07_advanced_typography.md) - Drop caps, special characters, entities
8. [Typography Best Practices](08_typography_best_practices.md) - Professional patterns, optimization

---

## Overview

Typography is crucial for creating professional, readable documents. This series covers everything from basic font properties to loading custom fonts, controlling text spacing and alignment, and applying advanced typographic techniques for polished PDF output.

## What Makes PDF Typography Special?

PDF typography has unique characteristics:

- **Font Embedding** - Fonts are embedded in the PDF for consistent rendering
- **Subsetting** - Only used characters are included to reduce file size
- **Standard Fonts** - 14 built-in fonts available without embedding
- **Custom Fonts** - Load TTF and OTF fonts via @font-face
- **Local Hosting** - Web fonts must be downloaded and hosted locally for PDF
- **Points Over Pixels** - Use pt units for precision in print

## Quick Example

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Professional Typography</title>
    <style>
        /* Load custom font */
        @font-face {
            font-family: 'Roboto';
            src: url('./fonts/Roboto-Regular.ttf') format('truetype');
            font-weight: 400;
        }

        @font-face {
            font-family: 'Roboto';
            src: url('./fonts/Roboto-Bold.ttf') format('truetype');
            font-weight: 700;
        }

        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: 'Roboto', Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
        }

        h1 {
            font-size: 24pt;
            font-weight: 700;
            line-height: 1.2;
            text-align: center;
            text-transform: uppercase;
            letter-spacing: 2pt;
        }

        p {
            text-align: justify;
            margin-bottom: 12pt;
        }
    </style>
</head>
<body>
    <h1>Professional Typography</h1>
    <p>
        This example demonstrates professional typography with custom fonts,
        optimal spacing, and proper alignment for PDF documents.
    </p>
</body>
</html>
```

## What You'll Learn

This series covers comprehensive typography for professional PDFs:

### 1. [Font Basics](01_font_basics.md)
- Font families and font stacks
- font-family, font-size, font-weight, font-style
- Generic font families (serif, sans-serif, monospace)
- Standard 14 PDF fonts
- System fonts and fallbacks

### 2. [Custom Fonts](02_custom_fonts.md)
- Loading custom fonts with @font-face
- Font file formats (TTF, OTF)
- Loading multiple font weights and styles
- Font paths and organization
- Font embedding in PDFs

### 3. [Web Fonts](03_web_fonts.md)
- Using Google Fonts in PDFs
- Why CDN links don't work for PDF generation
- Downloading and hosting fonts locally
- Font format selection
- Performance considerations

### 4. [Font Styling](04_font_styling.md)
- Text transforms (uppercase, lowercase, capitalize)
- Text decorations (underline, line-through, overline)
- Text shadows (limited support in PDF)
- Combining text properties
- Practical styling patterns

### 5. [Text Spacing](05_text_spacing.md)
- Line height (leading) for readability
- Letter spacing (tracking)
- Word spacing
- Vertical rhythm and baseline grids
- Optimal spacing values

### 6. [Text Alignment](06_text_alignment.md)
- Horizontal alignment (left, right, center, justify)
- Vertical alignment (top, middle, bottom, baseline)
- Justification considerations
- Table cell alignment
- Alignment best practices

### 7. [Advanced Typography](07_advanced_typography.md)
- Drop caps for articles
- Special characters and HTML entities
- Smart quotes and proper dashes
- Subscript and superscript
- Ligatures and small caps (when supported)

### 8. [Typography Best Practices](08_typography_best_practices.md)
- Professional typography patterns
- Clear visual hierarchy
- Font loading optimization
- Readability guidelines
- Common mistakes and solutions

## Prerequisites

Before starting this series:

- **Complete [Getting Started](/learning/01-getting-started/)** - Basic Scryber knowledge
- **Review [Styling & Appearance](/learning/03-styling/)** - CSS fundamentals

## Key Concepts

### Font Stack

Specify fallback fonts for reliability:

```css
body {
    font-family: 'Roboto', 'Helvetica Neue', Arial, sans-serif;
}
```

Order matters:
1. **Primary font** - Your preferred font
2. **Similar fallback** - Close alternative
3. **Generic font** - System default (sans-serif, serif, monospace)

### Font Properties

```css
.text {
    font-family: 'Arial', sans-serif;  /* Font name */
    font-size: 12pt;                    /* Size */
    font-weight: 700;                   /* Weight (100-900 or bold) */
    font-style: italic;                 /* Style */
    font-variant: small-caps;           /* Variant */
    line-height: 1.5;                   /* Leading */
    letter-spacing: 0.5pt;              /* Tracking */
}
```

### Standard PDF Fonts

These 14 fonts work without embedding:

**Serif:**
- Times-Roman
- Times-Bold
- Times-Italic
- Times-BoldItalic

**Sans-Serif:**
- Helvetica
- Helvetica-Bold
- Helvetica-Oblique
- Helvetica-BoldOblique

**Monospace:**
- Courier
- Courier-Bold
- Courier-Oblique
- Courier-BoldOblique

**Symbol:**
- Symbol
- ZapfDingbats

## Loading Custom Fonts

### Using @font-face

```css
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Regular.ttf') format('truetype');
    font-weight: 400;
    font-style: normal;
}

@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Bold.ttf') format('truetype');
    font-weight: 700;
    font-style: normal;
}

body {
    font-family: 'Roboto', Arial, sans-serif;
}
```

### Google Fonts (Local Hosting)

```css
/* Download fonts from Google Fonts and host locally */
@font-face {
    font-family: 'Open Sans';
    src: url('./fonts/OpenSans-Regular.ttf') format('truetype');
    font-weight: 400;
}

@font-face {
    font-family: 'Open Sans';
    src: url('./fonts/OpenSans-Bold.ttf') format('truetype');
    font-weight: 700;
}

body {
    font-family: 'Open Sans', Arial, sans-serif;
}
```

### Text Styling

```css
h1 {
    text-transform: uppercase;
    letter-spacing: 2pt;
    text-align: center;
}

.highlight {
    text-decoration: underline;
    color: #2563eb;
}
```

## Text Spacing for Readability

### Line Height

```css
/* Too tight */
p {
    line-height: 1.0;  /* Hard to read */
}

/* Optimal */
p {
    line-height: 1.5;  /* Good readability */
}

/* Too loose */
p {
    line-height: 2.5;  /* Text feels disconnected */
}
```

### Letter Spacing

```css
/* Headers benefit from spacing */
h1 {
    letter-spacing: 1pt;
    text-transform: uppercase;
}

/* Body text usually doesn't need it */
p {
    letter-spacing: 0;
}

/* Condensed for space */
.compact {
    letter-spacing: -0.25pt;
}
```

## Real-World Examples

### Professional Document

```css
/* Download and host fonts locally for PDF */
@font-face {
    font-family: 'Merriweather';
    src: url('./fonts/Merriweather-Regular.ttf') format('truetype');
    font-weight: 400;
}

@font-face {
    font-family: 'Merriweather';
    src: url('./fonts/Merriweather-Bold.ttf') format('truetype');
    font-weight: 700;
}

@font-face {
    font-family: 'Open Sans';
    src: url('./fonts/OpenSans-Regular.ttf') format('truetype');
    font-weight: 400;
}

body {
    font-family: 'Open Sans', Arial, sans-serif;
    font-size: 11pt;
    line-height: 1.6;
    color: #333;
}

h1, h2, h3 {
    font-family: 'Merriweather', Georgia, serif;
    line-height: 1.3;
}

h1 {
    font-size: 24pt;
    font-weight: 700;
    letter-spacing: -0.5pt;
}

h2 {
    font-size: 18pt;
    font-weight: 700;
}

h3 {
    font-size: 14pt;
    font-weight: 400;
}

code {
    font-family: 'Courier New', monospace;
    font-size: 10pt;
}
```

### Business Report with Special Characters

```html
<body>
    <h1>Annual Report 2024</h1>
    <p>&copy; 2025 Acme Corporation&reg;. All rights reserved.</p>

    <h2>Executive Summary</h2>
    <p>
        Revenue increased by 15&ndash;20% year-over-year&mdash;our best
        performance to date. The temperature range for optimal production
        is 20&deg;C&ndash;25&deg;C.
    </p>

    <p>
        Chemical formula: H<sub>2</sub>O<br/>
        Einstein&rsquo;s equation: E = mc<sup>2</sup>
    </p>
</body>
```

### Magazine Article with Drop Cap

```css
.article p:first-of-type::first-letter {
    float: left;
    font-size: 4em;
    line-height: 0.8;
    margin-right: 0.1em;
    font-weight: bold;
    color: #2563eb;
}

.article p {
    text-align: justify;
    line-height: 1.7;
}

blockquote {
    border-left: 4pt solid #2563eb;
    padding-left: 20pt;
    font-style: italic;
    margin: 20pt 0;
}
```

## Learning Path

**Recommended progression:**

1. **Master Font Basics** - Understand font properties and families
2. **Load Custom Fonts** - @font-face and TTF/OTF files
3. **Use Web Fonts** - Google Fonts local hosting for PDF
4. **Apply Font Styling** - Transforms, decorations, effects
5. **Control Text Spacing** - Line height, letter spacing, rhythm
6. **Master Alignment** - Horizontal and vertical text alignment
7. **Advanced Typography** - Drop caps, special characters, entities
8. **Apply Best Practices** - Professional patterns and optimization

## Tips for Success

1. **Limit Font Families** - 2-3 fonts maximum per document
2. **Pair Fonts Wisely** - Serif for headers, sans-serif for body (or vice versa)
3. **Host Fonts Locally** - Download web fonts for reliable PDF generation
4. **Test Font Loading** - Ensure fonts are available and embedded correctly
5. **Use Fallbacks** - Always specify complete font stack with generic family
6. **Optimize Line Height** - 1.5-1.7 for body text, 1.1-1.3 for headings
7. **Add Letter Spacing** - 1-2pt for uppercase text improves readability
8. **Use Smart Quotes** - HTML entities for professional typography

## Common Pitfalls

❌ **Too many fonts**
```css
/* Confusing and inconsistent */
h1 { font-family: 'Font1'; }
h2 { font-family: 'Font2'; }
p { font-family: 'Font3'; }
```

✅ **Limited, consistent font usage**
```css
h1, h2, h3 { font-family: 'Merriweather', serif; }
body { font-family: 'Open Sans', sans-serif; }
code { font-family: 'Courier New', monospace; }
```

❌ **Missing fallbacks**
```css
body { font-family: 'CustomFont'; }
```

✅ **Always include fallbacks**
```css
body { font-family: 'CustomFont', Arial, sans-serif; }
```

## Next Steps

Ready to master typography? Start with [Font Basics](01_font_basics.md) to understand font properties and the standard PDF fonts.

Jump to specific topics:
- [Web Fonts](03_web_fonts.md) for using Google Fonts locally
- [Text Spacing](05_text_spacing.md) for optimal readability
- [Advanced Typography](07_advanced_typography.md) for professional details

---

**Related Series:**
- [Styling & Appearance](/learning/03-styling/) - Text styling basics
- [Content Components](/learning/06-content/) - Using fonts with content
- [Practical Applications](/learning/08-practical/) - Real-world examples

---

**Begin your typography journey →** [Font Basics](01_font_basics.md)
