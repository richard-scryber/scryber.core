---
layout: default
title: border
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @border : The Border Attribute

The `border` attribute provides a simplified way to add borders to tables in PDF documents. This legacy HTML attribute sets a uniform border width around the table and its cells, offering quick border styling for table layouts.

## Usage

The `border` attribute controls table borders:
- Sets border width for the table and all cells
- Accepts numeric values in points or pixels
- Applied to `<table>` elements
- Affects table outer border and cell borders
- Simplified alternative to CSS border properties
- `border="0"` removes all borders
- Can be overridden by CSS border styles

```html
<!-- Table with 1pt border -->
<table border="1">
    <tr>
        <td>Cell 1</td>
        <td>Cell 2</td>
    </tr>
</table>

<!-- Table with thicker border -->
<table border="3">
    <tr>
        <td>Cell 1</td>
        <td>Cell 2</td>
    </tr>
</table>

<!-- Table with no border -->
<table border="0">
    <tr>
        <td>Cell 1</td>
        <td>Cell 2</td>
    </tr>
</table>
```

---

## Supported Elements

The `border` attribute is supported by the following element:

| Element | Description |
|---------|-------------|
| `<table>` | Table element for structured data |

**Note**: The `border` attribute can also be used on `<img>` elements for image borders, but this documentation focuses on table usage.

---

## Border Attribute Values

### Syntax

```html
<table border="value">
```

Where `value` is a number representing border width:
- `0`: No border (removes all borders)
- `1`: Thin border (1pt/1px) - most common
- `2+`: Thicker borders

### Common Values

| Value | Description | Visual Effect |
|-------|-------------|---------------|
| `0` | No border | Completely borderless table |
| `1` | Standard border (default) | 1pt/1px borders on table and cells |
| `2` | Medium border | 2pt/2px borders on table and cells |
| `3` | Thick border | 3pt/3px borders on table and cells |
| `5+` | Very thick border | Heavy borders for emphasis |

### Examples

```html
<!-- No border -->
<table border="0">...</table>

<!-- Thin border (standard) -->
<table border="1">...</table>

<!-- Medium border -->
<table border="2">...</table>

<!-- Thick border -->
<table border="5">...</table>
```

---

## Default Behavior

### Scryber Defaults

Without the `border` attribute, Scryber tables have:
- **Cell borders**: `1pt solid gray (#999999)`
- **Table border**: Usually matches cell borders
- **Default cell padding**: `2pt`
- **Default cell spacing**: `2pt`

To create a completely borderless table:

```html
<table border="0" cellpadding="0" cellspacing="0">
    <!-- No borders, padding, or spacing -->
</table>
```

### Border Application

The `border` attribute affects:
1. **Outer table border**: Border around the entire table
2. **Cell borders**: Borders around each `<td>` and `<th>` cell
3. **All sides**: Top, right, bottom, and left borders

**Note**: The attribute does not directly control border color or style (solid, dashed, etc.).

---

## CSS Equivalents

### Border Attribute to CSS

The `border` attribute can be replaced with more flexible CSS:

```html
<!-- Using border attribute -->
<table border="1">
    <tr>
        <td>Content</td>
    </tr>
</table>

<!-- Equivalent CSS (collapsed borders) -->
<table style="border: 1pt solid black; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid black;">Content</td>
    </tr>
</table>

<!-- Equivalent CSS (separate borders) -->
<table style="border: 1pt solid black; border-collapse: separate;">
    <tr>
        <td style="border: 1pt solid black;">Content</td>
    </tr>
</table>
```

### CSS Advantages

CSS provides more control:
- **Border color**: Choose any color
- **Border style**: Solid, dashed, dotted, double, etc.
- **Individual sides**: Different borders for top, right, bottom, left
- **Per-cell control**: Customize individual cell borders

```html
<table style="border: 2pt solid #336699; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid #cccccc;">Cell 1</td>
        <td style="border: 1pt dashed #999999;">Cell 2</td>
    </tr>
</table>
```

---

## Border Collapse Interaction

The `border-collapse` CSS property affects how borders render:

### Collapsed Borders

```html
<table border="1" style="border-collapse: collapse;">
    <!-- Single-line borders between cells -->
    <!-- Adjacent cell borders merge together -->
</table>
```

