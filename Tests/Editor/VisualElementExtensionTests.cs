using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Core.UI.Extensions.Tests
{
    // Behaviour of the hierarchy, bounds, layout and rendering helpers that the 0.1.15 mechanical pass touched.
    public sealed class VisualElementExtensionTests
    {
        const string SheetPath = "Packages/com.maemi.unity-uitoolkit-extensions/Tests/Editor/ToggleDisplayTests.uss";

        [Test]
        public void RemoveAllChildren_EmptiesTheElement()
        {
            var parent = new VisualElement();
            for (int i = 0; i < 5; i++) parent.Add(new VisualElement());

            parent.RemoveAllChildren();

            Assert.AreEqual(0, parent.childCount);
        }

        [Test]
        public void Destroy_RemovesAHierarchyChildWhoseParentHasAContentContainer()
        {
            var scroll = new ScrollView();
            var child = new VisualElement();
            scroll.hierarchy.Add(child);

            Assert.DoesNotThrow(() => child.Destroy());

            Assert.IsNull(child.parent);
        }

        [Test]
        public void IsInsideInclusive_AcceptsAnElementFlushWithTheRightAndBottomEdge()
        {
            var root = new Rect(0, 0, 100, 50);

            Assert.IsTrue(VisualElementExtension.IsInsideInclusive(new Rect(0, 0, 100, 50), root));
            Assert.IsTrue(VisualElementExtension.IsInsideInclusive(new Rect(60, 20, 40, 30), root));
            Assert.IsFalse(VisualElementExtension.IsInsideInclusive(new Rect(60, 20, 41, 30), root));
        }

        [Test]
        public void ToggleDisplay_FlipsAnInlineDisplay()
        {
            var shown = new VisualElement();
            shown.style.display = DisplayStyle.Flex;
            var hidden = new VisualElement();
            hidden.style.display = DisplayStyle.None;

            shown.ToggleDisplay();
            hidden.ToggleDisplay();

            Assert.AreEqual(DisplayStyle.None, shown.style.display.value);
            Assert.AreEqual(DisplayStyle.Flex, hidden.style.display.value);
        }

        [UnityTest]
        public IEnumerator ToggleDisplay_OnAnElementHiddenByUss_ShowsIt()
        {
            var sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(SheetPath);
            Assert.IsNotNull(sheet);
            var window = EditorWindow.CreateInstance<EditorWindow>();
            try
            {
                window.Show();
                window.rootVisualElement.styleSheets.Add(sheet);
                var element = new VisualElement();
                element.AddToClassList("hidden");
                window.rootVisualElement.Add(element);
                yield return null;
                Assert.AreEqual(DisplayStyle.None, element.resolvedStyle.display);

                element.ToggleDisplay();

                Assert.AreEqual(DisplayStyle.Flex, element.style.display.value);
            }
            finally
            {
                window.Close();
            }
        }

        [UnityTest]
        public IEnumerator IsInScreen_AcceptsAnElementFlushWithTheRightAndBottomEdge()
        {
            var window = EditorWindow.CreateInstance<EditorWindow>();
            try
            {
                window.Show();
                var element = new VisualElement();
                element.style.position = Position.Absolute;
                element.style.right = 0;
                element.style.bottom = 0;
                element.style.width = 20;
                element.style.height = 20;
                window.rootVisualElement.Add(element);
                yield return null;

                Assert.IsTrue(element.IsInScreen());
                Assert.IsFalse(element.IsOutOfScreen());
                Assert.IsFalse(new VisualElement().IsInScreen());
            }
            finally
            {
                window.Close();
            }
        }

        [Test]
        public void RemoveAllChildren_ClearsAScrollViewContentButKeepsItsScrollers()
        {
            var scroll = new ScrollView();
            int chrome = scroll.hierarchy.childCount;
            for (int i = 0; i < 3; i++) scroll.Add(new VisualElement());

            scroll.RemoveAllChildren();

            Assert.AreEqual(0, scroll.contentContainer.childCount);
            Assert.AreEqual(chrome, scroll.hierarchy.childCount);
        }

        [Test]
        public void SetPickingModeRecursive_Bool_MatchesTheEnumOverload()
        {
            var a = new VisualElement();
            a.Add(new VisualElement());
            var b = new VisualElement();
            b.Add(new VisualElement());

            a.SetPickingModeRecursive(false);
            b.SetPickingModeRecursive(PickingMode.Ignore);

            Assert.AreEqual(b.pickingMode, a.pickingMode);
            Assert.AreEqual(PickingMode.Ignore, a[0].pickingMode);
            a.SetPickingModeRecursive(true);
            Assert.AreEqual(PickingMode.Position, a[0].pickingMode);
        }

        [Test]
        public void ClearGradientBackground_ReturnsTheInlineBackgroundToTheStylesheet()
        {
            var element = new VisualElement();
            element.SetGradientBackground(Color.black, Color.white);
            Assert.AreEqual(StyleKeyword.Undefined, element.style.backgroundSize.keyword);

            element.ClearGradientBackground();

            Assert.AreEqual(StyleKeyword.Null, element.style.backgroundImage.keyword);
            Assert.AreEqual(StyleKeyword.Null, element.style.backgroundSize.keyword);
            Assert.AreEqual(StyleKeyword.Null, element.style.backgroundPositionX.keyword);
            Assert.AreEqual(StyleKeyword.Null, element.style.backgroundPositionY.keyword);
        }
    }
}
