using Hexa.NET.ImPlot;

namespace TedToolkit.Hexa.Raii.Generator;

/// <summary>
/// Generate the code with ImGui.
/// </summary>
internal sealed class ImPlotRaiiGenerator : RaiiGenerator
{
    protected override Type TargetType => typeof(ImPlot);
    protected override string ProjectDirectory => "TedToolkit.Hexa.Raii.ImPlot";
}