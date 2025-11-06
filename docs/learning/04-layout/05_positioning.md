---
layout: default
title: Positioning
nav_order: 5
parent: Layout & Positioning
parent_url: /learning/04-layout/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Positioning

Master CSS positioning for precise element placement in PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Understand positioning types (static, relative, absolute)
- Use absolute positioning for precise placement
- Apply relative positioning for offset adjustments
- Position elements relative to containers
- Create overlays and watermarks
- Position elements on specific pages
- Avoid common positioning pitfalls

---

## Position Types

### static (Default)

Normal document flow - elements appear in order.

```css
.element {
    position: static;  /* Default, not usually specified */
}
```

**Characteristics:**
- Elements appear in normal document flow
- top, right, bottom, left properties have no effect
- Cannot be used as positioning context for absolute children

### relative

Offset from normal position, but space preserved.

```css
.element {
    position: relative;
    top: 10pt;   /* Move down 10pt */
    left: 20pt;  /* Move right 20pt */
}
```

**Characteristics:**
- Element offset from where it would normally be
- Original space is preserved
- Can be positioning context for absolute children
- Useful for small adjustments

### absolute

Positioned relative to nearest positioned ancestor.

```css
.element {
    position: absolute;
    top: 50pt;    /* 50pt from top */
    right: 50pt;  /* 50pt from right */
}
```

**Characteristics:**
- Removed from normal document flow
- No space preserved
- Positioned relative to nearest positioned (non-static) ancestor
- If no positioned ancestor, positioned relative to page

---

## Relative Positioning

Offset elements from their normal position.

### Basic Offset

```css
.nudge-down {
    position: relative;
    top: 5pt;  /* Move down 5pt */
}

.nudge-right {
    position: relative;
    left: 10pt;  /* Move right 10pt */
}

.nudge-up {
    position: relative;
    top: -5pt;  /* Move up 5pt (negative) */
}
```

### Creating Positioning Context

```css
.container {
    position: relative;  /* Creates context for absolute children */
}

.child {
    position: absolute;  /* Positioned relative to .container */
    top: 0;
    right: 0;
}
```

---

## Absolute Positioning

Precise element placement.

### Positioning from Corners

```css
/* Top-left corner */
.top-left {
    position: absolute;
    top: 20pt;
    left: 20pt;
}

/* Top-right corner */
.top-right {
    position: absolute;
    top: 20pt;
    right: 20pt;
}

/* Bottom-left corner */
.bottom-left {
    position: absolute;
    bottom: 20pt;
    left: 20pt;
}

/* Bottom-right corner */
.bottom-right {
    position: absolute;
    bottom: 20pt;
    right: 20pt;
}
```

### Centering with Absolute Position

```css
.centered {
    position: absolute;
    width: 200pt;
    height: 100pt;

    /* Center horizontally */
    left: 50%;
    margin-left: -100pt;  /* Half of width */

    /* Center vertically */
    top: 50%;
    margin-top: -50pt;  /* Half of height */
}

/* Alternative with calc() */
.centered-calc {
    position: absolute;
    width: 200pt;
    height: 100pt;
    left: calc(50% - 100pt);
    top: calc(50% - 50pt);
}
```

### Stretching to Fill Container

```css
.fill-container {
    position: absolute;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    /* Stretches to fill entire container */
}

/* With padding/margins */
.fill-with-spacing {
    position: absolute;
    top: 20pt;
    right: 20pt;
    bottom: 20pt;
    left: 20pt;
}
```

---

## Positioning Context

Control what elements are positioned relative to.

### Page as Context (Default)

```css
/* No positioned ancestors - relative to page */
.watermark {
    position: absolute;
    bottom: 50pt;
    right: 50pt;
    opacity: 0.3;
}
```

### Container as Context

```css
/* Container creates positioning context */
.card {
    position: relative;  /* Creates context */
    padding: 20pt;
    border: 1pt solid #d1d5db;
}

/* Badge positioned relative to card */
.badge {
    position: absolute;
    top: -10pt;   /* Relative to .card */
    right: -10pt;
    background-color: #dc2626;
    color: white;
    padding: 5pt 10pt;
    border-radius: 10pt;
    font-size: 9pt;
    font-weight: bold;
}
```

---

## Z-Index (Stacking Order)

Control which elements appear on top.

```css
.layer-bottom {
    position: absolute;
    z-index: 1;  /* Lower number = further back */
}

.layer-middle {
    position: absolute;
    z-index: 10;  /* Higher than layer-bottom */
}

.layer-top {
    position: absolute;
    z-index: 100;  /* Highest - appears on top */
}
```

**Default stacking:**
- Elements later in HTML appear on top
- z-index only works on positioned elements (not static)
- Higher z-index = appears on top

---

## Practical Examples

