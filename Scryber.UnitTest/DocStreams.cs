#define OutPutToFile


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.CompilerServices;

namespace Scryber.Core.UnitTests
{
    public static class DocStreams
    {
        private static string _testProjectDirectory = null;

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
                var dir = Path.GetDirectoryName(sourceFilePath);
                while (dir != null && !string.IsNullOrEmpty(dir))
                {
                    // Look for the .csproj file
                    if (File.Exists(Path.Combine(dir, "Scryber.UnitTests.csproj")))
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
                if (File.Exists(Path.Combine(currentDir, "Scryber.UnitTests.csproj")))
                {
                    _testProjectDirectory = currentDir;
                    return _testProjectDirectory;
                }
                
                // Also check if we're in a subdirectory and need to go to Scryber.UnitTest
                var unitTestPath = Path.Combine(currentDir, "Scryber.UnitTest");
                if (Directory.Exists(unitTestPath) && File.Exists(Path.Combine(unitTestPath, "Scryber.UnitTests.csproj")))
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
            var contentPath = Path.Combine(projectDir, "Content", relativePath);
            
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

        /// <summary>
        /// Legacy method - use AssertGetTemplatePath instead for template files
        /// </summary>
        [Obsolete("Use AssertGetTemplatePath for template files in the Content directory")]
        public static string AssertGetContentPath(string relative, TestContext context)
        {
            // Try new method first (if path doesn't start with ../)
            if (!relative.StartsWith("../") && !relative.StartsWith("..\\"))
            {
                try
                {
                    return GetTemplatePath(relative);
                }
                catch
                {
                    // Fall through to legacy behavior
                }
            }

            var path = context.TestRunDirectory;
            path = System.IO.Path.Combine(path, relative);
            path = System.IO.Path.GetFullPath(path);
            
            if(!System.IO.File.Exists(path))
                Assert.Fail("Test cannot run as the content file cannot be found at path " + path);

            return path;
        }
        
        public static string AssertGetDirectoryPath(string relative, TestContext context)
        {
            var path = context.TestRunDirectory;
            path = System.IO.Path.Combine(path, relative);
            path = System.IO.Path.GetFullPath(path);
            
            if(!System.IO.Directory.Exists(path))
                Assert.Fail("Test cannot run as the content directory cannot be found at path " + path);

            return path;
        }
    }
}
