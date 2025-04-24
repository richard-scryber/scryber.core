using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF
{
    [PDFParsableValue()]
    public class PDFEmbeddedFileData
    {
        /// <summary>
        /// Gets the full file name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets the data that should be written to the file
        /// </summary>
        public byte[] FileData { get; set; }

        /// <summary>
        /// Gets or sets the Filters that are to be applied to the attachment
        /// </summary>
        public IStreamFilter[] Filters { get; set; }

        /// <summary>
        /// Gets the data length of the actual file without any filtering
        /// </summary>
        public int FileLength { get; set; }

        /// <summary>
        /// Gets the binary data lenght that will be written to the file (after any filtering is applied)
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// Gets or sets the flag to indicate if the specified filters could be applied to the file (can be too small or already compressed to apply).
        /// </summary>
        public bool FiltersApplied { get; set; }

        /// <summary>
        /// Gets the next embedded file that has a different set of filters applied
        /// </summary>
        public PDFEmbeddedFileData NextFilteredData { get; set; }

        public PDFEmbeddedFileData()
        {
            
        }

        public PDFEmbeddedFileData(string fullName, byte[] fileData) : this()
        {
            this.FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            this.FileData = fileData ?? throw new ArgumentNullException(nameof(fileData));
            this.FileLength = fileData.Length;
            this.DataLength = fileData.Length;
        }

        /// <summary>
        /// Looks in this linked list of embedded file data for a matching filtered instance.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public PDFEmbeddedFileData MatchFilters(IStreamFilter[] filters)
        {
            if (this.Filters == null || this.Filters.Length == 0)
            {
                if (filters == null || filters.Length == 0)
                    return this;
                else
                    return null == this.NextFilteredData ? null : this.NextFilteredData.MatchFilters(filters);
            }
            else
            {
                if (this.Filters.Length != filters.Length)
                    return null == this.NextFilteredData ? null : this.NextFilteredData.MatchFilters(filters);

                for (int i = 0; i < filters.Length; i++)
                {
                    if (this.Filters[i].FilterName != filters[i].FilterName)
                        return null == this.NextFilteredData ? null : this.NextFilteredData.MatchFilters(filters);
                }
                //we are the same length and contain the same filters applied in the same order
                return this;
            }
        }

        public static PDFEmbeddedFileData LoadFileDataFromFile(PDFLayoutContext context, string fullpath)
        {
            byte[] data = System.IO.File.ReadAllBytes(fullpath);
            PDFEmbeddedFileData all = new PDFEmbeddedFileData();
            all.FileData = data;
            all.Filters = null;
            all.FullName = fullpath;
            all.FileLength = data.Length;
            all.DataLength = data.Length;
            return all;
        }

        public static PDFEmbeddedFileData LoadFileDataFromUri(PDFLayoutContext context, string fullpath)
        {
            throw new NotSupportedException("Use the document remote file load capabilities");
        }

        public static PDFEmbeddedFileData Parse(string data)
        {
            return null;
        }

        
    }
}
