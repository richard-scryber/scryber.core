---
layout: default
title: continuation-footer Element
parent: HTML Elements
parent_url: /reference/htmlelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;continuation-footer&gt; : The Continuation Footer Element

The `<continuation-footer>` element defines content that appears at the bottom of continuation pages when content spans multiple pages. Unlike the regular `<footer>` element which appears only on the last page, the continuation footer appears on pages before the final page.

## Summary

The `<continuation-footer>` element is used within `<body>` or `<section>` elements to provide a footer that displays on overflow pages. This is particularly useful for:
- Indicating "Continued on next page"
- Showing running page totals before the final total
- Adding disclaimers or notices on each page
- Displaying context information throughout multi-page content

The continuation footer appears on pages 1, 2, 3... but NOT on the final page. The last page shows the regular `<footer>` element (if present).

---

## Usage

```html
<body>
    <div>
        Long content that spans multiple pages...
    </div>

    <continuation-footer>
        Continuation Footer - Appears on Pages 1, 2, 3...
        (but not the last page)
    </continuation-footer>

    <footer>
        Final Footer - Last Page Only
    </footer>
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

The `<continuation-footer>` element can be placed within:
- `<body>` - Applies to all pages in the document body
- `<section>` - Applies only to pages within that specific section

---

## Behavior

- **First Page**: Appears at the bottom
- **Middle Pages**: Appears at the bottom of pages 2, 3, 4, etc.
- **Last Page**: Does NOT appear (use `<footer>` for final page)
- **Styling**: Can be styled independently from the main `<footer>`
- **Data Binding**: Supports full data binding capabilities
- **Layout**: Automatically positioned at the bottom of each non-final page
- **Overflow**: If the continuation footer itself is too tall, it will span multiple pages

---

## Notes

- The continuation footer is completely independent from the regular `<footer>` element
- Can contain any HTML content (text, images, tables, etc.)
- Useful for "Continued..." indicators, running totals, or interim summaries
- Margin, padding, and borders are respected
- Can be styled differently from the main footer
- Does not appear if content fits on a single page
- Can include dynamic content through data binding
- Commonly used to indicate more content follows on the next page

---

## Examples

### Example 1: Basic Continuation Footer

```html
<body>
    <div>
        <!-- Long content that spans multiple pages -->
    </div>

    <continuation-footer>
        <p style="text-align: right; font-style: italic; color: #666;">
            Continued on next page...
        </p>
    </continuation-footer>

    <footer>
        <p>End of Document</p>
    </footer>
</body>
```

### Example 2: Page Number Indicator

```html
<style>
    continuation-footer {
        background-color: #f9f9f9;
        padding: 10pt;
        border-top: 2pt solid #333;
        text-align: center;
    }
</style>

<body>
    <div>
        <!-- Content -->
    </div>

    <continuation-footer>
        Page <page-number /> - Continued...
    </continuation-footer>

    <footer>
        Page <page-number /> - End of Report
    </footer>
</body>
```

### Example 3: Running Total vs Final Total

```html
<body>
    <var data-id="runningTotal" data-value="0"></var>

    <table>
        <tbody data-bind="{{invoice.items}}">
            <var data-id="runningTotal" data-value="{{params.runningTotal + .amount}}"></var>
            <tr>
                <td>{{.description}}</td>
                <td>${{.amount}}</td>
            </tr>
        </tbody>
    </table>

    <continuation-footer>
        <div style="text-align: right; padding: 10pt; background-color: #fff3cd;">
            <strong>Subtotal (page <page-number />): ${{params.runningTotal}}</strong><br/>
            <em>Continued on next page...</em>
        </div>
    </continuation-footer>

    <footer>
        <div style="text-align: right; padding: 10pt; background-color: #d4edda;">
            <strong>Final Total: ${{params.runningTotal}}</strong>
        </div>
    </footer>
</body>
```

### Example 4: Legal Disclaimer on Each Page

```html
<continuation-footer>
    <div style="font-size: 8pt; text-align: center; color: #666; padding: 5pt; border-top: 1pt solid #ccc;">
        CONFIDENTIAL - For Internal Use Only - More Content Follows
    </div>
</continuation-footer>

<footer>
    <div style="font-size: 8pt; text-align: center; color: #666; padding: 5pt; border-top: 1pt solid #ccc;">
        CONFIDENTIAL - For Internal Use Only - End of Document
    </div>
