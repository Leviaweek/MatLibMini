using System.Numerics;
using System.Runtime.CompilerServices;

namespace MatLibMini.Extensions;

public static class MatBaseOperations
{
    private const int BlockSize = 256;

    public static Mat<T> Add<T>(this Mat<T> matrix1, T scalar) where T : unmanaged, INumber<T>
    {
        Span<T> matrixSpan = matrix1.Values;

        var i = 0;
        var matrixSize = Vector<T>.Count;
        
        for (; i <= matrix1.Values.Length - matrixSize; i += matrixSize)
        {
            var vec = new Vector<T>(matrixSpan.Slice(i, matrixSize));
            var vec2 = new Vector<T>(scalar);
            (vec + vec2).CopyTo(matrixSpan.Slice(i, matrixSize));
        }
        
        for (; i < matrix1.Values.Length; i++)
            matrixSpan[i] += scalar;
        
        return matrix1;
    }
    
    public static Mat<T> Add<T>(this Mat<T> matrix1, Mat<T> matrix2) where T : unmanaged, INumber<T>
    {
        ValidateSameSize(matrix1, matrix2);
        
        Span<T> matrix1Span = matrix1.Values;
        ReadOnlySpan<T> matrix2Span = matrix2.Values;

        var i = 0;
        var matrixSize = Vector<T>.Count;
        
        for (; i <= matrix1.Values.Length - matrixSize; i += matrixSize)
        {
            var vec1 = new Vector<T>(matrix1Span.Slice(i, matrixSize));
            var vec2 = new Vector<T>(matrix2Span.Slice(i, matrixSize));
            (vec1 + vec2).CopyTo(matrix1Span.Slice(i, matrixSize));
        }
        
        for (; i < matrix1.Values.Length; i++)
            matrix1Span[i] += matrix2Span[i];
        
        return matrix1;
    }

