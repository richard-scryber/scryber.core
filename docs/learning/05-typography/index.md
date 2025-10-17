---
layout: default
title: Typography & Fonts
nav_order: 5
parent: Learning Guides
parent_url: /learning/
has_children: true
has_toc: true
---

# Typography & Fonts

Master typography and font usage, including custom fonts, Google Fonts, Font Awesome, and CSS counters for professional PDF documents.

---

## Table of Contents

1. [Font Basics](01_font_basics.md) - Font families, properties, standard PDF fonts
2. [Custom Fonts](02_custom_fonts.md) - Loading TTF/OTF/WOFF fonts, @font-face, embedding
3. [Google Fonts](03_google_fonts.md) - Using Google Fonts in PDFs
4. [Font Awesome](04_font_awesome.md) - Icon fonts integration
5. [Web Fonts](05_web_fonts.md) - Loading remote fonts, CDNs, caching
6. [Text Metrics](06_text_metrics.md) - Line height, letter spacing, word spacing
7. [CSS Counters](07_counters.md) - Automatic numbering, counter functions
8. [Typography Best Practices](08_typography_best_practices.md) - Selection, readability, licensing

---

## Overview

Typography is crucial for creating professional, readable documents. This series covers everything from basic font properties to loading custom fonts from Google Fonts and Font Awesome, plus advanced features like CSS counters for automatic numbering.

## What Makes PDF Typography Special?

PDF typography has unique characteristics:

- **Font Embedding** - Fonts are embedded in the PDF for consistent rendering
- **Subsetting** - Only used characters are included to reduce file size
- **Standard Fonts** - 14 built-in fonts available without embedding
- **Custom Fonts** - Load TTF, OTF, and WOFF fonts
- **Remote Fonts** - Use Google Fonts and other web fonts
- **Icon Fonts** - Font Awesome and other icon libraries

## Quick Example

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <!-- Load Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;700&display=swap" rel="stylesheet" />

    <!-- Load Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />

    <style>
        body {
            font-family: 'Roboto', Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
        }

        h1 {
            font-family: 'Roboto', sans-serif;
            font-weight: 700;
            font-size: 24pt;
            counter-increment: chapter;
        }

        h1::before {
            content: "Chapter " counter(chapter) ": ";
            color: #666;
        }

        .icon {
            font-family: 'Font Awesome 6 Free';
            font-weight: 900;
        }
    </style>
</head>
<body>
    <h1>Getting Started</h1>
    <p><span class="icon"></span> Welcome to beautiful typography!</p>

    <h1>Advanced Features</h1>
    <p><span class="icon"></span> Learn more advanced concepts.</p>
</body>
</html>
```

## What You'll Learn

This series covers comprehensive typography for professional PDFs:

### 1. [Font Basics](01_font_basics.md)
- Font families and font stack
- font-family, font-size, font-weight, font-style
- Generic font families
- Standard 14 PDF fonts
- When to use standard fonts

### 2. [Custom Fonts](02_custom_fonts.md)
- Loading custom fonts
- Font file formats (TTF, OTF, WOFF)
- @font-face declaration
- Font registration and paths
- Font embedding and subsetting

### 3. [Google Fonts](03_google_fonts.md)
- Using Google Fonts in PDFs
- Loading fonts from Google Fonts
- Font selection and pairing
- Performance considerations
- Best practices

### 4. [Font Awesome](04_font_awesome.md)
- Font Awesome integration
- Icon fonts in PDFs
- Using icon classes and Unicode
- Icon sizing and styling
- Other icon font libraries

### 5. [Web Fonts](05_web_fonts.md)
- Loading remote fonts
- Web font URLs and CDNs
- Font caching
- Fallback strategies
- Cross-origin considerations

### 6. [Text Metrics](06_text_metrics.md)
- Line height (leading)
- Letter spacing (tracking)
- Word spacing
- Text indentation
- Optimal readability settings

### 7. [CSS Counters](07_counters.md)
- **CSS counters for automatic numbering**
- counter-reset and counter-increment
- counter() and counters() functions
- Nested counters
- Custom counter styles
- Practical examples (chapters, sections, figures)

### 8. [Typography Best Practices](08_typography_best_practices.md)
- Font selection and pairing
- Readability guidelines
- Performance and file size
- Font licensing compliance
- Accessibility considerations

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
    font-family: 'CustomFont';
    src: url('./fonts/CustomFont-Regular.ttf') format('truetype');
    font-weight: 400;
    font-style: normal;
}

@font-face {
    font-family: 'CustomFont';
    src: url('./fonts/CustomFont-Bold.ttf') format('truetype');
    font-weight: 700;
    font-style: normal;
}

body {
    font-family: 'CustomFont', Arial, sans-serif;
}
```

