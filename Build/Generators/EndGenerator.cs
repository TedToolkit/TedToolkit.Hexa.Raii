using System.Reflection;
using System.Runtime.CompilerServices;

using Cysharp.Text;

using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    Build.Generators.EndGenerator>;

namespace Build.Generators;

internal sealed class EndGenerator(MethodInfo methodInfo, Type type)
{
    private bool _isGeneratedSucceed;
    private bool _isGeneratedResult;

    public ParameterInfo[] ParameterInfos { get; } = methodInfo.GetParameters();

    public string GenerateResultItem(TypeDeclaration declaration, string name, int index)
    {
        var structName = GetStructName(name, index);
        if (_isGeneratedResult)
            return structName;
        _isGeneratedResult = true;

        var resultType = new DataType("TResult");

        var structDeclaration = RefStruct(structName).Public.Readonly
            .AddBaseType<IDisposable>()
            .AddTypeParameter(TypeParameter("TResult"))
            .AddMember(Property(resultType, "Result").Public
                .AddAccessor(Accessor(AccessorType.GET))
                .AddDefault("result".ToSimpleName()))
            .AddMember(ImplicitConversionTo(resultType)
                .AddStatement("value.Result".ToSimpleName().Return)
                .AddAttribute(Attribute<MethodImplAttribute>()
                    .AddArgument(Argument(MethodImplOptions.AggressiveInlining.ToExpression()))));

        var invocation = GenerateInvocations(structDeclaration);

        declaration.AddMember(structDeclaration
            .AddParameter(Parameter(resultType, "result"))
            .AddMember(Method(nameof(IDisposable.Dispose)).Public
                .AddAttribute(Attribute<MethodImplAttribute>()
                    .AddArgument(Argument(MethodImplOptions.AggressiveInlining.ToExpression())))
                .AddStatement(invocation)));

        return structName;
    }

    public string GenerateSucceedItem(TypeDeclaration declaration, string name, int index)
    {
        var structName = GetStructName(name, index);
        if (_isGeneratedSucceed)
            return structName;
        _isGeneratedSucceed = true;

        var structDeclaration = RefStruct(structName).Public.Readonly
            .AddBaseType<IDisposable>()
            .AddMember(Property(DataType.Bool, "Succeed").Public
                .AddAccessor(Accessor(AccessorType.GET))
                .AddDefault("succeed".ToSimpleName()))
            .AddMember(Operator(new(DataType.Bool), "true")
                .AddAttribute(Attribute<MethodImplAttribute>()
                    .AddArgument(Argument(MethodImplOptions.AggressiveInlining.ToExpression())))
                .AddParameter(Parameter(new DataType(structName), "value"))
                .AddStatement("@value.Succeed".ToSimpleName().Return))
            .AddMember(Operator(new(DataType.Bool), "false")
                .AddAttribute(Attribute<MethodImplAttribute>()
                    .AddArgument(Argument(MethodImplOptions.AggressiveInlining.ToExpression())))
                .AddParameter(Parameter(new DataType(structName), "value"))
                .AddStatement("!@value.Succeed".ToSimpleName().Return));

        var invocation = GenerateInvocations(structDeclaration);

        declaration.AddMember(structDeclaration
            .AddParameter(Parameter(DataType.Bool, "succeed").AddDefault("true".ToSimpleName()))
            .AddMember(Method(nameof(IDisposable.Dispose)).Public
                .AddAttribute(Attribute<MethodImplAttribute>()
                    .AddArgument(Argument(MethodImplOptions.AggressiveInlining.ToExpression())))
                .AddStatement("Succeed".ToSimpleName().If
                    .AddStatement(invocation))));
        return structName;
    }

    private InvocationExpression GenerateInvocations(TypeDeclaration structDeclaration)
    {
        var invocation = methodInfo.StaticInvoke(type);

        foreach (var parameterInfo in ParameterInfos)
        {
            structDeclaration.AddParameter(Parameter(parameterInfo, "global"));
            if (parameterInfo.ParameterType.IsByRef)
            {
                var fieldName = ZString.Concat('_', parameterInfo.Name);
                structDeclaration.AddMember(Field(new DataType("void*"), fieldName).Private.Readonly
                    .AddDefault("System.Runtime.CompilerServices.Unsafe.AsPointer".ToSimpleName().Invoke()
                        .AddArgument(Argument(parameterInfo))));

                invocation.AddArgument(Argument("System.Runtime.CompilerServices.Unsafe.AsRef".ToSimpleName()
                    .Generic(DataType.FromType(parameterInfo.ParameterType, "global"))
                    .Invoke().AddArgument(Argument(fieldName.ToSimpleName())).Ref));
            }
            else
            {
                invocation.AddArgument(Argument(parameterInfo));
            }
        }

        return invocation;
    }

    private static string GetStructName(string name, int index)
        => ZString.Concat(name, "Disposable", index);
}