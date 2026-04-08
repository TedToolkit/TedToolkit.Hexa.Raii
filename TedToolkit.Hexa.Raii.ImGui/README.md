# TedToolkit.Hexa.Raii.ImGui

[![NuGet](https://img.shields.io/nuget/v/TedToolkit.Hexa.Raii.ImGui)](https://www.nuget.org/packages/TedToolkit.Hexa.Raii.ImGui)

RAII-style scope wrappers for [Hexa.NET.ImGui](https://github.com/HexaEngine/Hexa.NET.ImGui), enabling safe and ergonomic use of ImGui's `Begin`/`End` and `Push`/`Pop` API pairs via C# `using` statements.

Part of the [TedToolkit.Hexa.Raii](https://github.com/TedToolkit/TedToolkit.Scopes) project.

## Installation

```
dotnet add package TedToolkit.Hexa.Raii.ImGui
```

## Overview

ImGui requires paired calls such as `Begin`/`End`, `PushStyleVar`/`PopStyleVar`, etc. These wrappers guarantee the matching cleanup function is always invoked when a scope exits, using `readonly ref struct` types that implement `IDisposable`.

Each wrapper:
- Returns a stack-allocated `readonly ref struct` — zero heap allocation
- Exposes a `Succeed` property (and `bool` operators) for conditional content rendering
- Accepts an optional `enable` parameter to skip the `Begin`/`End` pair entirely
- Is marked `AggressiveInlining` for zero overhead

## Usage

```csharp
using TedToolkit.Hexa.Raii;

// Basic usage — End() is always called on scope exit
using (ImGuiRaii.Window("My Window"))
{
    ImGui.Text("Hello, World!");
}

// Check Succeed to render content only when the window is expanded
using var window = ImGuiRaii.Window("My Window");
if (window) // equivalent to if (window.Succeed)
{
    ImGui.Text("Window is expanded!");
}

// Conditionally skip Begin/End entirely
using (ImGuiRaii.Window("Debug", enable: showDebug))
{
    ImGui.Text("Only rendered when showDebug is true");
}

// Nested scopes
using (ImGuiRaii.Window("My Window"))
using (ImGuiRaii.TabBar("Tabs"))
using (ImGuiRaii.TabItem("General"))
{
    ImGui.Text("Tab content");
}

// Style and layout push/pop
using (ImGuiRaii.StyleColor(ImGuiCol.Text, new Vector4(1, 0, 0, 1)))
using (ImGuiRaii.Indent())
{
    ImGui.Text("Red indented text");
}
```

## Available Scopes

| Method | Wraps | Cleanup |
|--------|-------|---------|
| `Window` | `ImGui.Begin` | `ImGui.End` |
| `Window` *(clipper)* | `ImGui.Begin(ImGuiListClipper)` | `ImGui.End(ImGuiListClipper)` |
| `Child` | `ImGui.BeginChild` | `ImGui.EndChild` |
| `Group` | `ImGui.BeginGroup` | `ImGui.EndGroup` |
| `Combo` | `ImGui.BeginCombo` | `ImGui.EndCombo` |
| `ListBox` | `ImGui.BeginListBox` | `ImGui.EndListBox` |
| `MenuBar` | `ImGui.BeginMenuBar` | `ImGui.EndMenuBar` |
| `MainMenuBar` | `ImGui.BeginMainMenuBar` | `ImGui.EndMainMenuBar` |
| `Menu` | `ImGui.BeginMenu` | `ImGui.EndMenu` |
| `Tooltip` | `ImGui.BeginTooltip` | `ImGui.EndTooltip` |
| `Popup` | `ImGui.BeginPopup` | `ImGui.EndPopup` |
| `Table` | `ImGui.BeginTable` | `ImGui.EndTable` |
| `TabBar` | `ImGui.BeginTabBar` | `ImGui.EndTabBar` |
| `TabItem` | `ImGui.BeginTabItem` | `ImGui.EndTabItem` |
| `DragDropSource` | `ImGui.BeginDragDropSource` | `ImGui.EndDragDropSource` |
| `DragDropTarget` | `ImGui.BeginDragDropTarget` | `ImGui.EndDragDropTarget` |
| `Disabled` | `ImGui.BeginDisabled` | `ImGui.EndDisabled` |
| `Font` | `ImGui.PushFont` | `ImGui.PopFont` |
| `StyleColor` | `ImGui.PushStyleColor` | `ImGui.PopStyleColor` |
| `StyleVar` | `ImGui.PushStyleVar` | `ImGui.PopStyleVar` |
| `ItemFlag` | `ImGui.PushItemFlag` | `ImGui.PopItemFlag` |
| `ItemWidth` | `ImGui.PushItemWidth` | `ImGui.PopItemWidth` |
| `TextWrapPos` | `ImGui.PushTextWrapPos` | `ImGui.PopTextWrapPos` |
| `ID` | `ImGui.PushID` | `ImGui.PopID` |
| `ClipRect` | `ImGui.PushClipRect` | `ImGui.PopClipRect` |
| `Texture` | `ImGui.PushTexture` | `ImGui.PopTexture` |
| `TreeNode` | `ImGui.TreeNode` | `ImGui.TreePop` |
| `TreeNodeV` | `ImGui.TreeNodeV` | `ImGui.TreePop` |
| `TreeNodeEx` | `ImGui.TreeNodeEx` | `ImGui.TreePop` |
| `TreeNodeExV` | `ImGui.TreeNodeExV` | `ImGui.TreePop` |
| `Tree` | `ImGui.TreePush` | `ImGui.TreePop` |
| `Indent` | `ImGui.Indent` | `ImGui.Unindent` |

## Target Frameworks

net6.0 · net7.0 · net8.0 · net9.0 · net10.0 · netstandard2.0 · netstandard2.1

## License

Licensed under the [GNU Lesser General Public License v3.0](https://github.com/TedToolkit/TedToolkit.Scopes/blob/development/COPYING.LESSER).
