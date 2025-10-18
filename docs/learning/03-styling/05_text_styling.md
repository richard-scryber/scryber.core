---
layout: default
title: Text Styling
nav_order: 5
parent: Styling & Appearance
parent_url: /learning/03-styling/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Text Styling

Master text appearance, alignment, spacing, and decoration for professional PDF typography.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Control font size, weight, and style
- Apply text colors and alignment
- Adjust line height and letter spacing
- Use text decoration and transformation
- Control text indentation and spacing
- Create professional typography

---

## Font Size

Control the size of text.

### Basic Font Sizes

```css
/* Point sizes (recommended for PDF) */
.small { font-size: 9pt; }
.normal { font-size: 11pt; }
.large { font-size: 14pt; }
.heading { font-size: 24pt; }

/* Relative sizes */
.relative {
    font-size: 1.2em;  /* 120% of parent */
}

/* Keyword sizes */
.smaller { font-size: smaller; }
.larger { font-size: larger; }
```

### Typography Scale

```css
/* Consistent scale */
body { font-size: 11pt; }
.text-sm { font-size: 9pt; }
.text-base { font-size: 11pt; }
.text-lg { font-size: 13pt; }
.text-xl { font-size: 16pt; }
.text-2xl { font-size: 20pt; }
.text-3xl { font-size: 24pt; }
.text-4xl { font-size: 30pt; }
```

---

## Font Weight

Control the boldness of text.

```css
/* Numeric values */
.thin { font-weight: 100; }
.light { font-weight: 300; }
.normal { font-weight: 400; }
.medium { font-weight: 500; }
.semibold { font-weight: 600; }
.bold { font-weight: 700; }
.extrabold { font-weight: 800; }
.black { font-weight: 900; }

/* Keyword values */
.normal-weight { font-weight: normal; }
.bold-weight { font-weight: bold; }
.bolder { font-weight: bolder; }
.lighter { font-weight: lighter; }
```

**Note:** Not all weights work with all fonts. Standard fonts typically support `normal` (400) and `bold` (700).

---

## Font Style

Control italic and oblique styles.

```css
.normal-style { font-style: normal; }
.italic { font-style: italic; }
.oblique { font-style: oblique; }
```

**Usage:**
```html
<p>This is <em class="italic">emphasized</em> text.</p>
<p>This is <strong class="bold">strong</strong> text.</p>
```

---

## Text Color

Control the color of text.

```css
.text-black { color: #000000; }
.text-gray { color: #666666; }
.text-blue { color: #2563eb; }
.text-red { color: #dc2626; }
.text-green { color: #059669; }

/* With opacity */
.text-faded { color: rgba(0, 0, 0, 0.6); }
```

---

## Text Alignment

Control horizontal alignment of text.

```css
.text-left { text-align: left; }
.text-center { text-align: center; }
.text-right { text-align: right; }
.text-justify { text-align: justify; }
```

**Examples:**
```html
<p class="text-left">Left aligned paragraph.</p>
<p class="text-center">Centered paragraph.</p>
<p class="text-right">Right aligned paragraph.</p>
<p class="text-justify">Justified paragraph with text that spans multiple lines, adjusting spacing between words for even margins on both sides.</p>
```

---

## Line Height

Control the vertical spacing between lines of text.

```css
/* Unitless (recommended) */
.tight { line-height: 1.2; }
.normal { line-height: 1.5; }
.relaxed { line-height: 1.6; }
.loose { line-height: 2.0; }

/* Fixed units */
.fixed-height {
    font-size: 12pt;
    line-height: 18pt;
}

/* Percentage */
.percentage {
    line-height: 150%;  /* 1.5 times font size */
}
```

**Best Practice:** Use unitless values (1.2, 1.5, etc.) for scalability.

---

## Letter Spacing (Tracking)

Control spacing between characters.

