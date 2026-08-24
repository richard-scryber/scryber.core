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
        SubmitForm,
        ResetForm,
        Other
    }

    /// <summary>
    /// Whether a form field's default click behaviour is to submit or reset its Form, matching
    /// HTML's own &lt;button&gt;/&lt;input type="submit"|"reset"&gt; default actions.
    /// </summary>
    public enum FormSubmitBehavior
    {
        None = 0,
        Submit,
        Reset
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

    /// <summary>
    /// The PDF /Ff field-flag bits (PDF spec tables 221/226/227). Values are the field type's
    /// own bit position, not sequential - each PDF field type (button/text/choice) interprets
    /// bits beyond the first 3 (common) differently, so they legitimately overlap across types.
    /// </summary>
    [Flags]
    public enum FormFieldOptions : int
    {
        None = 0,
        //Common to all field types
        ReadOnly = 1,
        Required = 2,
        NoExport = 4,

        //Text fields
        MultiLine = 4096,
        Password = 8192,
        File = 1048576, //FileSelect
        DoNotScroll = 8388608,
        Comb = 16777216,

        //Button fields
        NoToggleToOff = 16384,
        Radio = 32768,
        Pushbutton = 65536,

        //Choice fields
        Combo = 131072,
        Edit = 262144, //editable combo box
        Sort = 524288,
        Multiselect = 2097152,

        //Shared between Text and Choice fields
        DoNotSpellCheck = 4194304,
    }

    public enum FormInputFieldType : int
    {
        Text = 0,
        Button,
        Choice,
        Signature,
        Hidden
    }

    public enum FormButtonFieldType : int
    {
        PushButton = 0,
        CheckBox,
        Radio
    }

    public enum FormFieldAppearanceState
    {
        Normal,
        Over,
        Down,
        On,
        Off
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

    public enum FrameOverlayRepeat
    {
        None,
        First,
        Once,
        Last,
        Repeat
    }

    public enum TextLengthAdjustType
    {
        Spacing,
        SpacingAndGlyphs
    }

}
