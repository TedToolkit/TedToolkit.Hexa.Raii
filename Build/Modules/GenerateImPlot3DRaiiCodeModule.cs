using Hexa.NET.ImPlot;
using Hexa.NET.ImPlot3D;

using Sourcy.DotNet;

namespace Build.Modules;

/// <summary>
/// Generate the code with ImGui.
/// </summary>
internal sealed class GenerateImPlot3DRaiiCodeModule : GenerateRaiiCodeModule
{
    protected override Type TargetType => typeof(ImPlot3D);
    protected override FileInfo Project => Projects.TedToolkit_Hexa_Raii_ImPlot3D;
}