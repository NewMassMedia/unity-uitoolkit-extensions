# com.maemi.unity-uitoolkit-extensions

Reusable UI Toolkit `VisualElement` extension helpers for Unity.

## Contents

- `VisualElement.Style`
- `VisualElement.Layout`
- `VisualElement.Text`
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
  `NoTrickleDown` registration, so it never removed a default `Callback` registration; that mismatch is gone.
