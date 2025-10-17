---
layout: default
title: menu
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;menu&gt; : The Menu List Element

The `<menu>` element represents an unordered list of menu items. It behaves identically to `<ul>` but with a default list style of "none" (no bullets or markers).

## Usage

The `<menu>` element creates an unordered list without default markers, making it suitable for:
- Navigation menus
- Toolbars and action lists
- Command lists
- Context menus
- Lists where markers should be styled or omitted

```html
<menu>
    <li>Home</li>
    <li>Products</li>
    <li>Services</li>
    <li>Contact</li>
</menu>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the list. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element. |

### List Numbering Attributes

While `<menu>` is an unordered list, it supports the same numbering control attributes as `<ul>`:

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-li-style` | string | List item marker style. Default: `None` for menu. |
| `data-li-group` | string | Shared numbering group name for continued sequences. |
| `data-li-concat` | boolean | Whether to concatenate parent numbers (e.g., "1.1", "1.2"). |
| `data-li-prefix` | string | Text to display before each marker. |
| `data-li-postfix` | string | Text to display after each marker. |
| `data-li-inset` | unit | Indentation of list markers from the left edge. |
| `data-li-align` | string | Alignment of list markers: `left`, `center`, `right`. |

### CSS Style Support

**List Styling**:
- `list-style-type`: Marker type (though default is `none`)
- `list-style-position`: `inside` or `outside`
- `-pdf-li-style`, `-pdf-li-inset`, `-pdf-li-align`, `-pdf-li-prefix`, `-pdf-li-postfix`

**Box Model**:
- `margin`, `padding`, `border`
- `width`, `min-width`, `max-width`

**Layout**:
- `display`: `block`, `flex`, `grid`
- `flex-direction`, `justify-content`, `align-items`

---

## Notes

### Menu vs UL

The key difference between `<menu>` and `<ul>`:

| Feature | `<ul>` | `<menu>` |
|---------|--------|----------|
| Default marker | Disc/bullet | None |
| Default list-style-type | `Disc` | `None` |
| Semantic purpose | General unordered lists | Menu/command lists |
| Behavior | Identical otherwise | Identical otherwise |

### Default Styling

The `<menu>` element has:
- **Default list-style-type**: `None` (no markers)
- All other properties inherited from `ListUnordered`
- Block display
- Standard list indentation (unless styled otherwise)

### When to Use Menu vs UL

**Use `<menu>` for**:
- Navigation menus
- Toolbars
- Action button lists
- Context menus
- Lists where you'll add custom styling/markers

**Use `<ul>` for**:
- General content lists
- Lists where default bullets are desired
- Traditional unordered lists

### Adding Markers to Menu

While menu has no default markers, you can add them via CSS:

```css
menu {
    list-style-type: disc;
}
```

Or using data attributes:
```html
<menu data-li-style="Disc">
```

---

## Examples

### Basic Navigation Menu

```html
<style>
    .nav-menu {
        margin: 0;
        padding: 0;
    }

    .nav-menu li {
        display: inline-block;
        margin-right: 20pt;
    }
</style>

<menu class="nav-menu">
    <li>Home</li>
    <li>About</li>
    <li>Services</li>
    <li>Contact</li>
</menu>
```

### Vertical Action Menu

```html
<style>
    .action-menu {
        border: 1pt solid #ddd;
        padding: 10pt;
        width: 150pt;
    }

    .action-menu li {
        padding: 5pt;
        border-bottom: 1pt solid #eee;
    }

    .action-menu li:hover {
        background-color: #f5f5f5;
    }
</style>

<menu class="action-menu">
    <li>Open</li>
    <li>Save</li>
    <li>Export</li>
    <li>Print</li>
    <li>Close</li>
</menu>
```

### Menu with Custom Markers

```html
<style>
    .custom-menu {
        list-style-type: none;
        padding-left: 0;
    }

    .custom-menu li::before {
        content: "▶ ";
        color: #336699;
        font-weight: bold;
    }
</style>

<menu class="custom-menu">
    <li>Dashboard</li>
    <li>Reports</li>
    <li>Analytics</li>
    <li>Settings</li>
</menu>
```

### Horizontal Menu Bar

```html
<style>
    .menu-bar {
        background-color: #2c3e50;
        margin: 0;
        padding: 0;
        overflow: hidden;
    }

    .menu-bar li {
        display: inline-block;
        color: white;
        padding: 10pt 15pt;
        font-weight: bold;
    }
</style>

<menu class="menu-bar">
    <li>Home</li>
    <li>Products</li>
    <li>Services</li>
    <li>About</li>
    <li>Contact</li>
</menu>
```

### Context Menu Styling

```html
<style>
    .context-menu {
        background-color: white;
        border: 1pt solid #999;
        box-shadow: 2pt 2pt 5pt rgba(0,0,0,0.2);
        padding: 5pt 0;
        width: 180pt;
    }

    .context-menu li {
        padding: 8pt 15pt;
        cursor: pointer;
    }

    .context-menu li.separator {
        border-bottom: 1pt solid #ddd;
        margin: 5pt 0;
        padding: 0;
        height: 0;
    }
</style>

<menu class="context-menu">
    <li>Cut</li>
    <li>Copy</li>
    <li>Paste</li>
    <li class="separator"></li>
    <li>Delete</li>
    <li>Rename</li>
    <li class="separator"></li>
    <li>Properties</li>
</menu>
```

