---
layout: default
title: lang
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @lang : The Language Attribute

The `lang` attribute specifies the language of an element's content using BCP 47 language tags. It enables proper text rendering, hyphenation, and accessibility in PDF documents, particularly important for multilingual content and internationalization. The attribute can be applied to any HTML element to define language scope.

## Usage

The `lang` attribute defines content language:
- Specifies the natural language of element content
- Uses BCP 47 language tags (e.g., en, fr, es, de, ja, zh)
- Can include regional variants (e.g., en-US, en-GB, fr-CA)
- Applied to `<html>` for document-wide language
- Applied to specific elements for mixed-language content
- Supports data binding for dynamic language selection

```html
<!-- Document-wide language -->
<html lang="en">
    <head>
        <title>Document in English</title>
    </head>
    <body>
        <p>This content is in English.</p>
    </body>
</html>

<!-- Regional variant -->
<html lang="en-US">
    <body>
        <p>American English content with US spelling: color, center.</p>
    </body>
</html>

<!-- Mixed language content -->
<p lang="en">
    The French word for "hello" is <span lang="fr">bonjour</span>.
</p>

<!-- Dynamic language -->
<html lang="{{model.language}}">
    <body>{{model.content}}</body>
</html>
```

---

## Supported Elements

The `lang` attribute can be used on **any HTML element**, including:

### Document Root
- `<html>` - Document-wide language declaration (most common)

### Structural Elements
- `<body>`, `<header>`, `<footer>`, `<main>`, `<section>`, `<article>`, `<aside>`, `<nav>`

### Text Elements
- `<p>`, `<h1>`-`<h6>`, `<span>`, `<div>`, `<blockquote>`, `<q>`

### Inline Elements
- `<em>`, `<strong>`, `<cite>`, `<code>`, `<abbr>`, `<time>`

### Lists and Tables
- `<ul>`, `<ol>`, `<li>`, `<table>`, `<tr>`, `<td>`, `<th>`

---

## Binding Values

The `lang` attribute supports data binding for dynamic language selection:

```html
<!-- Dynamic document language -->
<html lang="{{model.userLanguage}}">
    <head>
        <meta charset="UTF-8" />
        <title>{{model.title}}</title>
    </head>
    <body>
        {{model.content}}
    </body>
</html>

<!-- Dynamic section language -->
<section lang="{{model.sectionLanguage}}">
    <h2>{{model.heading}}</h2>
    <p>{{model.text}}</p>
</section>

<!-- Conditional language selection -->
<html lang="{{model.region === 'US' ? 'en-US' : 'en-GB'}}">
    <body>{{model.content}}</body>
</html>

<!-- Multilingual content with binding -->
<template data-bind="{{model.translations}}">
    <div lang="{{.languageCode}}">
        <h3>{{.title}}</h3>
        <p>{{.content}}</p>
    </div>
</template>

<!-- Language-specific quotes -->
<template data-bind="{{model.quotes}}">
    <blockquote lang="{{.language}}">
        <p>{{.text}}</p>
        <footer>— {{.author}}</footer>
    </blockquote>
</template>
```

**Data Model Example:**
```json
{
  "userLanguage": "en-US",
  "title": "Document Title",
  "content": "Document content",
  "sectionLanguage": "fr",
  "heading": "Section Heading",
  "text": "Section text",
  "region": "US",
  "translations": [
    {
      "languageCode": "en",
      "title": "Welcome",
      "content": "Welcome to our service."
    },
    {
      "languageCode": "es",
      "title": "Bienvenido",
      "content": "Bienvenido a nuestro servicio."
    }
  ],
  "quotes": [
    {
      "language": "fr",
      "text": "La vie est belle.",
      "author": "French Proverb"
    }
  ]
}
```

---

## Notes

### BCP 47 Language Tags

The `lang` attribute uses **BCP 47** (Best Current Practice 47) language tags:

