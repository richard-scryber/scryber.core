using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF.Secure
{
    internal class PDFEncrypterV2R3 : PDFEncrypterStd
    {
        private const int Version2 = 2;
        private const int Revision3 = 3;
        internal const int StandardKeyLength = 16; //128 Bit

        internal PDFEncrypterV2R3(byte[] owner, byte[] user, int keysizebytes, byte[] key, PermissionFlags protection)
            : base(Version2, Revision3, keysizebytes, owner, user, key, protection)
        {
        }


    }
}
