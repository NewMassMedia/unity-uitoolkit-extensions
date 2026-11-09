using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        private const long TRANSITION_FALLBACK_MARGIN_MS = 100;

        private static readonly ConditionalWeakTable<VisualElement, TransitionRun> s_transitionRuns = new();

        public static VisualElement FadeIn(this VisualElement element, float duration = 0.3f,
            float delay = 0f, EasingMode easing = EasingMode.EaseOut)
        {
            element.SetOpacity(0);

            ScheduleTransition(element, delay, () =>
            {
                TransitionRun run = TrackTransition(element, TransitionCache.OpacityProperty, duration, null);
                ApplyTransition(
                    element,
                    TransitionCache.OpacityProperties,
                    GetDurations(duration),
                    GetUniformEasings(easing, EasingMode.EaseOut, TransitionCache.EaseOut, 1));

                element.SetOpacity(1);
                run.Begin();
            });

            return element;
        }

        public static VisualElement FadeOut(this VisualElement element, float duration = 0.3f,
            float delay = 0f, EasingMode easing = EasingMode.EaseIn, Action onComplete = null)
        {
            ScheduleTransition(element, delay, () =>
            {
                TransitionRun run = TrackTransition(element, TransitionCache.OpacityProperty, duration, onComplete);
                ApplyTransition(
                    element,
                    TransitionCache.OpacityProperties,
                    GetDurations(duration),
                    GetUniformEasings(easing, EasingMode.EaseIn, TransitionCache.EaseIn, 1));

                element.SetOpacity(0);
                run.Begin();
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
                TransitionRun run = TrackTransition(element, TransitionCache.TranslateProperty, duration, null);
                ApplyTransition(
                    element,
                    TransitionCache.TranslateAndOpacityProperties,
                    GetDurations(duration, 2),
                    GetUniformEasings(easing, EasingMode.EaseOut, TransitionCache.EaseOutx2, 2));

                element.SetTranslate(Vector2.zero).SetOpacity(1);
                run.Begin();
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
                TransitionRun run = TrackTransition(element, TransitionCache.TranslateProperty, duration, onComplete);
                ApplyTransition(
                    element,
                    TransitionCache.TranslateAndOpacityProperties,
                    GetDurations(duration, 2),
                    GetUniformEasings(easing, EasingMode.EaseIn, TransitionCache.EaseInx2, 2));

                element.SetTranslate(offset).SetOpacity(0);
                run.Begin();
            });

            return element;
        }

        public static VisualElement ScaleIn(this VisualElement element, float from = 0.8f,
            float duration = 0.3f, float delay = 0f, EasingMode easing = EasingMode.EaseOutBack)
        {
            element.SetScale(new Vector2(from, from)).SetOpacity(0);

            ScheduleTransition(element, delay, () =>
            {
                TransitionRun run = TrackTransition(element, TransitionCache.ScaleProperty, duration, null);
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
                run.Begin();
            });

            return element;
        }

        public static VisualElement ScaleOut(this VisualElement element, float to = 0.8f,
            float duration = 0.3f, float delay = 0f, EasingMode easing = EasingMode.EaseInBack,
            Action onComplete = null)
        {
            ScheduleTransition(element, delay, () =>
            {
                TransitionRun run = TrackTransition(element, TransitionCache.ScaleProperty, duration, onComplete);
                ApplyTransition(
                    element,
                    TransitionCache.ScaleAndOpacityProperties,
                    GetDurations(duration, 2),
                    GetScaleEasings(
                        easing,
                        EasingMode.EaseInBack,
                        EasingMode.EaseIn,
                        TransitionCache.EaseInBackAndIn));

                element.SetScale(new Vector2(to, to)).SetOpacity(0);
                run.Begin();
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

        // One run per element: the latest Fade/Slide/Scale owns the inline transition it wrote. The run ends once, on
        // the element's own end or cancel of `property` after its own TransitionRunEvent, on detach, or after the
        // duration plus a margin when no transition event comes (duration 0, the value already at its target). Ending clears the inline transition so
        // the element's USS transitions apply again, then calls onComplete. A newer run on the element replaces this
        // one: its onComplete is dropped and its inline transition is left to the newer run.
        private static TransitionRun TrackTransition(VisualElement element, StylePropertyName property, float duration,
            Action onComplete)
        {
            if (s_transitionRuns.TryGetValue(element, out TransitionRun previous))
            {
                s_transitionRuns.Remove(element);
                previous.Stop();
            }

            var run = new TransitionRun(element, property, duration, onComplete);
            s_transitionRuns.Add(element, run);
            run.Listen();
            return run;
        }

        private sealed class TransitionRun
        {
            private readonly VisualElement _element;
            private readonly StylePropertyName _property;
            private readonly float _duration;
            private readonly long _fallbackMs;
            private Action _onComplete;
            private IVisualElementScheduledItem _fallback;
            private bool _isRunning;

            internal TransitionRun(VisualElement element, StylePropertyName property, float duration, Action onComplete)
            {
                _element = element;
                _property = property;
                _duration = duration;
                _fallbackMs = ToDelayMilliseconds(Mathf.Max(0f, duration)) + TRANSITION_FALLBACK_MARGIN_MS;
                _onComplete = onComplete;
            }

            // Before the values are written: the TransitionRunEvent of a transition reaches only the handlers that
            // were registered when the value changed.
            internal void Listen()
            {
                _element.RegisterCallback<TransitionRunEvent>(OnRun, TrickleDown.NoTrickleDown);
                _element.Callback(OnEnd, TrickleDown.NoTrickleDown)
                    .Callback(OnCancel, TrickleDown.NoTrickleDown)
                    .Callback(OnDetach, TrickleDown.NoTrickleDown);
            }

            // After the values are written.
            internal void Begin()
            {
                _fallback = _element.schedule.Execute(End).StartingIn(_fallbackMs);

                // No transition runs at duration 0. The inline transition is still cleared only at the fallback, after
                // the margin, so a USS transition does not animate the jump that was just written.
                if (_duration <= 0f) InvokeOnComplete();
            }

            internal void Stop()
            {
                _element.UnregisterCallback<TransitionRunEvent>(OnRun, TrickleDown.NoTrickleDown);
                _element.RemoveCallback(OnEnd, TrickleDown.NoTrickleDown)
                    .RemoveCallback(OnCancel, TrickleDown.NoTrickleDown)
                    .RemoveCallback(OnDetach, TrickleDown.NoTrickleDown);
                _fallback?.Pause();
                _onComplete = null;
            }

            // WHY: writing a new value while a transition runs (FadeOut during a FadeIn) cancels the running one, and
            // that cancel arrives after this run started listening. Only an end or cancel that follows this run's own
            // TransitionRunEvent belongs to it; without one the fallback ends the run. The fallback restarts from the
            // run event, so a transition that starts a slow frame late is not cut short.
            private void OnRun(TransitionRunEvent evt)
            {
                if (_isRunning || !IsOwnProperty(evt.target, evt.stylePropertyNames)) return;

                _isRunning = true;
                _fallback?.ExecuteLater(_fallbackMs);
            }

            private void OnEnd(TransitionEndEvent evt)
            {
                if (_isRunning && IsOwnProperty(evt.target, evt.stylePropertyNames)) End();
            }

            private void OnCancel(TransitionCancelEvent evt)
            {
                if (_isRunning && IsOwnProperty(evt.target, evt.stylePropertyNames)) End();
            }

            private void OnDetach(DetachFromPanelEvent _) => End();

            // A descendant's transition events (a button's hover fade) bubble up to these handlers too.
            private bool IsOwnProperty(IEventHandler target, StylePropertyNameCollection names)
                => target == _element && names.Contains(_property);

            private void End()
            {
                if (!s_transitionRuns.TryGetValue(_element, out TransitionRun current) || current != this) return;

                s_transitionRuns.Remove(_element);
                Action onComplete = _onComplete;
                Stop();
                _element.SetTransitionProperty(StyleKeyword.Null)
                    .SetTransitionDuration(StyleKeyword.Null)
                    .SetTransitionTimingFunction(StyleKeyword.Null);
                onComplete?.Invoke();
            }

            private void InvokeOnComplete()
            {
                Action onComplete = _onComplete;
                _onComplete = null;
                onComplete?.Invoke();
            }
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
