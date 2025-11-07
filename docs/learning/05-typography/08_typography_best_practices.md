---
layout: default
title: Typography Best Practices
nav_order: 8
parent: Typography & Fonts
parent_url: /learning/05-typography/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Typography Best Practices

Master professional typography patterns, optimization techniques, and common gotchas for PDF generation.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Apply professional typography patterns
- Create clear visual hierarchy
- Optimize font loading and performance
- Avoid common typography mistakes
- Build accessible, readable documents
- Follow industry-standard conventions

---

## Core Typography Principles

### 1. Establish Clear Hierarchy

```css
/* ==============================================
   TYPOGRAPHY SCALE - CLEAR HIERARCHY
   ============================================== */
h1 {
    font-size: 28pt;      /* Largest */
    font-weight: 700;
    line-height: 1.2;
    margin-bottom: 20pt;
}

h2 {
    font-size: 20pt;      /* Clearly smaller */
    font-weight: 600;
    line-height: 1.3;
    margin-bottom: 15pt;
}

h3 {
    font-size: 14pt;      /* Further differentiated */
    font-weight: 600;
    line-height: 1.4;
    margin-bottom: 10pt;
}

body {
    font-size: 11pt;      /* Base size */
    line-height: 1.6;
}
```

### 2. Limit Font Families

**❌ Too many fonts:**
```css
h1 { font-family: "Font A"; }
h2 { font-family: "Font B"; }
p { font-family: "Font C"; }
code { font-family: "Font D"; }
.special { font-family: "Font E"; }  /* 5 fonts! */
```

**✅ Limited, purposeful:**
```css
h1, h2, h3 {
    font-family: 'Helvetica', sans-serif;  /* One for headings */
}

body {
    font-family: 'Georgia', serif;  /* One for body */
}

code {
    font-family: 'Courier New', monospace;  /* One for code */
}
```

### 3. Consistent Spacing

```css
/* ==============================================
   SPACING SCALE
   ============================================== */
:root {
    --space-xs: 5pt;
    --space-sm: 10pt;
    --space-md: 20pt;
    --space-lg: 30pt;
    --space-xl: 40pt;
}

h1 { margin-bottom: var(--space-md); }
h2 { margin: var(--space-lg) 0 var(--space-md) 0; }
p { margin-bottom: var(--space-sm); }
```

---

## Professional Typography Patterns

### Pattern 1: Classic Document

```css
/* Serif for body, sans-serif for headings */
body {
    font-family: Georgia, "Times New Roman", serif;
    font-size: 11pt;
    line-height: 1.7;
}

h1, h2, h3 {
    font-family: Helvetica, Arial, sans-serif;
    font-weight: 700;
}
```

### Pattern 2: Modern Document

```css
/* Sans-serif throughout */
body {
    font-family: 'Open Sans', Helvetica, Arial, sans-serif;
    font-size: 11pt;
    line-height: 1.6;
}

h1, h2, h3 {
    font-family: 'Montserrat', Helvetica, sans-serif;
    font-weight: 700;
}
```

### Pattern 3: Technical Document

```css
/* Sans-serif with monospace code */
body {
    font-family: Helvetica, Arial, sans-serif;
    font-size: 11pt;
    line-height: 1.6;
}

h1, h2, h3 {
    font-weight: 600;
}

code, pre {
    font-family: 'Courier New', Courier, monospace;
    font-size: 10pt;
}
```

---

## Readability Guidelines

### Body Text

```css
/* Optimal readability */
p {
    font-size: 10-12pt;        /* Comfortable size */
    line-height: 1.5-1.7;      /* Not too tight */
    max-width: 600pt;          /* Line length limit */
    text-align: left;          /* Or justify */
    margin-bottom: 12pt;       /* Clear paragraphs */
}
```

### Line Length

```css
/* Optimal: 50-75 characters per line */
.content {
    max-width: 600pt;  /* ~75 characters at 11pt */
}

/* Too wide */
.wide {
    max-width: 1000pt;  /* Hard to track lines */
}

/* Too narrow */
.narrow {
    max-width: 200pt;  /* Excessive breaks */
}
```

