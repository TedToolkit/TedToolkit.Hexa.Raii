using Hexa.NET.ImPlot;
using Hexa.NET.ImPlot3D;

namespace TedToolkit.Hexa.Raii.Generator;

/// <summary>
/// Generate the code with ImGui.
/// </summary>
internal sealed class ImPlot3DRaiiGenerator : RaiiGenerator
{
    protected override Type TargetType => typeof(ImPlot3D);
    protected override string ProjectDirectory => "TedToolkit.Hexa.Raii.ImPlot3D";
}