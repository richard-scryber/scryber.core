The Scryber document engine is a great solution for building PDF documents for your dotnet core applications. With full support for styles, binding to data sources and adaptive layout. It's fully native c# and easy to use as a coded, or declarative document description.

## Getting Started

The easiest way to start is to use the NuGet packages available here. 

* Console or UI Applications : [https://www.nuget.org/packages/Scryber.Core/](https://www.nuget.org/packages/Scryber.Core/)
* Dot Net MVC Applications : [https://www.nuget.org/packages/Scryber.Core.Mvc/](https://www.nuget.org/packages/Scryber.Core.Mvc/)

_(The MVC package simply adds a couple of controller extensions, to make it easy to generate your PDF to an action result, along with the latest core package)_

Or you can download the source from this repository and reference the libraries.

## Namespaces

A couple of namespaces are important to add to the top of your class file or controller.

```csharp
using Scryber.Components;
using Scryber.Components.Mvc; //for MVC applications.
```

## Create A Document

AKA - Hello World in C#

```csharp
//create a document structure
var doc = new PDFDocument();

PDFPage pg = new PDFPage();
doc.Pages.Add(pg);

PDFLabel label = new PDFLabel();
label.Text = "Hello World";
pg.Contents.Add(label);
```

It's very simple, that can be added to and built up.

## Output the PDF

To create your pdf simply take the document and generate the pdf.

From your console or UI application

```csharp
doc.ProcessDocument([pathOrStream]);
```

or using the MVC controller extension methods that return an IActionResult

```csharp
return this.PDF(doc);
```

## Creating a template

One of the real advantages of scryber is the use of XML templates to describe your documents, pages, styles, or components.

Create a new file in your code `HelloWorld.pdfx` and paste the contents below _(the pdfx file extension is a convention we use to describe our document types)_

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<pdf:Document xmlns:pdf="https://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
              xmlns:styles="https://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd">
    <Pages>
        <pdf:Page>
            <Content>
                <pdf:Label>Hello World, from scryber.</pdf:Label>
            </Content>
       </pdf:Page>
    </Pages>
</pdf:Document>
```

And then you can load the pdf from the template, or generate directly.


### Running from Mac

If you are running the application from Visual Studio Mac (as we are), then you may need to follow the instructions here [Libgdiplus installation on Mac](Libgdiplus-For-Mac)