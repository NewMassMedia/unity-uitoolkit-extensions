using UnityEngine;

namespace Core.UI.Extensions
{
    public enum ResizeHandle
    {
        None,
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft,
    }

    /// <summary>
    /// Shared rectangle resize math for editor handles.
    /// </summary>
    public static class RectResizeUtility
    {
        public static Rect ResizeRect(Rect sourceRect, ResizeHandle handle, Vector2 delta, float minSize)
        {
            var left = sourceRect.xMin;
            var top = sourceRect.yMin;
            var right = sourceRect.xMax;
            var bottom = sourceRect.yMax;

            switch (handle)
            {
                case ResizeHandle.TopLeft:
                    left += delta.x;
                    top += delta.y;
                    break;
                case ResizeHandle.TopRight:
                    right += delta.x;
                    top += delta.y;
                    break;
                case ResizeHandle.BottomRight:
                    right += delta.x;
                    bottom += delta.y;
                    break;
                case ResizeHandle.BottomLeft:
                    left += delta.x;
                    bottom += delta.y;
                    break;
            }

            if (right - left < minSize)
            {
                if (handle == ResizeHandle.TopLeft || handle == ResizeHandle.BottomLeft)
                {
                    left = right - minSize;
                }
                else
                {
                    right = left + minSize;
                }
            }

            if (bottom - top < minSize)
            {
                if (handle == ResizeHandle.TopLeft || handle == ResizeHandle.TopRight)
                {
                    top = bottom - minSize;
                }
                else
                {
                    bottom = top + minSize;
                }
            }

            return Rect.MinMaxRect(left, top, right, bottom);
        }
    }
}
