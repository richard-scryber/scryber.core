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
using System.Collections;
using System.Text;
using Scryber.Components;
using System.Drawing.Text;

namespace Scryber.Data
{
    [PDFParsableComponent("ForEach")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_dataforeach")]
    public class ForEach : DataTemplateComponent
    {
        private int _start, _count, _step;

        /// <summary>
        /// Gets or sets the (zero based) index of the first item to be generated
        /// </summary>
        [PDFAttribute("start-index")]
        [PDFDesignable("Start Index", Priority = 3, Category ="Data", Type ="Number")]
        public virtual int StartIndex
        {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of items that will be generated.
        /// </summary>
        [PDFAttribute("count")]
        [PDFDesignable("Max. Count", Priority = 3, Category = "Data", Type = "Number")]
        public virtual int MaxCount
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// Gets or sets the step for the loop. Default is one for every item to be generated.
        /// </summary>
        [PDFAttribute("step")]
        [PDFDesignable("Step", Priority = 3, Category = "Data", Type = "Number")]
        public virtual int Step
        {
            get { return _step; }
            set { _step = value; }
        }



        #region public object Value {get;set;}

        /// <summary>
        /// Gets the root bound value to loop over.
        /// </summary>
        [PDFAttribute("value", BindingOnly = true)]
        public object Value
        {
            get;
            set;
        }

        #endregion

        #region public virtual IPDFTemplate Template {get;}

        private IPDFTemplate _template;

        /// <summary>
        /// Gets or sets the IPDFTemplate used to instantiate child Components
        /// </summary>
        [PDFTemplate()]
        [PDFElement("Template")]
        [PDFAttribute("template")]
        public virtual IPDFTemplate Template
        {
            get { return _template; }
            set { _template = value; }
        }

        [PDFAttribute("cache-styles")]
        public virtual bool CacheStyles
        {
            get;
            set;
        }

        public bool HasTemplate
        {
            get { return null != this.Template; }
        }

        #endregion

        #region public virtual IPDFTemplate SeparatorTemplate

        private IPDFTemplate _septemplate;

        /// <summary>
        /// Gets or sets the IPDFTemplate used to instantiate inbetween each of the created templates
        /// </summary>
        [PDFTemplate()]
        [PDFElement("Separator-Template")]
        public virtual IPDFTemplate SeparatorTemplate
        {
            get { return _septemplate; }
            set { _septemplate = value; }
        }

        public bool HasSeparator
        {
            get { return null != _septemplate; }
        }

        #endregion

        public ForEach()
            : this(PDFObjectTypes.NoOp)
        {
        }

        protected ForEach(PDFObjectType type)
            : base(type)
        {
            _start = 0;
            _count = int.MaxValue;
            _step = 1;
            CacheStyles = false;
        }


        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);
        }


        protected override IPDFTemplate GetTemplateForBinding(PDFDataContext context, int index, int count)
        {
            if(this.Template is IPDFDataTemplateGenerator)
            {
                ((IPDFDataTemplateGenerator)this.Template).DataStyleStem = this.DataStyleIdentifier;
                ((IPDFDataTemplateGenerator)this.Template).UseDataStyleIdentifier = this.CacheStyles;
            }
            return this.Template;
        }

        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
        {
            object data = null;
            bool hasdata = false;
            System.Xml.IXmlNamespaceResolver resolver = context.NamespaceResolver;

            //If we have a specific source then we use that otherwise we use the current datasource
            IPDFDataSource dataSource;

            if (this.HasObjectValue(context, out data))
            {
                hasdata = true;
                dataSource = null;

                if(data is System.Xml.XmlElement)
                {
                    Data.XMLDataSource source = new XMLDataSource();
                    this.Document.DataSources.Add(source);

                    source.XmlNodeData = (System.Xml.XmlElement)data;
                    data = source.Select(this.SelectPath, context);
                    dataSource = source;
                }
                else if(data is System.Xml.XPath.XPathNavigator)
                {
                    Data.XMLDataSource source = new XMLDataSource();
                    this.Document.DataSources.Add(source);

                    source.XPathNavData = (System.Xml.XPath.XPathNavigator)data;
                    data = source.Select(this.SelectPath, context);
                    dataSource = source;
                }                    
            }
            else if (this.HasAssignedDataSourceComponent(context, out dataSource))
            {
                data = dataSource.Select(this.SelectPath, context);
                if (null == data)
                    context.TraceLog.Add(TraceLevel.Warning, "Data For Each", string.Format("NULL data was returned for the path '{0}' on the PDFForEach component {1} with datasource {2}", this.SelectPath, this.ID, dataSource.ID));
                hasdata = true;
            }
            else if (string.IsNullOrEmpty(this.SelectPath) == false)
            {
                dataSource = context.DataStack.Source;
                data = dataSource.Select(this.SelectPath, context.DataStack.Current, context);
                if (null == data)
                    context.TraceLog.Add(TraceLevel.Warning, "Data For Each", string.Format("NULL data was returned for the path '{0}' on the PDFForEach component {1} with data source {2}", this.SelectPath, this.ID, dataSource.ID));
                hasdata = true;
            }
            else if(context.DataStack.HasData)
            {
                data = context.DataStack.Current;
                dataSource = context.DataStack.Source;
            }
            else
            {
                hasdata = false;
            }


            if (hasdata)
            {
                context.DataStack.Push(data, dataSource);
            }
            
            base.DoDataBind(context, includeChildren);

            if (hasdata)
            {
                context.DataStack.Pop();
            }

            context.NamespaceResolver = resolver;
        }

        protected override IEnumerator CreateEnumerator(IEnumerable enumerable)
        {
            if(enumerable is System.Xml.XPath.XPathNodeIterator)
            {
                System.Xml.XPath.XPathNodeIterator itter = (System.Xml.XPath.XPathNodeIterator)enumerable;
                if (this.StartIndex == 0 && this.MaxCount == int.MaxValue && this.Step == 1)
                    return new XPathDefaultEnumerator(itter);
                else
                    return new XPathRangeEnumerator(itter, this.StartIndex, this.MaxCount, this.Step);
            }
            if (this.StartIndex == 0 && this.MaxCount == int.MaxValue && this.Step == 1)
                return base.CreateEnumerator(enumerable);
            else
                return new RangeEnumerator(enumerable.GetEnumerator(), this.StartIndex, this.MaxCount, this.Step);
        }

        protected bool HasObjectValue(PDFContextBase context, out object data)
        {
            data = this.Value;
            return null != data;
        }

        #region private class RangeEnumerator : IEnumerator

        /// <summary>
        /// Implements enumerating over a range (with an optional step)
        /// </summary>
        private class RangeEnumerator : IEnumerator
        {
            private int _currindex;
            private IEnumerator _root;
            private int _start;
            private int _count;
            private int _step;

            

            public object Current
            {
                get { return this._root.Current; }
            }

            public RangeEnumerator(IEnumerator all, int start)
                : this(all, start, int.MaxValue)
            {
            }

            public RangeEnumerator(IEnumerator all, int start, int count)
                : this(all, start, count, 1)
            { }

            public RangeEnumerator(IEnumerator all, int start, int count, int step)
            {
                this._root = all;
                this._start = start;
                this._count = count;
                this._step = step;
                this._currindex = -1;
            }

            public bool MoveNext()
            {
                bool moved = false;
                do
                {
                    int stepped = 0;
                    do
                    {
                        moved = this._root.MoveNext();
                        stepped++;
                        this._currindex++;

                    } while ((_currindex > _start) && moved && stepped < this._step);



                } while (moved && (_currindex < _start));

                if (_currindex >= (_count + _start))
                    moved = false;
                return moved;
            }

            public void Reset()
            {
                this._currindex = -1;
                this._root.Reset();
            }

        }

        #endregion

        #region private class XPathDefaultIterator : IEnumerator

        private class XPathDefaultEnumerator : IEnumerator
        {
            private System.Xml.XPath.XPathNodeIterator _inner;
            private bool _isReset;

            public XPathDefaultEnumerator(System.Xml.XPath.XPathNodeIterator itter)
            {
                _inner = itter;
                _isReset = false;
            }



            public object Current
            {
                get { return _inner.Current; }
            }

            public bool MoveNext()
            {
                if (_isReset)
                    throw new InvalidOperationException("Cannot enumerate over an XPathNodeIterator more than once - it is forward only");
                return _inner.MoveNext();
            }

            public void Reset()
            {
                _isReset = true;
            }
        }

        #endregion

        #region private class XPathRangeEnumerator : IEnumerator

        /// <summary>
        /// Implements enumerating over a range (with an optional step)
        /// </summary>
        private class XPathRangeEnumerator : IEnumerator
        {
            private int _currindex;
            private System.Xml.XPath.XPathNodeIterator _root;
            private int _start;
            private int _count;
            private int _step;
            private const int IndexAfterReset = -100;


            public object Current
            {
                get { return this._root.Current; }
            }

            public XPathRangeEnumerator(System.Xml.XPath.XPathNodeIterator all, int start)
                : this(all, start, int.MaxValue)
            {
            }

            public XPathRangeEnumerator(System.Xml.XPath.XPathNodeIterator all, int start, int count)
                : this(all, start, count, 1)
            { }

            public XPathRangeEnumerator(System.Xml.XPath.XPathNodeIterator all, int start, int count, int step)
            {
                this._root = all;
                this._start = start;
                this._count = count;
                this._step = step;
                this._currindex = -1;
            }

            public bool MoveNext()
            {
                if (_currindex == IndexAfterReset)
                    throw new InvalidOperationException("Cannot enumerate over an XPathNodeIterator more than once - it is forward only.");

                if (_currindex > (_count + _start))
                    return false;

                bool moved = false;
                do
                {
                    int stepped = 0;
                    do
                    {
                        moved = this._root.MoveNext();
                        stepped++;
                        this._currindex++;

                    } while ((_currindex > _start) && moved && stepped < this._step);



                } while (moved && (_currindex < _start));

                if (_currindex >= (_count + _start))
                    moved = false;
                return moved;
            }

            public void Reset()
            {
                this._currindex = IndexAfterReset;
            }

        }

        #endregion

    }
}
