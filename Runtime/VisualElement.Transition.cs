using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UI.Extensions
{
    public static partial class VisualElementExtension
    {
        private static class TransitionCache
        {
            public static readonly StylePropertyName OpacityProperty = new("opacity");
            public static readonly StylePropertyName TranslateProperty = new("translate");
            public static readonly StylePropertyName ScaleProperty = new("scale");

            public static readonly List<StylePropertyName> OpacityProperties = new() { OpacityProperty };
            public static readonly List<StylePropertyName> TranslateAndOpacityProperties = new()
            {
                TranslateProperty,
                OpacityProperty
            };
            public static readonly List<StylePropertyName> ScaleAndOpacityProperties = new()
            {
                ScaleProperty,
                OpacityProperty
            };

            public static readonly List<TimeValue> D150 = new() { new TimeValue(0.15f) };
            public static readonly List<TimeValue> D200 = new() { new TimeValue(0.2f) };
            public static readonly List<TimeValue> D300 = new() { new TimeValue(0.3f) };

            public static readonly List<TimeValue> D150x2 = new() { new TimeValue(0.15f), new TimeValue(0.15f) };
            public static readonly List<TimeValue> D200x2 = new() { new TimeValue(0.2f), new TimeValue(0.2f) };
            public static readonly List<TimeValue> D300x2 = new() { new TimeValue(0.3f), new TimeValue(0.3f) };

            public static readonly List<EasingFunction> EaseIn = new() { new EasingFunction(EasingMode.EaseIn) };
            public static readonly List<EasingFunction> EaseOut = new() { new EasingFunction(EasingMode.EaseOut) };
            public static readonly List<EasingFunction> EaseInx2 = new()
            {
                new EasingFunction(EasingMode.EaseIn),
                new EasingFunction(EasingMode.EaseIn)
            };
            public static readonly List<EasingFunction> EaseOutx2 = new()
            {
                new EasingFunction(EasingMode.EaseOut),
                new EasingFunction(EasingMode.EaseOut)
            };
            public static readonly List<EasingFunction> EaseOutBackAndOut = new()
            {
                new EasingFunction(EasingMode.EaseOutBack),
                new EasingFunction(EasingMode.EaseOut)
            };
            public static readonly List<EasingFunction> EaseInBackAndIn = new()
            {
                new EasingFunction(EasingMode.EaseInBack),
                new EasingFunction(EasingMode.EaseIn)
            };
        }

        public static VisualElement FadeIn(this VisualElement element, float duration = 0.3f,
            float delay = 0f, EasingMode easing = EasingMode.EaseOut)
        {
            element.SetOpacity(0);

            ScheduleTransition(element, delay, () =>
            {
                ApplyTransition(
                    element,
                    TransitionCache.OpacityProperties,
                    GetDurations(duration),
                    GetUniformEasings(easing, EasingMode.EaseOut, TransitionCache.EaseOut, 1));

                element.SetOpacity(1);
            });

            return element;
        }

        public static VisualElement FadeOut(this VisualElement element, float duration = 0.3f,
            float delay = 0f, EasingMode easing = EasingMode.EaseIn, Action onComplete = null)
        {
            ScheduleTransition(element, delay, () =>
            {
                ApplyTransition(
                    element,
                    TransitionCache.OpacityProperties,
                    GetDurations(duration),
                    GetUniformEasings(easing, EasingMode.EaseIn, TransitionCache.EaseIn, 1));

                if (onComplete != null)
                {
                    RegisterTransitionEnd(element, TransitionCache.OpacityProperty, onComplete);
                }

                element.SetOpacity(0);
            });

            return element;
        }

        public static VisualElement SlideInFrom(this VisualElement element, SlideDirection direction,
            float distance = 300f, float duration = 0.3f, float delay = 0f,
            EasingMode easing = EasingMode.EaseOut)
        {
            var offset = GetOffsetForDirection(direction, distance);
            element.SetTranslate(offset).SetOpacity(0);

            ScheduleTransition(element, delay, () =>
            {
                ApplyTransition(
                    element,
                    TransitionCache.TranslateAndOpacityProperties,
                    GetDurations(duration, 2),
                    GetUniformEasings(easing, EasingMode.EaseOut, TransitionCache.EaseOutx2, 2));

                element.SetTranslate(Vector2.zero).SetOpacity(1);
            });

            return element;
        }

        public static VisualElement SlideOutTo(this VisualElement element, SlideDirection direction,
            float distance = 300f, float duration = 0.3f, float delay = 0f,
            EasingMode easing = EasingMode.EaseIn, Action onComplete = null)
        {
            var offset = GetOffsetForDirection(direction, distance);

            ScheduleTransition(element, delay, () =>
            {
                ApplyTransition(
                    element,
                    TransitionCache.TranslateAndOpacityProperties,
                    GetDurations(duration, 2),
                    GetUniformEasings(easing, EasingMode.EaseIn, TransitionCache.EaseInx2, 2));

                if (onComplete != null)
                {
                    RegisterTransitionEnd(element, TransitionCache.TranslateProperty, onComplete);
                }

                element.SetTranslate(offset).SetOpacity(0);
            });

            return element;
        }

        public static VisualElement ScaleIn(this VisualElement element, float from = 0.8f,
            float duration = 0.3f, float delay = 0f, EasingMode easing = EasingMode.EaseOutBack)
        {
            element.SetScale(new Vector2(from, from)).SetOpacity(0);

            ScheduleTransition(element, delay, () =>
            {
                ApplyTransition(
                    element,
                    TransitionCache.ScaleAndOpacityProperties,
                    GetDurations(duration, 2),
                    GetScaleEasings(
                        easing,
                        EasingMode.EaseOutBack,
                        EasingMode.EaseOut,
                        TransitionCache.EaseOutBackAndOut));

                element.SetScale(Vector2.one).SetOpacity(1);
            });

            return element;
        }

        public static VisualElement ScaleOut(this VisualElement element, float to = 0.8f,
            float duration = 0.3f, float delay = 0f, EasingMode easing = EasingMode.EaseInBack,
            Action onComplete = null)
        {
            ScheduleTransition(element, delay, () =>
            {
                ApplyTransition(
                    element,
                    TransitionCache.ScaleAndOpacityProperties,
                    GetDurations(duration, 2),
                    GetScaleEasings(
                        easing,
                        EasingMode.EaseInBack,
                        EasingMode.EaseIn,
                        TransitionCache.EaseInBackAndIn));

                if (onComplete != null)
                {
                    RegisterTransitionEnd(element, TransitionCache.ScaleProperty, onComplete);
                }

                element.SetScale(new Vector2(to, to)).SetOpacity(0);
            });

            return element;
        }

        public static VisualElement Enter(this VisualElement element,
            SlideDirection direction = SlideDirection.Bottom, float duration = 0.3f)
            => element.SlideInFrom(direction, 50f, duration);

        public static VisualElement Exit(this VisualElement element,
            SlideDirection direction = SlideDirection.Bottom, float duration = 0.3f)
            => element.SlideOutTo(direction, 50f, duration);

        private static Vector2 GetOffsetForDirection(SlideDirection direction, float distance) =>
            direction switch
            {
                SlideDirection.Left => new Vector2(-distance, 0),
                SlideDirection.Right => new Vector2(distance, 0),
                SlideDirection.Top => new Vector2(0, -distance),
                SlideDirection.Bottom => new Vector2(0, distance),
                _ => Vector2.zero
            };

        private static void ScheduleTransition(VisualElement element, float delay, Action action)
        {
            element.schedule.Execute(action).StartingIn(ToDelayMilliseconds(delay));
        }

        private static long ToDelayMilliseconds(float delay) => (long)(delay * 1000);

        private static void ApplyTransition(
            VisualElement element,
            List<StylePropertyName> properties,
            List<TimeValue> durations,
            List<EasingFunction> easings)
        {
            element.SetTransitionProperty(new StyleList<StylePropertyName>(properties))
                .SetTransitionDuration(new StyleList<TimeValue>(durations))
                .SetTransitionTimingFunction(new StyleList<EasingFunction>(easings));
        }

        private static List<TimeValue> GetDurations(float duration, int propertyCount = 1)
        {
            if (propertyCount == 1)
            {
                if (duration == 0.3f) return TransitionCache.D300;
                if (duration == 0.2f) return TransitionCache.D200;
                if (duration == 0.15f) return TransitionCache.D150;
                return new List<TimeValue>(1) { new TimeValue(duration) };
            }

            if (duration == 0.3f) return TransitionCache.D300x2;
            if (duration == 0.2f) return TransitionCache.D200x2;
            if (duration == 0.15f) return TransitionCache.D150x2;
            return new List<TimeValue>(2) { new TimeValue(duration), new TimeValue(duration) };
        }

        private static List<EasingFunction> GetUniformEasings(
            EasingMode easing,
            EasingMode defaultMode,
            List<EasingFunction> cached,
            int propertyCount)
        {
            if (easing == defaultMode) return cached;

            if (propertyCount == 1)
            {
                return new List<EasingFunction>(1) { new EasingFunction(easing) };
            }

            return new List<EasingFunction>(2) { new EasingFunction(easing), new EasingFunction(easing) };
        }

        private static List<EasingFunction> GetScaleEasings(
            EasingMode easing,
            EasingMode defaultScaleMode,
            EasingMode opacityMode,
            List<EasingFunction> cached)
        {
            if (easing == defaultScaleMode) return cached;

            return new List<EasingFunction>(2)
            {
                new EasingFunction(easing),
                new EasingFunction(opacityMode)
            };
        }

        private static void RegisterTransitionEnd(VisualElement element, StylePropertyName property, Action callback)
        {
            void Cleanup()
            {
                element.RemoveCallback(OnEnd)
                    .RemoveCallback(OnCancel);
            }

            void OnEnd(TransitionEndEvent evt)
            {
                if (!evt.stylePropertyNames.Contains(property)) return;

                Cleanup();
                callback();
            }

            void OnCancel(TransitionCancelEvent _)
            {
                Cleanup();
                callback();
            }

            element.Callback(OnEnd)
                .Callback(OnCancel);
        }
    }

    public enum SlideDirection
    {
        Left,
        Right,
        Top,
        Bottom
    }
}
