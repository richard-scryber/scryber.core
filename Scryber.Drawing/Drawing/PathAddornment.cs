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

        public virtual void EnsureAdornments(PDFGraphics inGraphics, PathAdornmentInfo info, ContextBase context,
            AdornmentOrder currentOrder, AdornmentPlacements currentPlacement)
        {
            if (currentOrder == this.Order && (currentPlacement & this.Placement) > 0)
            {
                this.Addorner.OutputAdornment(inGraphics, info, context);
            }
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

        public override void EnsureAdornments(PDFGraphics inGraphics, PathAdornmentInfo info, ContextBase context, AdornmentOrder currentOrder,
            AdornmentPlacements currentPlacement)
        {
            base.EnsureAdornments(inGraphics, info, context, currentOrder, currentPlacement);
            
            if(null != this.Next)
            {
                this.Next.EnsureAdornments(inGraphics, info, context, currentOrder, currentPlacement);
            }
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
    }


}