using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        #region Dashed Border

        // Weak keys: an element configured and never attached, or dropped, does not stay alive through this table.
        // The handler stays across detach and re-attach (generateVisualContent only runs while attached), so a
        // reparented element keeps its border.
        private static readonly ConditionalWeakTable<VisualElement, Action<MeshGenerationContext>> s_dashedBorderHandlers = new();

        public static T SetDashedBorder<T>(this T e, bool enable) where T : VisualElement
        {
            bool isOn = s_dashedBorderHandlers.TryGetValue(e, out Action<MeshGenerationContext> handler);
            if (enable && !isOn)
            {
                handler = ctx => RenderDashedBorder(e, ctx);
                s_dashedBorderHandlers.Add(e, handler);
                e.generateVisualContent += handler;
            }
            else if (!enable && isOn)
            {
                e.generateVisualContent -= handler;
                s_dashedBorderHandlers.Remove(e);
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

        private const int GRADIENT_HEIGHT = 64;

        // The gradient stays on across detach and re-attach until ClearGradientBackground. Its texture exists only
        // while the element is attached: built on attach, destroyed on detach, so a dropped element leaks none.
        private static readonly ConditionalWeakTable<VisualElement, GradientState> s_gradients = new();

        private sealed class GradientState
        {
            internal Color From;
            internal Color To;
            internal Texture2D Texture;
        }

        public static T SetGradientBackground<T>(this T e, Color from, Color to) where T : VisualElement
        {
            if (!s_gradients.TryGetValue(e, out GradientState state))
            {
                state = new GradientState();
                s_gradients.Add(e, state);
                e.Callback(OnGradientAttach, TrickleDown.NoTrickleDown)
                    .Callback(OnGradientDetach, TrickleDown.NoTrickleDown);
            }

            state.From = from;
            state.To = to;

            e.style.backgroundSize = new BackgroundSize(Length.Percent(100), Length.Percent(100));
            e.style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center);
            e.style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center);
            if (e.panel != null) ApplyGradientTexture(e, state);

            return e;
        }

        public static T ClearGradientBackground<T>(this T e) where T : VisualElement
        {
            if (!s_gradients.TryGetValue(e, out GradientState state)) return e;

            s_gradients.Remove(e);
            e.RemoveCallback(OnGradientAttach, TrickleDown.NoTrickleDown)
                .RemoveCallback(OnGradientDetach, TrickleDown.NoTrickleDown);
            DestroyGradientTexture(state);

            e.style.backgroundImage = StyleKeyword.Null;
            e.style.backgroundSize = StyleKeyword.Null;
            e.style.backgroundPositionX = StyleKeyword.Null;
            e.style.backgroundPositionY = StyleKeyword.Null;
            return e;
        }

        private static void OnGradientAttach(AttachToPanelEvent evt)
        {
            var e = (VisualElement)evt.currentTarget;
            if (s_gradients.TryGetValue(e, out GradientState state)) ApplyGradientTexture(e, state);
        }

        private static void OnGradientDetach(DetachFromPanelEvent evt)
        {
            var e = (VisualElement)evt.currentTarget;
            if (s_gradients.TryGetValue(e, out GradientState state)) DestroyGradientTexture(state);
        }

        private static void ApplyGradientTexture(VisualElement e, GradientState state)
        {
            if (state.Texture == null)
            {
                state.Texture = new Texture2D(1, GRADIENT_HEIGHT, TextureFormat.RGBA32, false)
                {
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear,
                };
            }

            var pixels = new Color[GRADIENT_HEIGHT];
            for (int y = 0; y < GRADIENT_HEIGHT; y++)
            {
                float t = (float)y / (GRADIENT_HEIGHT - 1);
                pixels[y] = Color.Lerp(state.From, state.To, t);
            }

            state.Texture.SetPixels(pixels);
            state.Texture.Apply();
            e.style.backgroundImage = new StyleBackground(state.Texture);
        }

        private static void DestroyGradientTexture(GradientState state)
        {
            if (state.Texture == null) return;

            if (Application.isPlaying)
                UnityEngine.Object.Destroy(state.Texture);
            else
                UnityEngine.Object.DestroyImmediate(state.Texture);
            state.Texture = null;
        }

        #endregion Gradient Background
    }
}
