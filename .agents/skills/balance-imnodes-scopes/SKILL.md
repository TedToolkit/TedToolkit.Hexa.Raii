---
name: balance-imnodes-scopes
description: >-
  Use TedToolkit.Hexa.Raii.ImNodes to balance editor, node, title bar, pin, and style scopes in C# rendering code.
---

# Balance ImNodes Scopes

- Nest `ImNodesRaii.NodeEditor()` around nodes, then place title bar, input, and output scopes within their node. Preserve the existing node and pin IDs.
- Dispose the innermost scope before opening a sibling. Keep editor-specific operations inside the editor lifetime and avoid manually ending a scope owned by `using`.
- Exercise a frame with a skipped editor and a frame with multiple nodes to verify that Begin and End remain balanced.
