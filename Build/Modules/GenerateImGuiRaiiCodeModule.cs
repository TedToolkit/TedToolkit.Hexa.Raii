using Build.Generators;

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

    protected override IReadOnlyList<Func<PairGenerator>> AdditionalGenerators { get; } =
    [
        () => new(
            static method => method.Name is "TreePush"
                ? "Tree"
                : method.Name.StartsWith("TreeNode")
                    ? method.Name
                    : "",
            static method => method.Name is "TreePop" ?
            [
                "Tree",
                "TreeNode",
                "TreeNodeEx",
                "TreeNodeV",
                "TreeNodeExV",
            ] : []),
    ];
}