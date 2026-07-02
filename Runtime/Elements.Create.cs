using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    /// <summary>Factory helpers for constructing pre-styled UI Toolkit elements in one call.</summary>
    public static class Elements
    {
        public static Label Label(string text, string ussClass, bool pickable = false)
            => new Label(text).AddClass(ussClass).SetPickingMode(pickable);
    }
}
