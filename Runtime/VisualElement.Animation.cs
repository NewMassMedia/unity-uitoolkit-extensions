using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static class Easing
    {
        public static float Linear(float t) => t;
        public static float EaseInQuad(float t) => t * t;
        public static float EaseOutQuad(float t) => 1f - (1f - t) * (1f - t);
        public static float EaseInCubic(float t) => t * t * t;
        public static float EaseOutCubic(float t) => 1f - (1f - t) * (1f - t) * (1f - t);

        public static float EaseInOutCubic(float t) =>
            t < 0.5f ? 4f * t * t * t : 1f - (-2f * t + 2f) * (-2f * t + 2f) * (-2f * t + 2f) / 2f;

        public static float EaseOutBack(float t)
        {
            const float OVERSHOOT = 1.70158f;
            return 1f + (OVERSHOOT + 1f) * Mathf.Pow(t - 1f, 3f) + OVERSHOOT * Mathf.Pow(t - 1f, 2f);
        }

        public static float EaseOutElastic(float t)
        {
            if (t == 0f || t == 1f) return t;
            const float PERIOD = 0.3f;
            return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - PERIOD / 4f) * (2f * Mathf.PI) / PERIOD) + 1f;
        }
    }

    public static partial class VisualElementExtension
    {
        #region Lifecycle

        public static void AfterLayout<T>(this T e, Action callback) where T : VisualElement
        {
            e.RegisterCallbackOnce<GeometryChangedEvent>(_ => callback());
        }

        #endregion Lifecycle

        #region Tween

        public static IVisualElementScheduledItem Tween<T>(
            this T e, float durationMs, Func<float, float> easing,
            Action<float> onUpdate, Action onComplete = null) where T : VisualElement
        {
            float startTime = Time.unscaledTime * 1000f;
            IVisualElementScheduledItem item = null;
            item = e.schedule.Execute(() =>
            {
                float elapsed = Time.unscaledTime * 1000f - startTime;
                float t = Mathf.Clamp01(elapsed / durationMs);
                onUpdate(easing(t));
                if (t >= 1f)
                {
                    item?.Pause();
                    onComplete?.Invoke();
                }
            }).Every(16);
            return item;
        }

        #endregion Tween

        #region Class Toggle

        public static IVisualElementScheduledItem StartClassToggle<T>(this T e, string className, int intervalMs)
            where T : VisualElement
        {
            return e.schedule.Execute(() =>
            {
                e.EnableClass(className, !e.ClassListContains(className));
            }).StartingIn(0).Every(intervalMs);
        }

        #endregion Class Toggle

        #region Rotation

        public static IVisualElementScheduledItem StartRotation<T>(this T e, float degreesPerStep, int intervalMs)
            where T : VisualElement
        {
            float rotation = 0f;
            return e.schedule.Execute(() =>
            {
                rotation += degreesPerStep;
                e.SetRotate(rotation);
            }).StartingIn(0).Every(intervalMs);
        }

        #endregion Rotation
    }
}
