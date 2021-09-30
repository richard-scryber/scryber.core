/*
 * Added the compression library from the PDFFileWriter project.
 * With thanks to Uzi for this one
 */

/////////////////////////////////////////////////////////////////////
//
//	PDF File Write C# Class Library.
//
//	DeflateMethod
//	Class designed to compress one file using the Deflate method
//	of compression.
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: April 1, 2013
//	Copyright (C) 2013 Granotech Limited. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software.
//	They is distributed under the Code Project Open License (CPOL).
//	The document PdfFileWriterReadmeAndLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	Version History:
//
//	Version 1.0 2013/04/01
//		Original revision
//
/////////////////////////////////////////////////////////////////////

using System;

namespace Scryber.PDF
{
    internal delegate void WriteBits(Int32 Bits, Int32 Count);

    /////////////////////////////////////////////////////////////////////
    // Deflate ZLib
    /////////////////////////////////////////////////////////////////////

    internal class PDFDeflateZLib : DeflateMethod
    {
        ////////////////////////////////////////////////////////////////////
        // Constructor
        ////////////////////////////////////////////////////////////////////

        public PDFDeflateZLib() : base() { }

        ////////////////////////////////////////////////////////////////////
        // Compress byte array
        ////////////////////////////////////////////////////////////////////

        public Byte[] Compress
                (
                Byte[] UserReadBuf
                )
        {
            // input buffer too small to compress
            if (UserReadBuf.Length < 16) return (null);

            // make write buffer equal in length to read buffer
            // if during the compression we reach the end, we do not need compression
            Byte[] UserWriteBuf = new Byte[UserReadBuf.Length];

            // reset adler32 checksum
            UInt32 ReadAdler32 = 1;
            ReadAdler32 = Adler32.Checksum(ReadAdler32, UserReadBuf, 0, UserReadBuf.Length);

            // Header is made out of 16 bits [iiiicccclldxxxxx]
            // iiii is compression information. It is WindowBit - 8 in this case 7. iiii = 0111
            // cccc is compression method. Deflate (8 dec) or Store (0 dec)
            // The first byte is 0x78 for deflate
            // ll is compression level 0 to 3 (in our case it is 2)
            // d is preset dictionary. The preset dictionary is not supported by this program. d is always 0
            // xxx is 5 bit check sum 0x4c

            // write two bytes in most significant byte first
            UserWriteBuf[0] = (Byte)0x78;
            UserWriteBuf[1] = (Byte)0x9c;
            WritePtr = 2;

            // trap exceptions
            try
            {
                // compress the file (reserve space for Adler plus one byte to make it all worth while
                Compress(UserReadBuf, UserWriteBuf, WritePtr, UserWriteBuf.Length - 5);
            }

            // compression failed
            catch (ApplicationException AE)
            {
                if (AE.Message == "No compression") return (null);
                throw;
            }

            // ZLib checksum is Adler32 write it big endian order, high byte first
            UserWriteBuf[WritePtr++] = (Byte)(ReadAdler32 >> 24);
            UserWriteBuf[WritePtr++] = (Byte)(ReadAdler32 >> 16);
            UserWriteBuf[WritePtr++] = (Byte)(ReadAdler32 >> 8);
            UserWriteBuf[WritePtr++] = (Byte)ReadAdler32;

            Byte[] NewBuf = new Byte[WritePtr];
            Array.Copy(UserWriteBuf, NewBuf, WritePtr);

            // successful exit
            return (NewBuf);
        }
    }

    /////////////////////////////////////////////////////////////////////
    // Deflate Method
    /////////////////////////////////////////////////////////////////////

    internal class DeflateMethod
    {
        // User compression level choices. The range is 0 to 9.
        // From no compression level=0, to best compression level 9.
        // No compression is the fastest, best compression is the slowest.
        // The default compression level is a optimum selection between speed and compression ratio.
        // No compression level=0 will result in CompFunc.Stored
        // Compression level=1 to 3 will result in CompFunc.Fast
        // Compression level=4 to 9 will result in CompFunc.Slow
        protected const Int32 DefaultCompression = 3;

        // These 5 arrays define match process constants one for each of the 10 levels.
        // The constants control the amount of time the program will spend trying to find a better match
        private const Int32 GoodLength = 8;
        private const Int32 NiceLength = 128;
        private const Int32 MaxChainLength = 128;

        // repeated string length range is from minimum 3 to maximum 258
        private const Int32 MinMatch = 3;
        private const Int32 MaxMatch = 258;

        // read buffer minimum look ahead (LookAhead = BufferEnd - CurrentPointer)
        private const Int32 MinLookahead = MaxMatch + MinMatch + 1;

        // Window size is the maximum distance the repeated string matching process will look for a match
        private const Int32 WindowSize = 32768;			// 0x8000
        private const Int32 MatchIsTooFar = 4096;		// minimum match (3) should not be further than that

        // Hash table is used to speed up the matching process.
        // The program calculate a hash value at the current pointer.
        // It converts 3 bytes (MinMatch) to a 16 Bits value.
        // The hash value is used as an index to the hash table to obtain the first possible match
        private const Int32 WindowMask = WindowSize - 1;	// 0x7FFF
        private const Int32 HashTableSize = 65536;			// 0x10000
        private Int32[] HashTable;						// Hash table
        private Int32[] HashPrevious;					// Linked chains of equal Hash Values
        private static readonly UInt16[] HashXlate;						// Translation table for the 3rd byte of hash value

        // Compression block type
        // Compressed file is divided into these type of blocks
        // At the end of each block the program will select the shortest block
        private enum BlockType
        {
            StoredBlock,	// no compression block
            StaticTrees,	// compress with static trees
            DynamicTrees	// compress with dynamic trees
        }

        private const Int32 MaxStoredBlockSize = 65535;	// Stored block maximum length 0xFFFF
        private const Int32 BlockBufSize = 16384;		// Block buffer size 0x4000
        private const Int32 EndOfBlockSymbol = 256;		// End of block marker 0x100
        private Int32[] BlockBuf;					// Block buffer. Each element is either
        // (Literal) or (Distance << 8) | (Length - 3)
        private Int32 BlockBufEnd;				// Current end of block buffer
        private Int32 BlockBufExtraBits;			// Number of extra bits associated with Distance or Length

        // The literal tree is a combination of literals, lengths and end of block marker
        private DeflateTree LiteralTree;
        private const Int32 MinLiteralCodes = 257;		// Minimum literal codes
        private const Int32 MaxLiteralCodes = 286;		// Maximum literal codes
        private const Int32 MaxLiteralBitLen = 15;		// Maximum number of bits in literal codes
        private static readonly UInt16[] LengthCode;					// translation between Length-3 and LengthCode
        private static readonly UInt16[] StaticLiteralCodes;
        private static readonly Byte[] StaticLiteralLength;

        // The distance tree. The distance to previous occurrence of matched string
        private DeflateTree DistanceTree;
        private const Int32 MinDistanceCodes = 2;		// Minimum distance codes
        private const Int32 MaxDistanceCodes = 30;		// Maximum distance codes
        private const Int32 MaxDistanceBitLen = 15;		// Maximum number of bits in distance codes
        private static readonly Byte[] DistanceCode;				// Translation between Distance-1 and DistanceCode
        private static readonly UInt16[] StaticDistanceCodes;
        private static readonly Byte[] StaticDistanceLength;

        // Bit Length tree is the tree encoding the other two trees in a dynamic block
        private DeflateTree BitLengthTree;
        private const Int32 MinBitLengthCodes = 4;		// Minimum bit length codes
        private const Int32 MaxBitLengthCodes = 19;		// Maximum bit length codes
        private const Int32 MaxBitLengthBitLen = 7;		// Maximum number of bites in bit length codes

        private Byte[] ReadBuffer;					// Input stream buffer
        private Int32 ReadBlockStart;				// Start of current compression block
        private Int32 ReadPtr;					// Current read pointer
        private Int32 ReadBufEnd;					// Read buffer logical end
        private Int32 ReadAvailableBytes;			// Available bytes for compression matching (=ReadBufEnd - ReadPtr)
        private Int32 MatchStart;					// Pointer to matched string in prior text
        private Int32 MatchLen;					// Length of matched string

