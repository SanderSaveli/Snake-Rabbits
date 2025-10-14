using UnityEngine;

namespace SanderSaveli.Pathfinding
{
    public class CellPathSelector : MonoBehaviour
    {
        public PathCellView StartCell;
        public PathCellView HoverCell;

        [Header("Raycast Settings")]
        [SerializeField] private Camera _camera;

        private bool _isSelecting = false;

        private void Awake()
        {
            if (_camera == null)
                _camera = Camera.main;
        }

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouseInput();
#elif UNITY_IOS || UNITY_ANDROID
            HandleTouchInput();
#endif
        }

        // 🖱 Для PC (мышь)
        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCell = GetCellUnderCursor(Input.mousePosition);
                if (StartCell != null)
                {
                    _isSelecting = true;
                }
            }

            if (_isSelecting)
            {
                var currentCell = GetCellUnderCursor(Input.mousePosition);
                if (currentCell != HoverCell)
                {
                    HoverCell = currentCell;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                ClearSelection();
            }
        }

        // 📱 Для мобильных устройств (тач)
        private void HandleTouchInput()
        {
            if (Input.touchCount == 0)
                return;

            Touch touch = Input.GetTouch(0);
            Vector2 pos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartCell = GetCellUnderCursor(pos);
                    if (StartCell != null)
                    {
                        _isSelecting = true;
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (_isSelecting)
                    {
                        var currentCell = GetCellUnderCursor(pos);
                        if (currentCell != HoverCell)
                        {
                            HoverCell = currentCell;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    ClearSelection();
                    break;
            }
        }

        private PathCellView GetCellUnderCursor(Vector2 screenPos)
        {
            Ray ray = _camera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.collider.GetComponent<PathCellView>();
            }

            // Если 2D-игра со SpriteRenderer (без 3D коллайдеров)
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
            if (hit2D.collider != null)
            {
                return hit2D.collider.GetComponent<PathCellView>();
            }

            return null;
        }

        private void ClearSelection()
        {
            _isSelecting = false;

            if (StartCell != null)
            {
                StartCell = null;
            }

            if (HoverCell != null)
            {
                HoverCell = null;
            }
        }
    }
}
