---
layout: default
title: data-content
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-content : The Dynamic Content Attribute

The `data-content` attribute enables dynamic content injection into elements at data binding time in Scryber PDF documents. It allows you to specify content as a string that will be parsed and inserted into the element, replacing or augmenting existing content based on the `data-content-action` setting.

---

## Summary

The `data-content` attribute provides a mechanism for dynamically generating and inserting content into elements during the data binding phase. Unlike static content declared in the template, `data-content` evaluates at runtime, allowing you to:

- Generate content from bound data values
- Insert dynamically constructed HTML/XHTML markup
- Replace element content conditionally
- Prepend or append dynamic content to existing elements
- Create inline templates without nested element structures

This attribute is particularly useful when:
- Content structure varies based on data
- You need to generate markup programmatically
- Template content comes from external sources
- You want to avoid deeply nested template structures

---

## Usage

The `data-content` attribute accepts a string value containing HTML/XHTML markup or plain text. The content is parsed according to the document's default parser or the MIME type specified in `data-content-type`.

### Basic Syntax

```html
<!-- Simple text content -->
<label data-content="{{model.userName}}">Default Name</label>

<!-- HTML markup content -->
<div data-content="<p>Hello <strong>{{model.name}}</strong></p>"></div>

<!-- Dynamic content from bound expression -->
<span data-content="{{model.dynamicContent}}"></span>
```

### Content Actions

The `data-content-action` attribute controls how the content is inserted:

| Action | Behavior | Default |
|--------|----------|---------|
| `append` | Add after existing content | No (except for VisualComponent) |
| `prepend` | Add before existing content | No |
| `replace` | Replace all existing content | Yes (for most components) |

```html
<!-- Replace existing content (default for labels) -->
<label data-content="{{model.newText}}">Old text</label>

<!-- Append to existing content -->
<div data-content="<p>{{model.additionalInfo}}</p>" data-content-action="append">
    <p>Existing content stays here</p>
</div>
```

---

## Supported Elements

The `data-content` attribute is supported on the following elements:

### Primary Support
- `<label>` - Text labels with dynamic content (HTMLLabel)
- `<template>` - Template elements with inline content definition (HTMLTemplate)
- `<frame>` - Frame elements for document assembly (HTMLFrame)

### General Support
- All `VisualComponent` descendants - Any visual element that inherits from VisualComponent

### Implementation Details

**HTMLLabel**: Content is set directly and rendered as text literal during pre-layout phase.

**HTMLTemplate**: Content is parsed as a template and used instead of child elements.

**HTMLFrame**: Content is parsed as HTML document and used for frame content.

**VisualComponent**: Content is parsed according to MIME type and inserted per the specified action.

---

## Binding Values

### Expression Evaluation

The `data-content` attribute value is evaluated during data binding, supporting:

```html
<!-- Direct property binding -->
<label data-content="{{model.userName}}"></label>

<!-- Legacy syntax -->
<label data-content="{@:model.userName}"></label>

<!-- String concatenation -->
<label data-content="{{concat('Hello ', model.name)}}"></label>

<!-- Conditional content -->
<label data-content="{{model.isActive ? 'Active' : 'Inactive'}}"></label>
```

### Content Type Specification

Use `data-content-type` to specify the MIME type of the content:

```html
<!-- HTML content (default) -->
<div data-content="<p>HTML content</p>" data-content-type="text/html"></div>

<!-- XHTML content -->
<div data-content="<div xmlns='http://www.w3.org/1999/xhtml'><p>XHTML</p></div>"
     data-content-type="application/xhtml+xml"></div>

<!-- Plain text -->
<div data-content="{{model.plainText}}" data-content-type="text/plain"></div>
```

### Markup Requirements

Content must be well-formed according to the specified type:

**For HTML/XHTML**:
- Must be valid markup
- Elements must be properly closed
- Attributes must be quoted
- Namespace declarations required for XHTML

**For Plain Text**:
- No parsing applied
- Rendered as literal text
- Special characters handled automatically

---

## Notes

### Content Parsing

1. **Parse Timing**: Content is parsed and inserted during the data binding phase, after all binding expressions are evaluated.

2. **Parser Selection**: The document's parser for the specified MIME type is used. If no type is specified, the default content MIME type is used.

3. **Validation**: In strict conformance mode, parse errors will throw exceptions. In lax mode, warnings are logged.

