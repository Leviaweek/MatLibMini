using System.Numerics;
using System.Text;
using MatLibMini.Extensions;

namespace MatLibMini;

public sealed class Mat<T> where T: unmanaged, INumber<T>
{
    internal readonly T[] Values;
    public int Width { get; }
    public int Height { get; }
    
    public unsafe Mat(T[,] values): this(values.GetLength(1), values.GetLength(0))
    {
        fixed (T* valuesPtr = values)
        {
            var valuesSpan = new ReadOnlySpan<T>(valuesPtr, Width * Height);
            valuesSpan.CopyTo(Values);
        }
    }
    
    public Mat(int width, int height)
    {
        Width = width;
        Height = height;
        Values = new T[Width * Height];
    }
    
    public Mat(Mat<T> matrix): this(matrix.Width, matrix.Height)
    {
        matrix.Values.CopyTo(Values.AsMemory());
    }
    
    public T this[int row, int column]
    {
        get => Values[row * Width + column];
        set => Values[row * Width + column] = value;
    }
    
    public bool Equals(Mat<T> obj)
    {
        return obj.Width == Width && obj.Height == Height && obj.Values.AsSpan().SequenceEqual(Values.AsSpan());
    }
    
    public static Mat<T> operator +(Mat<T> matrix1, Mat<T> matrix2) => matrix1.Clone().Add(matrix2);

    public static Mat<T> operator +(Mat<T> matrix1, T scalar) => matrix1.Clone().Add(scalar);

    public static Mat<T> operator *(Mat<T> matrix, T scalar) => matrix.Clone().Multiply(scalar);

    public static Mat<T> operator *(Mat<T> matrix1, Mat<T> matrix2) => matrix1.Clone().Multiply(matrix2);

    public static Mat<T> operator -(Mat<T> matrix1, Mat<T> matrix2) => matrix1.Clone().Subtract(matrix2);

    public static Mat<T> operator -(Mat<T> matrix, T scalar) => matrix.Clone().Subtract(scalar);

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                sb.Append(Values[i * Width + j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
    
    public Mat<T> Clone() => new(this);
}