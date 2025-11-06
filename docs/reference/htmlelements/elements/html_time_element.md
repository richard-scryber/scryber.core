---
layout: default
title: time
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;time&gt; : The Date/Time Element
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

The `<time>` element represents a specific date, time, or datetime value. In PDF output, it renders formatted date/time text according to the specified format string. It's ideal for displaying dates, timestamps, deadlines, and temporal information with consistent formatting throughout your document.

## Usage

The `<time>` element creates formatted date/time text that:
- Displays dates and times in customizable formats
- Uses the `datetime` attribute for the actual date/time value
- Supports the `data-format` attribute for custom formatting patterns
- Falls back to inner text content if parsing fails
- Can display text content without formatting when no format is specified
- Inherits text styling from parent elements
- Works seamlessly with data binding for dynamic dates
- Uses inline display by default

```html
<!-- Basic date display -->
<time datetime="2024-03-15">March 15, 2024</time>

<!-- Formatted date -->
<time datetime="2024-03-15" data-format="MMMM dd, yyyy">March 15, 2024</time>

<!-- Date and time -->
<time datetime="2024-03-15T14:30:00" data-format="yyyy-MM-dd HH:mm">2024-03-15 14:30</time>

<!-- Relative time -->
<time datetime="2024-03-15">2 days ago</time>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Tooltip text or outline title. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Time-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `datetime` | DateTime | **Required**. The machine-readable date/time value in ISO 8601 format. |
| `data-format` | string | .NET datetime format string. Determines how the date/time is displayed. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |

### CSS Style Support

The `<time>` element supports text styling:

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color`
- `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`, `word-spacing`

**Spacing**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`

**Positioning**:
- `display`: `inline` (default), `block`, `inline-block`, `none`
- `position`: `static`, `relative`, `absolute`
- `vertical-align`: `top`, `middle`, `bottom`, `baseline`

**Visual Effects**:
- `background-color`
- `border`, `border-width`, `border-color`, `border-style`
- `opacity`

---

## Notes

### DateTime Attribute Format

The `datetime` attribute accepts ISO 8601 format strings:

**Date only:**
- `2024-03-15` (March 15, 2024)
- `2024-03` (March 2024)
- `2024` (Year 2024)

**Date and time:**
- `2024-03-15T14:30:00` (2:30 PM on March 15, 2024)
- `2024-03-15T14:30:00-05:00` (with timezone offset)
- `2024-03-15T14:30:00Z` (UTC time)

**Time only:**
- `14:30:00` (2:30 PM)
- `14:30` (2:30 PM, seconds optional)

### Format Strings

Use .NET date/time format strings in the `data-format` attribute:

**Standard Format Strings:**
- `d` - Short date (3/15/2024)
- `D` - Long date (Friday, March 15, 2024)
- `t` - Short time (2:30 PM)
- `T` - Long time (2:30:00 PM)
- `f` - Full date/time, short time (Friday, March 15, 2024 2:30 PM)
- `F` - Full date/time (Friday, March 15, 2024 2:30:00 PM)
- `g` - General date/time, short time (3/15/2024 2:30 PM)
- `G` - General date/time (3/15/2024 2:30:00 PM)

**Custom Format Strings:**
- `yyyy` - Four-digit year (2024)
- `yy` - Two-digit year (24)
- `MMMM` - Full month name (March)
- `MMM` - Abbreviated month (Mar)
- `MM` - Two-digit month (03)
- `M` - Month without leading zero (3)
- `dd` - Two-digit day (15)
- `d` - Day without leading zero (15)
- `dddd` - Full day name (Friday)
- `ddd` - Abbreviated day (Fri)
- `HH` - 24-hour format, two digits (14)
- `hh` - 12-hour format, two digits (02)
- `h` - 12-hour format, no leading zero (2)
- `mm` - Minutes, two digits (30)
- `ss` - Seconds, two digits (00)
- `tt` - AM/PM designator (PM)

**Common Patterns:**
```html
<!-- March 15, 2024 -->
<time datetime="2024-03-15" data-format="MMMM dd, yyyy"></time>

