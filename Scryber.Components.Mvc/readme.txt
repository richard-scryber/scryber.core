-----------------------------------------------------
---------         Scryber.Core.Mvc          ---------
-----------------------------------------------------

Scryber is an advanced PDF generation tool that allows
complete definition of a dynamic PDF document based
on XML (and code) including cascading styles,
databinding and component separation in referenced files.

This framework is built entirely in .NETCore and is
open source under the LGPL licence so you can link to
it in commercial applications.



Thanks for using.

We are still checking and working everything out.
But it's looking good.

---------------------------------
Upgrading from previous versions
---------------------------------

If you are upgrading from pre-version 1.1.0 please note.
We have taken the opportunity in these early days to remove 
the PDF prefix from the Scryber.Components classes

PDFDocument is now Document
PDFPage is now Page
etc.

It's been needed for a while, but now is a good time.

ProcessDocument has changed to SaveToPDF - again much needed.

Many other changes have been made, see


---------------
Getting Started
---------------

There's an article here about creating your first
document on 'read the docs' here.

https://scrybercore.readthedocs.io/en/latest/


------------
Scryber.Core
-------------

For GUI or Console applications, use the base Scryber.Core
package so the web based references are not needed.


-----------
Xml Schemas
-----------

The xml schemas for the templates can be downloaded fron
the git hub source here.

https://github.com/richard-scryber/scryber.core/tree/master/Scryber.Core.Schemas

---------------------------
Mac (or Linux) - Read this.
---------------------------

Scryber needs the GDIPlus capabilies of dotnet.
By default these are not installed.

There is an article here for using homebrew to
install these.

https://scrybercore.readthedocs.io/en/latest/libgdiplus.html


