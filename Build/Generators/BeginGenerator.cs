using System.Reflection;
using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    Build.Generators.BeginGenerator>;

namespace Build.Generators;

internal readonly struct BeginGenerator(MethodInfo methodInfo, Type type)
{
    public void GenerateItem(TypeDeclaration declaration, IReadOnlyList<EndGenerator> endGenerators, string name)
    {
        var methodParameters = methodInfo.GetParameters();

        var parameterTypes = methodParameters.Select(i => i.ParameterType).ToArray();
        var (endGenerator, index) = endGenerators
            .Select((endGenerator, index) => (endGenerator, index))
            .Where(pair => pair.endGenerator.ParameterInfos.All(info => parameterTypes.Contains(info.ParameterType)))
            .OrderByDescending(pair => pair.endGenerator.ParameterInfos.Length)
            .FirstOrDefault();

        if (endGenerator is null)
        {
            declaration.AddMember(Method(methodInfo.ToString()));
            return;
        }

        var needAddSucceed = methodInfo.ReturnType != typeof(void);
        var isResultItem = needAddSucceed && methodInfo.ReturnType != typeof(bool);
        var typeName = isResultItem
            ? endGenerator.GenerateResultItem(declaration, name, index)
            : endGenerator.GenerateSucceedItem(declaration, name, index);

        var method = Method(name, new(isResultItem
                ? new DataType(typeName).Generic(DataType.FromType(methodInfo.ReturnType, "global"))
                : new DataType(typeName))).Static.Public
            .AddAttribute(Attribute<MethodImplAttribute>()
                .AddArgument(Argument(MethodImplOptions.AggressiveInlining.ToExpression())));
        var invocation = methodInfo.StaticInvoke(type);

        foreach (var parameterInfo in methodParameters)
        {
            method.AddParameter(Parameter(parameterInfo, "global"));
            invocation.AddArgument(Argument(parameterInfo));
        }

        var creation = new ObjectCreationExpression();
        foreach (var endGeneratorParameterInfo in endGenerator.ParameterInfos)
        {
            var parameter = methodParameters.First(p => p.ParameterType == endGeneratorParameterInfo.ParameterType);
            creation.AddArgument(Argument(parameter));
        }

        if (needAddSucceed)
        {
            creation.AddArgument(Argument("succeed".ToSimpleName()));
        }

        declaration.AddMember(method
            .AddStatement(needAddSucceed
                ? new VariableExpression(DataType.Var, "succeed").Operator("=",
                    invocation)
                : invocation)
            .AddStatement(creation.Return));
    }
}