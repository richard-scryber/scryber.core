using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Imaging.Utilities
{
    public static class DataUrlHelper
    {

        public static byte[] ExtractBase64Data(string fullDataUrl, string requiredMimeType)
        {
            // data:<mimetype>[;param=value...];base64,<imagedata>
            if (string.IsNullOrWhiteSpace(fullDataUrl))
                throw new FormatException("The data image url was empty");

            fullDataUrl = fullDataUrl.Trim();

            int start = 0;
            if (fullDataUrl.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                start = 5; //data: length
            else
                throw new FormatException("The data image url did not start with 'data:'");

            var dataFormatIndex = fullDataUrl.IndexOf(",", start, StringComparison.Ordinal);
            if (dataFormatIndex < 0)
                throw new FormatException("The data format for the image could not be determined");

            var metadata = fullDataUrl.Substring(start, dataFormatIndex - start);
            if (string.IsNullOrWhiteSpace(metadata))
                throw new FormatException("The mime type for the data image could not be determined");

            var metadataParts = metadata.Split(';');
            var mimeType = metadataParts[0].Trim();

            if (!string.IsNullOrEmpty(requiredMimeType)
                && !mimeType.Equals(requiredMimeType, StringComparison.OrdinalIgnoreCase))
                throw new FormatException("The only supported mime type is '" + requiredMimeType + "'");

            bool hasBase64 = false;
            for (int i = 1; i < metadataParts.Length; i++)
            {
                if (metadataParts[i].Trim().Equals("base64", StringComparison.OrdinalIgnoreCase))
                {
                    hasBase64 = true;
                    break;
                }
            }

            if (!hasBase64)
                throw new FormatException(
                    "The data format is not base 64, the only supported image encoding format for data images");

            string base64 = fullDataUrl.Substring(dataFormatIndex + 1).Trim();

            // Some pipelines can normalize '+' to whitespace; recover before decoding.
            if (base64.IndexOf(' ') >= 0)
                base64 = base64.Replace(' ', '+');
        
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