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
	public class ImageDataProxy : ImageRasterData
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

		public bool IsSuccessful
		{
			get
			{
				if (this._request.IsCompleted)
					return this._request.IsSuccessful;
				else
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

		public override ImageType ImageType
		{
			get
			{
				if (null == this.ImageData)
					return ImageType.Unknown;
				else
					return this.ImageData.ImageType;
			}
			protected set => base.ImageType = value;
		}


		public ImageData ImageData
		{
			get{ return this._innerImage; }
		}


        public ImageDataProxy(IRemoteRequest request, string path)
			: base(path, 1, 1)
		{
			this._request = request ?? throw new ArgumentNullException(nameof(request));
			this._innerImage = null;
			//this._request.Completed += this.RequestCompleted;
			this.VerticalResolution = 72;
			this.HorizontalResolution = 72;
			this.HasAlpha = false;

			if (this._request.IsCompleted)
			{

				this.RequestCompleted(this, new RequestCompletedEventArgs(request.Owner, request, request.Result));

				//We have got this far and the request still has an error.
				if(this._request.IsSuccessful == false && this._request.Error != null)
				{
                    var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
					if(null != service && !service.ImagingOptions.AllowMissingImages)
					{
						throw new Scryber.PDFMissingImageException("The image for " + request.FilePath + " could not be loaded", this._request.Error);
					}
                }
			}
			else
				this._request.Completed += this.RequestCompleted;

		}

        private void RequestCompleted(object sender, RequestCompletedEventArgs args)
        {
			if (ReferenceEquals(args.Request, this._request))
			{
				var log = (null != args.Raiser && null != args.Raiser.Document ? args.Raiser.Document.TraceLog : null);

				if (null != log)
					log.Add(TraceLevel.Verbose, "Image Proxy", "Marking the remote image request as complete for '" + _request.FilePath + "' and fulfilling");

				this.EnsureFulfilled(log);
			}
			else
				throw new PDFException("The reference should match the raised request");
        }

        public override string ToString()
        {
            return "Image Proxy for : " + base.ToString();
        }

        public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
			this.EnsureFulfilled(context.TraceLog);

			if (null == _innerImage)
			{
				context.TraceLog.Add(TraceLevel.Error, "Image Proxy", "As no image data is assigned to the inner image for the proxy, no image data can be rendered. Returning null for the object reference");
				return null;
			}
			else
				return _innerImage.Render(name, filters, context, writer);
        }

        public override Size GetSize()
        {
			this.EnsureFulfilled(null);

			if (null != this._innerImage)
				return this._innerImage.GetSize();
			else
				return base.GetSize();
        }

        public override void ResetFilterCache()
        {
			if (null != this._innerImage && !_initializing)
				this._innerImage.ResetFilterCache();
        }

		private bool _logFulfilled = false;
		

		private bool EnsureFulfilled(Logging.TraceLog log)
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
					if (null != log && !_logFulfilled)
					{
						log.Add(TraceLevel.Message, "Image Proxy", "Fulfilled the request for the remote image " + _request.FilePath + " and set the image data for the proxy ");
						_logFulfilled = true;
					}
					return true;
				}
				else
				{
                    if (null != log)
                        log.Add(TraceLevel.Warning, "Image Proxy", "Failed request for the remote image '" + _request.FilePath + "',  " + (null != _request.Error ? _request.Error.Message : "No Exception details"));

                    this._innerImage = GetMissingImage();
				}
			}
			else
			{
				if (null != log)
					log.Add(TraceLevel.Warning, "Image Proxy", "The request for the remote image '" + _request.FilePath + "' was not completed at the time of needing");

                this._innerImage = GetMissingImage();
			}

			return false;
		}

		public override Rect? GetClippingRect(Point offset, Size available, ContextBase context)
		{
			this.EnsureFulfilled(context.TraceLog);

			if (null != this._innerImage)
				return this._innerImage.GetClippingRect(offset, available, context);
			else
				return base.GetClippingRect(offset, available, context);
		}

		public override Point GetRequiredOffsetForRender(Point offset, Size available, ContextBase context)
		{
			this.EnsureFulfilled(context.TraceLog);

			if (null != this._innerImage)
				return this._innerImage.GetRequiredOffsetForRender(offset, available, context);
			else
				return base.GetRequiredOffsetForRender(offset, available, context);
		}

		public override Size GetRequiredSizeForRender(Point offset, Size available, ContextBase context)
		{
			this.EnsureFulfilled(context.TraceLog);
			if (null != this._innerImage)
				return this._innerImage.GetRequiredSizeForRender(offset, available, context);
			else
				return base.GetRequiredSizeForRender(offset, available, context);
		}

		protected ImageData GetMissingImage()
		{
			return null;
		}

		protected virtual void SetPixelData(ImageData inner)
		{
			this._initializing = true;
			
			this.SourcePath = inner.SourcePath;
			this.Filters = inner.Filters;
			this.HasAlpha = inner.HasAlpha;

			if (inner is ImageRasterData rasterData)
			{
				this.PixelWidth = rasterData.PixelWidth;
				this.PixelHeight = rasterData.PixelHeight;
				this.BitsPerColor = rasterData.BitsPerColor;
				this.ColorSpace = rasterData.ColorSpace;
				this.ColorsPerSample = rasterData.ColorsPerSample;
				this.HorizontalResolution = rasterData.HorizontalResolution;
				this.VerticalResolution = rasterData.VerticalResolution;
			}

			this._initializing = false;
		}
    }
}

