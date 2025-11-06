---
layout: default
title: data-li attributes
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-li-* : List Item Formatting Attributes

The `data-li-*` family of attributes controls the appearance and behavior of list item numbering in Scryber PDF documents. These attributes provide fine-grained control over list styling including numbering style, grouping, concatenation, prefixes, suffixes, alignment, and spacing.

---

## Summary

The `data-li-*` attributes extend standard HTML list formatting with PDF-specific capabilities. They allow you to:

- Customize list numbering appearance and format
- Group lists for continuous numbering across the document
- Concatenate parent and child list numbers
- Add custom prefixes and suffixes to list numbers
- Control number alignment and indentation
- Set custom labels for individual list items

These attributes work with both ordered (`<ol>`) and unordered (`<ul>`) lists, as well as individual list items (`<li>`). They provide capabilities beyond standard CSS list styling, enabling professional document formatting required for legal documents, technical manuals, and structured reports.

---

## Attributes Overview

| Attribute | Applies To | Description |
|-----------|------------|-------------|
| `data-li-style` | `<ol>`, `<ul>` | List numbering style (decimal, roman, alpha, etc.) |
| `data-li-group` | `<ol>`, `<ul>` | Group name for continuous numbering across lists |
| `data-li-concat` | `<ol>`, `<ul>` | Concatenate with parent list numbering |
| `data-li-prefix` | `<ol>`, `<ul>` | Text to prepend before list number |
| `data-li-postfix` | `<ol>`, `<ul>` | Text to append after list number |
| `data-li-inset` | `<ol>`, `<ul>`, `<li>` | Width of the number block (indentation control) |
| `data-li-align` | `<ol>`, `<ul>`, `<li>` | Alignment of number within the number block |
| `data-li-label` | `<li>` | Custom label text for an individual item |

---

## Usage

These attributes can be applied via HTML attributes or CSS custom properties:

### HTML Attribute Syntax

```html
<!-- On ordered list -->
<ol data-li-style="upper-roman" data-li-postfix="." data-li-group="main">
    <li>First item</li>
    <li>Second item</li>
</ol>

<!-- On list item -->
<li data-li-inset="50pt" data-li-align="left">Custom indented item</li>

<!-- On unordered list -->
<ul data-li-prefix="# " data-li-postfix=" -">
    <li>Bullet item</li>
</ul>
```

### CSS Custom Property Syntax

```html
<style>
    .legal-list {
        -pdf-li-style: decimal;
        -pdf-li-postfix: .;
        -pdf-li-group: legal;
    }
    .indented {
        -pdf-li-inset: 60pt;
        -pdf-li-align: left;
    }
</style>

<ol class="legal-list">
    <li>First provision</li>
    <li>Second provision</li>
</ol>
```

---

## Supported Elements

### List Container Elements

- `<ol>` - Ordered list (HTMLListOrdered)
- `<ul>` - Unordered list (HTMLListUnordered)
- `<menu>` - Menu list (HTMLListMenu)

All list-level attributes (`data-li-*`) are supported.

### List Item Element

- `<li>` - List item (HTMLListItem)

Supports: `data-li-inset`, `data-li-align`, `data-li-label`

---

## Attribute Details

### data-li-style

Controls the numbering style for ordered lists.

**Type**: Enumeration (ListNumberingGroupStyle)

**Values**:
- `decimal` - Arabic numerals (1, 2, 3, ...)
- `upper-roman` - Uppercase Roman (I, II, III, ...)
- `lower-roman` - Lowercase Roman (i, ii, iii, ...)
- `upper-alpha` - Uppercase letters (A, B, C, ...)
- `lower-alpha` - Lowercase letters (a, b, c, ...)
- `disc` - Bullet disc (•)
- `circle` - Bullet circle (○)
- `none` - No numbering displayed

**Default**: `decimal` for `<ol>`, `disc` for `<ul>`

**CSS Equivalent**: `list-style-type` (standard CSS property)

```html
<ol data-li-style="upper-roman">
    <li>First</li>
    <li>Second</li>
</ol>
<!-- Output: I. First, II. Second -->
```

