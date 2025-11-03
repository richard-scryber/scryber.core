---
layout: default
title: data-format
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-format : The Number and Date Formatting Attribute

The `data-format` attribute specifies the display format for numeric values and dates in Scryber PDF documents. It uses .NET-style format strings to control how numbers, currency, percentages, and dates are rendered in `<num>`, `<time>`, and `<page>` elements.

---

## Summary

The `data-format` attribute enables precise control over the textual representation of numeric and temporal values in PDF output. It applies .NET Standard or Custom format strings to transform raw values into human-readable, locale-appropriate text.

This attribute is essential for:
- Currency and financial displays
- Percentage calculations
- Date and time formatting
- Decimal precision control
- Custom number representations
- Page numbering formats
- Scientific notation
- Locale-specific formatting

The formatting occurs during document generation, allowing the same numeric data to be displayed differently in various contexts using different format strings.

---

## Usage

The `data-format` attribute accepts .NET format strings and is applied to elements that display numeric or temporal values.

### Basic Syntax

```html
<!-- Currency formatting -->
<num value="1234.56" data-format="C2" />
<!-- Output: $1,234.56 -->

<!-- Date formatting -->
<time value="2024-10-13" data-format="MMMM d, yyyy" />
<!-- Output: October 13, 2024 -->

<!-- Custom number format -->
<num value="42" data-format="#0.00" />
<!-- Output: 42.00 -->

<!-- Page number formatting -->
<page property="page" data-format="Page {0} of {1}" />
<!-- Output: Page 3 of 10 -->
```

---

## Supported Elements

The `data-format` attribute is supported on the following elements:

### Numeric Elements
- `<num>` - Number display element (HTMLNumber)
  - Uses `NumberFormat` property
  - Formats the `value` or parsed text content

### Temporal Elements
- `<time>` - Date and time display element (HTMLTime)
  - Uses `DateFormat` property
  - Formats date/time values

### Page Numbering Elements
- `<page>` - Page number element (HTMLPageNumber)
  - Uses `DisplayFormat` property
  - Formats page numbers with placeholders

---

## Binding Values

### Standard Numeric Format Strings

| Format | Description | Example Input | Example Output |
|--------|-------------|---------------|----------------|
| `C` or `C2` | Currency | 1234.56 | $1,234.56 |
| `D` or `D5` | Decimal (integers) | 42 | 00042 |
| `E` or `E2` | Exponential | 1234.5 | 1.23E+003 |
| `F` or `F2` | Fixed-point | 1234.567 | 1234.57 |
| `G` | General | 1234.567 | 1234.567 |
| `N` or `N2` | Number with separators | 1234.56 | 1,234.56 |
| `P` or `P1` | Percentage | 0.1234 | 12.3% |
| `X` or `X8` | Hexadecimal | 255 | FF |

### Custom Numeric Format Strings

| Pattern | Description | Example |
|---------|-------------|---------|
| `0` | Zero placeholder | `#0.00` → 1.00 |
| `#` | Digit placeholder | `#.##` → 1.5 |
| `.` | Decimal point | `0.00` → 1.50 |
| `,` | Thousands separator | `#,##0` → 1,234 |
| `%` | Percentage | `0.00%` → 12.34% |
| `‰` | Per mille | `0.00‰` → 12.34‰ |
| `E0` | Exponent | `0.00E0` → 1.23E2 |
| `'text'` | Literal text | `$#,##0.00` → $1,234.56 |
| `;` | Section separator | `#,##0;(#,##0)` |

### Standard Date/Time Format Strings

| Format | Description | Example Output |
|--------|-------------|----------------|
| `d` | Short date | 10/13/2024 |
| `D` | Long date | Sunday, October 13, 2024 |
| `t` | Short time | 3:45 PM |
| `T` | Long time | 3:45:30 PM |
| `f` | Full date/short time | Sunday, October 13, 2024 3:45 PM |
| `F` | Full date/long time | Sunday, October 13, 2024 3:45:30 PM |
| `g` | General date/short time | 10/13/2024 3:45 PM |
| `G` | General date/long time | 10/13/2024 3:45:30 PM |
| `M` or `m` | Month day | October 13 |
| `Y` or `y` | Year month | October 2024 |

