using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class LevelConfigWindow : EditorWindow
    {
        private LevelConfig _config;
        private List<IConfigGUIGroup> _configGUIGroups;
        private Vector2 _scrollPos;
        private void OnEnable()
        {
            _configGUIGroups = new List<IConfigGUIGroup> {
                new NumberLevelGUIGroup(),
                new StarLevelConfigGUIGroup(),
                new FieldLevelGUIGroup()
            };
        }

        [MenuItem("Tools/Snake/LevelConfig")]
        public static void CreateWindow()
        {
            GetWindow<LevelConfigWindow>("LevelConfig");
        }

        public static void OpenWithLevel(string path)
        {
            var window = GetWindow<LevelConfigWindow>("LevelConfig");
            window.LoadDirectly(path);
            window.Show();
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
                    try
                    {
                        GUILayout.Space(10);
                        group.Draw(_config);
                    }catch (Exception  ex)
                    {
                        Debug.Log(ex);
                    }
                }
            }
            GUILayout.EndScrollView();
        }

        private void HandleSave()
        {
            LevelConfigLoader.SaveConfig(_config);
        }

        private void HandleLoad()
        {
            string path = EditorUtility.OpenFilePanel(
                "Выберите LevelConfig JSON",
                Const.LEVEL_FOLDER_PATH,
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

            LoadDirectly(path);
        }

        private void HandleNew()
        {
            _config = new LevelConfig();
            int levelNumber = GetNearestLevel();
            _config.level_number = levelNumber;
            string fileName = $"{levelNumber}.json";
            string fullPath = Path.Combine(Const.LEVEL_FOLDER_PATH, fileName);

            string json = JsonUtility.ToJson(_config, true);
            File.WriteAllText(fullPath, json);

            LoadConfig(levelNumber);
        }


        private void LoadConfig(int levelNumber)
        {
            LevelConfig config = LevelConfigLoader.LoadConfig(levelNumber);
            if(config != null)
            {
                foreach (var group in _configGUIGroups)
                {
                    group.OpenNewConfig(_config);
                }
            }
        }

        private void LoadDirectly(string path)
        {
            _config= LevelConfigLoader.LoadDirectly(path);
            if (_config != null)
            {
                foreach (var group in _configGUIGroups)
                {
                    group.OpenNewConfig(_config);
                }
            }
            else
            {
                Debug.LogError("Error loading conig, config is null");
            }
        }

        private int GetNearestLevel()
        {
            int level = 1;
            while (File.Exists(Const.LEVEL_FOLDER_PATH + level + ".json"))
            {
                level++;
            }
            return level;
        }

    }
}
