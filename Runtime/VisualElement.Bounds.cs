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
            Rect bound = element.worldBound;
            Rect rootBound = element.GetRootBound();
            return rootBound.Contains(bound.min) && rootBound.Contains(bound.max);
        }

        public static bool IsOutOfScreen<T>(this T element) where T : VisualElement => !element.IsInScreen();
    }
}
