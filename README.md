<h1 align='center'>
    <img height='200' src='https://raw.githubusercontent.com/richard-scryber/scryber.core/master/ScyberLogo2_alpha_small.png'/>
    <br/>
    scryber.core pdf engine
</h1>

## Change the way you create documents.

The scryber engine is an advanced, complete, pdf creation library for dotnet core 5.
It supports the easy definition of documents, pages, content, shapes and images with html templates and simple code. 

With a styles based template layout it is easy to create good looking,
flowing documents with dynamic content from you applications or sites.


## HTML First

The latest version of scryber makes a significant switch to an XHTML first approach.
If you know HTML you can create documents.

scryber supports:

- standard html body, tables, lists, divs and spans and many newer html5 tags
- flowing and flexible layout with multiple pages in css sizes, along with page headers, footers and breaks.
- cascading styles: linked, embedded or inline using css syntax and priority.
- databinding for dynamic content on simple and complex objects with repeating templates.
- iframe imports of external content,
- sizing and positioning of elements inline, block, relative or absolute.
- images and colours with text and shape fills backgrounds and borders.
- multiple fonts, including google fonts, supporting text alignment; spacing; leading; decoration and breaking.

If you have used the previous pdfx files, the older templates should continue to work. But we will be concentrating on html, css and svg going forwards.


## Getting Started

The easiest way to begin is to use the Nuget Packages here

[scryber.core package](https://www.nuget.org/packages/scryber.core/)
(Base libraries for GUI or console applications)

OR for asp.net mvc

[scryber.core.mvc package](https://www.nuget.org/packages/scryber.core.mvc/)
(Which includes the scryber.core package).

Check out Read the Docs for more information on how to use the library.

[scryber.core documentation](https://scrybercore.readthedocs.io/en/latest/)

## Example Template

Create a new html template file with your content.

```html

    <!DOCTYPE HTML >
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <!-- support for standard document attributes -->
            <meta charset='utf-8' name='author' content='Richard Hewitson' />
            <title>Hello World</title>

            <!-- support for complex css selectors (or link to external style sheets )-->
            <style>
                body{
                    font-family: sans-serif;
                    font-size: 14pt;
                }

                p.header {
                    color: #AAA;
                    background-color: #333;
                    background-image: url('../html/images/ScyberLogo2_alpha_small.png');
                    background-repeat: no-repeat;
                    background-position: 10pt 10pt;
                    background-size: 20pt 20pt;
                    margin-top: 0pt;
                    padding: 10pt 10pt 10pt 35pt;
                }

                .foot td {
                    border: none;
                    text-align: center;
                    font-size: 10pt;
                    margin-bottom: 10pt;
                }

            </style>
        </head>
        <body>
            <header>
                <!-- document headers -->
                <p class="header">Scryber document creation</p>
            </header>
            <!-- support for many HTML5 tags-->
            <main style="padding:10pt">

                <!-- binding style and values on content -->
                <h2 style="{@:model.titlestyle}">{@:model.title}</h2>
                <div>We hope you like it.</div>
                <!-- Loop over or nested items binding in the parameters -->
                <ol>
                    <template data-bind='{@:model.items}'>
                        <!-- and bind the name value in the current object -->
                        <li>{@:.name}</li> 
                    </template>
                </ol>
            </main>
            <footer>
                <!-- footers and page numbers -->
                <table class="foot" style="width:100%">
                    <tr>
                        <td>{@:author}</td>
                        <td><page /></td>
                        <td>Hello World Sample</td>
                    </tr>
                </table>
            </footer>
        </body>
    </html>

```

### From your application code.

```cs

      //using Scryber.Components

      static void Main(string[] args)
      {
          var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/READMESample.html");

            //create our sample model data.

            var model = new
            {
                titlestyle = "color:#ff6347",
                title = "Hello from scryber",
                items = new[]
                {
                    new { name = "First item" },
                    new { name = "Second item" },
                    new { name = "Third item" },
                }
            };

            using (var doc = Document.ParseDocument(path))
            {
                //pass data paramters as needed, supporting simple values, arrays or complex classes.

                doc.Params["author"] = "Scryber Engine";
                doc.Params["model"] = model;

                //And save it to a file or a stream
                using (var stream = new System.IO.FileStream("READMESample.pdf", System.IO.FileMode.Create))
                {
                    doc.SaveAsPDF(stream);
                }

            }
      }
```

### Or from an MVC web application

```cs

      //using Scryber.Components
      //using Scryber.Components.Mvc

      public IActionResult HelloWorld(string title = "This is the title")
      {
          using(var doc = Document.ParseDocument("[input template]"))
          {
              doc.Params["author"] = "Scryber Engine";
              doc.Params["model"] = GetMyParameters(title);

              //This will output to the response inline.
              return this.PDF(doc); // inline:false, outputFileName:"HelloWorld.pdf"
          }
      }
```

### And the output

![Hello World Output](https://raw.githubusercontent.com/richard-scryber/scryber.core/svgParsing/docs/images/helloworld.png)

Check out Read the Docs for more information on how to use the library.

[scryber.core documentation](https://scrybercore.readthedocs.io/en/latest/)


## Getting Involved

We would love to hear your feedback. Feel free to get in touch.
Issues, ideas, includes are all welcome.

If you would like to help with building, extending then happy to get contributions

