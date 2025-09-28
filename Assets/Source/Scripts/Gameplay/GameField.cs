using UnityEngine;

public class GameField : MonoBehaviour
{
    public int FieldWidth => _field.Width;
    public int FieldHeight => _field.Height;
    public Cell this[int x, int y]
    {
        get
        {
            EnsureField();
            return _field.GetValue(x, y);
        }
        set
        {
            EnsureField();
            _field.SetValue(x, y, value);
        }
    }

    [Min(1)]
    [SerializeField] private int _fieldWith;
    [Min(2)]
    [SerializeField] private int _fieldHeight;

    private Matrix<Cell> _field;

    private void Awake()
    {
        EnsureField();
    }

    private void CreateField()
    {
        _field = new Matrix<Cell>(_fieldWith, _fieldHeight);
    }

    private void EnsureField()
    {
        if(_field == null)
        {
            CreateField();
        }
    }
}
