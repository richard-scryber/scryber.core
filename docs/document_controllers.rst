================================
Conntrollers for your templates
================================

Sometimes it's jjust not quite enough to give the data and render the output.
More control is needed over altering the content.

Scryber supports this through the use of code controllers, which can be attached to 
files through the scryber processing instruction. And have properties set or methods called
during its lifeycle.


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
            <img on-database='UpdateImagePath' src='{@:ContentImageName}' />
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

    1. Name - If set to a string value, then that will be the ID of the component to use (rather than defaulting to the name of the actual Property itself.
    2. Required - This is false by default, but if set to true, then if the outlet is not assigned during parsing of the template an error will be raised.

Once set then the instance can be used an manipulated at run time however is seen fit.

A PDFAction is a method that is called during the processing of a component from initialize to load to binding to render that
can used to change the content or output of the document. New content can be added, or specific content removed.
What ever is needed.

All actions have their own specific signature, but follow the standard .net event handling mechanism. (more below)

There are again 2 attribute customizers available to alter behaviour.

    1. Name - If set the name that will be looked for on a template event attribute in preference to the actual method name.
    2. IsAction - This is by default true, so can be used. If it's set to false (for example by an overriding class method, then it will be ignored.

Actions can be called more than once, and can also be called inside repeating templates.

Event calling pipeline
------------------------


Events in a <template>
-----------------------

Controllers in partial templates
--------------------------------


Cleaning Up
-------------

