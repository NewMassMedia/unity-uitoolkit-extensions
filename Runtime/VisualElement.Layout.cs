using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public readonly struct BoxStyleLengths
    {
        public BoxStyleLengths(StyleLength top, StyleLength right, StyleLength bottom, StyleLength left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public StyleLength Top { get; }
        public StyleLength Right { get; }
        public StyleLength Bottom { get; }
        public StyleLength Left { get; }
    }

    public static partial class VisualElementExtension
    {
        public static float GetOpacity<T>(this T element) where T : VisualElement => element.style.opacity.value;
        public static T SetOpacity<T>(this T element, float value) where T : VisualElement { element.style.opacity = value; return element; }
        public static T ToggleDisplay<T>(this T element) where T : VisualElement { element.style.display = element.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None; return element; }
        public static DisplayStyle GetDisplay<T>(this T element) where T : VisualElement => element.style.display.value;
        public static T SetDisplay<T>(this T element, DisplayStyle value) where T : VisualElement { element.style.display = value; return element; }
        public static T SetDisplay<T>(this T element, bool value) where T : VisualElement { element.style.display = value ? DisplayStyle.Flex : DisplayStyle.None; return element; }
        public static T Hide<T>(this T element) where T : VisualElement { element.style.display = DisplayStyle.None; return element; }
        public static T Show<T>(this T element, bool show) where T : VisualElement { element.style.display = show ? DisplayStyle.Flex : DisplayStyle.None; return element; }
        public static T Show<T>(this T element) where T : VisualElement { element.style.display = DisplayStyle.Flex; return element; }
        public static Visibility GetVisible<T>(this T element) where T : VisualElement => element.style.visibility.value;
        public static T SetVisible<T>(this T element, Visibility value) where T : VisualElement { element.style.visibility = value; return element; }
        public static T SetVisible<T>(this T element, bool value) where T : VisualElement { element.visible = value; return element; }
        public static Overflow GetOverflow<T>(this T element) where T : VisualElement => element.style.overflow.value;
        public static T SetOverflow<T>(this T element, Overflow value) where T : VisualElement { element.style.overflow = value; return element; }
        public static T SetOverflow<T>(this T element, bool value) where T : VisualElement { element.style.overflow = value ? Overflow.Visible : Overflow.Hidden; return element; }

        public static T CopyPosition<T>(this T element, T copyTarget) where T : VisualElement
        {
            return element.SetPosition(copyTarget.GetPosition())
                .SetLeft(copyTarget.GetLeftLength())
                .SetTop(copyTarget.GetTopLength())
                .SetRight(copyTarget.GetRightLength())
                .SetBottom(copyTarget.GetBottomLength());
        }

        public static T ClearPosition<T>(this T element) where T : VisualElement => element.SetPosition(new StyleLength(StyleKeyword.Auto));
        public static Position GetPosition<T>(this T element) where T : VisualElement => element.style.position.value;
        public static T SetPosition<T>(this T element, Position value) where T : VisualElement { element.style.position = value; return element; }

        public static T SetPosition<T>(this T element, StyleLength value) where T : VisualElement
        {
            element.style.left = value;
            element.style.right = value;
            element.style.bottom = value;
            element.style.top = value;
            return element;
        }

        public static T SetPositionWithAbsolute<T>(this T element, float value) where T : VisualElement
        {
            element.style.position = Position.Absolute;
            element.style.left = value;
            element.style.right = value;
            element.style.bottom = value;
            element.style.top = value;
            return element;
        }

        public static T SetPositionWithAbsolute<T>(this T element, Vector2 value) where T : VisualElement
        {
            element.style.position = Position.Absolute;
            element.style.left = value.x;
            element.style.top = value.y;
            return element;
        }

        public static Length GetLeftLength<T>(this T element) where T : VisualElement => element.style.left.value;
        public static T SetLeft<T>(this T element, Length value) where T : VisualElement { element.style.left = value; return element; }
        public static float GetLeft<T>(this T element) where T : VisualElement => element.style.left.value.value;
        public static T SetLeft<T>(this T element, float value) where T : VisualElement { element.style.left = value; return element; }
        public static Length GetTopLength<T>(this T element) where T : VisualElement => element.style.top.value;
        public static T SetTop<T>(this T element, Length value) where T : VisualElement { element.style.top = value; return element; }
        public static float GetTop<T>(this T element) where T : VisualElement => element.style.top.value.value;
        public static T SetTop<T>(this T element, float value) where T : VisualElement { element.style.top = value; return element; }
        public static T SetTop<T>(this T element, StyleKeyword value) where T : VisualElement { element.style.top = value; return element; }
        public static Length GetRightLength<T>(this T element) where T : VisualElement => element.style.right.value;
        public static T SetRight<T>(this T element, Length value) where T : VisualElement { element.style.right = value; return element; }
        public static float GetRight<T>(this T element) where T : VisualElement => element.style.right.value.value;
        public static T SetRight<T>(this T element, float value) where T : VisualElement { element.style.right = value; return element; }
        public static Length GetBottomLength<T>(this T element) where T : VisualElement => element.style.bottom.value;
        public static T SetBottom<T>(this T element, Length value) where T : VisualElement { element.style.bottom = value; return element; }
        public static float GetBottom<T>(this T element) where T : VisualElement => element.style.bottom.value.value;
        public static T SetBottom<T>(this T element, float value) where T : VisualElement { element.style.bottom = value; return element; }
        public static T SetBottom<T>(this T element, StyleKeyword value) where T : VisualElement { element.style.bottom = value; return element; }
        public static T SetFlexBasis<T>(this T element, float value) where T : VisualElement { element.style.flexBasis = value; return element; }
        public static T SetFlexShrink<T>(this T element, float value) where T : VisualElement { element.style.flexShrink = value; return element; }
        public static T SetFlexShrink<T>(this T element, bool value) where T : VisualElement { element.style.flexShrink = value ? 1 : 0; return element; }
        public static T SetFlexGrow<T>(this T element, float value) where T : VisualElement { element.style.flexGrow = value; return element; }
        public static T SetFlexGrow<T>(this T element, bool value) where T : VisualElement { element.style.flexGrow = value ? 1 : 0; return element; }
        public static T SetFlexDirection<T>(this T element, FlexDirection value) where T : VisualElement { element.style.flexDirection = value; return element; }
        public static T SetFlexWrap<T>(this T element, Wrap value) where T : VisualElement { element.style.flexWrap = value; return element; }
        public static T SetFlexWrap<T>(this T element, bool value) where T : VisualElement { element.style.flexWrap = value ? Wrap.Wrap : Wrap.NoWrap; return element; }
        public static T SetAlignItems<T>(this T element, Align value) where T : VisualElement { element.style.alignItems = value; return element; }
        public static T SetAlignContent<T>(this T element, Align value) where T : VisualElement { element.style.alignContent = value; return element; }
        public static T SetJustifyContent<T>(this T element, Justify value) where T : VisualElement { element.style.justifyContent = value; return element; }
        public static T SetAlignSelf<T>(this T element, Align value) where T : VisualElement { element.style.alignSelf = value; return element; }
        public static T SetSize<T>(this T element, Vector2 value) where T : VisualElement { element.SetWidth(value.x).SetHeight(value.y); return element; }
        public static StyleLength GetWidthStyle<T>(this T element) where T : VisualElement => element.style.width;
        public static float GetWidth(this VisualElement element) => element.style.width.value.value;
        public static StyleLength GetMinWidthStyle<T>(this T element) where T : VisualElement => element.style.minWidth;
        public static float GetMinWidth(this VisualElement element) => element.style.minWidth.value.value;
        public static StyleLength GetMaxWidthStyle<T>(this T element) where T : VisualElement => element.style.maxWidth;
        public static float GetMaxWidth(this VisualElement element) => element.style.maxWidth.value.value;
        public static T SetWidth<T>(this T element, float value) where T : VisualElement { element.style.width = value; return element; }
        public static T SetWidth<T>(this T element, Length value) where T : VisualElement { element.style.width = value; return element; }
        public static T SetWidth<T>(this T element, StyleKeyword value) where T : VisualElement { element.style.width = value; return element; }
        public static T SetMinWidth<T>(this T element, float value) where T : VisualElement { element.style.minWidth = value; return element; }
        public static T SetMaxWidth<T>(this T element, float value) where T : VisualElement { element.style.maxWidth = value; return element; }
        public static StyleLength GetHeightStyle<T>(this T element) where T : VisualElement => element.style.height;
        public static float GetHeight(this VisualElement element) => element.style.height.value.value;
        public static StyleLength GetMinHeightStyle<T>(this T element) where T : VisualElement => element.style.minHeight;
        public static float GetMinHeight(this VisualElement element) => element.style.minHeight.value.value;
        public static StyleLength GetMaxHeightStyle<T>(this T element) where T : VisualElement => element.style.maxHeight;
        public static float GetMaxHeight(this VisualElement element) => element.style.maxHeight.value.value;
        public static T SetHeight<T>(this T element, float value) where T : VisualElement { element.style.height = value; return element; }
        public static T SetHeight<T>(this T element, Length value) where T : VisualElement { element.style.height = value; return element; }
        public static T SetHeight<T>(this T element, StyleKeyword value) where T : VisualElement { element.style.height = value; return element; }
        public static T SetMinHeight<T>(this T element, float value) where T : VisualElement { element.style.minHeight = value; return element; }
        public static T SetMaxHeight<T>(this T element, float value) where T : VisualElement { element.style.maxHeight = value; return element; }
        public static BoxStyleLengths GetMarginStyles<T>(this T element) where T : VisualElement => new(element.style.marginTop, element.style.marginRight, element.style.marginBottom, element.style.marginLeft);
        public static Vector4 GetMargin<T>(this T element) where T : VisualElement => new(element.GetMargin(Side.Top), element.GetMargin(Side.Right), element.GetMargin(Side.Bottom), element.GetMargin(Side.Left));
        public static StyleLength GetMarginStyle<T>(this T element, Side side) where T : VisualElement
        {
            return side switch
            {
                Side.Top => element.style.marginTop,
                Side.Right => element.style.marginRight,
                Side.Bottom => element.style.marginBottom,
                Side.Left => element.style.marginLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        public static float GetMargin<T>(this T element, Side side) where T : VisualElement
        {
            return side switch
            {
                Side.Top => element.style.marginTop.value.value,
                Side.Right => element.style.marginRight.value.value,
                Side.Bottom => element.style.marginBottom.value.value,
                Side.Left => element.style.marginLeft.value.value,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        public static T SetMargin<T>(this T element, float margin) where T : VisualElement
        {
            element.style.marginTop = margin;
            element.style.marginRight = margin;
            element.style.marginBottom = margin;
            element.style.marginLeft = margin;
            return element;
        }

        public static T SetMargin<T>(this T element, float top, float right, float bottom, float left) where T : VisualElement
        {
            element.style.marginTop = top;
            element.style.marginRight = right;
            element.style.marginBottom = bottom;
            element.style.marginLeft = left;
            return element;
        }

        public static T SetMargin<T>(this T element, Side side, float margin) where T : VisualElement
        {
            switch (side)
            {
                case Side.Top: element.style.marginTop = margin; break;
                case Side.Right: element.style.marginRight = margin; break;
                case Side.Bottom: element.style.marginBottom = margin; break;
                case Side.Left: element.style.marginLeft = margin; break;
                default: throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            return element;
        }

        public static T SetMarginX<T>(this T element, float margin) where T : VisualElement { element.style.marginLeft = margin; element.style.marginRight = margin; return element; }
        public static T SetMarginY<T>(this T element, float margin) where T : VisualElement { element.style.marginTop = margin; element.style.marginBottom = margin; return element; }
        public static BoxStyleLengths GetPaddingStyles<T>(this T element) where T : VisualElement => new(element.style.paddingTop, element.style.paddingRight, element.style.paddingBottom, element.style.paddingLeft);
        public static Vector4 GetPadding<T>(this T element) where T : VisualElement => new(element.GetPadding(Side.Top), element.GetPadding(Side.Right), element.GetPadding(Side.Bottom), element.GetPadding(Side.Left));
        public static StyleLength GetPaddingStyle<T>(this T element, Side side) where T : VisualElement
        {
            return side switch
            {
                Side.Top => element.style.paddingTop,
                Side.Right => element.style.paddingRight,
                Side.Bottom => element.style.paddingBottom,
                Side.Left => element.style.paddingLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        public static float GetPadding<T>(this T element, Side side) where T : VisualElement
        {
            return side switch
            {
                Side.Top => element.style.paddingTop.value.value,
                Side.Right => element.style.paddingRight.value.value,
                Side.Bottom => element.style.paddingBottom.value.value,
                Side.Left => element.style.paddingLeft.value.value,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        public static T SetPadding<T>(this T element, float padding) where T : VisualElement
        {
            element.style.paddingTop = padding;
            element.style.paddingRight = padding;
            element.style.paddingBottom = padding;
            element.style.paddingLeft = padding;
            return element;
        }

        public static T SetPadding<T>(this T element, float top, float right, float bottom, float left) where T : VisualElement
        {
            element.style.paddingTop = top;
            element.style.paddingRight = right;
            element.style.paddingBottom = bottom;
            element.style.paddingLeft = left;
            return element;
        }

        public static T SetPadding<T>(this T element, Side side, float padding) where T : VisualElement
        {
            switch (side)
            {
                case Side.Top: element.style.paddingTop = padding; break;
                case Side.Right: element.style.paddingRight = padding; break;
                case Side.Bottom: element.style.paddingBottom = padding; break;
                case Side.Left: element.style.paddingLeft = padding; break;
                default: throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            return element;
        }

        public static T SetPaddingX<T>(this T element, float padding) where T : VisualElement { element.style.paddingLeft = padding; element.style.paddingRight = padding; return element; }
        public static T SetPaddingY<T>(this T element, float padding) where T : VisualElement { element.style.paddingTop = padding; element.style.paddingBottom = padding; return element; }
    }
}
