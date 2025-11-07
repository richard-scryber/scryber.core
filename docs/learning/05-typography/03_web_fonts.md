---
layout: default
title: Web Fonts & External Sources
nav_order: 3
parent: Typography & Fonts
parent_url: /learning/05-typography/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Web Fonts & External Sources

Understand how to use web fonts like Google Fonts in PDF generation, including limitations and best practices.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Understand web fonts in PDF context
- Use Google Fonts with PDF generation
- Download and host fonts locally
- Handle font loading issues
- Choose between CDN and local fonts
- Optimize font loading for PDF

---

## Web Fonts in PDF Context

### Important Considerations

**Web fonts work differently in PDF generation:**

❌ **What doesn't work:**
- CDN links may not load during PDF generation
- Font downloading during generation may be unreliable
- Network-dependent font loading can fail

✅ **What works:**
- Locally hosted web fonts
- Downloaded font files
- Embedded fonts in your project

### Recommended Approach

**For PDF generation:**
1. Download fonts to your project
2. Host fonts locally
3. Use @font-face with local files
4. Don't rely on external CDNs

---

## Google Fonts

Google Fonts offers thousands of free, open-source fonts.

### The Problem with CDN Links

**❌ This won't work reliably in PDF generation:**

```html
<head>
    <!-- CDN link - unreliable for PDF generation -->
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap" rel="stylesheet">
</head>
```

**Why it fails:**
- PDF generation happens server-side
- No browser to download fonts
- Network access may be restricted
- Timing/async loading issues

### The Solution: Download and Host Locally

**✅ Download fonts and host them:**

1. **Visit Google Fonts** and select your font
2. **Download the font files** (.ttf files)
3. **Add to your project** in a fonts directory
4. **Use @font-face** to load locally

---

## Downloading Google Fonts

### Method 1: Google Fonts Website

