#define OutPutToFile


using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.UnitLayouts
{
    public static class DocStreams
    {

        public static System.IO.Stream GetOutputStream(string fileNameWithExtension)
        {
#if OutPutToFile

            var path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


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
        
        
        private static string _testProjectDirectory;
        
        /// <summary>
        /// Gets the root directory of the test project (Scryber.UnitTest)
        /// </summary>
        public static string GetTestProjectDirectory([CallerFilePath] string sourceFilePath = "")
        {
            if (_testProjectDirectory != null)
                return _testProjectDirectory;

            // Try to find the project directory using the caller's source file path
            if (!string.IsNullOrEmpty(sourceFilePath))
            {
                var dir = System.IO.Path.GetDirectoryName(sourceFilePath);
                while (dir != null && !string.IsNullOrEmpty(dir))
                {
                    // Look for the .csproj file
                    if (File.Exists(System.IO.Path.Combine(dir, "Scryber.UnitLayouts.csproj")))
                    {
                        _testProjectDirectory = dir;
                        return _testProjectDirectory;
                    }

                    dir = Directory.GetParent(dir)?.FullName;
                }
            }
            
            // Fallback: try to find it from the current directory
            var currentDir = Directory.GetCurrentDirectory();
            while (currentDir != null && !string.IsNullOrEmpty(currentDir))
            {
                if (File.Exists(System.IO.Path.Combine(currentDir, "Scryber.UnitLayouts.csproj")))
                {
                    _testProjectDirectory = currentDir;
                    return _testProjectDirectory;
                }
                
                // Also check if we're in a subdirectory and need to go to Scryber.UnitTest
                var unitTestPath = System.IO.Path.Combine(currentDir, "Scryber.UnitLayouts");
                if (Directory.Exists(unitTestPath) && File.Exists(System.IO.Path.Combine(unitTestPath, "Scryber.UnitLayouts.csproj")))
                {
                    _testProjectDirectory = unitTestPath;
                    return _testProjectDirectory;
                }

                currentDir = Directory.GetParent(currentDir)?.FullName;
            }

            throw new InvalidOperationException("Could not locate the Scryber.UnitTest project directory");
        }
        
        /// <summary>
        /// Gets the full path to a template file in the Content directory
        /// </summary>
        /// <param name="relativePath">Path relative to the Content directory (e.g., "HTML/HelloWorld.xhtml")</param>
        public static string GetTemplatePath(string relativePath)
        {
            var projectDir = GetTestProjectDirectory();
            var contentPath = Path.Combine(projectDir, relativePath);
            
            if (!File.Exists(contentPath))
                throw new FileNotFoundException($"Template file not found: {contentPath}", contentPath);
            
            return contentPath;
        }
        
        /// <summary>
        /// Asserts that a template file exists and returns its full path
        /// </summary>
        public static string AssertGetTemplatePath(string relativePath)
        {
            try
            {
                return GetTemplatePath(relativePath);
            }
            catch (FileNotFoundException ex)
            {
                Assert.Fail($"Test cannot run as the template file cannot be found: {ex.Message}");
                return null; // Never reached
            }
        }
    }
}
