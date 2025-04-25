using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KerasMini;

public static class DataGenerator
{
    public static (Mat<float> x, Vec<float> y) GenerateRegression(int dataCount, int features, float noise, int randomSeed)
    {
        var random = new Random(randomSeed);
        var x = new Mat<float>(dataCount, features);
        var y = new Vec<float>(dataCount);
        
        var xSpan = x.Values.AsSpan();
        var ySpan = y.Values.AsSpan();
        
        Span<float> coeffs = new float[features];
        
        FillRandom(coeffs, random);
        
        for (var i = 0; i < dataCount; i++)
        {
            FillRandom(xSpan.Slice(i * features, features), random);
            
            for (var j = 0; j < features; j++)
                ySpan[i] += xSpan[i * features + j] * coeffs[j];
            
            if (noise == 0)
                continue;
            ySpan[i] += random.NextSingle() + NextGaussian(random, 0, noise);
        }
        
        return (x, y);
    }
    
    private static void FillRandom(Span<float> span, Random random)
    {
        for (var i = 0; i < span.Length; i++)
            span[i] = random.NextSingle() * 2 - 1;
    }
    
    private static float NextGaussian(Random random, float mean = 0, float stdDev = 1)
    {
        var u1 = 1.0 - random.NextDouble();
        var u2 = 1.0 - random.NextDouble();
        var randStdNormal = (float)Math.Sqrt(-2.0 * Math.Log(u1)) *
                            (float)Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}