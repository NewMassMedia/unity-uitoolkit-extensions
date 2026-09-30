# Changelog

All notable changes to `com.maemi.unity-uitoolkit-extensions`. Versions match the `vX.Y.Z` git tags.

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