1. Go to [fonts.google.com](https://fonts.google.com)
2. Select your font family
3. Click "Download family"
4. Extract .ttf files
5. Add to your project

### Method 2: Google Webfonts Helper

1. Visit [google-webfonts-helper.herokuapp.com](https://google-webfonts-helper.herokuapp.com/fonts)
2. Select font
3. Choose character sets
4. Download .ttf files
5. Copy provided @font-face CSS

### Method 3: Command Line (google-font-download)

```bash
# Install tool
npm install -g google-font-installer

# Download font
google-font-installer 'Roboto:400,700'
```

---

## Setting Up Local Google Fonts

### Directory Structure

```
project/
├── styles/
│   └── fonts.css
├── fonts/
│   ├── Roboto-Regular.ttf
│   ├── Roboto-Bold.ttf
│   ├── OpenSans-Regular.ttf
│   └── OpenSans-Bold.ttf
└── template.html
```

### Loading Fonts

**fonts.css:**
```css
/* ==============================================
   ROBOTO FONT FAMILY
   ============================================== */
@font-face {
    font-family: 'Roboto';
    src: url('../fonts/Roboto-Regular.ttf') format('truetype');
    font-weight: 400;
    font-style: normal;
}

@font-face {
    font-family: 'Roboto';
    src: url('../fonts/Roboto-Bold.ttf') format('truetype');
    font-weight: 700;
    font-style: normal;
}

/* ==============================================
   OPEN SANS FONT FAMILY
   ============================================== */
@font-face {
    font-family: 'Open Sans';
    src: url('../fonts/OpenSans-Regular.ttf') format('truetype');
    font-weight: 400;
    font-style: normal;
}

@font-face {
    font-family: 'Open Sans';
    src: url('../fonts/OpenSans-Bold.ttf') format('truetype');
    font-weight: 700;
    font-style: normal;
}
```

**template.html:**
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <link rel="stylesheet" href="./styles/fonts.css" />
    <style>
        body {
            font-family: 'Roboto', Arial, sans-serif;
        }

        h1, h2, h3 {
            font-family: 'Open Sans', Helvetica, sans-serif;
        }
    </style>
</head>
<body>
    <!-- Content -->
</body>
</html>
```

---

## Practical Examples

### Example 1: Google Fonts (Local Hosting)

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Google Fonts Local</title>
    <style>
        /* ==============================================
           GOOGLE FONTS - LOCALLY HOSTED
           ============================================== */
        /* Roboto - Body font */
        @font-face {
            font-family: 'Roboto';
            src: url('./fonts/Roboto-Regular.ttf') format('truetype');
            font-weight: 400;
            font-style: normal;
        }

        @font-face {
            font-family: 'Roboto';
            src: url('./fonts/Roboto-Italic.ttf') format('truetype');
            font-weight: 400;
            font-style: italic;
        }

        @font-face {
            font-family: 'Roboto';
            src: url('./fonts/Roboto-Bold.ttf') format('truetype');
            font-weight: 700;
            font-style: normal;
        }

        /* Merriweather - Heading font */
        @font-face {
            font-family: 'Merriweather';
            src: url('./fonts/Merriweather-Bold.ttf') format('truetype');
            font-weight: 700;
            font-style: normal;
        }

        /* ==============================================
           PAGE SETUP
           ============================================== */
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'Roboto', Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.7;
            color: #333;
            margin: 0;
        }

        /* ==============================================
           TYPOGRAPHY
           ============================================== */
        h1, h2, h3 {
            font-family: 'Merriweather', Georgia, serif;
            font-weight: 700;
        }

        h1 {
            font-size: 32pt;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 20pt;
        }

        h2 {
            font-size: 22pt;
            color: #2563eb;
            margin-top: 30pt;
            margin-bottom: 15pt;
        }

        h3 {
            font-size: 16pt;
            color: #3b82f6;
            margin-top: 20pt;
            margin-bottom: 10pt;
        }

        p {
            margin-top: 0;
            margin-bottom: 14pt;
        }

        /* ==============================================
           COMPONENTS
           ============================================== */
        .callout {
            border-left: 4pt solid #2563eb;
            background-color: #eff6ff;
            padding: 20pt;
            margin: 25pt 0;
        }

        .callout p {
            margin-bottom: 0;
        }
    </style>
</head>
<body>
    <h1>Using Google Fonts Locally</h1>

    <p>
        This document demonstrates using Google Fonts that have been downloaded and hosted
        locally. The body text uses <strong>Roboto</strong>, while headings use
        <strong>Merriweather</strong>.
    </p>

    <h2>Why Local Hosting?</h2>

    <p>
        For PDF generation, local hosting ensures:
    </p>

    <ul>
        <li>Reliable font loading without network dependencies</li>
        <li>Faster PDF generation</li>
        <li>Consistent results across environments</li>
        <li>No external service dependencies</li>
    </ul>

    <div class="callout">
        <p>
            <strong>Pro Tip:</strong> Download fonts from Google Fonts and add them to your
            project's fonts directory. Use @font-face to load them just like custom fonts.
        </p>
    </div>

    <h2>Implementation Steps</h2>

    <p>
        To use Google Fonts in your PDF documents:
    </p>

    <ol>
        <li>Visit <em>fonts.google.com</em> and select your fonts</li>
        <li>Download the font family (TTF files)</li>
        <li>Add files to your project's fonts directory</li>
        <li>Create @font-face declarations for each variant</li>
        <li>Use the fonts in your CSS with fallbacks</li>
    </ol>

    <h3>Font Variants Loaded</h3>

    <p style="font-weight: 400; font-style: normal;">
        Regular text using Roboto 400 normal
    </p>

    <p style="font-weight: 400; font-style: italic;">
        Italic text using Roboto 400 italic
    </p>

    <p style="font-weight: 700; font-style: normal;">
        Bold text using Roboto 700 normal
    </p>
</body>
</html>
```

### Example 2: Font Pairing

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Font Pairing</title>
    <style>
        /* ==============================================
           FONT PAIRING - LOCALLY HOSTED GOOGLE FONTS
           ============================================== */
        /* Playfair Display - Elegant serif for headings */
        @font-face {
            font-family: 'Playfair Display';
            src: url('./fonts/PlayfairDisplay-Bold.ttf') format('truetype');
            font-weight: 700;
            font-style: normal;
        }

        /* Source Sans Pro - Clean sans-serif for body */
        @font-face {
            font-family: 'Source Sans Pro';
            src: url('./fonts/SourceSansPro-Regular.ttf') format('truetype');
            font-weight: 400;
            font-style: normal;
        }

        @font-face {
            font-family: 'Source Sans Pro';
            src: url('./fonts/SourceSansPro-Italic.ttf') format('truetype');
            font-weight: 400;
            font-style: italic;
        }

        @font-face {
            font-family: 'Source Sans Pro';
            src: url('./fonts/SourceSansPro-Bold.ttf') format('truetype');
            font-weight: 700;
            font-style: normal;
        }

        /* ==============================================
           PAGE SETUP
           ============================================== */
        @page {
            size: Letter;
            margin: 1.25in 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'Source Sans Pro', Arial, sans-serif;
            font-size: 12pt;
            line-height: 1.7;
            color: #2c3e50;
            margin: 0;
        }

        /* ==============================================
           ELEGANT HEADINGS
           ============================================== */
        h1, h2, h3 {
            font-family: 'Playfair Display', Georgia, serif;
            font-weight: 700;
            line-height: 1.2;
        }

        h1 {
            font-size: 42pt;
            color: #1a202c;
            margin-top: 0;
            margin-bottom: 15pt;
            letter-spacing: -0.5pt;
        }

        .subtitle {
            font-family: 'Source Sans Pro', sans-serif;
            font-size: 16pt;
            font-weight: 400;
            font-style: italic;
            color: #718096;
            margin-top: -10pt;
            margin-bottom: 30pt;
        }

        h2 {
            font-size: 28pt;
            color: #2d3748;
            margin-top: 35pt;
            margin-bottom: 15pt;
            border-bottom: 1pt solid #e2e8f0;
            padding-bottom: 10pt;
        }

        h3 {
            font-size: 20pt;
            color: #4a5568;
            margin-top: 25pt;
            margin-bottom: 12pt;
        }

        /* ==============================================
           BODY TEXT
           ============================================== */
        p {
            margin-top: 0;
            margin-bottom: 16pt;
        }

        .lead {
            font-size: 14pt;
            line-height: 1.8;
            color: #4a5568;
            margin-bottom: 25pt;
        }

        /* ==============================================
           PULLQUOTE
           ============================================== */
        .pullquote {
            font-family: 'Playfair Display', Georgia, serif;
            font-size: 18pt;
            font-style: italic;
            line-height: 1.5;
            color: #2563eb;
            border-left: 4pt solid #2563eb;
            padding-left: 25pt;
            margin: 30pt 0;
        }

        .pullquote-attribution {
            font-family: 'Source Sans Pro', sans-serif;
            font-size: 12pt;
            font-style: normal;
            font-weight: 700;
            color: #4a5568;
            margin-top: 10pt;
        }
    </style>
</head>
<body>
    <h1>The Art of Typography</h1>
    <p class="subtitle">A study in elegant font pairing</p>

    <p class="lead">
        Great typography is the foundation of excellent design. The careful pairing of
        serif and sans-serif fonts creates visual harmony while establishing clear hierarchy.
    </p>

    <h2>The Power of Contrast</h2>

    <p>
        This document demonstrates the classic pairing of Playfair Display (an elegant
        serif font) for headings with Source Sans Pro (a clean sans-serif) for body text.
        This combination provides strong visual contrast while maintaining professional cohesion.
    </p>

    <p>
        Playfair Display brings a sense of <strong>elegance and tradition</strong> to headings,
        while Source Sans Pro ensures <em>excellent readability</em> for extended reading.
    </p>

    <div class="pullquote">
        "Typography is the craft of endowing human language with a durable visual form."
        <div class="pullquote-attribution">— Robert Bringhurst</div>
    </div>

    <h3>Historical Context</h3>

    <p>
        The pairing of serif and sans-serif fonts has its roots in traditional print design,
        where serif fonts dominated body text for centuries due to their superior readability
        in print.
    </p>

    <h3>Modern Applications</h3>

    <p>
        Today, we have the flexibility to use sans-serif fonts for body text, especially in
        digital contexts. The key is maintaining sufficient contrast and hierarchy between
        heading and body fonts.
    </p>

    <h2>Best Practices</h2>

    <p>
        When pairing fonts, consider these guidelines:
    </p>

    <ul>
        <li><strong>Contrast:</strong> Choose fonts that are visibly different</li>
        <li><strong>Harmony:</strong> Ensure fonts complement rather than clash</li>
        <li><strong>Hierarchy:</strong> Create clear visual distinction between levels</li>
        <li><strong>Restraint:</strong> Limit your document to 2-3 font families</li>
    </ul>

    <p>
        With thoughtful font pairing, your documents will achieve both beauty and functionality,
        guiding readers effortlessly through your content.
    </p>
</body>
</html>
```

### Example 3: Multiple Google Fonts

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Multiple Font Families</title>
    <style>
        /* ==============================================
           MULTIPLE GOOGLE FONTS - LOCAL HOSTING
           ============================================== */
        /* Montserrat - Headings */
        @font-face {
            font-family: 'Montserrat';
            src: url('./fonts/Montserrat-Bold.ttf') format('truetype');
            font-weight: 700;
            font-style: normal;
        }

        /* Lato - Body text */
        @font-face {
            font-family: 'Lato';
            src: url('./fonts/Lato-Regular.ttf') format('truetype');
            font-weight: 400;
            font-style: normal;
        }

        @font-face {
            font-family: 'Lato';
            src: url('./fonts/Lato-Bold.ttf') format('truetype');
            font-weight: 700;
            font-style: normal;
        }

        /* Inconsolata - Code */
        @font-face {
            font-family: 'Inconsolata';
            src: url('./fonts/Inconsolata-Regular.ttf') format('truetype');
            font-weight: 400;
            font-style: normal;
        }

        /* ==============================================
           BASE STYLES
           ============================================== */
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'Lato', Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        /* Headings */
        h1, h2, h3 {
            font-family: 'Montserrat', Helvetica, sans-serif;
            font-weight: 700;
        }

        h1 {
            font-size: 28pt;
            margin: 0 0 20pt 0;
            color: #1e40af;
        }

        h2 {
            font-size: 20pt;
            margin: 25pt 0 15pt 0;
            color: #2563eb;
        }

        /* Code */
        code, pre {
            font-family: 'Inconsolata', 'Courier New', monospace;
            font-size: 10pt;
        }

        code {
            background-color: #f9fafb;
            padding: 2pt 5pt;
            border-radius: 3pt;
        }

        pre {
            background-color: #f9fafb;
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
            padding: 15pt;
            overflow-x: auto;
            margin: 15pt 0;
        }

        /* Body text */
        p {
            margin: 0 0 12pt 0;
        }
    </style>
</head>
<body>
    <h1>Technical Documentation</h1>

    <p>
        This document uses three Google Fonts for different purposes:
        <strong>Montserrat</strong> for headings, <strong>Lato</strong> for body text,
        and <strong>Inconsolata</strong> for code examples.
    </p>

    <h2>Font Purposes</h2>

    <p>
        Each font serves a specific role:
    </p>

    <ul>
        <li><strong>Montserrat:</strong> Bold, geometric sans-serif for headings</li>
        <li><strong>Lato:</strong> Friendly, readable sans-serif for body text</li>
        <li><strong>Inconsolata:</strong> Monospace font optimized for code</li>
    </ul>

    <h2>Code Example</h2>

    <p>Here's an example with inline code: <code>const fontSize = '11pt';</code></p>

    <pre>// Function with Inconsolata font
function generatePDF(template, data) {
    const doc = Document.ParseHTML(template);
    doc.Params["model"] = data;
    return doc.SaveAsPDF();
}</pre>

    <p>
        The monospace font makes code easy to read and distinguish from regular text,
        while the sans-serif fonts provide modern, clean styling for documentation.
    </p>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Download and Host Google Font

1. Choose a Google Font
2. Download the font files
3. Add to your project
4. Create @font-face declarations
5. Use in a document

### Exercise 2: Font Pairing

Create a document with:
- Serif font for headings (e.g., Merriweather)
- Sans-serif for body (e.g., Open Sans)
- Test readability and contrast

### Exercise 3: Complete Font System

Set up a complete typography system:
- Heading font (bold weights)
- Body font (regular, italic, bold)
- Code font (monospace)
- All locally hosted

---

## Common Pitfalls

### ❌ Relying on CDN Links

```html
<!-- Won't work reliably in PDF generation -->
<link href="https://fonts.googleapis.com/css2?family=Roboto" rel="stylesheet">
```

✅ **Solution:** Download and host locally

```css
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Regular.ttf') format('truetype');
}
```

### ❌ Not Loading All Needed Weights

```css
/* Only loading regular */
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Regular.ttf');
}

