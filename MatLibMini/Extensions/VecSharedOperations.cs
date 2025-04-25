using System.Numerics;

namespace MatLibMini.Extensions;

public static class VecSharedOperations
{
    public static Vec<T> Square<T>(this Vec<T> vector) where T : INumber<T>
    {
        Span<T> values = vector.Values;
        
        var i = 0;
        var vectorSize = Vector<T>.Count;
        
        for (; i <= vector.Length - vectorSize; i += vectorSize)
        {
            var vec = new Vector<T>(values.Slice(i, vectorSize));
            (vec * vec).CopyTo(values.Slice(i, vectorSize));
        }
        
        for (; i < vector.Length; i++)
            values[i] *= values[i];

        return vector;
    }
    
    public static Vec<T> Abs<T>(this Vec<T> vector) where T : INumber<T>
    {
        
        Span<T> values = vector.Values;
        
        var i = 0;
        var vectorSize = Vector<T>.Count;
        
        for (; i <= vector.Length - vectorSize; i += vectorSize)
        {
            var slice = values.Slice(i, vectorSize);
            var vec = new Vector<T>(slice);
            Vector.Abs(vec).CopyTo(slice);
        }
        
        for (; i < vector.Length; i++)
            values[i] = T.Abs(values[i]);
        
        return vector;
    }
}