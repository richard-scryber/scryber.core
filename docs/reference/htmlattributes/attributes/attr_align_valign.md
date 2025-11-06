---
layout: default
title: align and valign
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @align and @valign : Alignment Attributes

The `align` and `valign` attributes control horizontal and vertical alignment of content within table cells and other block elements. These legacy HTML attributes provide quick alignment control for PDF documents and remain useful for table-based layouts.

## Usage

These attributes control content alignment:
- **`align`**: Controls horizontal alignment (left, center, right, justify)
- **`valign`**: Controls vertical alignment (top, middle, bottom, baseline)
- Apply to table cells (`<td>`, `<th>`) and table rows (`<tr>`)
- Can be applied to block elements for text alignment
- Affect content positioning within the element
- Can be overridden by CSS text-align and vertical-align

```html
<!-- Horizontal alignment -->
<td align="left">Left aligned</td>
<td align="center">Center aligned</td>
<td align="right">Right aligned</td>

<!-- Vertical alignment -->
<td valign="top">Top aligned</td>
<td valign="middle">Middle aligned</td>
<td valign="bottom">Bottom aligned</td>

<!-- Both alignments -->
<td align="center" valign="middle">Centered both ways</td>
```

---

## Supported Elements

These attributes are supported by the following elements:

| Element | align | valign | Description |
|---------|-------|--------|-------------|
| `<td>` | Yes | Yes | Table data cell |
| `<th>` | Yes | Yes | Table header cell |
| `<tr>` | Yes | Yes | Table row (applies to all cells in row) |
| `<thead>` | Yes | Yes | Table header section |
| `<tbody>` | Yes | Yes | Table body section |
| `<tfoot>` | Yes | Yes | Table footer section |
| `<p>` | Yes | No | Paragraph (horizontal only) |
| `<div>` | Yes | No | Division (horizontal only) |
| `<h1>`-`<h6>` | Yes | No | Headings (horizontal only) |

---

## Align Attribute (Horizontal Alignment)

### Description

The `align` attribute controls horizontal text alignment within an element.

### Supported Values

| Value | Description | Effect |
|-------|-------------|--------|
| `left` | Align content to the left (default for most elements) | Text starts at left edge |
| `center` | Center content horizontally | Text centered in available space |
| `right` | Align content to the right | Text ends at right edge |
| `justify` | Justify text (stretch to fill width) | Text aligned to both edges |

### Syntax

```html
<td align="value">Content</td>
```

### Default Values

- **Table cells (`<td>`)**: `left` (left-aligned)
- **Table headers (`<th>`)**: `center` (center-aligned by default in some browsers, left in Scryber)
- **Paragraphs and text**: `left` (left-aligned)

### Examples

```html
<!-- Left aligned (default for td) -->
<td align="left">Left aligned text</td>

<!-- Center aligned -->
<td align="center">Centered text</td>

<!-- Right aligned (common for numbers) -->
<td align="right">$1,234.56</td>

<!-- Justified text -->
<td align="justify">This longer text will be justified to fill the width.</td>
```

---

## Valign Attribute (Vertical Alignment)

### Description

The `valign` attribute controls vertical alignment of content within an element.

### Supported Values

| Value | Description | Effect |
|-------|-------------|--------|
| `top` | Align content to the top | Content starts at top of cell |
| `middle` | Center content vertically (default) | Content centered in available height |
| `bottom` | Align content to the bottom | Content ends at bottom of cell |
| `baseline` | Align baselines of content | Text baselines aligned |

### Syntax

```html
<td valign="value">Content</td>
```

### Default Values

- **Table cells**: `middle` (vertically centered)
- **Table headers**: `middle` (vertically centered)

### Examples

```html
<!-- Top aligned -->
<td valign="top">Content at top</td>

<!-- Middle aligned (default) -->
<td valign="middle">Content in middle</td>

<!-- Bottom aligned -->
<td valign="bottom">Content at bottom</td>

<!-- Baseline aligned -->
<td valign="baseline">Baseline aligned</td>
```

---

## CSS Equivalents

### Align to CSS

The `align` attribute can be replaced with CSS:

