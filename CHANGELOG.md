# Changelog

All notable changes to `com.maemi.unity-uitoolkit-extensions`. Versions match the `vX.Y.Z` git tags.

## [0.1.15] - 2026-11-09

### Fixed

- `ToggleDisplay` read only the inline `display`, so an element hidden by a USS class ignored its first toggle. It now
  toggles from the effective display.
- `IsInScreen` / `IsOutOfScreen` excluded the right and bottom edge, so an element flush with them (a full-screen
  overlay, a bottom bar) counted as out of screen. The edges are now inclusive. An element with no panel is still
  out of screen.
- `ClearGradientBackground` left an inline `background-image: none` and the forced size and position behind, which
  hid the element's stylesheet background. It now returns all four properties to their stylesheet values.
- `Destroy` threw for a hierarchy child of a parent whose `contentContainer` is another element. It now uses
  `RemoveFromHierarchy`.
- The `onComplete` of `FadeOut`, `SlideOutTo` and `ScaleOut` waited for a transition event only, so it never ran when no
  transition ran: duration 0, the value already at its target (a close in the same frame as the open), or the element
  detached while it ran. It now runs once: on the element's own end or cancel, on detach, or after the duration plus
  100 ms. At duration 0 it runs at once. Only the element's own end or cancel of the animated property, after that
  transition's own `TransitionRunEvent`, ends it now (any cancel did before), so the cancel of the transition it
  replaced (a `FadeOut` while a `FadeIn` still runs) no longer ends it at once. The 100 ms fallback restarts when the
  transition starts, so a transition that starts a slow frame late is not cut short.
- `FadeIn` / `FadeOut`, `SlideInFrom` / `SlideOutTo` and `ScaleIn` / `ScaleOut` left their inline `transition-property`,
  `-duration` and `-timing-function` behind, which replaced the element's USS transitions for good. They now clear them
  when the transition ends.
- `SetDashedBorder(true)` and `SetGradientBackground` turned themselves off when the element left its panel, so a
  reparented element (moved to a popup layer, a rebuilt list) lost its border or gradient while its owner still thought
  it was on. Both now stay on until turned off. The gradient's texture exists only while the element is attached: it is
  built on attach and destroyed on detach.
- The dashed-border and gradient state was held in static dictionaries with strong keys, so an element configured but
  never attached stayed in memory for good, together with its texture. The state now uses weak keys, and a detached
  element holds no texture.
- `AfterLayout` waited for the next `GeometryChangedEvent`, which comes only when the layout rect changes, so on an
  element already laid out at its final size (re-attached, re-opened) the callback ran late or never. It now also runs
  after the element's next repaint, whichever comes first, still once.
- `PlaceAnchored` wrote the panel-space top-left straight to `left` / `top`, which count from the parent, so inside an
  offset parent the element landed off by the parent's position. It now converts to the parent's space (inside its
  border). The element must be absolutely positioned, as before.

### Changed

- A new `FadeIn` / `FadeOut` / `SlideInFrom` / `SlideOutTo` / `ScaleIn` / `ScaleOut` on an element replaces the one still
  running there: the earlier call's `onComplete` is dropped instead of firing when the new one cancels it. A window
  closed and reopened in one frame no longer hides itself. A call still waiting out its `delay` is not cancelled: it
  starts when its delay ends and replaces whatever runs then.
- `PlaceAnchored`: a caller that already subtracted the parent's position from the result must stop doing so. A
  parent at the panel origin with no border gets the same result as before.
- `RemoveAllChildren` calls `Clear()` (was an O(n^2) `RemoveAt(0)` loop), `SetPickingModeRecursive(bool)` delegates to
  the `PickingMode` overload, and `StartClassToggle` uses `ToggleClass`. No behaviour change.
- First tests: `Tests/Editor` (`Core.UI.Extensions.Tests.Editor`).

## [0.1.14] - 2026-11-05

### Fixed

- `Tween` measured elapsed time with `Time.unscaledTime`, which does not advance in Edit Mode, so a tween in an
  EditorWindow (a foundation `Sonner` toast sliding in) stayed at its first frame. It now measures
  `Time.realtimeSinceStartupAsDouble`. Play Mode timing is unchanged except that a paused game no longer
  freezes UI tweens.

