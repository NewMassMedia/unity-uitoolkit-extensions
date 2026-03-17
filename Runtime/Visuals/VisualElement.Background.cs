using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static Color GetBackgroundColor<T>(this T element) where T : VisualElement => element.style.backgroundColor.value;
        public static T SetBackgroundColor<T>(this T element, Color value) where T : VisualElement { element.style.backgroundColor = value; return element; }
        public static T SetBackgroundImage<T>(this T element, Texture2D value) where T : VisualElement { element.style.backgroundImage = value; return element; }
        public static T SetBackgroundImage<T>(this T element, Sprite value) where T : VisualElement { element.style.backgroundImage = new StyleBackground(value); return element; }
        public static T SetBackgroundImage<T>(this T element, VectorImage value) where T : VisualElement { element.style.backgroundImage = new StyleBackground(value); return element; }
        public static T SetBackgroundImage<T>(this T element, RenderTexture value) where T : VisualElement { element.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(value)); return element; }
        public static T ClearBackgroundImage<T>(this T element) where T : VisualElement { element.style.backgroundImage = StyleKeyword.None; return element; }
        public static Color GetBackgroundTintColor<T>(this T element) where T : VisualElement => element.style.unityBackgroundImageTintColor.value;
        public static T SetBackgroundTintColor<T>(this T element, Color value) where T : VisualElement { element.style.unityBackgroundImageTintColor = value; return element; }
        public static T SetBackgroundSize<T>(this T element, BackgroundSizeType value) where T : VisualElement { element.style.backgroundSize = new BackgroundSize(value); return element; }
        public static T SetBackgroundRepeat<T>(this T element, BackgroundRepeat value) where T : VisualElement { element.style.backgroundRepeat = value; return element; }
    }
}