```html
<!-- Using align attribute -->
<td align="center">Content</td>

<!-- Equivalent CSS -->
<td style="text-align: center;">Content</td>
```

### Valign to CSS

The `valign` attribute can be replaced with CSS:

```html
<!-- Using valign attribute -->
<td valign="top">Content</td>

<!-- Equivalent CSS -->
<td style="vertical-align: top;">Content</td>
```

---

## Applying to Rows and Sections

### Row-Level Alignment

Apply alignment to entire rows:

```html
<!-- All cells in row are center-aligned -->
<tr align="center" valign="middle">
    <td>Cell 1</td>
    <td>Cell 2</td>
    <td>Cell 3</td>
</tr>
```

Individual cells can override row alignment:

```html
<tr align="center" valign="middle">
    <td>Center aligned (from row)</td>
    <td align="right">Right aligned (overrides row)</td>
    <td>Center aligned (from row)</td>
</tr>
```

### Section-Level Alignment

Apply alignment to table sections:

```html
<!-- All cells in thead are center-aligned -->
<thead align="center" valign="middle">
    <tr>
        <th>Header 1</th>
        <th>Header 2</th>
    </tr>
</thead>

<!-- All cells in tbody are left-aligned -->
<tbody align="left" valign="top">
    <tr>
        <td>Data 1</td>
        <td>Data 2</td>
    </tr>
</tbody>
```

---

## Binding Values

Both attributes support data binding:

### Static Values

```html
<td align="center" valign="middle">Static alignment</td>
```

### Dynamic Values with Data Binding

```html
<!-- Model: { textAlign: "right", vertAlign: "top" } -->
<td align="{{model.textAlign}}" valign="{{model.vertAlign}}">
    Dynamic alignment
</td>
```

### Conditional Alignment

```html
<!-- Model: { isHeader: true } -->
<td align="{{model.isHeader ? 'center' : 'left'}}"
    valign="{{model.isHeader ? 'middle' : 'top'}}">
    Content
</td>
```

---

## Notes

### Common Use Cases

**Numbers and Currency**: Right-align for easier comparison
```html
<td align="right">$1,234.56</td>
<td align="right">999</td>
```

**Headers**: Center-align for visual balance
```html
<th align="center">Column Title</th>
```

**Text Content**: Left-align for readability
```html
<td align="left">Regular text content</td>
```

**Actions/Buttons**: Center-align for emphasis
```html
<td align="center">
    <input type="button" value="Click Me" />
</td>
```

### Vertical Alignment Usage

**Top Alignment**: When cells have varying heights
```html
<tr valign="top">
    <td>Short content</td>
    <td>Much longer content that spans multiple lines...</td>
</tr>
```

**Middle Alignment**: For balanced appearance (default)
```html
<td valign="middle">Vertically centered</td>
```

**Bottom Alignment**: For alignment with baseline elements
```html
<td valign="bottom">Aligned to bottom</td>
```

### Precedence

Alignment is applied in this order (later overrides earlier):
1. Table section (`<thead>`, `<tbody>`, `<tfoot>`)
2. Table row (`<tr>`)
3. Table cell (`<td>`, `<th>`)
4. CSS styles (highest precedence)

### Legacy vs. Modern Approach

**Legacy (align/valign attributes)**:
```html
<td align="center" valign="top">Content</td>
```

**Modern (CSS)**:
```html
<td style="text-align: center; vertical-align: top;">Content</td>
```

**Best Practice**: Use CSS for new documents; use attributes for quick prototypes or when CSS is inconvenient.

---

## Examples

### Basic Horizontal Alignment

```html
<table cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <tr>
        <td align="left" style="border: 1pt solid black; width: 33%;">
            Left aligned text in this cell
        </td>
        <td align="center" style="border: 1pt solid black; width: 33%;">
            Center aligned text in this cell
        </td>
        <td align="right" style="border: 1pt solid black; width: 33%;">
            Right aligned text in this cell
        </td>
    </tr>
</table>
```

### Basic Vertical Alignment

