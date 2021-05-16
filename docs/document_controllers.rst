================================
Controllers for your templates
================================

Sometimes it's just not quite enough to give the data and render the output.
More control is needed over altering the content.

Scryber supports this through the use of code controllers, which can be attached to 
files through the scryber processing instruction. And have properties set or methods called
during the document lifeycle.


Template with controller
-------------------------

.. code-block:: html

    <?scryber controller='ControllerNamespace.MyController, MyAssembly' ?>
    <html xmlns='http://www.w3.org/1999/xhtml' id='MyDocument' >
    <head>
        <title>HTML Document</title>
        <style>
            .grey{ background-color: grey; }
        </style>
    </head>

    <body class="grey" title="Page 1">
        <p id='DocPara' title="Inner">Hello World, from scryber.</p>
        <img on-databound='UpdateImagePath' src='{@:HeaderImageName}' />
        <div style='padding:20pt'>
            <img on-databound='UpdateImagePath' src='{@:ContentImageName}' />
        </div>
    </body>
    </html>


And the controller code
------------------------

.. code-block:: csharp

    using System;
    using Scryber;
    using Scryber.Components;
    using Scryber.Html.Components;

    namespace ControllerNamespace
    {
        // A new instance of the class will be created for each document generation.
        // But the classes must have a parameterless constructor.

        public class MyController
        {
            // This will be set to the instance with id 'MyDocument'
            // As it is required, then an error will be raised if there
            // is no HTMLDocument with the specified id - so you can be sure it is set.

            [PDFOutlet("MyDocument", Required=true)]
            public HTMLDocument Document { get; set; }

            // This property will be set to a paragraph with id 'DocPara'
            // as there is no explicit name on the outlet attribute.
            // If a paragraph is not found with this id, it will be null
            // but not an error.

            [PDFOutlet]
            public HTMLPara DocPara { get; set; }


            // constructors need to be parameterless
            // scrybere does not currently support dependency injection,
            // but can use the document params collection to store anything needed

            public MyController()
            {
            }


            // This action will be called after the images have been databound
            // So we know the src in the template will have been set.

            [PDFAction]
            public void UpdateImagePath(object sender, PDFDataBindEventArgs args)
            {
                //Do whatever is needed here to update the component.
                var img = sender as HTMLImage;

                //We can access the document from the property
                var path = this.Document.Params["basePath"] as string;

                //Update the image source, which has already been data bound.
                img.Source = "/" + path +"/" + img.Source + ".png";
            }
        }
    }

Outlets and Actions
--------------------

Scryber uses an opt-in approach to controllers. This allows the re-use of other classes and
makes sure the content being served is wanted.

A PDFOutlet is on property that will be set just after parsing of a template and controller instantiation.
It can be strongly typed, but as long as the referencing element in the template can be assigned it will be.

By default it the parser finds a componet with the same ID as an outlet property it will try to assign the coponent to that propoerty.

