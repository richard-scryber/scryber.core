---
layout: default
title: Custom Fonts
nav_order: 2
parent: Typography & Fonts
parent_url: /learning/05-typography/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Custom Fonts

Learn to load and use custom fonts in your PDF documents for unique, branded typography.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Load custom fonts with @font-face
- Understand font file formats (TTF, OTF, WOFF)
- Embed fonts in PDF documents
- Handle font licensing correctly
- Troubleshoot font loading issues
- Optimize font file sizes

---

## Why Custom Fonts?

Custom fonts allow you to:
- **Brand consistency** - Match your organization's visual identity
- **Unique typography** - Stand out with distinctive fonts
- **Professional appearance** - High-quality, design-specific fonts
- **Multilingual support** - Fonts with extended character sets

---

## @font-face Rule

The @font-face rule loads custom fonts into your document.

### Basic Syntax

```css
@font-face {
    font-family: 'Custom Font Name';
    src: url('./fonts/CustomFont-Regular.ttf');
}

/* Use the font */
body {
    font-family: 'Custom Font Name', Arial, sans-serif;
}
```

### Complete Syntax

```css
@font-face {
    font-family: 'Roboto';  /* Name to reference */
    src: url('./fonts/Roboto-Regular.ttf') format('truetype');
    font-weight: 400;  /* Normal weight */
    font-style: normal;  /* Upright style */
    font-display: swap;  /* How to display during load */
}
```

---

## Font File Formats

### TrueType (.ttf)

**Best for PDF generation**

```css
@font-face {
    font-family: 'MyFont';
    src: url('./fonts/MyFont-Regular.ttf') format('truetype');
}
```

**Characteristics:**
- ✅ Widely supported
- ✅ Works in all PDF generators
- ✅ Good quality
- ⚠️ Larger file size

### OpenType (.otf)

**Modern, feature-rich format**

```css
@font-face {
    font-family: 'MyFont';
    src: url('./fonts/MyFont-Regular.otf') format('opentype');
}
```

**Characteristics:**
- ✅ Advanced typography features
- ✅ Cross-platform
- ✅ High quality
- ⚠️ May have compatibility issues

### WOFF/WOFF2 (.woff, .woff2)

**Web-optimized formats**

```css
@font-face {
    font-family: 'MyFont';
    src: url('./fonts/MyFont-Regular.woff2') format('woff2'),
         url('./fonts/MyFont-Regular.woff') format('woff'),
         url('./fonts/MyFont-Regular.ttf') format('truetype');
}
```

**Characteristics:**
- ✅ Compressed (smaller)
- ✅ Fast loading
- ⚠️ May not work in all PDF generators
- **Note:** Check Scryber documentation for WOFF support

---

## Loading Font Variants

### Multiple Weights

```css
/* Regular weight */
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Regular.ttf') format('truetype');
    font-weight: 400;
    font-style: normal;
}

/* Bold weight */
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Bold.ttf') format('truetype');
    font-weight: 700;
    font-style: normal;
}

/* Light weight */
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Light.ttf') format('truetype');
    font-weight: 300;
    font-style: normal;
}
```

### Italic Styles

```css
/* Regular italic */
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Italic.ttf') format('truetype');
    font-weight: 400;
    font-style: italic;
}

/* Bold italic */
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-BoldItalic.ttf') format('truetype');
    font-weight: 700;
    font-style: italic;
}
```

### Using Variants

```css
/* After loading variants above */
body {
    font-family: 'Roboto', sans-serif;
    font-weight: 400;  /* Uses Roboto-Regular.ttf */
}

strong {
    font-weight: 700;  /* Uses Roboto-Bold.ttf */
}

em {
    font-style: italic;  /* Uses Roboto-Italic.ttf */
}
```

---

## File Paths

### Relative Paths

```css
/* Relative to CSS file */
@font-face {
    font-family: 'MyFont';
    src: url('./fonts/MyFont-Regular.ttf');
}

/* Up one directory */
@font-face {
    font-family: 'MyFont';
    src: url('../fonts/MyFont-Regular.ttf');
}
```

### Absolute Paths

```css
/* From project root */
@font-face {
    font-family: 'MyFont';
    src: url('/assets/fonts/MyFont-Regular.ttf');
}
```

### Multiple Sources

```css
/* Fallback order: WOFF2 → WOFF → TTF */
@font-face {
    font-family: 'MyFont';
    src: url('./fonts/MyFont-Regular.woff2') format('woff2'),
         url('./fonts/MyFont-Regular.woff') format('woff'),
         url('./fonts/MyFont-Regular.ttf') format('truetype');
}
```

---

## Font Embedding in PDF

