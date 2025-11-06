---
layout: default
title: Style Organization
nav_order: 7
parent: Styling & Appearance
parent_url: /learning/03-styling/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Style Organization

Master techniques for organizing, maintaining, and scaling your CSS for professional PDF generation.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Choose between inline, embedded, and external styles
- Create reusable style classes
- Organize styles logically
- Use multiple stylesheets
- Maintain consistent styling across documents
- Build scalable style systems

---

## Three Ways to Apply Styles

### 1. Inline Styles

Styles applied directly to elements.

```html
<p style="color: #2563eb; font-size: 12pt; margin-bottom: 10pt;">
    This paragraph has inline styles.
</p>
```

**Pros:**
- Quick for one-off styling
- High specificity
- No external dependencies

**Cons:**
- Not reusable
- Hard to maintain
- Mixes content and presentation
- Bloats HTML

**When to use:**
- One-time, unique styling
- Dynamic styles from data binding
- Quick prototyping

### 2. Embedded Styles

Styles in `<style>` element within document.

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            margin: 40pt;
        }

        h1 {
            color: #1e40af;
            font-size: 24pt;
            margin-bottom: 20pt;
        }

        .highlight {
            background-color: #fef3c7;
            padding: 10pt;
        }
    </style>
</head>
<body>
    <!-- Content -->
</body>
</html>
```

**Pros:**
- All styles in one file
- Reusable within document
- Better organization
- Easy to share single file

**Cons:**
- Not reusable across documents
- Can become large
- Harder to maintain multiple documents

**When to use:**
- Self-contained documents
- Single-template projects
- When styles are document-specific

### 3. External Stylesheets

Styles in separate .css files.

**styles.css:**
```css
body {
    font-family: Helvetica, sans-serif;
    font-size: 11pt;
    margin: 40pt;
}

h1 {
    color: #1e40af;
    font-size: 24pt;
    margin-bottom: 20pt;
}

.highlight {
    background-color: #fef3c7;
    padding: 10pt;
}
```

**template.html:**
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document</title>
    <link rel="stylesheet" href="./styles/common.css" />
</head>
<body>
    <!-- Content -->
</body>
</html>
```

**Pros:**
- Reusable across documents
- Easy to maintain
- Centralized styling
- Better organization

**Cons:**
- Additional file dependencies
- Need to manage file paths
- Requires file system access

**When to use:**
- Multiple documents with shared styles
- Large projects
- Team collaboration
- When styles are reused

---

## Creating Reusable Classes

### Utility Classes

Small, single-purpose classes.

```css
/* Spacing utilities */
.m-0 { margin: 0; }
.m-10 { margin: 10pt; }
.m-20 { margin: 20pt; }
.mt-10 { margin-top: 10pt; }
.mb-20 { margin-bottom: 20pt; }

.p-0 { padding: 0; }
.p-10 { padding: 10pt; }
.p-20 { padding: 20pt; }

/* Text utilities */
.text-left { text-align: left; }
.text-center { text-align: center; }
.text-right { text-align: right; }
.text-bold { font-weight: bold; }
.text-italic { font-style: italic; }

/* Color utilities */
.text-blue { color: #2563eb; }
.text-red { color: #dc2626; }
.text-gray { color: #666; }

.bg-gray { background-color: #f9fafb; }
.bg-blue { background-color: #eff6ff; }
```

**Usage:**
```html
<p class="mt-20 mb-10 text-center text-bold text-blue">
    Centered, bold, blue text with spacing
</p>
```

### Component Classes

Styles for specific UI components.

```css
/* Card component */
.card {
    border: 1pt solid #d1d5db;
    border-radius: 5pt;
    padding: 20pt;
    margin-bottom: 20pt;
    background-color: white;
}

.card-header {
    border-bottom: 1pt solid #e5e7eb;
    padding-bottom: 15pt;
    margin-bottom: 15pt;
}

.card-title {
    margin: 0;
    color: #1e40af;
    font-size: 18pt;
}

.card-body {
    font-size: 11pt;
}

.card-footer {
    border-top: 1pt solid #e5e7eb;
    padding-top: 15pt;
    margin-top: 15pt;
    font-size: 9pt;
    color: #666;
}
```

**Usage:**
```html
<div class="card">
    <div class="card-header">
        <h2 class="card-title">Card Title</h2>
    </div>
    <div class="card-body">
        <p>Card content goes here.</p>
    </div>
    <div class="card-footer">
        Last updated: January 15, 2025
    </div>
</div>
```

### State Classes

Classes for different states.

