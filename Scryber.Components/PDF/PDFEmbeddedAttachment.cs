using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF
{
    /// <summary>
    /// Represents an EmbeddedFile artefact in the documents names dictionary
    /// </summary>
    public class PDFEmbeddedAttachment : Scryber.TypedObject, ICategorisedArtefactNamesEntry,  IArtefactEntry
    {

        public const string EmbeddedFilesNamesCategory = "EmbeddedFiles";
        public static readonly ObjectType EmbeddedFileObjectType = (ObjectType)"Embd";

        /// <summary>
        /// Gets or sets the full file path to the 
        /// </summary>
        public string FullFilePath { get; set; }

        /// <summary>
        /// Gets or sets the description that will be shown against the attachment in the reader.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the file name valuethat will be shown against the attachment in the reader.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the embedded file in the Destinations Name Tree for embedded files.
        /// </summary>
        public string DestinationName { get; set; }


        string ICategorisedArtefactNamesEntry.NamesCategory
        {
            get { return EmbeddedFilesNamesCategory; }
        }

        string ICategorisedArtefactNamesEntry.FullName
        {
            get { return this.DestinationName; }
        }

        /// <summary>
        /// Gets or sets the embedded file data itself to be witten to the document.
        /// </summary>
        public PDFEmbeddedFileData FileData
        {
            get;
            set;
        }

        public PDFEmbeddedAttachment(string fullpath, string filename, string destinationName, string description)
            : base(EmbeddedFileObjectType)
        {
            this.FullFilePath = fullpath;
            this.FileName = filename;
            this.DestinationName = destinationName;
            this.Description = description;
        }

        private Native.PDFObjectRef _oref = null;

        public Native.PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (null == _oref)
            {
                _oref = this.DoOutputToPDF(context, writer);
            }
            return _oref;
        }

        protected virtual Native.PDFObjectRef DoOutputToPDF(ContextBase context, PDFWriter writer)
        {
            if (context.ShouldLogVerbose)
                context.TraceLog.Begin(TraceLevel.Verbose, EmbeddedFilesNamesCategory, "Begining the output of attachment '" + this.FullFilePath + "'");

            Native.PDFObjectRef filespec = writer.BeginObject();

            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Filespec");
            writer.WriteDictionaryStringEntry("F", this.FileName);
            
            //EF entry is an inner dictionary with a reference to the actual file data stream
            writer.BeginDictionaryEntry("EF");
            writer.BeginDictionary();
            writer.BeginDictionaryEntry("F");
            Native.PDFObjectRef fdata = OutputFileData(writer, context);
            if (null != fdata)
                writer.WriteObjectRef(fdata);
            else
                writer.WriteNull();
            writer.EndDictionaryEntry();
            writer.EndDictionary();

            if (!string.IsNullOrEmpty(this.Description))
                writer.WriteDictionaryStringEntry("Desc", this.Description);


            writer.EndDictionary();
            writer.EndObject();

            if (context.ShouldLogVerbose)
                context.TraceLog.End(TraceLevel.Verbose, EmbeddedFilesNamesCategory, "Completed the output of attachment '" + this.FullFilePath + "'");

            return filespec;
        }

        private Native.PDFObjectRef OutputFileData(PDFWriter writer, ContextBase context)
        {
            if (null != this.FileData)
            {
                PDFEmbeddedFileData data = this.FileData;
                if (this.ShouldApplyFilters(writer, context))
                    data = GetFilteredData(data, writer, context);

                Native.PDFObjectRef dataObj = writer.BeginObject();
                writer.BeginStream(dataObj);
                writer.WriteRaw(data.FileData, 0, data.DataLength);
                writer.EndStream();
                writer.BeginDictionary();
                writer.WriteDictionaryNumberEntry("Length", data.DataLength);
                writer.WriteDictionaryNameEntry("Type", "EmbeddedFile");
                if (data.FiltersApplied == true && data.Filters != null && data.Filters.Length > 0)
                {
                    if (data.Filters.Length == 1)
                        writer.WriteDictionaryNameEntry("Filter", data.Filters[0].FilterName);
                    else
                    {
                        writer.BeginDictionaryEntry("Filter");
                        writer.BeginArray();
                        for (int i = 0; i < data.Filters.Length; i++)
                        {
                            writer.BeginArrayEntry();
                            writer.WriteName(data.Filters[i].FilterName);
                            writer.EndArrayEntry();
                        }
                        writer.EndArray();
                        writer.EndDictionaryEntry();
                    }
                }
                //Params with inner dictionary
                writer.BeginDictionaryEntry("Params");
                writer.BeginDictionary();
                writer.WriteDictionaryNumberEntry("Size", data.FileLength);
                writer.EndDictionary();
                writer.EndDictionaryEntry();

                writer.EndDictionary();
                writer.EndObject();

                return dataObj;
            }
            else
                return null;
        }


        protected virtual bool ShouldApplyFilters(PDFWriter writer, ContextBase context)
        {
            if (context.Compression != OutputCompressionType.None)
                return true;
            else
                return false;
        }

        protected virtual PDFEmbeddedFileData GetFilteredData(PDFEmbeddedFileData data, PDFWriter writer, ContextBase context)
        {
            IStreamFilter[] filters = GetStreamFilters(context);
            
            if(null == filters || filters.Length == 0)
                throw new NullReferenceException("No Filters to apply");

            PDFEmbeddedFileData filteredData = data.MatchFilters(filters);
            bool applied = true;

            if(null == filteredData)
            {
                byte[] actualfiltered = ApplyFiltersToData(filters, data.FileData);
                if (null == actualfiltered)
                {
                    actualfiltered = data.FileData;
                    applied = false;
                }
                    filteredData = new PDFEmbeddedFileData()
                    {
                        DataLength = actualfiltered.Length,
                        FileData = actualfiltered,
                        FileLength = data.FileLength,
                        FullName = data.FullName,
                        Filters = filters,
                        FiltersApplied = applied,
                        NextFilteredData = data.NextFilteredData
                    };
                
                //Add to the front of the queue
                data.NextFilteredData = filteredData;
            }

            return filteredData;
        }

        private IStreamFilter[] GetStreamFilters(ContextBase context)
        {
            switch (context.Compression)
            {
                case OutputCompressionType.None:
                    return new IStreamFilter[] { };

                case OutputCompressionType.FlateDecode:
                    return new IStreamFilter[] { PDFStreamFilters.FlateDecode };

                default:
                    throw new ArgumentOutOfRangeException("context.Compression", "The compression type " + context.Compression.ToString() + " is not supported");
            }
        }

        private byte[] ApplyFiltersToData(IStreamFilter[] filters, byte[] src)
        {
            byte[] dest;
            for (int i = 0; i < filters.Length; i++)
            {
                dest = filters[i].FilterStream(src);
                
                if (null == dest)
                    return null; // no compression applied
                src = dest;
            }
            return src;
        }



    }
}
