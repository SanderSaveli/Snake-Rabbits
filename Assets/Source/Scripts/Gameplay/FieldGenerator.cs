using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class FieldGenerator : MonoBehaviour
    {
        [SerializeField] private GameField _gameField;
        [SerializeField] private CellView _cellView;
        [SerializeField] private RectTransform _fieldBounds;
        [SerializeField] private Transform _fieldParent;

        [Space]
        [Min(0)][SerializeField] private float _distanceBetweenCells = 1f;

        private float _cellWidth;
        private float _cellHeight;
        private DiContainer _container;


        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }

        private void Start()
        {
            CacheCellSize();
            GenerateField();
        }

        private void CacheCellSize()
        {
            Renderer renderer = _cellView.GetComponent<Renderer>();

            _cellWidth = renderer.bounds.size.x;
            _cellHeight = renderer.bounds.size.y;
        }

        public void GenerateField()
        {
            int width = _gameField.FieldWidth;
            int height = _gameField.FieldHeight;

            Vector2 fieldSize = CalculateFieldSize(width, height);


            if (!GetBoundsScreenOffsetsAndCenter(_fieldBounds, out float leftPx, out float rightPx, out float topPx, out float bottomPx, out Vector3 boundsCenterWorld))
                return;

            Camera cam = Camera.main;
            cam.orthographic = true;

            float previousCamSize = cam.orthographicSize;
            float requiredSize = ComputeRequiredOrthoSize(fieldSize, leftPx, rightPx, topPx, bottomPx, cam);
            cam.orthographicSize = requiredSize;
            float camScaleFactor = requiredSize / previousCamSize;

            boundsCenterWorld.z = _fieldParent.position.z;
            _fieldParent.position = boundsCenterWorld * camScaleFactor;
            _fieldParent.rotation = Quaternion.identity;

            ClearChildren(_fieldParent);
            Vector3 originLocal = CalculateFieldOrigin(fieldSize);
            SpawnCells(width, height, originLocal, _fieldParent);
        }

        private Vector2 CalculateFieldSize(int width, int height)
        {
            float stepX = _cellWidth + _distanceBetweenCells;
            float stepY = _cellHeight + _distanceBetweenCells;
            float totalW = (width - 1) * stepX + _cellWidth;
            float totalH = (height - 1) * stepY + _cellHeight;
            return new Vector2(totalW, totalH);
        }

        private Vector3 CalculateFieldOrigin(Vector2 fieldSize)
        {
            float ox = -(fieldSize.x - _cellWidth) / 2f;
            float oy = -(fieldSize.y - _cellHeight) / 2f;
            return new Vector3(ox, oy, 0f);
        }

        private void SpawnCells(int width, int height, Vector3 originLocal, Transform parent)
        {
            float stepX = _cellWidth + _distanceBetweenCells;
            float stepY = _cellHeight + _distanceBetweenCells;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Vector3 localPos = originLocal + new Vector3(x * stepX, y * stepY, 0f);
                    CellView inst = _container.InstantiatePrefabForComponent<CellView>(_cellView, parent);
                    inst.SetPosition(new Vector2Int(x,y));
                    inst.transform.localPosition = localPos;
                    inst.transform.localRotation = Quaternion.identity;
                }
        }

        private bool GetBoundsScreenOffsetsAndCenter(RectTransform bounds, out float leftPx, out float rightPx, out float topPx, out float bottomPx, out Vector3 centerWorld)
        {
            Canvas canvas = bounds.GetComponentInParent<Canvas>();
            Camera canvasCam = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;

            Vector3[] corners = new Vector3[4];
            bounds.GetWorldCorners(corners);
            Vector2 blScreen = RectTransformUtility.WorldToScreenPoint(canvasCam, corners[0]);
            Vector2 trScreen = RectTransformUtility.WorldToScreenPoint(canvasCam, corners[2]);

            leftPx = blScreen.x;
            bottomPx = blScreen.y;
            rightPx = Screen.width - trScreen.x;
            topPx = Screen.height - trScreen.y;

            centerWorld = (corners[0] + corners[2]) * 0.5f;
            return true;
        }

        private float ComputeRequiredOrthoSize(Vector2 fieldSize, float leftPx, float rightPx, float topPx, float bottomPx, Camera cam)
        {
            float screenH = Screen.height;
            float aspect = cam.aspect;

            float horizPxSum = leftPx + rightPx;
            float vertPxSum = topPx + bottomPx;

            float denomX = aspect - (horizPxSum / screenH);
            float denomY = 1f - (vertPxSum / screenH);
            const float EPS = 1e-5f;
            if (denomX <= EPS) denomX = EPS;
            if (denomY <= EPS) denomY = EPS;

            float reqX = fieldSize.x / (2f * denomX);
            float reqY = fieldSize.y / (2f * denomY);
            return Mathf.Max(Mathf.Max(reqX, reqY), 0.01f);
        }

        private void ClearChildren(Transform t)
        {
            for (int i = t.childCount - 1; i >= 0; i--)
            {
                Destroy(t.GetChild(i).gameObject);
            }
        }
    }
}
