---
layout: default
title: Font Basics
nav_order: 1
parent: Typography & Fonts
parent_url: /learning/05-typography/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Font Basics

Master fundamental font properties and typography concepts for professional PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Understand font families and fallbacks
- Use font properties (size, weight, style)
- Apply system fonts reliably
- Understand font rendering in PDF
- Create readable, accessible typography
- Choose appropriate fonts for different contexts

---

## Font Families

### Generic Font Families

CSS defines five generic font families:

```css
/* Serif - traditional, formal */
.serif {
    font-family: serif;
}

/* Sans-serif - modern, clean */
.sans-serif {
    font-family: sans-serif;
}

/* Monospace - code, technical */
.monospace {
    font-family: monospace;
}

/* Cursive - handwriting style */
.cursive {
    font-family: cursive;
}

/* Fantasy - decorative */
.fantasy {
    font-family: fantasy;
}
```

### Specific Font Families

Named fonts with fallbacks:

```css
/* Helvetica with fallbacks */
body {
    font-family: Helvetica, Arial, sans-serif;
}

/* Times with fallbacks */
.formal {
    font-family: "Times New Roman", Times, serif;
}

/* Monaco for code */
.code {
    font-family: Monaco, "Courier New", monospace;
}
```

**Fallback chain:**
1. Try first font (Helvetica)
2. If unavailable, try second (Arial)
3. If unavailable, use generic (sans-serif)

### Quoting Font Names

Use quotes for multi-word font names:

```css
/* ❌ Wrong */
font-family: Times New Roman;

/* ✅ Correct */
font-family: "Times New Roman";

/* Single words don't need quotes */
font-family: Helvetica;  /* OK */
font-family: "Helvetica";  /* Also OK */
```

---

## Common System Fonts

### Sans-Serif Fonts

**Helvetica** - Clean, professional (Mac/iOS default)
```css
font-family: Helvetica, Arial, sans-serif;
```

**Arial** - Universal, Windows default
```css
font-family: Arial, Helvetica, sans-serif;
```

**Verdana** - Highly readable on screens
```css
font-family: Verdana, Geneva, sans-serif;
```

### Serif Fonts

**Times New Roman** - Traditional, formal
```css
font-family: "Times New Roman", Times, serif;
```

**Georgia** - Readable, elegant
```css
font-family: Georgia, "Times New Roman", serif;
```

**Palatino** - Classic, refined
```css
font-family: Palatino, "Palatino Linotype", serif;
```

### Monospace Fonts

**Courier** - Standard monospace
```css
font-family: Courier, "Courier New", monospace;
```

**Monaco** - Clean code font
```css
font-family: Monaco, Consolas, monospace;
```

**Consolas** - Windows code font
```css
font-family: Consolas, Monaco, monospace;
```

---

## Font Size

### Units for Font Size

**Points (pt) - Recommended for PDF:**
```css
body {
    font-size: 11pt;  /* Standard body text */
}

h1 {
    font-size: 24pt;  /* Large heading */
}

.small {
    font-size: 9pt;  /* Fine print */
}
```

**Pixels (px):**
```css
body {
    font-size: 12px;  /* Converted to points in PDF */
}
```

**Em and Rem:**
```css
html {
    font-size: 12pt;  /* Base size */
}

h1 {
    font-size: 2rem;  /* 24pt (2 × 12pt) */
}

.large {
    font-size: 1.5em;  /* 1.5 × parent size */
}
```

### Common Font Sizes

```css
/* ==============================================
   TYPOGRAPHY SCALE
   ============================================== */
.text-xs { font-size: 9pt; }    /* Fine print */
.text-sm { font-size: 10pt; }   /* Small text */
.text-base { font-size: 11pt; } /* Body text */
.text-lg { font-size: 12pt; }   /* Slightly larger */
.text-xl { font-size: 14pt; }   /* Large text */
.text-2xl { font-size: 18pt; }  /* Subheading */
.text-3xl { font-size: 24pt; }  /* Heading */
.text-4xl { font-size: 30pt; }  /* Large heading */
.text-5xl { font-size: 36pt; }  /* Display */
```

---

## Font Weight

Controls boldness of text.

### Keyword Values

```css
.light {
    font-weight: lighter;  /* Lighter than parent */
}

.normal {
    font-weight: normal;  /* Default (400) */
}

.bold {
    font-weight: bold;  /* Bold (700) */
}

.bolder {
    font-weight: bolder;  /* Bolder than parent */
}
```

### Numeric Values

