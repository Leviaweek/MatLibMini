using System.Collections;
using System.Numerics;
using System.Text;
using KerasMini.Extensions;

namespace KerasMini;

public sealed class Vec<T>: IEnumerable<T> where T: INumber<T>
{
    internal readonly T[] Values;
    
    private Vec(ReadOnlySpan<T> values)
    {
        Values = new T[values.Length];
        values.CopyTo(Values);
    }

    public Vec(int size)
    {
        Values = new T[size];
        Values.AsSpan().Fill(T.Zero);
    }
    
    public int Length => Values.Length;
    
    public T this[int index]
    {
        get => Values[index];
        set => Values[index] = value;
    }

    public static Vec<T> operator +(Vec<T> vector1, Vec<T> vector2) => vector1.Clone().Add(vector2);

    public static Vec<T> operator +(Vec<T> vector1, T scalar) => vector1.Clone().Add(scalar);

    public static Vec<T> operator *(Vec<T> vec, T scalar) => vec.Clone().Multiply(scalar);

    public static Vec<T> operator *(Vec<T> vector1, Vec<T> vector2) => vector1.Clone().Multiply(vector2);

    public static Vec<T> operator -(Vec<T> vector1, Vec<T> vector2) => vector1.Clone().Subtract(vector2);

    public static Vec<T> operator -(Vec<T> vector1, T scalar) => vector1.Clone().Add(scalar);


    public IEnumerator<T> GetEnumerator()
    {
        return new VecEnumerator<T>(this);
    }

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

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public Span<T> AsSpan() => Values;
    public Vec<T> Clone() => new(Values);
    
    public static Vec<T> From(params ReadOnlySpan<T> span) => new(span);
    public static Vec<T> From(Vec<T> vector) => new(vector.Values);
}

public sealed class VecEnumerator<T>: IEnumerator<T> where T: INumber<T>
{
    private readonly Vec<T> _vector;
    private int _index = -1;
    
    public VecEnumerator(Vec<T> vector)
    {
        _vector = vector;
    }
    
    public T Current => _vector[_index];
    
    object IEnumerator.Current => Current;
    
    public void Dispose()
    { }
    
    public bool MoveNext()
    {
        _index++;
        return _index < _vector.Length;
    }
    
    public void Reset()
    {
        _index = -1;
    }
}