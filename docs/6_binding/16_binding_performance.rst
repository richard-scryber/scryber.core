============================================
Binding Performance and style caching - PD
============================================

Scryber does a number of activities underneath to improve performance and reuse cached data where possible.
Downloaded font-files are kept locally
Images can be cached and re-used, and only one reference is used in the files.

It also caches the full stype of individual elements. Including those in templates.
Appending the log file can also give great insight into the 

If for example there is a need to output a 10,000 row listing to PDF then we can create a template to do that and inject the Model.

.. code-block:: html

    <?scryber append-log='true' log-level='messages' ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml' title="Root">
    <head>
        <title>Large File Test Document</title>
        <style>
            .grey{ background-color: grey; padding: 20pt}
            td.key{ color: #333; font-size:10pt;}
            td.value{  color: #333; font-size:10pt; }
            tr.odd{ background-color: #AAA;}
            tr.even{ background-color: #CCC; }
        </style>
    </head>
    <body class="grey" title="Outer">
        <header>
            <p style="text-align:center; font-size:12pt; padding:5pt; border-bottom: solid 1pt red; margin-bottom: 5pt;">Binding large data</p>
        </header>
        <p title="Inner">This is a test of binding the content to a table.</p>
        <div style="column-count: 2">
            <table style="width:100%;">
                <thead style="font-weight:bold;">
                    <tr>
                        <td class="key">Name</td>
                        <td class="value" style="width: 120pt">Value</td>
                    </tr>
                </thead>
                <!-- our table is built with 2 columns -->
                <template data-bind="{@:model.Items}" >
                    <tr class="{@:.Row}">
                        <td class="key">{@:.Key}</td>
                        <td class="value">{@:.Value}</td>
                    </tr>
                </template>
            </table>
        </div>
        <footer>
            <p style="text-align:center; font-size: 12pt; padding:5pt; border-top: solid 1pt red; margin-bottom: 5pt;">
                Page <page /> of <page property="total" />.
            </p>
        </footer>
    </body>
    </html>

.. code-block:: csharp


        public void LargeFileTest()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/LargeFile.html");

            data = new
            {
                Items = GetListItems(10000)
            };
            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["model"] = data;
                using (var stream = DocStreams.GetOutputStream("LargeFile.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        private class ListItem
        {
            public string Key { get; set; }
            public int Value { get; set; }
            public string Row { get; set; }
        }

        private static ListItem[] GetListItems(int count)
        {
            var mocks = new ListItem[count];

            for (int i = 0; i < count; i++)
            {
                ListItem m = new ListItem() { Key = "Item " + i.ToString(), Value = i, Row = (i % 2 == 1) ? "odd" : "even" };
                mocks[i] = m;
            }

            return mocks;
        }

And the output from this will be 157 pages of lovely tables of content.

.. image:: images/documentStyleCaching.png

As can be seen with the scryber processing instruction in the template, we are appending the trace log tables to this file.

.. image:: images/documentStyleCachingTrace.png


The contents of this show the breakdown of time including the template parsing - 680ms for 10,314 items (table rows, page header and footers).
