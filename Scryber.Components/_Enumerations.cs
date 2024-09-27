/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber
{
	
    public enum GraphicColorOp
    {
        Fill,
        Stroke,
        None
    }

    public enum AggregationType
    {
        Sum,
        Count,
        Average,
        Min,
        Max,
        Unknown
    }


    public enum LayoutBreakType
    {
        Page,
        Column,
        Line
    }

   

    public enum OutlineFit
    {
        FullPage,
        PageWidth,
        PageHeight,
        BoundingBox
    }

    public enum AnnotationHighlight
    {
        Push,
        Invert,
        InvertBorder,
        None
    }

    
    public enum LinkAction
    {
        Undefined = 0,
        Uri,
        Destination,
        ExternalDestination,
        Launch,
        NextPage,
        PrevPage,
        FirstPage,
        LastPage,
        Other
    }

    public enum PageDisplayMode
    {
        Undefined,
        None,
        Outlines,
        Thumbnails,
        FullScreen,
        Attachments
    }

    public enum PageLayoutMode
    {
        Undefined,
        SinglePage,
        TwoPageLeft,
        TwoPageRight,
        OneColumn,
        TwoColumnLeft,
        TwoColumnRight
    }

    public enum DataAutoBindContent
    {
        None,
       // Elements,
       // Attributes,
        All
    }

    

    public enum FieldLayoutType
    {
        NextTo,
        Above,
        Inline,
        ValueOnly
    }

    public enum UrlDisplayType
    {
        Text,
        Link,
        Image
    }


    // form fields

    public enum FormFieldOptions : int
    {
        None = 0,
        ReadOnly = 1,
        Required = 2,
        NoExport = 4,
        MultiLine = 4096, //8192,
        Password = 8192, //16384,
        File = 1048576
    }

    public enum FormInputFieldType : int
    {
        Text = 0,
        Button,
        Choice,
        Signature
    }

    public enum FormButtonFieldType : int
    {

    }

    public enum FormFieldAppearanceState
    {
        Normal,
        Over,
        Down
    }

    public enum DocumentExecMode
    {
        Immediate,
        Asyncronous,
        Phased
    }


    public enum DataContentAction
    {
        Append,
        PrePend,
        Replace
    }


    public enum TextLengthAdjustType
    {
        Spacing,
        SpacingAndGlyphs
    }

}