### How It Works

When generating a PDF:
1. Font file is read from specified path
2. Font data is embedded in PDF
3. PDF is self-contained with fonts

### Subsetting

Some PDF generators subset fonts (include only used characters):

**Benefits:**
- ✅ Smaller file size
- ✅ Faster generation
- ✅ Lower memory usage

**Considerations:**
- Character availability limited to what's embedded
- May affect text extraction/searching

---

## Practical Examples

### Example 1: Complete Font Family

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Custom Font Family</title>
    <style>
        /* ==============================================
           FONT LOADING
           ============================================== */
        /* Regular */
        @font-face {
            font-family: 'Open Sans';
            src: url('./fonts/OpenSans-Regular.ttf') format('truetype');
            font-weight: 400;
            font-style: normal;
        }

        /* Italic */
        @font-face {
            font-family: 'Open Sans';
            src: url('./fonts/OpenSans-Italic.ttf') format('truetype');
            font-weight: 400;
            font-style: italic;
        }

        /* Bold */
        @font-face {
            font-family: 'Open Sans';
            src: url('./fonts/OpenSans-Bold.ttf') format('truetype');
            font-weight: 700;
            font-style: normal;
        }

        /* Bold Italic */
        @font-face {
            font-family: 'Open Sans';
            src: url('./fonts/OpenSans-BoldItalic.ttf') format('truetype');
            font-weight: 700;
            font-style: italic;
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
            font-family: 'Open Sans', Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        h1 {
            font-size: 28pt;
            font-weight: 700;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 20pt;
        }

        h2 {
            font-size: 20pt;
            font-weight: 700;
            color: #2563eb;
            margin-top: 25pt;
            margin-bottom: 15pt;
        }

        p {
            margin-top: 0;
            margin-bottom: 12pt;
        }

        /* ==============================================
           TEXT STYLES
           ============================================== */
        .normal {
            font-weight: 400;
            font-style: normal;
        }

        .italic {
            font-weight: 400;
            font-style: italic;
        }

        .bold {
            font-weight: 700;
            font-style: normal;
        }

        .bold-italic {
            font-weight: 700;
            font-style: italic;
        }
    </style>
</head>
<body>
    <h1>Custom Font Family Demonstration</h1>

    <p>This document uses the Open Sans font family with multiple variants loaded.</p>

    <h2>Font Variants</h2>

    <p class="normal">
        <strong>Regular (400):</strong> This is regular weight text using Open Sans Regular.
    </p>

    <p class="italic">
        <strong style="font-style: normal;">Italic (400):</strong> This is italic text using Open Sans Italic.
    </p>

    <p class="bold">
        <strong>Bold (700):</strong> This is bold text using Open Sans Bold.
    </p>

    <p class="bold-italic">
        <strong>Bold Italic (700):</strong> This is bold italic text using Open Sans Bold Italic.
    </p>

    <h2>In Context</h2>

    <p>
        Regular text can include <strong>bold text</strong> and <em>italic text</em> naturally.
        You can even combine <strong><em>bold and italic</em></strong> together.
    </p>

    <p>
        The custom font family provides a cohesive, professional appearance throughout the document
        while maintaining all necessary text styling capabilities.
    </p>
</body>
</html>
```

### Example 2: Branded Document

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Branded Document</title>
    <style>
        /* ==============================================
           BRAND FONTS
           ============================================== */
        /* Heading font */
        @font-face {
            font-family: 'Montserrat';
            src: url('./fonts/Montserrat-Bold.ttf') format('truetype');
            font-weight: 700;
            font-style: normal;
        }

        /* Body font */
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
           PAGE & BASE STYLES
           ============================================== */
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'Source Sans Pro', Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.7;
            color: #333;
            margin: 0;
        }

        /* ==============================================
           HEADINGS - BRAND FONT
           ============================================== */
        h1, h2, h3 {
            font-family: 'Montserrat', Helvetica, sans-serif;
            font-weight: 700;
            line-height: 1.2;
        }

        h1 {
            font-size: 32pt;
            color: #0f172a;
            margin-top: 0;
            margin-bottom: 20pt;
            letter-spacing: -0.5pt;
        }

        h2 {
            font-size: 22pt;
            color: #1e293b;
            margin-top: 30pt;
            margin-bottom: 15pt;
        }

        h3 {
            font-size: 16pt;
            color: #334155;
            margin-top: 20pt;
            margin-bottom: 10pt;
        }

        /* ==============================================
           BODY TEXT
           ============================================== */
        p {
            margin-top: 0;
            margin-bottom: 14pt;
        }

        /* ==============================================
           COMPONENTS
           ============================================== */
        .brand-box {
            border-left: 4pt solid #2563eb;
            background-color: #eff6ff;
            padding: 20pt;
            margin: 25pt 0;
        }

        .brand-box h3 {
            margin-top: 0;
            color: #1e40af;
        }
    </style>
</head>
<body>
    <h1>Brand Guidelines 2025</h1>

    <p>
        This document demonstrates our corporate typography system using custom brand fonts.
        All headings use Montserrat Bold, while body text uses Source Sans Pro.
    </p>

    <h2>Typography Standards</h2>

    <p>
        Our brand identity relies on consistent, professional typography. These custom fonts
        have been carefully selected to represent our company's values of <strong>clarity</strong>,
        <em>innovation</em>, and <strong><em>excellence</em></strong>.
    </p>

    <div class="brand-box">
        <h3>Brand Font Usage</h3>
        <p>
            Montserrat Bold is reserved for headlines and titles. It provides strong visual
            impact and immediate recognition.
        </p>
        <p style="margin-bottom: 0;">
            Source Sans Pro handles all body text, offering excellent readability and a
            modern, approachable aesthetic.
        </p>
    </div>

    <h2>Implementation Notes</h2>

    <p>
        When creating branded documents, always load both font families to maintain consistency
        across all corporate communications. The combination creates a distinctive visual hierarchy
        that reinforces brand recognition.
    </p>

    <h3>Font Weights Available</h3>

    <ul>
        <li><strong>Montserrat:</strong> Bold (700) for headings</li>
        <li><strong>Source Sans Pro:</strong> Regular (400), Italic (400), Bold (700)</li>
    </ul>
</body>
</html>
```

### Example 3: Fallback Testing

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Font Fallbacks</title>
    <style>
        /* ==============================================
           CUSTOM FONT WITH FALLBACKS
           ============================================== */
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

        /* ==============================================
           STYLES WITH COMPLETE FALLBACK CHAIN
           ============================================== */
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            /* Complete fallback chain */
            font-family: 'Lato', -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            font-weight: 700;
            margin-top: 0;
            margin-bottom: 20pt;
            color: #1e40af;
        }

        .font-stack-demo {
            padding: 15pt;
            margin-bottom: 15pt;
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
        }

        .font-name {
            font-size: 10pt;
            color: #666;
            margin-bottom: 5pt;
        }

        .sample-text {
            font-size: 14pt;
            margin: 0;
        }
    </style>
