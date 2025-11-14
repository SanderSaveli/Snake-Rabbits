using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class FieldLevelGUIGroup : IConfigGUIGroup
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
            { FieldEntityTool.Obstacle, Color.black },
            { FieldEntityTool.SnakeTail, Color.cyan }
        };

        private readonly GUIStyle _centeredLabel = new(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleCenter
        };

        public void Draw(LevelConfig levelConfig)
        {
            EditorGUILayout.LabelField("Snake", _centeredLabel);
            levelConfig.start_direction = (Direction)EditorGUILayout.EnumPopup("Start direction: ", levelConfig.start_direction);
            levelConfig.start_segmets = Mathf.Max(0, EditorGUILayout.IntField("Start segments: ", levelConfig.start_segmets));

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
            AddTailSegments(levelConfig);

            DrawField(width, height);
            levelConfig.carrot_positions = GetAllCellsWithType(FieldEntityTool.Carrot);
            levelConfig.obstacle_positions = GetAllCellsWithType(FieldEntityTool.Obstacle);
            levelConfig.rabbit_positions = GetAllCellsWithType(FieldEntityTool.Rabbit);
            levelConfig.head_position = FindHead();
            DrawWarnings(levelConfig);
        }

        public void OpenNewConfig(LevelConfig levelConfig)
        {
            _fieldMatrix = GenerateFieldToolMatrix(levelConfig);
        }

        private void DrawField(int width, int height)
        {
            for (int y = height -1; y >= 0 ; y--)
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
            if (_entityTool == FieldEntityTool.SnakeHead)
            {
                _fieldMatrix.SetValue(FindHead(), FieldEntityTool.Empty);
            }
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

        private void DrawWarnings(LevelConfig levelConfig)
        {
            if (levelConfig.rabbit_positions == null || levelConfig.rabbit_positions.Count == 0)
            {
                EditorGUILayout.HelpBox("There is no one RABBIT on the filed!", MessageType.Warning);
            }
            if (levelConfig.carrot_positions == null || levelConfig.carrot_positions.Count == 0)
            {
                EditorGUILayout.HelpBox("There is no one CARROT on the filed!", MessageType.Warning);
            }
            int headCount = _fieldMatrix.AllValues().FindAll(t => t == FieldEntityTool.SnakeHead).Count;
            if (headCount > 1)
            {
                EditorGUILayout.HelpBox("There is too much head on field", MessageType.Error);
            }
            if (headCount < 1)
            {
                EditorGUILayout.HelpBox("There is no head on field", MessageType.Error);
            }
        }

        private void AddTailSegments(LevelConfig config)
        {
            Direction tailDirection = DirectionTool.GetOpposite(config.start_direction);
            for (int x = 0; x < _fieldMatrix.Width; x++)
            {
                for (int y = 0; y < _fieldMatrix.Width; y++)
                {
                    if (_fieldMatrix[x, y] == FieldEntityTool.SnakeTail)
                    {
                        _fieldMatrix.SetValue(x, y, FieldEntityTool.Empty);
                    }
                }
            }
            for (int i = 1; i <= config.start_segmets; i++)
            {
                try
                {
                    Vector2Int dir = DirectionTool.DirectionToVector2(tailDirection);
                    Vector2Int tailPos = config.head_position + (i * dir);
                    _fieldMatrix.SetValue(tailPos, FieldEntityTool.SnakeTail);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    break;
                }
            }
        }

        private Vector2Int FindHead()
        {
            for (int x = 0; x < _fieldMatrix.Width; x++)
            {
                for (int y = 0; y < _fieldMatrix.Width; y++)
                {
                    if (_fieldMatrix[x, y] == FieldEntityTool.SnakeHead)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            throw new Exception("There is no Head on the field");
        }
    }
}