    public static Mat<T> Multiply<T>(this Mat<T> matrix1, T scalar) where T : unmanaged, INumber<T>
    {
        Span<T> matrix1Span = matrix1.Values;
        var i = 0;
        var matrixSize = Vector<T>.Count;
        
        for (; i <= matrix1.Values.Length - matrixSize; i += matrixSize)
        {
            var vec = new Vector<T>(matrix1Span.Slice(i, matrixSize));
            var vec2 = new Vector<T>(scalar);
            (vec * vec2).CopyTo(matrix1Span.Slice(i, matrixSize));
        }
        
        for (; i < matrix1.Values.Length; i++)
            matrix1Span[i] *= scalar;
        
        return matrix1;
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static unsafe Mat<T> Multiply<T>(this Mat<T> matrix1, Mat<T> matrix2) where T : unmanaged, INumber<T>
    {
        if (matrix1.Width != matrix2.Height)
            throw new ArgumentException("Matrix1 width must be equal to Matrix2 height.");

        var width = matrix2.Width;
        var height = matrix1.Height;
        var commonDim = matrix1.Width;
        var resultMatrix = new Mat<T>(width, height);

        var vectorSize = Vector<T>.Count;
        var width2 = matrix2.Width;

        fixed (T* mat1Ptr = matrix1.Values)
        fixed (T* mat2Ptr = matrix2.Values)
        fixed (T* resPtr = resultMatrix.Values)
        {
            for (var i = 0; i < height; i++)
            {
                var rowRes = resPtr + i * width;
            
                for (var k = 0; k < commonDim; k++)
                {
                    var val1 = mat1Ptr[i * commonDim + k];
                    var row2 = mat2Ptr + k * width2;
                
                    var j = 0;
                
                    if (vectorSize > 1)
                    {
                        var vecVal1 = new Vector<T>(val1);
                        for (; j <= width - vectorSize; j += vectorSize)
                        {
                            var vec2 = Vector.Load(row2 + j);
                            var vecRes = Vector.Load(rowRes + j);
                            vecRes += vecVal1 * vec2;
                            vecRes.Store(rowRes + j);
                        }
                    }
                
                    for (; j < width; j++)
                    {
                        rowRes[j] += val1 * row2[j];
                    }
                }
            }
        }

        return resultMatrix;
    }
    

    public static Mat<T> Transpose<T>(this Mat<T> matrix) where T : unmanaged, INumber<T>
    {
        var resultMatrix = new Mat<T>(matrix.Height, matrix.Width);
        var data = resultMatrix.Values.AsSpan();
        ReadOnlySpan<T> inputData = matrix.Values;

        var width = matrix.Width;
        var height = matrix.Height;

        for (var i = 0; i < height; i += BlockSize)
        {
            for (var j = 0; j < width; j += BlockSize)
            {
                var maxI = Math.Min(i + BlockSize, height);
                var maxJ = Math.Min(j + BlockSize, width);

                for (var ii = i; ii < maxI; ii++)
                {
                    for (var jj = j; jj < maxJ; jj++)
                    {
                        data[jj * height + ii] = inputData[ii * width + jj];
                    }
                }
            }
        }
        return resultMatrix;
    }

    public static T Sum<T>(this Mat<T> matrix) where T : unmanaged, INumber<T>
    {
        ReadOnlySpan<T> values = matrix.Values;

        var sumVector = Vector<T>.Zero;
        
        var vectorSize = Vector<T>.Count;
        
        var i = 0;
        
        for (; i <= values.Length - vectorSize; i += vectorSize)
        {
            var vec1 = new Vector<T>(values.Slice(i, vectorSize));
            sumVector += vec1;
        }
        
        var sum = Vector.Sum(sumVector);
        
        for (; i < values.Length; i++)
            sum += values[i];
        
        return sum;
    }
    
    public static Mat<T> Subtract<T>(this Mat<T> matrix1, T scalar) where T : unmanaged, INumber<T>
    {
        Span<T> matrixSpan = matrix1.Values;

        var i = 0;
        var matrixSize = Vector<T>.Count;
        
        for (; i <= matrix1.Values.Length - matrixSize; i += matrixSize)
        {
            var vec = new Vector<T>(matrixSpan.Slice(i, matrixSize));
            var vec2 = new Vector<T>(scalar);
            (vec - vec2).CopyTo(matrixSpan.Slice(i, matrixSize));
        }
        
        for (; i < matrix1.Values.Length; i++)
            matrixSpan[i] -= scalar;
        
        return matrix1;
    }
    
    public static Mat<T> Subtract<T>(this Mat<T> matrix1, Mat<T> matrix2) where T : unmanaged, INumber<T>
    {
        ValidateSameSize(matrix1, matrix2);
        
        Span<T> matrix1Span = matrix1.Values;
        ReadOnlySpan<T> matrix2Span = matrix2.Values;

        var i = 0;
        var matrixSize = Vector<T>.Count;
        
        for (; i <= matrix1.Values.Length - matrixSize; i += matrixSize)
        {
            var vec1 = new Vector<T>(matrix1Span.Slice(i, matrixSize));
            var vec2 = new Vector<T>(matrix2Span.Slice(i, matrixSize));
            (vec1 - vec2).CopyTo(matrix1Span.Slice(i, matrixSize));
        }
        
        for (; i < matrix1.Values.Length; i++)
            matrix1Span[i] -= matrix2Span[i];
        
        return matrix1;
    }
    
    public static Vec<T> Flatten<T>(this Mat<T> matrix) where T: unmanaged, INumber<T> => Vec<T>.From(matrix.Values);

    private static void ValidateSameSize<T>(Mat<T> mat1, Mat<T> mat2) where T : unmanaged, INumber<T>
    {
        if (mat1.Width != mat2.Width || mat1.Height != mat2.Height)
            throw new ArgumentException("Vectors must have the same length.");
    }
}