using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static void RemoveAllChildren(this VisualElement element)
        {
            if (element == null) return;
            while (element.childCount > 0)
            {
                element.RemoveAt(0);
            }
        }

        public static T AddChild<T, U>(this T root, U child) where T : VisualElement where U : VisualElement
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (child == null) throw new ArgumentNullException(nameof(child));
            root.Add(child);
            return root;
        }

        public static void AddChildren<T>(this VisualElement element, T children) where T : IEnumerable<VisualElement>
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (children == null) throw new ArgumentNullException(nameof(children));
            foreach (VisualElement child in children)
            {
                element.Add(child);
            }
        }

        public static void Destroy<T>(this T element) where T : VisualElement
        {
            if (element == null) return;
            element.parent?.Remove(element);
        }
    }
}
