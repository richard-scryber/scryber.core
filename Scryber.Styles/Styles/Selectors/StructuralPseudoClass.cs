using System.Text;

namespace Scryber.Styles.Selectors
{
    /// <summary>
    /// The kind of structural (positional) pseudo-class applied to a selector.
    /// </summary>
    public enum StructuralPseudoClassKind
    {
        NthChild,
        NthLastChild,
        FirstChild,
        LastChild,
        OnlyChild,
        NthOfType,
        NthLastOfType,
        FirstOfType,
        LastOfType,
        OnlyOfType
    }

    /// <summary>
    /// Represents a parsed structural pseudo-class (:nth-child, :first-child, :nth-of-type, etc.)
    /// and evaluates it against a component's reported sibling position, via ISiblingIndexProvider.
    /// Evaluated once per component based on tree position - unlike state pseudo-classes
    /// (:hover, :before, :after) this is not a runtime/render-time variant.
    /// </summary>
    public class StructuralPseudoClass
    {
        public StructuralPseudoClassKind Kind { get; }

        /// <summary>
        /// The 'a' coefficient of the An+B formula. Only meaningful for the Nth* kinds.
        /// </summary>
        public int A { get; }

        /// <summary>
        /// The 'b' coefficient of the An+B formula. Only meaningful for the Nth* kinds.
        /// </summary>
        public int B { get; }

        public StructuralPseudoClass(StructuralPseudoClassKind kind, int a = 0, int b = 0)
        {
            this.Kind = kind;
            this.A = a;
            this.B = b;
        }

        public bool IsMatch(ISiblingIndexProvider provider)
        {
            if (null == provider)
                return false;

            switch (this.Kind)
            {
                case StructuralPseudoClassKind.FirstChild:
                    return provider.SiblingIndex == 1;

                case StructuralPseudoClassKind.LastChild:
                    return provider.SiblingIndex == provider.SiblingCount;

                case StructuralPseudoClassKind.OnlyChild:
                    return provider.SiblingCount == 1;

                case StructuralPseudoClassKind.NthChild:
                    return MatchesFormula(provider.SiblingIndex);

                case StructuralPseudoClassKind.NthLastChild:
                    return MatchesFormula(provider.SiblingCount - provider.SiblingIndex + 1);

                case StructuralPseudoClassKind.FirstOfType:
                    return provider.SiblingOfTypeIndex == 1;

                case StructuralPseudoClassKind.LastOfType:
                    return provider.SiblingOfTypeIndex == provider.SiblingOfTypeCount;

                case StructuralPseudoClassKind.OnlyOfType:
                    return provider.SiblingOfTypeCount == 1;

                case StructuralPseudoClassKind.NthOfType:
                    return MatchesFormula(provider.SiblingOfTypeIndex);

                case StructuralPseudoClassKind.NthLastOfType:
                    return MatchesFormula(provider.SiblingOfTypeCount - provider.SiblingOfTypeIndex + 1);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true if there exists an integer n >= 0 such that index == (A * n) + B.
        /// </summary>
        private bool MatchesFormula(int index)
        {
            if (this.A == 0)
                return index == this.B;

            int diff = index - this.B;
            if (diff % this.A != 0)
                return false;

            return (diff / this.A) >= 0;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            this.ToString(sb);
            return sb.ToString();
        }

        internal void ToString(StringBuilder sb)
        {
            switch (this.Kind)
            {
                case StructuralPseudoClassKind.FirstChild: sb.Append(":first-child"); break;
                case StructuralPseudoClassKind.LastChild: sb.Append(":last-child"); break;
                case StructuralPseudoClassKind.OnlyChild: sb.Append(":only-child"); break;
                case StructuralPseudoClassKind.FirstOfType: sb.Append(":first-of-type"); break;
                case StructuralPseudoClassKind.LastOfType: sb.Append(":last-of-type"); break;
                case StructuralPseudoClassKind.OnlyOfType: sb.Append(":only-of-type"); break;
                case StructuralPseudoClassKind.NthChild: sb.Append(":nth-child(").Append(this.A).Append("n").Append(this.B >= 0 ? "+" : "").Append(this.B).Append(")"); break;
                case StructuralPseudoClassKind.NthLastChild: sb.Append(":nth-last-child(").Append(this.A).Append("n").Append(this.B >= 0 ? "+" : "").Append(this.B).Append(")"); break;
                case StructuralPseudoClassKind.NthOfType: sb.Append(":nth-of-type(").Append(this.A).Append("n").Append(this.B >= 0 ? "+" : "").Append(this.B).Append(")"); break;
                case StructuralPseudoClassKind.NthLastOfType: sb.Append(":nth-last-of-type(").Append(this.A).Append("n").Append(this.B >= 0 ? "+" : "").Append(this.B).Append(")"); break;
            }
        }
    }
}
