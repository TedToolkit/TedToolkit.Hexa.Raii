using System.Reflection;
using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    Build.Generators.BeginGenerator>;

namespace Build.Generators;

internal readonly struct BeginGenerator(MethodInfo methodInfo, Type type, bool endAlways)
{
    public void GenerateItem(TypeDeclaration declaration, IReadOnlyList<EndGenerator> endGenerators, string name)
    {
        var methodParameters = methodInfo.GetParameters();

        var (endGenerator, index) = endGenerators
            .Select((endGenerator, index) => (endGenerator, index))
            .Where(pair => pair.endGenerator.ParameterInfos.All(info1 =>
                methodParameters.Any(info2 => info1.ParameterType == info2.ParameterType && info1.Name == info2.Name)))
            .OrderByDescending(pair => pair.endGenerator.ParameterInfos.Length)
            .First();

        var needAddSucceed = methodInfo.ReturnType != typeof(void);
        var isResultItem = needAddSucceed && methodInfo.ReturnType != typeof(bool);
        var typeName = isResultItem
            ? endGenerator.GenerateResultItem(declaration, name, index)
            : endGenerator.GenerateSucceedItem(declaration, name, index, endAlways);

        var method = Method(name, new(isResultItem
                ? new DataType(typeName).Generic(DataType.FromType(methodInfo.ReturnType))
                : new DataType(typeName))).Static.Public
            .AddAttribute(Attribute<MethodImplAttribute>()
                .AddArgument(Argument(MethodImplOptions.AggressiveInlining.ToExpression())));
        var invocation = methodInfo.StaticInvoke(type);

        foreach (var parameterInfo in methodParameters)
        {
            method.AddParameter(Parameter(parameterInfo));
            invocation.AddArgument(Argument(parameterInfo));
        }

        method.AddParameter(Parameter(DataType.Bool, "enable").AddDefault("true".ToSimpleName()))
            .AddStatement("!enable".ToSimpleName().If
                .AddStatement("default".ToSimpleName().Return));

        var creation = new ObjectCreationExpression();
        foreach (var endGeneratorParameterInfo in endGenerator.ParameterInfos)
        {
            var parameter = methodParameters.First(p =>
                p.ParameterType == endGeneratorParameterInfo.ParameterType
                && p.Name == endGeneratorParameterInfo.Name);
            creation.AddArgument(Argument(parameter));
        }

        creation.AddArgument(Argument(needAddSucceed
            ? "succeed".ToSimpleName()
            : "true".ToSimpleName()));

        if (endAlways)
            creation.AddArgument(Argument("true".ToSimpleName()));

        declaration.AddMember(method
            .AddStatement(needAddSucceed
                ? new VariableExpression(DataType.Var, "succeed").Operator("=",
                    invocation)
                : invocation)
            .AddStatement(creation.Return));
    }
}