```css
.thin { font-weight: 100; }      /* Thin */
.extra-light { font-weight: 200; } /* Extra Light */
.light { font-weight: 300; }     /* Light */
.normal { font-weight: 400; }    /* Normal/Regular */
.medium { font-weight: 500; }    /* Medium */
.semi-bold { font-weight: 600; } /* Semi Bold */
.bold { font-weight: 700; }      /* Bold */
.extra-bold { font-weight: 800; } /* Extra Bold */
.black { font-weight: 900; }     /* Black/Heavy */
```

**Note:** Not all fonts support all weights. Most fonts support:
- 400 (normal)
- 700 (bold)

---

## Font Style

Controls italic and oblique styles.

```css
.normal {
    font-style: normal;  /* Default, upright */
}

.italic {
    font-style: italic;  /* Italic (designed) */
}

.oblique {
    font-style: oblique;  /* Slanted (computed) */
}
```

**Difference between italic and oblique:**
- **Italic:** Uses designed italic glyphs (preferred)
- **Oblique:** Slants regular glyphs (fallback)

---

## Font Variant

Small caps and other variants.

```css
.normal {
    font-variant: normal;  /* Default */
}

.small-caps {
    font-variant: small-caps;  /* Small capital letters */
}
```

**Example:**
```html
<p style="font-variant: small-caps;">
    This Text Uses Small Caps
</p>
<!-- Renders as: Tʜɪs Tᴇxᴛ Usᴇs Sᴍᴀʟʟ Cᴀᴘs -->
```

---

## Font Shorthand

Combine multiple font properties:

```css
/* Syntax: [style] [variant] [weight] size[/line-height] family */

/* Full shorthand */
.text {
    font: italic small-caps bold 12pt/1.6 Helvetica, sans-serif;
}

/* Minimum required: size and family */
.minimal {
    font: 11pt Arial;
}

/* With line height */
.spaced {
    font: 11pt/1.8 Georgia, serif;
}
```

**Equivalent longhand:**
```css
.text {
    font-style: italic;
    font-variant: small-caps;
    font-weight: bold;
    font-size: 12pt;
    line-height: 1.6;
    font-family: Helvetica, sans-serif;
}
```

---

## Practical Examples

### Example 1: Document Type Scale

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Typography Scale</title>
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

        /* ==============================================
           TYPOGRAPHY HIERARCHY
           ============================================== */
        h1 {
            font-size: 30pt;
            font-weight: bold;
            line-height: 1.2;
            margin-top: 0;
            margin-bottom: 20pt;
            color: #1e40af;
        }

        h2 {
            font-size: 24pt;
            font-weight: bold;
            line-height: 1.3;
            margin-top: 30pt;
            margin-bottom: 15pt;
            color: #2563eb;
        }

        h3 {
            font-size: 18pt;
            font-weight: 600;
            line-height: 1.4;
            margin-top: 20pt;
            margin-bottom: 10pt;
            color: #3b82f6;
        }

        h4 {
            font-size: 14pt;
            font-weight: 600;
            line-height: 1.4;
            margin-top: 15pt;
            margin-bottom: 8pt;
        }

        p {
            margin-top: 0;
            margin-bottom: 12pt;
        }

        .lead {
            font-size: 14pt;
            font-weight: 300;
            line-height: 1.7;
            color: #666;
        }

        .small {
            font-size: 9pt;
            color: #666;
        }
    </style>
</head>
<body>
    <h1>Typography Hierarchy Example</h1>

    <p class="lead">
        This document demonstrates a well-structured typography hierarchy using consistent font sizing and weights.
    </p>

    <h2>Second Level Heading</h2>
    <p>Regular body text at 11pt provides comfortable reading. The font family uses Helvetica with Arial as a fallback.</p>

    <h3>Third Level Heading</h3>
    <p>Each heading level is visually distinct through size, weight, and spacing. This creates a clear information hierarchy.</p>

    <h4>Fourth Level Heading</h4>
    <p>Even smaller headings maintain readability while showing subordinate importance.</p>

    <p class="small">
        Small text is used for footnotes, captions, and secondary information. Font size: 9pt.
    </p>
