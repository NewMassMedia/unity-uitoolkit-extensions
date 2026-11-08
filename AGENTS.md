# unity-uitoolkit-extensions (`com.maemi.unity-uitoolkit-extensions`) — rules for agents

`VisualElement` fluent extension helpers (`Callback`, `Text`, `Layout`, `Tween`, ...) for UI Toolkit. This package is the
sibling of `unity-uitoolkit-foundation`, and its rules are that repo's: read
[`../unity-uitoolkit-foundation/AGENTS.md`](../unity-uitoolkit-foundation/AGENTS.md), "Editing this package" first.
This file adds only what is specific here.

- **Enter/leave handlers register with `TrickleDown.NoTrickleDown`** and unregister the same way; the phase is part of
  the registration identity. See [README](README.md), "Callback phase defaults".
- **Text is set with `SetTextValue`**, never `SetText(<string>)`, which binds to Unity's `SetText(span)` on 6000.6+.
  See [README](README.md), "SetText on Unity 6000.6+".
- Namespace `Core.UI.Extensions.*`. `Core.UI.Foundation.*` is the sibling package, not this repo.
- Licence MIT, holder "Maemi", same as the foundation (`LICENSE.md`).
- `CHANGELOG.md` versions match the `vX.Y.Z` git tags; a code change gets an entry and a version bump, a docs-only
  change does not.
