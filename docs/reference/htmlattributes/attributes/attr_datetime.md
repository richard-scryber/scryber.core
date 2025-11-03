---
layout: default
title: datetime
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @datetime : The Date and Time Attribute

The `datetime` attribute specifies a machine-readable date, time, or duration for `<time>`, `<ins>`, and `<del>` elements. It provides precise temporal information in ISO 8601 format, enabling proper date/time representation in PDF documents while allowing human-readable content to be displayed separately.

## Usage

The `datetime` attribute defines temporal values:
- Specifies exact dates and times in machine-readable format
- Used with `<time>` elements for semantic date/time markup
- Documents when content was inserted (`<ins>`) or deleted (`<del>`)
- Follows ISO 8601 date/time format standards
- Supports dates, times, datetimes, durations, and time zones
- Enables data binding for dynamic date/time values

```html
<!-- Date only -->
<time datetime="2025-01-15">January 15, 2025</time>

<!-- Date and time -->
<time datetime="2025-01-15T14:30:00">2:30 PM on January 15, 2025</time>

<!-- Date with timezone -->
<time datetime="2025-01-15T14:30:00-05:00">2:30 PM EST</time>

<!-- Insertion timestamp -->
<ins datetime="2025-01-15T10:00:00">New content added</ins>

<!-- Deletion timestamp -->
<del datetime="2025-01-15T10:00:00">Old content removed</del>

<!-- Dynamic datetime -->
<time datetime="{{model.publishDate}}">{{model.formattedDate}}</time>
```

---

## Supported Elements

The `datetime` attribute is used with:

### Time Element
- `<time>` - Semantic time/date markup (primary use)

### Change Tracking Elements
- `<ins>` - Inserted content with timestamp
- `<del>` - Deleted content with timestamp

---

## Binding Values

The `datetime` attribute supports data binding for dynamic date/time values:

```html
<!-- Dynamic datetime from model -->
<time datetime="{{model.eventDate}}">{{model.displayDate}}</time>

<!-- Formatted publication date -->
<time datetime="{{model.article.publishedAt}}">
    Published: {{model.article.formattedDate}}
</time>

<!-- Insertion with dynamic timestamp -->
<ins datetime="{{model.lastModified}}">
    {{model.newContent}}
</ins>

<!-- Deletion with timestamp -->
<del datetime="{{model.removalDate}}">
    {{model.oldContent}}
</del>

<!-- Repeating events with dates -->
<template data-bind="{{model.events}}">
    <div>
        <h3>{{.title}}</h3>
        <time datetime="{{.startDate}}">{{.displayDate}}</time>
    </div>
</template>

<!-- Change log with timestamps -->
<template data-bind="{{model.changes}}">
    <p>
        <del datetime="{{.timestamp}}">{{.before}}</del>
        <ins datetime="{{.timestamp}}">{{.after}}</ins>
    </p>
</template>
```

**Data Model Example:**
```json
{
  "eventDate": "2025-06-15T19:00:00-04:00",
  "displayDate": "June 15, 2025 at 7:00 PM EDT",
  "article": {
    "publishedAt": "2025-01-15T09:00:00Z",
    "formattedDate": "January 15, 2025"
  },
  "lastModified": "2025-01-15T14:30:00Z",
  "newContent": "Updated information",
  "removalDate": "2025-01-15T14:30:00Z",
  "oldContent": "Outdated information",
  "events": [
    {
      "title": "Annual Conference",
      "startDate": "2025-06-15",
      "displayDate": "June 15, 2025"
    }
  ],
  "changes": [
    {
      "timestamp": "2025-01-15T10:00:00Z",
      "before": "Original text",
      "after": "Updated text"
    }
  ]
}
```

---

## Notes

### ISO 8601 Format

The `datetime` attribute requires **ISO 8601** format:

