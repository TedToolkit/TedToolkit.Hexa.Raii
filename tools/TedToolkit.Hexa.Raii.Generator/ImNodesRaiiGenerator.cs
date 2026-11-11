using Hexa.NET.ImNodes;

namespace TedToolkit.Hexa.Raii.Generator;

/// <summary>
/// Generate the code with ImGui.
/// </summary>
internal sealed class ImNodesRaiiGenerator : RaiiGenerator
{
    protected override Type TargetType => typeof(ImNodes);
    protected override string ProjectDirectory => "TedToolkit.Hexa.Raii.ImNodes";
}