**Effect**: Borders between adjacent cells merge into a single line.

### Separate Borders

```html
<table border="1" style="border-collapse: separate;">
    <!-- Each cell has its own border -->
    <!-- Gaps visible between cells (controlled by cellspacing) -->
</table>
```

**Effect**: Each cell maintains its own distinct border with visible gaps.

### Default in Scryber

Scryber typically uses `border-collapse: separate` by default, but this can vary.

---

## Binding Values

The `border` attribute supports data binding:

### Static Border

```html
<table border="1">
    <tr>
        <td>Static border</td>
    </tr>
</table>
```

### Dynamic Border with Data Binding

```html
<!-- Model: { borderWidth: 2 } -->
<table border="{{model.borderWidth}}">
    <tr>
        <td>Dynamic border</td>
    </tr>
</table>
```

### Conditional Border

```html
<!-- Model: { showBorders: true } -->
<table border="{{model.showBorders ? '1' : '0'}}">
    <tr>
        <td>Conditional border</td>
    </tr>
</table>

<!-- Or with ternary for different widths -->
<!-- Model: { isHighlighted: true } -->
<table border="{{model.isHighlighted ? '3' : '1'}}">
    <tr>
        <td>Content</td>
    </tr>
</table>
```

---

## Notes

### When to Use the Border Attribute

**Use `border` attribute when**:
- Quick prototyping or simple tables
- Need uniform borders across entire table
- Working with legacy HTML templates
- Want simple, consistent border styling

**Use CSS border properties when**:
- Need different border colors
- Want varying border styles (dashed, dotted, etc.)
- Require per-cell border customization
- Building complex table designs

### Limitations

1. **No color control**: Always uses default color (usually black or gray)
2. **No style control**: Always solid borders (cannot create dashed, dotted, etc.)
3. **Uniform width**: All borders have the same width
4. **Cannot control individual sides**: All four sides get the same border

### Border Width Units

The border value is interpreted as:
- **Points (pt)** in PDF context
- **Pixels (px)** in web context
- Scryber typically treats the value as points

### Removing Default Borders

To create tables without borders:

```html
<!-- Remove all borders -->
<table border="0">
    <tr>
        <td>No borders</td>
    </tr>
</table>

<!-- Or use CSS -->
<table style="border: none;">
    <tr>
        <td style="border: none;">No borders</td>
    </tr>
</table>
```

### Mixing Attribute and CSS

You can combine the `border` attribute with CSS for enhanced control:

```html
<table border="1" style="border-color: #336699;">
    <tr>
        <td style="border-color: #cccccc;">Custom colored borders</td>
    </tr>
</table>
```

**Note**: CSS properties take precedence over the attribute.

### Performance Considerations

The `border` attribute has no performance impact on PDF generation.

---

## Examples

### Basic Border Widths

```html
<!-- No border -->
<table border="0" cellpadding="10" style="width: 100%; margin-bottom: 15pt;">
    <tr>
        <td style="background-color: #f0f0f0;">No border</td>
        <td style="background-color: #f0f0f0;">border="0"</td>
        <td style="background-color: #f0f0f0;">Borderless cells</td>
    </tr>
</table>

<!-- Thin border (1pt) -->
<table border="1" cellpadding="10" style="width: 100%; margin-bottom: 15pt;">
    <tr>
        <td>Thin border</td>
        <td>border="1"</td>
        <td>Standard thickness</td>
    </tr>
</table>

<!-- Medium border (2pt) -->
<table border="2" cellpadding="10" style="width: 100%; margin-bottom: 15pt;">
    <tr>
        <td>Medium border</td>
        <td>border="2"</td>
        <td>Slightly thicker</td>
    </tr>
</table>

<!-- Thick border (3pt) -->
<table border="3" cellpadding="10" style="width: 100%; margin-bottom: 15pt;">
    <tr>
        <td>Thick border</td>
        <td>border="3"</td>
        <td>Very visible</td>
    </tr>
</table>

<!-- Very thick border (5pt) -->
<table border="5" cellpadding="10" style="width: 100%; margin-bottom: 15pt;">
    <tr>
        <td>Very thick border</td>
        <td>border="5"</td>
        <td>Heavy emphasis</td>
    </tr>
</table>
```

