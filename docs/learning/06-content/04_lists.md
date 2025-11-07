---
layout: default
title: Lists
nav_order: 4
parent: Content Components
parent_url: /learning/06-content/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Lists

Master ordered lists, unordered lists, definition lists, styling, nesting, and data binding to create structured content in PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Create ordered and unordered lists
- Customize list markers and styles
- Nest lists for hierarchical content
- Use definition lists
- Style lists with CSS
- Bind data to generate dynamic lists
- Create professional list layouts

---

## Unordered Lists

### Basic Unordered List

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Unordered List</title>
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
    </style>
</head>
<body>
    <h1>Product Features</h1>

    <ul>
        <li>High performance processing</li>
        <li>Energy efficient design</li>
        <li>Wireless connectivity</li>
        <li>Two-year warranty</li>
    </ul>
</body>
</html>
```

### Marker Styles

```css
/* Disc (default) */
ul.disc {
    list-style-type: disc;
}

/* Circle */
ul.circle {
    list-style-type: circle;
}

/* Square */
ul.square {
    list-style-type: square;
}

/* None (remove markers) */
ul.none {
    list-style-type: none;
}
```

```html
<ul class="disc">
    <li>Disc marker</li>
    <li>Default style</li>
</ul>

<ul class="circle">
    <li>Circle marker</li>
    <li>Hollow circle</li>
</ul>

<ul class="square">
    <li>Square marker</li>
    <li>Filled square</li>
</ul>
```

---

## Ordered Lists

### Basic Ordered List

```html
<h1>Installation Steps</h1>

<ol>
    <li>Download the software installer</li>
    <li>Run the installation wizard</li>
    <li>Accept the license agreement</li>
    <li>Choose installation directory</li>
    <li>Complete the installation</li>
</ol>
```

### Numbering Styles

```css
/* Decimal numbers (default) */
ol.decimal {
    list-style-type: decimal;
}

/* Leading zeros */
ol.decimal-zero {
    list-style-type: decimal-leading-zero;
}

/* Lowercase letters */
ol.lower-alpha {
    list-style-type: lower-alpha;
}

/* Uppercase letters */
ol.upper-alpha {
    list-style-type: upper-alpha;
}

/* Lowercase Roman numerals */
ol.lower-roman {
    list-style-type: lower-roman;
}

/* Uppercase Roman numerals */
ol.upper-roman {
    list-style-type: upper-roman;
}
```

```html
<ol class="decimal">
    <li>First item</li>
    <li>Second item</li>
</ol>

<ol class="upper-alpha">
    <li>Item A</li>
    <li>Item B</li>
</ol>

<ol class="lower-roman">
    <li>Item i</li>
    <li>Item ii</li>
</ol>
```

### Starting Number

```html
<!-- Start at specific number -->
<ol start="5">
    <li>This is item 5</li>
    <li>This is item 6</li>
    <li>This is item 7</li>
</ol>

<!-- Reverse ordering -->
<ol reversed>
    <li>Third place</li>
    <li>Second place</li>
    <li>First place</li>
</ol>
```

---

## Nested Lists

### Nested Unordered Lists

```html
<ul>
    <li>Web Technologies
        <ul>
            <li>HTML</li>
            <li>CSS</li>
            <li>JavaScript</li>
        </ul>
    </li>
    <li>Backend Languages
        <ul>
            <li>Python</li>
            <li>Java</li>
            <li>C#</li>
        </ul>
    </li>
</ul>
```

### Nested Ordered Lists

```html
<ol>
    <li>Project Planning
        <ol>
            <li>Define objectives</li>
            <li>Identify stakeholders</li>
            <li>Set timeline</li>
        </ol>
    </li>
    <li>Execution
        <ol>
            <li>Assign tasks</li>
            <li>Monitor progress</li>
            <li>Address issues</li>
        </ol>
    </li>
</ol>
```

### Mixed Nesting

```html
<ol>
    <li>Hardware Requirements
        <ul>
            <li>CPU: 2.0 GHz or faster</li>
            <li>RAM: 8GB minimum</li>
            <li>Storage: 256GB SSD</li>
        </ul>
    </li>
    <li>Software Requirements
        <ul>
            <li>Operating System: Windows 10+</li>
            <li>Browser: Chrome, Firefox, or Edge</li>
        </ul>
    </li>
</ol>
```

---

## Definition Lists

### Basic Definition List

```html
<dl>
    <dt>HTML</dt>
    <dd>HyperText Markup Language - the standard markup language for web pages</dd>

    <dt>CSS</dt>
    <dd>Cascading Style Sheets - used for styling and layout</dd>

    <dt>PDF</dt>
    <dd>Portable Document Format - a file format for documents</dd>
</dl>
```

### Styled Definition List

```css
dl {
    margin: 20pt 0;
}

dt {
    font-weight: 700;
    color: #1e40af;
    margin-top: 15pt;
    margin-bottom: 5pt;
}

dd {
    margin-left: 20pt;
    color: #666;
}
```

---

## List Styling

### Spacing and Indentation

```css
ul, ol {
    margin: 15pt 0;              /* Vertical spacing */
    padding-left: 30pt;          /* Indentation */
}

