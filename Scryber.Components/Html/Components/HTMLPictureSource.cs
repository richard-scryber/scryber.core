using System;
using System.Collections.Generic;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Styles.Selectors;

namespace Scryber.Html.Components
{
	[PDFParsableComponent("source")]
	public class HTMLPictureSource : Scryber.Components.Component
	{

		/// <summary>
		/// Gets or sets the media selector for this picture source
		/// </summary>
		[PDFAttribute("media")]
		public MediaMatcher MediaType { get; set; }

		/// <summary>
		/// Gets or sets the comma separated list or image sources with density or width attributes
		/// </summary>
		[PDFAttribute("srcset")]
		public string SourceSet { get; set; }

		/// <summary>
		/// Gets or sets the single source value for the image
		/// </summary>
		[PDFAttribute("src")]
		public string Source { get; set; }


		[PDFAttribute("type")]
		public string MIMEType { get; set; }


		public HTMLPictureSource() : this(HTMLObjectTypes.PictureSource)
		{
		}

		protected HTMLPictureSource(ObjectType type) : base(type)
		{
		}

		public string GetBestImageSource()
		{
			string src;

			if (string.IsNullOrEmpty(this.SourceSet))
				return null;

			if (!string.IsNullOrEmpty(MIMEType) && !IsSupportedMimeType(this.MIMEType))
				return null;

			if (!string.IsNullOrEmpty(SourceSet) && TryGetBestSourceSetImage(this.SourceSet, out src))
				return src;

			else
				return this.Source;

		}

		protected bool TryGetBestSourceSetImage(string srcset, out string found)
		{
			if (srcset.IndexOf(',') > 0) {

				var all = srcset.Split(',');
				double max = 0.0;
				found = string.Empty;

				//go through each one looking for the highest multiplier (we are for a document so this is the best image).

				for (var i = 0; i < all.Length; i++)
				{
					var str = all[i].Trim();
					if (str.EndsWith("x") || str.EndsWith("w")) {
						var last = str.LastIndexOf(' ');
						if (last > -1)
						{
							var mult = str.Substring(last, str.Length - (last + 1));

							if (double.TryParse(mult, out double parsed) && parsed > max)
							{
								max = parsed;
								found = str.Substring(0, last).Trim();
							}
						}
						else if(max == 0.0)
						{
							found = str;
						}
					}

					else
					{
						if(max == 0.0)
						{
							found = str;
						}
					}
				}

				return !string.IsNullOrEmpty(found);
			}
			else
			{
				found = srcset.Trim();
				if(found.EndsWith("x") || found.EndsWith("w"))
				{
					var last = found.LastIndexOf(' ');
					if (last > 0)
					{
						found = found.Substring(0, last).Trim();
					}
				}
				return !string.IsNullOrEmpty(found);
			}
		}

		protected virtual bool IsSupportedMimeType(string type)
		{
			switch (type.ToLowerInvariant())
			{
				case "image/jpeg":
				case "image/bmp":
				case "image/x-png":
				case "image/png":
				case "image/gif":
				case "image/svg":
				case "image/svg+xml":
				case "image/tiff":
                    return true;
				default:
					return false;
			}
		}
    }

	/// <summary>
	/// A simple list of sources
	/// </summary>
    public class HTMLPictureSourceList : List<HTMLPictureSource>
    {
        public HTMLPictureSourceList() : base()
        {
        }
    }
}

