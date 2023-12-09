using System;
using Scryber.Styles;

namespace Scryber.Html.Components
{
	[PDFParsableComponent("picture")]
	public class HTMLPicture : Scryber.Components.Panel
	{
		

		private HTMLPictureSourceList _sources;
		private HTMLImage _innerImage;

		/// <summary>
		/// Gets the array of alternative sources for the picture.
		/// </summary>
		[PDFArray(typeof(HTMLPictureSource))]
		[PDFElement]
		public HTMLPictureSourceList Sources
		{
			get
			{
				if (this._sources == null)
					this._sources = new HTMLPictureSourceList();
				return this._sources;
			}
		}

		/// <summary>
		/// The default image source that will be used if none of the sources match. 
		/// </summary>
		[PDFElement("img")]
		public HTMLImage Image
		{
			get { return _innerImage; }
			set
			{
				if(null != this._innerImage)
				{
					this.ClearCurrentImage();
				}

				this._innerImage = value;

				if(null != this._innerImage)
				{
					this.SetCurrentImage();
				}
			}
		}


        public HTMLPicture() : this(HTMLObjectTypes.Picture)
        {
        }

        protected HTMLPicture(ObjectType type) : base(type)
        { }



		protected virtual void ClearCurrentImage()
		{
			this.InnerContent.Remove(this._innerImage);
			this._innerImage = null;
		}


		protected virtual void SetCurrentImage()
		{
			this.InnerContent.Add(this._innerImage);
		}

		protected virtual void EnsureCorrectSourceSet()
		{
			string foundsrc = null;

			if(null != _sources && _sources.Count > 0)
			{
				foreach (var src in _sources)
				{
					if (src.MediaType != null && src.MediaType.IsPrintMedia())
					{
						foundsrc = src.GetBestImageSource();

						if (!string.IsNullOrEmpty(foundsrc))
						{
							break;
						}
					}
				}

				//Nothing explicity set for the print media type - so get teh first that doesn't have a media type

				if (string.IsNullOrEmpty(foundsrc))
				{
                    foreach (var src in _sources)
                    {
                        if (src.MediaType == null)
                        {
                            foundsrc = src.GetBestImageSource();

                            if (!string.IsNullOrEmpty(foundsrc))
                            {
                                break;
                            }
                        }
                    }
                }
			}

			if (!string.IsNullOrEmpty(foundsrc))
			{
				if(null == this._innerImage)
				{
					this._innerImage = new HTMLImage();
					this._innerImage.ElementName = "img";
					this.SetCurrentImage();
				}
				this._innerImage.Source = foundsrc;
			}
		}

		//
		// base overrides for component lifcycle
		//

        /// <summary>
        /// Overrides the default behaviour so the registered sources are also initialized.
        /// </summary>
        /// <param name="context"></param>
        protected override void DoInit(InitContext context)
        {
            base.DoInit(context);

            if (null != this._sources && this._sources.Count > 0)
            {
                foreach (var src in this._sources)
                    src.Init(context);
            }
        }

        /// <summary>
        /// Overrides the default behaviour so the registered sources are also loaded.
        /// </summary>
        /// <param name="context"></param>
        protected override void DoLoad(LoadContext context)
        {
            base.DoLoad(context);

            if (null != this._sources && this._sources.Count > 0)
            {
                foreach (var src in this._sources)
                    src.Load(context);
            }
        }


        /// <summary>
        /// Overrides the default behaviour so the registered sources are also databound.
        /// </summary>
        /// <param name="context"></param>
		/// <param name="includeChildren"></param>
        protected override void DoDataBind(DataContext context, bool includeChildren)
        {
            if (null != this._sources && this._sources.Count > 0)
            {
                foreach (var src in this._sources)
                    src.DataBind(context);
            }

			this.EnsureCorrectSourceSet();

            base.DoDataBind(context, includeChildren);
        }

        protected override Style GetBaseStyle()
        {
			var style = base.GetBaseStyle();
			style.Position.PositionMode = Drawing.PositionMode.Inline;

			return style;
        }


    }


}

