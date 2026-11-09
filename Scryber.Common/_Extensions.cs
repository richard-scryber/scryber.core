using Scryber.PDF.Native;
using Scryber.PDF;

namespace Scryber
{
    /// <summary>
    /// Adds Extension methods to the PDFArray class
    /// </summary>
    public static class PDFArrayExtensions
    {
        public static T[] ContentAs<T>(this PDFArray ary) where T : IPDFFileObject
        {
            if (null == ary)
                return null;

            T[] all = new T[ary.Count];
            for (int i = 0; i < ary.Count; i++)
            {
                all[i] = (T)ary[i];
            }
            return all;
        }
    }
}