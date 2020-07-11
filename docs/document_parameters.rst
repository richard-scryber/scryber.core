================================
A Scryber Document parameters
================================

The document parameters are values that can be set within the template or in code.

Declaring in Documents
======================

Simple Parameter Types
======================

Changing the values
===================


Complex Dot notation
====================



The MVC model
=============




Passing to References
=====================


XML and Template parameters
===========================

Combining selector paths
========================

Both the object and xpath selectors support complex notation for retrieving values.


* {@:*dotnotation*} for binding to a paramter passed to the document. This supports complex paths
    * {@:ParamName} for the direct value.
    * {@:ParamName.Property} for getting a property value.
    * {@:ParamName[n]} for getting the n'th value from an array
    * {@:ParamName['key']} for geting a dictionary value based on key.
    * The statements can be chained together as long as needed.
* {xpath:*selector*} for binding to xml content.
    * {xpath://root} for the root element
    * {xpath:element/@attribute} for inner selection
    * {xpath:../parentsibling} for traversing back up the tree
    * {xpath:concat('prefix ',selector)} for using the xpath functions.

Object Types
============

Scryber is strongly typed. And the object notation enforces this too. There are various types of parameter, and the object selectors 
also rely on the type being set, to be the same as the value assigned.



