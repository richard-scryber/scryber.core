#define OUTPUT_FILES

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.UnitSamples
{
    public class SampleBase
    {
        public const Environment.SpecialFolder baseOutput = Environment.SpecialFolder.MyDocuments;

        public const string SampleOutputFolder = "Scryber Test Output";
        public const string TemplatesFolder = "../../../Templates/";

        /// <summary>
        /// Gets a new output stream to a file with the path
        /// </summary>
        /// <param name="category"></param>
        /// <param name="fileNameWithExtension"></param>
        /// <returns></returns>
        protected static Stream GetOutputStream(string category, string fileNameWithExtension)
        {

#if OUTPUT_FILES

            //We are actually outputting to a file on the test machine.
            var path = System.Environment.GetFolderPath(baseOutput);

            path = Path.Combine(path, SampleOutputFolder);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, category);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var output = Path.Combine(path, fileNameWithExtension);

            return new FileStream(output, System.IO.FileMode.Create);

#else

            //No output wanted so just use a memory stream.
            var ms = new MemoryStream();
            return ms;
            
#endif
        }


        /// <summary>
        /// Gets the full path to the template specified, and (by default) checks to make sure the file exists.
        /// </summary>
        /// <param name="category">The category grouping (inner folder for the template). If null or empty, then the template file will be assumed to be in the root TemplatesFolder</param>
        /// <param name="fileNameWithExtension">The file name + extension for the file e.g MySample.html</param>
        /// <param name="assertExists">Optional but if true, a check will be made to ensure the file actually exists before returning</param>
        /// <returns>The full path to the template specified</returns>
        protected static string GetTemplatePath(string category, string fileNameWithExtension, bool assertExists = true)
        {
            var path = System.Environment.CurrentDirectory;

            if (string.IsNullOrEmpty(category))
                path = Path.Combine(path, TemplatesFolder, fileNameWithExtension);
            else
                path = Path.Combine(path, TemplatesFolder, category, fileNameWithExtension);

            //Clean the path
            path = Path.GetFullPath(path);

            if (assertExists)
                Assert.IsTrue(File.Exists(path), "The sample file at path '" + path + "' does not exist");

            return path;
        }
    }
}