```html
<!-- Primary language codes (ISO 639-1) -->
<html lang="en">English</html>
<html lang="es">Spanish</html>
<html lang="fr">French</html>
<html lang="de">German</html>
<html lang="it">Italian</html>
<html lang="pt">Portuguese</html>
<html lang="ru">Russian</html>
<html lang="zh">Chinese</html>
<html lang="ja">Japanese</html>
<html lang="ko">Korean</html>
<html lang="ar">Arabic</html>

<!-- With regional subtags -->
<html lang="en-US">American English</html>
<html lang="en-GB">British English</html>
<html lang="fr-FR">French (France)</html>
<html lang="fr-CA">French (Canada)</html>
<html lang="es-ES">Spanish (Spain)</html>
<html lang="es-MX">Spanish (Mexico)</html>
<html lang="pt-BR">Portuguese (Brazil)</html>
<html lang="pt-PT">Portuguese (Portugal)</html>
<html lang="zh-CN">Chinese (Simplified)</html>
<html lang="zh-TW">Chinese (Traditional)</html>
```

### Language vs Script

Language tags can include script subtags:

```html
<!-- Chinese with script specification -->
<html lang="zh-Hans">Chinese (Simplified script)</html>
<html lang="zh-Hant">Chinese (Traditional script)</html>

<!-- Serbian in different scripts -->
<html lang="sr-Latn">Serbian (Latin script)</html>
<html lang="sr-Cyrl">Serbian (Cyrillic script)</html>

<!-- Uzbek in different scripts -->
<html lang="uz-Latn">Uzbek (Latin script)</html>
<html lang="uz-Cyrl">Uzbek (Cyrillic script)</html>
```

### Document-Wide Language

Set `lang` on the `<html>` element for the entire document:

```html
<!-- English document -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>English Document</title>
</head>
<body>
    <p>All content inherits English language.</p>
</body>
</html>

<!-- Spanish document -->
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8" />
    <title>Documento en Español</title>
</head>
<body>
    <p>Todo el contenido hereda el idioma español.</p>
</body>
</html>
```

### Mixed Language Content

Override document language for specific sections:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Multilingual Document</title>
</head>
<body>
    <h1>Welcome</h1>
    <p>This page contains content in multiple languages.</p>

    <section lang="es">
        <h2>Español</h2>
        <p>Este es un párrafo en español.</p>
    </section>

    <section lang="fr">
        <h2>Français</h2>
        <p>Ceci est un paragraphe en français.</p>
    </section>

    <section lang="de">
        <h2>Deutsch</h2>
        <p>Dies ist ein Absatz auf Deutsch.</p>
    </section>
</body>
</html>
```

### Inline Language Changes

Mark inline language changes:

```html
<p lang="en">
    The German word for "thank you" is <span lang="de">danke</span>.
    In French, it's <span lang="fr">merci</span>.
    In Spanish, it's <span lang="es">gracias</span>.
</p>

<p lang="en">
    The restaurant's signature dish is <span lang="it">risotto ai funghi</span>,
    which means mushroom risotto.
</p>

<p lang="fr">
    Le mot anglais <span lang="en">"computer"</span> se traduit par
    <span lang="fr">ordinateur</span> en français.
</p>
```

### Language and Hyphenation

Language affects text hyphenation in PDF:

```html
<style>
    p {
        text-align: justify;
        hyphens: auto;  /* Enable automatic hyphenation */
    }
</style>

<!-- English hyphenation rules -->
<p lang="en">
    Supercalifragilisticexpialidocious is an extraordinarily long word
    that demonstrates English hyphenation.
</p>

<!-- German hyphenation rules -->
<p lang="de">
    Donaudampfschifffahrtsgesellschaftskapitän ist ein sehr langes
    deutsches Wort.
</p>
```

### Language and Text Direction

While `lang` doesn't set text direction, it's often used with `dir`:

```html
<!-- Arabic (right-to-left) -->
<html lang="ar" dir="rtl">
    <body>
        <p>مرحبا بكم في موقعنا</p>
    </body>