```html
<!-- Date formats -->
<time datetime="2025">Year only</time>
<time datetime="2025-01">Year and month</time>
<time datetime="2025-01-15">Full date (YYYY-MM-DD)</time>

<!-- Time formats -->
<time datetime="14:30">Time only (24-hour)</time>
<time datetime="14:30:45">Time with seconds</time>
<time datetime="14:30:45.123">Time with milliseconds</time>

<!-- Date and time combined -->
<time datetime="2025-01-15T14:30:00">Date and time</time>

<!-- With timezone offset -->
<time datetime="2025-01-15T14:30:00-05:00">EST (UTC-5)</time>
<time datetime="2025-01-15T14:30:00+00:00">UTC</time>
<time datetime="2025-01-15T14:30:00Z">UTC (Z notation)</time>

<!-- Duration (prefixed with P) -->
<time datetime="PT2H30M">2 hours 30 minutes</time>
<time datetime="P3DT4H">3 days 4 hours</time>
```

### Date-Only Format

For dates without time:

```html
<!-- Year -->
<time datetime="2025">The year 2025</time>

<!-- Year and month -->
<time datetime="2025-06">June 2025</time>

<!-- Full date -->
<time datetime="2025-01-15">January 15, 2025</time>

<!-- Week notation -->
<time datetime="2025-W03">Week 3 of 2025</time>
```

### Time-Only Format

For times without dates:

```html
<!-- Hour and minute -->
<time datetime="14:30">2:30 PM</time>

<!-- Hour, minute, and second -->
<time datetime="09:15:30">9:15:30 AM</time>

<!-- With fractional seconds -->
<time datetime="14:30:45.500">2:30:45.5 PM</time>
```

### DateTime with Timezone

Include timezone information for precise timestamps:

```html
<!-- UTC timezone (Z notation) -->
<time datetime="2025-01-15T14:30:00Z">
    January 15, 2025 at 2:30 PM UTC
</time>

<!-- Positive offset (ahead of UTC) -->
<time datetime="2025-01-15T14:30:00+09:00">
    2:30 PM JST (Japan Standard Time)
</time>

<!-- Negative offset (behind UTC) -->
<time datetime="2025-01-15T14:30:00-05:00">
    2:30 PM EST (Eastern Standard Time)
</time>

<!-- UTC+0 -->
<time datetime="2025-01-15T14:30:00+00:00">
    2:30 PM GMT
</time>
```

### Duration Format

Durations use the period prefix (`P`):

```html
<!-- Hours and minutes -->
<time datetime="PT2H30M">2 hours 30 minutes</time>

<!-- Days -->
<time datetime="P5D">5 days</time>

<!-- Weeks -->
<time datetime="P2W">2 weeks</time>

<!-- Combined -->
<time datetime="P1DT12H">1 day and 12 hours</time>

<!-- Years, months, days -->
<time datetime="P1Y2M15D">1 year, 2 months, 15 days</time>

<!-- Complex duration -->
<time datetime="P1Y2M15DT6H30M">
    1 year, 2 months, 15 days, 6 hours, 30 minutes
</time>
```

Duration format breakdown:
- `P` = Period (required prefix)
- `Y` = Years
- `M` = Months (before T) or Minutes (after T)
- `W` = Weeks
- `D` = Days
- `T` = Time separator
- `H` = Hours
- `M` = Minutes (after T)
- `S` = Seconds

### Machine-Readable vs Display

The `datetime` attribute is machine-readable; element content is for display:

```html
<!-- Machine format in datetime, human format in content -->
<time datetime="2025-01-15">January 15, 2025</time>
<time datetime="2025-01-15T14:30:00">2:30 PM on January 15</time>
<time datetime="PT2H30M">two and a half hours</time>

<!-- Different languages/formats for display -->
<time datetime="2025-01-15">15 janvier 2025</time>  <!-- French -->
<time datetime="2025-01-15">15. Januar 2025</time>  <!-- German -->
<time datetime="2025-01-15">2025年1月15日</time>    <!-- Japanese -->
```

### Using with Insertions and Deletions

Track when content changes occurred:

