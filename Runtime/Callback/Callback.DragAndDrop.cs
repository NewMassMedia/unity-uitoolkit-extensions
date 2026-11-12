using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        // Enter/leave do not bubble but trickle through every ancestor, so they default to at-target like the pointer pair.
        public static T Callback<T>(this T e, EventCallback<DragEnterEvent> action, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<DragEnterEvent> action, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<DragLeaveEvent> action, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<DragLeaveEvent> action, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<DragUpdatedEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<DragUpdatedEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<DragPerformEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<DragPerformEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<DragExitedEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<DragExitedEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
    }
}
