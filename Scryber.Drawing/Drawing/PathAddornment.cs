using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{
    public abstract class PathAdornment
    {
        public IPathAdorner Addorner { get; set; }
        
        public AdornmentOrder Order { get; set; }

        public AdornmentPlacements Placement { get; set; }

        protected PathAdornment(IPathAdorner adorner, AdornmentOrder order, AdornmentPlacements placements)
        {
            this.Addorner = adorner;
            this.Order = order;
            this.Placement = placements;
        }

        /// <summary>
        /// Renders any adornments that are relevant for the current order and placement using the info and returns the final point in the path that was used.
        /// </summary>
        /// <param name="inGraphics"></param>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <param name="currentOrder"></param>
        /// <param name="currentPlacement"></param>
        /// <returns></returns>
        public virtual Point EnsureAdornments(PDFGraphics inGraphics, PathAdornmentInfo info, ContextBase context,
            AdornmentOrder currentOrder, AdornmentPlacements currentPlacement)
        {
            if (currentOrder == this.Order && (currentPlacement & this.Placement) > 0)
            {
                this.Addorner.OutputAdornment(inGraphics, info, context);
            }

            //TODO: Update this based on the path and adorner data.
            return info.Location;
        }

        public virtual bool HasStarts(AdornmentOrder order)
        {
            if (this.Order == order)
            {
                if ((this.Placement & AdornmentPlacements.Start) > 0)
                    return true;
            }

            return false;

        }
        
        public virtual bool HasMids(AdornmentOrder order)
        {
            if (this.Order == order)
            {
                if ((this.Placement & AdornmentPlacements.Middle) > 0)
                    return true;
            }

            return false;

        }
        
        public virtual bool HasEnds(AdornmentOrder order)
        {
            if (this.Order == order)
            {
                if ((this.Placement & AdornmentPlacements.End) > 0)
                    return true; 
                
            }

            return false;

        }
        
    }
    
    

    /// <summary>
    /// Extendes the PathAdornemt to be a linked list.
    /// </summary>
    public class PathMultiAdornment : PathAdornment
    {
        public PathMultiAdornment Next { get; set; }

        public PathMultiAdornment(IPathAdorner adorner, AdornmentOrder order, AdornmentPlacements placements) :
            base(adorner, order, placements)
        {
            
        }

        public override Point EnsureAdornments(PDFGraphics inGraphics, PathAdornmentInfo info, ContextBase context, AdornmentOrder currentOrder,
            AdornmentPlacements currentPlacement)
        {
            var loc = base.EnsureAdornments(inGraphics, info, context, currentOrder, currentPlacement);
            
            if(null != this.Next)
            {
                loc =this.Next.EnsureAdornments(inGraphics, info, context, currentOrder, currentPlacement);
            }

            return loc;
        }

        public void Append(IPathAdorner adorner, AdornmentOrder order, AdornmentPlacements placements)
        {
            if (null == this.Next)
            {
                this.Next = new PathMultiAdornment(adorner, order, placements);
            }
            else
            {
                this.Next.Append(adorner, order, placements);
            }
        }

        public override bool HasStarts(AdornmentOrder order)
        {
            if (base.HasStarts(order))
                return true;
            else if (null != this.Next)
                return this.Next.HasStarts(order);
            else
                return false;
        }

        public override bool HasMids(AdornmentOrder order)
        {
            if (base.HasMids(order))
                return true;
            else if (null != this.Next)
                return this.Next.HasMids(order);
            else
                return false;
        }

        public override bool HasEnds(AdornmentOrder order)
        {
            if (base.HasEnds(order))
                return true;
            else if (null != this.Next)
                return this.Next.HasEnds(order);
            else
                return false;
        }
    }


}