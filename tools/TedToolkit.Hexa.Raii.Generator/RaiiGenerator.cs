using System.Reflection;

using TedToolkit.Hexa.Raii.Generator.Composition;
using TedToolkit.FileSystem;

using TedToolkit.RoslynHelper;

using static TedToolkit.RoslynHelper.SourceComposer;
using static TedToolkit.RoslynHelper.SourceComposer<
    TedToolkit.Hexa.Raii.Generator.ImGuiRaiiGenerator>;

namespace TedToolkit.Hexa.Raii.Generator;

/// <summary>
/// Generate the code with raii.
/// </summary>
internal abstract class RaiiGenerator
{
    private static readonly IReadOnlyList<Func<PairGenerator>> _defaultGenerators =
    [
        () => new(
            static method => method.Name is "Begin"
                ? "Window"
                : method.Name.StartsWith("Begin")
                    ? method.Name[5..]
                    : "",
            static method => method.Name is "End"
                ? ["Window"]
                : method.Name.StartsWith("End")
                    ? [method.Name[3..]]
                    : [],
            static method => method.Name is "Begin" or "BeginChild"),

        () => new(
            static method => method.Name.StartsWith("Push") ? method.Name[4..] : "",
            static method => method.Name.StartsWith("Pop") ? [method.Name[3..]] : []),
    ];

    protected abstract Type TargetType { get; }
    protected abstract string ProjectDirectory { get; }

    protected virtual IReadOnlyList<Func<PairGenerator>> AdditionalGenerators => [];

    internal async Task GenerateAsync(DirectoryPath repository, CancellationToken cancellationToken)
    {
        var className = TargetType.Name + "Raii";
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

        var file = repository / ProjectDirectory / className.AsFileName("g.cs");
        await file.WriteAllTextAsync(codes, cancellationToken).ConfigureAwait(false);
    }
}