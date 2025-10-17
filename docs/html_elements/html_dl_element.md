---
layout: default
title: dl
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;dl&gt; : The Definition List Element

The `<dl>` element represents a definition list (also known as a description list) containing term-description pairs. It consists of `<dt>` (definition term) elements and `<dd>` (definition description) elements. Definition lists are ideal for glossaries, metadata, key-value pairs, FAQs, and any content where terms need to be associated with their descriptions or definitions.

## Usage

The `<dl>` element creates a definition list that:
- Contains pairs of terms (`<dt>`) and descriptions (`<dd>`)
- Groups related term-description content semantically
- Supports multiple descriptions for a single term
- Supports multiple terms for a single description
- Can contain complex nested content in descriptions
- Flows naturally across pages and columns
- Supports full CSS styling for layout control
- Can be data-bound for dynamic content generation

```html
<dl>
    <dt>Term</dt>
    <dd>Description of the term</dd>

    <dt>Another Term</dt>
    <dd>Description of another term</dd>
</dl>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the definition list in the PDF structure. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### CSS Style Support

The `<dl>` element supports extensive CSS styling through the `style` attribute or CSS classes:

**Box Model**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`

**Layout & Positioning**:
- `display`: `block`, `none`
- `page-break-before`, `page-break-after`, `page-break-inside`
- `break-before`, `break-after`, `break-inside`
- `column-count`, `column-gap` (for multi-column layouts)
- `width`, `min-width`, `max-width`

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `color` (inherited by terms and descriptions)
- `opacity`

**Typography** (inherited by child elements):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `line-height`, `letter-spacing`

### Child Elements

The `<dl>` element must contain the following child elements:

| Element | Description |
|---------|-------------|
| `<dt>` | Definition term - the term being defined. Displayed in **bold** by default. |
| `<dd>` | Definition description - the description/definition of the term. Indented by default (40pt left margin). |

---

## Notes

### Default Behavior

The `<dl>` element has the following default behavior:

1. **Block Display**: Displays as a block-level element
2. **No Special Styling**: The `<dl>` container itself has minimal default styling
3. **Child Styling**:
   - `<dt>` elements are **bold** by default
   - `<dd>` elements have a 40pt left margin for indentation
4. **Natural Flow**: Content flows across pages and columns when space is limited

### Class Hierarchy

In the Scryber codebase:
- `HTMLListDefinition` extends `ListDefinition` extends `Panel` extends `VisualComponent`
- `HTMLListDefinitionTerm` (`<dt>`) extends `ListDefinitionTerm` - bold by default
- `HTMLListDefinitionItem` (`<dd>`) extends `ListDefinitionItem` - 40pt left margin by default
- Implements standard HTML definition list behavior with PDF rendering

### Structure Patterns

Definition lists support several structural patterns:

**One Term, One Description** (most common):
```html
<dl>
    <dt>Term</dt>
    <dd>Description</dd>
</dl>
```

**One Term, Multiple Descriptions**:
```html
<dl>
    <dt>Term</dt>
    <dd>First description</dd>
    <dd>Second description</dd>
</dl>
```

**Multiple Terms, One Description**:
```html
<dl>
    <dt>First Term</dt>
    <dt>Second Term</dt>
    <dd>Shared description for both terms</dd>
</dl>
```

**Mixed Pattern**:
```html
<dl>
    <dt>Term 1</dt>
    <dt>Term 2</dt>
    <dd>Description for both terms 1 and 2</dd>

    <dt>Term 3</dt>
    <dd>Description for term 3</dd>
    <dd>Additional description for term 3</dd>
</dl>
```

### Content Types

Definition lists are ideal for:
- **Glossaries**: Technical terms and their definitions
- **Metadata**: Property names and values
- **FAQs**: Questions (terms) and answers (descriptions)
- **Specifications**: Feature names and details
- **Contact Information**: Labels and values
- **Configuration Settings**: Setting names and explanations

