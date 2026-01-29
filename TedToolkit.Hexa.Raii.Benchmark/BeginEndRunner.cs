using System.Numerics;

using BenchmarkDotNet.Attributes;

using Hexa.NET.ImGui;

namespace TedToolkit.Hexa.Raii.Benchmark;

[MemoryDiagnoser]
public class BeginEndRunner
{
    private ImGuiContextPtr _ctx;

    [GlobalSetup]
    public void Setup()
    {
        _ctx = ImGui.CreateContext();
        ImGui.SetCurrentContext(_ctx);
        var io = ImGui.GetIO();
        io.DisplaySize = new Vector2(1920, 1080);

        io.BackendFlags |= ImGuiBackendFlags.RendererHasTextures;
    }

    [Benchmark(Baseline = true)]
    public void RawRun()
    {
        ImGui.NewFrame();
        if (ImGui.Begin(nameof(RawRun)))
            ImGui.End();
        ImGui.EndFrame();
    }

    [Benchmark]
    public void RaiiRefStruct()
    {
        ImGui.NewFrame();
        using (EndRefStruct(nameof(RaiiRefStruct)))
        {

        }
        ImGui.EndFrame();
    }

    [Benchmark]
    public void RaiiStruct()
    {
        ImGui.NewFrame();
        using (EndRaiiStruct(nameof(RaiiStruct)))
        {

        }
        ImGui.EndFrame();
    }

    [Benchmark]
    public void RaiiStructInterface()
    {
        ImGui.NewFrame();
        using (EndRaiiStructDisposable(nameof(RaiiStructInterface)))
        {

        }
        ImGui.EndFrame();
    }

    [Benchmark]
    public void RaiiClass()
    {
        ImGui.NewFrame();
        using (EndRaiiClass(nameof(RaiiClass)))
        {

        }
        ImGui.EndFrame();
    }

    [Benchmark]
    public void RaiiClassInterface()
    {
        ImGui.NewFrame();
        using (EndRaiiClassDisposable(nameof(RaiiClassInterface)))
        {

        }
        ImGui.EndFrame();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        ImGui.DestroyContext(_ctx);
    }

    public readonly ref struct ImGuiEnd(bool succeed) :
        IDisposable
    {
        public void Dispose()
        {
            if (succeed)
                ImGui.End();
        }
    }

    private static ImGuiEnd EndRefStruct(string name)
    {
        return new ImGuiEnd(ImGui.Begin(name));
    }

    public readonly struct ImGuiEndStruct(bool succeed) :
        IDisposable
    {
        public void Dispose()
        {
            if (succeed)
                ImGui.End();
        }
    }

    public sealed class ImGuiEndClass(bool succeed) :
        IDisposable
    {
        public void Dispose()
        {
            if (succeed)
                ImGui.End();
        }
    }

    private static ImGuiEndStruct EndRaiiStruct(string name)
    {
        return new ImGuiEndStruct(ImGui.Begin(name));
    }

    private static IDisposable EndRaiiStructDisposable(string name)
    {
        return new ImGuiEndStruct(ImGui.Begin(name));
    }

    private static ImGuiEndClass EndRaiiClass(string name)
    {
        return new ImGuiEndClass(ImGui.Begin(name));
    }

    private static IDisposable EndRaiiClassDisposable(string name)
    {
        return new ImGuiEndClass(ImGui.Begin(name));
    }
}