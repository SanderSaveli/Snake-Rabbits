using UnityEngine;

namespace SanderSaveli.Pathfinding
{
    public class PathCellView : CellView
    {
        [SerializeField] private Color _pathColor;
        [SerializeField] private Color _obstacleColor = Color.black;
        [SerializeField] private Transform _rotation;

        public void SelectAsPath(bool isSelect, Transform comeFrom = null)
        {
            if (isSelect)
            {
                _spriteRenderer.color = _pathColor;

                if (comeFrom != null && _rotation != null)
                {
                    _rotation.gameObject.SetActive(true);
                    Vector3 dir3 = comeFrom.position - transform.position;
                    Vector2 dir = new Vector2(dir3.x, dir3.y);

                    // Если длина направления почти нулевая — не меняем поворот
                    if (dir.sqrMagnitude > Mathf.Epsilon)
                    {
                        // atan2 возвращает угол в градусах, где 0° = (1,0) (вправо).
                        // Нам нужно 0° = вверх, поэтому вычитаем 90°.
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                        // Устанавливаем локальный поворот по Z (2D)
                        _rotation.localEulerAngles = new Vector3(0f, 0f, angle);
                    }
                }
            }
            else
            {
                SetColor();
                _rotation.gameObject.SetActive(false);
                if (_rotation != null)
                    _rotation.localRotation = Quaternion.identity;
            }
        }

        public void SelectAsStart()
        {
            _spriteRenderer.color = Color.yellow;
        }

        public void SelectAsObstracle()
        {
            _spriteRenderer.color = _obstacleColor;
        }
    }
}