```html
<table cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <tr>
        <td valign="top" style="border: 1pt solid black; height: 100pt; width: 33%;">
            Top aligned content
        </td>
        <td valign="middle" style="border: 1pt solid black; height: 100pt; width: 33%;">
            Middle aligned content
        </td>
        <td valign="bottom" style="border: 1pt solid black; height: 100pt; width: 33%;">
            Bottom aligned content
        </td>
    </tr>
</table>
```

### Combined Alignment

```html
<table cellpadding="15" style="width: 100%; border-collapse: collapse;">
    <tr>
        <td align="left" valign="top"
            style="border: 1pt solid black; height: 80pt; width: 33%;">
            Top-Left
        </td>
        <td align="center" valign="middle"
            style="border: 1pt solid black; height: 80pt; width: 33%;">
            Center-Middle
        </td>
        <td align="right" valign="bottom"
            style="border: 1pt solid black; height: 80pt; width: 33%;">
            Bottom-Right
        </td>
    </tr>
    <tr>
        <td align="right" valign="top"
            style="border: 1pt solid black; height: 80pt;">
            Top-Right
        </td>
        <td align="left" valign="bottom"
            style="border: 1pt solid black; height: 80pt;">
            Bottom-Left
        </td>
        <td align="center" valign="top"
            style="border: 1pt solid black; height: 80pt;">
            Top-Center
        </td>
    </tr>
</table>
```

### Row-Level Alignment

```html
<table cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <!-- All cells in this row are center-aligned -->
    <tr align="center" valign="middle">
        <td style="border: 1pt solid black;">Cell 1</td>
        <td style="border: 1pt solid black;">Cell 2</td>
        <td style="border: 1pt solid black;">Cell 3</td>
    </tr>

    <!-- All cells in this row are right-aligned -->
    <tr align="right" valign="top">
        <td style="border: 1pt solid black;">Cell 4</td>
        <td style="border: 1pt solid black;">Cell 5</td>
        <td style="border: 1pt solid black;">Cell 6</td>
    </tr>

    <!-- Mixed: row default with cell override -->
    <tr align="left" valign="bottom">
        <td style="border: 1pt solid black;">Left (from row)</td>
        <td align="center" style="border: 1pt solid black;">Center (overridden)</td>
        <td style="border: 1pt solid black;">Left (from row)</td>
    </tr>
</table>
```

### Financial Report with Alignment

```html
<table cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <thead align="center" valign="middle">
        <tr>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Account
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Q1 2024
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Q2 2024
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Total
            </th>
        </tr>
    </thead>
    <tbody>
        <tr valign="middle">
            <td align="left" style="border: 1pt solid black; font-weight: bold;">
                Revenue
            </td>
            <td align="right" style="border: 1pt solid black; font-family: monospace;">
                $125,000
            </td>
            <td align="right" style="border: 1pt solid black; font-family: monospace;">
                $145,000
            </td>
            <td align="right" style="border: 1pt solid black; font-weight: bold;
                               background-color: #e3f2fd; font-family: monospace;">
                $270,000
            </td>
        </tr>
        <tr valign="middle">
            <td align="left" style="border: 1pt solid black; font-weight: bold;">
                Expenses
            </td>
            <td align="right" style="border: 1pt solid black; font-family: monospace;">
                $85,000
            </td>
            <td align="right" style="border: 1pt solid black; font-family: monospace;">
                $95,000
            </td>
            <td align="right" style="border: 1pt solid black; font-weight: bold;
                               background-color: #f8d7da; font-family: monospace;">
                $180,000
            </td>
        </tr>
        <tr valign="middle">
            <td align="left" style="border: 2pt solid #336699; font-weight: bold;
                               font-size: 12pt;">
                Net Profit
            </td>
            <td align="right" style="border: 2pt solid #336699; font-family: monospace;">
                $40,000
            </td>
            <td align="right" style="border: 2pt solid #336699; font-family: monospace;">
                $50,000
            </td>
            <td align="right" style="border: 2pt solid #336699; font-weight: bold;
                               font-size: 12pt; background-color: #d4edda;
                               font-family: monospace;">
                $90,000
            </td>
        </tr>
    </tbody>
</table>
```

### Product Catalog with Mixed Alignment

