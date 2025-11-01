using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Matrix<T>
{
    private List<List<T>> _matrix;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public T this[int x, int y]
    {
        get => GetValue(x, y);
        set => SetValue(x, y, value);
    }

    public Matrix(int width, int height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException("Width and Height must be positive integers.");

        Width = width;
        Height = height;

        _matrix = new List<List<T>>(width);
        for (int x = 0; x < width; x++)
        {
            var column = new List<T>(height);
            for (int y = 0; y < height; y++)
            {
                column.Add(default);
            }
            _matrix.Add(column);
        }
    }

    public Matrix(Vector2Int size) : this(size.x, size.y)
    { }

    public T GetValue(int x, int y)
    {
        ValidateIndices(x, y);
        return _matrix[x][y];
    }

    public T GetValue(Vector2Int pos) => GetValue(pos.x, pos.y);

    public void SetValue(int x, int y, T value)
    {
        ValidateIndices(x, y);
        _matrix[x][y] = value;
    }

    public void SetValue(Vector2Int pos, T value) => SetValue(pos.x, pos.y, value);

    public List<T> AllValues() =>
        _matrix.SelectMany(row => row).ToList();

    public bool IsInBounds(Vector2Int vector)
    {
        return IsInBounds(vector.x, vector.y);
    }

    public bool IsInBounds(int x, int y)
    {
        return x>= 0 && y>= 0 && x < Width && y < Height;
    }

    private void ValidateIndices(int x, int y)
    {
        if (x < 0 || x >= Width)
            throw new ArgumentOutOfRangeException(nameof(x), $"X index is out of range. X = {x}, max width = {Width}");
        if (y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y), $"Y index is out of range. Y = {y}, max height = {Height}");
    }
}