```html
<!-- Document when content was added -->
<p>
    <ins datetime="2025-01-15T10:30:00Z">
        This paragraph was added on January 15, 2025.
    </ins>
</p>

<!-- Document when content was removed -->
<p>
    Price: <del datetime="2025-01-10T14:00:00Z">$99.99</del>
    <ins datetime="2025-01-15T14:00:00Z">$89.99</ins>
</p>

<!-- Multiple revisions -->
<p>
    The event is on
    <del datetime="2025-01-05">January 20</del>
    <del datetime="2025-01-10">January 25</del>
    <ins datetime="2025-01-15">February 1</ins>
</p>
```

### Combining with cite Attribute

Use both `datetime` and `cite` for complete change tracking:

```html
<p>
    Product specifications have been updated:
    <del datetime="2025-01-15T10:00:00Z"
         cite="https://example.com/changelog">
        8GB RAM
    </del>
    <ins datetime="2025-01-15T10:00:00Z"
         cite="https://example.com/changelog">
        16GB RAM
    </ins>
</p>
```

### Pubdate (Obsolete)

In HTML5, the `pubdate` attribute is obsolete. Use `<time>` with appropriate semantics:

```html
<!-- Obsolete (don't use) -->
<time datetime="2025-01-15" pubdate>Published: January 15, 2025</time>

<!-- Current best practice -->
<article>
    <header>
        <h1>Article Title</h1>
        <p>Published: <time datetime="2025-01-15">January 15, 2025</time></p>
    </header>
    <p>Article content...</p>
</article>
```

### Validation

Valid datetime values must follow ISO 8601:

```html
<!-- VALID -->
<time datetime="2025-01-15">Valid</time>
<time datetime="2025-01-15T14:30:00">Valid</time>
<time datetime="2025-01-15T14:30:00Z">Valid</time>
<time datetime="2025-01-15T14:30:00-05:00">Valid</time>

<!-- INVALID -->
<time datetime="01/15/2025">Invalid (US format)</time>
<time datetime="15-01-2025">Invalid (DD-MM-YYYY)</time>
<time datetime="January 15, 2025">Invalid (text)</time>
<time datetime="2:30 PM">Invalid (12-hour format)</time>
```

### Missing datetime Attribute

The `datetime` attribute is optional for `<time>` elements:

```html
<!-- Without datetime - content must be valid datetime -->
<time>2025-01-15</time>

<!-- With datetime - content can be any format -->
<time datetime="2025-01-15">January 15, 2025</time>
<time datetime="2025-01-15">The 15th of January</time>

<!-- For ins/del, datetime is optional but recommended -->
<ins>Added content</ins>
<ins datetime="2025-01-15">Added content with timestamp</ins>
```

---

## Examples

### Basic Date Markup

```html
<article>
    <h1>Event Announcement</h1>

    <p>
        Join us for our annual conference on
        <time datetime="2025-06-15">June 15, 2025</time>.
    </p>

    <p>
        Registration opens on
        <time datetime="2025-03-01">March 1st</time>
        and closes on
        <time datetime="2025-06-01">June 1st</time>.
    </p>
</article>
```

### Date and Time Markup

```html
<article>
    <h1>Webinar Schedule</h1>

    <div>
        <h2>Introduction to PDF Generation</h2>
        <p>
            Date: <time datetime="2025-02-15T14:00:00-05:00">
            February 15, 2025 at 2:00 PM EST
            </time>
        </p>
        <p>Duration: <time datetime="PT1H30M">1 hour 30 minutes</time></p>
    </div>

    <div>
        <h2>Advanced Techniques</h2>
        <p>
            Date: <time datetime="2025-02-22T14:00:00-05:00">
            February 22, 2025 at 2:00 PM EST
            </time>
        </p>
        <p>Duration: <time datetime="PT2H">2 hours</time></p>
    </div>
</article>
```

### Publication Dates

```html
<article>
    <header>
        <h1>Understanding Modern Web Development</h1>
        <p>
            <strong>Author:</strong> Jane Smith<br/>
            <strong>Published:</strong>
            <time datetime="2025-01-15T09:00:00Z">January 15, 2025</time><br/>
            <strong>Last Updated:</strong>
            <time datetime="2025-01-20T14:30:00Z">January 20, 2025</time>
        </p>
    </header>

    <p>Article content goes here...</p>

    <footer>
        <p>
            <small>
                Article published <time datetime="2025-01-15">January 15, 2025</time>.
                Last modified <time datetime="2025-01-20">January 20, 2025</time>.
            </small>
        </p>
    </footer>
</article>
```

