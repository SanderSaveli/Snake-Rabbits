using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class FieldConfigGUI : IConfigGUIGroup
    {
        private FieldEntityTool _entityTool;
        private Matrix<FieldEntityTool> _fieldMatrix;

        private static readonly GUILayoutOption CellSize = GUILayout.Width(18);

        private static readonly Dictionary<FieldEntityTool, Color> EntityColors = new()
        {
            { FieldEntityTool.Empty, Color.gray },
            { FieldEntityTool.Rabbit, Color.white },
            { FieldEntityTool.Carrot, Color.yellow },
            { FieldEntityTool.SnakeHead, Color.blue },
            { FieldEntityTool.Obstacle, Color.black }
        };

        private readonly GUIStyle _centeredLabel = new(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleCenter
        };

        public void Draw(LevelConfig levelConfig)
        {
            EditorGUILayout.LabelField("Field Size", _centeredLabel);

            int width = Mathf.Max(1, EditorGUILayout.IntField("Width (x): ", levelConfig.field_width));
            int height = Mathf.Max(1, EditorGUILayout.IntField("Height (y): ", levelConfig.field_height));
            levelConfig.field_width = width;
            levelConfig.field_height = height;

            if (_fieldMatrix == null ||
                _fieldMatrix.Width != width ||
                _fieldMatrix.Height != height)
            {
                _fieldMatrix = GenerateFieldToolMatrix(levelConfig);
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Field", _centeredLabel);

            _entityTool = (FieldEntityTool)EditorGUILayout.EnumPopup("Active Tool", _entityTool);

            DrawField(width, height);

            levelConfig.carrot_positions = GetAllCellsWithType(FieldEntityTool.Carrot);
            levelConfig.obstacle_positions = GetAllCellsWithType(FieldEntityTool.Obstacle);
            levelConfig.rabbit_positions = GetAllCellsWithType(FieldEntityTool.Rabbit);
        }

        public void OpenNewConfig(LevelConfig levelConfig)
        {
            _fieldMatrix = GenerateFieldToolMatrix(levelConfig);
        }

        private void DrawField(int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                GUILayout.BeginHorizontal(GUILayout.Height(15));
                for (int x = 0; x < width; x++)
                {
                    DrawCell(x, y);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void DrawCell(int x, int y)
        {
            FieldEntityTool entity = _fieldMatrix[x, y];
            Color prevColor = GUI.backgroundColor;

            GUI.backgroundColor = EntityColors.TryGetValue(entity, out var color)
                ? color
                : Color.magenta;
            if (GUILayout.Button("", CellSize))
            {
                SetEntityAtCell(x, y);
            }

            GUI.backgroundColor = prevColor;
        }

        private void SetEntityAtCell(int x, int y)
        {
            _fieldMatrix.SetValue(x, y, _entityTool);
        }

        private Matrix<FieldEntityTool> GenerateFieldToolMatrix(LevelConfig config)
        {
            Matrix<FieldEntityTool> matrix = new(config.field_width, config.field_height);

            matrix.SetValue(config.head_position, FieldEntityTool.SnakeHead);

            ApplyEntities(matrix, config.carrot_positions, FieldEntityTool.Carrot);
            ApplyEntities(matrix, config.rabbit_positions, FieldEntityTool.Rabbit);
            ApplyEntities(matrix, config.obstacle_positions, FieldEntityTool.Obstacle);

            return matrix;
        }

        private void ApplyEntities(Matrix<FieldEntityTool> matrix, IEnumerable<Vector2Int> positions, FieldEntityTool type)
        {
            if (positions == null) return;

            foreach (var pos in positions)
            {
                if (matrix.IsInBounds(pos))
                    matrix.SetValue(pos, type);
            }
        }

        private List<Vector2Int> GetAllCellsWithType(FieldEntityTool tool)
        {
            HashSet<Vector2Int> cells = new();
            for (int x = 0; x < _fieldMatrix.Width; x++)
            {
                for (int y = 0; y < _fieldMatrix.Height; y++)
                {
                    if (_fieldMatrix[x, y] == tool)
                        cells.Add(new Vector2Int(x, y));
                }
            }
            return cells.ToList();
        }
    }
}

