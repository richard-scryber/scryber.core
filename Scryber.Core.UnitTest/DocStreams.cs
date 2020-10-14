#define OutPutToFile


using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Core.UnitTests
{
    public static class DocStreams
    {

        public static System.IO.Stream GetOutputStream(string fileNameWithExtension)
        {
#if OutPutToFile

            var path = System.IO.Path.GetTempPath();
            path = System.IO.Path.Combine(path, "Scryber Test Output");

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            var output = System.IO.Path.Combine(path, fileNameWithExtension);

            System.Diagnostics.Debug.WriteLine("Beginning the document output for " + fileNameWithExtension + " to path '" + output + "'");

            return new System.IO.FileStream(output, System.IO.FileMode.Create);
#else
            var ms = new System.IO.MemoryStream();
            return ms;
#endif

        }
    }
}
