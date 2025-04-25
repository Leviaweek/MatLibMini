using BenchmarkDotNet.Attributes;
using MatLibMini;
using MatLibMini.Extensions;

[MemoryDiagnoser]
public class VectorBenchmark
{
    private const int Size = 1000000;
    private Vec<float> _vec1;
    private Vec<float> _vec2;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _vec1 = new Vec<float>(Size);
        _vec2 = new Vec<float>(Size);
        for (var i = 0; i < Size; i++)
        {
            var value = random.NextSingle();
            _vec1[i] = value;
            _vec2[i] = value;
        }
    }
    
    [Benchmark]
    public Vec<float> Add() => _vec1 + _vec2;
    
    [Benchmark]
    public Vec<float> Subtract() => _vec1 - _vec2;
    
    [Benchmark]
    public Vec<float> Multiply() => _vec1 * _vec2;
    
    [Benchmark]
    public Vec<float> AddScalar() => _vec1 + 1.0f;
    
    [Benchmark]
    public Vec<float> SubtractScalar() => _vec1 - 1.0f;
    
    [Benchmark]
    public Vec<float> MultiplyScalar() => _vec1 * 2.0f;
    
    [Benchmark]
    public float Sum() => _vec1.Sum();
}