---
name: balance-imgui-scopes
description: >-
  Use TedToolkit.Hexa.Raii.ImGui to balance ImGui windows, children, tabs, and pushed styles in C# rendering code.
---

# Balance ImGui Scopes

- Wrap each native Begin/End or Push/Pop pair in its matching `ImGuiRaii` scope. Keep `using` as narrow as the original native pair so early returns still dispose the scope.
- Use the scope’s visibility or open result to decide whether to draw contents. Keep disposal even when the native window is collapsed.
- Check that nested scopes unwind in reverse order and that manual `End` or `Pop` calls are removed for pairs owned by a wrapper.
