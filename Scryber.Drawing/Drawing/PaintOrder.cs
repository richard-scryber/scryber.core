using System;

namespace Scryber.Drawing
{
    
    public enum PaintOrders
    {
        Fill,
        Markers,
        Stroke
    }
    
    [PDFParsableValue()]
    public struct PaintOrder
    {
        
        public PaintOrders First { get; set; }
        
        public PaintOrders Second { get; set; }
        
        public PaintOrders Third { get; set; }

        
        
        public PaintOrder(PaintOrders first, PaintOrders second, PaintOrders third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }

        public static PaintOrder Default
        {
            get { return new PaintOrder(PaintOrders.Fill, PaintOrders.Markers, PaintOrders.Stroke); }
        }

        public static bool TryParse(string value, out PaintOrder po)
        {
            var result = false;
            try
            {
                po = Parse(value);
                result = true;
            }
            catch (Exception e)
            {
                po = Default;
            }
            return result;
        }

        public static PaintOrder Parse(string value)
        {
            if(string.IsNullOrEmpty(value))
                return new PaintOrder();
            else
            {
                var each = value.Split(',', ' ');
                PaintOrders first = PaintOrders.Fill;
                PaintOrders second = PaintOrders.Markers;
                PaintOrders third = PaintOrders.Stroke;
                Type pot = typeof(PaintOrders);
                
                if (each.Length >= 3)
                {
                    
                    first = (PaintOrders)Enum.Parse(pot, each[0].Trim(), true);
                    second = (PaintOrders)Enum.Parse(pot, each[1].Trim(), true);
                    third = (PaintOrders)Enum.Parse(pot, each[2].Trim(), true);
                }
                else if (each.Length == 2)
                {
                    first = (PaintOrders)Enum.Parse(pot, each[0].Trim(), true);
                    second = (PaintOrders)Enum.Parse(pot, each[1].Trim(), true);
                    
                    switch (first)
                    {
                        case PaintOrders.Fill:
                            if (second == PaintOrders.Markers)
                                third = PaintOrders.Stroke;
                            else
                                third = PaintOrders.Markers;
                            break;
                            
                        case PaintOrders.Markers:
                            if (second == PaintOrders.Stroke)
                                third = PaintOrders.Fill;
                            else
                                third = PaintOrders.Stroke;
                            break;
                        case PaintOrders.Stroke:
                            if (second == PaintOrders.Markers)
                                third = PaintOrders.Fill;
                            else
                                third = PaintOrders.Markers;
                            break;
                    }
                    
                }
                else if (each.Length == 1)
                {
                    first = (PaintOrders)Enum.Parse(pot, each[0].Trim(), true);
                    switch (first)
                    {
                        case PaintOrders.Fill:
                            second = PaintOrders.Markers;
                            third = PaintOrders.Stroke;
                            break;
                            
                        case PaintOrders.Markers:
                            second = PaintOrders.Fill;
                            third = PaintOrders.Stroke;
                            break;
                        case PaintOrders.Stroke:
                            second = PaintOrders.Fill;
                            third = PaintOrders.Markers;
                            break;
                    }
                }
                
                return new PaintOrder(first, second, third);
            }
        }
        
    }
    
}