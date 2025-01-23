using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace KerasMini;

public static class MatBaseOperations
{
    public static void Add<T>(this Mat<T> matrix1, T scalar) where T : unmanaged, INumber<T>
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
    }
    
    public static void Add<T>(this Mat<T> matrix1, Mat<T> matrix2) where T : unmanaged, INumber<T>
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
    }

    public static void Multiply<T>(this Mat<T> matrix1, T scalar) where T : unmanaged, INumber<T>
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
    }
    
    public static Mat<T> Multiply<T>(this Mat<T> matrix1, Mat<T> matrix2) where T : unmanaged, INumber<T>
    {
        if (matrix1.Width != matrix2.Height)
            throw new ArgumentException("Matrix1 width must be equal to Matrix2 height.");

        var width = matrix2.Width;
        var height = matrix1.Height;
        var commonDim = matrix1.Width;
        var resultMatrix = new Mat<T>(width, height);

        ReadOnlySpan<T> values1Span = matrix1.Values;
        var resultSpan = resultMatrix.Values.AsSpan();

        ReadOnlySpan<T> transposedValues2 = matrix2.Transpose().Values;

        var vectorSize = Vector<T>.Count;

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var sumVector = Vector<T>.Zero;
                var k = 0;

                for (; k <= commonDim - vectorSize; k += vectorSize)
                {
                    var vec1 = new Vector<T>(values1Span.Slice(i * commonDim + k, vectorSize));
                    var vec2 = new Vector<T>(transposedValues2.Slice(j * commonDim + k, vectorSize));
                    sumVector += vec1 * vec2;
                }

                var scalarSum = Vector.Sum(sumVector);

                for (; k < commonDim; k++)
                {
                    scalarSum += values1Span[i * commonDim + k] * transposedValues2[j * commonDim + k];
                }

                resultSpan[i * width + j] = scalarSum;
            }
        }

        return resultMatrix;
    }


    public static Mat<T> Transpose<T>(this Mat<T> matrix) where T : unmanaged, INumber<T>
    {
        var resultMatrix = new Mat<T>(matrix.Width, matrix.Height);
        var data = resultMatrix.Values.AsSpan();
        ReadOnlySpan<T> inputData = matrix.Values;

        var width = matrix.Width;
        var height = matrix.Height;
        
        const int blockSize = 64;

        for (var i = 0; i < height; i += blockSize)
        {
            for (var j = 0; j < width; j += blockSize)
            {
                var maxI = Math.Min(i + blockSize, height);
                var maxJ = Math.Min(j + blockSize, width);

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


    
    private static void ValidateSameSize<T>(Mat<T> mat1, Mat<T> mat2) where T : unmanaged, INumber<T>
    {
        if (mat1.Width != mat2.Width || mat1.Height != mat2.Height)
            throw new ArgumentException("Vectors must have the same length.");
    }
}