</body>
</html>
```

### Example 2: Multi-Font Document

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Mixed Fonts</title>
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

        /* ==============================================
           HEADINGS - SANS-SERIF
           ============================================== */
        h1, h2, h3 {
            font-family: Helvetica, Arial, sans-serif;
            font-weight: bold;
            line-height: 1.2;
        }

        h1 {
            font-size: 28pt;
            margin-top: 0;
            margin-bottom: 20pt;
            color: #1e40af;
        }

        h2 {
            font-size: 20pt;
            margin-top: 25pt;
            margin-bottom: 15pt;
            color: #2563eb;
        }

        /* ==============================================
           BODY TEXT - SERIF
           ============================================== */
        p {
            margin-top: 0;
            margin-bottom: 12pt;
        }

        /* ==============================================
           CODE - MONOSPACE
           ============================================== */
        code, pre {
            font-family: "Courier New", Courier, monospace;
            font-size: 10pt;
            background-color: #f9fafb;
        }

        code {
            padding: 2pt 4pt;
            border-radius: 3pt;
        }

        pre {
            padding: 15pt;
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
            overflow-x: auto;
            margin: 15pt 0;
        }

        /* ==============================================
           SPECIAL TEXT
           ============================================== */
        .caption {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 9pt;
            font-style: italic;
            color: #666;
            margin-top: 5pt;
        }

        blockquote {
            font-style: italic;
            border-left: 4pt solid #2563eb;
            padding-left: 20pt;
            margin: 20pt 0;
            color: #444;
        }
    </style>
</head>
<body>
    <h1>Document with Mixed Fonts</h1>

    <p>This document demonstrates effective use of multiple font families. Body text uses Georgia, a serif font that's highly readable for long-form content.</p>

    <h2>Headings Use Sans-Serif</h2>

    <p>Headings use Helvetica, creating clear visual contrast with the serif body text. This is a classic, professional combination.</p>

    <p>Here's an example of inline code: <code>document.getElementById('example')</code>. Code uses a monospace font for clarity.</p>

    <pre>// Code blocks also use monospace
function example() {
    return "Hello, World!";
}</pre>

    <blockquote>
        Quotations are set in italic to distinguish them from regular body text. This adds emphasis while maintaining readability.
    </blockquote>

    <p class="caption">
        Figure 1: Captions use a smaller sans-serif font in italic style for clear differentiation.
    </p>
</body>
</html>
```

### Example 3: Font Weight Demonstration

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Font Weights</title>
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
            font-size: 24pt;
            margin-top: 0;
            margin-bottom: 30pt;
            text-align: center;
            color: #1e40af;
        }

        .weight-example {
            padding: 15pt;
            margin-bottom: 15pt;
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
        }

        .weight-label {
            font-size: 10pt;
            color: #666;
            margin-bottom: 5pt;
        }

        .weight-text {
            font-size: 16pt;
            margin: 0;
        }

        /* Font weight variations */
        .w-100 { font-weight: 100; }
        .w-200 { font-weight: 200; }
        .w-300 { font-weight: 300; }
        .w-400 { font-weight: 400; }
        .w-500 { font-weight: 500; }
        .w-600 { font-weight: 600; }
        .w-700 { font-weight: 700; }
        .w-800 { font-weight: 800; }
        .w-900 { font-weight: 900; }
    </style>
</head>
<body>
    <h1>Font Weight Variations</h1>

    <div class="weight-example">
        <div class="weight-label">font-weight: 100 (Thin)</div>
        <p class="weight-text w-100">The quick brown fox jumps over the lazy dog</p>
    </div>

    <div class="weight-example">
        <div class="weight-label">font-weight: 300 (Light)</div>
        <p class="weight-text w-300">The quick brown fox jumps over the lazy dog</p>
    </div>

    <div class="weight-example">
        <div class="weight-label">font-weight: 400 (Normal/Regular)</div>
        <p class="weight-text w-400">The quick brown fox jumps over the lazy dog</p>
    </div>

    <div class="weight-example">
        <div class="weight-label">font-weight: 500 (Medium)</div>
        <p class="weight-text w-500">The quick brown fox jumps over the lazy dog</p>
    </div>

    <div class="weight-example">
        <div class="weight-label">font-weight: 600 (Semi Bold)</div>
        <p class="weight-text w-600">The quick brown fox jumps over the lazy dog</p>
    </div>

    <div class="weight-example">
        <div class="weight-label">font-weight: 700 (Bold)</div>
        <p class="weight-text w-700">The quick brown fox jumps over the lazy dog</p>
    </div>

    <div class="weight-example">
        <div class="weight-label">font-weight: 900 (Black)</div>
        <p class="weight-text w-900">The quick brown fox jumps over the lazy dog</p>
    </div>

    <p style="font-size: 9pt; color: #666; margin-top: 30pt; text-align: center;">
        Note: Not all fonts support all weight variations. Helvetica/Arial typically support 400 and 700.
    </p>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Typography Hierarchy

