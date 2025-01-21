using System.Numerics;

namespace Keracocks;

public static class VectorOperations
{
    public static T Dot<T>(this Vec<T> vector1, Vec<T> vector2) where T: INumber<T>
    {
        Vec<T>.ValidateSameLength(vector1, vector2);

        var result = T.Zero;

        ReadOnlySpan<T> span1 = vector1.Values;
        ReadOnlySpan<T> span2 = vector2.Values;
        
        for (var i = 0; i < vector1.Length; i++)
            result += span1[i] * span2[i];

        return result;
    }

    public static Vec<T> Add<T>(this Vec<T> vector1, Vec<T> vector2) where T : INumber<T>
    {
        Vec<T>.ValidateSameLength(vector1, vector2);
        var result = new Vec<T>(vector1.Length);
        
        Span<T> resultSpan = result.Values;
        ReadOnlySpan<T> span1 = vector1.Values;
        ReadOnlySpan<T> span2 = vector2.Values;

        for (var i = 0; i < vector1.Length; i++)
            resultSpan[i] = span1[i] + span2[i];

        return result;
    }
    
    public static Vec<T> Add<T>(this Vec<T> vec, T scalar) where T : INumber<T>
    {
        var result = new Vec<T>(vec.Length);
        
        Span<T> resultSpan = result.Values;
        ReadOnlySpan<T> span = vec.Values;

        for (var i = 0; i < vec.Length; i++)
            resultSpan[i] = span[i] + scalar;

        return result;
    }
    
    public static Vec<T> Multiply<T>(this Vec<T> vec, T scalar) where T : INumber<T>
    {
        var result = new Vec<T>(vec.Length);
        
        Span<T> resultSpan = result.Values;
        ReadOnlySpan<T> span = vec.Values;

        for (var i = 0; i < vec.Length; i++)
            resultSpan[i] = span[i] * scalar;

        return result;
    }
    
    public static Vec<T> Multiply<T>(this Vec<T> vector1, Vec<T> vector2) where T : INumber<T>
    {
        Vec<T>.ValidateSameLength(vector1, vector2);

        var result = new Vec<T>(vector1.Length);
        
        Span<T> resultSpan = result.Values;
        ReadOnlySpan<T> span1 = vector1.Values;
        ReadOnlySpan<T> span2 = vector2.Values;

        for (var i = 0; i < vector1.Length; i++)
            resultSpan[i] = span1[i] * span2[i];

        return result;
    }
}