li {
    margin-bottom: 8pt;          /* Space between items */
    line-height: 1.6;            /* Line height */
}

/* Compact lists */
ul.compact li {
    margin-bottom: 3pt;
}
```

### Custom Markers with CSS

```css
/* Custom marker color */
ul {
    color: #2563eb;  /* Marker color */
}

ul li {
    color: #000;  /* Text color */
}

/* Position markers inside */
ul.inside {
    list-style-position: inside;
}

/* Position markers outside (default) */
ul.outside {
    list-style-position: outside;
}
```

### Using Custom Bullets

```css
ul.custom {
    list-style: none;
    padding-left: 0;
}

ul.custom li::before {
    content: "→ ";
    color: #2563eb;
    font-weight: bold;
    margin-right: 8pt;
}
```

---

## Data Binding

### Dynamic List Generation

{% raw %}
```html
<h1>Product Catalog</h1>

<ul>
    {{#each products}}
    <li>{{this.name}} - ${{this.price}}</li>
    {{/each}}
</ul>
```
{% endraw %}

### Conditional List Items

{% raw %}
```html
<h2>Available Features</h2>

<ul>
    {{#if features.wifi}}
    <li>WiFi Connectivity</li>
    {{/if}}
    {{#if features.bluetooth}}
    <li>Bluetooth Support</li>
    {{/if}}
    {{#if features.gps}}
    <li>GPS Navigation</li>
    {{/if}}
</ul>
```
{% endraw %}

### Nested Dynamic Lists

{% raw %}
```html
<h1>Department Structure</h1>

<ul>
    {{#each departments}}
    <li>
        {{this.name}}
        <ul>
            {{#each this.employees}}
            <li>{{this.name}} - {{this.title}}</li>
            {{/each}}
        </ul>
    </li>
    {{/each}}
</ul>
```
{% endraw %}

---

## Practical Examples

### Example 1: Feature Comparison List

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Feature Comparison</title>
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
            margin: 0 0 30pt 0;
            color: #1e40af;
        }

        h2 {
            font-size: 18pt;
            margin: 25pt 0 15pt 0;
            color: #2563eb;
        }

        /* ==============================================
           COMPARISON LISTS
           ============================================== */
        .comparison {
            display: table;
            width: 100%;
            margin: 20pt 0;
        }

        .comparison-column {
            display: table-cell;
            width: 50%;
            padding: 20pt;
            vertical-align: top;
            border: 2pt solid #e5e7eb;
            border-radius: 5pt;
        }

        .comparison-column + .comparison-column {
            border-left: none;
        }

        .plan-name {
            font-size: 16pt;
            font-weight: 700;
            margin: 0 0 15pt 0;
            color: #1e40af;
        }

        .feature-list {
            list-style: none;
            padding-left: 0;
            margin: 0;
        }

        .feature-list li {
            padding: 8pt 0 8pt 30pt;
            position: relative;
            border-bottom: 1pt solid #f3f4f6;
        }

        .feature-list li::before {
            content: "✓";
            position: absolute;
            left: 0;
            color: #10b981;
            font-weight: bold;
            font-size: 14pt;
        }

        .feature-unavailable {
            color: #9ca3af;
        }

        .feature-unavailable::before {
            content: "✗";
            color: #ef4444;
        }

        .price {
            font-size: 20pt;
            font-weight: 700;
            color: #2563eb;
            margin-top: 15pt;
        }
    </style>
</head>
<body>
    <h1>Pricing Plans</h1>

    <div class="comparison">
        <!-- Basic Plan -->
        <div class="comparison-column">
            <p class="plan-name">Basic Plan</p>

            <ul class="feature-list">
                <li>5 GB Storage</li>
                <li>Email Support</li>
                <li>Mobile App Access</li>
                <li class="feature-unavailable">Priority Support</li>
                <li class="feature-unavailable">Advanced Analytics</li>
                <li class="feature-unavailable">Custom Branding</li>
            </ul>

            <p class="price">$9.99/month</p>
        </div>

        <!-- Professional Plan -->
        <div class="comparison-column">
            <p class="plan-name">Professional Plan</p>

            <ul class="feature-list">
                <li>50 GB Storage</li>
                <li>24/7 Phone Support</li>
                <li>Mobile App Access</li>
                <li>Priority Support</li>
                <li>Advanced Analytics</li>
                <li>Custom Branding</li>
            </ul>

            <p class="price">$29.99/month</p>
        </div>
    </div>
</body>
</html>
```
{% endraw %}

### Example 2: Table of Contents

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Table of Contents</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Georgia, "Times New Roman", serif;
            font-size: 11pt;
            margin: 0;
        }

        h1 {
            font-size: 28pt;
            text-align: center;
            margin: 0 0 40pt 0;
            color: #1e40af;
        }

        /* ==============================================
           TABLE OF CONTENTS
           ============================================== */
        .toc {
            list-style: none;
            padding-left: 0;
            counter-reset: chapter;
        }

        .toc > li {
            counter-increment: chapter;
            margin-bottom: 20pt;
        }

        .toc > li::before {
            content: counter(chapter) ". ";
            font-weight: 700;
            color: #2563eb;
            font-size: 14pt;
        }

        .chapter-title {
            font-size: 14pt;
            font-weight: 700;
            color: #1e40af;
            margin-bottom: 8pt;
        }

        /* ==============================================
           NESTED SECTIONS
           ============================================== */
        .sections {
            list-style: none;
            padding-left: 20pt;
            counter-reset: section;
            margin-top: 8pt;
        }

        .sections li {
            counter-increment: section;
            margin-bottom: 5pt;
            font-size: 10pt;
        }

        .sections li::before {
            content: counter(chapter) "." counter(section) " ";
            color: #666;
        }

        /* ==============================================
           PAGE NUMBERS
           ============================================== */
        .page-number {
            float: right;
            color: #666;
        }
    </style>
</head>
<body>
    <h1>Table of Contents</h1>

    <ol class="toc">
        <li>
            <div class="chapter-title">
                Introduction to PDF Generation
                <span class="page-number">1</span>
            </div>
            <ul class="sections">
                <li>What is PDF?</li>
                <li>Benefits of Programmatic PDF Creation</li>
                <li>Getting Started</li>
            </ul>
        </li>

        <li>
            <div class="chapter-title">
                Working with Content
                <span class="page-number">15</span>
            </div>
            <ul class="sections">
                <li>Text and Typography</li>
                <li>Images and Graphics</li>
                <li>Tables and Lists</li>
            </ul>
        </li>

        <li>
            <div class="chapter-title">
                Advanced Features
                <span class="page-number">42</span>
            </div>
            <ul class="sections">
                <li>Dynamic Content</li>
                <li>Data Binding</li>
                <li>Custom Styling</li>
            </ul>
        </li>

        <li>
            <div class="chapter-title">
                Best Practices
                <span class="page-number">78</span>
            </div>
            <ul class="sections">
                <li>Performance Optimization</li>
                <li>Accessibility</li>
                <li>Production Deployment</li>
            </ul>
        </li>
    </ol>
</body>
</html>
```
{% endraw %}

---

## Try It Yourself

### Exercise 1: Checklist

Create a task checklist with:
- Custom checkbox markers (☐/☑)
- Completed items styled differently
- Progress indicator at top
- Data binding for tasks

### Exercise 2: Multi-Level Outline

Build a document outline with:
- 3 levels of nesting (Chapter → Section → Topic)
- Different numbering styles per level
- Proper indentation
- Page numbers

### Exercise 3: Feature Grid

Design a feature comparison:
- 3 product tiers side by side
- Check/X marks for features
- Highlighted differences
- Pricing at bottom

---

## Common Pitfalls

### ❌ Excessive Nesting

```html
<!-- Too many levels, hard to follow -->
<ul>
    <li>Level 1
        <ul>
            <li>Level 2
                <ul>
                    <li>Level 3
                        <ul>
                            <li>Level 4</li>
                        </ul>
                    </li>
                </ul>
            </li>
        </ul>
    </li>
</ul>
```

✅ **Solution:**

```html
<!-- Limit to 2-3 levels -->
<ul>
    <li>Level 1
        <ul>
            <li>Level 2</li>
            <li>Level 2</li>
        </ul>
    </li>
</ul>
```

### ❌ No Spacing Between Items

```css
li {
    margin-bottom: 0;  /* Too cramped */
}
```

✅ **Solution:**

```css
li {
    margin-bottom: 8pt;  /* Comfortable spacing */
}
```

### ❌ Inconsistent List Styles

```html
<!-- Mixed styles in same context -->
<ul class="disc">
    <li>Item 1</li>
</ul>
<ul class="square">
    <li>Item 2</li>
</ul>
<ul class="circle">
    <li>Item 3</li>
</ul>
```

✅ **Solution:**

```html
<!-- Consistent style throughout -->
<ul class="disc">
    <li>Item 1</li>
    <li>Item 2</li>
    <li>Item 3</li>
</ul>
```

---

## List Styling Checklist

- [ ] Appropriate list type chosen (ordered/unordered)
- [ ] Sufficient spacing between items
- [ ] Proper indentation for nested lists
- [ ] Consistent marker styles
- [ ] Line height set for readability
- [ ] Margin around list blocks
- [ ] Custom styles tested in PDF
- [ ] Data binding working correctly

---

## Best Practices

1. **Choose Appropriate Type** - Ordered for sequences, unordered for collections
2. **Limit Nesting** - 2-3 levels maximum
3. **Consistent Spacing** - 8-12pt between items
4. **Clear Hierarchy** - Use different marker styles for levels
5. **Proper Indentation** - 20-30pt per level
6. **Data Binding** - Generate lists dynamically
7. **Test with Data** - Varying list lengths
8. **Semantic HTML** - Use correct list elements

---

## Next Steps

1. **[Tables - Basics](05_tables_basics.md)** - Structured tabular data
2. **[Tables - Advanced](06_tables_advanced.md)** - Dynamic tables
3. **[Content Best Practices](08_content_best_practices.md)** - Optimization

---

**Continue learning →** [Tables - Basics](05_tables_basics.md)
