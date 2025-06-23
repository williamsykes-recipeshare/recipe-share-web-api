``` ini

BenchmarkDotNet=v0.13.5, OS=ubuntu 20.04
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=8.0.404
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  Job-JEOVOY : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2

IterationCount=500  LaunchCount=1  WarmupCount=1  

```
|                       Method |    Mean |    Error |   StdDev |   Median | Allocated |
|----------------------------- |--------:|---------:|---------:|---------:|----------:|
| SequentialGetRecipes500Times | 1.009 s | 0.0209 s | 0.1377 s | 0.9566 s | 137.68 KB |
