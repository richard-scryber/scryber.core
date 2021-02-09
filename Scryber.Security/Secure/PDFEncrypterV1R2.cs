using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Native;
namespace Scryber.Secure
{
    internal class PDFEncrypterV1R2 : PDFEncrypterStd
    {
        private const int Version1 = 1;
        private const int Revsion2 = 2;
        private const int Revision2KeySizeBytes = 5;

        public PDFEncrypterV1R2(byte[] owner, byte[] user, byte[] key, PermissionFlags protection)
            : base(Version1, Revsion2, Revision2KeySizeBytes, owner, user, key, protection)
        {
        }

        
        
    }
}
