using System;
namespace Scryber.Utilities
{
    public class LocalFilePathMappingService : IPDFPathMappingService
    {
        public LocalFilePathMappingService()
        {
        }

        public string MapPath(ParserLoadType loadtype, string reference, string parent, out bool isFile)
        {
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

            if (System.IO.Path.IsPathRooted(reference))
                return reference;
            else if (reference.StartsWith("~"))
                return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.Environment.CurrentDirectory, reference.Substring(1)));
            else if (!string.IsNullOrEmpty(parent))
                return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(parent), reference));
            else
                return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.Environment.CurrentDirectory, reference));
            
        }
    }
}
