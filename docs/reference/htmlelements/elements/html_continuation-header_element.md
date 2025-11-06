---
layout: default
title: continuation-header
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;continuation-header&gt; : The Continuation Header Element
{: .no_toc }
---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---


## Summary

The `<continuation-header>` element defines content that appears at the top of continuation pages when content spans multiple pages. The regular `<header>` element will appear on all pages, unless the continuation header is present, and it will appear on second and subsequent pages.


It is used within `<body>` or `<section>` elements to provide a header that displays on overflow pages. This is particularly useful for:
- Multi-page tables with repeated column headers
- Long sections that need context on continuation pages
- Reports where each page needs identifying information
- Documents where "Continued from previous page" indicators are needed

The continuation header only appears when content flows to additional pages. The first page shows the regular `<header>` element (if present), while subsequent pages show the `<continuation-header>`.
If the continuation header is not present then any template `<header>` will show on all pages.

---

## Usage

```html
<body>
    <header>
        Main Header - First Page Only
    </header>

    <continuation-header>
        Continuation Header - Appears on Pages 2, 3, 4...
    </continuation-header>

    <!-- Main content -->
    <div>
        Long content that spans multiple pages...
    </div>
</body>
```

---

## Supported Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element |
| `class` | string | CSS class name(s) for styling |
| `style` | string | Inline CSS styles |
| `hidden` | string | If set to "hidden", the element is not visible |
| `data-content` | expression | Data binding expression for dynamic content |

---

## Parent Elements

The `<continuation-header>` element can be placed within:
- `<body>` - Applies to all pages in the document body
- `<section>` - Applies only to pages within that specific section

---

## Behavior

- **First Page**: Does NOT appear (use `<header>` for first page)
- **Continuation Pages**: Appears at the top of pages 2, 3, 4, etc.
- **Styling**: Can be styled independently from the main `<header>`
- **Data Binding**: Supports full data binding capabilities
- **Layout**: Automatically positioned at the top of each continuation page
- **Overflow**: If the continuation header itself is too tall, it will block the layout of **any** content.

---

## Notes

- The continuation header is completely independent from the regular `<header>` element
- Can contain any HTML content (text, images, tables, etc.)
- Useful for "Continued..." indicators, page context, or repeated table headers
- Margin, padding, and borders are respected
- Can be styled differently from the main header
- Does not appear if content fits on a single page
- Can include dynamic content through data binding

---

## Examples

### Basic Continuation Header

```html
<body>
    <header>
        <h1>Annual Report 2024</h1>
        <p>Company Overview</p>
    </header>

    <continuation-header>
        <p style="font-style: italic; color: #666;">
            Annual Report 2024 - Continued
        </p>
    </continuation-header>

    <div>
        <!-- Long content that spans multiple pages -->
    </div>
</body>
```

### Multi-page Table with Headers

```html
{% raw %}<style>
    continuation-header {
        background-color: #f0f0f0;
        padding: 10pt;
        border-bottom: 2pt solid #333;
    }
</style>

<body>
    <header>
        <h1>Sales Report</h1>
        <p>January 2024</p>
    </header>

    <continuation-header>
        <table style="width: 100%">
            <tr style="background-color: #333; color: white;">
                <th style='width: 25%'>Date</th>
                <th style='width: 25%'>Product</th>
                <th style='width: 25%'>Quantity</th>
                <th style='width: 25%'>Amount</th>
            </tr>
        </table>
    </continuation-header>

    <table style="width: 100%">
        <tbody>
            <template data-bind="{{sales.transactions}}">
            <tr>
                <td style='width: 25%'>{{.date}}</td>
                <td style='width: 25%'>{{.product}}</td>
                <td style='width: 25%'>{{.quantity}}</td>
                <td style='width: 25%'>${{.amount}}</td>
            </tr>
            </template>
        </tbody>
    </table>
</body>{% endraw %}
```

### Section-Specific Continuation Header

```html
<section>
    <header>
        <h2>Financial Data</h2>
    </header>

    <continuation-header>
        <p><strong>Financial Data (continued)</strong></p>
    </continuation-header>

    <div>
        <!-- Long financial tables and data -->
    </div>
</section>

<section>
    <header>
        <h2>Employee Records</h2>
    </header>

    <continuation-header>
        <p><strong>Employee Records (continued)</strong></p>
    </continuation-header>

    <div>
        <!-- Long employee listings -->
    </div>
</section>
```

### Continuation Header with Page Numbers

```html
<continuation-header>
    <div style="display: flex; justify-content: space-between;">
        <span>Report Continued</span>
        <span>Page <page /></span>
    </div>
</continuation-header>
```

### Data-Bound Continuation Header

```html
{% raw %}<body>
    <header>
        <h1>{{report.title}}</h1>
        <p>Generated: {{report.date}}</p>
    </header>

    <continuation-header>
        <div style="font-size: 10pt; color: #666;">
            <strong>{{report.title}}</strong> - Page <page /> - {{report.date}}
        </div>
    </continuation-header>

    <div>
        <template data-bind="{{report.sections}}">
            <h2>{{.heading}}</h2>
            <p data-content="{{.content}}"></p>
        </template>
    </div>
</body>{% endraw %}
```

### Styled Continuation Indicator

```html
<style>
    continuation-header {
        background: linear-gradient(to right, #f0f0f0, white);
        padding: 8pt 20pt;
        border-left: 4pt solid #0066cc;
        font-style: italic;
    }
</style>

<body>
    <header>
        <h1>Technical Documentation</h1>
    </header>

    <continuation-header>
        Technical Documentation (continued from previous page)
    </continuation-header>

    <div>
        <!-- Documentation content -->
    </div>
</body>
```

