---
layout: default
title: Styling Best Practices
nav_order: 8
parent: Styling & Appearance
parent_url: /learning/03-styling/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Styling Best Practices

Master professional styling patterns, performance optimization, and common gotchas for PDF generation.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Apply performance optimization techniques
- Understand PDF vs browser rendering differences
- Avoid common styling pitfalls
- Troubleshoot styling issues effectively
- Follow maintainable styling patterns
- Build professional, production-ready documents

---

## Performance Best Practices

### Keep Selectors Simple

**❌ Complex, slow selectors:**
```css
body div.container div.content div.section div.box p.text {
    color: blue;
}
```

**✅ Simple, fast selectors:**
```css
.text {
    color: blue;
}
```

**Why?** Simple selectors are faster to match and easier to maintain.

### Minimize Specificity Wars

**❌ Escalating specificity:**
```css
.card { background: white; }
.card.highlighted { background: yellow; }
div.card.highlighted { background: yellow !important; }
```

**✅ Controlled specificity:**
```css
.card { background: white; }
.card-highlighted { background: yellow; }
```

### Use Efficient Properties

**❌ Expensive:**
```css
.element {
    box-shadow: 0 10pt 20pt rgba(0, 0, 0, 0.5);  /* Complex rendering */
    filter: blur(5pt);  /* May not be supported */
}
```

**✅ Efficient:**
```css
.element {
    border: 1pt solid #d1d5db;  /* Simple, fast */
    background-color: #f9fafb;  /* Solid colors are fast */
}
```

---

## PDF vs Browser Differences

### Key Differences

| Feature | Browser | PDF/Scryber |
|---------|---------|-------------|
| Layout | Continuous scroll | Fixed pages |
| Interactive | Yes | Limited |
| Fonts | Web fonts cached | Embedded in PDF |
| Units | Pixels (device-dependent) | Points (absolute) |
| Media queries | Yes | Limited |
| Flexbox | Full support | No support |
| Grid | Full support | No support |
| JavaScript | Full support | No support |

### Layout Differences