### Breaking Behavior

Definition lists handle page breaks naturally:
- Terms and descriptions can break independently by default
- Use `break-inside: avoid` on `<dt>` or `<dd>` to keep them together
- Use `break-inside: avoid` on the term-description pair wrapper to keep pairs together

---

## Examples

### Basic Definition List

```html
<dl>
    <dt>HTML</dt>
    <dd>HyperText Markup Language - the standard markup language for web pages.</dd>

    <dt>CSS</dt>
    <dd>Cascading Style Sheets - used for describing the presentation of a document.</dd>

    <dt>PDF</dt>
    <dd>Portable Document Format - a file format for presenting documents independently of software and hardware.</dd>
</dl>
```

### Glossary

```html
<style>
    .glossary {
        border: 1pt solid #ddd;
        padding: 15pt;
        background-color: #f9f9f9;
    }
    .glossary dt {
        color: #2c3e50;
        font-size: 13pt;
        margin-top: 10pt;
    }
    .glossary dt:first-child {
        margin-top: 0;
    }
    .glossary dd {
        margin-left: 20pt;
        color: #555;
    }
</style>

<dl class="glossary">
    <dt>API</dt>
    <dd>Application Programming Interface - a set of protocols and tools for building software applications.</dd>

    <dt>REST</dt>
    <dd>Representational State Transfer - an architectural style for designing networked applications.</dd>

    <dt>JSON</dt>
    <dd>JavaScript Object Notation - a lightweight data interchange format that is easy for humans to read and write.</dd>
</dl>
```

### Metadata Display

```html
<style>
    .metadata dt {
        font-weight: bold;
        color: #666;
        width: 150pt;
        float: left;
        clear: left;
    }
    .metadata dd {
        margin-left: 160pt;
        margin-bottom: 8pt;
    }
</style>

<dl class="metadata">
    <dt>Author:</dt>
    <dd>Jane Smith</dd>

    <dt>Date:</dt>
    <dd>January 15, 2024</dd>

    <dt>Version:</dt>
    <dd>2.1.0</dd>

    <dt>Status:</dt>
    <dd>Published</dd>

    <dt>Category:</dt>
    <dd>Technical Documentation</dd>
</dl>
```

### FAQ Style

```html
<style>
    .faq {
        border-left: 3pt solid #3498db;
        padding-left: 15pt;
    }
    .faq dt {
        font-size: 14pt;
        color: #2980b9;
        margin-top: 15pt;
        margin-bottom: 5pt;
    }
    .faq dt:first-child {
        margin-top: 0;
    }
    .faq dt:before {
        content: "Q: ";
        font-weight: bold;
    }
    .faq dd {
        margin-left: 20pt;
        color: #555;
        line-height: 1.6;
    }
    .faq dd:before {
        content: "A: ";
        font-weight: bold;
        color: #27ae60;
    }
</style>

<dl class="faq">
    <dt>How do I install the software?</dt>
    <dd>Download the installer from our website and run it. Follow the on-screen instructions to complete the installation.</dd>

    <dt>What are the system requirements?</dt>
    <dd>You need Windows 10 or later, 4GB RAM minimum, and 500MB of free disk space.</dd>

    <dt>Is there a free trial available?</dt>
    <dd>Yes, we offer a 30-day free trial with full access to all features.</dd>
</dl>
```

### Product Specifications

```html
<style>
    .specs {
        background-color: #fff;
        border: 1pt solid #e0e0e0;
        padding: 20pt;
    }
    .specs dt {
        background-color: #f5f5f5;
        padding: 8pt;
        margin-top: 5pt;
        border-left: 4pt solid #4caf50;
        font-weight: bold;
    }
    .specs dd {
        padding: 8pt;
        margin-left: 0;
        margin-bottom: 5pt;
        border-bottom: 1pt solid #eee;
    }
</style>

<dl class="specs">
    <dt>Processor</dt>
    <dd>Intel Core i7-12700K, 3.6 GHz Base, 5.0 GHz Turbo</dd>

    <dt>Memory</dt>
    <dd>32GB DDR5 4800MHz (2x16GB)</dd>

    <dt>Storage</dt>
    <dd>1TB NVMe SSD + 2TB HDD</dd>

    <dt>Graphics</dt>
    <dd>NVIDIA GeForce RTX 4070 Ti, 12GB GDDR6X</dd>

    <dt>Operating System</dt>
    <dd>Windows 11 Pro 64-bit</dd>
</dl>
```