### Multi-Column Report Header

```html
<continuation-header>
    <table style="width: 100%; border-collapse: collapse;">
        <tr>
            <td style="width: 33%; text-align: left;">
                <strong>Project Status Report</strong>
            </td>
            <td style="width: 34%; text-align: center;">
                Continued
            </td>
            <td style="width: 33%; text-align: right;">
                Page <page />
            </td>
        </tr>
    </table>
</continuation-header>
```

### Continuation with Company Branding

```html
<continuation-header>
    <div style="border-bottom: 2pt solid #0066cc; padding-bottom: 5pt;">
        <img src="logo-small.png" style="height: 20pt; float: left;" />
        <span style="float: right; font-size: 9pt; color: #666;">
            Confidential - Page <page />
        </span>
        <div style="clear: both;"></div>
    </div>
</continuation-header>
```

### Invoice Continuation

```html
{% raw %}<body>
    <header>
        <h1>Invoice #{{invoice.number}}</h1>
        <p>Bill To: {{invoice.customer.name}}</p>
        <p>Date: {{invoice.date}}</p>
    </header>

    <continuation-header>
        <div style="background-color: #f9f9f9; padding: 10pt; border: 1pt solid #ddd;">
            <strong>Invoice #{{invoice.number}}</strong> - Continued<br/>
            Customer: {{invoice.customer.name}}
        </div>
    </continuation-header>

    <table>
        <thead>
            <tr>
                <th>Item</th>
                <th>Quantity</th>
                <th>Price</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody data-bind="{{invoice.lineItems}}">
            <tr>
                <td>{{.description}}</td>
                <td>{{.quantity}}</td>
                <td>${{.unitPrice}}</td>
                <td>${{.quantity * .unitPrice}}</td>
            </tr>
        </tbody>
    </table>
</body>{% endraw %}
```

### Legal Document Continuation

```html
{% raw %}<style>
    continuation-header {
        font-family: 'Times New Roman', serif;
        font-size: 10pt;
        text-align: center;
        padding: 5pt;
        border-bottom: 1pt solid black;
    }
</style>

<body>
    <header>
        <h1 style="text-align: center;">CONTRACT AGREEMENT</h1>
        <p style="text-align: center;">Between {{contract.party1}} and {{contract.party2}}</p>
    </header>

    <continuation-header>
        CONTRACT - {{contract.party1}} / {{contract.party2}} - Page <page />
    </continuation-header>

    <div>
        <!-- Contract clauses -->
    </div>
</body>{% endraw %}
```

### Medical Records with Patient Info

```html
{% raw %}<body>
    <header>
        <h1>Patient Medical Record</h1>
        <p>Name: {{patient.name}}</p>
        <p>DOB: {{patient.dob}} | MRN: {{patient.mrn}}</p>
    </header>

    <continuation-header style="background-color: #e8f4f8; padding: 8pt;">
        <strong>Patient: {{patient.name}}</strong> |
        MRN: {{patient.mrn}} |
        Page <page />
    </continuation-header>

    <main >
        <template data-bind="{{patient.records}}">
            <div class='visit'>
            <h3>{{.date}}: {{.visitType}}</h3>
            <p data-content="{{.notes}}"></p>
            <div>
        </template>
    </main>
</body>{% endraw %}
```

### Academic Transcript

```html
{% raw %}<body>
    <header>
        <h1>Official Academic Transcript</h1>
        <p>Student: {{student.name}} (ID: {{student.id}})</p>
        <p>Program: {{student.program}}</p>
    </header>

    <continuation-header>
        <div style="font-size: 9pt; background-color: #f5f5f5; padding: 5pt;">
            Transcript - {{student.name}} - {{student.id}} - Continued
        </div>
    </continuation-header>

    <table data-content="{{student.courses}}">
        <!-- Course listings -->
    </table>
</body>{% endraw %}
```

### Inventory List with Context

```html
{% raw %}<continuation-header>
    <table style="width: 100%; background-color: #333; color: white; padding: 5pt;">
        <tr>
            <td><strong>Warehouse Inventory</strong></td>
            <td style="text-align: right;">
                Location: {{warehouse.location}} |
                Date: {{report.date}}
            </td>
        </tr>
    </table>
</continuation-header>{% endraw %}
```

### Conference Schedule

```html
{% raw %}<body>
    <header>
        <h1>{{conference.name}}</h1>
        <h2>Schedule - {{conference.date}}</h2>
    </header>

    <continuation-header style="border-top: 3pt solid #0066cc; border-bottom: 1pt solid #0066cc; padding: 8pt;">
        <strong>{{conference.name}}</strong> - Schedule (continued) - {{conference.date}}
    </continuation-header>

    <main>
        <template data-bind="{{conference.sessions}}">
            <div >
                <h3>{{.time}}: {{.title}}</h3>
                <p>Speaker: {{.speaker}} | Room: {{.room}}</p>
            </div>
        </template>
    </main>
</body>{% endraw %}
```

---

## See Also

- [continuation-footer](html_continuation-footer_element) - The footer on continuation pages
- [header](html_header_element) - Individual page headers.
- [body](html_body_element) - The document body element.
- [section](html_section_element) - Section element.
- [page](html_page_element) - Page number display.
- [Page Management](learning/styles/page_layout) - Page breaks, sections and content flow.
- [Multi-page Documents](/learning/styles/page_sizes) - Page sizing, numbering and grouping.

---
