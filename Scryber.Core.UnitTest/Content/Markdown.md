# scryber.core
The dotnet core scryber pdf creation library

## Scryber PDF Library

The scryber library is an advanced, complete, pdf creation library for dotnet core. 
It supports the easy definition of documents, pages, content, shapes and images either by xml templates or simple code. 

With a styles based layout it is easy to create good looking and flowing documents. 
Binding in xml allows you to quickly load data from many sources and output to PDF. 


## Getting Started

The easiest way to begin is to use the Nuget Packages here

[scryber.core package](https://www.nuget.org/packages/scryber.core/)
(Base libraries for GUI or console applications)

OR for asp.net mvc

[scryber.core.mvc package](https://www.nuget.org/packages/scryber.core.mvc/)
(Which includes the scryber.core package).

The full documentation is available here

[scryber.core documentation](https://scrybercore.readthedocs.io/en/latest/)

## Hello World Plus

Just a bit more than a hello world example.

### Create your template pdfx (xml) file.

```xml

      <?xml version='1.0' encoding='utf-8' ?>
      <pdf:Document xmlns:pdf='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                    xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                    xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
        <Params>
          <pdf:String-Param id='Title' value='Hello World'  />
        </Params>
        <Styles>
          <styles:Style applied-type='pdf:Page'>
            <styles:Font family='Arial' size='14pt' />
          </styles:Style>

          <styles:Style applied-class='heading' >
            <styles:Fill color='#FF7777'/>
            <styles:Text decoration='Underline'/>
          </styles:Style>
      
        </Styles>
        <Pages>

          <pdf:Page styles:margins='20pt'>
            <Content>
              <pdf:H1 styles:class='heading' text='{@:Title}' />
              <pdf:Div>We hope you like scryber.</pdf:Div>

            </Content>
          </pdf:Page>
        </Pages>

      </pdf:Document>
```

### From your application code.

```cs

      //using Scryber.Components

      static void Main(string[] args)
      {
          using(var doc = PDFDocument.ParseDocument("[input template].pdfx"))
          {
              doc.Params["Title"] = "Hello World from Scryber";
              var page = doc.Pages[0] as PDFPage;
              page.Contents.Add(new PDFLabel(){ Text = "My Content" });
              doc.ProcessDocument("[output file].pdf");
          }
      }
```

### Or from an MVC web application

```cs

      //using Scryber.Components
      //using Scryber.Components.Mvc

      public IActionResult HelloWorld(string title = "Hello World from Scryber")
      {
        using(var doc = PDFDocument.ParseDocument("[input template].pdfx"))
          {
              doc.Params["Title"] = title;
              var page = doc.Pages[0] as PDFPage;
              page.Contents.Add(new PDFLabel(){ Text = "My Content" });
          
              return this.PDF(doc); // inline:false, outputFileName:"HelloWorld.pdf"
          }
      }
```

### And the output


Check out Read the Docs for more information on how to use the library.

[scryber.core documentation](https://scrybercore.readthedocs.io/en/latest/)


## Getting Involved

We would love to hear your feedback. Feel free to get in touch.
Issues, ideas, includes are all welcome.

If you would like to help with building, extending then happy to get contributions