### Standard Data Table with Border

```html
<table border="1" cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="background-color: #336699; color: white;">ID</th>
            <th style="background-color: #336699; color: white;">Name</th>
            <th style="background-color: #336699; color: white;">Email</th>
            <th style="background-color: #336699; color: white;">Status</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>001</td>
            <td>John Doe</td>
            <td>john.doe@example.com</td>
            <td style="color: #28a745; font-weight: bold;">Active</td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td>002</td>
            <td>Jane Smith</td>
            <td>jane.smith@example.com</td>
            <td style="color: #28a745; font-weight: bold;">Active</td>
        </tr>
        <tr>
            <td>003</td>
            <td>Bob Johnson</td>
            <td>bob.johnson@example.com</td>
            <td style="color: #dc3545; font-weight: bold;">Inactive</td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td>004</td>
            <td>Alice Williams</td>
            <td>alice.w@example.com</td>
            <td style="color: #28a745; font-weight: bold;">Active</td>
        </tr>
    </tbody>
</table>
```

### Borderless Layout Table

```html
<table border="0" cellpadding="15" cellspacing="0" style="width: 100%;">
    <tr>
        <td style="width: 30%; vertical-align: top;">
            <h3 style="color: #336699; margin-top: 0;">Company Info</h3>
            <p><strong>Acme Corporation</strong></p>
            <p>123 Business Street<br />
               Suite 100<br />
               City, ST 12345</p>
            <p>Phone: (555) 123-4567<br />
               Email: info@acme.com</p>
        </td>
        <td style="width: 70%; vertical-align: top;">
            <h3 style="color: #336699; margin-top: 0;">Invoice Details</h3>
            <table border="1" cellpadding="8" style="width: 100%; border-collapse: collapse;">
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Invoice #:</td>
                    <td>INV-2024-001</td>
                </tr>
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Date:</td>
                    <td>January 15, 2024</td>
                </tr>
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Due Date:</td>
                    <td style="color: #d32f2f; font-weight: bold;">February 15, 2024</td>
                </tr>
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Amount:</td>
                    <td style="font-size: 14pt; font-weight: bold; color: #336699;">$1,234.56</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
```

### Financial Report with Thick Border

```html
<table border="3" cellpadding="12" style="width: 100%; border-collapse: collapse;
              border-color: #336699;">
    <thead>
        <tr>
            <th style="background-color: #336699; color: white; text-align: left; padding: 15pt;">
                Quarter
            </th>
            <th style="background-color: #336699; color: white; text-align: right; padding: 15pt;">
                Revenue
            </th>
            <th style="background-color: #336699; color: white; text-align: right; padding: 15pt;">
                Expenses
            </th>
            <th style="background-color: #336699; color: white; text-align: right; padding: 15pt;">
                Profit
            </th>
            <th style="background-color: #336699; color: white; text-align: center; padding: 15pt;">
                Growth
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="font-weight: bold;">Q1 2024</td>
            <td style="text-align: right; font-family: monospace;">$125,000</td>
            <td style="text-align: right; font-family: monospace;">$85,000</td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       color: #28a745;">$40,000</td>
            <td style="text-align: center; font-weight: bold; color: #28a745;">↑ 8%</td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="font-weight: bold;">Q2 2024</td>
            <td style="text-align: right; font-family: monospace;">$145,000</td>
            <td style="text-align: right; font-family: monospace;">$95,000</td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       color: #28a745;">$50,000</td>
            <td style="text-align: center; font-weight: bold; color: #28a745;">↑ 16%</td>
        </tr>
        <tr>
            <td style="font-weight: bold;">Q3 2024</td>
            <td style="text-align: right; font-family: monospace;">$138,000</td>
            <td style="text-align: right; font-family: monospace;">$92,000</td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       color: #28a745;">$46,000</td>
            <td style="text-align: center; font-weight: bold; color: #28a745;">↑ 10%</td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="font-weight: bold;">Q4 2024</td>
            <td style="text-align: right; font-family: monospace;">$155,000</td>
            <td style="text-align: right; font-family: monospace;">$98,000</td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       color: #28a745;">$57,000</td>
            <td style="text-align: center; font-weight: bold; color: #28a745;">↑ 13%</td>
        </tr>
    </tbody>
    <tfoot>
        <tr style="background-color: #e3f2fd;">
            <td style="font-weight: bold; font-size: 12pt;">ANNUAL TOTAL</td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       font-size: 12pt;">$563,000</td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       font-size: 12pt;">$370,000</td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       font-size: 12pt; color: #336699;">$193,000</td>
            <td style="text-align: center; font-weight: bold; font-size: 12pt;
                       color: #336699;">↑ 12%</td>
        </tr>
    </tfoot>
</table>
```

