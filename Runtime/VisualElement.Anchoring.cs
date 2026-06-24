using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    /// <summary>Result of <see cref="VisualElementExtension.ResolveAnchoredPlacement"/>: the floating content's
    /// top-left in the anchor/bounds coordinate space, plus the side it actually landed on (differs from the
    /// requested side when an edge flip was needed).</summary>
    public readonly struct AnchoredPlacement
    {
        public AnchoredPlacement(Vector2 topLeft, Side side)
        {
            TopLeft = topLeft;
            Side = side;
        }

        public Vector2 TopLeft { get; }
        public Side Side { get; }
    }

    public static partial class VisualElementExtension
    {
        // Place `size` content beside `anchor` on `preferredSide`, flipping to the opposite side when the preferred
        // side lacks room and the opposite side has more, then clamping the whole rect inside `bounds` (inset by
        // `edgeMargin`). Pure geometry — no panel/layout — so it unit-tests directly. Side.Top/Bottom = above/below
        // the point, Left/Right = beside it; `sideOffset` is the gap between the anchor and the content's near edge.
        public static AnchoredPlacement ResolveAnchoredPlacement(Rect bounds, Vector2 anchor, Vector2 size,
                                                                 Side preferredSide, float sideOffset, float edgeMargin)
        {
            Side side = FlipIfNoRoom(bounds, anchor, size, preferredSide, sideOffset);

            float left, top;
            switch (side)
            {
                case Side.Top:    left = anchor.x - size.x * 0.5f;       top = anchor.y - sideOffset - size.y;  break;
                case Side.Bottom: left = anchor.x - size.x * 0.5f;       top = anchor.y + sideOffset;           break;
                case Side.Left:   left = anchor.x - sideOffset - size.x; top = anchor.y - size.y * 0.5f;        break;
                default:          left = anchor.x + sideOffset;          top = anchor.y - size.y * 0.5f;        break; // Right
            }

            left = ClampAxis(left, size.x, bounds.xMin, bounds.xMax, edgeMargin);
            top  = ClampAxis(top,  size.y, bounds.yMin, bounds.yMax, edgeMargin);
            return new AnchoredPlacement(new Vector2(left, top), side);
        }

        // Position THIS element anchored at a panel-space point, using its resolved size and the panel rect as bounds.
        // Call after layout (the element needs a real resolvedStyle size). For a tail-less floating UI (tooltip /
        // dropdown / menu); a callout that must also move a tail consumes ResolveAnchoredPlacement directly.
        public static T PlaceAnchored<T>(this T element, Vector2 anchorPanelPoint, Side preferredSide,
                                         float sideOffset, float edgeMargin) where T : VisualElement
        {
            Vector2 size = new(element.resolvedStyle.width, element.resolvedStyle.height);
            AnchoredPlacement p = ResolveAnchoredPlacement(element.GetRootBound(), anchorPanelPoint, size,
                                                           preferredSide, sideOffset, edgeMargin);
            return element.SetLeft(p.TopLeft.x).SetTop(p.TopLeft.y);
        }

        // Flip only when the preferred side overflows AND the opposite side has more room, so content larger than
        // either gap stays put (then gets clamped) instead of thrashing between two too-small sides.
        private static Side FlipIfNoRoom(Rect bounds, Vector2 anchor, Vector2 size, Side side, float offset)
        {
            float roomAbove = (anchor.y - offset) - bounds.yMin;
            float roomBelow = bounds.yMax - (anchor.y + offset);
            float roomLeft  = (anchor.x - offset) - bounds.xMin;
            float roomRight = bounds.xMax - (anchor.x + offset);
            return side switch
            {
                Side.Top    => roomAbove < size.y && roomBelow > roomAbove ? Side.Bottom : side,
                Side.Bottom => roomBelow < size.y && roomAbove > roomBelow ? Side.Top    : side,
                Side.Left   => roomLeft  < size.x && roomRight > roomLeft  ? Side.Right  : side,
                _           => roomRight < size.x && roomLeft  > roomRight ? Side.Left   : side, // Right
            };
        }

        // Clamp a [pos, pos+len] span inside [min+margin, max-margin]; if the span is wider than the available room,
        // pin to the near edge so the overflow spills past the far edge rather than the position going past it.
        private static float ClampAxis(float pos, float len, float min, float max, float margin)
        {
            float lo = min + margin;
            float hi = max - margin - len;
            return hi < lo ? lo : Mathf.Clamp(pos, lo, hi);
        }
    }
}
