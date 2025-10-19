using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Scryber.Generation
{
    public class HBarHelperSplitter
    {
        

        
        private static readonly Regex _helper_old = new Regex("{{#([a-z]+)\\s*(.*)*}}|{{\\/([a-z]*)}}|{{else}}|{{else\\s+if\\s(.*)}}");

        private static readonly Regex _helper =
            new Regex("{{#([a-z]+)\\s*([^{])*}}|{{\\/([a-z]*)}}|{{(else)}}|{{(else\\s+if)\\s(.*)}}");

        private static readonly Dictionary<string, HBarHelperMapping> _knownMappings =
            new Dictionary<string, HBarHelperMapping>()
            {
                { "using", new HBarUsing() },
                { "with", new HBarWith() },
                { "equals", new HBarEquals()},
                { "if", new HBarIf()},
                {"each", new HBarEach()},
                {"else", new HBarElse()},
                {"else if", new HBarElseIf()}
            };
        
        private string _mappingNamespace;
        private string _mappingPrefix;

        public string MappingNamespace
        {
            get => _mappingNamespace;
            set => _mappingNamespace = value;
        }

        public string MappingPrefix
        {
            get => _mappingPrefix;
            set => _mappingPrefix = value;
        }
        
        private Dictionary<string, HBarHelperMapping> _keyedHelpers;

        public HBarHelperSplitter() : this(XHtmlHandleHelperReader.HandlebarNamespace,
            XHtmlHandleHelperReader.HandlebarPrefix)
        {
        }

        public HBarHelperSplitter(string mappingNamespace, string mappingPrefix) : this(mappingNamespace, mappingPrefix, _knownMappings)
        {}

        
        public HBarHelperSplitter(string mappingNamespace, string mappingPrefix,
            params HBarHelperMapping[] mappingHelpers)
        {
            this._mappingNamespace = mappingNamespace;
            this._mappingPrefix = mappingPrefix;
            this._keyedHelpers = new Dictionary<string, HBarHelperMapping>();
            
            foreach (var helper in mappingHelpers)
            {
                this._keyedHelpers[helper.Match] = helper;
            }
        }

        private HBarHelperSplitter(string mappingNamespace, string mappingPrefix,
            Dictionary<string, HBarHelperMapping> mappings)
        {
            this._mappingNamespace = mappingNamespace;
            this._mappingPrefix = mappingPrefix;
            
            this._keyedHelpers = mappings;
        }
        
        public string ReplaceAll(string input)
        {
            Stack<Match> looping = new Stack<Match>();
            const int GlobalMatchIndex = 0;
            const int HelperStartIndex = 1;
            const int HelperEndIndex = 3;
            const int HelperElseIndex = 4;
            const int HelperElseIfIndex = 5;
            
            var updated = _helper.Replace(input, (match) =>
            {
                var name = match.Groups[HelperStartIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var starthelper))
                    return starthelper.Replace(this, looping, match);
                
                name = match.Groups[HelperEndIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var endhelper))
                    return endhelper.Replace(this, looping, match);

                name = match.Groups[GlobalMatchIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var globalHelper))
                    return globalHelper.Replace(this, looping, match);

                name = match.Groups[HelperElseIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var elseHelper))
                    return elseHelper.Replace(this, looping, match);
                
                name = match.Groups[HelperElseIfIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var elseifHelper))
                    return elseifHelper.Replace(this, looping, match);
                
                return "";

            });


            return updated;
        }

    }

    public delegate string MatchReplacer(HBarHelperSplitter splitter, Stack<Match> currentStack, Match newMatch);

    public class HBarHelperMapping
    {
        public string Match { get; set; }
        public MatchReplacer Replace { get; set; }

        public HBarHelperMapping(string match, MatchReplacer replace)
        {
            this.Match = match;
            this.Replace = replace;
        }
    }

    public class HBarEquals : HBarHelperMapping
    {

        public HBarEquals() : base("equals", new MatchReplacer(ReplaceEquals))
        {
        }

        static string ReplaceEquals(HBarHelperSplitter splitter, Stack<Match> matches, Match newMatch)
        {
            return "";
        }
    }

    public class HBarUsing : HBarHelperMapping
    {
        public HBarUsing() : base("using", new MatchReplacer(ReplaceUsing))
        {}

        static string ReplaceUsing(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            var result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{#using"))
            {
                if (value.StartsWith("{{#using "))
                {
                    var path = "";
                    value = value.Substring(9); //remove '{{#using '
                    if (value.Length > 2 && value.EndsWith("}}"))
                    {
                        value = value.Substring(0, value.Length - 2); //remove the '}}' at the end.
                        value = value.Trim();
                        path = "data-bind='{{" + value + "}}'";
                    }
                    else
                    {
                        path = "data-bind='{{.}}'";
                    }

                    result = "<" + splitter.MappingPrefix + ":using " + splitter.MappingPrefix +":xmlns='" + splitter.MappingNamespace + "' " + path +
                             " >";
                    tracker.Push(newMatch);
                }
                else if (value.StartsWith("{{/using"))
                {
                    var prev = tracker.Pop();
                    if (prev.Value.StartsWith("{{#using"))
                        result = "</" + splitter.MappingPrefix + ":using>";
                    else
                    {
                        throw new InvalidOperationException("The '" + prev.Value +
                                                            "' does not match the end /using statement in the content.");
                    }
                }
                else
                {
                    result = "<" + splitter.MappingPrefix + ":using xmlns='" + splitter.MappingNamespace + "' data-bind='{{.}}'>";
                    tracker.Push(newMatch);
                }
            }

            return result;
        }
    }
    
    public class HBarEach : HBarHelperMapping
    {
        public HBarEach() : base("each", new MatchReplacer(ReplaceWith))
        {}

        static string ReplaceWith(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            var result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{#each"))
            {
                if (value.StartsWith("{{#each "))
                {
                    var path = "";
                    value = value.Substring(7); //remove '{{#each '
                    if (value.Length > 2 && value.EndsWith("}}"))
                    {
                        value = value.Substring(0, value.Length - 2); //remove the '}}' at the end.
                        value = value.Trim();
                        path = "data-bind='{{" + value + "}}' ";


                        result = "<" + splitter.MappingPrefix + ":each " + splitter.MappingPrefix + ":xmlns='" +
                                 splitter.MappingNamespace + "' " + path +
                                 ">";
                        tracker.Push(newMatch);
                    }
                }
                else
                {
                    result = "<" + splitter.MappingPrefix + ":each " + splitter.MappingPrefix + ":xmlns='" + splitter.MappingNamespace + "' data-bind='{{.}}' >";
                    tracker.Push(newMatch);
                }
            }
            else if (value.StartsWith("{{/each"))
            {
                var prev = tracker.Pop();
                if (prev.Value.StartsWith("{{#each"))
                    result = "</" + splitter.MappingPrefix + ":each>";
                else
                {
                    throw new InvalidOperationException("The '" + prev.Value +
                                                        "' does not match the end /each statement in the content.");
                }
            }

            return result;
        }
    }
    
    public class HBarWith : HBarHelperMapping
    {
        public HBarWith() : base("with", new MatchReplacer(ReplaceWith))
        {}

        static string ReplaceWith(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            
            
            return "";
        }
    }
    
    public class HBarIf : HBarHelperMapping
    {
        public HBarIf() : base("if", new MatchReplacer(ReplaceIf))
        {}

        static string ReplaceIf(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            string result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{#if "))
            {
                var path = "";
                value = value.Substring(6); //remove '{{#if '
                if (value.Length > 2 && value.EndsWith("}}"))
                {
                    value = value.Substring(0, value.Length - 2); //
                    value = value.Trim();
                    path = "data-test='{{" + value + "}}' ";
                    
                    result = "<" + splitter.MappingPrefix + ":if " + splitter.MappingPrefix + ":xmlns='" +
                             splitter.MappingNamespace + "' " + path +
                             ">";
                    tracker.Push(newMatch);
                }
            }
            else if(value.StartsWith("{{/if"))
            {
                var ended = false;
                while (!ended)
                {
                    var prev = tracker.Pop();

                    if (prev.Value.StartsWith("{{else if"))
                    {
                        result += "</" + splitter.MappingPrefix + ":elseif>";
                    }
                    else if (prev.Value.StartsWith("{{else"))
                    {
                        result += "</" + splitter.MappingPrefix + ":else>";
                    }
                    else if (prev.Value.StartsWith("{{#if"))
                    {
                        result += "</" + splitter.MappingPrefix + ":if>";
                        ended = true;
                    }
                    else
                    {
                        throw new Scryber.PDFParserException(
                            "The handle bar helper sequence is not balanced found a match on '" + prev.Value +
                            "' when expecting to end an 'if' clause at index:" + prev.Index);
                    }
                }
            }
            return result;
        }
    }
    
    public class HBarElse : HBarHelperMapping
    {
        public HBarElse() : base("else", new MatchReplacer(ReplaceElse))
        {}

        static string ReplaceElse(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            string result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{else"))
            {
                var path = "";
                value = value.Substring(6); //remove '{{#if '
                if (value.Length >= 2 && value.EndsWith("}}"))
                {
                    result = "<" + splitter.MappingPrefix + ":else>";
                    tracker.Push(newMatch);
                }
            }
            else
                ;//else does not have an end token
            
            return result;
        }
    }

    public class HBarElseIf : HBarHelperMapping
    {
        public HBarElseIf(): base("else if", new MatchReplacer(ReplaceElseIf))
        {}

        static string ReplaceElseIf(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            string result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{else if "))
            {
                var path = "";
                value = value.Substring(10); //remove '{{else if '
                if (value.Length > 2 && value.EndsWith("}}"))
                {
                    value = value.Substring(0, value.Length - 2); //
                    value = value.Trim();
                    path = "data-test='{{" + value + "}}' ";
                    
                    result = "<" + splitter.MappingPrefix + ":elseif " + path +
                             ">";
                    tracker.Push(newMatch);
                }
            }
            else
                ;//else if does not have an end token
            return result;
        }
    }
}

