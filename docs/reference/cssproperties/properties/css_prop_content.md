---
layout: default
title: content
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# content : Generated Content Property

The `content` property is used with the `::before` and `::after` pseudo-elements to insert generated content into a document. This property enables automatic numbering, decorative elements, quotes, and dynamic content insertion without modifying the HTML markup.

## Usage

```css
selector::before {
    content: value;
}

selector::after {
    content: value;
}
```

The content property accepts various value types including strings, attribute values, counters, and special keywords.

---

## Supported Values

### String Values
- `content: "text"` - Insert literal text
- `content: "●"` - Insert special characters or symbols
- `content: ""` - Empty content (still creates the pseudo-element)

### Attribute Values
- `content: attr(attribute-name)` - Insert the value of an HTML attribute
- `content: attr(href)` - Insert link URL
- `content: attr(data-label)` - Insert custom data attribute

### Counter Values
- `content: counter(name)` - Insert counter value
- `content: counter(name, style)` - Insert counter with specific style (decimal, lower-alpha, upper-roman, etc.)
- `content: counters(name, separator)` - Insert nested counter with separator
- `content: counters(name, ".", decimal)` - Nested counter with style

### Combined Values
- `content: "Chapter " counter(chapter)` - Combine text and counter
- `content: attr(title) " - " counter(section)` - Combine multiple values
- `content: "(" attr(data-id) ")"` - Text with attribute

### Special Keywords
- `none` - No content (default)
- `normal` - Equivalent to `none` for `::before` and `::after`

---

## Supported Elements

