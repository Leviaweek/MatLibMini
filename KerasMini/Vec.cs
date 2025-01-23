using System.Collections;
using System.Numerics;
using System.Text;

namespace KerasMini;

public sealed class Vec<T>: IEnumerable<T> where T: INumber<T>
{
    internal readonly T[] Values;
    
    public Vec(params ReadOnlySpan<T> values)
    {
        Values = new T[values.Length];
        values.CopyTo(Values);
    }
    
    public Vec(Vec<T> vector)
    {
        Values = new T[vector.Length];
        vector.Values.CopyTo(Values.AsMemory());
    }
    
    public Vec(IEnumerable<T> values)
    {
        Values = values.ToArray();
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

    public static Vec<T> operator +(Vec<T> vector1, Vec<T> vector2)
    {
        var result = new Vec<T>(vector1);
        result.Add(vector2);
        return result;
    }

    public static Vec<T> operator +(Vec<T> vector1, T scalar)
    {
        var result = new Vec<T>(vector1);
        result.Add(scalar);
        return result;
    }

    public static Vec<T> operator *(Vec<T> vec, T scalar)
    {
        var result = new Vec<T>(vec);
        
        result.Multiply(scalar);
        
        return result;
    }

    public static Vec<T> operator *(Vec<T> vector1, Vec<T> vector2)
    {
        var result = new Vec<T>(vector1);
        result.Multiply(vector2);
        return result;
    }


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