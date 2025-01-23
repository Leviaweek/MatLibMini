using System.Numerics;

namespace KerasMini;

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

    public static void Add<T>(this Vec<T> vector1, Vec<T> vector2) where T : INumber<T>
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
    }
    
    public static void Add<T>(this Vec<T> vec, T scalar) where T : INumber<T>
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
    }
    
    public static void Multiply<T>(this Vec<T> vec, T scalar) where T : INumber<T>
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
    }
    
    public static void Multiply<T>(this Vec<T> vector1, Vec<T> vector2) where T : INumber<T>
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
    }
    
    private static void ValidateSameLength<T>(Vec<T> vector1, Vec<T> vector2) where T : INumber<T>
    {
        if (vector1.Length != vector2.Length)
            throw new ArgumentException("Vectors must have the same length.");
    }
}