### Custom Date/Time Format Strings

| Pattern | Description | Example |
|---------|-------------|---------|
| `yyyy` | 4-digit year | 2024 |
| `yy` | 2-digit year | 24 |
| `MMMM` | Full month name | October |
| `MMM` | Abbreviated month | Oct |
| `MM` | 2-digit month | 10 |
| `M` | Month | 10 |
| `dddd` | Full day name | Sunday |
| `ddd` | Abbreviated day | Sun |
| `dd` | 2-digit day | 13 |
| `d` | Day | 13 |
| `HH` | Hour (24-hour) | 15 |
| `hh` | Hour (12-hour) | 03 |
| `mm` | Minutes | 45 |
| `ss` | Seconds | 30 |
| `tt` | AM/PM | PM |

### Page Number Format Placeholders

For `<page>` elements, the format string uses placeholders:

| Placeholder | Description |
|-------------|-------------|
| `{0}` | Current page number |
| `{1}` | Total page count |
| `{2}` | Current section page number |
| `{3}` | Section page count |

---

## Notes

### Format String Evaluation

1. **Timing**: Format is applied during layout/render phase
2. **Culture**: Respects document locale settings
3. **Validation**: Invalid format strings may cause errors
4. **Fallback**: Bad formats typically fall back to ToString()

### Number Formatting Specifics

**Integer vs Decimal**
- Some formats (D, X) only work with integers
- Decimal formats work with all numeric types
- Automatic conversion where possible

**Precision Control**
- Default precision varies by format
- Explicit precision recommended for currency
- Trailing zeros behavior controlled by format

**Negative Numbers**
- Format sections can specify negative formatting
- Syntax: `positive;negative;zero`
- Example: `#,##0;(#,##0);Zero`

### Date/Time Formatting Specifics

**Date Parsing**
- Input must be parseable as DateTime
- ISO 8601 format recommended
- Locale-specific parsing supported

**Time Zones**
- No automatic timezone conversion
- Display as provided
- Include timezone in format if needed

### Page Number Formatting Specifics

**Placeholder Formatting**
- Placeholders can have their own format
- Example: `{0:D3}` for zero-padded page numbers
- Combine with text: `Page {0} of {1}`

**Total Page Count**
- May require multiple passes
- Use `data-page-hint` for optimization
- Total calculated after full layout

### Culture and Localization

Format strings respect the current culture:
- Currency symbols vary by locale
- Date formats differ by region
- Decimal and thousands separators change
- Set culture at document level

### Performance Considerations

- Format strings compiled once per unique format
- Caching reduces overhead
- Complex custom formats slower than standard
- Avoid format changes in tight loops

### Expression Integration

Format strings can be bound from data:

```html
<!-- Format from model -->
<num value="{{.price}}" data-format="{{model.currencyFormat}}" />

<!-- Conditional format -->
<num value="{{.value}}" data-format="{{.isPercentage ? 'P2' : 'N2'}}" />
```

---

## Examples

### 1. Basic Currency Formatting

Display money with currency symbol:

```html
<!-- Value: 1234.56 -->
<num value="1234.56" data-format="C2" />
<!-- Output: $1,234.56 -->

<!-- Bound value -->
<num value="{{model.price}}" data-format="C2" />
```

### 2. Currency with Custom Symbol

Use custom currency format:

```html
<num value="1234.56" data-format="£#,##0.00" />
<!-- Output: £1,234.56 -->

<num value="1234.56" data-format="€#,##0.00" />
<!-- Output: €1,234.56 -->
```

### 3. Percentage Display

Show values as percentages:

```html
<!-- Value: 0.1234 -->
<num value="0.1234" data-format="P1" />
<!-- Output: 12.3% -->

<num value="0.8567" data-format="P2" />
<!-- Output: 85.67% -->

<!-- From calculation -->
<num value="{{.sold / .total}}" data-format="P0" />
<!-- Output: 45% -->
```

### 4. Fixed Decimal Places

Control decimal precision:

```html
<!-- Always 2 decimal places -->
<num value="42" data-format="F2" />
<!-- Output: 42.00 -->

<num value="3.14159" data-format="F2" />
<!-- Output: 3.14 -->

<num value="{{.measurement}}" data-format="F4" />
<!-- Output: 1.4142 -->
```

### 5. Thousands Separator

Format large numbers with separators:

```html
<num value="1234567" data-format="N0" />
<!-- Output: 1,234,567 -->

<num value="1234567.89" data-format="N2" />
<!-- Output: 1,234,567.89 -->
```

### 6. Zero-Padded Numbers

Pad with leading zeros:

```html
<!-- Invoice numbers -->
<num value="42" data-format="D5" />
<!-- Output: 00042 -->

<!-- Item IDs -->
<num value="{{.itemId}}" data-format="D6" />
<!-- Output: 000123 -->
```

### 7. Scientific Notation

Display in exponential format:

```html
<num value="1234567.89" data-format="E2" />
<!-- Output: 1.23E+006 -->

<num value="0.00001234" data-format="E3" />
<!-- Output: 1.234E-005 -->
```

### 8. Custom Format with Prefix/Suffix

Add custom text around numbers:

```html
<num value="75" data-format="'Score: '#0' points'" />
<!-- Output: Score: 75 points -->

<num value="{{.temperature}}" data-format="#0.0'°C'" />
<!-- Output: 23.5°C -->
```

### 9. Positive/Negative/Zero Formats

Different formats for different values:

```html
<!-- Format: positive;negative;zero -->
<num value="100" data-format="#,##0;(#,##0);Zero" />
<!-- Output: 100 -->

<num value="-50" data-format="#,##0;(#,##0);Zero" />
<!-- Output: (50) -->

<num value="0" data-format="#,##0;(#,##0);'No Value'" />
<!-- Output: No Value -->
```

### 10. Date - Long Format

Display full date:

```html
<!-- Value: 2024-10-13 -->
<time value="2024-10-13" data-format="D" />
<!-- Output: Sunday, October 13, 2024 -->
```

### 11. Date - Custom Format

Create custom date display:

```html
<time value="2024-10-13" data-format="MMMM d, yyyy" />
<!-- Output: October 13, 2024 -->

<time value="2024-10-13" data-format="MMM d, yy" />
<!-- Output: Oct 13, 24 -->

<time value="2024-10-13" data-format="dddd, MMM d" />
<!-- Output: Sunday, Oct 13 -->
```

### 12. Time Formatting

Display time values:

```html
<time value="2024-10-13T15:45:00" data-format="t" />
<!-- Output: 3:45 PM -->

<time value="2024-10-13T15:45:30" data-format="T" />
<!-- Output: 3:45:30 PM -->

<time value="2024-10-13T15:45:00" data-format="HH:mm" />
<!-- Output: 15:45 -->
```

### 13. Date and Time Combined

Show both date and time:

```html
<time value="2024-10-13T15:45:00" data-format="f" />
<!-- Output: Sunday, October 13, 2024 3:45 PM -->

<time value="2024-10-13T15:45:30" data-format="yyyy-MM-dd HH:mm:ss" />
<!-- Output: 2024-10-13 15:45:30 -->
```

### 14. Page Number - Simple

Basic page numbering:

```html
<page property="page" data-format="{0}" />
<!-- Output: 3 -->

<page property="page" data-format="Page {0}" />
<!-- Output: Page 3 -->
```

### 15. Page Number - With Total

Show current and total pages:

```html
<page property="page" data-format="Page {0} of {1}" />
<!-- Output: Page 3 of 10 -->

<page property="page" data-format="{0}/{1}" />
<!-- Output: 3/10 -->
```

### 16. Page Number - Formatted

Apply formatting to placeholders:

```html
<!-- Zero-padded page numbers -->
<page property="page" data-format="Page {0:D3}" />
<!-- Output: Page 003 -->

<page property="page" data-format="{0:D3} of {1:D3}" />
<!-- Output: 003 of 010 -->
```

### 17. Invoice Table with Formatted Amounts

Format currency in table:

```html
<table style="width: 100%;">
    <thead>
        <tr>
            <th>Description</th>
            <th>Qty</th>
            <th>Price</th>
            <th>Total</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.items}}">
            <tr>
                <td>{{.description}}</td>
                <td>{{.quantity}}</td>
                <td><num value="{{.unitPrice}}" data-format="C2" /></td>
                <td><num value="{{.lineTotal}}" data-format="C2" /></td>
            </tr>
        </template>
    </tbody>
</table>
```

### 18. Financial Report with Percentages

Show financial metrics:

```html
<div style="padding: 10pt; border: 1pt solid #ccc;">
    <div>Revenue: <num value="{{model.revenue}}" data-format="C0" /></div>
    <div>Growth: <num value="{{model.growthRate}}" data-format="P1" /></div>
    <div>Margin: <num value="{{model.margin}}" data-format="P2" /></div>
</div>

<!-- Output:
     Revenue: $1,234,567
     Growth: 15.3%
     Margin: 23.45%
-->
```

### 19. Measurement Display

Show measurements with units:

```html
<div>
    <div>Length: <num value="{{.length}}" data-format="#0.00'm'" /></div>
    <div>Weight: <num value="{{.weight}}" data-format="#0.0'kg'" /></div>
    <div>Temp: <num value="{{.temperature}}" data-format="#0.0'°C'" /></div>
</div>

<!-- Output:
     Length: 12.50m
     Weight: 45.3kg
     Temp: 23.5°C
-->
```

### 20. Date Header with Custom Format

Create formatted headers:

```html
<header>
    <div style="text-align: right; font-size: 9pt; color: #666;">
        Report Date: <time value="{{model.reportDate}}" data-format="MMMM d, yyyy" />
    </div>
</header>

<!-- Output: Report Date: October 13, 2024 -->
```

### 21. Dynamic Format Selection

Choose format based on data:

```html
<!-- Model: { value: 1234.56, formatType: "currency" } -->
<num value="{{model.value}}"
     data-format="{{model.formatType === 'currency' ? 'C2' : model.formatType === 'percentage' ? 'P1' : 'N2'}}" />
```

### 22. Conditional Negative Formatting

Format negative values in red:

```html
<span style="color: {{model.profit < 0 ? '#dc3545' : '#28a745'}};">
    <num value="{{model.profit}}" data-format="C2" />
</span>
```

### 23. Multi-Currency Display

Show multiple currencies:

```html
<div>
    <div>USD: <num value="{{.amountUSD}}" data-format="$#,##0.00" /></div>
    <div>EUR: <num value="{{.amountEUR}}" data-format="€#,##0.00" /></div>
    <div>GBP: <num value="{{.amountGBP}}" data-format="£#,##0.00" /></div>
    <div>JPY: <num value="{{.amountJPY}}" data-format="¥#,##0" /></div>
</div>
```

### 24. Score Card with Conditional Formatting

Display scores with context:

```html
<template data-bind="{{model.students}}">
    <div style="padding: 10pt; border-bottom: 1pt solid #ddd;">
        <div style="display: inline-block; width: 50%;">{{.name}}</div>
        <div style="display: inline-block; width: 25%;">
            <num value="{{.score}}" data-format="'Score: '#0" />
        </div>
        <div style="display: inline-block; width: 25%;">
            <num value="{{.score / .maxScore}}" data-format="P0" />
        </div>
    </div>
</template>
```

### 25. Time Duration Formatting

Show elapsed time:

```html
<!-- For TimeSpan or duration values -->
<time value="{{model.duration}}" data-format="hh\:mm\:ss" />
<!-- Output: 02:15:30 -->

<time value="{{model.meetingTime}}" data-format="h\:mm tt" />
<!-- Output: 2:15 PM -->
```

---

## See Also

- [num element](/reference/htmltags/num.html) - Number display element
- [time element](/reference/htmltags/time.html) - Date/time display element
- [page element](/reference/htmltags/page.html) - Page number element
- [.NET Standard Format Strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings) - Microsoft documentation
- [.NET Custom Format Strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings) - Microsoft documentation
- [.NET Date/Time Format Strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings) - Microsoft documentation
- [Data Binding](/reference/binding/) - Data binding guide
- [Expressions](/reference/expressions/) - Expression syntax

---
