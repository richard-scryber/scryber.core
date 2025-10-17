---
layout: default
title: continuation-header Element
parent: HTML Elements
parent_url: /reference/htmlelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;continuation-header&gt; : The Continuation Header Element

The `<continuation-header>` element defines content that appears at the top of continuation pages when content spans multiple pages. Unlike the regular `<header>` element which appears only on the first page, the continuation header appears on second and subsequent pages.

## Summary

The `<continuation-header>` element is used within `<body>` or `<section>` elements to provide a header that displays on overflow pages. This is particularly useful for:
- Multi-page tables with repeated column headers
- Long sections that need context on continuation pages
- Reports where each page needs identifying information
- Documents where "Continued from previous page" indicators are needed

The continuation header only appears when content flows to additional pages. The first page shows the regular `<header>` element (if present), while subsequent pages show the `<continuation-header>`.

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
| `data-bind` | expression | Data binding expression for dynamic content |

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
- **Overflow**: If the continuation header itself is too tall, it will span multiple pages

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

### Example 1: Basic Continuation Header

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

### Example 2: Multi-page Table with Headers

```html
<style>
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
                <th>Date</th>
                <th>Product</th>
                <th>Quantity</th>
                <th>Amount</th>
            </tr>
        </table>
    </continuation-header>

    <table style="width: 100%">
        <tbody data-bind="{{sales.transactions}}">
            <tr>
                <td>{{.date}}</td>
                <td>{{.product}}</td>
                <td>{{.quantity}}</td>
                <td>${{.amount}}</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 3: Section-Specific Continuation Header

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

### Example 4: Continuation Header with Page Numbers

```html
<continuation-header>
    <div style="display: flex; justify-content: space-between;">
        <span>Report Continued</span>
        <span>Page <page-number /></span>
    </div>
</continuation-header>
```

### Example 5: Data-Bound Continuation Header

```html
<body>
    <header>
        <h1>{{report.title}}</h1>
        <p>Generated: {{report.date}}</p>
    </header>

    <continuation-header>
        <div style="font-size: 10pt; color: #666;">
            <strong>{{report.title}}</strong> - Page <page-number /> - {{report.date}}
        </div>
    </continuation-header>

    <div data-bind="{{report.sections}}">
        <h2>{{.heading}}</h2>
        <p>{{.content}}</p>
    </div>
</body>
```

### Example 6: Styled Continuation Indicator

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

### Example 7: Multi-Column Report Header

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
                Page <page-number />
            </td>
        </tr>
    </table>
</continuation-header>
```

### Example 8: Continuation with Company Branding

```html
<continuation-header>
    <div style="border-bottom: 2pt solid #0066cc; padding-bottom: 5pt;">
        <img src="logo-small.png" style="height: 20pt; float: left;" />
        <span style="float: right; font-size: 9pt; color: #666;">
            Confidential - Page <page-number />
        </span>
        <div style="clear: both;"></div>
    </div>
</continuation-header>
```

### Example 9: Invoice Continuation

```html
<body>
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
</body>
```

### Example 10: Legal Document Continuation

```html
<style>
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
        CONTRACT - {{contract.party1}} / {{contract.party2}} - Page <page-number />
    </continuation-header>

    <div>
        <!-- Contract clauses -->
    </div>
</body>
```

### Example 11: Medical Records with Patient Info

```html
<body>
    <header>
        <h1>Patient Medical Record</h1>
        <p>Name: {{patient.name}}</p>
        <p>DOB: {{patient.dob}} | MRN: {{patient.mrn}}</p>
    </header>

    <continuation-header style="background-color: #e8f4f8; padding: 8pt;">
        <strong>Patient: {{patient.name}}</strong> |
        MRN: {{patient.mrn}} |
        Page <page-number />
    </continuation-header>

    <div data-bind="{{patient.records}}">
        <h3>{{.date}}: {{.visitType}}</h3>
        <p>{{.notes}}</p>
    </div>
</body>
```

### Example 12: Academic Transcript

```html
<body>
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

    <table data-bind="{{student.courses}}">
        <!-- Course listings -->
    </table>
</body>
```

### Example 13: Inventory List with Context

```html
<continuation-header>
    <table style="width: 100%; background-color: #333; color: white; padding: 5pt;">
        <tr>
            <td><strong>Warehouse Inventory</strong></td>
            <td style="text-align: right;">
                Location: {{warehouse.location}} |
                Date: {{report.date}}
            </td>
        </tr>
    </table>
</continuation-header>
```

