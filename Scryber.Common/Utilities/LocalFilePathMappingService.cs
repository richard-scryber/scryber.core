using System;
using System.Text.RegularExpressions;

namespace Scryber.Utilities
{
    public class LocalFilePathMappingService : IPathMappingService
    {
        public LocalFilePathMappingService()
        {
        }

        public string MapPath(ParserLoadType loadtype, string reference, string parent, out bool isFile)
        {
            if (IsDataUrl(reference))
            {
                isFile = false;
                return reference;
            }
            if(Uri.IsWellFormedUriString(reference, UriKind.Absolute))
            {
                isFile = false;
                return reference;
            }
            else if(Uri.IsWellFormedUriString(reference, UriKind.Relative) && !string.IsNullOrEmpty(parent) && Uri.IsWellFormedUriString(parent, UriKind.Absolute))
            {
                if(reference.StartsWith("/"))
                {
                    // a starting / on the reference is from the beginning of the authority
                    isFile = false;
                    Uri uri = new Uri(parent);
                    return uri.Scheme + "://" + uri.Authority + reference;
                }

                if(!parent.EndsWith("/")) // we have a file as the parent reference so go up one.
                {
                    parent = parent.Substring(0, parent.LastIndexOf("/"));
                }
                while (reference.StartsWith("../")) // traverse up the tree for ../
                {
                    parent = parent.Substring(0, parent.LastIndexOf("/"));
                    reference = reference.Substring(3);
                }

                if (reference.StartsWith("./")) // ./ is where we are at.
                    reference = reference.Substring(2);

                isFile = false;
                if (parent.EndsWith("/"))
                {
                    if (reference.StartsWith("/"))
                        return parent + reference.Substring(1);
                    else
                        return parent + reference;
                }
                else if (reference.StartsWith("/"))
                    return parent + reference;
                else
                    return parent + "/" + reference;
            }

            isFile = true;

            //Replace any alternate separator chars with the actual character.
            var dirSep = System.IO.Path.DirectorySeparatorChar;

            if (dirSep == '/')
                reference = reference.Replace('\\', '/');
            else if (dirSep == '\\')
                reference = reference.Replace('/', '\\');

            if (System.IO.Path.IsPathRooted(reference))
                return reference;
            else if (reference.StartsWith("~"))
            {
                var dir = System.Environment.CurrentDirectory;
                var combined = System.IO.Path.Combine(dir, reference.Substring(1));
                var clean = System.IO.Path.GetFullPath(combined);
                return clean;
            }
            else if (!string.IsNullOrEmpty(parent))
            {
                var dir = System.IO.Path.GetDirectoryName(parent);
                var combined = System.IO.Path.Combine(dir, reference);
                var clean = System.IO.Path.GetFullPath(combined);
                return clean;
            }
            else
            {
                var dir = System.Environment.CurrentDirectory;
                var combined = System.IO.Path.Combine(dir, reference);
                var clean = System.IO.Path.GetFullPath(combined);
                return clean;
            }
            
        }
        
        private static readonly Regex DataUrlMatch = new Regex("^\\s*data:([a-z0-9]+)\\/([a-z0-9\\+]+\\;).*",
            RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        public bool IsDataUrl(string path)
        {
            if (DataUrlMatch.IsMatch(path))
                return true;

            return false;
        }
    }
}