</html>

<!-- Hebrew (right-to-left) -->
<html lang="he" dir="rtl">
    <body>
        <p>שלום</p>
    </body>
</html>

<!-- English with Arabic quote -->
<p lang="en" dir="ltr">
    The Arabic greeting is <span lang="ar" dir="rtl">مرحبا</span>.
</p>
```

### Accessibility Benefits

The `lang` attribute helps screen readers:
- Pronounce words correctly
- Apply proper language-specific rules
- Switch between languages seamlessly

```html
<p lang="en">
    Screen readers will pronounce this in English.
    <span lang="ja">これは日本語です</span>
    This returns to English.
</p>
```

### Case Sensitivity

Language codes are case-insensitive, but conventions exist:

```html
<!-- All valid but follow conventions -->
<html lang="en">Standard lowercase</html>
<html lang="EN">Uppercase (avoid)</html>
<html lang="En">Mixed case (avoid)</html>

<!-- Convention: lowercase primary, UPPERCASE region -->
<html lang="en-US">Conventional</html>
<html lang="en-us">Works but non-standard</html>
```

**Best Practice:**
- Primary language: lowercase (en, fr, es)
- Region subtag: uppercase (US, GB, FR)
- Script subtag: title case (Hans, Latn, Cyrl)

### Empty or Invalid Lang

```html
<!-- Valid document -->
<html lang="en">Valid language code</html>

<!-- Empty lang (not recommended) -->
<html lang="">Unknown language</html>

<!-- Missing lang (acceptable but not ideal) -->
<html>No language specified</html>

<!-- Invalid code (not recommended) -->
<html lang="english">Use ISO codes, not names</html>
```

### Language Inheritance

Child elements inherit parent language unless overridden:

```html
<html lang="en">
    <body>
        <!-- Inherits en -->
        <p>English paragraph</p>

        <!-- Overrides to fr -->
        <section lang="fr">
            <p>French paragraph (inherits fr from section)</p>

            <!-- Overrides back to en -->
            <p lang="en">English paragraph within French section</p>
        </section>

        <!-- Returns to en from html -->
        <p>English paragraph again</p>
    </body>
</html>
```

### Historical Languages

BCP 47 supports historical and extinct languages:

```html
<!-- Latin -->
<blockquote lang="la">
    <p>Lorem ipsum dolor sit amet.</p>
</blockquote>

<!-- Old English -->
<p lang="ang">Hwæt! We Gardena in geardagum</p>

<!-- Ancient Greek -->
<p lang="grc">Ἐν ἀρχῇ ἦν ὁ λόγος</p>

<!-- Sanskrit -->
<p lang="sa">ॐ</p>
```

---

## Examples

### Basic Document Language

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>English Document</title>
</head>
<body>
    <h1>Welcome</h1>
    <p>This entire document is in English.</p>
</body>
</html>
```

### Regional Language Variants

```html
<!DOCTYPE html>
<html lang="en-US">
<head>
    <meta charset="UTF-8" />
    <title>American English Document</title>
</head>
<body>
    <h1>Welcome to Our Service</h1>

    <p>
        We prioritize quality and customer satisfaction.
        Our color selection and customization options are
        designed with your needs in center focus.
    </p>

    <p>
        <em>Note: This document uses American English spelling.</em>
    </p>
</body>
</html>
```

### Multilingual Welcome Page

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Welcome - Multilingual</title>
    <style>
        .language-section {
            margin: 20pt 0;
            padding: 15pt;
            border: 1pt solid #ccc;
            border-radius: 5pt;
        }
        .language-label {
            font-weight: bold;
            color: #336699;
            margin-bottom: 10pt;
        }
    </style>
