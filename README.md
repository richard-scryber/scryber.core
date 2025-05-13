
# Scryber.Core PDF Engine

## Scryber makes creating beautiful documents easy.

The scryber engine is an advanced, complete, pdf creation library for dotnet core.

It supports the easy definition of document templates with, pages, content, shapes and images using xhtml and/or code.

With a styles based template layout, it is easy to create good looking, paginated and flowing documents.

With dynamic content from you applications or sites it is easy to add dynamic data, and repeaters.

**Now** includes support for expressions in both templates and css styles.



## scryber supports:

- standard [html body, tables, lists, divs and spans](https://scrybercore.readthedocs.io/en/latest/document_components.html) and many newer html5 [tags](https://scrybercore.readthedocs.io/en/latest/document_components.html)
- flowing and flexible layout with multiple [pages in css sizes](https://scrybercore.readthedocs.io/en/latest/document_pages.html), along with page [headers, footers and breaks](https://scrybercore.readthedocs.io/en/latest/document_pages.html), and [page numbering](https://scrybercore.readthedocs.io/en/latest/document_pagenumbering.html)
- cascading styles: linked, embedded or inline using [css syntax and priority](https://scrybercore.readthedocs.io/en/latest/document_styles.html).
- databinding for [dynamic content or style](https://scrybercore.readthedocs.io/en/latest/binding_model.html) on simple and complex object models with [repeating templates](https://scrybercore.readthedocs.io/en/latest/binding_model.html#using-objects).
- [embedding](https://scrybercore.readthedocs.io/en/latest/document_references.html) of external content,
- [sizing](https://scrybercore.readthedocs.io/en/latest/component_sizing.html) and [positioning](https://scrybercore.readthedocs.io/en/latest/component_positioning.html) of elements inline, block, relative or absolute.
- [images](https://scrybercore.readthedocs.io/en/latest/drawing_images.html) and colours with text and shape [fills, backgrounds](https://scrybercore.readthedocs.io/en/latest/drawing_image_backgrounds.html) and borders.
- [multiple fonts](https://scrybercore.readthedocs.io/en/latest/drawing_fonts.html), including google fonts, supporting text [alignment; spacing; leading; decoration](https://scrybercore.readthedocs.io/en/latest/document_textlayout.html) and breaking.
- Graphics support for [drawing and paths](https://scrybercore.readthedocs.io/en/latest/drawing_paths.html) and text.
- Password [security and restrictions](https://scrybercore.readthedocs.io/en/latest/document_security.html) on pdf files.
- [Full code](https://scrybercore.readthedocs.io/en/latest/document_code_vs_xml.html) support either as a whole document, or partial content, and controllers, along with the html/css templates.
- Document [controllers](https://scrybercore.readthedocs.io/en/latest/document_controllers.html) for complete control of the layout

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
                /* use of css variables that can be changed at generation time */
                :root{
                    --head-bg: #FFF;
                    --head-txt: #000;
                    --head-logo: url('../html/images/ScyberLogo2_alpha_small.png');
                    --head-space: 20px;
                }

                body{
                    font-family: sans-serif;
                    font-size: 14pt;
                }

                p.header {
                    color: var(--head-txt);
                    background-color: var(--head-bg);
                    background-image: var(--head-logo);
                    background-repeat: no-repeat;
                    background-position: var(--head-space) var(--head-space);
                    background-size: 20pt 20pt;
                    margin-top: 0pt;
                    padding: var(--head-space);
                    padding-bottom: calc(--head-space + 25pt); /* full calc support */
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
                <h2 style="{{model.titlestyle}}">{{model.title}}</h2>
                <div>We hope you like it.</div>
                <!-- Loop over or nested items binding in the parameters -->
                <ol>
                    <template data-bind='{{model.items}}'>
                        <!-- and bind the name value in the current object -->
                        <li>{{.name}}</li> 
                    </template>
                </ol>
            </main>
            <footer>
                <!-- footers and page numbers -->
                <table class="foot" style="width:100%">
                    <tr>
                        <td>{{author}}</td>
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
            //Load your template from a 
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
                //pass values to the document, including css using params

                doc.Params["author"] = "Scryber Engine";
                doc.Params["--head-bg"] = "#333"; //Override for the header background
                doc.Params["--head-txt"] = "#FFF";
                
                //pass data paramters as needed, supporting simple values, arrays or complex classes.

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

      public async IActionResult HelloWorld(string title = "This is the title")
      {
          using(var doc = Document.ParseDocument("[input template]"))
          {
              doc.Params["author"] = "Scryber Engine";
              doc.Params["--head-bg"] = "#333"; //Override for the header background
              doc.Params["--head-txt"] = "#FFF";

              doc.Params["model"] = GetMyParameters(title);

              //This will output to the response inline.
              return await this.PDFAsync(doc); // inline:false, outputFileName:"HelloWorld.pdf"
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

