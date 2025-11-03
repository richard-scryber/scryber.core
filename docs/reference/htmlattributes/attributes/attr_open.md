---
layout: default
title: open
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @open : The Details Open State Attribute

The `open` attribute specifies whether a `<details>` element should be expanded (open) or collapsed (closed) by default. As a boolean attribute, its presence indicates the element is open, while its absence means closed. This enables creation of expandable/collapsible content sections in PDF documents, though PDF support for interactive elements is limited.

## Usage

The `open` attribute controls disclosure widget state:
- Boolean attribute (presence = open, absence = closed)
- Used exclusively with `<details>` element
- Determines initial expanded/collapsed state
- No value required (presence is sufficient)
- In PDF context, typically renders content as expanded
- Supports data binding for dynamic state control

```html
<!-- Open/expanded by default -->
<details open>
    <summary>Click to collapse</summary>
    <p>This content is visible by default.</p>
</details>

<!-- Closed/collapsed by default -->
<details>
    <summary>Click to expand</summary>
    <p>This content is hidden by default.</p>
</details>

<!-- Boolean attribute - these are equivalent -->
<details open>Expanded</details>
<details open="">Expanded</details>
<details open="open">Expanded</details>

<!-- Dynamic open state -->
<details open="{{model.isExpanded ? 'open' : null}}">
    <summary>{{model.title}}</summary>
    <p>{{model.content}}</p>
</details>
```

---

## Supported Elements

The `open` attribute is used exclusively with:

### Details Element
- `<details>` - Disclosure widget (only element that uses open)

---

## Binding Values

The `open` attribute supports data binding for dynamic state control:

```html
<!-- Dynamic open state from model -->
<details open="{{model.expanded ? 'open' : null}}">
    <summary>Section Title</summary>
    <p>Section content</p>
</details>

<!-- Conditional open -->
<details open="{{model.showByDefault ? 'open' : null}}">
    <summary>{{model.heading}}</summary>
    <div>{{model.body}}</div>
</details>

<!-- Repeating details with different states -->
<template data-bind="{{model.sections}}">
    <details open="{{.isOpen ? 'open' : null}}">
        <summary>{{.title}}</summary>
        <div>{{.content}}</div>
    </details>
</template>

<!-- Open first section, close others -->
<template data-bind="{{model.faqs}}">
    <details open="{{.index === 0 ? 'open' : null}}">
        <summary>{{.question}}</summary>
        <p>{{.answer}}</p>
    </details>
</template>
```

**Data Model Example:**
```json
{
  "expanded": true,
  "showByDefault": true,
  "heading": "Important Information",
  "body": "Content details...",
  "sections": [
    {
      "title": "Section 1",
      "content": "First section content",
      "isOpen": true
    },
    {
      "title": "Section 2",
      "content": "Second section content",
      "isOpen": false
    }
  ],
  "faqs": [
    {
      "index": 0,
      "question": "What is this?",
      "answer": "This is the answer."
    },
    {
      "index": 1,
      "question": "How does it work?",
      "answer": "It works like this."
    }
  ]
}
```

---

## Notes

### Boolean Attribute

The `open` attribute is a boolean attribute:

```html
<!-- These all mean "open" -->
<details open>Expanded</details>
<details open="">Expanded</details>
<details open="open">Expanded</details>
<details open="true">Expanded</details>

<!-- These all mean "closed" -->
<details>Collapsed</details>
<!-- Omitting the attribute = closed -->

<!-- WRONG: Cannot set to false -->
<details open="false">Still treated as open!</details>
```

**Important:** To close a details element, **omit the attribute entirely**. Setting `open="false"` still counts as having the attribute and will be treated as open.

### Details Element Structure

The `<details>` element works with `<summary>`:

```html
<details open>
    <!-- Summary is the visible heading/toggle -->
    <summary>Click to toggle</summary>

    <!-- Content shown when open -->
    <p>This is the expandable content.</p>
    <p>Multiple elements can be included.</p>
</details>
```

Components:
- `<details>` - Container for the disclosure widget
- `<summary>` - Heading/label (always visible)
- Content - Additional elements (visible when open)

### Default State

Without the `open` attribute:

```html
<!-- Closed by default -->
<details>
    <summary>Expand for more</summary>
    <p>Hidden content</p>
</details>

<!-- Equivalent to: -->
<details open="null">
    <summary>Expand for more</summary>
    <p>Hidden content</p>
</details>
```