</footer>
```

### Example 5: Data-Bound Continuation Footer

```html
<body>
    <div data-bind="{{report.sections}}">
        <h2>{{.title}}</h2>
        <p>{{.content}}</p>
    </div>

    <continuation-footer>
        <div style="background-color: #e9ecef; padding: 8pt;">
            <strong>{{report.title}}</strong> |
            Generated: {{report.date}} |
            <em>Continued...</em>
        </div>
    </continuation-footer>

    <footer>
        <div style="background-color: #d4edda; padding: 8pt;">
            <strong>{{report.title}}</strong> |
            Generated: {{report.date}} |
            <strong>END OF REPORT</strong>
        </div>
    </footer>
</body>
```

### Example 6: Table with Continuation Notice

```html
<style>
    continuation-footer {
        background: linear-gradient(to top, #f0f0f0, white);
        padding: 8pt 20pt;
        border-left: 4pt solid #ffc107;
        font-style: italic;
        text-align: right;
    }
</style>

<body>
    <table>
        <!-- Many rows -->
    </table>

    <continuation-footer>
        Table continues on next page →
    </continuation-footer>

    <footer>
        End of table
    </footer>
</body>
```

### Example 7: Invoice with Page Subtotals

```html
<body>
    <h1>Invoice #{{invoice.number}}</h1>

    <table>
        <thead>
            <tr>
                <th>Item</th>
                <th>Qty</th>
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

    <continuation-footer>
        <table style="width: 100%; background-color: #fff3cd; padding: 10pt;">
            <tr>
                <td style="text-align: right;">
                    <strong>Page Subtotal: ${{params.pageSubtotal}}</strong><br/>
                    <em style="font-size: 9pt;">Invoice continues on next page</em>
                </td>
            </tr>
        </table>
    </continuation-footer>

    <footer>
        <table style="width: 100%;">
            <tr>
                <td colspan="3" style="text-align: right;"><strong>Subtotal:</strong></td>
                <td style="text-align: right;">${{invoice.subtotal}}</td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: right;"><strong>Tax (8%):</strong></td>
                <td style="text-align: right;">${{invoice.subtotal * 0.08}}</td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: right;"><strong>Total:</strong></td>
                <td style="text-align: right;"><strong>${{invoice.subtotal * 1.08}}</strong></td>
            </tr>
        </table>
    </footer>
</body>
```

### Example 8: Multi-Column Footer Layout

```html
<continuation-footer>
    <table style="width: 100%; border-collapse: collapse; border-top: 2pt solid #333;">
        <tr>
            <td style="width: 50%; font-size: 9pt; padding: 5pt;">
                Document continues...
            </td>
            <td style="width: 50%; text-align: right; font-size: 9pt; padding: 5pt;">
                Page <page-number /> of <page-count />
            </td>
        </tr>
    </table>
</continuation-footer>

<footer>
    <table style="width: 100%; border-collapse: collapse; border-top: 2pt solid #333;">
        <tr>
            <td style="width: 50%; font-size: 9pt; padding: 5pt;">
                End of document
            </td>
            <td style="width: 50%; text-align: right; font-size: 9pt; padding: 5pt;">
                Page <page-number /> of <page-count />
            </td>
        </tr>
    </table>
</footer>
```

### Example 9: Section-Specific Footers

```html
<section>
    <header>
        <h2>Financial Summary</h2>
    </header>

    <div>
        <!-- Financial data -->
    </div>

    <continuation-footer>
        <p style="text-align: center; font-size: 9pt;">
            Financial Summary continues on next page
        </p>
    </continuation-footer>

    <footer>
        <p style="text-align: center; font-size: 9pt;">
            End of Financial Summary
        </p>
    </footer>
</section>

<section>
    <header>
        <h2>Employee Records</h2>
    </header>

    <div>
        <!-- Employee data -->
    </div>

    <continuation-footer>
        <p style="text-align: center; font-size: 9pt;">
            Employee Records continue on next page
        </p>
    </continuation-footer>

    <footer>
        <p style="text-align: center; font-size: 9pt;">
            End of Employee Records
        </p>
    </footer>
</section>
```

### Example 10: Progress Indicator

```html
<continuation-footer>
    <div style="background-color: #e3f2fd; padding: 10pt; text-align: center;">
        <div style="background-color: #2196f3; height: 5pt; width: {{(params.currentPage / params.totalPages) * 100}}%;"></div>
        <p style="margin-top: 5pt; font-size: 9pt;">
            Reading progress: {{params.currentPage}} / {{params.totalPages}} pages
        </p>
    </div>
</continuation-footer>
```

### Example 11: Medical Report Continuation

```html
<body>
    <h1>Patient Medical Record</h1>
    <p>Patient: {{patient.name}} | MRN: {{patient.mrn}}</p>

    <div data-bind="{{patient.visits}}">
        <h3>{{.date}}: {{.visitType}}</h3>
        <p>{{.notes}}</p>
    </div>

    <continuation-footer style="background-color: #fff3cd; padding: 8pt; border-top: 2pt solid #ffc107;">
        <strong>Patient: {{patient.name}}</strong> |
        MRN: {{patient.mrn}} |
        <em>Record continues...</em>
    </continuation-footer>

    <footer style="background-color: #d4edda; padding: 8pt; border-top: 2pt solid #28a745;">
        <strong>Patient: {{patient.name}}</strong> |
        MRN: {{patient.mrn}} |
        <strong>END OF RECORD</strong>
    </footer>
</body>
```

### Example 12: Catalog with Item Counts

```html
<body>
    <var data-id="itemCount" data-value="0"></var>

    <div data-bind="{{catalog.products}}">
        <var data-id="itemCount" data-value="{{params.itemCount + 1}}"></var>
        <div class="product">
            <h3>{{.name}}</h3>
            <p>${{.price}}</p>
        </div>
    </div>

    <continuation-footer>
        <div style="text-align: right; font-size: 10pt; background-color: #f5f5f5; padding: 8pt;">
            Items shown: {{params.itemCount}} | More products on next page →
        </div>
    </continuation-footer>

    <footer>
        <div style="text-align: right; font-size: 10pt; background-color: #e8f5e9; padding: 8pt;">
            Total items in catalog: {{params.itemCount}}
        </div>
    </footer>
</body>
```

### Example 13: Contract Continuation Notice

```html
<style>
    continuation-footer {
        font-family: 'Times New Roman', serif;
        font-size: 10pt;
        text-align: center;
        padding: 10pt;
        border-top: 1pt solid black;
    }
</style>

<body>
    <h1 style="text-align: center;">AGREEMENT</h1>

    <div>
        <!-- Contract clauses -->
    </div>

    <continuation-footer>
        — Page <page-number /> — Contract continues on next page —
    </continuation-footer>

    <footer>
        <p style="text-align: center; margin-top: 20pt;">
            — End of Agreement —
        </p>
        <div style="margin-top: 30pt;">
            <div style="display: inline-block; width: 45%;">
                _______________________<br/>
                Party A Signature
            </div>
            <div style="display: inline-block; width: 45%; float: right;">
                _______________________<br/>
                Party B Signature
            </div>
        </div>
    </footer>
</body>
```

### Example 14: Newsletter Article Continuation

```html
<section data-bind="{{newsletter.articles}}">
    <header>
        <h2>{{.title}}</h2>
        <p>By {{.author}}</p>
    </header>

    <div>
        {{.content}}
    </div>

    <continuation-footer style="background-color: #f0f8ff; padding: 8pt; font-style: italic; text-align: right;">
        Article "{{.title}}" continues →
    </continuation-footer>

    <footer style="background-color: #e8f5e9; padding: 8pt; text-align: center;">
        End of article: "{{.title}}"
    </footer>
</section>
```

### Example 15: Audit Trail with Timestamps

```html
<body>
    <h1>System Audit Log</h1>

    <table data-bind="{{audit.entries}}">
        <tr>
            <td>{{.timestamp}}</td>
            <td>{{.user}}</td>
            <td>{{.action}}</td>
        </tr>
    </table>

    <continuation-footer style="font-size: 8pt; background-color: #f8f9fa; padding: 5pt; text-align: center;">
        Audit log continues on page <page-number data-offset="1" /> |
        Report generated: {{audit.generatedDate}}
    </continuation-footer>

    <footer style="font-size: 8pt; background-color: #d4edda; padding: 5pt; text-align: center;">
        End of audit log ({{audit.totalEntries}} entries) |
        Report generated: {{audit.generatedDate}}
    </footer>
</body>
```

### Example 16: Academic Transcript Continuation

```html
<body>
    <h1>Academic Transcript</h1>
    <p>Student: {{student.name}} | ID: {{student.id}}</p>

    <table>
        <thead>
            <tr>
                <th>Course</th>
                <th>Credits</th>
                <th>Grade</th>
            </tr>
        </thead>
        <tbody data-bind="{{student.courses}}">
            <tr>
                <td>{{.code}} - {{.title}}</td>
                <td>{{.credits}}</td>
                <td>{{.grade}}</td>
            </tr>
        </tbody>
    </table>

    <continuation-footer style="background-color: #e3f2fd; padding: 10pt;">
        <div style="text-align: right;">
            <em>Transcript continues...</em>
        </div>
    </continuation-footer>

    <footer style="background-color: #c8e6c9; padding: 10pt;">
        <table style="width: 100%;">
            <tr>
                <td><strong>Total Credits:</strong></td>
                <td style="text-align: right;">{{student.totalCredits}}</td>
            </tr>
            <tr>
                <td><strong>GPA:</strong></td>
                <td style="text-align: right;">{{student.gpa}}</td>
            </tr>
        </table>
    </footer>
</body>
```

### Example 17: Conditional Continuation Footer

```html
<body>
    <div>
        <!-- Content -->
    </div>

    <continuation-footer data-if="{{document.showContinuationNotice}}">
        <p style="text-align: center; font-style: italic; color: #666;">
            {{document.continuationMessage}}
        </p>
    </continuation-footer>

    <footer>
        <p style="text-align: center;">
            {{document.finalMessage}}
        </p>
    </footer>
</body>
```

### Example 18: Multi-Page Table Summary

```html
<body>
    <var data-id="pageTotal" data-value="0"></var>
    <var data-id="grandTotal" data-value="0"></var>

    <table data-bind="{{transactions}}">
        <var data-id="pageTotal" data-value="{{params.pageTotal + .amount}}"></var>
        <var data-id="grandTotal" data-value="{{params.grandTotal + .amount}}"></var>
        <tr>
            <td>{{.date}}</td>
            <td>{{.description}}</td>
            <td>${{.amount}}</td>
        </tr>
    </table>

    <continuation-footer>
        <table style="width: 100%; background-color: #fff9e6; padding: 5pt;">
            <tr>
                <td><strong>Page Total:</strong></td>
                <td style="text-align: right;"><strong>${{params.pageTotal}}</strong></td>
            </tr>
            <tr>
                <td style="font-size: 9pt;"><em>Running Total:</em></td>
                <td style="text-align: right; font-size: 9pt;"><em>${{params.grandTotal}}</em></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center; font-size: 8pt; padding-top: 5pt;">
                    Continued on next page...
                </td>
            </tr>
        </table>
    </continuation-footer>

    <footer>
        <table style="width: 100%; background-color: #e8f5e9; padding: 10pt;">
            <tr>
                <td><strong>Grand Total:</strong></td>
                <td style="text-align: right; font-size: 14pt;">
                    <strong>${{params.grandTotal}}</strong>
                </td>
            </tr>
        </table>
    </footer>
</body>
```

### Example 19: Warranty Document

```html
<continuation-footer>
    <div style="border-top: 2pt solid #dc3545; padding: 10pt; background-color: #f8d7da;">
        <p style="font-size: 8pt; text-align: center; margin: 0;">
            <strong>IMPORTANT:</strong> This warranty document continues.
            Please read all pages carefully.
        </p>
    </div>
</continuation-footer>

<footer>
    <div style="border-top: 2pt solid #28a745; padding: 10pt; background-color: #d4edda;">
        <p style="font-size: 8pt; text-align: center; margin: 0;">
            End of warranty document. Customer signature required.
        </p>
    </div>
</footer>
```

### Example 20: Technical Specification Pages

```html
<body>
    <h1>{{product.name}} - Technical Specifications</h1>

    <div data-bind="{{product.specifications}}">
        <h2>{{.category}}</h2>
        <table>
            <tr data-bind="{{.specs}}">
                <td><strong>{{.name}}:</strong></td>
                <td>{{.value}}</td>
            </tr>
        </table>
    </div>

    <continuation-footer>
        <div style="background: linear-gradient(to right, #e3f2fd, #bbdefb); padding: 10pt; border-top: 3pt solid #2196f3;">
            <table style="width: 100%;">
                <tr>
                    <td style="font-weight: bold;">{{product.name}}</td>
                    <td style="text-align: center; font-style: italic;">More specifications follow</td>
                    <td style="text-align: right; font-size: 9pt;">Page <page-number /></td>
                </tr>
            </table>
        </div>
    </continuation-footer>

    <footer>
        <div style="background: linear-gradient(to right, #c8e6c9, #a5d6a7); padding: 10pt; border-top: 3pt solid #4caf50;">
            <table style="width: 100%;">
                <tr>
                    <td style="font-weight: bold;">{{product.name}}</td>
                    <td style="text-align: center; font-weight: bold;">END OF SPECIFICATIONS</td>
                    <td style="text-align: right; font-size: 9pt;">Page <page-number /></td>
                </tr>
            </table>
        </div>
    </footer>
</body>
```

---

## See Also

- [continuation-header Element](/reference/htmlelements/html_continuation-header_element)
- [footer Element](/reference/htmlelements/html_footer_element)
- [body Element](/reference/htmlelements/html_body_element)
- [section Element](/reference/htmlelements/html_section_element)
- [Page Numbers](/reference/components/page-number)
- [Multi-page Documents](/guides/multi-page)

---
