using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Scryber.Generation.Handlebars
{
    public class HBarWith : HBarHelperMapping
    {
        public HBarWith() : base("with", new HandlebarMatchReplacer(ReplaceWith))
        {}

        static string ReplaceWith(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            string result = "";

            var value = newMatch.Value.Trim();

            if (value.StartsWith("{{#with "))
            {
                var path = "";
                value = value.Substring(7); //remove {{#with
                if (value.Length > 2 && value.EndsWith("}}"))
                {
                    value = value.Substring(0, value.Length - 2).Trim();

                    var hasAs = false;
                    var newName = "";
                    var asIndex = value.IndexOf(" as ");
                    if (asIndex > 0)
                    {
                        hasAs = TryExtractVariableName(asIndex, value, ref value, ref newName);
                        if (!hasAs)
                            throw new PDFParserException("Could not understand the with statement " + newMatch.Value +
                                                         ". Expected an 'as | newName |' reference");
                        
                    }
                    path = "data-bind=\"{{" + SanitizeBindExpression(value) + "}}\" ";

                    //Start the outer wrapper
                    result = "<" + splitter.MappingPrefix + ":with xmlns:" + splitter.MappingPrefix + "='" +
                             splitter.MappingNamespace + "' " + path + " >";
                    
                    //Start the with content
                    result += "<" + splitter.MappingPrefix + ":withContent ><Template>";

                    if (hasAs)
                        result += "\n\t\t<var xmlns='http://www.w3.org/1999/xhtml' data-id='" + newName + "' data-value='{{.}}' />";
                    
                    //Track it
                    tracker.Push(newMatch);
                }

            }
            else if (value.StartsWith("{{/with"))
            {
                if (tracker.Count <= 0)
                    throw new PDFParserException(
                        "The Handlebars stack is un-balanced. Expecting an existing {{#with to have been previously processed.");

                var prev = tracker.Pop();
                if (prev.Value.StartsWith("{{#with"))
                {
                    result = "</Template></" + splitter.MappingPrefix + ":withContent>";
                }
                else if (prev.Value.StartsWith("{{else"))
                {
                    result = "</Template></" + splitter.MappingPrefix + ":elseContent>";
                }
                else
                {
                    throw new Scryber.PDFParserException(
                        "The handle bar helper sequence is not balanced found a match on '" + prev.Value +
                        "' when expecting to end an 'with' or 'else' clause at index:" + prev.Index);
                }

                result += "</" + splitter.MappingPrefix + ":with>"; //finally end the outer with.
            }
            
            return result;
        }

        static bool TryExtractVariableName(int asIndex, string content, ref string newpath, ref string newName)
        {
            var predictedAs = content.Substring(asIndex);
            var predictedPath = content.Substring(0, content.Length - (asIndex-1)).Trim();
            
            var offset = predictedAs.IndexOf('|');
            
            if (offset > 0 && offset < predictedAs.Length)
                predictedAs = predictedAs.Substring(offset + 1);
            else
                return false;
            
            offset = predictedAs.IndexOf('|');

            if (offset > 0)
                predictedAs = predictedAs.Substring(0, offset);
            else
                return false;

            predictedAs = predictedAs.Trim();
            
            if (predictedAs.IndexOf(" ") >= 0)
                return false;

            newName = predictedAs;
            newpath = predictedPath;
            return true;

        }
    }
}