---

### data-li-group

Assigns a list to a named numbering group. Lists in the same group share continuous numbering across the entire document.

**Type**: String

**Default**: Each list has independent numbering

**Case Sensitive**: Yes

**Behavior**:
- First list in a group starts at 1
- Subsequent lists in same group continue numbering
- Group names are document-scoped
- Different groups maintain independent counters

**CSS Equivalent**: `-pdf-li-group`

```html
<!-- First occurrence of group "main" -->
<ol data-li-group="main">
    <li>Item 1</li>
    <li>Item 2</li>
</ol>

<!-- Content in between -->
<p>Some paragraph text</p>

<!-- Continues numbering from group "main" -->
<ol data-li-group="main">
    <li>Item 3</li>  <!-- Continues as 3 -->
    <li>Item 4</li>  <!-- Continues as 4 -->
</ol>
```

---

### data-li-concat

Concatenates child list numbers with parent list numbers.

**Type**: Boolean

**Values**:
- `true` - Concatenate with parent
- `false` - Independent numbering (default)

**CSS Equivalent**: `-pdf-li-concat` (accepts `true`, `1`, or `concatenate`)

**Behavior**:
- Nested lists inherit and append their number to parent
- Creates multi-level numbering (e.g., 1.a, 1.b, 2.a)
- Works with prefixes and postfixes
- Multiple nesting levels supported

```html
<ol data-li-postfix=".">
    <li>First item
        <ol data-li-concat="true" data-li-prefix=" " data-li-postfix=".">
            <li>Sub-item one</li>     <!-- 1. a. -->
            <li>Sub-item two</li>     <!-- 1. b. -->
        </ol>
    </li>
    <li>Second item</li>              <!-- 2. -->
</ol>
```

---

### data-li-prefix

Text prepended before the list number.

**Type**: String

**Default**: Empty string

**Behavior**:
- Added before the number
- Can include spaces and special characters
- In CSS, wrap in quotes to preserve spaces: `'# '`
- Without quotes in CSS, whitespace is trimmed

**CSS Equivalent**: `-pdf-li-prefix`

```html
<!-- HTML attribute -->
<ol data-li-prefix="# ">
    <li>Item</li>  <!-- # 1 -->
</ol>

<!-- CSS property -->
<style>
    .custom { -pdf-li-prefix: '# '; }
</style>
<ol class="custom">
    <li>Item</li>  <!-- # 1 -->
</ol>
```

---

### data-li-postfix

Text appended after the list number.

**Type**: String

**Default**: Empty string (ordered lists default to `.` in base style)

**Behavior**:
- Added after the number
- Can include spaces and special characters
- In CSS, wrap in quotes to preserve spaces
- Without quotes in CSS, whitespace is trimmed

**CSS Equivalent**: `-pdf-li-postfix`

```html
<ol data-li-postfix=")">
    <li>Item</li>  <!-- 1) -->
</ol>

<ol data-li-prefix="(" data-li-postfix=")">
    <li>Item</li>  <!-- (1) -->
</ol>
```

---

### data-li-inset

Width of the number block, controlling indentation of list content.

**Type**: Unit (points, inches, cm, etc.)

**Default**: 30pt for numbers, 10pt alley to content

**Behavior**:
- Defines total width allocated for the number
- Content starts after inset plus 10pt alley
- Increase for long numbers or concatenated lists
- Can be set on list (affects all items) or individual items

**CSS Equivalent**: `-pdf-li-inset`

```html
<!-- List level -->
<ol data-li-inset="50pt">
    <li>Item with wide number space</li>
</ol>

<!-- Item level -->
<ol>
    <li data-li-inset="60pt">Extra wide item</li>
    <li>Normal width item</li>
</ol>
```

---

### data-li-align

Horizontal alignment of the number within the number block.

**Type**: Enumeration (HorizontalAlignment)

**Values**:
- `right` - Align right (default)
- `left` - Align left
- `center` - Align center

**Default**: `right`

**Behavior**:
- Controls how number aligns within the inset width
- Right alignment typical for numbered lists
- Left alignment common for bullets or custom labels