### PDF Context Limitations

In PDF documents:
1. **Interactive toggles** are not typically supported
2. **Open state** usually determines static rendering
3. **Content with open** is rendered as expanded
4. **Content without open** may be rendered collapsed or expanded depending on PDF generator

```html
<!-- In PDF: Usually renders expanded -->
<details open>
    <summary>Important Information</summary>
    <p>This content is visible in the PDF.</p>
</details>

<!-- In PDF: May render collapsed or expanded -->
<details>
    <summary>Additional Details</summary>
    <p>Rendering depends on PDF generator settings.</p>
</details>
```

**Best Practice for PDFs:**
- Use `open` for important content that should always be visible
- Omit `open` for supplementary content (may be omitted or included)
- Test rendering in your PDF generator

### Summary Element

The `<summary>` element is the visible label:

```html
<details open>
    <!-- Summary is always visible -->
    <summary>Chapter 1: Introduction</summary>

    <!-- Content visible only when open -->
    <p>
        This chapter introduces the fundamental concepts
        that will be explored throughout the document.
    </p>
</details>
```

If `<summary>` is omitted, browsers typically add default text like "Details":

```html
<!-- Without summary -->
<details open>
    <p>Content without explicit summary</p>
</details>

<!-- Browser may render as: -->
<!-- Details -->
<!-- Content without explicit summary -->
```

**Best Practice:** Always include `<summary>` for clarity.

### Nesting Details

Details elements can be nested:

```html
<details open>
    <summary>Parent Section</summary>

    <p>Parent content</p>

    <!-- Nested details (closed by default) -->
    <details>
        <summary>Subsection A</summary>
        <p>Subsection A content</p>
    </details>

    <details>
        <summary>Subsection B</summary>
        <p>Subsection B content</p>
    </details>
</details>
```

### Styling Details

CSS can style details elements:

```html
<style>
    details {
        border: 1pt solid #ccc;
        padding: 10pt;
        margin: 10pt 0;
        border-radius: 5pt;
    }

    summary {
        font-weight: bold;
        cursor: pointer;
        padding: 5pt;
        background-color: #f8f9fa;
        margin: -10pt -10pt 10pt -10pt;
        padding: 10pt;
    }

    /* Style for open state */
    details[open] {
        background-color: #f0f8ff;
    }

    details[open] summary {
        background-color: #e3f2fd;
        border-bottom: 1pt solid #ccc;
    }
</style>

<details open>
    <summary>Styled Details Element</summary>
    <p>Content with custom styling</p>
</details>
```

### Accessibility

Proper use improves accessibility:

```html
<!-- Good: Clear summary text -->
<details>
    <summary>What are the system requirements?</summary>
    <ul>
        <li>Operating System: Windows 10 or later</li>
        <li>RAM: 4GB minimum, 8GB recommended</li>
        <li>Storage: 500MB available space</li>
    </ul>
</details>

<!-- Good: Descriptive summary -->
<details open>
    <summary>Important Safety Information</summary>
    <p>Read this before operating the equipment...</p>
</details>
```

Screen readers announce:
- The summary text
- The expanded/collapsed state
- How to toggle (if interactive)

### JavaScript Interaction (Web)

In web browsers (not PDFs), details can be controlled via JavaScript:

```html
<details id="myDetails">
    <summary>Toggle Me</summary>
    <p>Content</p>
</details>

<script>
    // Get element
    const details = document.getElementById('myDetails');

    // Open programmatically
    details.open = true;

    // Close programmatically
    details.open = false;

    // Toggle
    details.open = !details.open;

    // Listen for toggle event
    details.addEventListener('toggle', function() {
        console.log('Details toggled:', this.open);
    });
</script>
```

**Note:** This JavaScript functionality is for web browsers only and does not apply to static PDFs.

### Use Cases

Common uses for details elements:

1. **FAQ sections** - Collapsible question/answer pairs
2. **Additional information** - Optional supplementary content
3. **Long content** - Breaking up lengthy documents
4. **Progressive disclosure** - Revealing information as needed
5. **Footnotes** - Expandable annotations

