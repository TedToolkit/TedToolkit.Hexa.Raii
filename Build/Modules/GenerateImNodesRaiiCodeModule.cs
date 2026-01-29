using Hexa.NET.ImNodes;

using Sourcy.DotNet;

namespace Build.Modules;

/// <summary>
/// Generate the code with ImGui.
/// </summary>
internal sealed class GenerateImNodesRaiiCodeModule : GenerateRaiiCodeModule
{
    protected override Type TargetType => typeof(ImNodes);
    protected override FileInfo Project => Projects.TedToolkit_Hexa_Raii_ImNodes;
}