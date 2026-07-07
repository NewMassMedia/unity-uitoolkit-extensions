using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        #region Dashed Border

        private static readonly Dictionary<VisualElement, Action<MeshGenerationContext>> _dashedBorderHandlers = new();
        private static readonly Dictionary<VisualElement, EventCallback<DetachFromPanelEvent>> _dashedBorderCleanupHandlers = new();

        public static T SetDashedBorder<T>(this T e, bool enable) where T : VisualElement
        {
            if (enable)
            {
                if (!_dashedBorderHandlers.ContainsKey(e))
                {
                    void handler(MeshGenerationContext ctx) => RenderDashedBorder(e, ctx);
                    EventCallback<DetachFromPanelEvent> cleanup = _ => e.SetDashedBorder(false);

                    _dashedBorderHandlers[e] = handler;
                    _dashedBorderCleanupHandlers[e] = cleanup;

                    e.generateVisualContent += handler;
                    e.RegisterCallback(cleanup);
                }
            }
            else if (_dashedBorderHandlers.TryGetValue(e, out var handler))
            {
                e.generateVisualContent -= handler;
                _dashedBorderHandlers.Remove(e);

                if (_dashedBorderCleanupHandlers.TryGetValue(e, out var cleanup))
                {
                    e.UnregisterCallback(cleanup);
                    _dashedBorderCleanupHandlers.Remove(e);
                }
            }

            e.MarkDirtyRepaint();
            return e;
        }

        private static void RenderDashedBorder(VisualElement e, MeshGenerationContext ctx)
        {
            var w = e.layout.width;
            var h = e.layout.height;
            if (w <= 0 || h <= 0) return;

            const float DASH = 6f;
            const float GAP = 4f;
            const float INSET = 0.5f;

            var rect = new Rect(INSET, INSET, w - INSET * 2f, h - INSET * 2f);
            var radius = Mathf.Min(
                Mathf.Min(e.resolvedStyle.borderTopLeftRadius, e.resolvedStyle.borderTopRightRadius),
                Mathf.Min(e.resolvedStyle.borderBottomLeftRadius, e.resolvedStyle.borderBottomRightRadius)
            );
            radius = Mathf.Min(radius, Mathf.Min(rect.width, rect.height) * 0.5f);

            var p = ctx.painter2D;
            p.strokeColor = e.resolvedStyle.borderTopColor;
            p.lineWidth = 1f;

            var points = BuildRoundedRectPoints(rect, radius, arcSegs: 8);
            p.BeginPath();
            DrawDashedPolyline(p, points, DASH, GAP);
            p.Stroke();
        }

        private static List<Vector2> BuildRoundedRectPoints(Rect rect, float r, int arcSegs)
        {
            var pts = new List<Vector2>();

            if (r <= 0f)
            {
                pts.Add(new Vector2(rect.xMin, rect.yMin));
                pts.Add(new Vector2(rect.xMax, rect.yMin));
                pts.Add(new Vector2(rect.xMax, rect.yMax));
                pts.Add(new Vector2(rect.xMin, rect.yMax));
                pts.Add(new Vector2(rect.xMin, rect.yMin));
                return pts;
            }

            AddArc(pts, new Vector2(rect.xMin + r, rect.yMin + r), r, 180f, 270f, arcSegs);
            pts.Add(new Vector2(rect.xMax - r, rect.yMin));
            AddArc(pts, new Vector2(rect.xMax - r, rect.yMin + r), r, 270f, 360f, arcSegs, skipFirst: true);
            pts.Add(new Vector2(rect.xMax, rect.yMax - r));
            AddArc(pts, new Vector2(rect.xMax - r, rect.yMax - r), r, 0f, 90f, arcSegs, skipFirst: true);
            pts.Add(new Vector2(rect.xMin + r, rect.yMax));
            AddArc(pts, new Vector2(rect.xMin + r, rect.yMax - r), r, 90f, 180f, arcSegs, skipFirst: true);
            pts.Add(pts[0]);

            return pts;
        }

        private static void AddArc(List<Vector2> pts, Vector2 center, float radius,
            float startDeg, float endDeg, int segs, bool skipFirst = false)
        {
            int start = skipFirst ? 1 : 0;
            for (int i = start; i <= segs; i++)
            {
                float angle = Mathf.Lerp(startDeg, endDeg, (float)i / segs) * Mathf.Deg2Rad;
                pts.Add(center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius);
            }
        }

        private static void DrawDashedPolyline(Painter2D p, List<Vector2> pts, float dash, float gap)
        {
            if (pts.Count < 2) return;

            var drawing = true;
            var remaining = dash;

            for (int i = 0; i < pts.Count - 1; i++)
            {
                var a = pts[i];
                var b = pts[i + 1];
                var seg = b - a;
                var segLen = seg.magnitude;
                if (segLen <= 0f) continue;
                var dir = seg / segLen;

                var pos = 0f;
                while (pos < segLen)
                {
                    var step = Mathf.Min(remaining, segLen - pos);
                    if (drawing && step > 0f)
                    {
                        p.MoveTo(a + dir * pos);
                        p.LineTo(a + dir * (pos + step));
                    }
                    pos += step;
                    remaining -= step;
                    if (remaining <= 0.001f)
                    {
                        drawing = !drawing;
                        remaining = drawing ? dash : gap;
                    }
                }
            }
        }

        #endregion Dashed Border

        #region Gradient Background

        private static readonly Dictionary<VisualElement, Texture2D> _gradientTextures = new();
        private static readonly Dictionary<VisualElement, EventCallback<DetachFromPanelEvent>> _gradientCleanupHandlers = new();

        public static T SetGradientBackground<T>(this T e, Color from, Color to) where T : VisualElement
        {
            ClearGradientBackground(e);

            const int HEIGHT = 64;
            var tex = new Texture2D(1, HEIGHT, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
            };

            var pixels = new Color[HEIGHT];
            for (int y = 0; y < HEIGHT; y++)
            {
                float t = (float)y / (HEIGHT - 1);
                pixels[y] = Color.Lerp(from, to, t);
            }

            tex.SetPixels(pixels);
            tex.Apply();

            _gradientTextures[e] = tex;

            if (!_gradientCleanupHandlers.ContainsKey(e))
            {
                EventCallback<DetachFromPanelEvent> cleanup = _ => e.ClearGradientBackground();
                _gradientCleanupHandlers[e] = cleanup;
                e.RegisterCallback(cleanup);
            }

            e.style.backgroundImage = new StyleBackground(tex);
            e.style.backgroundSize = new BackgroundSize(Length.Percent(100), Length.Percent(100));
            e.style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center);
            e.style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center);

            return e;
        }

        public static T ClearGradientBackground<T>(this T e) where T : VisualElement
        {
            if (_gradientTextures.TryGetValue(e, out var tex))
            {
                e.style.backgroundImage = StyleKeyword.None;
                if (Application.isPlaying)
                    UnityEngine.Object.Destroy(tex);
                else
                    UnityEngine.Object.DestroyImmediate(tex);
                _gradientTextures.Remove(e);
            }

            if (_gradientCleanupHandlers.TryGetValue(e, out var cleanup))
            {
                e.UnregisterCallback(cleanup);
                _gradientCleanupHandlers.Remove(e);
            }

            return e;
        }

        #endregion Gradient Background
    }
}