```html
<!-- FAQ -->
<details>
    <summary>What is your return policy?</summary>
    <p>We accept returns within 30 days of purchase...</p>
</details>

<!-- Additional info -->
<details>
    <summary>Technical specifications</summary>
    <table>
        <!-- Detailed specs -->
    </table>
</details>

<!-- Footnote -->
<p>
    Important concept<sup>1</sup>
</p>

<details>
    <summary>Footnote 1</summary>
    <p>Additional context and references...</p>
</details>
```

---

## Examples

### Basic Open and Closed Details

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Details Example</title>
    <style>
        details {
            border: 1pt solid #ccc;
            padding: 10pt;
            margin: 10pt 0;
        }
        summary {
            font-weight: bold;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <h1>Product Information</h1>

    <!-- Open by default -->
    <details open>
        <summary>Description (expanded by default)</summary>
        <p>
            This product features advanced technology and premium
            materials for exceptional performance.
        </p>
    </details>

    <!-- Closed by default -->
    <details>
        <summary>Technical Specifications (collapsed by default)</summary>
        <ul>
            <li>Dimensions: 10 x 5 x 3 inches</li>
            <li>Weight: 2.5 lbs</li>
            <li>Material: Aluminum alloy</li>
            <li>Color: Matte black</li>
        </ul>
    </details>

    <!-- Closed by default -->
    <details>
        <summary>Shipping Information (collapsed by default)</summary>
        <p>
            Free shipping on orders over $50.
            Standard delivery: 5-7 business days.
            Express delivery: 2-3 business days.
        </p>
    </details>
</body>
</html>
```

### FAQ Section

```html
<section>
    <h1>Frequently Asked Questions</h1>

    <!-- First FAQ open, others closed -->
    <details open>
        <summary>What is Scryber?</summary>
        <p>
            Scryber is a .NET library for generating PDF documents from
            HTML and CSS templates.
        </p>
    </details>

    <details>
        <summary>How do I install Scryber?</summary>
        <p>
            Install via NuGet Package Manager:
        </p>
        <pre><code>Install-Package Scryber.Core</code></pre>
    </details>

    <details>
        <summary>Is Scryber free to use?</summary>
        <p>
            Yes, Scryber.Core is open source and free to use under
            the LGPL-3.0 license.
        </p>
    </details>

    <details>
        <summary>What HTML elements are supported?</summary>
        <p>
            Scryber supports most standard HTML5 elements including
            divs, spans, tables, lists, images, and semantic elements.
        </p>
    </details>

    <details>
        <summary>Can I use CSS for styling?</summary>
        <p>
            Yes, Scryber supports CSS for styling including external
            stylesheets, inline styles, and embedded style blocks.
        </p>
    </details>
</section>
```

### Styled Details Elements

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Styled Details</title>
    <style>
        .info-section {
            border: 2pt solid #336699;
            border-radius: 5pt;
            margin: 15pt 0;
            overflow: hidden;
        }

        .info-section summary {
            background-color: #336699;
            color: white;
            padding: 15pt;
            font-size: 12pt;
            font-weight: bold;
            cursor: pointer;
        }

        .info-section[open] summary {
            background-color: #4a7ba7;
        }

        .info-section > *:not(summary) {
            padding: 15pt;
        }

        .warning-section {
            border: 2pt solid #ffc107;
            border-left-width: 5pt;
            padding: 10pt;
            margin: 15pt 0;
            background-color: #fff3cd;
        }

        .warning-section summary {
            color: #856404;
            font-weight: bold;
            font-size: 11pt;
        }
    </style>
</head>
<body>
    <h1>Important Information</h1>

    <!-- Blue styled section (open) -->
    <details class="info-section" open>
        <summary>Getting Started</summary>
        <div>
            <p>Follow these steps to get started:</p>
            <ol>
                <li>Install the required software</li>
                <li>Configure your environment</li>
                <li>Run the setup wizard</li>
            </ol>
        </div>
    </details>

    <!-- Warning styled section (closed) -->
    <details class="warning-section">
        <summary>⚠ Important Safety Notice</summary>
        <p>
            Always disconnect power before performing maintenance.
            Failure to do so may result in electric shock or injury.
        </p>
    </details>

    <!-- Blue styled section (closed) -->
    <details class="info-section">
        <summary>Advanced Configuration</summary>
        <div>
            <p>Advanced users can customize additional settings...</p>
        </div>
    </details>
</body>
</html>
```

### Nested Details

```html
<section>
    <h1>Product Documentation</h1>

    <!-- Parent details (open) -->
    <details open style="border: 2pt solid #336699; padding: 15pt; margin: 10pt 0;">
        <summary style="font-size: 14pt; font-weight: bold; margin-bottom: 10pt;">
            Chapter 1: Getting Started
        </summary>

        <p>This chapter covers the basics of using the product.</p>

        <!-- Nested child details (closed) -->
        <details style="border: 1pt solid #ccc; padding: 10pt; margin: 10pt 0;">
            <summary style="font-weight: bold;">1.1 Installation</summary>
            <p>Step-by-step installation instructions...</p>
            <ol>
                <li>Download the installer</li>
                <li>Run setup.exe</li>
                <li>Follow the wizard</li>
            </ol>
        </details>

        <!-- Nested child details (open) -->
        <details open style="border: 1pt solid #ccc; padding: 10pt; margin: 10pt 0;">
            <summary style="font-weight: bold;">1.2 First Steps</summary>
            <p>Getting started with your first project...</p>

            <!-- Deeply nested details (closed) -->
            <details style="border: 1pt solid #ddd; padding: 5pt; margin: 5pt 0;">
                <summary>1.2.1 Creating a Project</summary>
                <p>Click File → New → Project...</p>
            </details>

            <details style="border: 1pt solid #ddd; padding: 5pt; margin: 5pt 0;">
                <summary>1.2.2 Basic Configuration</summary>
                <p>Configure your project settings...</p>
            </details>
        </details>

        <!-- Nested child details (closed) -->
        <details style="border: 1pt solid #ccc; padding: 10pt; margin: 10pt 0;">
            <summary style="font-weight: bold;">1.3 Quick Tutorial</summary>
            <p>A quick 5-minute tutorial...</p>
        </details>
    </details>

    <!-- Parent details (closed) -->
    <details style="border: 2pt solid #336699; padding: 15pt; margin: 10pt 0;">
        <summary style="font-size: 14pt; font-weight: bold;">
            Chapter 2: Advanced Features
        </summary>
        <p>Advanced topics for experienced users...</p>
    </details>
</section>
```

### Data-Bound FAQ

```html
<!-- Model: {
    faqs: [
        {
            question: "What payment methods do you accept?",
            answer: "We accept all major credit cards, PayPal, and bank transfers.",
            defaultOpen: true
        },
        {
            question: "How long does shipping take?",
            answer: "Standard shipping takes 5-7 business days. Express shipping takes 2-3 days.",
            defaultOpen: false
        },
        {
            question: "What is your return policy?",
            answer: "We accept returns within 30 days of purchase with original packaging.",
            defaultOpen: false
        }
    ]
} -->

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>FAQ</title>
    <style>
        details {
            border: 1pt solid #ccc;
            padding: 15pt;
            margin: 10pt 0;
            border-radius: 5pt;
        }
        summary {
            font-weight: bold;
            font-size: 11pt;
            cursor: pointer;
            color: #336699;
        }
        details[open] {
            background-color: #f8f9fa;
        }
    </style>
</head>
<body>
    <h1>Frequently Asked Questions</h1>

    <template data-bind="{{model.faqs}}">
        <details open="{{.defaultOpen ? 'open' : null}}">
            <summary>{{.question}}</summary>
            <p>{{.answer}}</p>
        </details>
    </template>
</body>
</html>
```

### Technical Documentation

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>API Documentation</title>
    <style>
        .api-method {
            border: 1pt solid #ccc;
            margin: 15pt 0;
            border-radius: 3pt;
        }
        .api-method summary {
            background-color: #f8f9fa;
            padding: 10pt;
            font-family: 'Courier New', monospace;
            font-weight: bold;
        }
        .api-method[open] summary {
            background-color: #e9ecef;
            border-bottom: 1pt solid #ccc;
        }
        .api-method > div {
            padding: 15pt;
        }
        .http-method {
            display: inline-block;
            padding: 3pt 8pt;
            border-radius: 3pt;
            font-size: 9pt;
            font-weight: bold;
            margin-right: 5pt;
        }
        .get { background-color: #28a745; color: white; }
        .post { background-color: #007bff; color: white; }
        .delete { background-color: #dc3545; color: white; }
    </style>
</head>
<body>
    <h1>REST API Documentation</h1>

    <h2>User Endpoints</h2>

    <!-- GET endpoint (open) -->
    <details class="api-method" open>
        <summary>
            <span class="http-method get">GET</span>
            /api/users
        </summary>
        <div>
            <p><strong>Description:</strong> Retrieve all users</p>

            <p><strong>Response:</strong></p>
            <pre><code>{
  "users": [
    { "id": 1, "name": "John Doe" },
    { "id": 2, "name": "Jane Smith" }
  ]
}</code></pre>
        </div>
    </details>

    <!-- POST endpoint (closed) -->
    <details class="api-method">
        <summary>
            <span class="http-method post">POST</span>
            /api/users
        </summary>
        <div>
            <p><strong>Description:</strong> Create a new user</p>

            <p><strong>Request Body:</strong></p>
            <pre><code>{
  "name": "New User",
  "email": "user@example.com"
}</code></pre>

            <p><strong>Response:</strong></p>
            <pre><code>{
  "id": 3,
  "name": "New User",
  "email": "user@example.com"
}</code></pre>
        </div>
    </details>

    <!-- DELETE endpoint (closed) -->
    <details class="api-method">
        <summary>
            <span class="http-method delete">DELETE</span>
            /api/users/:id
        </summary>
        <div>
            <p><strong>Description:</strong> Delete a user by ID</p>

            <p><strong>Parameters:</strong></p>
            <ul>
                <li><code>id</code> (required): User ID</li>
            </ul>

            <p><strong>Response:</strong></p>
            <pre><code>{
  "success": true,
  "message": "User deleted"
}</code></pre>
        </div>
    </details>
</body>
</html>
```

### Expandable Sections in Report

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Annual Report</title>
    <style>
        .report-section {
            border: 2pt solid #336699;
            margin: 20pt 0;
            border-radius: 5pt;
        }
        .report-section summary {
            background-color: #336699;
            color: white;
            padding: 15pt;
            font-size: 14pt;
            font-weight: bold;
        }
        .report-section > div {
            padding: 20pt;
        }
    </style>
</head>
<body>
    <h1>Annual Report 2025</h1>

    <p><em>Click section headings to expand/collapse content.</em></p>

    <!-- Executive Summary (open) -->
    <details class="report-section" open>
        <summary>Executive Summary</summary>
        <div>
            <p>
                Fiscal year 2025 demonstrated strong performance across
                all business units with 25% revenue growth and market
                expansion into three new regions.
            </p>
            <ul>
                <li>Total Revenue: $50M (+25% YoY)</li>
                <li>Net Profit: $10M (+30% YoY)</li>
                <li>Customer Base: 10,000 (+40% YoY)</li>
            </ul>
        </div>
    </details>

    <!-- Financial Performance (closed) -->
    <details class="report-section">
        <summary>Financial Performance</summary>
        <div>
            <h3>Revenue Breakdown</h3>
            <table style="width: 100%; border-collapse: collapse;">
                <tr>
                    <th style="border: 1pt solid #ddd; padding: 8pt;">Category</th>
                    <th style="border: 1pt solid #ddd; padding: 8pt;">Revenue</th>
                    <th style="border: 1pt solid #ddd; padding: 8pt;">Growth</th>
                </tr>
                <tr>
                    <td style="border: 1pt solid #ddd; padding: 8pt;">Product Sales</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;">$30M</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;">+20%</td>
                </tr>
                <tr>
                    <td style="border: 1pt solid #ddd; padding: 8pt;">Services</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;">$20M</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;">+35%</td>
                </tr>
            </table>
        </div>
    </details>

    <!-- Operations (closed) -->
    <details class="report-section">
        <summary>Operations</summary>
        <div>
            <p>
                Operational efficiency improved through automation
                initiatives and process optimization...
            </p>
        </div>
    </details>

    <!-- Future Outlook (closed) -->
    <details class="report-section">
        <summary>Future Outlook</summary>
        <div>
            <p>
                Looking ahead to 2026, we anticipate continued growth
                driven by product innovation and market expansion...
            </p>
        </div>
    </details>
</body>
</html>
```

### Product Comparison with Expandable Details

```html
<section>
    <h1>Product Comparison</h1>

    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr>
                <th style="border: 1pt solid #ddd; padding: 10pt;">Feature</th>
                <th style="border: 1pt solid #ddd; padding: 10pt;">Basic</th>
                <th style="border: 1pt solid #ddd; padding: 10pt;">Pro</th>
                <th style="border: 1pt solid #ddd; padding: 10pt;">Enterprise</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 10pt;">Price</td>
                <td style="border: 1pt solid #ddd; padding: 10pt;">$9.99/mo</td>
                <td style="border: 1pt solid #ddd; padding: 10pt;">$29.99/mo</td>
                <td style="border: 1pt solid #ddd; padding: 10pt;">$99.99/mo</td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 10pt;">Storage</td>
                <td style="border: 1pt solid #ddd; padding: 10pt;">10 GB</td>
                <td style="border: 1pt solid #ddd; padding: 10pt;">100 GB</td>
                <td style="border: 1pt solid #ddd; padding: 10pt;">Unlimited</td>
            </tr>
        </tbody>
    </table>

    <h2>Detailed Feature Comparison</h2>

    <!-- Basic plan details -->
    <details style="border: 1pt solid #ccc; padding: 10pt; margin: 10pt 0;">
        <summary style="font-weight: bold;">Basic Plan Details</summary>
        <ul>
            <li>10 GB storage</li>
            <li>1 user</li>
            <li>Email support</li>
            <li>Basic features</li>
        </ul>
    </details>

    <!-- Pro plan details (open) -->
    <details open style="border: 1pt solid #ccc; padding: 10pt; margin: 10pt 0;">
        <summary style="font-weight: bold;">Pro Plan Details (Most Popular)</summary>
        <ul>
            <li>100 GB storage</li>
            <li>Up to 5 users</li>
            <li>Email + chat support</li>
            <li>Advanced features</li>
            <li>Priority support</li>
        </ul>
    </details>

    <!-- Enterprise plan details -->
    <details style="border: 1pt solid #ccc; padding: 10pt; margin: 10pt 0;">
        <summary style="font-weight: bold;">Enterprise Plan Details</summary>
        <ul>
            <li>Unlimited storage</li>
            <li>Unlimited users</li>
            <li>24/7 phone support</li>
            <li>All features</li>
            <li>Dedicated account manager</li>
            <li>Custom integrations</li>
            <li>SLA guarantee</li>
        </ul>
    </details>
</section>
```

### Troubleshooting Guide

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Troubleshooting Guide</title>
    <style>
        .problem {
            border-left: 4pt solid #dc3545;
            padding: 10pt;
            margin: 15pt 0;
            background-color: #f8d7da;
        }
        .problem summary {
            font-weight: bold;
            color: #721c24;
        }
        .solution {
            margin-top: 10pt;
            padding: 10pt;
            background-color: white;
            border: 1pt solid #ccc;
        }
    </style>
</head>
<body>
    <h1>Troubleshooting Guide</h1>

    <p>Click on a problem to see the solution.</p>

    <details class="problem" open>
        <summary>Problem: Application won't start</summary>
        <div class="solution">
            <p><strong>Solution:</strong></p>
            <ol>
                <li>Verify system requirements are met</li>
                <li>Check for conflicting software</li>
                <li>Reinstall the application</li>
                <li>Contact support if issue persists</li>
            </ol>
        </div>
    </details>

    <details class="problem">
        <summary>Problem: Slow performance</summary>
        <div class="solution">
            <p><strong>Solution:</strong></p>
            <ol>
                <li>Close unnecessary background applications</li>
                <li>Clear application cache</li>
                <li>Increase available memory</li>
                <li>Check for software updates</li>
            </ol>
        </div>
    </details>

    <details class="problem">
        <summary>Problem: Error message on startup</summary>
        <div class="solution">
            <p><strong>Solution:</strong></p>
            <ol>
                <li>Note the exact error message</li>
                <li>Check the error log in /logs/error.log</li>
                <li>Search our knowledge base</li>
                <li>Contact support with error details</li>
            </ol>
        </div>
    </details>
</body>
</html>
```

---

## See Also

- [details](/reference/htmltags/details.html) - Details disclosure element
- [summary](/reference/htmltags/summary.html) - Summary element for details
- [hidden](/reference/htmlattributes/hidden.html) - Hidden attribute for visibility
- [Collapsible Content](/reference/techniques/collapsible.html) - Creating collapsible sections
- [Interactive Elements](/reference/interactive/) - Interactive HTML elements

---
