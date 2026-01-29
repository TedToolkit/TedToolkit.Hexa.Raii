// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;

using TedToolkit.Hexa.Raii.Benchmark;

Console.WriteLine("Hello, World!");

BenchmarkRunner.Run<BeginEndRunner>();