### Event Listing

```html
<article>
    <h1>Upcoming Events</h1>

    <section>
        <h2>Company Annual Meeting</h2>
        <p>
            <strong>Date:</strong>
            <time datetime="2025-03-15">March 15, 2025</time><br/>
            <strong>Time:</strong>
            <time datetime="09:00">9:00 AM</time> -
            <time datetime="17:00">5:00 PM</time><br/>
            <strong>Duration:</strong>
            <time datetime="PT8H">8 hours</time>
        </p>
    </section>

    <section>
        <h2>Product Launch</h2>
        <p>
            <strong>Date:</strong>
            <time datetime="2025-04-01T10:00:00-04:00">
            April 1, 2025 at 10:00 AM EDT
            </time><br/>
            <strong>Duration:</strong>
            <time datetime="PT45M">45 minutes</time>
        </p>
    </section>

    <section>
        <h2>Summer Conference</h2>
        <p>
            <strong>Dates:</strong>
            <time datetime="2025-07-20">July 20</time> through
            <time datetime="2025-07-23">July 23, 2025</time><br/>
            <strong>Duration:</strong>
            <time datetime="P3D">3 days</time>
        </p>
    </section>
</article>
```

### Document Revision History

```html
<article>
    <h1>Product Specifications</h1>

    <section>
        <h2>Revision History</h2>

        <p>
            <strong>Version 1.0</strong>
            <time datetime="2024-12-01">(December 1, 2024)</time><br/>
            Initial release
        </p>

        <p>
            <strong>Version 1.1</strong>
            <time datetime="2025-01-10">(January 10, 2025)</time><br/>
            <ins datetime="2025-01-10T14:00:00Z">
                Added support for new file formats
            </ins>
        </p>

        <p>
            <strong>Version 1.2</strong>
            <time datetime="2025-01-15">(January 15, 2025)</time><br/>
            <ins datetime="2025-01-15T10:30:00Z">
                Enhanced performance and bug fixes
            </ins>
        </p>
    </section>
</article>
```

### Content Changes with Timestamps

```html
<article>
    <h1>Policy Update</h1>

    <h2>Work From Home Policy</h2>

    <p>
        Employees may work remotely up to
        <del datetime="2025-01-10T09:00:00Z">two</del>
        <ins datetime="2025-01-15T09:00:00Z">three</ins>
        days per week.
    </p>

    <h2>Vacation Days</h2>

    <p>
        Full-time employees receive
        <del datetime="2025-01-10T09:00:00Z">15</del>
        <ins datetime="2025-01-15T09:00:00Z">18</ins>
        vacation days per year.
    </p>

    <p style="margin-top: 20pt; font-size: 9pt; color: #666;">
        <em>
            Policy updated on
            <time datetime="2025-01-15">January 15, 2025</time>.
            Previous version dated
            <time datetime="2025-01-10">January 10, 2025</time>.
        </em>
    </p>
</article>
```

### International Timezones

```html
<article>
    <h1>Global Conference Call Schedule</h1>

    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #f2f2f2;">
                <th style="border: 1pt solid #ddd; padding: 8pt;">Region</th>
                <th style="border: 1pt solid #ddd; padding: 8pt;">Local Time</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">New York (EST)</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <time datetime="2025-02-15T14:00:00-05:00">2:00 PM</time>
                </td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">London (GMT)</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <time datetime="2025-02-15T19:00:00+00:00">7:00 PM</time>
                </td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">Tokyo (JST)</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <time datetime="2025-02-16T04:00:00+09:00">4:00 AM (next day)</time>
                </td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">Sydney (AEDT)</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <time datetime="2025-02-16T06:00:00+11:00">6:00 AM (next day)</time>
                </td>
            </tr>
        </tbody>
    </table>

    <p style="margin-top: 15pt;">
        <strong>Note:</strong> All times for
        <time datetime="2025-02-15">February 15, 2025</time>.
    </p>
</article>
```

