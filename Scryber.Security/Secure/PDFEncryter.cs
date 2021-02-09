using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using Scryber.Native;

namespace Scryber.Secure
{
    internal abstract class PDFEncryter : IDisposable
    {

        private byte[] _owner;
        private byte[] _user;
        private byte[] _keystem;
        private int _vers;
        private int _rev;
        private int _keysizebytes;
        private PermissionFlags _flags;
        

        /// <summary>
        /// Gets the owner password as a secure string
        /// </summary>
        public byte[] OwnerHash
        {
            get { return _owner; }
            private set { _owner = value; }
        }

        /// <summary>
        /// Gets the client password as a secure string
        /// </summary>
        public byte[] UserHash
        {
            get { return _user; }
            private set { _user = value; }
        }


        /// <summary>
        /// Gets the key stem that can be used to build the individual object encrytpion keys
        /// </summary>
        public byte[] KeyStem
        {
            get { return _keystem; }
            private set { _keystem = value; }
        }
       
        /// <summary>
        /// Gets the encryption version that this security info will render
        /// </summary>
        public virtual int EncryptionVersion
        {
            get { return _vers; }
            private set { _vers = value; }
        }

        /// <summary>
        /// Gets the encryption revision that this security info will render.
        /// </summary>
        public virtual int EncryptionRevision
        {
            get { return _rev; }
            private set { _rev = value; }
        }

        /// <summary>
        /// Gets the size in bytes of the encryption 
        /// </summary>
        public virtual int EncryptionKeySizeBytes
        {
            get { return _keysizebytes; }
            private set { _keysizebytes = value; }
        }

        /// <summary>
        /// Gets the flags that set the permissions and restrictions that this rendered document can perform
        /// </summary>
        internal virtual PermissionFlags ProtectionFlags
        {
            get { return _flags; }
            private set { _flags = value; }
        }

        //
        // .ctor(s)
        //


        internal PDFEncryter(int version, int revision, int keysizebytes, 
                                      byte[] owner, byte[] user, byte[] keystem, 
                                      PermissionFlags protection)
        {
            this.EncryptionVersion = version;
            this.EncryptionRevision = revision;
            this.EncryptionKeySizeBytes = keysizebytes;
            this.OwnerHash = owner;
            this.UserHash = user;
            this.KeyStem = keystem;
            this.ProtectionFlags = protection;
        }


        internal abstract PDFObjectRef WriteTo(PDFWriter writer);

        internal IStreamFilter CreateEncryptionFilter(PDFObjectRef oref)
        {
            if (null == oref)
                throw new ArgumentNullException("oref");

            return CreateEncryptionFilter(oref.Number, oref.Generation);
        }

        internal abstract IStreamFilter CreateEncryptionFilter(int objectnum, int objectgen);

        //
        // IDisposable
        //

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        internal void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != this._owner)
                    Array.Clear(_owner, 0, _owner.Length);
                if (null != this._keystem)
                    Array.Clear(_keystem, 0, _keystem.Length);
                if (null != this._user)
                    Array.Clear(_user, 0, _user.Length);
            }
        }





    }
}
