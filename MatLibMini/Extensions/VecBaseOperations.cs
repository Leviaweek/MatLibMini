using System.Numerics;

namespace MatLibMini.Extensions;

public static class VecBaseOperations
{
    public static T Dot<T>(this Vec<T> vector1, Vec<T> vector2) where T: INumber<T>
    {
        ValidateSameLength(vector1, vector2);

        var result = T.Zero;

        ReadOnlySpan<T> span1 = vector1.Values;
        ReadOnlySpan<T> span2 = vector2.Values;

        var i = 0;
        var vectorSize = Vector<T>.Count;
        
        for (; i <= vector1.Length - vectorSize; i += vectorSize)
        {
            var vec1 = new Vector<T>(span1.Slice(i, vectorSize));
            var vec2 = new Vector<T>(span2.Slice(i, vectorSize));
            result += Vector.Dot(vec1, vec2);
        }
        
        for (; i < vector1.Length; i++)
            result += span1[i] * span2[i];

        return result;
    }

    public static Vec<T> Add<T>(this Vec<T> vector1, Vec<T> vector2) where T : INumber<T>
    {
        ValidateSameLength(vector1, vector2);
        
        Span<T> vector1Span = vector1.Values;
        ReadOnlySpan<T> vector2Span = vector2.Values;

        var i = 0;
        var vectorSize = Vector<T>.Count;
        
        for (; i <= vector1.Length - vectorSize; i += vectorSize)
        {
            var vec1 = new Vector<T>(vector1Span.Slice(i, vectorSize));
            var vec2 = new Vector<T>(vector2Span.Slice(i, vectorSize));
            (vec1 + vec2).CopyTo(vector1Span.Slice(i, vectorSize));
        }
        
        for (; i < vector1.Length; i++)
            vector1Span[i] += vector2Span[i];
        
        return vector1;
    }
    
    public static Vec<T> Add<T>(this Vec<T> vec, T scalar) where T : INumber<T>
    {
        Span<T> vectorSpan = vec.Values;

        var i = 0;
        var vectorSize = Vector<T>.Count;
        
        for (; i <= vec.Length - vectorSize; i += vectorSize)
        {
            var vec1 = new Vector<T>(vectorSpan.Slice(i, vectorSize));
            var vec2 = new Vector<T>(scalar);
            (vec1 + vec2).CopyTo(vectorSpan.Slice(i, vectorSize));
        }
        
        for (; i < vec.Length; i++)
            vectorSpan[i] += scalar;
        
        
        return vec;
    }
    
    public static Vec<T> Multiply<T>(this Vec<T> vec, T scalar) where T : INumber<T>
    {
        Span<T> vecSpan = vec.Values;

        var i = 0;
        var vectorSize = Vector<T>.Count;
        
        for (; i <= vec.Length - vectorSize; i += vectorSize)
        {
            var vec1 = new Vector<T>(vecSpan.Slice(i, vectorSize));
            var vec2 = new Vector<T>(scalar);
            (vec1 * vec2).CopyTo(vecSpan.Slice(i, vectorSize));
        }
        
        for (; i < vec.Length; i++)
            vecSpan[i] *= scalar;
        
        return vec;
    }
    
    public static Vec<T> Multiply<T>(this Vec<T> vector1, Vec<T> vector2) where T : INumber<T>
    {
        ValidateSameLength(vector1, vector2);
        
        Span<T> vector1Span = vector1.Values;
        ReadOnlySpan<T> vector2Span = vector2.Values;

        var i = 0;
        var vectorSize = Vector<T>.Count;
        
        for (; i <= vector1.Length - vectorSize; i += vectorSize)
        {
            var vec1 = new Vector<T>(vector1Span.Slice(i, vectorSize));
            var vec2 = new Vector<T>(vector2Span.Slice(i, vectorSize));
            (vec1 * vec2).CopyTo(vector1Span.Slice(i, vectorSize));
        }
        
        for (; i < vector1.Length; i++)
            vector1Span[i] *= vector2Span[i];

        return vector1;
    }
    
    public static T Min<T>(this Vec<T> vec) where T : INumber<T>
    {
        ReadOnlySpan<T> values = vec.Values;
        var min = values[0];
        
        for (var i = 1; i < vec.Length; i++)
            min = T.Min(min, values[i]);
        
        return min;
    }
    
    public static T Max<T>(this Vec<T> vec) where T : INumber<T>
    {
        ReadOnlySpan<T> values = vec.Values;
        var max = values[0];
        
        for (var i = 1; i < vec.Length; i++)
            max = T.Max(max, values[i]);
        
        return max;
    }
    
    public static T Mean<T>(this Vec<T> vec) where T : INumber<T>
    {
        if (vec.Length == 0) return T.Zero;

        var sum = vec.Sum();
        
        var result = sum / T.CreateSaturating(vec.Length);

        
        if (!T.IsNaN(result))
            return result;
        
        return sum / T.CreateTruncating(vec.Length);
    }

    public static T Sum<T>(this Vec<T> vec) where T : INumber<T>
    {
        ReadOnlySpan<T> values = vec.Values;
        var sumVector = Vector<T>.Zero;
        
        var vectorSize = Vector<T>.Count;
        
        var i = 0;
        
        for (; i <= vec.Length - vectorSize; i += vectorSize)
        {
            var vec1 = new Vector<T>(values.Slice(i, vectorSize));
            sumVector += vec1;
        }
        
        var sum = Vector.Sum(sumVector);
        
        for (; i < vec.Length; i++)
            sum += values[i];
        
        return sum;
    }
    
    public static Vec<T> Subtract<T>(this Vec<T> vector1, Vec<T> vector2) where T : INumber<T>
    {
        ValidateSameLength(vector1, vector2);
        
        ReadOnlySpan<T> vector2Span = vector2.Values;
        Span<T> vector1Span = vector1.Values;
        
        var i = 0;
        var vectorSize = Vector<T>.Count;
        
        for (; i <= vector1.Length - vectorSize; i += vectorSize)
        {
            var vec1 = new Vector<T>(vector1Span.Slice(i, vectorSize));
            var vec2 = new Vector<T>(vector2Span.Slice(i, vectorSize));
            (vec1 - vec2).CopyTo(vector1Span.Slice(i, vectorSize));
        }
        
        for (; i < vector1.Length; i++)
            vector1Span[i] -= vector2Span[i];
        
        return vector1;
    }
    
    public static Vec<T> Subtract<T>(this Vec<T> vector1, T scalar) where T : INumber<T>
    {
        Span<T> vector1Span = vector1.Values;
        
        var i = 0;
        var vectorSize = Vector<T>.Count;
        
        for (; i <= vector1.Length - vectorSize; i += vectorSize)
        {
            var vec1 = new Vector<T>(vector1Span.Slice(i, vectorSize));
            var vec2 = new Vector<T>(scalar);
            (vec1 - vec2).CopyTo(vector1Span.Slice(i, vectorSize));
        }
        
        for (; i < vector1.Length; i++)
            vector1Span[i] -= scalar;
        
        return vector1;
    }
    
    private static void ValidateSameLength<T>(Vec<T> vector1, Vec<T> vector2) where T : INumber<T>
    {
        if (vector1.Length != vector2.Length)
            throw new ArgumentException("Vectors must have the same length.");
    }
}