```html
<table cellpadding="12" style="width: 100%; border-collapse: collapse;">
    <thead align="center" valign="middle">
        <tr>
            <th style="border: 1pt solid #dddddd; background-color: #2c3e50; color: white;">
                Product Image
            </th>
            <th style="border: 1pt solid #dddddd; background-color: #2c3e50; color: white;">
                Product Name
            </th>
            <th style="border: 1pt solid #dddddd; background-color: #2c3e50; color: white;">
                Description
            </th>
            <th style="border: 1pt solid #dddddd; background-color: #2c3e50; color: white;">
                Price
            </th>
            <th style="border: 1pt solid #dddddd; background-color: #2c3e50; color: white;">
                Action
            </th>
        </tr>
    </thead>
    <tbody valign="middle">
        <tr>
            <td align="center" style="border: 1pt solid #dddddd;">
                [Image]
            </td>
            <td align="left" style="border: 1pt solid #dddddd; font-weight: bold;">
                Premium Widget
            </td>
            <td align="left" valign="top" style="border: 1pt solid #dddddd; font-size: 9pt;">
                High-quality widget with advanced features and extended warranty
            </td>
            <td align="right" style="border: 1pt solid #dddddd; font-size: 14pt;
                               font-weight: bold; color: #28a745;">
                $99.99
            </td>
            <td align="center" style="border: 1pt solid #dddddd;">
                <input type="button" value="Add to Cart"
                       style="padding: 8pt 16pt; background-color: #336699;
                              color: white; border: none; border-radius: 3pt;" />
            </td>
        </tr>
        <tr>
            <td align="center" style="border: 1pt solid #dddddd; background-color: #f9f9f9;">
                [Image]
            </td>
            <td align="left" style="border: 1pt solid #dddddd; background-color: #f9f9f9;
                               font-weight: bold;">
                Standard Gadget
            </td>
            <td align="left" valign="top"
                style="border: 1pt solid #dddddd; background-color: #f9f9f9; font-size: 9pt;">
                Reliable gadget for everyday use with one-year warranty
            </td>
            <td align="right" style="border: 1pt solid #dddddd; background-color: #f9f9f9;
                               font-size: 14pt; font-weight: bold; color: #28a745;">
                $49.99
            </td>
            <td align="center" style="border: 1pt solid #dddddd; background-color: #f9f9f9;">
                <input type="button" value="Add to Cart"
                       style="padding: 8pt 16pt; background-color: #336699;
                              color: white; border: none; border-radius: 3pt;" />
            </td>
        </tr>
        <tr>
            <td align="center" style="border: 1pt solid #dddddd;">
                [Image]
            </td>
            <td align="left" style="border: 1pt solid #dddddd; font-weight: bold;">
                Basic Tool
            </td>
            <td align="left" valign="top" style="border: 1pt solid #dddddd; font-size: 9pt;">
                Essential tool for basic tasks at an affordable price
            </td>
            <td align="right" style="border: 1pt solid #dddddd; font-size: 14pt;
                               font-weight: bold; color: #28a745;">
                $19.99
            </td>
            <td align="center" style="border: 1pt solid #dddddd;">
                <input type="button" value="Add to Cart"
                       style="padding: 8pt 16pt; background-color: #336699;
                              color: white; border: none; border-radius: 3pt;" />
            </td>
        </tr>
    </tbody>
</table>
```

### Invoice Table with Strategic Alignment

