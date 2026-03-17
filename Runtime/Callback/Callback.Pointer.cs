using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static T Callback<T>(this T e, EventCallback<PointerCaptureOutEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<PointerCaptureOutEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<PointerEnterEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<PointerEnterEvent> action) where T : VisualElement { e.UnregisterCallback(action); return e; }
        public static T Callback<T>(this T e, EventCallback<PointerLeaveEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<PointerLeaveEvent> action) where T : VisualElement { e.UnregisterCallback(action); return e; }
        public static T Callback<T>(this T e, EventCallback<PointerDownEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<PointerDownEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<PointerMoveEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<PointerMoveEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<PointerUpEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<PointerUpEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<PointerCancelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<PointerCancelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
    }
}
