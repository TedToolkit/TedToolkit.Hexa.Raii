---
name: balance-implot3d-scopes
description: >-
  Use TedToolkit.Hexa.Raii.ImPlot3D to scope 3D plots, styles, and colormaps in Hexa.NET rendering code.
---

# Balance ImPlot3D Scopes

- Open the plot with `ImPlot3DRaii.Plot` inside `using`. Check the returned scope before submitting data or making plot-only calls.
- Use matching `ImPlot3DRaii` style and colormap scopes and keep each scope’s lifetime within the intended drawing region.
- Verify both an active plot and a skipped plot. Disposal should balance native state even if drawing exits early.
