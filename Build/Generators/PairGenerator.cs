using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace Build.Generators;

internal readonly struct PairGenerator
{
    private readonly Dictionary<string, List<BeginGenerator>> _begins = [];
    private readonly Dictionary<string, List<BeginGenerator>> _pushes = [];
    private readonly Dictionary<string, List<EndGenerator>> _ends = [];
    private readonly Dictionary<string, List<EndGenerator>> _pops = [];

    public PairGenerator(Type type)
    {
        foreach (var runtimeMethod in type.GetRuntimeMethods()
                     .Where(m => m.IsPublic))
        {
            var name = runtimeMethod.Name;
            if (name.StartsWith("Begin"))
                AddItem(_begins, new BeginGenerator(runtimeMethod, type), name[5..]);
            else if (name.StartsWith("Push"))
                AddItem(_pushes, new BeginGenerator(runtimeMethod, type), name[4..]);
            else if (name.StartsWith("End"))
                AddItem(_ends, new EndGenerator(runtimeMethod, type), name[3..]);
            else if (name.StartsWith("Pop"))
                AddItem(_pops, new EndGenerator(runtimeMethod, type), name[3..]);
        }
    }

    private static void AddItem<T>(Dictionary<string, List<T>> dict, T item, string name)
    {
        ref var list = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, name, out var exists);
        if (exists && list is not null)
            list.Add(item);
        else
            list = [item];
    }

    private static void GenerateItems(TypeDeclaration declaration,
        Dictionary<string, List<BeginGenerator>> begins, Dictionary<string, List<EndGenerator>> ends)
    {
        foreach (var (key, beginList) in begins)
        {
            if (!ends.TryGetValue(key, out var endList))
                continue;

            var name = string.IsNullOrEmpty(key) ? "Body" : key;

            foreach (var beginGenerator in beginList)
            {
                beginGenerator.GenerateItem(declaration, endList, name);
            }
        }
    }

    public void GenerateItems(TypeDeclaration declaration)
    {
        GenerateItems(declaration, _begins, _ends);
        GenerateItems(declaration, _pushes, _pops);
    }
}