```css
/* Button states */
.btn {
    padding: 10pt 20pt;
    border-radius: 5pt;
    font-weight: bold;
}

.btn-primary {
    background-color: #2563eb;
    color: white;
    border: 2pt solid #2563eb;
}

.btn-secondary {
    background-color: white;
    color: #2563eb;
    border: 2pt solid #2563eb;
}

.btn-disabled {
    opacity: 0.5;
    cursor: not-allowed;
}

/* Alert states */
.alert {
    padding: 15pt;
    border-radius: 5pt;
    margin-bottom: 20pt;
}

.alert-success {
    background-color: #d1fae5;
    color: #065f46;
    border-left: 4pt solid #059669;
}

.alert-error {
    background-color: #fee2e2;
    color: #991b1b;
    border-left: 4pt solid #dc2626;
}
```

---

## Organizing Stylesheet Structure

### Logical Organization

```css
/* ==============================================
   BASE STYLES
   ============================================== */

/* Page setup */
@page {
    size: Letter;
    margin: 1in;
}

/* Typography */
body {
    font-family: Helvetica, sans-serif;
    font-size: 11pt;
    line-height: 1.6;
    color: #333;
}

h1, h2, h3, h4, h5, h6 {
    font-weight: bold;
    line-height: 1.2;
}

h1 { font-size: 24pt; margin-bottom: 20pt; color: #1e40af; }
h2 { font-size: 20pt; margin-bottom: 15pt; color: #2563eb; }
h3 { font-size: 16pt; margin-bottom: 12pt; color: #3b82f6; }

/* ==============================================
   LAYOUT
   ============================================== */

.container {
    width: 100%;
    max-width: 600pt;
    margin: 0 auto;
}

.section {
    margin-bottom: 40pt;
}

.row {
    display: table;
    width: 100%;
}

.col {
    display: table-cell;
    padding: 15pt;
}

/* ==============================================
   COMPONENTS
   ============================================== */

/* Cards */
.card {
    border: 1pt solid #d1d5db;
    border-radius: 5pt;
    padding: 20pt;
    margin-bottom: 20pt;
}

/* Buttons */
.btn {
    display: inline-block;
    padding: 10pt 20pt;
    border-radius: 5pt;
    font-weight: bold;
}

/* Alerts */
.alert {
    padding: 15pt;
    border-radius: 5pt;
    margin-bottom: 20pt;
}

/* ==============================================
   UTILITIES
   ============================================== */

/* Spacing */
.mt-10 { margin-top: 10pt; }
.mt-20 { margin-top: 20pt; }
.mb-10 { margin-bottom: 10pt; }
.mb-20 { margin-bottom: 20pt; }

/* Text alignment */
.text-left { text-align: left; }
.text-center { text-align: center; }
.text-right { text-align: right; }

/* Colors */
.text-blue { color: #2563eb; }
.text-gray { color: #666; }

/* ==============================================
   CUSTOM STYLES
   ============================================== */

/* Project-specific styles */
```

### Multiple Stylesheets

Split styles into logical files:

**base.css** - Foundation styles
```css
/* Typography, colors, base elements */
body { font-family: Helvetica, sans-serif; }
h1 { font-size: 24pt; }
/* ... */
```

**layout.css** - Layout and structure
```css
/* Grid, containers, sections */
.container { width: 100%; }
.row { display: table; }
/* ... */
```

**components.css** - UI components
```css
/* Cards, buttons, alerts, etc. */
.card { border: 1pt solid #d1d5db; }
.btn { padding: 10pt 20pt; }
/* ... */
```

**utilities.css** - Utility classes
```css
/* Spacing, text, colors */
.mt-10 { margin-top: 10pt; }
.text-center { text-align: center; }
/* ... */
```

**HTML with multiple stylesheets:**
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document</title>
    <link rel="stylesheet" href="./styles/base.css" />
    <link rel="stylesheet" href="./styles/layout.css" />
    <link rel="stylesheet" href="./styles/components.css" />
    <link rel="stylesheet" href="./styles/utilities.css" />
</head>
<body>
    <!-- Content -->
</body>
</html>
```

---

## Naming Conventions

### BEM (Block Element Modifier)

```css
/* Block */
.card { }

/* Element */
.card__header { }
.card__body { }
.card__footer { }

/* Modifier */
.card--highlighted { }
.card--large { }
```

**Usage:**
```html
<div class="card card--highlighted">
    <div class="card__header">
        <h2>Title</h2>
    </div>
    <div class="card__body">
        <p>Content</p>
    </div>
