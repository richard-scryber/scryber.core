using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
	[PDFParsableComponent("progress")]
	public class HTMLProgress : Scryber.Components.Panel
	{
		public const string ProgressBarClass = "-webkit-progress-bar progress-bar";
		private static readonly Unit ProgressBarWidth = new Unit(10, PageUnits.RootEMHeight);
		private static readonly Unit ProgressBarHeight = new Unit(1, PageUnits.RootEMHeight);
		public const string ProgressValueClass = "-webkit-progress-value progress-value";


		private double _value;
		private double _max;

		private ProgressBar _barComponent;
		private ProgressValue _valComponent;

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        /// <summary>
        /// Global Html hidden attribute used with xhtml as hidden='hidden'
        /// </summary>
        [PDFAttribute("hidden")]
        public string Hidden
        {
            get
            {
                if (this.Visible)
                    return string.Empty;
                else
                    return "hidden";
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value != "hidden")
                    this.Visible = true;
                else
                    this.Visible = false;
            }
        }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }


        [PDFAttribute("value")]
		public double Value
		{
			get { return _value; }
			set { _value = value;
                this.UpdateProgressValues();
            }
		}

		[PDFAttribute("max")]
		public double Max
		{
			get { return _max; }
			set {
				_max = value;
				this.UpdateProgressValues();
			}
		}
		public HTMLProgress() : this(HTMLObjectTypes.Progress)
		{
		}

		public HTMLProgress(ObjectType type) : base(type)
		{
			this.Max = 1.0;
			this.InitProgressContent();
		}

		protected virtual void InitProgressContent()
		{
			this._barComponent = new ProgressBar(ProgressBarClass);
			

			this._valComponent = new ProgressValue(ProgressValueClass);

			this._barComponent.Contents.Add(this._valComponent);

			this.InnerContent.Add(_barComponent);
		}

		protected virtual void UpdateProgressValues()
		{
			var max = this._max;
			var val = this._value;

			if (val >= max)
				val = 1.0;

			else if (val < 0.0)
				val = 0.0;

			else if(max != 1.0)
				val = val / max;

			if (null != _valComponent)
				_valComponent.SetPercentWidth(val);
		}

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
			style.Position.PositionMode = PositionMode.InlineBlock;
			style.Background.Color = StandardColors.Gray;
			style.Size.Width = ProgressBarWidth;
			style.Size.Height = ProgressBarHeight;
			style.Overflow.Action = OverflowAction.Clip;
			return style;
        }

        protected override void OnPreLayout(LayoutContext context)
        {
            base.OnPreLayout(context);
			this.UpdateProgressValues();
        }

        public class ProgressBar : Div
		{
            [PDFAttribute("class")]
            public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

            [PDFAttribute("style")]
            public override Style Style { get => base.Style; set => base.Style = value; }

            /// <summary>
            /// Global Html hidden attribute used with xhtml as hidden='hidden'
            /// </summary>
            [PDFAttribute("hidden")]
            public string Hidden
            {
                get
                {
                    if (this.Visible)
                        return string.Empty;
                    else
                        return "hidden";
                }
                set
                {
                    if (string.IsNullOrEmpty(value) || value != "hidden")
                        this.Visible = true;
                    else
                        this.Visible = false;
                }
            }

            [PDFAttribute("title")]
            public override string OutlineTitle
            {
                get => base.OutlineTitle;
                set => base.OutlineTitle = value;
            }


            public ProgressBar(string styleClass)
			{
				this.StyleClass = styleClass;
			}

            protected override Style GetBaseStyle()
            {
                var style = base.GetBaseStyle();
				//style.Background.Color = StandardColors.Gray;
				style.Position.PositionMode = PositionMode.Relative;
                style.Size.Height = new Unit(100, PageUnits.Percent);
				style.Size.Width = new Unit(100, PageUnits.Percent);
                return style;
            }
        }

		private class ProgressValue : Div
		{
            [PDFAttribute("class")]
            public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

            [PDFAttribute("style")]
            public override Style Style { get => base.Style; set => base.Style = value; }

            /// <summary>
            /// Global Html hidden attribute used with xhtml as hidden='hidden'
            /// </summary>
            [PDFAttribute("hidden")]
            public string Hidden
            {
                get
                {
                    if (this.Visible)
                        return string.Empty;
                    else
                        return "hidden";
                }
                set
                {
                    if (string.IsNullOrEmpty(value) || value != "hidden")
                        this.Visible = true;
                    else
                        this.Visible = false;
                }
            }

            [PDFAttribute("title")]
            public override string OutlineTitle
            {
                get => base.OutlineTitle;
                set => base.OutlineTitle = value;
            }


            public ProgressValue(string styleClass)
			{
				this.StyleClass = styleClass;
			}

			public void SetPercentWidth(double zeroToOne)
			{
				double pcent = zeroToOne * 100.0;
				this.Style.Size.Width = new Unit(pcent, PageUnits.Percent);
			}

            protected override Style GetBaseStyle()
            {
                var style = base.GetBaseStyle();
				style.Background.Color = StandardColors.Green;
				style.Position.PositionMode = PositionMode.Relative;
				style.Position.X = 0;
				style.Position.Y = 0;
				style.Size.Width = 0;
				style.Size.Height = new Unit(100, PageUnits.Percent);
				return style;
            }
        }
    }
}

