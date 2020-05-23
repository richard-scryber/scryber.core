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
using System.Linq;
using System.Text;

namespace Scryber
{
    /// <summary>
    /// Defines a complete list of all the data items
    /// in a data source schema for a specific path.
    /// Allows runtime analysis and evaluation of a data source.
    /// </summary>
    public class PDFDataSchema
    {
        //
        // properties
        //

        #region public string RootPath { get; private set; }

        /// <summary>
        /// Gets the path from the root where this data schema refers to.
        /// </summary>
        public string RootPath { get; private set; }

        #endregion

        #region public PDFDataItemCollection Items { get; private set; }

        /// <summary>
        /// Gets the collection of data items in this schema
        /// </summary>
        public PDFDataItemCollection Items { get; private set; }

        #endregion

        //
        // .ctor
        //

        #region public PDFDataSchema(string rootpath, PDFDataItemCollection items)

        /// <summary>
        /// Creates a new instance of the data schema with the specified root path and items
        /// </summary>
        /// <param name="rootpath"></param>
        /// <param name="items"></param>
        public PDFDataSchema(string rootpath, PDFDataItemCollection items)
        {
            this.RootPath = rootpath;
            this.Items = items;
        }

        #endregion



        public static string CombineElementNames(params string[] names)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in names)
            {
                if (sb.Length > 0)
                    sb.Append("/");
                sb.Append(s);
            }