### Example 1: Watermark

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document with Watermark</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
            position: relative;  /* Create positioning context */
        }

        /* ==============================================
           WATERMARK
           ============================================== */
        .watermark {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%) rotate(-45deg);
            font-size: 72pt;
            color: #e5e7eb;
            opacity: 0.3;
            font-weight: bold;
            z-index: 0;  /* Behind content */
            pointer-events: none;  /* Don't interfere with text selection */
        }

        /* ==============================================
           CONTENT
           ============================================== */
        .content {
            position: relative;
            z-index: 1;  /* Above watermark */
        }

        h1 {
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 20pt;
        }

        p {
            margin-bottom: 12pt;
        }
    </style>
</head>
<body>
    <div class="watermark">CONFIDENTIAL</div>

    <div class="content">
        <h1>Confidential Report</h1>

        <p>This document contains confidential information and is intended only for authorized personnel.</p>

        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>

        <p>Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>

        <p>Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.</p>
    </div>
</body>
</html>
```

### Example 2: Badge on Cards

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Cards with Badges</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            margin: 0;
        }

        h1 {
            text-align: center;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 30pt;
        }

        /* ==============================================
           CARD CONTAINER
           ============================================== */
        .cards-container {
            display: table;
            width: 100%;
            border-spacing: 20pt 20pt;
        }

        .card-row {
            display: table-row;
        }

        .card-cell {
            display: table-cell;
            width: 50%;
            vertical-align: top;
        }

        /* ==============================================
           CARD
           ============================================== */
        .card {
            position: relative;  /* Creates positioning context */
            border: 1pt solid #d1d5db;
            border-radius: 8pt;
            padding: 20pt;
            background-color: white;
        }

        .card-title {
            font-size: 16pt;
            font-weight: bold;
            color: #1e40af;
            margin: 0 0 10pt 0;
        }

        .card-content {
            font-size: 10pt;
            color: #666;
            margin: 0;
        }

        /* ==============================================
           BADGES
           ============================================== */
        .badge {
            position: absolute;
            top: -10pt;
            right: -10pt;
            padding: 5pt 12pt;
            border-radius: 15pt;
            font-size: 9pt;
            font-weight: bold;
            color: white;
            z-index: 10;
        }

        .badge-new {
            background-color: #059669;
        }

        .badge-sale {
            background-color: #dc2626;
        }

        .badge-popular {
            background-color: #2563eb;
        }

        .badge-limited {
            background-color: #f59e0b;
        }

        /* ==============================================
           PRICE TAG
           ============================================== */
        .price {
            position: absolute;
            bottom: 15pt;
            right: 15pt;
            font-size: 20pt;
            font-weight: bold;
            color: #2563eb;
        }
    </style>
</head>
<body>
    <h1>Featured Products</h1>

    <div class="cards-container">
        <div class="card-row">
            <div class="card-cell">
                <div class="card">
                    <span class="badge badge-new">NEW</span>
                    <h2 class="card-title">Premium Widget</h2>
                    <p class="card-content">
                        Our flagship product with advanced features and premium build quality. Perfect for professional use.
                    </p>
                    <div class="price">$299</div>
                </div>
            </div>

            <div class="card-cell">
                <div class="card">
                    <span class="badge badge-sale">SALE</span>
                    <h2 class="card-title">Standard Widget</h2>
                    <p class="card-content">
                        Reliable performance at an affordable price. Great value for everyday use.
                    </p>
                    <div class="price">$149</div>
                </div>
            </div>
        </div>

        <div class="card-row">
            <div class="card-cell">
                <div class="card">
                    <span class="badge badge-popular">POPULAR</span>
                    <h2 class="card-title">Deluxe Widget</h2>
                    <p class="card-content">
                        Enhanced features with excellent value. Our most popular choice among customers.
                    </p>
                    <div class="price">$199</div>
                </div>
            </div>

            <div class="card-cell">
                <div class="card">
                    <span class="badge badge-limited">LIMITED</span>
                    <h2 class="card-title">Special Edition</h2>
                    <p class="card-content">
                        Exclusive limited edition with unique features. Only available while supplies last.
                    </p>
                    <div class="price">$399</div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
```

### Example 3: Header with Logo Positioning

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Positioned Header</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        /* ==============================================
           HEADER
           ============================================== */
        .page-header {
            position: relative;  /* Creates positioning context */
            height: 80pt;
            margin-bottom: 30pt;
            border-bottom: 2pt solid #2563eb;
            padding: 15pt 0;
        }

        .logo {
            position: absolute;
            top: 15pt;
            left: 0;
            width: 60pt;
            height: 60pt;
            background-color: #2563eb;
            border-radius: 5pt;
            color: white;
            font-size: 24pt;
            font-weight: bold;
            display: table;
        }

        .logo-text {
            display: table-cell;
            vertical-align: middle;
            text-align: center;
        }

        .company-info {
            position: absolute;
            top: 15pt;
            left: 80pt;  /* After logo */
        }

        .company-name {
            font-size: 20pt;
            font-weight: bold;
            color: #1e40af;
            margin: 0 0 5pt 0;
        }

        .company-tagline {
            font-size: 11pt;
            color: #666;
            margin: 0;
        }

        .contact-info {
            position: absolute;
            top: 15pt;
            right: 0;
            text-align: right;
            font-size: 9pt;
            color: #666;
        }

        .contact-info p {
            margin: 0 0 3pt 0;
        }

        /* ==============================================
           CONTENT
           ============================================== */
        h1 {
            color: #1e40af;
            font-size: 24pt;
            margin-top: 0;
            margin-bottom: 20pt;
        }

        p {
            margin-bottom: 12pt;
        }
    </style>
