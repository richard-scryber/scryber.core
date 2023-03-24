using System;
namespace Scryber.Data
{

	/// <summary>
	/// Wraps the DataContent values into a single class.
	/// </summary>
	public class DataBindingContent
	{
		/// <summary>
		/// Gets or sets the actual string content of the binding content
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets the mime type for the defined content so it can be parsed
		/// </summary>
		public MimeType Type { get; set; }

		/// <summary>
		/// Gets or sets the action to take when adding any parsed content to a component
		/// </summary>
		public DataContentAction Action { get; set; }


		public DataBindingContent(string content, MimeType type, DataContentAction action)
		{
			this.Content = content;
			this.Type = type;
			this.Action = action;
		}
	}
}

