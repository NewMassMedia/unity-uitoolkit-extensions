using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Core.UI.Extensions.Tests
{
    // Transition completion, AfterLayout, the dashed border and gradient across re-attach, and PlaceAnchored inside an
    // offset parent (the 0.1.15 judgment pass).
    public sealed class VisualElementLifecycleTests
    {
        private const double TIMEOUT_SECONDS = 3d;
        private const int NEVER_ATTACHED_COUNT = 100;

        private static readonly StylePropertyName OPACITY = new("opacity");

        private sealed class HostWindow : EditorWindow { }

        private HostWindow _window;

        private VisualElement Root => _window.rootVisualElement;

        [SetUp]
        public void SetUp()
        {
            _window = ScriptableObject.CreateInstance<HostWindow>();
            _window.ShowUtility();
            _window.position = new Rect(0f, 0f, 400f, 300f);
        }

        [TearDown]
        public void TearDown()
        {
            if (_window != null) _window.Close();
            _window = null;
        }

        [UnityTest]
        public IEnumerator FadeOut_WithZeroDuration_CompletesOnceAndClearsTheInlineTransition()
        {
            var element = AddBox();
            int completed = 0;

            element.FadeOut(duration: 0f, onComplete: () => completed++);
            yield return WaitUntil(() => completed > 0);
            yield return WaitUntil(() => element.style.transitionProperty.keyword == StyleKeyword.Null);
            yield return Frames(5);

            Assert.AreEqual(1, completed);
            Assert.AreEqual(StyleKeyword.Null, element.style.transitionDuration.keyword);
            Assert.AreEqual(StyleKeyword.Null, element.style.transitionTimingFunction.keyword);
        }

        [UnityTest]
        public IEnumerator FadeOut_CompletesOnItsOwnTransitionEnd()
        {
            var element = AddBox();
            yield return Frames(3);
            int completed = 0;
            int atOwnEnd = -1;

            element.FadeOut(duration: 0.05f, onComplete: () => completed++);
            yield return WaitUntil(() => element.style.transitionProperty.keyword == StyleKeyword.Undefined);
            // Registered after the run's handler, so it runs after it.
            element.RegisterCallback<TransitionEndEvent>(evt => { if (evt.target == element) atOwnEnd = completed; });
            yield return WaitUntil(() => atOwnEnd >= 0);

            Assert.AreEqual(1, atOwnEnd, "onComplete waited for the fallback instead of the element's own TransitionEnd");
        }

        [UnityTest]
        public IEnumerator ScaleOut_WhenTheValuesAreAlreadyAtTheTarget_StillCompletesOnce()
        {
            var element = AddBox();
            element.SetScale(new Vector2(0.8f, 0.8f)).SetOpacity(0f);
            yield return Frames(3);
            int completed = 0;

            element.ScaleOut(to: 0.8f, duration: 0.05f, onComplete: () => completed++);
            yield return WaitUntil(() => completed > 0);
            yield return Frames(5);

            Assert.AreEqual(1, completed);
        }

        [UnityTest]
        public IEnumerator FadeOut_IgnoresADescendantsTransitionEvents_AndEndsOnItsOwnCancel()
        {
            var element = AddBox();
            var child = new VisualElement();
            child.SetWidth(10f).SetHeight(10f);
            element.Add(child);
            yield return Frames(3);
            int childEnds = 0;
            int childCancels = 0;
            element.RegisterCallback<TransitionEndEvent>(evt => { if (evt.target == child) childEnds++; });
            element.RegisterCallback<TransitionCancelEvent>(evt => { if (evt.target == child) childCancels++; });
            int completed = 0;
            element.FadeOut(duration: 30f, onComplete: () => completed++);
            yield return WaitUntil(() => element.style.transitionProperty.keyword == StyleKeyword.Undefined);

            child.SetTransitionProperty(new List<StylePropertyName> { OPACITY })
                .SetTransitionDuration(new List<TimeValue> { new(0.05f) });
            yield return Frames(3);
            child.SetOpacity(0.5f);
            yield return Seconds(0.5d);
            child.SetTransitionDuration(new List<TimeValue> { new(10f) });
            yield return Frames(3);
            child.SetOpacity(1f);
            yield return Seconds(0.3d);
            child.SetTransitionProperty(StyleKeyword.Null).SetTransitionDuration(StyleKeyword.Null);
            yield return WaitUntil(() => childCancels > 0);
            Assert.AreEqual(1, childEnds);
            Assert.AreEqual(0, completed, "a child's opacity end or cancel ended the parent's fade");

            element.SetTransitionProperty(StyleKeyword.Null).SetTransitionDuration(StyleKeyword.Null);
            yield return WaitUntil(() => completed > 0);
            Assert.AreEqual(1, completed);
        }

        [UnityTest]
        public IEnumerator ScaleOut_ReplacedByScaleIn_DropsItsOnComplete()
        {
            var element = AddBox();
            int completed = 0;

            element.ScaleOut(duration: 0.05f, onComplete: () => completed++);
            element.ScaleIn(duration: 0.05f);
            yield return Seconds(0.6d);

            Assert.AreEqual(0, completed, "a window closed and reopened in one frame would hide itself");
            Assert.AreEqual(StyleKeyword.Null, element.style.transitionProperty.keyword);
        }

        [UnityTest]
        public IEnumerator FadeOut_StartedWhileAFadeInRuns_IgnoresTheFadeInsCancel_AndCompletesOnItsOwnEnd()
        {
            var element = AddBox();
            yield return Frames(3);
            element.FadeIn(duration: 2f);
            yield return WaitUntil(() => element.style.transitionProperty.keyword == StyleKeyword.Undefined);
            yield return Seconds(0.3d);
            int completed = 0;
            int atCancel = -1;
            int atOwnEnd = -1;

            element.FadeOut(duration: 0.5f, onComplete: () => completed++);
            // On the parent, bubble-up: runs after the element's own handlers, which the FadeOut registers only later.
            Root.RegisterCallback<TransitionCancelEvent>(evt => { if (evt.target == element && atCancel < 0) atCancel = completed; });
            Root.RegisterCallback<TransitionEndEvent>(evt => { if (evt.target == element) atOwnEnd = completed; });
            yield return WaitUntil(() => atOwnEnd >= 0);
            yield return Frames(5);

            Assert.AreEqual(0, atCancel, "the cancel of the replaced FadeIn ended the FadeOut, or no cancel came");
            Assert.AreEqual(1, atOwnEnd, "onComplete did not run on the FadeOut's own TransitionEnd");
            Assert.AreEqual(1, completed);
        }

        [UnityTest]
        public IEnumerator SlideOutTo_CompletesWhenTheElementIsDetached()
        {
            var element = AddBox();
            int completed = 0;
            element.SlideOutTo(SlideDirection.Left, duration: 30f, onComplete: () => completed++);
            yield return WaitUntil(() => element.style.transitionProperty.keyword == StyleKeyword.Undefined);

            element.RemoveFromHierarchy();

            Assert.AreEqual(1, completed);
            Assert.AreEqual(StyleKeyword.Null, element.style.transitionProperty.keyword);
        }

        [UnityTest]
        public IEnumerator AfterLayout_OnAnElementWhoseGeometryDoesNotChange_RunsOnce()
        {
            var element = AddBox();
            yield return WaitUntil(() => element.layout.width > 0f);
            int calls = 0;

            element.AfterLayout(() => calls++);
            yield return WaitUntil(() => calls > 0);
            element.style.width = 120f;
            yield return WaitUntil(() => Mathf.Approximately(element.layout.width, 120f));
            yield return Frames(3);

            Assert.AreEqual(1, calls);
        }

        [UnityTest]
        public IEnumerator AfterLayout_OnANewElement_RunsWithItsLaidOutSize()
        {
            var element = new VisualElement();
            element.style.width = 64f;
            element.style.height = 32f;
            float measured = -1f;
            element.AfterLayout(() => measured = element.resolvedStyle.width);

            Root.Add(element);
            yield return WaitUntil(() => measured >= 0f);

            Assert.AreEqual(64f, measured, 0.01f);
        }

        [UnityTest]
        public IEnumerator SetDashedBorder_StaysOnAcrossAReparent()
        {
            var element = AddBox();
            var other = AddBox();
            element.SetDashedBorder(true);

            other.Add(element);
            yield return null;

            Assert.IsNotNull(element.generateVisualContent, "the border was dropped on detach");
            element.SetDashedBorder(false);
            Assert.IsNull(element.generateVisualContent);
        }

        [UnityTest]
        public IEnumerator SetGradientBackground_IsRebuiltAfterAReparent_AndOnlyWhileAttached()
        {
            var detached = new VisualElement();
            detached.SetGradientBackground(Color.black, Color.white);
            Assert.AreNotEqual(StyleKeyword.Undefined, detached.style.backgroundImage.keyword, "a texture was built while detached");

            var element = AddBox();
            var other = AddBox();
            element.SetGradientBackground(Color.black, Color.white);
            Texture2D first = element.style.backgroundImage.value.texture;
            Assert.IsTrue(first != null);

            element.RemoveFromHierarchy();
            Assert.IsTrue(first == null, "the texture outlived the detach");
            other.Add(element);
            yield return null;

            Texture2D rebuilt = element.style.backgroundImage.value.texture;
            Assert.IsTrue(rebuilt != null, "the gradient was dropped on detach");
            element.ClearGradientBackground();
            Assert.IsTrue(rebuilt == null);
            Assert.AreEqual(StyleKeyword.Null, element.style.backgroundImage.keyword);
            detached.ClearGradientBackground();
        }

        [Test]
        public void SetDashedBorder_OnANeverAttachedElement_DoesNotKeepItAlive()
        {
            List<WeakReference> elements = ConfigureNeverAttached(element => element.SetDashedBorder(true));

            Assert.Less(CountAlive(elements), NEVER_ATTACHED_COUNT / 2, "a never-attached element stayed alive through the static state");
        }

        [Test]
        public void SetGradientBackground_OnANeverAttachedElement_DoesNotKeepItAlive()
        {
            List<WeakReference> elements = ConfigureNeverAttached(element => element.SetGradientBackground(Color.black, Color.white));

            Assert.Less(CountAlive(elements), NEVER_ATTACHED_COUNT / 2, "a never-attached element stayed alive through the static state");
        }

        [UnityTest]
        public IEnumerator PlaceAnchored_InsideAnOffsetParentWithABorder_LandsAtThePanelPoint()
        {
            var parent = AddBox();
            parent.style.position = Position.Absolute;
            parent.style.left = 40f;
            parent.style.top = 30f;
            parent.style.width = 300f;
            parent.style.height = 200f;
            parent.style.borderLeftWidth = 5f;
            parent.style.borderTopWidth = 7f;
            parent.style.paddingLeft = 11f;
            parent.style.paddingTop = 13f;
            var popup = new VisualElement();
            popup.style.position = Position.Absolute;
            popup.style.width = 40f;
            popup.style.height = 20f;
            parent.Add(popup);
            yield return WaitUntil(() => popup.layout.width > 0f);

            popup.PlaceAnchored(new Vector2(150f, 100f), Side.Bottom, sideOffset: 4f, edgeMargin: 0f);
            yield return Frames(5);

            Assert.AreEqual(130f, popup.worldBound.x, 0.01f);
            Assert.AreEqual(104f, popup.worldBound.y, 0.01f);
        }

        private VisualElement AddBox()
        {
            var element = new VisualElement();
            element.style.width = 80f;
            element.style.height = 40f;
            Root.Add(element);
            return element;
        }

        // Not inlined, so no local in the test method keeps an element reachable.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static List<WeakReference> ConfigureNeverAttached(Action<VisualElement> configure)
        {
            var references = new List<WeakReference>(NEVER_ATTACHED_COUNT);
            for (int i = 0; i < NEVER_ATTACHED_COUNT; i++)
            {
                var element = new VisualElement();
                configure(element);
                references.Add(new WeakReference(element));
            }
            return references;
        }

        private static int CountAlive(List<WeakReference> references)
        {
            for (int i = 0; i < 4; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            int alive = 0;
            foreach (WeakReference reference in references)
            {
                if (reference.IsAlive) alive++;
            }
            return alive;
        }

        private IEnumerator Frames(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _window.Repaint();
                yield return null;
            }
        }

        private IEnumerator Seconds(double seconds)
        {
            double end = EditorApplication.timeSinceStartup + seconds;
            while (EditorApplication.timeSinceStartup < end)
            {
                _window.Repaint();
                yield return null;
            }
        }

        private IEnumerator WaitUntil(Func<bool> condition)
        {
            double deadline = EditorApplication.timeSinceStartup + TIMEOUT_SECONDS;
            while (!condition())
            {
                if (EditorApplication.timeSinceStartup > deadline) Assert.Fail("condition not met in time");
                _window.Repaint();
                yield return null;
            }
        }
    }
}
