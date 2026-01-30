using System.Reflection;

using Build.Generators;

using Cysharp.Text;

using ModularPipelines.Context;

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
    private static readonly IReadOnlyList<Func<PairGenerator>> _defaultGenerators =
    [
        () => new(
            static method => method.Name.StartsWith("Begin") ? method.Name[5..] : "",
            static method => method.Name.StartsWith("End") ? [method.Name[3..]] : []),

        () => new(
            static method => method.Name.StartsWith("Push") ? method.Name[4..] : "",
            static method => method.Name.StartsWith("Pop") ? [method.Name[3..]] : []),
    ];

    protected abstract Type TargetType { get; }
    protected abstract FileInfo Project { get; }

    protected virtual IReadOnlyList<Func<PairGenerator>> AdditionalGenerators => [];

    /// <inheritdoc />
    protected sealed override async Task<bool> ExecuteAsync(IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var className = ZString.Concat(TargetType.Name, "Raii");
        var raii = Class(className).Public.Static.Partial.Unsafe;

        var generators = _defaultGenerators
            .Concat(AdditionalGenerators)
            .Select(i => i())
            .ToArray();

        foreach (var runtimeMethod in TargetType.GetRuntimeMethods()
                     .Where(m => m.IsPublic))
        {
            foreach (var singlePariGenerator in generators)
            {
                singlePariGenerator.AppendMethod(TargetType, runtimeMethod);
            }
        }

        foreach (var singlePariGenerator in generators)
        {
            singlePariGenerator.GenerateItems(raii);
        }

        var codes = File()
            .AddNameSpace(NameSpace("TedToolkit.Hexa.Raii").AddMember(raii))
            .ToCode();

        var file = Path.Combine(Project.Directory!.FullName, ZString.Concat(className, ".g.cs"));
        await System.IO.File.WriteAllTextAsync(file, codes, cancellationToken).ConfigureAwait(false);
        return true;
    }
}