```css
/* Tighter spacing */
.tight-letters { letter-spacing: -0.5pt; }

/* Normal spacing */
.normal-letters { letter-spacing: normal; }

/* Wider spacing */
.wide-letters { letter-spacing: 1pt; }
.wider-letters { letter-spacing: 2pt; }

/* Heading letterspacing */
h1 {
    letter-spacing: -1pt;  /* Slightly tighter for large text */
}

/* All caps letterspacing */
.uppercase {
    text-transform: uppercase;
    letter-spacing: 1.5pt;  /* Wider for readability */
}
```

---

## Word Spacing

Control spacing between words.

```css
.tight-words { word-spacing: -2pt; }
.normal-words { word-spacing: normal; }
.wide-words { word-spacing: 5pt; }
```

---

## Text Decoration

Add underlines, overlines, and strikethroughs.

```css
/* Underline */
.underline { text-decoration: underline; }

/* Overline */
.overline { text-decoration: overline; }

/* Line-through (strikethrough) */
.line-through { text-decoration: line-through; }

/* None (remove decoration) */
.no-decoration { text-decoration: none; }

/* Multiple decorations */
.multiple { text-decoration: underline overline; }
```

**Usage:**
```html
<p>This has an <span class="underline">underlined word</span>.</p>
<p>This text is <span class="line-through">crossed out</span>.</p>
<a href="#" class="no-decoration">Link without underline</a>
```

---

## Text Transform

Change text case.

```css
.uppercase { text-transform: uppercase; }
.lowercase { text-transform: lowercase; }
.capitalize { text-transform: capitalize; }
.none { text-transform: none; }
```

**Examples:**
```html
<p class="uppercase">this becomes UPPERCASE</p>
<p class="lowercase">THIS becomes lowercase</p>
<p class="capitalize">this becomes This</p>
```

---

## Text Indentation

Indent the first line of text.

```css
/* Positive indent */
.indent {
    text-indent: 30pt;
}

/* Hanging indent */
.hanging {
    text-indent: -30pt;
    padding-left: 30pt;
}

/* No indent */
.no-indent {
    text-indent: 0;
}
```

---

## White Space

Control how whitespace is handled.

```css
/* Normal - collapse whitespace */
.normal { white-space: normal; }

/* Preserve whitespace and line breaks */
.pre { white-space: pre; }

/* Preserve line breaks, collapse spaces */
.pre-line { white-space: pre-line; }

/* No wrapping */
.nowrap { white-space: nowrap; }
```

---

## Vertical Alignment

Control vertical alignment of inline elements.

```css
.baseline { vertical-align: baseline; }
.top { vertical-align: top; }
.middle { vertical-align: middle; }
.bottom { vertical-align: bottom; }
.text-top { vertical-align: text-top; }
.text-bottom { vertical-align: text-bottom; }

/* Subscript and superscript */
.sub { vertical-align: sub; }
.super { vertical-align: super; }
```

**Usage:**
```html
<p>H<sub class="sub">2</sub>O</p>
<p>E = mc<sup class="super">2</sup></p>
```

---

## Practical Examples

### Example 1: Article Typography

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Article Typography</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
            margin: 40pt;
        }

        h1 {
            font-size: 30pt;
            font-weight: bold;
            line-height: 1.2;
            color: #1e40af;
            margin-bottom: 10pt;
            letter-spacing: -0.5pt;
        }

        .subtitle {
            font-size: 18pt;
            font-weight: normal;
            color: #666;
            margin-bottom: 30pt;
            font-style: italic;
        }

        h2 {
            font-size: 20pt;
            font-weight: bold;
            color: #2563eb;
            margin-top: 30pt;
            margin-bottom: 15pt;
            letter-spacing: -0.3pt;
        }

        .lead {
            font-size: 13pt;
            line-height: 1.8;
            color: #444;
            margin-bottom: 20pt;
        }

        p {
            margin-bottom: 12pt;
            text-align: justify;
        }

        .byline {
            font-size: 10pt;
            color: #666;
            margin-bottom: 30pt;
            font-style: italic;
        }

        .quote {
            font-size: 14pt;
            font-style: italic;
            color: #1e40af;
            line-height: 1.8;
            padding: 20pt;
            padding-left: 30pt;
            border-left: 4pt solid #2563eb;
            margin: 30pt 0;
        }

        .footnote {
            font-size: 9pt;
            color: #666;
            line-height: 1.4;
            margin-top: 40pt;
            padding-top: 15pt;
            border-top: 1pt solid #d1d5db;
        }
    </style>