/* Bold text won't display correctly */
```

✅ **Solution:** Load all variants

```css
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Regular.ttf');
    font-weight: 400;
}

@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Bold.ttf');
    font-weight: 700;
}
```

### ❌ Inconsistent Font Names

```css
@font-face {
    font-family: 'Roboto Regular';  /* Wrong */
    src: url('./fonts/Roboto-Regular.ttf');
}

@font-face {
    font-family: 'Roboto Bold';  /* Wrong - different name */
    src: url('./fonts/Roboto-Bold.ttf');
}
```

✅ **Solution:** Same family, different weights

```css
@font-face {
    font-family: 'Roboto';  /* Same name */
    src: url('./fonts/Roboto-Regular.ttf');
    font-weight: 400;
}

@font-face {
    font-family: 'Roboto';  /* Same name */
    src: url('./fonts/Roboto-Bold.ttf');
    font-weight: 700;
}
```

### ❌ Using Too Many Fonts

```css
/* 5 different font families - too many! */
h1 { font-family: 'Font A'; }
h2 { font-family: 'Font B'; }
p { font-family: 'Font C'; }
code { font-family: 'Font D'; }
.special { font-family: 'Font E'; }
```

✅ **Solution:** Limit to 2-3 families

```css
h1, h2, h3 { font-family: 'Montserrat', sans-serif; }
body { font-family: 'Lato', sans-serif; }
code { font-family: 'Inconsolata', monospace; }
```

---

## Google Fonts Best Practices

1. **Download, don't link** - Local hosting for PDF
2. **Select needed weights** - Don't download all weights
3. **Use TTF format** - Most reliable for PDF
4. **Provide fallbacks** - System fonts as backup
5. **Limit font families** - 2-3 maximum
6. **Test thoroughly** - Generate PDFs to verify
7. **Check licenses** - Most Google Fonts are SIL OFL (free)
8. **Optimize file size** - Only include needed characters if possible

---

## Font Sources

### Free & Open Source

**Google Fonts**
- fonts.google.com
- 1000+ free fonts
- SIL OFL license (free for commercial use)

**Font Squirrel**
- fontsquirrel.com
- Curated free fonts
- 100% free for commercial use filter

**Adobe Fonts** (Free Tier)
- fonts.adobe.com
- Some fonts free with Creative Cloud

### Commercial Sources

**MyFonts**
- myfonts.com
- Large commercial library
- Various licenses

**Fonts.com**
- fonts.com
- Professional fonts
- Desktop and web licenses

---

## Web Fonts Checklist

- [ ] Fonts downloaded from source
- [ ] Files added to project directory
- [ ] @font-face declarations created
- [ ] All needed weights/styles loaded
- [ ] Fallback fonts specified
- [ ] Local paths (not CDN links)
- [ ] License verified (free or purchased)
- [ ] Tested in generated PDF

---

## Best Practices Summary

1. **Local hosting only** - Don't use CDN links for PDF
2. **Download from Google Fonts** - Use google-webfonts-helper
3. **Use TTF format** - Most reliable for PDF generation
4. **Load all variants** - Regular, bold, italic, bold-italic
5. **Provide fallbacks** - Always include system fonts
6. **Limit font count** - Maximum 2-3 font families
7. **Test thoroughly** - Generate PDFs with fonts
8. **Respect licenses** - Verify embedding permissions

---

## Next Steps

Now that you understand web fonts:

1. **[Font Styling](04_font_styling.md)** - Text transforms, decorations
2. **[Text Spacing](05_text_spacing.md)** - Line height, letter spacing
3. **[Typography Best Practices](08_typography_best_practices.md)** - Professional patterns

---

**Continue learning →** [Font Styling](04_font_styling.md)