4. **Component Requirements**: Parsed content must result in compatible components for the target element.

### Label-Specific Behavior

For `<label>` elements:
- Content is set as simple text during pre-layout
- Existing content is cleared
- No HTML parsing occurs
- Most efficient for simple text replacement
- Always replaces existing content

### Template-Specific Behavior

For `<template>` elements:
- Content defines the template to repeat
- Takes precedence over child elements
- Must contain valid HTML/XHTML
- Creates parsable template generator
- Useful for dynamic template definition

### Frame-Specific Behavior

For `<frame>` elements:
- Content must be a complete HTML document
- Parsed as HTMLDocument component
- Used for document assembly scenarios
- Initialized and data-bound after parsing
- Only accepts full XHTML documents

### Content Action Behavior

The default `data-content-action` varies by component:

- **Label**: Always replaces (action ignored)
- **Template**: Always replaces (action ignored)
- **Frame**: Always replaces (action ignored)
- **VisualComponent**: Default is `append`

### Performance Considerations

- Parsing content has overhead; cache when possible
- Use `data-style-identifier` with repeated patterns
- Avoid complex markup in large iterations
- Consider building content in code for complex scenarios
- Pre-process and simplify content when feasible

### Security Considerations

- Content is parsed and executed in the document context
- Validate external content sources
- Sanitize user-provided content
- Be cautious with dynamic code generation
- Use strict conformance mode for validation

---

## Examples

### 1. Simple Text Replacement in Label

Replace label text with bound data:

```html
<!-- Model: { userName: "John Smith" } -->
<label data-content="{{model.userName}}">Loading...</label>

<!-- Result: John Smith -->
```

### 2. Dynamic Formatted Text

Generate formatted content from data:

```html
<!-- Model: { firstName: "Jane", lastName: "Doe", title: "Dr." } -->
<label data-content="{{concat(model.title, ' ', model.firstName, ' ', model.lastName)}}">
    Name Placeholder
</label>

<!-- Result: Dr. Jane Doe -->
```

### 3. Conditional Content Display

Show different content based on conditions:

```html
<!-- Model: { isActive: true, status: "Premium" } -->
<label data-content="{{model.isActive ? concat('Active - ', model.status) : 'Inactive'}}">
    Status Unknown
</label>

<!-- Result: Active - Premium -->
```

### 4. Dynamic HTML Content in Div

Insert formatted HTML markup:

```html
<!-- Model: { productName: "Widget Pro", price: 29.99, inStock: true } -->
<div data-content="<div><strong>{{model.productName}}</strong><br/>Price: ${{model.price}}<br/><em>{{model.inStock ? 'In Stock' : 'Out of Stock'}}</em></div>"></div>

<!-- Renders formatted product info -->
```

### 5. Inline Template Content

Define template content inline without nested elements:

```html
<!-- Model: { items: [{name: "A"}, {name: "B"}] } -->
<template data-bind="{{model.items}}"
          data-content="<div style='padding:5pt; border:1pt solid #ccc;'>{{.name}}</div>">
</template>

<!-- Generates: -->
<!-- <div>A</div> -->
<!-- <div>B</div> -->
```

### 6. Appending Dynamic Content

Add content after existing elements:

```html
<div data-content="<p><em>{{model.disclaimer}}</em></p>" data-content-action="append">
    <p>Main content appears here first.</p>
</div>

<!-- Results in main content followed by disclaimer -->
```

### 7. Prepending Header Content

Add content before existing elements:

```html
<div data-content="<h2>{{model.title}}</h2>" data-content-action="prepend">
    <p>This paragraph will appear after the title.</p>
</div>

<!-- Results in title followed by paragraph -->
```

### 8. Complex Multi-Line HTML Content

Generate complex markup structure:

```html
<!-- Model: { user: {name: "Alice", role: "Admin", department: "IT"} } -->
<div data-content="<div style='border: 2pt solid #336699; padding: 10pt;'>
    <h3 style='margin: 0; color: #336699;'>{{model.user.name}}</h3>
    <div style='margin-top: 5pt;'>
        <strong>Role:</strong> {{model.user.role}}<br/>
        <strong>Department:</strong> {{model.user.department}}
    </div>
</div>">
</div>
```

### 9. Dynamic Table Content Generation

Create table structure dynamically:

```html
<!-- Model: { columns: ["ID", "Name", "Status"], hasData: true } -->
<div data-content="<table style='width:100%;'>
    <thead>
        <tr>
            <template data-bind='{{model.columns}}'>
                <th>{{.}}</th>
            </template>
        </tr>
    </thead>
    <tbody>
        <tr><td colspan='3' style='text-align:center;'>
            {{model.hasData ? 'Data will appear here' : 'No data available'}}
        </td></tr>
    </tbody>
</table>">
</div>
```

### 10. Conditional Section Inclusion

Include entire sections conditionally:

```html
<!-- Model: { showWarning: true, warningMessage: "Important notice" } -->
<div data-content="{{model.showWarning ? concat('<div style=\'background-color: #fff3cd; padding: 10pt; border: 1pt solid #ffc107;\'><strong>Warning:</strong> ', model.warningMessage, '</div>') : ''}}">
</div>

<!-- Only renders if showWarning is true -->
```

### 11. List Generation with Dynamic Styling

Create styled list items:

```html
<!-- Model: { priority: "high", tasks: ["Task A", "Task B"] } -->
<template data-bind="{{model.tasks}}"
          data-content="<div style='padding: 8pt; margin: 5pt; border-left: 3pt solid {{model.priority === 'high' ? '#dc3545' : '#28a745'}};'>{{.}}</div>">
</template>

<!-- Each task gets colored border based on priority -->
```

### 12. Placeholder with Fallback Content

Show dynamic content or fallback:

```html
<!-- Model: { description: null } -->
<div data-content="{{model.description ? model.description : '<em style=\'color: #999;\'>No description available</em>'}}">
    Loading...
</div>

<!-- Shows fallback when description is null -->
```

### 13. Frame with Dynamic Document Content

Assemble documents dynamically:

```html
<!-- Model: { reportContent: "<html>...</html>" } -->
<frameset>
    <frame src="header.html"></frame>
    <frame data-content="{{model.reportContent}}"></frame>
    <frame src="footer.html"></frame>
</frameset>

<!-- Inserts dynamic report between header and footer -->
```

### 14. Localized Content Injection

Insert locale-specific content:

```html
<!-- Model: { locale: "es", welcomeMessages: {en: "Welcome", es: "Bienvenido"} } -->
<label data-content="{{model.welcomeMessages[model.locale]}}">
    Welcome
</label>

<!-- Shows: Bienvenido -->
```

### 15. Rich Text with Embedded Bindings

Create complex formatted content with multiple bindings:

```html
<!-- Model: { order: {id: 1234, date: "2024-10-13", customer: "Acme Corp", total: 1500.00} } -->
<div data-content="<div style='padding: 15pt; border: 2pt solid #336699;'>
    <h2 style='margin: 0 0 10pt 0; color: #336699;'>Order Confirmation</h2>
    <div style='margin-bottom: 5pt;'><strong>Order Number:</strong> {{model.order.id}}</div>
    <div style='margin-bottom: 5pt;'><strong>Date:</strong> {{model.order.date}}</div>
    <div style='margin-bottom: 5pt;'><strong>Customer:</strong> {{model.order.customer}}</div>
    <div style='margin-top: 10pt; padding-top: 10pt; border-top: 1pt solid #ccc;'>
        <strong>Total:</strong> <span style='font-size: 14pt; color: #336699;'>${{model.order.total}}</span>
    </div>
</div>">
</div>
```

### 16. Template with Alternate Content Definition

Define template behavior inline:

```html
<!-- Model: { items: [{type: "A", name: "Item 1"}, {type: "B", name: "Item 2"}] } -->
<template data-bind="{{model.items}}"
          data-content="<div style='padding: 10pt; background-color: {{.type === 'A' ? '#e3f2fd' : '#f3e5f5'}}; margin-bottom: 5pt;'>
              <strong>{{.type}}:</strong> {{.name}}
          </div>">
</template>

<!-- Type A items get blue background, Type B get purple -->
```

---

## See Also

- [data-content-type attribute](/reference/htmlattributes/data-content-type.html) - Specify content MIME type
- [data-content-action attribute](/reference/htmlattributes/data-content-action.html) - Control content insertion
- [label element](/reference/htmltags/label.html) - Label element reference
- [template element](/reference/htmltags/template.html) - Template element reference
- [frame element](/reference/htmltags/frame.html) - Frame element reference
- [Data Binding](/reference/binding/) - Data binding overview
- [Expressions](/reference/expressions/) - Expression syntax guide

---
