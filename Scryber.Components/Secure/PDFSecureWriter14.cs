using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Scryber.Native;
using Scryber.Components;

namespace Scryber.Secure
{
    /// <summary>
    /// Extends the PDF Writer to encrypt all the object streams and any strings using the provided PDFEncrypter
    /// </summary>
    /// <remarks>The PDFSecureDocument will instantiate this class for writing the document.</remarks>
    
    internal sealed class PDFSecureWriter14 : Scryber.PDFWriter14
    {

        #region private class PDFObjectEncryptionStream
        /// <summary>
        /// Encapsulates the data about a PDFObject for encryption
        /// </summary>
        private class PDFObjectEncryptionStream
        {
            /// <summary>
            /// Gets the object reference for this encryption stream
            /// </summary>
            public PDFObjectRef ObjectRef { get; private set; }

            /// <summary>
            /// Returns true if this object reference's stream is being written to, or we are writing to the general info.
            /// </summary>
            public bool InsideStream { get; set; }

            /// <summary>
            /// Gets or sets the encrypter for this object
            /// </summary>
            public IStreamFilter Encrypter { get; set; }

            /// <summary>
            /// Returns true if this instance has an encrypter
            /// </summary>
            public bool HasEncrypter
            {
                get { return null != Encrypter; }
            }

            public PDFObjectEncryptionStream(PDFObjectRef oref)
            {
                if (null == oref)
                    throw new ArgumentNullException("oref");
                this.ObjectRef = oref;
            }
        }

        #endregion

        #region ivars

        private PDFEncryter _security;
        private PDFObjectRef _encryptionobject;
        private Stack<PDFObjectEncryptionStream> _encrypters;

        #endregion

        //
        // properties
        //

        #region internal PDFEncryter Security {get;}

        /// <summary>
        /// Gets the encrypter associated with this secure writer
        /// </summary>
        internal PDFEncryter Security
        {
            get { return _security; }
        }

        #endregion

        #region private PDFObjectEncryptionStream AssertEncrypterStream {get;}

        /// <summary>
        /// Gets the current encrypter stream data (checking that there is one and throwing an exception if not)
        /// </summary>
        private PDFObjectEncryptionStream AssertEncrypterStream
        {
            get
            {
                if (_encrypters.Count == 0)
                    throw new IndexOutOfRangeException();
                return _encrypters.Peek();
            }
        }

        #endregion

        //
        // .ctor
        //

        #region internal PDFSecureWriter14(Stream stream, PDFTraceLog log, PDFEncryter security)

        /// <summary>
        /// Creates a new PDFSecureWriter14 that will write to the provded stream and encrypt output using the PDFEncrypter (cannot be null).
        /// </summary>
        /// <param name="stream">The stream to ultimately write the data to.</param>
        /// <param name="log">The log to write messages to.</param>
        /// <param name="security">The PDFEncrypter to use.</param>
        internal PDFSecureWriter14(Stream stream, PDFTraceLog log, PDFEncryter security)
            : base(stream, log)
        {
            if (null == security)
                throw new ArgumentNullException("security");
            _security = security;
            _encrypters = new Stack<PDFObjectEncryptionStream>();
        }

        #endregion

        #region internal PDFSecureWriter14(Stream stream, int generation, PDFTraceLog log, PDFEncryter security)

        /// <summary>
        /// Creates a new PDFSecureWriter14 that will write to the provded stream and encrypt output using the PDFEncrypter (cannot be null).
        /// </summary>
        /// <param name="stream">The stream to ultimately write the data to.</param>
        /// <param name="generation">The current generation of the PDF docucment.</param>
        /// <param name="log">The log to write messages to.</param>
        /// <param name="security">The PDFEncrypter to use.</param>
        internal PDFSecureWriter14(Stream stream, int generation, PDFTraceLog log, Version vers, PDFEncryter security)
            : base(stream, generation, log, vers)
        {
            if (null == security)
                throw new ArgumentNullException("security");
            _security = security;
            _encrypters = new Stack<PDFObjectEncryptionStream>();
        }

        #endregion


        //
        // overrides
        //

        #region BeginObject(string name)

        /// <summary>
        /// Overrides the default implementation to push state of encryption data onto a stack before returning the base implementations object
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override PDFObjectRef BeginObject(string name)
        {
            PDFObjectRef oref = base.BeginObject(name);
            PDFObjectEncryptionStream enc = new PDFObjectEncryptionStream(oref);
            _encrypters.Push(enc);
            return oref;
        }

        #endregion

        #region EndObject()

        /// <summary>
        /// Overrides the default implementation to pop the encryption data state from the stack
        /// </summary>
        public override void EndObject()
        {
            _encrypters.Pop();
            base.EndObject();
        }

        #endregion

        #region BeginStream(PDFObjectRef onobject, IStreamFilter[] filters) + EndStream()

        /// <summary>
        /// Overrides the default implementation to record that we are inside a stream 
        /// (we don't encrypt data when we are inside a stream, we encrypt the whole stream at the end).
        /// </summary>
        /// <param name="onobject"></param>
        /// <param name="filters"></param>
        public override void BeginStream(Native.PDFObjectRef onobject, IStreamFilter[] filters)
        {
            AssertEncrypterStream.InsideStream = true;
            base.BeginStream(onobject, filters);
        }

        /// <summary>
        /// Overrides the default implementation to record that we are no longer inside a stream
        /// </summary>
        /// <returns></returns>
        public override long EndStream()
        {
            AssertEncrypterStream.InsideStream = false;
            return base.EndStream();
        }

        #endregion

