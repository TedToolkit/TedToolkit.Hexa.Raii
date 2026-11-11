using System.Reflection;

using TedToolkit.RoslynHelper;
using TedToolkit.RoslynHelper.Syntaxes;

namespace Build;

internal static class Helpers
{
    public static InvocationExpression StaticInvoke(this MethodInfo methodInfo, Type type)
    {
        return DataType.FromType(type).Type
            .Sub(methodInfo.Name).Invoke();
    }
}