<!-- 03/15/2024 -->
<time datetime="2024-03-15" data-format="MM/dd/yyyy"></time>

<!-- Friday, March 15, 2024 -->
<time datetime="2024-03-15" data-format="dddd, MMMM dd, yyyy"></time>

<!-- Mar 15, 2024 at 2:30 PM -->
<time datetime="2024-03-15T14:30:00" data-format="MMM dd, yyyy 'at' h:mm tt"></time>

<!-- 2024-03-15 14:30:00 -->
<time datetime="2024-03-15T14:30:00" data-format="yyyy-MM-dd HH:mm:ss"></time>
```

### Text Content Behavior

The `<time>` element has special text handling:

1. **With format specified**: Parses inner text as date, formats using `data-format`
   ```html
   <time data-format="MMMM dd, yyyy">2024-03-15</time>
   <!-- Displays: March 15, 2024 -->
   ```

2. **Without format**: Displays inner text as-is
   ```html
   <time datetime="2024-03-15">Two weeks ago</time>
   <!-- Displays: Two weeks ago -->
   ```

3. **Parse failure**: Falls back to displaying the original text
   ```html
   <time data-format="yyyy-MM-dd">Invalid date</time>
   <!-- Displays: Invalid date -->
   ```

### Class Hierarchy

In the Scryber codebase:
- `HTMLTime` extends `Date` extends `TextBase` extends `VisualComponent`
- Inherits date formatting capabilities from the base `Date` component
- Supports both `datetime` attribute and inner text content

### Use Cases in PDF

The `<time>` element is perfect for:
1. **Document Metadata**: Display creation dates, modification dates, version dates
2. **Reports**: Show report generation timestamps, data cutoff dates
3. **Invoices**: Display invoice dates, due dates, payment dates
4. **Certificates**: Show issue dates, expiration dates
5. **Event Information**: Display event dates, deadlines, schedules
6. **Historical Data**: Show data points with timestamps
7. **Audit Trails**: Display action timestamps in logs

---

## Examples

### Basic Date Display

```html
<!-- Simple date -->
<time datetime="2024-03-15">March 15, 2024</time>

<!-- Formatted date -->
<time datetime="2024-03-15" data-format="MMMM dd, yyyy">March 15, 2024</time>

<!-- Short date format -->
<time datetime="2024-03-15" data-format="MM/dd/yyyy">03/15/2024</time>

<!-- Long date format -->
<time datetime="2024-03-15" data-format="dddd, MMMM dd, yyyy">Friday, March 15, 2024</time>
```

### Date and Time Display

```html
<!-- Date with time -->
<time datetime="2024-03-15T14:30:00" data-format="MMMM dd, yyyy 'at' h:mm tt">
    March 15, 2024 at 2:30 PM
</time>

<!-- ISO format -->
<time datetime="2024-03-15T14:30:00" data-format="yyyy-MM-dd'T'HH:mm:ss">
    2024-03-15T14:30:00
</time>

<!-- Custom format -->
<time datetime="2024-03-15T14:30:00" data-format="MMM dd, yyyy - hh:mm tt">
    Mar 15, 2024 - 02:30 PM
</time>
```

### Document Header with Dates

```html
<div style="border-bottom: 2pt solid #336699; padding: 15pt; margin-bottom: 20pt;">
    <h1 style="margin: 0 0 10pt 0; color: #336699;">Monthly Report</h1>

    <div style="font-size: 10pt; color: #666;">
        <div style="margin-bottom: 4pt;">
            <strong>Report Period:</strong>
            <time datetime="2024-03-01" data-format="MMMM yyyy">March 2024</time>
        </div>
        <div style="margin-bottom: 4pt;">
            <strong>Generated:</strong>
            <time datetime="2024-03-15T14:30:00" data-format="MMMM dd, yyyy 'at' h:mm tt">
                March 15, 2024 at 2:30 PM
            </time>
        </div>
        <div>
            <strong>Report ID:</strong> RPT-2024-03-001
        </div>
    </div>
