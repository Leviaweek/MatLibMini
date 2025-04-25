using BenchmarkDotNet.Attributes;
using MatLibMini;
using MatLibMini.Extensions;

[MemoryDiagnoser]
public class MatrixBenchmark
{
    private const int Size = 1000;
    private Mat<float> _mat1;
    private Mat<float> _mat2;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _mat1 = new Mat<float>(Size, Size);
        _mat2 = new Mat<float>(Size, Size);
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                var value = random.NextSingle();
                _mat1[i, j] = value;
                _mat2[i, j] = value;
            }
        }
    }
    
    [Benchmark]
    public Mat<float> Add() => _mat1 + _mat2;
    
    [Benchmark]
    public Mat<float> Subtract() => _mat1 - _mat2;
    
    [Benchmark]
    public Mat<float> Multiply() => _mat1 * _mat2;
    
    [Benchmark]
    public Mat<float> AddScalar() => _mat1 + 1.0f;
    
    [Benchmark]
    public Mat<float> SubtractScalar() => _mat1 - 1.0f;
    
    [Benchmark]
    public Mat<float> MultiplyScalar() => _mat1 * 2.0f;
    
    [Benchmark]
    public float Sum() => _mat1.Sum();
}