The `content` property applies to:
- `::before` pseudo-element (inserts content before element's content)
- `::after` pseudo-element (inserts content after element's content)

Note: The `content` property only works with pseudo-elements, not regular elements.

---

## Notes

- Content is inserted inline with the element's content
- Generated content is not part of the DOM and cannot be selected by users
- Use empty string `""` to create decorative pseudo-elements with borders/backgrounds
- Counters must be initialized with `counter-reset` before use
- Counters are incremented with `counter-increment`
- The `attr()` function retrieves attribute values from the selected element
- Multiple space-separated values are concatenated
- Generated content inherits styles from the parent element
- Essential for creating automatic numbering systems and decorative elements
- Particularly useful for table of contents, figure numbering, and footnotes
- Can be combined with positioning for advanced layouts

---

## Data Binding

The `content` property can be dynamically populated through data binding with counters and attribute values, enabling automatic numbering, generated labels, and data-driven content insertion. This is essential for creating dynamic lists, automatic figure numbering, and responsive content generation.

### Example 1: Dynamic list with data-bound counter labels

```html
<style>
    body { counter-reset: item; }
    .item-list li {
        counter-increment: item;
        list-style: none;
        padding: 10px;
        border-bottom: 1px solid #e5e7eb;
    }
    .item-list li::before {
        content: "Item " counter(item) ": ";
        font-weight: bold;
        color: #3b82f6;
        margin-right: 8px;
    }
</style>
<body>
    <ul class="item-list">
        <template data-bind="{{#each items}}">
            <li>{{description}}</li>
        </template>
    </ul>
</body>
```

### Example 2: Generated badges with data-driven counters

```html
<style>
    .notification-area { counter-reset: notification; }
    .notification-item {
        counter-increment: notification;
        position: relative;
        padding: 15px;
        margin: 10px 0;
        background: #f3f4f6;
        border-radius: 8px;
    }
    .notification-item::before {
        content: counter(notification);
        position: absolute;
        right: 15px;
        top: 15px;
        background: #ef4444;
        color: white;
        width: 24px;
        height: 24px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 12px;
        font-weight: bold;
    }
</style>
<body>
    <div class="notification-area">
        <template data-bind="{{#each notifications}}">
            <div class="notification-item">{{message}}</div>
        </template>
    </div>
</body>
```

### Example 3: Data-driven figure numbering

```html
<style>
    article { counter-reset: figure; }
    .figure-container {
        counter-increment: figure;
        margin: 20px 0;
        padding: 15px;
        border: 1px solid #d1d5db;
        border-radius: 8px;
    }
    .figure-caption::before {
        content: "Figure " counter(figure) ": ";
        font-weight: bold;
        color: #1f2937;
    }
    .figure-caption {
        margin-top: 10px;
        font-size: 14px;
        color: #6b7280;
    }
</style>
<body>
    <article>
        <template data-bind="{{#each figures}}">
            <div class="figure-container">
                <img src="{{imageUrl}}" alt="{{altText}}" style="width: 100%; max-width: 400px;"/>
                <div class="figure-caption">{{caption}}</div>
            </div>
        </template>
    </article>
</body>
```

---

## Examples

### Example 1: Simple text insertion with ::before

```html
<style>
    .note::before {
        content: "Note: ";
        font-weight: bold;
        color: #3b82f6;
    }
</style>
<body>
    <p class="note">This is an important message.</p>
</body>
```

### Example 2: Adding bullets with ::before

```html
<style>
    .custom-list li::before {
        content: "▸ ";
        color: #10b981;
        font-weight: bold;
        margin-right: 8px;
    }
    .custom-list li {
        list-style: none;
    }
</style>
<body>
    <ul class="custom-list">
        <li>First item</li>
        <li>Second item</li>
        <li>Third item</li>
    </ul>
</body>
```

### Example 3: Displaying link URLs with attr()

```html
<style>
    @media print {
        a[href]::after {
            content: " (" attr(href) ")";
            color: #6b7280;
            font-size: 0.9em;
        }
    }
</style>
<body>
    <p>Visit our <a href="https://example.com">website</a> for more information.</p>
</body>
```

### Example 4: Chapter numbering with counters

```html
<style>
    body {
        counter-reset: chapter;
    }
    h1::before {
        counter-increment: chapter;
        content: "Chapter " counter(chapter) ": ";
        color: #3b82f6;
    }
</style>
<body>
    <h1>Introduction</h1>
    <h1>Getting Started</h1>
    <h1>Advanced Topics</h1>
</body>
```

### Example 5: Section numbering with nested counters

```html
<style>
    body {
        counter-reset: section;
    }
    h2 {
        counter-reset: subsection;
    }
    h2::before {
        counter-increment: section;
        content: counter(section) ". ";
    }
    h3::before {
        counter-increment: subsection;
        content: counter(section) "." counter(subsection) " ";
    }
</style>
<body>
    <h2>First Section</h2>
    <h3>Subsection</h3>
    <h3>Subsection</h3>
    <h2>Second Section</h2>
    <h3>Subsection</h3>
</body>
```

### Example 6: Decorative quotes

```html
<style>
    blockquote::before {
        content: """;
        font-size: 3em;
        color: #3b82f6;
        line-height: 0;
        vertical-align: -0.4em;
    }
    blockquote::after {
        content: """;
        font-size: 3em;
        color: #3b82f6;
        line-height: 0;
        vertical-align: -0.4em;
    }
</style>
<body>
    <blockquote>This is a beautiful quotation with decorative marks.</blockquote>
</body>
```

### Example 7: Figure numbering

```html
<style>
    body {
        counter-reset: figure;
    }
    figure figcaption::before {
        counter-increment: figure;
        content: "Figure " counter(figure) ": ";
        font-weight: bold;
        color: #1f2937;
    }
</style>
<body>
    <figure>
        <img src="chart1.png" alt="Sales Chart"/>
        <figcaption>Quarterly sales data</figcaption>
    </figure>
    <figure>
        <img src="chart2.png" alt="Growth Chart"/>
        <figcaption>Year-over-year growth</figcaption>
    </figure>
</body>
```

### Example 8: Adding icons with ::before

```html
<style>
    .warning::before {
        content: "⚠ ";
        color: #f59e0b;
        font-size: 1.2em;
    }
    .success::before {
        content: "✓ ";
        color: #10b981;
        font-weight: bold;
    }
    .error::before {
        content: "✗ ";
        color: #ef4444;
        font-weight: bold;
    }
</style>
<body>
    <p class="warning">Warning message</p>
    <p class="success">Success message</p>
    <p class="error">Error message</p>
</body>
```

### Example 9: Table row numbering

```html
<style>
    table {
        counter-reset: row;
    }
    tbody tr::before {
        counter-increment: row;
        content: counter(row);
        display: table-cell;
        text-align: center;
        font-weight: bold;
        padding: 8px;
        background: #f3f4f6;
    }
</style>
<body>
    <table>
        <thead>
            <tr>
                <th>#</th>
                <th>Name</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Item A</td>
                <td>$100</td>
            </tr>
            <tr>
                <td>Item B</td>
                <td>$200</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 10: Status badges with custom data attributes

```html
<style>
    .status::before {
        content: attr(data-status);
        padding: 4px 12px;
        border-radius: 12px;
        font-size: 0.85em;
        font-weight: bold;
        margin-right: 8px;
        background: #e5e7eb;
        color: #1f2937;
    }
    .status[data-status="Active"]::before {
        background: #d1fae5;
        color: #065f46;
    }
    .status[data-status="Pending"]::before {
        background: #fef3c7;
        color: #92400e;
    }
</style>
<body>
    <p class="status" data-status="Active">User is logged in</p>
    <p class="status" data-status="Pending">Awaiting approval</p>
</body>
```

### Example 11: Required field indicator

```html
<style>
    .required::after {
        content: " *";
        color: #ef4444;
        font-weight: bold;
    }
</style>
<body>
    <form>
        <label class="required">Full Name</label>
        <input type="text" required/>

        <label class="required">Email Address</label>
        <input type="email" required/>

        <label>Phone Number</label>
        <input type="tel"/>
    </form>
</body>
```

### Example 12: Multi-level list numbering

```html
<style>
    .outline {
        counter-reset: level1;
        list-style: none;
    }
    .outline > li {
        counter-increment: level1;
        counter-reset: level2;
    }
    .outline > li::before {
        content: counter(level1) ". ";
        font-weight: bold;
    }
    .outline > li > ul {
        counter-reset: level2;
        list-style: none;
    }
    .outline > li > ul > li {
        counter-increment: level2;
    }
    .outline > li > ul > li::before {
        content: counter(level1) "." counter(level2) " ";
    }
</style>
<body>
    <ul class="outline">
        <li>Introduction
            <ul>
                <li>Overview</li>
                <li>Background</li>
            </ul>
        </li>
        <li>Main Content
            <ul>
                <li>Section One</li>
                <li>Section Two</li>
            </ul>
        </li>
    </ul>
</body>
```

### Example 13: Page numbers for print

```html
<style>
    @page {
        @bottom-right {
            content: "Page " counter(page) " of " counter(pages);
        }
    }
</style>
<body>
    <p>Document content...</p>
</body>
```

### Example 14: External link indicator

```html
<style>
    a[href^="http"]::after {
        content: " ↗";
        font-size: 0.8em;
        color: #3b82f6;
    }
    a[href^="http"][href*="example.com"]::after {
        content: "";
    }
</style>
<body>
    <p>Visit <a href="https://external-site.com">this external site</a> or
       stay on <a href="https://example.com/page">our site</a>.</p>
</body>
```

### Example 15: Footnote references

```html
<style>
    body {
        counter-reset: footnote;
    }
    .footnote {
        counter-increment: footnote;
    }
    .footnote::after {
        content: "[" counter(footnote) "]";
        vertical-align: super;
        font-size: 0.75em;
        color: #3b82f6;
    }
    .footnotes {
        counter-reset: footnote;
        border-top: 1px solid #d1d5db;
        margin-top: 2em;
        padding-top: 1em;
    }
    .footnotes li {
        counter-increment: footnote;
    }
    .footnotes li::before {
        content: "[" counter(footnote) "] ";
        font-weight: bold;
    }
</style>
<body>
    <p>This is a statement<span class="footnote"></span> that requires citation<span class="footnote"></span>.</p>

    <ol class="footnotes">
        <li>Source: Journal of Examples, 2024</li>
        <li>See also: Reference Manual, page 42</li>
    </ol>
</body>
```

---

## See Also

- [counter-reset](/reference/cssproperties/css_prop_counter-reset) - Initialize counters
- [counter-increment](/reference/cssproperties/css_prop_counter-increment) - Increment counters
- [display](/reference/cssproperties/css_prop_display) - Element display type
- [list-style-type](/reference/cssproperties/css_prop_list-style-type) - List marker style
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
