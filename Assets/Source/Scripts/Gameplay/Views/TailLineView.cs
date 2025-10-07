using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class TailLineView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private SnakeHead _snakeHead;
        [SerializeField] private TailManager _tailManager;

        private SnakeTail _lastTailView;
        private Vector3 _entityLayer;
        private GameField _gameField;

        [SerializeField] private float _smoothStep = 0.05f;   

        [Inject]
        public void Construct(GraficConfig graficConfig, GameField gameField)
        {
            _entityLayer = new Vector3(0, 0, graficConfig.EntityLayer);
            _gameField = gameField;
        }

        private void Start()
        {
            ResetTileView();
        }

        private void OnEnable()
        {
            _tailManager.OnChangeTailCount += ResetTileView;
        }

        private void OnDisable()
        {
            _tailManager.OnChangeTailCount -= ResetTileView;
        }

        private void ResetTileView()
        {
            int index = _tailManager.TailLength - 1;
            _lastTailView = _tailManager.TailsParts[index];
        }

        private void Update()
        {
            List<Vector3> rawPoints = new List<Vector3>
            {
                _snakeHead.transform.position
            };

            foreach (SnakeTail tail in _tailManager.TailsParts)
            {
                Vector3 pos = _gameField.CellToWorld(tail.CurrentCell.Position);
                pos.z = 0;
                rawPoints.Add(pos);
            }
            Vector3 lastTailPosition = _lastTailView.transform.position;
            lastTailPosition.z = 0;
            rawPoints.Add(lastTailPosition);

            // —глаживаем путь (интерпол€ци€ между клетками)
            List<Vector3> smoothPoints = SmoothPath(rawPoints, _smoothStep);

            // ќбновл€ем LineRenderer
            _lineRenderer.positionCount = smoothPoints.Count;
            for (int i = 0; i < smoothPoints.Count; i++)
                _lineRenderer.SetPosition(i, smoothPoints[i] + _entityLayer);
        }

        private List<Vector3> SmoothPath(List<Vector3> points, float step)
        {
            List<Vector3> result = new List<Vector3>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 a = points[i];
                a.z = 0;
                Vector3 b = points[i + 1];
                b.z = 0;
                float dist = Vector3.Distance(a, b);
                int segments = Mathf.Max(1, Mathf.CeilToInt(dist / step));

                for (int s = 0; s < segments; s++)
                {
                    float t = (float)s / segments;
                    result.Add(Vector3.Lerp(a, b, t));
                }
            }

            result.Add(points[points.Count - 1]);
            return result;
        }
    }
}