**CSS Equivalent**: `-pdf-li-align`

```html
<ol data-li-align="left">
    <li>Left-aligned number</li>
</ol>

<ol data-li-inset="50pt" data-li-align="center">
    <li>Centered number in 50pt space</li>
</ol>
```

---

### data-li-label

Custom text label for an individual list item, overriding generated numbers.

**Type**: String

**Default**: Auto-generated number

**Behavior**:
- Replaces the automatic number
- Only affects the specific item
- Still respects alignment and inset
- Other items continue automatic numbering
- Useful for special markers or annotations

**CSS Equivalent**: None (attribute only)

```html
<ol>
    <li>Item 1</li>
    <li data-li-label="*">Special item</li>  <!-- Displays: * Special item -->
    <li>Item 3</li>
</ol>
```

---

## Notes

### CSS vs HTML Attributes

**HTML Attributes**:
- Direct on element: `data-li-prefix="# "`
- Immediate evaluation
- Spaces preserved in quoted values

**CSS Properties**:
- In style sheets: `-pdf-li-prefix: '# ';`
- Must quote values with spaces
- Without quotes, whitespace trimmed
- Can be applied via classes

### Concatenation Behavior

- Child lists show combined numbering: `parent.child`
- Prefix/postfix of both levels included
- Example with concat: `1. a. First sub-item`
- Requires explicit `data-li-concat="true"`

### Grouping Rules

- Group names are case-sensitive
- Groups span entire document
- First use initializes counter at 1
- Subsequent uses continue counting
- Different groups are independent
- Useful for multi-section legal documents

### Number Block Layout

The number block layout consists of:
1. **Prefix** - Custom text before number
2. **Number** - Generated or custom number
3. **Postfix** - Custom text after number
4. **Alignment** - Within inset width (default right)
5. **Alley** - 10pt space before content

Total space: `inset width + 10pt alley`

### Style Inheritance

- List-level attributes apply to all items
- Item-level attributes override list defaults
- CSS class styles can be overridden by inline attributes
- Nesting creates inheritance hierarchy

### Performance

- Style caching recommended for large lists
- Use consistent formatting for efficiency
- Avoid per-item variations when possible
- Group similar lists for performance

---

## Examples

### 1. Basic Numbered List with Period

Standard decimal numbering:

```html
<ol data-li-postfix=".">
    <li>First item</li>
    <li>Second item</li>
    <li>Third item</li>
</ol>

<!-- Output:
     1. First item
     2. Second item
     3. Third item
-->
```

### 2. Roman Numeral List

Use Roman numerals:

```html
<ol data-li-style="upper-roman" data-li-postfix=".">
    <li>Introduction</li>
    <li>Main Content</li>
    <li>Conclusion</li>
</ol>

<!-- Output:
     I. Introduction
     II. Main Content
     III. Conclusion
-->
```

### 3. Alphabetic List

Letter-based numbering:

```html
<ol data-li-style="lower-alpha" data-li-postfix=")">
    <li>Option A</li>
    <li>Option B</li>
    <li>Option C</li>
</ol>

<!-- Output:
     a) Option A
     b) Option B
     c) Option C
-->
```

### 4. Custom Prefix and Postfix

Add custom text around numbers:

```html
<ol data-li-prefix="Section " data-li-postfix=":">
    <li>Requirements</li>
    <li>Implementation</li>
</ol>

<!-- Output:
     Section 1: Requirements
     Section 2: Implementation
-->
```

### 5. Grouped Lists for Continuous Numbering

Maintain numbering across separate lists:

```html
<h3>Part One</h3>
<ol data-li-group="chapters" data-li-postfix=".">
    <li>Introduction</li>
    <li>Background</li>
</ol>

<p>Some content between lists</p>

<h3>Part Two</h3>
<ol data-li-group="chapters" data-li-postfix=".">
    <li>Methodology</li>      <!-- Continues as 3. -->
    <li>Results</li>          <!-- Continues as 4. -->
</ol>
```

### 6. Nested List with Concatenation

Multi-level numbering:

```html
<ol data-li-postfix=".">
    <li>First Major Point
        <ol data-li-style="lower-alpha"
            data-li-concat="true"
            data-li-prefix=" "
            data-li-postfix=".">
            <li>Sub-point one</li>     <!-- 1. a. -->
            <li>Sub-point two</li>     <!-- 1. b. -->
        </ol>
    </li>
    <li>Second Major Point</li>        <!-- 2. -->
</ol>
```

### 7. Legal Document Numbering

Professional legal formatting:

```html
<style>
    .legal-main {
        -pdf-li-style: decimal;
        -pdf-li-postfix: .;
        -pdf-li-group: legal;
    }
    .legal-sub {
        -pdf-li-style: lower-alpha;
        -pdf-li-concat: concatenate;
        -pdf-li-prefix: ' ';
        -pdf-li-postfix: .;
    }
</style>

<ol class="legal-main">
    <li>General Provisions
        <ol class="legal-sub">
            <li>Definitions</li>       <!-- 1. a. -->
            <li>Scope</li>             <!-- 1. b. -->
        </ol>
    </li>
    <li>Specific Terms
        <ol class="legal-sub">
            <li>Payment</li>           <!-- 2. a. -->
            <li>Delivery</li>          <!-- 2. b. -->
        </ol>
    </li>
</ol>
```

### 8. Custom Bullet Prefix

Add prefix to unordered lists:

```html
<ul data-li-prefix="# ">
    <li>First bullet</li>
    <li>Second bullet</li>
    <li>Third bullet</li>
</ul>

<!-- Output:
     # • First bullet
     # • Second bullet
     # • Third bullet
-->
```

### 9. Wide Number Inset for Concatenated Lists

Increase space for long numbers:

```html
<ol data-li-postfix=".">
    <li>Main item
        <ol data-li-concat="true"
            data-li-prefix=" "
            data-li-postfix="."
            data-li-inset="50pt"
            data-li-align="left">
            <li>Sub-item with extra space</li>
        </ol>
    </li>
</ol>
```

### 10. Left-Aligned Numbers

Align numbers to the left:

```html
<ol data-li-align="left" data-li-inset="40pt">
    <li>Left-aligned one</li>
    <li>Left-aligned two</li>
    <li>Left-aligned three</li>
</ol>
```

### 11. Custom Labels for Specific Items

Override automatic numbering:

```html
<ol>
    <li>First item</li>
    <li>Second item</li>
    <li data-li-label="*">Important item marked with asterisk</li>
    <li>Fourth item (continues as 4)</li>
</ol>

<!-- Output:
     1. First item
     2. Second item
     * Important item marked with asterisk
     4. Fourth item
-->
```

### 12. Three-Level Nested Numbering

Deep hierarchy with concatenation:

```html
<ol data-li-postfix=".">
    <li>Level 1
        <ol data-li-style="lower-alpha"
            data-li-concat="true"
            data-li-prefix=" "
            data-li-postfix=".">
            <li>Level 2
                <ol data-li-style="lower-roman"
                    data-li-concat="true"
                    data-li-prefix=" "
                    data-li-postfix="."
                    data-li-inset="60pt"
                    data-li-align="left">
                    <li>Level 3 item</li>  <!-- 1. a. i. -->
                </ol>
            </li>
        </ol>
    </li>
</ol>
```

### 13. Technical Manual Section Numbering

Section numbering with groups:

```html
<h2>Chapter 1</h2>
<ol data-li-group="sections" data-li-prefix="Section " data-li-postfix=":">
    <li>Overview</li>         <!-- Section 1: -->
    <li>Installation</li>     <!-- Section 2: -->
</ol>

<h2>Chapter 2</h2>
<ol data-li-group="sections" data-li-prefix="Section " data-li-postfix=":">
    <li>Configuration</li>    <!-- Section 3: -->
    <li>Usage</li>            <!-- Section 4: -->
</ol>
```

### 14. Outline Format with Parentheses

Standard outline style:

