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

<br><br>
```
Built for performance. Designed for simplicity. 💡
```