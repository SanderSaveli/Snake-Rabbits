using SanderSaveli.Snake;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class CellView : MonoBehaviour
{
    public Vector2Int Position { get; private set; }
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected bool _isDebug;

    private GraficConfig _graficConfig;
    public Cell Cell;

    [Inject]
    public void Construct(GraficConfig graficConfig)
    {
        _graficConfig = graficConfig;
    }

    public void SetPosition(Vector2Int position)
    {
        Position = position;
        SetColor();
    }

    protected void SetColor()
    {
        bool isEven = (Position.x + Position.y) % 2 == 0;

        _spriteRenderer.color = isEven ? _graficConfig.CellEvenColor : _graficConfig.CellOddColor;
    }

    private void Update()
    {
        if (!_isDebug)
        {
            return;
        }

        if (Cell.IsOccupied)
        {
            _spriteRenderer.color = Color.gray;
        }
        else
        {
            SetColor();
        }
    }
}
