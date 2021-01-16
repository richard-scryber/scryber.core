<h1 align='center'>
    <img height='200' src='https://raw.githubusercontent.com/richard-scryber/scryber.core/master/ScyberLogo2_alpha_small.png'/>
    <br/>
    scryber.core pdf engine
</h1>

## Make documents easily

The scryber engine is an advanced, complete, pdf creation library for dotnet core. 
It supports the easy definition of documents, pages, content, shapes and images either by html and templates or simple code. 

With a styles based layout it is easy to create good looking and flowing documents. 
Binding in xml allows you to quickly load data from many sources and output to PDF.

## HTML First

The latest version of scryber makes a significant switch to an XHTML first approach.
This supports cascading style sheets, databinding, repeating templates, imports and page directives.

## Getting Started

The easiest way to begin is to use the Nuget Packages here

[scryber.core package](https://www.nuget.org/packages/scryber.core/)
(Base libraries for GUI or console applications)

OR for asp.net mvc

[scryber.core.mvc package](https://www.nuget.org/packages/scryber.core.mvc/)
(Which includes the scryber.core package).

The full documentation is available here

[scryber.core documentation](https://scrybercore.readthedocs.io/en/latest/)

## Example Template


```html

    <!DOCTYPE HTML >
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <!-- support for standard document attributes -->
            <meta charset='utf-8' name='author' content='Richard Hewitson' />
            <title>Hello World</title>
            <!-- support for complex css selectors (or link ot external style sheets )-->
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

                .foot{ display:none; }
                page { display:none; }
            

                /* Page directives and breaks are supported, as are @media
                    directives for print only */

                @page{
                    size: A4;
                }

                @media print {

                    page {
                        display: inline;
                    }

                    .foot {
                        display: block;
                        font-size: 10pt;
                        margin-bottom: 10pt;
                    }
                    .foot td{
                        border:none;
                        text-align:center;
                    }
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
                <ol>
                    <!-- Loop through the items in the model -->
                    <template data-bind='{@:model.items}'>
                        <li>{@:.name}</li> <!-- and bind the name value -->
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

            //pass paramters as needed, supporting arrays or complex classes.
            var items = new[]
            {
                new { name = "First item" },
                new { name = "Second item" },
                new { name = "Third item" },
            };

            var model = new
            {
                titlestyle = "color:#ff6347",
                title = "Hello from scryber",
                items = items
            };

            using (var doc = Document.ParseDocument(path))
            {
                //pass paramters as needed, supporting simple values, arrays or complex classes.
                doc.Params["author"] = "Scryber Engine";
                doc.Params["model"] = model;
                using (var stream = DocStreams.GetOutputStream("READMESample.pdf"))
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
              doc.Params["model"] = GetMyParameters(title);
              
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