### Product Comparison with Different Borders

```html
<h3 style="color: #336699;">Product Comparison</h3>

<table border="2" cellpadding="15" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="background-color: #2c3e50; color: white;">Feature</th>
            <th style="background-color: #2c3e50; color: white;">Basic Plan</th>
            <th style="background-color: #336699; color: white;">Pro Plan</th>
            <th style="background-color: #2c3e50; color: white;">Enterprise</th>
        </tr>
    </thead>
    <tbody style="text-align: center;">
        <tr>
            <td style="text-align: left; font-weight: bold;">Storage</td>
            <td>10 GB</td>
            <td style="background-color: #e3f2fd; font-weight: bold;">100 GB</td>
            <td>Unlimited</td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="text-align: left; font-weight: bold;">Users</td>
            <td>1</td>
            <td style="background-color: #e3f2fd; font-weight: bold;">5</td>
            <td>Unlimited</td>
        </tr>
        <tr>
            <td style="text-align: left; font-weight: bold;">Support</td>
            <td>Email</td>
            <td style="background-color: #e3f2fd; font-weight: bold;">Priority</td>
            <td>24/7 Dedicated</td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="text-align: left; font-weight: bold;">API Access</td>
            <td>—</td>
            <td style="background-color: #e3f2fd; font-weight: bold;">✓</td>
            <td>✓</td>
        </tr>
        <tr>
            <td style="text-align: left; font-weight: bold;">Custom Branding</td>
            <td>—</td>
            <td style="background-color: #e3f2fd; font-weight: bold;">—</td>
            <td>✓</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td style="text-align: left; font-weight: bold; font-size: 12pt;">Price</td>
            <td style="font-size: 14pt; font-weight: bold;">$9/mo</td>
            <td style="background-color: #336699; color: white; font-size: 14pt;
                       font-weight: bold;">$29/mo</td>
            <td style="font-size: 14pt; font-weight: bold;">Contact Us</td>
        </tr>
    </tfoot>
</table>
```

### Schedule Table with Minimal Border

```html
<table border="1" cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th style="text-align: left;">Time</th>
            <th>Monday</th>
            <th>Tuesday</th>
            <th>Wednesday</th>
            <th>Thursday</th>
            <th>Friday</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="background-color: #f0f0f0; font-weight: bold;">9:00 AM</td>
            <td>Team Meeting</td>
            <td>Development</td>
            <td>Client Call</td>
            <td>Development</td>
            <td>Code Review</td>
        </tr>
        <tr>
            <td style="background-color: #f0f0f0; font-weight: bold;">11:00 AM</td>
            <td>Development</td>
            <td>Sprint Planning</td>
            <td>Development</td>
            <td>Training</td>
            <td>Development</td>
        </tr>
        <tr>
            <td style="background-color: #f0f0f0; font-weight: bold;">1:00 PM</td>
            <td colspan="5" style="text-align: center; background-color: #fff3cd;">
                Lunch Break
            </td>
        </tr>
        <tr>
            <td style="background-color: #f0f0f0; font-weight: bold;">2:00 PM</td>
            <td>Development</td>
            <td>Design Review</td>
            <td>Development</td>
            <td>Team Sync</td>
            <td>Retrospective</td>
        </tr>
        <tr>
            <td style="background-color: #f0f0f0; font-weight: bold;">4:00 PM</td>
            <td>Code Review</td>
            <td>Development</td>
            <td>Documentation</td>
            <td>Development</td>
            <td style="background-color: #d4edda;">Demo Day</td>
        </tr>
    </tbody>
</table>
```

