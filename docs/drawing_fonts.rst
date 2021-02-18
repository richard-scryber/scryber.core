======================================
Document fonts and text styles
======================================

PDF supports standard fonts. Scryber also supports the standard names of 'sans-serif', 'serif', 'monospace'.

Fonts available in the current operating system that the application has access to, can also be used. 
They are referenced by their postscript name.

Fonts can also be dynamically included with the @font-face rule.

Scryber supports the use of the font fallback in styles.

..  note:: scryber currently only supports True type &tm; and Open Type fonts - ttf (otf) and ttc (otc)

Built in fonts
----------------

The standard built in fonts with PDF readers that can be used in documents. These are as follows

* sans-serif / Helvetica - Regular, Bold, Italic and Bold Italic.
* serif / Times - Regular, Bold, Italic and Bold Italic.
* monospace / Courier - Regular, Bold, Italic and Bold Italic.

If used, then if the font is available it will be embedded, as is best practice.


.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>{@:Content.Title}</title>
        <meta name="author" content="{@:DocAuthor}" />
        <style type="text/css">
            
            .std-font{
                font-size: 20pt;
                background-color:#AAA;
                padding: 4pt;
                margin-bottom:10pt;
            }

        </style>
    </head>
    <body style="padding: 20pt">
        <div id="first" class="std-font" style="font-family:sans-serif">
            Helvetica is the default (sans-serif) font.<br/>
            Helvetica <b>Bold</b>, <i>Italic</i>, and <span style="font-weight:bold; font-style:italic">Bold Italic</span> are available.
        </div>

        <div id="first" class="std-font" style="font-family:serif">
            Times is the serif font.<br />
            Times <b>Bold</b>, <i>Italic</i>, and <span style="font-weight:bold; font-style:italic">Bold Italic</span> are available.
        </div>

        <div id="first" class="std-font" style="font-family:monospace">
            Courier is the monospaced font.<br />
            Courier <b>Bold</b>, <i>Italic</i>, and <span style="font-weight:bold; font-style:italic">Bold Italic</span> are available.
        </div>


    </body>
    </html>

.. image:: images/drawingfontsStandard.png



Using different fonts
----------------------

Along with the standard fonts, scryber supports the systems fonts (the fonts in the Environment.SpecialFolder.Fonts).
It does not support postscript font files but does support.

* ttf & otf - A truetype font file or opentype font file.
* ttc & otc - A truetype font collection (multiple styles) or open type collection


Fonts should be referred to by their Unicode Name, usually displayed through the font browser of the underlying operating system.
Rather than the file name of the ttf or ttc file.

.. image:: images/drawingFontsSelect.png

The following uses 3 different ttf fonts installed on the machine generating the document.
But using the standard css font fallback if a font does not exist it can fall back to one of the known fonts 

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>System Font Files</title>
        <meta name="author" content="Scryber Team" />
        <style type="text/css">
            
            .std-font{
                font-size: 20pt;
                background-color:#AAA;
                padding: 4pt;
                margin-bottom:10pt;
            }

            .sans {
                font-family: 'Segoe UI', sans-serif;
            }

            .serif{
                font-family: Optima, Times, Times New Roman, serif;
            }

            .avenir{
                font-family:'Avenir Next Condensed', sans-serif;
            }

            .none {
                font-family: 'Made Up Font', monospace;
            }
        </style>
    </head>
    <body style="padding: 20pt">
        <div id="first" class="std-font sans">
            Segoe UI is used from a font style from the system fonts.<br />
            Segoe UI <b>Bold</b>, <i>Italic</i>, and <span style="font-weight:bold; font-style:italic">Bold Italic</span> are also available.
        </div>

        <div id="first" class="std-font serif">
            Optima is used from a font style from the system fonts.<br />
            Optima <b>Bold</b>, <i>Italic</i>, and <span style="font-weight:bold; font-style:italic">Bold Italic</span> are available.
        </div>

        <div id="first" class="std-font avenir">
            Avenir Next Condensed is used from a font style from the system fonts.<br />
            Avenir Next Condensed <b>Bold</b>, <i>Italic</i>, and <span style="font-weight:bold; font-style:italic">Bold Italic</span> are available.
        </div>

        <div id="first" class="std-font none">
            Fonts that are not avialable can use the fallback method.<br />
            They should also apply to <b>Bold</b>, <i>Italic</i>, and <span style="font-weight:bold; font-style:italic">Bold Italic</span> styles.
        </div>
    </body>
    </html>

