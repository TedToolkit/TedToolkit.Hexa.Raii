# TedToolkit.Hexa.Raii.ImNodes

RAII-style scope wrappers for [Hexa.NET.ImNodes](https://github.com/HexaEngine/Hexa.NET.ImNodes), enabling safe and ergonomic use of ImNodes' `Begin`/`End` and `Push`/`Pop` API pairs via C# `using` statements.

## Installation

```
dotnet add package TedToolkit.Hexa.Raii.ImNodes
```

## Overview

ImNodes requires paired calls such as `BeginNodeEditor`/`EndNodeEditor`, `BeginNode`/`EndNode`, `PushColorStyle`/`PopColorStyle`, etc. These wrappers guarantee the matching cleanup function is always invoked when a scope exits, using `readonly ref struct` types that implement `IDisposable`.

Each wrapper:
- Returns a stack-allocated `readonly ref struct` — zero heap allocation
- Exposes a `Succeed` property (and `bool` operators) for conditional content rendering
- Accepts an optional `enable` parameter to skip the `Begin`/`End` pair entirely
- Is marked `AggressiveInlining` for zero overhead

## Usage

```csharp
using TedToolkit.Hexa.Raii;

// Node editor with nodes
using (ImNodesRaii.NodeEditor())
{
    using (ImNodesRaii.Node(nodeId))
    {
        using (ImNodesRaii.NodeTitleBar())
        {
            ImGui.Text("My Node");
        }

        using (ImNodesRaii.InputAttribute(pinId))
        {
            ImGui.Text("Input");
        }

        using (ImNodesRaii.OutputAttribute(outPinId))
        {
            ImGui.Text("Output");
        }
    }
}

// Conditional rendering via enable parameter
using (ImNodesRaii.NodeEditor(enable: showEditor))
{
    // Only rendered when showEditor is true
}

// Style push/pop
using (ImNodesRaii.ColorStyle(ImNodesCol.NodeBackground, 0xFF2255AA))
{
    // Nodes rendered here use the custom background color
}
```

## Available Scopes

| Method | Wraps | Cleanup |
|--------|-------|---------|
| `NodeEditor` | `ImNodes.BeginNodeEditor` | `ImNodes.EndNodeEditor` |
| `Node` | `ImNodes.BeginNode` | `ImNodes.EndNode` |
| `NodeTitleBar` | `ImNodes.BeginNodeTitleBar` | `ImNodes.EndNodeTitleBar` |
| `InputAttribute` | `ImNodes.BeginInputAttribute` | `ImNodes.EndInputAttribute` |
| `OutputAttribute` | `ImNodes.BeginOutputAttribute` | `ImNodes.EndOutputAttribute` |
| `StaticAttribute` | `ImNodes.BeginStaticAttribute` | `ImNodes.EndStaticAttribute` |
| `ColorStyle` | `ImNodes.PushColorStyle` | `ImNodes.PopColorStyle` |
| `StyleVar` | `ImNodes.PushStyleVar` | `ImNodes.PopStyleVar` |
| `AttributeFlag` | `ImNodes.PushAttributeFlag` | `ImNodes.PopAttributeFlag` |

## Target Frameworks

net6.0 · net7.0 · net8.0 · net9.0 · net10.0 · netstandard2.0 · netstandard2.1
