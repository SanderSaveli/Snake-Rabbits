using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class LevelConfigWindow : EditorWindow
    {
        private LevelConfig _config;
        public const string LEVEL_PATH = "Assets/Source/Levels/";
        private List<IConfigGUIGroup> _configGUIGroups;
        private Vector2 _scrollPos;
        private void OnEnable()
        {
            _configGUIGroups = new List<IConfigGUIGroup> {
                new LevelNumberGroup(),
                new FieldConfigGUI()
            };
        }

        [MenuItem("Tools/Snake/LevelConfig")]
        public static void CreateWindow()
        {
            GetWindow<LevelConfigWindow>("LevelConfig");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Height(30));

            if (GUILayout.Button(new GUIContent("Save", "Save current conig"), GUILayout.ExpandHeight(true)))
                HandleSave();

            if (GUILayout.Button(new GUIContent("Load", "Load conig"), GUILayout.ExpandHeight(true)))
                HandleLoad();

            if (GUILayout.Button(new GUIContent("New", "Create new config"), GUILayout.ExpandHeight(true)))
                HandleNew();
            GUILayout.EndHorizontal();

            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            if (_config != null)
            {
                foreach (var group in _configGUIGroups)
                {
                    group.Draw(_config);
                }
            }
            GUILayout.EndScrollView();
        }

        private void HandleSave()
        {
            string fileName = $"{_config.level_number}.json";
            string fullPath = Path.Combine(LEVEL_PATH, fileName);
            string json = JsonUtility.ToJson(_config, true);
            File.WriteAllText(fullPath, json);
        }

        private void HandleLoad()
        {
            string path = EditorUtility.OpenFilePanel(
                "Выберите LevelConfig JSON",
                LEVEL_PATH,
                "json"
            );

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!File.Exists(path))
            {
                Debug.LogError($"There is no File to path{path}");
                return;
            }

            LoadConfig(path);
        }

        private void HandleNew()
        {
            _config = new LevelConfig();
            int levelNumber = GetNearestLevel();
            _config.level_number = levelNumber;
            string fileName = $"{levelNumber}.json";
            string fullPath = Path.Combine(LEVEL_PATH, fileName);

            string json = JsonUtility.ToJson(_config, true);
            File.WriteAllText(fullPath, json);

            LoadConfig(fullPath);
        }


        private void LoadConfig(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                _config = JsonUtility.FromJson<LevelConfig>(json);
                foreach (var group in _configGUIGroups)
                {
                    group.OpenNewConfig(_config);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Ошибка при загрузке LevelConfig: " + e.Message);
            }
        }

        private int GetNearestLevel()
        {
            int level = 1;
            while (File.Exists(LEVEL_PATH + level + ".json"))
            {
                level++;
            }
            return level;
        }

    }
}