### Multiple Descriptions per Term

```html
<dl>
    <dt>Scryber</dt>
    <dd>A .NET library for generating PDF documents from HTML templates.</dd>
    <dd>Supports CSS styling, data binding, and complex layouts.</dd>
    <dd>Can be used in ASP.NET applications, console apps, and web services.</dd>

    <dt>PDF</dt>
    <dd>Portable Document Format</dd>
    <dd>A file format developed by Adobe for presenting documents consistently across platforms.</dd>
</dl>
```

### Multiple Terms per Description

```html
<dl>
    <dt>HTML</dt>
    <dt>HyperText Markup Language</dt>
    <dd>The standard markup language for creating web pages and web applications.</dd>

    <dt>CSS</dt>
    <dt>Cascading Style Sheets</dt>
    <dt>Style Sheets</dt>
    <dd>A style sheet language used for describing the presentation of a document written in HTML.</dd>
</dl>
```

### Nested Content in Descriptions

```html
<dl>
    <dt>Features</dt>
    <dd>
        <p>Our software includes the following capabilities:</p>
        <ul>
            <li>Real-time data processing</li>
            <li>Advanced analytics dashboard</li>
            <li>Automated reporting</li>
            <li>Cloud synchronization</li>
        </ul>
    </dd>

    <dt>System Requirements</dt>
    <dd>
        <table style="border-collapse: collapse; width: 100%;">
            <tr>
                <td style="border: 1pt solid #ddd; padding: 5pt; font-weight: bold;">OS</td>
                <td style="border: 1pt solid #ddd; padding: 5pt;">Windows 10+, macOS 11+, Linux</td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 5pt; font-weight: bold;">RAM</td>
                <td style="border: 1pt solid #ddd; padding: 5pt;">8GB minimum, 16GB recommended</td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 5pt; font-weight: bold;">Disk Space</td>
                <td style="border: 1pt solid #ddd; padding: 5pt;">2GB available space</td>
            </tr>
        </table>
    </dd>
</dl>
```

### Two-Column Layout

```html
<style>
    .two-column {
        column-count: 2;
        column-gap: 30pt;
        column-rule: 1pt solid #ddd;
    }
    .two-column dt {
        font-weight: bold;
        color: #2c3e50;
        margin-top: 8pt;
    }
    .two-column dt:first-child {
        margin-top: 0;
    }
    .two-column dd {
        margin-left: 0;
        margin-bottom: 12pt;
        padding-left: 10pt;
        border-left: 2pt solid #e8e8e8;
    }
</style>

<dl class="two-column">
    <dt>Alpha</dt>
    <dd>First letter of the Greek alphabet</dd>

    <dt>Beta</dt>
    <dd>Second letter of the Greek alphabet</dd>

    <dt>Gamma</dt>
    <dd>Third letter of the Greek alphabet</dd>

    <dt>Delta</dt>
    <dd>Fourth letter of the Greek alphabet</dd>

    <dt>Epsilon</dt>
    <dd>Fifth letter of the Greek alphabet</dd>

    <dt>Zeta</dt>
    <dd>Sixth letter of the Greek alphabet</dd>
</dl>
```

### Contact Information Style

