using TedToolkit.Hexa.Raii.Generator.Composition;

using Hexa.NET.ImGui;

namespace TedToolkit.Hexa.Raii.Generator;

/// <summary>
/// Generate the code with ImGui.
/// </summary>
internal sealed class ImGuiRaiiGenerator : RaiiGenerator
{
    protected override Type TargetType => typeof(ImGui);
    protected override string ProjectDirectory => "TedToolkit.Hexa.Raii.ImGui";

    protected override IReadOnlyList<Func<PairGenerator>> AdditionalGenerators { get; } =
    [
        () => new(
            static method => method.Name is "TreePush"
                ? "Tree"
                : method.Name.StartsWith("TreeNode")
                    ? method.Name
                    : "",
            static method => method.Name is "TreePop"
                ?
                [
                    "Tree",
                    "TreeNode",
                    "TreeNodeEx",
                    "TreeNodeV",
                    "TreeNodeExV",
                ]
                : []),

        () => new(
            static method => method.Name is "Indent" ? "Indent" : "",
            static method => method.Name is "Unindent" ? ["Indent"] : [])
    ];
}