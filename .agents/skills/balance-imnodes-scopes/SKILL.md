---
name: balance-imnodes-scopes
description: >-
  Use TedToolkit.Hexa.Raii.ImNodes to balance editor, node, title bar, pin, and style scopes in C# rendering code.
---

# Balance ImNodes Scopes

Keep the editor, node, and pin lifetimes distinct. Use stable IDs for graph identity and nest pin scopes inside the node that owns them.

## Workflow

- Nest `ImNodesRaii.NodeEditor()` around nodes, then place title bar, input, and output scopes within their node. Preserve the existing node and pin IDs.
- Dispose the innermost scope before opening a sibling. Keep editor-specific operations inside the editor lifetime and avoid manually ending a scope owned by `using`.
- Exercise a frame with a skipped editor and a frame with multiple nodes to verify that Begin and End remain balanced.

## Example

```csharp
using var editor = ImNodesRaii.NodeEditor();
using var node = ImNodesRaii.Node(nodeId);
using (ImNodesRaii.InputAttribute(pinId))
{
    // Draw the input pin here.
}
```

## Check the result

Exercise a node with both input and output pins and a skipped editor frame. Preserve IDs and verify sibling nodes do not inherit each other’s scoped style.
