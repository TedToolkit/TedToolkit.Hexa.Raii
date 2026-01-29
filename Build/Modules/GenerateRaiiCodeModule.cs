using Build.Generators;

using Cysharp.Text;

using Hexa.NET.ImGui;

using ModularPipelines.Context;

using Sourcy.DotNet;

using TedToolkit.ModularPipelines.Modules;
using TedToolkit.RoslynHelper.Generators;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    Build.Modules.GenerateImGuiRaiiCodeModule>;


namespace Build.Modules;

/// <summary>
/// Generate the code with raii.
/// </summary>
internal abstract class GenerateRaiiCodeModule : PrepareModule<bool>
{
    protected abstract Type TargetType { get; }
    protected abstract FileInfo Project { get; }

    /// <inheritdoc />
    protected sealed override async Task<bool> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var className = ZString.Concat(TargetType.Name, "Raii");
        var raii = Class(className).Public.Static.Partial.Unsafe;

        new PairGenerator(TargetType).GenerateItems(raii);

        var codes = File()
            .AddNameSpace(NameSpace("TedToolkit.Hexa.Raii").AddMember(raii))
            .ToCode();

        var file = Path.Combine(Project.Directory!.FullName, ZString.Concat(className, ".g.cs"));
        await System.IO.File.WriteAllTextAsync(file, codes, cancellationToken).ConfigureAwait(false);
        return true;
    }
}