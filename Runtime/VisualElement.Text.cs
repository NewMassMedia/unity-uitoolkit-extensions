using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static T SetFont<T>(this T element, Font value) where T : VisualElement { element.style.unityFont = value; return element; }
        public static T SetFontDefinition<T>(this T element, FontDefinition value) where T : VisualElement { element.style.unityFontDefinition = value; return element; }
        public static T SetFontSize<T>(this T element, int value) where T : VisualElement { element.style.fontSize = value; return element; }
        public static T SetFontSize<T>(this T element, uint value) where T : VisualElement { element.style.fontSize = value; return element; }
        public static T SetFontStyleAndWeight<T>(this T element, FontStyle value) where T : VisualElement { element.style.unityFontStyleAndWeight = value; return element; }
        public static T SetWhiteSpace<T>(this T element, WhiteSpace whiteSpace) where T : VisualElement { element.style.whiteSpace = whiteSpace; return element; }
        public static T SetTextOverflow<T>(this T element, TextOverflow textOverflow) where T : VisualElement { element.style.textOverflow = textOverflow; return element; }
        public static T SetTextAlign<T>(this T element, TextAnchor value) where T : VisualElement { element.style.unityTextAlign = value; return element; }
        public static T SetTextColor<T>(this T element, Color value) where T : VisualElement { element.style.color = value; return element; }
        public static T SetTextColor<T>(this T element, StyleKeyword value) where T : VisualElement { element.style.color = value; return element; }
        public static T SetMultiline<T>(this T element, bool value) where T : TextField { element.multiline = value; return element; }
        public static T SetMaxLength<T>(this T element, int value) where T : TextField { element.maxLength = value; return element; }
        public static T SetIsPassword<T>(this T element, bool value) where T : TextField { element.isPasswordField = value; return element; }
        public static T SetIsReadonly<T>(this T element, bool value) where T : TextField { element.isReadOnly = value; return element; }
        public static T SetMaskChar<T>(this T element, char value) where T : TextField { element.maskChar = value; return element; }
        public static T SetText<T>(this T element, string value) where T : TextElement { element.text = value; return element; }
        public static T SetTextWithDisplay<T>(this T element, string value) where T : TextElement => element.SetText(value).SetDisplay(!string.IsNullOrEmpty(value));
        public static T SetEnableRichText<T>(this T element, bool value) where T : TextElement { element.enableRichText = value; return element; }
        public static T SetDisplayTooltipWhenElided<T>(this T element, bool displayTooltipWhenElided) where T : TextElement { element.displayTooltipWhenElided = displayTooltipWhenElided; return element; }
        public static T SetParseEscapeSequences<T>(this T element, bool parseEscapeSequences) where T : TextElement { element.parseEscapeSequences = parseEscapeSequences; return element; }
        public static T SetIsSelectable<T>(this T element, bool value) where T : TextElement { element.selection.isSelectable = value; return element; }
#pragma warning disable CS0618
        public static T SetSelectionColor<T>(this T element, Color value) where T : TextElement { element.selection.selectionColor = value; return element; }
        public static T SetCursorColor<T>(this T element, Color value) where T : TextElement { element.selection.cursorColor = value; return element; }
#pragma warning restore CS0618
        public static T SetCursorIndex<T>(this T element, int value) where T : TextElement { element.selection.cursorIndex = value; return element; }
        public static T SetSelectIndex<T>(this T element, int value) where T : TextElement { element.selection.selectIndex = value; return element; }
        public static T SetDoubleClickSelectsWord<T>(this T element, bool value) where T : TextElement { element.selection.doubleClickSelectsWord = value; return element; }
        public static T SetTripleClickSelectsLine<T>(this T element, bool value) where T : TextElement { element.selection.tripleClickSelectsLine = value; return element; }
        public static Image SetScaleMode(this Image element, ScaleMode value) { element.scaleMode = value; return element; }
        public static Image SetTintColor(this Image element, Color value) { element.tintColor = value; return element; }
    }
}