</head>
<body>
    <div class="page-header">
        <div class="logo">
            <div class="logo-text">A</div>
        </div>

        <div class="company-info">
            <div class="company-name">Acme Corporation</div>
            <p class="company-tagline">Excellence in Innovation</p>
        </div>

        <div class="contact-info">
            <p>123 Business Street, Suite 100</p>
            <p>New York, NY 10001</p>
            <p>(555) 123-4567</p>
        </div>
    </div>

    <h1>Business Proposal</h1>

    <p>This proposal outlines our comprehensive solution for your business needs.</p>

    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>

    <p>Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Corner Badges

Create a document with:
- Multiple cards or boxes
- Different colored badges in corners (NEW, SALE, etc.)
- Badges positioned with absolute positioning
- Various badge positions (top-left, top-right, etc.)

### Exercise 2: Watermark

Create a watermarked document:
- Diagonal "DRAFT" or "CONFIDENTIAL" watermark
- Semi-transparent overlay
- Watermark behind content (z-index)
- Content remains readable

### Exercise 3: Complex Header

Create a header with:
- Logo on the left (absolute position)
- Company name in center
- Contact info on the right (absolute position)
- All within relative container

---

## Common Pitfalls

### ❌ Forgetting Positioning Context

```css
/* Parent has no position */
.parent {
    /* position: static (default) */
}

.child {
    position: absolute;
    top: 0;  /* Positioned relative to page, not parent! */
}
```

✅ **Solution:** Create positioning context

```css
.parent {
    position: relative;  /* Creates context */
}

.child {
    position: absolute;
    top: 0;  /* Now relative to .parent */
}
```

### ❌ Overlapping Content Unintentionally

```css
.element {
    position: absolute;
    top: 100pt;
    left: 100pt;
    /* May overlap with other content */
}
```

✅ **Solution:** Use z-index and plan layout

```css
.background-element {
    position: absolute;
    z-index: 0;  /* Behind */
}

.content {
    position: relative;
    z-index: 1;  /* On top */
}
```

### ❌ Using Position Without Offset

```css
.element {
    position: absolute;
    /* No top/right/bottom/left - appears at default position */
}
```

✅ **Solution:** Specify position

```css
.element {
    position: absolute;
    top: 50pt;
    left: 100pt;
}
```

### ❌ Centering Without Width/Height

```css
.centered {
    position: absolute;
    left: 50%;
    /* No width specified - can't calculate offset */
}
```

✅ **Solution:** Specify dimensions

```css
.centered {
    position: absolute;
    width: 200pt;  /* Known width */
    left: calc(50% - 100pt);  /* Center */
}
```

### ❌ Position: Fixed (Not Supported)

```css
.element {
    position: fixed;  /* Not supported in PDF */
}
```

✅ **Solution:** Use absolute positioning

```css
.element {
    position: absolute;
    top: 0;
    right: 0;
}
```

---

## Positioning Checklist

- [ ] Positioning context created (position: relative on parent)
- [ ] Offset values specified (top, right, bottom, left)
- [ ] z-index set if stacking elements
- [ ] Width/height specified for centered elements
- [ ] Doesn't overlap important content unintentionally
- [ ] Works across page breaks (test multi-page documents)
- [ ] Avoid position: fixed (not supported)

---

## Best Practices

1. **Create positioning context** - Use `position: relative` on containers
2. **Use absolute for overlays** - Watermarks, badges, decorative elements
3. **Use relative for adjustments** - Small offsets from normal position
4. **Control stacking with z-index** - Plan layer order intentionally
5. **Specify dimensions** - Required for centering calculations
6. **Test with content** - Positioning may behave differently with real data
7. **Avoid excessive nesting** - Keep positioning contexts simple
8. **Document complex positioning** - Comment why elements are positioned

---

## Next Steps

Now that you master positioning:

1. **[Tables](06_tables.md)** - Advanced table layouts
2. **[Headers & Footers](07_headers_footers.md)** - Repeating page elements
3. **[Layout Best Practices](08_layout_best_practices.md)** - Professional patterns

---

**Continue learning →** [Tables](06_tables.md)
