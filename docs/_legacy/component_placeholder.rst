==========================================
Placeholders and dynamic content - td
==========================================

Placeholders are just that, content that can be replaced at generation time explicitly or databound.

Without content a placeholder will be empty, invisible, and take up zero space. Adding content will make this as part of the flow, without the controller or code
knowing how it will be used, or where it will be placed.



Adding a placeholder
=====================


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                    xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd" >

    <Styles>
        <styles:Style applied-class="bordered" >
        <styles:Border color="#777" width="1pt" style="Solid"/>
        <styles:Background color="#EEE"/>
        <styles:Padding all="4pt"/>
        </styles:Style>
    </Styles>
    <Pages>
    
        <doc:Page styles:margins="20pt" styles:font-size="18pt">
        <Content>
            <doc:Div styles:class="bordered" >
            The content of this div is all as a block (by default)
            </doc:Div>

            <!-- and empty placeholder -->
            <doc:PlaceHolder id="DynamicContent" />
            
            <doc:Div styles:class="bordered" styles:width="300pt" >
            This is after the placeholder.
            </doc:Div>
        
        </Content>
        </doc:Page>
    </Pages>

    </doc:Document>

.. image:: images/documentPlaceholders.png


Assigning content
=================

Once the placeholder is available it can be assigned as needed.

.. code-block:: csharp

        [HttpGet]
        public IActionResult PlaceholderDocument()
        {
            var path = _rootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "DocumentParameters.pdfx");
            var doc = PDFDocument.ParseDocument(path);

            //In this example, just create a random table
            //Replace with anything needed.

            var place = doc.FindAComponentById("DynamicContent") as PDFPlaceHolder;
            var table = new PDFTableGrid();

            table.Style.Size.FullWidth = true;
            table.Style.Padding.All = 5;

            place.Contents.Add(table);

            for(var r = 0; r < 5; r++)
            {
                var row = new PDFTableRow();
                table.Rows.Add(row);

                for (var c = 0; c < 3; c++)
                {
                    var cell = new PDFTableCell();
                    var literal = new PDFTextLiteral("Cell " + ((r * 3) + c));
                    cell.Contents.Add(literal);
                    row.Cells.Add(cell);
                }
            }

            return this.PDF(doc);
        }


And the content will become a fully qualified member of the page.

.. image:: images/documentPlaceholdersFilled.png

.. note:: This could just have easily be done within a console application or app.

Passing content as a parameter
==============================

If the placeholder is in a template, or used in multiple places, 
it is often easier to simply specifiy a template parameter and use that as a template for the content in a placeholder.

The parameter can then be updated in the code, and complete independence is made.

.. code-block:: xml

    <?xml version='1.0' encoding='utf-8' ?>
    <doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                    xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd' >

    <Params>
        <!-- template parameter with default content -->
        <doc:Template-Param id='datatable' >
            <doc:Div styles:class='bordered'>No data is available for the table</doc:Div>
        </doc:Template-Param>
    </Params>
    
    <Styles>
        <styles:Style applied-class='bordered' >
        <styles:Border color='#777' width='1pt' style='Solid'/>
        <styles:Background color='#EEE'/>
        <styles:Padding all='4pt'/>
        </styles:Style>
    </Styles>
    <Pages>
    
        <doc:Page styles:margins='20pt' styles:font-size='18pt'>
        <Content>
            <doc:Div styles:class='bordered' >
            The content of this div is all as a block (by default)
            </doc:Div>
            
            <!-- bind to a template parameter with a default value -->
            <doc:PlaceHolder id='DynamicContent' template='{@:datatable}' />
            
            <doc:Div styles:class='bordered' styles:width='300pt' >
                This is after the placeholder.
            </doc:Div>
        
        </Content>
        </doc:Page>
    </Pages>

    </doc:Document>


In our code set the template parameter to a string of components 
(or load from another source, file or view).

.. code-block:: csharp

        [HttpGet]
        public IActionResult PlaceholderTemplateDocument()
        {
            var path = _rootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "DocumentPlaceholderDynamic.pdfx");
            var doc = PDFDocument.ParseDocument(path);

            //In this example, just create a random table
            var data = GetTemplateData();
            doc.Params["datatable"] = data;

            return this.PDF(doc);
        }

        private string GetTemplateData()
        {
            //This content could be from any source, and could use XElements, XMLNodes or any other source.
            //And really we should be using a string builder anyway.

            var str = "<doc:Table styles:full-width='true' >";
            for(var i = 0; i < 5; i++)
            {
                str += "<doc:Row><doc:Cell >" + i + "</doc:Cell><doc:Cell >" + (i + 5) + "</doc:Cell></doc:Row>";
            }
            str += "</doc:Table>";

            return str;
            
        }


And output the document as normal.

.. image:: images/documentPlaceholderTemplate.png


..note:: We could have also used a string parameter (`doc:String-Param`), and then bound the `content` attribute 
to that string.