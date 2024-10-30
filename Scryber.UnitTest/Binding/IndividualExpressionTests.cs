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
        
        [TestMethod]
        public void ExponentOperatorTest()
        {
            
            var content = GetContent("50 ^ 4");
            using var stream = new System.IO.StringReader(content);
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent);
            doc.Params["model"] = GetData();

            using ( var output = DocStreams.GetOutputStream("BinaryNotationNumber.pdf"))
            {
                doc.SaveAsPDF(output);
            }
            
            var h1 = doc.FindAComponentById("boundContent") as Head1;
            AssertBoundContent(h1, "6250000");
        }
        
        string collectdata = @"{
       ""sections"": [
      	{
            ""id"": ""binary_ops"",
            ""name"": ""Binary Operators"",
            ""desc"": ""Binary operators have both a left and right value with the operator between, and will return a single result from the combined operation. The precedence order is in the standard PEDMAS - however parentheses (brackets) are always recommended to clarify and explain intent."",
            ""funcs"": [
                {
                    ""id"": ""plus_op"",
                    ""name"": ""Plus (+) operator"",
                    ""desc"": ""The plus operator will add 2 numbers together or join 2 strings, if the first operand is a string"",
                    ""usage"": ""{{LHS + RHS}}"",
                    ""returns"": [{
                       ""type"": ""number"",
                       ""desc"": ""If both values are convertable to numeric values.""
                    },{
                       ""type"": ""string"",
                       ""desc"": ""If the first value is a string, then the concatenation of both parameters.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""object"",
                      ""required"": true,
                      ""desc"": ""Any object, constant or expression. If it is not a number then it will be converted to a string.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""object"",
                      ""required"": true,
                      ""desc"": ""Any object, constant or expression. If the LHS parameter is a number then this RHS value should be a number, or be able to be converted to a number""
                    }],
                    ""examples"": [
                        { ""text"": ""{{12 + 4}}"", 
                          ""expr"": ""12 + 4"",
                          ""desc"": ""Simply adding two numbers together""
                        },
                        { ""text"": ""12 + model.number"", ""expr"": ""12 + model.number"" },
                        { ""text"": ""\""12\"" + model.number"", ""expr"": ""\""12\"" + model.number"" },
                        { ""text"": ""model.number + \""12\"""", ""expr"": ""model.number + \""12\"""" },
                        { ""text"": ""model.number + \"" - as a string\"""", ""expr"": ""model.number + \"" - as a string\"""" },
                        { ""text"": ""\""To a string - \"" + model.number"", ""expr"": ""\""To a string - \"" + model.number"" },
                        { ""text"": ""integer(\""12\"") + model.number"", ""expr"": ""integer(\""12\"") + model.number"" }
                    ],
                    ""notes"":""When the first operand is a string then the following argument will be concatenated, however if the first argument is a number then the second will attempt to be converted to a number. If in doubt use the conversion functions, or concatenate function."",
                    ""seealso"": [""minus_op"",""binary_ops"",""number_func"",""string_func""]
                },
                {
                    ""id"": ""minus_op"",
                    ""name"": ""Minus (-) operator"",
                    ""desc"": ""The minus operator will subtract the second (right) operand from the first (left) operand."",
                    ""usage"": ""{{LHS - RHS}}"",
                    ""returns"": [{
                       ""type"": ""number"",
                       ""desc"": ""The result of the subtract operation as a numeric value.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    }],
                    ""examples"": [
                        { ""text"": ""50 - 4"", ""expr"": ""50 - 4"" },
                        { ""text"": ""50 - model.number"", ""expr"": ""50 - model.number"" },
                        { ""text"": ""model.number - \""12\"""", ""expr"": ""model.number - \""12\"""" },
                        { ""text"": ""model.number - \""two\"""", ""expr"": ""model.number - \""two\"""" }
                    ],
                   ""notes"" : ""If either the LHS, or RHS parameters are not numbers then an attempt will be made to convert the parameters string representation to a number. If the conversion cannot be made, then the entire expression will error."",
                   ""seealso"": [""plus_op"",""binary_ops"",""number_func"",""string_func""]
                },
                {
                    ""id"": ""multiply_op"",
                    ""name"": ""Multiply (*) operator"",
                    ""desc"": ""The multiply operator will return the product value of the LHS by the RHS"",
                    ""usage"": ""{{LHS + RHS}}"",
                    ""returns"": [{
                       ""type"": ""number"",
                       ""desc"": ""The result of the multiply operation as a numeric value.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    }],
                    ""examples"": [
                        { ""text"": ""50 * 4"", ""expr"": ""50 * 4"" },
                        { ""text"": ""50 * model.number"", ""expr"": ""50 * model.number"" },
                        { ""text"": ""model.number * \""12\"""", ""expr"": ""model.number * \""12\"""" },
                        { ""text"": ""model.number * \""two\"""", ""expr"": ""model.number * \""two\"""" }
                    ],
                   ""notes"" : ""If either the LHS, or RHS parameters are not numbers then an attempt will be made to convert the parameters string representation to a number. If the conversion cannot be made, then the entire expression will error."",
                   ""seealso"": [""divide_op"",""binary_ops"",""number_func"",""string_func""]
                },
                {
                    ""id"": ""divide_op"",
                    ""name"": ""Divide (/) operator"",
                    ""desc"": ""The divide operator will divide the LHS value by the RHS value and return the result"",
                    ""usage"": ""{{LHS + RHS}}"",
                    ""returns"": [{
                       ""type"": ""number"",
                       ""desc"": ""The result of the divide operation as a numeric value.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    }],
                    ""examples"": [
                        { ""text"": ""50 / 4"", ""expr"": ""50 / 4"" },
                        { ""text"": ""50 / model.number"", ""expr"": ""50 / model.number"" },
                        { ""text"": ""model.number / \""12\"""", ""expr"": ""model.number / \""12\"""" },
                        { ""text"": ""model.number / \""two\"""", ""expr"": ""model.number / \""two\"""" }
                    ],
                   ""notes"" : ""If either the LHS, or RHS parameters are not numbers then an attempt will be made to convert the parameters string representation to a number. If the conversion cannot be made, then the entire expression will error."",
                   ""seealso"": [""divide_op"",""binary_ops"",""number_func"",""string_func""]
                },
                {
                    ""id"": ""modulo_op"",
                    ""name"": ""Modulo (%) operator"",
                    ""desc"": ""The modulo operator will divide the LHS value by the RHS value and return the remainder of the division"",
                    ""usage"": ""{{LHS % RHS}}"",
                    ""returns"": [{
                       ""type"": ""number"",
                       ""desc"": ""The result of the modulo operation as a numeric value.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    }],
                    ""examples"": [
                        { ""text"": ""50 % 4"", ""expr"": ""50 % 4"" },
                        { ""text"": ""50 % model.number"", ""expr"": ""50 % model.number"" },
                        { ""text"": ""model.number % \""12\"""", ""expr"": ""model.number % \""12\"""" },
                        { ""text"": ""model.number % \""two\"""", ""expr"": ""model.number % \""two\"""" }
                    ],
                   ""notes"" : ""If either the LHS, or RHS parameters are not numbers then an attempt will be made to convert the parameters string representation to a number. If the conversion cannot be made, then the entire expression will error."",
                   ""seealso"": [""divide_op"",""binary_ops"",""number_func"",""string_func""]
                },
                {
                    ""id"": ""exponent_op"",
                    ""name"": ""Exponent (^) operator"",
                    ""desc"": ""The exponent operator raise the LHS number to the power of the RHS number"",
                    ""usage"": ""{{LHS + RHS}}"",
                    ""returns"": [{
                       ""type"": ""number"",
                       ""desc"": ""The result of the exponent operation as a numeric value.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""number"",
                      ""required"": true,
                      ""desc"": ""Any number, or a constant or expression that returns a number value.""
                    }],
                    ""examples"": [
                        { ""text"": ""50 ^ 4"", ""expr"": ""50 ^ 4"" },
                        { ""text"": ""50 ^ (model.int / 10)"", ""expr"": ""50 ^ (model.int / 10)"" },
                        { ""text"": ""model.number ^ \""2\"""", ""expr"": ""model.number ^ \""12\"""" },
                        { ""text"": ""\""12\"" ^ 2"", ""expr"": ""\""12\"" ^ 2"" },
                        { ""text"": ""model.number ^ \""two\"""", ""expr"": ""model.number ^ \""two\"""" }
                    ],
                   ""notes"" : ""If either the LHS, or RHS parameters are not numbers then an attempt will be made to convert the parameters string representation to a number. If the conversion cannot be made, then the entire expression will error."",
                   ""seealso"": [""divide_op"",""binary_ops"",""number_func"",""string_func""]
                },
                {
                    ""id"": ""bitwise_and_op"",
                    ""name"": ""Bitwise And (&) operator"",
                    ""desc"": ""The bitwise operators will perform bit level comparison operations on integer values. All bits within the values will be compared based on the AND truth tables."",
                    ""usage"": ""{{LHS & RHS}}"",
                    ""returns"": [{
                       ""type"": ""integer"",
                       ""desc"": ""The result of the bitwise operation as a numeric value.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""integer"",
                      ""required"": true,
                      ""desc"": ""Any integer, or a constant or expression that returns an integer value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""integer"",
                      ""required"": true,
                      ""desc"": ""Any integer value, or a constant or expression that returns an integer value.""
                    }],
                  ""examples"": [
                        { ""text"": ""11 & 7"", ""expr"": ""11 & 7"" },
                        { ""text"": ""string(0b1011 & 7, 'b')"", ""expr"": ""string((0b1011 & 7), 'b')"" },
                        { ""text"": ""model.int & 7"", ""expr"": ""model.int & 7"" }
                    ],
                  ""notes"" : ""If either the LHS, or RHS parameters are not integers, then an attempt will be made to convert the parameters string representation to an integer. If the conversion cannot be made, then the entire expression will error. For explicit conversion, use the integer() function."",
                  ""seealso"": [""binary_ops"",""integer_func"",""string_func""]
                },
                {
                    ""id"": ""bitwise_or_op"",
                    ""name"": ""Bitwise Or (|) operator"",
                    ""desc"": ""The bitwise operators will perform bit level comparison operations on integer values. All bits within the values will be compared based on the OR truth tables."",
                    ""usage"": ""{{LHS | RHS}}"",
                    ""returns"": [{
                       ""type"": ""integer"",
                       ""desc"": ""The result of the bitwise operation as a numeric value.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""integer"",
                      ""required"": true,
                      ""desc"": ""Any integer, or a constant or expression that returns an integer value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""integer"",
                      ""required"": true,
                      ""desc"": ""Any integer value, or a constant or expression that returns an integer value.""
                    }],
                  ""examples"": [
                        { ""text"": ""11 | 7"", ""expr"": ""11 | 7"" },
                        { ""text"": ""string(0b1011 | 7, 'b')"", ""expr"": ""string(0b1011 | 7, 'b')"" },
                        { ""text"": ""model.int | 7"", ""expr"": ""model.int | 7"" }
                    ],
                  ""notes"" : ""If either the LHS, or RHS parameters are not integers, then an attempt will be made to convert the parameters string representation to an integer. If the conversion cannot be made, then the entire expression will error. For explicit conversion, use the integer() function."",
                  ""seealso"": [""binary_ops"",""integer_func"",""string_func""]
                },
                {
                    ""id"": ""bitwise_left_op"",
                    ""name"": ""Bitwise Shift Left (<<) operator"",
                    ""desc"": ""The left shift (<<) operator returns a number whose binary representation is the first operand shifted by the specified number of bits to the left. Excess bits shifted off to the left are discarded, and zero bits are shifted in from the right."",
                    ""usage"": ""{{LHS << RHS}}"",
                    ""returns"": [{
                       ""type"": ""integer"",
                       ""desc"": ""The result of the bitwise operation as a numeric value.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""integer"",
                      ""required"": true,
                      ""desc"": ""Any integer, or a constant or expression that returns an integer value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""integer"",
                      ""required"": true,
                      ""desc"": ""Any integer value, or a constant or expression that returns an integer value.""
                    }],
                  ""examples"": [
                        { ""text"": ""11 << 7"", ""expr"": ""11 << 7"" },
                        { ""text"": ""string(0b1011 << 7, 'b')"", ""expr"": ""string(0b1011 << 7, 'b')"" },
                        { ""text"": ""string(model.int << 7)"", ""expr"": ""model.int << 7"" }
                    ],
                  ""notes"" : ""If either the LHS, or RHS parameters are not integers, then an attempt will be made to convert the parameters string representation to an integer. If the conversion cannot be made, then the entire expression will error. For explicit conversion, use the integer() function."",
                  ""seealso"": [""bitwise_right_op"",""binary_ops"",""integer_func"",""string_func""]
                },
                {
                    ""id"": ""bitwise_right_op"",
                    ""name"": ""Bitwise Shift Right (>>) operator"",
                    ""desc"": ""The right shift (>>) operator returns a number whose binary representation is the first operand shifted by the specified number of bits to the right. Excess bits shifted off to the right are discarded, and zero bits are shifted in from the left."",
                    ""usage"": ""{{LHS >> RHS}}"",
                    ""returns"": [{
                       ""type"": ""integer"",
                       ""desc"": ""The result of the bitwise operation as a numeric value.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""integer"",
                      ""required"": true,
                      ""desc"": ""Any integer, or a constant or expression that returns an integer value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""integer"",
                      ""required"": true,
                      ""desc"": ""Any integer value, or a constant or expression that returns an integer value.""
                    }],
                  ""examples"": [
                        { ""text"": ""11 >> 2"", ""expr"": ""11 >> 2"" },
                        { ""text"": ""string(0b1011 >> 2, 'b')"", ""expr"": ""string(0b1011 >> 2, 'b')"" },
                        { ""text"": ""string(model.int >> 2)"", ""expr"": ""model.int >> 2"" }
                    ],
                  ""notes"" : ""If either the LHS, or RHS parameters are not integers, then an attempt will be made to convert the parameters string representation to an integer. If the conversion cannot be made, then the entire expression will error. For explicit conversion, use the integer() function."",
                  ""seealso"": [""bitwise_left_op"",""binary_ops"",""integer_func"",""string_func""]
                },
                {
                    ""id"": ""nullcoalescing_op"",
                    ""name"": ""Null coalescing (??) operator"",
                    ""desc"": ""The null coalesce operator (??) will provide the first (left) value if it is not null, otherwise it will provide the second (right) value."",
                    ""usage"": ""{{LHS ?? RHS}}"",
                    ""returns"": [{
                       ""type"": ""object"",
                       ""desc"": ""The result of the null comparison in the type provided of either the left or right hand side arguments. No conversion is automatically done.""
                    }],
                    ""parameters"": [{
                      ""name"": ""LHS"",
                      ""type"": ""object"",
                      ""required"": true,
                      ""desc"": ""Any object, or a constant or expression that returns an object value.""
                    },{
                      ""name"": ""RHS"",
                      ""type"": ""object"",
                      ""required"": true,
                      ""desc"": ""Any object, or a constant or expression that returns an object value.""
                    }],
                  ""examples"": [
                        { ""text"": ""null ?? 'replaced'"", ""expr"": ""null ?? 'replaced'"" },
                        { ""text"": ""'not-replaced' ?? 'replaced'"", ""expr"": ""'not-replaced' ?? 'replaced'"" },
                        { ""text"": ""model.color ?? '#aaaaaa'"", ""expr"": ""model.color ?? '#aaaaaa'"" },
                        { ""text"": ""model.notset ?? '#aaaaaa'"", ""expr"": ""model.notset ?? '#aaaaaa'"" }
                    ],
                  ""notes"" : """",
                  ""seealso"": [""bitwise_left_op"",""binary_ops"",""integer_func"",""string_func""]
                },
                {
                    ""id"": ""combining"",
                    ""name"": ""Combining binary operators"",
                    ""desc"": ""The operators can be combined with their nartual precedence or, explicit precedence with parenthese."",
                    ""examples"": [
                        { ""text"": ""12 + 2 - 6 * 2 / 4"", ""expr"": ""12 + 2 - 6 * 2 / 4"" },
                        { ""text"": ""((12 + 2) - 6) * (2 / 4)"", ""expr"": ""((12 + 2) - 6) * (2 / 4)"" }
                    ]
                }
            ]
        },
        {
            ""id"": ""relational-ops"",
            ""name"": ""Relational Operators"",
            ""funcs"": [
                {
                    ""id"": ""equals"",
                    ""name"": ""Equal (==) operator"",
                    ""desc"": ""The equality (==) operator will return true if the left and right side are the same value. If the types of values are different, then an attempt to convert to the same value will be made."",
                    ""notes"": ""Zero is classed as false, any other value is true. More information on the 'if' function can be found later in the logical functions."",
                    ""usage"": ""{{LHS == RHS}}"",
                    ""returns"": {
                        ""type"": ""boolean"",
                        ""desc"":""true if the LHS and RHS objects are considered equal; otherwise, false""
                    },
                    ""parameters"": [{
                        ""name"": ""LHS"",
                        ""type"": ""Object"",
                        ""desc"": ""Required. Any object, constant or expression""
                        },{
                        ""name"": ""RHS"",
                        ""type"": ""Object"",
                        ""desc"": ""Required. Any object, constant or expression""
                        }],
                    ""modeldata"": {
                        ""number"": 20.9,
                        ""int"": 11,
                        ""bg"": ""silver""
                        
                    },
                    ""examples"": [
                        {
                            ""text"": ""20.9 == 20.9"",
                            ""expr"": ""20.9 == 20.9"",
                            ""value"": ""true"",
                            ""desc"": ""The direct comparison of 2 numbers will result in true""
                        },
                        {
                            ""text"": ""model.number == 20.0"",
                            ""expr"": ""model.number == 20.0"",
                            ""value"": ""false"",
                            ""desc"": ""The comparison of the bound model values and the number will result in false""
                        },
                        {
                            ""text"": ""model.number == '20.9'"",
                            ""expr"": ""model.number == '20.9'"",
                            ""value"": ""true"",
                            ""desc"": ""The RHS value will be converted to a number and then the comparison will result in true""
                        },
                        {
                            ""text"": ""model.int == 11"",
                            ""expr"": ""model.int == 11"",
                            ""value"": ""true"",
                            ""desc"": ""The RHS value matches the LHS type and value, so the comparison will result in true""
                        },
                        {
                            ""text"": ""model.int == 11.1"",
                            ""expr"": ""model.int == 11.1"",
                            ""value"": ""true"",
                            ""desc"": ""The LHS value is converted to a floating point value and the comparison will result in false""
                        },
                        {
                            ""text"": ""0 == false"",
                            ""expr"": ""0 == false"",
                            ""value"": ""true"",
                            ""desc"": ""As 0 is considered the same as false the comparison will result in true""
                        },
                        {
                            ""text"": ""1 == false"",
                            ""expr"": ""1 == false"",
                            ""value"": ""false"",
                            ""desc"": ""As any value other than 0 is considered the same as true the comparison will result in false""
                        },
                        {
                            ""text"": ""-1 == false"",
                            ""expr"": ""-1 == false"",
                            ""value"": ""false"",
                            ""desc"": ""As any value other than 0 is considered the same as true the comparison will result in false""
                        },
                        {
                            ""text"": ""model.number == null"",
                            ""expr"": ""model.number == null"",
                            ""value"": ""false"",
                            ""desc"": ""As the LHS has a value, it is not null, and the comparison will result in false""
                        },
                        {
                            ""text"": ""model.notset == null"",
                            ""expr"": ""model.notset == null"",
                            ""value"": ""true"",
                            ""desc"": ""As the LHS does NOT have a value, it is undefined and is null, and the comparison will result in true""
                        },
                        {
                            ""text"": ""model.bg == 'silver'"",
                            ""expr"": ""model.bg == 'silver'"",
                            ""value"": ""true"",
                            ""desc"": ""The RHS value matches the LHS type and value, so the comparison will result in true""
                        },
                        {
                            ""text"": ""model.bg == 'SILVER'"",
                            ""expr"": ""model.bg == 'SILVER'"",
                            ""value"": ""false"",
                            ""desc"": ""The RHS value matches the LHS type but string comparisons are case sensitive, so the comparison will result in false""
                        },
                        {
                            ""text"": ""if(model.number == 20.9,'Equal', 'Not equal')"",
                            ""expr"": ""if(model.number == 20.9,'Equal', 'Not equal')"",
                            ""value"": ""'Equal'"",
                            ""desc"": ""Using the if() function the second argument is returned, as the result of the comparison was true""
                        },
                        {
                            ""text"": ""if(model.number == 20.0,'', model.bg)"",
                            ""expr"": ""if(model.number == 20.0,'', model.bg)"",
                            ""value"": ""'silver'"",
                            ""desc"": ""Using the if() function the third argument is evaluated and returned, as the result of the comparison was false""
                        }
                    ],
                    ""seealso"": [""notequals"", ""relational-ops"", ""if""]
                },
                {
                    ""id"": ""notequals"",
                    ""name"": ""Not Equal (!=) operator"",
                    ""desc"": ""The non-equality (!=) operator will return true if the left and right side are *not* the same value. Again, if the types of values are different, then an attempt to convert to the same value will be made."",
                    ""notes"": ""More information on the 'if' function can be found later in the logical functions."",
                    ""examples"": [
                        { ""text"": ""20.9 != 20.9"", 
                          ""expr"": ""20.9 != 20.9"",
                          ""value"" : ""false"",
                          ""desc"": ""The direct comparison of 2 numbers will result in false if they are not of the same equivalent value""
                        },
                        { ""text"": ""model.number != 20.0"", ""expr"": ""model.number != 20.0"" },
                        { ""text"": ""model.number != '20.9'"", ""expr"": ""model.number != '20.9'"" },
                        { ""text"": ""model.int != 11.0"", ""expr"": ""model.int != 11.0"" },
                        { ""text"": ""model.int != 11.1"", ""expr"": ""model.int != 11.1"" },
                        { ""text"": ""0 != true"", ""expr"": ""0 != true"" },
                        { ""text"": ""1 != true"", ""expr"": ""1 != true"" },
                        { ""text"": ""-1 != true"", ""expr"": ""-1 != true"" },
                        { ""text"": ""model.number != null"", ""expr"": ""model.number != null"" },
                        { ""text"": ""model.notset != null"", ""expr"": ""model.notset != null"" },
                        { ""text"": ""model.bg != 'silver'"", ""expr"": ""model.bg != 'silver'"" },
                        { ""text"": ""model.bg != 'SILVER'"", ""expr"": ""model.bg != 'SILVER'"" },
                        { ""text"": ""if(model.number != 20.9,'Equal', 'Not equal')"", ""expr"": ""if(model.number != 20.9,'Equal', 'Not equal')"" },
                        { ""text"": ""if(model.number != 20.0,'Equal', 'Not equal')"", ""expr"": ""if(model.number != 20.0,'Equal', 'Not equal')"" }
                    ]
                },
                {
                    ""id"": ""lessthan"",
                    ""name"": ""Less than (<) operator"",
                    ""desc"": ""The less than (<) operator will return true if the left side is less than the right side. Again, if the types of values are different, then an attempt to convert to the same type will be made. String comparison is case sensitive"",
                    ""notes"": ""More information on the 'if' function can be found later in the logical functions."",
                    ""examples"": [
                        { ""text"": ""20.9 < 21.0"", ""expr"": ""20.9 < 21.0"" },
                        { ""text"": ""20.9 < 20.9"", ""expr"": ""20.9 < 20.9"" },
                        { ""text"": ""model.number < 30.0"", ""expr"": ""model.number < 30.0"" },
                        { ""text"": ""model.bg < 'silver'"", ""expr"": ""model.bg < 'silver'"" },
                        { ""text"": ""if(model.number < 21,'Less', 'More or equal')"", ""expr"": ""if(model.number < 21,'Less', 'More or equal')"" },
                        { ""text"": ""if(model.bg < 'silver','Less', 'More or equal')"", ""expr"": ""if(model.bg < 'silver','Less', 'More or equal')"" },
                        { ""text"": ""if(model.bg < 'SILVER','Less', 'More or equal')"", ""expr"": ""if(model.bg < 'SILVER','Less', 'More or equal')"" }
                    ]
                },
                {
                    ""id"": ""greaterthan"",
                    ""name"": ""More than (>) operator"",
                    ""desc"": ""The More than (>) operator will return true if the left side is greater than the right side. Again, if the types of values are different, then an attempt to convert to the same type will be made. String comparison is case sensitive"",
                    ""notes"": ""More information on the 'if' function can be found later in the logical functions."",
                    ""examples"": [
                        { ""text"": ""20.9 > 21.0"", ""expr"": ""20.9 > 21.0"" },
                        { ""text"": ""20.9 > 20.0"", ""expr"": ""20.9 > 20.0"" },
                        { ""text"": ""model.number > 30.0"", ""expr"": ""model.number > 30.0"" },
                        { ""text"": ""model.bg > 'silver'"", ""expr"": ""model.bg > 'silver'"" },
                        { ""text"": ""if(model.number > 20,'More', 'Less or equal')"", ""expr"": ""if(model.number > 20,'More', 'Less or equal')"" },
                        { ""text"": ""if(model.bg > 'silver','More', 'Less or equal')"", ""expr"": ""if(model.bg > 'silver','More', 'Less or equal')"" },
                        { ""text"": ""if(model.bg > 'SILVER','More', 'Less or equal')"", ""expr"": ""if(model.bg > 'SILVER','More', 'Less or equal')"" }
                    ]
                },
                {
                    ""id"": ""lessthanequal"",
                    ""name"": ""Less than or equal (<=) operator"",
                    ""desc"": ""The less than or equal (<=) operator will return true if the left side is less than or the same as the right side. If the types of values are different, then an attempt to convert to the same type will be made. String comparison is case sensitive"",
                    ""notes"": ""More information on the 'if' function can be found later in the logical functions."",
                    ""examples"": [
                        { ""text"": ""20.9 <= 20.0"", ""expr"": ""20.9 <= 20.0"" },
                        { ""text"": ""20.9 <= 20.9"", ""expr"": ""20.9 <= 20.9"" },
                        { ""text"": ""model.number <= 30.0"", ""expr"": ""model.number <= 30.0"" },
                        { ""text"": ""model.bg <= 'silver'"", ""expr"": ""model.bg <= 'silver'"" },
                        { ""text"": ""model.bg <= 'liver'"", ""expr"": ""model.bg <= 'liver'"" },
                        { ""text"": ""if(model.number <= 21,'Less or equal', 'More')"", ""expr"": ""if(model.number <= 21,'Less or equal', 'More')"" },
                        { ""text"": ""if(model.bg <= 'silver','Less or equal', 'More')"", ""expr"": ""if(model.bg <= 'silver','Less or equal', 'More')"" },
                        { ""text"": ""if(model.bg <= 'SILVER','Less or equal', 'More')"", ""expr"": ""if(model.bg <= 'SILVER','Less or equal', 'More')"" }
                    ]
                },
                {
                    ""id"": ""greaterthanequal"",
                    ""name"": ""Greater than or equal(>=) operator"",
                    ""desc"": ""The greater than or equal (>=) operator will return true if the left side is more than or the same as the right side. If the types of values are different, then an attempt to convert to the same type will be made. String comparison is case sensitive"",
                    ""notes"": ""More information on the 'if' function can be found later in the logical functions."",
                    ""examples"": [
                        { ""text"": ""20.9 >= 20.0"", ""expr"": ""20.9 >= 20.0"" },
                        { ""text"": ""20.9 >= 30.0"", ""expr"": ""20.9 >= 30.0"" },
                        { ""text"": ""model.number >= 30.0"", ""expr"": ""model.number >= 30.0"" },
                        { ""text"": ""model.bg >= 'silver'"", ""expr"": ""model.bg >= 'silver'"" },
                        { ""text"": ""model.bg >= 'quivers'"", ""expr"": ""model.bg >= 'liver'"" },
                        { ""text"": ""if(model.number >= 20,'More or equal', 'Less')"", ""expr"": ""if(model.number >= 20,'More or equal', 'Less')"" },
                        { ""text"": ""if(model.bg >= 'silver','More or equal', 'Less')"", ""expr"": ""if(model.bg >= 'silver','More or equal', 'Less')"" },
                        { ""text"": ""if(model.bg >= 'SILVER','More or equal', 'Less')"", ""expr"": ""if(model.bg >= 'SILVER','More or equal', 'Less')"" }
                    ]
                }]
        },
        {
            ""id"": ""logical-ops"",
            ""name"": ""Logical Operators"",
            ""funcs"": [
                {
                    ""id"": ""and"",
                    ""name"": ""And (&&) operator"",
                    ""desc"": ""The logical and (&&) operator will return true if the left and right side are both true, otherwise it will return false. Either side can be a constant or an expression, and if the types of values are different, then an attempt to convert to the same value will be made."",
                    ""notes"": ""The value of zero (0), or 'null' is classed as false, anything else is true. Strings are parsed as boolean, rather than an empty string being false. More information on the 'if' function can be found later in the logical functions."",
                    ""examples"": [
                        { ""text"": ""true && true"", ""expr"": ""true && true"" },
                        { ""text"": ""true && false"", ""expr"": ""true && false"" },
                        { ""text"": ""model.boolean && true"", ""expr"": ""model.boolean && true"" },
                        { ""text"": ""null && true"", ""expr"": ""null && true"" },
                        { ""text"": ""'' && true"", ""expr"": ""'' && true"" },
                        { ""text"": ""'true' && true"", ""expr"": ""'true' && true"" },
                        { ""text"": ""model.boolean && 1"", ""expr"": ""model.boolean && 1"" },
                        { ""text"": ""model.boolean && 0"", ""expr"": ""model.boolean && 0"" },
                        { ""text"": ""model.boolean && -1"", ""expr"": ""model.boolean && -1"" },
                        { ""text"": ""model.bg && ''"", ""expr"": ""model.bg && ''"" },
                        { ""text"": ""model.boolean && -1"", ""expr"": ""model.boolean && -1"" },
                        { ""text"": ""if(model.number && true, 'Set', 'Not Set')"", ""expr"": ""if(model.number && true, 'Set', 'Not Set')"" },
                        { ""text"": ""if(model.number > 20 && model.number > 21,'Between', 'Outside')"", ""expr"": ""if(model.number > 20 && model.number < 21,'Between', 'Outside')"" }
                    ]
                },
                {
                    ""id"": ""or"",
                    ""name"": ""Or (||) operator"",
                    ""desc"": ""The logical or (||) operator will return true if the left or right side are true, otherwise it will return false. Either side can be a constant or an expression, and if the types of values are different, then an attempt to convert to the same value will be made."",
                    ""notes"": ""The value of zero (0), or 'null' is classed as false, anything else is true. Strings are parsed as boolean, rather than an empty string being false. More information on the 'if' function can be found later in the logical functions."",
                    ""examples"": [
                        { ""text"": ""true || true"", ""expr"": ""true || true"" },
                        { ""text"": ""true || false"", ""expr"": ""true || false"" },
                        { ""text"": ""model.boolean || true"", ""expr"": ""model.boolean || true"" },
                        { ""text"": ""null || true"", ""expr"": ""null && true"" },
                        { ""text"": ""'' || true"", ""expr"": ""'' || true"" },
                        { ""text"": ""'true' || true"", ""expr"": ""'true' || true"" },
                        { ""text"": ""model.boolean || 1"", ""expr"": ""model.boolean || 1"" },
                        { ""text"": ""model.boolean || 0"", ""expr"": ""model.boolean || 0"" },
                        { ""text"": ""model.boolean || -1"", ""expr"": ""model.boolean || -1"" },
                        { ""text"": ""model.bg || ''"", ""expr"": ""model.bg || ''"" },
                        { ""text"": ""if(model.number || false, 'Set', 'Not Set')"", ""expr"": ""if(model.number || false, 'Set', 'Not Set')"" },
                        { ""text"": ""if(model.number > 21 || model.number < 20,'Outside', 'Between')"", ""expr"": ""if(model.number > 21 || model.number < 20,'Outside', 'Between')"" }
                    ]
                },
                {
                    ""id"": ""not"",
                    ""name"": ""Not (!) operator"",
                    ""desc"": ""The not (!) unary operator will return true if the contained expression results in false, or false if the contained expression results in true. The contained expression can be a constant or another expression, and if the type of value is not a boolean value, then an attempt to convert to boolean will be made."",
                    ""notes"": ""The !! operator must currently be separated by parentheses e.g. !(!true), directly using !!true will cause an error"",
                    ""examples"": [
                        { ""text"": ""!true"", ""expr"": ""!true"" },
                        { ""text"": ""!0"", ""expr"": ""!0"" },
                        { ""text"": ""!10"", ""expr"": ""!10"" },
                        { ""text"": ""!(model.number + 10)"", ""expr"": ""!(model.number + 10)"" },
                        { ""text"": ""!(model.number - 20.9)"", ""expr"": ""!(model.number - 20.9)"" },
                        { ""text"": ""!'false'"", ""expr"": ""!'false'"" },
                        { ""text"": ""!model.boolean"", ""expr"": ""!model.boolean"" },
                        { ""text"": ""if(!(model.number &gt; 20), 20, model.number)"", ""expr"": ""if(!(model.number > 20), 20, model.number)"" },
                        { ""text"": ""!(!model.number)"", ""expr"": ""!(!model.number)"" }
                    ]
                }
            ]
        },
        {
            ""id"": ""conversion-func"",
            ""name"": ""Conversion Functions"",
            ""funcs"": [
                {
                    ""id"": ""boolean"",
                    ""name"": ""Boolean Function"",
                    ""desc"": ""The boolean() function will return true if the contained expression can be converted to a true value, otherwise false, or false if the contained expression results in false. The contained expression can be a constant or another expression, and an attempt to convert to boolean will be made."",
                    ""examples"": [
                        { ""text"": ""boolean(1)"", ""expr"": ""boolean(1)"" },
                        { ""text"": ""boolean(0)"", ""expr"": ""boolean(0)"" },
                        { ""text"": ""boolean(-1)"", ""expr"": ""boolean(-1)"" },
                        { ""text"": ""boolean(null)"", ""expr"": ""boolean(null)"" },
                        { ""text"": ""boolean(model.number)"", ""expr"": ""boolean(model.number)"" },
                        { ""text"": ""boolean('')"", ""expr"": ""boolean('')"" },
                        { ""text"": ""boolean('a string')"", ""expr"": ""boolean('a string')"" },
                        { ""text"": ""boolean('false')"", ""expr"": ""boolean('false')"" },
                        { ""text"": ""boolean(model.array)"", ""expr"": ""boolean(model.array)"" }
                    ]
                },
                {
                    ""id"": ""date"",
                    ""name"": ""Date Function"",
                    ""desc"": ""The date function will return the value, of the contained expression, as a date. If no contained expression is provided, then it will return the current date and time. The contained expression can be a constant or another expression, and an attempt to convert to date will be made. If a second *format* parameter is provided, then the date will be parsed according to that format. "",
                    ""notes"": ""Once converted to a date there are many date manipulation functions available to alter the value."",
                    ""examples"": [
                        { ""text"": ""date()"", ""expr"": ""date()"" },
                      {""text"": ""date(1707930243785)"", ""expr"": ""date(1707930243785)""},
                        { ""text"": ""date('12 June 2023')"", ""expr"": ""date('12 June 2023')"" },
                        { ""text"": ""date('12 06 2023', 'dd MM yyyy')"", ""expr"": ""date('12 06 2023', 'dd MM yyyy')"" },
                        { ""text"": ""date('12 06 2023', 'MM dd yyyy')"", ""expr"": ""date('12 06 2023', 'MM dd yyyy')"" },
                        { ""text"": ""string(date('12 06 2023', 'MM dd yyyy'),'dd MMM yyyy')"", ""expr"": ""string(date('12 06 2023', 'MM dd yyyy'),'dd MMM yyyy')"" }
                    ]
                },
                {
                    ""id"": ""decimal"",
                    ""name"": ""Decimal Function"",
                    ""desc"": ""The decimal function will return the value as a decimal of the contained expression. The contained expression can be a constant or another expression, and an attempt to convert to decmial will be made."",
                    ""examples"": [
                        { ""text"": ""decimal(0)"", ""expr"": ""decimal(0)"" },
                        { ""text"": ""decimal('0.25')"", ""expr"": ""decimal('0.25')"" },
                        { ""text"": ""decimal(false)"", ""expr"": ""decimal(false)"" },
                        { ""text"": ""decimal(true)"", ""expr"": ""decimal(true)"" },
                        { ""text"": ""decimal(model.int)"", ""expr"": ""decimal(model.int)"" },
                        { ""text"": ""decimal(model.array)"", ""expr"": ""decimal(model.array)"" }
                    ]
                },
                {
                    ""id"": ""double"",
                    ""name"": ""Double Function"",
                    ""desc"": ""The double function will return the value as a double of the contained expression. The contained expression can be a constant or another expression, and an attempt to convert to double will be made."",
                    ""examples"": [
                        { ""text"": ""double(0)"", ""expr"": ""double(0)"" },
                        { ""text"": ""double('0.25')"", ""expr"": ""double('0.25')"" },
                        { ""text"": ""double(false)"", ""expr"": ""double(false)"" },
                        { ""text"": ""double(true)"", ""expr"": ""double(true)"" },
                        { ""text"": ""double(model.int)"", ""expr"": ""double(model.int)"" },
                        { ""text"": ""double(model.array)"", ""expr"": ""double(model.array)"" }
                    ]
                },
                {
                    ""id"": ""integer"",
                    ""name"": ""Integer Function"",
                    ""desc"": ""The integer function will return the value as an integer of the contained expression. The contained expression can be a constant or another expression, and an attempt to convert to integer will be made. Null or invalid values will cause an error"",
                    ""notes"": ""If the value to be converted to an integer is a floating point (double, decimal) value, then it will be rounded to the nearest integer. If it is a string, then it must be able to be converted to an integral value."",
                    ""examples"": [
                        { ""text"": ""integer(0)"", ""expr"": ""integer(0)"" },
                        { ""text"": ""integer('10')"", ""expr"": ""integer('10')"" },
                        { ""text"": ""integer('10.6')"", ""expr"": ""integer('10.6')"" },
                        { ""text"": ""integer(false)"", ""expr"": ""integer(false)"" },
                        { ""text"": ""integer(true)"", ""expr"": ""integer(true)"" },
                        { ""text"": ""integer(model.int)"", ""expr"": ""integer(model.int)"" },
                        { ""text"": ""integer(model.number)"", ""expr"": ""integer(model.number)"" },
                        { ""text"": ""integer(model.array)"", ""expr"": ""integer(model.array)"" },
                        { ""text"": ""integer(model.notset)"", ""expr"": ""integer(model.notset)"" }
                    ]
                },
                {
                    ""id"": ""string"",
                    ""name"": ""String Function"",
                    ""desc"": ""The string function will return the value of the contained expression as a string. Complex (object and array values will be expanded). A formatting parameter can also be specifified to alter how the value is encoded, and a third parameter for the culture can be specified."",
                    ""notes"": ""The standard .NET format conversion notations can be used, but cultures are not currently set on the expressions. This may be supported in the future, however the formatting string can be bound to a model value, as well, for flexibility."",
                    ""examples"": [
                        { ""text"": ""string(10)"", ""expr"": ""string(10)"" },
                        { ""text"": ""string(10 + 1)"", ""expr"": ""string(10 + 1)"" },
                        { ""text"": ""string(10) + 1"", ""expr"": ""string(10) + 1"" },
                        { ""text"": ""string(false)"", ""expr"": ""string(false)"" },
                        { ""text"": ""string(true)"", ""expr"": ""string(true)"" },
                        { ""text"": ""string(model.int)"", ""expr"": ""string(model.int)"" },
                        { ""text"": ""string(model.number)"", ""expr"": ""string(model.number)"" },
                        { ""text"": ""string(model.array)"", ""expr"": ""string(model.array)"" },
                        { ""text"": ""string(model.items[1])"", ""expr"": ""string(model.items[1])"" },
                        { ""text"": ""string(model.notset)"", ""expr"": ""string(model.notset)"" },
                        { ""text"": ""string(10.3456,'##0.00')"", ""expr"": ""string(10.3456,'##0.00')"" },
                        { ""text"": ""string(10.3456,'000.00#')"", ""expr"": ""string(10.3456,'000.00#')"" },
                        { ""text"": ""string(10.3456,'£#,##0.00')"", ""expr"": ""string(10.3456,'£#,##0.00')"" },
                        { ""text"": ""string(model.number + 1,'# ##0,00 €')"", ""expr"": ""string(model.number + 1,'# ##0,00 €')"" },
                        { ""text"": ""string(date())"", ""expr"": ""string(date())"" },
                        { ""text"": ""string(date(),'D')"", ""expr"": ""string(date(),'D')"" },
                        { ""text"": ""string(date(),'dd MMM yyyy')"", ""expr"": ""string(date(), 'dd MMM yyyy')"" },
                        { ""text"": ""string(integer(model.number))"", ""expr"": ""string(integer(model.number))"" },
                        { ""text"": ""string(date('12 06 2023', 'MM dd yyyy'),'D')"", ""expr"": ""string(date('12 06 2023', 'MM dd yyyy'),'D')"" }
                    ]
                }

            ]
        }
    ]
}";
        private const string func_Names = "Plus (+) operator,Minus (-) operator,Multiply (*) operator,Divide (/) operator,Modulo (%) operator,Exponent (^) operator,Bitwise And (&) operator,Bitwise Or (|) operator,Bitwise Shift Left (<<) operator,Bitwise Shift Right (>>) operator,Null coalescing (??) operator,Combining binary operators,Equal (==) operator,Not Equal (!=) operator,Less than (<) operator,More than (>) operator,Less than or equal (<=) operator,Greater than or equal(>=) operator,And (&&) operator,Or (||) operator,Not (!) operator,Boolean Function,Date Function,Decimal Function,Double Function,Integer Function,String Function";

        private const string sortedSectionNames =
            "Binary Operators,Conversion Functions,Logical Operators,Relational Operators";
        
        private object GetCollectData()
        {
            object parsed = System.Text.Json.JsonSerializer.Deserialize(collectdata, typeof(object));
            return parsed;
        }
        
        [TestMethod]
        public void CollectFunctionTest()
        {
            //get each of the func arrays in the model.sections.
            //collect each array together into a single array.
            //for each of the names in the items in this array
            //join as a single string
            
            var content = GetContent("join(',',eachOf(collect(eachOf(model.sections, .funcs)), .name))");
            using var stream = new System.IO.StringReader(content);
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent);
            doc.Params["model"] = GetCollectData();

            using ( var output = DocStreams.GetOutputStream("CollectFunction.pdf"))
            {
                doc.SaveAsPDF(output);
            }
            
            var h1 = doc.FindAComponentById("boundContent") as Head1;
            
            //and compare to the known value of each of the function names.
            
            AssertBoundContent(h1, func_Names);
        }
        
        [TestMethod]
        public void SortFunctionTest()
        {
            //get each of the func arrays in the model.sections.
            //collect each array together into a single array.
            //for each of the names in the items in this array
            //join as a single string
            
            var content = GetContent("join(',',eachOf(sortBy(model.sections, .name), .name))");
            using var stream = new System.IO.StringReader(content);
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent);
            doc.Params["model"] = GetCollectData();

            using ( var output = DocStreams.GetOutputStream("SortByFunction.pdf"))
            {
                doc.SaveAsPDF(output);
            }
            
            var h1 = doc.FindAComponentById("boundContent") as Head1;
            
            //and compare to the known value of each of the function names.
            
            AssertBoundContent(h1, sortedSectionNames);
        }
	}
}