</head>
<body>
    <h1>Font Fallback Demonstration</h1>

    <p>
        This document demonstrates proper font fallback chains. If Lato isn't available,
        the system will progressively try each fallback font.
    </p>

    <div class="font-stack-demo">
        <div class="font-name">Primary: Lato (Custom Font)</div>
        <p class="sample-text" style="font-family: 'Lato', sans-serif;">
            The quick brown fox jumps over the lazy dog
        </p>
    </div>

    <div class="font-stack-demo">
        <div class="font-name">Fallback 1: System Font (-apple-system)</div>
        <p class="sample-text" style="font-family: -apple-system, sans-serif;">
            The quick brown fox jumps over the lazy dog
        </p>
    </div>

    <div class="font-stack-demo">
        <div class="font-name">Fallback 2: Helvetica</div>
        <p class="sample-text" style="font-family: Helvetica, sans-serif;">
            The quick brown fox jumps over the lazy dog
        </p>
    </div>

    <div class="font-stack-demo">
        <div class="font-name">Fallback 3: Arial</div>
        <p class="sample-text" style="font-family: Arial, sans-serif;">
            The quick brown fox jumps over the lazy dog
        </p>
    </div>

    <div class="font-stack-demo">
        <div class="font-name">Final Fallback: sans-serif (System Default)</div>
        <p class="sample-text" style="font-family: sans-serif;">
            The quick brown fox jumps over the lazy dog
        </p>
    </div>

    <p style="margin-top: 30pt; font-size: 10pt; color: #666;">
        <strong>Note:</strong> The fallback chain ensures your document always displays with an appropriate font,
        even if the custom font fails to load.
    </p>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Load a Custom Font

1. Download a free font from Google Fonts or Font Squirrel
2. Save .ttf file in your project
3. Load it with @font-face
4. Use it in a document

### Exercise 2: Complete Font Family

Load multiple weights of a font:
- Regular (400)
- Bold (700)
- Italic (400)
- Test each variant in your document

### Exercise 3: Branded Typography

Create a branded document with:
- Custom heading font
- Custom body font
- Proper fallback chains
- Consistent styling throughout

---

## Common Pitfalls

### ❌ Wrong File Path

