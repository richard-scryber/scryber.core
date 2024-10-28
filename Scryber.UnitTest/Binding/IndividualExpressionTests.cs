using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Html;

namespace Scryber.Core.UnitTests.Binding
{
	[TestClass()]
	public class IndividualExpressionTests
	{
        string data = @"{
    ""number"": 20.9,
    ""int"": 11,
    ""boolean"": true,
    ""date"": ""12 June 2023 11:45:00"",
    ""array"": [10,11,12],
    ""color"": ""#330033"",
    ""bg"": ""silver"",
    ""padding"": ""20pt"",
    ""items"": [
      {""name"": ""First Item"", ""index"" : 1},
      {""name"": ""Second Item"", ""index"": 3},
      {""name"": ""Third Item"", ""index"": 2}
    ],
      
    ""nested"" : {
      ""p1"" : ""one"",
      ""p2"" : ""two""
    }
}";

        private object GetData()
        {
            object parsed = System.Text.Json.JsonSerializer.Deserialize(data, typeof(object));
            return parsed;
        }

        private string GetContent(string expression)
        {
            var content = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Paperwork Expressions</title>
</head>
<body>
    <div>
        <h1 id='boundContent' class='title'>{{" + expression + @"}}</h1>
    </div>
</body>
</html>";

            return content;
        }

        private void AssertBoundContent(Panel container, string literalContent)
        {
            Assert.IsNotNull(container, "No container found to check the content");

            var content = container.Contents[0] as TextLiteral;
            Assert.IsNotNull(content, "Cound not find any content in the container to check");

            Assert.AreEqual(literalContent, content.Text, "The content did not match the expected result");
        }


        [TestMethod()]
        public void EachTest()
		{

            var content = GetContent("join(', ', eachOf(model.items, .index))");

            using var stream = new System.IO.StringReader(content);
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent);
            doc.Params["model"] = GetData();

            using (var output = DocStreams.GetOutputStream("JoinEachExpression.pdf"))
            {
                //doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(output);
            }

            var h1 = doc.FindAComponentById("boundContent") as Head1;
            AssertBoundContent(h1, "1, 3, 2");


        }

        [TestMethod]
        public void DictionaryPropertyTest()
        {
            var content = GetContent("model.nested['p1']");
            using var stream = new System.IO.StringReader(content);
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent);
            doc.Params["model"] = GetData();

            using ( var output = DocStreams.GetOutputStream("DictionaryPropertyAccessor.pdf"))
            {
                doc.SaveAsPDF(output);
            }
            
            var h1 = doc.FindAComponentById("boundContent") as Head1;
            AssertBoundContent(h1, "one");
        }
        
        [TestMethod]
        public void HexNumberTest()
        {
            var parsed = Convert.ToInt32("0xFFFF", 16);
            
            var content = GetContent("0xFFF0 &amp; 17");
            using var stream = new System.IO.StringReader(content);
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent);
            doc.Params["model"] = GetData();

            using ( var output = DocStreams.GetOutputStream("BinaryNotationNumber.pdf"))
            {
                doc.SaveAsPDF(output);
            }
            
            var h1 = doc.FindAComponentById("boundContent") as Head1;
            AssertBoundContent(h1, "16");
        }
        
        [TestMethod]
        public void BinaryNumberTest()
        {
            var parsed = Convert.ToInt32("1111", 2);
            
            var content = GetContent("0b0110 &amp; 7");
            using var stream = new System.IO.StringReader(content);
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent);
            doc.Params["model"] = GetData();

            using ( var output = DocStreams.GetOutputStream("BinaryNotationNumber.pdf"))
            {
                doc.SaveAsPDF(output);
            }
            
            var h1 = doc.FindAComponentById("boundContent") as Head1;
            AssertBoundContent(h1, "6");
        }
        
        [TestMethod]
        public void BinaryParamTest()
        {
            
            var content = GetContent("integer(paramValue) &amp; 7");
            using var stream = new System.IO.StringReader(content);
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent);
            doc.Params["model"] = GetData();
            doc.Params["paramValue"] = "0b0110";
            using ( var output = DocStreams.GetOutputStream("BinaryNotationNumber.pdf"))
            {
                doc.SaveAsPDF(output);
            }
            
            var h1 = doc.FindAComponentById("boundContent") as Head1;
            AssertBoundContent(h1, "6");
        }
	}
}

