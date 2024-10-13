using System.Linq.Expressions;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Styles.Selectors;

namespace Scryber.Svg.Components;

public class SVGBaseStyleSheet : StyleGroup
{

    protected SVGBaseStyleSheet() : base()
    {
        
    }







    private static StyleGroup _default;
    
    public static StyleGroup Default
    {
        get
        {
            return _default;
        }
    }

    static SVGBaseStyleSheet()
    {
        var sheet = new SVGBaseStyleSheet();
        AddRect(sheet);
        AddCircle(sheet);
        AddEllipse(sheet);
        AddAnchor(sheet);
        AddLine(sheet);
        AddPath(sheet);
        AddPolygon(sheet);
        AddPolyline(sheet);
        AddText(sheet);
        _default = sheet;
        sheet.Immutable = true;
    }

    private static void AddRect(SVGBaseStyleSheet tosheet)
    {
        StyleDefn defn = new StyleDefn();
        defn.Match = new StyleMatcher(new StyleSelector() { AppliedElement = "rect" });
        
        defn.Position.PositionMode = PositionMode.Absolute;
        defn.Position.X = 0;
        defn.Position.Y = 0;
        defn.IsSVGGeometry = true;
        tosheet.Styles.Add(defn);
    }

    private static void AddCircle(SVGBaseStyleSheet tosheet)
    {
        StyleDefn defn = new StyleDefn();
        defn.Match = new StyleMatcher(new StyleSelector() { AppliedElement = "circle" });
        defn.SetValue(StyleKeys.SVGGeometryXKey, Unit.Zero);
        defn.SetValue(StyleKeys.SVGGeometryYKey, Unit.Zero);
        
        defn.Position.PositionMode = PositionMode.Absolute;
        
        defn.IsSVGGeometry = true;
        
        tosheet.Styles.Add(defn);
    }
    
    private static void AddEllipse(SVGBaseStyleSheet tosheet)
    {
        StyleDefn defn = new StyleDefn();
        defn.Match = new StyleMatcher(new StyleSelector() { AppliedElement = "ellipse" });
        
        defn.SetValue(StyleKeys.SVGFillKey, new SVGFillColorValue(StandardColors.Black, "black"));
        defn.SetValue(StyleKeys.SVGGeometryXKey, Unit.Zero);
        defn.SetValue(StyleKeys.SVGGeometryYKey, Unit.Zero);
        
        defn.Position.PositionMode = PositionMode.Absolute;
        defn.IsSVGGeometry = true;
        
        tosheet.Styles.Add(defn);
    }
    
    private static void AddAnchor(SVGBaseStyleSheet tosheet)
    {
        StyleDefn defn = new StyleDefn();
        defn.Match = new StyleMatcher(new StyleSelector() { AppliedElement = "a" });
        
        defn.SetValue(StyleKeys.SVGGeometryXKey, Unit.Zero);
        defn.SetValue(StyleKeys.SVGGeometryYKey, Unit.Zero);
        
        defn.Position.PositionMode = PositionMode.Absolute;
        defn.IsSVGGeometry = true;
        
        tosheet.Styles.Add(defn);
    }
    
    private static void AddLine(SVGBaseStyleSheet tosheet)
    {
        StyleDefn defn = new StyleDefn();
        defn.Match = new StyleMatcher(new StyleSelector() { AppliedElement = "line" });
        
        defn.SetValue(StyleKeys.SVGGeometryXKey, Unit.Zero);
        defn.SetValue(StyleKeys.SVGGeometryYKey, Unit.Zero);
        
        defn.Position.PositionMode = PositionMode.Absolute;
        defn.IsSVGGeometry = true;
        
        tosheet.Styles.Add(defn);
    }
    
    private static void AddPath(SVGBaseStyleSheet tosheet)
    {
        StyleDefn defn = new StyleDefn();
        defn.Match = new StyleMatcher(new StyleSelector() { AppliedElement = "path" });
        
        
        defn.Position.PositionMode = PositionMode.Absolute;
        defn.IsSVGGeometry = true;
        
        tosheet.Styles.Add(defn);
    }
    
    private static void AddPolygon(SVGBaseStyleSheet tosheet)
    {
        StyleDefn defn = new StyleDefn();
        defn.Match = new StyleMatcher(new StyleSelector() { AppliedElement = "polygon" });
        
        
        defn.Position.PositionMode = PositionMode.Absolute;
        defn.IsSVGGeometry = true;
        
        tosheet.Styles.Add(defn);
    }
    
    private static void AddPolyline(SVGBaseStyleSheet tosheet)
    {
        StyleDefn defn = new StyleDefn();
        defn.Match = new StyleMatcher(new StyleSelector() { AppliedElement = "polyline" });
        
        
        defn.Position.PositionMode = PositionMode.Absolute;
        defn.IsSVGGeometry = true;
        
        tosheet.Styles.Add(defn);
    }
    
    private static void AddText(SVGBaseStyleSheet tosheet)
    {
        StyleDefn defn = new StyleDefn();
        defn.Match = new StyleMatcher(new StyleSelector() { AppliedElement = "text" });
        
        defn.SetValue(StyleKeys.SVGGeometryXKey, Unit.Zero);
        defn.SetValue(StyleKeys.SVGGeometryYKey, Unit.Zero);
        
        defn.Position.DisplayMode = DisplayMode.Block;
        defn.Position.PositionMode = PositionMode.Absolute;
        defn.Padding.Right = 4;
        defn.Text.WrapText = Text.WordWrap.NoWrap;
        defn.Text.PositionFromBaseline = true;
        defn.Text.Leading = Unit.Auto;
        defn.Text.CharacterSpacing = 0;
        defn.Text.WordSpacing = 0;
        defn.Text.Decoration = Scryber.Text.TextDecoration.None;
        defn.Text.FirstLineInset = 0;
        defn.Text.PreserveWhitespace = false;
        defn.IsSVGGeometry = true;
        
        tosheet.Styles.Add(defn);
    }
    
    
    
    
}