</head>
<body>
    <h1 lang="en">Welcome to Our International Service</h1>

    <div class="language-section" lang="en">
        <div class="language-label">English</div>
        <p>Welcome! We're delighted to have you here.</p>
    </div>

    <div class="language-section" lang="es">
        <div class="language-label">Español</div>
        <p>¡Bienvenido! Estamos encantados de tenerle aquí.</p>
    </div>

    <div class="language-section" lang="fr">
        <div class="language-label">Français</div>
        <p>Bienvenue! Nous sommes ravis de vous accueillir.</p>
    </div>

    <div class="language-section" lang="de">
        <div class="language-label">Deutsch</div>
        <p>Willkommen! Wir freuen uns, Sie hier zu haben.</p>
    </div>

    <div class="language-section" lang="it">
        <div class="language-label">Italiano</div>
        <p>Benvenuto! Siamo felici di averti qui.</p>
    </div>

    <div class="language-section" lang="pt">
        <div class="language-label">Português</div>
        <p>Bem-vindo! Estamos felizes em tê-lo aqui.</p>
    </div>

    <div class="language-section" lang="ja">
        <div class="language-label">日本語</div>
        <p>ようこそ！お越しいただきありがとうございます。</p>
    </div>

    <div class="language-section" lang="zh">
        <div class="language-label">中文</div>
        <p>欢迎！我们很高兴您的到来。</p>
    </div>

    <div class="language-section" lang="ko">
        <div class="language-label">한국어</div>
        <p>환영합니다! 여기에 오신 것을 기쁘게 생각합니다.</p>
    </div>

    <div class="language-section" lang="ar" dir="rtl">
        <div class="language-label">العربية</div>
        <p>مرحبا! نحن سعداء لوجودك هنا.</p>
    </div>
</body>
</html>
```

### Academic Paper with Citations

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Comparative Linguistics Study</title>
</head>
<body>
    <h1>Comparative Analysis of Romance Languages</h1>

    <section>
        <h2>Introduction</h2>
        <p>
            This paper examines common phrases across Romance languages,
            demonstrating their shared Latin origins.
        </p>
    </section>

    <section>
        <h2>Common Greetings</h2>

        <p>The word "hello" varies across Romance languages:</p>

        <ul>
            <li lang="it"><strong>Italian:</strong> Ciao</li>
            <li lang="es"><strong>Spanish:</strong> Hola</li>
            <li lang="fr"><strong>French:</strong> Bonjour</li>
            <li lang="pt"><strong>Portuguese:</strong> Olá</li>
            <li lang="ro"><strong>Romanian:</strong> Bună ziua</li>
        </ul>
    </section>

    <section>
        <h2>Example Sentences</h2>

        <div style="margin: 15pt 0;">
            <p><strong>Latin root phrase:</strong></p>
            <blockquote lang="la">
                <p>Tempus fugit.</p>
                <footer><em>(Time flies.)</em></footer>
            </blockquote>
        </div>

        <div style="margin: 15pt 0;">
            <p><strong>French adaptation:</strong></p>
            <blockquote lang="fr">
                <p>Le temps passe vite.</p>
                <footer><em>(Time passes quickly.)</em></footer>
            </blockquote>
        </div>

        <div style="margin: 15pt 0;">
            <p><strong>Spanish adaptation:</strong></p>
            <blockquote lang="es">
                <p>El tiempo vuela.</p>
                <footer><em>(Time flies.)</em></footer>
            </blockquote>
        </div>
    </section>
</body>
</html>
```

### International Business Document

