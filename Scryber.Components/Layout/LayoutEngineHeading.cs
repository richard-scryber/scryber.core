using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.Layout
{
    /// <summary>
    /// Implements the layout engine for a Heading component
    /// </summary>
    public class LayoutEngineHeading : LayoutEnginePanel
    {

        public LayoutEngineHeading(PDFHeadingBase heading, IPDFLayoutEngine parent)
            : base(heading, parent)
        {

        }

        protected PDFHeadingBase Heading
        {
            get { return this.Component as PDFHeadingBase; }
        }

        protected override void DoLayoutComponent()
        {
            bool restoreText = false;
            ListNumberingGroupStyle style = this.FullStyle.GetValue(PDFStyleKeys.ListNumberStyleKey, ListNumberingGroupStyle.None);
            string grpname = this.FullStyle.GetValue(PDFStyleKeys.ListGroupKey, string.Empty);

            if (style != ListNumberingGroupStyle.None && !string.IsNullOrEmpty(grpname))
            {
                PDFHeadingNumbers headingNumbers = this.GetOrCreateHeadingNumbersFromDocument();

                string listnumber = headingNumbers.PushHeading(this.Heading.HeadingDepth, grpname , this.FullStyle);
                Heading.SetHeadingNumber(listnumber);
                restoreText = true;
            }
            
            base.DoLayoutComponent();

            if (restoreText)
                this.Heading.SetHeadingNumber(string.Empty);
        }


        private const string HeadingNumbersKey = "__scryber.headings";


        private PDFHeadingNumbers GetOrCreateHeadingNumbersFromDocument()
        {
            object found = this.Heading.Document.Items[HeadingNumbersKey];
            if (null == found)
            {
                found = new PDFHeadingNumbers();
                this.Heading.Document.Items[HeadingNumbersKey] = found;
            }
            return (PDFHeadingNumbers)found;
        }

        //
        // Heading numbering logic
        //
        // With H1 we reset all lower registered headings so they start at 1 again.
        // If we then have an H2 we 


        /// <summary>
        /// Private class that stores the heading numbers
        /// </summary>
        private class PDFHeadingNumbers
        {
            /// <summary>
            /// The value for a groug index that has not been registered
            /// </summary>
            private const string UnregisteredGroup = null;
            private const PDFStyle UnregisteredStyle = null;

            /// <summary>
            /// A list of all the names of the groups registered based on their index
            /// </summary>
            private List<string> _registeredGroups = new List<string>();
            private List<PDFStyle> _registeredStyles = new List<PDFStyle>();

            
            /// <summary>
            /// The headings list numbering options
            /// </summary>
            private PDFListNumbering _numbering = new PDFListNumbering();

            /// <summary>
            /// A stack of the route of heading indexes
            /// </summary>
            private List<int> _currRoute = new List<int>();

            public string PushHeading(int index, string grpName, PDFStyle style)
            {

                //We add nulls in to the list of group names up to the index
                //So we have the complete list by index

                while (_registeredGroups.Count <= index)
                {
                    _registeredGroups.Add(UnregisteredGroup);
                    _registeredStyles.Add(UnregisteredStyle);
                }

                //Store the group and style in the lists at the index.

                _registeredGroups[index] = grpName;
                _registeredStyles[index] = style;

                // Reset all the known groups that are of a higher index (smaller headings)

                for (int i = index + 1; i < _registeredGroups.Count; i++)
                {
                    if (_registeredGroups[i] != UnregisteredGroup)
                    {
                        PDFListNumberGroup grp = _numbering.GetGroup(_registeredGroups[i]);
                        grp.Reset();
                    }
                }

                // remove the groups that are in the route but of a higher index

                while (_currRoute.Count > 0 && _currRoute[_currRoute.Count-1] >= index)
                {
                    _currRoute.RemoveAt(_currRoute.Count - 1);
                }
                
                // put the index on the bottom od the route
                _currRoute.Add(index);

                // clear out the list numbering

                while (_numbering.HasCurrentGroup)
                {
                    _numbering.PopGroup();
                }

                

                // and build it back

                foreach (int exist in _currRoute)
                {
                    string grpname = _registeredGroups[exist];
                    PDFStyle grpstyle = _registeredStyles[exist];
                    _numbering.PushGroup(grpname, grpstyle);
                }

                // get the full number style value

                string value =_numbering.Increment();


                //and return the value

                return value;
            }
        }
        

    }
}