</div>
```

### Functional/Utility Names

```css
.text-center { text-align: center; }
.bg-blue { background-color: #2563eb; }
.p-20 { padding: 20pt; }
.mb-10 { margin-bottom: 10pt; }
```

### Descriptive Names

```css
.primary-button { }
.warning-message { }
.success-badge { }
.sidebar-navigation { }
```

---

## Practical Example: Complete Organization

**base.css:**
```css
/* ==============================================
   TYPOGRAPHY
   ============================================== */
body {
    font-family: Helvetica, sans-serif;
    font-size: 11pt;
    line-height: 1.6;
    color: #333;
}

h1 { font-size: 24pt; color: #1e40af; margin-bottom: 20pt; }
h2 { font-size: 20pt; color: #2563eb; margin-bottom: 15pt; }
h3 { font-size: 16pt; color: #3b82f6; margin-bottom: 12pt; }

p { margin-bottom: 12pt; }

/* ==============================================
   COLORS
   ============================================== */
:root {
    --color-primary: #2563eb;
    --color-secondary: #3b82f6;
    --color-success: #059669;
    --color-warning: #f59e0b;
    --color-error: #dc2626;
    --color-text: #333;
    --color-text-muted: #666;
    --color-border: #d1d5db;
    --color-bg-light: #f9fafb;
}
```

**components.css:**
```css
/* ==============================================
   CARD COMPONENT
   ============================================== */
.card {
    border: 1pt solid var(--color-border);
    border-radius: 5pt;
    padding: 20pt;
    margin-bottom: 20pt;
    background-color: white;
}

.card__header {
    border-bottom: 1pt solid var(--color-border);
    padding-bottom: 15pt;
    margin-bottom: 15pt;
}

.card__title {
    margin: 0;
    color: var(--color-primary);
    font-size: 18pt;
}

/* ==============================================
   ALERT COMPONENT
   ============================================== */
.alert {
    padding: 15pt;
    padding-left: 20pt;
    border-radius: 5pt;
    margin-bottom: 20pt;
}

.alert--success {
    background-color: #d1fae5;
    color: #065f46;
    border-left: 4pt solid var(--color-success);
}

.alert--error {
    background-color: #fee2e2;
    color: #991b1b;
    border-left: 4pt solid var(--color-error);
}
```

**document.html:**
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Well-Organized Document</title>
    <link rel="stylesheet" href="./styles/base.css" />
    <link rel="stylesheet" href="./styles/components.css" />
    <link rel="stylesheet" href="./styles/utilities.css" />
</head>
<body>
    <h1>Document Title</h1>

    <div class="card">
        <div class="card__header">
            <h2 class="card__title">Section Title</h2>
        </div>
        <div class="card__body">
            <p>Content goes here.</p>
        </div>
    </div>

    <div class="alert alert--success">
        <strong>Success!</strong> Operation completed.
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Extract Inline Styles

Take a document with inline styles and:
- Move styles to `<style>` element
- Create reusable classes
- Apply classes to elements

### Exercise 2: Build Component Library

Create a stylesheet with:
- Card component with variants
- Button component with different styles
- Alert component with different types
- Badge component

### Exercise 3: Multi-File Organization

Create multiple stylesheets:
- base.css (typography, colors)
- layout.css (containers, grids)
- components.css (UI components)
- Include all in a document

---

## Common Pitfalls

### ❌ Overly Specific Selectors

```css
body div.container div.content div.box p.text {
    color: blue;
}
```

✅ **Solution:** Keep selectors simple

```css
.box-text {
    color: blue;
}
```

### ❌ Mixing Organization Methods

```html
<link rel="stylesheet" href="styles.css" />
<style>
    /* More styles */
</style>
<p style="color: red;">Text</p>
```

✅ **Solution:** Choose one primary method

```html
<link rel="stylesheet" href="styles.css" />
<!-- Stick to external stylesheet -->
```

### ❌ No Organization or Comments

```css
.card { border: 1pt solid black; }
.btn { padding: 10pt; }
h1 { color: blue; }
.alert { background: yellow; }
```

✅ **Solution:** Group and comment

```css
/* ==============================================
   COMPONENTS
   ============================================== */
.card { border: 1pt solid black; }
.alert { background: yellow; }

/* ==============================================
   BUTTONS
   ============================================== */
.btn { padding: 10pt; }
```

---

## Best Practices

1. **Choose one primary method** - External for projects, embedded for single files
2. **Group related styles** - Components, utilities, layout
3. **Use meaningful class names** - Descriptive, not presentational
4. **Comment your CSS** - Explain sections and complex rules
5. **Keep specificity low** - Avoid deeply nested selectors
6. **Create reusable classes** - DRY (Don't Repeat Yourself)
7. **Maintain consistent naming** - Choose a convention and stick to it
8. **Organize logically** - Base, layout, components, utilities

---

## Next Steps

Now that you can organize styles:

1. **[Styling Best Practices](08_styling_best_practices.md)** - Professional patterns
2. **[Layout & Positioning](/learning/04-layout/)** - Advanced layout techniques
3. **[Typography & Fonts](/learning/05-typography/)** - Custom fonts and typography

---

**Continue learning →** [Styling Best Practices](08_styling_best_practices.md)
