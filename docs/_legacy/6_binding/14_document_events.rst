=====================================
Binding to events - TD
=====================================


6.14.1. Document Processing lifecycle
------------------------------------

When creating a PDF document from a template in your code there is a clear linear process that is followed to generate the final output.

* Parsing the template creates the document object model.
* Initialize and load on each component to ensure the correct state.
* Databind to any data models (which can create further components)
* Layout converts the high level components to lower level entities
* Render allows the layout entities to render themselves to a PDFWriter

Between parsing and initializing is a neat point to add your own content, or add any model(s) needed to the document,
along with setting up any events or custom code.

Each of the stages raises events that can be captured to perform any custom processing required

.. figure:: ../images/doc_lifecycle.png
    :target: ../_images/doc_lifecycle.png
    :alt: Package and library references
    :class: with-shadow

`Full size version <../_images/doc_lifecycle.png>`_
