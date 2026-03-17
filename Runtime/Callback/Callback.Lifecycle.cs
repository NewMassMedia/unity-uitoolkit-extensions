using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static T Callback<T>(this T e, EventCallback<AttachToPanelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<AttachToPanelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<DetachFromPanelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<DetachFromPanelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<GeometryChangedEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<GeometryChangedEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<CustomStyleResolvedEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<CustomStyleResolvedEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<TransitionEndEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<TransitionEndEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<TransitionCancelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<TransitionCancelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
    }
}
