using System.Numerics;
using System.Text;

namespace Keracocks;

public sealed class Vec<T> where T: INumber<T>
{
    internal readonly T[] Values;
    
    public Vec(params T[] values)
    {
        Values = new T[values.Length];
        values.CopyTo(Values.AsMemory());
    }
    
    public Vec(IEnumerable<T> values) { Values = values.ToArray(); }
    
    public Vec(int size)
    {
        Values = new T[size];
        
        for (var i = 0; i < size; i++)
            Values[i] = T.Zero;
    }
    
    public int Length => Values.Length;
    
    public T this[int index]
    {
        get => Values[index];
        set => Values[index] = value;
    }

    internal static void ValidateSameLength(Vec<T> vector1, Vec<T> vector2)
    {
        if (vector1.Length != vector2.Length)
            throw new ArgumentException("Vectors must have the same length.");
    }
    

    public static Vec<T> operator +(Vec<T> vector1, Vec<T> vector2) => vector1.Add(vector2);
    
    public static Vec<T> operator +(Vec<T> vector1, T scalar) => vector1.Add(scalar);

    public static Vec<T> operator *(Vec<T> vec, T scalar) => vec.Multiply(scalar);

    public static Vec<T> operator *(Vec<T> vector1, Vec<T> vector2) => vector1.Multiply(vector2);
    

    public override string ToString()
    {
        const int bracesCount = 2;
        var commas = Values.Length - 1;
        var spaces = Values.Length - 1;
        
        var sb = new StringBuilder(bracesCount + commas + spaces + Values.Length);
        
        sb.Append('[');
        
        for (var i = 0; i < Values.Length; i++)
        {
            sb.Append(Values[i]);
            
            if (i < commas)
                sb.Append(", ");
        }
        
        sb.Append(']');
        
        return sb.ToString();
    }
}