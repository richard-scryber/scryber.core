
# Scryber.Core PDF Engine

## Scryber makes creating beautiful, dynamic documents easy.

The scryber engine is an advanced, complete, pdf creation library for dotnet (including support for Blazor WASM)

- Easy definition of document templates with, pages, content, shapes and images using html, and svg.
- Bring your data in from an api or object model.
- Bind with handlebars syntax and full complex expression support.
- Layout with styles including calculated and relative values, and binding to your data.
- Finally with a couple of lines of code, output the document to a stream or file.

With a styles based template layout, it is easy to create good looking, paginated and flowing documents, with dynamic content from your applications or sites.


## Getting Started

The easiest way to begin is to use the Nuget Packages here

[scryber.core package](https://www.nuget.org/packages/scryber.core/)
(Base libraries for GUI or console applications)

OR for asp.net mvc

[scryber.core.mvc package](https://www.nuget.org/packages/scryber.core.mvc/)
(Which includes the scryber.core package).

Check out the documentation for more information on how to use the library.

- [getting started](https://paperwork.help/quickstart/quickstart_core.html), a quick start article that gives a good overview of getting started and producing your first styles and bound template.

- [scryber.core learning guides](https://www.paperwork.help/learning/), a full series for learning the main capabilities of the library.

- [scryber.core reference guides](https://www.paperwork.help/reference/), a complete core library reference of all the supported elements, style and binding expressions.

---

## Using Scryber

### Content template

Use standard (x)html content to define the template with handlebar `{{ }}` notation for dynamic values.

```html
<html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <!-- support for standard document attributes -->
    <meta charset='utf-8' name='author' content='{{meta.author}}' />
    <title>Hello World</title>
    <!-- add your styles -->
    <style> 
    </style>
    <!-- and reference remote style sheets -->
    <link href="/relative-stylesheet.css" type="text/css" />
</head>
<body class="name" style="border: solid 1pt #AAA">
<!-- full binding support on content, styles and dynamic expressions -->
<h1 style="font-size: calc(theme.titleSize);">Hello {{name ?? 'World'}}</h1>
</body>
</html>

```
https://paperwork.help/learning/01-getting-started/03_html_to_pdf.html

---

### Loading the DOM
The document is parsed into a complete object graph from local file paths, streams or dynamically created content.
```csharp
using Scryber.Components
    
    using var doc1 = Document.ParseDocument("xhtml-template-path.html");
    using var doc2 = Document.ParseDocument(myContentStream);
    using var doc3 = Document.ParseHtmlDocument("non-formal-html-template-path.html");
    using var doc4 = Document.ParseDocument(new StringReader("<html xmlns='http://www.w3.org/1999/xhtml' >....</html>"))
```
https://paperwork.help/learning/01-getting-started/02_first_document.html

---

### Adding data for binding
Binding values are added using the documents' Params dictionary and can be set to any value, object graph, or json data.
```csharp
    //simple dynamic object
    doc.Params["meta"] = new { author = "My Name", reportDate = DateTime.Now };
    
    //structured object data
    doc.Params["model"] = GetMyModelData();
    
    //json data
    doc.Params["json"] = System.Text.Json.JsonSerializer.Deserialize(remoteAPIResult);
    
    //injected templated content
    doc.Params["templates"] = new 
    {
        innerContent = "<span class='header'>{{.title}}</span><br/><span class='desc'>{{.desc}}</span>"
    };
```

https://paperwork.help/learning/02-data-binding/

---

### Outputting the result

The final output can be saved either to a file or stream, synchronously or asynchronously.

```csharp
//Simply save to a file
doc.SaveAsPDF("filePath.pdf");
            
//A standard IO stream
using var memory = new System.IO.MemoryStream();
doc.SaveAsPDF(memory);

//Or your own custom Stream
using var sourceStream = GetDataStream();
doc.SaveAsPDF(sourceStream);

//Also supports asyncromous operation
await doc.SaveAsPDFAsync("asyncfilepath.pdf");
   
```
https://paperwork.help/learning/01-getting-started/07_output_options.html

Using the Scryber.Components.MVC package extensions

```csharp
public async IActionResult GetDocument()
{
    .
    .
    return await this.PDFAsync(doc)
}
```

---

## Other features

### Binding to complex content
Handlebar support for dynamic content binding, and expressions
```html
<!-- looping support -->
<div id="contentWrapper" class="wrapper">
{{#each model.items}}
    <div>Item {{@index}}: {{.name}}
    <!-- conditional output and function support -->
    {{#if count(.nested)}}
        <!-- nested content and complex content creation with expressions -->
        <ul>{{#each .nested}}<li data-content="{{templates.innerContent}}"></li></ul>
    {{else}}<div class="muted">No inner items</div>
    {{/if}}
    </div>
{{/each}}
</div>
```

Add standard external templates using the embed element
```html
<div class="tsCs" >
    <embed src="./includes/termsAndConditions.html" />
</div>
```

Or create and inject compoments directly into the DOM
```csharp
var table = this.BuildTableGrid(loadedData);
var container = doc.FindAComponentById("contentWrapper") as Div;
container.Contents.Add(table);
```
---

### Page Sizes, headers and footers

The default page size is A4, but this can be changed to US specific letter or any standard size (or explicit).
Inner pages can also have their own sizing.

```html
<html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style> 
        
        @page{
            size: letter;
        }
        
        @page landscape{
            size: letter landscape;
        }
        
        .wide{
            page: landscape;
        }
        
    </style>
</head>
<body class="col2 landscape" style="border: solid 1pt #AAA">
    <h1>Main page in letter sizing</h1>
    <section class="wide">
        Content is by default on a new page with a 'section', and this will use the 'letter landscape' size based on the 'wide' class
    </section>
</body>
</html>
```

---

### Column layout.

The engine fully supports columnar layout both at the page level and also within blocks.

```html
<html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style> 
        @page landscape{
            size: A4 landscape;
        }
        
        @media print {
            /* force a landscape page with 2 column layout on the body *only* when printing */
            body {
                page: landscape;
            }
            
            .col2{
                column-count: 2;
                column-rule: 1pt solid silver;
            }
             
        }
    </style>
</head>
<body class="col2 landscape" style="border: solid 1pt #AAA">
    <h1>Main page flow in 2 columns</h1>
    <article class="col2">
        Content will flow in the inner columns, then onto the next side of the page
    </article>
</body>
</html>
```

Columns can also be used in footers, and a new column forced with break-before.

```html
<footer style="column-count: 3">
    <time datetime="{{meta.reportDate}}" data-format="dd MMM yyyy" />
    <page data-format="Page {0} of {1}" style="break-before: column; text-align: center" />
    <span style="break-before: column; text-align: right">{{meta.author}}</span>
</footer>
```
---

### Graphic Support
Images are supported for png, jpeg, tiff, gif, svg and webP (v9.5+)

```html
<img src="https://fullremoteurl.com/images/myimage.png" />
<img src="./relativeUrlFromTemplateBasePath/OrWorkingDirectory/myImage.jpg" />
<img src="./mySvgImage.svg" />
<img src="data:image/png;base64,iVBORw0KGgoAAAANS..." />
```
https://paperwork.help/learning/06-content/01_images.html

Images can also be used as backgrounds and patterns
```css
.patterned{
    background-repeat: repeat-x;
    background-image: url("./localpath/diamond.png)";
}
```
https://paperwork.help/reference/cssproperties/properties/css_prop_background-image.html

SVG's can be included inline within the document or using the embed element, that allows the content to use dynamic binding.
```html
<div class="logo" >
    <svg xmlns='http://www.w3.org/2000/svg' width='100%' height='100pt' preserveAspectRatio="xMidYMid">
        <rect x='10' y='10' width='{{length(meta.title) * 20' height='10' fill="#AAA" />
        <text fill="#AA0" text-anchor="middle">{{meta.title}}</text>
    </svg>
</div>
```
https://paperwork.help/learning/06-content/02_svg_basics.html

---
### Font Support

The library includes 16 standard fonts that can be embedded. 
THe system fonts, and custom fonts can be configured and loaded at initial execution time

https://paperwork.help/configuration/font-configuration.html

Other fonts can also be defined through links or the @font-face rule

```html
<!-- link to google (watch the XML tag for a link) -->
<link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&amp;display=swap" rel="stylesheet"/>
<!-- or download and reference locally -->
<style>
    @font-face {
        font-family: 'Roboto';
        src: url('../fonts/Roboto-Regular.ttf') format('truetype');
        font-weight: 400;
        font-style: normal;
    }
    
    body{
        font-family: 'Roboto', sans-serif;
    }
</style>
```
https://paperwork.help/learning/05-typography/03_web_fonts.html

---

### Modifying existing files

Scryber supports modification of existing pdfs using framesets for page insertion, overlays, or contractions.

```html
<frameset xmlns='http://www.w3.org/1999/xhtml'>
    <!-- Add summary page before the referernced pdf -->
    <frame src="summary.html"></frame>
    <!-- Use only pages 1-3 from source PDF (0-indexed) -->
    <frame src="large-document.pdf" data-page-start="0" data-page-count="3">
        <!-- add an overloay to each page if we are outputting a proof document -->
        <h1 hidden="{{model.isProof ? '' : 'hidden'}}" 
            style="transform: rotate(45deg); opacity: 0.5; color: #444;">PROOF CONTENT</h1>
    </frame>
</frameset>
```
https://www.paperwork.help/reference/htmltags/elements/html_frameset_frame_element.html

---

### Adding security
Add restrictions and encryption mechanism to the document either through a template meta tag, or the documents RenderOptions.Permissions property.

```html
<head>
    <meta name="print-restrictions" content="printing, accessibility" />
    <meta name="print-encryption" content="128bit"
</head>
```

```csharp
doc.RenderOptions.Permissions.AllowCopying = true;
doc.RenderOptions.Permissions.AllowPrinting = true;
doc.RenderOptions.Permissions.AllowAccessiblity = true;
```

When using restrictions it is advisable that an owner password be set on the document output (in lax mode, a random value will be assigned)

```csharp
using SecureString owner = CollectOwnerPassword();
doc.RenderOptions.PasswordProvider = new DocumentPasswordProvider(owner);
```
To force a password to be required to open a document, then set a user password (this can be the same as the owner password).

```csharp
using SecureString owner = CollectOwnerPassword();
using SecureString user = CollectUserPassword();

doc.RenderOptions.PasswordProvider = new DocumentPasswordProvider(owner);
doc.RenderOptions.PasswordProvider = new DocumentPasswordProvider(owner, user);
```
https://paperwork.help/learning/07-configuration/05_security.html

---

### Checking the output

To check the results, and what is happening underneath use the trace log support - either with the scryber/paperwork processing instructions at the very top of the template. 
This will add the processing log to the end of the resultant document, along with execution timing and any resources (fonts, images, etc) included.

```xml
<?scryber append-log=true log-level=messages ?>
```
or use code on the document itself.

```csharp
doc.AppendTraceLog = true;
doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
```

https://paperwork.help/configuration/processing-instructions.html

---

## The engine also supports:

- standard [html body, tables, lists, divs and spans](https://www.paperwork.help/reference/htmltags/) and many newer html5 [tags](https://www.paperwork.help/reference/htmltags/)
- cascading styles: linked, embedded or inline using [css syntax and priority](https://www.paperwork.help/learning/03-styling/).
- [attaching](https://www.paperwork.help/reference/htmltags/elements/html_object_element.html) of external files or streams.
- [sizing](https://www.paperwork.help/learning/03-styling/04_units_measurements.html) and [positioning](https://papwerwok.help/learning/04-layout/) of elements inline, block, relative or absolute.
- support for [relative units](https://paperwork.help/learning/03-styling/04_units_measurements.html) and [transformations](https://paperwork.help/reference/cssproperties/properties/css_prop_transform.html).
- [gradients](https://paperwork.help/reference/cssproperties/properties/css_prop_background-image.html) and colours with text and shape [fills, backgrounds](https://www.paperwork.help/reference/cssproperties/) and borders.
- [multiple fonts](https://www.paperwork.help/learning/05-typography/), including google fonts, supporting text [alignment; spacing; leading; decoration](https://www.paperwork.help/learning/05-typography/05_text_spacing.html).
- Graphical binding support with SVG for [drawings and charts](https://paperwork.help/learning/06-content/03_svg_drawing.html) and text.
- Full code support either as a whole document, or partial content, and [controllers](https://paperwork.help/configuration/document-controllers.html).
- [Custom Component](https://paperwork.help/configuration/custom-components.html) support for defining your own layouts or parts.

---

## Getting Involved

We would love to hear your feedback. Feel free to get in touch.
Issues, ideas, includes are all welcome.

If you would like to help with building, extending then happy to get contributions

