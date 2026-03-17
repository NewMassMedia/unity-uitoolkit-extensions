using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        public static Dictionary<TKey, T> ToDictionary<T, TKey>(this UQueryBuilder<T> queryBuilder, Func<T, TKey> keySelector) where T : VisualElement
        {
            if (queryBuilder == null) throw new ArgumentNullException(nameof(queryBuilder));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            Dictionary<TKey, T> result = new();
            foreach (T item in queryBuilder.Build())
            {
                TKey key = keySelector.Invoke(item);
                if (!result.TryAdd(key, item))
                {
                    Debug.LogError($"[Core.UI.Extensions] Key duplicated in ToDictionary: {key}");
                }
            }

            return result;
        }

        public static T[] ToArray<T>(this UQueryBuilder<T> queryBuilder) where T : VisualElement
        {
            if (queryBuilder == null) throw new ArgumentNullException(nameof(queryBuilder));
            return queryBuilder.ToList().ToArray();
        }

        public static T[] ToReverseArray<T>(this UQueryBuilder<T> queryBuilder) where T : VisualElement
        {
            if (queryBuilder == null) throw new ArgumentNullException(nameof(queryBuilder));
            List<T> list = queryBuilder.ToList();
            list.Reverse();
            return list.ToArray();
        }
    }
}
