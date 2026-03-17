using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static Vector4 GetBorderWidth<T>(this T element) where T : VisualElement => new(element.GetBorderWidth(Side.Top), element.GetBorderWidth(Side.Right), element.GetBorderWidth(Side.Bottom), element.GetBorderWidth(Side.Left));

        public static float GetBorderWidth<T>(this T element, Side side) where T : VisualElement
        {
            return side switch
            {
                Side.Top => element.style.borderTopWidth.value,
                Side.Right => element.style.borderRightWidth.value,
                Side.Bottom => element.style.borderBottomWidth.value,
                Side.Left => element.style.borderLeftWidth.value,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        public static T SetBorderWidth<T>(this T element, float width) where T : VisualElement
        {
            element.style.borderTopWidth = width;
            element.style.borderRightWidth = width;
            element.style.borderBottomWidth = width;
            element.style.borderLeftWidth = width;
            return element;
        }

        public static T SetBorderWidth<T>(this T element, float top, float right, float bottom, float left) where T : VisualElement
        {
            element.style.borderTopWidth = top;
            element.style.borderRightWidth = right;
            element.style.borderBottomWidth = bottom;
            element.style.borderLeftWidth = left;
            return element;
        }

        public static T SetBorderWidth<T>(this T element, Side side, float width) where T : VisualElement
        {
            switch (side)
            {
                case Side.Top: element.style.borderTopWidth = width; break;
                case Side.Right: element.style.borderRightWidth = width; break;
                case Side.Bottom: element.style.borderBottomWidth = width; break;
                case Side.Left: element.style.borderLeftWidth = width; break;
                default: throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            return element;
        }

        public static Vector4 GetBorderRadius<T>(this T element) where T : VisualElement => new(element.GetBorderRadius(BorderSide.TopLeft), element.GetBorderRadius(BorderSide.TopRight), element.GetBorderRadius(BorderSide.BottomRight), element.GetBorderRadius(BorderSide.BottomLeft));

        public static float GetBorderRadius<T>(this T element, BorderSide side) where T : VisualElement
        {
            return side switch
            {
                BorderSide.TopLeft => element.style.borderTopLeftRadius.value.value,
                BorderSide.TopRight => element.style.borderTopRightRadius.value.value,
                BorderSide.BottomRight => element.style.borderBottomRightRadius.value.value,
                BorderSide.BottomLeft => element.style.borderBottomLeftRadius.value.value,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        public static T SetBorderRadius<T>(this T element, float radius) where T : VisualElement
        {
            element.style.borderTopLeftRadius = radius;
            element.style.borderTopRightRadius = radius;
            element.style.borderBottomRightRadius = radius;
            element.style.borderBottomLeftRadius = radius;
            return element;
        }

        public static T SetBorderRadius<T>(this T element, float topLeft, float topRight, float bottomRight, float bottomLeft) where T : VisualElement
        {
            element.style.borderTopLeftRadius = topLeft;
            element.style.borderTopRightRadius = topRight;
            element.style.borderBottomRightRadius = bottomRight;
            element.style.borderBottomLeftRadius = bottomLeft;
            return element;
        }

        public static T SetBorderRadius<T>(this T element, BorderSide side, float radius) where T : VisualElement
        {
            switch (side)
            {
                case BorderSide.TopLeft: element.style.borderTopLeftRadius = radius; break;
                case BorderSide.TopRight: element.style.borderTopRightRadius = radius; break;
                case BorderSide.BottomLeft: element.style.borderBottomLeftRadius = radius; break;
                case BorderSide.BottomRight: element.style.borderBottomRightRadius = radius; break;
                default: throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            return element;
        }

        public static Color GetBorderColor<T>(this T element, Side side) where T : VisualElement
        {
            return side switch
            {
                Side.Top => element.style.borderTopColor.value,
                Side.Right => element.style.borderRightColor.value,
                Side.Bottom => element.style.borderBottomColor.value,
                Side.Left => element.style.borderLeftColor.value,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        public static T SetBorderColor<T>(this T element, Color color) where T : VisualElement
        {
            element.style.borderTopColor = color;
            element.style.borderRightColor = color;
            element.style.borderBottomColor = color;
            element.style.borderLeftColor = color;
            return element;
        }

        public static T SetBorderColor<T>(this T element, Color top, Color right, Color bottom, Color left) where T : VisualElement
        {
            element.style.borderTopColor = top;
            element.style.borderRightColor = right;
            element.style.borderBottomColor = bottom;
            element.style.borderLeftColor = left;
            return element;
        }

        public static T SetBorderColor<T>(this T element, Side side, Color color) where T : VisualElement
        {
            switch (side)
            {
                case Side.Top: element.style.borderTopColor = color; break;
                case Side.Right: element.style.borderRightColor = color; break;
                case Side.Bottom: element.style.borderBottomColor = color; break;
                case Side.Left: element.style.borderLeftColor = color; break;
                default: throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            return element;
        }
    }
}