.. image:: images/drawingfontsSystem.png

As the font is set to inherit, all child text components will use the specified font of the parent. If the
font is changed, then all children will use the new font.

.. note:: .woff or woff files are not currently supported, but these can be easily converted to their ttf components online. They may be supported in future.

Font face loading
------------------

Along with the standard and system installed fonts, scryber supports the importing and declaration of
custom fonts from specific files.

These can either be relative to the current file, or an absolute url.

This is also a good way of specifying various weights, as scryber (currently) only supports the bold variant. It is on our list of todo's.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>

    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>Fonts loaded directly</title>
        <meta name="author" content="Scryber Team" />
        <!-- 
            link to google fonts API's.
          -->
        <link href="https://fonts.googleapis.com/css2?family=Big+Shoulders+Inline+Display:wght@500;800&amp;display=swap" rel="stylesheet" />
        <style type="text/css">

            /* Open light font file on the local drive */

            @font-face {
                src: url(../../Resources/OpenSans-Light.ttf) format('truetype');
                font-family: 'Open Light';
            }

            /* Long Cang is downloaded from google but is cached locally */

            @font-face {
                font-family: 'Long Cang';
                font-style: normal;
                font-weight: 400;
                src: url(https://fonts.gstatic.com/s/longcang/v5/LYjAdGP8kkgoTec8zkRgrQ.ttf) format('truetype');
            }

            .std-font {
                font-size: 20pt;
                background-color: #AAA;
                padding: 4pt;
                margin-bottom: 10pt;
            }

            /* Setting the classes to the fonts above */

            .sans {
                font-family: 'Open Light', monospace;
            }

            .grafitti {
                font-family: 'Long Cang', serif;
            }

            .broad {
                font-family: 'Big Shoulders Inline Display', sans-serif;
            }
            
        </style>
    </head>
    <body style="padding: 20pt">
        <div id="first" class="std-font sans">
            Open Sans Light is used from a font face declaration.<br />
            As we did not define <b>Bold</b>, <i>Italic</i>, or <span style="font-weight:bold; font-style:italic">Bold Italic</span> they are <u>not</u> available and will fallback.
        </div>

        <div id="first" class="std-font grafitti">
            Long Kang is downloaded from the google fonts api.<br />
            No variations are idetnfied for the <b>bold</b> or <i>italic</i> are available.
        </div>

        <div id="first" class="std-font broad">
            Big shoulders is used from a css file imported from the google fonts.<br />
            It does have a <b>Bold</b> variation, but not <i>Italic</i>.
        </div>

    </body>
    </html>


.. image:: images/drawingfontsStyles.png


.. warning:: The link for the font css from google is not XHTML compliant. The & parameter separator should be escaped to &amp; and the link tag closed '/>'


Text styles and decoration
---------------------------

Along with the bold and italic variants, scryber also supports underlines, strikethrough and overline text rendering features.
As with HTML these are default styles, and can be altered as needed.

* Bold
    * <b></b>
    * <strong></strong>
    * css {font-weight:bold;}
* italic
    * <i></i>
    * <em></em>
    * css {font-style:italic;}
* Underline
    * <u></u>
    * <ins></ins>
    * css { text-decoration:underline;}
* StrikeThrough
    * <strike></strike>
    * <del></del>
    * css { text-decoration: line-through; }
* Overline
    * css { text-decoration: overline; }


As with css text-decoration values can be combined e.g. 'line-through underline' , and the decorations will flow across lines.

Scryber does not (currently) support the text-decoration-color or text-decoration-style.


.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>Fonts decorations</title>
        <meta name="author" content="Scryber Team" />
        <style type="text/css">

            .std-font {
                font-size: 20pt;
                background-color: #AAA;
                padding: 4pt;
                margin-bottom: 10pt;
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            }

            .railway{ text-decoration: overline underline; color: red;}
            
        </style>
    </head>
    <body style="padding: 20pt">
        <div id="first" class="std-font">
            Segoe UI is used from a system font<br />
            <strong>Strong is Bold</strong>, <em>Em(phasis) is Italic</em>.
            <ins>Ins(ert) is underlined</ins> and <del>del(eted) is strike through.</del>
        </div>

        <div id="first" class="std-font">
            The decorations can be combined by multiple tags<br />
            Such as <b><em><u>Bold italic underlined</u></em></b>
            or by the style <span class="railway" >over and under lined.</span>
        </div>

        <div class="std-font" style="font-weight:bold; text-decoration: underline;" >
            The decoration will flow down into child tags.
            <div style="margin:0 30pt 0 30pt; font-size:12pt">And any inner content can
                <span style="font-weight:normal; text-decoration: overline;">override the settings</span>
                as needed.
            </div>
        </div>
    </body>
    </html>


.. image:: images/drawingfontsDecoration.png




Line Leading
-------------

The leading is the height of the lines including ascenders and descenders. 
The default is set by the font (usually about 120% of the font size), but can be manually adjusted as needed.

Inline components will ignore the block level style for leading.

The leading value is a unit value rather than a relative percent.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >

    <Styles>
        
        <!-- Add a style to images -->
        <styles:Style applied-type="doc:Div" applied-class="std-font" >
        <styles:Background color="#AAA"/>
        <styles:Padding all="4pt"/>
        <styles:Margins bottom="10pt" />
        </styles:Style>

        <!-- Alter the default bold component -->
        <styles:Style applied-type="doc:B">
        <styles:Font size="25pt" italic="true"/>
        </styles:Style>

    </Styles>
    <Pages>
        
        <!-- Setting the font on the page, rather than at each level. -->
        <doc:Page styles:padding="10" styles:font-family="Segoe UI" >
        <Content>
            <doc:Div styles:column-count="3" styles:font-size="10pt">

                <doc:Div styles:class="std-font" >
                    Segoe UI in 10pt font size with the default
                    leading used on each line of the paragraph. So the text looks well spaced and consistent.
                </doc:Div>
                <doc:ColumnBreak/>
                
                <doc:Div styles:class="std-font" styles:text-leading="20pt">
                    Segoe UI in 10pt font size with the leading increased to 20pt
                    on each line of the paragraph. So the text is more spaced out.
                </doc:Div>
                <doc:ColumnBreak/>
                <doc:Div styles:class="std-font"  styles:text-leading="7pt">
                    Segoe UI in 10pt font size with the leading reduced to 8pt
                    on each line of the paragraph. It is not an error for the letters to collide.
                </doc:Div>

            </doc:Div>

            <doc:Div styles:class="std-font"  styles:text-leading="17pt">
                Even using various 
                <doc:Span styles:font-size="30" styles:font-family="Comic Sans MS">font sizes and families</doc:Span>
                will not affect the fixed size of the leading, 
                but may impact the <doc:B>baselines of the content</doc:B>.
            </doc:Div>
        </Content>
        </doc:Page>
    </Pages>
    
    </doc:Document>

.. image:: images/drawingfontsLeading.png

Character and Word Spacing
--------------------------

With scryber the character and word spacing is supported at the style definition level (not on the component attributes). 
They are less frequently used, but can help in adjusting fonts that are too narrow at a particular size, or for graphical effect.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >

    <Styles>
        
        <!-- Add a style to images -->
        <styles:Style applied-type="doc:Div" applied-class="std-font" >
            <styles:Background color="#AAA"/>
            <styles:Padding all="4pt"/>
            <styles:Margins bottom="10pt" />
        </styles:Style>

        <!-- Alter the default bold component -->
        <styles:Style applied-type="doc:B">
            <styles:Font size="20pt" italic="true"/>
            <!-- Adding character and word spacing too -->
            <styles:Text char-spacing="5pt" word-spacing="10pt"/>
        </styles:Style>

        <styles:Style applied-class="narrow" >
            <styles:Text char-spacing="-0.5pt"/>
        </styles:Style>

        <styles:Style applied-class="wide" >
            <styles:Text char-spacing="1.5pt" leading="15pt"/>
        </styles:Style>

        <styles:Style applied-class="wide-word" >
            <styles:Text char-spacing="0" word-spacing="8pt" />
        </styles:Style>
    </Styles>
    <Pages>
        
        <!-- Setting the font on the page, rather than at each level. -->
        <doc:Page styles:padding="10" styles:font-family="Segoe UI" >
        <Content>
            <doc:Div styles:column-count="3" styles:font-size="10pt">
                <doc:Div styles:class="std-font narrow" >
                    Segoe UI in 10pt font size with the default
                    leading used on each line of the paragraph. But the character spacing is reduced by 0.5 points.
                </doc:Div>
                <doc:ColumnBreak/>
                <doc:Div styles:class="std-font wide">
                    Segoe UI in 10pt font size with the leading increased to 15pt
                    on each line of the paragraph. The character spacing is also
                    set to an extra 1.5 points.
                </doc:Div>
                <doc:ColumnBreak/>
                <doc:Div styles:class="std-font wide-word" >
                    Segoe UI in 10pt font size with the leading and character space normal, but the word
                    spacing increased by 5 points. It should continue to flow nicely onto multiple lines.
                </doc:Div>
            </doc:Div>

            <doc:Div styles:class="std-font wide" styles:text-leading="35pt" >
                Even using various 
                <doc:Span styles:font-size="30" styles:font-family="Comic Sans MS">font sizes and families</doc:Span>
                will maintain the character and 
                word spacing that <doc:B>has been applied.</doc:B>
            </doc:Div>
        </Content>
        </doc:Page>
    </Pages>
    
    </doc:Document>


.. image:: images/drawingfontsSpacing.png


Multi-byte Characters
----------------------

Scryber supports multi-byte characters, anywhere in the document. Whether that is only a couple of characters, or whole paragraphs.

.. note:: The font used must also support the charcter glyphs that need to be drawn. If they are not in the font, then they cannot be rendered by the reader.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >

    <Styles>
        
        <!-- Add a style to images -->
        <styles:Style applied-type="doc:Div" applied-class="std-font" >
            <styles:Background color="#AAA"/>
            <styles:Padding all="4pt"/>
            <styles:Margins bottom="10pt" />
        </styles:Style>

        <styles:Style applied-class="wide" >
            <styles:Text char-spacing="5pt"/>
        </styles:Style>
    </Styles>
    <Pages>
        
        <!-- Setting the font to a chinese traditional. -->
        <doc:Page styles:padding="10" styles:font-family="Microsoft JhengHei UI" >
        <Content>
            <doc:Div styles:column-count="3" styles:font-size="14pt" styles:height="150pt">
            記第功際被治年待中所正向持。害供雪指載載道表職渉彩明文界早琶。本要逆使健貿市執多格紙録指璧。
            高規要来広北的夜競語進文務配界重報史。松強約協交均刊後旅昼毎民御年必荒人稿線塁。
            代細募問毒会順債著用育探重早価時職。
            生出型掲事険市映女員雑誌賞盆山注医王放北。真催英落業投提協金策結状士社更観。
            好角野成集顧演委事被対断陣前考武。<doc:Br/>
            <doc:Br/>
            <doc:Span styles:font-bold="true">
            意能自至診発億間誕作業丹製橋内。大起阪企昌重週向入村着体首産優深男米。三外高本墨度投右未掲玲予伏望着。
            経鈴向表田週健会断縄駅夜長。受稿照主著運国果暮治待困。極面五遺間方天質聞査違武梨整許削武祉。
            合第面歳多料夜産選禁連聞旅可章勝策高十近。車氏意技済覇対思数祭町検開面玲術道給提座。
            泉南追夜育挙性成卵要本物似界知減塾奈傷。</doc:Span>
            </doc:Div>
            <!-- mixed character sets, with leading and spacing -->
            <doc:Div styles:class="wide" styles:text-leading="35pt" >
            We can intermix the characters 記第功際被治年待中所正向持。害供雪指載載道表職渉彩明文界早琶。本要逆使健貿市執多格紙録指璧。
            高規要来広北的夜競語進文務配界重報史。松強約協交均刊後旅昼毎民御年必荒人稿線塁。
            <doc:Span styles:font-family="Segoe UI"  >代細募問毒会順債著用育探重早価時職。
            But the font must contain the glyphs.</doc:Span> 
            </doc:Div>
        </Content>
        </doc:Page>
    </Pages>
    
    </doc:Document>


.. image:: images/drawingfontsUnicode.png


.. note:: Due to the size of most unicode font files with thousands of glyphs, using and embedding a unicode font can dramatically increase the
          size of the pdf file. The example above came in at 23Mb without any images. Beware!

Right to Left
---------------

Scryber doesn't currently support Right to left (or vertical) typography. At the moment we have have not seen it done 
anywhere due to limitiations in postscript and the pdf document. But we will keep trying.