```html
<style>
    .contact-info {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 20pt;
        border-radius: 5pt;
    }
    .contact-info dt {
        font-weight: normal;
        font-size: 10pt;
        text-transform: uppercase;
        letter-spacing: 1pt;
        color: rgba(255,255,255,0.7);
        margin-bottom: 5pt;
    }
    .contact-info dd {
        font-size: 14pt;
        font-weight: bold;
        margin-left: 0;
        margin-bottom: 15pt;
    }
</style>

<dl class="contact-info">
    <dt>Name</dt>
    <dd>John Doe</dd>

    <dt>Email</dt>
    <dd>john.doe@example.com</dd>

    <dt>Phone</dt>
    <dd>+1 (555) 123-4567</dd>

    <dt>Address</dt>
    <dd>123 Main Street, Suite 100<br />San Francisco, CA 94102</dd>
</dl>
```

### Data Binding with Definition Lists

```html
<!-- Template with model.terms = [{term: "API", def: "Application Programming Interface"}, ...] -->
<dl>
    <template data-bind="{{model.terms}}">
        <dt>{{.term}}</dt>
        <dd>{{.definition}}</dd>
    </template>
</dl>

<!-- Output renders all term-definition pairs from the data model -->
```

### Complex Data Binding Example

```html
<!-- Template with model.settings = [{name: "Timeout", value: "30s", desc: "Connection timeout"}, ...] -->
<style>
    .settings dt {
        background-color: #ecf0f1;
        padding: 8pt;
        font-weight: bold;
        color: #2c3e50;
    }
    .settings dd {
        margin-left: 0;
        padding: 10pt;
        border-bottom: 1pt solid #bdc3c7;
    }
    .settings .value {
        font-family: monospace;
        background-color: #34495e;
        color: #ecf0f1;
        padding: 2pt 6pt;
        border-radius: 3pt;
    }
</style>

<dl class="settings">
    <template data-bind="{{model.settings}}">
        <dt>{{.name}}</dt>
        <dd>
            <span class="value">{{.value}}</span>
            <p style="margin: 5pt 0 0 0; color: #7f8c8d;">{{.description}}</p>
        </dd>
    </template>
</dl>
```

### Keeping Terms and Descriptions Together

```html
<style>
    .keep-together dt,
    .keep-together dd {
        break-inside: avoid;
    }
</style>

<dl class="keep-together">
    <dt>First Term</dt>
    <dd>This description will stay with its term and won't split across pages.</dd>

    <dt>Second Term</dt>
    <dd>Another description that won't break away from its term.</dd>
</dl>
```

### Bordered Term-Description Pairs

```html
<style>
    .bordered-pairs dt {
        background-color: #3498db;
        color: white;
        padding: 10pt;
        margin-top: 10pt;
        border-radius: 3pt 3pt 0 0;
    }
    .bordered-pairs dt:first-child {
        margin-top: 0;
    }
    .bordered-pairs dd {
        background-color: #ecf0f1;
        margin-left: 0;
        padding: 10pt;
        border: 1pt solid #bdc3c7;
        border-top: none;
        border-radius: 0 0 3pt 3pt;
        margin-bottom: 10pt;
    }
</style>

<dl class="bordered-pairs">
    <dt>Question 1</dt>
    <dd>Answer to the first question with detailed explanation.</dd>

    <dt>Question 2</dt>
    <dd>Answer to the second question with more details.</dd>

    <dt>Question 3</dt>
    <dd>Answer to the third question.</dd>
</dl>
```

### Inline Style Layout

```html
<style>
    .inline-layout dt {
        float: left;
        clear: left;
        width: 120pt;
        font-weight: bold;
        color: #2c3e50;
    }
    .inline-layout dd {
        margin-left: 130pt;
        padding-bottom: 8pt;
    }
</style>

<dl class="inline-layout">
    <dt>Name:</dt>
    <dd>Widget Pro 3000</dd>

    <dt>SKU:</dt>
    <dd>WP3000-BLK-001</dd>

    <dt>Price:</dt>
    <dd>$299.99</dd>

    <dt>Availability:</dt>
    <dd>In Stock</dd>

    <dt>Shipping:</dt>
    <dd>Free on orders over $50</dd>
</dl>
```