```html
<table cellpadding="10" style="width: 100%; border-collapse: collapse; font-size: 11pt;">
    <thead align="center" valign="middle">
        <tr>
            <th style="border-bottom: 2pt solid #336699; padding: 12pt; text-align: left;
                       color: #336699;">
                Item Description
            </th>
            <th style="border-bottom: 2pt solid #336699; padding: 12pt; color: #336699;">
                Quantity
            </th>
            <th style="border-bottom: 2pt solid #336699; padding: 12pt; color: #336699;">
                Unit Price
            </th>
            <th style="border-bottom: 2pt solid #336699; padding: 12pt; color: #336699;">
                Amount
            </th>
        </tr>
    </thead>
    <tbody valign="middle">
        <tr>
            <td align="left" style="border-bottom: 1pt solid #e0e0e0;">
                Professional Web Design Services
            </td>
            <td align="center" style="border-bottom: 1pt solid #e0e0e0;">
                40 hours
            </td>
            <td align="right" style="border-bottom: 1pt solid #e0e0e0; font-family: monospace;">
                $150.00
            </td>
            <td align="right" style="border-bottom: 1pt solid #e0e0e0; font-weight: bold;
                               font-family: monospace;">
                $6,000.00
            </td>
        </tr>
        <tr>
            <td align="left" style="border-bottom: 1pt solid #e0e0e0;">
                Logo Design and Branding
            </td>
            <td align="center" style="border-bottom: 1pt solid #e0e0e0;">
                1 project
            </td>
            <td align="right" style="border-bottom: 1pt solid #e0e0e0; font-family: monospace;">
                $1,500.00
            </td>
            <td align="right" style="border-bottom: 1pt solid #e0e0e0; font-weight: bold;
                               font-family: monospace;">
                $1,500.00
            </td>
        </tr>
        <tr>
            <td align="left" style="border-bottom: 1pt solid #e0e0e0;">
                Content Management System Setup
            </td>
            <td align="center" style="border-bottom: 1pt solid #e0e0e0;">
                1 setup
            </td>
            <td align="right" style="border-bottom: 1pt solid #e0e0e0; font-family: monospace;">
                $800.00
            </td>
            <td align="right" style="border-bottom: 1pt solid #e0e0e0; font-weight: bold;
                               font-family: monospace;">
                $800.00
            </td>
        </tr>
    </tbody>
    <tfoot>
        <tr valign="middle">
            <td colspan="3" align="right" style="padding: 12pt; font-size: 12pt; font-weight: bold;">
                Subtotal:
            </td>
            <td align="right" style="padding: 12pt; font-size: 12pt; font-weight: bold;
                               font-family: monospace;">
                $8,300.00
            </td>
        </tr>
        <tr valign="middle">
            <td colspan="3" align="right" style="padding: 12pt;">
                Tax (8%):
            </td>
            <td align="right" style="padding: 12pt; font-family: monospace;">
                $664.00
            </td>
        </tr>
        <tr valign="middle">
            <td colspan="3" align="right"
                style="border-top: 2pt solid #336699; padding: 12pt;
                       font-size: 14pt; font-weight: bold; color: #336699;">
                TOTAL:
            </td>
            <td align="right"
                style="border-top: 2pt solid #336699; padding: 12pt; font-size: 14pt;
                       font-weight: bold; font-family: monospace; background-color: #e3f2fd;
                       color: #336699;">
                $8,964.00
            </td>
        </tr>
    </tfoot>
</table>
```

### Schedule Table with Top Alignment

```html
<table cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <thead align="center" valign="middle">
        <tr>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Time
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Monday
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Tuesday
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Wednesday
            </th>
        </tr>
    </thead>
    <tbody align="left" valign="top">
        <tr>
            <td align="center" style="border: 1pt solid black; font-weight: bold;
                               background-color: #f0f0f0;">
                9:00 AM
            </td>
            <td style="border: 1pt solid black;">
                <strong>Team Meeting</strong><br />
                Conference Room A
            </td>
            <td style="border: 1pt solid black;">
                <strong>Project Review</strong><br />
                Conference Room B<br />
                <span style="font-size: 9pt; color: #666666;">With stakeholders</span>
            </td>
            <td style="border: 1pt solid black;">
                <strong>Client Call</strong><br />
                Virtual Meeting
            </td>
        </tr>
        <tr>
            <td align="center" style="border: 1pt solid black; font-weight: bold;
                               background-color: #f0f0f0;">
                11:00 AM
            </td>
            <td style="border: 1pt solid black;">
                <strong>Development</strong><br />
                Focus time
            </td>
            <td style="border: 1pt solid black;">
                <strong>Code Review</strong><br />
                Team collaboration
            </td>
            <td style="border: 1pt solid black;">
                <strong>Design Session</strong><br />
                Creative Room<br />
                <span style="font-size: 9pt; color: #666666;">Bring mockups</span>
            </td>
        </tr>
        <tr>
            <td align="center" style="border: 1pt solid black; font-weight: bold;
                               background-color: #f0f0f0;">
                2:00 PM
            </td>
            <td style="border: 1pt solid black;">
                <strong>Training</strong><br />
                New tools overview
            </td>
            <td style="border: 1pt solid black;">
                <strong>Development</strong><br />
                Focus time
            </td>
            <td style="border: 1pt solid black;">
                <strong>Sprint Planning</strong><br />
                Conference Room A<br />
                <span style="font-size: 9pt; color: #666666;">All team members</span>
            </td>
        </tr>
    </tbody>
</table>
```

