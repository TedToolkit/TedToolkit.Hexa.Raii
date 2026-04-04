# TedToolkit.Hexa.Raii.ImPlot

RAII-style scope wrappers for [Hexa.NET.ImPlot](https://github.com/HexaEngine/Hexa.NET.ImPlot), enabling safe and ergonomic use of ImPlot's `Begin`/`End` and `Push`/`Pop` API pairs via C# `using` statements.

## Installation

```
dotnet add package TedToolkit.Hexa.Raii.ImPlot
```

## Overview

ImPlot requires paired calls such as `BeginPlot`/`EndPlot`, `BeginSubplots`/`EndSubplots`, `PushStyleColor`/`PopStyleColor`, etc. These wrappers guarantee the matching cleanup function is always invoked when a scope exits, using `readonly ref struct` types that implement `IDisposable`.

Each wrapper:
- Returns a stack-allocated `readonly ref struct` — zero heap allocation
- Exposes a `Succeed` property (and `bool` operators) for conditional content rendering
- Accepts an optional `enable` parameter to skip the `Begin`/`End` pair entirely
- Is marked `AggressiveInlining` for zero overhead

## Usage

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

## Available Scopes

| Method | Wraps | Cleanup |
|--------|-------|---------|
| `Plot` | `ImPlot.BeginPlot` | `ImPlot.EndPlot` |
| `Subplots` | `ImPlot.BeginSubplots` | `ImPlot.EndSubplots` |
| `AlignedPlots` | `ImPlot.BeginAlignedPlots` | `ImPlot.EndAlignedPlots` |
| `LegendPopup` | `ImPlot.BeginLegendPopup` | `ImPlot.EndLegendPopup` |
| `Window` | `ImPlot.BeginPlotWindow` | `ImPlot.EndPlotWindow` |
| `Item` | `ImPlot.BeginItem` | `ImPlot.EndItem` |
| `StyleColor` | `ImPlot.PushStyleColor` | `ImPlot.PopStyleColor` |
| `StyleVar` | `ImPlot.PushStyleVar` | `ImPlot.PopStyleVar` |
| `Colormap` | `ImPlot.PushColormap` | `ImPlot.PopColormap` |
| `PlotClipRect` | `ImPlot.PushPlotClipRect` | `ImPlot.PopPlotClipRect` |

## Target Frameworks

net6.0 · net7.0 · net8.0 · net9.0 · net10.0 · netstandard2.0 · netstandard2.1
