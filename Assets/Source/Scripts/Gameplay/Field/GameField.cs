using SanderSaveli.Snake;
using UnityEngine;

public class GameField : MonoBehaviour
{
    public int FieldWidth => _field.Width;
    public int FieldHeight => _field.Height;
    public Cell this[int x, int y]
    {
        get => _field.GetValue(x, y);
        set => _field.SetValue(x, y, value);
    }

    [Min(1)]
    [SerializeField] private int _fieldWith;
    [Min(2)]
    [SerializeField] private int _fieldHeight;
    [SerializeField] private FieldGenerator _fieldGenerator;

    private Matrix<Cell> _field;

    public void EnsureField()
    {
        if (_field == null)
        {
            CreateField();
        }
    }

    public Vector3 CellToWorld(Vector2Int pos) =>
        CellToWorld(pos.x, pos.y);

    public Vector3 CellToWorld(int x, int y) =>
        _field[x, y].View.transform.position;

    public bool IsInBounds(Vector2Int pos) =>
        IsInBounds(pos.x, pos.y);

    public bool IsInBounds(int x, int y)
    {
        return x > 0 && x< _fieldWith && y > 0 && y < _fieldHeight;
    }

    private void CreateField()
    {
        _field = new Matrix<Cell>(_fieldWith, _fieldHeight);
        Matrix<CellView> cellsView = _fieldGenerator.GenerateField(_fieldWith, _fieldHeight);

        for (int x = 0; x < _fieldWith; x++)
        {
            for (int y = 0; y < _fieldHeight; y++)
            {
                _field[x, y] = new Cell(x, y, cellsView[x, y]);
            }
        }
    }
}
