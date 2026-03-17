using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static T SetTransitionProperty<T>(this T element, StyleList<StylePropertyName> value) where T : VisualElement { element.style.transitionProperty = value; return element; }
        public static T SetTransitionDuration<T>(this T element, StyleList<TimeValue> value) where T : VisualElement { element.style.transitionDuration = value; return element; }
        public static T SetTransitionTimingFunction<T>(this T element, StyleList<EasingFunction> value) where T : VisualElement { element.style.transitionTimingFunction = value; return element; }
        public static T SetOrigin<T>(this T element, TransformOrigin value) where T : VisualElement { element.style.transformOrigin = value; return element; }
        public static T SetOrigin<T>(this T element, Vector2 value) where T : VisualElement { element.style.transformOrigin = new TransformOrigin(value.x, value.y); return element; }
        public static T SetTranslate<T>(this T element, Vector2 value) where T : VisualElement { element.style.translate = new Translate(value.x, value.y); return element; }
        public static T SetTranslate<T>(this T element, Translate value) where T : VisualElement { element.style.translate = new StyleTranslate(value); return element; }
        public static T SetScale<T>(this T element, Vector2 value) where T : VisualElement { element.style.scale = value; return element; }
        public static T SetRotate<T>(this T element, float value) where T : VisualElement { element.style.rotate = new Rotate(new Angle(value)); return element; }
    }
}
