=================================
Using the documentation Samples
=================================

All the samples in the documentation are available in the source code to follow along with.

`Scryber.Core Git repository <https://github.com/richard-scryber/scryber.core>`_

They are located as unit tests, so can be run individually, or as a group / whole in the project Scryber.UnitSamples

They will load any needed templates from the project folder /Templates/[category]/[filename.html]

And they will save files to the output /My Documents/Scryber Test Output/[category]/[filename.pdf]


Sample test base class
-----------------------

To reduce the boilerplate code the methods ``GetTemplatePath`` and ``GetOutputStream`` have been 
set up on a base class called ``TestBase`` that all sample tests inherit from.



.. code:: csharp

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

                if (!Directory.Exists(path))
                    throw new DirectoryNotFoundException("The special folder directory " + baseOutput.ToString() + " does not exist");

                path = Path.Combine(path, SampleOutputFolder);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = Path.Combine(path, category);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var output = Path.Combine(path, fileNameWithExtension);

                return new FileStream(output, FileMode.Create);

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


Changing the defaults
----------------------

The pre-defined values for the output folder, the location of the templates folder, and the 'SpecialFolder' where the output will be saved can be modified to alter location
either if you are experiencing dificulties in locating the samples or want to change where they will be created.

If you **do not** want to execute the tests to save to an actual file, the compiler directive OUTPUT_FILES can be removed (or commented)


Empty Sample Test class
------------------------

A basic set up for a sample in a test class is 

.. code:: csharp

    //Standard using namespaces

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scryber.Components;
    using Scryber.Styles;
    using Scryber.Drawing;

    namespace Scryber.UnitSamples
    {
        //Inherits from Scryber.UnitSamples.SampleBase

        [TestClass]
        public class MySamples : SampleBase
        {
            //Declare a test method

            [TestMethod]
            public void SimpleSample()
            {
                //Get the path to the template
                var path = GetTemplatePath("Samples", "Simple.html");

                //Parse the document at the path
                using (var doc = Document.ParseDocument(path))
                {
                    //do any further processing needed

                    //Create an output stream 
                    using(var stream = GetOutputStream("Samples", "Simple.pdf"))
                    {
                        //And save the document to that file
                        doc.SaveAsPDF(stream);
                    }

                }
            }
        }
    }

Contributing examples
---------------------

We would love to add more samples and starter documents / recipies. 

If you have an example you are proud of, or think would be useful to others. Please **do** fork the repository and propose the additions.