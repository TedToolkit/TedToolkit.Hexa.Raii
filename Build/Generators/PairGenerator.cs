using System.Reflection;
using System.Runtime.InteropServices;

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace Build.Generators;

public readonly struct PairGenerator(Func<MethodInfo, string> begin, Func<MethodInfo, string[]> end)
{
    private readonly Dictionary<string, List<BeginGenerator>> _begins = [];
    private readonly Dictionary<string, List<EndGenerator>> _ends = [];

    private static void AddItem<T>(Dictionary<string, List<T>> dict, T item, string name)
    {
        ref var list = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, name, out var exists);
        if (exists && list is not null)
            list.Add(item);
        else
            list = [item];
    }

    public void AppendMethod(Type type, MethodInfo method)
    {
        var beginString = begin(method);
        if (!string.IsNullOrEmpty(beginString))
            AddItem(_begins, new BeginGenerator(method, type), beginString);

        foreach (var endString in end(method))
            AddItem(_ends, new EndGenerator(method, type), endString);
    }

    public void GenerateItems(TypeDeclaration declaration)
    {
        foreach (var (key, beginList) in _begins)
        {
            if (!_ends.TryGetValue(key, out var endList))
                continue;

            foreach (var beginGenerator in beginList)
            {
                beginGenerator.GenerateItem(declaration, endList, key);
            }
        }
    }
}