</div>
```

### Invoice Date Section

```html
<div style="border: 1pt solid #ddd; border-radius: 6pt; padding: 20pt; margin: 15pt 0;">
    <h2 style="margin: 0 0 15pt 0; color: #333;">Invoice #INV-2024-0315</h2>

    <table style="width: 100%; font-size: 10pt;">
        <tr>
            <td style="padding: 8pt 0; width: 140pt; font-weight: 600;">Invoice Date:</td>
            <td style="padding: 8pt 0;">
                <time datetime="2024-03-15" data-format="MMMM dd, yyyy"
                      style="color: #333;">March 15, 2024</time>
            </td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="padding: 8pt 0; font-weight: 600;">Due Date:</td>
            <td style="padding: 8pt 0;">
                <time datetime="2024-04-14" data-format="MMMM dd, yyyy"
                      style="color: #FF5722; font-weight: bold;">April 14, 2024</time>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt 0; font-weight: 600;">Payment Terms:</td>
            <td style="padding: 8pt 0;">Net 30 Days</td>
        </tr>
    </table>
</div>
```

### Event Schedule

```html
<div style="border: 2pt solid #4CAF50; border-radius: 8pt; padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0; color: #4CAF50;">Conference Schedule</h3>

    <div style="border-left: 4pt solid #4CAF50; padding-left: 15pt; margin-bottom: 15pt;">
        <h4 style="margin: 0 0 5pt 0; color: #333;">Registration</h4>
        <time datetime="2024-06-10T08:00:00" data-format="dddd, MMMM dd 'at' h:mm tt"
              style="color: #666; font-weight: 600;">
            Monday, June 10 at 8:00 AM
        </time>
    </div>

    <div style="border-left: 4pt solid #4CAF50; padding-left: 15pt; margin-bottom: 15pt;">
        <h4 style="margin: 0 0 5pt 0; color: #333;">Keynote Address</h4>
        <time datetime="2024-06-10T09:30:00" data-format="dddd, MMMM dd 'at' h:mm tt"
              style="color: #666; font-weight: 600;">
            Monday, June 10 at 9:30 AM
        </time>
    </div>

    <div style="border-left: 4pt solid #4CAF50; padding-left: 15pt; margin-bottom: 15pt;">
        <h4 style="margin: 0 0 5pt 0; color: #333;">Workshop Sessions</h4>
        <time datetime="2024-06-10T11:00:00" data-format="h:mm tt"
              style="color: #666; font-weight: 600;">11:00 AM</time>
        <span style="color: #666;"> - </span>
        <time datetime="2024-06-10T17:00:00" data-format="h:mm tt"
              style="color: #666; font-weight: 600;">5:00 PM</time>
    </div>

    <div style="border-left: 4pt solid #4CAF50; padding-left: 15pt;">
        <h4 style="margin: 0 0 5pt 0; color: #333;">Closing Reception</h4>
        <time datetime="2024-06-10T18:00:00" data-format="dddd, MMMM dd 'at' h:mm tt"
              style="color: #666; font-weight: 600;">
            Monday, June 10 at 6:00 PM
        </time>
    </div>
