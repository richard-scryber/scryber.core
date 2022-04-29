==========================================
Document lifecycle behaviour - TD
==========================================

The examples so far have shown various phases of the lifecycle from parsing to output, and also places where it is possible to alter 
content within the template, object graph and output.

There is a complete object lifecycle behind the creation, building and binding, layout and finally rendering and output to allows content to
be modified at almost any point. :see::../6_binding/14_document_events for more information.

But at the main level there are 3 points for changing content.

1. Before the parsing of a template
-----------------------------------


This can alter the content before it becomes an object. It is a string, a file or an XML graph (or an MVC view)

2. After parsing and before processing.
---------------------------------------

Once the template has been loaded the returned graph can be modified, by navigating the tree or looking directly for specific components.


3. During data binding
-----------------------

As part of the lifecycle a document will be bound to any provided sources. 
This is a good place to add dynamic content before any assumptions on layout and positioning are made, but after the resolution of css files.


Asyncronous operations
----------------------

As part of an MVC appliction the use of dynamic content can be performed asyncronously.
This means that referenced css files and do not block the executng threads whilst they are loaded remotely.