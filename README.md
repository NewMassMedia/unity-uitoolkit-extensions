# com.maemi.unity-uitoolkit-extensions

Reusable UI Toolkit `VisualElement` extension helpers for Unity.

## Contents

- `VisualElement.Style`
- `VisualElement.Layout`
- `VisualElement.Text` — see [SetText on Unity 6000.6+](#settext-on-unity-60006)
- `VisualElement.Query`
- `VisualElement.Hierarchy`
- `VisualElement.Bounds`
- `VisualElement.Anchoring`
- `VisualElement.Animation` — Easing, Tween, rotation/class scheduling
- `VisualElement.Rendering` — dashed border, gradient background
- `VisualElement.Transition` — fade/slide/scale enter-exit
- `RectFitUtility`, `RectResizeUtility` — pure Rect geometry
- `Callback/Callback.*`
- `Visuals/VisualElement.*`

## Notes

- Namespace: `Core.UI.Extensions`
- Assembly: `Core.UI.Extensions`
- This package is intended to stay focused on lightweight `VisualElement` helper APIs.

## Callback phase defaults

`Callback(...)` / `RemoveCallback(...)` register in the trickle-down phase by default (`TrickleDown.TrickleDown`),
except the enter/leave overloads, which default to `TrickleDown.NoTrickleDown` since 0.1.12:

| Overloads | Default |
|---|---|
| `MouseEnterEvent`, `MouseLeaveEvent`, `PointerEnterEvent`, `PointerLeaveEvent` | `NoTrickleDown` (at target) |
| every other event | `TrickleDown` |

Why: enter/leave events do not bubble, but they trickle down through every ancestor of the element that was
entered or left. A trickle-down handler on a container therefore also runs for each descendant's enter/leave —
a tooltip trigger closed as soon as the pointer moved from the button onto the label inside it. Each element
already receives its own enter/leave at target, so the at-target registration is what hover logic wants.

Migration from 0.1.11 and earlier:

- `Callback(OnEnter)` + `RemoveCallback(OnEnter)` with defaults on both sides: nothing to do (both moved together).
- Code that relied on receiving descendants' enter/leave: pass `TrickleDown.TrickleDown` to both `Callback` and
  `RemoveCallback`, or use `MouseOverEvent`/`MouseOutEvent` (which bubble) through `RegisterCallback`.
- A registration with an explicit phase must be removed with the same explicit phase.
- 0.1.11 `RemoveCallback` for `PointerEnterEvent`/`PointerLeaveEvent` took no phase and always removed the
  `NoTrickleDown` registration, so it never removed a default `Callback` registration; fixed in 0.1.12.
- An enter/leave fluent `Callback(h)` removed with raw `UnregisterCallback(h, TrickleDown.TrickleDown)` (the pairing that matched
  the old enter/leave default) now leaves the handler registered: the fluent side registers `NoTrickleDown` since
  0.1.12. Remove with `RemoveCallback(h)`, or pass the same explicit phase on both sides.

Migration from 0.1.12:

- `FocusOutEvent`: `RemoveCallback(h)` without a phase removed the `NoTrickleDown` registration while
  `Callback(h)` registers `TrickleDown`, so the default pair never removed the handler. Since 0.1.13 the two
  overloads are one with an optional phase that defaults to `TrickleDown`, like `Callback`. A raw
  `RegisterCallback<FocusOutEvent>(h)` (which is `NoTrickleDown`) removed with fluent `RemoveCallback(h)` now needs
  `RemoveCallback(h, TrickleDown.NoTrickleDown)` or raw `UnregisterCallback(h)`.

From 0.1.13 every fluent `Callback` / `RemoveCallback` pair uses the same default phase, so a pair written with
defaults on both sides is symmetric. The phase is part of the registration identity: mixing fluent and raw APIs
is symmetric only when both sides end up with the same phase, even on the element's own (at-target) events.

### Focus and blur see descendants

`FocusEvent` and `BlurEvent` do not bubble, but, like enter/leave, they trickle down through every ancestor of
the element that gained or lost focus. Their fluent overloads keep the `TrickleDown` default, so
`container.Callback(OnFocus)` also runs when any descendant takes focus. That is often what a container wants
(for example highlighting a form group), so the default is unchanged. To react only to the element's own focus,
pass `TrickleDown.NoTrickleDown` to both `Callback` and `RemoveCallback`, or check `evt.target == element`.
`FocusInEvent` / `FocusOutEvent` are the bubbling variants.

## SetText on Unity 6000.6+

Unity 6000.6 adds instance `TextElement.SetText` overloads. A string converts to `ReadOnlySpan<char>`, and C#
prefers an applicable instance method over an extension method, so `label.SetText("x")` binds to Unity's void
method and never reaches this package. Use `SetTextValue`.

`SetText(string)` is marked `[Obsolete]`, but the warning only appears where the extension still binds, which is
Unity 6000.0-6000.5, where it also works correctly. On 6000.6+ the call sites that need fixing get no warning,
because the compiler never considers the extension. Find them by search (`\.SetText\(`) or with a
lint/pre-commit hook; do not rely on the obsolete warning.
