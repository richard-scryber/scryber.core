using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.PDF.Parsing
{
    public class PDFParsedResources
    {
        private const string ProcSetKey = "ProcSet";
        private const string FontsKey = "Fonts";
        private const string ExtGStateKey = "ExtGState";
        private const string ColorSpaceKey = "ColorSpace";
        private const string PatternKey = "Pattern";
        private const string ShadingKey = "Shading";
        private const string xObjectKey = "XObject";
        private const string PropertiesKey = "Properties";

        private PDFDictionary _source;
        private PDFArray _procset;

        private PDFDictionary _fonts, _extGstate, _colorspace, _pattern, _shading, _xObject, _properties;

        public PDFDictionary OriginalDictionary
        {
            get { return _source; }
        }

        public PDFArray ProcSet
        {
            get { return _procset; }
        }

        public PDFDictionary Fonts
        {
            get { return _fonts; }
        }

        public PDFDictionary ExtGSState
        {
            get { return _extGstate; }
        }

        public PDFDictionary ColorSpace
        {
            get { return _colorspace; }
        }

        public PDFDictionary Patterns
        {
            get { return this._pattern; }
        }

        public PDFDictionary Shading
        {
            get { return _shading; }
        }

        public PDFDictionary XObjects
        {
            get { return _xObject; }
        }

        public PDFDictionary Properties
        {
            get { return _properties; }
        }

        public PDFParsedResources(PDFDictionary source)
        {
            this._source = source;
            if (null != this._source)
                this.PopulateResourceContents();
        }

        protected virtual void PopulateResourceContents()
        {
            PDFDictionary found;
            this._source.TryGet(ProcSetKey,out found);
            this._source.TryGet(FontsKey, out this._fonts);
            this._source.TryGet(ColorSpaceKey, out this._colorspace);
            this._source.TryGet(ExtGStateKey, out this._extGstate);
            this._source.TryGet(PatternKey, out this._pattern);
            this._source.TryGet(PropertiesKey, out this._properties);
            this._source.TryGet(ShadingKey, out this._shading);
            this._source.TryGet(xObjectKey, out this._xObject);
            
        }

        public ICollection<PDFResource> GetExistingResources(PDFFile source)
        {
            List<PDFResource> found = new List<PDFResource>();
            
            foreach (var rsrcType in this.OriginalDictionary.Keys)
            {
                if(rsrcType.Value == ProcSetKey)
                    //skip the procsets
                    continue;

                IPDFFileObject value = this.OriginalDictionary[rsrcType];
                if (value is PDFObjectRef)
                    value = source.AssertGetContent(value as PDFObjectRef);

                if (value is PDFDictionary)
                {
                    this.CollectResourceEntries(rsrcType, value as PDFDictionary, source, found);
                }
            }

            return found;
        }

        private int CollectResourceEntries(PDFName type, PDFDictionary entries, PDFFile source, List<PDFResource> intoCollection)
        {
            int count = 0;
            foreach (PDFName key in entries.Keys)
            {
                PDFExistingResource rsrc = new PDFExistingResource(type.Value, key.Value, key.Value, entries[key]);
                intoCollection.Add(rsrc);
                count++;
            }

            return count;
        }

        // public virtual void CopyResourcesToPage(PageBase page, PDFFile source)
        // {
        //     foreach (PDFName rsrcType in this.OriginalDictionary.Keys)
        //     {
        //         if (rsrcType.Value == ProcSetKey)
        //             //We automatically include the ProcSet anyway
        //             continue;
        //
        //         IPDFFileObject value = this.OriginalDictionary[rsrcType];
        //         if (value is PDFObjectRef)
        //             value = source.AssertGetContent(value as PDFObjectRef);
        //
        //         //We only support Dictionary entries within the Resource Dictionary
        //         if (value is PDFDictionary)
        //         {
        //             this.CopyResourceEntriesToPage(page, rsrcType, value as PDFDictionary, source);
        //         }
        //         
        //     }
        // }
        //
        // private void CopyResourceEntriesToPage(PageBase page, PDFName rstcType, PDFDictionary contents, PDFFile source)
        // {
        //     foreach (PDFName key in contents.Keys)
        //     {
        //         PDFExistingResource rsrc = new PDFExistingResource(rstcType.Value, key.Value, key.Value, contents[key]);
        //         page.Register(rsrc);
        //         page.Document.RegisterExistingResource(rsrc);
        //     }
        // }
    }
}