### Example 14: Conference Schedule

```html
<body>
    <header>
        <h1>{{conference.name}}</h1>
        <h2>Schedule - {{conference.date}}</h2>
    </header>

    <continuation-header style="border-top: 3pt solid #0066cc; border-bottom: 1pt solid #0066cc; padding: 8pt;">
        <strong>{{conference.name}}</strong> - Schedule (continued) - {{conference.date}}
    </continuation-header>

    <div data-bind="{{conference.sessions}}">
        <h3>{{.time}}: {{.title}}</h3>
        <p>Speaker: {{.speaker}} | Room: {{.room}}</p>
    </div>
</body>
```

### Example 15: Multi-Department Report

```html
<body data-bind="{{report.departments}}">
    <header>
        <h1>{{.departmentName}} Report</h1>
        <p>Quarter: {{report.quarter}} {{report.year}}</p>
    </header>

    <continuation-header>
        <div style="background-color: #e9ecef; padding: 10pt;">
            <strong>{{.departmentName}}</strong> - {{report.quarter}} {{report.year}} - Continued
        </div>
    </continuation-header>

    <div data-bind="{{.data}}">
        <!-- Department data -->
    </div>
</body>
```

### Example 16: Catalog Continuation

```html
<style>
    continuation-header {
        background-color: #fff3cd;
        border: 1pt solid #ffc107;
        padding: 10pt;
        margin-bottom: 10pt;
    }
</style>

<continuation-header>
    <div style="display: flex; justify-content: space-between; align-items: center;">
        <div>
            <strong>Product Catalog 2024</strong><br/>
            <small>Categories: {{category.name}}</small>
        </div>
        <div style="text-align: right;">
            <small>Page <page-number /></small>
        </div>
    </div>
</continuation-header>
```

### Example 17: Conditional Continuation Header

```html
<body>
    <header>
        <h1>{{document.title}}</h1>
    </header>

    <continuation-header data-if="{{document.showContinuationHeader}}">
        <p style="font-style: italic; color: #666;">
            {{document.title}} - Continued
        </p>
    </continuation-header>

    <div>
        <!-- Content -->
    </div>
</body>
```

### Example 18: Audit Trail Report

```html
<body>
    <header>
        <h1>System Audit Trail</h1>
        <p>Period: {{audit.startDate}} to {{audit.endDate}}</p>
        <p>Generated: {{audit.generatedDate}}</p>
    </header>

    <continuation-header style="font-size: 9pt; background-color: #f8f9fa; padding: 8pt; border-bottom: 2pt solid #dee2e6;">
        <strong>Audit Trail</strong> |
        Period: {{audit.startDate}} - {{audit.endDate}} |
        Page <page-number />
    </continuation-header>

    <table data-bind="{{audit.entries}}">
        <tr>
            <td>{{.timestamp}}</td>
            <td>{{.user}}</td>
            <td>{{.action}}</td>
        </tr>
    </table>
</body>
```

### Example 19: Newsletter with Sections

```html
<section data-bind="{{newsletter.articles}}">
    <header>
        <h2>{{.title}}</h2>
        <p>By {{.author}}</p>
    </header>

    <continuation-header style="background-color: #f0f8ff; padding: 5pt; font-size: 10pt;">
        <em>{{.title}}</em> (continued)
    </continuation-header>

    <div>
        {{.content}}
    </div>
</section>
```

### Example 20: Phone Directory

```html
<body>
    <header>
        <h1>Company Directory</h1>
        <p>{{company.name}} - Internal Use Only</p>
    </header>

    <continuation-header>
        <table style="width: 100%; font-size: 9pt; background-color: #e9ecef;">
            <tr>
                <td><strong>Name</strong></td>
                <td><strong>Department</strong></td>
                <td><strong>Extension</strong></td>
                <td style="text-align: right;">Page <page-number /></td>
            </tr>
        </table>
    </continuation-header>

    <table style="width: 100%;">
        <tbody data-bind="{{employees}}">
            <tr>
                <td>{{.name}}</td>
                <td>{{.department}}</td>
                <td>{{.extension}}</td>
            </tr>
        </tbody>
    </table>
</body>
```

---

## See Also

- [continuation-footer Element](/reference/htmlelements/html_continuation-footer_element)
- [header Element](/reference/htmlelements/html_header_element)
- [body Element](/reference/htmlelements/html_body_element)
- [section Element](/reference/htmlelements/html_section_element)
- [Page Numbers](/reference/components/page-number)
- [Multi-page Documents](/guides/multi-page)

---