### Contrast

```css
/* Sufficient contrast for readability */
body {
    color: #333;  /* Dark gray, not pure black */
    background-color: white;
}

/* Too low contrast */
.low-contrast {
    color: #999;  /* Hard to read */
}

/* Secondary text */
.secondary {
    color: #666;  /* Lighter, but still readable */
}
```

---

## Font Loading Best Practices

### 1. Local Hosting

**✅ Recommended:**
```css
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Regular.ttf') format('truetype');
}
```

**❌ Avoid for PDF:**
```html
<!-- CDN links unreliable in PDF generation -->
<link href="https://fonts.googleapis.com/css2?family=Roboto" rel="stylesheet">
```

### 2. Load Only Needed Weights

```css
/* Load only what you use */
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

/* Don't load unused weights (100, 200, 300, 500, 600, 800, 900) */
```

### 3. Provide Fallbacks

```css
/* Complete fallback chain */
body {
    font-family: 'Custom Font', Helvetica, Arial, sans-serif;
}
```

---

## Performance Optimization

### 1. Font Subsetting

Use only characters you need (when possible):
- Reduces file size
- Faster PDF generation
- Lower memory usage

### 2. Efficient Selectors

**❌ Slow:**
```css
body div.container div.content p span.highlight {
    font-weight: bold;
}
```

**✅ Fast:**
```css
.highlight {
    font-weight: bold;
}
```

### 3. Avoid Expensive Properties

**❌ May be slow/unsupported:**
```css
.fancy {
    text-shadow: 2pt 2pt 5pt rgba(0, 0, 0, 0.5);
    font-feature-settings: "swsh" 1;
}
```

**✅ Reliable:**
```css
.reliable {
    font-weight: 700;
    color: #2563eb;
}
```

---

## Accessibility

### 1. Sufficient Color Contrast

```css
/* Good contrast */
.readable {
    color: #333;
    background-color: white;
}

/* Warning */
.warning {
    color: #dc2626;  /* Red - sufficient contrast */
}

/* Poor contrast - avoid */
.poor {
    color: #ccc;
    background-color: white;
}
```

### 2. Readable Font Sizes

```css
/* Minimum 9pt for body text */
body {
    font-size: 11pt;  /* Comfortable */
}

.small {
    font-size: 9pt;  /* Minimum for readability */
}

/* Avoid */
.too-small {
    font-size: 7pt;  /* Too small */
}
```

### 3. Clear Structure

```html
<!-- Proper heading hierarchy -->
<h1>Main Title</h1>
  <h2>Section</h2>
    <h3>Subsection</h3>
    <h3>Subsection</h3>
  <h2>Section</h2>

<!-- Don't skip levels -->
<h1>Title</h1>
  <h3>Don't skip h2!</h3>  <!-- ❌ Bad -->
```

---

## Common Typography Mistakes

### Mistake 1: All Caps Everything

**❌ Hard to read:**
```css
body {
    text-transform: uppercase;
}
```

**✅ Selective use:**
```css
h1, h2 {
    text-transform: uppercase;
    letter-spacing: 1pt;  /* Add spacing */
}
```

### Mistake 2: Too Many Weights

**❌ Inconsistent:**
```css
.text1 { font-weight: 300; }
.text2 { font-weight: 400; }
.text3 { font-weight: 500; }
.text4 { font-weight: 600; }
.text5 { font-weight: 700; }
.text6 { font-weight: 800; }  /* Too many! */
```

**✅ Limited weights:**
```css
p { font-weight: 400; }      /* Normal */
strong { font-weight: 700; }  /* Bold */
/* That's it! */
```

### Mistake 3: Poor Line Height

**❌ Too tight:**
```css
p {
    line-height: 1.1;  /* Cramped */
}
```

**✅ Optimal:**
```css
p {
    line-height: 1.6;  /* Comfortable */
}
```

### Mistake 4: No Fallbacks

**❌ Risky:**
```css
body {
    font-family: 'Custom Font';  /* No fallback */
}
```

**✅ Safe:**
```css
body {
    font-family: 'Custom Font', Helvetica, Arial, sans-serif;
}
```