### Menu with Icons/Symbols

```html
<style>
    .icon-menu li {
        padding: 8pt 10pt;
    }

    .icon {
        display: inline-block;
        width: 20pt;
        text-align: center;
        margin-right: 5pt;
        font-weight: bold;
    }
</style>

<menu class="icon-menu">
    <li><span class="icon">🏠</span> Home</li>
    <li><span class="icon">👤</span> Profile</li>
    <li><span class="icon">⚙</span> Settings</li>
    <li><span class="icon">📧</span> Messages</li>
    <li><span class="icon">🚪</span> Logout</li>
</menu>
```

### Data-Bound Menu

```html
<!-- With model.menuItems = [{text: "Item 1"}, {text: "Item 2"}] -->
<style>
    .dynamic-menu {
        border: 1pt solid #ccc;
        padding: 10pt;
    }

    .dynamic-menu li {
        padding: 5pt;
    }
</style>

<menu class="dynamic-menu">
    <template data-bind="{{model.menuItems}}">
        <li>{{.text}}</li>
    </template>
</menu>
```

### Nested Menu (Submenu)

```html
<style>
    .main-menu {
        background-color: #34495e;
        padding: 10pt;
    }

    .main-menu > li {
        color: white;
        font-weight: bold;
        margin-bottom: 5pt;
    }

    .submenu {
        margin-left: 20pt;
        margin-top: 5pt;
    }

    .submenu li {
        color: #ecf0f1;
        font-weight: normal;
        font-size: 9pt;
        padding: 3pt 0;
    }
</style>

<menu class="main-menu">
    <li>
        File
        <menu class="submenu">
            <li>New</li>
            <li>Open</li>
            <li>Save</li>
            <li>Exit</li>
        </menu>
    </li>
    <li>
        Edit
        <menu class="submenu">
            <li>Cut</li>
            <li>Copy</li>
            <li>Paste</li>
        </menu>
    </li>
</menu>
```

### Two-Column Menu

```html
<style>
    .two-column-menu {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 10pt;
        padding: 10pt;
        border: 1pt solid #ddd;
    }

    .two-column-menu li {
        padding: 5pt;
        border-bottom: 1pt dotted #ccc;
    }
</style>

<menu class="two-column-menu">
    <li>Option 1</li>
    <li>Option 2</li>
    <li>Option 3</li>
    <li>Option 4</li>
    <li>Option 5</li>
    <li>Option 6</li>
</menu>
```

### Menu with Descriptions

```html
<style>
    .desc-menu li {
        padding: 10pt;
        border-bottom: 1pt solid #eee;
    }

    .menu-title {
        font-weight: bold;
        color: #2c3e50;
        font-size: 11pt;
    }

    .menu-desc {
        color: #7f8c8d;
        font-size: 9pt;
        margin-top: 3pt;
    }
</style>

<menu class="desc-menu">
    <li>
        <div class="menu-title">Dashboard</div>
        <div class="menu-desc">View overview and statistics</div>
    </li>
    <li>
        <div class="menu-title">Reports</div>
        <div class="menu-desc">Generate and view reports</div>
    </li>
    <li>
        <div class="menu-title">Settings</div>
        <div class="menu-desc">Configure application preferences</div>
    </li>
</menu>
```

### Toolbar Menu

```html
<style>
    .toolbar {
        background-color: #ecf0f1;
        border: 1pt solid #bdc3c7;
        padding: 5pt;
    }

    .toolbar li {
        display: inline-block;
        padding: 5pt 10pt;
        margin-right: 2pt;
        background-color: white;
        border: 1pt solid #bdc3c7;
        font-size: 9pt;
    }
</style>

<menu class="toolbar">
    <li>New</li>
    <li>Open</li>
    <li>Save</li>
    <li>|</li>
    <li>Cut</li>
    <li>Copy</li>
    <li>Paste</li>
    <li>|</li>
    <li>Undo</li>
    <li>Redo</li>
</menu>
```

### Menu with Badges

```html
<style>
    .badge-menu li {
        padding: 8pt;
        position: relative;
    }

    .badge {
        background-color: #e74c3c;
        color: white;
        border-radius: 10pt;
        padding: 2pt 6pt;
        font-size: 8pt;
        margin-left: 5pt;
    }
</style>

<menu class="badge-menu">
    <li>Home</li>
    <li>Messages <span class="badge">3</span></li>
    <li>Notifications <span class="badge">12</span></li>
    <li>Settings</li>
</menu>
```

### Adding Markers via Data Attribute

```html
<!-- Menu with markers added via data attribute -->
<menu data-li-style="Disc">
    <li>Item with disc marker</li>
    <li>Another item</li>
    <li>Third item</li>
</menu>

<!-- Menu with custom prefix -->
<menu data-li-prefix="→ ">
    <li>Arrow prefix item</li>
    <li>Another item</li>
</menu>
```

---

## See Also

- [ul](/reference/htmltags/ul.html) - Unordered list (with default markers)
- [li](/reference/htmltags/li.html) - List item element
- [ol](/reference/htmltags/ol.html) - Ordered list
- [nav](/reference/htmltags/nav.html) - Navigation section
- [CSS List Styling](/reference/styles/lists.html) - List styling reference

---
