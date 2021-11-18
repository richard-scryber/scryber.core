using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Imaging.Utilities
{
    public static class DataUrlHelper
    {

        public static byte[] ExtractBase64Data(string fullDataUrl, string requiredMimeType)
        {
            
            //data:<mimetype>;base64,<imagedata>
            
            if (char.IsWhiteSpace(fullDataUrl, 0))
                fullDataUrl = fullDataUrl.TrimStart();

            int start = 0;
            if (fullDataUrl.StartsWith("data:", StringComparison.Ordinal))
                start = 5; //data: length
            else
                throw new FormatException("The data image url did not start with 'data:'");

            var mimeIndex = fullDataUrl.IndexOf(";", start, StringComparison.Ordinal);
            if (mimeIndex < 0)
                throw new FormatException("The mime type for the data image could not be determined");
            
            var mimeType = fullDataUrl.Substring(start, mimeIndex - start);
            
            if (!string.IsNullOrEmpty(requiredMimeType) && mimeType != requiredMimeType)
                throw new FormatException("The only supported mime type is '" + requiredMimeType + "'");

            start = mimeIndex + 1;

            var dataFormatIndex = fullDataUrl.IndexOf(",", start, StringComparison.Ordinal);
            if (dataFormatIndex < 0)
                throw new FormatException("The data format for the image could not be determined");


            var dataFormat = fullDataUrl.Substring(start, dataFormatIndex - start);
            start = dataFormatIndex + 1;

            if (dataFormat != "base64")
                throw new FormatException(
                    "The data format is not base 64, the only supported image encoding format for data images");

            string base64 = fullDataUrl.Substring(start);
        
#if ValidateBase64

            var match = Regex.Match(path, "[^A-Za-z0-9+/=]");
            if (match.Success)
                throw new FormatException("The base64 data in the data url contains invalid character " +
                                               match.Value);
#endif
            
            var bin = Convert.FromBase64String(base64);

            return bin;

        }
    }
}