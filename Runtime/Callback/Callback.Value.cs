using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static T Callback<T, TValue>(this T e, EventCallback<ChangeEvent<TValue>> callback) where T : VisualElement, INotifyValueChanged<TValue> { e.RegisterValueChangedCallback(callback); return e; }
        public static T RemoveCallback<T, TValue>(this T e, EventCallback<ChangeEvent<TValue>> callback) where T : VisualElement, INotifyValueChanged<TValue> { e.UnregisterValueChangedCallback(callback); return e; }
        public static T Callback<T>(this T e, EventCallback<ChangeEvent<bool>> callback) where T : VisualElement, INotifyValueChanged<bool> { e.RegisterValueChangedCallback(callback); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<ChangeEvent<bool>> callback) where T : VisualElement, INotifyValueChanged<bool> { e.UnregisterValueChangedCallback(callback); return e; }
        public static T Callback<T>(this T e, EventCallback<ChangeEvent<int>> callback) where T : VisualElement, INotifyValueChanged<int> { e.RegisterValueChangedCallback(callback); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<ChangeEvent<int>> callback) where T : VisualElement, INotifyValueChanged<int> { e.UnregisterValueChangedCallback(callback); return e; }
        public static T Callback<T>(this T e, EventCallback<ChangeEvent<float>> callback) where T : VisualElement, INotifyValueChanged<float> { e.RegisterValueChangedCallback(callback); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<ChangeEvent<float>> callback) where T : VisualElement, INotifyValueChanged<float> { e.UnregisterValueChangedCallback(callback); return e; }
        public static T Callback<T>(this T e, EventCallback<ChangeEvent<Vector2>> callback) where T : VisualElement, INotifyValueChanged<Vector2> { e.RegisterValueChangedCallback(callback); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<ChangeEvent<Vector2>> callback) where T : VisualElement, INotifyValueChanged<Vector2> { e.UnregisterValueChangedCallback(callback); return e; }
    }
}
