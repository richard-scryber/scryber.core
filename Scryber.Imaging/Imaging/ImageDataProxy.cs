using System;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.Imaging
{

	/// <summary>
	/// The image data proxy acts as a wrapper around an asyncronously loaded image,
	/// so it can be referenced and cached as normal before the image is actually loaded.
	/// But the request should be fufilled before it is used.
	/// </summary>
	public class ImageDataProxy : ImageData
	{
		/// <summary>
		/// The captured async request
		/// </summary>
		private IRemoteRequest _request;

		/// <summary>
		/// The actual image that has been loaded.
		/// </summary>
		private ImageData _innerImage;

		/// <summary>
		/// Flag that identifies if the current pixel data is being set.
		/// </summary>
		private bool _initializing = false;

        /// <summary>
        /// Returns true if this proxy's request is complete
        /// </summary>
        public bool IsFulfilled
		{
			get
			{
				if (this._request.IsCompleted)
				{
					return true;
				}
				return false;
			}
		}


        public override bool IsPrecompressedData
		{
			get {
				if (null != this._innerImage)
					return this._innerImage.IsPrecompressedData;
				else
					return false;
			}
		}


        public ImageDataProxy(IRemoteRequest request, string path)
			: base(path, 1, 1)
		{
			this._request = request ?? throw new ArgumentNullException(nameof(request));
			this._innerImage = null;
			this._request.Completed += this.RequestCompleted;
			this.VerticalResolution = 72;
			this.HorizontalResolution = 72;
			this.HasAlpha = false;
		}

        private void RequestCompleted(object sender, RequestCompletedEventArgs args)
        {
			if (ReferenceEquals(args.Request, this._request))
				this.EnsureFulfilled();
			else
				throw new PDFException("The reference should match the raised request");
        }

        public override string ToString()
        {
            return "Image Proxy for : " + base.ToString();
        }

        public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
			this.EnsureFulfilled();

			if (null == _innerImage)
				return null;
			else
				return _innerImage.Render(name, filters, context, writer);
        }

        public override Size GetSize()
        {
			this.EnsureFulfilled();

            return base.GetSize();
        }

        public override void ResetFilterCache()
        {
			if (null != this._innerImage && !_initializing)
				this._innerImage.ResetFilterCache();
        }


		private bool EnsureFulfilled()
		{
			if (_request.IsCompleted)
			{
				if (_request.IsSuccessful)
				{
					if (null == _innerImage)
					{
						_innerImage = (ImageData)_request.Result;
						this.SetPixelData(_innerImage);
					}

					return true;
				}
				else
				{
					this._innerImage = GetMissingImage();
				}
			}
			else
				this._innerImage = GetMissingImage();

			return false;
		}

		protected ImageData GetMissingImage()
		{
			return null;
		}

		protected virtual void SetPixelData(ImageData inner)
		{
			this._initializing = true;
			
			this.SourcePath = inner.SourcePath;
			this.PixelWidth = inner.PixelWidth;
			this.PixelHeight = inner.PixelHeight;
			this.BitsPerColor = inner.BitsPerColor;
			this.ColorSpace = inner.ColorSpace;
			this.ColorsPerSample = inner.ColorsPerSample;
			this.HorizontalResolution = inner.HorizontalResolution;
			this.VerticalResolution = inner.VerticalResolution;
			this.Filters = inner.Filters;
			this.HasAlpha = inner.HasAlpha;

			this._initializing = false;
		}
    }
}

