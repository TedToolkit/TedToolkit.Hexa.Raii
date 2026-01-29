using Hexa.NET.ImPlot;

using Sourcy.DotNet;

namespace Build.Modules;

/// <summary>
/// Generate the code with ImGui.
/// </summary>
internal sealed class GenerateImPlotRaiiCodeModule : GenerateRaiiCodeModule
{
    protected override Type TargetType => typeof(ImPlot);
    protected override FileInfo Project => Projects.TedToolkit_Hexa_Raii_ImPlot;
}