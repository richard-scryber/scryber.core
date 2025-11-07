#define OutPutToFile


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.Core.UnitTests
{
    public static class DocStreams
    {

        public static System.IO.Stream GetOutputStream(string fileNameWithExtension)
        {
#if OutPutToFile

#if MAC_OS
            var path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#else
            var path = System.IO.Path.GetTempPath();
#endif

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

        public static string AssertGetContentPath(string relative, TestContext context)
        {
            var path = context.TestRunDirectory;
            path = System.IO.Path.Combine(path, relative);
            path = System.IO.Path.GetFullPath(path);
            
            if(!System.IO.File.Exists(path))
                Assert.Inconclusive("Test cannot run as the content file cannot be found at path " + path);

            return path;
        }
        
        public static string AssertGetDirectoryPath(string relative, TestContext context)
        {
            var path = context.TestRunDirectory;
            path = System.IO.Path.Combine(path, relative);
            path = System.IO.Path.GetFullPath(path);
            
            if(!System.IO.Directory.Exists(path))
                Assert.Inconclusive("Test cannot run as the content directory cannot be found at path " + path);

            return path;
        }
    }
}
