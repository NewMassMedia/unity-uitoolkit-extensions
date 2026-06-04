using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public enum Side
    {
        Top,
        Right,
        Bottom,
        Left,
    }

    public enum BorderSide
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft,
    }

    public static partial class VisualElementExtension
    {
        public static T SetName<T>(this T element, string value) where T : VisualElement { element.name = value; return element; }
        public static T SetValue<T, TValue>(this T element, TValue value) where T : INotifyValueChanged<TValue> { element.value = value; return element; }
        public static T OverrideStyleSheet<T>(this T element, StyleSheet value) where T : VisualElement { element.ClearStyleSheet().AddStyleSheet(value); return element; }
        public static T AddStyleSheet<T>(this T element, StyleSheet value) where T : VisualElement { element.styleSheets.Add(value); return element; }
        public static T ClearStyleSheet<T>(this T element) where T : VisualElement { element.styleSheets.Clear(); return element; }
        public static PickingMode GetPickingMode<T>(this T element) where T : VisualElement => element.pickingMode;

        public static T SetPickingModeRecursive<T>(this T element, PickingMode mode) where T : VisualElement
        {
            element.SetPickingMode(mode);
            foreach (VisualElement child in element.hierarchy.Children())
            {
                child.SetPickingModeRecursive(mode);
            }

            return element;
        }

        public static T SetPickingModeRecursive<T>(this T element, bool mode) where T : VisualElement
        {
            element.SetPickingMode(mode);
            foreach (VisualElement child in element.hierarchy.Children())
            {
                child.SetPickingModeRecursive(mode);
            }

            return element;
        }

        public static T SetPickingMode<T>(this T element, PickingMode mode) where T : VisualElement { element.pickingMode = mode; return element; }
        public static T SetPickingMode<T>(this T element, bool mode) where T : VisualElement { element.pickingMode = mode ? PickingMode.Position : PickingMode.Ignore; return element; }
        public static bool GetFocusable<T>(this T element) where T : VisualElement => element.focusable;
        public static T SetFocusable<T>(this T element, bool value) where T : VisualElement { element.focusable = value; return element; }
        public static bool GetDelegatesFocus<T>(this T element) where T : VisualElement => element.delegatesFocus;
        public static T SetDelegatesFocus<T>(this T element, bool value) where T : VisualElement { element.delegatesFocus = value; return element; }
        public static T OverrideClass<T>(this T element, params string[] newClass) where T : VisualElement => element.ClearClass().AddClass(newClass);
        public static T OverrideClass<T>(this T element, string newClass) where T : VisualElement => element.ClearClass().AddClass(newClass);

        public static T AddClass<T>(this T element, params string[] classNames) where T : VisualElement
        {
            foreach (string className in classNames)
            {
                element.AddClass(className);
            }

            return element;
        }

        public static T AddClass<T>(this T element, string className) where T : VisualElement { element.AddToClassList(className); return element; }
        public static T RemoveClass<T>(this T element, string className) where T : VisualElement { element.RemoveFromClassList(className); return element; }
        public static T ClearClass<T>(this T element) where T : VisualElement { element.ClearClassList(); return element; }
        public static T EnableClass<T>(this T element, string className, bool enable) where T : VisualElement { element.EnableInClassList(className, enable); return element; }
        public static T ToggleClass<T>(this T element, string className) where T : VisualElement { element.ToggleInClassList(className); return element; }
    }
}
