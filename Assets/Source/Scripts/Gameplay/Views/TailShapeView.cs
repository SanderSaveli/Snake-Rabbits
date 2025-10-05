using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;

namespace SanderSaveli.Snake
{
    [RequireComponent(typeof(SpriteShapeController))]
    public class TailShapeView : MonoBehaviour
    {
        [SerializeField] private SnakeHead _snakeHead;
        [SerializeField] private TailManager _tailManager;
        [Header("Настройки")]
        [Tooltip("Минимальное расстояние между точками сплайна (в мировых единицах).")]
        [SerializeField] private float _minDistance = 0.02f;
        [Tooltip("Если >0 — включено сглаживание (чем больше — тем быстрее подстраивается).")]
        [SerializeField] private float _smoothSpeed = 50f;
        [Tooltip("Если true — будет вызываться BakeCollider() при изменениях (дорого).")]
        [SerializeField] private bool _bakeCollider = false;

        private SpriteShapeController _shapeController;
        private List<SnakeTailView> _tailViews;
        private readonly List<Vector3> _smoothedLocalPositions = new List<Vector3>();
        private int _lastSplineCount = 0;

        private Vector3 _entityLayerOffset = Vector3.zero;

        [Inject]
        public void Construct(GraficConfig graficConfig)
        {
            // Помещаем контроллер на нужный Z-слой (чтобы сплайн был в "правильной" плоскости)
            _entityLayerOffset = new Vector3(0f, 0f, graficConfig.EntityLayer);
        }

        private void Awake()
        {
            _shapeController = GetComponent<SpriteShapeController>();

            // ВАЖНО: контроллер должен иметь локальную шкалу 1,1,1. Если нет — поправь.
            if (transform.localScale != Vector3.one)
                Debug.LogWarning($"{name}: SpriteShapeController GameObject should have localScale = Vector3.one for predictable results.");

            // Установим Z позиции контроллера равной слою сущностей (чтобы в локальных координатах сплайн мог использовать z=0)
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, _entityLayerOffset.z);
        }

        private void Start()
        {
            RebuildTailViewsList();
            // Инициализируем пустые значения
            _smoothedLocalPositions.Clear();
        }

        // Вызываем в LateUpdate чтобы все позиции (tail/head) уже обновились в этом кадре
        private void LateUpdate()
        {
            if (_snakeHead == null || _tailManager == null)
                return;

            if (_tailViews == null)
                RebuildTailViewsList();

            UpdateTailShape();
        }

        private void RebuildTailViewsList()
        {
            _tailViews = new List<SnakeTailView>();
            foreach (SnakeTail tail in _tailManager.TailsParts)
            {
                var view = tail.GetComponent<SnakeTailView>();
                if (view != null) _tailViews.Add(view);
            }
        }

        private void UpdateTailShape()
        {
            // 1) Собираем мировые позиции (голова + отфильтрованный хвост)
            List<Vector3> worldPositions = new List<Vector3>();
            Vector3 headWorld = _snakeHead.CurrentCell.WorldPosition;
            worldPositions.Add(headWorld);

            for (int i = 0; i < _tailViews.Count; i++)
            {
                Vector3 tailWorld = _tailViews[i].transform.position;
                if (Vector3.Distance(worldPositions[worldPositions.Count - 1], tailWorld) > _minDistance)
                    worldPositions.Add(tailWorld);
            }

            // Если нет ни одной хвостовой точки — убедимся, что как минимум голова присутствует
            if (worldPositions.Count == 0)
                worldPositions.Add(headWorld);

            // 2) Конвертируем мировые в локальные координаты контроллера
            List<Vector3> localPositions = new List<Vector3>(worldPositions.Count);
            for (int i = 0; i < worldPositions.Count; i++)
            {
                // Перевод и обнуление z (сплайн в локальной плоскости XY)
                Vector3 local = _shapeController.transform.InverseTransformPoint(worldPositions[i]);
                local.z = 0f;
                localPositions.Add(local);
            }

            // 3) Синхронизируем smoothed список (для сглаживания)
            if (_smoothedLocalPositions.Count != localPositions.Count)
            {
                _smoothedLocalPositions.Clear();
                for (int i = 0; i < localPositions.Count; i++)
                    _smoothedLocalPositions.Add(localPositions[i]);
            }

            // 4) Применяем (опциональное) сглаживание
            if (_smoothSpeed > 0f)
            {
                float t = Mathf.Clamp01(_smoothSpeed * Time.deltaTime);
                for (int i = 0; i < localPositions.Count; i++)
                    _smoothedLocalPositions[i] = Vector3.Lerp(_smoothedLocalPositions[i], localPositions[i], t);
            }
            else
            {
                for (int i = 0; i < localPositions.Count; i++)
                    _smoothedLocalPositions[i] = localPositions[i];
            }

            // 5) Синхронизируем количество точек в сплайне безопасно
            var spline = _shapeController.spline;
            int current = spline.GetPointCount();
            int target = _smoothedLocalPositions.Count;

            // Удалим лишние
            if (current > target)
            {
                for (int i = current - 1; i >= target; i--)
                    spline.RemovePointAt(i);
                current = spline.GetPointCount();
            }

            // Добавим недостающие — добавляем по одному в конец (InsertPointAt в конце безопаснее)
            if (current < target)
            {
                for (int i = current; i < target; i++)
                {
                    // InsertPointAt может бросать исключение если точки очень близко — мы уже отфильтровали это выше
                    spline.InsertPointAt(i, _smoothedLocalPositions[i]);
                }
            }

            // 6) Установим позиции и tangent mode
            bool anyPositionChanged = false;
            for (int i = 0; i < target; i++)
            {
                Vector3 prev = spline.GetPosition(i);
                Vector3 next = _smoothedLocalPositions[i];
                if (prev != next)
                {
                    spline.SetPosition(i, next);
                    anyPositionChanged = true;
                }

                spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            }

            // 7) Обновление коллайдера/месша: делаем только при изменении количества точек или если явно включено
            if (_bakeCollider)
            {
                if (target != _lastSplineCount || anyPositionChanged)
                {
                    _shapeController.BakeCollider();
                }
            }

            _lastSplineCount = target;
        }
    }
}