</head>
<body>
    <h1>The Future of PDF Generation</h1>
    <p class="subtitle">Exploring modern approaches to document creation</p>
    <p class="byline">By John Doe | January 15, 2025</p>

    <p class="lead">
        PDF generation has evolved significantly over the past decade,
        moving from complex, low-level APIs to high-level, HTML-based solutions
        that simplify the development process.
    </p>

    <h2>Introduction</h2>
    <p>
        Modern PDF generation tools leverage familiar web technologies,
        allowing developers to create sophisticated documents using HTML and CSS.
        This approach reduces the learning curve and accelerates development.
    </p>

    <div class="quote">
        "The best way to predict the future is to invent it."
        <span style="display: block; margin-top: 10pt; font-size: 11pt; font-style: normal;">
            — Alan Kay
        </span>
    </div>

    <h2>Key Benefits</h2>
    <p>
        Using HTML for PDF generation provides numerous advantages,
        including faster development, easier maintenance, and the ability
        to leverage existing web development skills and tools.
    </p>

    <p class="footnote">
        * This article represents the author's personal views and experiences.
    </p>
</body>
</html>
```

### Example 2: Business Card with Text Styling

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Business Card</title>
    <style>
        @page {
            size: 3.5in 2in;
            margin: 0;
        }

        body {
            margin: 0;
            font-family: Helvetica, sans-serif;
        }

        .card {
            width: 3.5in;
            height: 2in;
            padding: 0.25in;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }

        .name {
            font-size: 18pt;
            font-weight: bold;
            letter-spacing: 0.5pt;
            margin-bottom: 5pt;
            text-transform: uppercase;
        }

        .title {
            font-size: 11pt;
            font-weight: normal;
            letter-spacing: 1pt;
            margin-bottom: 15pt;
            opacity: 0.9;
            text-transform: uppercase;
        }

        .company {
            font-size: 14pt;
            font-weight: 600;
            margin-bottom: 10pt;
        }

        .contact {
            font-size: 9pt;
            line-height: 1.4;
            opacity: 0.9;
        }
    </style>
</head>
<body>
    <div class="card">
        <div class="name">John Doe</div>
        <div class="title">Senior Developer</div>
        <div class="company">Tech Corp</div>
        <div class="contact">
            john.doe@techcorp.com | +1 (555) 123-4567
        </div>
    </div>
</body>
</html>
```

### Example 3: Document with Various Text Styles

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Text Styling Examples</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 40pt;
        }

        .examples > div {
            padding: 15pt;
            margin-bottom: 20pt;
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
        }

        .examples h3 {
            margin: 0 0 10pt 0;
            color: #1e40af;
            font-size: 14pt;
        }

        .uppercase-text {
            text-transform: uppercase;
            letter-spacing: 2pt;
            font-weight: bold;
        }

        .lowercase-text {
            text-transform: lowercase;
        }

        .capitalize-text {
            text-transform: capitalize;
        }

        .tight-leading {
            line-height: 1.2;
        }

        .loose-leading {
            line-height: 2.0;
        }

        .wide-tracking {
            letter-spacing: 2pt;
        }

        .centered-text {
            text-align: center;
        }

        .right-aligned {
            text-align: right;
        }

        .justified {
            text-align: justify;
        }

        .indented {
            text-indent: 30pt;
        }

        .hanging-indent {
            text-indent: -30pt;
            padding-left: 30pt;
        }
    </style>