Create a document with:
- Clear heading hierarchy (h1 through h4)
- Appropriate font sizes for each level
- Consistent font weights
- Different font families for headings vs body

### Exercise 2: Multi-Font Layout

Create a document using:
- Serif font for body text
- Sans-serif font for headings
- Monospace font for code examples
- Test readability with real content

### Exercise 3: Font Weight Study

Create a comparison document showing:
- All available font weights (100-900)
- Same text in different weights
- Visual hierarchy using only weight variations

---

## Common Pitfalls

### ❌ Not Providing Fallbacks

```css
body {
    font-family: "Custom Font";  /* No fallback! */
}
```

✅ **Solution:** Always provide fallbacks

```css
body {
    font-family: "Custom Font", Helvetica, Arial, sans-serif;
}
```

### ❌ Inconsistent Font Sizing

```css
h1 { font-size: 23pt; }
h2 { font-size: 17pt; }
h3 { font-size: 13.5pt; }  /* Random sizes */
```

✅ **Solution:** Use a consistent scale

```css
h1 { font-size: 24pt; }  /* 2× base */
h2 { font-size: 18pt; }  /* 1.5× base */
h3 { font-size: 14pt; }  /* ~1.2× base */
```

### ❌ Forgetting to Quote Multi-Word Fonts

```css
font-family: Times New Roman;  /* Error! */
```

✅ **Solution:** Use quotes

```css
font-family: "Times New Roman", Times, serif;
```

### ❌ Using Too Many Fonts

```css
h1 { font-family: "Font A"; }
h2 { font-family: "Font B"; }
p { font-family: "Font C"; }
code { font-family: "Font D"; }  /* Too many! */
```

✅ **Solution:** Limit to 2-3 font families

```css
h1, h2, h3 { font-family: Helvetica, sans-serif; }  /* One for headings */
body { font-family: Georgia, serif; }  /* One for body */
code { font-family: Courier, monospace; }  /* One for code */
```

### ❌ Extreme Font Weights Without Support

```css
.text {
    font-weight: 100;  /* May not be supported */
}
```

✅ **Solution:** Use commonly supported weights

```css
.light {
    font-weight: 300;  /* More widely supported */
}

.bold {
    font-weight: 700;  /* Standard bold */
}
```

---

## Font Selection Guide

### For Body Text

**Serif fonts** (traditional, readable):
- Georgia
- Times New Roman
- Palatino

**Sans-serif fonts** (modern, clean):
- Helvetica
- Arial
- Verdana

**Recommended sizes:**
- Body: 10-12pt
- Optimal: 11pt

### For Headings

**Sans-serif fonts** (strong, clear):
- Helvetica
- Arial
- Verdana

**Serif fonts** (formal, traditional):
- Georgia
- Times New Roman

**Recommended sizes:**
- H1: 24-30pt
- H2: 18-24pt
- H3: 14-18pt

### For Code

**Monospace fonts**:
- Courier New
- Monaco
- Consolas

**Recommended size:**
- 9-11pt (slightly smaller than body)

---

## Font Basics Checklist

- [ ] Font family with fallbacks specified
- [ ] Generic family at end of fallback chain
- [ ] Multi-word font names in quotes
- [ ] Appropriate font sizes (10-12pt body, 18-30pt headings)
- [ ] Font weights supported by chosen fonts
- [ ] Consistent typography hierarchy
- [ ] Maximum 2-3 font families used
- [ ] Monospace for code examples

---

## Best Practices

1. **Always provide fallbacks** - Don't rely on single font
2. **Use points for PDF** - More precise than pixels
3. **Limit font families** - 2-3 maximum for cohesion
4. **Establish hierarchy** - Clear visual distinction between levels
5. **Use standard weights** - 400 and 700 are universally supported
6. **Quote multi-word names** - "Times New Roman"
7. **Test font availability** - Ensure fallbacks work
8. **Maintain consistency** - Same fonts for same purposes throughout

---

## Next Steps

Now that you understand font basics:

1. **[Custom Fonts](02_custom_fonts.md)** - Load and embed your own fonts
2. **[Web Fonts](03_web_fonts.md)** - Use Google Fonts and other services
3. **[Text Spacing](05_text_spacing.md)** - Line height, letter spacing

---

**Continue learning →** [Custom Fonts](02_custom_fonts.md)
