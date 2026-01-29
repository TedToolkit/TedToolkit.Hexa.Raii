using Hexa.NET.ImGui;

using Sourcy.DotNet;

namespace Build.Modules;

/// <summary>
/// Generate the code with ImGui.
/// </summary>
internal sealed class GenerateImGuiRaiiCodeModule : GenerateRaiiCodeModule
{
    protected override Type TargetType => typeof(ImGui);
    protected override FileInfo Project => Projects.TedToolkit_Hexa_Raii_ImGui;
}