### Google Fonts

```html
<head>
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@300;400;700&display=swap" rel="stylesheet" />

    <style>
        body {
            font-family: 'Open Sans', sans-serif;
        }
    </style>
</head>
```

### Font Awesome Icons

```html
<head>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />

    <style>
        .fa {
            font-family: 'Font Awesome 6 Free';
            font-weight: 900;
        }
    </style>
</head>
<body>
    <p><span class="fa"></span> Check mark icon</p>
    <p><span class="fa"></span> Star icon</p>
</body>
```

## CSS Counters

### Automatic Chapter Numbering

```css
body {
    counter-reset: chapter;
}

h1 {
    counter-increment: chapter;
    counter-reset: section;
}

h1::before {
    content: "Chapter " counter(chapter) ": ";
}

h2 {
    counter-increment: section;
}

h2::before {
    content: counter(chapter) "." counter(section) " ";
}
```

Result:
```
Chapter 1: Introduction
1.1 Getting Started
1.2 Basic Concepts

Chapter 2: Advanced Topics
2.1 Advanced Features
2.2 Best Practices
```

### Figure Numbering

```css
body {
    counter-reset: figure;
}

.figure {
    counter-increment: figure;
}

.figure-caption::before {
    content: "Figure " counter(figure) ": ";
    font-weight: bold;
}
```

### Nested Lists with Counters

```css
ol {
    counter-reset: item;
    list-style-type: none;
}

ol li {
    counter-increment: item;
}

ol li::before {
    content: counters(item, ".") " ";
}
```

Result:
```
1 First item
  1.1 Nested item
  1.2 Another nested
2 Second item
  2.1 Nested here
```

## Text Metrics for Readability

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
@import url('https://fonts.googleapis.com/css2?family=Merriweather:wght@300;400;700&family=Open+Sans:wght@400;600&display=swap');

body {
    font-family: 'Open Sans', sans-serif;
    font-size: 11pt;
    line-height: 1.6;
    color: #333;
}

h1, h2, h3 {
    font-family: 'Merriweather', serif;
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

### Invoice with Icons

```html
<head>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />

    <style>
        .icon {
            font-family: 'Font Awesome 6 Free';
            font-weight: 900;
            color: #2563eb;
            margin-right: 5pt;
        }
    </style>
</head>
<body>
    <h1><span class="icon"></span> Invoice</h1>
    <p><span class="icon"></span> Date: {{invoice.date}}</p>
    <p><span class="icon"></span> Total: {{invoice.total}}</p>
</body>
```

### Technical Document with Counter

```css
body {
    counter-reset: chapter section figure table;
}

h1 {
    counter-increment: chapter;
    counter-reset: section figure table;
}

h1::before {
    content: counter(chapter) ". ";
}

h2 {
    counter-increment: section;
}

h2::before {
    content: counter(chapter) "." counter(section) " ";
}

.figure {
    counter-increment: figure;
}

.figure figcaption::before {
    content: "Figure " counter(chapter) "-" counter(figure) ": ";
    font-weight: bold;
}

.table caption::before {
    counter-increment: table;
    content: "Table " counter(chapter) "-" counter(table) ": ";
    font-weight: bold;
}
```

## Learning Path

**Recommended progression:**

1. **Master Font Basics** - Understand font properties
2. **Load Custom Fonts** - @font-face and local fonts
3. **Use Google Fonts** - Access thousands of fonts
4. **Add Icons** - Font Awesome integration
5. **Optimize Web Fonts** - Performance and caching
6. **Fine-Tune Metrics** - Line height and spacing
7. **Implement Counters** - Automatic numbering
8. **Apply Best Practices** - Professional typography

## Tips for Success

1. **Limit Font Families** - 2-3 fonts maximum per document
2. **Pair Fonts Wisely** - Serif for headers, sans-serif for body (or vice versa)
3. **Embed Only What You Need** - Subsetting reduces file size
4. **Test Font Loading** - Ensure fonts are available
5. **Use Fallbacks** - Always specify generic font family
6. **Check Licensing** - Verify font licenses for PDF embedding
7. **Optimize Line Height** - 1.4-1.6 for body text
8. **Use Counters** - Automatic numbering is more maintainable

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
- [Google Fonts](03_google_fonts.md) for easy custom fonts
- [Font Awesome](04_font_awesome.md) for icons
- [CSS Counters](07_counters.md) for automatic numbering

---

**Related Series:**
- [Styling & Appearance](/learning/03-styling/) - Text styling basics
- [Content Components](/learning/06-content/) - Using fonts with content
- [Practical Applications](/learning/08-practical/) - Real-world examples

---

**Begin your typography journey →** [Font Basics](01_font_basics.md)