### Mistake 5: Justified Narrow Text

**❌ Large gaps:**
```css
.narrow-column {
    width: 150pt;
    text-align: justify;  /* Creates huge gaps */
}
```

**✅ Left-aligned:**
```css
.narrow-column {
    width: 150pt;
    text-align: left;
}
```

---

## Typography Checklist

### Font Selection
- [ ] Maximum 2-3 font families
- [ ] Appropriate font for document type
- [ ] All fonts properly loaded
- [ ] Complete fallback chains
- [ ] Font licensing verified

### Hierarchy
- [ ] Clear size differences between levels
- [ ] Consistent font weights
- [ ] Proper heading structure (h1→h2→h3)
- [ ] Visual distinction between elements

### Spacing
- [ ] Line height: 1.5-1.7 for body
- [ ] Line height: 1.1-1.3 for headings
- [ ] Letter spacing for uppercase text
- [ ] Consistent margin/padding scale
- [ ] Proper paragraph spacing

### Readability
- [ ] Body text: 10-12pt
- [ ] Sufficient color contrast
- [ ] Line length: 50-75 characters
- [ ] Proper alignment (left or justified)
- [ ] No tiny text (minimum 9pt)

### Technical
- [ ] Fonts locally hosted for PDF
- [ ] Only needed weights loaded
- [ ] No unsupported features
- [ ] Tested in generated PDF
- [ ] Performance optimized

---

## Professional Typography Template

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Professional Typography</title>
    <style>
        /* ==============================================
           FONTS
           ============================================== */
        @font-face {
            font-family: 'Source Sans Pro';
            src: url('./fonts/SourceSansPro-Regular.ttf');
            font-weight: 400;
        }

        @font-face {
            font-family: 'Source Sans Pro';
            src: url('./fonts/SourceSansPro-Bold.ttf');
            font-weight: 700;
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

        /* ==============================================
           TYPOGRAPHY SYSTEM
           ============================================== */
        body {
            font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
            margin: 0;
        }

        :root {
            --space-sm: 10pt;
            --space-md: 20pt;
            --space-lg: 30pt;
        }

        h1 {
            font-size: 28pt;
            font-weight: 700;
            line-height: 1.2;
            margin: 0 0 var(--space-md) 0;
            color: #1e40af;
        }

        h2 {
            font-size: 20pt;
            font-weight: 700;
            line-height: 1.3;
            margin: var(--space-lg) 0 var(--space-md) 0;
            color: #2563eb;
        }

        h3 {
            font-size: 14pt;
            font-weight: 600;
            line-height: 1.4;
            margin: var(--space-md) 0 var(--space-sm) 0;
        }

        p {
            margin: 0 0 var(--space-sm) 0;
        }

        .lead {
            font-size: 13pt;
            line-height: 1.7;
            margin-bottom: var(--space-md);
        }

        .small {
            font-size: 9pt;
            color: #666;
        }

        strong {
            font-weight: 700;
        }

        em {
            font-style: italic;
        }
    </style>
</head>
<body>
    <h1>Document Title</h1>
    <p class="lead">Lead paragraph with larger text...</p>
    <!-- Content -->
</body>
</html>
```

---

## Best Practices Summary

1. **Limit fonts** - Maximum 2-3 families
2. **Clear hierarchy** - Obvious size differences
3. **Consistent spacing** - Use a scale
4. **Optimal line height** - 1.5-1.7 for body
5. **Local hosting** - Don't rely on CDNs for PDF
6. **Provide fallbacks** - Always include system fonts
7. **Sufficient contrast** - Minimum 4.5:1 ratio
8. **Test thoroughly** - Generate PDFs early and often
9. **Professional details** - Smart quotes, proper dashes
10. **Keep it simple** - Clarity over decoration

---

## Next Steps

You've completed the Typography & Fonts series! Continue your journey:

1. **[Content Components](/learning/06-content/)** - Images, lists, tables
2. **[Configuration](/learning/07-configuration/)** - Document settings
3. **[Practical Applications](/learning/08-practical/)** - Real-world documents

---

**Continue learning →** [Content Components](/learning/06-content/)**
