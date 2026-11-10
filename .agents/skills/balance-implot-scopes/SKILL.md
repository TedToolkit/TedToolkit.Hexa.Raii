---
name: balance-implot-scopes
description: >-
  Use TedToolkit.Hexa.Raii.ImPlot for balanced plot, subplot, style, and colormap lifetimes in C# rendering code.
---

# Balance ImPlot Scopes

- Use `ImPlotRaii.Plot` inside a `using` scope and submit series only when the scope indicates an active plot.
- Nest individual plots inside `ImPlotRaii.Subplots` for a subplot grid. Scope styles or colormaps to exactly the plots they should affect.
- Check the collapsed and active cases and confirm that an early return still releases every active wrapper.
