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
using Scryber.Native;

namespace Scryber.Components
{
    /// <summary>
    /// Contains the information about the document
    /// </summary>
    public class PDFDocumentInfo : IPDFBindableComponent
    {
        #region ivars

        private string _title;
        private string _author;
        private string _subject;
        private string _keywords;
        private string _creator = Const.ScryberProducer;
        private string _producer = Const.ScryberProducer;
        private DateTime _creationdate = DateTime.Now;
        private DateTime _moddate = DateTime.Now;
        private object _trapping;
        private PDFDocumentInfoExtraCollection _extras;

        #endregion

        //
        // element properties
        //

        #region public string Title {get;set;}

        /// <summary>
        /// Gets or sets the set of title associated with this document
        /// </summary>
        [PDFElement("Title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        #endregion

        #region public string Author {get;set;}

        /// <summary>
        /// Gets or sets the set of author associated with this document
        /// </summary>
        [PDFElement("Author")]
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        #endregion

        #region public string Subject {get;set;}

        /// <summary>
        /// Gets or sets the set of subject associated with this document
        /// </summary>
        [PDFElement("Subject")]
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        #endregion

        #region public string Keywords {get;set;}

        /// <summary>
        /// Gets or sets the set of keywords associated with this document
        /// </summary>
        [PDFElement("Keywords")]
        public string Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }

        #endregion

        //
        // attribute properties
        //

        #region public string Creator {get;set;}

        /// <summary>
        /// Gets the Creator for this document.
        /// </summary>  
        [PDFAttribute("creator")]
        public string Creator
        {
            get { return _creator; }
            set
            {
                _creator = value;
            }
        }

        #endregion

        #region public string Producer {get;set;}

        /// <summary>
        /// Gets the producer for this document.
        /// </summary>
        [PDFAttribute("producer")]
        public string Producer
        {
            get { return _producer; }
            set
            {
                _producer = value;
            }
        }

        #endregion

        #region public DateTime CreationDate {get;set;}

        /// <summary>
        /// Gets or sets the last modified date for the document.
        /// A value of DateTime.MinValue represents a non-value. The default is the current date time
        /// </summary>
        [PDFAttribute("created-date")]
        public DateTime CreationDate
        {
            get { return _creationdate; }
            set { _creationdate = value; }
        }

        #endregion

        #region public DateTime ModifiedDate {get;set;}

        /// <summary>
        /// Gets or sets the last modified date for the document.
        /// A value of DateTime.MinValue is the default and represents a non-value.
        /// </summary>
        [PDFAttribute("modified-date")]
        public DateTime ModifiedDate
        {
            get { return _moddate; }
            set { _moddate = value; }
        }

        #endregion

        #region public bool Trapped {get; set;} + HasTrapping {get;set;}
        /// <summary>
        /// Gets or sets the Traping flag. 
        /// Returns true if the document has trapping information, or false if it does not (or has not been set).
        /// </summary>
        [PDFAttribute("trapped")]
        public bool Trapped
        {
            get { return (null == _trapping) ? false : (bool)_trapping; }
            set { _trapping = value; }
        }

        /// <summary>
        /// Gets or sets the has trapping information flag.
        /// Returns true if a value has been set (either true or false).
        /// </summary>
        public bool HasTrapping
        {
            get { return _trapping is bool; }
            set
            {
                if (value)
                    _trapping = true;
                else
                    _trapping = null;
            }
        }

        #endregion

        //
        // inner collection of extra elements
        //

        #region public PDFDocumentInfoExtraCollection Extras {get;set;} + HasExtras {get;}

        /// <summary>
        /// Gets or sets the collection of extra document info items
        /// </summary>
        [PDFArray(typeof(PDFDocumentInfoExtra))]
        [PDFElement("")]
        public PDFDocumentInfoExtraCollection Extras
        {
            get { return _extras; }
            set { _extras = value; }
        }

        /// <summary>
        /// returns true if this document info has any extra items
        /// </summary>
        public bool HasExtras { get { return _extras != null && _extras.Count > 0; } }

        #endregion


        //
        // methods
        //

        #region public virtual PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer) + Support methods

        /// <summary>
        /// Outputs this Info to the current writer 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFObjectRef inforef = writer.BeginObject("Info");
            writer.BeginDictionary();
            OutputInfoEntry("Title", this.Title, writer);
            OutputInfoEntry("Subject", this.Subject, writer);
            OutputInfoEntry("Author", this.Author, writer);
            OutputInfoEntry("Keywords", this.Keywords, writer);
            OutputInfoEntry("Producer", this.Producer, writer);
            OutputInfoEntry("Creator", this.Creator, writer);
            OutputInfoEntry("CreationDate", this.CreationDate, writer);
            OutputInfoEntry("ModDate", this.ModifiedDate, writer);

            if (this.HasTrapping)
                writer.WriteDictionaryNameEntry("Trapped", this.Trapped.ToString());

            if (this.HasExtras)
            {
                foreach (PDFDocumentInfoExtra extra in this.Extras)
                {
                    OutputInfoEntry(extra.Name, extra.Value, writer);
                }
            }
            writer.EndDictionary();
            writer.EndObject();

            return inforef;
        }