        private Byte[] WriteBuffer;				// Output stream buffer
        protected Int32 WritePtr;					// Current pointer to output stream buffer
        private Int32 WriteBufEnd;				// End pointer to output stream buffer
        private UInt32 BitBuffer;					// 32 bit buffer
        private Int32 BitCount;					// number of active bits in bit buffer
        private Boolean WriteFirstBlock;			// First compressed block is not out yet

        ////////////////////////////////////////////////////////////////////
        // Deflate static constructor
        // We use the static constructor to build all static read only arrays
        ////////////////////////////////////////////////////////////////////

        static DeflateMethod()
        {
            // translation table for the third byte of Hash calculations
            // Bit 0 to 15, Bit 2 to 14, Bit 4 to 13, Bit 6 to 12
            // Bit 1 to 7,  Bit 3 to 6,  Bit 5 to 5,  Bit 7 to 4
            HashXlate = new UInt16[]
			{
			0x0000, 0x8000, 0x0080, 0x8080, 0x4000, 0xC000, 0x4080, 0xC080,
			0x0040, 0x8040, 0x00C0, 0x80C0, 0x4040, 0xC040, 0x40C0, 0xC0C0, 
			0x2000, 0xA000, 0x2080, 0xA080, 0x6000, 0xE000, 0x6080, 0xE080,
			0x2040, 0xA040, 0x20C0, 0xA0C0, 0x6040, 0xE040, 0x60C0, 0xE0C0, 
			0x0020, 0x8020, 0x00A0, 0x80A0, 0x4020, 0xC020, 0x40A0, 0xC0A0,
			0x0060, 0x8060, 0x00E0, 0x80E0, 0x4060, 0xC060, 0x40E0, 0xC0E0, 
			0x2020, 0xA020, 0x20A0, 0xA0A0, 0x6020, 0xE020, 0x60A0, 0xE0A0,
			0x2060, 0xA060, 0x20E0, 0xA0E0, 0x6060, 0xE060, 0x60E0, 0xE0E0, 
			0x1000, 0x9000, 0x1080, 0x9080, 0x5000, 0xD000, 0x5080, 0xD080,
			0x1040, 0x9040, 0x10C0, 0x90C0, 0x5040, 0xD040, 0x50C0, 0xD0C0, 
			0x3000, 0xB000, 0x3080, 0xB080, 0x7000, 0xF000, 0x7080, 0xF080,
			0x3040, 0xB040, 0x30C0, 0xB0C0, 0x7040, 0xF040, 0x70C0, 0xF0C0, 
			0x1020, 0x9020, 0x10A0, 0x90A0, 0x5020, 0xD020, 0x50A0, 0xD0A0,
			0x1060, 0x9060, 0x10E0, 0x90E0, 0x5060, 0xD060, 0x50E0, 0xD0E0, 
			0x3020, 0xB020, 0x30A0, 0xB0A0, 0x7020, 0xF020, 0x70A0, 0xF0A0,
			0x3060, 0xB060, 0x30E0, 0xB0E0, 0x7060, 0xF060, 0x70E0, 0xF0E0, 
			0x0010, 0x8010, 0x0090, 0x8090, 0x4010, 0xC010, 0x4090, 0xC090,
			0x0050, 0x8050, 0x00D0, 0x80D0, 0x4050, 0xC050, 0x40D0, 0xC0D0, 
			0x2010, 0xA010, 0x2090, 0xA090, 0x6010, 0xE010, 0x6090, 0xE090,
			0x2050, 0xA050, 0x20D0, 0xA0D0, 0x6050, 0xE050, 0x60D0, 0xE0D0, 
			0x0030, 0x8030, 0x00B0, 0x80B0, 0x4030, 0xC030, 0x40B0, 0xC0B0,
			0x0070, 0x8070, 0x00F0, 0x80F0, 0x4070, 0xC070, 0x40F0, 0xC0F0, 
			0x2030, 0xA030, 0x20B0, 0xA0B0, 0x6030, 0xE030, 0x60B0, 0xE0B0,
			0x2070, 0xA070, 0x20F0, 0xA0F0, 0x6070, 0xE070, 0x60F0, 0xE0F0, 
			0x1010, 0x9010, 0x1090, 0x9090, 0x5010, 0xD010, 0x5090, 0xD090,
			0x1050, 0x9050, 0x10D0, 0x90D0, 0x5050, 0xD050, 0x50D0, 0xD0D0, 
			0x3010, 0xB010, 0x3090, 0xB090, 0x7010, 0xF010, 0x7090, 0xF090,
			0x3050, 0xB050, 0x30D0, 0xB0D0, 0x7050, 0xF050, 0x70D0, 0xF0D0, 
			0x1030, 0x9030, 0x10B0, 0x90B0, 0x5030, 0xD030, 0x50B0, 0xD0B0,
			0x1070, 0x9070, 0x10F0, 0x90F0, 0x5070, 0xD070, 0x50F0, 0xD0F0, 
			0x3030, 0xB030, 0x30B0, 0xB0B0, 0x7030, 0xF030, 0x70B0, 0xF0B0,
			0x3070, 0xB070, 0x30F0, 0xB0F0, 0x7070, 0xF070, 0x70F0, 0xF0F0, 
			};

            // Length Code (See RFC 1951 3.2.5)
            //		 Extra               Extra               Extra
            //	Code Bits Length(s) Code Bits Lengths   Code Bits Length(s)
            //	---- ---- ------     ---- ---- -------   ---- ---- -------
            //	 257   0     3       267   1   15,16     277   4   67-82
            //	 258   0     4       268   1   17,18     278   4   83-98
            //	 259   0     5       269   2   19-22     279   4   99-114
            //	 260   0     6       270   2   23-26     280   4  115-130
            //	 261   0     7       271   2   27-30     281   5  131-162
            //	 262   0     8       272   2   31-34     282   5  163-194
            //	 263   0     9       273   3   35-42     283   5  195-226
            //	 264   0    10       274   3   43-50     284   5  227-257
            //	 265   1  11,12      275   3   51-58     285   0    258
            //	 266   1  13,14      276   3   59-66
            //
            // build translation table between length and the code representing the length
            // length range is 3 to 258 (index 0 represent length 3)
            // codes will be in the range 257 to 285
            LengthCode = new UInt16[256];
            Int32 Base = 257;
            Int32 Divider = 1;
            Int32 Next = 8;
            for (UInt32 Len = 0; Len < 255; Len++)
            {
                if (Len == Next)
                {
                    Base += 4;
                    Divider <<= 1;
                    Next <<= 1;
                }
                LengthCode[Len] = (UInt16)(Base + Len / Divider);
            }
            LengthCode[255] = 285;

            // Distance Codes (See RFC 1951 3.2.5)
            //		 Extra           Extra                Extra
            //	Code Bits Dist  Code Bits   Dist     Code Bits Distance
            //	---- ---- ----  ---- ----  ------    ---- ---- --------
            //	  0   0    1     10   4     33-48    20    9   1025-1536
            //	  1   0    2     11   4     49-64    21    9   1537-2048
            //	  2   0    3     12   5     65-96    22   10   2049-3072
            //	  3   0    4     13   5     97-128   23   10   3073-4096
            //	  4   1   5,6    14   6    129-192   24   11   4097-6144
            //	  5   1   7,8    15   6    193-256   25   11   6145-8192
            //	  6   2   9-12   16   7    257-384   26   12  8193-12288
            //	  7   2  13-16   17   7    385-512   27   12 12289-16384
            //	  8   3  17-24   18   8    513-768   28   13 16385-24576
            //	  9   3  25-32   19   8   769-1024   29   13 24577-32768
            //
            // build translation table between distance and distance code
            // distance range is 1 to 32768 (index 0 represent distance 1)
            // distance codes will be in the range of 0 to 29
            DistanceCode = new Byte[WindowSize];
            Base = 0;
            Divider = 1;
            Next = 4;
            for (UInt32 Dist = 0; Dist < WindowSize; Dist++)
            {
                if (Dist == Next)
                {
                    Base += 2;
                    Divider <<= 1;
                    Next <<= 1;
                }
                DistanceCode[Dist] = (Byte)(Base + Dist / Divider);
            }

            // static literal codes and length codes  (See RFC 1951 3.2.6)
            //	Lit Value    Bits        Codes
            //	---------    ----        -----
            //	  0 - 143     8          00110000 through 10111111
            //	144 - 255     9          110010000 through 111111111
            //	256 - 279     7          0000000 through 0010111
            //	280 - 287     8          11000000 through 11000111
            //
            StaticLiteralCodes = new UInt16[288];
            StaticLiteralLength = new Byte[288];
            Int32 Code = 0;
            for (Int32 Index = 256; Index <= 279; Index++)
            {
                StaticLiteralCodes[Index] = BitReverse.Reverse16Bits(Code);
                StaticLiteralLength[Index] = (Byte)7;
                Code += 1 << (16 - 7);
            }
            for (Int32 Index = 0; Index <= 143; Index++)
            {
                StaticLiteralCodes[Index] = BitReverse.Reverse16Bits(Code);
                StaticLiteralLength[Index] = (Byte)8;
                Code += 1 << (16 - 8);
            }
            for (Int32 Index = 280; Index <= 287; Index++)
            {
                StaticLiteralCodes[Index] = BitReverse.Reverse16Bits(Code);
                StaticLiteralLength[Index] = (Byte)8;
                Code += 1 << (16 - 8);
            }
            for (Int32 Index = 144; Index <= 255; Index++)
            {
                StaticLiteralCodes[Index] = BitReverse.Reverse16Bits(Code);
                StaticLiteralLength[Index] = (Byte)9;
                Code += 1 << (16 - 9);
            }

            //	Static distance codes (See RFC 1951 3.2.6)
            //	Distance codes 0-31 are represented by (fixed-length) 5-bit
            //	codes, with possible additional bits as shown in the table
            //	shown in Paragraph 3.2.5, above.  Note that distance codes 30-
            //	31 will never actually occur in the compressed data.
            //
            StaticDistanceCodes = new UInt16[MaxDistanceCodes];
            StaticDistanceLength = new Byte[MaxDistanceCodes];
            for (Int32 Index = 0; Index < MaxDistanceCodes; Index++)
            {
                StaticDistanceCodes[Index] = BitReverse.Reverse16Bits(Index << 11);
                StaticDistanceLength[Index] = 5;
            }
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Deflate constructor
        // The constructor is used to allocate buffers.
        ////////////////////////////////////////////////////////////////////

        public DeflateMethod()
        {
            // create the three trees that control the compression process
            LiteralTree = new DeflateTree(WriteBits, MinLiteralCodes, MaxLiteralCodes, MaxLiteralBitLen);
            DistanceTree = new DeflateTree(WriteBits, MinDistanceCodes, MaxDistanceCodes, MaxDistanceBitLen);
            BitLengthTree = new DeflateTree(WriteBits, MinBitLengthCodes, MaxBitLengthCodes, MaxBitLengthBitLen);

            // allocate compression block buffer
            BlockBuf = new Int32[BlockBufSize];

            // hash tables initialization
            HashTable = new Int32[HashTableSize];
            HashPrevious = new Int32[WindowSize];
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Compress read stream to write stream
        // This is the main function of the DefaultMethod class
        ////////////////////////////////////////////////////////////////////

        public void Compress
                (
                Byte[] ReadBuffer,
                Byte[] WriteBuffer,
                Int32 WriteBufStart,
                Int32 WriteBufEnd
                )
        {
            // save read buffer
            this.ReadBuffer = ReadBuffer;

            // save write buffer
            this.WriteBuffer = WriteBuffer;
            this.WritePtr = WriteBufStart;
            this.WriteBufEnd = WriteBufEnd;

            // read process initialization
            ReadBlockStart = 0;
            ReadPtr = 0;
            MatchStart = 0;
            MatchLen = MinMatch - 1;

            // read first block (the derived class will supply this routine)
            ReadBufEnd = ReadBuffer.Length;

            // available bytes in the buffer
            ReadAvailableBytes = ReadBufEnd;

            // write process initialization
            BitBuffer = 0;
            BitCount = 0;
            WriteFirstBlock = true;

            // hash tables initialization
            for (Int32 HashPtr = HashTable.Length - 1; HashPtr >= 0; HashPtr--) HashTable[HashPtr] = -1;
            for (Int32 HashPtr = HashPrevious.Length - 1; HashPtr >= 0; HashPtr--) HashPrevious[HashPtr] = -1;

            // Compress
            DeflateSlow();

            // flush any leftover bits in bit buffer
            WriteAlignToByte();

            // successful return
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Normal compression
        // The program will use this compression method if
        // user compression level was 4 to 9
        ////////////////////////////////////////////////////////////////////

        private void DeflateSlow()
        {
            // Set block buffer to empty
            BlockReset();

            // This is the main compression loop for compression level 4 to 9.
            // The program will scan the whole file for matching strings.
            // If no match was found, the current literal is saved in block buffer.
            // If match is found, the program will try the next location for a better match.
            // If the first match is better, the distance and length are saved in block buffer.
            // If the second match is better, one literal is saved.
            // When the block buffer is full, the block is compressed into the write buffer.
            Boolean PrevLiteralNotSaved = false;
            for (; ; )
            {
                // End of file. Number of literals left is 0 to 2.
                if (ReadAvailableBytes < MinMatch)
                {
                    // save the last character if it is available
                    if (PrevLiteralNotSaved) SaveLiteralInBlockBuf(ReadBuffer[ReadPtr - 1]);

                    // One or two characters are still in the read buffer
                    while (ReadAvailableBytes > 0)
                    {
                        // Block is full. Compress the block buffer
                        if (BlockBufEnd == BlockBufSize) CompressBlockBuf(false);

                        // save the literal in the block buffer
                        SaveLiteralInBlockBuf(ReadBuffer[ReadPtr]);

                        // update pointer
                        ReadPtr++;
                        ReadAvailableBytes--;
                    }

                    // Compress the last block of data.
                    CompressBlockBuf(true);
                    return;
                }

                // save current match
                Int32 PrevMatch = MatchStart;
                Int32 PrevLen = MatchLen;

                // find the longest match for current ReadPtr
                if (MatchLen < NiceLength && MatchLen < ReadAvailableBytes) FindLongestMatch();

                // previous match was better
                if (PrevLen >= MinMatch && PrevLen >= MatchLen)
                {
                    // save the previous match
                    SaveDistanceInBlockBuf(ReadPtr - 1 - PrevMatch, PrevLen);

                    // move the pointer to the last literal of the current matched block
                    for (PrevLen -= 2; PrevLen > 0; PrevLen--)
                    {
                        // update pointer
                        ReadPtr++;
                        ReadAvailableBytes--;

                        // update the hash table for each literal of the current matched block
                        HashInsertString();
                    }

                    // previous literal is not available.
                    PrevLiteralNotSaved = false;

                    // reset previous match.
                    MatchLen = MinMatch - 1;
                }

                // current match is better
                else
                {
                    // save the previous single literal
                    if (PrevLiteralNotSaved) SaveLiteralInBlockBuf(ReadBuffer[ReadPtr - 1]);

                    // try again for a better match
                    PrevLiteralNotSaved = true;
                }

                // update pointer to next literal
                ReadPtr++;
                ReadAvailableBytes--;

                // compress block buffer
                if (BlockBufEnd == BlockBufSize)
                {
                    if (PrevLiteralNotSaved)
                    {
                        ReadPtr--;
                        CompressBlockBuf(false);
                        ReadPtr++;
                    }
                    else
                    {
                        // if read pointer is at the end of the buffer it is end of file situation
                        CompressBlockBuf(ReadPtr == ReadBufEnd);
                        if (ReadPtr == ReadBufEnd) return;
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////
        // Find Longest Match
        ////////////////////////////////////////////////////////////////////

        private void FindLongestMatch()
        {
            // stop the search if the scan pointer is greater than MaxMatch beyond current pointer
            Int32 MaxScanPtr = ReadPtr + Math.Min(MaxMatch, ReadAvailableBytes);

            // initially MatchLen is MinMatch - 1
            // for slow deflate MatchLen will be the best match of the previous byte (ReadStringPtr -1)
            // in that case the current match must be better than the previous one
            Int32 ScanEnd = ReadPtr + MatchLen;

            // byte value at scan end and one before
            Byte ScanEndValue = ReadBuffer[ScanEnd];

            // Pointer to maximum distance backward in the read buffer
            Int32 MaxDistanceLimit = Math.Max(ReadPtr - WindowSize, 0);

            // reset current match to no-match
            Int32 CurMatch = -1;

            // HashPrevious array has a chained pointers to all locations in the current window
            // that have equal hash values to the ReadStringPtr
            // maximum number of tries along the chain pointers
            for (Int32 ChainLength = MatchLen < GoodLength ? MaxChainLength : MaxChainLength / 4; ChainLength > 0; ChainLength--)
            {
                // get the first possible matched string based on hash code
                // or get the next possible matched string from the linked chain
                CurMatch = CurMatch < 0 ? HashInsertString() : HashPrevious[CurMatch & WindowMask];

                // exit if hash entry is empty or it is too far back
                if (CurMatch < MaxDistanceLimit) break;

                // if we have a previous match test the end characters for possible progress
                if (ReadBuffer[CurMatch + MatchLen] != ScanEndValue) continue;

                // the distance between current pointer and previous possible match
                Int32 MatchDelta = ReadPtr - CurMatch;

                // find the length of the match				
                Int32 ScanPtr = 0;
                for (ScanPtr = ReadPtr; ScanPtr < MaxScanPtr && ReadBuffer[ScanPtr] == ReadBuffer[ScanPtr - MatchDelta]; ScanPtr++) ;

                // we have a longer match
                if (ScanPtr > ScanEnd)
                {
                    // replace current match in a global area
                    MatchStart = CurMatch;
                    MatchLen = ScanPtr - ReadPtr;

                    // break if this is good enough
                    if (MatchLen >= NiceLength) break;

                    // end of current match and length
                    ScanEnd = ScanPtr;

                    // we cannot do any better
                    if (ScanPtr == MaxScanPtr) break;

                    // replace the byte values at the end of this scan
                    ScanEndValue = ReadBuffer[ScanEnd];
                }
            }

            // Discard match if too small and too far away
            if (MatchLen == MinMatch && ReadPtr - MatchStart > MatchIsTooFar) MatchLen = MinMatch - 1;

            // exit
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Add one byte to the literal list
        ////////////////////////////////////////////////////////////////////

        private void SaveLiteralInBlockBuf
                (
                Int32 Literal
                )
        {
            // save literal in block buffer
            BlockBuf[BlockBufEnd++] = Literal;

            // update frequency array
            LiteralTree.CodeFreq[Literal]++;
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Add one distance/length pair to the literal list
        ////////////////////////////////////////////////////////////////////

        private void SaveDistanceInBlockBuf
                (
                Int32 Distance,
                Int32 Length
                )
        {
            // adjust length (real length range is 3 to 258, saved length range is 0 to 255)
            Length -= MinMatch;

            // save distance and length in one integer
            BlockBuf[BlockBufEnd++] = (Distance << 8) | Length;

            // build frequency array for length code
            Int32 LenCode = LengthCode[Length];
            LiteralTree.CodeFreq[LenCode]++;

            // accumulate number of extra bits for length codes
            if (LenCode >= 265 && LenCode < 285) BlockBufExtraBits += (LenCode - 261) / 4;

            // build frequency array for distance codes
            Int32 DistCode = DistanceCode[Distance - 1];
            DistanceTree.CodeFreq[DistCode]++;

            // accumulate extra bits for distance codes
            if (DistCode >= 4) BlockBufExtraBits += DistCode / 2 - 1;
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Compress the block buffer when it is full
        // The block buffer is made of literals and distance length pairs
        ////////////////////////////////////////////////////////////////////

        private void CompressBlockBuf
                (
                bool LastBlock
                )
        {
            // add end of block to code frequency array
            LiteralTree.CodeFreq[EndOfBlockSymbol]++;

            // Build trees
            LiteralTree.BuildTree();
            DistanceTree.BuildTree();

            // Calculate bit length frequency
            Int32 BitLengthExtraBits = LiteralTree.CalcBLFreq(BitLengthTree);
            BitLengthExtraBits += DistanceTree.CalcBLFreq(BitLengthTree);

            // Build bit length tree
            BitLengthTree.BuildTree();

            // calculate length in bits of bit length tree
            Int32 blTreeCodes = BitLengthTree.MaxUsedCodesBitLength();

            // calculate total block length for dynamic coding
            // The 17 is made of: 3 bits block header, 5 bits literal codes, 5 bits distance codes, 4 bits bit-length codes
            Int32 CompressedLen = 17 + blTreeCodes * 3 + BitLengthTree.GetEncodedLength() + BitLengthExtraBits +
                    LiteralTree.GetEncodedLength() + DistanceTree.GetEncodedLength() + BlockBufExtraBits;

            // compressed block length in bytes for dynamic coding
            CompressedLen = (CompressedLen + 7) / 8;

            // calculate total block length for static coding
            Int32 StaticLen = 3 + BlockBufExtraBits;
            for (Int32 i = 0; i < MaxLiteralCodes; i++) StaticLen += LiteralTree.CodeFreq[i] * StaticLiteralLength[i];
            for (Int32 i = 0; i < MaxDistanceCodes; i++) StaticLen += DistanceTree.CodeFreq[i] * StaticDistanceLength[i];

            // static block length in bytes
            StaticLen = (StaticLen + 7) / 8;

            // static trees look better
            if (StaticLen <= CompressedLen) CompressedLen = StaticLen;

            // uncompressed read block length in bytes
            Int32 StoredBlockLen = ReadPtr - ReadBlockStart;

            // This is the last compressed block
            if (LastBlock)
            {
                // If this is the first block and the last block at the same time (relatively small file)
                // and the uncompressed block is better than compressed block, change compression function from deflate to stored
                if (WriteFirstBlock && StoredBlockLen <= CompressedLen) throw new ApplicationException("No compression");

                // Test compressed overall file length.
                // If overall compressed length is more than the original uncompressed size
                // the derived class will rewind the read and write stream.
                if (WritePtr + (BitCount + 7) / 8 + Math.Min(CompressedLen, StoredBlockLen + 5) > WriteBufEnd) throw new ApplicationException("No compression");
            }

            // Uncompressed block length is better than compressed length.
            // Uncompressed block has 5 bytes overhead.
            // Stored block header plus 2 bytes length and 2 bytes length complement.
            if (StoredBlockLen + 5 < CompressedLen)
            {
                // loop in case block length is larger than maximum allowed
                while (StoredBlockLen > 0)
                {
                    // block length (max 65535)
                    Int32 Len = Math.Min(StoredBlockLen, (Int32)0xffff);

                    // adjust remaining length
                    StoredBlockLen -= Len;

                    // Write the block on even byte boundary, Signal if this is the last block of the file
                    WriteStoredBlock(Len, LastBlock && StoredBlockLen == 0);

                    // adjust block start pointer
                    ReadBlockStart += Len;
                }
            }

            // Encode with static tree
            else if (CompressedLen == StaticLen)
            {
                // write static block header to output file
                WriteBits(((Int32)BlockType.StaticTrees << 1) + (LastBlock ? 1 : 0), 3);

                // replace the dynamic codes with static codes
                LiteralTree.SetStaticCodes(StaticLiteralCodes, StaticLiteralLength);
                DistanceTree.SetStaticCodes(StaticDistanceCodes, StaticDistanceLength);

                // Compress the block and send it to the output buffer
                // This process converts the block buffer values into variable length sequence of bits.
                CompressBlock();

                // adjust block pointer
                ReadBlockStart += StoredBlockLen;
            }

            // Encode with dynamic tree
            else
            {
                // write dynamic block header to output file
                WriteBits(((Int32)BlockType.DynamicTrees << 1) + (LastBlock ? 1 : 0), 3);

                // write the dynamic tree to the output stream
                SendAllTrees(blTreeCodes);

                // Compress the block and send it to the output buffer
                // This process converts the block buffer values into variable length sequence of bits.
                CompressBlock();

                // adjust block pointer
                ReadBlockStart += StoredBlockLen;
            }

            // Set block buffer to empty
            BlockReset();

            // Reset write first block
            WriteFirstBlock = false;
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Block Reset
        ////////////////////////////////////////////////////////////////////

        private void BlockReset()
        {
            // Set block buffer to empty
            BlockBufEnd = 0;
            BlockBufExtraBits = 0;

            // Reset literal, distance and bit-length trees
            LiteralTree.Reset();
            DistanceTree.Reset();
            BitLengthTree.Reset();
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // At the start of each dynamic block transmit all trees
        ////////////////////////////////////////////////////////////////////

        private void SendAllTrees
                (
                Int32 blTreeCodes
                )
        {
            // Calculate the Huffman codes for all used literals and lengths
            LiteralTree.BuildCodes();

            // write to output buffer the number of used literal/length codes
            WriteBits(LiteralTree.MaxUsedCodes - 257, 5);

            // Calculate the Huffman codes for all used distances
            DistanceTree.BuildCodes();

            // write to output buffer the number of used distance codes
            WriteBits(DistanceTree.MaxUsedCodes - 1, 5);

            // Calculate the Huffman codes for transmitting the first two trees
            BitLengthTree.BuildCodes();

            // write to output buffer the number of used bit-length codes
            WriteBits(blTreeCodes - 4, 4);

            // In the next three statements we send the Huffman codes associated with each code used.
            // The decompressor will used this information to build a decoder.
            // The decoder will translate incoming codes into original literals.
            // Send to output stream the bit-length tree codes. 
            BitLengthTree.WriteBitLengthCodeLength(blTreeCodes);

            // Send to output stream the literal/length tree codes.
            LiteralTree.WriteTree(BitLengthTree);

            // Send to output stream the distance tree codes.
            DistanceTree.WriteTree(BitLengthTree);
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Compress the static or dynamic block
        ////////////////////////////////////////////////////////////////////

        private void CompressBlock()
        {
            // loop for all entries in the scan buffer
            for (Int32 BlockBufPtr = 0; BlockBufPtr < BlockBufEnd; BlockBufPtr++)
            {
                // length and distance pair
                Int32 Distance = BlockBuf[BlockBufPtr] >> 8;
                Int32 Length = (Byte)BlockBuf[BlockBufPtr];

                // literal
                if (Distance == 0)
                {
                    // WriteSymbol translates the literal code to variable length Huffman code and write it to the output stream
                    LiteralTree.WriteSymbol(Length);
                    continue;
                }

                // Translate length to length code
                // The LengthCode translation array is defined above in the DeflateMethod static constructor.
                Int32 LenCode = LengthCode[Length];

                // WriteSymbol translates the length code to variable length Huffman code and write it to the output stream
                LiteralTree.WriteSymbol(LenCode);

                // send extra bits
                // note: LenCode=285 is the highest code and it has no extra bits
                if (LenCode >= 265 && LenCode < 285)
                {
                    Int32 LenBits = (LenCode - 261) / 4;
                    WriteBits(Length & ((1 << LenBits) - 1), LenBits);
                }

                // translate distance to distance code
                // The DistanceCode translation array is defined above in the DeflateMethod static constructor.
                Int32 DistCode = DistanceCode[--Distance];

                // WriteSymbol translates the distance code to variable length Huffman code and write it to the output stream
                DistanceTree.WriteSymbol(DistCode);

                // send extra bits
                if (DistCode >= 4)
                {
                    Int32 DistBits = DistCode / 2 - 1;
                    WriteBits(Distance & ((1 << DistBits) - 1), DistBits);
                }
            }

            // write the end of block symbol to output file
            LiteralTree.WriteSymbol(EndOfBlockSymbol);
            return;
        }

        /////////////////////////////////////////////////////////////////////
        // Write bits
        /////////////////////////////////////////////////////////////////////

        private void WriteBits
                (
                Int32 Bits,
                Int32 Count
                )
        {
            // add bits to bits buffer
            BitBuffer |= (UInt32)(Bits << BitCount);
            BitCount += Count;

            // we have more than 16 bits in the buffer
            if (BitCount >= 16)
            {
                // test for room in the buffer
                if (WriteBufEnd - WritePtr < 2) throw new ApplicationException("No compression");

                // move two bytes from bit buffer to write buffer
                WriteBuffer[WritePtr++] = (Byte)BitBuffer;
                WriteBuffer[WritePtr++] = (Byte)(BitBuffer >> 8);

                // adjust bit buffer
                BitBuffer >>= 16;
                BitCount -= 16;
            }
            return;
        }

        /////////////////////////////////////////////////////////////////////
        // Align bit buffer to byte boundry
        /////////////////////////////////////////////////////////////////////

        private void WriteAlignToByte()
        {
            if (BitCount > 0)
            {
                // test for room in the buffer
                if (WriteBufEnd - WritePtr < (BitCount > 8 ? 2 : 1)) throw new ApplicationException("No compression"); ;

                // write first byte
                WriteBuffer[WritePtr++] = (Byte)BitBuffer;

                // write second byte if needed
                if (BitCount > 8) WriteBuffer[WritePtr++] = (Byte)(BitBuffer >> 8);

                // clear bit buffer
                BitBuffer = 0;
                BitCount = 0;
            }
            return;
        }

        /////////////////////////////////////////////////////////////////////
        // Flush Stored Block to Write Buffer
        /////////////////////////////////////////////////////////////////////

        private void WriteStoredBlock
                (
                Int32 Length,
                Boolean LastBlock
                )
        {
            // block header of stored block (3 bits)
            WriteBits(((Int32)BlockType.StoredBlock << 1) + (LastBlock ? 1 : 0), 3);

            // flush the bit buffer
            WriteAlignToByte();

            // test for room in the buffer
            if (WriteBufEnd - WritePtr < Length + 4) throw new ApplicationException("No compression");

            // write block length (16 bits)
            WriteBuffer[WritePtr++] = (Byte)Length;
            WriteBuffer[WritePtr++] = (Byte)(Length >> 8);

            // write inverted block length (16 bits)
            WriteBuffer[WritePtr++] = (Byte)(~Length);
            WriteBuffer[WritePtr++] = (Byte)((~Length) >> 8);

            Array.Copy(ReadBuffer, ReadBlockStart, WriteBuffer, WritePtr, Length);
            WritePtr += Length;
            return;
        }

        /////////////////////////////////////////////////////////////////////
        // Hash insert string
        /////////////////////////////////////////////////////////////////////

        private Int32 HashInsertString()
        {
            // end of file test
            if (ReadAvailableBytes < MinMatch) return (-1);

            // hash value
            // NOTE: the original hash table was 15 bits (32768)
            // the hash value was calculated as
            // HashValue = (ReadBuffer[ReadPtr] << 10 ^ ReadBuffer[ReadPtr + 1] << 5 | ReadBuffer[ReadPtr + 2]) & 0x7FFF;
            // The method used here is faster and produces less collisions 
            Int32 HashValue = BitConverter.ToUInt16(ReadBuffer, ReadPtr) ^ HashXlate[ReadBuffer[ReadPtr + 2]];

            // get the previous pointer at the hash value position
            Int32 PreviousPtr = HashTable[HashValue];

            // save current file position in the hash table
            HashTable[HashValue] = ReadPtr;

            // save the previous pointer in a circular buffer
            HashPrevious[ReadPtr & WindowMask] = PreviousPtr;

            // return with a pointer to read buffer position with the same hash value as the current pointer
            // if there was no previous match, the return value is -1
            return (PreviousPtr);
        }
    }

    /////////////////////////////////////////////////////////////////////
    // Frequency Node
    // This class is used to calculate the Huffman coding
    // base on frequency
    /////////////////////////////////////////////////////////////////////

    internal struct FreqNode : IComparable<FreqNode>
    {
        public Int32 Code;
        public Int32 Freq;
        public Int32 Child;	// left child is Child - 1

        public Int32 CompareTo
                (
                FreqNode Other
                )
        {
            return (this.Freq - Other.Freq);
        }
    }

    /////////////////////////////////////////////////////////////////////
    // Deflate Tree
    /////////////////////////////////////////////////////////////////////

    internal class DeflateTree
    {
        private const Int32 RepeatSymbol_3_6 = 16;
        private const Int32 RepeatSymbol_3_10 = 17;
        private const Int32 RepeatSymbol_11_138 = 18;

        private static Int32[] BitLengthOrder = {RepeatSymbol_3_6, RepeatSymbol_3_10, RepeatSymbol_11_138,
											  0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15};

        public Int32[] CodeFreq;			// number of times a given code was used
        public Int32 MaxUsedCodes;		// the number of highest code used plus one

        private FreqNode[] FreqTree;			// frequency tree
        private Int32 FreqTreeEnd;		// number of nodes in the frequency tree
        private Int32 MaxUsedBitLen;		// maximum used bit length

        private UInt16[] Codes;				// dynamic or static variable length bit code
        private Byte[] CodeLength;			// dynamic or static code length
        private Int32[] BitLengthFreq;		// frequency of code length (how many times code length is used)
        private Int32[] FirstCode;			// first code of a group of equal code length
        private UInt16[] SaveCodes;			// dynamic variable length bit code
        private Byte[] SaveCodeLength;		// dynamic code length

        private WriteBits WriteBits;			// write bits method (void WriteBits(Int32 Bits, Int32 Count))
        private Int32 MinCodes;			// minimum number of codes
        private Int32 MaxCodes;			// maximum number of codes
        private Int32 MaxBitLength;		// maximum length in bits of a code

        ////////////////////////////////////////////////////////////////////
        // Constructor
        ////////////////////////////////////////////////////////////////////
        //					Literal Tree		MinCodes = 257, MaxCodes = 286, MaxBitLength = 15
        //					Distance Tree		MinCodes =   2, MaxCodes =  30, MaxBitLength = 15
        //					Bit Length Tree		MinCodes =   4, MaxCodes =  19, MaxBitLength = 7
        public DeflateTree(WriteBits WriteBits, Int32 MinCodes, Int32 MaxCodes, Int32 MaxBitLength)
        {
            // write bits method
            this.WriteBits = WriteBits;

            // save minimum codes, maximum codes, and maximum bit length
            this.MinCodes = MinCodes;
            this.MaxCodes = MaxCodes;
            this.MaxBitLength = MaxBitLength;

            // allocate arrays
            CodeFreq = new Int32[MaxCodes];
            SaveCodeLength = new Byte[MaxCodes];
            SaveCodes = new UInt16[MaxCodes];
            FreqTree = new FreqNode[2 * MaxCodes - 1];
            BitLengthFreq = new Int32[MaxBitLength];
            FirstCode = new Int32[MaxBitLength];

            // clear arrays
            Reset();
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Reset class after each block
        ////////////////////////////////////////////////////////////////////

        public void Reset()
        {
            // The deflate method will overwrite the CodeLength and Codes arrays
            // if static block was selected. We restore it to the dynamic arrays
            CodeLength = SaveCodeLength;
            Codes = SaveCodes;

            // clear code frequency array
            Array.Clear(CodeFreq, 0, CodeFreq.Length);
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Write symbol to output stream
        ////////////////////////////////////////////////////////////////////

        public void WriteSymbol(Int32 code)
        {
            WriteBits(Codes[code], CodeLength[code]);
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // For static trees overwrite the Codes and CodeLength arrays
        // the reset function will restore the dynamic arrays
        ////////////////////////////////////////////////////////////////////

        public void SetStaticCodes
                (
                UInt16[] StaticCodes,
                Byte[] StaticLength
                )
        {
            Codes = StaticCodes;
            CodeLength = StaticLength;
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Build Huffman tree
        ////////////////////////////////////////////////////////////////////

        public void BuildTree()
        {
            // find the highest code in use
            for (MaxUsedCodes = MaxCodes - 1; MaxUsedCodes >= MinCodes && CodeFreq[MaxUsedCodes] == 0; MaxUsedCodes--) ;
            MaxUsedCodes++;

            // clear frequency tree
            Array.Clear(FreqTree, 0, FreqTree.Length);

            // initialize Huffman frequency tree with leaf nodes
            FreqTreeEnd = 0;
            for (Int32 Code = 0; Code < MaxUsedCodes; Code++)
            {
                // include used codes. ignore codes with zero frequency
                if (CodeFreq[Code] != 0)
                {
                    FreqTree[FreqTreeEnd].Code = Code;
                    FreqTree[FreqTreeEnd++].Freq = CodeFreq[Code];
                }
            }

            // there are less than 2 codes in use
            if (FreqTreeEnd < 2)
            {
                // zero codes in use
                if (FreqTreeEnd == 0)
                {
                    // create artificial node of code 0 with zero frequency
                    FreqTree[0].Code = 0;
                    FreqTree[0].Freq = 0;
                    FreqTreeEnd++;
                }
                // one code in use
                // create artificial second node
                // if the first code is 0 the second is 1, if the first is non zero make the second zero
                // the frequency is zero (not used)
                FreqTree[1].Code = FreqTree[0].Code == 0 ? 1 : 0;
                FreqTree[1].Freq = 0;
                FreqTreeEnd++;
            }

            // sort it by frequency low to high
            Array.Sort(FreqTree, 0, FreqTreeEnd);

            // clear code length
            Array.Clear(CodeLength, 0, CodeLength.Length);

            // loop in case of bit length exceeding the maximum
            // for literals and distance it is 15 and for bit length it is 7
            for (; ; )
            {
                // build Huffman tree by combining pairs of nodes
                CombinePairs();

                // calculate length in bits of each code
                MaxUsedBitLen = 0;
                BuildCodeTree(FreqTreeEnd - 1, 0);
                if (MaxUsedBitLen <= MaxBitLength) break;

                // adjust the lowest frequency nodes such that we get a flatter tree
                AdjustNodes();
            }

            // exit
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Combine pairs of lower frequency node to a parent node
        ////////////////////////////////////////////////////////////////////

        private void CombinePairs()
        {
            // combine pairs of nodes
            for (Int32 Ptr = 2; Ptr < FreqTreeEnd; Ptr += 2)
            {
                // combined frequency of current pair
                Int32 CombFreq = FreqTree[Ptr - 2].Freq + FreqTree[Ptr - 1].Freq;

                // right pointer
                Int32 RightPtr = FreqTreeEnd;

                // set left pointer to starting point
                Int32 LeftPtr = Ptr;

                // perform binary search
                Int32 InsertPtr;
                Int32 Cmp;
                for (; ; )
                {
                    // middle pointer
                    InsertPtr = (LeftPtr + RightPtr) / 2;

                    // compare
                    Cmp = CombFreq - FreqTree[InsertPtr].Freq;

                    // range is one or exact match
                    if (InsertPtr == LeftPtr || Cmp == 0) break;

                    // move search to the right
                    if (Cmp > 0) LeftPtr = InsertPtr;

                    // move search to the left
                    else RightPtr = InsertPtr;
                }

                // exact compare (point to end of a group of equal frequencies)
                // positive compare (move to the right as long as it is equal)
                if (Cmp >= 0)
                {
                    for (InsertPtr++; InsertPtr < FreqTreeEnd && FreqTree[InsertPtr].Freq == CombFreq; InsertPtr++) ;
                }

                // open a hole at insert point
                Array.Copy(FreqTree, InsertPtr, FreqTree, InsertPtr + 1, FreqTreeEnd - InsertPtr);
                FreqTreeEnd++;

                // add node with combined frequency
                FreqTree[InsertPtr].Code = -1;
                FreqTree[InsertPtr].Freq = CombFreq;
                FreqTree[InsertPtr].Child = Ptr - 1;
            }

            // add final root node with combined frequency
            FreqTree[FreqTreeEnd].Code = -1;
            FreqTree[FreqTreeEnd].Freq = FreqTree[FreqTreeEnd - 2].Freq + FreqTree[FreqTreeEnd - 1].Freq;
            FreqTree[FreqTreeEnd].Child = FreqTreeEnd - 1;
            FreqTreeEnd++;

            // exit
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Scan the tree using recursive routine and set the number
        // of bits to represent the code
        ////////////////////////////////////////////////////////////////////

        private void BuildCodeTree
                (
                Int32 Ptr,
                Int32 BitCount
                )
        {
            FreqNode FN = FreqTree[Ptr];

            // we are at a parent node
            if (FN.Code < 0)
            {
                // go to children on the left side of this node
                BuildCodeTree(FN.Child - 1, BitCount + 1);

                // go to children on the right side of this node
                BuildCodeTree(FN.Child, BitCount + 1);
            }

            // we are at child node
            else
            {
                if (BitCount > MaxUsedBitLen) MaxUsedBitLen = BitCount;
                CodeLength[FN.Code] = (Byte)BitCount;
            }
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Adjust low frequency nodes to make the tree more symmetric
        ////////////////////////////////////////////////////////////////////

        public void AdjustNodes()
        {
            // remove all non leaf nodes
            Int32 Ptr;
            for (Ptr = 0; Ptr < FreqTreeEnd; Ptr++)
            {
                if (FreqTree[Ptr].Code >= 0) continue;
                FreqTreeEnd--;
                Array.Copy(FreqTree, Ptr + 1, FreqTree, Ptr, FreqTreeEnd - Ptr);
                Ptr--;
            }

            // look for first change in frequency
            Int32 Freq = FreqTree[0].Freq;
            for (Ptr = 1; Ptr < FreqTreeEnd && FreqTree[Ptr].Freq == Freq; Ptr++) ;
            if (Ptr == FreqTreeEnd) throw new ApplicationException("Adjust nodes failed");

            // adjust the frequency of least frequent nodes
            Freq = FreqTree[Ptr].Freq;
            for (Ptr--; Ptr >= 0; Ptr--) FreqTree[Ptr].Freq = Freq;
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Build code array from bit length frequency distribution
        ////////////////////////////////////////////////////////////////////

        public void BuildCodes()
        {
            // build bit length frequency array
            Array.Clear(BitLengthFreq, 0, BitLengthFreq.Length);
            for (Int32 Code = 0; Code < MaxUsedCodes; Code++)
            {
                if (CodeLength[Code] != 0) BitLengthFreq[CodeLength[Code] - 1]++;
            }

            // build array of initial code for each block of codes
            Int32 InitCode = 0;
            for (Int32 BitNo = 0; BitNo < MaxBitLength; BitNo++)
            {
                // save the initial code of the block
                FirstCode[BitNo] = InitCode;

                // advance the code by the frequency times 2 to the power of 15 less bit length
                InitCode += BitLengthFreq[BitNo] << (15 - BitNo);
            }

            // it must add up to 2 ** 16
            if (InitCode != 65536) throw new ApplicationException("Inconsistent bl_counts!");

            // now fill up the entire code array
            Array.Clear(Codes, 0, Codes.Length);
            for (Int32 Index = 0; Index < MaxUsedCodes; Index++)
            {
                Int32 Bits = CodeLength[Index];
                if (Bits > 0)
                {
                    Codes[Index] = BitReverse.Reverse16Bits(FirstCode[Bits - 1]);
                    FirstCode[Bits - 1] += 1 << (16 - Bits);
                }
            }

            // exit
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // calculate encoded data block length
        ////////////////////////////////////////////////////////////////////

        public Int32 GetEncodedLength()
        {
            Int32 Len = 0;
            for (Int32 Index = 0; Index < MaxUsedCodes; Index++) Len += CodeFreq[Index] * CodeLength[Index];
            return (Len);
        }

        ////////////////////////////////////////////////////////////////////
        // Bit length tree only. Calculate total length
        ////////////////////////////////////////////////////////////////////

        public Int32 MaxUsedCodesBitLength()
        {
            // calculate length in bits of bit length tree
            for (Int32 Index = 18; Index >= 4; Index--) if (CodeLength[BitLengthOrder[Index]] > 0) return (Index + 1);
            return (4);
        }

        ////////////////////////////////////////////////////////////////////
        // Bit length tree only. Calculate total length
        ////////////////////////////////////////////////////////////////////

        public void WriteBitLengthCodeLength
                (
                Int32 blTreeCodes
                )
        {
            // send to output stream the bit length array
            for (Int32 Rank = 0; Rank < blTreeCodes; Rank++) WriteBits(CodeLength[BitLengthOrder[Rank]], 3);
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // calculate Bit Length frequency
        ///	0 - 15: Represent code lengths of 0 - 15
        //	16: Copy the previous code length 3 - 6 times.
        //		The next 2 bits indicate repeat length
        //		(0 = 3, ... , 3 = 6)
        //		Example:  Codes 8, 16 (+2 bits 11), 16 (+2 bits 10) will expand to
        //		12 code lengths of 8 (1 + 6 + 5)
        //	17: Repeat a code length of 0 for 3 - 10 times. (3 bits of length)
        //	18: Repeat a code length of 0 for 11 - 138 times. (7 bits of length)
        ////////////////////////////////////////////////////////////////////

        public Int32 CalcBLFreq
                (
                DeflateTree blTree
                )
        {
            Int32 ExtraBits = 0;
            for (Int32 Code = 0; Code < MaxUsedCodes; )
            {
                // current code length
                Int32 CodeLen = CodeLength[Code];

                // scan the code array for equal code lengths
                Int32 Count = 1;
                for (Code++; Code < MaxUsedCodes && CodeLen == CodeLength[Code]; Code++) Count++;

                // less than three equal code length
                if (Count < 3)
                {
                    blTree.CodeFreq[CodeLen] += (Int16)Count;
                    continue;
                }

                // code length is other than zero
                if (CodeLen != 0)
                {
                    // add one to the frequency of the code itself
                    blTree.CodeFreq[CodeLen]++;

                    // reduce the count by one
                    Count--;

                    // for every full block of 6 repeats add one REP_3_6 code
                    blTree.CodeFreq[RepeatSymbol_3_6] += Count / 6;
                    ExtraBits += 2 * (Count / 6);

                    // get the remainder
                    if ((Count = Count % 6) == 0) continue;

                    // remainder is less than 3
                    if (Count < 3)
                    {
                        blTree.CodeFreq[CodeLen] += Count;
                        continue;
                    }

                    // remainder is between 3 to 5
                    blTree.CodeFreq[RepeatSymbol_3_6]++;
                    ExtraBits += 2;
                    continue;
                }

                // code length is zero and count is 3 or more
                // for every full block of 138 repeats add one REP_11_138 code
                blTree.CodeFreq[RepeatSymbol_11_138] += Count / 138;
                ExtraBits += 7 * (Count / 138);

                // get the remainder
                if ((Count = Count % 138) == 0) continue;

                // remainder is less than 3
                if (Count < 3)
                {
                    blTree.CodeFreq[CodeLen] += Count;
                    continue;
                }

                // remainder is between 3 to 10
                if (Count <= 10)
                {
                    blTree.CodeFreq[RepeatSymbol_3_10]++;
                    ExtraBits += 3;
                    continue;
                }

                // remainder is between 11 to 137
                blTree.CodeFreq[RepeatSymbol_11_138]++;
                ExtraBits += 7;
            }
            return (ExtraBits);
        }

        ////////////////////////////////////////////////////////////////////
        // Write the tree to output file
        ////////////////////////////////////////////////////////////////////

        public void WriteTree
                (
                DeflateTree blTree
                )
        {
            for (Int32 Code = 0; Code < MaxUsedCodes; )
            {
                // current code length
                Int32 CodeLen = CodeLength[Code];

                // scan the code array for equal code lengths
                Int32 Count = 1;
                for (Code++; Code < MaxUsedCodes && CodeLen == CodeLength[Code]; Code++) Count++;

                // less than three equal code length
                if (Count < 3)
                {
                    // write the first code length to output file
                    blTree.WriteSymbol(CodeLen);

                    // write the second code length to output file
                    if (Count > 1) blTree.WriteSymbol(CodeLen);
                    continue;
                }

                // Used code. Code length is other than zero.
                if (CodeLen != 0)
                {
                    // write the code length to output file
                    blTree.WriteSymbol(CodeLen);

                    // reduce the count by one
                    Count--;

                    // for every full block of 6 repeats add one REP_3_6 code and two bits of 1 (11)
                    for (Int32 Index = Count / 6; Index > 0; Index--)
                    {
                        blTree.WriteSymbol(RepeatSymbol_3_6);
                        WriteBits(6 - 3, 2);
                    }

                    // get the remainder
                    if ((Count = Count % 6) == 0) continue;

                    // remainder is less than 3
                    if (Count < 3)
                    {
                        // write the code length to output file
                        blTree.WriteSymbol(CodeLen);
                        if (Count > 1) blTree.WriteSymbol(CodeLen);
                        continue;
                    }

                    // remainder is between 3 to 5
                    blTree.WriteSymbol(RepeatSymbol_3_6);
                    WriteBits(Count - 3, 2);
                    continue;
                }

                // code length is zero and count is 3 or more
                // for every full block of 138 repeats add one REP_11_138 code plus 7 bits of 1
                for (Int32 Index = Count / 138; Index > 0; Index--)
                {
                    blTree.WriteSymbol(RepeatSymbol_11_138);
                    WriteBits(138 - 11, 7);
                }

                // get the remainder
                if ((Count = Count % 138) == 0) continue;

                // remainder is less than 3
                if (Count < 3)
                {
                    // write the code length to output file
                    blTree.WriteSymbol(CodeLen);
                    if (Count > 1) blTree.WriteSymbol(CodeLen);
                    continue;
                }

                // remainder is between 3 to 10
                if (Count <= 10)
                {
                    blTree.WriteSymbol(RepeatSymbol_3_10);
                    WriteBits(Count - 3, 3);
                    continue;
                }

                // remainder is between 11 to 137
                blTree.WriteSymbol(RepeatSymbol_11_138);
                WriteBits(Count - 11, 7);
            }
            return;
        }
    }

    /////////////////////////////////////////////////////////////////////
    // Adler32 Checksum
    /////////////////////////////////////////////////////////////////////

    internal static class Adler32
    {
        /////////////////////////////////////////////////////////////////////
        // Accumulate Adler Checksum
        /////////////////////////////////////////////////////////////////////

        public static UInt32 Checksum
                (
                UInt32 AdlerValue,
                Byte[] Buffer,
                Int32 Pos,
                Int32 Len
                )
        {
            const UInt32 Adler32Base = 65521;

            // split current Adler checksum into two 
            UInt32 AdlerLow = AdlerValue & 0xFFFF;
            UInt32 AdlerHigh = AdlerValue >> 16;

            while (Len > 0)
            {
                // We can defer the modulo operation:
                // Under worst case the starting value of the two halves is 65520 = (AdlerBase - 1)
                // each new byte is maximum 255
                // The low half grows AdlerLow(n) = AdlerBase - 1 + n * 255
                // The high half grows AdlerHigh(n) = (n + 1)*(AdlerBase - 1) + n * (n + 1) * 255 / 2
                // The maximum n before overflow of 32 bit unsigned integer is 5552
                // it is the solution of the following quadratic equation
                // 255 * n * n + (2 * (AdlerBase - 1) + 255) * n + 2 * (AdlerBase - 1 - UInt32.MaxValue) = 0
                Int32 n = Len < 5552 ? Len : 5552;
                Len -= n;
                while (--n >= 0)
                {
                    AdlerLow += (UInt32)Buffer[Pos++];
                    AdlerHigh += AdlerLow;
                }
                AdlerLow %= Adler32Base;
                AdlerHigh %= Adler32Base;
            }
            return ((AdlerHigh << 16) | AdlerLow);
        }
    }

    ////////////////////////////////////////////////////////////////////
    // Reverse the bits of short integer 16 bits
    ////////////////////////////////////////////////////////////////////

    internal static class BitReverse
    {
        ////////////////////////////////////////////////////////////////////
        // Reverse the bits of one byte
        ////////////////////////////////////////////////////////////////////

        private static Byte[] BitReverseTable =  
		{ 
		0x00, 0x80, 0x40, 0xc0, 0x20, 0xa0, 0x60, 0xe0, 0x10, 0x90, 0x50, 0xd0, 0x30, 0xb0, 0x70, 0xf0,  
		0x08, 0x88, 0x48, 0xc8, 0x28, 0xa8, 0x68, 0xe8, 0x18, 0x98, 0x58, 0xd8, 0x38, 0xb8, 0x78, 0xf8,  
		0x04, 0x84, 0x44, 0xc4, 0x24, 0xa4, 0x64, 0xe4, 0x14, 0x94, 0x54, 0xd4, 0x34, 0xb4, 0x74, 0xf4,  
		0x0c, 0x8c, 0x4c, 0xcc, 0x2c, 0xac, 0x6c, 0xec, 0x1c, 0x9c, 0x5c, 0xdc, 0x3c, 0xbc, 0x7c, 0xfc,  
		0x02, 0x82, 0x42, 0xc2, 0x22, 0xa2, 0x62, 0xe2, 0x12, 0x92, 0x52, 0xd2, 0x32, 0xb2, 0x72, 0xf2,  
		0x0a, 0x8a, 0x4a, 0xca, 0x2a, 0xaa, 0x6a, 0xea, 0x1a, 0x9a, 0x5a, 0xda, 0x3a, 0xba, 0x7a, 0xfa, 
		0x06, 0x86, 0x46, 0xc6, 0x26, 0xa6, 0x66, 0xe6, 0x16, 0x96, 0x56, 0xd6, 0x36, 0xb6, 0x76, 0xf6,  
		0x0e, 0x8e, 0x4e, 0xce, 0x2e, 0xae, 0x6e, 0xee, 0x1e, 0x9e, 0x5e, 0xde, 0x3e, 0xbe, 0x7e, 0xfe, 
		0x01, 0x81, 0x41, 0xc1, 0x21, 0xa1, 0x61, 0xe1, 0x11, 0x91, 0x51, 0xd1, 0x31, 0xb1, 0x71, 0xf1, 
		0x09, 0x89, 0x49, 0xc9, 0x29, 0xa9, 0x69, 0xe9, 0x19, 0x99, 0x59, 0xd9, 0x39, 0xb9, 0x79, 0xf9,  
		0x05, 0x85, 0x45, 0xc5, 0x25, 0xa5, 0x65, 0xe5, 0x15, 0x95, 0x55, 0xd5, 0x35, 0xb5, 0x75, 0xf5, 
		0x0d, 0x8d, 0x4d, 0xcd, 0x2d, 0xad, 0x6d, 0xed, 0x1d, 0x9d, 0x5d, 0xdd, 0x3d, 0xbd, 0x7d, 0xfd, 
		0x03, 0x83, 0x43, 0xc3, 0x23, 0xa3, 0x63, 0xe3, 0x13, 0x93, 0x53, 0xd3, 0x33, 0xb3, 0x73, 0xf3,  
		0x0b, 0x8b, 0x4b, 0xcb, 0x2b, 0xab, 0x6b, 0xeb, 0x1b, 0x9b, 0x5b, 0xdb, 0x3b, 0xbb, 0x7b, 0xfb, 
		0x07, 0x87, 0x47, 0xc7, 0x27, 0xa7, 0x67, 0xe7, 0x17, 0x97, 0x57, 0xd7, 0x37, 0xb7, 0x77, 0xf7,  
		0x0f, 0x8f, 0x4f, 0xcf, 0x2f, 0xaf, 0x6f, 0xef, 0x1f, 0x9f, 0x5f, 0xdf, 0x3f, 0xbf, 0x7f, 0xff 
		};

        // Reverse the bits of a 16 bit value.
        public static UInt16 Reverse16Bits(Int32 Value)
        {
            return ((UInt16)((BitReverseTable[Value & 0xff] << 8) | BitReverseTable[(Value >> 8) & 0xff]));
        }
    }
}
