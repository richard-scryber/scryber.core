using System;
using Scryber;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Scryber.Generation.Handlebars
{
    public class HBarLog : HBarHelperMapping
    {
        public HBarLog() : base("log", new HandlebarMatchReplacer(ReplaceLog))
        {}

        static string ReplaceLog(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            var result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{log "))
            {

                var content = "";
                value = value.Substring(5); //remove '{{log'
                if (value.Length > 2 && value.EndsWith("}}"))
                {
                    value = value.Substring(0, value.Length - 2); //remove the '}}' at the end.
                    value = value.Trim();
                    SplitLogParts(value, out var message, out var level, out var category, out var stage);
                    message = ConcatenateMessage(message);
                    content = BuildLogHelper(splitter, message, level, category, stage);
                }
                else
                {
                    content = "message='Log Entry Found'";
                }

                result = "<" + splitter.MappingPrefix + ":log ";
                if (tracker.Count == 0)
                    result += " xmlns:" + splitter.MappingPrefix + "='" + splitter.MappingNamespace + "' ";
                result += content + " />";
               
                
            }

            return result;
        }

        private static readonly char[] breakChars = new char[] { '"', '\'', '(' };
        private static string ConcatenateMessage(string message)
        {
            
            StringBuilder sb = new StringBuilder(message.Length);
            var start = 0;
            int count = 0;
            var index = start;
            var length = message.Length;
            var isEscaped = false;
            
            while (index < length)
            {
                var curr = message[index];
                if (curr == '"')
                {
                    if (count > 0)
                        sb.Append(", ");
                    index = ConcatenateQuote(sb, message, index, '"');
                    count++;

                }
                else if (curr == '\'')
                {
                    if (count > 0)
                        sb.Append(", ");
                    index = ConcatenateQuote(sb, message, index, '\'');
                    count++;
                }
                else if (curr == '(')
                {
                    if (count > 0)
                        sb.Append(", ");
                    index = ConcatenateExpression(sb, message, index);
                    count++;
                }
                else if (char.IsLetterOrDigit(curr) || curr == '_' || curr == '@')
                {
                    if (count > 0)
                        sb.Append(", ");
                    index = ConcatenateExpression(sb, message, index);
                    count++;
                }
                
                index++;
            }

            //wrap in a binding expression
            sb.Insert(0, "\"{{concat(");
            sb.Append(")}}\"");
            
            

            return sb.ToString();
        }

        private static int ConcatenateQuote(StringBuilder sb, string value, int currIndex, char ending)
        {
            int newIndex = currIndex + 1;
            bool isEscaped = false;
            
            while (newIndex < value.Length)
            {
                var curr = value[newIndex];

                if (curr == ending)
                {
                    if (isEscaped)
                        isEscaped = false; //skip but also clear the flog
                    else
                    {
                        var quoted = value.Substring(currIndex, (newIndex + 1) - currIndex);
                        quoted = SanitizeBindExpression(quoted);
                        sb.Append(quoted); //currIndex = the quote, new index = the quote so length of string in quotes is +1
                        return newIndex;
                    }
                }
                else if (curr == '\\')
                {
                    if (isEscaped)
                        isEscaped = false; //double escape
                    else
                        isEscaped = true;
                }

                newIndex++;
            }

            throw new ArgumentOutOfRangeException("Unclosed " + ending + " in log expression " + value);
        }

        private static int ConcatenateExpression(StringBuilder sb, string value, int currIndex)
        {
            int newIndex = currIndex;
            int depth = 0;
            string part = "";
            
            if (value[newIndex] == '(')
            {
                depth++;
                newIndex++;
            }
            
            while (newIndex < value.Length)
            {
                if (char.IsWhiteSpace(value, newIndex))
                {
                    if (depth == 0)
                    {
                        //extract and return
                        
                        part = value.Substring(currIndex, (newIndex + 1) - currIndex);
                        part = SanitizeBindExpression(part);
                        sb.Append(part);
                        return newIndex;
                    }
                }
                else if (value[newIndex] == '(')
                {
                    depth++;
                }
                else if (value[newIndex] == ')')
                {
                    depth--;
                    if (depth == 0)
                    {
                        //extract and return
                        part = value.Substring(currIndex, (newIndex + 1) - currIndex);
                        part = SanitizeBindExpression(part);
                        sb.Append(part);
                        return newIndex;
                    }
                }

                newIndex++;
            }
            
            //end of the string, so just return.
            part = value.Substring(currIndex, newIndex - currIndex);
            part = SanitizeBindExpression(part);
            sb.Append(part);
            return newIndex;
            
        }
        private static readonly Regex MatchParameters = new Regex("([a-zA-Z_0-9]+)=(\"[a-zA-Z_ 0-9]*\"|'[a-zA-Z_ 0-9]*')");
        private static void SplitLogParts(string value, out string message, out string level, out string category, out string stage)
        {
            value = value.Trim();
            bool hasMessage = false;
            bool hasBinding = false;
            message = string.Empty;
            
            var foundLevel = string.Empty;
            var foundCategory = string.Empty;
            var foundStage = string.Empty;
            
            value = MatchParameters.Replace(value, (match) =>
            {
                var keyword = match.Groups[1].Value;
                var keyvalue = match.Groups[2].Value;
                keyvalue = keyvalue.Substring(1, keyvalue.Length - 2);

                if (keyword == "level")
                    foundLevel = keyvalue;
                else if (keyword == "category")
                    foundCategory = keyvalue;
                //else if (keyword == "stage") - not using
                //    foundStage = keyvalue;

                return string.Empty;
            });

            level = foundLevel;
            category = foundCategory;
            stage = foundStage;

            message = value.Trim();
        }

        private static string ExtractFirstMessage(string value, char breakChar, out string message)
        {
            message = string.Empty;
            return value;
        }
        
        private static string ExtractFirstStatement(string value, char breakChar, out string message)
        {
            message = string.Empty;
            return value;
        }

        private static string ExtractLogPart(string value, out string part, int partStartIndex, string partName)
        {
            if (value.Length > partStartIndex + partName.Length + 2)
            {
                int start = partStartIndex + partName.Length;
                
                if (value[start] == '"')
                {
                    int curr = start + 1;
                    curr++;
                    while (curr < value.Length)
                    {
                        if (value[curr] == '"')
                        {
                            //Extract the part and get out
                            part = value.Substring(start + 1, curr - (start +1));
                            value = value.Substring(0, partStartIndex) + value.Substring(curr + 1);
                            return value;
                        }
                        
                        curr++;
                    }
                }
                else if (value[start] == '\'')
                {
                    int curr = start + 1;
                    curr++;
                    while (curr < value.Length)
                    {
                        if (value[curr] == '\'')
                        {
                            //Extract the part and get out
                            part = value.Substring(start + 1, curr - (start+1));
                            value = value.Substring(0, partStartIndex) + value.Substring(curr + 1);
                            return value;
                        }
                        
                        curr++;
                    }
                }
            }
            part = string.Empty;
            value = value.Substring(partStartIndex, partName.Length);
            

            return value;
        }

        private static string BuildLogHelper(HBarHelperSplitter splitter, string message, string level, string category, string stage)
        {
            string content = "";
            
            if(!string.IsNullOrEmpty(message))
                content += "data-message=" + message + " ";
            

            if (!string.IsNullOrEmpty(level))
            {
                switch (level)
                {
                    case("debug"):
                        content += "data-level=\"" + TraceLevel.Verbose + "\" ";
                        break;
                    case("info"):
                        content += "data-level=\"" + TraceLevel.Message + "\" ";
                        break;
                    case("warn"):
                        content += "data-level=\"" + TraceLevel.Warning + "\" ";
                        break;
                    case("error"):
                        content += "data-level=\"" + TraceLevel.Error + "\" ";
                        break;
                    default:
                        content += "data-level=\"" + TraceLevel.Message + "\" ";
                         break;
                }
            }
            
            if (!string.IsNullOrEmpty(category))
            {
                category = SanitizeBindExpression(category);
                content += "data-category=\"" + category + "\" ";
            }
            
            if (!string.IsNullOrEmpty(stage))
            {
                if (EnumParser.TryParse(typeof(LogEntryStages), stage, true, out var stages))
                    content += "data-log-stage=\"" + stages + "\" ";

            }
            
            return content;
        }
    }
}