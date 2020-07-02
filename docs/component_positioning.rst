========================
Positioning your content
========================

Scryber has an intelligent layout engine. By default eveything will be laid out as per the flowing layout of the document Pages and columns.
Each component, be it block level or inline will have a position next to its siblings and move and following content along in the document.
If the content comes to the end of the page and cannot be fitted, then if allowed, it will be moved to the next page.

Relative Positioning
====================

This declares the position of the component relative to the block parent.
By default the position will be 0,0 (top, left), but using the x and y attributes it can be altered.

The component will no longer be in the flow of any inline content, nor alter the layout of the following components.

The parent block will however grow to accomodate the content including it's relative positioning.

[Example TBD]


Absolute Positioning
====================

The declares the position of the component relative to the current output page.
By default the position will again be 0,0 (top, left), but using the x and y attributes it can be altered.

The component will no longer be in the flow of any content, nor alter the layout of following components.

The parent block will NOT grow to accomodate the content, it is outside of the document flow completely.

If the absolutely positioned component is too big to fit on the page it will be clipped and not cause any overflow.

[Example TBD]


Numeric Positioning
===================

All content positioning is from the top left corner of the page or parent. 
This is a natural positioning mechanism for most cultures and developers.

Units of position can either be specified in 

* points (1/72 of an inch) e.g `36pt`, 
* inches e.g. `0.5in` or 
* millimeters e.g. `12.7mm`


If no units are specified then the default is points. See `Scryber Units <drawing_units>`_ for more information

Inner content
=============

Whether a block is relative or absolutely positioned does not matter to the inner content. 
The inner content will continue to flow as normal, or also be relatively or absolutely positioned.

[Example TBD]

Rendering Order
===============

All relative or absolutely positioned content will be rendered to the output in the order it appears in the document.
If a block is relatively positioned, it will overlay any content that preceded it, but anything coming after will be over the top.

[Example TBD]

Positioning components
======================

There are 2 components that take advantage of the positioning within Scryber.

1. **The Canvas** positions all direct child components in the canvas as relative, whether they have been decared as such or not.
2. **The Layer Group** has a collection of child Layers. These will be relatively positioned to the group.