```html
<!DOCTYPE html>
<html lang="en-US">
<head>
    <meta charset="UTF-8" />
    <title>Global Business Report</title>
</head>
<body>
    <h1>2025 Global Market Analysis</h1>

    <section lang="en">
        <h2>Executive Summary</h2>
        <p>
            Our analysis reveals strong growth in international markets,
            with particular emphasis on emerging economies.
        </p>
    </section>

    <section lang="es">
        <h2>Mercado Latinoamericano</h2>
        <p>
            El mercado latinoamericano ha experimentado un crecimiento
            del 35% en el último trimestre, superando las proyecciones
            iniciales.
        </p>
    </section>

    <section lang="de">
        <h2>Europäischer Markt</h2>
        <p>
            Der europäische Markt zeigt eine stabile Entwicklung mit
            einem Wachstum von 15% im Jahresvergleich.
        </p>
    </section>

    <section lang="ja">
        <h2>アジア太平洋市場</h2>
        <p>
            アジア太平洋地域は、前年比40%の成長を記録し、
            最も急成長している市場となっています。
        </p>
    </section>

    <section lang="en">
        <h2>Conclusion</h2>
        <p>
            Global expansion continues to drive growth across all regions.
        </p>
    </section>
</body>
</html>
```

### Restaurant Menu

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Restaurant Menu</title>
    <style>
        .menu-item {
            margin: 15pt 0;
            padding: 10pt;
            border-bottom: 1pt solid #eee;
        }
        .dish-name {
            font-weight: bold;
            font-size: 12pt;
        }
        .price {
            float: right;
            color: #336699;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <h1>La Bella Vista Restaurant</h1>
    <p lang="it" style="font-style: italic; text-align: center;">
        Cucina Italiana Autentica
    </p>

    <h2>Appetizers</h2>

    <div class="menu-item">
        <span class="price">$12.00</span>
        <div class="dish-name" lang="it">Bruschetta al Pomodoro</div>
        <p>Toasted bread with fresh tomatoes, basil, and olive oil</p>
    </div>

    <div class="menu-item">
        <span class="price">$14.00</span>
        <div class="dish-name" lang="it">Carpaccio di Manzo</div>
        <p>Thinly sliced raw beef with arugula and parmesan</p>
    </div>

    <h2>Main Courses</h2>

    <div class="menu-item">
        <span class="price">$24.00</span>
        <div class="dish-name" lang="it">Risotto ai Funghi</div>
        <p>Creamy mushroom risotto with porcini and white truffle oil</p>
    </div>

    <div class="menu-item">
        <span class="price">$28.00</span>
        <div class="dish-name" lang="it">Osso Buco alla Milanese</div>
        <p>Braised veal shanks served with saffron risotto</p>
    </div>

    <div class="menu-item">
        <span class="price">$26.00</span>
        <div class="dish-name" lang="it">Spaghetti alle Vongole</div>
        <p>Spaghetti with fresh clams, garlic, and white wine</p>
    </div>

    <h2>Desserts</h2>

    <div class="menu-item">
        <span class="price">$8.00</span>
        <div class="dish-name" lang="it">Tiramisù</div>
        <p>Classic Italian dessert with espresso and mascarpone</p>
    </div>

    <div class="menu-item">
        <span class="price">$9.00</span>
        <div class="dish-name" lang="it">Panna Cotta</div>
        <p>Silky vanilla cream with berry compote</p>
    </div>

    <footer style="margin-top: 30pt; text-align: center; font-style: italic;">
        <p lang="it">Buon appetito!</p>
    </footer>
</body>
</html>
```

### Data-Bound Multilingual Content

```html
<!-- Model: {
    language: "es",
    translations: {
        en: {
            title: "Welcome",
            message: "Thank you for visiting our site.",
            button: "Learn More"
        },
        es: {
            title: "Bienvenido",
            message: "Gracias por visitar nuestro sitio.",
            button: "Aprende Más"
        },
        fr: {
            title: "Bienvenue",
            message: "Merci de visiter notre site.",
            button: "En Savoir Plus"
        }
    },
    content: {
        title: "Bienvenido",
        message: "Gracias por visitar nuestro sitio.",
        button: "Aprende Más"
    }
} -->

<!DOCTYPE html>
<html lang="{{model.language}}">
<head>
    <meta charset="UTF-8" />
    <title>{{model.content.title}}</title>
</head>
<body>
    <h1>{{model.content.title}}</h1>
    <p>{{model.content.message}}</p>
    <button>{{model.content.button}}</button>
</body>
</html>
```

### Product Documentation

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Product Manual</title>
</head>
<body>
    <h1>User Manual - Model X1000</h1>

    <section>
        <h2>Safety Information</h2>
        <p>
            Read all safety instructions before operating this device.
        </p>

        <div style="margin: 15pt 0; padding: 10pt; background-color: #fff3cd;">
            <p><strong lang="en">Warning (English):</strong>
            Do not expose to water.</p>

            <p lang="es"><strong>Advertencia (Español):</strong>
            No exponer al agua.</p>

            <p lang="fr"><strong>Avertissement (Français):</strong>
            Ne pas exposer à l'eau.</p>

            <p lang="de"><strong>Warnung (Deutsch):</strong>
            Nicht dem Wasser aussetzen.</p>

            <p lang="ja"><strong>警告 (日本語):</strong>
            水にさらさないでください。</p>
        </div>
    </section>

    <section>
        <h2>Technical Specifications</h2>
        <table style="width: 100%; border-collapse: collapse;">
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">Model</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">X1000</td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">Power</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">100-240V AC</td>
            </tr>
        </table>
    </section>
</body>
</html>
```

### Literary Quotations

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Famous Quotations</title>
    <style>
        blockquote {
            margin: 20pt 0;
            padding: 15pt;
            border-left: 4pt solid #336699;
            background-color: #f8f9fa;
        }
    </style>
</head>
<body>
    <h1>Literary Quotations from Around the World</h1>

    <article>
        <h2>French Literature</h2>
        <blockquote lang="fr">
            <p>« La vie est un sommeil, l'amour en est le rêve. »</p>
            <footer>— Alfred de Musset</footer>
        </blockquote>
        <p><em lang="en">Translation: "Life is a sleep, love is its dream."</em></p>
    </article>

    <article>
        <h2>Spanish Literature</h2>
        <blockquote lang="es">
            <p>« En un lugar de la Mancha, de cuyo nombre no quiero acordarme... »</p>
            <footer>— Miguel de Cervantes, Don Quijote</footer>
        </blockquote>
        <p><em lang="en">Translation: "In a place in La Mancha, whose name I do not wish to remember..."</em></p>
    </article>

    <article>
        <h2>German Literature</h2>
        <blockquote lang="de">
            <p>« Wer nie sein Brot mit Tränen aß, wer nie die kummervollen Nächte auf seinem Bette weinend saß, der kennt euch nicht, ihr himmlischen Mächte. »</p>
            <footer>— Johann Wolfgang von Goethe</footer>
        </blockquote>
    </article>

    <article>
        <h2>Japanese Literature</h2>
        <blockquote lang="ja">
            <p>「人間は生まれながらにして孤独である。」</p>
            <footer>— 夏目漱石</footer>
        </blockquote>
        <p><em lang="en">Translation: "Human beings are lonely from birth."</em></p>
    </article>

    <article>
        <h2>Russian Literature</h2>
        <blockquote lang="ru">
            <p>« Все счастливые семьи похожи друг на друга, каждая несчастливая семья несчастлива по-своему. »</p>
            <footer>— Лев Толстой, Анна Каренина</footer>
        </blockquote>
        <p><em lang="en">Translation: "All happy families are alike; each unhappy family is unhappy in its own way."</em></p>
    </article>
</body>
</html>
```

### Travel Guide

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Travel Guide: Essential Phrases</title>
</head>
<body>
    <h1>Essential Travel Phrases</h1>

    <section>
        <h2>Greetings and Basic Phrases</h2>

        <table style="width: 100%; border-collapse: collapse;">
            <thead>
                <tr style="background-color: #f2f2f2;">
                    <th style="border: 1pt solid #ddd; padding: 8pt;">English</th>
                    <th style="border: 1pt solid #ddd; padding: 8pt;">Spanish</th>
                    <th style="border: 1pt solid #ddd; padding: 8pt;">French</th>
                    <th style="border: 1pt solid #ddd; padding: 8pt;">Italian</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="en">Hello</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="es">Hola</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="fr">Bonjour</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="it">Ciao</td>
                </tr>
                <tr>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="en">Thank you</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="es">Gracias</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="fr">Merci</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="it">Grazie</td>
                </tr>
                <tr>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="en">Goodbye</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="es">Adiós</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="fr">Au revoir</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="it">Arrivederci</td>
                </tr>
                <tr>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="en">Please</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="es">Por favor</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="fr">S'il vous plaît</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="it">Per favore</td>
                </tr>
                <tr>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="en">Excuse me</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="es">Disculpe</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="fr">Excusez-moi</td>
                    <td style="border: 1pt solid #ddd; padding: 8pt;" lang="it">Mi scusi</td>
                </tr>
            </tbody>
        </table>
    </section>
</body>
</html>
```

### Language Learning Material

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>French Lesson 1: Introductions</title>
    <style>
        .example {
            margin: 15pt 0;
            padding: 10pt;
            background-color: #f0f8ff;
            border-left: 3pt solid #336699;
        }
        .translation {
            margin-top: 5pt;
            font-style: italic;
            color: #666;
        }
    </style>
</head>
<body>
    <h1>French Lesson 1: Introductions</h1>

    <section>
        <h2>Basic Greetings</h2>

        <div class="example">
            <p lang="fr"><strong>Bonjour!</strong> Comment allez-vous?</p>
            <p class="translation" lang="en">Hello! How are you?</p>
        </div>

        <div class="example">
            <p lang="fr"><strong>Je m'appelle Marie.</strong> Et vous?</p>
            <p class="translation" lang="en">My name is Marie. And you?</p>
        </div>

        <div class="example">
            <p lang="fr"><strong>Enchanté(e)!</strong></p>
            <p class="translation" lang="en">Pleased to meet you!</p>
        </div>
    </section>

    <section>
        <h2>Common Phrases</h2>

        <div class="example">
            <p lang="fr">Parlez-vous anglais?</p>
            <p class="translation" lang="en">Do you speak English?</p>
        </div>

        <div class="example">
            <p lang="fr">Je ne comprends pas.</p>
            <p class="translation" lang="en">I don't understand.</p>
        </div>

        <div class="example">
            <p lang="fr">Pouvez-vous répéter, s'il vous plaît?</p>
            <p class="translation" lang="en">Can you repeat, please?</p>
        </div>
    </section>
</body>
</html>
```

### Corporate Multi-Region Document

```html
<!-- Model: {
    regions: [
        { code: "en-US", name: "United States", message: "Welcome to our US office." },
        { code: "en-GB", name: "United Kingdom", message: "Welcome to our UK office." },
        { code: "es-MX", name: "Mexico", message: "Bienvenido a nuestra oficina en México." },
        { code: "fr-CA", name: "Canada", message: "Bienvenue à notre bureau canadien." }
    ]
} -->

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Global Offices</title>
</head>
<body>
    <h1>Our Global Presence</h1>

    <template data-bind="{{model.regions}}">
        <section lang="{{.code}}" style="margin: 20pt 0; padding: 15pt; border: 1pt solid #ccc;">
            <h2>{{.name}}</h2>
            <p>{{.message}}</p>
        </section>
    </template>
</body>
</html>
```

---

## See Also

- [charset](/reference/htmlattributes/charset.html) - Character encoding attribute
- [dir](/reference/htmlattributes/dir.html) - Text direction attribute (ltr, rtl)
- [translate](/reference/htmlattributes/translate.html) - Translation hint attribute
- [html](/reference/htmltags/html.html) - Root HTML element
- [Internationalization](/reference/i18n/) - Creating multilingual PDFs
- [BCP 47](/reference/standards/bcp47.html) - Language tag specification

---
