using SanderSaveli.Snake;
using UnityEngine;
using Zenject;

public class CellView : MonoBehaviour
{
    public Vector2Int Position { get; private set; }
    [SerializeField] private SpriteRenderer _spriteRenderer;

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

    private void SetColor()
    {
        bool isEven = (Position.x + Position.y) % 2 == 0;

        _spriteRenderer.color = isEven ? _graficConfig.CellEvenColor : _graficConfig.CellOddColor;
    }

    private void Update()
    {
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
