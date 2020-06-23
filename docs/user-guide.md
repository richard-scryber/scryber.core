# Getting Started

The Scryber document engine is a great solution for building PDF documents for your dotnet core applications and MVC sites. With full support for styles, binding to data sources and adaptive layout. It's fully native c# and easy to use as a coded, or declarative document description.

## Nuget Packages

The easiest way to start is to use the NuGet packages available here. 

* For Console or UI Applications : [https://www.nuget.org/packages/Scryber.Core/](https://www.nuget.org/packages/Scryber.Core/)
* For MVC Applications : [https://www.nuget.org/packages/Scryber.Core.Mvc/](https://www.nuget.org/packages/Scryber.Core.Mvc/)

_(The MVC package simply adds a couple of controller extensions, to make it easy to generate your PDF to an action result, along with the latest core package)_

Or you can download the source from this repository and reference the libraries.

## Namespaces

A couple of namespaces are important to add to the top of your class file or controller.

```csharp
using Scryber.Components;
using Scryber.Components.Mvc; //for MVC applications.
```

## Create A Document

Hello World in C#

```csharp
//create a document
var doc = new PDFDocument();

//add a page to it
PDFPage pg = new PDFPage();
doc.Pages.Add(pg);

//and add a label with some text to it
PDFLabel label = new PDFLabel();
label.Text = "Hello World";
pg.Contents.Add(label);
```

It's a very simple document, that can be added to and built up.

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

The real advantage of scryber is the use of XML templates to describe your documents, pages, styles, or components.

Create a new folder in your project called `PDFs`, and then add new file called `HelloWorld.pdfx` and paste the contents below _(the pdfx file extension is a convention we use to describe our document types)_

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
And then you can load the pdf from the template.

```csharp

//Parse the XML and process
var path = "[ContentRootPath]"; //Get the root path
path = System.IO.Path.Combine(path, "PDFs", "HelloWorld.pdfx");
var doc = PDFDocument.ParseDocument(path);

```

Or generate directly as an MVC action result

```csharp

//Parse the XML and process
var path = "[ContentRootPath]"; //Get the root path
path = System.IO.Path.Combine(path, "PDFs", "HelloWorld.pdfx");

//This extension method will load the template from the path and return a processed PDF inline
return this.PDF(path);

```


## Running from Mac

If you are running the application from Visual Studio Mac (as we are), then you may need to follow the instructions here [Libgdiplus installation on Mac](libgdiplus)
