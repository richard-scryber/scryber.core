---
layout: default
title: Display & Visibility
nav_order: 6
parent: Styling & Appearance
parent_url: /learning/03-styling/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Display & Visibility

Control element display modes, visibility, and conditional rendering in your PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Use display property (block, inline, inline-block, none)
- Control visibility with the visibility property
- Hide elements conditionally with data binding
- Understand the difference between display and visibility
- Create responsive, conditional layouts

---

## The display Property

Controls how an element is displayed in the document flow.

### display: block

Element takes full width and starts on a new line.

```css
.block {
    display: block;
    width: 100%;  /* Can set width */
    margin: 10pt 0;  /* Top/bottom margins work */
}
```

**Default block elements:** `div`, `p`, `h1-h6`, `section`, `article`, `table`

**Characteristics:**
- Takes full available width
- Starts on a new line
- Can set width and height
- Top and bottom margins work

### display: inline

Element flows with text, takes only needed width.

```css
.inline {
    display: inline;
    /* width and height have no effect */
    /* top/bottom margins have no effect */
    padding: 0 5pt;  /* Left/right padding works */
}
```

**Default inline elements:** `span`, `a`, `strong`, `em`, `code`

**Characteristics:**
- Flows with text
- Takes only content width
- Cannot set width/height
- Top/bottom margins ignored
- Left/right padding and margins work

### display: inline-block

Hybrid: flows inline but can have dimensions.

```css
.inline-block {
    display: inline-block;
    width: 100pt;  /* Width works */
    height: 50pt;  /* Height works */
    margin: 10pt;  /* All margins work */
    padding: 10pt;  /* All padding works */
}
```

**Characteristics:**
- Flows inline with other elements
- Can set width and height
- All margins and padding work
- Useful for buttons, badges, inline boxes

### display: none

Completely removes element from document flow.

```css
.hidden {
    display: none;  /* Element not rendered, no space occupied */
}
```

**Characteristics:**
- Element not rendered
- No space occupied
- Not accessible
- Useful for conditional content

### display: table and table-cell

Create table-like layouts without `<table>` elements.

```css
.table {
    display: table;
    width: 100%;
}

.table-row {
    display: table-row;
}

.table-cell {
    display: table-cell;
    padding: 10pt;
    vertical-align: top;
}
```

---

## The visibility Property

Controls whether an element is visible without affecting layout.

### visibility: visible (default)

Element is visible.

```css
.visible {
    visibility: visible;
}
```

### visibility: hidden

Element is hidden but space is preserved.

```css
.hidden {
    visibility: hidden;  /* Hidden but space occupied */
}
```

**Characteristics:**
- Element hidden
- Space still occupied
- Layout not affected

---

## display: none vs visibility: hidden

### Key Differences

```html
<style>
.display-none {
    display: none;  /* No space, removed from flow */
}

.visibility-hidden {
    visibility: hidden;  /* Space preserved */
}
</style>

<p>Before</p>
<p class="display-none">This paragraph is display: none</p>
<p>After (no gap)</p>

<p>Before</p>
<p class="visibility-hidden">This paragraph is visibility: hidden</p>
<p>After (gap present)</p>
```

**Use `display: none` when:**
- You want to completely remove the element
- You don't want it to take up space
- Conditional content that may or may not exist

**Use `visibility: hidden` when:**
- You want to maintain layout spacing
- You want to toggle visibility without layout shifts
- Temporarily hiding content

---

## Conditional Display with Data Binding

Control element visibility based on data.

### Using Conditional Blocks

{% raw %}
```html
<!-- Show only if condition is true -->
{{#if model.showSection}}
    <div class="content">
        <h2>{{model.title}}</h2>
        <p>{{model.content}}</p>
    </div>
{{/if}}

<!-- Show/hide with else -->
{{#if model.isPremium}}
    <div class="premium-features">
        <h2>Premium Features</h2>
        <!-- Premium content -->
    </div>
{{else}}
    <div class="upgrade-prompt">
        <h2>Upgrade to Premium</h2>
        <!-- Upgrade information -->
    </div>
{{/if}}
```
{% endraw %}

### Using Inline Conditionals with display