</div>
```

### Certificate with Dates

```html
<div style="border: 4pt solid #336699; padding: 40pt; text-align: center;
            background: linear-gradient(to bottom, #fff, #f9f9f9);">
    <h1 style="margin: 0 0 20pt 0; color: #336699; font-size: 24pt;">
        Certificate of Completion
    </h1>

    <p style="font-size: 12pt; margin: 20pt 0;">
        This certifies that
    </p>

    <h2 style="margin: 10pt 0 20pt 0; color: #333; font-size: 18pt;">
        John Smith
    </h2>

    <p style="font-size: 11pt; margin: 20pt 40pt;">
        has successfully completed the course<br/>
        <strong style="font-size: 13pt; color: #336699;">
            Advanced PDF Generation with Scryber
        </strong>
    </p>

    <div style="margin: 30pt 0 20pt 0; padding-top: 20pt; border-top: 1pt solid #ddd;">
        <div style="display: inline-block; margin: 0 20pt;">
            <div style="font-size: 9pt; color: #666; margin-bottom: 4pt;">Issue Date</div>
            <time datetime="2024-03-15" data-format="MMMM dd, yyyy"
                  style="font-weight: bold; font-size: 11pt;">
                March 15, 2024
            </time>
        </div>

        <div style="display: inline-block; margin: 0 20pt;">
            <div style="font-size: 9pt; color: #666; margin-bottom: 4pt;">Valid Until</div>
            <time datetime="2026-03-15" data-format="MMMM dd, yyyy"
                  style="font-weight: bold; font-size: 11pt;">
                March 15, 2026
            </time>
        </div>
    </div>
</div>
```

### Timeline with Multiple Dates

```html
<div style="padding: 20pt;">
    <h3 style="margin: 0 0 25pt 0;">Project Timeline</h3>

    <div style="border-left: 3pt solid #336699; padding-left: 20pt; margin-left: 10pt;">
        <div style="margin-bottom: 25pt; position: relative;">
            <div style="position: absolute; left: -26pt; width: 16pt; height: 16pt;
                        background-color: #4CAF50; border: 3pt solid white; border-radius: 8pt;"></div>
            <h4 style="margin: 0 0 5pt 0; color: #4CAF50;">Project Initiated</h4>
            <time datetime="2024-01-15" data-format="MMMM dd, yyyy"
                  style="color: #666; font-size: 10pt;">January 15, 2024</time>
            <p style="margin: 8pt 0 0 0; color: #666; font-size: 10pt;">
                Project kickoff meeting and requirements gathering phase began.
            </p>
        </div>

        <div style="margin-bottom: 25pt; position: relative;">
            <div style="position: absolute; left: -26pt; width: 16pt; height: 16pt;
                        background-color: #4CAF50; border: 3pt solid white; border-radius: 8pt;"></div>
            <h4 style="margin: 0 0 5pt 0; color: #4CAF50;">Design Complete</h4>
            <time datetime="2024-02-28" data-format="MMMM dd, yyyy"
                  style="color: #666; font-size: 10pt;">February 28, 2024</time>
            <p style="margin: 8pt 0 0 0; color: #666; font-size: 10pt;">
                All design mockups approved and development ready to begin.
            </p>
        </div>

        <div style="margin-bottom: 25pt; position: relative;">
            <div style="position: absolute; left: -26pt; width: 16pt; height: 16pt;
                        background-color: #2196F3; border: 3pt solid white; border-radius: 8pt;"></div>
            <h4 style="margin: 0 0 5pt 0; color: #2196F3;">Development Phase</h4>
            <time datetime="2024-03-15" data-format="MMMM dd, yyyy"
                  style="color: #666; font-size: 10pt;">March 15, 2024</time>
            <span style="color: #2196F3; font-weight: bold; margin-left: 10pt;">← Current</span>
            <p style="margin: 8pt 0 0 0; color: #666; font-size: 10pt;">
                Active development in progress, 78% complete.
            </p>
        </div>

        <div style="margin-bottom: 0; position: relative;">
            <div style="position: absolute; left: -26pt; width: 16pt; height: 16pt;
                        background-color: #ccc; border: 3pt solid white; border-radius: 8pt;"></div>
            <h4 style="margin: 0 0 5pt 0; color: #999;">Launch Date</h4>
            <time datetime="2024-04-30" data-format="MMMM dd, yyyy"
                  style="color: #666; font-size: 10pt;">April 30, 2024</time>
            <p style="margin: 8pt 0 0 0; color: #666; font-size: 10pt;">
                Planned launch date for production release.
            </p>
        </div>
    </div>
</div>
```

### Data-Bound Time Elements

```html
<!-- With model = {
    createdDate: "2024-03-15T10:30:00",
    modifiedDate: "2024-03-20T14:45:00",
    expiryDate: "2024-12-31"
} -->
<div style="padding: 15pt; background-color: #f5f5f5; border-radius: 6pt;">
    <h4 style="margin: 0 0 12pt 0;">Document Information</h4>

    <table style="width: 100%; font-size: 10pt;">
        <tr>
            <td style="padding: 6pt 0; width: 120pt; font-weight: 600;">Created:</td>
            <td style="padding: 6pt 0;">
                <time datetime="{{model.createdDate}}" data-format="MMMM dd, yyyy 'at' h:mm tt">
                </time>
            </td>
        </tr>
        <tr>
            <td style="padding: 6pt 0; font-weight: 600;">Last Modified:</td>
            <td style="padding: 6pt 0;">
                <time datetime="{{model.modifiedDate}}" data-format="MMMM dd, yyyy 'at' h:mm tt">
                </time>
            </td>
        </tr>
        <tr>
            <td style="padding: 6pt 0; font-weight: 600;">Expires:</td>
            <td style="padding: 6pt 0;">
                <time datetime="{{model.expiryDate}}" data-format="MMMM dd, yyyy"
                      style="color: #FF5722; font-weight: bold;">
                </time>
            </td>
        </tr>
    </table>
</div>
```

### Meeting Minutes Header

```html
<div style="background-color: #263238; color: white; padding: 25pt; border-radius: 8pt 8pt 0 0;">
    <h2 style="margin: 0 0 15pt 0; font-size: 18pt;">Board Meeting Minutes</h2>

    <div style="display: inline-block; margin-right: 30pt;">
        <div style="font-size: 9pt; opacity: 0.8; margin-bottom: 4pt;">Meeting Date</div>
        <time datetime="2024-03-15" data-format="dddd, MMMM dd, yyyy"
              style="font-weight: 600; font-size: 12pt;">
            Friday, March 15, 2024
        </time>
    </div>

    <div style="display: inline-block; margin-right: 30pt;">
        <div style="font-size: 9pt; opacity: 0.8; margin-bottom: 4pt;">Start Time</div>
        <time datetime="2024-03-15T09:00:00" data-format="h:mm tt"
              style="font-weight: 600; font-size: 12pt;">
            9:00 AM
        </time>
    </div>

    <div style="display: inline-block;">
        <div style="font-size: 9pt; opacity: 0.8; margin-bottom: 4pt;">End Time</div>
        <time datetime="2024-03-15T11:30:00" data-format="h:mm tt"
              style="font-weight: 600; font-size: 12pt;">
            11:30 AM
        </time>
    </div>
</div>
```

### Blog Post Metadata

```html
<article style="border: 1pt solid #e0e0e0; border-radius: 6pt; padding: 20pt; margin: 15pt 0;">
    <h2 style="margin: 0 0 10pt 0; color: #333;">
        Understanding PDF Generation with Scryber
    </h2>

    <div style="font-size: 9pt; color: #666; margin-bottom: 15pt; padding-bottom: 15pt;
                border-bottom: 1pt solid #eee;">
        <span style="margin-right: 20pt;">
            <strong>By:</strong> Jane Developer
        </span>
        <span style="margin-right: 20pt;">
            <strong>Published:</strong>
            <time datetime="2024-03-15" data-format="MMMM dd, yyyy">March 15, 2024</time>
        </span>
        <span>
            <strong>Reading Time:</strong> 5 minutes
        </span>
    </div>

    <p style="color: #555; line-height: 1.6;">
        Learn how to generate professional PDF documents using Scryber's powerful
        HTML-to-PDF conversion capabilities...
    </p>
</article>
```

### Audit Log with Timestamps

```html
<div style="border: 1pt solid #ddd; border-radius: 6pt; padding: 15pt;">
    <h4 style="margin: 0 0 15pt 0; color: #333;">Activity Log</h4>

    <table style="width: 100%; font-size: 9pt; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #f5f5f5;">
                <th style="padding: 8pt; text-align: left; border-bottom: 2pt solid #ddd;">
                    Timestamp
                </th>
                <th style="padding: 8pt; text-align: left; border-bottom: 2pt solid #ddd;">
                    User
                </th>
                <th style="padding: 8pt; text-align: left; border-bottom: 2pt solid #ddd;">
                    Action
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                    <time datetime="2024-03-15T09:15:23" data-format="yyyy-MM-dd HH:mm:ss">
                        2024-03-15 09:15:23
                    </time>
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #eee;">admin</td>
                <td style="padding: 8pt; border-bottom: 1pt solid #eee;">Document created</td>
            </tr>
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                    <time datetime="2024-03-15T09:23:45" data-format="yyyy-MM-dd HH:mm:ss">
                        2024-03-15 09:23:45
                    </time>
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #eee;">jdoe</td>
                <td style="padding: 8pt; border-bottom: 1pt solid #eee;">Content updated</td>
            </tr>
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                    <time datetime="2024-03-15T10:05:12" data-format="yyyy-MM-dd HH:mm:ss">
                        2024-03-15 10:05:12
                    </time>
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #eee;">jdoe</td>
                <td style="padding: 8pt; border-bottom: 1pt solid #eee;">Approved for publishing</td>
            </tr>
            <tr>
                <td style="padding: 8pt;">
                    <time datetime="2024-03-15T14:30:00" data-format="yyyy-MM-dd HH:mm:ss">
                        2024-03-15 14:30:00
                    </time>
                </td>
                <td style="padding: 8pt;">system</td>
                <td style="padding: 8pt;">PDF generated</td>
            </tr>
        </tbody>
    </table>
</div>
```

### Deadline Warning Box

```html
<div style="border: 2pt solid #FF5722; border-radius: 6pt; padding: 15pt;
            background-color: #FFF3E0; margin: 15pt 0;">
    <div style="display: inline-block; vertical-align: middle; margin-right: 15pt;">
        <span style="font-size: 36pt; color: #FF5722;">⚠</span>
    </div>
    <div style="display: inline-block; vertical-align: middle;">
        <h4 style="margin: 0 0 5pt 0; color: #FF5722;">Urgent: Payment Due Soon</h4>
        <p style="margin: 0; color: #666;">
            Your payment is due on
            <time datetime="2024-03-25" data-format="MMMM dd, yyyy"
                  style="font-weight: bold; color: #FF5722;">March 25, 2024</time>.
            Please submit payment to avoid late fees.
        </p>
    </div>
</div>
```

### Different Date Format Examples

```html
<div style="padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0;">Date Format Examples</h3>

    <table style="width: 100%; border-collapse: collapse;">
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee; width: 200pt;">
                <strong>Long Date:</strong>
            </td>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <time datetime="2024-03-15" data-format="dddd, MMMM dd, yyyy">
                    Friday, March 15, 2024
                </time>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <strong>Short Date:</strong>
            </td>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <time datetime="2024-03-15" data-format="MM/dd/yyyy">
                    03/15/2024
                </time>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <strong>ISO Date:</strong>
            </td>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <time datetime="2024-03-15" data-format="yyyy-MM-dd">
                    2024-03-15
                </time>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <strong>Month Year:</strong>
            </td>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <time datetime="2024-03-15" data-format="MMMM yyyy">
                    March 2024
                </time>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <strong>Day Month:</strong>
            </td>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <time datetime="2024-03-15" data-format="dd MMMM">
                    15 March
                </time>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <strong>Time Only:</strong>
            </td>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <time datetime="2024-03-15T14:30:00" data-format="h:mm tt">
                    2:30 PM
                </time>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <strong>24-Hour Time:</strong>
            </td>
            <td style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <time datetime="2024-03-15T14:30:00" data-format="HH:mm:ss">
                    14:30:00
                </time>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt;">
                <strong>Custom Format:</strong>
            </td>
            <td style="padding: 8pt;">
                <time datetime="2024-03-15T14:30:00" data-format="'Week of' MMM dd, yyyy">
                    Week of Mar 15, 2024
                </time>
            </td>
        </tr>
    </table>
</div>
```

### Expiry Date Warning

```html
<style>
    .expiry-box {
        border: 1pt solid #ddd;
        border-radius: 6pt;
        padding: 15pt;
        margin: 10pt 0;
    }

    .expiry-date {
        font-weight: bold;
        font-size: 12pt;
    }

    .expiry-warning {
        background-color: #FFF3E0;
        border-left: 4pt solid #FF9800;
    }

    .expiry-critical {
        background-color: #FFEBEE;
        border-left: 4pt solid #F44336;
    }

    .expiry-valid {
        background-color: #E8F5E9;
        border-left: 4pt solid #4CAF50;
    }
</style>

<div class="expiry-box expiry-valid">
    <h4 style="margin: 0 0 8pt 0; color: #4CAF50;">✓ License Active</h4>
    <p style="margin: 0; color: #666;">
        Valid until
        <time datetime="2025-12-31" data-format="MMMM dd, yyyy" class="expiry-date"
              style="color: #4CAF50;">December 31, 2025</time>
    </p>
</div>

<div class="expiry-box expiry-warning">
    <h4 style="margin: 0 0 8pt 0; color: #FF9800;">⚠ Certificate Expiring Soon</h4>
    <p style="margin: 0; color: #666;">
        Expires on
        <time datetime="2024-04-15" data-format="MMMM dd, yyyy" class="expiry-date"
              style="color: #FF9800;">April 15, 2024</time>
        - Renewal required
    </p>
</div>

<div class="expiry-box expiry-critical">
    <h4 style="margin: 0 0 8pt 0; color: #F44336;">✕ Subscription Expired</h4>
    <p style="margin: 0; color: #666;">
        Expired on
        <time datetime="2024-02-28" data-format="MMMM dd, yyyy" class="expiry-date"
              style="color: #F44336;">February 28, 2024</time>
        - Immediate action required
    </p>
</div>
```

### Repeating Times from Collection

```html
<!-- With model.events = [
    {name: "Team Standup", date: "2024-03-15T09:00:00"},
    {name: "Client Meeting", date: "2024-03-15T14:00:00"},
    {name: "Code Review", date: "2024-03-15T16:00:00"}
] -->
<div style="border: 1pt solid #ddd; border-radius: 6pt; padding: 20pt;">
    <h3 style="margin: 0 0 15pt 0; color: #333;">Today's Schedule</h3>

    <template data-bind="{{model.events}}">
        <div style="padding: 12pt; margin-bottom: 10pt; background-color: #f9f9f9;
                    border-left: 3pt solid #2196F3; border-radius: 4pt;">
            <div style="font-weight: bold; font-size: 11pt; margin-bottom: 4pt;">
                {{.name}}
            </div>
            <time datetime="{{.date}}" data-format="h:mm tt"
                  style="color: #666; font-size: 10pt;">
            </time>
        </div>
    </template>
</div>
```

### Version History

```html
<div style="padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0;">Document Version History</h3>

    <table style="width: 100%; border-collapse: collapse; font-size: 10pt;">
        <thead>
            <tr style="background-color: #f5f5f5;">
                <th style="padding: 10pt; text-align: left; border-bottom: 2pt solid #ddd;">
                    Version
                </th>
                <th style="padding: 10pt; text-align: left; border-bottom: 2pt solid #ddd;">
                    Date
                </th>
                <th style="padding: 10pt; text-align: left; border-bottom: 2pt solid #ddd;">
                    Author
                </th>
                <th style="padding: 10pt; text-align: left; border-bottom: 2pt solid #ddd;">
                    Changes
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee; font-weight: bold;">
                    v1.0
                </td>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee;">
                    <time datetime="2024-01-15" data-format="MMM dd, yyyy">
                        Jan 15, 2024
                    </time>
                </td>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee;">J. Smith</td>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee;">Initial release</td>
            </tr>
            <tr>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee; font-weight: bold;">
                    v1.1
                </td>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee;">
                    <time datetime="2024-02-10" data-format="MMM dd, yyyy">
                        Feb 10, 2024
                    </time>
                </td>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee;">M. Johnson</td>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee;">Bug fixes and improvements</td>
            </tr>
            <tr>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee; font-weight: bold;">
                    v2.0
                </td>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee;">
                    <time datetime="2024-03-15" data-format="MMM dd, yyyy">
                        Mar 15, 2024
                    </time>
                </td>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee;">A. Williams</td>
                <td style="padding: 10pt; border-bottom: 1pt solid #eee;">Major feature update</td>
            </tr>
        </tbody>
    </table>
</div>
```

### Contract Dates Section

```html
<div style="border: 2pt solid #333; padding: 25pt; margin: 20pt 0;">
    <h2 style="margin: 0 0 20pt 0; text-align: center; color: #333;">
        Service Agreement
    </h2>

    <div style="background-color: #f9f9f9; padding: 20pt; border-radius: 6pt; margin-bottom: 20pt;">
        <h4 style="margin: 0 0 15pt 0; color: #555;">Agreement Terms</h4>

        <div style="margin-bottom: 15pt;">
            <div style="font-weight: 600; margin-bottom: 4pt;">Effective Date:</div>
            <time datetime="2024-01-01" data-format="dddd, MMMM dd, yyyy"
                  style="font-size: 11pt; color: #333;">
                Monday, January 01, 2024
            </time>
        </div>

        <div style="margin-bottom: 15pt;">
            <div style="font-weight: 600; margin-bottom: 4pt;">Termination Date:</div>
            <time datetime="2024-12-31" data-format="dddd, MMMM dd, yyyy"
                  style="font-size: 11pt; color: #333;">
                Tuesday, December 31, 2024
            </time>
        </div>

        <div style="margin-bottom: 0;">
            <div style="font-weight: 600; margin-bottom: 4pt;">Notice Period:</div>
            <span style="font-size: 11pt; color: #333;">30 days prior to termination date</span>
        </div>
    </div>

    <div style="font-size: 9pt; color: #666; text-align: center;">
        Agreement executed on
        <time datetime="2023-12-15" data-format="MMMM dd, yyyy">December 15, 2023</time>
    </div>
</div>
```

### Weather Report with Times

```html
<div style="background: linear-gradient(to bottom, #64B5F6, #E3F2FD);
            padding: 20pt; border-radius: 8pt; color: #1565C0;">
    <h3 style="margin: 0 0 15pt 0; color: #0D47A1;">Weather Forecast</h3>

    <div style="margin-bottom: 10pt; padding: 10pt; background-color: rgba(255,255,255,0.5);
                border-radius: 4pt;">
        <time datetime="2024-03-15" data-format="dddd, MMMM dd"
              style="font-weight: bold; font-size: 12pt; color: #0D47A1;">
            Friday, March 15
        </time>
    </div>

    <table style="width: 100%; margin-top: 15pt;">
        <tr>
            <td style="padding: 8pt; background-color: rgba(255,255,255,0.4); border-radius: 4pt 0 0 4pt;">
                <time datetime="2024-03-15T06:00:00" data-format="h tt"
                      style="font-weight: bold;">6 AM</time>
                <div style="font-size: 9pt; margin-top: 4pt;">54°F ⛅</div>
            </td>
            <td style="padding: 8pt; background-color: rgba(255,255,255,0.4);">
                <time datetime="2024-03-15T12:00:00" data-format="h tt"
                      style="font-weight: bold;">12 PM</time>
                <div style="font-size: 9pt; margin-top: 4pt;">68°F ☀</div>
            </td>
            <td style="padding: 8pt; background-color: rgba(255,255,255,0.4);">
                <time datetime="2024-03-15T18:00:00" data-format="h tt"
                      style="font-weight: bold;">6 PM</time>
                <div style="font-size: 9pt; margin-top: 4pt;">61°F 🌤</div>
            </td>
            <td style="padding: 8pt; background-color: rgba(255,255,255,0.4); border-radius: 0 4pt 4pt 0;">
                <time datetime="2024-03-16T00:00:00" data-format="h tt"
                      style="font-weight: bold;">12 AM</time>
                <div style="font-size: 9pt; margin-top: 4pt;">52°F 🌙</div>
            </td>
        </tr>
    </table>
</div>
```

---

## See Also

- [span](/reference/htmltags/span.html) - Inline container element (for wrapping time elements)
- [div](/reference/htmltags/div.html) - Block container element
- [Data Binding](/reference/binding/) - Data binding and expressions
- [Date Component](/reference/components/date.html) - Base date component in Scryber namespace
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Text Formatting](/reference/text/formatting.html) - Text formatting and display

---