        protected void OutputInfoEntry(string name, DateTime value, PDFWriter writer)
        {
            if (value > DateTime.MinValue)
            {
                writer.BeginDictionaryEntry(name);
                writer.WriteDate(value);
                writer.EndDictionaryEntry();
            }
        }


        protected void OutputInfoEntry(string name, string value, PDFWriter writer)
        {
            if (!string.IsNullOrEmpty(value))
                writer.WriteDictionaryStringEntry(name, value.Trim());
        }

        #endregion

        //
        // IBindableComponent
        //

        public event PDFDataBindEventHandler DataBinding;

        public event PDFDataBindEventHandler DataBound;

        protected virtual void OnDataBinding(PDFDataBindEventArgs args)
        {
            if (null != this.DataBinding)
                this.DataBinding(this, args);
        }

        protected virtual void OnDataBound(PDFDataBindEventArgs args)
        {
            if (null != this.DataBound)
                this.DataBound(this, args);
        }
        
        public void DataBind(PDFDataContext context)
        {
            PDFDataBindEventArgs args = new PDFDataBindEventArgs(context);
            this.OnDataBinding(args);
            this.OnDataBound(args);
            
        }
    }

    /// <summary>
    /// A simple key value class that defines some extra information about the document
    /// </summary>
    [PDFParsableComponent("Extra")]
    public class PDFDocumentInfoExtra
    {
        #region public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of this chunk of information
        /// </summary>
        [PDFAttribute("name")]
        public string Name { get; set; }

        #endregion

        #region public string Value { get; set; }

        /// <summary>
        /// Gets or sets the value of this information
        /// </summary>
        [PDFElement("")]
        public string Value { get; set; }

        #endregion

        //
        //ctors
        //

        #region public PDFDocumentInfoExtra()

        /// <summary>
        /// Creates a new clear extra document info entry
        /// </summary>
        public PDFDocumentInfoExtra()
            : this(string.Empty, string.Empty)
        {
        }

        #endregion

        #region public PDFDocumentInfoExtra(string name, string value)

        /// <summary>
        /// Creates a new extra document info entry with the specified name and value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public PDFDocumentInfoExtra(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        #endregion

    }

    /// <summary>
    /// A list of extra info entries.
    /// </summary>
    /// <remarks>This collection can be accessed by index, but there is also a capability
    /// to get and set string values based on names. e.g. Extras["name"] = "my value";
    /// The list is not indexed as it is not expected to exceed more than 7 entries.</remarks>
    public class PDFDocumentInfoExtraCollection : List<PDFDocumentInfoExtra>
    {

        #region public string this[string name] {get;set;}

        /// <summary>
        /// Gets or sets the value of an extra document info entry 
        /// based on the name provided (case sensitive)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>When retrieving a value - If the extra info is not present then an empty string will be returned.
        /// When setting a value, if the entry does not exist then an new entry will be added.
        /// If the value is null or an empty string, then any existing entry with the specified name will be removed
        /// from the collection.</remarks>
        public string this[string name]
        {
            get
            {
                PDFDocumentInfoExtra extra;
                if (this.TryGetEntryForName(name, out extra))
                    return extra.Value;
                else
                    return string.Empty;
            }
            set
            {
                PDFDocumentInfoExtra extra;
                if (this.TryGetEntryForName(name, out extra))
                {
                    if (string.IsNullOrEmpty(value))
                        this.Remove(extra);
                    else
                        extra.Value = value;
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        extra = new PDFDocumentInfoExtra(name, value);
                        this.Add(extra);
                    }
                }
            }
        }

        #endregion

        #region protected virtual bool TryGetEntryForName(string name, out PDFDocumentInfoExtra extra)

        /// <summary>
        /// returns true if there is an entry in this list with the specified name.
        /// And the extra parameter is set to the matching instance 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        protected virtual bool TryGetEntryForName(string name, out PDFDocumentInfoExtra extra)
        {
            if (this.Count > 0)
            {
                foreach (PDFDocumentInfoExtra item in this)
                {
                    if (string.Equals(item.Name, name))
                    {
                        extra = item;
                        return true;
                    }
                }
            }

            extra = null;
            return false;
        }

        #endregion
    }
}