### Timeline Style

```html
<style>
    .timeline {
        border-left: 3pt solid #3498db;
        padding-left: 20pt;
        margin-left: 10pt;
    }
    .timeline dt {
        font-size: 12pt;
        font-weight: bold;
        color: #3498db;
        margin-top: 20pt;
        position: relative;
    }
    .timeline dt:first-child {
        margin-top: 0;
    }
    .timeline dt:before {
        content: '';
        width: 15pt;
        height: 15pt;
        background-color: #3498db;
        border: 3pt solid white;
        border-radius: 50%;
        position: absolute;
        left: -31pt;
    }
    .timeline dd {
        margin-left: 0;
        margin-top: 5pt;
        margin-bottom: 15pt;
        color: #555;
    }
</style>

<dl class="timeline">
    <dt>2020 - Foundation</dt>
    <dd>Company established with initial funding of $1M. Opened first office in San Francisco.</dd>

    <dt>2021 - Growth</dt>
    <dd>Expanded to 50 employees. Launched flagship product. Revenue reached $5M.</dd>

    <dt>2022 - Expansion</dt>
    <dd>Opened offices in New York and London. Grew to 150 employees. Revenue doubled to $10M.</dd>

    <dt>2023 - Innovation</dt>
    <dd>Released next-generation platform. Acquired competitor. Revenue reached $25M.</dd>
</dl>
```

### Code Documentation Style

```html
<style>
    .code-docs dt {
        font-family: monospace;
        background-color: #2d2d2d;
        color: #f8f8f2;
        padding: 8pt;
        font-size: 11pt;
        margin-top: 15pt;
        border-radius: 3pt;
    }
    .code-docs dt:first-child {
        margin-top: 0;
    }
    .code-docs dd {
        margin-left: 0;
        padding: 10pt;
        background-color: #f5f5f5;
        border: 1pt solid #e0e0e0;
        border-top: none;
        margin-bottom: 10pt;
    }
</style>

<dl class="code-docs">
    <dt>getUserById(id: string): User</dt>
    <dd>
        Retrieves a user by their unique identifier.
        <br /><strong>Parameters:</strong> id - The unique user identifier
        <br /><strong>Returns:</strong> User object containing user details
    </dd>

    <dt>createUser(data: UserData): User</dt>
    <dd>
        Creates a new user in the system.
        <br /><strong>Parameters:</strong> data - Object containing user information
        <br /><strong>Returns:</strong> Newly created User object
    </dd>

    <dt>deleteUser(id: string): boolean</dt>
    <dd>
        Deletes a user from the system.
        <br /><strong>Parameters:</strong> id - The unique user identifier
        <br /><strong>Returns:</strong> true if successful, false otherwise
    </dd>
</dl>
```

### Card-Style Layout

```html
<style>
    .card-layout dt {
        background-color: #34495e;
        color: white;
        padding: 12pt;
        font-size: 14pt;
        margin-top: 15pt;
        border-radius: 5pt 5pt 0 0;
    }
    .card-layout dt:first-child {
        margin-top: 0;
    }
    .card-layout dd {
        margin-left: 0;
        padding: 15pt;
        background-color: white;
        border: 1pt solid #ddd;
        border-top: none;
        border-radius: 0 0 5pt 5pt;
        box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);
        margin-bottom: 15pt;
    }
</style>

<dl class="card-layout">
    <dt>Basic Plan</dt>
    <dd>
        <p><strong>$9.99/month</strong></p>
        <ul style="margin: 0; padding-left: 20pt;">
            <li>Up to 10 users</li>
            <li>5GB storage</li>
            <li>Email support</li>
        </ul>
    </dd>

    <dt>Pro Plan</dt>
    <dd>
        <p><strong>$29.99/month</strong></p>
        <ul style="margin: 0; padding-left: 20pt;">
            <li>Up to 50 users</li>
            <li>50GB storage</li>
            <li>Priority support</li>
            <li>Advanced analytics</li>
        </ul>
    </dd>

    <dt>Enterprise Plan</dt>
    <dd>
        <p><strong>Contact for pricing</strong></p>
        <ul style="margin: 0; padding-left: 20pt;">
            <li>Unlimited users</li>
            <li>Unlimited storage</li>
            <li>24/7 phone support</li>
            <li>Custom integrations</li>
            <li>Dedicated account manager</li>
        </ul>
    </dd>
</dl>
```