**Browser approach (doesn't work in PDF):**
```css
/* Flexbox - NOT supported */
.container {
    display: flex;
    justify-content: space-between;
}

/* CSS Grid - NOT supported */
.grid {
    display: grid;
    grid-template-columns: 1fr 1fr 1fr;
}
```

**PDF approach (works correctly):**
```css
/* Table-based layout */
.container {
    display: table;
    width: 100%;
}

.column {
    display: table-cell;
    width: 33.33%;
}

/* Float-based layout */
.column {
    float: left;
    width: 33.33%;
}
```

### Supported CSS Features

**✅ Well-supported:**
- Typography (font-size, font-weight, font-family, color)
- Box model (margin, padding, border, width, height)
- Positioning (static, relative, absolute)
- Display (block, inline, inline-block, table, none)
- Colors (hex, RGB, RGBA, named)
- Text alignment and decoration
- Lists and tables
- Page breaks
- Background colors and images

**⚠️ Limited/Not supported:**
- Flexbox
- CSS Grid
- Transformations (rotate, scale, skew)
- Animations and transitions
- Some pseudo-elements
- Advanced filters
- Media queries (limited)
- Viewport units (vw, vh)
- calc() with complex expressions

---

## Common Pitfalls and Solutions

### Pitfall 1: Expecting Browser Behavior

**❌ Problem:**
```css
/* Expecting smooth scrolling, animations */
.element {
    transition: all 0.3s ease;
    animation: fadeIn 1s;
}
```

**✅ Solution:**
```css
/* Design for static, page-based layout */
.element {
    /* Static styles only */
    opacity: 1;
}
```

### Pitfall 2: Using Unsupported Units

**❌ Problem:**
```css
.element {
    width: 50vw;   /* Viewport width - not supported */
    height: 100vh; /* Viewport height - not supported */
}
```

**✅ Solution:**
```css
.element {
    width: 50%;    /* Percentage works */
    height: 400pt; /* Points work */
}
```

### Pitfall 3: Complex Backgrounds

**❌ Problem:**
```css
.element {
    background: linear-gradient(45deg, #667eea 0%, #764ba2 100%),
                url('pattern.png'),
                radial-gradient(circle, #fff 0%, #000 100%);
}
```

**✅ Solution:**
```css
/* Simpler background */
.element {
    background-color: #667eea;
    background-image: url('pattern.png');
}
```

### Pitfall 4: Forgetting Page Breaks

**❌ Problem:**
```css
/* No control over page breaks */
.section {
    margin-bottom: 40pt;
}
```

**✅ Solution:**
```css
/* Control page breaks explicitly */
.section {
    margin-bottom: 40pt;
    page-break-inside: avoid;  /* Don't break inside */
}

.chapter {
    page-break-before: always;  /* Always start new page */
}
```

### Pitfall 5: Using External Resources

**❌ Problem:**
```css
/* Remote resources may fail to load */
@import url('https://fonts.googleapis.com/css2?family=Roboto');

.element {
    background-image: url('https://example.com/slow-loading-image.jpg');
}
```

**✅ Solution:**
```css
/* Use local resources or embed */
@font-face {
    font-family: 'Roboto';
    src: url('./fonts/Roboto-Regular.ttf');
}

.element {
    background-image: url('./images/image.jpg');
    /* Or use base64 for small images */
}
```

---

## Maintainable Patterns

### Pattern 1: Consistent Spacing Scale

**❌ Inconsistent:**
```css
.element1 { margin: 11pt; }
.element2 { margin: 17pt; }
.element3 { margin: 23pt; }
```

**✅ Consistent scale:**
```css
/* Use scale: 10pt, 20pt, 30pt, 40pt */
.element1 { margin: 10pt; }
.element2 { margin: 20pt; }
.element3 { margin: 30pt; }
```

### Pattern 2: Consistent Color Palette

**❌ Random colors:**
```css
.heading1 { color: #2563eb; }
.heading2 { color: #3b70f4; }
.heading3 { color: #4a82f7; }
```

**✅ Defined palette:**
```css
/* Define colors */
:root {
    --color-primary: #2563eb;
    --color-secondary: #3b82f6;
    --color-accent: #60a5fa;
}

/* Use consistently */
.heading1 { color: var(--color-primary); }
.heading2 { color: var(--color-secondary); }
```

### Pattern 3: Reusable Components

**❌ Duplicated styles:**
```css
.box1 {
    border: 1pt solid #d1d5db;
    padding: 20pt;
    border-radius: 5pt;
}

.box2 {
    border: 1pt solid #d1d5db;
    padding: 20pt;
    border-radius: 5pt;
}
```

**✅ Component class:**
```css
/* Base component */
.card {
    border: 1pt solid #d1d5db;
    padding: 20pt;
    border-radius: 5pt;
}

/* Variants */
.card-large {
    padding: 30pt;
}

.card-highlighted {
    border-color: #2563eb;
    background-color: #eff6ff;
}
```

### Pattern 4: Mobile-First Thinking

**❌ Desktop-first:**
```css
.element {
    width: 800pt;  /* Fixed desktop width */
}
```

**✅ Flexible approach:**
```css
.element {
    width: 100%;    /* Flexible */
    max-width: 800pt;  /* Constrained */
}
```

---

## Troubleshooting Guide

### Issue: Styles Not Applying

**Checklist:**
1. ✅ Check specificity (use browser dev tools simulation)
2. ✅ Verify CSS syntax (missing semicolons, braces)
3. ✅ Check property names (correct spelling)
4. ✅ Ensure stylesheet is loaded (check file path)
5. ✅ Verify element exists in HTML
6. ✅ Check for typos in class names

**Debug technique:**
```css
/* Add temporary high-visibility style */
.debug {
    border: 5pt solid red !important;
    background-color: yellow !important;
}
```

### Issue: Layout Not Working

**Checklist:**
1. ✅ Verify box-sizing is set
2. ✅ Check for flexbox/grid (not supported)
3. ✅ Use table or float layout instead
4. ✅ Check for width overflow
5. ✅ Verify margins aren't collapsing
6. ✅ Test with simpler layout first

### Issue: Text Not Visible

**Checklist:**
1. ✅ Check color contrast
2. ✅ Verify font-size is set
3. ✅ Check if element has display: none
4. ✅ Verify text color vs background color
5. ✅ Check for overflow: hidden
6. ✅ Verify font is loaded

### Issue: Images Not Showing

**Checklist:**
1. ✅ Verify file path is correct
2. ✅ Check if file exists
3. ✅ Use absolute paths when possible
4. ✅ Verify image format is supported (PNG, JPEG, GIF)
5. ✅ Check file size (very large images may fail)
6. ✅ Test with local file first

---

## Production Checklist

Before deploying PDF generation:

### Styling Checklist

- [ ] All colors have sufficient contrast
- [ ] Consistent spacing scale used throughout
- [ ] Typography hierarchy is clear
- [ ] No flexbox or grid layout
- [ ] All fonts are loaded/embedded
- [ ] All images load correctly
- [ ] External resources are local or cached
- [ ] Page breaks are controlled
- [ ] Styles are organized and commented
- [ ] No !important unless necessary

### Testing Checklist

- [ ] Test with sample data
- [ ] Test with edge cases (empty, null, very long)
- [ ] Test all conditional branches
- [ ] Verify page breaks
- [ ] Check file size
- [ ] Test on different page sizes
- [ ] Verify metadata (title, author, etc.)
- [ ] Check for console errors/warnings
- [ ] Validate generated PDF opens correctly
- [ ] Test print output if applicable

### Performance Checklist

- [ ] Images are optimized
- [ ] External resources are minimized
- [ ] CSS is efficient (simple selectors)
- [ ] No unnecessary complex calculations
- [ ] Fonts are subsetted if possible
- [ ] Large tables have explicit widths
- [ ] Templates are cached when reused
- [ ] Error handling is in place

---

## Quick Reference

### Do's ✅

1. **Use points (pt)** for measurements
2. **Keep selectors simple** (max 3 levels)
3. **Use table-cell** for multi-column layouts
4. **Control page breaks** explicitly
5. **Test with real data** early and often
6. **Organize styles** logically
7. **Use consistent spacing** scale
8. **Define color palette** upfront
9. **Comment complex styles**
10. **Test edge cases**

### Don'ts ❌

1. **Don't use flexbox or grid**
2. **Don't rely on viewport units**
3. **Don't use animations/transitions**
4. **Don't use !important excessively**
5. **Don't forget page breaks**
6. **Don't use remote resources** without caching
7. **Don't overcomplicate selectors**
8. **Don't forget to test**
9. **Don't ignore file size**
10. **Don't skip error handling**

---

## Real-World Example

**Professional Invoice with Best Practices:**

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Invoice</title>
    <style>
        /* ==============================================
           BASE STYLES - Use points, simple selectors
           ============================================== */
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;  /* Predictable sizing */
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
            margin: 0;
        }

        /* ==============================================
           TYPOGRAPHY - Consistent hierarchy
           ============================================== */
        h1 {
            font-size: 24pt;
            color: #1e40af;
            margin: 0 0 10pt 0;
        }

        h2 {
            font-size: 18pt;
            color: #2563eb;
            margin: 20pt 0 10pt 0;
        }

        /* ==============================================
           COMPONENTS - Reusable, well-organized
           ============================================== */
        .header {
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 20pt;
            margin-bottom: 30pt;
        }

        .invoice-details {
            margin-bottom: 30pt;
        }

        .info-row {
            margin-bottom: 5pt;
        }

        .label {
            font-weight: bold;
            display: inline-block;
            width: 150pt;
        }

        /* Table with explicit structure */
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        thead {
            background-color: #2563eb;
            color: white;
        }

        th {
            padding: 10pt;
            text-align: left;
            font-weight: bold;
        }

        td {
            padding: 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        tfoot {
            background-color: #eff6ff;
            font-weight: bold;
        }

        /* ==============================================
           UTILITIES - Consistent spacing
           ============================================== */
        .text-right {
            text-align: right;
        }

        .text-bold {
            font-weight: bold;
        }

        .mb-10 {
            margin-bottom: 10pt;
        }

        .mb-20 {
            margin-bottom: 20pt;
        }

        /* ==============================================
           PAGE BREAKS - Controlled pagination
           ============================================== */
        .no-break {
            page-break-inside: avoid;
        }
    </style>
</head>
<body>
    <div class="header no-break">
        <h1>INVOICE</h1>
        <p>Invoice #: INV-2025-001</p>
        <p>Date: January 15, 2025</p>
    </div>

    <div class="invoice-details no-break">
        <h2>Bill To</h2>
        <div class="info-row">
            <span class="label">Company:</span>
            <span>Acme Corporation</span>
        </div>
        <div class="info-row">
            <span class="label">Address:</span>
            <span>123 Main St, City, ST 12345</span>
        </div>
    </div>

    <table class="no-break">
        <thead>
            <tr>
                <th>Description</th>
                <th>Quantity</th>
                <th>Rate</th>
                <th class="text-right">Amount</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Consulting Services</td>
                <td>10</td>
                <td>$150.00</td>
                <td class="text-right">$1,500.00</td>
            </tr>
            <tr>
                <td>Development</td>
                <td>20</td>
                <td>$200.00</td>
                <td class="text-right">$4,000.00</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3">Subtotal</td>
                <td class="text-right">$5,500.00</td>
            </tr>
            <tr>
                <td colspan="3">Tax (8%)</td>
                <td class="text-right">$440.00</td>
            </tr>
            <tr style="font-size: 14pt;">
                <td colspan="3">TOTAL</td>
                <td class="text-right">$5,940.00</td>
            </tr>
        </tfoot>
    </table>

    <div style="margin-top: 40pt; padding-top: 20pt; border-top: 1pt solid #d1d5db; font-size: 9pt; color: #666;">
        <p class="text-bold">Payment Terms:</p>
        <p>Payment due within 30 days. Thank you for your business!</p>
    </div>
</body>
</html>
```

---

## Next Steps

You've completed the Styling & Appearance series! Continue your journey:

1. **[Layout & Positioning](/learning/04-layout/)** - Master page layout and positioning
2. **[Typography & Fonts](/learning/05-typography/)** - Custom fonts and advanced typography
3. **[Practical Applications](/learning/08-practical/)** - Build real-world documents

---

**Continue learning →** [Layout & Positioning](/learning/04-layout/)**