### Invoice Table with Custom Border Styling

```html
<table border="1" cellpadding="12" style="width: 100%; border-collapse: collapse;
              border: 2pt solid #336699;">
    <thead>
        <tr>
            <th style="background-color: #336699; color: white; text-align: left; padding: 15pt;
                       border: 1pt solid #336699;">
                Description
            </th>
            <th style="background-color: #336699; color: white; text-align: center; padding: 15pt;
                       border: 1pt solid #336699;">
                Qty
            </th>
            <th style="background-color: #336699; color: white; text-align: right; padding: 15pt;
                       border: 1pt solid #336699;">
                Rate
            </th>
            <th style="background-color: #336699; color: white; text-align: right; padding: 15pt;
                       border: 1pt solid #336699;">
                Amount
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #cccccc;">Web Development Services</td>
            <td style="text-align: center; border: 1pt solid #cccccc;">40 hrs</td>
            <td style="text-align: right; font-family: monospace; border: 1pt solid #cccccc;">
                $150.00
            </td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       border: 1pt solid #cccccc;">
                $6,000.00
            </td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="border: 1pt solid #cccccc;">Design Consultation</td>
            <td style="text-align: center; border: 1pt solid #cccccc;">10 hrs</td>
            <td style="text-align: right; font-family: monospace; border: 1pt solid #cccccc;">
                $120.00
            </td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       border: 1pt solid #cccccc;">
                $1,200.00
            </td>
        </tr>
        <tr>
            <td style="border: 1pt solid #cccccc;">Server Configuration</td>
            <td style="text-align: center; border: 1pt solid #cccccc;">5 hrs</td>
            <td style="text-align: right; font-family: monospace; border: 1pt solid #cccccc;">
                $100.00
            </td>
            <td style="text-align: right; font-weight: bold; font-family: monospace;
                       border: 1pt solid #cccccc;">
                $500.00
            </td>
        </tr>
    </tbody>
    <tfoot>
        <tr style="background-color: #e3f2fd;">
            <td colspan="3" style="text-align: right; font-size: 14pt; font-weight: bold;
                                   padding: 15pt; border: 2pt solid #336699;">
                TOTAL:
            </td>
            <td style="text-align: right; font-size: 14pt; font-weight: bold;
                       font-family: monospace; padding: 15pt; border: 2pt solid #336699;
                       color: #336699;">
                $7,700.00
            </td>
        </tr>
    </tfoot>
</table>
```

### Data Table with Zebra Striping and Border

```html
<table border="1" cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #2c3e50; color: white;">
            <th style="text-align: left; padding: 12pt;">Employee Name</th>
            <th style="text-align: center; padding: 12pt;">Department</th>
            <th style="text-align: center; padding: 12pt;">Performance</th>
            <th style="text-align: center; padding: 12pt;">Rating</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Alice Johnson</td>
            <td style="text-align: center;">Engineering</td>
            <td style="text-align: center; font-weight: bold;">95</td>
            <td style="text-align: center; background-color: #2ecc71; color: white;
                       font-weight: bold;">
                Excellent
            </td>
        </tr>
        <tr style="background-color: #ecf0f1;">
            <td>Bob Smith</td>
            <td style="text-align: center;">Marketing</td>
            <td style="text-align: center; font-weight: bold;">88</td>
            <td style="text-align: center; background-color: #3498db; color: white;
                       font-weight: bold;">
                Good
            </td>
        </tr>
        <tr>
            <td>Carol White</td>
            <td style="text-align: center;">Sales</td>
            <td style="text-align: center; font-weight: bold;">92</td>
            <td style="text-align: center; background-color: #2ecc71; color: white;
                       font-weight: bold;">
                Excellent
            </td>
        </tr>
        <tr style="background-color: #ecf0f1;">
            <td>David Brown</td>
            <td style="text-align: center;">Operations</td>
            <td style="text-align: center; font-weight: bold;">85</td>
            <td style="text-align: center; background-color: #3498db; color: white;
                       font-weight: bold;">
                Good
            </td>
        </tr>
        <tr>
            <td>Eve Davis</td>
            <td style="text-align: center;">Finance</td>
            <td style="text-align: center; font-weight: bold;">78</td>
            <td style="text-align: center; background-color: #f39c12; color: white;
                       font-weight: bold;">
                Satisfactory
            </td>
        </tr>
    </tbody>
    <tfoot>
        <tr style="background-color: #34495e; color: white;">
            <td colspan="2" style="text-align: right; font-weight: bold; padding: 12pt;">
                Average Score:
            </td>
            <td colspan="2" style="text-align: center; font-weight: bold; font-size: 13pt;
                                   padding: 12pt;">
                87.6
            </td>
        </tr>
    </tfoot>
</table>
```