### Data-Bound Table with Dynamic Alignment

```html
<!-- Model: {
    alignHeader: "center",
    alignData: "left",
    valignData: "middle",
    employees: [
        {name: "Alice Johnson", role: "Engineer", score: 95},
        {name: "Bob Smith", role: "Designer", score: 88},
        {name: "Carol White", role: "Manager", score: 92}
    ]
} -->

<table cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <thead align="{{model.alignHeader}}" valign="middle">
        <tr>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Employee Name
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Role
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Performance Score
            </th>
        </tr>
    </thead>
    <tbody align="{{model.alignData}}" valign="{{model.valignData}}">
        <template data-bind="{{model.employees}}">
            <tr>
                <td style="border: 1pt solid black;">{{.name}}</td>
                <td style="border: 1pt solid black;">{{.role}}</td>
                <td align="center" style="border: 1pt solid black; font-weight: bold;">
                    {{.score}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### Comparison Grid with All Alignments

```html
<table cellpadding="15" style="width: 100%; border-collapse: collapse;">
    <tr>
        <td align="left" valign="top"
            style="border: 2pt solid black; height: 100pt; background-color: #ffebee;">
            <strong>Top-Left</strong><br />
            align="left"<br />
            valign="top"
        </td>
        <td align="center" valign="top"
            style="border: 2pt solid black; height: 100pt; background-color: #fff3e0;">
            <strong>Top-Center</strong><br />
            align="center"<br />
            valign="top"
        </td>
        <td align="right" valign="top"
            style="border: 2pt solid black; height: 100pt; background-color: #e8f5e9;">
            <strong>Top-Right</strong><br />
            align="right"<br />
            valign="top"
        </td>
    </tr>
    <tr>
        <td align="left" valign="middle"
            style="border: 2pt solid black; height: 100pt; background-color: #f3e5f5;">
            <strong>Middle-Left</strong><br />
            align="left"<br />
            valign="middle"
        </td>
        <td align="center" valign="middle"
            style="border: 2pt solid black; height: 100pt; background-color: #e1f5fe;">
            <strong>Middle-Center</strong><br />
            align="center"<br />
            valign="middle"
        </td>
        <td align="right" valign="middle"
            style="border: 2pt solid black; height: 100pt; background-color: #fff9c4;">
            <strong>Middle-Right</strong><br />
            align="right"<br />
            valign="middle"
        </td>
    </tr>
    <tr>
        <td align="left" valign="bottom"
            style="border: 2pt solid black; height: 100pt; background-color: #fce4ec;">
            <strong>Bottom-Left</strong><br />
            align="left"<br />
            valign="bottom"
        </td>
        <td align="center" valign="bottom"
            style="border: 2pt solid black; height: 100pt; background-color: #f1f8e9;">
            <strong>Bottom-Center</strong><br />
            align="center"<br />
            valign="bottom"
        </td>
        <td align="right" valign="bottom"
            style="border: 2pt solid black; height: 100pt; background-color: #e0f2f1;">
            <strong>Bottom-Right</strong><br />
            align="right"<br />
            valign="bottom"
        </td>
    </tr>
</table>
```

---

## See Also

- [table](/reference/htmltags/table.html) - Table element
- [td](/reference/htmltags/td.html) - Table cell elements (td and th)
- [tr](/reference/htmltags/tr.html) - Table row element
- [cellpadding](/reference/htmlattributes/cellpadding_cellspacing.html) - Table spacing attributes
- [border](/reference/htmlattributes/border.html) - Border attribute
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Dynamic data binding

---