```html
<ol data-li-prefix="(" data-li-postfix=")">
    <li>First Point
        <ol data-li-style="lower-alpha"
            data-li-prefix="(" data-li-postfix=")">
            <li>Sub-point A</li>      <!-- (a) -->
            <li>Sub-point B</li>      <!-- (b) -->
        </ol>
    </li>
    <li>Second Point</li>             <!-- (2) -->
</ol>
```

### 15. Mixed Content with Data Binding

Dynamic list with formatting:

```html
<ol data-li-style="upper-roman" data-li-postfix=".">
    <template data-bind="{{model.chapters}}">
        <li>
            <strong>{{.title}}</strong>
            <ol data-li-style="decimal"
                data-li-concat="true"
                data-li-prefix=" "
                data-li-postfix=".">
                <template data-bind="{{.sections}}">
                    <li>{{.name}}</li>
                </template>
            </ol>
        </li>
    </template>
</ol>
```

### 16. Custom Numbered Checkboxes

Create checkbox-style numbering:

```html
<ol data-li-prefix="[ ] ">
    <li>Task one</li>          <!-- [ ] 1. Task one -->
    <li>Task two</li>          <!-- [ ] 2. Task two -->
    <li>Task three</li>        <!-- [ ] 3. Task three -->
</ol>
```

### 17. Department Procedures with Sections

Corporate document numbering:

```html
<style>
    .procedure {
        -pdf-li-prefix: 'Procedure ';
        -pdf-li-postfix: :;
        -pdf-li-group: procedures;
    }
    .step {
        -pdf-li-prefix: 'Step ';
        -pdf-li-postfix: .;
    }
</style>

<ol class="procedure">
    <li>Safety Protocols
        <ol class="step">
            <li>Check equipment</li>
            <li>Verify settings</li>
        </ol>
    </li>
    <li>Operating Guidelines
        <ol class="step">
            <li>Power on</li>
            <li>Initialize</li>
        </ol>
    </li>
</ol>
```

### 18. Conditional List Item Labels

Use custom labels based on data:

```html
<ol>
    <template data-bind="{{model.items}}">
        <li data-li-label="{{.isPriority ? '★' : ''}}">
            {{.description}}
        </li>
    </template>
</ol>
```

### 19. Question and Answer Numbering

Q&A format:

```html
<ol data-li-prefix="Q" data-li-postfix=":">
    <li>What is Scryber?
        <ol data-li-prefix="A" data-li-postfix=":" data-li-style="none">
            <li>Scryber is a PDF generation library.</li>
        </ol>
    </li>
    <li>How do I install it?
        <ol data-li-prefix="A" data-li-postfix=":" data-li-style="none">
            <li>Use NuGet package manager.</li>
        </ol>
    </li>
</ol>

<!-- Output:
     Q1: What is Scryber?
     A: Scryber is a PDF generation library.
     Q2: How do I install it?
     A: Use NuGet package manager.
-->
```

### 20. Specification Document with Article Numbering

Formal specification style:

```html
<ol data-li-prefix="Article " data-li-postfix="." data-li-group="articles">
    <li>Definitions
        <ol data-li-concat="true" data-li-prefix=" Para " data-li-postfix=".">
            <li>The term "document" means...</li>
            <li>The term "user" means...</li>
        </ol>
    </li>
    <li>Scope
        <ol data-li-concat="true" data-li-prefix=" Para " data-li-postfix=".">
            <li>This specification applies to...</li>
        </ol>
    </li>
</ol>

<!-- Output:
     Article 1. Definitions
     Article 1. Para 1. The term "document" means...
     Article 1. Para 2. The term "user" means...
     Article 2. Scope
     Article 2. Para 1. This specification applies to...
-->
```

---

## See Also

- [ol element](/reference/htmltags/ol.html) - Ordered list element
- [ul element](/reference/htmltags/ul.html) - Unordered list element
- [li element](/reference/htmltags/li.html) - List item element
- [Lists Reference](/reference/components/lists.html) - Complete lists guide
- [CSS List Styling](/reference/styles/list-styles.html) - Standard CSS list properties
- [ListOrdered Component](/reference/components/listordered.html) - Base list component
- [List Samples](/samples/lists.html) - List examples and demonstrations

---