</head>
<body>
    <h1>Text Styling Examples</h1>

    <div class="examples">
        <div>
            <h3>Text Transform</h3>
            <p class="uppercase-text">This text is uppercase with wide letter spacing</p>
            <p class="lowercase-text">THIS TEXT BECOMES LOWERCASE</p>
            <p class="capitalize-text">this text is capitalized</p>
        </div>

        <div>
            <h3>Line Height</h3>
            <p class="tight-leading">
                This paragraph has tight line height (1.2). Notice how the lines are closer together,
                which can be useful for compact layouts or saving space.
            </p>
            <p class="loose-leading">
                This paragraph has loose line height (2.0). Notice how the lines are further apart,
                which can improve readability for some content.
            </p>
        </div>

        <div>
            <h3>Letter Spacing</h3>
            <p class="wide-tracking">This text has wide letter spacing</p>
        </div>

        <div>
            <h3>Text Alignment</h3>
            <p class="centered-text">This text is centered</p>
            <p class="right-aligned">This text is right-aligned</p>
            <p class="justified">
                This paragraph uses justified alignment, which adjusts the spacing between words
                to ensure that both the left and right edges are flush. This is commonly used in
                professional documents and books.
            </p>
        </div>

        <div>
            <h3>Text Indentation</h3>
            <p class="indented">
                This paragraph has a first-line indent of 30pt. This is commonly used in books
                and formal documents to indicate the start of a new paragraph without extra vertical spacing.
            </p>
            <p class="hanging-indent">
                This paragraph has a hanging indent. The first line extends to the left while
                subsequent lines are indented. This is useful for lists, bibliographies, and citations.
            </p>
        </div>
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Typography Hierarchy

Create a document with:
- Multiple heading levels (h1-h4)
- Different font sizes, weights, and colors
- Consistent line heights
- Appropriate letter spacing

### Exercise 2: Formatted Paragraph

Create paragraphs with:
- First-line indents
- Justified alignment
- Optimal line height
- Proper spacing between paragraphs

### Exercise 3: Text Decorations

Create examples of:
- Underlined text
- Strikethrough text
- Uppercase with letter spacing
- Italic emphasis

---

## Common Pitfalls

### ❌ Poor Line Height

```css
.too-tight {
    font-size: 11pt;
    line-height: 11pt;  /* No space between lines! */
}
```

✅ **Solution:** Use 1.5-1.6 for body text

```css
.good-spacing {
    font-size: 11pt;
    line-height: 1.6;  /* ~17.6pt */
}
```

### ❌ Over-using Text Transforms

```css
.overused {
    text-transform: uppercase;  /* Hard to read in long passages */
}
```

✅ **Solution:** Use sparingly for headings

```css
h3 {
    text-transform: uppercase;
    font-size: 12pt;
    letter-spacing: 1pt;
}
```

### ❌ Insufficient Color Contrast

```css
.poor-contrast {
    color: #ccc;  /* Light gray */
    background-color: white;  /* Hard to read */
}
```

✅ **Solution:** Ensure sufficient contrast

```css
.good-contrast {
    color: #333;  /* Dark gray */
    background-color: white;  /* Easy to read */
}
```

---

## Typography Best Practices

1. **Line height:** 1.5-1.6 for body text
2. **Font size:** 10-12pt for body text
3. **Line length:** 50-75 characters per line
4. **Alignment:** Left-aligned or justified for paragraphs
5. **Letter spacing:** Subtle adjustments only
6. **Text transform:** Use sparingly, mainly for headings
7. **Color contrast:** Minimum 4.5:1 ratio

---

## Next Steps

Now that you master text styling:

1. **[Display & Visibility](06_display_visibility.md)** - Control element display
2. **[Style Organization](07_style_organization.md)** - Organize your CSS
3. **[Styling Best Practices](08_styling_best_practices.md)** - Professional styling patterns

---

**Continue learning →** [Display & Visibility](06_display_visibility.md)