There are 2 attribute customizers that can be used to alter behaviour

    1. **Name** - If set to a string value, then that will be the ID of the component to use (rather than defaulting to the name of the actual Property itself.
    2. **Required** - This is false by default, but if set to true, then if the outlet is not assigned during parsing of the template an error will be raised.

Once set then the instance can be used an manipulated at run time however is seen fit.

A PDFAction is a method that is called during the processing of a component from initialize to load to binding to render that
can used to change the content or output of the document. New content can be added, or specific content removed.
What ever is needed.

All actions have their own specific signature, but follow the standard .net event handling mechanism. (more below)

There are again 2 attribute customizers available to alter behaviour.

    1. **Name** - If set the name that will be looked for on a template event attribute in preference to the actual method name.
    2. **IsAction** - This is by default true, so can be used. If it's set to false (for example by an overriding class method, then it will be ignored.

Actions can be called more than once, and can also be called inside repeating templates.

Event calling pipeline
------------------------

Scryber has a full event piplene that can be used at any stage in the document lifecycle.
All template handler attributes start with on-xxxx and are available on all element tags 

    1. **on-init** - Will be called at the very start, with the sender as the registered receiver and a PDFInitEventArgs instance. The full document structure many not be in place by this point.
    2. **on-load** - Will be called once all the document has be parsed and has the heirarchy in place, but not databound.
    3. **on-databind** - Will be called on each component in turn before any databinding statements are executed. e.g. {@:MyValue} will still be unset.
    4. **on-itemdatabound** - Will be called by a template each time a new item is databound in the content, passing the item that has been created as well as the context.
    5. **on-databound** - Will be called on each component in turn after any databinding statements have been executed and their values set.
    6. **on-prelayout** - Will be the last chance to inject any content into the document graph before it is converted to an explicit page layout.
    7. **on-postlayout** - Will be called with the actual content measured and laid out into eplicit pages, blocks, regions, lines and runs.
    8. **on-prerender** - Will be called before the layout is output to a stream with the right structure.
    9. **on-postrender** - Will be called after everything is done and rendered.

Some of the most opportune times to capture events are

**on-init** or **on-load** for the document, to prepare anything you may need as a controller.

**on-itemdatabound** for a template, so other content can be added or set up based on the context.

**on-databound** or **on-prelayout** for a component with dynamic content, that can be adjusted before laying out.

**on-postrender** for the document, so any resources can be cleaned up and/or disposed.


Event Method Signatures
-----------------------

As mentioned all the events follow the standard .net event method signature, each with specific arguments based on the type.
All the arguments contain at least a reference to the current context which can be specific to the pipeline but will as a minimum contain the following

    * Document - As an IPDFDocument which has been parsed.
    * Items - The **currrent** collection of Items that been assigned as parameters on the document or explicitly set.
    * TraceLog - For addign messages and statements to the log output
    * PerformanceMonitor - For capturing specific performance metrics
    * OutputFormat - Can only bbe PDF
    * ConformanceMode - Should errors be raised as exceptions, or logged.

Initialize has the PDFInitEventArgs with the PDFInitContext.

.. code-block:: csharp

    [PDFAction("init-para")]
    public void ParagraphInit(object sender, PDFInitEventArgs args)
    {
        (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We are initialized", StyleClass = "block"});
        args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Initialized the paragraph");
    }

Load has the PDFLoadEventArgs with the PDFLoadContext.

.. code-block:: csharp

    [PDFAction("load-para")]
    public void ParagraphLoad(object sender, PDFLoadEventArgs args)
    {
        (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We have loaded", StyleClass = "block"});
        args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Loaded the paragraph");
    }

DataBinding and DataBound have the PDFDataEventArgs with the PDFDataContext, which is far more interesting.
The PDFDataContext also has the current DataStack, CurrentIndex and a namespace resolver for inner parsing if needed.

The DataStack is a stack of the objects and IDataSource implementors that is consistent across all binding calls.
It is possible to push data onto the stack in the DataBind method, and pop it off after on the databound method.
This gives complete control over what children will use for binding at runtime, even without setting an explict document model parameters.

.. code-block:: csharp

    [PDFAction("bind-para")]
    public void ParagraphBinding(object sender, PDFDataBindEventArgs args)
    {
        (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We are binding", StyleClass = "block"});
        args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Binding the paragraph");
    }

    [PDFAction("bound-para")]
    public void ParagraphBound(object sender, PDFDataBindEventArgs args)
    {
        (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We have bound", StyleClass = "block"});
        args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Bound the paragraph");
    }

The Layout and render event handlers are more useful for component developers as they can really affect the quality of output, but are documented here for completeness.
They Layout contains a reference to the current document layout along with the PDFGraphics and the PDFStyleStack.
And the PDFRenderContext contains the current rendering pages, offsets and sizes along with the PDFGraphics and PDFStyleStack.

.. code-block:: csharp

    [PDFAction("pre-layout-para")]
    public void ParagraphPreLayout(object sender, PDFLayoutEventArgs args)
    {
        (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We are laying out", StyleClass = "block"});
        args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Laying-out the paragraph");
    }

    [PDFAction("post-layout-para")]
    public void ParagraphPostLayout(object sender, PDFLayoutEventArgs args)
    {
        //This label will not appear as we have finished using the components
        (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We have been laid out", StyleClass = "block"});
        
        args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Laid-out the paragraph");
    }


    [PDFAction("pre-render-para")]
    public void ParagraphPreRender(object sender, PDFRenderEventArgs args)
    {
        args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Rendering the paragraph");
    }

    [PDFAction("post-render-para")]
    public void ParagraphBinding(object sender, PDFRenderEventArgs args)
    {
        args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Rendered the paragraph");
    }


Adding to a template
---------------------

If we apply the methods above to a template with our controller specified

.. code-block:: html

    <?scryber append-log='true' controller='Scryber.Core.UnitTests.Mocks.GenericControllerClass, Scryber.UnitTests' ?>
    <!DOCTYPE HTML>
    <html xmlns='http://www.w3.org/1999/xhtml' id="MyDocument" on-init="Initialized">
    <head>
        <title></title>
        <style type="text/css">

            body {
                font-size: 14pt;
            }

            .block{
                display:block;
                border:solid 1px blue;
                padding:5pt;
                margin-bottom: 5pt;
                width:100%;
            }

        </style>
    </head>
    <body style="padding:20pt">
        <p on-init="init-para" on-loaded="load-para"
        on-databinding="bind-para" on-databound="bound-para"
        on-prelayout="pre-layout-para" on-post-layout="post-layout-para"
        on-prerender="pre-render-para" on-postrender="post-render-para"></p>
    </body>
    </html>

We can see the output in the page up to the point of layout and the messages in the log.

 
.. image:: images/BindingResults.png

.. image:: images/BindingResultsLog.png


Dependency Injection
--------------------

The controller must have a parameterless constructor, but if access to other 
instances and services is needed, they can be passed to the document and then used on the controller.

.. code-block:: csharp

    //document parsing

    var doc = Document.ParseTemplate("path.html");
    doc.Params["DataService"] = GetDataService();

    doc.SaveAsPDF("Path.pdf");


.. code-block:: csharp

    [PDFAction("load-doc")]
    public void DocumentLoaded(object sender, PDFLoadEventArgs args)
    {
        PDFDocument doc = (PDFDocument)args.Document;
        this.DataService = (MyDataService)doc.Params["DataService"];

        //Do what ever else is needed.
    }

Events inside a <template>
-----------------------

The **on-item-databound** event will be called each and every time a template creates and binds the inner content.
Any events registered within the template, on components, will be raised for each and every component.


.. code-block:: html

    <?scryber controller='ControllerNamespace.MyController, MyAssembly' ?>
    <html xmlns='http://www.w3.org/1999/xhtml' id='MyDocument' >
    <head>
        <title>HTML Document</title>
        <style>
            .grey{ background-color: grey; }
        </style>
    </head>

    <body class="grey" title="Page 1">
        
        <!-- The template item binding event will be bound for each of the items -->
        <template data-bind='{@:AllItems}' on-item-databound='template-item-bound" >

            <!-- The image will be bound for each of the items -->
            <img on-databound='image-item-bound' src='{@:ContentImageName}' />
        </div>
    </body>
    </html>