## [0.1.13] - 2026-09-30

### Fixed

- `RemoveCallback` for `FocusOutEvent`: the no-phase overload removed the `NoTrickleDown` registration while
  `Callback(h)` registers `TrickleDown`, so `Callback(h)` + `RemoveCallback(h)` never removed the handler. The two
  overloads are now one with an optional phase that defaults to `TrickleDown`, matching `Callback`. Existing
  `RemoveCallback(h)` and `RemoveCallback(h, phase)` calls still compile. Every fluent `Callback` / `RemoveCallback`
  pair now uses the same default phase.

### Behavior change

- `FocusOutEvent` `RemoveCallback(h)` without a phase now removes the `TrickleDown` registration instead of the
  `NoTrickleDown` one.

### Migration

- Raw `RegisterCallback<FocusOutEvent>(h)` (which registers `NoTrickleDown`) removed with fluent `RemoveCallback(h)`:
  change the removal to `RemoveCallback(h, TrickleDown.NoTrickleDown)` or raw `UnregisterCallback(h)`.

### Documentation

- README: `FocusEvent` / `BlurEvent` trickle down through ancestors, so the `TrickleDown` default on a container
  also sees descendants' focus and blur (behaviour unchanged).
- README: on Unity 6000.6+ the `[Obsolete]` warning on `SetText(string)` never fires, because Unity's instance
  `TextElement.SetText(ReadOnlySpan<char>)` wins overload resolution. Find those call sites by search or hook.
- Added this changelog.

## [0.1.12] - 2026-09-29

### Behavior change

- `Callback` / `RemoveCallback` for `MouseEnterEvent`, `MouseLeaveEvent`, `PointerEnterEvent` and
  `PointerLeaveEvent` default to `TrickleDown.NoTrickleDown` (was `TrickleDown.TrickleDown`). Enter/leave events
  trickle down through every ancestor, so a container's trickle-down handler also ran for each descendant's
  enter/leave. Every other event keeps the `TrickleDown` default. Shipped as a patch release although it changes a
  default phase.

### Fixed

- `RemoveCallback` for `PointerEnterEvent` / `PointerLeaveEvent` took no phase and always removed the
  `NoTrickleDown` registration, so it never removed a default `Callback` registration. Both now take an optional
  phase that matches `Callback`.

### Migration

- Fluent `Callback(h)` + fluent `RemoveCallback(h)` with defaults on both sides: nothing to do.
- Code that relied on receiving descendants' enter/leave: pass `TrickleDown.TrickleDown` to both sides, or use the
  bubbling `MouseOverEvent` / `MouseOutEvent`.
- Fluent enter/leave `Callback(h)` removed with raw `UnregisterCallback(h, TrickleDown.TrickleDown)` (the pairing
  that matched the old default): the handler now stays registered, because the fluent side registers
  `NoTrickleDown`. Remove with fluent `RemoveCallback(h)`, or pass the same explicit phase on both sides.
- Raw `RegisterCallback<...Enter/LeaveEvent>(h)` (`NoTrickleDown`) removed with fluent `RemoveCallback(h)`: now
  symmetric for all four events. Before 0.1.12 it was asymmetric for mouse enter/leave (fluent removal used
  `TrickleDown`) and already symmetric for pointer enter/leave.

## [0.1.11] - 2026-09-29

### Added

- `SetTextValue<T>(string)`: always assigns `element.text`. It has no Unity counterpart, so it binds on every
  Unity version.

### Deprecated

- `SetText<T>(string)` is `[Obsolete]` in favour of `SetTextValue`. On Unity 6000.6+ `label.SetText("x")` binds to
  Unity's instance `TextElement.SetText(ReadOnlySpan<char>)`, which also warns when the element is not attached to a
  panel yet. The obsolete warning only appears on Unity 6000.0-6000.5 (see 0.1.13).

## [0.1.10] - 2026-09-23

### Added

- `SetTextOverflowPosition<T>(TextOverflowPosition)` next to `SetTextOverflow`, for start / middle / end ellipsis
  in the fluent chain.
