# MatLibMini

**MatLibMini** is a minimalist and high-performance C# library for working with matrices and vectors. It supports generic numeric types (`T : unmanaged, INumber<T>`) and SIMD optimizations for common operations such as addition, multiplication, transposition, and more.

---

## 🚀 Features

- ✅ Generic numeric type support: `int`, `float`, `double`, etc.
- ✅ SIMD acceleration using `System.Numerics.Vector<T>`
- ✅ Core matrix operations:
    - Addition (with scalar or another matrix)
    - Multiplication (with scalar or another matrix)
    - Transposition
    - Flattening to vector
- ✅ Vector operations:
    - Dot product
    - Arithmetic operations (add, subtract, multiply)
- ✅ Extension-based modular design
- ✅ No dependencies, easy integration

---

## 📦 Installation

You can either clone the repo or add it as a submodule:

```bash
git submodule add https://github.com/your-username/MatLibMini.git
```

Or reference the project directly in your .csproj file:

```xml
<ProjectReference Include="MatLibMini/MatLibMini.csproj" />
```

## 🧠 Example Usage

### Matrix Operations

```csharp
using MatLibMini;
using MatLibMini.Extensions;

var matA = new Mat<float>(new float[,] {
    {1, 2},
    {3, 4}
});

var matB = new Mat<float>(new float[,] {
    {5, 6},
    {7, 8}
});

matA.Add(matB); // matA += matB

var result = matA.Multiply(matB.Transpose());

Console.WriteLine(result);
```

### Vector Operations

```csharp
var vec1 = Vec<float>.From(stackalloc float[] {1, 2, 3});
var vec2 = Vec<float>.From(stackalloc float[] {4, 5, 6});

var dot = vec1.Dot(vec2); // 1*4 + 2*5 + 3*6 = 32
Console.WriteLine($"Dot product: {dot}");
```

## Benchmarks

1.000x1.000 matrix benchmark results (using BenchmarkDotNet):
```
| Method         | Mean         | Error       | StdDev      | Median       | Gen0     | Gen1     | Gen2     | Allocated |
|--------------- |-------------:|------------:|------------:|-------------:|---------:|---------:|---------:|----------:|
| Add            |   1,612.7 us |    29.33 us |    54.36 us |   1,609.4 us | 152.3438 | 152.3438 | 152.3438 | 4000158 B |
| Subtract       |   1,591.9 us |    34.44 us |    96.57 us |   1,591.3 us | 152.3438 | 152.3438 | 152.3438 | 4000158 B |
| Multiply       | 134,622.0 us | 2,513.08 us | 4,901.58 us | 132,890.0 us | 250.0000 | 250.0000 | 250.0000 | 8000464 B |
| AddScalar      |   1,589.8 us |    45.76 us |   132.76 us |   1,551.9 us | 152.3438 | 152.3438 | 152.3438 | 4000158 B |
| SubtractScalar |   1,511.7 us |    29.85 us |    49.88 us |   1,494.1 us | 152.3438 | 152.3438 | 152.3438 | 4000157 B |
| MultiplyScalar |   1,485.6 us |    28.22 us |    32.50 us |   1,491.9 us | 152.3438 | 152.3438 | 152.3438 | 4000158 B |
| Sum            |     156.9 us |     3.02 us |     3.93 us |     156.1 us |        - |        - |        - |         - |
```

1.000.000 vector benchmark results (using BenchmarkDotNet):
```
| Method         | Mean       | Error    | StdDev    | Median     | Gen0     | Gen1     | Gen2     | Allocated |
|--------------- |-----------:|---------:|----------:|-----------:|---------:|---------:|---------:|----------:|
| Add            | 1,611.0 us | 32.03 us |  90.87 us | 1,597.0 us | 152.3438 | 152.3438 | 152.3438 | 4000150 B |
| Subtract       | 1,551.3 us | 29.58 us |  36.33 us | 1,553.2 us | 152.3438 | 152.3438 | 152.3438 | 4000150 B |
| Multiply       | 1,584.7 us | 31.33 us |  76.85 us | 1,564.9 us | 152.3438 | 152.3438 | 152.3438 | 4000150 B |
| AddScalar      | 1,546.0 us | 41.17 us | 118.80 us | 1,514.9 us | 152.3438 | 152.3438 | 152.3438 | 4000150 B |
| SubtractScalar | 1,370.7 us | 34.61 us |  94.75 us | 1,335.6 us | 156.2500 | 156.2500 | 156.2500 | 4000154 B |
| MultiplyScalar | 1,313.8 us | 21.72 us |  19.25 us | 1,315.4 us | 152.3438 | 152.3438 | 152.3438 | 4000150 B |
| Sum            |   135.9 us |  1.04 us |   0.92 us |   135.5 us |        - |        - |        - |         - |
```

## 📁 Project Structure


- Mat<T> – Generic matrix structure
- Vec<T> – Generic vector structure
- MatBaseOperations – Core matrix operations (Add, Multiply, Transpose)
- VecBaseOperations – Core vector operations (Dot, Arithmetic)
- Extensions – Modular extension methods with SIMD support

## ⚙️ Requirements

- .NET 7 or later
- C# 11 (INumber<T> support)
- JIT/runtime with SIMD support (System.Numerics.Vector<T>)

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

```
Built for performance. Designed for simplicity. 💡
```
