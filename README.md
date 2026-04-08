# TedToolkit.Hexa.Raii

[![Build](https://github.com/TedToolkit/TedToolkit.Scopes/actions/workflows/build.yml/badge.svg)](https://github.com/TedToolkit/TedToolkit.Scopes/actions/workflows/build.yml)

RAII-style scope wrappers for the [Hexa.NET](https://github.com/HexaEngine) family of ImGui bindings. Replace manual `Begin`/`End` and `Push`/`Pop` call pairs with C# `using` statements that guarantee cleanup on scope exit.

## Packages

| Package | NuGet | Wraps |
|---------|-------|-------|
| `TedToolkit.Hexa.Raii.ImGui` | [![NuGet](https://img.shields.io/nuget/v/TedToolkit.Hexa.Raii.ImGui)](https://www.nuget.org/packages/TedToolkit.Hexa.Raii.ImGui) | [Hexa.NET.ImGui](https://www.nuget.org/packages/Hexa.NET.ImGui) |
| `TedToolkit.Hexa.Raii.ImPlot` | [![NuGet](https://img.shields.io/nuget/v/TedToolkit.Hexa.Raii.ImPlot)](https://www.nuget.org/packages/TedToolkit.Hexa.Raii.ImPlot) | [Hexa.NET.ImPlot](https://www.nuget.org/packages/Hexa.NET.ImPlot) |
| `TedToolkit.Hexa.Raii.ImPlot3D` | [![NuGet](https://img.shields.io/nuget/v/TedToolkit.Hexa.Raii.ImPlot3D)](https://www.nuget.org/packages/TedToolkit.Hexa.Raii.ImPlot3D) | [Hexa.NET.ImPlot3D](https://www.nuget.org/packages/Hexa.NET.ImPlot3D) |
| `TedToolkit.Hexa.Raii.ImNodes` | [![NuGet](https://img.shields.io/nuget/v/TedToolkit.Hexa.Raii.ImNodes)](https://www.nuget.org/packages/TedToolkit.Hexa.Raii.ImNodes) | [Hexa.NET.ImNodes](https://www.nuget.org/packages/Hexa.NET.ImNodes) |

## Why

ImGui APIs rely on balanced call pairs. Forgetting the `End` or `Pop` causes subtle rendering bugs that are hard to track down:

```csharp
// Without RAII — easy to forget End() on an early return
if (ImGui.Begin("My Window"))
{
    if (someCondition) return; // Bug: End() never called
    ImGui.Text("Hello");
}
ImGui.End();
```

With these wrappers the cleanup is automatic:

```csharp
// With RAII — End() is always called when the using block exits
using (ImGuiRaii.Window("My Window"))
{
    if (someCondition) return; // End() is called correctly
    ImGui.Text("Hello");
}
```

## Quick Start

Install the package for your binding of choice:

```
dotnet add package TedToolkit.Hexa.Raii.ImGui
```

Then use the static `*Raii` class in the `TedToolkit.Hexa.Raii` namespace:

```csharp
using TedToolkit.Hexa.Raii;

void DrawUI()
{
    using (ImGuiRaii.Window("My Window"))
    using (ImGuiRaii.TabBar("Tabs"))
    {
        using (ImGuiRaii.TabItem("Settings"))
        {
            using (ImGuiRaii.StyleColor(ImGuiCol.Text, new Vector4(1, 0.8f, 0, 1)))
            using (ImGuiRaii.Indent())
            {
                ImGui.Text("Indented golden text");
            }
        }
    }
}
```

To use multiple bindings together, install additional packages:

```
dotnet add package TedToolkit.Hexa.Raii.ImPlot
```

```csharp
using (ImGuiRaii.TabItem("Plot"))
{
    using var plot = ImPlotRaii.Plot("My Chart", new Vector2(-1, 200));
    if (plot)
    {
        ImPlot.PlotLine("Data", ref x[0], ref y[0], count);
    }
}
```

## Design

All wrappers share the same three features:

**Zero allocation** — each method returns a `readonly ref struct`, keeping the disposable on the stack with no GC involvement.

**`Succeed` property** — methods whose `Begin` call can return `false` (e.g. collapsed windows, filtered popups) expose this as the `Succeed` property. The struct also defines `true`/`false`/`!` operators so it can be used directly in `if` conditions:

```csharp
using var window = ImGuiRaii.Window("My Window");
if (window) // window.Succeed
{
    ImGui.Text("Window is expanded");
}
```

**`enable` parameter** — pass `enable: false` to skip the `Begin`/`End` pair entirely without changing the surrounding code structure:

```csharp
using (ImGuiRaii.Window("Debug", enable: showDebug))
{
    ImGui.Text("Only rendered when showDebug is true");
}
```

## Packages at a Glance

See each package's README for its full scope list.

- **ImGui** — `Window`, `Child`, `Group`, `Combo`, `ListBox`, `MenuBar`, `MainMenuBar`, `Menu`, `Tooltip`, `Popup`, `Table`, `TabBar`, `TabItem`, `DragDropSource`, `DragDropTarget`, `Disabled`, `Font`, `StyleColor`, `StyleVar`, `ItemFlag`, `ItemWidth`, `TextWrapPos`, `ID`, `ClipRect`, `Texture`, `TreeNode`/`V`/`Ex`/`ExV`, `Tree`, `Indent`
- **ImPlot** — `Plot`, `Subplots`, `AlignedPlots`, `LegendPopup`, `Window`, `Item`, `StyleColor`, `StyleVar`, `Colormap`, `PlotClipRect`
- **ImPlot3D** — `Plot`, `StyleColor`, `StyleVar`, `Colormap`
- **ImNodes** — `NodeEditor`, `Node`, `NodeTitleBar`, `InputAttribute`, `OutputAttribute`, `StaticAttribute`, `ColorStyle`, `StyleVar`, `AttributeFlag`

## Target Frameworks

net6.0 · net7.0 · net8.0 · net9.0 · net10.0 · netstandard2.0 · netstandard2.1

## License

Licensed under the [GNU Lesser General Public License v3.0](COPYING.LESSER).