        #region public override void Close(PDFDocumentID documentid)

        /// <summary>
        /// Overides the default implementaton to output the Encryption object before calling the base method
        /// </summary>
        /// <param name="documentid"></param>
        /// <remarks>A reference to the written encrypt object is held so that it can be output on the trailer dictionary later on</remarks>
        public override void CloseDocument(PDFDocumentID documentid)
        {
            PDFObjectRef enc = this.Security.WriteTo(this);
            _encryptionobject = enc;
            base.CloseDocument(documentid);
        }

        #endregion

        #region protected override void WriteIndirectStreamData(IIndirectObject pfo)

        /// <summary>
        /// Overrides the default implementation to encrypt the data, then write it to the underlying stream
        /// </summary>
        /// <param name="pfo"></param>
        protected override void WriteIndirectStreamData(IIndirectObject pfo)
        {
            if(this.TraceLog.ShouldLog(TraceLevel.Debug))
                this.TraceLog.Begin(TraceLevel.Debug, "Secure Writer", "Encrypting stream for object '" + pfo.Number + " " + pfo.Generation + "'");
            
            byte[] unencrypted = pfo.Stream.GetStreamData();
            IStreamFilter enc = CreateEncryptionFilter(pfo.Number, pfo.Generation);
            byte[] encrypted = enc.FilterStream(unencrypted);
            if (this.TraceLog.ShouldLog(TraceLevel.Debug))
                this.TraceLog.Add(TraceLevel.Debug,"Encryption", "Encrypted stream data, now writing");
            this.BaseStream.Write(Constants.StartStream);
            this.BaseStream.Write(encrypted);
            this.BaseStream.Write(Constants.EndStream);

            if (this.TraceLog.ShouldLog(TraceLevel.Debug))
                this.TraceLog.End(TraceLevel.Debug, "Secure Writer", "Encrypting stream for object '" + pfo.Number + " " + pfo.Generation + "'");
        }

        #endregion

        #region protected override void WriteTrailerDictionary(PDFDocumentID documentid)

        /// <summary>
        /// Overrides the base implementation to write the Encrypt reference to the trailer dictionary.
        /// </summary>
        /// <param name="documentid"></param>
        /// <remarks>The reference comes from the Close method which should have already been called</remarks>
        protected override void WriteTrailerDictionaryEntries(PDFDocumentID documentid)
        {
            base.WriteTrailerDictionaryEntries(documentid);

            //Append the encrypt entry in the document trailer
            if (null != _encryptionobject)
                this.WriteDictionaryObjectRefEntry("Encrypt", _encryptionobject);
        }

        #endregion


        #region public override void WriteStringLiteral(string value, Scryber.FontEncoding encoding)


        /// <summary>
        /// Overrides the base implementation to write strings that are encrypted if they are not a part of an object stream
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        public override void WriteStringLiteral(string value, Scryber.FontEncoding encoding)
        {
            PDFObjectEncryptionStream encrypting = this.AssertEncrypterStream;

            if (encrypting.InsideStream == false)
            {
                //If we are not in an object stream, then we want to encrypt the text
                if (encrypting.HasEncrypter == false)
                    encrypting.Encrypter = this.CreateEncryptionFilter(encrypting.ObjectRef);
                byte[] all = GetStringBytes(value, encoding);
                all = encrypting.Encrypter.FilterStream(all);
                this.WriteStringHex(all);
            }
            else
                //We are in an object stream so don't do any encryption now - wait until we close the indirect object
                base.WriteStringLiteral(value, encoding);
        }

#endregion

        //
        // support methods
        //

#region private byte[] GetStringBytes(string value, Scryber.FontEncoding encoding)

        /// <summary>
        /// Converts a string in the specified encoding into a byte[]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private byte[] GetStringBytes(string value, Scryber.FontEncoding encoding)
        {
            Scryber.Text.PDFEncoding enc = Scryber.Text.PDFEncoding.GetEncoding(encoding);
            byte[] all = enc.GetBytes(value);
            return all;
        }

#endregion

#region private IStreamFilter CreateEncryptionFilter(PDFObjectRef oref)

        /// <summary>
        /// Creates an IStreamFilter for Encrypting data based on the object reference.
        /// </summary>
        /// <param name="oref"></param>
        /// <returns></returns>
        private IStreamFilter CreateEncryptionFilter(PDFObjectRef oref)
        {
            return this._security.CreateEncryptionFilter(oref);
        }

#endregion

#region private IStreamFilter CreateEncryptionFilter(int number, int generation)

        /// <summary>
        /// Creates an IStreamFilter for Encrypting data based on the object number and generation number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="generation"></param>
        /// <returns></returns>
        private IStreamFilter CreateEncryptionFilter(int number, int generation)
        {
            return this._security.CreateEncryptionFilter(number, generation);
        }

#endregion

#region  protected override void Dispose(bool disposing)

        /// <summary>
        /// Overrides the base implmentation to dispose of the security instance
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _security)
                    _security.Dispose();
            }
            base.Dispose(disposing);
        }

#endregion
    }


    public class PDFSecureWrite14Factory : PDFWriterFactory
    {
        private PDFEncryter _enc;

        public PDFSecureWrite14Factory(PDFEncryter enc)
            : base()
        {
            if (null == enc)
                throw new ArgumentNullException(nameof(enc));
            this._enc = enc;
        }

        protected override PDFWriter DoGetInstance(Document forDoc, Stream stream, int generation, PDFDocumentRenderOptions options, PDFTraceLog log)
        {
            return new PDFSecureWriter14(stream, log, this._enc);
        }
    }
}
