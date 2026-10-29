using System;

namespace Scryber.PDF.Secure
{
    /// <summary>
    /// A managed MD5 (RFC 1321) hash, for the PDF standard security handler.
    /// </summary>
    /// <remarks>
    /// The PDF format needs MD5 to make its encryption keys, and System.Security.Cryptography.MD5 is not
    /// available in the browser (Blazor WASM), so the hash is calculated here on every platform.
    /// It is only used because the format requires it, and not for any other security.
    /// </remarks>
    public static class MD5Managed
    {
        /// <summary>
        /// The size of the hash in bytes
        /// </summary>
        public const int HashSize = 16;

        private const int BlockSize = 64;

        // the per-round shift amounts
        private static readonly int[] _shifts = new int[]
        {
            7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
            5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
            4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
            6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        };

        // the integer part of abs(sin(i + 1)) * 2^32
        private static readonly uint[] _sines = new uint[]
        {
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee, 0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be, 0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa, 0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed, 0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c, 0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05, 0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039, 0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1, 0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        };

        /// <summary>
        /// Returns the 16 byte MD5 hash of all the data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] data)
        {
            if (null == data)
                throw new ArgumentNullException(nameof(data));

            return ComputeHash(data, 0, data.Length);
        }

        /// <summary>
        /// Returns the 16 byte MD5 hash of the count bytes in data from the offset
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] data, int offset, int count)
        {
            if (null == data)
                throw new ArgumentNullException(nameof(data));
            if (offset < 0 || count < 0 || offset + count > data.Length)
                throw new ArgumentOutOfRangeException(nameof(count));

            uint a0 = 0x67452301;
            uint b0 = 0xefcdab89;
            uint c0 = 0x98badcfe;
            uint d0 = 0x10325476;

            uint[] words = new uint[16];

            //the whole blocks in the data
            int whole = count / BlockSize;
            for (int block = 0; block < whole; block++)
            {
                ReadBlock(data, offset + (block * BlockSize), words);
                HashBlock(words, ref a0, ref b0, ref c0, ref d0);
            }

            //the remaining bytes, then 0x80, zeros, and the length in bits as 8 low order bytes,
            //padded to the end of one block, or two if the length does not fit after the remaining bytes
            int remain = count - (whole * BlockSize);
            byte[] tail = new byte[remain < BlockSize - 8 ? BlockSize : BlockSize * 2];
            Array.Copy(data, offset + (whole * BlockSize), tail, 0, remain);
            tail[remain] = 0x80;

            ulong bits = (ulong)count * 8;
            for (int i = 0; i < 8; i++)
                tail[tail.Length - 8 + i] = (byte)(bits >> (8 * i));

            for (int start = 0; start < tail.Length; start += BlockSize)
            {
                ReadBlock(tail, start, words);
                HashBlock(words, ref a0, ref b0, ref c0, ref d0);
            }

            byte[] hash = new byte[HashSize];
            WriteWord(a0, hash, 0);
            WriteWord(b0, hash, 4);
            WriteWord(c0, hash, 8);
            WriteWord(d0, hash, 12);
            return hash;
        }

        private static void ReadBlock(byte[] data, int start, uint[] words)
        {
            //each word is 4 bytes with the low order byte first
            for (int i = 0; i < 16; i++)
            {
                int pos = start + (i * 4);
                words[i] = (uint)data[pos]
                           | ((uint)data[pos + 1] << 8)
                           | ((uint)data[pos + 2] << 16)
                           | ((uint)data[pos + 3] << 24);
            }
        }

        private static void WriteWord(uint value, byte[] hash, int start)
        {
            hash[start] = (byte)value;
            hash[start + 1] = (byte)(value >> 8);
            hash[start + 2] = (byte)(value >> 16);
            hash[start + 3] = (byte)(value >> 24);
        }

        private static void HashBlock(uint[] words, ref uint a0, ref uint b0, ref uint c0, ref uint d0)
        {
            uint a = a0;
            uint b = b0;
            uint c = c0;
            uint d = d0;

            for (int i = 0; i < 64; i++)
            {
                uint f;
                int g;

                if (i < 16)
                {
                    f = (b & c) | (~b & d);
                    g = i;
                }
                else if (i < 32)
                {
                    f = (d & b) | (~d & c);
                    g = ((5 * i) + 1) % 16;
                }
                else if (i < 48)
                {
                    f = b ^ c ^ d;
                    g = ((3 * i) + 5) % 16;
                }
                else
                {
                    f = c ^ (b | ~d);
                    g = (7 * i) % 16;
                }

                f = unchecked(f + a + _sines[i] + words[g]);
                a = d;
                d = c;
                c = b;
                b = unchecked(b + RotateLeft(f, _shifts[i]));
            }

            a0 = unchecked(a0 + a);
            b0 = unchecked(b0 + b);
            c0 = unchecked(c0 + c);
            d0 = unchecked(d0 + d);
        }

        private static uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }
    }
}