### Blog Post with Multiple Dates

```html
<article>
    <header>
        <h1>The Future of PDF Generation</h1>

        <div style="color: #666; font-size: 10pt; margin: 10pt 0;">
            <p>
                <strong>Originally Published:</strong>
                <time datetime="2024-12-15T08:00:00Z">December 15, 2024</time>
            </p>
            <p>
                <strong>Last Updated:</strong>
                <time datetime="2025-01-15T14:30:00Z">January 15, 2025 at 2:30 PM UTC</time>
            </p>
            <p>
                <strong>Reading Time:</strong>
                <time datetime="PT12M">12 minutes</time>
            </p>
        </div>
    </header>

    <section>
        <h2>Introduction</h2>
        <p>Article content...</p>

        <p>
            <ins datetime="2025-01-15T14:30:00Z">
                <strong>Update:</strong> New information added on January 15, 2025
                regarding recent developments in the field.
            </ins>
        </p>
    </section>

    <footer style="margin-top: 30pt; padding: 15pt; background-color: #f8f9fa;">
        <p>
            <small>
                © 2024-<time datetime="2025">2025</time> Company Name.
                Published <time datetime="2024-12-15">December 15, 2024</time>.
            </small>
        </p>
    </footer>
</article>
```

### Data-Bound Event Schedule

```html
<!-- Model: {
    events: [
        {
            title: "Keynote Speech",
            datetime: "2025-06-15T09:00:00-04:00",
            displayTime: "9:00 AM EDT",
            duration: "PT1H",
            displayDuration: "1 hour"
        },
        {
            title: "Technical Workshop",
            datetime: "2025-06-15T14:00:00-04:00",
            displayTime: "2:00 PM EDT",
            duration: "PT2H30M",
            displayDuration: "2.5 hours"
        }
    ]
} -->

<article>
    <h1>Conference Schedule</h1>

    <template data-bind="{{model.events}}">
        <div style="margin: 15pt 0; padding: 10pt; border: 1pt solid #ccc;">
            <h2>{{.title}}</h2>
            <p>
                <strong>Time:</strong>
                <time datetime="{{.datetime}}">{{.displayTime}}</time>
            </p>
            <p>
                <strong>Duration:</strong>
                <time datetime="{{.duration}}">{{.displayDuration}}</time>
            </p>
        </div>
    </template>
</article>
```

### Historical Timeline

```html
<article>
    <h1>Company History Timeline</h1>

    <div style="border-left: 2pt solid #336699; padding-left: 15pt; margin-left: 10pt;">
        <div style="margin-bottom: 20pt;">
            <h3><time datetime="2010-01">January 2010</time></h3>
            <p>Company founded</p>
        </div>

        <div style="margin-bottom: 20pt;">
            <h3><time datetime="2012-06">June 2012</time></h3>
            <p>First product launched</p>
        </div>

        <div style="margin-bottom: 20pt;">
            <h3><time datetime="2015-03">March 2015</time></h3>
            <p>Reached 1 million customers</p>
        </div>

        <div style="margin-bottom: 20pt;">
            <h3><time datetime="2018-09">September 2018</time></h3>
            <p>Expanded to international markets</p>
        </div>

        <div style="margin-bottom: 20pt;">
            <h3><time datetime="2020-05">May 2020</time></h3>
            <p>Launched cloud platform</p>
        </div>

        <div>
            <h3><time datetime="2025-01">January 2025</time></h3>
            <p>Celebrating 15 years of innovation</p>
        </div>
    </div>
</article>
```

### Course Schedule

