using SanderSaveli.Snake;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameField : MonoBehaviour, IGameField
{
    public int FieldWidth => _field.Width;
    public int FieldHeight => _field.Height;
    public Matrix<Cell> Field => _field;
    public Cell this[int x, int y]
    {
        get => _field.GetValue(x, y);
        set => _field.SetValue(x, y, value);
    }

    [SerializeField] private FieldGenerator _fieldGenerator;

    private Matrix<Cell> _field;
    private int _fieldWith;
    private int _fieldHeight;

    [Inject]
    public void Construct(LevelConfig levelConfig)
    {
        _fieldWith = levelConfig.fieldWidth;
        _fieldHeight = levelConfig.fieldHeight;
    }

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

    public bool IsInBounds(int x, int y) =>
        x >= 0 && x< _fieldWith && y >= 0 && y < _fieldHeight;

    public List<Cell> GetFreeCell() =>
        _field.AllValues()
                .Where(c => !c.IsOccupied)
                .ToList();

    public List<Cell> FindAllCellsWithEntity<T>() where T : CellEntity
    {
        List<Cell> result = new List<Cell>();

        List<Cell> allCells = Field.AllValues();

        foreach (var cell in allCells)
        {
            if (cell.IsOccupied)
            {
                if (cell.Entity.GetType() == typeof(T))
                {
                    result.Add(cell);
                }
            }
        }
        return result;
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

    public List<T> FindAllEntity<T>() where T : CellEntity
    {
        List<Cell> cells = FindAllCellsWithEntity<T>();
        List<T> result = new List<T>();
        foreach (var cell in cells)
        {
            result.Add(cell.Entity as T);
        }
        return result;
    }
}
