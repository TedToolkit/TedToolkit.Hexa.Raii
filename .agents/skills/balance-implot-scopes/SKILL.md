---
name: balance-implot-scopes
description: >-
  Use TedToolkit.Hexa.Raii.ImPlot for balanced plot, subplot, style, and colormap lifetimes in C# rendering code.
---

# Balance ImPlot Scopes

Keep per-plot calls inside an active plot. Open the subplot container before starting child plots and close each plot before starting the next.

## Workflow

- Use `ImPlotRaii.Plot` inside a `using` scope and submit series only when the scope indicates an active plot.
- Nest individual plots inside `ImPlotRaii.Subplots` for a subplot grid. Scope styles or colormaps to exactly the plots they should affect.
- Check the collapsed and active cases and confirm that an early return still releases every active wrapper.

## Example

```csharp
using TedToolkit.Hexa.Raii;

// Basic plot
using var plot = ImPlotRaii.Plot("My Plot");
if (plot)
{
    ImPlot.PlotLine("Series 1", ref xData[0], ref yData[0], count);
}

// With size and flags
using var plot = ImPlotRaii.Plot("My Plot", new Vector2(400, 300), ImPlotFlags.NoLegend);
if (plot)
{
    ImPlot.PlotBars("Bars", ref data[0], count);
}

// Subplots
using var subplots = ImPlotRaii.Subplots("##subplots", rows: 2, cols: 2, new Vector2(-1, 400));
if (subplots)
{
    for (int i = 0; i < 4; i++)
    {
        using var subplot = ImPlotRaii.Plot($"Plot {i}");
        if (subplot)
        {
            // ...
        }
    }
}

// Style push/pop
using (ImPlotRaii.StyleColor(ImPlotCol.Line, new Vector4(1, 0.5f, 0, 1)))
using (ImPlotRaii.Colormap(ImPlotColormap.Viridis))
{
    // Plots rendered here use the custom style
}
```

## Check the result

Exercise inactive plots and several subplots. Verify series draw only on successful begin and style/colormap state restores after early returns.