```html
<article>
    <h1>Training Course Schedule</h1>

    <section>
        <h2>Week 1: Introduction</h2>
        <p>
            <strong>Dates:</strong>
            <time datetime="2025-03-03">March 3</time> -
            <time datetime="2025-03-07">March 7, 2025</time><br/>
            <strong>Duration:</strong> <time datetime="P5D">5 days</time>
        </p>
        <ul>
            <li>
                Day 1: <time datetime="2025-03-03">Monday</time> -
                Orientation (Duration: <time datetime="PT4H">4 hours</time>)
            </li>
            <li>
                Day 2: <time datetime="2025-03-04">Tuesday</time> -
                Fundamentals (Duration: <time datetime="PT6H">6 hours</time>)
            </li>
        </ul>
    </section>

    <section>
        <h2>Week 2: Advanced Topics</h2>
        <p>
            <strong>Dates:</strong>
            <time datetime="2025-03-10">March 10</time> -
            <time datetime="2025-03-14">March 14, 2025</time><br/>
            <strong>Duration:</strong> <time datetime="P5D">5 days</time>
        </p>
    </section>
</article>
```

### Product Pricing Changes

```html
<article>
    <h1>Pricing History</h1>

    <h2>Premium Plan</h2>

    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #f2f2f2;">
                <th style="border: 1pt solid #ddd; padding: 8pt;">Effective Date</th>
                <th style="border: 1pt solid #ddd; padding: 8pt;">Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <time datetime="2024-01-01">January 1, 2024</time>
                </td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <del datetime="2024-12-01">$79.99/month</del>
                </td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <time datetime="2024-12-01">December 1, 2024</time>
                </td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <del datetime="2025-01-15">$89.99/month</del>
                </td>
            </tr>
            <tr style="background-color: #e8f4ea;">
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <time datetime="2025-01-15">January 15, 2025</time>
                </td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">
                    <ins datetime="2025-01-15"><strong>$99.99/month</strong></ins>
                </td>
            </tr>
        </tbody>
    </table>
</article>
```

### Meeting Minutes with Timestamps

```html
<article>
    <h1>Board Meeting Minutes</h1>
    <p>
        <strong>Date:</strong>
        <time datetime="2025-01-15T14:00:00-05:00">January 15, 2025 at 2:00 PM EST</time>
    </p>
    <p><strong>Duration:</strong> <time datetime="PT2H15M">2 hours 15 minutes</time></p>

    <section>
        <h2>Agenda Items</h2>

        <h3>1. Financial Report (<time datetime="14:00">2:00 PM</time>)</h3>
        <p>Q4 financial results presented. Revenue exceeded projections by 12%.</p>

        <h3>2. Strategic Planning (<time datetime="14:30">2:30 PM</time>)</h3>
        <p>Discussed expansion plans for 2025.</p>
        <p>
            <ins datetime="2025-01-15T14:45:00-05:00">
                Motion passed to approve $5M budget for new initiatives.
            </ins>
        </p>

        <h3>3. Policy Updates (<time datetime="15:45">3:45 PM</time>)</h3>
        <p>
            Updated remote work policy:
            <del datetime="2025-01-15T15:50:00-05:00">2 days per week</del>
            <ins datetime="2025-01-15T15:50:00-05:00">3 days per week</ins>
        </p>
    </section>

    <p style="margin-top: 20pt;">
        <strong>Meeting Adjourned:</strong>
        <time datetime="2025-01-15T16:15:00-05:00">4:15 PM</time>
    </p>
</article>
```

### Copyright and Dates

```html
<footer style="margin-top: 40pt; padding: 20pt; background-color: #f8f9fa; text-align: center;">
    <p>
        © <time datetime="2020">2020</time>-<time datetime="2025">2025</time> Company Name.
        All rights reserved.
    </p>
    <p>
        <small>
            Last updated: <time datetime="2025-01-15T10:30:00Z">January 15, 2025</time>
        </small>
    </p>
</footer>
```

---

## See Also

- [time](/reference/htmltags/time.html) - Time element for semantic date/time markup
- [ins](/reference/htmltags/ins.html) - Inserted content element
- [del](/reference/htmltags/del.html) - Deleted content element
- [cite](/reference/htmlattributes/cite.html) - Citation source attribute
- [data-format](/reference/htmlattributes/data-format.html) - Date formatting in data binding
- [ISO 8601](/reference/standards/iso8601.html) - Date/time format standard

---
