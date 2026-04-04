# TedToolkit.Hexa.Raii.ImPlot3D

RAII-style scope wrappers for [Hexa.NET.ImPlot3D](https://github.com/HexaEngine/Hexa.NET.ImPlot3D), enabling safe and ergonomic use of ImPlot3D's `Begin`/`End` and `Push`/`Pop` API pairs via C# `using` statements.

## Installation

```
dotnet add package TedToolkit.Hexa.Raii.ImPlot3D
```

## Overview

ImPlot3D requires paired calls such as `BeginPlot`/`EndPlot`, `PushStyleColor`/`PopStyleColor`, etc. These wrappers guarantee the matching cleanup function is always invoked when a scope exits, using `readonly ref struct` types that implement `IDisposable`.

Each wrapper:
- Returns a stack-allocated `readonly ref struct` — zero heap allocation
- Exposes a `Succeed` property (and `bool` operators) for conditional content rendering
- Accepts an optional `enable` parameter to skip the `Begin`/`End` pair entirely
- Is marked `AggressiveInlining` for zero overhead

## Usage

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

## Available Scopes

| Method | Wraps | Cleanup |
|--------|-------|---------|
| `Plot` | `ImPlot3D.BeginPlot` | `ImPlot3D.EndPlot` |
| `StyleColor` | `ImPlot3D.PushStyleColor` | `ImPlot3D.PopStyleColor` |
| `StyleVar` | `ImPlot3D.PushStyleVar` | `ImPlot3D.PopStyleVar` |
| `Colormap` | `ImPlot3D.PushColormap` | `ImPlot3D.PopColormap` |

## Target Frameworks

net6.0 · net7.0 · net8.0 · net9.0 · net10.0 · netstandard2.0 · netstandard2.1
