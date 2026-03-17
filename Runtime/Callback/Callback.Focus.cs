using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static T Callback<T>(this T e, EventCallback<FocusOutEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<FocusOutEvent> action) where T : VisualElement { e.UnregisterCallback(action); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<FocusOutEvent> action, TrickleDown trickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<FocusInEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<FocusInEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<BlurEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<BlurEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<FocusEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<FocusEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
    }
}
