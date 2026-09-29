using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static T Callback<T>(this T e, EventCallback<MouseDownEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<MouseDownEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<MouseUpEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<MouseUpEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<MouseMoveEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<MouseMoveEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        /// <summary>
        /// Registers <see cref="MouseEnterEvent"/> at target by default (<see cref="TrickleDown.NoTrickleDown"/>). Enter/leave events do not bubble,
        /// but they trickle down through every ancestor, so a trickle-down registration also receives each descendant's
        /// enter. Pass <see cref="TrickleDown.TrickleDown"/> only to observe descendants on purpose. Default changed in 0.1.12.
        /// </summary>
        public static T Callback<T>(this T e, EventCallback<MouseEnterEvent> action, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        /// <summary>Unregisters <see cref="MouseEnterEvent"/>; the default phase matches the <c>Callback</c> overload (<see cref="TrickleDown.NoTrickleDown"/>).</summary>
        public static T RemoveCallback<T>(this T e, EventCallback<MouseEnterEvent> action, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        /// <summary>
        /// Registers <see cref="MouseLeaveEvent"/> at target by default (<see cref="TrickleDown.NoTrickleDown"/>). Enter/leave events do not bubble,
        /// but they trickle down through every ancestor, so a trickle-down registration also receives each descendant's
        /// leave. Pass <see cref="TrickleDown.TrickleDown"/> only to observe descendants on purpose. Default changed in 0.1.12.
        /// </summary>
        public static T Callback<T>(this T e, EventCallback<MouseLeaveEvent> action, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        /// <summary>Unregisters <see cref="MouseLeaveEvent"/>; the default phase matches the <c>Callback</c> overload (<see cref="TrickleDown.NoTrickleDown"/>).</summary>
        public static T RemoveCallback<T>(this T e, EventCallback<MouseLeaveEvent> action, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
        public static T Callback<T>(this T e, EventCallback<WheelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.RegisterCallback(action, trickleDown); return e; }
        public static T RemoveCallback<T>(this T e, EventCallback<WheelEvent> action, TrickleDown trickleDown = TrickleDown.TrickleDown) where T : VisualElement { e.UnregisterCallback(action, trickleDown); return e; }
    }
}
