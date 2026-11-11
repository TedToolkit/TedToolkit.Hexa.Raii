using TedToolkit.FileSystem;
using TedToolkit.Hexa.Raii.Generator;
using TedToolkit.Orchestration.Pipeline;
using TedToolkit.Orchestration.Pipeline.Attributes;

await new SourceGeneration.GeneratePipeline().ExecuteAsync(DirectoryPath.CurrentDirectory);

internal static partial class SourceGeneration
{
    [Pipeline]
    public static void Generate(StepGraph pipeline, DirectoryPath repository)
    {
        pipeline.GenerateImGui(repository);
        pipeline.GenerateImNodes(repository);
        pipeline.GenerateImPlot(repository);
        pipeline.GenerateImPlot3D(repository);
    }
}

internal static class GeneratorSteps
{
    [Step]
    internal static Task GenerateImGui(DirectoryPath repository, CancellationToken cancellationToken) =>
        new ImGuiRaiiGenerator().GenerateAsync(repository, cancellationToken);

    [Step]
    internal static Task GenerateImNodes(DirectoryPath repository, CancellationToken cancellationToken) =>
        new ImNodesRaiiGenerator().GenerateAsync(repository, cancellationToken);

    [Step]
    internal static Task GenerateImPlot(DirectoryPath repository, CancellationToken cancellationToken) =>
        new ImPlotRaiiGenerator().GenerateAsync(repository, cancellationToken);

    [Step]
    internal static Task GenerateImPlot3D(DirectoryPath repository, CancellationToken cancellationToken) =>
        new ImPlot3DRaiiGenerator().GenerateAsync(repository, cancellationToken);

}
