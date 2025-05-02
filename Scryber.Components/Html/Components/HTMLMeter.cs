using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
	[PDFParsableComponent("meter")]
	public class HTMLMeter : HTMLProgress
	{
		public const string MeterBarClass = "-webkit-meter-bar meter-bar";
        public const string MeterOptimalClass = "-webkit-meter-optimum-value meter-optimum-value optimum";
        public const string MeterSubOptimalClass = "-webkit-meter-suboptimum-value meter-suboptimal-value suboptimum";
        public const string MeterSubSubOptimalClass = "-webkit-meter-suboptimum-value meter-suboptimal-value meter-sub-suboptimal-value suboptimum";

        private static readonly Unit ProgressBarWidth = new Unit(10, PageUnits.RootEMHeight);
		private static readonly Unit ProgressBarHeight = new Unit(1, PageUnits.RootEMHeight);


        private double _low = double.NaN;
        private double _high = double.NaN;
        private double _optimal = double.NaN;
        private double _min = double.NaN;

        [PDFAttribute("min")]
        public double MinValue
        {
            get { return _min; }
            set
            {
                _min = value;
                this.UpdateProgressValues();
            }
        }

        [PDFAttribute("low")]
		public double LowValue
		{
			get { return _low; }
			set { _low = value;
                this.UpdateProgressValues();
            }
		}

		[PDFAttribute("high")]
		public double HighValue
        {
			get { return _high; }
			set {
				_high = value;
				this.UpdateProgressValues();
			}
		}

        [PDFAttribute("optimum")]
        public double OptimumValue
        {
            get { return _optimal; }
            set
            {
                _optimal = value;
                this.UpdateProgressValues();
            }
        }

        private ProgressSubOptimalValue _subValComponent;

        protected ProgressSubOptimalValue SubOptimalValueComponent
        {
            get { return _subValComponent; }
        }

        public HTMLMeter() : this(HTMLObjectTypes.Meter)
		{
		}

		public HTMLMeter(ObjectType type) : base(type)
		{
			this.Max = 1.0;
		}

		protected override void InitProgressContent(string barClass, string valueClass)
		{
            barClass = MeterBarClass;
            valueClass = MeterOptimalClass;

            base.InitProgressContent(barClass, valueClass);

            this._subValComponent = new ProgressSubOptimalValue(MeterSubOptimalClass);

            this.BarComponent.Contents.Add(_subValComponent);
		}

		protected override void UpdateProgressValues()
		{
			var max = this.Max;
            var min = this.MinValue;
            bool suboptimal = false;
            bool subsuboptimal = false;

            if (double.IsNaN(min))
                min = 0.0;

			var val = this.Value;

            var low = this.LowValue;
            if (double.IsNaN(low))
                low = min;

            var high = this.HighValue;
            if (double.IsNaN(high))
                high = max;

            if (val >= max)
                val = 1.0;

            else if (val < min)
                val = 0.0;
            else {

                if (val < low)
                {
                    suboptimal = true;
                    subsuboptimal = true;
                }
                else if (val > high)
                    suboptimal = true;
  
                if (max != 1.0)
                    val = val / max;

            }

            if (null != ValueComponent)
            {
                if (suboptimal)
                {
                    this.SubOptimalValueComponent.SetPercentWidth(val);
                    this.ValueComponent.Hidden = "hidden";
                    this.SubOptimalValueComponent.Hidden = "";

                    if (subsuboptimal)
                        this.SubOptimalValueComponent.StyleClass = MeterSubSubOptimalClass;
                    else
                        this.SubOptimalValueComponent.StyleClass = MeterSubOptimalClass;
                }
                else
                {
                    this.ValueComponent.Hidden = "";
                    this.ValueComponent.SetPercentWidth(val);
                    this.SubOptimalValueComponent.Hidden = "hidden";
                }
            }
		}

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
			
			return style;
        }

        protected override void OnPreLayout(LayoutContext context)
        {
            base.OnPreLayout(context);
			this.UpdateProgressValues();
        }


        /// <summary>
        /// The bar for sub-optimum values
        /// </summary>
		protected class ProgressSubOptimalValue : Div
		{
            [PDFAttribute("class")]
            public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

            [PDFAttribute("style")]
            public override Style Style { get => base.Style; set => base.Style = value; }

            private static readonly Color SubOptColor = Color.Parse("#FFAA00");
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


            public ProgressSubOptimalValue(string styleClass)
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
                style.Background.Color = SubOptColor;
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