            return sb.ToString();            
        }
    }

    /// <summary>
    /// Represents a single chunck of selectable data returned from a data source.
    /// This can either be an item with inner child items, or a discreet value source.
    /// </summary>
    public class PDFDataItem
    {

        #region public string FullPath { get; private set; }

        /// <summary>
        /// Gets the full path to the data item
        /// </summary>
        public string FullPath { get; private set; }

        #endregion

        #region public string RelativePath { get; private set; }

        /// <summary>
        /// Gets the relative path to the data item from it's container
        /// </summary>
        public string RelativePath { get; private set; }

        #endregion

        #region public string Name { get; private set; }

        /// <summary>
        /// Gets the human readable name for this data item
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region public string Title { get; private set; }

        /// <summary>
        /// Gets or sets the display title of this column.
        /// </summary>
        public string Title { get; private set; }

        #endregion

        #region public Type DataType { get; private set; }

        /// <summary>
        /// Gets the underlying data type for this data item
        /// </summary>
        private DataType _dataType;

        public virtual string DataTypeName { get { return _dataType == DataType.Unknown ? "" : _dataType.ToString(); } }

        public DataType DataType { get { return _dataType; } }

        #endregion

        #region public System.Xml.XmlNodeType NodeType { get; private set; }

        /// <summary>
        /// Defines the node type of this item in the xml (usually within the range Attribute, Element, Text, CDATA)
        /// </summary>
        public System.Xml.XmlNodeType NodeType { get; private set; }

        #endregion

        #region public bool HasChildren { get; private set; }

        /// <summary>
        /// Returns true if this data item has inner child items
        /// </summary>
        public bool HasChildren { get { return null != _children && _children.Count > 0; } }

        #endregion

        public bool HasAttributes { get { return null != _attrs && _attrs.Count > 0; } }

        private PDFDataItemCollection _attrs;

        public IEnumerable<PDFDataItem> Attributes
        {
            get
            {
                return _attrs;
            }
            set
            {
                if (null == value)
                {
                    _attrs = null;
                }
                else
                {
                    _attrs = new PDFDataItemCollection(value);
                }
            }
        }

        #region public PDFDataItemCollection Children { get; private set; }

        private PDFDataItemCollection _children;

        /// <summary>
        /// Gets the inner collection of child data items. 
        /// Will be null if this instance has no children
        /// </summary>
        public PDFDataItemCollection Children { get { return _children; } set { _children = value; } }

        #endregion

        //
        // .ctor
        //

        #region public PDFDataItem(string fullpath, string relativepath, string name, string title, System.Xml.XmlNodeType nodeType, Type datatype)

        /// <summary>
        /// Creates a new simple data item (with no children)
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="name"></param>
        /// <param name="datatype"></param>
        public PDFDataItem(string fullpath, string relativepath, string name, string title, System.Xml.XmlNodeType nodeType, DataType datatype)
            : this(fullpath, relativepath, name, title, nodeType, datatype, null)
        {
        }

        #endregion

        #region public PDFDataItem(string path, string name, PDFDataItemCollection children)

        /// <summary>
        /// Creates a new container data item (has children and the DataType is Array)
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="name"></param>
        /// <param name="children"></param>
        public PDFDataItem(string fullpath, string relativepath, string name, PDFDataItemCollection children)
            : this(fullpath, relativepath, name, name, System.Xml.XmlNodeType.Element, Scryber.DataType.Array, children)
        {
        }

        #endregion

        #region protected PDFDataItem(string path, string name, string title, System.Xml.XmlNodeType nodeType, Type datatype, bool hasChildren, PDFDataItemCollection children)

        /// <summary>
        /// Protected constructor that initializes all the parameters.
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="name"></param>
        /// <param name="datatype"></param>
        /// <param name="hasChildren"></param>
        /// <param name="children"></param>
        protected PDFDataItem(string fullpath, string relativepath, string name, string title, System.Xml.XmlNodeType nodeType, DataType datatype, PDFDataItemCollection children)
        {
            if (string.IsNullOrEmpty(fullpath))
                throw new ArgumentNullException("path");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.FullPath = fullpath;
            this.RelativePath = relativepath;
            this.Name = name;
            this.Title = title;
            this._dataType = datatype;
            this.NodeType = nodeType;
            this.Children = children;
        }

        #endregion

        #region public override string ToString()

        /// <summary>
        /// Overrides the base implmentation to return a string representing this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name + " (" + this.DataTypeName + ")";
        }

        #endregion
    }

    /// <summary>
    /// Represents a read only collection of data items (either as children of 
    /// another data item or as the root items in the schema)
    /// </summary>
    public class PDFDataItemCollection : IEnumerable<PDFDataItem>
    {

        #region ivars

        private List<PDFDataItem> _inneritems;

        #endregion

        //
        // properties
        //

        #region public int Count {get;}

        /// <summary>
        /// Gets the number of items in this collection
        /// </summary>
        public int Count
        {
            get { return _inneritems.Count; }
        }

        #endregion

        #region public bool IsReadOnly {get;}

        /// <summary>
        /// Returns true - as this collection IS readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        #endregion

        #region public PDFDataItem this[int index] {get;}

        /// <summary>
        /// Gets the data item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PDFDataItem this[int index]
        {
            get { return _inneritems[index]; }
        }

        #endregion

        #region public PDFDataItem this[string path] {get;}

        /// <summary>
        /// Gets the data item with the specified path (case sensitive)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public PDFDataItem this[string path]
        {
            get
            {
                for (int i = 0; i < _inneritems.Count; i++)
                {
                    if (string.Equals(_inneritems[i].FullPath, path, StringComparison.Ordinal))
                        return _inneritems[i];
                }
                return null;
            }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFDataItemCollection(IEnumerable<PDFDataItem> items)

        /// <summary>
        /// Creates a new instace of the PDFDataItemCollection with a read only collection of items specified
        /// </summary>
        /// <param name="items">The PDFDataItes in this collection</param>
        public PDFDataItemCollection(IEnumerable<PDFDataItem> items)
        {
            this._inneritems = new List<PDFDataItem>(items);
        }

        #endregion

        //
        // methods
        //

        #region public void CopyTo(PDFDataItem[] array, int arrayIndex)

        /// <summary>
        /// Copies the items in this collection to an array starting at the specified index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(PDFDataItem[] array, int arrayIndex)
        {
            _inneritems.CopyTo(array, arrayIndex);
        }

        #endregion

        #region public IEnumerator<PDFDataItem> GetEnumerator()

        /// <summary>
        /// Returns an enumerator for the data item collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<PDFDataItem> GetEnumerator()
        {
            return _inneritems.GetEnumerator();
        }

        #endregion

        #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

}
