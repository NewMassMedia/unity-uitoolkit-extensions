using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static Rect GetRootBound<T>(this T element) where T : VisualElement
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            VisualElement root = element.panel?.visualTree;
            return root != null ? root.worldBound : Rect.zero;
        }

        public static bool IsInScreen<T>(this T element) where T : VisualElement
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (element.panel == null) return false;
            return IsInsideInclusive(element.worldBound, element.GetRootBound());
        }

        // Rect.Contains excludes xMax/yMax, so an element flush with the right or bottom edge would read as outside.
        internal static bool IsInsideInclusive(Rect inner, Rect outer)
        {
            return inner.xMin >= outer.xMin && inner.yMin >= outer.yMin
                   && inner.xMax <= outer.xMax && inner.yMax <= outer.yMax;
        }

        public static bool IsOutOfScreen<T>(this T element) where T : VisualElement => !element.IsInScreen();
    }
}
