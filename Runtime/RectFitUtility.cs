using UnityEngine;

namespace Core.UI.Extensions
{
    public static class RectFitUtility
    {
        public static Vector2 FitSizeWithin(Vector2 sourceSize, Vector2 maxSize)
        {
            var safeWidth = Mathf.Max(sourceSize.x, Mathf.Epsilon);
            var safeHeight = Mathf.Max(sourceSize.y, Mathf.Epsilon);
            var scale = Mathf.Min(maxSize.x / safeWidth, maxSize.y / safeHeight);
            if (float.IsNaN(scale) || float.IsInfinity(scale) || scale <= 0f)
            {
                scale = 1f;
            }

            return new Vector2(safeWidth * scale, safeHeight * scale);
        }

        public static Rect FitRectWithin(Rect containerRect, Vector2 contentSize)
        {
            var fittedSize = FitSizeWithin(contentSize, containerRect.size);
            return new Rect(
                containerRect.xMin + ((containerRect.width - fittedSize.x) * 0.5f),
                containerRect.yMin + ((containerRect.height - fittedSize.y) * 0.5f),
                fittedSize.x,
                fittedSize.y);
        }
    }
}