```css
@font-face {
    font-family: 'MyFont';
    src: url('MyFont.ttf');  /* File not found! */
}
```

✅ **Solution:** Use correct relative path

```css
@font-face {
    font-family: 'MyFont';
    src: url('./fonts/MyFont-Regular.ttf');  /* Correct path */
}
```

### ❌ Not Specifying font-weight/font-style

```css
@font-face {
    font-family: 'MyFont';
    src: url('./fonts/MyFont-Bold.ttf');
    /* Missing font-weight: 700 */
}
```

✅ **Solution:** Specify weight and style

```css
@font-face {
    font-family: 'MyFont';
    src: url('./fonts/MyFont-Bold.ttf');
    font-weight: 700;  /* Explicit weight */
    font-style: normal;
}
```

### ❌ Using Different Family Names for Variants

```css
@font-face {
    font-family: 'MyFont';
    src: url('./fonts/MyFont-Regular.ttf');
}

@font-face {
    font-family: 'MyFont Bold';  /* Wrong! Different name */
    src: url('./fonts/MyFont-Bold.ttf');
}
```

✅ **Solution:** Same family name, different weights

```css
@font-face {
    font-family: 'MyFont';  /* Same name */
    src: url('./fonts/MyFont-Regular.ttf');
    font-weight: 400;
}

@font-face {
    font-family: 'MyFont';  /* Same name */
    src: url('./fonts/MyFont-Bold.ttf');
    font-weight: 700;  /* Different weight */
}
```

### ❌ No Fallback Fonts

```css
body {
    font-family: 'Custom Font';  /* No fallback! */
}
```

✅ **Solution:** Always provide fallbacks

```css
body {
    font-family: 'Custom Font', Helvetica, Arial, sans-serif;
}
```

### ❌ Forgetting Font Licensing

```
/* Using commercial font without license */
```

✅ **Solution:** Always check and respect font licenses
- Use open-source fonts (SIL OFL, Apache, etc.)
- Purchase commercial licenses
- Check embedding permissions

---

## Font Licensing

### Open Source Fonts

**Free to use, embed, and distribute:**
- Google Fonts (mostly SIL Open Font License)
- Adobe Fonts (some free options)
- Font Squirrel (filtered for commercial use)

### Commercial Fonts

**Require licenses:**
- Check if PDF embedding is allowed
- Verify commercial use permissions
- Read license terms carefully
- Keep license documentation

### License Types

**SIL Open Font License (OFL):**
- ✅ Free for personal and commercial use
- ✅ Can embed in PDFs
- ✅ Can modify
- ⚠️ Cannot sell the font itself

**Apache License:**
- ✅ Free for any use
- ✅ Can embed
- ✅ Very permissive

**Commercial Licenses:**
- Vary by vendor
- May restrict embedding
- May require per-document fees

---

## Troubleshooting Font Loading

### Font Not Appearing

**Check:**
1. ✅ File path is correct
2. ✅ File exists and is accessible
3. ✅ Font family name matches usage
4. ✅ Font format is supported
5. ✅ Font file isn't corrupted

### Wrong Font Weight/Style

**Check:**
1. ✅ font-weight matches @font-face declaration
2. ✅ font-style matches @font-face declaration
3. ✅ Multiple variants properly declared

### Performance Issues

**Solutions:**
1. ✅ Use font subsetting
2. ✅ Load only needed weights
3. ✅ Use compressed formats (WOFF2)
4. ✅ Cache font files

---

## Custom Fonts Checklist

- [ ] Font files in accessible location
- [ ] @font-face rules properly configured
- [ ] font-weight and font-style specified
- [ ] All needed variants loaded (regular, bold, italic)
- [ ] Fallback fonts provided
- [ ] Font licensing verified
- [ ] File paths tested
- [ ] Font displays correctly in generated PDF

---

## Best Practices

1. **Use TTF/OTF for PDF** - Most reliable formats
2. **Load all variants** - Regular, bold, italic, bold-italic
3. **Specify weight/style** - Explicit @font-face declarations
4. **Provide fallbacks** - System fonts as backup
5. **Check licenses** - Respect font licensing
6. **Use relative paths** - Portable across environments
7. **Test thoroughly** - Generate PDFs with custom fonts
8. **Optimize file sizes** - Use font subsetting if available

---

## Next Steps

Now that you can load custom fonts:

1. **[Web Fonts](03_web_fonts.md)** - Use Google Fonts and CDNs
2. **[Font Styling](04_font_styling.md)** - Advanced text effects
3. **[Text Spacing](05_text_spacing.md)** - Line height, letter spacing

---

**Continue learning →** [Web Fonts](03_web_fonts.md)
