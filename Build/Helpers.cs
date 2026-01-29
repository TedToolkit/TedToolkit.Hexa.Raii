using System.Reflection;

using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace Build;

internal static class Helpers
{
    public static InvocationExpression StaticInvoke(this MethodInfo methodInfo, Type type)
    {
        return new AliasExpression("global", DataType.FromType(type).Type)
            .Sub(methodInfo.Name).Invoke();
    }
}