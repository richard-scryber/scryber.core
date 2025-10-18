---
layout: default
title: Text Spacing
nav_order: 5
parent: Typography & Fonts
parent_url: /learning/05-typography/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Text Spacing

Master line height, letter spacing, and word spacing for optimal readability in PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Control line height for readability
- Apply letter spacing effectively
- Use word spacing when appropriate
- Create vertical rhythm with spacing
- Optimize spacing for different font sizes
- Understand spacing units and values

---

## Line Height

Controls vertical spacing between lines of text.

### Values and Units

```css
/* Unitless (recommended) - relative to font size */
p {
    line-height: 1.6;  /* 1.6 × font-size */
}

/* Fixed units */
p {
    line-height: 18pt;  /* Fixed 18pt */
}

/* Percentage */
p {
    line-height: 160%;  /* 160% of font-size */
}

/* Normal (browser default, ~1.2) */
p {
    line-height: normal;
}
```

### Recommended Line Heights

```css
/* Body text - comfortable reading */
body {
    font-size: 11pt;
    line-height: 1.6;  /* 17.6pt */
}

/* Headings - tighter spacing */
h1 {
    font-size: 28pt;
    line-height: 1.2;  /* 33.6pt */
}

h2 {
    font-size: 20pt;
    line-height: 1.3;  /* 26pt */
}

/* Small text - more spacing */
.small {
    font-size: 9pt;
    line-height: 1.7;  /* 15.3pt */
}

/* Large display text */
.display {
    font-size: 36pt;
    line-height: 1.1;  /* 39.6pt */
}
```

### Line Height Guidelines

| Text Type | Font Size | Recommended line-height |
|-----------|-----------|------------------------|
| Body text | 10-12pt | 1.5-1.7 |
| Headings | 18-30pt | 1.1-1.3 |
| Small print | 8-9pt | 1.6-1.8 |
| Display | 36pt+ | 1.0-1.2 |

---

## Letter Spacing

Controls horizontal space between characters.

### Basic Syntax

```css
/* letter-spacing: value */
.spaced {
    letter-spacing: 1pt;  /* Add 1pt between characters */
}

.tight {
    letter-spacing: -0.5pt;  /* Remove 0.5pt */
}

.normal {
    letter-spacing: normal;  /* Default (0) */
}
```

### Common Applications

```css
/* Uppercase text needs spacing */
.uppercase-header {
    text-transform: uppercase;
    letter-spacing: 2pt;  /* Improves readability */
}

/* Tight spacing for large headings */
h1 {
    font-size: 36pt;
    letter-spacing: -0.5pt;  /* Tighten slightly */
}

/* Spaced small caps */
.small-caps {
    font-variant: small-caps;
    letter-spacing: 1pt;
}

/* Tracking for emphasis */
.emphasis {
    letter-spacing: 3pt;
    text-transform: uppercase;
}
```

### Letter Spacing Guidelines

```css
/* Body text - usually normal */
p {
    letter-spacing: normal;  /* 0pt */
}

/* Uppercase - add spacing */
.uppercase {
    text-transform: uppercase;
    letter-spacing: 1-2pt;
}

/* Large headings - slight tightening */
h1 {
    letter-spacing: -0.5pt to 0pt;
}

/* Small text - slight spacing */
.small {
    font-size: 9pt;
    letter-spacing: 0.2pt;
}
```

---

## Word Spacing

Controls horizontal space between words.

### Basic Syntax

```css
/* word-spacing: value */
.loose {
    word-spacing: 3pt;  /* Add 3pt between words */
}

.tight {
    word-spacing: -1pt;  /* Reduce spacing */
}

.normal {
    word-spacing: normal;  /* Default */
}
```

**Note:** Word spacing is rarely needed. Line-height and letter-spacing are more commonly used.

### When to Use

```css
/* Justified text might need adjustment */
.justified {
    text-align: justify;
    word-spacing: normal;  /* Let browser adjust */
}

/* Headers with specific spacing needs */
.spaced-header {
    word-spacing: 5pt;
    letter-spacing: 1pt;
}
```

---

## Vertical Rhythm

Create consistent vertical spacing throughout the document.

### Establishing a Baseline

```css
/* ==============================================
   BASE RHYTHM - 6pt baseline
   ============================================== */
body {
    font-size: 12pt;
    line-height: 1.5;  /* 18pt = 3 × 6pt baseline */
}

h1 {
    font-size: 30pt;
    line-height: 1.2;  /* 36pt = 6 × 6pt baseline */
    margin-bottom: 18pt;  /* 3 × 6pt */
}

h2 {
    font-size: 24pt;
    line-height: 1.25;  /* 30pt = 5 × 6pt baseline */
    margin-top: 24pt;  /* 4 × 6pt */
    margin-bottom: 12pt;  /* 2 × 6pt */
}

p {
    margin-bottom: 12pt;  /* 2 × 6pt */
}
```

---

## Practical Examples

### Example 1: Optimal Body Text Spacing

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Optimal Spacing</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Georgia, "Times New Roman", serif;
            font-size: 11pt;
            line-height: 1.7;  /* Comfortable reading */
            color: #333;
            margin: 0;
        }

        h1 {
            font-family: Helvetica, sans-serif;
            font-size: 28pt;
            line-height: 1.2;
            letter-spacing: -0.5pt;
            margin: 0 0 20pt 0;
            color: #1e40af;
        }

        h2 {
            font-family: Helvetica, sans-serif;
            font-size: 20pt;
            line-height: 1.3;
            margin: 30pt 0 15pt 0;
            color: #2563eb;
        }

        p {
            margin: 0 0 14pt 0;
        }

        .intro {
            font-size: 13pt;
            line-height: 1.8;
            margin-bottom: 20pt;
        }
    </style>