### Calendar with Border

```html
<table border="1" cellpadding="12" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th>Sunday</th>
            <th>Monday</th>
            <th>Tuesday</th>
            <th>Wednesday</th>
            <th>Thursday</th>
            <th>Friday</th>
            <th>Saturday</th>
        </tr>
    </thead>
    <tbody style="text-align: center;">
        <tr>
            <td style="color: #cccccc;">30</td>
            <td style="color: #cccccc;">31</td>
            <td style="background-color: #fff3cd; font-weight: bold;">1</td>
            <td>2</td>
            <td>3</td>
            <td>4</td>
            <td>5</td>
        </tr>
        <tr>
            <td>6</td>
            <td>7</td>
            <td>8</td>
            <td>9</td>
            <td>10</td>
            <td>11</td>
            <td>12</td>
        </tr>
        <tr>
            <td>13</td>
            <td>14</td>
            <td style="background-color: #d4edda; font-weight: bold;">15</td>
            <td>16</td>
            <td>17</td>
            <td>18</td>
            <td>19</td>
        </tr>
        <tr>
            <td>20</td>
            <td>21</td>
            <td>22</td>
            <td>23</td>
            <td>24</td>
            <td>25</td>
            <td>26</td>
        </tr>
        <tr>
            <td>27</td>
            <td>28</td>
            <td>29</td>
            <td>30</td>
            <td>31</td>
            <td style="color: #cccccc;">1</td>
            <td style="color: #cccccc;">2</td>
        </tr>
    </tbody>
</table>
```

### Data-Bound Table with Dynamic Border

```html
<!-- Model: {
    borderWidth: 2,
    showBorders: true,
    products: [
        {name: "Widget A", price: 25.00, stock: 150},
        {name: "Gadget B", price: 50.00, stock: 75},
        {name: "Tool C", price: 15.00, stock: 200}
    ]
} -->

<table border="{{model.showBorders ? model.borderWidth : '0'}}"
       cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th style="text-align: left;">Product Name</th>
            <th style="text-align: right;">Price</th>
            <th style="text-align: center;">In Stock</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.products}}">
            <tr>
                <td>{{.name}}</td>
                <td style="text-align: right; font-family: monospace;">${{.price}}</td>
                <td style="text-align: center; font-weight: bold;">{{.stock}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### Nested Tables with Different Borders

```html
<table border="2" cellpadding="15" style="width: 100%; border-collapse: collapse;
              border-color: #336699;">
    <tr>
        <td style="vertical-align: top;">
            <h4 style="margin-top: 0; color: #336699;">Customer Information</h4>
            <table border="1" cellpadding="8" style="width: 100%; border-collapse: collapse;">
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Name:</td>
                    <td>John Doe</td>
                </tr>
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Email:</td>
                    <td>john@example.com</td>
                </tr>
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Phone:</td>
                    <td>(555) 123-4567</td>
                </tr>
            </table>
        </td>
        <td style="vertical-align: top;">
            <h4 style="margin-top: 0; color: #336699;">Order Summary</h4>
            <table border="1" cellpadding="8" style="width: 100%; border-collapse: collapse;">
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Order #:</td>
                    <td>ORD-2024-001</td>
                </tr>
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Date:</td>
                    <td>Jan 15, 2024</td>
                </tr>
                <tr>
                    <td style="background-color: #f0f0f0; font-weight: bold;">Total:</td>
                    <td style="font-size: 13pt; font-weight: bold; color: #336699;">$299.99</td>
                </tr>
            </table>
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
- [align](/reference/htmlattributes/align.html) - Alignment attributes
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Dynamic data binding

---