### Striped Rows Style

```html
<style>
    .striped dt {
        background-color: #f8f9fa;
        padding: 8pt;
        font-weight: bold;
        margin: 0;
    }
    .striped dd {
        background-color: #ffffff;
        padding: 8pt;
        margin: 0 0 1pt 0;
    }
    .striped dt:nth-of-type(even),
    .striped dd:nth-of-type(even) {
        background-color: #e9ecef;
    }
</style>

<dl class="striped">
    <dt>Monday</dt>
    <dd>Team meeting at 9:00 AM, Project review at 2:00 PM</dd>

    <dt>Tuesday</dt>
    <dd>Client presentation at 10:00 AM, Development sprint planning</dd>

    <dt>Wednesday</dt>
    <dd>Code review session, Documentation update</dd>

    <dt>Thursday</dt>
    <dd>Testing phase begins, QA coordination meeting</dd>

    <dt>Friday</dt>
    <dd>Weekly retrospective, Deploy to staging environment</dd>
</dl>
```

### Dictionary/Encyclopedia Style

```html
<style>
    .dictionary {
        font-family: Georgia, serif;
    }
    .dictionary dt {
        font-size: 16pt;
        font-weight: bold;
        color: #1a1a1a;
        margin-top: 20pt;
        padding-bottom: 5pt;
        border-bottom: 2pt solid #333;
    }
    .dictionary dt:first-child {
        margin-top: 0;
    }
    .dictionary dd {
        margin-left: 20pt;
        margin-top: 8pt;
        line-height: 1.6;
        color: #333;
    }
    .dictionary .pronunciation {
        font-style: italic;
        color: #666;
        font-size: 11pt;
    }
    .dictionary .part-of-speech {
        font-style: italic;
        color: #888;
        font-size: 10pt;
    }
</style>

<dl class="dictionary">
    <dt>adapt <span class="pronunciation">/əˈdæpt/</span></dt>
    <dd>
        <span class="part-of-speech">verb</span>
        <p style="margin: 5pt 0;">
            1. To make suitable for a new use or purpose; modify.
            <br /><em>Example: "They adapted the building to serve as a museum."</em>
        </p>
        <p style="margin: 5pt 0;">
            2. To adjust or modify to suit different conditions.
            <br /><em>Example: "Organisms must adapt to survive in changing environments."</em>
        </p>
    </dd>

    <dt>innovation <span class="pronunciation">/ˌɪnəˈveɪʃən/</span></dt>
    <dd>
        <span class="part-of-speech">noun</span>
        <p style="margin: 5pt 0;">
            1. The introduction of something new; a new idea, method, or device.
            <br /><em>Example: "The company is known for its technological innovations."</em>
        </p>
        <p style="margin: 5pt 0;">
            2. The action or process of innovating.
            <br /><em>Example: "Innovation is essential for business growth."</em>
        </p>
    </dd>
</dl>
```

---

## See Also

- [dt](/reference/htmltags/dt.html) - Definition term element
- [dd](/reference/htmltags/dd.html) - Definition description element
- [ul](/reference/htmltags/ul.html) - Unordered list element
- [ol](/reference/htmltags/ol.html) - Ordered list element
- [li](/reference/htmltags/li.html) - List item element
- [div](/reference/htmltags/div.html) - Generic block container
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions

---