</head>
<body>
    <h1>The Importance of Spacing</h1>

    <p class="intro">
        Proper spacing is fundamental to readable typography. This document demonstrates optimal spacing values for comfortable reading in PDF documents.
    </p>

    <h2>Line Height</h2>

    <p>
        The line height (also called leading) controls vertical space between lines. For body text at 11pt, a line-height of 1.7 provides comfortable reading without lines appearing disconnected or cramped.
    </p>

    <p>
        Research shows that line heights between 1.5 and 1.8 work best for body text. Too tight, and text becomes difficult to track. Too loose, and the connection between lines is lost.
    </p>

    <h2>Letter Spacing</h2>

    <p>
        Letter spacing (tracking) affects horizontal space between characters. For most body text, the default spacing is optimal. Adjustments are primarily needed for uppercase text and large headings.
    </p>
</body>
</html>
```

### Example 2: Uppercase with Proper Spacing

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Uppercase Spacing</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        .example-box {
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
            padding: 20pt;
            margin-bottom: 20pt;
        }

        .example-label {
            font-size: 9pt;
            color: #666;
            margin-bottom: 10pt;
        }

        /* ==============================================
           SPACING VARIATIONS
           ============================================== */
        .no-spacing {
            text-transform: uppercase;
            letter-spacing: normal;
            font-size: 18pt;
        }

        .small-spacing {
            text-transform: uppercase;
            letter-spacing: 1pt;
            font-size: 18pt;
        }

        .optimal-spacing {
            text-transform: uppercase;
            letter-spacing: 2pt;
            font-size: 18pt;
        }

        .excessive-spacing {
            text-transform: uppercase;
            letter-spacing: 5pt;
            font-size: 18pt;
        }
    </style>
</head>
<body>
    <h1 style="font-size: 28pt; margin: 0 0 30pt 0;">Uppercase Letter Spacing</h1>

    <div class="example-box">
        <div class="example-label">No letter spacing (letter-spacing: normal)</div>
        <p class="no-spacing">Important Announcement</p>
    </div>

    <div class="example-box">
        <div class="example-label">Small spacing (letter-spacing: 1pt)</div>
        <p class="small-spacing">Important Announcement</p>
    </div>

    <div class="example-box" style="background-color: #eff6ff;">
        <div class="example-label">✓ Optimal spacing (letter-spacing: 2pt)</div>
        <p class="optimal-spacing">Important Announcement</p>
    </div>

    <div class="example-box">
        <div class="example-label">Excessive spacing (letter-spacing: 5pt)</div>
        <p class="excessive-spacing">Important Announcement</p>
    </div>

    <p style="margin-top: 30pt; font-size: 10pt; color: #666;">
        <strong>Recommendation:</strong> For uppercase text, add 1-2pt letter spacing for improved readability.
    </p>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Line Height Comparison

Create a document comparing:
- Line-height: 1.2 (tight)
- Line-height: 1.6 (optimal)
- Line-height: 2.0 (loose)

### Exercise 2: Letter Spacing Practice

Create examples showing:
- Uppercase without spacing
- Uppercase with proper spacing
- Large headings with tight spacing

### Exercise 3: Vertical Rhythm

Build a document with:
- Consistent baseline grid (6pt)
- All spacing multiples of baseline
- Visual harmony throughout

---

## Common Pitfalls

### ❌ Too-Tight Line Height

```css
p {
    line-height: 1.1;  /* Too tight! */
}
```

✅ **Solution:**

```css
p {
    line-height: 1.6;  /* Comfortable */
}
```

### ❌ Uppercase Without Letter Spacing

```css
.header {
    text-transform: uppercase;
    /* Missing letter-spacing */
}
```

✅ **Solution:**

```css
.header {
    text-transform: uppercase;
    letter-spacing: 2pt;
}
```

### ❌ Inconsistent Spacing

```css
h1 { margin-bottom: 17pt; }
h2 { margin-bottom: 23pt; }
p { margin-bottom: 11pt; }
```

✅ **Solution:**

```css
/* Use consistent scale */
h1 { margin-bottom: 20pt; }
h2 { margin-bottom: 15pt; }
p { margin-bottom: 12pt; }
```

---

## Text Spacing Checklist

- [ ] Body text line-height: 1.5-1.7
- [ ] Heading line-height: 1.1-1.3
- [ ] Uppercase text has letter-spacing: 1-2pt
- [ ] Large headings slightly tightened
- [ ] Consistent vertical rhythm
- [ ] Spacing enhances readability
- [ ] Tested with real content

---

## Best Practices

1. **Unitless line-height** - Scales with font-size
2. **Body text: 1.5-1.7** - Optimal readability
3. **Headings: 1.1-1.3** - Tighter for impact
4. **Uppercase needs spacing** - Add 1-2pt
5. **Consistent vertical rhythm** - Baseline grid
6. **Test with real content** - Varies by font
7. **Less is more** - Avoid excessive spacing

---

## Next Steps

1. **[Text Alignment](06_text_alignment.md)** - Alignment and justification
2. **[Advanced Typography](07_advanced_typography.md)** - Drop caps, special features
3. **[Typography Best Practices](08_typography_best_practices.md)** - Professional patterns

---

**Continue learning →** [Text Alignment](06_text_alignment.md)
