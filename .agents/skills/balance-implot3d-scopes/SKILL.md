---
name: balance-implot3d-scopes
description: >-
  Use TedToolkit.Hexa.Raii.ImPlot3D to scope 3D plots, styles, and colormaps in Hexa.NET rendering code.
---

# Balance ImPlot3D Scopes

Submit surfaces and other series only while the 3D plot is active. Keep style and colormap scopes narrow so an unrelated plot cannot inherit state.

## Workflow

- Open the plot with `ImPlot3DRaii.Plot` inside `using`. Check the returned scope before submitting data or making plot-only calls.
- Use matching `ImPlot3DRaii` style and colormap scopes and keep each scope’s lifetime within the intended drawing region.
- Verify both an active plot and a skipped plot. Disposal should balance native state even if drawing exits early.

## Example

```csharp
using TedToolkit.Hexa.Raii;

// Basic 3D plot
using var plot = ImPlot3DRaii.Plot("My 3D Plot");
if (plot)
{
    ImPlot3D.PlotSurface("Surface", ref xData[0], ref yData[0], ref zData[0], nx, ny);
}

// With size and flags
using var plot = ImPlot3DRaii.Plot("My 3D Plot", new Vector2(400, 400), ImPlot3DFlags.NoLegend);
if (plot)
{
    ImPlot3D.PlotScatter("Points", ref xData[0], ref yData[0], ref zData[0], count);
}

// Style push/pop
using (ImPlot3DRaii.StyleColor(ImPlot3DCol.PlotBg, new Vector4(0.1f, 0.1f, 0.1f, 1)))
using (ImPlot3DRaii.Colormap(ImPlot3DColormap.Plasma))
{
    // Plots rendered here use the custom style
}

// Conditional rendering via enable parameter
using (ImPlot3DRaii.Plot("My 3D Plot", enable: show3D))
{
    // Only rendered when show3D is true
}
```

## Check the result

Test an inactive and an active plot. Ensure grid dimensions agree with the supplied data and a skipped plot does not submit series data.