{% raw %}
```html
<div style="display: {{if(model.showContent, 'block', 'none')}};">
    <p>This content is conditionally displayed.</p>
</div>

<!-- More complex conditions -->
<div style="display: {{if(model.userLevel >= 5, 'block', 'none')}};">
    <p>Advanced content for level 5+ users.</p>
</div>
```
{% endraw %}

---

## Practical Examples

### Example 1: Conditional Sections

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Conditional Display</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }

        h1 {
            color: #1e40af;
        }

        .section {
            padding: 20pt;
            margin-bottom: 20pt;
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
        }

        .section h2 {
            margin-top: 0;
            color: #2563eb;
        }

        .premium-badge {
            display: inline-block;
            background-color: #fef3c7;
            color: #92400e;
            padding: 5pt 10pt;
            border-radius: 3pt;
            font-size: 9pt;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <h1>User Dashboard</h1>

    <!-- Always visible section -->
    <div class="section">
        <h2>Welcome</h2>
        <p>Welcome to your dashboard!</p>
    </div>

    <!-- Conditional sections -->
    {% raw %}
    {{#if model.showStats}}
        <div class="section">
            <h2>Statistics</h2>
            <p>Total orders: {{model.stats.orders}}</p>
            <p>Revenue: {{format(model.stats.revenue, 'C0')}}</p>
        </div>
    {{/if}}

    {{#if model.isPremium}}
        <div class="section">
            <h2>Premium Features <span class="premium-badge">PREMIUM</span></h2>
            <p>Access to exclusive features and content.</p>
        </div>
    {{else}}
        <div class="section">
            <h2>Upgrade to Premium</h2>
            <p>Unlock advanced features by upgrading to premium.</p>
        </div>
    {{/if}}

    {{#if model.hasNotifications}}
        <div class="section">
            <h2>Notifications ({{model.notifications.length}})</h2>
            {{#each model.notifications}}
                <p>- {{this.message}}</p>
            {{/each}}
        </div>
    {{else}}
        <div class="section">
            <p style="color: #666;">No new notifications.</p>
        </div>
    {{/if}}
    {% endraw %}
</body>
</html>
```

### Example 2: Badge and Button Styles

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Inline-Block Elements</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }

        .badge {
            display: inline-block;
            padding: 5pt 12pt;
            border-radius: 15pt;
            font-size: 9pt;
            font-weight: bold;
            margin-right: 5pt;
        }

        .badge-success {
            background-color: #d1fae5;
            color: #065f46;
        }

        .badge-warning {
            background-color: #fef3c7;
            color: #92400e;
        }

        .badge-error {
            background-color: #fee2e2;
            color: #991b1b;
        }

        .badge-info {
            background-color: #dbeafe;
            color: #1e40af;
        }

        .button {
            display: inline-block;
            padding: 10pt 20pt;
            border-radius: 5pt;
            font-size: 11pt;
            font-weight: bold;
            text-decoration: none;
            margin-right: 10pt;
            margin-bottom: 10pt;
        }

        .button-primary {
            background-color: #2563eb;
            color: white;
            border: 2pt solid #2563eb;
        }

        .button-secondary {
            background-color: white;
            color: #2563eb;
            border: 2pt solid #2563eb;
        }

        .icon {
            display: inline-block;
            width: 20pt;
            height: 20pt;
            background-color: #2563eb;
            border-radius: 50%;
            text-align: center;
            line-height: 20pt;
            color: white;
            font-size: 12pt;
            margin-right: 5pt;
            vertical-align: middle;
        }
    </style>
</head>
<body>
    <h1>Status Badges</h1>

    <p>
        <span class="badge badge-success">Active</span>
        <span class="badge badge-warning">Pending</span>
        <span class="badge badge-error">Failed</span>
        <span class="badge badge-info">New</span>
    </p>

    <h2>Action Buttons</h2>

    <div>
        <a href="#" class="button button-primary">Primary Action</a>
        <a href="#" class="button button-secondary">Secondary Action</a>
    </div>

    <h2>Icons with Text</h2>

    <p>
        <span class="icon">i</span>
        <span style="vertical-align: middle;">Information message</span>
    </p>

    <p>
        <span class="icon">!</span>
        <span style="vertical-align: middle;">Warning message</span>
    </p>
</body>
</html>
```

### Example 3: Two-Column Layout

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Two-Column Layout</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }

        .container {
            display: table;
            width: 100%;
        }

        .sidebar {
            display: table-cell;
            width: 200pt;
            background-color: #f9fafb;
            padding: 20pt;
            vertical-align: top;
            border-right: 1pt solid #d1d5db;
        }

        .main-content {
            display: table-cell;
            padding: 20pt;
            padding-left: 30pt;
            vertical-align: top;
        }

        .sidebar h3 {
            margin-top: 0;
            color: #1e40af;
            font-size: 14pt;
        }

        .sidebar-item {
            padding: 8pt;
            margin-bottom: 5pt;
            border-radius: 3pt;
        }

        .sidebar-item.active {
            background-color: #dbeafe;
            color: #1e40af;
            font-weight: bold;
        }

        .main-content h1 {
            margin-top: 0;
            color: #1e40af;
        }

        .card {
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
            padding: 15pt;
            margin-bottom: 20pt;
        }

        .card h2 {
            margin: 0 0 10pt 0;
            color: #2563eb;
            font-size: 16pt;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="sidebar">
            <h3>Navigation</h3>
            <div class="sidebar-item active">Dashboard</div>
            <div class="sidebar-item">Reports</div>
            <div class="sidebar-item">Settings</div>
            <div class="sidebar-item">Help</div>
        </div>

        <div class="main-content">
            <h1>Dashboard</h1>

            <div class="card">
                <h2>Recent Activity</h2>
                <p>No recent activity to display.</p>
            </div>

            <div class="card">
                <h2>Quick Stats</h2>
                <p>Total Sales: $125,000</p>
                <p>New Customers: 45</p>
            </div>
        </div>
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Conditional Content

Create a document with:
- Sections that display based on user role
- Premium content toggle
- Notification display when notifications exist
- Empty state when no data

### Exercise 2: Badge System

Create various badges:
- Status badges (success, warning, error)
- Count badges
- Icon badges
- Use inline-block display

### Exercise 3: Multi-Column Layout

Create a layout with:
- Fixed sidebar (display: table-cell)
- Fluid main content
- Toggle sidebar visibility with conditions

---

## Common Pitfalls

### ❌ Using display: inline with Dimensions

```css
.wrong {
    display: inline;
    width: 200pt;   /* Won't work */
    height: 100pt;  /* Won't work */
}
```

✅ **Solution:** Use inline-block

```css
.correct {
    display: inline-block;
    width: 200pt;   /* Works! */
    height: 100pt;  /* Works! */
}
```

### ❌ Confusing display: none and visibility: hidden

```css
/* Want to preserve space? Don't use display: none */
.wrong {
    display: none;  /* Removes space */
}
```

✅ **Solution:** Use visibility

```css
.correct {
    visibility: hidden;  /* Preserves space */
}
```

### ❌ Forgetting Conditional Closing Tags

{% raw %}
```html
{{#if model.showSection}}
    <div>Content</div>
<!-- Missing {{/if}} - will cause errors -->
```
{% endraw %}

✅ **Solution:** Always close blocks

{% raw %}
```html
{{#if model.showSection}}
    <div>Content</div>
{{/if}}
```
{% endraw %}

---

## Display Modes Summary

| Display | Takes Width | Dimensions | Margins | Use Case |
|---------|-------------|------------|---------|----------|
| `block` | Full | Yes | All | Containers, sections |
| `inline` | Content | No | Left/Right | Text formatting |
| `inline-block` | Content | Yes | All | Buttons, badges |
| `table` | Full/Auto | Yes | All | Table layouts |
| `table-cell` | Auto | Yes | None | Column layouts |
| `none` | N/A | N/A | N/A | Hide elements |

---

## Best Practices

1. **Use semantic HTML** - Choose appropriate elements first
2. **display: none** for conditional content
3. **inline-block** for buttons and badges
4. **table-cell** for multi-column layouts
5. **Check closing tags** in conditional blocks
6. **Test all conditions** to ensure proper rendering
7. **Use visibility: hidden** when layout must be preserved

---

## Next Steps

Now that you control display and visibility:

1. **[Style Organization](07_style_organization.md)** - Organize your CSS
2. **[Styling Best Practices](08_styling_best_practices.md)** - Professional patterns
3. **[Layout & Positioning](/learning/04-layout/)** - Advanced layout techniques

---

**Continue learning →** [Style Organization](07_style_organization.md)
